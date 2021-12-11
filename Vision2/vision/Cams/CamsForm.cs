using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vision2.vision.Cams
{
    public partial class CamsForm : Form
    {
        public CamsForm()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (var item in flowLayoutPanel1.Controls)
                {
                    CamShow itemCAM= item as CamShow;
                    if (itemCAM!=null)
                    {
                        itemCAM.Straing();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void CamsForm_Load(object sender, EventArgs e)
        {
            try
            {
                treeView1.Nodes.Clear();


                foreach (var item in Vision.Instance.RunCams)
                {
                    TreeNode treeNode= treeView1.Nodes.Add(item.Key);
                    treeNode.Name = item.Key;
                    treeNode.Tag = item.Value;
                    CamShow camShow = new CamShow();
                    camShow.Camera = item.Value;
                    flowLayoutPanel1.Controls.Add(camShow);
                }
             

            }
            catch (Exception)
            {
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                propertyGrid1.SelectedObject = e.Node.Tag;
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (var item in flowLayoutPanel1.Controls)
                {
                    CamShow itemCAM = item as CamShow;
                    if (itemCAM != null)
                    {
                        itemCAM.Stop();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
