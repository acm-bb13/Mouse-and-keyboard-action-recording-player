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
    public partial class FileSaveForm : Form
    {
        public bool isAns = false;
        public FileSaveForm(string s1, string s2, string s3 , string s4 , string s5)
        {
            InitializeComponent();
            this.Text = s1;
            textBox1.Text = s2;
            label4.Text = s3;
            richTextBox1.Text = s4;
            button1.Text = s5;
        }

        private void Form11_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            isAns = true;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
