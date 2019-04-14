using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rock;

namespace Sunway
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
          //  try
          //  {
                Application.Run(new Form1());
           // }
          //  catch (Exception e)
           // {
           //     var logger = new UnhandledExceptionLogger(sheep.exceptionFile, e);
           // }
        }
    }
}
