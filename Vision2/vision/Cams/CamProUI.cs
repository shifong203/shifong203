using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Vision2.vision.Cams
{
    public partial class CamProUI : UserControl
    {
        public CamProUI()
        {
            InitializeComponent();
        }
        ICamera Cam;
        vision.HWindID HWindI;
        public CamProUI(ICamera camParam) : this()
        {
            Cam = camParam;
            HWindI = new HWindID();
            HWindI.Initialize(hWindowControl1);
        }

        private void CamProUI_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;
            comboBox1.Items.Clear();
            string[] Files = System.IO.Directory.GetFiles(Application.StartupPath + "\\Caltab", "*.descr");
            for (int i = 0; i < Files.Length; i++)
            {
                comboBox1.Items.Add(System.IO.Path.GetFileNameWithoutExtension(Files[i]));
            }


        }
        List<HObject> LisImage { get; set; } = new List<HObject>();
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                HWindI.ClearObj();
                LisImage.Add(Cam.GetImage());
                listBox1.Items.Add(LisImage.Count);
                HWindI.SetImaage(LisImage[LisImage.Count - 1]);
                HOperatorSet.FindCalibObject(LisImage[LisImage.Count - 1], calibDataID, 0, 0, LisImage.Count - 1, new HTuple(), new HTuple());
                HOperatorSet.GetCalibDataObservContours(out HObject hObject, calibDataID, "caltab", 0, 0, LisImage.Count - 1);
                HOperatorSet.GetCalibDataObservPoints(calibDataID, 0, 0, LisImage.Count - 1, out HTuple rows, out HTuple Cols, out HTuple index, out HTuple pose);
                HOperatorSet.GenCircle(out HObject Circle, rows, Cols, HTuple.TupleGenConst(rows.Length, 2));
                vision.Calib.AutoCalibPoint.Disp3DCoordSystem(StartParameters, pose, 0.5, out HObject x, out HObject y, out HObject z);
                HWindI.HalconResult.AddObj(x, HalconRunFile.RunProgramFile.RunProgram.ColorResult.red);
                HWindI.HalconResult.AddObj(y, HalconRunFile.RunProgramFile.RunProgram.ColorResult.green);
                HWindI.HalconResult.AddObj(z, HalconRunFile.RunProgramFile.RunProgram.ColorResult.blue);
                HWindI.HalconResult.AddObj(Circle);
                HWindI.HalconResult.AddObj(hObject, "green");
                HWindI.ShowImage();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                for (int i = 0; i < LisImage.Count; i++)
                {
                    HWindI.ClearObj();
                    HOperatorSet.FindCalibObject(LisImage[i], calibDataID, 0, 0, i, new HTuple(), new HTuple());
                    HOperatorSet.GetCalibDataObservContours(out HObject hObject, calibDataID, "caltab", 0, 0, i);
                    HOperatorSet.GetCalibDataObservPoints(calibDataID, 0, 0, i, out HTuple rows, out HTuple Cols, out HTuple index, out HTuple pose);
                    HOperatorSet.GenCircle(out HObject Circle, rows, Cols, HTuple.TupleGenConst(rows.Length, 2));
                    vision.Calib.AutoCalibPoint.Disp3DCoordSystem(StartParameters, pose, 0.5, out HObject x, out HObject y, out HObject z);
                    HWindI.HalconResult.AddObj(x, HalconRunFile.RunProgramFile.RunProgram.ColorResult.red);
                    HWindI.HalconResult.AddObj(y, HalconRunFile.RunProgramFile.RunProgram.ColorResult.green);
                    HWindI.HalconResult.AddObj(z, HalconRunFile.RunProgramFile.RunProgram.ColorResult.blue);
                    HWindI.HalconResult.AddObj(Circle);
                    HWindI.HalconResult.AddObj(hObject, "green");
                    HWindI.ShowImage();

                }
                HOperatorSet.CalibrateCameras(calibDataID, out HTuple error);
                HOperatorSet.GetCalibData(calibDataID, "camera", 0, "params", out CamParam);
                HWindI.HalconResult.Massage = new HTuple();
                HWindI.HalconResult.AddMeassge("标定误差结果:" + error.TupleString("5.4f") + "px");
                HWindI.HalconResult.AddMeassge("焦距:" + (CamParam.TupleSelect(1) * 1000).TupleString("5.2f") + "mm");
                HWindI.HalconResult.AddMeassge("畸变:" + (CamParam.TupleSelect(2)).TupleString("2.2f"));
                HWindI.HalconResult.AddMeassge("Cx:" + CamParam[5]);
                HWindI.HalconResult.AddMeassge("Cy:" + CamParam[6]);
                HWindI.HalconResult.AddMeassge("Sx:" + (CamParam.TupleSelect(3) * 1000000).TupleString("5.2f") + "um");
                HWindI.HalconResult.AddMeassge("Sy:" + (CamParam.TupleSelect(4) * 1000000).TupleString("5.2f") + "um");
                Cam.Kappa = Math.Round(CamParam.TupleSelect(2).D, 2);
                Cam.Focal = Math.Round(CamParam.TupleSelect(1).D * 1000, 2);

                Cam.Cx = Math.Round(CamParam[5].D, 2);
                Cam.Cy = Math.Round(CamParam[6].D, 2);
                Cam.Sx = Math.Round((CamParam.TupleSelect(3) * 1000000).D, 2);
                Cam.Sy = Math.Round((CamParam.TupleSelect(4) * 1000000).D, 2);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        HTuple StartParameters = new HTuple();
        HTuple CamParam;
        HTuple calibDataID;
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                //LisImage.Clear();
                //listBox1.Items.Clear();
                StartParameters = new HTuple();


                HOperatorSet.CreateCalibData("calibration_object", 1, 1, out calibDataID);
                StartParameters.Append("area_scan_division");
                StartParameters.Append(Cam.Focal / 1000);
                StartParameters.Append(0);
                StartParameters.Append(Cam.Sx / 1000000);
                StartParameters.Append(Cam.Sy / 1000000);
                StartParameters.Append(Cam.Width / 2);
                StartParameters.Append(Cam.Height / 2);
                StartParameters.Append(Cam.Width);
                StartParameters.Append(Cam.Height);
                HOperatorSet.SetCalibDataCamParam(calibDataID, 0, new HTuple(), StartParameters);
                HOperatorSet.SetCalibDataCalibObject(calibDataID, 0, Application.StartupPath + "//Caltab//" + comboBox1.SelectedItem.ToString() + ".descr");
                MessageBox.Show("创建成功，请开始标定");
                button1.Enabled = true;
                return;
            }
            catch (Exception ex)
            {
            }

            MessageBox.Show("标定失败");
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                HWindI.ClearObj();

                HWindI.SetImaage(LisImage[listBox1.SelectedIndex]);
                Cam.Width = (int)HWindI.WidthImage;
                Cam.Height = (int)HWindI.HeigthImage;
                HOperatorSet.FindCalibObject(LisImage[listBox1.SelectedIndex], calibDataID, 0, 0, listBox1.SelectedIndex, new HTuple(), new HTuple());
                HOperatorSet.GetCalibDataObservContours(out HObject hObject, calibDataID, "caltab", 0, 0, listBox1.SelectedIndex);
                HOperatorSet.GetCalibDataObservPoints(calibDataID, 0, 0, listBox1.SelectedIndex, out HTuple rows, out HTuple Cols, out HTuple index, out HTuple pose);
                HOperatorSet.GenCircle(out HObject Circle, rows, Cols, HTuple.TupleGenConst(rows.Length, 2));
                vision.Calib.AutoCalibPoint.Disp3DCoordSystem(StartParameters, pose, 0.5, out HObject x, out HObject y, out HObject z);
                HWindI.HalconResult.AddObj(x, HalconRunFile.RunProgramFile.RunProgram.ColorResult.red);
                HWindI.HalconResult.AddObj(y, HalconRunFile.RunProgramFile.RunProgram.ColorResult.green);
                HWindI.HalconResult.AddObj(z, HalconRunFile.RunProgramFile.RunProgram.ColorResult.blue);
                HWindI.HalconResult.AddObj(Circle);
                HWindI.HalconResult.AddObj(hObject, "green");
                HWindI.ShowImage();
            }
            catch (Exception ex)
            {

            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "请选择图片文件可多选";
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = "图片文件|*.jpg;*.tif;*.tiff;*.gif;*.bmp;*.jpg;*.jpeg;*.jp2;*.png;*.pcx;*.pgm;*.ppm;*.pbm;*.xwd;*.ima;*.hobj";
                if (openFileDialog.ShowDialog() != DialogResult.OK) return;
                for (int i = 0; i < openFileDialog.FileNames.Length; i++)
                {
                    HOperatorSet.ReadImage(out HObject hObject, openFileDialog.FileNames[i]);
                    LisImage.Add(hObject);
                    listBox1.Items.Add(LisImage.Count);
                }
            }
            catch (Exception)
            {
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                LisImage.RemoveAt(listBox1.SelectedIndex);
                listBox1.Items.RemoveAt(listBox1.Items.Count - 1);
            }
            catch (Exception)
            {
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                HOperatorSet.FindCalibObject(LisImage[listBox1.SelectedIndex], calibDataID, 0, 0, 0, new HTuple(), new HTuple());
                HOperatorSet.GetCalibDataObservContours(out HObject hObject, calibDataID, "caltab", 0, 0, 0);
                HOperatorSet.GetCalibDataObservPoints(calibDataID, 0, 0, 0, out HTuple rows, out HTuple Cols, out HTuple index, out HTuple pose);
                HOperatorSet.CaltabPoints(Application.StartupPath + "//Caltab//" + comboBox1.SelectedItem.ToString() + ".descr", out HTuple x, out HTuple y, out HTuple Z);
                HOperatorSet.CameraCalibration(x, y, Z, rows, Cols, StartParameters, pose, "all", out CamParam, out HTuple nfinalPose, out HTuple errore);
                HOperatorSet.SetOriginPose(nfinalPose, 0, 0, 0, out HTuple FinalPose);
                HOperatorSet.VectorToHomMat2d(rows, Cols, y, x, out HTuple hTuple);//hom_mat2d_to_affine_par
                HOperatorSet.HomMat2dToAffinePar(hTuple, out HTuple sx, out HTuple sy, out HTuple aphi, out HTuple theta, out HTuple tx, out HTuple ty);
                HTuple ScaleOrigin = (sx.TupleFabs() + sy.TupleFabs()) / 2.0;
                FinalPose[5] = 0;

                HOperatorSet.ImagePointsToWorldPlane(CamParam, FinalPose, 0, 0, "m", out HTuple xx, out HTuple yy);
                HOperatorSet.SetOriginPose(FinalPose, xx, yy, 0, out HTuple PoseTemp);
                HOperatorSet.GenImageToWorldPlaneMap(out HObject map, CamParam, PoseTemp, Cam.Width, Cam.Height, Cam.Width, Cam.Height, ScaleOrigin, "bilinear");


                //HOperatorSet.GenRadialDistortionMap(out  map, StartParameters, CamParam, "bilinear");
                Cam.Map = map;
                HOperatorSet.MapImage(LisImage[listBox1.SelectedIndex], Cam.Map, out hObject);
                HWindI.SetImaage(hObject);
                HWindI.ShowImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
