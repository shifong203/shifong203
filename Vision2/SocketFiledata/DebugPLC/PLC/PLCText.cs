using ErosSocket.ErosConLink;
using System;
using System.ComponentModel;

namespace ErosSocket.DebugPLC.PLC
{
    public class PLCText : System.Windows.Forms.TextBox
    {
        [DescriptionAttribute("。"), Category("PLC地址"), DisplayName("读写名称")]
        public string LinkNameStr { get; set; } = ".";

        private bool ISCT;

        private dynamic TextPlc_ValueCyEvent(UClass.ErosValues.ErosValueD key)
        {
            this.Invoke(new Action(() =>
            {
                ISCT = true;
                this.Text = key.Value.ToString();
                ISCT = false;
            }));

            return key.Value;
        }

        protected override void CreateHandle()
        {
            base.CreateHandle();
            this.Invoke(new Action(() =>
            {
                try
                {
                    String[] TAS = LinkNameStr.Split('.');
                    if (StaticCon.GetSocketClint(TAS[0]) != null)
                    {
                        if (StaticCon.GetSocketClint(TAS[0]).KeysValues.DictionaryValueD.ContainsKey(TAS[1])
                         && StaticCon.GetSocketClint(TAS[0]).KeysValues.DictionaryValueD[TAS[1]]._Type != UClass.Boolean)
                        {
                            StaticCon.GetSocketClint(TAS[0]).KeysValues.DictionaryValueD[TAS[1]].ValueCyEvent += TextPlc_ValueCyEvent;
                            this.Text = StaticCon.GetSocketClint(TAS[0]).KeysValues.DictionaryValueD[TAS[1]].Value.ToString();
                        }
                    }
                }
                catch (Exception)
                {
                }
            }));
        }

        protected override void OnTextChanged(EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                String[] TAS = LinkNameStr.Split('.');
                if (StaticCon.GetSocketClint(TAS[0]) != null)
                {
                    string valueName = LinkNameStr.Remove(0, TAS[0].Length + 1);
                    if (StaticCon.GetSocketClint(TAS[0]).KeysValues.DictionaryValueD.ContainsKey(valueName))
                    {
                        if (UClass.GetTypeValue(StaticCon.GetSocketClint(TAS[0]).KeysValues.DictionaryValueD[valueName]._Type, this.Text, out dynamic vae))
                        {
                            //this.Text = StaticCon.GetSocketClint(TAS[0]).KeysValues.DictionaryValueD[TAS[1]].Value.ToString();
                        }
                        else
                        {
                            return;
                        }
                    }
                    base.OnTextChanged(e);
                    if (!ISCT)
                    {
                        StaticCon.SetLingkValue(LinkNameStr, this.Text, out string err);
                    }
                    else
                    {
                    }
                    ISCT = false;
                }
            }));
        }

        protected override void Dispose(bool disposing)
        {
            String[] TAS = LinkNameStr.Split('.');
            if (StaticCon.GetSocketClint(TAS[0]) != null)
            {
                if (StaticCon.GetSocketClint(TAS[0]).KeysValues.DictionaryValueD.ContainsKey(TAS[1])
                    && StaticCon.GetSocketClint(TAS[0]).KeysValues.DictionaryValueD[TAS[1]]._Type != UClass.Boolean)
                {
                    StaticCon.GetSocketClint(TAS[0]).KeysValues.DictionaryValueD[TAS[1]].ValueCyEvent -= TextPlc_ValueCyEvent;
                }
            }

            base.Dispose(disposing);
        }
    }
}