﻿using System;
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
        public Int64 timeTickConst = 10;

        public List<CoreData> dates = new List<CoreData>();
        public string formName = "未命名";
        public string formTime = "0秒";
        public string formMessage = "无描述";

        private void MainForm_Load(object sender, EventArgs e)
        {
            MouseMove += Form_MouseMove;
            MouseDown += Form_MouseDown;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DeclareForm form13 = new DeclareForm();
            form13.ShowDialog();
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


        //打开监听窗口
        private void button7_Click(object sender, EventArgs e)
        {
            MainMonitorForm form3 = null;
            this.Visible = false;
            MainKeySettingForm form1 = new MainKeySettingForm();
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
                form3 = new MainMonitorForm();
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

        int r = 0;
        int g = 250;
        int b = 250;
        int cooo = 10;
        private void timer1_Tick(object sender, EventArgs e)
        {
            string ss = this.richTextBox1.Text;
            this.richTextBox1.Text = "";
            string []ssArr = ss.Split(new char[1] { '\n' }, 2);
            string str = ssArr[0];
            string str2 = ssArr[1];
            this.richTextBox1.Text = str2+"\n" + str;

            if(g >= 235 && b < 235)
            {
                b += cooo;
                r -= cooo;
            }else
            if (b >= 235 && r < 235)
            {
                r += cooo;
                g -= cooo;
            }
            else
            if (r >= 235 && g < 235)
            {
                g += cooo;
                b -= cooo;
            }
            this.button5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(r)))), ((int)(((byte)(g)))), ((int)(((byte)(b)))));
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }


        


        //打开播放窗口
        private void button9_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            DisplayForm form6 = new DisplayForm();
            form6.dateArr = dates;
            form6.keyEnd = keyEnd;
            form6.mainForm = this;
            form6.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //this.Visible = false;
            ServerManagementForm form7 = new ServerManagementForm();
            form7.dates = dates;
            form7.formName = formName;
            form7.formTime = formTime;
            form7.formMessage = formMessage;
            form7.mainForm = this;
            form7.ShowDialog();

            form7.Dispose();
            //this.Visible = true;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //this.Visible = false;
            FileMainForm form4 = new FileMainForm();
            form4.dates = dates;
            form4.formName = formName;
            form4.formTime = formTime;
            form4.formMessage = formMessage;
            form4.mainForm = this;
            form4.ShowDialog();
            dates = form4.dates;
            formName = form4.formName;
            formTime = form4.formTime;
            formMessage = form4.formMessage;
            //this.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SurprisedForm form12 = new SurprisedForm();
            form12.ShowDialog();
        }
    }
}
