using System;
using System.Drawing;
using System.Windows.Forms;

namespace Mine
{
    public partial class FrmMain : Form
    {
        //游戏等级
        private GameLevel _gameLevel = GameLevel.Beginner;
        //游戏实例
        private Game _game;
        //绘图图面
        private Graphics _graphics;

        public FrmMain()
        {
            InitializeComponent();
        }

        #region 私有实例方法

        /// <summary>
        /// 创建新游戏
        /// </summary>
        /// <param name="level">游戏等级</param>
        private void NewGame(GameLevel level)
        {
            if (level == GameLevel.Customize)
            {
                Size logicalSize = new Size(Properties.Settings.Default.Width, Properties.Settings.Default.Height);
                int minesCount = Properties.Settings.Default.MinesCount;
                _gameLevel = GameLevel.Customize;

                NewGame(logicalSize, minesCount);
                return;
            }

            //游戏区偏移
            Size gameFieldOffset = new Size(0, ClientRectangle.Y + menuStrip1.Height);

            if (_game != null) _game.Dispose();

            //创建游戏实例
            _game = new Game(level, _graphics, gameFieldOffset);
            //开始游戏以创建所有方块区
            _game.Start();
            //获取游戏区像素大小
            Size gamePixcelSize = _game.GetGamePixelSize();
            //窗口边框宽度
            int borderWidth = (Width - ClientSize.Width) / 2;
            //标题栏高度
            int titleBarHeight = (Height - ClientSize.Height) - borderWidth * 2;
            //固定尺寸
            Size = new Size(gamePixcelSize.Width + borderWidth * 2,
                gamePixcelSize.Height + menuStrip1.Height + titleBarHeight + borderWidth * 2);
            //重绘
            _game.Draw();
        }

        /// <summary>
        /// 创建自定义新游戏
        /// </summary>
        /// <param name="logicalSize">游戏逻辑大小(二维数组宽高)</param>
        /// <param name="minesCount">雷区数</param>
        private void NewGame(Size logicalSize, int minesCount)
        {
            //游戏区偏移
            Size gameFieldOffset = new Size(0, ClientRectangle.Y + menuStrip1.Height);

            if (_game != null) _game.Dispose();

            //创建游戏实例
            _game = new Game(logicalSize, minesCount, _graphics, gameFieldOffset);
            //开始游戏以创建所有方块区
            _game.Start();
            //获取游戏区像素大小
            Size gamePixcelSize = _game.GetGamePixelSize();
            //窗口边框宽度
            int borderWidth = (Width - ClientSize.Width) / 2;
            //标题栏高度
            int titleBarHeight = (Height - ClientSize.Height) - borderWidth * 2;
            //固定尺寸
            Size = new Size(gamePixcelSize.Width + borderWidth * 2,
                gamePixcelSize.Height + menuStrip1.Height + titleBarHeight + borderWidth * 2);
            //重绘
            _game.Draw();
        }

        #endregion 私有实例方法

        #region 事件处理

        #region 窗体

        //窗体:加载
        private void FrmMain_Load(object sender, EventArgs e)
        {
            Size = Screen.PrimaryScreen.Bounds.Size;
            _graphics = CreateGraphics();
            NewGame(_gameLevel);
        }

        //窗体:重绘
        private void FrmMain_Paint(object sender, PaintEventArgs e)
        {
            _game.Draw();
        }

        #endregion 窗体

        #region 鼠标

        //鼠标:单击
        private void FrmMain_MouseClick(object sender, MouseEventArgs e)
        {
            int result = 0;
            if (e.Button == MouseButtons.Right)
                _game.RightClick(e.Location);
            else if (e.Button == MouseButtons.Left)
                result = _game.LeftClick(e.Location);

            _game.Draw();

            switch (result)
            {
                case -1:
                    if (MessageBox.Show("游戏结束！\r\n是否开始新的游戏？", @"扫雷", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        == DialogResult.Yes)
                        NewGame(_gameLevel);
                    else
                        _game.Stop();
                    break;
                case 1:
                    if (MessageBox.Show("游戏胜利！\r\n是否开始新的游戏？", @"扫雷", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        == DialogResult.Yes)
                        NewGame(_gameLevel);
                    else
                        _game.Stop();
                    break;
            }
        }

        #endregion 鼠标

        #region 菜单项

        //菜单:游戏-开局
        private void tsmiNewGame_Click(object sender, EventArgs e)
        {
            NewGame(_gameLevel);
        }

        //菜单:游戏-初级
        private void tsmiBeginner_Click(object sender, EventArgs e)
        {
            NewGame(_gameLevel = GameLevel.Beginner);
        }

        //菜单:游戏-中级
        private void tsmiIntermediate_Click(object sender, EventArgs e)
        {
            NewGame(_gameLevel = GameLevel.Intermediate);
        }

        //菜单:游戏-高级
        private void tsmiExpert_Click(object sender, EventArgs e)
        {
            NewGame(_gameLevel = GameLevel.Expert);
        }

        //菜单:游戏-自定义
        private void tsmiCustomize_Click(object sender, EventArgs e)
        {
            FrmCustomize f = new FrmCustomize();
            if (f.ShowDialog() != DialogResult.OK)
                return;

            Size logicalSize = new Size(Properties.Settings.Default.Width, Properties.Settings.Default.Height);
            int minesCount = Properties.Settings.Default.MinesCount;
            _gameLevel = GameLevel.Customize;

            NewGame(logicalSize, minesCount);
        }

        //菜单:游戏-退出
        private void tsmiExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        //菜单:帮助
        private void tsmiHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("不会玩扫雷和咸鱼有什么分别？！");
        }

        #endregion 菜单项

        #region 按键

        //按键
        private void FrmMain_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //F2开局
            if (e.KeyCode == Keys.F2)
                NewGame(_gameLevel);
#if DEBUG
            if (e.KeyCode == Keys.F3)
            {
                _game.Win();
                if (MessageBox.Show("游戏胜利！\r\n是否开始新的游戏？", @"扫雷", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                         == DialogResult.Yes)
                    NewGame(_gameLevel);
                else
                    _game.Stop();
            }
#endif
        }

        #endregion 按键

        #endregion 事件处理
    }
}
