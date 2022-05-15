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
    public partial class GetKeyTempForm : Form
    {
        public GetKeyTempForm()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            showKeyText();
        }

        //记录按下的组合键
        public SortedSet<int> keyArr = new SortedSet<int>();
        public int ppp = 0;

        void showKeyText()
        {
            //MessageBox.Show("test");
            string text = "(";
            bool p = false;
            foreach(int i in keyArr)
            {
                if (p)
                    text += "+";
                else
                    p = true;
                Keys keys = (Keys)i;
                //text += keys == Keys.ControlKey;
                if (keys == Keys.ControlKey)
                {
                    text += "Ctrl";
                }
                else if (keys == Keys.ShiftKey)
                    text += "Shift";
                else if (keys == Keys.Menu)
                    text += "Alt";
                else
                    text += keys.ToString();
            }
            text += ")";
            label2.Text = text;
        }

        private void Key_Down(object sender, KeyEventArgs e)
        {
            label1.Text = "松开按键完成录制";

            //查重
            if (!keyArr.Contains((int)e.KeyCode))
            {
                ppp++;
                keyArr.Add((int)e.KeyCode);
            }
            showKeyText();
        }

        private void Key_Up(object sender, KeyEventArgs e)
        {
            ppp--;
            if(ppp == 0)
            {
                label1.Text = "已录制";
                this.Close();
            }
        }

    }
}
