using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NokidaE.Project.ProcessControl
{
    public partial class ProcessUserControl : UserControl
    {
        public ProcessUserControl()
        {
            InitializeComponent();
        }
        public string code;

        public void Up(){

            try
            {
                if (ErosSocket.ErosConLink.StaticCon.GetLingkNameValue(ProcessUser.GetThis().EapEnName))
                {
                    label8.BackColor = Color.GreenYellow;
                }
                else
                {
                    label8.BackColor = Color.Red;
                }
                label21.Text  = ErosSocket.ErosConLink.StaticCon.GetLingkNameValueString(ProcessUser.GetThis().EapGetQRCodeName);
                label17.Text = "当前生产:" + Project.formula.Product.ProductionName;
                string itmes = ErosSocket.ErosConLink.
                              StaticCon.GetLingkNameValueString("打码清洗.二维码0");
                if (itmes != code)
                {
                    if (code != "")
                    {

                        //dataGridView2.Rows[0].Cells[1].Value = dataGridView2.Rows[1].Cells[1].Value;
                        //dataGridView2.Rows[1].Cells[1].Value = dataGridView2.Rows[2].Cells[1].Value;
                    }
                    code = itmes;
                }
                label14.Text = code;
                label14.Text += ErosSocket.ErosConLink.
                StaticCon.GetLingkNameValueString("打码清洗.二维码堆栈")+Environment.NewLine;
                label14.Text += ErosSocket.ErosConLink.
                StaticCon.GetLingkNameValueString("打码清洗.二维码堆栈1")+Environment.NewLine;
                label14.Text += ErosSocket.ErosConLink.
                StaticCon.GetLingkNameValueString("打码清洗.二维码2") + Environment.NewLine;
                label14.Text += ErosSocket.ErosConLink.
                StaticCon.GetLingkNameValueString("打码清洗.二维码3") + Environment.NewLine;
                label14.Text += ErosSocket.ErosConLink.
                StaticCon.GetLingkNameValueString("打码清洗.二维码4") + Environment.NewLine;

            }
            catch (Exception)
            {
            }
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}
