using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cognex.VisionPro.ToolGroup;
using Cognex.VisionPro.ToolBlock;
using Cognex.VisionPro.ToolGroup;
using Cognex.VisionPro;
using Cognex.VisionPro.ResultsAnalysis;
using Cognex.VisionPro.ToolBlock;
using Cognex.VisionPro.Display;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using DataBind;
using System.Drawing;
using Cognex.VisionPro.PMAlign;
using Rock;
namespace Sunway
{
  public static   class sheep
  {
      public static volatile bool application_isRunning = true;
      private static ushort out_OK_bit = 10, out_NG_bit = 11, out_NOPRODUCT_bit = 12;
      public static string exceptionFile = "exceptionLog.txt";
      public static Form1 f1;
      public static CogToolGroup group;
      public static CogToolBlock block;
      public static string datapath = Directory.GetCurrentDirectory() + @"\data\";
      public static int sign=1;
      public static bool RunBool = false;
      public static double X1, X2, X3, X4, Y;
      public static volatile bool RunB ;
      public static Thread listenThread, excutionThread;
      public static string projectDir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
      public static string vppFilePath = Path.Combine(projectDir, "Template.vpp");


      public static void ini()
      {

          f1.模板设置.Enabled = false;
          f1.图片.Enabled = false;
          f1.数据日志.Enabled = false;
          f1.手动运行.Enabled = false;
          f1.登出.Enabled = false;
          try
          {
              int a = IOC0640.ioc_board_init();
              if (a > 0)
              {
                  f1.textBox2.AppendText("IO卡打开成功\r\n");
              }
          }
          catch
          {
              f1.textBox2.AppendText("IO卡打开失败\r\n");
          }
          
         
      
      }

      public static void ini2()
      {

          f1.模板设置.Enabled = true;
          f1.图片.Enabled = true;
          f1.数据日志.Enabled = true;
          f1.手动运行.Enabled = true;
          f1.登出.Enabled = true;

      }
      public static void LoadVpp()
      {
          try
          {
              group = CogSerializer.LoadObjectFromFile(datapath + @"\Template.vpp") as CogToolGroup;
             block = group.Tools[0] as CogToolBlock;
              f1.textBox2.AppendText("工具组加载成功\r\n");
          }
          catch (Exception x)
          {
              f1.textBox2.AppendText("工具加载失败:" + x.Message);
          }
         
         
      }
      public static void SaveVpp()
      {
          try
          {
              CogSerializer.SaveObjectToFile(sheep.group, sheep.datapath + @"\Template.vpp");
                LoadVpp();
              MessageBox.Show("保存成功");

          }
          catch
          {
              MessageBox.Show("保存失败");
          }
      
      }
      public static void SaveLog()
      {
          //创建文件夹
          string dir = AppDomain.CurrentDomain.BaseDirectory + "Log";
          if (!Directory.Exists(dir))
          {
              Directory.CreateDirectory(dir);
          }

          //新文件则添加表头
          string fileName = dir + "\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".csv";
          if (!File.Exists(fileName))
          {
              StreamWriter sw = new StreamWriter(fileName, true);
              string title = "时间,Y1,X4,X3,X2,X1,结果";
              sw.WriteLine(title);
              sw.Flush();
              sw.Close();
          }

          string item = DateTime.Now.ToString("HH:mm:ss:fff")
              + "," + Y.ToString("f3")
              + "," + X1.ToString("f3")
              + "," + X2.ToString("f3")
              + "," + X3.ToString("f3")
              + "," + X4.ToString("f3");
              //+ "," + WZ1.ToString("f3")
              //+ "," + WZ2.ToString("f3")
              //+ "," + WZ3.ToString("f3")
              //+ "," + WZ11.ToString("f3")
              //+ "," + WZ31.ToString("f3")
            

          StreamWriter sw2 = new StreamWriter(fileName, true);
          sw2.WriteLine(item);
          sw2.Flush();
          sw2.Close();

          //删除过期文件
          string[] files = Directory.GetFiles(dir);
          for (int i = 0; i < files.Length; i++)
          {
              DateTime dt = File.GetCreationTime(files[i]);
              TimeSpan ts = DateTime.Now - dt;
              if (ts.Days > 30)
              {
                  File.Delete(files[i]);
              }
          }
      }

      public static void SaveImage(CogRecordDisplay crd, bool b)
      {
          if (b == true)
          {
              string ymd = DateTime.Now.ToString("yyyy-MM-dd");
              string mtime = DateTime.Now.ToString("hh点mm分ss秒");
              string path1 = datapath + @"Image\" + ymd;


              CogRectangle crt = new CogRectangle();

              if (File.Exists(path1))
              {
                  crd.CreateContentBitmap(CogDisplayContentBitmapConstants.Image, crt, 0).Save(path1 + @"\" + mtime + ".jpg");
              }
              else
              {
                  Directory.CreateDirectory(path1);
                  crd.CreateContentBitmap(CogDisplayContentBitmapConstants.Image, crt, 0).Save(path1 + @"\" + mtime + ".jpg");


              }
          }
      }
      public delegate void del(string n ,System .Windows .Forms.Label lab);
      public static void Light(string n, System.Windows.Forms.Label lab)
      {
          if (n=="1")
          { lab.ForeColor = Color.Green; }

          if (n == "2")
          { lab.ForeColor  = Color.Silver; }

         
      
      }
      public static void listen()
      {

        //  try
         // {
              del del = new del(Light);
              while (application_isRunning)
              {
              
                  f1.Invoke(del, "2", f1.label3);
                  var sign1 = IOC0640.ioc_read_inbit(0, 1);
                  if (sign1 != sign)
                  {
                      sign = sign1;
                      if (sign == 0)
                      {
                          f1.label3.Invoke(del, "1", f1.label3);
                          RunB = true;
                      }

                  }
                  Thread.Sleep(20);

              }
          }
          //catch (Exception e)
         // {
        //      var logger = new UnhandledExceptionLogger(sheep.exceptionFile, e);
        //  }
          
          
    
    //  }
      public static void ListenRun()
      {
        //  try
        //  {
              while (application_isRunning)
              {
                  Thread.Sleep(20);
                  if (RunB == true)
                  {
                    run();
                      RunB = false;
                  }
              }
        //  }
        //  catch (Exception e)
        //  {
        //      var logger = new UnhandledExceptionLogger(sheep.exceptionFile, e);
         // }


      
      
      }
      public static void GCF(CogRecordDisplay mdisplay, string s1, string font, int size, double d1, double d2, string lab, CogColorConstants mcolor, CogGraphicLabelAlignmentConstants mali)
      {
          CogGraphicLabel mlable = new CogGraphicLabel();
          //mfont = "微软雅黑";

          System.Drawing.Font mfont = new System.Drawing.Font(font, size, FontStyle.Regular);
          mlable.GraphicDOFEnableBase = CogGraphicDOFConstants.None;
          mlable.SelectedSpaceName = s1;
          mlable.Interactive = false;
          mlable.Font = mfont;
          mlable.Color = mcolor;
          mlable.Alignment = mali;
          mlable.SetXYText(d1, d2, lab);
          mdisplay.InteractiveGraphics.Add(mlable, "", false);

      }
      public static void run()
      {
          block.Run();
        
          f1.cogRecordDisplay1.StaticGraphics.Clear();
          f1.cogRecordDisplay1.InteractiveGraphics.Clear();
          f1.cogRecordDisplay1.Image = null;

          ICogRecord icg = block.CreateLastRunRecord();
           f1. cogRecordDisplay1.Record = icg.SubRecords["CogIPOneImageTool1.OutputImage"];
           f1. cogRecordDisplay1.AutoFit = true;

           del del2 = new del(Light );

          // f1.label4.Invoke(del2, "2", f1.label4);
           
          


           if (block.RunStatus.Result == CogToolResultConstants.Accept)
           {
               f1.label4.Invoke(del2, "1", f1.label4);
               GCF(f1.cogRecordDisplay1, "#", "微软雅黑", 20, 100, 100, "OK", CogColorConstants.Green, CogGraphicLabelAlignmentConstants.TopLeft);
               IOC0640.ioc_write_outbit(0, out_OK_bit, 0);
               Thread.Sleep(100);
               IOC0640.ioc_write_outbit(0, out_OK_bit, 1);
               f1.label4.Invoke(del2, "2", f1.label4);
              
             
           }

           if (block.RunStatus.Result != CogToolResultConstants.Accept)
           {
               f1.label4.Invoke(del2, "1", f1.label5);
               GCF(f1.cogRecordDisplay1, "#", "微软雅黑", 20, 100, 100, "NG", CogColorConstants.Red , CogGraphicLabelAlignmentConstants.TopLeft);
               IOC0640.ioc_write_outbit(0, out_NG_bit, 0);
               Thread.Sleep(100);
               IOC0640.ioc_write_outbit(0, out_NG_bit, 1);
               f1.label4.Invoke(del2, "2", f1.label5);
               SaveImage(f1.cogRecordDisplay1 ,true );
           }

           CogPMAlignTool pma = (CogPMAlignTool)block.Tools["判断有无料"];
           if (pma.Results.Count == 0)
           {
               f1.label4.Invoke(del2, "1", f1.label7);
               GCF(f1.cogRecordDisplay1, "#", "微软雅黑", 20, 100, 100, "无料", CogColorConstants.Red, CogGraphicLabelAlignmentConstants.TopLeft);
               IOC0640.ioc_write_outbit(0, out_NOPRODUCT_bit, 0);
               Thread.Sleep(100);
               IOC0640.ioc_write_outbit(0, out_NOPRODUCT_bit, 1);
               f1.label4.Invoke(del2, "2", f1.label7);
               SaveImage(f1.cogRecordDisplay1, true);
           }




           Y = (double)block.Outputs["Y"].Value;
           X1 = (double)block.Outputs["X1"].Value;
           X2 = (double)block.Outputs["X2"].Value;
           X3 = (double)block.Outputs["X3"].Value;
           X4 = (double)block.Outputs["X4"].Value;
           SaveImage(f1.cogRecordDisplay1, true);
           SaveLog();
      
      
      }

      public static void HandRun()
      {

          block.Run();

          f1.cogRecordDisplay1.StaticGraphics.Clear();
          f1.cogRecordDisplay1.InteractiveGraphics.Clear();
          f1.cogRecordDisplay1.Image = null;

          ICogRecord icg = block.CreateLastRunRecord();
          f1.cogRecordDisplay1.Record = icg.SubRecords["CogIPOneImageTool1.OutputImage"];
          f1.cogRecordDisplay1.AutoFit = true;
          SaveLog();
      }




    }
}
