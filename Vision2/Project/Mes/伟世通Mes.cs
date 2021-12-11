using ErosSocket.DebugPLC.Robot;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Vision2.Project.formula;

namespace Vision2.Project.Mes
{
    public class 伟世通Mes : MesInfon
    {
        public override event IMesData.ResTMesd ResDoneEvent;

        public static void WeirtFlg(string path, string code, bool ok)
        {
            path = path + "//TestFlag//" + code + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            if (ok)
            {
                Vision2.ErosProjcetDLL.Excel.Npoi.WriteF(path, new List<string> { "1" }, ".flg");
            }
            else
            {
                Vision2.ErosProjcetDLL.Excel.Npoi.WriteF(path, new List<string> { "2" }, ".flg");
            }
        }

        public static void WeirtDATA(string path, string code, List<string> ListText)
        {
            path = path + "//TestData//" + code + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            Vision2.ErosProjcetDLL.Excel.Npoi.WriteF(path, ListText, ".dat");
        }



        public override bool ReadMes(out string resetMesString, TrayData trayData)
        {
            throw new NotImplementedException();
        }

        public override bool ReadMes(string sn, out string resetMesString)
        {
            throw new NotImplementedException();
        }

        public override void WrietMesAll<T>(T datas,  string Product_Name)
        {
            WrietMes(datas as OneDataVale, Product_Name);
        }

        public override void WrietMes(TrayData data, string Product_Name)
        {
            for (int i = 0; i < data.GetDataVales().Count; i++)
            {
                WrietMes(data.GetDataVales()[i], Product_Name);
            }
        }

        public override void WrietMes(OneDataVale trayData, string Product_Name)
        {
            try
            {
                if (trayData.PanelID.Length < 4)
                {
                    return;
                }
                List<string> ListText = new List<string>();
                string timeStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string timeLong =  DateTime.Now.ToString("yyyy年M月d日");
                string reset = "";
                ListText.Clear();
                if (trayData.OK)
                {
                    reset = "P";
                }
                else
                {
                    reset = "F";
                }
                ListText.Add("ObjectID=" + trayData.PanelID);
                ListText.Add("StartTime=" + UserFormulaContrsl.timeStrStrat.ToString("yyyy-MM-dd HH:mm:ss"));
                ListText.Add("TestSteps=0");
                ListText.Add("No.   Test Item   Low Limit   Criterion High Limit Unit    P / F      Value");
                ListText.Add("1	   焊点检查	   1	  GELE	  1	  BOOL	  " + reset + "   1");
                ListText.Add("EndTime=" + timeStr);
                伟世通Mes.WeirtFlg(ProcessControl.ProcessUser.Instancen.ExcelPath, trayData.PanelID, trayData.OK);
                伟世通Mes.WeirtDATA(ProcessControl.ProcessUser.Instancen.ExcelPath, trayData.PanelID, ListText);
                string st1 = DateTime.Now.ToString("HH:mm");
                string st2 = "6:30";
                string st3 = "18:30";
                DateTime dt1 = Convert.ToDateTime(st1);

                DateTime dt2 = Convert.ToDateTime(st2);

                DateTime dt3 = Convert.ToDateTime(st3);
                string DiName = DateTime.Now.ToString("yyyyMMdd");

                if (DateTime.Compare(dt1, dt3) > 0 || (DateTime.Compare(dt1, dt2) > 0))
                {
                    //DiName = DateTime.Now.Subtract(). ToString("yyyyMMdd");
                }
                string path = ProcessControl.ProcessUser.Instancen.ExcelPath + "//" + DiName + "//"
               + trayData.PanelID + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + reset;
                ErosProjcetDLL.Excel.Npoi.WriteF(path, ListText, ".dat");
            }
            catch (Exception ex)
            {
                ErosProjcetDLL.Project.AlarmText.LogErr(ex.Message, "写入数据");
            }
        }

        public override Form GetForm()
        {
            return null;
        }
    }
}