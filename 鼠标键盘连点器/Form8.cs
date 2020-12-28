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
    public partial class Form8 : Form
    {
        public Form8()
        {
            InitializeComponent();
        }

        //保存信息
        public string formName;
        public string formTime;
        public string formMessage;

        public Int64 timeTickRecord;

        public bool ans = false;

        //按下保存
        private void button1_Click(object sender, EventArgs e)
        {
            ans = true;
            formName = textBox1.Text;
            formMessage = richTextBox1.Text;
            this.Close();
        }

        //按下取消保存
        private void button2_Click(object sender, EventArgs e)
        {
            ans = false;
            this.Close();
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            formTime = TimeToString();
            label4.Text = formTime;
            formName = textBox1.Text;
            formMessage = richTextBox1.Text;
        }

        string TimeToString()
        {
            string text = "";
            timeTickRecord /= 100;
            if (timeTickRecord == 0)
            {
                return "0秒";
            }
            long miao = timeTickRecord % 60;
            timeTickRecord /= 60;
            text = miao+"秒"+text;
            if(timeTickRecord == 0)
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
    }
}
