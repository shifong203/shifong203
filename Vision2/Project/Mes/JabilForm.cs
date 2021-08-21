using System;
using System.Collections.Generic;
using System.IO;
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
            propertyGrid1.SelectedObject = mesJi.MesData;
        }

        private delegate void dgNotifyShowRecieveMsg(params string[] message);

        private MesJib mesJib;

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                ShowRecieveMsg("客户:" + mesJib.MesData.Customer,
                    "版本:" + mesJib.MesData.DiviSion,
                "SN:" + textBox1.Text, "产品名:" + Product.ProductionName,
                "电脑名:" + mesJib.MesData.Testre_Name,
                "设备名:" + mesJib.MesData.Test_Process);
                mesJib.ReadMes(textBox1.Text, out string resetMesString);
                ShowRecieveMsg("OKToTest返回：" + resetMesString);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                string resetMesString = mesJib.GetMES_TIS().GetCurrentRouteStep(textBox1.Text);
                ShowRecieveMsg("GetCurrentRouteStep返回：" + resetMesString);
            }
            catch (Exception)
            {
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> trayID = new List<int>();
                List<string> Sns = new List<string>();
                List<string> Rseult = new List<string>();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells[0].Value != null)
                    {
                        trayID.Add(int.Parse(dataGridView1.Rows[i].Cells[0].Value.ToString()));
                        Sns.Add(dataGridView1.Rows[i].Cells[1].Value.ToString());
                        Rseult.Add(dataGridView1.Rows[i].Cells[2].Value.ToString());
                    }
                }

                ShowRecieveMsg(mesJib.ReadFvt(trayID.ToArray(), Sns.ToArray(), Rseult.ToArray()));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                string resetMesString = "";
                resetMesString = mesJib.GetMES_TIS().OKToTest(mesJib.MesData.Customer,
           mesJib.MesData.DiviSion,
           textBox1.Text, Product.ProductionName,
           mesJib.MesData.Testre_Name,
          mesJib.MesData.Test_Process);
                ShowRecieveMsg("OKToTest返回：" + resetMesString);
                resetMesString = mesJib.GetMES_TIS().GetCurrentRouteStep(textBox1.Text);
                ShowRecieveMsg("GetCurrentRouteStep返回：" + resetMesString);
                resetMesString = mesJib.GetMES_TIS().OKToTestLinkMaterial(
                mesJib.MesData.Customer, mesJib.MesData.DiviSion,
                textBox1.Text, Product.ProductionName,
                mesJib.MesData.Testre_Name, mesJib.MesData.Test_Process);
                ShowRecieveMsg("OKToTestLinkMaterial返回：" + resetMesString);
                resetMesString = mesJib.GetMES_TIS().GetTestDataFormats();
                ShowRecieveMsg("GetTestDataFormats返回：" + resetMesString);
                resetMesString = mesJib.GetMES_TIS().GetLastTestResult(textBox1.Text, mesJib.MesData.Customer, mesJib.MesData.DiviSion, mesJib.MesData.Test_Process);
                ShowRecieveMsg("GetLastTestResult：" + resetMesString);
                resetMesString = mesJib.GetMES_TIS().GetPanelSerializeResult(mesJib.MesData.Customer, mesJib.MesData.DiviSion, textBox1.Text);
                ShowRecieveMsg("GetPanelSerializeResult返回：" + resetMesString);
                resetMesString = mesJib.GetMES_TIS().GetTestHistory(textBox1.Text, mesJib.MesData.Customer, mesJib.MesData.DiviSion);
                ShowRecieveMsg("GetTestHistory返回：" + resetMesString);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void JabilForm_Load(object sender, EventArgs e)
        {
            try
            {
                fileS = Directory.GetFiles(mesJib.MesData.FVTPath);
                for (int i = 0; i < fileS.Length; i++)
                {
                    string snt = Path.GetFileNameWithoutExtension(fileS[i]);
                    listBox1.Items.Add(snt);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private string[] fileS;

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string[] strDatas = File.ReadAllLines(fileS[listBox1.SelectedIndex]);
                dataGridView1.Rows.Clear();
                richTextBox2.Lines = strDatas;
                int number = 0;
                for (int id = (int)numericUpDown1.Value; id < strDatas.Length; id++)
                {
                    if (number >= 5)
                    {
                        break;
                    }
                    number++;
                    string[] dat = strDatas[id].Split(';');
                    string[] datas = dat[1].Split(':');
                    int ds = dataGridView1.Rows.Add();

                    dataGridView1.Rows[ds].Cells[1].Value = datas[1];
                    datas = dat[0].Split(':');
                    dataGridView1.Rows[ds].Cells[0].Value = datas[1];
                    datas = dat[2].Split(':');
                    dataGridView1.Rows[ds].Cells[2].Value = datas[1];
                }
            }
            catch (Exception)
            {
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(ProcessControl.ProcessUser.Instancen.ExcelPath))
                {
                    System.Diagnostics.Process.Start(ProcessControl.ProcessUser.Instancen.ExcelPath);
                }
                else
                {
                    System.Diagnostics.Process.Start(ProcessControl.ProcessUser.Instancen.ExcelPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                listBox1.Items.Clear();
                fileS = Directory.GetFiles(mesJib.MesData.FVTPath);
                for (int i = 0; i < fileS.Length; i++)
                {
                    string snt = Path.GetFileNameWithoutExtension(fileS[i]);
                    listBox1.Items.Add(snt);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}