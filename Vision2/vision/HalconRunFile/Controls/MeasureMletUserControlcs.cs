using Microsoft.VisualBasic;
using System;
using System.Linq;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class MeasureMletUserControlcs : UserControl
    {
        public MeasureMletUserControlcs()
        {
            InitializeComponent();
            comboBox1.Items.Clear();
            ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView1);
        }

        private RunProgramFile.MeasureMlet measure;
        private HalconRunFile.RunProgramFile.HalconRun halcon;
        private bool isChanged = true;

        public MeasureMletUserControlcs(MeasureMlet measureMlet) : this()
        {
            isChanged = true;
            propertyGrid1.SelectedObject = measureMlet;
            dynamicParameter1.SetUpData(measureMlet);
            measure = measureMlet;
            comboBox4.SelectedItem = measure.SelePointName;
            numericUpDown2.Value = (decimal)measure.DistanceMin;
            numericUpDown3.Value = (decimal)measure.DistanceMax;
            numericUpDown1.Value = (decimal)measure.AngleMin;
            numericUpDown4.Value = (decimal)measure.AngleMax;
            comboBox1.Items.AddRange(Enum.GetNames(typeof(MeasureMlet.MeasureModeEnum)));
            comboBox2.Items.AddRange(measure.Dic_Measure.Keys_Measure.Keys.ToArray());
            comboBox3.Items.AddRange(measure.Dic_Measure.Keys_Measure.Keys.ToArray());
            comboBox2.SelectedItem = measure.MeasureName1;
            comboBox3.SelectedItem = measure.MeasureName2;
            comboBox1.SelectedItem = measure.MeasureMode.ToString();
            foreach (var item in measure.MestaNames)
            {
                listBox2.Items.Add(item.Key);
            }
            halcon = measure.GetPThis();
            listBox1.Items.Clear();
            listBox1.Items.AddRange(measure.Dic_Measure.Keys_Measure.Keys.ToArray());
            isChanged = false;
        }

        /// <summary>
        /// 绘制
        /// </summary>
        /// <param name="halcon"></param>
        /// <returns></returns>
        private HalconDotNet.HObject Draw(RunProgramFile.HalconRun halcon)
        {
            try
            {
                HalconDotNet.HOperatorSet.DrawRegion(out HalconDotNet.HObject hObject, halcon.hWindowHalcon());
                return hObject;
            }
            catch (Exception)
            {
            }
            return null;
        }

      

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                halcon.HobjClear();
                if (listBox1.SelectedItem != null)
                {
                    //Measure.Dic_Measure[listBox1.SelectedItem.ToString()].HomName = Measure.HomName;
                    measureConTrolEx1.Updata(measure.Dic_Measure[listBox1.SelectedItem.ToString()], halcon);
                    halcon.AddObj(measure.Dic_Measure[listBox1.SelectedItem.ToString()].GetHamMatDraw());
                    halcon.ShowObj();
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
                string name = "测量";
                foreach (var item in measure.Dic_Measure.Keys_Measure)
                {
                    name = item.Key;
                    break;
                }
            st:
                string nameStr = "";
                if (measure.Dic_Measure.Keys_Measure.ContainsKey(name))
                {
                    int idx = ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(name, out name);
                    name = name + (idx + 1);
                    goto st;
                }
                if (listBox1.SelectedItem != null)
                {
                    RunProgramFile.Measure measureM = measure.Dic_Measure.Add(listBox1.SelectedItem.ToString());
                    if (measureM != null)
                    {
                        nameStr = measureM.Name;
                    }
                }
                else
                {
                    nameStr = Interaction.InputBox("请输入产品名", "新建产品", name, 100, 100);
                    if (nameStr == "")
                    {
                        return;
                    }
                    measure.Dic_Measure.Add(nameStr);
                }

                if (nameStr.Length == 0)
                {
                    return;
                }
                listBox1.Items.Add(nameStr);
                comboBox2.Items.Add(nameStr);
                comboBox3.Items.Add(nameStr);
            }
            catch (Exception)
            { }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (measure.Dic_Measure.Keys_Measure.ContainsKey(listBox1.SelectedItem.ToString()))
                {
                    measure.Dic_Measure.Keys_Measure.Remove(listBox1.SelectedItem.ToString());
                }
                comboBox2.Items.Remove(listBox1.SelectedItem);
                comboBox3.Items.Remove(listBox1.SelectedItem);
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
            catch (Exception)
            {
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isChanged)
            {
                return;
            }
            try
            {
                measure.MeasureMode = (MeasureMlet.MeasureModeEnum)Enum.Parse(typeof(MeasureMlet.MeasureModeEnum), comboBox1.SelectedItem.ToString());
            }
            catch (Exception)
            {
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            measure.MeasureName1 = comboBox2.SelectedItem.ToString();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            measure.MeasureName2 = comboBox3.SelectedItem.ToString();
        }

        private void comboBox2_DropDown(object sender, EventArgs e)
        {
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            if (isChanged)
            {
                return;
            }
            try
            {
                measure.DistanceMin = (double)numericUpDown2.Value;
                measure.DistanceMax = (double)numericUpDown3.Value;
                measure.AngleMin = (double)numericUpDown1.Value;
                measure.AngleMax = (double)numericUpDown4.Value;
            }
            catch (Exception)
            {
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isChanged)
            {
                return;
            }
            try
            {
                measure.SelePointName = comboBox4.SelectedItem.ToString();
            }
            catch (Exception)
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                measure.Run(halcon.GetOneImageR(), new AoiObj());
                double vaet = (double)numericUpDown5.Value / measure.ValuePP;
                DialogResult dialogResult = MessageBox.Show("校准值:" + vaet + "=" + numericUpDown5.Value + "/" + measure.ValuePP, "校准完成!点击YES完成校准", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes) measure.Scale = vaet;
            }
            catch (Exception ex)
            {
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                AoiObj aoiObj = measure.GetAoi();
                aoiObj.DebugID = 1;
                measure.Run(halcon.GetOneImageR(), aoiObj);
                halcon.GetOneImageR().DebugID = 2;
                propertyGrid2.SelectedObject = measure.MestaNames[listBox2.SelectedItem.ToString()];
                measure.MestaNames[listBox2.SelectedItem.ToString()].Mestan(halcon.GetOneImageR(), measure,
                measure.XYzroeRows, measure.  XYzroeColus, measure.dinRowX2, 
                measure.dinColX2, measure.dinRowY2, measure.dinColY2);
                halcon.ShowObj();
            }
            catch (Exception ex)
            {

            }
        }

        private void 添加ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                string name = "P";
                foreach (var item in measure.MestaNames)
                {
                    name = item.Key;
                    break;
                }
               st:
                string nameStr = "";
                if (measure.MestaNames.ContainsKey(name))
                {
                    int idx = ErosProjcetDLL.Project.ProjectINI.GetStrReturnInt(name, out name);
                    name = name + (idx + 1);
                    goto st;
                }
                    nameStr = Interaction.InputBox("请输入产品名", "新建产品", name, 100, 100);
                    if (nameStr == "")
                    {
                        return;
                    }
                    measure.MestaNames.Add(nameStr,new MeasureMlet.MestanData());
                if (nameStr.Length == 0)
                {
                    return;
                }
                listBox2.Items.Add(nameStr);
               
            }
            catch (Exception)
            { }
        }

        private void 删除ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                measure.MestaNames.Remove(listBox2.SelectedItem.ToString());
                listBox2.Items.Remove(listBox2.SelectedItem.ToString());

            }
            catch (Exception)
            { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
            AoiObj aoiObj=     measure.GetAoi();
                aoiObj.DebugID = 1;
                measure.Run(halcon.GetOneImageR(), aoiObj);
            }
            catch (Exception ex)
            {

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                isChanged = true;
                dataGridView1.Rows.Clear();
                Column6.Items.Clear();
              
                Column6.Items.AddRange(measure.Dic_Measure.Keys_Measure.Keys.ToArray());
                foreach (var item in measure.MestaNames)
                {
                    int det=dataGridView1.Rows.Add();
                    dataGridView1.Rows[det].Cells[0].Value = item.Key;
                    dataGridView1.Rows[det].Cells[1].Value = item.Value.MeasureX;
                    dataGridView1.Rows[det].Cells[2].Value = item.Value.MeasureY;
                    dataGridView1.Rows[det].Cells[3].Value = item.Value.ValueX;
                    dataGridView1.Rows[det].Cells[4].Value = item.Value.ValueY;
                    dataGridView1.Rows[det].Cells[5].Value = item.Value.MeasureName;
                    //dataGridView1.Rows[det].Cells[5].Value = item.Value.ValueXp;
                    //dataGridView1.Rows[det].Cells[6].Value = item.Value.ValueYp;
                }
            }
            catch (Exception)
            {
            }
            isChanged = false;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (isChanged)
                {
                    return;
                }
                string names = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                if (e.ColumnIndex==1)
                {
                    measure.MestaNames[names].MeasureX =double.Parse( dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
                }
                if (e.ColumnIndex == 2)
                {
                    measure.MestaNames[names].MeasureY = double.Parse(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString());
                }
                if (e.ColumnIndex == 5)
                {
                    measure.MestaNames[names].MeasureName = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                AoiObj aoiObj = measure.GetAoi();
                aoiObj.DebugID = 2;
                halcon.GetOneImageR().DebugID = 2;
                measure.Run(halcon.GetOneImageR(), aoiObj);
            }
            catch (Exception ex)
            {

            }
        }
    }
}