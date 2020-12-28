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

namespace 鼠标键盘连点器
{
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
        }

        public List<Date1> dates = new List<Date1>();
        public string formName;
        public string formTime;
        public string formMessage;


        private void Form7_Load(object sender, EventArgs e)
        {
            //string sql = "select TABLE_NAME from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA='Mouse-and-keyboard-action-recording-player' and TABLE_NAME='date1'";
            flush();

        }

        void flush()
        {
            if (dataGridView1.Rows.Count > 0) { dataGridView1.Rows.Clear(); }
            string sql = "select * from amainform";
            MySqlDataReader mySql = SQLManage.GetReader(sql);
            int record = 0;
            while (mySql.Read())
            {
                record++;
                int index = dataGridView1.Rows.Add();
                int p = 0;
                dataGridView1.Rows[index].Cells[p++].Value = mySql[p];
                dataGridView1.Rows[index].Cells[p++].Value = mySql[p];
                dataGridView1.Rows[index].Cells[p++].Value = mySql[p];
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

        void UpData()
        {
            Form9 form9 = new Form9("正在上传......", "正在上传动作");
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
            foreach (Date1 d in dates)
            {
                //将每个动作保存至数据库中
                sql += "INSERT INTO data" + mainKey
                    + " SET " +
                    " timeTickRecord = " + d.timeTickRecord
                    + " , IsMouseOrKeyboard = " + (int)d.isMouseOrKeyboard
                    + " , IsUpOrDown = " + (int)d.isUpOrDown + " ";

                if (d.isMouseOrKeyboard == Date1.IsMouseOrKeyboard.Mouse)
                {
                    sql += " , MouseButtons = " + (int)d.mouseEventArgs.Button
                    + "  , MouseX =  " + d.mouseEventArgs.X
                    + "  , MouseY =  " + d.mouseEventArgs.Y
                    + "  , MouseDelta =  " + d.mouseEventArgs.Delta
                    + " ; ";
                }

                if (d.isMouseOrKeyboard == Date1.IsMouseOrKeyboard.Keyboard)
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

        private void button1_Click(object sender, EventArgs e)
        {
            Form8 form8 = new Form8();
            form8.formTime = formTime;
            form8.ShowDialog();
            if (form8.ans)
            {
                formName = form8.formName;
                formMessage = form8.formMessage;
            }
            form8.Dispose();
            UpData();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            flush();
        }
    }
}
