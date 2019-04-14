using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Sunway
{
    public partial class Form1 : Form
    {
        public delegate void del(string n);

        public 加载窗体 T2;

        public Form1()
        {
            var ld = new Thread(LoadForm);
            ld.IsBackground = true;
            ld.Start();
            InitializeComponent();
            sheep.f1 = this;
        }

        public void LoadForm()
        {
            T2 = new 加载窗体();
            T2.ShowDialog();
        }

        public void T2Close(string n)
        {
            T2.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sheep.ini();
            sheep.LoadVpp();
            del T2C = T2Close;
            Invoke(T2C, "");

            sheep.listenThread = new Thread(sheep.listen);
            sheep.listenThread.IsBackground = true;
            sheep.listenThread.Start();
            sheep.excutionThread = new Thread(sheep.ListenRun);
            sheep.excutionThread.IsBackground = true;
            sheep.excutionThread.Start();
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
            var T = new 模板设置();
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