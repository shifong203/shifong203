using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
    public partial class ThresholdControl : UserControl
    {
        public delegate void EventValue( List<Threshold_Min_Max> threshold_Min_s);

        public event EventValue evValue;
       
        public ThresholdControl()
        {
            InitializeComponent();
            Column1.Items.AddRange(Enum.GetNames(typeof(ImageTypeObj)));
        }
        bool isChanged;
        
        public ThresholdControl(List<Threshold_Min_Max>  threshold_Min_s):this()
        {
            SetData(threshold_Min_s);
        }
        public void SetData(List<Threshold_Min_Max> threshold_Min_s)
        {
            isChanged = true;
            threshold_Min_Maxes = threshold_Min_s;
            dataGridView1.Rows.Clear();
            for (int i = 0; i < threshold_Min_Maxes.Count; i++)
            {
                int det = dataGridView1.Rows.Add();
                dataGridView1.Rows[det].Cells[0].Value = threshold_Min_Maxes[i].ImageTypeObj.ToString();
                dataGridView1.Rows[det].Cells[1].Value = threshold_Min_Maxes[i].Min;
                dataGridView1.Rows[det].Cells[2].Value = threshold_Min_Maxes[i].Max;
                dataGridView1.Rows[det].Cells[3].Value = threshold_Min_Maxes[i].Enabled;
            }
            isChanged = false;
        }

        List<Threshold_Min_Max> threshold_Min_Maxes = new List<Threshold_Min_Max>();
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (isChanged)
                {
                    return;
                }
                threshold_Min_Maxes.Clear();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    Threshold_Min_Max threshold_Min_Max = new Threshold_Min_Max();
                    threshold_Min_Max.ImageTypeObj = (ImageTypeObj)Enum.Parse(typeof(ImageTypeObj),
                        dataGridView1.Rows[i].Cells[0].EditedFormattedValue.ToString());
                    threshold_Min_Max.Min =byte.Parse( dataGridView1.Rows[i].Cells[1].Value.ToString());
                    threshold_Min_Max.Max = byte.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString());
                    threshold_Min_Max.Enabled =bool.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
                    threshold_Min_Maxes.Add(threshold_Min_Max);
                }
                evValue?.Invoke( threshold_Min_Maxes);
            }
            catch (Exception ex)
            {
            }
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            try
            {
           
            }
            catch (Exception ex)
            {
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int det = dataGridView1.SelectedCells[0].RowIndex;
                dataGridView1.Rows.RemoveAt(det);
                threshold_Min_Maxes.RemoveAt(det);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
            int DET=    dataGridView1.Rows.Add();
                isChanged = true;
                dataGridView1.Rows[DET].Cells[0].Value = ImageTypeObj.Image3.ToString();
                dataGridView1.Rows[DET].Cells[1].Value = 1;
                dataGridView1.Rows[DET].Cells[2].Value = 100;
                dataGridView1.Rows[DET].Cells[3].Value = true;
                //dataGridView1.Rows[DET].Cells[4].Value = 1;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            isChanged = false;
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }
    }
}
