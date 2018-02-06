using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Mine
{
    /// <summary>
    /// 表示一局游戏
    /// </summary>
    public sealed class Game : IDisposable
    {
        #region 只读字段

        //游戏数据
        private readonly Square[,] _gameData;
        //游戏等级
        private readonly GameLevel _level;
        //雷区数
        private readonly int _minesCount;
        //游戏区与窗口边界的偏移量
        private readonly Size _gameFieldOffset;
        //窗口图面
        private readonly Graphics _wndGraphics;
        //缓冲区
        private readonly Bitmap _buffer;
        //缓冲区图面
        private readonly Graphics _bufferGraphics;

        //随机生成器
        private static readonly Random Rnd = new Random();

        #endregion 只读字段

        #region 可写字段

        //游戏是否开始
        private bool _gameStarted;
        //游戏是否结束
        private bool _gameStoped;

        #endregion 可写字段

        #region 常量

        //边界常量
        public const int
            MaxWidth = 24,//最大宽度
            MaxHeight = 30,//最大高度
            MinWidth = 9,//最小宽度
            MinHeight = 9,//最小高度
            MinMinesCount = 10;//最小雷数

        #endregion 常量

        #region 构造器

        /// <summary>
        /// 从预设等级初始化游戏
        /// </summary>
        /// <param name="level">预设等级</param>
        /// <param name="g">图面</param>
        /// <param name="gameFieldOffset">游戏区偏移</param>
        public Game(GameLevel level, Graphics g, Size gameFieldOffset)
        {
            _level = level;
            _gameFieldOffset = gameFieldOffset;
            _wndGraphics = g;

            //初始化游戏数据
            switch (level)
            {
                case GameLevel.Beginner:
                    _gameData = new Square[9, 9];
                    _minesCount = 10;
                    break;
                case GameLevel.Intermediate:
                    _gameData = new Square[16, 16];
                    _minesCount = 40;
                    break;
                case GameLevel.Expert:
                    _gameData = new Square[30, 16];
                    _minesCount = 99;
                    break;
                case GameLevel.Customize:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("level", level, null);
            }

            //初始化缓冲区
            _buffer = new Bitmap(_gameData.GetLength(0) * Square.SquareSize, _gameData.GetLength(1) * Square.SquareSize);
            //创建缓冲区图面
            _bufferGraphics = Graphics.FromImage(_buffer);
        }

        /// <summary>
        /// 创建自定义游戏实例
        /// </summary>
        /// <param name="gameSize">游戏宽高</param>
        /// <param name="minesCount">雷数</param>
        /// <param name="g">图面</param>
        /// <param name="gameFieldOffset">游戏区偏移</param>
        /// <exception cref="ArgumentOutOfRangeException">游戏尺寸或雷数非法</exception>
        public Game(Size gameSize, int minesCount, Graphics g, Size gameFieldOffset)
        {
            _level = GameLevel.Customize;
            _gameFieldOffset = gameFieldOffset;
            _wndGraphics = g;

            //边界值验证
            if (gameSize.Width > MaxWidth)
                gameSize.Width = MaxWidth;
            else if (gameSize.Width < MinWidth)
                gameSize.Width = MinWidth;
            if (gameSize.Height > MaxHeight)
                gameSize.Height = MaxHeight;
            else if (gameSize.Height < MinHeight)
                gameSize.Height = MinHeight;

            if (minesCount < MinMinesCount)
                minesCount = MinMinesCount;
            else if (minesCount > (gameSize.Width - 1) * (gameSize.Height - 1))
                minesCount = (gameSize.Width - 1) * (gameSize.Height - 1);

            //初始化游戏数据
            _gameData = new Square[gameSize.Width, gameSize.Height];
            _minesCount = minesCount;

            //初始化缓冲区
            _buffer = new Bitmap(_gameData.GetLength(0) * Square.SquareSize, _gameData.GetLength(1) * Square.SquareSize);
            //创建缓冲区图面
            _bufferGraphics = Graphics.FromImage(_buffer);
        }

        #endregion 构造器

        #region 调试相关

#if DEBUG

        /// <summary>
        /// 直接胜利
        /// </summary>
        public void Win()
        {
            if (!_gameStarted)
            {
                Start();
                Mine(new Point(0, 0));
            }

            for (int i = 0; i < _gameData.GetLength(0); i++)
            {
                for (int j = 0; j < _gameData.GetLength(1); j++)
                {
                    Square s = _gameData[i, j];
                    if (s.Mined) s.RightClick();
                    else s.LeftClick();
                }
            }
            Draw();
        }

#endif

        #endregion 调试相关

        #region 公共实例方法

        /// <summary>
        /// 开始游戏
        /// </summary>
        public void Start()
        {
            //假设所有方块区均非雷区
            for (int i = 0; i < _gameData.GetLength(0); i++)
                for (int j = 0; j < _gameData.GetLength(1); j++)
                    _gameData[i, j] = new Square(new Point(i, j), false, 0);
        }

        /// <summary>
        /// 获取游戏区像素尺寸
        /// </summary>
        /// <returns></returns>
        public Size GetGamePixelSize()
        {
            return new Size(_gameData.GetLength(0) * Square.SquareSize, _gameData.GetLength(1) * Square.SquareSize);
        }

        /// <summary>
        /// 左键单击
        /// </summary>
        /// <param name="pixelPt">鼠标单击像素点</param>
        /// <returns>
        /// 0:空白处
        /// 1:获胜
        /// -1:游戏结束
        /// </returns>
        public int LeftClick(Point pixelPt)
        {
            if (_gameStoped) return 0;

            //相对于游戏区的像素点
            Point gameFieldpt = new Point(pixelPt.X - _gameFieldOffset.Width, pixelPt.Y - _gameFieldOffset.Height);
            //游戏逻辑点(坐标)
            Point logicalPt = new Point(gameFieldpt.X / Square.SquareSize, gameFieldpt.Y / Square.SquareSize);

            //首次单击时布雷
            if (!_gameStarted)
                Mine(logicalPt);

            //没有踩中雷区
            if (!_gameData[logicalPt.X, logicalPt.Y].LeftClick())
            {
                //如果是空白区，则递归相邻的所有空白区
                if (_gameData[logicalPt.X, logicalPt.Y].MinesAround == 0)
                    AutoOpenAround(logicalPt);

                //获胜判定：所有空白处均已打开
                for (int i = 0; i < _gameData.GetLength(0); i++)
                {
                    for (int j = 0; j < _gameData.GetLength(1); j++)
                    {
                        if (_gameData[i, j].Mined)
                            continue;
                        if (_gameData[i, j].Status == Square.SquareStatus.Opened)
                            continue;

                        return 0;
                    }
                }

                //获胜
                return 1;
            }

            //游戏结束
            for (int i = 0; i < _gameData.GetLength(0); i++)
                for (int j = 0; j < _gameData.GetLength(1); j++)
                    if (_gameData[i, j].Mined)
                        _gameData[i, j].GameOver(logicalPt);
                    else
                        if (_gameData[i, j].Status == Square.SquareStatus.Marked)
                            _gameData[i, j].IndicateMarkMissed();

            return -1;
        }

        /// <summary>
        /// 右键单击
        /// </summary>
        /// <param name="pixelPt">鼠标单击像素点</param>
        public void RightClick(Point pixelPt)
        {
            if (_gameStoped) return;
            Point pt = new Point(pixelPt.X - _gameFieldOffset.Width, pixelPt.Y - _gameFieldOffset.Height);
            _gameData[pt.X / Square.SquareSize, pt.Y / Square.SquareSize].RightClick();
        }

        /// <summary>
        /// 结束游戏
        /// </summary>
        public void Stop()
        {
            _gameStoped = true;
        }

        /// <summary>
        /// 绘制一帧
        /// </summary>
        public void Draw()
        {
            for (int i = 0; i < _gameData.GetLength(0); i++)
                for (int j = 0; j < _gameData.GetLength(1); j++)
                    _gameData[i, j].Draw(_bufferGraphics);

            _wndGraphics.DrawImage(_buffer, new Point(_gameFieldOffset.Width, _gameFieldOffset.Height));
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_buffer != null)
                _buffer.Dispose();
        }

        #endregion 公共实例方法

        #region 私有实例方法

        /// <summary>
        /// 布雷
        /// </summary>
        /// <param name="startPt">首次单击点</param>
        private void Mine(Point startPt)
        {
            Size area = new Size(_gameData.GetLength(0), _gameData.GetLength(1));
            List<Point> excluded = new List<Point> { startPt };

            //随机创建雷区
            for (int i = 0; i < _minesCount; i++)
            {
                Point pt = GetRandomPoint(area, excluded);
                _gameData[pt.X, pt.Y] = new Square(pt, true, 0);
                excluded.Add(pt);
            }

            //创建非雷区
            for (int i = 0; i < _gameData.GetLength(0); i++)
                for (int j = 0; j < _gameData.GetLength(1); j++)
                    if (!_gameData[i, j].Mined)//非雷区
                    {
                        int minesAround = EnumSquaresAround(new Point(i, j)).Cast<Square>().Count(square => square.Mined);//周围雷数

                        _gameData[i, j] = new Square(new Point(i, j), false, minesAround);
                    }

            _gameStarted = true;
        }

        /// <summary>
        /// 自动打开周围非雷区方块(递归)
        /// </summary>
        /// <param name="squarePt">原方块逻辑坐标</param>
        private void AutoOpenAround(Point squarePt)
        {
            //遍历周围方块
            foreach (Square square in EnumSquaresAround(squarePt))
            {
                if (square.Mined || square.Status == Square.SquareStatus.Marked || square.Status == Square.SquareStatus.Opened)
                    continue;

                square.LeftClick();//打开
                //周围无雷区
                if (square.MinesAround == 0)
                    AutoOpenAround(square.Location);//递归打开
            }
        }

        /// <summary>
        /// 枚举周围所有方块区
        /// </summary>
        /// <param name="squarePt">原方块区</param>
        /// <returns>枚举数</returns>
        private IEnumerable EnumSquaresAround(Point squarePt)
        {
            int i = squarePt.X, j = squarePt.Y;

            //周围所有方块区
            for (int x = i - 1; x <= i + 1; ++x)//横向
            {
                if (x < 0 || x >= _gameData.GetLength(0))//越界
                    continue;

                for (int y = j - 1; y <= j + 1; ++y)//纵向
                {
                    if (y < 0 || y >= _gameData.GetLength(1))//越界
                        continue;

                    if (x == squarePt.X && y == squarePt.Y)//排除自身
                        continue;

                    yield return _gameData[x, y];
                }
            }
        }

        #endregion 私有实例方法

        #region 私有静态方法

        /// <summary>
        /// 获取指定区域内的一个不在排除项列表中的随机点
        /// </summary>
        /// <param name="areaSize">区域大小，指定了随机点坐标的范围</param>
        /// <param name="excluded">排除项</param>
        /// <returns>随机点</returns>
        /// <exception cref="ArgumentNullException">排除项为空</exception>
        private static Point GetRandomPoint(Size areaSize, List<Point> excluded)
        {
            if (excluded == null)
                throw new ArgumentNullException("excluded");

            int x, y;

            do
            {
                x = Rnd.Next(0, areaSize.Width);
                y = Rnd.Next(0, areaSize.Height);
            } while (excluded.Any(pt => pt.X == x && pt.Y == y));

            return new Point(x, y);
        }

        #endregion 私有静态方法
    }
}
