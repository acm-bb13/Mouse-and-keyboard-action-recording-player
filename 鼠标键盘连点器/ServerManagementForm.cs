using MySql.Data.MySqlClient;
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
    public partial class ServerManagementForm : Form
    {
        public ServerManagementForm()
        {
            InitializeComponent();
        }

        public List<CoreData> dates = new List<CoreData>();
        public string formName;
        public string formTime;
        public string formMessage;

        public MainForm mainForm;


        private void Form7_Load(object sender, EventArgs e)
        {
            //string sql = "select TABLE_NAME from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA='Mouse-and-keyboard-action-recording-player' and TABLE_NAME='date1'";
            flush();

        }


        //刷新列表
        void flush()
        {
            if (dataGridView1.Rows.Count > 0) { dataGridView1.Rows.Clear(); }
            string sql = "select * from amainform ORDER BY mainKey DESC";
            MySqlDataReader mySql = SQLManage.GetReader(sql);
            int record = 0;
            while (mySql.Read())
            {
                record++;
                int index = dataGridView1.Rows.Add();
                int p = 0;
                dataGridView1.Rows[index].Cells[p].Value = mySql[p++].ToString();
                dataGridView1.Rows[index].Cells[p].Value = mySql[p++];
                dataGridView1.Rows[index].Cells[p].Value = mySql[p++];
                dataGridView1.Rows[index].Cells[p].Value = mySql[p++];
            }
            if (record == 0)
            {
                int index = dataGridView1.Rows.Add();
                int p = 0;
                dataGridView1.Rows[index].Cells[p++].Value = "暂无数据";
                dataGridView1.Rows[index].Cells[p++].Value = "暂无数据";
                dataGridView1.Rows[index].Cells[p++].Value = "暂无数据";
            }
            SQLManage.closeConn();
        }


        //上传本地动作
        void UpData()
        {
            ProgressBarForm form9 = new ProgressBarForm("正在上传......");
            form9.progressBar1.Maximum = dates.Count;
            form9.progressBar1.Step = 1;
            form9.Show();
            string sql;
            //sql = "CREATE TABLE `data"+1
            //    +"` (\n  `mainKey` int NOT NULL AUTO_INCREMENT,\n  `timeTickRecord` bigint NOT NULL,\n  `IsMouseOrKeyboard` enum('Mouse','Keyboard') DEFAULT NULL,\n  `IsUpOrDown` enum('Up','Down','Move','Wheel') DEFAULT NULL,\n  `KeyEventArgs` int DEFAULT NULL,\n  `MouseButtons` enum('None','Left','Right','Middle','XButton1','XButton2') DEFAULT 'None',\n  `MouseX` int DEFAULT NULL,\n  `MouseY` int DEFAULT NULL,\n  `MouseDelta` int DEFAULT NULL,\n  PRIMARY KEY (`mainKey`)\n) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;";
            //SQLManage.commands(1);

            //生成一个主表
            sql = "INSERT INTO amainform SET formName = '" + formName
                + "' , formTime = '" + formTime
                + "' , formMessage = '" + formMessage + "'; ";
            SQLManage.command(sql);

            //获取刚刚生成的键值
            sql = "SELECT mainKey FROM amainform ORDER BY mainKey DESC LIMIT 1;";
            MySqlDataReader mySql = SQLManage.GetReader(sql);
            mySql.Read();
            int mainKey = (int)mySql[0];
            SQLManage.closeConn();

            //根据获取的键值生成新表
            sql = "CREATE TABLE `data" + mainKey
                + "` (  `mainKey` int NOT NULL AUTO_INCREMENT,  `timeTickRecord` bigint NOT NULL,  `IsMouseOrKeyboard` int DEFAULT NULL,  `IsUpOrDown` int DEFAULT NULL,  `KeyEventArgs` int DEFAULT NULL,  `MouseButtons` int DEFAULT NULL,  `MouseX` int DEFAULT NULL,  `MouseY` int DEFAULT NULL,  `MouseDelta` int DEFAULT NULL, PRIMARY KEY (`mainKey`) ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;";
            SQLManage.command(sql);

            int p = 0;
            sql = "";
            foreach (CoreData d in dates)
            {
                //将每个动作保存至数据库中
                sql += "INSERT INTO data" + mainKey
                    + " SET " +
                    " timeTickRecord = " + d.timeTickRecord
                    + " , IsMouseOrKeyboard = " + (int)d.isMouseOrKeyboard
                    + " , IsUpOrDown = " + (int)d.isUpOrDown + " ";

                if (d.isMouseOrKeyboard == CoreData.IsMouseOrKeyboard.Mouse)
                {
                    sql += " , MouseButtons = " + (int)d.mouseEventArgs.Button
                    + "  , MouseX =  " + d.mouseEventArgs.X
                    + "  , MouseY =  " + d.mouseEventArgs.Y
                    + "  , MouseDelta =  " + d.mouseEventArgs.Delta
                    + " ; ";
                }

                if (d.isMouseOrKeyboard == CoreData.IsMouseOrKeyboard.Keyboard)
                {
                    sql += " , KeyEventArgs =  " + (int)d.keyEventArgs.KeyData
                        + " ; ";
                }
                p++;
                sql += " \n ";
                if (p > 1000)
                {
                    p = 0;
                    SQLManage.command(sql);
                    sql = "";
                }
                form9.progressBar1.PerformStep();
            }
            if (p > 0)
            {
                p = 0;
                SQLManage.command(sql);
                sql = "";
            }
            //SQLManage.commands(3, sql);
            flush();
            form9.Close();
            form9.Dispose();
        }

        void DownData(int mainkey)
        {
            string sql;

            //获取动作长度
            sql = "SELECT mainKey FROM data"+mainkey+" ORDER BY mainKey DESC LIMIT 1;";
            MySqlDataReader mySql = SQLManage.GetReader(sql);
            mySql.Read();
            int size = (int)mySql[0];
            SQLManage.closeConn();

            //弹出加载窗口
            ProgressBarForm form9 = new ProgressBarForm("正在下载动作......");
            form9.progressBar1.Maximum = size;
            form9.progressBar1.Step = 1;
            form9.Show();

            //创建临时动作保存
            List<CoreData> date1 = new List<CoreData>();
            sql = "SELECT * FROM data"+mainkey+" ORDER BY mainKey;";
            mySql = SQLManage.GetReader(sql);
            while (mySql.Read())
            {
                //将数据库的动作读取出来

                long x1 = (long)mySql[1]; 
                CoreData.IsMouseOrKeyboard x2 = (CoreData.IsMouseOrKeyboard)mySql[2];
                CoreData.IsUpOrDown x3 = (CoreData.IsUpOrDown)mySql[3];
                if(x2 == CoreData.IsMouseOrKeyboard.Keyboard)
                {
                    Keys x4 = (Keys)mySql[4];
                    date1.Add(CoreData.create(
                        x1,
                    x2,
                    x3,
                    new KeyEventArgs(x4)
                    ));
                }
                if (x2 == CoreData.IsMouseOrKeyboard.Mouse)
                {
                    MouseButtons x5 = (MouseButtons)mySql[5];
                    int x6 = (int)mySql[6];
                    int x7 = (int)mySql[7];
                    int x8 = (int)mySql[8];
                    date1.Add(CoreData.create(
                        x1,
                    x2,
                    x3,
                    new MouseEventArgs(x5,0,x6,x7,x8)
                    ));
                }
                //进度条往前一格
                form9.progressBar1.PerformStep();
            }
            //读取完毕
            SQLManage.closeConn();

            //读取动作名称长度介绍等信息
            sql = "SELECT * FROM amainform WHERE mainKey = "+ mainkey + ";";
            mySql = SQLManage.GetReader(sql);
            mySql.Read();
            formName = (string)mySql[1];
            formTime = (string)mySql[2];
            formMessage = (string)mySql[3];
            SQLManage.closeConn();
            form9.Close();
            form9.Dispose();

            //同步主窗口和本窗口的数据
            dates = date1;
            mainForm.dates = date1;
            mainForm.formName = formName;
            mainForm.formTime = formTime;
            mainForm.formMessage = formMessage;

            MessageBox.Show(formName + "\n动作下载完成");
        }


        //点击上传动作时
        private void button1_Click(object sender, EventArgs e)
        {
            ServerUploadForm form8 = new ServerUploadForm();
            form8.formTime = formTime;

            form8.ShowDialog();
            if (form8.ans)
            {
                formName = form8.formName;
                formMessage = form8.formMessage;
                UpData();
            }
            form8.Dispose();
        }

        //点击刷新时
        private void button2_Click(object sender, EventArgs e)
        {
            flush();
        }




        //点击查看动作时
        private void button3_Click(object sender, EventArgs e)
        {
            //获取选中mainKey
            //dataGridView1.CurrentRow.Index;
            int index = dataGridView1.CurrentRow.Index;
            int mainkey = int.Parse(dataGridView1.Rows[index].Cells[0].Value.ToString());
            //测试点
            //MessageBox.Show(mainkey+"");

            //读取选中的动作信息并召唤弹窗
            string s1 = dataGridView1.Rows[index].Cells[1].Value.ToString();
            string s2 = dataGridView1.Rows[index].Cells[2].Value.ToString();
            string s3 = dataGridView1.Rows[index].Cells[3].Value.ToString();
            ServerDownloadForm form10 = new ServerDownloadForm(s1,s2,s3);
            form10.ShowDialog();
            if (form10.isAns)
            {
                DownData(mainkey);
            }
            form10.Dispose();
        }
    }
}
