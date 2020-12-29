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
    public partial class MouseTest : Form
    {
        MouseHook mh;
        bool LeftTag = false;
        bool RightTag = false;
        Point p1 = new Point(0, 0);
        Point p2 = new Point(0, 0);

        public MouseTest()
        {
            InitializeComponent();
        }

        private void MouseTest_Load(object sender, EventArgs e)
        {
            mh = new MouseHook();
            mh.SetHook();
            mh.MouseDownEvent += mh_MouseDownEvent;
            mh.MouseUpEvent += mh_MouseUpEvent;
            mh.MouseMoveEvent += mh_MouseMoveEvent;
        }

        private void mh_MouseMoveEvent(object sender, MouseEventArgs e)
        {

        }

        //按下鼠标键触发的事件
        private void mh_MouseDownEvent(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                LeftTag = true;
                richTextBox1.Text = "按下了左键\n"+richTextBox1.Text;
            }
            if (e.Button == MouseButtons.Right)
            {
                RightTag = true;
                richTextBox1.Text = "按下了右键\n" + richTextBox1.Text;
            }
            p1 = e.Location;

        }
        //松开鼠标键触发的事件
        private void mh_MouseUpEvent(object sender, MouseEventArgs e)
        {
            p2 = e.Location;
            richTextBox1.Text = "拖动鼠标从:(x:" + p1.X + ",y:" + p1.Y + ")\n移动到了(x:" + p2.X + ",y:" + p2.Y + ")\n" + richTextBox1.Text;
            double value = Math.Sqrt(Math.Abs(p1.X - p2.X) * Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y) * Math.Abs(p1.Y - p2.Y));
            //if (LeftTag && RightTag && value > 100)
            //{
            //    MessageBox.Show("ok");
            //}
            if (e.Button == MouseButtons.Left)
            {
                richTextBox1.Text = "松开了左键\n" + richTextBox1.Text;
            }
            if (e.Button == MouseButtons.Right)
            {
                richTextBox1.Text = "松开了右键\n" + richTextBox1.Text;
            }
            //richTextBox1.AppendText("移动了" + value + "距离\n");
            RightTag = false;
            LeftTag = false;
            p1 = new Point(0, 0);
            p2 = new Point(0, 0);
        }
        private void Form1_FormClosed_1(object sender, FormClosedEventArgs e)
        {
            mh.UnHook();
        }
    }
}
