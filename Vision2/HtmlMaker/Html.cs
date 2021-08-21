using System;
using System.IO;
using Vision2.Project.formula;
using Vision2.Project.Mes;

namespace Vision2.HtmlMaker
{
    public class Html
    {
        private static string filename = "";
        private static StreamWriter writer = null;

        public static bool GenerateCode(string filepth, double timems, DateTime sTime, DateTime eTime, OneDataVale dataVale)
        {
            // 文件存在时是否覆盖
            filename = filepth + ".html";
            FileInfo f = new FileInfo(filename);
            if (f.Exists)
            {
                f.Delete();
            }
            // 写文件
            FileStream outputfile = null;
            try
            {
                outputfile = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
                writer = new StreamWriter(outputfile);
                writer.BaseStream.Seek(0, SeekOrigin.End);
                MesJib mesJib = RecipeCompiler.Instance.GetMes() as MesJib;
                DoWrite("<HTML>");
                DoWrite("<HEAD>");
                DoWrite("<TITLE> Test Results For Serial Number:" + dataVale.PanelID + " Executed At 4:10:48 PM</TITLE>");
                DoWrite("</HEAD>");
                DoWrite("<BODY>");
                DoWrite("<CENTER><h3><B>Test Report</B></h3></CENTER>");
                DoWrite("<H3>Serial Number:" + dataVale.PanelID + "</h3>");
                //string.Format("{0:R}", sTime)
                DoWrite("<H3>Start Time:" + sTime.ToString() + "</h3>");
                DoWrite("<H3>Stop Time:" + eTime.ToString() + "</h3>");
                DoWrite("<H3>Test Station:" + mesJib.MesData.Testre_Name + "</h3>");
                DoWrite("<H3>Test Operator:" + ErosProjcetDLL.Project.ProjectINI.In.UserID + "</H3>");
                DoWrite("<H3>Test Cell:" + "1" + "</H3>");
                if (dataVale.OK) DoWrite("<H3>Test Result:<FONT COLOR=GREEN>PASS</FONT>");
                else DoWrite("<H3>Test Result:<FONT COLOR=RED>FILL</FONT>");

                DoWrite(@"<H3>Test Script:D:\NGP_SOFT\NGP BOARD STACK DIMENSION TEST_REV.B.JTS</H3>");
                DoWrite("<H3>Test Script Validation Hash:" + "1" + "</H3>");
                DoWrite("<H3>Elapsed Time:" + timems + "</H3>");
                DoWrite("<H3>Total Execution Time:" + timems + "</H3>");
                ///表格
                DoWrite("<TABLE BORDER=1>");
                DoWrite("<TR bgColor=#336699><TD COLSPAN=11><CENTER><B><FONT COLOR=white>Test Results</font></B></CENTER></TD></TR>");
                DoWrite("<TR bgColor=#eeeeee>");
                ///表列
                DoWrite("<TD><CENTER><B>Test Group</B></CENTER></TD>");
                DoWrite("<TD><CENTER><B>Test Name</B></CENTER></TD>");
                DoWrite("<TD><CENTER><B>Test Status</B></CENTER></TD>");
                DoWrite("<TD><CENTER><B>Parametric Test</B></CENTER></TD>");
                DoWrite("<TD><CENTER><B>Is Measurement</B></CENTER></TD>");
                DoWrite("<TD><CENTER><B>Lower Limit</B></CENTER></TD>");
                DoWrite("<TD><CENTER><B>Upper Limit</B></CENTER></TD>");
                DoWrite("<TD><CENTER><B>Measurement</B></CENTER></TD>");
                DoWrite("<TD><CENTER><B>Measurement Unit</B></CENTER></TD>");
                DoWrite("<TD><CENTER><B>Elapsed Time</B></CENTER></TD>");
                DoWrite("<TD><CENTER><B>Execution Time</B></CENTER></TD>");
                ///第一行
                DoWrite("<TR>");
                DoWrite("<TD>Serial Number Check</TD>");
                DoWrite("<TD>SerialNumber</TD>");
                if (dataVale.OK) DoWrite("<TD bgColor=#00ff00><B>PASS</B></TD>");
                else DoWrite("<TD bgColor=RED><B>FILL</B></TD>");

                DoWrite("<TD bgColor=#FFFF99><CENTER>&#149;</CENTER></TD><TD bgColor=#FFFF99><CENTER>&#149;</CENTER></TD><TD  bgColor=#FFFF99></TD>");
                DoWrite("<TD bgColor=#FFFF99></TD>");
                DoWrite("<TD bgColor=#FFFF99>" + dataVale.PanelID + "</TD>");
                DoWrite("<TD>N/A</TD><TD>" + timems + "</TD>");
                DoWrite("<TD>0</TD>");
                DoWrite("</TR>");
                ///第二行
                DoWrite("<TR>");
                DoWrite("<TD>Select Test Mode</TD>");
                DoWrite("<TD>Test mode</TD>");
                if (dataVale.OK)
                {
                    DoWrite("<TD bgColor=#00ff00><B>PASS</B></TD>");
                }
                else
                {
                    DoWrite("<TD bgColor=RED><B>FILL</B></TD>");
                }

                DoWrite("<TD bgColor=#FFFF99><CENTER>&nbsp;</CENTER></TD><TD bgColor=#FFFF99><CENTER>&#149;</CENTER></TD><TD  bgColor=#FFFF99></TD>");
                DoWrite("<TD bgColor=#FFFF99></TD>");
                if (ErosProjcetDLL.Project.ProjectINI.DebugMode)
                {
                    DoWrite("<TD bgColor=#FFFF99>Debug</TD>");
                }
                else
                {
                    DoWrite("<TD bgColor=#FFFF99>PRODUCTION</TD>");
                }
                DoWrite("<TD>N/A</TD><TD>" + timems + "</TD>");
                DoWrite("<TD>0</TD>");
                DoWrite("</TR>");
                ///第3行
                TimeSpan timeSpan = dataVale.EndTime - dataVale.StrTime;

                foreach (var item in dataVale.ListCamsData)
                {
                    int runo = 1;
                    foreach (var itemd in item.Value.GetAllCompOBJs().DicOnes)
                    {
                        DoWrite("<TR>");
                        DoWrite("<TD>Dimension Analysis</TD>");
                        DoWrite("<TD>" + itemd.Key + "</TD>");
                        runo++;
                        if (itemd.Value.OK)
                        {
                            DoWrite("<TD bgColor=#00ff00><B>PASS</B></TD>");
                        }
                        else
                        {
                            DoWrite("<TD bgColor=#ff0000><B>FIll</B></TD>");
                        }
                        DoWrite("<TD bgColor=#FFFF99><CENTER>&#149;</CENTER></TD><TD bgColor=#FFFF99><CENTER>&#149;</CENTER></TD><TD  bgColor=#FFFF99>" + itemd.Value.oneRObjs[0].dataMinMax.Reference_ValueMin[0] + "</TD>");
                        DoWrite("<TD bgColor=#FFFF99>" + itemd.Value.oneRObjs[0].dataMinMax.Reference_ValueMax[0] + "</TD>");
                        DoWrite("<TD bgColor=#FFFF99>" + itemd.Value.oneRObjs[0].dataMinMax.doubleV[0].Value.ToString("0.000000") + "</TD>");
                        DoWrite("<TD>inches</TD><TD>" + timeSpan.TotalSeconds + "</TD>");
                        DoWrite("<TD>0</TD>");
                        DoWrite("</TR>");
                    }
                    break;
                }
                //DoWrite("<TD bgColor=#00ff00><B>PASS</B></TD>");

                /////第4行
                //DoWrite("<TR>");
                //DoWrite("<TD>Dimension Analysis</TD>");
                //DoWrite("<TD>p2_dist</TD>");

                //DoWrite("<TD bgColor=#00ff00><B>PASS</B></TD>");

                //DoWrite("<TD bgColor=#FFFF99><CENTER>&#149;</CENTER></TD><TD bgColor=#FFFF99><CENTER>&#149;</CENTER></TD><TD  bgColor=#FFFF99>0.534</TD>");
                //DoWrite("<TD bgColor=#FFFF99>0.55</TD>");
                //DoWrite("<TD bgColor=#FFFF99>0.545897</TD>");
                //DoWrite("<TD>inches</TD><TD>11.7271991</TD>");
                //DoWrite("<TD>0</TD>");
                //DoWrite("</TR>");
                /////第5行
                //DoWrite("<TR>");
                //DoWrite("<TD>Dimension Analysis</TD>");
                //DoWrite("<TD>p3_dist</TD>");
                //foreach (var item in dataVale.ListCamsData)
                //{
                //    foreach (var itemd in item.Value.AllCompObjs.DicOnes)
                //    {
                //        if (itemd.Value.OK)
                //        {
                //            DoWrite("<TD bgColor=#00ff00><B>PASS</B></TD>");
                //        }
                //        else
                //        {
                //            DoWrite("<TD bgColor=#ff0000><B>FIll</B></TD>");
                //        }
                //        break;
                //    }
                //    break;
                //}

                //DoWrite("<TD bgColor=#FFFF99><CENTER>&#149;</CENTER></TD><TD bgColor=#FFFF99><CENTER>&#149;</CENTER></TD><TD  bgColor=#FFFF99>0.025</TD>");
                //DoWrite("<TD  bgColor=#FFFF99>0.041</TD>");
                //DoWrite("<TD bgColor=#FFFF99>0.03325</TD>");
                //DoWrite("<TD>inches</TD><TD>11.7281986</TD>");
                //DoWrite("<TD>0</TD>");
                //DoWrite("</TR>");
                /////第6行
                //DoWrite("<TR>");
                //DoWrite("<TD>Dimension Analysis</TD>");
                //DoWrite("<TD>p4_dist</TD>");

                //DoWrite("<TD bgColor=#00ff00><B>PASS</B></TD>");

                //DoWrite("<TD bgColor=#FFFF99><CENTER>&#149;</CENTER></TD><TD bgColor=#FFFF99><CENTER>&#149;</CENTER></TD><TD  bgColor=#FFFF99>0.025</TD>");
                //DoWrite("<TD  bgColor=#FFFF99>0.041</TD>");
                //DoWrite("<TD bgColor=#FFFF99>0.033915</TD>");
                //DoWrite("<TD>inches</TD><TD>11.7301974</TD>");
                //DoWrite("<TD>0</TD>");
                //DoWrite("</TR>");

                DoWrite("<HEAD>");
                //DoWrite("<HTML>");
                //DoWrite("<HEAD>");

                //DoWrite("</HEAD>");
                //DoWrite("<TITLE>" + sn + "</TITLE>");
                //DoWrite("<BODY BGCOLOR='white'>");
                //DoWrite("<CENTER>");
                //DoWrite("<BR><BR><H2>Trst Report</H2>");
                //DoWrite("</CENTER>");
                //DoWrite("<Label>");
                //DoWrite("<H2>Serial Number:" + sn + "</H2>");
                //DoWrite("<H2>Start Time:" + sTime + "</H2>");

                //DoWrite("<H2>Stop Time:" + eTime + "</H2>");
                //DoWrite("<H2>Text Station:" + RecipeCompiler.Instance.MesDatat.Testre_Name + "</H2>");
                //DoWrite("<H2>Text Operator:" + RecipeCompiler.Instance.MesDatat.UserID + "</H2>");
                //DoWrite("<H2>Text Cell:" + "1" + "</H2>");
                //DoWrite("<H2>Text Result:" + pass + "</H2>");
                //DoWrite("<H2>Text Script:" + filename + "</H2>");
                //DoWrite("<H2>Text Script Validation Hash:" + "1" + "</H2>");
                //DoWrite("<H2>Elapsed Time:" + timems + "</H2>");
                //DoWrite("<H2>Total Execution Time:" + timems + "</H2>");
                //DoWrite("</Label>");
                //DoWrite("</DataGrid >");

                //DoWrite("</BODY>");
                //DoWrite("</HTML>");

                //DoWrite("</HTML>");
                writer.Close();
                return true;
                //////
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception GenerateCode = " + ex);
                //stbMsg.Text = "Error";
                outputfile = null;
                writer = null;
            }
            return false;
        }

        private static void DoWrite(String line)
        {
            writer.WriteLine(line);
            writer.Flush();
        }
    }
}