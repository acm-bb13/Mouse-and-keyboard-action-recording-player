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
    public partial class Form1 : Form
    {

        public bool play = false;

        public SortedSet<int> keyStart = new SortedSet<int>();
        public SortedSet<int> keyDisplay = new SortedSet<int>();
        public SortedSet<int> keyEnd = new SortedSet<int>();


        public Form1()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //最小化窗口
            this.WindowState = FormWindowState.Minimized;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (keyDisplay.Count > 0 ||
                MessageBox.Show("“隐藏/显示监控窗口”快捷键未设置，可能会导致后台隐藏功能无法使用，是否继续？",
                "！！！！！！！警告！！！！！！！",
               MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                play = true;
                this.Close();
            }
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

        private void Form1_Load(object sender, EventArgs e)
        {
            MouseMove += Form_MouseMove;
            MouseDown += Form_MouseDown;
            showKeyText();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
            keyStart = form2.keyArr;
            form2.Dispose();
            //查重
            if (keyStart.SetEquals(keyDisplay))
            {
                keyDisplay = new SortedSet<int>();
            }
            if (keyStart.SetEquals(keyEnd))
            {
                keyEnd = new SortedSet<int>();
            }
            showKeyText();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
            if (form2.ppp == 0)
                keyDisplay = form2.keyArr;
            form2.Dispose();
            //查重
            if (keyDisplay.SetEquals(keyStart))
            {
                keyStart = new SortedSet<int>();
            }
            if (keyDisplay.SetEquals(keyEnd))
            {
                keyEnd = new SortedSet<int>();
            }
            showKeyText();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
            if (form2.ppp == 0)
                keyEnd = form2.keyArr;
            form2.Dispose();
            //查重
            if (keyEnd.SetEquals(keyStart))
            {
                keyStart = new SortedSet<int>();
            }
            if (keyEnd.SetEquals(keyDisplay))
            {
                keyDisplay = new SortedSet<int>();
            }
            showKeyText();
        }

        void showKeyText(SortedSet<int> key)
        {
            string text = "(";
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
            if (key == keyStart)
                this.button1.Text = text;
            if (key == keyDisplay)
                this.button2.Text = text;
            if (key == keyEnd)
                this.button4.Text = text;
        }
        void showKeyText()
        {
            showKeyText(keyStart);
            showKeyText(keyDisplay);
            showKeyText(keyEnd);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            keyStart = new SortedSet<int>();
            showKeyText();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            keyDisplay = new SortedSet<int>();
            showKeyText();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            keyEnd = new SortedSet<int>();
            showKeyText();
        }
    }
}
