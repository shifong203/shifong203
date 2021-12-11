using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vision2.vision
{
    public partial class Offline_Debugging_Form1 : Form
    {
        public Offline_Debugging_Form1()
        {
            InitializeComponent();
        }

          List<  Project.DebugF.Device_State> devices;

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            UpdataP();
        }
        public void UpdataP()
        {
            try
            {
                treeView1.Nodes.Clear();
                devices = new List<Project.DebugF.Device_State>();
                for (int i = 0; i < ErosProjcetDLL.Project.ProjectINI.In.DicNameRunFacility.Count; i++)
                {
                    string[] paths = System.IO.Directory.GetDirectories(ErosProjcetDLL.Project.ProjectINI.In.DicNameRunFacility[i]);
                    string[] imtes = System.IO.Directory.GetDirectories(ErosProjcetDLL.Project.ProjectINI.In.DicNameRunFacility[i] + "\\Temp");
                    string[] imageFile = System.IO.Directory.GetFiles(ErosProjcetDLL.Project.ProjectINI.In.DicNameRunFacility[i] + "\\Temp");
                    ErosProjcetDLL.Project.ProjectINI.ReadPathJsonToCalss(
                        ErosProjcetDLL.Project.ProjectINI.In.DicNameRunFacility[i] + "\\Temp\\设备状态", out Project.DebugF.Device_State dATS);
                    devices.Add(dATS);
                    TreeNode treeNode = treeView1.Nodes.Add(dATS.DeviceName);
                    treeNode.Tag = dATS;
                    treeNode.ImageIndex = 0;
                    for (int ij = 0; ij < dATS.ProductNames.Count; ij++)
                    {
                        TreeNode treeNo = treeNode.Nodes.Add(dATS.ProductNames[ij]);
                        treeNo.ImageIndex = 2;
                        if (treeNo.Text == dATS.Product_Name)
                        {
                            treeNo.ImageIndex = 5;
                        }
                    }


                }
                treeView1.ExpandAll();
            }
            catch (Exception ex)
            {
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
             propertyGrid1.SelectedObject=    e.Node.Tag;
            }
            catch (Exception)
            {

            }
        }

        private void Offline_Debugging_Form1_Load(object sender, EventArgs e)
        {
            try
            {
                UpdataP();
                Task task = new Task(() => {
                    while (true)
                    {
                        try
                        {
                            devices = new List<Project.DebugF.Device_State>();
                            for (int i = 0; i < ErosProjcetDLL.Project.ProjectINI.In.DicNameRunFacility.Count; i++)
                            {
                                string[] paths = System.IO.Directory.GetDirectories(ErosProjcetDLL.Project.ProjectINI.In.DicNameRunFacility[i]);
                                string[] imtes = System.IO.Directory.GetDirectories(ErosProjcetDLL.Project.ProjectINI.In.DicNameRunFacility[i] + "\\Temp");
                                string[] imageFile = System.IO.Directory.GetFiles(ErosProjcetDLL.Project.ProjectINI.In.DicNameRunFacility[i] + "\\Temp");
                                ErosProjcetDLL.Project.ProjectINI.ReadPathJsonToCalssEX(
                                    ErosProjcetDLL.Project.ProjectINI.In.DicNameRunFacility[i] + "\\Temp\\设备状态", out Project.DebugF.Device_State dATS);
                                if (dATS!=null)
                                {
                                    devices.Add(dATS);
                                    TreeNode treeNode;
                                    if (!treeView1.Nodes.ContainsKey(dATS.DeviceName))
                                    {
                                        treeNode = treeView1.Nodes.Add(dATS.DeviceName);
                                    }
                                    treeNode = treeView1.Nodes.Find(dATS.DeviceName, false)[0];
                                    treeNode.Tag = dATS;
                                    treeNode.ImageIndex = 0;
                                    for (int ij = 0; ij < dATS.ProductNames.Count; ij++)
                                    {
                                        TreeNode treeNo;
                                        if (!treeView1.Nodes.ContainsKey(dATS.DeviceName))
                                        {
                                            treeNode = treeView1.Nodes.Add(dATS.DeviceName);
                                        }
                                        else
                                        {
                                            treeNo = treeNode.Nodes.Add(dATS.ProductNames[ij]);
                                        }
                                        treeNo = treeNode.Nodes.Find(dATS.ProductNames[ij], false)[0];
                                        treeNo.ImageIndex = 2;
                                        if (treeNo.Text == dATS.Product_Name)
                                        {
                                            treeNo.ImageIndex = 5;
                                        }
                                    }

                                }
                    
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        Thread.Sleep(100);
                    }
                   
                });

                task.Start();

            }
            catch (Exception ex)
            {   }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {

            }    
        }
    }
}
