
using ErosSocket.ErosConLink;
using ErosSocket.ErosUI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace ErosSocket.DebugPLC.PLC
{
    public class PLCButtonV2 : System.Windows.Forms.Button
    {
        [DescriptionAttribute("。"), Category("PLC地址"), DisplayName("读写名称")]
        public string LinkNameStr { get; set; } = "";


        public bool ISAdd { get; set; }

        public PLCBtn.BtnModel BtnModel { get; set; }

        bool ISCT;
        private dynamic TextPlc_ValueCyEvent(UClass.ErosValues.ErosValueD key)
        {
            this.Invoke(new Action(() =>
            {
                if (key.Value)
                {
                    this.BackColor = Color.GreenYellow;
                }
                else
                {
                    this.BackColor = SystemColors.ButtonShadow;
                }
                ISCT = key.Value;
            }));

            return key.Value;
        }

        public override Font Font { get; set; } = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

        protected override void CreateHandle()
        {
            base.CreateHandle();
            this.Create();
        }

        protected override void OnClick(EventArgs e)
        {
            try
            {
                string err;
                switch (BtnModel)
                {
                    case PLCBtn.BtnModel.交替:
                        if (!ISCT)
                        {
                            StaticCon.SetLingkValue(LinkNameStr, true, out err);
                        }
                        else
                        {
                            StaticCon.SetLingkValue(LinkNameStr, false, out err);
                        }
                        break;
                    case PLCBtn.BtnModel.按下写入1:
                        StaticCon.SetLingkValue(LinkNameStr, true, out err);
                        break;
                    case PLCBtn.BtnModel.按下写入0:
                        StaticCon.SetLingkValue(LinkNameStr, false, out err);
                        break;
                    case PLCBtn.BtnModel.按下写1抬起0:
                        break;
                    case PLCBtn.BtnModel.按下写0抬起写1:
                        break;
                    default:
                        break;
                }


            }
            catch (Exception)
            {


            }
            base.OnClick(e);

        }
        protected override void OnMouseUp(MouseEventArgs mevent)
        {

            try
            {
                string err;
                switch (BtnModel)
                {
                    case PLCBtn.BtnModel.按下写1抬起0:
                        StaticCon.SetLingkValue(LinkNameStr, false, out err);
                        break;
                    case PLCBtn.BtnModel.按下写0抬起写1:
                        StaticCon.SetLingkValue(LinkNameStr, true, out err);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {


            }
            base.OnMouseUp(mevent);
        }
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            try
            {

                string err;
                switch (BtnModel)
                {
                    case PLCBtn.BtnModel.按下写1抬起0:
                        StaticCon.SetLingkValue(LinkNameStr, true, out err);
                        break;
                    case PLCBtn.BtnModel.按下写0抬起写1:
                        StaticCon.SetLingkValue(LinkNameStr, false, out err);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {


            }
            base.OnMouseDown(mevent);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            try
            {
                String[] TAS = LinkNameStr.Split('.');
                if (StaticCon.GetSocketClint(TAS[0]) != null)
                {
                    if (StaticCon.GetSocketClint(TAS[0]).KeysValues.DictionaryValueD.ContainsKey(TAS[1])
                  && StaticCon.GetSocketClint(TAS[0]).KeysValues.DictionaryValueD[TAS[1]]._Type == UClass.Boolean)
                    {
                        StaticCon.GetSocketClint(TAS[0]).KeysValues.DictionaryValueD[TAS[1]].ValueCyEvent -= TextPlc_ValueCyEvent;

                    }
                }

            }
            catch (Exception)
            {
            }
        }

        void Create()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(Create));
                return;
            }
            try
            {
                this.Font = new System.Drawing.Font("宋体", 12.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                String[] TAS = LinkNameStr.Split('.');
                if (TAS[0] == "")
                {
                    return;
                }
                if (StaticCon.GetSocketClint(TAS[0]) != null)
                {
                    if (StaticCon.GetSocketClint(TAS[0]).KeysValues.DictionaryValueD.ContainsKey(TAS[1])
                       && StaticCon.GetSocketClint(TAS[0]).KeysValues.DictionaryValueD[TAS[1]]._Type == UClass.Boolean)
                    {
                        StaticCon.GetSocketClint(TAS[0]).KeysValues.DictionaryValueD[TAS[1]].ValueCyEvent += TextPlc_ValueCyEvent;
                        ISCT = StaticCon.GetSocketClint(TAS[0]).KeysValues.DictionaryValueD[TAS[1]].Value;
                        if (StaticCon.GetSocketClint(TAS[0]).KeysValues.DictionaryValueD[TAS[1]].Value)
                        {
                            this.BackColor = Color.GreenYellow;
                        }
                        else
                        {
                            this.BackColor = SystemColors.ButtonShadow;
                        }
                    }

                }

            }
            catch (Exception)
            {
            }
        }
    }
}
