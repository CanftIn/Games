using System;
using System.Drawing;

namespace Mine
{
    /// <summary>
    /// 表示游戏中一个方块区
    /// </summary>
    public sealed class Square
    {
        /// <summary>
        /// 方块区状态
        /// </summary>
        public enum SquareStatus
        {
            /// <summary>
            /// 闲置
            /// </summary>
            Idle,
            /// <summary>
            /// 已打开
            /// </summary>
            Opened,
            /// <summary>
            /// 已标记
            /// </summary>
            Marked,
            /// <summary>
            /// 已质疑
            /// </summary>
            Queried,
            /// <summary>
            /// 游戏结束
            /// </summary>
            GameOver,
            /// <summary>
            /// 标记失误(仅在游戏结束时用于绘制)
            /// </summary>
            MarkMissed
        }

        #region 静态字段

        //绘图字体
        private static readonly Font Font = new Font(new FontFamily("Arial"), SquareSize / 1.5f, FontStyle.Bold);

        #endregion 静态字段

        #region 常量

        public const int SquareSize = 16;

        #endregion 常量

        #region 只读字段

        //逻辑坐标(对应二维数组索引)
        private readonly Point _location;
        //是否为雷区
        private readonly bool _mined;
        //周围雷数
        private readonly int _minesAround;

        #endregion 只读字段

        #region 可写字段

        //状态
        private SquareStatus _status;
        //是否为爆炸雷区(用于游戏结束时绘制)
        private bool _isBombPoint;

        #endregion

        #region 调试相关

        //仅用于调试时在对象查看器中显示对象摘要
#if DEBUG
        public override string ToString()
        {
            return string.Format("[{0},{1}]是否雷区：{2}，周围雷数：{3}，状态：{4}", _location.X, _location.Y, _mined, _minesAround, _status);
        }
#endif

        #endregion 调试相关

        #region 属性

        /// <summary>
        /// 是否为雷区
        /// </summary>
        public bool Mined
        {
            get { return _mined; }
        }

        /// <summary>
        /// 状态
        /// </summary>
        public SquareStatus Status
        {
            get { return _status; }
        }

        /// <summary>
        /// 周围雷数
        /// </summary>
        public int MinesAround
        {
            get { return _minesAround; }
        }

        /// <summary>
        /// 逻辑坐标(对应二维数组索引)
        /// </summary>
        public Point Location
        {
            get { return _location; }
        }

        #endregion 属性

        #region 构造器

        /// <summary>
        /// 初始化一个方块区
        /// </summary>
        /// <param name="loc">位置</param>
        /// <param name="mined">是否为雷区</param>
        /// <param name="minesAround">周围雷区数</param>
        public Square(Point loc, bool mined, int minesAround)
        {
            _location = loc;
            _mined = mined;
            _minesAround = minesAround;
            _status = SquareStatus.Idle;
            _isBombPoint = false;
        }

        #endregion 构造器

        #region 公共实例方法

        /// <summary>
        /// 左键单击
        /// </summary>
        /// <returns>是否踩到雷</returns>
        public bool LeftClick()
        {
            switch (_status)
            {
                case SquareStatus.Idle:
                case SquareStatus.Queried:
                    _status = SquareStatus.Opened;
                    return _mined;
                case SquareStatus.Opened:
                case SquareStatus.Marked:
                case SquareStatus.GameOver:
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// 右键单击
        /// </summary>
        public void RightClick()
        {
            switch (_status)
            {
                case SquareStatus.Idle:
                    _status = SquareStatus.Marked;
                    break;
                case SquareStatus.Opened:
                case SquareStatus.GameOver:
                    break;
                case SquareStatus.Marked:
                    _status = SquareStatus.Queried;
                    break;
                case SquareStatus.Queried:
                    _status = SquareStatus.Idle;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// 绘制
        /// </summary>
        /// <param name="g">图面</param>
        public void Draw(Graphics g)
        {
            //绘制区矩形
            Rectangle rect = new Rectangle(_location.X * SquareSize, _location.Y * SquareSize, SquareSize, SquareSize);

            switch (_status)
            {
                case SquareStatus.Idle:
                    DrawIdleBackground(g, rect);//仅绘制闲置时背景
                    break;
                case SquareStatus.Marked:
                    DrawIdleBackground(g, rect);
                    //红旗三角三点坐标
                    Point[] flagTriangle =
                    {
                        new Point(rect.X + SquareSize / 2, rect.Y + 1), 
                        new Point(rect.X + SquareSize / 2, rect.Y + SquareSize / 2 + 1), 
                        new Point(rect.X + SquareSize / 4 - 1, rect.Y + SquareSize / 4)
                    };
                    //红旗底座三角三点坐标
                    Point[] flagBaseTriangle =
                    {
                        new Point(rect.X + SquareSize / 2, rect.Y +  SquareSize / 2),
                        new Point(rect.X + SquareSize / 4 - 3, rect.Y +  SquareSize - 3), 
                        new Point(rect.X + SquareSize / 4 * 3,  rect.Y + SquareSize - 3)
                    };
                    //填充红旗三角
                    g.FillPolygon(Brushes.Red, flagTriangle);
                    //填充底座三角
                    g.FillPolygon(Brushes.Black, flagBaseTriangle);
                    break;
                case SquareStatus.Queried:
                    DrawIdleBackground(g, rect);
                    //绘制问号
                    SizeF querySize = g.MeasureString("?", Font);
                    g.DrawString("?", Font, Brushes.Black, rect.X + SquareSize / 2 - querySize.Width / 2 - 1, rect.Y + SquareSize / 2 - querySize.Height / 2);
                    break;
                case SquareStatus.Opened:
                    //绘制已打开时背景图
                    g.FillRectangle(Brushes.Silver, rect);
                    g.DrawRectangle(new Pen(Color.Gray, 2f), rect);
                    //周围雷数大于0
                    if (_minesAround > 0)
                    {
                        //绘制周围雷数数字
                        string numStr = _minesAround.ToString();
                        SizeF numSize = g.MeasureString(numStr, Font);
                        g.DrawString(numStr, Font, GetNumberBrush(), rect.X + SquareSize / 2 - numSize.Width / 2 - 1, rect.Y + SquareSize / 2 - numSize.Height / 2);
                    }
                    break;
                case SquareStatus.GameOver://游戏结束时绘制雷区
                    if (!_mined) break;
                    //绘制背景图,爆炸点背景为红色,否则银色
                    g.FillRectangle(_isBombPoint ? Brushes.Red : Brushes.Silver, rect);
                    g.DrawRectangle(new Pen(Color.Gray, 2f), rect);
                    //绘制地雷
                    g.FillEllipse(Brushes.Black, new Rectangle(rect.X + 2, rect.Y + 2, rect.Width - 2 * 2, rect.Height - 2 * 2));
                    break;
                case SquareStatus.MarkMissed://游戏结束时绘制标记未命中的方块区
                    g.FillRectangle(Brushes.Silver, rect);
                    g.DrawRectangle(new Pen(Color.Gray, 2f), rect);
                    //绘制地雷
                    g.FillEllipse(Brushes.Black, new Rectangle(rect.X + 2, rect.Y + 2, rect.Width - 2 * 2, rect.Height - 2 * 2));
                    //绘制叉号
                    g.DrawLine(Pens.Red, rect.X, rect.Y, rect.Right, rect.Bottom);//左上到右下
                    g.DrawLine(Pens.Red, rect.Right, rect.Y, rect.X, rect.Bottom);//右上到左下
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// 游戏结束
        /// </summary>
        /// <param name="bombPt">爆炸点</param>
        public void GameOver(Point bombPt)
        {
            _status = SquareStatus.GameOver;
            _isBombPoint = bombPt == _location;
        }

        /// <summary>
        /// 指定标记失误
        /// </summary>
        public void IndicateMarkMissed()
        {
            _status = SquareStatus.MarkMissed;
        }

        #endregion 公共实例方法

        #region 私有实例方法

        /// <summary>
        /// 获取附近雷数数字颜色画刷
        /// </summary>
        /// <returns>画刷</returns>
        private Brush GetNumberBrush()
        {
            switch (_minesAround)
            {
                case 1:
                    return Brushes.Blue;
                case 2:
                    return Brushes.Green;
                case 3:
                    return Brushes.Red;
                case 4:
                    return Brushes.Navy;
                case 5:
                    return Brushes.Maroon;
                case 6:
                    return Brushes.Teal;
                case 7:
                    return Brushes.Black;
                case 8:
                    return Brushes.Gray;
            }
            return null;
        }

        #endregion 私有实例方法

        #region 私有静态方法

        /// <summary>
        /// 绘制闲置时的背景图
        /// </summary>
        /// <param name="g">图面</param>
        /// <param name="rect">矩形</param>
        private static void DrawIdleBackground(Graphics g, Rectangle rect)
        {
            //银色矩形填充
            g.FillRectangle(Brushes.Silver, new Rectangle(rect.X + 1, rect.Y + 1, rect.Width - 4, rect.Height - 4));

            //绘制明暗变化以突显3D凸起效果
            //左上到右上白线
            g.DrawLine(new Pen(Color.White, 2f), rect.X, rect.Y, rect.Right - 2, rect.Y);
            //左上到左上白线
            g.DrawLine(new Pen(Color.White, 2f), rect.X, rect.Y, rect.X, rect.Bottom - 2);
            //右上到右下灰线
            g.DrawLine(new Pen(Color.Gray, 2f), rect.Right - 2, rect.Y, rect.Right - 2, rect.Bottom - 1);
            //左下到右下灰线
            g.DrawLine(new Pen(Color.Gray, 2f), rect.X, rect.Bottom - 2, rect.Right - 1, rect.Bottom - 2);
        }

        #endregion 私有静态方法
    }
}
