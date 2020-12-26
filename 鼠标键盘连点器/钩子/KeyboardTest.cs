using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices; //调用WINDOWS API函数时要用到
using Microsoft.Win32; //写入注册表时要用到

namespace 鼠标键盘连点器
{
    public partial class KeyboardTest : Form
    {
        public KeyboardTest()
        {
            InitializeComponent();
        }

        private void hook_KeyDown(object sender, KeyEventArgs e)
        {
            richTextBox1.Text = e.KeyData.ToString()+"\n"+
                richTextBox1.Text;
            //判断按下的键（Alt + A）
            /*if (e.KeyValue == (int)Keys.X && (int)Control.ModifierKeys == (int)Keys.Alt && (int)Control.ModifierKeys == (int)Keys.Control)
            {
                System.Windows.Forms.MessageBox.Show("按下了指定快捷键组合");
            }*/
        }

        private void KeyboardTest_Load(object sender, EventArgs e)
        {
            KeyboardHook k_hook = new KeyboardHook();
            k_hook.KeyDownEvent += new KeyEventHandler(hook_KeyDown);//钩住键按下
            //k_hook.KeyUpEvent;
            k_hook.Start();//安装键盘钩子
        }
    }
}
