using ErosSocket.DebugPLC.Robot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision2.Project.formula;

namespace Vision2.Project.Mes
{
   public class DefaultMes : MesInfon
    {
    
        public override event IMesData.ResTMesd ResDoneEvent;

        public override Form GetForm()
        {
            return new DefaultMesForm1(this);
        }

        public override bool ReadMes(out string resetMesString, TrayData trayData)
        {
            throw new NotImplementedException();
        }

        public override bool ReadMes(string sn, out string resetMesString)
        {
            throw new NotImplementedException();
        }

    

  

       

        public override void WrietMes(TrayData trayData, string Product_Name)
        {
            try
            {
                for (int i = 0; i < trayData.Count; i++)
                {
                    WrietMes(trayData.GetOneDataVale(i));
                }
            }
            catch (Exception ex)
            {
            }
        }

        public override void WrietMes(OneDataVale oneData, string Product_Name)
        {
            List<string> ListText = new List<string>();
            string timeStr = DateTime.Now.ToString();
            string timeLong = DateTime.Now.ToString("yyyy年M月d日");
            string paht = ProcessControl.ProcessUser.Instancen.ExcelPath + "//" + oneData.PanelID;
            //if (RecipeCompiler.Instance.TrayQRType == RecipeCompiler.TrayEnumType.多个流程一个产品)
            //{
            //    paht = ProcessControl.ProcessUser.Instancen.ExcelPath + "//" + DateTime.Now.ToString("yyyy年M月d日HH时mm分ss秒");
            //}
            if (System.IO.File.Exists(paht))
            {
                System.IO.File.Delete(paht);
            }
            string conmtext = "程序名,条码,检测结果,开始时间,结束时间";
            string QrCode = oneData.PanelID;
            string reset ="";
            if (oneData.OK)
            {
                reset = "OK";
            }
            else
            {
                reset = "NG";
            }
            string MesData = "";
            foreach (var item in oneData.GetNGCompData().DicOnes)
            {
                string dd = item.Value.ComponentID ;
                conmtext += ","+ dd;
            //    +"," + item.Value.NGText + "," + item.Value.RestText + ","
                if (item.Value.aOK)
                {
                    MesData += ",";
                }
                else
                {
                    MesData += ","+ item.Value.NGText;
                }
            }
            conmtext += ",机检结果,复判结果";
            if (oneData.AutoOK)
            {
                MesData += ",OK";
            }
            else
            {
                MesData += ",NG";
            }
            if (oneData.OK)
            {
                MesData += ",OK" ;
            }
            else
            {
                MesData += ",NG";
            }
           
            ListText.Add(conmtext);
            ListText.Add(Product.ProductionName + "," + QrCode + "," + reset + "," + 
                oneData.StrTime.ToString("yyyy-MM-dd HH:mm:ss")
                + "," + timeStr + MesData);
            //ListText.Add("检查项,结果,复判结果, ,");
            //foreach (var item in oneData.GetNGCompData().DicOnes)
            //{
            //    string dd = item.Value.ComponentID + ","+ item.Value.NGText+","+ item.Value.RestText+",";
     
            //    if (!ListText.Contains(dd))
            //        {
            //            ListText.Add(dd);
            //        }
            //}

            QrCode = "";
            ErosProjcetDLL.Excel.Npoi.WriteF(paht, ListText);
            if (ListText.Count == 1)
            {
                ErosProjcetDLL.Project.AlarmText.LogErr("写入Mes错误长度1", "写入数据");
            }
        }
    }   
}
