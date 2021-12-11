using HalconDotNet;
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
using Vision2.Project.Mes;
using Vision2.vision;
using Vision2.vision.HalconRunFile.RunProgramFile;
using static Vision2.vision.HalconRunFile.RunProgramFile.OneCompOBJs;

namespace Vision2.Project.DebugF.IO
{
    public partial class TrayDataForm : Form
    {
        public TrayDataForm()
        {
            InitializeComponent();
       
            HWi.Initialize(hWindowControl1);
            ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView2);
            TrayDataFo = this;
           
        }
        public static TrayDataForm TrayDataFo
        {
            get {
                if (form==null|| form.IsDisposed)
                {
                    form = new TrayDataForm();
                }
                return form;
            }
            set {form = value;}
        }
         static TrayDataForm form;

        private HWindID HWi = new HWindID();

        public OneDataVale OneProductV;


        public static void UPStaticData(OneDataVale oneDataVale)
        {
            try
            {
                TrayDataFo.UPData(oneDataVale);
                ErosProjcetDLL.UI. UICon.SwitchToThisWindow(TrayDataFo.Handle, true);
                TrayDataFo.Show();
            }
            catch (Exception ex)
            {

            }
      

        }
        public void UPData(OneDataVale dataObj)
        {
          try
                {
                    dataGridView2.Visible = false;
                    if (dataObj != null)
                    {
                        toolStripLabel1.Text = "N:" + dataObj.TrayLocation + ";SN:";
                        toolStripTextBox1.Text = dataObj.PanelID;
                        if (dataObj != null)
                        {   treeView1.Nodes.Clear();
                            if (dataObj.ListCamsData.Count == 0)
                            {
                                tabControl1.TabPages.Remove(tabPage2);
                            }
                            else
                            {
                                if (!tabControl1.TabPages.Contains(tabPage2))
                                {
                                    tabControl1.TabPages.Add(tabPage2);
                                }
                            }
                            if (dataObj.GetNGImage() != null)
                            {
                                if (!dataObj.GetNGImage().IsInitialized())
                                {
                                    tabControl1.TabPages.Remove(tabPage1);
                                }
                                else
                                {
                                    if (!tabControl1.TabPages.Contains(tabPage1))
                                    {
                                        tabControl1.TabPages.Add(tabPage1);
                                    }
                                }
                            }
                            else
                            {
                                tabControl1.TabPages.Remove(tabPage1);
                            }
                            if (tabControl1.TabPages.Count == 0)
                            {
                                this.panel1.Height = 100;
                            }
                            else
                            {
                                this.panel1.Height = 600;
                            }
                            foreach (var item in dataObj.ListCamsData)
                            {
                                TreeNode treeNode = treeView1.Nodes.Add(item.Key);
                                treeNode.Tag = item.Value;
                                treeNode.ImageIndex = 0;
                                dataGridView2.Rows.Clear();
                                int index = 0;
                                foreach (var itemd in item.Value.NGObj.DicOnes)
                                {
                                    if (itemd.Value.oneRObjs.Count > 0)
                                    {
                                        DataMinMax da = itemd.Value.oneRObjs[0].dataMinMax;
                                        if (da != null)
                                        {
                                            if (da.Reference_Name.Count == 0)
                                            {
                                                continue;
                                            }
                                            dataGridView2.Rows.Add(da.Reference_Name.Count);
                                            for (int i = 0; i < da.Reference_Name.Count; i++)
                                            {
                                                dataGridView2.Rows[index].Cells[0].Value = itemd.Key + "." + da.Reference_Name[i];
                                                if (da.ValueStrs.Count > i)
                                                {
                                                    dataGridView2.Rows[index].Cells[1].Value = da.ValueStrs[i];
                                                }
                                                dataGridView2.Rows[index].Cells[2].Value = da.Reference_ValueMin[i];
                                                dataGridView2.Rows[index].Cells[3].Value = da.Reference_ValueMax[i];
                                                index++;
                                            }
                                            if (da.ValueStrs.Count > dataGridView2.Rows.Count)
                                            {
                                                dataGridView2.Rows.Add(da.ValueStrs.Count - da.Reference_Name.Count);
                                                for (int i = da.Reference_Name.Count; i < da.ValueStrs.Count; i++)
                                                {
                                                    dataGridView2.Rows[index].Cells[1].Value = da.ValueStrs[i];
                                                    index++;
                                                }
                                            }
                                        }
                                    }
                                }
                                dataGridView2.Dock = DockStyle.Fill;
                                dataGridView2.Visible = true;
                                dataGridView2.BringToFront();
                                dataGridView2.Show();
                                foreach (var itemd in item.Value.NGObj.DicOnes)
                                {
                                    TreeNode treeNode1 = treeNode.Nodes.Add(itemd.Key);
                                    if (itemd.Value.aOK)
                                    {
                                        treeNode1.ImageIndex = 7;
                                    }
                                    else
                                    {
                                        treeNode1.ImageIndex = 2;
                                    }
                                    treeNode1.Tag = itemd.Value;
                                }

                                foreach (var itemd in item.Value.GetAllCompOBJs().DicOnes)
                                {
                                    TreeNode treeNode1 = treeNode.Nodes.Add(itemd.Key);
                                    if (itemd.Value.aOK)
                                    {
                                        treeNode1.ImageIndex = 7;
                                    }
                                    else
                                    {
                                        treeNode1.ImageIndex = 2;
                                    }

                                    treeNode1.Tag = itemd.Value;
                                }
                                treeNode.Expand();
                            }
                            foreach (var item in dataObj.ListCamsData)
                            {
                                form.HWi.OneResIamge = item.Value.ResuOBj()[0];
                                form.HWi.SetImaage(item.Value.GetImagePlus());
                                //HWi.ShowImage();
                                form.HWi.ShowObj();
                                break;
                            }
                        }
                    }
                    if (DebugCompiler.EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.运行中)
                    {
                        return;
                    }
                    HalconRun halcon = Vision.GetRunNameVision();
                    if (halcon != null)
                    {
                        HWindowControl controlH = halcon.GetHWindow().GetNmaeWindowControl("Image." + dataObj.TrayLocation);
                        if (controlH != null)
                        {
                            //halcon.HobjClear();
                            //OneResultOBj halconResult = controlH.Tag as OneResultOBj;
                            //halcon.ShowImage(halconResult.Image);
                            //halcon.SetResultOBj(halconResult);
                            //halcon.GetOneImageR().ShowAll(halcon.hWindowHalcon());
                        }
                    }
                }
                catch (Exception)
                {
                }
        
        }
        private void TrayDataForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            this.WindowState = FormWindowState.Minimized;
            e.Cancel = true;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void TrayDataForm_Load(object sender, EventArgs e)
        {
            try
            {
             
            }
            catch (Exception)
            {

            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                TreeNode CurrentNode = e.Node;
                if (CurrentNode != null)
                {
                    dataGridView2.Dock = DockStyle.Fill;
                    if (CurrentNode.Tag is OneCamData)
                    {
                        dataGridView2.Rows.Clear();
                        OneCamData oneCamData = CurrentNode.Tag as OneCamData;
                        int index = 0;
                        foreach (var itemd in oneCamData.NGObj.DicOnes)
                        {
                            DataMinMax da = itemd.Value.oneRObjs[0].dataMinMax;
                            if (da != null)
                            {
                                if (da.Reference_Name.Count == 0)
                                {
                                    continue;
                                }
                                dataGridView2.Rows.Add(da.Reference_Name.Count);
                                for (int i = 0; i < da.Reference_Name.Count; i++)
                                {
                                    dataGridView2.Rows[index].Cells[0].Value = itemd.Key + "." + da.Reference_Name[i];
                                    if (da.ValueStrs.Count > i)
                                    {
                                        dataGridView2.Rows[index].Cells[1].Value = da.ValueStrs[i];
                                    }
                                    dataGridView2.Rows[index].Cells[2].Value = da.Reference_ValueMin[i];
                                    dataGridView2.Rows[index].Cells[3].Value = da.Reference_ValueMax[i];
                                    index++;
                                }
                                if (da.ValueStrs.Count > dataGridView2.Rows.Count)
                                {
                                    dataGridView2.Rows.Add(da.ValueStrs.Count - da.Reference_Name.Count);
                                    for (int i = da.Reference_Name.Count; i < da.ValueStrs.Count; i++)
                                    {
                                        dataGridView2.Rows[index].Cells[1].Value = da.ValueStrs[i];
                                        index++;
                                    }
                                }
                            }
                            dataGridView2.BringToFront();
                            dataGridView2.Show();
                        }
                    }
                    else if (CurrentNode.Tag is OneComponent)
                    {
                        OneComponent oneCamData = CurrentNode.Tag as OneComponent;
                        dataGridView2.Visible = true;
                        dataGridView2.Location = new Point(150, 100);
                        dataGridView2.Dock = DockStyle.Fill;
                        DataMinMax da = oneCamData.oneRObjs[0].dataMinMax;
                        if (da != null)
                        {
                            if (da.Reference_Name.Count == 0)
                            {
                                return;
                            }
                            dataGridView2.Rows.Add(da.Reference_Name.Count);
                            for (int i = 0; i < da.Reference_Name.Count; i++)
                            {
                                dataGridView2.Rows[i].Cells[0].Value = da.Reference_Name[i];
                                if (da.ValueStrs.Count > i)
                                {
                                    dataGridView2.Rows[i].Cells[1].Value = da.ValueStrs[i];
                                }
                                dataGridView2.Rows[i].Cells[2].Value = da.Reference_ValueMin[i];
                                dataGridView2.Rows[i].Cells[3].Value = da.Reference_ValueMax[i];
                            }
                            if (da.ValueStrs.Count > dataGridView2.Rows.Count)
                            {
                                dataGridView2.Rows.Add(da.ValueStrs.Count - da.Reference_Name.Count);
                                for (int i = da.Reference_Name.Count; i < da.ValueStrs.Count; i++)
                                {
                                    dataGridView2.Rows[i].Cells[1].Value = da.ValueStrs[i];
                                }
                            }
                        }
                        dataGridView2.BringToFront();
                        dataGridView2.Show();
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                TreeNode CurrentNode = e.Node;
                if (CurrentNode != null)
                {
                    dataGridView2.Dock = DockStyle.Fill;
                    if (CurrentNode.Tag is OneCamData)
                    {
                        dataGridView2.Rows.Clear();
                        OneCamData oneCamData = CurrentNode.Tag as OneCamData;
                        int index = 0;
                        foreach (var itemd in oneCamData.NGObj.DicOnes)
                        {
                            DataMinMax da = itemd.Value.oneRObjs[0].dataMinMax;
                            if (da != null)
                            {
                                if (da.Reference_Name.Count == 0)
                                {
                                    continue;
                                }
                                dataGridView2.Rows.Add(da.Reference_Name.Count);
                                for (int i = 0; i < da.Reference_Name.Count; i++)
                                {
                                    dataGridView2.Rows[index].Cells[0].Value = itemd.Key + "." + da.Reference_Name[i];
                                    if (da.ValueStrs.Count > i)
                                    {
                                        dataGridView2.Rows[index].Cells[1].Value = da.ValueStrs[i];
                                    }
                                    dataGridView2.Rows[index].Cells[2].Value = da.Reference_ValueMin[i];
                                    dataGridView2.Rows[index].Cells[3].Value = da.Reference_ValueMax[i];
                                    index++;
                                }
                                if (da.ValueStrs.Count > dataGridView2.Rows.Count)
                                {
                                    dataGridView2.Rows.Add(da.ValueStrs.Count - da.Reference_Name.Count);
                                    for (int i = da.Reference_Name.Count; i < da.ValueStrs.Count; i++)
                                    {
                                        dataGridView2.Rows[index].Cells[1].Value = da.ValueStrs[i];
                                        index++;
                                    }
                                }
                            }
                            dataGridView2.BringToFront();
                            dataGridView2.Show();
                        }
                    }
                    else if (CurrentNode.Tag is OneComponent)
                    {
                        OneComponent oneCamData = CurrentNode.Tag as OneComponent;
                        dataGridView2.Visible = true;
                        dataGridView2.Location = new Point(150, 100);
                        dataGridView2.Dock = DockStyle.Fill;
                        DataMinMax da = oneCamData.oneRObjs[0].dataMinMax;
                        if (da != null)
                        {
                            if (da.Reference_Name.Count == 0)
                            {
                                return;
                            }
                            dataGridView2.Rows.Add(da.Reference_Name.Count);
                            for (int i = 0; i < da.Reference_Name.Count; i++)
                            {
                                dataGridView2.Rows[i].Cells[0].Value = da.Reference_Name[i];
                                if (da.ValueStrs.Count > i)
                                {
                                    dataGridView2.Rows[i].Cells[1].Value = da.ValueStrs[i];
                                }
                                dataGridView2.Rows[i].Cells[2].Value = da.Reference_ValueMin[i];
                                dataGridView2.Rows[i].Cells[3].Value = da.Reference_ValueMax[i];
                            }
                            if (da.ValueStrs.Count > dataGridView2.Rows.Count)
                            {
                                dataGridView2.Rows.Add(da.ValueStrs.Count - da.Reference_Name.Count);
                                for (int i = da.Reference_Name.Count; i < da.ValueStrs.Count; i++)
                                {
                                    dataGridView2.Rows[i].Cells[1].Value = da.ValueStrs[i];
                                }
                            }
                        }
                        dataGridView2.BringToFront();
                        dataGridView2.Show();
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
