using System;
using System.IO;
using System.Windows.Forms;

namespace Rock
{
    public class UnhandledExceptionLogger
    {
        public UnhandledExceptionLogger(string logFile, Exception exception)
        {
            if (File.Exists(logFile))
            {
                using (StreamWriter sw = File.AppendText(logFile))
                {
                    sw.WriteLine(DateTime.Now.ToString("MM-dd hh:mm:ss"));
                    sw.WriteLine("Exception message:\n " + exception.Message);
                    sw.WriteLine("Stack trace:\n" + exception.StackTrace + "\n\n");
                }
            }
            else
            {
                using (StreamWriter sw = File.CreateText(logFile))
                {
                    sw.WriteLine(DateTime.Now.ToString("MM-dd hh:mm:ss"));
                    sw.WriteLine("Exception message:\n " + exception.Message);
                    sw.WriteLine("Stack trace:\n" + exception.StackTrace + "\n\n");
                }
            }
            MessageBox.Show("已保存异常信息。请重启软件并联系软件维护人员。", "未处理的异常",MessageBoxButtons.OK,MessageBoxIcon.Error);
            Application.Exit();
        }
    }
}
