using System;
using System.Drawing;
using System.Windows.Forms;
using Vision2.Project.Mes;

namespace Vision2.Project.formula
{
    public partial class JabilForm : Form
    {
        public JabilForm(MesJib mesJi)
        {
            InitializeComponent();
            mesJib = mesJi;
            propertyGrid1.SelectedObject = RecipeCompiler.Instance.MesDatat;
        }
        private delegate void dgNotifyShowRecieveMsg(params string[] message);

        MesJib mesJib;
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                ShowRecieveMsg("客户:" + RecipeCompiler.Instance.MesDatat.Customer, "版本:" + RecipeCompiler.Instance.MesDatat.DiviSion,
                "SN:" + textBox1.Text, RecipeCompiler.Instance.MesDatat.AssemblyNumber, "电脑名:" + RecipeCompiler.Instance.MesDatat.Testre_Name, "设备名:" + RecipeCompiler.Instance.MesDatat.Test_Process);

                mesJib.ReadMes(textBox1.Text, out string resetMesString);

                //         resetMesString = mesJib.webservice.OKToTest(  RecipeCompiler.Instance.MesDatat.Customer, RecipeCompiler.Instance.MesDatat.DiviSion,
                //textBox1.Text, RecipeCompiler.Instance.MesDatat.AssemblyNumber,  RecipeCompiler.Instance.MesDatat.Testre_Name, RecipeCompiler.Instance.MesDatat.Test_Process);

                ShowRecieveMsg("OKToTest返回：" + resetMesString);
                if (resetMesString.Contains("FAIL"))
                {
                    Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine(resetMesString, Color.Red);
                }
                else
                {
                    Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine(resetMesString);
                }

                //resetMesString = mesJib.webservice.GetLastTestResult(textBox1.Text);
                //ShowRecieveMsg("GetCurrentRouteStep返回：" + resetMesString

                //          resetMesString = mesJib.webservice.OKToTestLinkMaterial(RecipeCompiler.Instance.MesDatat.Customer, RecipeCompiler.Instance.MesDatat.DiviSion,
                //textBox1.Text, RecipeCompiler.Instance.MesDatat.AssemblyNumber, RecipeCompiler.Instance.MesDatat.Testre_Name, RecipeCompiler.Instance.MesDatat.Test_Process);
                //          ShowRecieveMsg("OKToTestLinkMaterial返回：" + resetMesString);
                //          resetMesString = mesJib.webservice.GetTestDataFormats();
                //          ShowRecieveMsg("GetTestDataFormats返回：" + resetMesString);
                //          resetMesString = mesJib.webservice.GetLastTestResult(textBox1.Text, RecipeCompiler.Instance.MesDatat.Customer, RecipeCompiler.Instance.MesDatat.DiviSion, RecipeCompiler.Instance.MesDatat.Test_Process);
                //          ShowRecieveMsg("GetLastTestResult：" + resetMesString);
                //          resetMesString = mesJib.webservice.GetPanelSerializeResult(RecipeCompiler.Instance.MesDatat.Customer, RecipeCompiler.Instance.MesDatat.DiviSion, textBox1.Text);
                //          ShowRecieveMsg("GetPanelSerializeResult返回：" + resetMesString);

                //          resetMesString = mesJib.webservice.GetTestHistory( textBox1.Text,  RecipeCompiler.Instance.MesDatat.Customer,  RecipeCompiler.Instance.MesDatat.DiviSion);
                //          ShowRecieveMsg("GetTestHistory返回：" + resetMesString);


            }
            catch (Exception)
            {

            }

        }
        private void ShowRecieveMsg(params string[] contents)
        {
            if (richTextBox1.InvokeRequired)
            {
                this.Invoke(new dgNotifyShowRecieveMsg(ShowRecieveMsg), contents);
            }
            else
            {
                for (int i = 0; i < contents.Length; i++)
                {
                    richTextBox1.Text += contents[i] + ";";
                }
                richTextBox1.Text += Environment.NewLine;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string resetMesString = mesJib.webservice.GetCurrentRouteStep(textBox1.Text);
                ShowRecieveMsg("GetCurrentRouteStep返回：" + resetMesString);
            }
            catch (Exception)
            {


            }

        }
    }
}
