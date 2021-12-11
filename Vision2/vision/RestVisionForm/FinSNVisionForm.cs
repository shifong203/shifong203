using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision2.Project.Mes;
using static Vision2.vision.HalconRunFile.RunProgramFile.OneCompOBJs;

namespace Vision2.vision.RestVisionForm
{
    public partial class SVisionForm : Form
    {
        public SVisionForm()
        {
            InitializeComponent();
        }
        HWindID HWindd = new HWindID();

        private void sVisionForm_Load(object sender, EventArgs e)
        {
            imaget.GenEmptyObj();
            HWindd.Initialize(hWindowControl1);
            treeView1.Nodes.Clear();
            hWindowControl2.Resize += HWindowControl2_Resize;
            hWindowControl2.HMouseWheel += HWindowControl2_HMouseWheel;
            hWindowControl1.HMouseUp += HWindowControl1_HMouseUp;
            hWindowControl1.HMouseMove += HWindowControl1_HMouseMove;
            hWindowControl1.HMouseDown += HWindowControl1_HMouseDown;
            hWindowControl1.HMouseWheel += HWindowControl2_HMouseWheel;
        }

    

        private void HWindowControl2_HMouseWheel(object sender, HMouseEventArgs e)
        {
            try
            {
                if (e.Delta>0)
                {
                    length1= length1+10;
                    UP(ROWy, colX, length1.TupleInt());
                }
                else
                {
                    length1= length1 -10;
                    UP(ROWy, colX, length1.TupleInt());
                }
              
            }
            catch (Exception)
            {
            }
        }

       
       string[] ImagePath;
        private void HWindowControl1_HMouseDown(object sender, HMouseEventArgs e)
        {
            try
            {
                isMoveT = true;
                UP((int)e.Y, (int)e.X, length1.TupleInt());
            }
            catch (Exception)
            {

            }
          
        }

        private void HWindowControl1_HMouseMove(object sender, HMouseEventArgs e)
        {
            try
            {
                if (isMoveT)
                {
                   UP((int)e.Y, (int)e.X, length1.TupleInt());
                }
            }
            catch (Exception)
            {
            }
        }
        bool isMoveT;
        int ROWy;
        int colX;
        public void UP(int row,int col,int length1)
        {
            try
            {
                ROWy = row;
                colX = col;
                hWindowControl2.HalconWindow.ClearWindow();
                hWindowControl3.HalconWindow.ClearWindow();
                HWindID.SetPart(hWindowControl2.HalconWindow, row, col, length1 * 2, ratio);
                HWindID.SetPart(hWindowControl3.HalconWindow, row, col, length1 * 2, ratio);
           
                hWindowControl3.HalconWindow.DispObj(imaget);
                hWindowControl2.HalconWindow.DispObj(ImageDT);
              //hWindowControl3.HalconWindow.DispObj(imaget);
            //if (item.Value.oneRObjs.Count >= 0)
            //{
            //    HWindd.OneResIamge.Massage = new HTuple();
            //    List<string> vstd = item.Value.oneRObjs[0].dataMinMax.GetStrTextNG();
            //    HTuple hTuple2 = new HTuple(vstd.ToArray());
            //    HWindd.AddMeassge(hTuple2);
            //    List<string> vs = item.Value.oneRObjs[0].dataMinMax.GetStrNG();
            //    HTuple hTuple = new HTuple(vs.ToArray());
            //    HWindd.AddMeassge(hTuple);
            //}
            //HOperatorSet.DilationCircle(hObject1, out HObject hObject5, 50);
            //HOperatorSet.AreaCenter(item.Value.NGROI, out HTuple areas, out HTuple rows, out HTuple colus);
            //hWindowControl2.HalconWindow.DispObj(hObject5);
            //hWindowControl2.HalconWindow.DispText(item.Value.NGText + "{" + item.Value.NGText + "}", "window", 0, 0, "red", new HTuple(), new HTuple());



             }
            catch (Exception ex)
            {
            }
        }
        private void HWindowControl1_HMouseUp(object sender, HMouseEventArgs e)
        {
            try
            {
                isMoveT = false;
          //      UP((int)e.Y, (int)e.X, 200);
            }
            catch (Exception)
            {

            }
        }

        private void HWindowControl2_Resize(object sender, EventArgs e)
        {
            UpData(OneProductV);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode==Keys.Enter)
                {
                    Finde(textBox1.Text); 
                }
            }
            catch (Exception)
            { 
            }
        }
 
        bool  FindImageSN(string sn,string pathImage)
        {
            try
            {
                if (!Directory.Exists(pathImage))
                {
                    return false;
                }
                string selePath = pathImage;
                //string dataTime = dateTimePicker1.Value.ToString("D");
                string dataTime = dateTimePicker1.Value.ToString("yyyy年M月d日");
                if (File.Exists(selePath + "\\" + dataTime))
                {
                    string[] paths = Directory.GetDirectories(selePath + "\\" + dataTime);
                }
                //for (int i = 0; i < paths.Length; i++)
                //{
                //    string[] pathst = Directory.GetDirectories(paths[i]);
                //    string[] imagePaths = Array.FindAll(pathst, delegate (string x) { return x.Contains(sn); });
                //    if (imagePaths.Length == 1)
                //    {
                //        pathst = Directory.GetFiles(imagePaths[0]);
                //        imagePaths = pathst;
                //        ImagePath = Array.FindAll(imagePaths, delegate (string x) { return !x.Contains("NG"); });
                //        return true;
                //    }
                //}
                int cont = 1;
                if (checkBox1.Checked)
                {
                    cont = 7;
                }
                for (int j = 0; j < cont; j++)
                {
                    if (finDone)
                    {
                        return false;
                    }
                    dataTime = dateTimePicker1.Value.AddDays((-1*j)).ToString("yyyy年M月d日");
                    if (Directory.Exists(selePath + "\\" + dataTime))
                    {
                       string[] paths = Directory.GetDirectories(selePath + "\\" + dataTime);
                       for (int i = 0; i < paths.Length; i++)
                    {
                        if (finDone)
                        {
                            return false;
                        }
                        string[] pathst = Directory.GetDirectories(paths[i]);
                        string[] imagePaths = Array.FindAll(pathst, delegate (string x) { return x.Contains(sn); });
                        if (imagePaths.Length == 1)
                        {
                            pathst = Directory.GetFiles(imagePaths[0]);
                            imagePaths = pathst;
                            ImagePath = Array.FindAll(imagePaths, delegate (string x) { return !x.Contains("NG"); });
                            return true;
                        }
                    }
                    }
                }
             
                //string[] paths = ErosProjcetDLL.Project.ProjectINI.GetFilesArrayPath(selePath + "\\" + dataTime);
 
            }
            catch (Exception ex)
            {
            }
            return false;
        }
        List<string> SNLsitD = new List<string>();
        string distName = "";
        bool finDone;
        public void Finde(string sn)
        {
            try
            {
                finDone = false;
                OneRObjT = null;
                ImagePath = null;
                textBox1.Text = "";
                string path = "";
                //hWindowControl1.Dock = DockStyle.Top;

                Task task = new Task(()=> {
                    if (FindImageSN(sn, Vision.Instance.StrigPathImages))
                    {
                        finDone = true;
                           distName = "机台1";
                    } 
           
                });
                Task task1 = new Task(() => {
                    if (FindImageSN(sn, Vision.Instance.StrigPathImages2))
                    {
                        finDone = true;
                        distName = "机台2";
                    } 
    
                });
                Task task2 = new Task(() => {
                    if (FindImageSN(sn, Vision.Instance.StrigPathImages3))
                    {
                        finDone = true;
                        distName = "机台3";
                    }

                });
                task.Start();
                task1.Start();
                task2.Start();

                task.Wait();
                task1.Wait();
                task2.Wait();
                label2.Text = sn;
                if (ImagePath == null)
                {
                    label2.Text += "不存在！";
                    return;
                }
                if (ImagePath.Length == 0)
                {
                    label2.Text += "不存在！";
                    return;
                }
                if (!SNLsitD.Contains(sn))
                {
                    SNLsitD.Add(sn);
                }

                if (SNLsitD.Count>20)
                {
                    SNLsitD.RemoveAt(0);
                }
                path = Array.Find(ImagePath, x => x.Contains("Data"));
                if (path!=null)
                {
                    ErosProjcetDLL.Project.ProjectINI.ReadPathJsonToCalss<OneDataVale>(path, out OneProductV);
                    foreach (var item in OneProductV.ListCamsData)
                    {
                        foreach (var itemt in ImagePath)
                        {
                            if (itemt.Contains(item.Key))
                            {
                                if (itemt.Contains("拼图"))
                                {
                                    HOperatorSet.ReadImage(out HObject hObject, itemt);
                                    item.Value.GetImagePlus(hObject);
                                    RaedCamImage(item.Key);
                                    break;
                                }
                            }
                        }
                    }
                    treeView1.Nodes.Clear();
                    treeView2.Nodes.Clear();
                    foreach (var itemdt in OneProductV.ListCamsData)
                    {
                        if (oneCamData==null)
                        {
                            itemdt.Value.RunVisionName = itemdt.Key;
                            oneCamData = itemdt.Value;
                        }
                        TreeNode treeNode = treeView1.Nodes.Add(itemdt.Key);
                        treeNode.Tag = itemdt.Value;
                        TreeNode treeNodeOK = treeView2.Nodes.Add(itemdt.Key);
                        treeNodeOK.Tag = itemdt.Value;
                        foreach (var itemdte in itemdt.Value.AllCompObjs.DicOnes)
                        {
                            TreeNode treeNode1 = treeNodeOK.Nodes.Add(itemdte.Key);
                            treeNode1.Tag = itemdte.Value;
                            treeNode1.ImageIndex = 3;
                        }
                        foreach (var itemdte in itemdt.Value.NGObj.DicOnes)
                        {
                            if (!itemdte.Value.aOK)
                            {
                                if (OneRObjT==null)
                                {
                                    OneRObjT = itemdte.Value;
                                }
                                TreeNode treeNode1 = treeNode.Nodes.Add(itemdte.Key);
                                treeNode1.Tag = itemdte.Value;
                                treeNode1.ImageIndex = 5;
                                HWindd.OneResIamge.GetNgOBJS().Add(itemdte.Value);
                            }
                            if (itemdte.Value.NGText=="")
                            {
                                if (OneRObjT == null)
                                {
                                    OneRObjT = itemdte.Value;
                                }
                                TreeNode treeNode1 = treeNode.Nodes.Add(itemdte.Key);
                                treeNode1.Tag = itemdte.Value;
                                treeNode1.ImageIndex = 2;
                                HWindd.OneResIamge.GetNgOBJS().Add(itemdte.Value);
                            }
                            //itemdte.Value.Done = false;
                        }
                        treeNodeOK.Expand();
                        treeNode.Expand();
                    }
                    UpData(OneProductV, OneRObjT);
                }
                else
                {
                    label2.Text += "Data.txt文件丢失！";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        double ratio ;

        /// <summary>
        /// 单个元件
        /// </summary>
        private OneComponent OneRObjT;

        OneDataVale OneProductV;
        /// <summary>
        /// 模板图片
        /// </summary>
        HObject imaget = new HObject();
        HObject ImageDT ;
    
        HTuple length1 = new HTuple(500);
        HObject ROI = new HObject();
        HObject NGErr = new HObject();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="OneProductV"></param>
        public void UpData(OneDataVale OneProductV, OneComponent oneC=null)
        {
            try
            {
                if (OneProductV==null)
                {
                    return;
                }
             
                if (OneProductV != null)
                {
                    HOperatorSet.GetImageSize(OneProductV.GetNGImage(), out HTuple width, out HTuple heigth);
                    if (width.Length==1)
                    {
                        HTuple det = width.D / heigth.D;
                        hWindowControl1.Dock = DockStyle.None;

                        double heig = (double)hWindowControl1.Width / det.D;
                        hWindowControl1.Height = (int)heig;
                        if (hWindowControl1.Height<400)
                        {
                            hWindowControl1.Height = 400;
                            hWindowControl1.Width = (int) (400 *det.D);
                        }
                    }
                }


                string PatText = OneProductV.PanelID+":";
                    
  

                if (OneProductV.OK)
                {
                    PatText += "Pass";
                }
                else
                {
                    PatText += "Fail";
                }
                PatText+= Environment.NewLine + "托盘号:" + OneProductV.TrayLocation + Environment.NewLine;
                PatText += "机台号:" + distName + Environment.NewLine;
                PatText += OneProductV.StrTime.ToString() + Environment.NewLine ;
     


                label2.Text = PatText;
                HWindd.OneResIamge.GetNgOBJS().DicOnes.Clear();
                HObject hObject6 = new HObject();
                hObject6.GenEmptyObj();
                HWindd.OneResIamge.SetCross(hObject6);
                hWindowControl2.HalconWindow.ClearWindow();
                hWindowControl3.HalconWindow.ClearWindow();
                HOperatorSet.GetImageSize(imaget, out HTuple wid, out HTuple hei);

                foreach (TreeNode item in treeView1.Nodes)
                    {
                        //if (item.Nodes.Count == 0)
                        //{
                        //    continue;
                        //}
                    if (oneCamData.RunVisionName!=item.Text)
                    {
                        continue;
                    }
                    if (oneC!=null)
                    {
                        HWindd.OneResIamge.GetNgOBJS().Add(oneC);
           
                        if (wid.Length == 1)
                        {
                            ratio = (double)wid / (double)hei;
                       
                        }
                        try
                        {
                            HOperatorSet.SetDraw(hWindowControl2.HalconWindow, "margin");
                            HOperatorSet.SetLineWidth(hWindowControl2.HalconWindow, Vision.Instance.LineWidth);
                            Vision.SetFont(hWindowControl2.HalconWindow);
                            HOperatorSet.SetColor(hWindowControl2.HalconWindow, "red");
                        }
                        catch (Exception) { }
        
                        HOperatorSet.Union1(oneC.NGROI, out HObject hObject1);
                        hObject1 = Vision.XLD_To_Region(hObject1);
                        HOperatorSet.SmallestRectangle1(hObject1, out HTuple row1, out HTuple col1, out HTuple row2, out HTuple clo2);
                        if (row1.Length != 0)
                        {
                            HOperatorSet.SmallestRectangle2(hObject1, out HTuple row, out HTuple col, out HTuple phi, out length1, out HTuple leng2);
                            HOperatorSet.GenContourPolygonXld(out HObject hObject, new HTuple(row, row), new HTuple(new HTuple(0), col1));
                            HOperatorSet.GenContourPolygonXld(out HObject hObject2, new HTuple(new HTuple(0), row1), new HTuple(col, col));
                            HOperatorSet.GenContourPolygonXld(out HObject hObject3, new HTuple(new HTuple(row), row), new HTuple(clo2, wid));
                            HOperatorSet.GenContourPolygonXld(out HObject hObject4, new HTuple(new HTuple(row2), hei), new HTuple(col, col));
                            HWindd.OneResIamge.SetCross(hObject.ConcatObj(hObject2).ConcatObj(hObject3).ConcatObj(hObject4));
                            //HWindID.SetPart(hWindowControl2.HalconWindow, row.TupleInt(), col.TupleInt(), length1.TupleInt() * 2, ratio);
                            //HWindID.SetPart(hWindowControl3.HalconWindow, row.TupleInt(), col.TupleInt(), length1.TupleInt() * 2, ratio);
                            UP(row.TupleInt(), col.TupleInt(), length1.TupleInt());
                        }
                        else
                        {
                            hObject1 = Vision.XLD_To_Region(oneC.ROI);
                            HOperatorSet.SmallestRectangle1(hObject1, out row1, out col1, out row2, out clo2);
                            if (row1.Length != 0)
                            {
                                HOperatorSet.SmallestRectangle2(hObject1, out HTuple row, out HTuple col, out HTuple phi, out length1, out HTuple leng2);
                                HOperatorSet.GenContourPolygonXld(out HObject hObject, new HTuple(row, row), new HTuple(new HTuple(0), col1));
                                HOperatorSet.GenContourPolygonXld(out HObject hObject2, new HTuple(new HTuple(0), row1), new HTuple(col, col));
                                HOperatorSet.GenContourPolygonXld(out HObject hObject3, new HTuple(new HTuple(row), row), new HTuple(clo2, wid));
                                HOperatorSet.GenContourPolygonXld(out HObject hObject4, new HTuple(new HTuple(row2), hei), new HTuple(col, col));
                                HWindd.OneResIamge.SetCross(hObject.ConcatObj(hObject2).ConcatObj(hObject3).ConcatObj(hObject4));
                                UP(row.TupleInt(), col.TupleInt(), length1.TupleInt());
                                //HWindID.SetPart(hWindowControl2.HalconWindow, row.TupleInt(), col.TupleInt(), length1.TupleInt() * 2, ratio);
                                //HWindID.SetPart(hWindowControl3.HalconWindow, row.TupleInt(), col.TupleInt(), length1.TupleInt() * 2, ratio);
                            }
                        }
                        NGErr = oneC.NGROI;
            
                        if (oneC.oneRObjs.Count > 0)
                        {
                            HWindd.OneResIamge.Massage = new HTuple();
                            List<string> vstd = oneC.oneRObjs[0].dataMinMax.GetStrTextNG();
                            HTuple hTuple2 = new HTuple(vstd.ToArray());
                            HWindd.AddMeassge(hTuple2);
                            List<string> vs = oneC.oneRObjs[0].dataMinMax.GetStrNG();
                            HTuple hTuple = new HTuple(vs.ToArray());
                            HWindd.AddMeassge(hTuple);
                            HOperatorSet.Union1(oneC.oneRObjs[0].ROI, out hObject1);
                            hObject1 = Vision.XLD_To_Region(hObject1);
                            HOperatorSet.SmallestRectangle1(hObject1, out row1, out col1, out row2, out clo2);
                            NGErr = oneC.oneRObjs[0].NGROI;
                            if (row1.Length != 0)
                            {
                                HOperatorSet.SmallestRectangle2(hObject1, out HTuple row, out HTuple col, out HTuple phi, out length1, out HTuple leng2);
                                HOperatorSet.GenContourPolygonXld(out HObject hObject, new HTuple(row, row), new HTuple(new HTuple(0), col1));
                                HOperatorSet.GenContourPolygonXld(out HObject hObject2, new HTuple(new HTuple(0), row1), new HTuple(col, col));
                                HOperatorSet.GenContourPolygonXld(out HObject hObject3, new HTuple(new HTuple(row), row), new HTuple(clo2, wid));
                                HOperatorSet.GenContourPolygonXld(out HObject hObject4, new HTuple(new HTuple(row2), hei), new HTuple(col, col));
                                HWindd.OneResIamge.SetCross(hObject.ConcatObj(hObject2).ConcatObj(hObject3).ConcatObj(hObject4));
                                //HWindID.SetPart(hWindowControl2.HalconWindow, row.TupleInt(), col.TupleInt(), length1.TupleInt() * 2, ratio);
                                //HWindID.SetPart(hWindowControl3.HalconWindow, row.TupleInt(), col.TupleInt(), length1.TupleInt() * 2, ratio);
                                UP(row.TupleInt(), col.TupleInt(), length1.TupleInt());
                            }
                            else
                            {
                                hObject1 = Vision.XLD_To_Region(oneC.oneRObjs[0].NGROI);
                              
                                HOperatorSet.SmallestRectangle1(hObject1, out row1, out col1, out row2, out clo2);
                                if (row1.Length != 0)
                                {
                                    HOperatorSet.SmallestRectangle2(hObject1, out HTuple row, out HTuple col, out HTuple phi, out length1, out HTuple leng2);
                                    HOperatorSet.GenContourPolygonXld(out HObject hObject, new HTuple(row, row), new HTuple(new HTuple(0), col1));
                                    HOperatorSet.GenContourPolygonXld(out HObject hObject2, new HTuple(new HTuple(0), row1), new HTuple(col, col));
                                    HOperatorSet.GenContourPolygonXld(out HObject hObject3, new HTuple(new HTuple(row), row), new HTuple(clo2, wid));
                                    HOperatorSet.GenContourPolygonXld(out HObject hObject4, new HTuple(new HTuple(row2), hei), new HTuple(col, col));
                                    HWindd.OneResIamge.SetCross(hObject.ConcatObj(hObject2).ConcatObj(hObject3).ConcatObj(hObject4));
                                    HWindID.SetPart(hWindowControl2.HalconWindow, row.TupleInt(), col.TupleInt(), length1.TupleInt() * 2, ratio);
                                    HWindID.SetPart(hWindowControl3.HalconWindow, row.TupleInt(), col.TupleInt(), length1.TupleInt() * 2, ratio);
                                 }
                            }
                        }
                        hWindowControl2.HalconWindow.DispObj(ImageDT);
                        hWindowControl3.HalconWindow.DispObj(imaget);
                   
                        HOperatorSet.DilationCircle(hObject1, out  ROI, 50);
                        HOperatorSet.AreaCenter(oneC.NGROI, out HTuple areas, out HTuple rows, out HTuple colus);
                        hWindowControl2.HalconWindow.SetColor(ColorResult.blue.ToString());
                        hWindowControl2.HalconWindow.DispObj(ROI);
                        hWindowControl2.HalconWindow.SetColor(ColorResult.red.ToString());
                        hWindowControl2.HalconWindow.DispObj(NGErr);
                  
                        hWindowControl2.HalconWindow.DispText(oneC.NGText + "{" + oneC.NGText + "}", "window", 0, 0, "red", new HTuple(), new HTuple());

                        }
                        else
                        {
                        length1 = hei/4;
                        //HWindID.SetPart(hWindowControl2.HalconWindow, 0, 0, hei, wid);
                        //HWindID.SetPart(hWindowControl3.HalconWindow,0, 0, hei, wid);
                        HWindID.SetPart(hWindowControl2.HalconWindow, hei.TupleInt()/2, wid.TupleInt()/2, length1.TupleInt() *2, ratio);
                        HWindID.SetPart(hWindowControl3.HalconWindow, hei.TupleInt() / 2, wid.TupleInt() / 2, length1.TupleInt()*2 , ratio);
                        hWindowControl2.HalconWindow.DispObj(ImageDT);
                        hWindowControl3.HalconWindow.DispObj(imaget);
                        foreach (TreeNode itemdt in item.Nodes)
                            {
                                if (itemdt.Tag is OneComponent)
                                {
                                    OneComponent oneComponent = itemdt.Tag as OneComponent;
           
                                    HWindd.OneResIamge.GetNgOBJS().Add(oneComponent);
                                    //if (!oneComponent.Done &&  (OneRObjT == null|| OneRObjT.Done))
                                    //{
                                    if ((OneRObjT == null || OneRObjT.Done))
                                    {
                                        OneRObjT = oneComponent;
                                        restOneComUserControl1.Location = new Point(itemdt.Bounds.X, itemdt.Bounds.Y + itemdt.Bounds.Height + 2);
                                        restOneComUserControl1.Visible = true;
                                        restOneComUserControl1.BringToFront();
                                        restOneComUserControl1.UpData(OneRObjT);
                                        //break;
                                    }
                                    else
                                    {
                                        if (oneComponent.aOK)
                                        {
                                            if (itemdt.ImageIndex != 3)
                                            {
                                                itemdt.ImageIndex = 2;
                                            }
                                        }
                                        else
                                        {
                                            itemdt.ImageIndex = 5;
                                        }
                                    }
                                }
                            }
                            if (OneRObjT != null)
                            {
                                if (!OneRObjT.Done)
                                {
                                    break;
                                }
                            }
                        }
                    }
                if (!OneProductV.Done)
                {
                }
                else
                {
                    restOneComUserControl1.Visible = false;
                }
                HWindd.ShowImage();
         
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void RaedCamImage(string camKey)
        {
            try
            {
                string[] images = Directory.GetFiles(Vision.VisionPath + "Image\\");
                List<string> imageStr = new List<string>(images);
                bool isbde = false;
                bool isModet = false;
                for (int i = 0; i < images.Length; i++)
                {
                    if (images[i].StartsWith(Vision.VisionPath + "Image\\" + camKey + "拼图"))
                    {
                        foreach (var itemt in ImagePath)
                        {
                            if (itemt.Contains(camKey))
                            {
                                if (itemt.Contains("拼图"))
                                {
                                    HOperatorSet.ReadImage(out ImageDT, itemt);
                                    HWindd.SetImaage(ImageDT);
                                    isModet = true;
                                    break;
                                }
                            }
                        }
                        if (!isModet)
                        {
                            foreach (var itemt in ImagePath)
                            {
                                if (itemt.Contains(camKey))
                                {
                                        HOperatorSet.ReadImage(out ImageDT, itemt);
                                        HWindd.SetImaage(ImageDT);
                                        isModet = true;
                                        break;
                                }
                            }

                        }
                   
                        HOperatorSet.ReadImage(out imaget, images[i]);
                        hWindowControl3.HalconWindow.DispObj(imaget);
                        hWindowControl2.HalconWindow.DispObj(ImageDT);
                        isbde = true;
                        break;
                    }
                }
                if (!isbde)
                {
                    hWindowControl3.HalconWindow.DispText("未创建参考图片" + camKey ,
                        "window", 0, 0, "red", new HTuple(), new HTuple());
                }
            }
            catch (Exception ex)
            {

            }
        }
        OneCamData oneCamData;
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                OneComponent component = null;
                if (e.Node.Tag is OneCamData)
                {
                    oneCamData = e.Node.Tag as OneCamData;
                    oneCamData.RunVisionName = e.Node.Text;
                    RaedCamImage(e.Node.Text);
                }
                else
                {
                    oneCamData= e.Node.Parent.Tag as OneCamData;
                    if (oneCamData!=null)
                    {
                        oneCamData.RunVisionName = e.Node.Parent.Text;
                        RaedCamImage(e.Node.Parent.Text);
                    }
         
                }

                if (e.Node.Tag is OneComponent)
                {
                    component = e.Node.Tag as OneComponent;
                    //UpOneData(component);
                }
                UpData(OneProductV, component);
                HWindd.ShowImage();
                propertyGrid2.SelectedObject = e.Node.Tag;
              
            }
            catch (Exception ex)
            {
            }
        }

        private void SVisionForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (OneProductV != null)
                {
                    if (textBox1.Focused)
                    {
                       return;
                    }
                    if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Back)
                    {
                        this.hWindowControl1.Focus();

                        if (OneProductV.Done)
                        {
                            treeView1.Nodes.Clear();
                            treeView2.Nodes.Clear();
                            if (OneProductV.OK)
                            {
                                if (OneProductV.AutoOK != OneProductV.OK)
                                {
                                    //RecipeCompiler.AddRlsNumber();
                                    //label3.Text = RecipeCompiler.Instance.GetSPC();
                                }
                            }
                            foreach (var item in OneProductV.ListCamsData)
                            {
                                TreeNode treeNode = treeView1.Nodes.Add(item.Key);
                                foreach (var itemdt in item.Value.NGObj.DicOnes)
                                {
                                    if (!itemdt.Value.aOK)
                                    {
                                        TreeNode treeNode1 = treeNode.Nodes.Add(itemdt.Key);
                                        treeNode1.Tag = itemdt.Value;
                                        treeNode1.ImageIndex = 6;
                                    }
                                }
                                TreeNode treeNodeOK = treeView2.Nodes.Add(item.Key);

                                foreach (var itemdt in item.Value.AllCompObjs.DicOnes)
                                {
                                    TreeNode treeNode1 = treeNodeOK.Nodes.Add(itemdt.Key);
                                    treeNode1.Tag = itemdt.Value;
                                    treeNode1.ImageKey = "OK";
                                }
                                treeNodeOK.Expand();
                                treeNode.ImageIndex = 5;
                                treeNode.Expand();
                                treeNode.Tag = item.Value;
                            }
                            label1.Text = "NG";
                            label1.BackColor = Color.Red;
                            if (OneProductV.GetNGImage() != null)
                            {
                                HWindd.SetImaage(OneProductV.GetNGImage());
                            }
                            UpData(OneProductV);
                        }
                        if (e.KeyCode == Keys.Space)
                        {
                            if (OneProductV.Done)
                            {

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
                            //if (TrayImage.Done)
                            //{
                            //    button1_Click(null, null);
                            //    return;
                            //}
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
                        if (OneProductV.OK)
                        {/*.SetNumberValue(OneProductV.TrayLocation, OneProductV.OK);*/

                            label1.Text = "OK";
                            label1.BackColor = Color.Green;
                        }
                        else
                        {
                            label1.Text = "NG";
                            label1.BackColor = Color.Red;
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
            }

        }

        private void treeView2_AfterSelect(object sender, TreeViewEventArgs e)
        {
            propertyGrid2.SelectedObject = e.Node.Tag;
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            try
            {
                propertyGrid2.SelectedObject = OneProductV;
                textBox1.Text = OneProductV.PanelID;

            }
            catch (Exception)
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Finde(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Directory.CreateDirectory(Vision.Instance.StrigPathImages + "\\" + DateTime.Now.ToString("yyyy年M月d日") + "\\测试1");
                string[] data = Directory.GetFiles(@"E:\图片\2021年10月8日\Bay26_PCBA004-A-RT_F\JSH21370A84D");
                for (int i = 0; i < 5000; i++)
                {
                    string datas = Vision.Instance.StrigPathImages + "\\" + DateTime.Now.ToString("yyyy年M月d日") + "\\测试1\\SN2021" + i.ToString("0000");

                    Directory.CreateDirectory(Vision.Instance.StrigPathImages + "\\" + DateTime.Now.ToString("yyyy年M月d日") + "\\测试1\\SN2021" + i.ToString("0000"));

                    for (int j = 0; j < data.Length; j++)
                    {
                      
                        File.Copy(data[j], datas+"\\" + Path.GetFileName(data[j]));
                    }


                }
            }
            catch (Exception ex)
            {

            }
    
          

        }

        private void textBox1_DoubleClick(object sender, EventArgs e)
        {
            try
            {
              

          

            }
            catch (Exception)
            {
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
 
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

 

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                contextMenuStrip1.Items.Clear();
                for (int i = 0; i < SNLsitD.Count; i++)
                {
                    contextMenuStrip1.Items.Add(SNLsitD[i]);
                }
            }
            catch (Exception)
            {
            }
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                textBox1.Text = e.ClickedItem.Text;
            }
            catch (Exception)
            {
            }
        }
    }
}
