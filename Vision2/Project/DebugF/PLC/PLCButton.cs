using ErosSocket.ErosConLink;
using System;
using System.ComponentModel;
using System.Drawing;

namespace Vision2.Project.DebugF.PLC
{
    public class PLCButton : System.Windows.Forms.Button
    {
        [DescriptionAttribute("。"), Category("PLC地址"), DisplayName("读写名称")]
        public string LinkNameStr { get; set; } = "";

        private bool ISCT;

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

        protected override void CreateHandle()
        {
            base.CreateHandle();
            this.Create();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            if (!ISCT)
            {
                StaticCon.SetLingkValue(LinkNameStr, true, out string err);
            }
            else
            {
                StaticCon.SetLingkValue(LinkNameStr, false, out string err);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            try
            {
                String[] TAS = LinkNameStr.Split('.');
                if (StaticCon.GetSocketClint(TAS[0]).KeysValues.DictionaryValueD.ContainsKey(TAS[1])
                 && StaticCon.GetSocketClint(TAS[0]).KeysValues.DictionaryValueD[TAS[1]]._Type == UClass.Boolean)
                {
                    StaticCon.GetSocketClint(TAS[0]).KeysValues.DictionaryValueD[TAS[1]].ValueCyEvent -= TextPlc_ValueCyEvent;
                }
            }
            catch (Exception)
            {
            }
        }

        private void Create()
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
            catch (Exception)
            {
            }
        }
    }
}