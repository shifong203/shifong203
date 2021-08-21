using ErosSocket.ErosConLink;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace ErosSocket.ErosUI
{
    public partial class UserInterfaceControl : UserControl
    {
        public UserInterfaceControl()
        {
            InitializeComponent();
            Name = "UserInterface";
        }

        private UserInterfaceData data;

        public enum EnumEquipmentStatus
        {
            未初始化 = 0,
            已停止 = 1,
            运行中 = 2,
            暂停中 = 3,
            错误暂停中 = 4,
            错误停止中 = 5,
            未上电 = 6,
        }

        /// <summary>
        /// 控制数据
        /// </summary>
        public class UserInterfaceData
        {
            [DescriptionAttribute("。"), Category("显示"), DisplayName("true颜色")]
            public Color True_Color { get; set; }

            [DescriptionAttribute("。"), Category("显示"), DisplayName("Fales颜色")]
            public Color FalesColor { get; set; }

            [DescriptionAttribute("。"), Category("控制"), DisplayName("运行变量名")]
            public LinkPLC LinkStart { get; set; } = new LinkPLC();

            [DescriptionAttribute("。"), Category("控制"), DisplayName("暂停变量名")]
            public LinkPLC LinkPause { get; set; } = new LinkPLC();

            [DescriptionAttribute("。"), Category("控制"), DisplayName("停止变量名")]
            public LinkPLC LinkStop { get; set; } = new LinkPLC();

            [DescriptionAttribute("。"), Category("控制"), DisplayName("初始化变量名")]
            public LinkPLC LinkInitialize { get; set; } = new LinkPLC();

            [DescriptionAttribute("。"), Category("控制"), DisplayName("复位变量名")]
            public LinkPLC LinkRestoration { get; set; } = new LinkPLC();

            [DescriptionAttribute("。"), Category("控制"), DisplayName("读取设备状态地址名")]
            [Editor(typeof(ErosSocket.ErosConLink.LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
            public string GetStatLinkName { get; set; }

            [Editor(typeof(ErosSocket.ErosConLink.LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
            [DescriptionAttribute("。"), Category("控制"), DisplayName("错误变量名")]
            public string LinkAlarmName { get; set; }

            [Editor(typeof(ErosSocket.ErosConLink.LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
            [DescriptionAttribute("。"), Category("控制"), DisplayName("继续变量名")]
            public string LinkConnectName { get; set; }
        }

        public void SetData(UserInterfaceData userInterface)
        {
            data = userInterface;
            //Btn_Start.KeyValuePairs = data.LinkStart;
            //Btn_Stop.KeyValuePairs = this.data.LinkStop;
            //Btn_Pause.KeyValuePairs = this.data.LinkPause;
            //Btn_Reset.KeyValuePairs = this.data.LinkRestoration;
            //Btn_Initialize.KeyValuePairs = this.data.LinkInitialize;
            //    Btn_Stop.KeyValuePairs.StatusColor[0] =
            //    Btn_Reset.KeyValuePairs.StatusColor[0] =
            //    Btn_Start.KeyValuePairs.StatusColor[0] =
            //    Btn_Initialize.KeyValuePairs.StatusColor[0] =
            //    Btn_Pause.KeyValuePairs.StatusColor[0] =
            //    this.data.FalesColor;

            //    Btn_Reset.KeyValuePairs.StatusColor[1] =
            //    Btn_Stop.KeyValuePairs.StatusColor[1] =
            //    Btn_Start.KeyValuePairs.StatusColor[1] =
            //    Btn_Pause.KeyValuePairs.StatusColor[1] =
            //    Btn_Initialize.KeyValuePairs.StatusColor[1] =
            //    this.data.True_Color;
        }

        private void Btn_Massge_Click(object sender, System.EventArgs e)
        {
        }

        private void Btn_Debug_Click(object sender, System.EventArgs e)
        {
        }

        private void Btn_Reset_Click(object sender, System.EventArgs e)
        {
            try
            {
                Vision2.ErosProjcetDLL.Project.AlarmText.ResetAlarm();
                //Btn_Pause.KeyValuePairs.BoolSetValue(data.LinkRestoration.SetName, true);
                //Btn_Pause.KeyValuePairs.BoolSetValue(data.LinkRestoration.SetName, false);
                //this.Btn_Reset.KeyValuePairs = this.data.LinkRestoration;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Btn_Pause_Click(object sender, System.EventArgs e)
        {
            try
            {
                //if (DebugCompiler.EquipmentStatus == DebugCompiler.EnumEquipmentStatus.运行中)
                //{
                if (StaticCon.SetLingkValue(data.LinkPause.SetName, true.ToString(), out string err))
                {
                    //Btn_Pause.Text = "继续";
                    //DebugCompiler.EquipmentStatus = DebugCompiler.EnumEquipmentStatus.暂停中;
                }
                StaticCon.SetLingkValue(data.LinkPause.SetName, false.ToString(), out err);
                //}
                //else if(DebugCompiler.EquipmentStatus == DebugCompiler.EnumEquipmentStatus.暂停中)
                //{
                //    if (ErosSocket.ErosConLink.StaticCon.SetLingkValue(data.LinkConnectName, true.ToString(), out string err))
                //    {
                //        Btn_Pause.Text = "暂停";
                //        //DebugCompiler.EquipmentStatus = DebugCompiler.EnumEquipmentStatus.运行中;
                //    }
                //}
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Btn_Start_Click(object sender, System.EventArgs e)
        {
            try
            {
                //Btn_Start.KeyValuePairs = data.LinkStart;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Btn_Stop_Click(object sender, System.EventArgs e)
        {
            try
            {
                //Btn_Stop.KeyValuePairs = this.data.LinkStop;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Btn_Initialize_Click(object sender, System.EventArgs e)
        {
            try
            {
                //Btn_Initialize.KeyValuePairs = this.data.LinkInitialize;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}