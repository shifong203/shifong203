using ErosSocket.ErosConLink;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace ErosSocket.ErosUI
{
    /// <summary>
    ///
    /// </summary>
    public class PLCBtn : Button
    {
        /// <summary>
        /// 按键触发模式
        /// </summary>
        public enum BtnModel
        {
            /// <summary>
            /// 按下交替
            /// </summary>
            交替 = 0,

            /// <summary>
            /// 按下写true
            /// </summary>
            按下写入1 = 1,

            /// <summary>
            /// 按下写fales
            /// </summary>
            按下写入0 = 2,

            /// <summary>
            /// 按下为ture,抬起为fales
            /// </summary>
            按下写1抬起0 = 3,

            /// <summary>
            /// 按下为fales,抬起为true
            /// </summary>
            按下写0抬起写1 = 4,
        }

        public PLCBtn()
        {
            Name = "PLCButton";
            this.FlatStyle = FlatStyle.Popup;
            BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        }

        private System.Drawing.Color Icolor;
        private bool ISCT;

        [DescriptionAttribute("按键触发的方式,交替、按下true、按下Fales、按下ture抬起Fales、按下fales抬起True。"),
           Category("控制"), DisplayName("按键模式")]
        public BtnModel Btn_Model { get; set; }

        [DescriptionAttribute("。"),
           Category("控制"), DisplayName("读取地址")]
        public string GetName { get; set; }

        [DescriptionAttribute("。"),
         Category("控制"), DisplayName("写入地址")]
        public string SetName { get; set; }

        [DescriptionAttribute("。"),
        Category("控制"), DisplayName("true颜色")]
        public System.Drawing.Color TrueColor { get; set; } = System.Drawing.Color.Lime;

        [DescriptionAttribute("。"),
        Category("控制"), DisplayName("False颜色")]
        public System.Drawing.Color FalseColor { get; set; } = System.Drawing.Color.DimGray;

        public override string Text { get => base.Text; set => base.Text = value; }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            try
            {
                //if (KeyValuePairs == null)
                //{
                //    KeyValuePairs = new LinkPLC();
                //}
                BtnModeMet(Btn_Model);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 单击
        /// </summary>
        /// <param name="btnModel"></param>
        private void BtnModeMet(BtnModel btnModel)
        {
            switch (btnModel)
            {
                case BtnModel.交替:
                    LinkPLC.SBoolNegate(SetName);
                    break;

                case BtnModel.按下写入1:
                    LinkPLC.SBoolSetValue(SetName, true);
                    break;

                case BtnModel.按下写入0:
                    LinkPLC.SBoolSetValue(SetName, false);
                    break;

                case BtnModel.按下写1抬起0:
                    LinkPLC.SBoolSetValue(SetName, true);
                    break;

                case BtnModel.按下写0抬起写1:
                    LinkPLC.SBoolSetValue(SetName, false);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 抬起
        /// </summary>
        /// <param name="btnModel"></param>
        private void BtnModeClickUPMet(BtnModel btnModel)
        {
            switch (btnModel)
            {
                case BtnModel.交替:

                    break;

                case BtnModel.按下写入0:
                    break;

                case BtnModel.按下写入1:

                    break;

                case BtnModel.按下写1抬起0:
                    LinkPLC.SBoolSetValue(SetName, false);
                    break;

                case BtnModel.按下写0抬起写1:
                    LinkPLC.SBoolSetValue(SetName, true);
                    break;

                default:
                    break;
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            if (mevent.Button == MouseButtons.Left)
            {
                this.BackColor = Icolor;
                //System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
                BtnModeClickUPMet(Btn_Model);
            }
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            if (mevent.Button == MouseButtons.Left)
            {
                Icolor = this.BackColor;
                this.BackColor = System.Drawing.Color.Blue;
            }
        }

        protected override void CreateHandle()
        {
            base.CreateHandle();
            this.UpRun();
        }

        /// <summary>
        /// 初始化关联显示变量
        /// </summary>
        private void UpRun()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(UpRun));
                return;
            }
            try
            {
                //this.Font = new System.Drawing.Font("宋体", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                //this.FlatStyle = FlatStyle.Popup;
                //this.FalseColor = System.Drawing.Color.Black;
                //this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

                if (GetName != null)
                {
                    String[] TAS = GetName.Split('.');
                    if (StaticCon.GetSocketClint(TAS[0]).KeysValues.DictionaryValueD.ContainsKey(TAS[1])
                       && StaticCon.GetSocketClint(TAS[0]).KeysValues.DictionaryValueD[TAS[1]]._Type == UClass.Boolean)
                    {
                        StaticCon.GetSocketClint(TAS[0]).KeysValues.DictionaryValueD[TAS[1]].ValueCyEvent += ErosBtn_ValueCyEvent;
                        ErosBtn_ValueCyEvent(StaticCon.GetSocketClint(TAS[0]).KeysValues.DictionaryValueD[TAS[1]]);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 值改变
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private dynamic ErosBtn_ValueCyEvent(UClass.ErosValues.ErosValueD key)
        {
            ISCT = key.Value;
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    ErosBtn_ValueCyEvent(key);
                }));
                return key.Value;
            }
            try
            {
                if (key._Type == "Boolean")
                {
                    if (key.Value)
                    {
                        this.BackColor = TrueColor;
                    }
                    else
                    {
                        this.BackColor = FalseColor;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        public void Updata(dynamic value)
        {
            try
            {
                if (GetName != null && GetName != "" && GetName.Contains("."))
                {
                    if (StaticCon.SocketClint.ContainsKey(GetName.Split('.')[0]))
                    {
                        string sd = StaticCon.SocketClint[GetName.Split('.')[0]].KeysValues[GetName.Split('.')[1]].Value.ToStrng();
                        if (sd == true.ToString())
                        {
                            this.ForeColor = TrueColor;
                        }
                        else
                        {
                            this.ForeColor = FalseColor;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            try
            {
                if (GetName == null)
                {
                    return;
                }
                String[] TAS = GetName.Split('.');
                if (StaticCon.GetSocketClint(TAS[0]).KeysValues.DictionaryValueD.ContainsKey(TAS[1])
                 && StaticCon.GetSocketClint(TAS[0]).KeysValues.DictionaryValueD[TAS[1]]._Type == UClass.Boolean)
                {
                    StaticCon.GetSocketClint(TAS[0]).KeysValues.DictionaryValueD[TAS[1]].ValueCyEvent -= ErosBtn_ValueCyEvent;
                }
            }
            catch (Exception)
            {
            }
        }
    }

    public class FloatNumTextBox : TextBox
    {
        protected override void WndProc(ref Message m)
        {
            int WM_CHAR = 0x0102;
            if (m.Msg == WM_CHAR)
            {
                if (((char)m.WParam >= '0') && ((char)m.WParam <= '9') || ((char)m.WParam == '-') ||
                (int)m.WParam == (int)Keys.Back || (int)m.WParam == (int)Keys.Delete)
                {
                    base.WndProc(ref m);
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }
    }
}