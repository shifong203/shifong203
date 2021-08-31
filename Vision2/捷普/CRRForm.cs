using System;
using System.Threading;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;
using Vision2.Project.DebugF;
using Vision2.Project.formula;
using Vision2.vision;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.捷普
{
    public partial class CRRForm : Form
    {
        public CRRForm()
        {
            InitializeComponent();
            ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                Thread thread = new Thread(() =>
                {
                    bool Donet = false;
                    dataGridView1.Rows.Clear();
                    int number = 0;
                    HalconDotNet.HTuple p1Diff = new HalconDotNet.HTuple();
                    HalconDotNet.HTuple p1DiffIn = new HalconDotNet.HTuple();
                    HalconDotNet.HTuple p2Diff = new HalconDotNet.HTuple();
                    HalconDotNet.HTuple p2DiffIn = new HalconDotNet.HTuple();
                    HalconDotNet.HTuple p3Diff = new HalconDotNet.HTuple();
                    HalconDotNet.HTuple p3DiffIn = new HalconDotNet.HTuple();
                    HalconDotNet.HTuple p4Diff = new HalconDotNet.HTuple();
                    HalconDotNet.HTuple p4DiffIn = new HalconDotNet.HTuple();
                    while (true)
                    {
                        try
                        {
                            if (DebugCompiler.GetDoDi().Int[0] != Donet)
                            {
                                Donet = DebugCompiler.GetDoDi().Int[0];
                                if (Donet)
                                {
                                    number++;
                                    DebugCompiler.Start();
                                    int dwt = dataGridView1.Rows.Add();
                                    HalconRun halconRun = Vision.GetRunNameVision();
                                    foreach (var item in halconRun.GetRunProgram())
                                    {
                                        if (item.Key == "P1_dist")
                                        {
                                            item.Value.Run(halconRun.GetOneImageR(), new AoiObj());
                                            MeasureMlet measureMlet = item.Value as MeasureMlet;
                                            dataGridView1.Rows[dwt].Cells[0].Value = measureMlet.ValuePP;
                                            p1Diff.Append(measureMlet.ValuePP);
                                            p1DiffIn.Append(measureMlet.ScaleMM(measureMlet.ValuePP));
                                            dataGridView1.Rows[dwt].Cells[4].Value = measureMlet.ScaleMM(measureMlet.ValuePP);
                                            //double vaet = (double)numericUpDown1.Value / measureMlet.ValuePP;
                                            //label5.Text = "校准值:" + vaet + "=" + numericUpDown1.Value + "/" + measureMlet.ValuePP;
                                            //measureMlet.Scale = vaet;
                                        }
                                        else if (item.Key == "P2_dist")
                                        {
                                            item.Value.Run(halconRun.GetOneImageR(), new AoiObj());
                                            MeasureMlet measureMlet = item.Value as MeasureMlet;
                                            dataGridView1.Rows[dwt].Cells[1].Value = measureMlet.ValuePP;
                                            p2Diff.Append(measureMlet.ValuePP);
                                            p2DiffIn.Append(measureMlet.ScaleMM(measureMlet.ValuePP));
                                            dataGridView1.Rows[dwt].Cells[5].Value = measureMlet.ScaleMM(measureMlet.ValuePP);
                                        }
                                        else if (item.Key == "P3_dist")
                                        {
                                            item.Value.Run(halconRun.GetOneImageR(), new AoiObj());
                                            MeasureMlet measureMlet = item.Value as MeasureMlet;
                                            dataGridView1.Rows[dwt].Cells[2].Value = measureMlet.ValuePP;
                                            p3Diff.Append(measureMlet.ValuePP);
                                            p3DiffIn.Append(measureMlet.ScaleMM(measureMlet.ValuePP));
                                            dataGridView1.Rows[dwt].Cells[6].Value = measureMlet.ScaleMM(measureMlet.ValuePP);
                                        }
                                        else if (item.Key == "P4_dist")
                                        {
                                            item.Value.Run(halconRun.GetOneImageR(), new AoiObj());
                                            MeasureMlet measureMlet = item.Value as MeasureMlet;
                                            dataGridView1.Rows[dwt].Cells[3].Value = measureMlet.ValuePP;
                                            p4Diff.Append(measureMlet.ValuePP);
                                            p4DiffIn.Append(measureMlet.ScaleMM(measureMlet.ValuePP));
                                            dataGridView1.Rows[dwt].Cells[7].Value = measureMlet.ScaleMM(measureMlet.ValuePP);
                                        }
                                    }
                                    label9.Text = "执行" + number;
                                }
                            }
                            if (number >= numericUpDown5.Value)
                            {
                                this.Invoke(new Action(() =>
                                {
                                    richTextBox1.AppendText("像素精度" + Environment.NewLine);
                                    richTextBox1.AppendText("P1:Min" + p1Diff.TupleMin() + "Men" + p1Diff.TupleMean() + "max" + p1Diff.TupleMax()
                                        + "差值" + p1Diff.TupleMax().TupleSub(p1Diff.TupleMin()).TupleString("0.06f") + Environment.NewLine);
                                    richTextBox1.AppendText("P2:Min" + p2Diff.TupleMin() + "Men" + p2Diff.TupleMean() + "max" + p2Diff.TupleMax()
                                        + "差值" + p2Diff.TupleMax().TupleSub(p2Diff.TupleMin()).TupleString("0.06f") + Environment.NewLine);
                                    richTextBox1.AppendText("P3:Min" + p3Diff.TupleMin() + "Men" + p3Diff.TupleMean() + "max" + p3Diff.TupleMax()
                                        + "差值" + p3Diff.TupleMax().TupleSub(p3Diff.TupleMin()).TupleString("0.06f") + Environment.NewLine);
                                    richTextBox1.AppendText("P4:Min" + p1Diff.TupleMin() + "Men" + p4Diff.TupleMean() + "max" + p4Diff.TupleMax()
                                        + "差值" + p4Diff.TupleMax().TupleSub(p4Diff.TupleMin()).TupleString("0.06f") + Environment.NewLine);
                                    richTextBox1.AppendText("实际精度" + Environment.NewLine);
                                    richTextBox1.AppendText("P1:Min" + p1DiffIn.TupleMin() + "Men" + p1DiffIn.TupleMean() + "max" + p1DiffIn.TupleMax()
                                        + "差值" + p1DiffIn.TupleMax().TupleSub(p1DiffIn.TupleMin()).TupleString("0.06f") + Environment.NewLine);
                                    richTextBox1.AppendText("P2:Min" + p2DiffIn.TupleMin() + "Men" + p2DiffIn.TupleMean() + "max" + p2DiffIn.TupleMax() +
                                   "差值" + p2DiffIn.TupleMax().TupleSub(p2DiffIn.TupleMin()).TupleString("0.06f") + Environment.NewLine);
                                    richTextBox1.AppendText("P3:Min" + p3DiffIn.TupleMin() + "Men" + p3DiffIn.TupleMean() + "max" + p3DiffIn.TupleMax()
                                        + "差值" + p3DiffIn.TupleMax().TupleSub(p3DiffIn.TupleMin()).TupleString("0.06f") + Environment.NewLine);
                                    richTextBox1.AppendText("P4:Min" + p4DiffIn.TupleMin() + "Men" + p4DiffIn.TupleMean() + "max" + p4DiffIn.TupleMax()
                                        + "差值" + p4DiffIn.TupleMax().TupleSub(p4DiffIn.TupleMin()).TupleString("0.06f") + Environment.NewLine);
                                }));

                                HalconRun halconRun = Vision.GetRunNameVision();
                                foreach (var item in halconRun.GetRunProgram())
                                {
                                    if (item.Key == "P1_dist")
                                    {
                                        item.Value.Run(halconRun.GetOneImageR(), new AoiObj());
                                        MeasureMlet measureMlet = item.Value as MeasureMlet;
                                        double vaet = p1DiffIn.TupleMean() / p1Diff.TupleMean();
                                        measureMlet.Scale = vaet; label5.Text = "比例:" + vaet.ToString("0.00000000");
                                    }
                                    else if (item.Key == "P2_dist")
                                    {
                                        item.Value.Run(halconRun.GetOneImageR(), new AoiObj());
                                        MeasureMlet measureMlet = item.Value as MeasureMlet;
                                        double vaet = p2DiffIn.TupleMean() / p2Diff.TupleMean();
                                        measureMlet.Scale = vaet; label6.Text = "比例:" + vaet.ToString("0.00000000");
                                    }
                                    else if (item.Key == "P3_dist")
                                    {
                                        item.Value.Run(halconRun.GetOneImageR(), new AoiObj());
                                        MeasureMlet measureMlet = item.Value as MeasureMlet;
                                        double vaet = p3DiffIn.TupleMean() / p3Diff.TupleMean();
                                        measureMlet.Scale = vaet; label7.Text = "比例:" + vaet.ToString("0.00000000");
                                    }
                                    else if (item.Key == "P4_dist")
                                    {
                                        item.Value.Run(halconRun.GetOneImageR(), new AoiObj());
                                        MeasureMlet measureMlet = item.Value as MeasureMlet;
                                        double vaet = p4DiffIn.TupleMean() / p4Diff.TupleMean();
                                        measureMlet.Scale = vaet;
                                        label8.Text = "比例:" + vaet.ToString("0.00000000");
                                    }
                                }
                                break;
                            }
                        }
                        catch (Exception ex)
                        { }
                        Thread.Sleep(10);
                    }
                });
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception)
            {
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                UserFormulaContrsl.StaticAddQRCode("校验");
                DebugCompiler.Start();
                HalconRun halconRun = Vision.GetRunNameVision();
                foreach (var item in halconRun.GetRunProgram())
                {
                    if (item.Key == "P1_dist")
                    {
                        item.Value.Run(halconRun.GetOneImageR(), new AoiObj());
                        MeasureMlet measureMlet = item.Value as MeasureMlet;
                        double vaet = (double)numericUpDown1.Value / measureMlet.ValuePP;
                        label5.Text = "比例:" + vaet.ToString("0.00000000"); /*+ "=" + numericUpDown1.Value + "/" + measureMlet.ValuePP;*/
                        measureMlet.Scale = vaet;
                        measureMlet.ResValue = (double)numericUpDown1.Value;
                    }
                    else if (item.Key == "P2_dist")
                    {
                        item.Value.Run(halconRun.GetOneImageR(), new AoiObj());
                        MeasureMlet measureMlet = item.Value as MeasureMlet;
                        double vaet = (double)numericUpDown2.Value / measureMlet.ValuePP;
                        label6.Text = "比例:" + vaet.ToString("0.00000000"); /*+ "=" + numericUpDown2.Value + "/" + measureMlet.ValuePP;*/
                        measureMlet.Scale = vaet;
                        measureMlet.ResValue = (double)numericUpDown2.Value;
                    }
                    else if (item.Key == "P3_dist")
                    {
                        item.Value.Run(halconRun.GetOneImageR(), new AoiObj());
                        MeasureMlet measureMlet = item.Value as MeasureMlet;
                        double vaet = (double)numericUpDown3.Value / measureMlet.ValuePP;
                        label7.Text = "比例:" + vaet.ToString("0.00000000"); /*+ "=" + numericUpDown3.Value + "/" + measureMlet.ValuePP;*/
                        measureMlet.Scale = vaet;
                        measureMlet.ResValue = (double)numericUpDown3.Value;
                    }
                    else if (item.Key == "P4_dist")
                    {
                        item.Value.Run(halconRun.GetOneImageR(), new AoiObj());
                        MeasureMlet measureMlet = item.Value as MeasureMlet;
                        double vaet = (double)numericUpDown4.Value / measureMlet.ValuePP;
                        label8.Text = "比例:" + vaet.ToString("0.00000000"); /*+ "=" + numericUpDown4.Value + "/" + measureMlet.ValuePP;*/
                        measureMlet.Scale = vaet;
                        measureMlet.ResValue = (double)numericUpDown4.Value;
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
                Vision.Instance.SaveRunPojcet();
                HalconRun halconRun = Vision.GetRunNameVision();

                foreach (var item in halconRun.GetRunProgram())
                {
                    MeasureMlet measureMlet = item.Value as MeasureMlet;
                    if (measureMlet == null)
                    {
                        continue;
                    }
                    if (item.Key == "P1_dist")
                    {
                        measureMlet.ResValue = (double)numericUpDown1.Value;
                        ProjectINI.WritePrivateProfileString("cal_data_mm", "P1_dist_pixels", measureMlet.Scale.ToString("0.00000000"), Application.StartupPath + @"\setting.ini");
                        ProjectINI.WritePrivateProfileString("dist_mm", "P1_dist", measureMlet.ResValue.ToString(), Application.StartupPath + @"\setting.ini");
                    }
                    else if (item.Key == "P2_dist")
                    {
                        measureMlet.ResValue = (double)numericUpDown2.Value;
                        ProjectINI.WritePrivateProfileString("cal_data_mm", "P2_dist_pixels", measureMlet.Scale.ToString("0.00000000"), Application.StartupPath + @"\setting.ini");
                        ProjectINI.WritePrivateProfileString("dist_mm", "P2_dist", measureMlet.ResValue.ToString(), Application.StartupPath + @"\setting.ini");
                    }
                    else if (item.Key == "P3_dist")
                    {
                        measureMlet.ResValue = (double)numericUpDown3.Value;
                        ProjectINI.WritePrivateProfileString("cal_data_mm", "P3_dist_pixels", measureMlet.Scale.ToString("0.00000000"), Application.StartupPath + @"\setting.ini");
                        ProjectINI.WritePrivateProfileString("dist_mm", "P3_dist", measureMlet.ResValue.ToString(), Application.StartupPath + @"\setting.ini");
                    }
                    else if (item.Key == "P4_dist")
                    {
                        measureMlet.ResValue = (double)numericUpDown4.Value;
                        ProjectINI.WritePrivateProfileString("cal_data_mm", "P4_dist_pixels", measureMlet.Scale.ToString("0.00000000"), Application.StartupPath + @"\setting.ini");
                        ProjectINI.WritePrivateProfileString("dist_mm", "P4_dist", measureMlet.ResValue.ToString(), Application.StartupPath + @"\setting.ini");
                    }
                }

                MessageBox.Show("保存成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "保存失败");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
        }

        private void CRRForm_Load(object sender, EventArgs e)
        {
            try
            {
                HalconRun halconRun = Vision.GetRunNameVision();

                foreach (var item in halconRun.GetRunProgram())
                {
                    MeasureMlet measureMlet = item.Value as MeasureMlet;
                    if (measureMlet == null)
                    {
                        continue;
                    }
                    if (item.Key == "P1_dist")
                    {
                        numericUpDown1.Value = (decimal)measureMlet.ResValue;
                    }
                    else if (item.Key == "P2_dist")
                    {
                        numericUpDown2.Value = (decimal)measureMlet.ResValue;
                    }
                    else if (item.Key == "P3_dist")
                    {
                        numericUpDown3.Value = (decimal)measureMlet.ResValue;
                    }
                    else if (item.Key == "P4_dist")
                    {
                        numericUpDown4.Value = (decimal)measureMlet.ResValue;
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}