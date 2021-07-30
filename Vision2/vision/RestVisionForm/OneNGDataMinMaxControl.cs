using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision2.Project.formula;

namespace Vision2.vision.RestVisionForm
{
    public partial class OneNGDataMinMaxControl : UserControl
    {
        public OneNGDataMinMaxControl()
        {
            InitializeComponent();
        }
        DataMinMax dataMinValue;
        public void UpDataMax(DataMinMax dataMin)
        {
            try
            {
                dataMinValue = dataMin;
                dataGridView1.Rows.Clear();
                for (int i = 0; i < dataMinValue.Reference_Name.Count; i++)
                {
                     int index=  dataGridView1.Rows.Add();
                    if (dataMinValue.Reference_Name.Count > i)
                    {
                        dataGridView1.Rows[index].Cells[0].Value = dataMinValue.Reference_Name[i];
                    }
                    if (dataMinValue.Reference_ValueMin.Count > i)
                    {
                        dataGridView1.Rows[index].Cells[2].Value = dataMinValue.Reference_ValueMin[i];
                    }
                    if (dataMinValue.Reference_ValueMax.Count > i)
                    {
                        dataGridView1.Rows[index].Cells[3].Value = dataMinValue.Reference_ValueMax[i];
                    }
                    if (dataMinValue.ValueStrs.Count>i)
                    {
                        dataGridView1.Rows[index].Cells[1].Value = dataMinValue.ValueStrs[i];
                    }
                }


            }
            catch (Exception)
            {

            }


        }

    }
}
