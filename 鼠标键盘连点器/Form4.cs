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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }


        //监听数据
        public List<Date1> dates = new List<Date1>();

        //单位毫秒
        public Int64 timeTickConst = 1;

        private void Form4_Load(object sender, EventArgs e)
        {
            /*
             * DataGridView 禁止列排序
             * https://www.cnblogs.com/henyihanwobushi/archive/2013/05/15/3079051.html
             */
            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                //禁止列排序SortMode = DataGridViewColumnSortMode.NotSortable
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                //使列长度根据数据的长度来显示
                this.dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            }

            //将监听到的数据显示出来
            if (dates.Count>0)
            foreach(Date1 d in dates)
            {
                int index = dataGridView1.Rows.Add();
                int p = 0;
                dataGridView1.Rows[index].Cells[p++].Value = timeToString(d.timeTickRecord);
                dataGridView1.Rows[index].Cells[p++].Value = (d.isMouseOrKeyboard == Date1.IsMouseOrKeyboard.Keyboard?"键盘":"鼠标");
                dataGridView1.Rows[index].Cells[p++].Value = (d.isUpOrDown == Date1.IsUpOrDown.Up? "松开" : "按下");
                if(d.isMouseOrKeyboard == Date1.IsMouseOrKeyboard.Keyboard)
                {
                    dataGridView1.Rows[index].Cells[p++].Value = d.keyEventArgs.KeyData.ToString();
                }
                if (d.isMouseOrKeyboard == Date1.IsMouseOrKeyboard.Mouse)
                {
                    string text = "";
                    if (d.mouseEventArgs.Button == MouseButtons.Left)
                        text += "左键";
                    if (d.mouseEventArgs.Button == MouseButtons.Right)
                        text += "右键";
                    if (d.mouseEventArgs.Button == MouseButtons.Middle)
                        text += "中键";
                    text += "(" + d.mouseEventArgs.X + "," + d.mouseEventArgs.Y + ")";
                    dataGridView1.Rows[index].Cells[p++].Value = text;
                }
            }
        }

        string timeToString(Int64 tt)
        {
            Int64 day, hour, minute, second, millisecond;
            Int64 time = tt;
            millisecond = time % timeTickConst;
            time /= timeTickConst;
            second = time % 60;
            time /= 60;
            minute = time % 60;
            time /= 60;
            hour = time % 24;
            time /= 24;
            day = time;
            int p = 1;
            while (Math.Pow(10, p - 1) != timeTickConst) 
                p++;
            string str = string.Format("{0:D2}", day) + ":" +
                string.Format("{0:D2}", hour) + ":" +
                string.Format("{0:D2}", minute) + ":" +
                string.Format("{0:D2}", second) + ":" +
                string.Format("{0:D"+p+"}", millisecond);
            return str;
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            if(dates.Count>0)
            label1.Text = "("+ dataGridView1.CurrentRow.Index+"," + dataGridView1.CurrentCell.ColumnIndex+ ")";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //dataGridView1.AllowSorting = false;
        }
    }
}
