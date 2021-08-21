using System;
using System.Windows.Forms;

namespace Vision2.vision
{
    public partial class RunVisionPrForm : Form
    {
        public RunVisionPrForm()
        {
            InitializeComponent();
        }

        private void RunVisionPrForm_Load(object sender, EventArgs e)
        {
            try
            {
                HWindID.Initialize(hWindowControl1);
                foreach (var item in Vision.GetHimageList())
                {
                    TreeNode treeNode = new TreeNode();
                    treeNode.Tag = item.Value;
                    treeNode.Text = item.Key;
                    treeView1.Nodes.Add(treeNode);
                    foreach (var itemiDX in item.Value.GetRunProgram())
                    {
                        TreeNode treeNodeitem = new TreeNode();
                        treeNodeitem.Text = itemiDX.Key;
                        treeNodeitem.Tag = itemiDX.Value;

                        treeNode.Nodes.Add(treeNodeitem);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private HWindID HWindID = new HWindID();
        private OneResultOBj oneResultOBj;

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                propertyGrid1.SelectedObject = e.Node.Tag;

                HalconRunFile.RunProgramFile.HalconRun halconRun = e.Node.Tag as HalconRunFile.RunProgramFile.HalconRun;
                if (halconRun != null)
                {
                    oneResultOBj = halconRun.GetOneImageR();
                    HWindID.SetImaage(oneResultOBj.Image);
                }
                else
                {
                    HalconRunFile.RunProgramFile.RunProgram run = e.Node.Tag as HalconRunFile.RunProgramFile.RunProgram;
                    if (run != null)
                    {
                        tabPage2.Text = run.Name;
                        tabPage2.Controls.Clear();
                        tabPage2.Controls.Add(run.GetControl(run.GetPThis()));
                        halconRun = run.GetPThis();
                        if (halconRun != null)
                        {
                            if (!oneResultOBj.Equals(halconRun.GetOneImageR()))
                            {
                                oneResultOBj = halconRun.GetOneImageR();
                                HWindID.SetImaage(oneResultOBj.Image);
                            }
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