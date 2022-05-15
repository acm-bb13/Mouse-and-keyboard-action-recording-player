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
    public partial class SurprisedForm : Form
    {
        public SurprisedForm()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"https://github.com/acm-bb13/Mouse-and-keyboard-action-recording-player");
        }

        private void Form12_Load(object sender, EventArgs e)
        {
            
        }
    }
}
