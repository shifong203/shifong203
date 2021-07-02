using System;
using System.Drawing;
using System.Windows.Forms;
using Vision2.Project.formula;
using Vision2.Project.ProcessControl;

namespace Vision2.捷普
{
    public partial class MForm : Form
    {
        public MForm()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text.Length>=numericUpDown1.Value)
                {
                    if (ErosProjcetDLL.Project.ProjectINI.Enbt)
                    {
                        DebugSele debugSele = new DebugSele();
                        debugSele.ShowDialog();
                    }
                    if (!ErosProjcetDLL.Project.ProjectINI.DebugMode)
                    {
                        if (RecipeCompiler.Instance.GetMes().ReadMes(textBox1.Text, out string strErr))
                        {
                            label6.BackColor = Color.ForestGreen;
                            OperatorFormShow operatorFormShow = new OperatorFormShow();
                            operatorFormShow.ShowDialog();
                        }
                        else
                        {
                            label6.BackColor = Color.Red;
                        }
                        label6.Text = strErr;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (label6.Text=="Pass" || ErosProjcetDLL.Project.ProjectINI.DebugMode)
                {
                    //RecipeCompiler.Instance.MesDatat.UserID = textBox3.Text;
                    textBox2.Text = RecipeCompiler.Instance.MesDatat.Testre_Name;
                    UserFormulaContrsl.StaticAddQRCode(textBox1.Text);
                    Project.DebugF.DebugCompiler.Start();
                }
                tabControl1.SelectedIndex = 1;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text  +=Application.StartupPath;
                comboBox1.Items.Clear();
                textBox2.Text = RecipeCompiler.Instance.MesDatat.Testre_Name;
                vision.Vision.GetRunNameVision().EventDoen += MForm_EventDoen;
                 comboBox1.Items.Add(@"D:\");
                comboBox1.SelectedIndex = 0;
                tabControl1.SelectedIndex = 1;
                timer1.Interval = 100;
                timer1.Start();
            }
            catch (Exception ex)
            {
            }
        }

   

        private void RunCodeT_RunDone(Project.DebugF.IO.RunCodeStr.RunErr key)
        {
            try
            {
               
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells[8].Value = key.RunTime;
                }
                //data.EndTime.ToString()
                //(MM-dd-yyyy HH - mm - ss)
                string Name = textBox1.Text + UserFormulaContrsl.GetDataVale().EndTime.ToString(RecipeCompiler.Instance.MesDatat.FileTimeName);
                if (ErosProjcetDLL.Project.ProjectINI.DebugMode)
                {
                    Name = "DEBUG-" + Name;
                }
                string path = ProcessUser.GetThis().ExcelPath + "\\" + Name;
                if (UserFormulaContrsl.GetDataVale().OK)
                {
                    label7.BackColor = Color.Green;
                    label7.Text = "Pass";
                }
                else
                {
                    label7.BackColor = Color.Red;
                    label7.Text = "Fill";
                }
                HtmlMaker.Html.GenerateCode(ProcessUser.GetThis().ExcelPath 
                    +"\\历史数据\\"+ DateTime.Now .ToString("yyyyMMdd") +"\\"+ Name
                    , key.RunTime, UserFormulaContrsl.timeStrStrat, DateTime.Now,UserFormulaContrsl.GetDataVale());
                HtmlMaker.Html.GenerateCode(path, key.RunTime, UserFormulaContrsl.timeStrStrat, DateTime.Now,
               UserFormulaContrsl.GetDataVale());
            }
            catch (Exception ex)
            {
            }
        }

        private void MForm_EventDoen(vision.OneResultOBj oneResultO)
        {
            try
            {
                dataGridView1.Rows.Clear();
                foreach (var item in oneResultO.GetNgOBJS().DicOnes)
                {
                    foreach (var itemdt in item.Value.oneRObjs)
                    {
                        int index = dataGridView1.Rows.Add();
                        dataGridView1.Rows[index].DefaultCellStyle.BackColor = Color.White;
                        if (itemdt.dataMinMax.GetRsetOK())
                        {
                            dataGridView1.Rows[index].Cells[2].Value = "Pass";
                        }
                        else
                        {
                            dataGridView1.Rows[index].Cells[2].Value = "Fill";
                            dataGridView1.Rows[index].DefaultCellStyle.BackColor = Color.Red;
                        }
                        dataGridView1.Rows[index].Cells[1].Value = item.Value.ComponentID;
                        dataGridView1.Rows[index].Cells[1].Value = itemdt.ComponentID;
                        dataGridView1.Rows[index].Cells[5].Value =vision.Vision.Instance.TransformName;
                        dataGridView1.Rows[index].Cells[3].Value = itemdt.dataMinMax.Reference_Name[0];
                        dataGridView1.Rows[index].Cells[4].Value = itemdt.dataMinMax.doubleV[0].Value.ToString("0.000000");
                        dataGridView1.Rows[index].Cells[6].Value = itemdt.dataMinMax.Reference_ValueMin[0];
                        dataGridView1.Rows[index].Cells[7].Value = itemdt.dataMinMax.Reference_ValueMax[0];
                    }
                }
             
            }
            catch (Exception)
            {
            }
        }

        private void MForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //if (RecipeCompiler.Instance.GetMes() != null)
            //{
            //    RecipeCompiler.Instance.GetMes().ResDoneEvent += MForm_ResDoneEvent;
            //}
            timer1.Stop();
            Project.DebugF.DebugCompiler.GetThis().DDAxis.RunCodeT.RunDone += RunCodeT_RunDone;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            htmlMaker.Form1 form1 = new htmlMaker.Form1();
            form1.ShowDialog();

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
            
                        if (RecipeCompiler.Instance.GetMes().ReadMes(textBox1.Text, out string strErr))
                        {
                            label6.BackColor = Color.ForestGreen;
                            OperatorFormShow operatorFormShow = new OperatorFormShow();
                            operatorFormShow.ShowDialog();
                        }
                        else
                        {
                            label6.BackColor = Color.Red;

                        }
                        label6.Text = strErr;
                }
            }
            catch (Exception)
            {
            }
   
        }


        private void tsButton1_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tsButton2_Click(object sender, EventArgs e)
        {

            try
            {
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
           
        }

  


    

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void toolStripDropDownButton3_Click_1(object sender, EventArgs e)
        {
            try
            {
                ErosProjcetDLL.Project.LandingForm landingForm = new ErosProjcetDLL.Project.LandingForm();
                landingForm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
