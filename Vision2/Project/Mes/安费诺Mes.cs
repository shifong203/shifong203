using AmphenolMES;
using System;
using System.ComponentModel;
using System.Drawing;
using Vision2.Project.formula;
using Vision2.Project.formula;
using ErosSocket.DebugPLC.Robot;

namespace Vision2.Project.Mes
{
    public class 安费诺Mes : IMesData
    {
        AmphenolMES.mespublic mespubl = new mespublic();

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


        public bool ReadMes(string SerialNumber, out string resetMesString)
        {
            resetMesString = "";
            try
            {
                mespubl.AMP_GETPRODUCTINFO(SerialNumber, RecipeCompiler.Instance.MesDa.ResCode, out int outruResult, out resetMesString);
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

        public bool ReadMes(out string resetMesString)
        {
            resetMesString = "未实现";
            return false;
        }

        public void WrietMes(UserFormulaContrsl userFormulaContrsl, string QRCODE, string Product_Name)
        {

        }

        public void WrietMes(DataVale data, string Product_Name)
        {

        }

        public void WrietMesAll<T>(T data, string QRCODE, string Product_Name)
        {
            try
            {
                int restOK = Convert.ToInt32(data);
                //AmphenolMES.mespublic.a
                mespubl.AMP_SaveProcedure(QRCODE, restOK, RecipeCompiler.Instance.MesDa.ResCode, RecipeCompiler.Instance.MesDa.UserCode, RecipeCompiler.Instance.MesDa.SWVesion, RecipeCompiler.Instance.MesDa.ecgcode, RecipeCompiler.Instance.MesDa.Ecode, out int outruResult, out string outerrorinfo);
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

        public void WrietMes(TrayData trayData, string Product_Name)
        {
     
        }

  
    }
}
