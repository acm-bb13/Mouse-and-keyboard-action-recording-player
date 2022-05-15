using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace 动作监听播放器
{
    public partial class MainMonitorForm : Form
    {
        

        public MainMonitorForm()
        {
            InitializeComponent();
        }

        

        //DateArr获取;
        private List<CoreData> dateArr = new List<CoreData>();

        //动作信息
        public string formName;
        public string formTime;
        public string formMessage;

        //窗口启动时打开钩子
        private void Form3_Load(object sender, EventArgs e)
        {
            //使窗口放在右上角
            int x = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Size.Width - 
                this.Width-100;
            int y = 100;
            this.SetDesktopLocation(x, y);

            this.TopMost = true;

            timeTickRecord = 0;
            timeTickConst = 1000 / this.timer1.Interval;
            //载入钩子
            Mouse_Load();
            Keyboard_Load();
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        //窗口关闭时关闭钩子
        public MainForm mainForm = null;

        private void Form3_Closed(object sender, FormClosedEventArgs e)
        {
            mh.UnHook();
            k_hook.Stop();
            mainForm.timeTickConst = timeTickConst;
            this.timer1.Enabled = false;
            mainForm.dates = dateArr;
            if (dateArr.Count > 0)
            {
                mainForm.formTime = TimeToStringCH();
            }
            mainForm.Visible = true;
        }

        //计时
        public static Int64 timeTickRecord = 0;
        public Int64 timeTickConst = 1;
        private void timer1_Tick(object sender, EventArgs e)
        {
            timeTickRecord++;
            timeTickConst = 1000 / this.timer1.Interval;
            label1.Text = timeToString();
            if(mh != null)
            {
                var e2 = new MouseEventArgs(MouseButtons.None, 0, p3.X, p3.Y, 0);
                dateArr.Add(CoreData.create(timeTickRecord,
                CoreData.IsMouseOrKeyboard.Mouse,
                CoreData.IsUpOrDown.Move,
                e2));
            }
        }

        //将时间刻转换成字符串
        string timeToString()
        {
            Int64 day, hour, minute, second, millisecond;
            Int64 time = timeTickRecord;
            millisecond = time % timeTickConst;
            time /= timeTickConst;
            second = time % 60;
            time /= 60;
            minute = time % 60;
            time /= 60;
            hour = time % 24;
            time /= 24;
            day = time;
            string str = day + ":" +
                hour + ":" +
                minute + ":" +
                second + ":" +
                millisecond;
            return str;
        }

        string timeToString(Int64 tt)
        {
            Int64 day, hour, minute, second, millisecond;
            Int64 time = tt;
            millisecond = time % timeTickConst;
            time /= timeTickConst;
            second = time % 60;
            time /= 60;
            minute = time % 60;
            time /= 60;
            hour = time % 24;
            time /= 24;
            day = time;
            string str = day + ":" +
                hour + ":" +
                minute + ":" +
                second + ":" +
                millisecond;
            return str;
        }

        string TimeToStringCH()
        {
            string text = "";
            timeTickRecord /= 100;
            if (timeTickRecord == 0)
            {
                return "0秒";
            }
            long miao = timeTickRecord % 60;
            timeTickRecord /= 60;
            text = miao + "秒" + text;
            if (timeTickRecord == 0)
                return text;

            miao = timeTickRecord % 60;
            timeTickRecord /= 60;
            text = miao + "分" + text;
            if (timeTickRecord == 0)
                return text;

            miao = timeTickRecord % 24;
            timeTickRecord /= 24;
            text = miao + "小时" + text;
            if (timeTickRecord == 0)
                return text;

            miao = timeTickRecord;
            text = miao + "天" + text;
            return text;
        }

        #region 鼠标监听模块
        //载入监听事件
        MouseHook mh;
        bool LeftTag = false;
        bool RightTag = false;
        Point p1 = new Point(0, 0);
        Point p2 = new Point(0, 0);
        Point p3 = new Point(0, 0);
        Int64 t1 = 0, t2 = 0;
        
        private void Mouse_Load()
        {
            mh = new MouseHook();
            mh.SetHook();
            mh.MouseDownEvent += mh_MouseDownEvent;
            mh.MouseUpEvent += mh_MouseUpEvent;
            mh.MouseMoveEvent += mh_MouseMoveEvent;
            hook = Hook.GlobalEvents();     //新钩子读取滚轮
            hook.MouseWheelExt += Hook_MouseWheelExt;
        }

        //按下鼠标键触发的事件
        private void mh_MouseDownEvent(object sender, MouseEventArgs e)
        {
            if (!IsStart) return;
            if (e.Button == MouseButtons.Left)
            {
                LeftTag = true;
            }
            if (e.Button == MouseButtons.Right)
            {
                RightTag = true;
            }
            p1 = e.Location;
            t1 = timeTickRecord;
            dateArr.Add(CoreData.create(timeTickRecord,
                CoreData.IsMouseOrKeyboard.Mouse,
                CoreData.IsUpOrDown.Down,
                e));
        }
        //松开鼠标键触发的事件
        private void mh_MouseUpEvent(object sender, MouseEventArgs e)
        {
            if (!IsStart) return;
            t2 = timeTickRecord;
            p2 = e.Location;
            string text = "";
            if(t2-t1 <=20 && p1 == p2)
                text += timeToString() + "\n完成了点击动作";
            else
                if (p1 == p2)
                    text += timeToString() + "\n完成了长按动作";
                else
                    text += timeToString() + "\n完成了拖动动作";

            if (e.Button == MouseButtons.Left)
            {
                text += "(左键)";
            }
            if (e.Button == MouseButtons.Right)
            {
                text += "(右键)";
                //SendKeys.Send("{}");
            }
            if (p1 != p2)
                text += "\n\t" + "拖动鼠标从:(x:" + p1.X + ",y:" + p1.Y + ")\n\t到(x:" + p2.X + ",y:" + p2.Y + ")位置\n";
            else if (t2 - t1 > 20)
                text += "(x:" + p1.X + ",y:" + p1.Y + ")\n\t" + "长按了:" + timeToString(t2 - t1) + "\n";
            else
                text += "(x:" + p1.X + ",y:" + p1.Y + ")\n";
            text += "============\n";
            richTextBox1.Text = text + richTextBox1.Text;
            RightTag = false;
            LeftTag = false;
            p1 = new Point(0, 0);
            p2 = new Point(0, 0);
            dateArr.Add(CoreData.create(timeTickRecord,
                CoreData.IsMouseOrKeyboard.Mouse,
                CoreData.IsUpOrDown.Up,
                e));
        }


        //鼠标移动触发的事件
        private void mh_MouseMoveEvent(object sender, MouseEventArgs e)
        {
            /*dateArr.Add(Date1.create(timeTickRecord,
                Date1.IsMouseOrKeyboard.Mouse,
                Date1.IsUpOrDown.Up,
                e));*/
            p3 = e.Location;
        }

        //鼠标滚轮触发的事件
        private IKeyboardMouseEvents hook;
        private void Hook_MouseWheelExt(object sender, MouseEventExtArgs e)
        {
            if (!IsStart) return;
            string text = "";
            if (e.Delta > 0)        //鼠标滚轮向前
            {
                text += "滚轮向上移动";
            }
            else
            {
                text += "滚轮向下移动";
            }
            text += "\n============\n";
            richTextBox1.Text = text + richTextBox1.Text;

            dateArr.Add(CoreData.create(timeTickRecord,
                CoreData.IsMouseOrKeyboard.Mouse,
                CoreData.IsUpOrDown.Wheel,
                e));
        }

        #endregion

        #region 键盘监听模块

        KeyboardHook k_hook;

        internal List<CoreData> DateArr { get => dateArr; set => dateArr = value; }

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

        private void hook_KeyDown(object sender, KeyEventArgs e)
        {
            string text = "";

            //将当前按键信息转换成可识别的按键信息
            if (!keyNow.Contains( (int)sb(e) ) )
            {
                    keyNow.Add((int)sb(e));
            }

            //判断快捷键是否按下
            if (keyStart.Count > 0 && keyStart.SetEquals(keyNow))
            {
                button7_Click(null, null);
            }
            if (keyDisplay.Count > 0 && keyDisplay.SetEquals(keyNow))
            {
                button1_Click(null, null);
            }
            if (keyEnd.Count > 0 && keyEnd.SetEquals(keyNow))
            {
                //调用结束按钮事件
                button2_Click(null, null);
            }
            

            if (!IsStart) return;
            text += timeToString() + "按下了:" + e.KeyData.ToString() + "键\n";
            text += "============\n";
            richTextBox1.Text = text + richTextBox1.Text;
            dateArr.Add(CoreData.create(timeTickRecord,
                CoreData.IsMouseOrKeyboard.Keyboard,
                CoreData.IsUpOrDown.Down,
                e));
        }


        private void hook_KeyUpEvent(object sender, KeyEventArgs e)
        {
            if (keyNow.Contains((int)sb(e)))
            {
                keyNow.Remove((int)sb(e));
            }
            if (!IsStart) return;
            string text = "";
            text += timeToString() + "松开了:" + e.KeyData.ToString() + "键\n";
            text += "============\n";
            richTextBox1.Text = text + richTextBox1.Text;
            dateArr.Add(CoreData.create(timeTickRecord,
                CoreData.IsMouseOrKeyboard.Keyboard,
                CoreData.IsUpOrDown.Up,
                e));
            
        }
        #endregion

        //监听键盘快捷键
        public SortedSet<int> keyStart = new SortedSet<int>();
        public SortedSet<int> keyDisplay = new SortedSet<int>();
        public SortedSet<int> keyEnd = new SortedSet<int>();

        //当前已按下的按键
        public SortedSet<int> keyNow = new SortedSet<int>();

        //是否开始状态
        public bool IsStart = false;

        private void button1_Click(object sender, EventArgs e)
        {
            if(keyDisplay.Count == 0)
            {
                MessageBox.Show("警告:\n未设置\"显示监控窗口\"快捷键，无法隐藏窗口", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if(this.Visible == true)
            {
                this.Visible = false;
            }
            else
            {
                this.Visible = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //实现暂停继续监听功能
        private void button7_Click(object sender, EventArgs e)
        {
            if (!IsStart)
            {
                button7.Text = "暂停监听";
                timer1.Enabled = true;
                IsStart = true;
            }
            else
            {
                timer1.Enabled = false;
                button7.Text = "开始监听";
                IsStart = false;
            }
        }
    }
}
