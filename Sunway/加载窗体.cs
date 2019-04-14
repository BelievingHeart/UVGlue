using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sunway
{
    public partial class 加载窗体 : Form
    {
        public 加载窗体()
        {
            InitializeComponent();
        }
        int x;
        private void timer1_Tick(object sender, EventArgs e)
        {
            x = x + 2;
            if (x > 100)
            {
                x = 0;
            }
            progressBar1.Value = x;
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}
