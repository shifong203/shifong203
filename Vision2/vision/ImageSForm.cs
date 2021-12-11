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
using Vision2.ErosProjcetDLL.Project;

namespace Vision2.vision
{
    public partial class ImagesForm : Form
    {
        public ImagesForm()
        {
            InitializeComponent();
            ImagesFormThis = this;
        }
        static ImagesForm ImagesFormThis = new ImagesForm();


        private void ImagesForm_Load(object sender, EventArgs e)
        {
            try
            {


            }
            catch (Exception)
            {

            }
        }
        public static void ShowD()
        {

            if (ImagesFormThis == null || ImagesFormThis.IsDisposed)
            {
                ImagesFormThis = new ImagesForm();
            }
            ImagesFormThis.Show();
        }

        public static void AddImage(OneResultOBj oneResultOBj)
        {
            ImagesFormThis. UPOneImage(oneResultOBj);
        }
        public void UPOneImage(OneResultOBj oneImage)
        {
            try
            {

                if (ImagesFormThis == null || ImagesFormThis.IsDisposed)
                {
                    ImagesFormThis = new ImagesForm();
                }
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<OneResultOBj>(UPOneImage), oneImage);
                    return;
                }
                string names = oneImage.RunName.ToString()  + ":"+ oneImage.RunID.ToString();
                flowLayoutPanel1.Update();
               
                double sel = (double)oneImage.Width / (double)oneImage.Height;
                //flowLayoutPanel1.Width = (int)(halcon.Form2Heigth * sel);
                //flowLayoutPanel1.Dock = DockStyle.Right;
                flowLayoutPanel1.AutoScroll = true;
                //flowLayoutPanel1.AutoScrollMinSize = new Size(20, 1000);
                    HWindowControl hWindowControl2;
                    GroupBox groupBox2;
                    if (flowLayoutPanel1.Controls.ContainsKey(names))
                    {
                        groupBox2 = flowLayoutPanel1.Controls[names] as GroupBox;

                        hWindowControl2 = groupBox2.Controls[0] as HWindowControl;

                        hWindowControl2.Tag = oneImage;
                    }
                    else
                    {
                        groupBox2 = new GroupBox();
                        //groupBox2.Dock = DockStyle.Top;
                        hWindowControl2 = new HWindowControl();
                        hWindowControl2.HMouseDown += HWindowControl_HMouseDownD;
                        hWindowControl2.Tag = oneImage;
                        groupBox2.Text = groupBox2.Name = names;
  

                        hWindowControl2.Name = oneImage.RunID + ":";
                        groupBox2.Width = (int)(400 * sel);
                        groupBox2.Height = 400+20;
                        hWindowControl2.Dock = DockStyle.Fill;
                        groupBox2.Controls.Add(hWindowControl2);
                       flowLayoutPanel1.Controls.Add(groupBox2);
                    }
                    if (this.Visible)
                    {
                        Vision.SetFont(hWindowControl2.HalconWindow);
                        HOperatorSet.SetDraw(hWindowControl2.HalconWindow, "margin");
                        HSystem.SetSystem("flush_graphic", "false");
                        HOperatorSet.ClearWindow(hWindowControl2.HalconWindow);
                        HSystem.SetSystem("flush_graphic", "true");
                        if (Vision.IsObjectValided(oneImage.Image))
                        {
                            HOperatorSet.GetImageSize(oneImage.Image, out HTuple width, out HTuple heigth);
                            HOperatorSet.SetPart(hWindowControl2.HalconWindow, 0, 0, heigth, width);
                            HOperatorSet.DispObj(oneImage.Image, hWindowControl2.HalconWindow);
                        }
                        OneResultOBj halconResult = hWindowControl2.Tag as OneResultOBj;
                        if (halconResult != null)
                        {
                            halconResult.ShowAll(hWindowControl2.HalconWindow);
                        }
                    }
                    flowLayoutPanel1.Refresh();
                
                void HWindowControl_HMouseDownD(object sender, HMouseEventArgs e)
                {
                    try
                    {
                        HWindowControl hWindowControl = sender as HWindowControl;
                        OneResultOBj oneResultOBj = hWindowControl.Tag as OneResultOBj;
                        HalconRunFile.RunProgramFile.HalconRun halcon= oneResultOBj.GetHalcon();
                        //halcon.HobjClear();
                        halcon.SetResultOBj(oneResultOBj);

                        Project.MainForm1.SelectTab(halcon.Name);
                        //halcon.Focus();
                        halcon.GetOneImageR().ShowAll(halcon.hWindowHalcon());
                        if (e.Clicks == 2)
                        {
                            halcon.Image(halcon.GetOneImageR().Image.Clone());
                        }
                        //HalconResult halconResultT = (sender as HWindowControl).Tag as HalconResult;
                   
                        if (oneResultOBj != null)
                        {
                            if (Vision.IsObjectValided(oneResultOBj.Image))
                            {
                                HOperatorSet.SetDraw(hWindowControl.HalconWindow, "margin");
                                HOperatorSet.GetImageSize(oneResultOBj.Image, out HTuple width, out HTuple heigth);
                                HOperatorSet.SetPart(hWindowControl.HalconWindow, 0, 0, heigth, width);
                                HOperatorSet.DispObj(oneResultOBj.Image, hWindowControl.HalconWindow);
                            }
                            oneResultOBj.ShowAll(hWindowControl.HalconWindow);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                ErrForm.Show(ex, "图像界面");
            }
        }

        private void ImagesForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                try
                {
                    for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
                    {
                        GroupBox groupBox2 = flowLayoutPanel1.Controls[i] as GroupBox;
                        if (groupBox2!=null)
                        {
                            HWindowControl hWindowControl= groupBox2.Controls[0] as HWindowControl;
                            if (hWindowControl!=null)
                            {
                                try
                                {
                                    OneResultOBj oneResultOBj = hWindowControl.Tag as OneResultOBj;
                                    HalconRunFile.RunProgramFile.HalconRun halcon = oneResultOBj.GetHalcon();
                                    //halcon.HobjClear();
                                    halcon.SetResultOBj(oneResultOBj);
            
                                    halcon.GetOneImageR().ShowAll(halcon.hWindowHalcon());
                                    if (oneResultOBj != null)
                                    {
                                        if (Vision.IsObjectValided(oneResultOBj.Image))
                                        {
                                            HOperatorSet.SetDraw(hWindowControl.HalconWindow, "margin");
                                            HOperatorSet.GetImageSize(oneResultOBj.Image, out HTuple width, out HTuple heigth);
                                            HOperatorSet.SetPart(hWindowControl.HalconWindow, 0, 0, heigth, width);
                                            HOperatorSet.DispObj(oneResultOBj.Image, hWindowControl.HalconWindow);
                                        }
                                        oneResultOBj.ShowAll(hWindowControl.HalconWindow);
                                    }
                                }
                                catch (Exception)
                                {
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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }
    }
}
