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
    public partial class ServerDownloadForm : Form
    {
        public bool isAns = false;
        public ServerDownloadForm(string s1,string s2,string s3)
        {
            InitializeComponent();
            label5.Text = s1;
            label4.Text = s2;
            richTextBox1.Text = s3;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            isAns = true;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            isAns = false;
            this.Close();
        }

        private void Form10_Load(object sender, EventArgs e)
        {

        }
    }
}
