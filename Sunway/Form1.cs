using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cognex.VisionPro.ToolGroup;
using Cognex.VisionPro.ToolBlock;
using Cognex.VisionPro;
using System.Threading;
using Cognex.VisionPro.Display;

namespace Sunway
{
    public partial class Form1 : Form
    {
        public 加载窗体 T2;
        public void LoadForm()
        {
            T2 = new 加载窗体();
            T2.ShowDialog();
        }
        public Form1()
        {
            Thread ld = new Thread(LoadForm);
            ld.IsBackground = true;
            ld.Start();
            InitializeComponent();
            sheep.f1 = this;
        }


        public delegate void del(string n);
        public void T2Close(string n)
        {
            T2.Close();
        
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            sheep.ini();
            sheep.LoadVpp();
            del T2C = new del(T2Close);
            this.Invoke(T2C, "");
          
            sheep.listenThread = new Thread(sheep.listen);
//            sheep.listenThread.IsBackground = true;
            sheep.listenThread.Start();
            sheep.excutionThread = new Thread(sheep.ListenRun);
//            sheep.excutionThread.IsBackground = true;
            sheep.excutionThread.Start();
            MessageBox.Show(sheep.vppFilePath);



        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_DoubleClick(object sender, EventArgs e)
        {
            sheep.ini2();
        }

        private void 模板设置_Click(object sender, EventArgs e)
        {
            模板设置 T = new 模板设置();
            T.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sheep.ini();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sheep.HandRun();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            sheep.application_isRunning = false;
            sheep.excutionThread.Join();
            sheep.listenThread.Join();
        }

        private void 图片_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", sheep.datapath + "Image");
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void 数据日志_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", sheep.datapath + "Log");
        }
    }
}
