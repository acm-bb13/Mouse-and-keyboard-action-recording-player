using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 动作监听播放器
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }

        //mouse_event函数，位于user32.dll这个库文件
        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern int mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);


        /// <summary>
        /// 导入模拟键盘的方法
        /// </summary>
        /// <param name="bVk" >按键的虚拟键值</param>
        /// <param name= "bScan" >扫描码，一般不用设置，用0代替就行</param>
        /// <param name= "dwFlags" >选项标志：0：表示按下，2：表示松开</param>
        /// <param name= "dwExtraInfo">一般设置为0</param>
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        public List<Date1> dateArr = new List<Date1>();
        int p = 0;


        //窗口关闭时关闭钩子
        public MainForm mainForm = null;

        //计时
        public Int64 timeTickConst = 1;


        bool isPlaying = false;
        bool isLoop = false;
        int playCount = 1;
        private void button1_Click(object sender, EventArgs e)
        {
            if (!isPlaying)
            {
                //判断次数播放还是循环播放
                if (radioButton1.Checked)
                {
                    isLoop = false;
                    playCount = int.Parse(textBox1.Text);
                    start();
                }
                if (radioButton2.Checked)
                {
                    isLoop = true;
                    start();
                }
            }
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            //使窗口放在右上角
            int x = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Width -
                this.Width - 100;
            int y = 100;
            this.SetDesktopLocation(x, y);

            //使应用始终显示在顶层
            this.TopMost = true;

            showKeyText();

            //载入键盘钩子，监听快捷方式
            Keyboard_Load();
        }

        private void Form6_Closed(object sender, FormClosedEventArgs e)
        {
            mainForm.keyEnd = keyEnd;
            mainForm.Visible = true;
            k_hook.Stop();
        }

        public long timeTickRecord = 0;

        //执行开始播放功能
        void start()
        {
            //this.Visible = false;
            isPlaying = true;
            this.timer1.Enabled = true;
            timeTickConst = 1000 / timer1.Interval;
            p = 0;
            timeTickRecord = 0;
        }

        void stop()
        {
            //this.Visible = true;
            isPlaying = false;
            this.timer1.Enabled = false;
            timeTickConst = 1000 / timer1.Interval;
            p = 0;
            timeTickRecord = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(dateArr.Count>0 && p < dateArr.Count)
            {
                while(p < dateArr.Count && dateArr.ElementAt(p).timeTickRecord <= timeTickRecord)
                {
                    //保存当前动作
                    Date1 d = dateArr.ElementAt(p);

                    if (d.isMouseOrKeyboard
                        == Date1.IsMouseOrKeyboard.Mouse)
                    {
                        //0x8000采用绝对坐标
                        //0x0001移动鼠标
                        int dwFlags = 0x8000 | 0x0001 | 0x0800;
                        int x = 0;
                        int y = 0;
                        //判断按键代码块
                        {
                            if (d.mouseEventArgs.Button == MouseButtons.Left)
                            {
                                if (d.isUpOrDown == Date1.IsUpOrDown.Down)
                                {
                                    dwFlags |= 0x0002;
                                }
                                if (d.isUpOrDown == Date1.IsUpOrDown.Up)
                                {
                                    dwFlags |= 0x0004;
                                }
                            }
                            if (d.mouseEventArgs.Button == MouseButtons.Right)
                            {
                                if (d.isUpOrDown == Date1.IsUpOrDown.Down)
                                {
                                    dwFlags |= 0x0008;
                                }
                                if (d.isUpOrDown == Date1.IsUpOrDown.Up)
                                {
                                    dwFlags |= 0x0010;
                                }
                            }
                        }
                        x = d.mouseEventArgs.X;
                        x = x * 65536 / Screen.PrimaryScreen.Bounds.Width;
                        y = d.mouseEventArgs.Y;
                        y = y * 65536 / Screen.PrimaryScreen.Bounds.Height;
                        mouse_event(dwFlags,
                            x,
                            y,
                            d.mouseEventArgs.Delta,0
                           );
                    }

                    if (d.isMouseOrKeyboard
                        == Date1.IsMouseOrKeyboard.Keyboard)
                    {

                        int dwFlags = 0;
                        if (d.isUpOrDown == Date1.IsUpOrDown.Up)
                            dwFlags = 2;
                        keybd_event((byte)d.keyEventArgs.KeyValue
                            ,0,dwFlags,0);
                    }

                        //keybd_event
                    p++;
                }
            }
            timeTickRecord++;
            if(p >= dateArr.Count)
            {
                if (!isLoop)
                {
                    playCount--;
                    if (playCount > 0)
                    {
                        start();
                    }
                    else
                    {
                        stop();
                    }
                }
                else
                {
                    start();
                }
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        //当键盘输入框时
        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != '\b' && !Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

        }


        //当输入框变化时
        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            if(textBox1.Text == "" || textBox1.Text == "0")
            {
                textBox1.Text = "1";
            }
        }

        //更新显示快捷方式
        void showKeyText()
        {
            SortedSet<int> key = keyEnd;
            string text = "终止播放快捷方式\n(";
            bool p = false;
            foreach (int i in key)
            {
                if (p)
                    text += "+";
                else
                    p = true;
                Keys keys = (Keys)i;
                if (keys == Keys.ControlKey)
                    text += "Ctrl";
                else if (keys == Keys.ShiftKey)
                    text += "Shift";
                else if (keys == Keys.Menu)
                    text += "Alt";
                else
                    text += keys.ToString();
            }
            text += ")";
            if (key.Count() == 0)
            {
                text = "未设置快捷键";
            }
            if (key == keyEnd)
                this.button2.Text = text;
        }

        #region 键盘监听模块

        KeyboardHook k_hook;

        internal List<Date1> DateArr { get => dateArr; set => dateArr = value; }

        //载入监听模块
        private void Keyboard_Load()
        {
            k_hook = new KeyboardHook();
            k_hook.KeyDownEvent += new KeyEventHandler(hook_KeyDown);//钩住键按下
            k_hook.KeyUpEvent += new KeyEventHandler(hook_KeyUpEvent); ;
            k_hook.Start();//安装键盘钩子
        }


        //恶心的bug
        Keys sb(KeyEventArgs e)
        {
            Keys k = e.KeyCode;
            if (e.KeyCode == Keys.Control ||
                    e.KeyCode == Keys.LControlKey ||
                    e.KeyCode == Keys.RControlKey
                    )
                k = Keys.ControlKey;
            else if (e.KeyCode == Keys.Alt ||
                e.KeyCode == Keys.LMenu ||
                e.KeyCode == Keys.RMenu
                )
                k = Keys.Menu;
            else if (e.KeyCode == Keys.Shift ||
                e.KeyCode == Keys.LShiftKey ||
                e.KeyCode == Keys.RShiftKey
                )
                k = Keys.ShiftKey;
            return k;
        }

        //保存终止快捷键
        public SortedSet<int> keyEnd = new SortedSet<int>();

        //当前已按下的按键
        public SortedSet<int> keyNow = new SortedSet<int>();

        //当键盘按下时
        private void hook_KeyDown(object sender, KeyEventArgs e)
        {
            //将当前按键信息转换成可识别的按键信息
            if (!keyNow.Contains((int)sb(e)))
            {
                keyNow.Add((int)sb(e));
            }

            if (keyEnd.Count > 0 && keyEnd.SetEquals(keyNow))
            {
                //调用结束按钮事件
                stop();
            }
        }

        //按键松开时
        private void hook_KeyUpEvent(object sender, KeyEventArgs e)
        {
            //按键松开，移出列表
            if (keyNow.Contains((int)sb(e)))
            {
                keyNow.Remove((int)sb(e));
            }
        }
        #endregion

        //更改快捷方式
        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
            if (form2.ppp == 0)
                keyEnd = form2.keyArr;
            form2.Dispose();
            showKeyText();
        }
    }
}
