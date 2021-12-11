using ErosSocket.DebugPLC.Robot;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;
using Vision2.ErosProjcetDLL.UI;
using Vision2.Project.DebugF;
using Vision2.Project.formula;
using Vision2.Project.Mes;
using Vision2.vision.RestVisionForm;
using static Vision2.vision.HalconRunFile.RunProgramFile.OneCompOBJs;

namespace Vision2.vision
{
    public partial class RestObjImage2 : Form
    {
        public RestObjImage2()
        {
            InitializeComponent();
            treeView3.Nodes.Clear();
            objImageFrom = this;
        }

        public int MaxNumber = 0;

        /// <summary>
        /// 产品集合
        /// </summary>
        private  Queue<OneDataVale> OneProductVS = new Queue<OneDataVale>();

        private HWindID HWindd;

        /// <summary>
        /// 单个产品
        /// </summary>
        private  OneDataVale OneProductV;

        /// <summary>
        /// 单个元件
        /// </summary>
        private OneComponent OneRObjT;



        public static RestObjImage2 RestObjImageFrom
        {
            get
            {
                if (objImageFrom == null || objImageFrom.IsDisposed)
                {
                    objImageFrom = new RestObjImage2();
                }
                return objImageFrom;
            }
            set
            {
                objImageFrom = value;
            }
        }

        private static RestObjImage2 objImageFrom;

        public bool IsMaintain;
        public void ShowImage(OneDataVale oneDataVale)
        {
            try
            {
                hWindowControl1.Focus();
                if (HWindd == null)
                {
                    HWindd = new HWindID();
                    HWindd.Initialize(hWindowControl1);
                }
                if (oneDataVale.Done)
                {
                    try
                    {
                        if (File.Exists(oneDataVale.KeyPamgr["path"]))
                        {
                            File.Delete(oneDataVale.KeyPamgr["path"]);
                        }
                    }
                    catch (Exception)
                    { }
                    return;
                }
                if (OneProductVS.Contains(oneDataVale))
                {
                    return;
                }
                OneProductVS.Enqueue(oneDataVale);
                UICon.SwitchToThisWindow(RestObjImage2.RestObjImageFrom.Handle, true);
                RestObjImage2.RestObjImageFrom.Show();
            }
            catch (Exception ex)
            {
            }
        }

        public void WeirtTray(TrayData trayImage)
        {
            try
            {
                for (int i = 0; i < trayImage.Count; i++)
                {
                    if (trayImage.GetDataVales()[i].NotNull)
                    {
                        string sn = trayImage.GetOneDataVale(i).PanelID;
                        if (sn == "")
                        {
                            sn = "SN";
                        }
                        string path = Vision.GetSaveImageInfo("上相机").SavePath + "\\" +  DateTime.Now.ToString("yyyy年M月d日") + "\\" + Product.ProductionName + "\\" +
                            sn + "\\" +"Data";
                        foreach (var item in trayImage.GetOneDataVale(i).ListCamsData)
                        {
                            item.Value.GetImagePlus().GenEmptyObj();
                        }
                        ProjectINI.ClassToJsonSavePath(trayImage.GetOneDataVale(i), path);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrForm.Show(ex);
            }      
         
        }

        public void WeirtAll(TrayData trayImage)
        {
            try
            {
                UserFormulaContrsl.WeirtAll(trayImage);
                WeirtTray(trayImage);
            }
            catch (Exception ex)
            {
                ErosProjcetDLL.Project.ErrForm.Show(ex);
            }
        }

        public  void Clser()
        {
            try
            {
                OneProductVS.Clear();
                //TrayImageTs.Clear();
            }
            catch (Exception)
            {
            }
        }

        private double dee;
        HObject imaget = new HObject();
      

        private void UpData(OneDataVale onePro)
        {

            try
            {
                for (int i = 0; i < OneProductVS.Count; i++)
                {
                    TreeNode[] treeNodesDevices = treeView3.Nodes.Find(OneProductVS.ToArray()[i].DeviceName, false);
                    TreeNode treeNodeDevices = null;
                    if (treeNodesDevices.Length==0)
                    {
                        treeNodeDevices= treeView3.Nodes.Add(OneProductVS.ToArray()[i].DeviceName);
                        treeNodeDevices.Name = OneProductVS.ToArray()[i].DeviceName;
                    }
                    else
                    {
                        treeNodeDevices = treeNodesDevices[0];
                    }
                    TreeNode[] treeNodes = treeNodeDevices.Nodes.Find(OneProductVS.ToArray()[i].PanelID, false);
                    if (treeNodes.Length == 0)
                    {
                        treeNodeDevices = treeNodeDevices.Nodes.Add(OneProductVS.ToArray()[i].PanelID);
                        treeNodeDevices.Name = OneProductVS.ToArray()[i].PanelID;
                        treeNodeDevices.Tag = OneProductVS.ToArray()[i];
                    }
                }
                treeView3.ExpandAll();
     
            }
            catch (Exception)
            {
            }
            if (onePro==null)
            {
                treeView1.Nodes.Clear();
                return;
            }
            if (onePro.OK)
            {/*.SetNumberValue(OneProductV.TrayLocation, OneProductV.OK);*/
                //TrayImage.GetITrayRobot().UpData();
                label1.Text = "OK";
                label1.BackColor = Color.Green;
            }
            else
            {
                label1.Text = "NG";
                label1.BackColor = Color.Red;
            }
            string PatText = "";
            dee = HWindd.HeigthImage / HWindd.WidthImage;
            hWindowControl1.Height = (int)(hWindowControl1.Width * dee);
            if (panel2.Height < hWindowControl1.Height)
            {
                hWindowControl1.Height = panel2.Height;
            }
            hWindowControl1.Dock = DockStyle.Top;
            textBox1.Text = onePro.PanelID;
            if (onePro != null)
            {
                onePro.UesrRest = true;
                button1.Enabled = true;
                HWindd.OneResIamge.GetNgOBJS(onePro.GetNGCompData());
                HWindd.SetImaage(onePro.GetNGImage());
            }
            PatText = "托盘号:" + onePro.TrayLocation + "." + onePro.GetNGCamName() + Environment.NewLine;
            if (Vision.Instance.RestT)
            {
                foreach (TreeNode item in treeView1.Nodes)
                {
                    if (item.Nodes.Count == 0)
                    {
                        continue;
                    }
                    foreach (TreeNode itemdt in item.Nodes)
                    {
                        if (itemdt.Tag is OneComponent)
                        {
                            OneComponent oneComponent = itemdt.Tag as OneComponent;
                            if (!oneComponent.Done)
                            {
                                OneRObjT = oneComponent;
                                itemdt.EnsureVisible();
                                //restOneComUserControl1.Location = new Point(itemdt.Bounds.X, itemdt.Bounds.Y + itemdt.Bounds.Height + 2);
                                restOneComUserControl1.Visible = true;
                                //restOneComUserControl1.BringToFront();
                                restOneComUserControl1.UpData(OneRObjT);
                                break;
                            }
                            else
                            {
                                if (oneComponent.aOK)
                                {
                                    if (itemdt.ImageIndex != 3)
                                    {
                                        itemdt.ImageIndex = 3;
                                    }
                                }
                                else
                                {
                                    itemdt.ImageIndex = 4;
                                }
                            }
                        }
                    }
                    if (OneRObjT!=null)
                    {
                        if (!OneRObjT.Done)
                        {
                            break;
                        }
                    }
               
                }
                if (!onePro.Done)
                {
                }
                else
                {
                    restOneComUserControl1.Visible = false;
                }
            }
            if (OneRObjT != null)
            {
                PatText += OneRObjT.ComponentID + "NG信息:" + OneRObjT.NGText + "\\" + onePro.NGNumber;
                if (OneRObjT.ComponentID != "")
                {
                    //PatText += ";位号:" + OneRObjT.ComponentID + Environment.NewLine;
                }
                restOneComUserControl1.UpData(OneRObjT);
            }
            label3.Text ="设备名:"+ onePro.DeviceName;
            label4.Text = PatText;
            //label3.Text = RecipeCompiler.GetSPC();
            try
            {
                if (Vision.Instance.RestT)
                {
                    if (onePro != null)
                    {
                        foreach (var item in onePro.GetNGCompData().DicOnes)
                        {
                            if (item.Value.Done)
                            {
                                continue;
                            }
                            HOperatorSet.GetImageSize(onePro.GetNGImage(), out HTuple wid, out HTuple hei);
                            hWindowControl3.HalconWindow.ClearWindow();
                            hWindowControl4.HalconWindow.ClearWindow();
                            try
                            {
                                HOperatorSet.SetDraw(hWindowControl3.HalconWindow, "margin");
                                HOperatorSet.SetLineWidth(hWindowControl3.HalconWindow, Vision.Instance.LineWidth);
                                Vision.SetFont(hWindowControl3.HalconWindow);
                                HOperatorSet.SetColor(hWindowControl3.HalconWindow, "red");
                            }
                            catch (Exception) { }
                            imaget.Dispose();
                            //HOperatorSet.ReadImage(out HObject imaget, Vision.VisionPath + "Image\\" + OneRImage.LiyID + ".bmp");
                            bool isbde = false;
                            if (Directory.Exists(Vision.VisionPath + "Image\\"))
                            {
                                string[] images = Directory.GetFiles(Vision.VisionPath + "Image\\");
                                List<string> imageStr = new List<string>(images);
                        
                                for (int i = 0; i < images.Length; i++)
                                {
                                    if (images[i].StartsWith(Vision.VisionPath + "Image\\" + onePro.GetNGCamName() + "拼图"))
                                    {
                                        HOperatorSet.ReadImage(out imaget, images[i]);
                                        isbde = true;
                                        break;
                                    }
                                }
                            }
                            if (!isbde)
                            {
                                hWindowControl4.HalconWindow.DispText("未创建参考图片" + onePro.GetNGCamName(),
                                    "window", 0, 0, "red", new HTuple(), new HTuple());
                            }
                            //HOperatorSet.Union1(item.Value.NGROI, out HObject hObject1);
                            //hObject1 = Vision.XLD_To_Region(hObject1);

                            //HOperatorSet.SmallestRectangle1(hObject1, out HTuple row1, out HTuple col1, out HTuple row2, out HTuple clo2);
                            ////if (row1.Length != 0)
                            //{
                            //    HOperatorSet.AreaCenter(hObject1, out HTuple area, out HTuple row, out HTuple col);

                            //    HOperatorSet.GenContourPolygonXld(out HObject hObject, new HTuple(row, row), new HTuple(new HTuple(0), col1));
                            //    HOperatorSet.GenContourPolygonXld(out HObject hObject2, new HTuple(new HTuple(0), row1), new HTuple(col, col));
                            //    HOperatorSet.GenContourPolygonXld(out HObject hObject3, new HTuple(new HTuple(row), row), new HTuple(clo2, wid));
                            //    HOperatorSet.GenContourPolygonXld(out HObject hObject4, new HTuple(new HTuple(row2), hei), new HTuple(col, col));
                            //    HWindd.OneResIamge.SetCross(hObject.ConcatObj(hObject2).ConcatObj(hObject3).ConcatObj(hObject4));
                            //    double d = (double)wid / (double)hei;
                            //    hWindowControl3.HalconWindow.SetPart(row1 - (200 * 1), col1 - (200 * d), row2 + (200 * 1), clo2 + (200 * d));
                            //    hWindowControl4.HalconWindow.SetPart(row1 - (200 * 1), col1 - (200 * d), row2 + (200 * 1), clo2 + (200 * d));
                            //}
                            //else
                            //{
                
                            //    hObject1 = Vision.XLD_To_Region(item.Value.ROI);
                            //    HOperatorSet.SmallestRectangle1(hObject1, out row1, out col1, out row2, out clo2);
                            //    if (row1.Length != 0)
                            //    {
                            //        HOperatorSet.AreaCenter(hObject1, out HTuple area, out HTuple row, out HTuple col);
                            //        HOperatorSet.GenContourPolygonXld(out HObject hObject, new HTuple(row, row), new HTuple(new HTuple(0), col1));
                            //        HOperatorSet.GenContourPolygonXld(out HObject hObject2, new HTuple(new HTuple(0), row1), new HTuple(col, col));
                            //        HOperatorSet.GenContourPolygonXld(out HObject hObject3, new HTuple(new HTuple(row), row), new HTuple(clo2, wid));
                            //        HOperatorSet.GenContourPolygonXld(out HObject hObject4, new HTuple(new HTuple(row2), hei), new HTuple(col, col));
                            //        HWindd.OneResIamge.SetCross(hObject.ConcatObj(hObject2).ConcatObj(hObject3).ConcatObj(hObject4));
                            //        double d = (double)wid / (double)hei;
                            //        hWindowControl3.HalconWindow.SetPart(row1 - (200 * 1), col1 - (200 * d), row2 + (200 * 1), clo2 + (200 * d));
                            //        hWindowControl4.HalconWindow.SetPart(row1 - (200 * 1), col1 - (200 * d), row2 + (200 * 1), clo2 + (200 * d));
                            //    }
                            //}
                            HWindd.OneResIamge.SetObjCross(item.Value.NGROI, out HTuple row1, out HTuple col1, out HTuple row2, out HTuple col2);
                            if (row1.Length == 0)
                            {
                                HWindd.OneResIamge.SetObjCross(item.Value.ROI, out row1, out col1, out row2, out col2);

                            }
                            if (row1.Length != 0)
                            {
                                hWindowControl3.HalconWindow.SetPart(row1, col1, row2, col2);
                                hWindowControl4.HalconWindow.SetPart(row1, col1, row2, col2);
                            }
                            HOperatorSet.Union1(item.Value.NGROI, out HObject hObject1);
                            hWindowControl3.HalconWindow.DispObj(onePro.GetNGImage());
                            hWindowControl4.HalconWindow.DispObj(imaget);
                            if (item.Value.oneRObjs.Count >= 0)
                            {
                                HWindd.OneResIamge.Massage = new HTuple();
                                List<string> vstd = item.Value.oneRObjs[0].dataMinMax.GetStrTextNG();
                                HTuple hTuple2 = new HTuple(vstd.ToArray());
                                HWindd.AddMeassge(hTuple2);
                                List<string> vs = item.Value.oneRObjs[0].dataMinMax.GetStrNG();

                                HTuple hTuple = new HTuple(vs.ToArray());

                                HWindd.AddMeassge(hTuple);
                            }
                            HOperatorSet.DilationCircle(hObject1, out HObject hObject5, 50);
                            HOperatorSet.AreaCenter(item.Value.NGROI, out HTuple areas, out HTuple rows, out HTuple colus);
                            hWindowControl3.HalconWindow.DispObj(hObject5);
                            hWindowControl3.HalconWindow.DispText(item.Value.NGText + "{" + item.Value.NGText + "}", "window", 0, 0, "red", new HTuple(), new HTuple());
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               ErrForm.Show(ex, "复判显示");
            }
            HWindd.ShowImage();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (OneProductV==null)
                {
                    button1.Enabled = false;
                    return;
                }
                if (OneProductV.PanelID == "")
                {
                    textBox1.Focus();
                    textBox1.ReadOnly = false;
                    MessageBox.Show("SN为空,请输入SN");
                    return;
                }
                if (!button1.Enabled)
                {
                    return;
                }
                button1.Enabled = false;
                hWindowControl1.Focus();
                if (OneProductV.OK)
                {
                    label1.Text = "OK";
                    label1.BackColor = Color.Green;
                }
                else
                {
                    label1.Text = "NG";
                    label1.BackColor = Color.Red;
                }
                OneProductV.Done = true;
                treeView1.Nodes.Clear();
                treeView2.Nodes.Clear();
                  string path=  OneProductV.KeyPamgr["path"]+ "\\Temp\\ResetImageData\\" + OneProductV.PanelID + ".txt";
                 if (!File.Exists(path))
                  {
                    ProjectINI.ClassToJsonSave(OneProductV,path);
                   if (File.Exists(OneProductV.KeyPamgr["path"] + "\\Temp\\ResetImage\\Time\\" + OneProductV.PanelID + ".txt"))
                   {
                       File.Delete(OneProductV.KeyPamgr["path"] + "\\Temp\\ResetImage\\Time\\" + OneProductV.PanelID + ".txt");
                    }
                 }
              
                if (OneProductV != null)
                {
                    TreeNode[] treeNodesDevices = treeView3.Nodes.Find(OneProductV.DeviceName, false);
                    if (treeNodesDevices.Length != 0)
                    {
                        treeNodesDevices[0].Nodes.RemoveByKey(OneProductV.PanelID);
                    }
                }
                OneProductVS.Dequeue();
                restOneComUserControl1.Visible = false;
                OneProductV.Dispose();
                OneProductV = null;
                GC.Collect();
            }
            catch (Exception ex) {
                ErrForm.Show(ex, "复判提交");
            }
        }
        /// <summary>
        /// 更新复判托盘
        /// </summary>
        public void DequeueData()
        {
           this.Invoke(new Action(() =>
                {
                    try
                    {
                        panel3.Visible = Vision.Instance.RestT;
 
                        label1.Text = "NG";
                        label1.BackColor = Color.Red;
                        if (OneProductV != null)
                        {
                            if (OneProductV.GetNGImage() != null)
                            {
                                HWindd.SetImaage(OneProductV.GetNGImage());
                                //HWindd.OneResIamge = halconResult;
                            }
                            UpData(OneProductV);
                        }
                        UICon.SwitchToThisWindow(RestObjImage2.RestObjImageFrom.Handle, true);
                        RestObjImage2.RestObjImageFrom.Show();
                    }
                    catch (Exception ex)
                    {
                        ErosProjcetDLL.Project.AlarmText.AddTextNewLine("复判窗口:" + ex.StackTrace, Color.Red);
                    }
                }));
        }
        int number = 0;
        private void RestObjImage_Load(object sender, EventArgs e)
        {
            try
            {
                treeView1.Nodes.Clear();
                this.restOneComUserControl1.EventShowObj += RestOneComUserControl1_EventShowObj;
                Thread thread = new Thread(() => {
                    while (!this.IsDisposed)
                    {
                        try
                        {
                            try
                            {
                                foreach (var item in ProjectINI.In.DicNameRunFacility)
                                {
                                    string PATH = item + "\\Temp\\ResetImage";
                                    if (Directory.Exists(PATH))
                                    {
                                        string DTA = ProjectINI.TempPath + "\\ResetImage\\";
                                        string[] imagef = Directory.GetFiles(PATH);
                                        for (int i = 0; i < imagef.Length; i++)
                                        {
                                            if (Path.GetFileNameWithoutExtension(imagef[i]) == "")
                                            {
                                                File.Delete(imagef[i]);
                                                continue;
                                            }
                                            if (File.Exists(imagef[i]))
                                            {
                                                string NAME = Path.GetFileNameWithoutExtension(imagef[i]);
                                                TreeNode[] treeNodes = treeView3.Nodes.Find(NAME, true);
                                                if (treeNodes.Length == 0)
                                                {
                                                    Thread.Sleep(10);
                                                    if (ProjectINI.ReadPathJsonToCalssEX(imagef[i], out OneDataVale OneP))
                                                    {
                                                        number++;
                                                        if (OneP.Done)
                                                        {
                                                            continue;
                                                        }
                                                        if (!OneP.KeyPamgr.ContainsKey("path"))
                                                        {
                                                            OneP.KeyPamgr.Add("path", item);
                                                        }
                                                        OneP.KeyPamgr["path"] = item;
                                                        Directory.CreateDirectory(PATH + "\\Time\\");
                                                        if (File.Exists(PATH + "\\Time\\" + Path.GetFileName(imagef[i])))
                                                        {
                                                            File.Delete(PATH + "\\Time\\" + Path.GetFileName(imagef[i]));
                                                        }
                                                        File.Move(imagef[i], PATH + "\\Time\\" + Path.GetFileName(imagef[i]));
                                                        if (OneP.ImagePaht!="")
                                                        {
                                                            string image = OneP.ImagePaht;
                                                            if (item.StartsWith(@"\\"))
                                                            {
                                                                string[] data = item.Split('\\');
                                                                image = @"\\"+ data[2]+"\\"+ OneP.ImagePaht.Substring(0,1)+ OneP.ImagePaht.Remove(0,2);
                                                            }
                                                            if (Directory.Exists(image))
                                                            {
                                                               string[] imagephtus= Directory.GetFiles(image);
                                                                foreach (var itemTd in OneP.ListCamsData)
                                                                {   string imagePt = itemTd.Value.ImagePaht;
                                                                    if (imagePt == "")
                                                                    {
                                                                        for (int j = imagephtus.Length - 1; j > 0; j--)
                                                                        {
                                                                            if (imagephtus[j].Contains(itemTd.Key)&& imagephtus[j].Contains("拼图"))
                                                                            {
                                                                                if (!OneP.KeyPamgr.ContainsKey(itemTd.Key+ "pathImage"))
                                                                                {
                                                                                    OneP.KeyPamgr.Add(itemTd.Key + "pathImage", imagephtus[j]);
                                                                                }
                                                                                OneP.KeyPamgr[itemTd.Key + "pathImage"] = imagephtus[j];
                                                                                break;
                                                                            }
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        for (int j = imagephtus.Length - 1; j > 0; j--)
                                                                        {
                                                                            if (imagephtus[j].EndsWith(imagePt.Remove(0, 10)))
                                                                            {
                                                                                if (!OneP.KeyPamgr.ContainsKey(itemTd.Key + "pathImage"))
                                                                                {
                                                                                    OneP.KeyPamgr.Add(itemTd.Key + "pathImage", imagephtus[j]);
                                                                                }
                                                                                OneP.KeyPamgr[itemTd.Key + "pathImage"] = imagephtus[j];
                                                                                break;
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        this.Invoke(new Action(() =>
                                                        {
                                                            try
                                                            {
                                                                  toolStripLabel1.Text = "计数"+number;
                                                                    TreeNode[] treeNodesDevices = treeView3.Nodes.Find(OneP.DeviceName, false);
                                                                    TreeNode treeNodeDevices = null;
                                                                    if (treeNodesDevices.Length == 0)
                                                                    {
                                                                        treeNodeDevices = treeView3.Nodes.Add(OneP.DeviceName);
                                                                        treeNodeDevices.Name = OneP.DeviceName;
                                                                    }
                                                                    else
                                                                    {
                                                                        treeNodeDevices = treeNodesDevices[0];
                                                                    }
                                                                    TreeNode[] treeNodes = treeNodeDevices.Nodes.Find(OneP.PanelID, false);
                                                                    if (treeNodes.Length == 0)
                                                                    {
                                                                        treeNodeDevices = treeNodeDevices.Nodes.Add(OneP.PanelID);
                                                                        treeNodeDevices.Name = OneP.PanelID;
                                                                        treeNodeDevices.Tag = OneP;
                                                                    }
                                                                treeView3.ExpandAll();
                                                            }
                                                            catch (Exception ex)
                                                            {
                                                            }
                                                        }));
                                                        RestObjImage2.RestObjImageFrom.ShowImage(OneP);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                               ErrForm.Show(ex,"读取图像");
                            }
                            if (OneProductV==null)
                                {
                                    if (OneProductVS.Count!=0)
                                    {
                                        OneProductV = OneProductVS.Peek();
                                        this.Invoke(new Action(() => {
                                            treeView1.Nodes.Clear();
                                            treeView2.Nodes.Clear();
                                            foreach (var itemdt in OneProductV.ListCamsData)
                                            {
                                                if (OneProductV.KeyPamgr.ContainsKey(itemdt.Key + "pathImage"))
                                                {
                                                    HOperatorSet.ReadImage(out HObject hObject, OneProductV.KeyPamgr[itemdt.Key + "pathImage"]);
                                                    itemdt.Value.GetImagePlus(hObject);
                                                    TreeNode treeNode = treeView1.Nodes.Add(itemdt.Key);
                                                    treeNode.Tag = itemdt.Value;
                                                    TreeNode treeNodeOK = treeView2.Nodes.Add(itemdt.Key);
                                                    treeNodeOK.Tag = itemdt.Value;
                                                    foreach (var itemdte in itemdt.Value.AllCompObjs.DicOnes)
                                                    {
                                                        TreeNode treeNode1 = treeNodeOK.Nodes.Add(itemdte.Key);
                                                        treeNode1.Tag = itemdte.Value;
                                                        treeNode1.ImageIndex = 6;
                                                    }
                                                    foreach (var itemdte in itemdt.Value.NGObj.DicOnes)
                                                    {
                                                        if (!itemdte.Value.aOK)
                                                        {
                                                            TreeNode treeNode1 = treeNode.Nodes.Add(itemdte.Key);
                                                            treeNode1.Tag = itemdte.Value;
                                                            treeNode1.ImageIndex = 5;
                                                        }
                                                    }
                                                    treeNodeOK.Expand();
                                                    treeNode.Expand();
                                                }
                                                else
                                                {
                                                    AlarmListBoxt.AddAlarmText( new AlarmText.alarmStruct(){ Name = itemdt.Key+"图片读取",
                                                   Text =  "图片读取失败"});  
                                                } 
                                            }})); 
                                    UpData(OneProductV);
                                }
                                }
                        }
                        catch (Exception ex)
                        {
                            ErrForm.Show(ex,"刷新图像");
                        }
                        Thread.Sleep(100);
                    }
                });
                thread.Priority = ThreadPriority.Highest;
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception ex)
            {
                ErrForm.Show(ex,"执行失败,请关闭重新打开");
            }
        }

        private void RestOneComUserControl1_EventShowObj(int objName)
        {
            try
            {
                UpData(OneProductV);
            }
            catch (Exception)
            {
            }
        }

        private void RestObjImage_FormClosing(object sender, FormClosingEventArgs e)
        {
      
        }

        private void RestObjImage_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (OneProductV != null)
                {
                    if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Back)
                    {
                        this.hWindowControl1.Focus();
                        if (OneProductV.Done)
                        {
                            if (OneProductV.OK)
                            {
                                if (OneProductV.AutoOK != OneProductV.OK)
                                {
                                    RecipeCompiler.AddRlsNumber();
                                    //label3.Text = RecipeCompiler.Instance.GetSPC();
                                }
                            }
                            button1_Click(null, null);

                            treeView1.Nodes.Clear();
                            treeView2.Nodes.Clear();
                            return;
                        }
                        if (e.KeyCode == Keys.Space)
                        {
                            OneProductV.SetOKBit();
                            if (OneProductV.Done)
                            {
                                button1_Click(null, null);
                                return;
                            }
                            if (Vision.Instance.RestT)
                            {
                                restOneComUserControl1.SetRest(-1);
                            }
                            else
                            {
                                OneProductV.Done = true;
                                OneProductV.OK = true;
                            }
                        }
                        else
                        {
                            if (OneProductV.Done)
                            {
                                button1_Click(null, null);
                                return;
                            }
                            if (Vision.Instance.RestT)
                            {
                                restOneComUserControl1.SetRest(0);
                            }
                            else
                            {
                                OneProductV.Done = true;
                                OneProductV.OK = false;
                            }
                        }
                    }
                    else if (e.KeyCode == Keys.D1)
                    {
                        restOneComUserControl1.SetRest(1);
                    }
                    else if (e.KeyCode == Keys.D2)
                    {
                        restOneComUserControl1.SetRest(2);
                    }
                    else if (e.KeyCode == Keys.D3)
                    {
                        restOneComUserControl1.SetRest(3);
                    }
                    else if (e.KeyCode == Keys.D4)
                    {
                        restOneComUserControl1.SetRest(4);
                    }
                    else if (e.KeyCode == Keys.D5)
                    {
                        restOneComUserControl1.SetRest(5);
                    }
                    else if (e.KeyCode == Keys.D6)
                    {
                        restOneComUserControl1.SetRest(6);
                    }
                    else if (e.KeyCode == Keys.D7)
                    {
                        restOneComUserControl1.SetRest(7);
                    }
                    else
                    {
                        return;
                    }
                    UpData(OneProductV);
                }
            }
            catch (Exception ex)
            {
                ErosProjcetDLL.Project.ErrForm.Show(ex, "复判按键");
            }
        }

        private void RestObjImage_Resize(object sender, EventArgs e)
        {
            try
            {
                if (HWindd != null)
                {
                    HWindd.ShowImage();
                }
            }
            catch (Exception)
            { }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int i = 0;

                UpData(OneProductV);
                HWindd.ShowImage();
            }
            catch (Exception ex)
            {
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                button1.Enabled = true;
                timer1.Stop();
                if (OneProductVS.Count == 0)
                {
                    OneProductV = null;
                }
                else
                {
                    DequeueData();
                }
            }
            catch (Exception)
            {
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void tsButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Directory.CreateDirectory(Application.StartupPath + "\\截取屏幕\\" +  DateTime.Now.ToString("yyyy年M月d日"));
                Bitmap bitmap = UICon.GetScreenCapture();
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.InitialDirectory = Application.StartupPath + @"\截取屏幕\";
                saveFileDialog.Filter = "图像|*.jpg";
                string timeStr = DateTime.Now.ToString("HH时mm分ss秒");
                saveFileDialog.FileName = timeStr;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    bitmap.Save(saveFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                ErosProjcetDLL.Project.ErrForm.Show(ex);
            }
        }

        //需添加using System.Runtime.InteropServices;
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        private void toolStrip1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture(); //释放鼠标捕捉
                                  //发送左键点击的消息至该窗体(标题栏)
                SendMessage(Handle, 0xA1, 0x02, 0);
            }
            if (e.Clicks == 2)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (this.WindowState != FormWindowState.Maximized)
                    {
                        SetFormMax(this);
                    }
                    else
                    {
                        this.WindowState = FormWindowState.Minimized;
                    }
                }
            }
        }

        public virtual void SetFormMax(Form frm)
        {
            frm.Top = 0;
            frm.Left = 0;
            frm.Width = Screen.PrimaryScreen.WorkingArea.Width;
            frm.Height = Screen.PrimaryScreen.WorkingArea.Height;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                OneProductV.PanelID = textBox1.Text.Trim();
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("取消提交将不处理当前产品！", "是否取消提交？", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    timer1.Start();
                    hWindowControl1.Focus();
                    if (OneProductVS.Count==0)
                    {
                        treeView3.Nodes.Clear();
                        return;
                    }
                    OneProductVS.Dequeue();
                    if (OneProductV!=null)
                    {
                        TreeNode[] treeNodesDevices = treeView3.Nodes.Find(OneProductV.DeviceName, false);
                        if (treeNodesDevices.Length != 0)
                        {
                            treeNodesDevices[0].Nodes.RemoveByKey(OneProductV.PanelID);
                        }
                    }
                    OneProductV.Dispose();
                     OneProductV = null;
                    GC.Collect();
                    treeView1.Nodes.Clear();
                    treeView2.Nodes.Clear();
                    label4.Text = "";
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
                UpData(OneProductV);
                HWindd.ShowImage();

            }
            catch (Exception ex)
            {
                ErosProjcetDLL.Project.ErrForm.Show(ex);
            }
        }

        private OneNGDataMinMaxControl oneNGDataMinMaxControl;

        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                OneComponent one = e.Node.Tag as OneComponent;
                if (oneNGDataMinMaxControl == null)
                {
                    oneNGDataMinMaxControl = new OneNGDataMinMaxControl();
                    panel5.Controls.Add(oneNGDataMinMaxControl);
                }
                panel5.AutoScroll = true;
                oneNGDataMinMaxControl.Dock = DockStyle.Top;
                oneNGDataMinMaxControl.UpDataMax(one.oneRObjs[0].dataMinMax);
            }
            catch (Exception ex)
            {
                ErosProjcetDLL.Project.ErrForm.Show(ex);
   
            }
        }

        private void RestObjImage_VisibleChanged(object sender, EventArgs e)
        {
            try
            {
                if (Vision.Instance.RestWait)
                {
                    if (this.Visible)
                    {
                        Project.DebugF.IO.DODIAxis.RresOK = false;
                        Project.DebugF.IO.DODIAxis.RresWait = true;
                    }
                    else
                    {
                        Project.DebugF.IO.DODIAxis.RresOK = false;
                        Project.DebugF.IO.DODIAxis.RresWait = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ErosProjcetDLL.Project.ErrForm.Show(ex);
    
            }
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
        
        }

        private void treeView3_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                propertyGrid1.SelectedObject = e.Node.Tag;
                    treeView1.Nodes.Clear();
                    treeView2.Nodes.Clear();
                    foreach (var itemdt in OneProductV.ListCamsData)
                    {
                        TreeNode treeNode = treeView1.Nodes.Add(itemdt.Key);
                        treeNode.Tag = itemdt.Value;
                        TreeNode treeNodeOK = treeView2.Nodes.Add(itemdt.Key);
                        treeNodeOK.Tag = itemdt.Value;
                        foreach (var itemdte in itemdt.Value.AllCompObjs.DicOnes)
                        {
                            TreeNode treeNode1 = treeNodeOK.Nodes.Add(itemdte.Key);
                            treeNode1.Tag = itemdte.Value;
                            treeNode1.ImageIndex = 6;
                        }
                        foreach (var itemdte in itemdt.Value.NGObj.DicOnes)
                        {
                            if (!itemdte.Value.aOK)
                            {
                                TreeNode treeNode1 = treeNode.Nodes.Add(itemdte.Key);
                                treeNode1.Tag = itemdte.Value;
                                treeNode1.ImageIndex = 5;
                            }
                        }
                        treeNodeOK.Expand();
                        treeNode.Expand();
                    }
                UpData(OneProductV);
            }
            catch (Exception)
            {

            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
           
            this.Close();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            try
            {

                foreach (var item in ErosProjcetDLL.Project.ProjectINI.In.DicNameRunFacility)
                {

                }
            }
            catch (Exception)
            {

            }
        }
    }
}