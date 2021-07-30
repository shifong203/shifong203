using ErosSocket.DebugPLC.Robot;
using ErosSocket.ErosConLink;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;
using Vision2.Project.formula;

namespace Vision2.Project.Mes.环旭SISF
{
  public  class SISF : MesInfon
  {
        [DescriptionAttribute("通信连接名称。"), Category("通信连接"), DisplayName("通信名")]
        public string LinkName { get; set; } = "";
      
        public  void initialization()
        {

            SocketClint = ErosSocket.ErosConLink.StaticCon.GetSocketClint(LinkName);
            if (SocketClint!=null)
            {
                SocketClint.PassiveEvent += SocketClint_PassiveEvent; 
            }
            //SISF sISF = new SISF();
            //  base.ReadThis("", sISF);   
            //SocketClint.IP = IP;
            //SocketClint.Port = Prot;
            //SocketClint.initialization();
        }
        public class SisfData
        {

        }

        public string Send(params string[] TEXT)
        {
            string dataStr = "";
            try
            {
              
                for (int i = 0; i < TEXT.Length; i++)
                {
                    dataStr += TEXT[i] + ",";
                }
                dataStr = dataStr.Trim(',');
          
                this.GetSocketClint().Send(dataStr);
            }
            catch (Exception ex)
            {
               AlarmText.AddTextNewLine("SISF写入错误:"+ex.Message,Color.Red);
            }
            dataStr = DateTime.Now +"S:"+ dataStr;
            ErosProjcetDLL.Excel.Npoi.AddTextLine(ProcessControl.ProcessUser.GetThis().ExcelPath +
                      DateTime.Now.ToString("年月日") + "\\SFIS记录",  dataStr);
            return dataStr;
        }

        public string SendText1(string Cid)
        {
           return   Send(Fixture_ID, Cid,"7",ProjectINI.In.UserID, Line_Name,"", Status, CODE_NAME,"","","", "", "", "");
        }
        public string SendText2(string Cid,params string[] traySn)
        {
           return  Send(Fixture_ID, Cid, "2", ProjectINI.In.UserID, Line_Name, "", Status, "PANEL_SERIAL_NUMBER="+Cid+ ";1_SUFFIX_SERIAL_NUMBER="+traySn[0] + ";2_SUFFIX_SERIAL_NUMBER="+traySn[1]);
        }
        [DescriptionAttribute("线体标示。"), Category("设备标识"), DisplayName("Fixture_ID线体名")]
        public string Fixture_ID { get; set; } = "EQDIW00011-01";

   
        [DescriptionAttribute("位置标示。"), Category("设备标识"), DisplayName("Line位置标示")]
        public string Line_Name { get; set; } = "JORDAN";

        [DescriptionAttribute("状态。"), Category("设备标识"), DisplayName("Status状态")]
        public string Status { get; set; } = "OK";

        [DescriptionAttribute("CODE_NAME,"), Category("设备标识"), DisplayName("CODE_NAME")]
        public string CODE_NAME { get; set; } = "";


        private string SocketClint_PassiveEvent(byte[] key,
            ErosSocket.ErosConLink.SocketClint socket, System.Net.Sockets.Socket socketR)
        {

            return "";
        }
        public SocketClint GetSocketClint()
        {
            if (SocketClint==null)
            {
                initialization();
            }
            return SocketClint;
        }
        SocketClint SocketClint;

        public override event IMesData.ResTMesd ResDoneEvent;

        //public bool SendText(string magazineId,string sn, string userID)
        //{
        //    try
        //    {
        //        if (SocketClint != null)
        //        {
        //            SFIS(Fixture_ID, magazineId, Step, userID, Line_Name, "", Status, "", "","","","",
        //                    "", "", "", sn.TrimEnd(';'), "");
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    ErosProjcetDLL.Excel.Npoi.AddTextLine(ProcessControl.ProcessUser.GetThis().ExcelPath +
        //          DateTime.Now.ToString("年月日") + "\\SFIS记录", DateTime.Now+ " MID:" + magazineId + ";SN:" + sn + ";userID:" + userID );
        //    return false;
        //}
  

        public override Form GetForm()
        {
            return new SisfForm1(this);
        }

        public override void WrietMes(UserFormulaContrsl userFormulaContrsl, string QRCODE, string Product_Name)
        {
            throw new NotImplementedException();
        }

        public override void WrietMes(TrayData trayData, string Product_Name)
        {
            throw new NotImplementedException();
        }

        public override void WrietMes(OneDataVale trayData, string Product_Name)
        {
            throw new NotImplementedException();
        }

        public override void WrietMesAll<T>(T data, string QRCODE, string Product_Name)
        {
            throw new NotImplementedException();
        }

        public override bool ReadMes(string SerialNumber, out string resetMesString)
        {
            throw new NotImplementedException();
        }

        public override bool ReadMes(out string resetMesString)
        {
            throw new NotImplementedException();
        }
    }
}
