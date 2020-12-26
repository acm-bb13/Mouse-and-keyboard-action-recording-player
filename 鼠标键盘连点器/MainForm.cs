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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        //默认按键
        public SortedSet<int> keyStart = new SortedSet<int>() {
            (int)Keys.ControlKey,
            (int)Keys.K
            };
        public SortedSet<int> keyDisplay = new SortedSet<int>(){
            (int)Keys.ControlKey,
            (int)Keys.G
            };
        public SortedSet<int> keyEnd = new SortedSet<int>(){
            (int)Keys.ControlKey,
            (int)Keys.L
            };

        //获取监听数据
        public List<Date1> dates = new List<Date1>();
        public Int64 timeTickConst = 1;

        private void MainForm_Load(object sender, EventArgs e)
        {
            MouseMove += Form_MouseMove;
            MouseDown += Form_MouseDown;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            MouseTest mouseTest = new MouseTest();
            mouseTest.ShowDialog();
            mouseTest.Dispose();
            this.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            KeyboardTest mouseTest = new KeyboardTest();
            mouseTest.ShowDialog();
            mouseTest.Dispose();
            this.Visible = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region 窗口随鼠标拖动
        //窗口随鼠标拖动
        //代码来源
        //https://www.cnblogs.com/gc2013/p/4000647.html
        private Point _mousePoint;
        private void Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Top = MousePosition.Y - _mousePoint.Y;
                Left = MousePosition.X - _mousePoint.X;
            }
        }
        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _mousePoint.X = e.X;
                _mousePoint.Y = e.Y;
            }
        }
        #endregion

        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form3 form3 = null;
            this.Visible = false;
            Form1 form1 = new Form1();
            //读取按键
            form1.keyStart = keyStart;
            form1.keyDisplay = keyDisplay;
            form1.keyEnd = keyEnd;
            form1.ShowDialog();
            //写入按键
            keyStart = form1.keyStart;
            keyDisplay = form1.keyDisplay;
            keyEnd = form1.keyEnd;
            bool play = form1.play;
            form1.Dispose();
            if (play)
            {
                form3 = new Form3();
                //读取按键
                form3.keyStart = keyStart;
                form3.keyDisplay = keyDisplay;
                form3.keyEnd = keyEnd;
                form3.mainForm = this;
                form3.Show();
            }
            else
            {
                this.Visible = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string ss = this.richTextBox1.Text;
            this.richTextBox1.Text = "";
            string []ssArr = ss.Split(new char[1] { '\n' }, 2);
            string str = ssArr[0];
            string str2 = ssArr[1];
            this.richTextBox1.Text = str2+"\n" + str;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            Form4 form4 = new Form4();
            form4.dates = dates;
            form4.timeTickConst = timeTickConst;
            form4.ShowDialog();

            form4.Dispose();
            this.Visible = true;
        }
    }
}
