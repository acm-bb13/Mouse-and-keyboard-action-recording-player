using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace 动作监听播放器
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }


        public MainForm mainForm;

        //监听数据
        public List<Date1> dates = new List<Date1>();
        public string formName;
        public string formTime;
        public string formMessage;


        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            duqu();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string s1 = "编辑动作信息";
            string s2 = formName;
            string s3 = formTime;
            string s4 = formMessage;
            string s5 = "保存";
            Form11 form11 = new Form11(s1,s2,s3,s4,s5);
            form11.ShowDialog();
            if (form11.isAns)
            {
                baocun();
            }
        }

        void baocun()
        {

            SaveFileDialog SaveAddr = new SaveFileDialog();
            SaveAddr.Filter = "动作信息文件(*.dodata)|*.dodata";
            SaveAddr.DefaultExt = "*.dodata";//默认文件扩展名
            if (SaveAddr.ShowDialog() == DialogResult.OK)
            {
                string result = "《未加密动作信息》"; //输入文本

                StreamWriter sw = new StreamWriter(SaveAddr.FileName, false);

                sw.WriteLine(result);

                sw.WriteLine(formName);
                sw.WriteLine(formTime);
                sw.WriteLine(formMessage);
                result = "<!===================================!>";
                sw.WriteLine(result);
                foreach(Date1 d in dates)
                {
                    string str = "";
                    str += d.timeTickRecord + ",";
                    str += (int)d.isMouseOrKeyboard + ",";
                    str += (int)d.isUpOrDown + ",";

                    if(d.isMouseOrKeyboard == Date1.IsMouseOrKeyboard.Keyboard)
                    {
                        str += (int)d.keyEventArgs.KeyData ;
                    }

                    if (d.isMouseOrKeyboard == Date1.IsMouseOrKeyboard.Mouse)
                    {
                        str += (int)d.mouseEventArgs.Button + ",";
                        str += d.mouseEventArgs.X + ",";
                        str += d.mouseEventArgs.Y + ",";
                        str += d.mouseEventArgs.Delta;
                    }
                    sw.WriteLine(str);
                }
                sw.Flush();
                sw.Close();
            }

           
        }

        void duqu()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "动作信息文件(*.dodata)|*.dodata|所有文件|*.*";
            ofd.ValidateNames = true;
            ofd.CheckPathExists = true;
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string strFileName = ofd.FileName;
                //其他代码
                string[] lines = System.IO.File.ReadAllLines(strFileName);
                int mod = 1;
                
                //MessageBox.Show("读取成功");
                foreach (string line in lines)
                {
                    if (mod == 1 &&!"《未加密动作信息》".Equals(line))
                    {
                        MessageBox.Show("警告!该文件非动作信息!");
                        return;
                    }
                    else if (mod == 1)
                    {
                        mod = 2;
                        dates = new List<Date1>();
                        continue;
                    }
                    if (mod == 2)
                    {
                        formName = line;
                        mod++;
                        continue;
                    }
                    if (mod == 3)
                    {
                        formTime = line;
                        formMessage = "";
                        mod++;
                        continue;
                    }
                    string temp = "<!===================================!>";
                    
                    if (mod == 4 && !temp.Equals(line))
                    {
                        formMessage += line;
                        continue;
                    }else if(mod == 4)
                    {
                        mod = 5;
                        continue;
                    }
                    if(mod == 5)
                    {
                        string[] str = line.Split(',');
                        long x1 = long.Parse(str[0]);
                        Date1.IsMouseOrKeyboard x2 = (Date1.IsMouseOrKeyboard)int.Parse(str[1]);
                        Date1.IsUpOrDown x3 = (Date1.IsUpOrDown)int.Parse(str[2]);
                        if (x2 == Date1.IsMouseOrKeyboard.Keyboard)
                        {
                            Keys x4 = (Keys)int.Parse(str[3]);
                            dates.Add(Date1.create(
                                x1,
                            x2,
                            x3,
                            new KeyEventArgs(x4)
                            ));
                        }
                        if (x2 == Date1.IsMouseOrKeyboard.Mouse)
                        {
                            MouseButtons x5 = (MouseButtons)int.Parse(str[3]);
                            int x6 = int.Parse(str[4]);
                            int x7 = int.Parse(str[5]);
                            int x8 = int.Parse(str[6]);
                            dates.Add(Date1.create(
                                x1,
                            x2,
                            x3,
                            new MouseEventArgs(x5, 0, x6, x7, x8)
                            ));
                        }
                    }
                }
            }
        }
    }
}
