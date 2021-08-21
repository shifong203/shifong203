using System;
using System.Linq;
using Vision2.ErosProjcetDLL.Project;

namespace ErosSocket.DebugPLC
{
    public partial class EnumTextUserControl1 : System.Windows.Forms.UserControl
    {
        public EnumTextUserControl1()
        {
            InitializeComponent();
        }

        public EnumTextUserControl1(PLCIntEnum pLCInt) : this()
        {
            Enum = pLCInt;
            progressBar1.Minimum = pLCInt.kaayValue.Keys.ToArray().Min();
            progressBar1.Maximum = pLCInt.kaayValue.Keys.ToArray().Max();
        }

        private PLCIntEnum Enum;

        public void UpEnumText(int intEnu)
        {
            try
            {
                this.Invoke(new Action(() =>
                {
                    if (progressBar1.Maximum > intEnu)
                    {
                        progressBar1.Value = intEnu;
                    }
                    string text = EnumText + ":" + intEnu.ToString();

                    if (Enum != null)
                    {
                        text += ":" + Enum.kaayValue[intEnu];
                    }
                    if (text != label1.Text)
                    {
                        label1.Text = text;
                    }
                }));
            }
            catch (Exception)
            {
            }
        }

        public void UpEnumText(int intEnu, PLCIntEnum pLCInt)
        {
            Enum = pLCInt;
            progressBar1.Minimum = pLCInt.kaayValue.Keys.ToArray().Min();
            progressBar1.Maximum = pLCInt.kaayValue.Keys.ToArray().Max();
            UpEnumText(intEnu);
        }

        private string EnumText;

        public void UpEnumText(int intEnu, string intEnumName)
        {
            if (Vision2.ErosProjcetDLL.PLCUI.HMIDIC.This.DicPLCEnum.ContainsKey(intEnumName))
            {
                Enum = Vision2.ErosProjcetDLL.PLCUI.HMIDIC.This.DicPLCEnum[intEnumName];
                progressBar1.Minimum = Enum.kaayValue.Keys.ToArray().Min();
                progressBar1.Maximum = Enum.kaayValue.Keys.ToArray().Max();
            }
            if (intEnumName != EnumText)
            {
                EnumText = intEnumName;
            }
            UpEnumText(intEnu, Enum);
        }
    }
}