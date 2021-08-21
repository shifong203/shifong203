using ErosSocket.DebugPLC.DIDO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Vision2.vision;

namespace Vision2.Project.DebugF.IO
{
    public partial class Form1C154DIDO : Form
    {
        public Form1C154DIDO()
        {
            InitializeComponent();
        }

        private vision.HWindID HWindID2 = new vision.HWindID();

        private bool isChange = true;

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                axisControl1.UpAxisData(DebugCompiler.Instance.DDAxis.AxisS[listBox1.SelectedIndex]);
                propertyGrid1.SelectedObject = DebugCompiler.Instance.DDAxis.AxisS[listBox1.SelectedIndex];
                AxisT = DebugCompiler.Instance.DDAxis.AxisS[listBox1.SelectedIndex];
            }
            catch (Exception)
            {
            }
            isChange = false;
        }

        private Axis AxisT;

        private void Form1C154DIDO_Load(object sender, EventArgs e)
        {
            try
            {
                HWindID2.Initialize(hWindowControl1);
                HWindID2.HeigthImage = (int)numericUpDown3.Value;
                HWindID2.WidthImage = (int)numericUpDown4.Value;
                HWindID2.OneResIamge = new OneResultOBj();
                //HWindID2.SetImaage
                listBox1.ContextMenuStrip = contextMenuStrip1;
                this.didoUserControl1.setDODI(DebugCompiler.GetDoDi());
                DebugCompiler.Instance.DDAxis.UpCycle += DDAxis_UpCycle;
                listBox2.Items.Clear();
                Column1.Items.Clear();
                listBox1.Items.Clear();
                for (int i = 0; i < DebugCompiler.Instance.DDAxis.AxisS.Count; i++)
                {
                    listBox1.Items.Add(DebugCompiler.Instance.DDAxis.AxisS[i].Name);
                    Column1.Items.Add(DebugCompiler.Instance.DDAxis.AxisS[i].Name);
                }
                foreach (var item in DebugCompiler.Instance.DDAxis.AxisGrot)
                {
                    listBox2.Items.Add(item.Key);
                }
                foreach (var item in DebugCompiler.Instance.DDAxis.Cylinders)
                {
                    listBox4.Items.Add(item.Name);
                }
                listBox5.Items.Clear();

                for (int it = 0; it < DebugCompiler.Instance.DDAxis.ListTray.Count; it++)
                {
                    listBox5.Items.Add(it);
                }

                listBox6.Items.Clear();
                for (int it = 0; it < DebugCompiler.Instance.ListMatrix.Count; it++)
                {
                    listBox6.Items.Add(it);
                }
            }
            catch (Exception)
            {
            }
        }

        private void DDAxis_UpCycle(DODIAxis key)
        {
            try
            {
                if (this.IsDisposed)
                {
                    DebugCompiler.Instance.DDAxis.UpCycle -= DDAxis_UpCycle;
                }
                if (AxisT != null)
                {
                    if (AxisT.Negative_Limit)
                    {
                        label8.BackColor = Color.Green;
                    }
                    else
                    {
                        label8.BackColor = Color.Gray;
                    }
                    if (AxisT.Origin_Limit)
                    {
                        label7.BackColor = Color.Green;
                    }
                    else
                    {
                        label7.BackColor = Color.Gray;
                    }
                    if (AxisT.Positive_Limit)
                    {
                        label9.BackColor = Color.Green;
                    }
                    else
                    {
                        label9.BackColor = Color.Gray;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DebugCompiler.Instance.DDAxis.AxisS.Add(new Axis());
                listBox1.Items.Add(DebugCompiler.Instance.DDAxis.AxisS[DebugCompiler.Instance.DDAxis.AxisS.Count - 1].Name);
            }
            catch (Exception)
            {
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < DebugCompiler.Instance.DDAxis.AxisS.Count; i++)
                {
                    if (listBox1.SelectedItem.ToString() == DebugCompiler.Instance.DDAxis.AxisS[i].Name)
                    {
                        DebugCompiler.Instance.DDAxis.AxisS.RemoveAt(i);
                    }
                }

                listBox1.Items.Remove(listBox1.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tsButton1_Click(object sender, EventArgs e)
        {
            if (DebugCompiler.Instance.ListKat == "FY6400")
            {
                Run_project run_Project = DebugCompiler.Instance.DDAxis as Run_project;
                DebugCompiler.GetRunP(run_Project);
                ////socketClint.initialization();
                IO.FY6400 fY6400 = new IO.FY6400() { ID = 0, };

                fY6400.Initial();
                fY6400.Int = DebugCompiler.Instance.DDAxis.Int;
                fY6400.Out = DebugCompiler.Instance.DDAxis.Out;
                DebugCompiler.GetDoDi(fY6400 as IDIDO);
            }
            else
            {
                DebugCompiler.Instance.DDAxis.Initial();
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listBox2.SelectedItem == null)
                {
                    return;
                }
                if (DebugCompiler.Instance.DDAxis.AxisGrot.ContainsKey(listBox2.SelectedItem.ToString()))
                {
                    textBox1.Text = listBox2.SelectedItem.ToString();
                    UpDataAxisGrot(DebugCompiler.Instance.DDAxis.AxisGrot[listBox2.SelectedItem.ToString()]);
                }
            }
            catch (Exception)
            {
            }
            isChange = false;
        }

        private void UpDataAxisGrot(List<string> vs)
        {
            isChange = true;
            dataGridView1.Rows.Clear();
            for (int i = 0; i < vs.Count; i++)
            {
                int dt = dataGridView1.Rows.Add();
                dataGridView1.Rows[dt].Cells[0].Value = vs[i];

                for (int it = 0; it < DebugCompiler.Instance.DDAxis.AxisS.Count; it++)
                {
                    if (DebugCompiler.Instance.DDAxis.AxisS[it].Name == vs[i])
                    {
                        dataGridView1.Rows[dt].Cells[1].Value = DebugCompiler.Instance.DDAxis.AxisS[it].AxisType;
                        dataGridView1.Rows[dt].Cells[2].Value = DebugCompiler.Instance.DDAxis.AxisS[it].AxisNo;
                        break;
                    }
                }
            }
        }

        private void 添加轴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (DebugCompiler.Instance.DDAxis.AxisGrot.ContainsKey(listBox2.SelectedItem.ToString()))
                {
                    dataGridView1.Rows.Add();
                }
            }
            catch (Exception)
            {
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (isChange)
                {
                    return;
                }
                if (e.ColumnIndex == 0)
                {
                    DebugCompiler.Instance.DDAxis.AxisGrot[textBox1.Text].Clear();
                    for (int it = 0; it < DebugCompiler.Instance.DDAxis.AxisS.Count; it++)
                    {
                        if (DebugCompiler.Instance.DDAxis.AxisS[it].Name == dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString())
                        {
                            dataGridView1.Rows[e.RowIndex].Cells[1].Value = DebugCompiler.Instance.DDAxis.AxisS[it].AxisType;
                            dataGridView1.Rows[e.RowIndex].Cells[2].Value = DebugCompiler.Instance.DDAxis.AxisS[it].AxisNo;

                            break;
                        }
                    }
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if (DebugCompiler.Instance.DDAxis.AxisGrot.ContainsKey(textBox1.Text))
                        {
                            DebugCompiler.Instance.DDAxis.AxisGrot[textBox1.Text].Add(dataGridView1.Rows[i].Cells[0].Value.ToString());
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!DebugCompiler.Instance.DDAxis.AxisGrot.ContainsKey(textBox1.Text))
                {
                    DebugCompiler.Instance.DDAxis.AxisGrot.Add(textBox1.Text, new List<string>());
                    listBox2.Items.Add(textBox1.Text);
                }
            }
            catch (Exception)
            {
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        private void 删除ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox2.SelectedItem == null)
                {
                    return;
                }
                if (DebugCompiler.Instance.DDAxis.AxisGrot.ContainsKey(listBox2.SelectedItem.ToString()))
                {
                    DebugCompiler.Instance.DDAxis.AxisGrot.Remove(listBox2.SelectedItem.ToString());
                }
                listBox2.Items.Remove(listBox2.SelectedItem);
            }
            catch (Exception)
            {
            }
        }

        private void 删除ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (DebugCompiler.Instance.DDAxis.AxisGrot[textBox1.Text].Contains(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString()))
                {
                }
                DebugCompiler.Instance.DDAxis.AxisGrot[textBox1.Text].Remove(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString());
                dataGridView1.Rows.RemoveAt(dataGridView1.SelectedCells[0].RowIndex);
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                listBox2.Items.Clear();
                Column1.Items.Clear();
                listBox4.Items.Clear();
                listBox1.Items.Clear();
                for (int i = 0; i < DebugCompiler.Instance.DDAxis.AxisS.Count; i++)
                {
                    listBox1.Items.Add(DebugCompiler.Instance.DDAxis.AxisS[i].Name);
                    Column1.Items.Add(DebugCompiler.Instance.DDAxis.AxisS[i].Name);
                }
                foreach (var item in DebugCompiler.Instance.DDAxis.AxisGrot)
                {
                    listBox2.Items.Add(item.Key);
                }
                foreach (var item in DebugCompiler.Instance.DDAxis.Cylinders)
                {
                    if (item.Name == null)
                    {
                        item.Name = "";
                    }
                    listBox4.Items.Add(item.Name);
                }
                //numericUpDown1.Value = DebugCompiler.GetThis().DDAxis.Is_braking;
            }
            catch (Exception)
            {
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void 添加气缸ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DebugCompiler.Instance.DDAxis.Cylinders.Add(new C154Cylinder { Name = "气缸1" });
                listBox4.Items.Clear();
                foreach (var item in DebugCompiler.Instance.DDAxis.Cylinders)
                {
                    listBox4.Items.Add(item.Name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (listBox4.SelectedItem == null)
                {
                    return;
                }
                propertyGrid2.SelectedObject = DebugCompiler.Instance.DDAxis.Cylinders[listBox4.SelectedIndex];
                cylinderControl1.Up(DebugCompiler.Instance.DDAxis.Cylinders[listBox4.SelectedIndex]);
            }
            catch (Exception)
            {
            }
            isChange = false;
        }

        private void 删除ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox4.SelectedItem == null)
                {
                    return;
                }
                DebugCompiler.Instance.DDAxis.Cylinders.RemoveAt(listBox4.SelectedIndex);
                listBox4.Items.RemoveAt(listBox4.SelectedIndex);
            }
            catch (Exception)
            {
            }
        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                propertyGrid3.SelectedObject = DebugCompiler.Instance.DDAxis.ListTray[listBox5.SelectedIndex];
                trayControl1.SetTray(DebugCompiler.Instance.DDAxis.ListTray[listBox5.SelectedIndex]);
            }
            catch (Exception)
            {
            }
        }

        private void tsButton2_Click(object sender, EventArgs e)
        {
            DebugCompiler.GetRunP().IsInitialBool = false;
        }

        private void trayControl1_Load(object sender, EventArgs e)
        {
        }

        private 工艺库.MatrixC MatrixC;

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                HWindID2.HeigthImage = (int)numericUpDown3.Value;
                HWindID2.WidthImage = (int)numericUpDown4.Value;
                HWindID2.OneResIamge = new OneResultOBj();
                HWindID2.OneResIamge.ClearAllObj();
                //ErosSocket.DebugPLC.PointFile point = DebugCompiler.GetThis().DDAxis.ToPointFile(comboBox2.SelectedItem.ToString());
                //HWindID2.SetImaage(vision.Vision.GetRunNameVision("下相机").Image());

                vision.Vision.Gen_arrow_contour_xld(out HalconDotNet.HObject hObject, 0, 0, 0, 100);
                HWindID2.OneResIamge.AddObj(hObject, ColorResult.green);
                HWindID2.OneResIamge.AddImageMassage(10, 110, "x");
                vision.Vision.Gen_arrow_contour_xld(out HalconDotNet.HObject hObject22, 0, 0, 100, 0);
                HWindID2.OneResIamge.AddImageMassage(100, 10, "y");

                HWindID2.OneResIamge.AddObj(hObject22, ColorResult.yellow);
                MatrixC.Calculate(HWindID2);
                HWindID2.ShowImage();
            }
            catch (Exception)
            {
            }
        }

        private void listBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                MatrixC = DebugCompiler.Instance.ListMatrix[listBox6.SelectedIndex];
                propertyGrid4.SelectedObject = MatrixC;
            }
            catch (Exception)
            {
            }
        }

        private void 添加点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DebugCompiler.Instance.ListMatrix.Add(new 工艺库.MatrixC());
                listBox6.Items.Add(DebugCompiler.Instance.ListMatrix.Count - 1);
            }
            catch (Exception)
            {
            }
        }

        private void 删除矩阵ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DebugCompiler.Instance.ListMatrix.RemoveAt(listBox6.SelectedIndex);
                listBox6.Items.RemoveAt(listBox6.SelectedIndex);
            }
            catch (Exception)
            {
            }
        }
    }
}