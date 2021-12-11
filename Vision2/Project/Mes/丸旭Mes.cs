using ErosSocket.DebugPLC.Robot;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Vision2.Project.formula;

namespace Vision2.Project.Mes
{
    public class 丸旭Mes : MesInfon
    {
        public override event IMesData.ResTMesd ResDoneEvent;

        public override Form GetForm()
        {
            return null;
        }

        public override bool ReadMes(out string resetMesString, TrayData trayData)
        {
            resetMesString = "";
            return true;
        }

        public override bool ReadMes(string sn, out string resetMesString)
        {
            resetMesString = "";
            return true;
        }

        public  void WrietMes(UserFormulaContrsl userFormulaContrsl, string QRCODE, string product_Name)
        {
            List<string> ListText = new List<string>();
            string timeStr = DateTime.Now.ToString();
            string timeLong = DateTime.Now.ToString("yyyy年M月d日");
            string paht = ProcessControl.ProcessUser.Instancen.ExcelPath + "//" + QRCODE;
            if (RecipeCompiler.Instance.TrayQRType == RecipeCompiler.TrayEnumType.多个流程一个产品)
            {
                paht = ProcessControl.ProcessUser.Instancen.ExcelPath + "//" + DateTime.Now.ToString("yy年MM月dd日HH时mm分ss秒");
            }
            if (System.IO.File.Exists(paht))
            {
                System.IO.File.Delete(paht);
            }
            ListText.Add("程序名,条码,检测结果,开始时间,结束时间,NG点,机检不良项,人工复判结果,高度");
            string reset = "";
            if (userFormulaContrsl.ListReslutOKTR != null && userFormulaContrsl.ListReslutOKTR.Count != 0)
            {
                for (int i = 0; i < userFormulaContrsl.ListReslutOKTR.Count; i++)
                {
                    if (userFormulaContrsl.ListReslutOKTR[i] != "OK" && userFormulaContrsl.ListReslutOK[i].Split('=')[1] != "")
                    {
                        reset = "NG";
                    }
                }
                if (reset == "")
                {
                    reset = "OK";
                }
            }
            else
            {
                if (userFormulaContrsl.ISOk)
                {
                    reset = "OK";
                }
                else
                {
                    reset = "NG";
                }
            }
            string NG1 = "";
            string QrCode = "";
            string Mest = "";
            string jtt = "";
            string jtvales = "";
            string name = "";
            for (int i = 0; i < userFormulaContrsl.ListReslutMestTR.Count; i++)
            {
                Mest += userFormulaContrsl.ListReslutMestTR[i] + ",";
            }
            for (int i = 0; i < userFormulaContrsl.dataGridView1.Rows.Count; i++)
            {
                name = userFormulaContrsl.dataGridView1.Rows[i].Cells[0].Value.ToString();
                if (userFormulaContrsl.dataGridView1.Columns.Contains("二维码"))
                {
                    if (userFormulaContrsl.dataGridView1["二维码", i].Value == null)
                    {
                        QrCode = "无码";
                    }
                    else
                    {
                        QrCode = userFormulaContrsl.dataGridView1["二维码", i].Value.ToString();
                    }
                }
                if (bool.Parse(userFormulaContrsl.dataGridView1["OK", i].Value.ToString()))
                {
                    reset = "OK";
                }
                else
                {
                    reset = "NG";
                }
                if (userFormulaContrsl.ListReslutOKTR.Count > i)
                {
                    NG1 = userFormulaContrsl.ListReslutOKTR[i];
                }
                if (userFormulaContrsl.ListReslutOK.Count > i)
                {
                    jtt = userFormulaContrsl.ListReslutOK[i].Split('=')[0];
                    jtvales = userFormulaContrsl.ListReslutOK[i].Split('=')[1];
                }
                if (jtt == name)
                {
                    if (i == 0)
                    {
                        ListText.Add(Product.ProductionName + "," + QRCODE + "," + reset + "," + UserFormulaContrsl.timeStrStrat.ToString("yyyy-MM-dd HH:mm:ss") + "," + timeStr + "," + name + "," + jtvales + "," + NG1 + "," + Mest);
                    }
                    else
                    {
                        ListText.Add("," + QrCode + "," + reset + ", , ," + name + "," + jtvales + "," + NG1);
                    }
                    QrCode = "";
                }
                else
                {
                    ListText.Add("," + QrCode + "," + reset + ", , ," + name + "," + jtvales + "," + NG1);
                }
                reset = "";
                jtvales = "";
                name = "";
                NG1 = "";
            }
            if (userFormulaContrsl.dataGridView1.Rows.Count == 0)
            {
                ListText.Add(product_Name + "," + QRCODE + "," + reset + "," + UserFormulaContrsl.timeStrStrat.ToString("yyyy-MM-dd HH:mm:ss") + "," + timeStr + ",,," + Mest);
            }
            ErosProjcetDLL.Excel.Npoi.WriteF(paht, ListText);
            if (ListText.Count == 1)
            {
                ErosProjcetDLL.Project.AlarmText.LogErr("写入Mes错误长度1", "写入数据");
            }
        }

        public override void WrietMes(OneDataVale data, string Product_Name)
        {
            try
            {
               
                List<string> ListText = new List<string>();
                string timeStr = DateTime.Now.ToString();
                string timeLong = DateTime.Now.ToString("yyyy年M月d日");
                string paht = ProcessControl.ProcessUser.Instancen.ExcelPath + "//" + data.PanelID;
                if (RecipeCompiler.Instance.TrayQRType == RecipeCompiler.TrayEnumType.多个流程一个产品)
                {
                    paht = ProcessControl.ProcessUser.Instancen.ExcelPath + "//" + DateTime.Now.ToString("yy年MM月dd日HH时mm分ss秒");
                }
                if (System.IO.File.Exists(paht))
                {
                    System.IO.File.Delete(paht);
                }
                ListText.Add("程序名,条码,检测结果,开始时间,结束时间,NG点,机检不良项,人工复判结果,高度");
                string reset = "";
           
                    if (data.OK)
                    {
                        reset = "OK";
                    }
                    else
                    {
                        reset = "NG";
                    }
                
                string NG1 = "";
                string QrCode = "";
                string Mest = "";
                string jtt = "";
                string jtvales = "";
                string name = "";
                ListText.Add(Product.ProductionName + "," + data.PanelID + "," + reset + "," + 
                    data.StrTime.ToString("yyyy-MM-dd HH:mm:ss")  + "," + timeStr + "," + name + "," + jtvales + "," + NG1 + "," + Mest);
                //if (jtt == name)
                //    {
                //        if (i == 0)
                //        {
                //            ListText.Add(Product.ProductionName + "," + data.PanelID + "," + reset + "," + UserFormulaContrsl.timeStrStrat.ToString("yyyy-MM-dd HH:mm:ss") + "," + timeStr + "," + name + "," + jtvales + "," + NG1 + "," + Mest);
                //        }
                //        else
                //        {
                //            ListText.Add("," + QrCode + "," + reset + ", , ," + name + "," + jtvales + "," + NG1);
                //        }
                //        QrCode = "";
                //    }
                //    else
                //    {
                //        ListText.Add("," + QrCode + "," + reset + ", , ," + name + "," + jtvales + "," + NG1);
                //    }
                    reset = "";
                    jtvales = "";
                    name = "";
                    NG1 = "";
                ErosProjcetDLL.Excel.Npoi.WriteF(paht, ListText);
            }
            catch (Exception ex)
            {

            }
        }

        public override void WrietMes(TrayData trayData, string Product_Name)
        {
        }

        public override void WrietMesAll<T>(T data,  string product_Name)
        {
            try
            {
                TrayRobot dataVale = data as TrayRobot;

                List<string> ListText = new List<string>();
                string timeStr = DebugF.DebugCompiler.Instance.DDAxis.StartTime.ToString();
                string timeLong =  DateTime.Now.ToString("yyyy年M月d日");
                string paht = ProcessControl.ProcessUser.Instancen.ExcelPath + "//" + ProcessControl.ProcessUser.QRCode;
                if (RecipeCompiler.Instance.TrayQRType == RecipeCompiler.TrayEnumType.多个流程一个产品)
                {
                    paht = ProcessControl.ProcessUser.Instancen.ExcelPath + "//" + DateTime.Now.ToString("yyyy年M月d日HH时mm分ss秒");
                }
                if (System.IO.File.Exists(paht))
                {
                    System.IO.File.Delete(paht);
                }
                ListText.Add("程序名,条码,检测结果,开始时间,结束时间,NG点,机检不良项,人工复判结果,高度,数据");
                string reset = "";
                string NG1 = "";
                string QrCode = "";
                string Mest = "";
                string jtvales = "";
                string name = "";
                QrCode = dataVale.GetTrayData().TrayIDQR;
                if (dataVale.GetTrayData().OK)
                {
                    reset = "OK";
                }
                else
                {
                    reset = "NG";
                }
                ListText.Add(Product.ProductionName + "," + ProcessControl.ProcessUser.QRCode + "," + reset + "," + UserFormulaContrsl.timeStrStrat.ToString("yyyy-MM-dd HH:mm:ss") + "," + timeStr + "," + name + "," + jtvales + "," + NG1 + "," + Mest);
                QrCode = "";
                foreach (var item in dataVale.GetTrayData().GetDataVales())
                {
                    foreach (var itemDT in item.GetNGCompData().DicOnes)
                    {
                    }
                }

                ErosProjcetDLL.Excel.Npoi.WriteF(paht, ListText);
                if (ListText.Count == 1)
                {
                    ErosProjcetDLL.Project.AlarmText.LogErr("写入Mes错误长度1", "写入数据");
                }
            }
            catch (Exception ex)
            {
                ErosProjcetDLL.Project.AlarmText.AddTextNewLine("写入Mes错误" + ex.Message);
            }
        }
    }
}