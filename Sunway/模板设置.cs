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
    public partial class 模板设置 : Form
    {
        public 模板设置()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sheep.SaveVpp();
        }

        private void 模板设置_Load(object sender, EventArgs e)
        {
          cogToolGroupEditV21.Subject = sheep.group;
        }
    }
}
