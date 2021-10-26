using HalconDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class ImageForm1 : Form
    {
        public ImageForm1()
        {
            InitializeComponent();
        }
        public void SetData(RunProgramFile.HalconRun halcon )
        {
            HalconRun = halcon;
        }


        RunProgramFile.HalconRun HalconRun;

        private HObject Image;

        private HObject BT;




        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                HSystem.SetSystem("flush_graphic", "false");
                HOperatorSet.ClearWindow(visionUserC1.HalconWindow);
                HSystem.SetSystem("flush_graphic", "true");
                HalconRun.ImageHdt(HalconRun.Image());

                Image= HalconRun.GetImageOBJ(ImageTypeObj.Image3);
                BT = HalconRun.GetImageOBJ(ImageTypeObj.Gray);
                R = HalconRun.GetImageOBJ(ImageTypeObj.R);
                G = HalconRun.GetImageOBJ(ImageTypeObj.G);
                B = HalconRun.GetImageOBJ(ImageTypeObj.B);
                H = HalconRun.GetImageOBJ(ImageTypeObj.H);
                S = HalconRun.GetImageOBJ(ImageTypeObj.S);
                V = HalconRun.GetImageOBJ(ImageTypeObj.V);
                ImagePrt = HalconRun.GetImageOBJ(ImageTypeObj.ImagePretreatment);
                ImageTypeObj ImageTypeO = (ImageTypeObj)Enum.Parse(typeof(ImageTypeObj), listBox1.SelectedItem.ToString());
                switch (ImageTypeO)
                {
                    case ImageTypeObj.Image3:

                        visionUserC1.hWindwC.Image(Image);
                        break;
                    case ImageTypeObj.Gray:
                        visionUserC1.hWindwC.Image(BT);
                        break;
                    case ImageTypeObj.R:
                        visionUserC1.hWindwC.Image(R);
                        break;
                    case ImageTypeObj.G:
                        visionUserC1.hWindwC.Image(G);
                        break;
                    case ImageTypeObj.B:
                        visionUserC1.hWindwC.Image(B);
                        break;
                    case ImageTypeObj.H:
                        visionUserC1.hWindwC.Image(H);
                        break;
                    case ImageTypeObj.S:
                        visionUserC1.hWindwC.Image(S);
                        break;
                    case ImageTypeObj.V:
                        visionUserC1.hWindwC.Image(V);
                        break;
                    case ImageTypeObj.ImagePretreatment:
                        visionUserC1.hWindwC.Image(ImagePrt);
                        break;
                    default:
                        break;
                }
                visionUserC1.hWindwC.ShowOBJ();
            }
            catch (Exception)
            {
            }
        }

        public void SetUPHalc(HObject hObject,RunProgramFile.HalconRun halconRun)
        {
            try
            {
                Image = hObject;
                VisionUserC.HWindwC hWindwC = new VisionUserC.HWindwC();
                HalconRun = halconRun;

                hWindwC.Image(hObject);
                listBox1.Items.Clear();
                listBox1.Items.AddRange(halconRun.GetHoamgeName());

                visionUserC1.UpHalcon(hWindwC);
                //image = hObject;
                HOperatorSet.CountChannels(hWindwC.Image(), out HTuple htcon);
                //listBox1.Items.Clear();
                Column2.Items.Clear();
                Column1.Items.Clear();
                Column1.Items.AddRange(date);
                if (htcon != 3)
                {
                    //listBox1.Items.AddRange(b);
                    Column2.Items.AddRange(halconRun.GetHoamgeName());
                }
                else
                {

                    if (htcon == 3)
                    {
                        //listBox1.Items.AddRange(hsv);
                     
                        Column2.Items.AddRange(halconRun.GetHoamgeName());
                        HOperatorSet.Rgb1ToGray(Image, out BT);
                        HOperatorSet.Decompose3(hWindwC.Image(), out R, out G, out B);
                        HOperatorSet.TransFromRgb(R, G, B, out H, out S, out V, "hsv");
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private HObject R, G, B, H, S, V,ImagePrt;

        private void dataGridView1_VisibleChanged(object sender, EventArgs e)
        {
        }

        //private string[] hsv = new string[] { "Image", "黑白", "R", "G", "B", "H", "S", "V" };

        //private string[] b = new string[] { "Image" };

        private string[] date = new string[] { "Th", "Selset", "con", "int" };

        private List<string> listCode = new List<string>();

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            HOperatorSet.SetDraw(this.visionUserC1.HalconWindow, "fill");
            HOperatorSet.SetColor(this.visionUserC1.HalconWindow, "#ff000040");
            this.visionUserC1.hWindwC.Drawing = true;
            HOperatorSet.DrawCircle(this.visionUserC1.HalconWindow, out HTuple row, out HTuple column, out HTuple radius);
            HOperatorSet.GenCircle(out HObject cicle, row, column, radius);
            HOperatorSet.DispObj(cicle, visionUserC1.HalconWindow);
            HOperatorSet.SetDraw(this.visionUserC1.HalconWindow, "margin");
            this.visionUserC1.hWindwC.Drawing = false;
            this.visionUserC1.hWindwC.keyValuePairs.Add("1", new RunProgramFile.HalconRun.ObjectColor() { _HObject = cicle });
            //toolStripButton2.Checked = true;
            visionUserC1.hWindwC.Keys = true;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (Image == null)
            {
                MessageBox.Show("未关联执行程序");
                return;
            }
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "jpg|*.jpg|BMP|*.bmp|tif|*.tif|tiff|*.tiff|hobj|*.hobj|所有文件|*.*";   //筛选器

            saveFile.Title = "请选择保存路径";      //文件框名称
            saveFile.ShowDialog();    //弹出对话框
            string path = saveFile.FileName;
            if (path == "") return;    //地址为空返回
            try
            {
                string da = Path.GetFileName(path).Split('.')[1].ToLower();
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                HOperatorSet.WriteImage(visionUserC1.hWindwC.Image(), da, 0, path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripCheckbox1_Click(object sender, EventArgs e)
        {
            try
            {
                visionUserC1.hWindwC.FillMode = toolStripCheckbox1.GetBase().Checked;
            }
            catch (Exception)
            {
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                listCode.Clear();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    string data = "";

                    for (int i2 = 0; i2 < dataGridView1.Columns.Count; i2++)
                    {
                        data += dataGridView1.Rows[i].Cells[i2].Value.ToString() + ',';
                    }
                    listCode.Add(data);
                    if (e.RowIndex == i)
                    {
                        break;
                    }
                }

                for (int i = 0; i < listCode.Count; i++)
                {
                    Code(listCode[i]);
                    if (i == e.RowIndex)
                    {
                        if (visionUserC1.hWindwC.keyValuePairs.ContainsKey(i.ToString()))
                        {
                            visionUserC1.hWindwC.keyValuePairs[i.ToString()]._HObject = hObject1;
                        }
                        else
                        {
                            visionUserC1.hWindwC.keyValuePairs.Add(i.ToString(), new RunProgramFile.HalconRun.ObjectColor() { _HObject = hObject1 });
                        }
                        visionUserC1.hWindwC.ShowOBJ();
                        return;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private HObject hObject1;

        private void Code(string codeStr)
        {
            string[] datas = codeStr.Split(',');
            HObject hObject = ShowObj(datas[1]);
            switch (datas[0])
            {
                case "Th":
                    HOperatorSet.Threshold(hObject, out hObject1, int.Parse(datas[2]), int.Parse(datas[3]));
                    break;

                case "Selset":
                    HOperatorSet.Connection(hObject1, out hObject1);

                    HOperatorSet.SelectShape(hObject1, out hObject1, "area", "and", double.Parse(datas[2]), double.Parse(datas[3]));
                    HOperatorSet.AreaCenter(hObject1, out HTuple area, out HTuple row, out HTuple column);
                    break;

                default:
                    break;
            }
        }

        private HObject ShowObj(string imaget)
        {
            HSystem.SetSystem("flush_graphic", "false");
            HOperatorSet.ClearWindow(visionUserC1.HalconWindow);
            HSystem.SetSystem("flush_graphic", "true");

            ImageTypeObj ImageTypeO = (ImageTypeObj)Enum.Parse(typeof(ImageTypeObj), imaget);
            switch (ImageTypeO)
            {
                case ImageTypeObj.Image3:

                    visionUserC1.hWindwC.Image(Image);
                    break;
                case ImageTypeObj.Gray:
                    visionUserC1.hWindwC.Image(BT);
                    break;
                case ImageTypeObj.R:
                    visionUserC1.hWindwC.Image(R);
                    break;
                case ImageTypeObj.G:
                    visionUserC1.hWindwC.Image(G);
                    break;
                case ImageTypeObj.B:
                    visionUserC1.hWindwC.Image(B);
                    break;
                case ImageTypeObj.H:
                    visionUserC1.hWindwC.Image(H);
                    break;
                case ImageTypeObj.S:
                    visionUserC1.hWindwC.Image(S);
                    break;
                case ImageTypeObj.V:
                    visionUserC1.hWindwC.Image(V);
                    break;
                case ImageTypeObj.ImagePretreatment:
                    visionUserC1.hWindwC.Image(ImagePrt);
                    break;
                default:
                    break;
            }
            //HOperatorSet.GetImageSize(hWindwC.Image, out HTuple width, out HTuple height);
            ////HOperatorSet.SetPart(visionUserC1.HalconWindow, -1, -1, height, width);
            //switch (imaget)
            //{
            //    case "RGB":
            //    case "Image":
            //        visionUserC1.hWindwC.Image(Image);

            //        //HOperatorSet.DispObj(hWindwC.Image, visionUserC1.HalconWindow);

            //        break;

            //    case "R":
            //        visionUserC1.hWindwC.Image(R);
            //        //HOperatorSet.DispObj(R, visionUserC1.HalconWindow);

            //        break;

            //    case "G":
            //        visionUserC1.hWindwC.Image(G);
            //        //HOperatorSet.DispObj(G, visionUserC1.HalconWindow);

            //        break;

            //    case "B":
            //        visionUserC1.hWindwC.Image(B);
            //        //HOperatorSet.DispObj(B, visionUserC1.HalconWindow);

            //        break;

            //    case "H":
            //        visionUserC1.hWindwC.Image(H);
            //        //HOperatorSet.DispObj(H, visionUserC1.HalconWindow);
            //        break;

            //    case "S":
            //        visionUserC1.hWindwC.Image(S);
            //        //HOperatorSet.DispObj(S, visionUserC1.HalconWindow);
            //        break;

            //    case "V":
            //        visionUserC1.hWindwC.Image(V);
            //        //HOperatorSet.DispObj(V, visionUserC1.HalconWindow);
            //        break;

            //    default:
            //        break;
            //}
            visionUserC1.hWindwC.ShowOBJ();
            return visionUserC1.hWindwC.Image();
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
        }
    }
}