using ErosSocket.DebugPLC.Robot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.UI;
using Vision2.Project.Mes;

namespace Vision2.vision.RestVisionForm
{
    public partial class AutoSNForm : Form
    {
        public AutoSNForm()
        {
            InitializeComponent();
        }
        HWindID HindID = new HWindID();

        /// <summary>
        /// 单个产品
        /// </summary>
        private OneDataVale OneProductV;
        /// <summary>
        /// 整盘
        /// </summary>
        private  TrayData TrayImage;
        private void AutoSNForm_Load(object sender, EventArgs e)
        {
            try
            {
                ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView1);
                HindID.Initialize(hWindowControl1);
                    treeView1.Nodes.Clear();
                TreeNode treeNode = treeView1.Nodes.Add("AVI");
                treeView1.ExpandAll();
                Thread thread = new Thread(() => {
                    try
                    {
                        while (!this.IsDisposed)
                        {
                            try
                            {
                                foreach (var item in ErosProjcetDLL.Project.ProjectINI.In.DicNameRunFacility)
                                    {
                                    string PATH = item + "\\Temp\\ResetImage\\";
                                    if (Directory.Exists(PATH))
                                    {
                                        string DTA = ErosProjcetDLL.Project.ProjectINI.TempPath + "\\ResetImage\\";
                                        string[] imagef = Directory.GetFiles(PATH);
                                        for (int i = 0; i < imagef.Length; i++)
                                        {
                                            if (File.Exists(imagef[i]))
                                            {
                                                string NAME = Path.GetFileNameWithoutExtension(imagef[i]);
                                                TreeNode[] treeNodes = treeNode.Nodes.Find(NAME, false);
                                                if (treeNodes.Length == 0)
                                                {
                                                    this.Invoke(new MethodInvoker(() => {
                                                        TreeNode treeNod = treeNode.Nodes.Add(NAME);
                                                        treeNod.Name = NAME;
                                                        if (imagef[i].EndsWith(".Tray"))
                                                        {
                                                            TrayImage = new TrayData();
                                                            if (ErosProjcetDLL.Project.ProjectINI.ReadPathJsonToCalssEX<TrayData>(imagef[i], out TrayImage))
                                                            {
                                                                treeNod.Tag = TrayImage;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (ErosProjcetDLL.Project.ProjectINI.ReadPathJsonToCalssEX(imagef[i], out OneProductV))
                                                            {
                                                                propertyGrid1.SelectedObject = treeNod.Tag = OneProductV;
                                                                //RestObjImage2.RestObjImageFrom.ShowImage(OneProductV);
                                                            }
                                                        }
                                                    }));
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            Thread.Sleep(200);
                        }
                    }
                    catch (Exception EX)
                    {
                    }
                });
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
  
                propertyGrid1.SelectedObject = e.Node.Tag;
                OneDataVale dataVale= propertyGrid1.SelectedObject as OneDataVale;
                if (dataVale != null)
                {
              
                    //OneProductV.GetNGImage();
                    HindID.SetImaage(dataVale.GetNGImage());
                    RestObjImage2.RestObjImageFrom.ShowImage(dataVale);
                }
                
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            UICon.SwitchToThisWindow(RestObjImage2.RestObjImageFrom.Handle, true);
            RestObjImage2.RestObjImageFrom.Show();
         
        }

        //private void UpData()
        //{
        //    string PatText = "";
        //    dee = HWindd.HeigthImage / HWindd.WidthImage;
        //    hWindowControl1.Height = (int)(hWindowControl1.Width * dee);
        //    if (panel2.Height < hWindowControl1.Height)
        //    {
        //        hWindowControl1.Height = panel2.Height;
        //    }
        //    hWindowControl1.Dock = DockStyle.Top;
        //    textBox1.Text = OneProductV.PanelID;
        //    if (OneProductV != null)
        //    {
        //        OneProductV.UesrRest = true;
        //        HWindd.OneResIamge.GetNgOBJS(OneProductV.GetNGCompData());
        //        HWindd.SetImaage(OneProductV.GetNGImage());
        //    }
        //    PatText = "托盘号:" + OneProductV.TrayLocation + "." + OneProductV.GetNGCamName() + Environment.NewLine;
        //    if (Vision.Instance.RestT)
        //    {
        //        foreach (TreeNode item in treeView1.Nodes)
        //        {
        //            if (item.Nodes.Count == 0)
        //            {
        //                continue;
        //            }
        //            foreach (TreeNode itemdt in item.Nodes)
        //            {
        //                if (itemdt.Tag is OneComponent)
        //                {
        //                    OneComponent oneComponent = itemdt.Tag as OneComponent;
        //                    if (!oneComponent.Done)
        //                    {
        //                        OneRObjT = oneComponent;
        //                        restOneComUserControl1.Location = new Point(itemdt.Bounds.X, itemdt.Bounds.Y + itemdt.Bounds.Height + 2);
        //                        restOneComUserControl1.Visible = true;
        //                        restOneComUserControl1.BringToFront();
        //                        restOneComUserControl1.UpData(OneRObjT);
        //                        break;
        //                    }
        //                    else
        //                    {
        //                        if (oneComponent.aOK)
        //                        {
        //                            if (itemdt.ImageIndex != 3)
        //                            {
        //                                itemdt.ImageIndex = 3;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            itemdt.ImageIndex = 4;
        //                        }
        //                    }
        //                }
        //            }
        //            if (!OneRObjT.Done)
        //            {
        //                break;
        //            }
        //        }
        //        if (!OneProductV.Done)
        //        {
        //        }
        //        else
        //        {
        //            restOneComUserControl1.Visible = false;
        //        }
        //    }
        //    if (OneRObjT != null)
        //    {
        //        PatText += OneRObjT.ComponentID + "NG信息:" + OneRObjT.NGText + "\\" + OneProductV.NGNumber;
        //        if (OneRObjT.ComponentID != "")
        //        {
        //            //PatText += ";位号:" + OneRObjT.ComponentID + Environment.NewLine;
        //        }
        //        restOneComUserControl1.UpData(OneRObjT);
        //    }
        //    label4.Text = PatText;
        //    label3.Text = RecipeCompiler.GetSPC();
        //    try
        //    {
        //        if (Vision.Instance.RestT)
        //        {
        //            if (OneProductV != null)
        //            {
        //                foreach (var item in OneProductV.GetNGCompData().DicOnes)
        //                {
        //                    if (item.Value.Done)
        //                    {
        //                        continue;
        //                    }
        //                    HOperatorSet.GetImageSize(OneProductV.GetNGImage(), out HTuple wid, out HTuple hei);
        //                    hWindowControl3.HalconWindow.ClearWindow();
        //                    hWindowControl4.HalconWindow.ClearWindow();
        //                    try
        //                    {
        //                        HOperatorSet.SetDraw(hWindowControl3.HalconWindow, "margin");
        //                        HOperatorSet.SetLineWidth(hWindowControl3.HalconWindow, Vision.Instance.LineWidth);
        //                        Vision.SetFont(hWindowControl3.HalconWindow);
        //                        HOperatorSet.SetColor(hWindowControl3.HalconWindow, "red");
        //                    }
        //                    catch (Exception) { }
        //                    HObject imaget = new HObject();
        //                    imaget.GenEmptyObj();
        //                    //HOperatorSet.ReadImage(out HObject imaget, Vision.VisionPath + "Image\\" + OneRImage.LiyID + ".bmp");
        //                    bool isbde = false;
        //                    if (Directory.Exists(Vision.VisionPath + "Image\\"))
        //                    {
        //                        string[] images = Directory.GetFiles(Vision.VisionPath + "Image\\");
        //                        List<string> imageStr = new List<string>(images);

        //                        for (int i = 0; i < images.Length; i++)
        //                        {
        //                            if (images[i].StartsWith(Vision.VisionPath + "Image\\" + OneProductV.GetNGCamName() + "拼图"))
        //                            {
        //                                HOperatorSet.ReadImage(out imaget, images[i]);
        //                                isbde = true;
        //                                break;
        //                            }
        //                        }
        //                    }
        //                    if (!isbde)
        //                    {
        //                        hWindowControl4.HalconWindow.DispText("未创建参考图片" + OneProductV.GetNGCamName(),
        //                            "window", 0, 0, "red", new HTuple(), new HTuple());
        //                    }
        //                    HOperatorSet.Union1(item.Value.NGROI, out HObject hObject1);
        //                    hObject1 = Vision.XLD_To_Region(hObject1);

        //                    HOperatorSet.SmallestRectangle1(hObject1, out HTuple row1, out HTuple col1, out HTuple row2, out HTuple clo2);
        //                    if (row1.Length != 0)
        //                    {
        //                        HOperatorSet.AreaCenter(hObject1, out HTuple area, out HTuple row, out HTuple col);

        //                        HOperatorSet.GenContourPolygonXld(out HObject hObject, new HTuple(row, row), new HTuple(new HTuple(0), col1));
        //                        HOperatorSet.GenContourPolygonXld(out HObject hObject2, new HTuple(new HTuple(0), row1), new HTuple(col, col));
        //                        HOperatorSet.GenContourPolygonXld(out HObject hObject3, new HTuple(new HTuple(row), row), new HTuple(clo2, wid));
        //                        HOperatorSet.GenContourPolygonXld(out HObject hObject4, new HTuple(new HTuple(row2), hei), new HTuple(col, col));
        //                        HWindd.OneResIamge.SetCross(hObject.ConcatObj(hObject2).ConcatObj(hObject3).ConcatObj(hObject4));
        //                        double d = (double)wid / (double)hei;
        //                        hWindowControl3.HalconWindow.SetPart(row1 - (200 * 1), col1 - (200 * d), row2 + (200 * 1), clo2 + (200 * d));
        //                        hWindowControl4.HalconWindow.SetPart(row1 - (200 * 1), col1 - (200 * d), row2 + (200 * 1), clo2 + (200 * d));
        //                    }
        //                    else
        //                    {
        //                        hObject1 = Vision.XLD_To_Region(item.Value.ROI);
        //                        HOperatorSet.SmallestRectangle1(hObject1, out row1, out col1, out row2, out clo2);
        //                        if (row1.Length != 0)
        //                        {
        //                            HOperatorSet.AreaCenter(hObject1, out HTuple area, out HTuple row, out HTuple col);
        //                            HOperatorSet.GenContourPolygonXld(out HObject hObject, new HTuple(row, row), new HTuple(new HTuple(0), col1));
        //                            HOperatorSet.GenContourPolygonXld(out HObject hObject2, new HTuple(new HTuple(0), row1), new HTuple(col, col));
        //                            HOperatorSet.GenContourPolygonXld(out HObject hObject3, new HTuple(new HTuple(row), row), new HTuple(clo2, wid));
        //                            HOperatorSet.GenContourPolygonXld(out HObject hObject4, new HTuple(new HTuple(row2), hei), new HTuple(col, col));
        //                            HWindd.OneResIamge.SetCross(hObject.ConcatObj(hObject2).ConcatObj(hObject3).ConcatObj(hObject4));
        //                            double d = (double)wid / (double)hei;
        //                            hWindowControl3.HalconWindow.SetPart(row1 - (200 * 1), col1 - (200 * d), row2 + (200 * 1), clo2 + (200 * d));
        //                            hWindowControl4.HalconWindow.SetPart(row1 - (200 * 1), col1 - (200 * d), row2 + (200 * 1), clo2 + (200 * d));
        //                        }
        //                    }
        //                    hWindowControl3.HalconWindow.DispObj(OneProductV.GetNGImage());
        //                    hWindowControl4.HalconWindow.DispObj(imaget);
        //                    if (item.Value.oneRObjs.Count >= 0)
        //                    {
        //                        HWindd.OneResIamge.Massage = new HTuple();
        //                        List<string> vstd = item.Value.oneRObjs[0].dataMinMax.GetStrTextNG();
        //                        HTuple hTuple2 = new HTuple(vstd.ToArray());
        //                        HWindd.AddMeassge(hTuple2);
        //                        List<string> vs = item.Value.oneRObjs[0].dataMinMax.GetStrNG();

        //                        HTuple hTuple = new HTuple(vs.ToArray());

        //                        HWindd.AddMeassge(hTuple);
        //                    }
        //                    HOperatorSet.DilationCircle(hObject1, out HObject hObject5, 50);
        //                    HOperatorSet.AreaCenter(item.Value.NGROI, out HTuple areas, out HTuple rows, out HTuple colus);
        //                    hWindowControl3.HalconWindow.DispObj(hObject5);
        //                    hWindowControl3.HalconWindow.DispText(item.Value.NGText + "{" + item.Value.NGText + "}", "window", 0, 0, "red", new HTuple(), new HTuple());
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("复判显示" + ex.Message);
        //    }
        //    HWindd.ShowImage();
        //}
    }
}
