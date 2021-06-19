using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vision2.vision.HalconRunFile.PCBFile
{
    public partial class PCBLibraryForm : Form
    {
        public PCBLibraryForm()
        {
            InitializeComponent();
            HWindID.Initialize(hWindowControl1);
        }
        HWindID HWindID = new HWindID();

        public PCBLibraryForm(PCBAEX pCBAEX) : this()
        {
            PCBAEX = pCBAEX;
        }
        PCBAEX PCBAEX;

        public void UPdata()
        {
            try
            {
                treeView1.Nodes.Clear();
                foreach (var item in PCBAEX.DictRoi)
                {
                    TreeNode treeNode= treeView1.Nodes.Add(item.Key);
                    treeNode.Tag = item.Value;
                }
            }
            catch (Exception)
            {}
        }

        private void PCBLibraryForm_Load(object sender, EventArgs e)
        {
            UPdata();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
