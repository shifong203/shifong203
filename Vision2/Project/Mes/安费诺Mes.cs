using AmphenolMES;
using ErosSocket.DebugPLC.Robot;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Vision2.Project.formula;

namespace Vision2.Project.Mes
{
    public class 安费诺Mes : MesInfon
    {
        private mespublic mespubl = new mespublic();

        public override event IMesData.ResTMesd ResDoneEvent;

        public class 安费诺MesData
        {
            [Description("员工信息"), Category("Mes标识"), DisplayName("员工信息")]
            /// <summary>
            /// 员工信息
            /// </summary>
            public string UserCode { get; set; } = "";

            [Description("资源码"), Category("Mes标识"), DisplayName("资源码")]
            /// <summary>
            /// 资源码
            /// </summary>
            public string ResCode { get; set; } = "";

            [Description("软件版本"), Category("Mes标识"), DisplayName("软件版本")]
            /// <summary>
            /// 软件版本
            /// </summary>
            public string SWVesion { get; set; } = "";

            [Description("不良代码组"), Category("Mes标识"), DisplayName("不良代码组")]
            /// <summary>
            /// 不良代码组
            /// </summary>
            public string ecgcode { get; set; } = "";

            [Description("不良代码"), Category("Mes标识"), DisplayName("不良代码")]
            /// <summary>
            /// 不良代码
            /// </summary>
            public string Ecode { get; set; } = "";
        }

        public 安费诺Mes()
        {
            //System.Data.OleDb.OleDbCommand oleDbCommand = new System.Data.OleDb.OleDbCommand();
        }

        public override bool ReadMes(out string resetMesString, TrayData trayData)
        {
            resetMesString = "";
            try
            {
                mespubl.AMP_GETPRODUCTINFO(trayData.TrayIDQR, RecipeCompiler.Instance.MesDa.ResCode, out int outruResult, out resetMesString);
                if (outruResult == 1)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                ErosProjcetDLL.Project.AlarmText.AddTextNewLine("读Mes出错:" + ex.Message, Color.Red);
            }

            return false;
        }

        public override bool ReadMes(string sn, out string resetMesString)
        {
            resetMesString = "未实现";
            return false;
        }

      

        public override void WrietMes(OneDataVale data, string Product_Name)
        {
        }

        public override void WrietMesAll<T>(T data,  string Product_Name)
        {
            try
            {
                int restOK = Convert.ToInt32(data);
                //AmphenolMES.mespublic.a
                mespubl.AMP_SaveProcedure(ProcessControl.ProcessUser.QRCode, restOK, RecipeCompiler.Instance.MesDa.ResCode, RecipeCompiler.Instance.MesDa.UserCode, RecipeCompiler.Instance.MesDa.SWVesion, RecipeCompiler.Instance.MesDa.ecgcode, RecipeCompiler.Instance.MesDa.Ecode, out int outruResult, out string outerrorinfo);
                if (outruResult == 1)
                {
                    ErosProjcetDLL.Project.AlarmText.AddTextNewLine("结果ok:" + outerrorinfo, Color.Green);
                }
                else
                {
                    ErosProjcetDLL.Project.AlarmText.AddTextNewLine("结果NG:" + outerrorinfo, Color.Red);
                }
            }
            catch (Exception ex)
            {
                ErosProjcetDLL.Project.AlarmText.AddTextNewLine("安费诺写Mes出错:" + ex.Message, Color.Red);
            }
        }

        public override void WrietMes(TrayData trayData, string Product_Name)
        {
        }

        public override Form GetForm()
        {
            return null;
        }
    }
}