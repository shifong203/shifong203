using System;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;
namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class ObjDataGr : UserControl
    {
        public ObjDataGr()
        {
            InitializeComponent();
            dataGridView1.Rows.Clear();
            UpObj();
        }

        public void UpObj()
        {
            HalconRun.DicHObject dicHObject = this.Tag as HalconRun.DicHObject;
            try
            {
                if (dicHObject != null)
                {
                    int ds = 0;

                }
                else
                {
                    HalconRun.DicHObjectColot objectColot = this.Tag as HalconRun.DicHObjectColot;
                    if (objectColot != null)
                    {
                        int ds = 0;
                        foreach (var item in objectColot.DirectoryHObject)
                        {
                            ds = dataGridView1.Rows.Add();
                            dataGridView1.Rows[ds].Cells[0].Value = item.Key;
                            dataGridView1.Rows[ds].Cells[1].Value = item.Value._HObject.CountObj();
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}