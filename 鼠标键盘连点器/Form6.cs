using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 鼠标键盘连点器
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

        //DateArr为当前要播放的动作
        public List<Date1> dateArr = new List<Date1>();
        int p = 0;

        //窗口关闭时关闭钩子
        public MainForm mainForm = null;

        //计时
        public static Int64 timeTickRecord = 0;
        public Int64 timeTickConst = 1;

        private void button1_Click(object sender, EventArgs e)
        {
            start();
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


        }

        private void Form6_Closed(object sender, FormClosedEventArgs e)
        {
            mainForm.Visible = true;
        }

        //执行开始播放功能
        void start()
        {
            this.timer1.Enabled = true;
            timeTickConst = 1000 / timer1.Interval;
            p = 0;
            timeTickRecord = 0;
        }

        void stop()
        {
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
                        int dwFlags = 0x8000 | 0x0001;
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
                            0,0
                           );
                        richTextBox1.Text = d.mouseEventArgs.X+","+ d.mouseEventArgs.Y +
                            "\n"+richTextBox1.Text;
                    }

                    if (d.isMouseOrKeyboard
                        == Date1.IsMouseOrKeyboard.Keyboard)
                    {

                        int dwFlags = 0;
                        if (d.isUpOrDown == Date1.IsUpOrDown.Up)
                            dwFlags = 2;
                        keybd_event((byte)d.keyEventArgs.KeyValue
                            ,0,dwFlags,0);
                        richTextBox1.Text = d.keyEventArgs.KeyCode + "," + dwFlags +
                            "\n" + richTextBox1.Text;
                    }

                        //keybd_event
                    p++;
                }
            }
            timeTickRecord++;
            if(p >= dateArr.Count)
            {
                stop();
            }

        }
    }
}
