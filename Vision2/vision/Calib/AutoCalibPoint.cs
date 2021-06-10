using HalconDotNet;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;
using Vision2.vision.HalconRunFile.RunProgramFile;
using static Vision2.ErosProjcetDLL.FileCon.FileConStatic;
using static Vision2.vision.HalconRunFile.RunProgramFile.RunProgram;

namespace Vision2.vision.Calib
{
    public class AutoCalibPoint : ProjectNodet.IClickNodeProject
    {
        public string Name { get; set; }

        public const string FileName = "CalibFile";
        public static string GetFileName()
        {
            Directory.CreateDirectory(Vision.GetFilePath() + "CalibFile\\");
            return Vision.GetFilePath() + "CalibFile\\";
        }

        public string MCalibPaht { get; set; }

        public string TCalibPaht { get; set; }

        public string TRobotCall { get; set; }

        public string MRobotCall { get; set; }
        /// <summary>
        /// 标定类型
        /// </summary>
        public enum CalibMode
        {
            移动抓取 = 0,
            固定相机 = 1,
            移动放置 = 2,
        }
        /// <summary>
        /// 相机标定句柄
        /// </summary>
        HTuple calibDataID;
        #region 移动相机

        /// <summary>
        /// 相机内参
        /// </summary>
        [DisplayName("相机内参"), DescriptionAttribute(""), Category("移动相机")]
        public HTuple CamParamT { get { return camParam; } }
        /// <summary>
        /// 相机内参
        /// </summary>
        public HTuple camParam;
        /// <summary>
        /// 相机Tool坐标
        /// </summary>
        [DisplayName("相机Tool坐标"), DescriptionAttribute(""), Category("移动相机")]
        public HTuple ToolInCamPoseT { get { return ToolInCamPose; } }
        /// <summary>
        /// 相机Tool坐标
        /// </summary>
        public HTuple ToolInCamPose;

        /// <summary>
        /// 标定板到相机位置
        /// </summary>
        [DisplayName("标定板到相机位置"), DescriptionAttribute(""), Category("移动相机")]
        public HTuple CalibInCamPose { get { return calibInCamPose; } }
        /// <summary>
        /// 移动标定板到相机位置
        /// </summary>
        public HTuple calibInCamPose;

        #endregion
        #region 固定相机
        [DisplayName("相机内参"), DescriptionAttribute(""), Category("固定相机")]
        public HTuple TCamParam { get { return tCamParam; } set { tCamParam = value; } }
        /// <summary>
        /// 固定相机位置
        /// </summary>
        [DisplayName("相机到机械手坐标"), DescriptionAttribute(""), Category("固定相机")]
        public HTuple TBaseInCamPose { get { return tBaseInCamPose; } }
        /// <summary>
        /// 固定相机位置
        /// </summary>
        public HTuple tBaseInCamPose;
        /// <summary>
        /// 固定相机
        /// </summary>
        HTuple tCamParam;
        [DisplayName("标定板到相机位置"), DescriptionAttribute(""), Category("固定相机")]
        public HTuple TCalibInCamPose { get { return tCalibInCamPose; } set { tCalibInCamPose = value; } }
        /// <summary>
        /// 固定相机标定板位置
        /// </summary>
        public HTuple tCalibInCamPose;
        [DisplayName("动态Tool位置"), DescriptionAttribute(""), Category("固定相机")]
        public HTuple Tool1Base { get; set; }
        #endregion
        public HTuple Errs;
        string[] lPaths;
        string[] lPoses;
        /// <summary>
        ///  保存全部相机，标定坐标参数存储到文件
        /// </summary>
        /// <param name="pathDirectory"></param>
        /// <returns></returns>
        public bool SaveCalib(string pathDirectory, bool misT)
        {

            try
            {
                Directory.CreateDirectory(pathDirectory + "固定相机\\");
                Directory.CreateDirectory(pathDirectory + "移动相机\\");
                if (misT)
                {

                    HOperatorSet.WriteCamPar(tCamParam, pathDirectory + "固定相机\\final_campar.dat");
                    HOperatorSet.WritePose(tCalibInCamPose, pathDirectory + "固定相机\\CalibInCamPose.dat");
                    HOperatorSet.WritePose(this.tBaseInCamPose, pathDirectory + "固定相机\\toolInbasePoseBaseInCamPose.dat");
                }
                else
                {
                    HOperatorSet.WriteCamPar(camParam, pathDirectory + "移动相机\\final_campar.dat");
                    HOperatorSet.WritePose(ToolInCamPose, pathDirectory + "移动相机\\final_pose_cam_tool.dat");
                    HOperatorSet.WritePose(calibInCamPose, pathDirectory + "移动相机\\final_pose_base_calplate.dat");
                }
                return true;
            }
            catch (System.Exception)
            {
            }
            return false;
        }

        /// <summary>
        /// 读取移动或固定相机参数
        /// </summary>
        /// <param name="mist">true读取固定相机，false读取移动相机</param>
        /// <returns></returns>
        public bool ReadCalib(string pathDirectory, bool mist)
        {
            try
            {
                if (!mist)///移动相机
                {
                    string fileName = "\\移动相机\\";
                    HOperatorSet.ReadCamPar(pathDirectory + fileName + "final_campar.dat", out camParam);

                    HOperatorSet.ReadPose(pathDirectory + fileName + "final_pose_cam_tool.dat", out ToolInCamPose);

                    HOperatorSet.ReadPose(pathDirectory + fileName + "final_pose_base_calplate.dat", out calibInCamPose);
                }
                else //固定相机
                {
                    string fileName = "\\固定相机\\";
                    HOperatorSet.ReadCamPar(pathDirectory + fileName + "final_campar.dat", out tCamParam);
                    HOperatorSet.ReadPose(pathDirectory + fileName + "toolInbasePoseBaseInCamPose.dat", out tBaseInCamPose);
                    HOperatorSet.ReadPose(pathDirectory + fileName + "CalibInCamPose.dat", out tCalibInCamPose);
                }
                return true;
            }
            catch (System.Exception ex)
            {
            }
            return false;
        }
        /// <summary>
        /// 读取固定相机和移动相机的标定参数
        /// </summary>
        /// <param name="pathDirectory"></param>
        /// <returns></returns>
        public bool ReadCalib(string pathDirectory)
        {
            ///读取固定相机
            if (!ReadCalib(pathDirectory, true))
            {
                return false;
            }
            //读取移动相机参数
            if (!ReadCalib(pathDirectory, false))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 读取相机内参
        /// </summary>
        /// <param name="path">文件地址</param>
        /// <param name="misT">等于True时读取固定相机</param>
        public void ReadCamProg(string path, bool misT = false)
        {
            try
            {
                if (!misT)
                {
                    HOperatorSet.ReadCamPar(path, out camParam);
                }
                else
                {
                    HOperatorSet.ReadCamPar(path, out tCamParam);
                }
                MessageBox.Show("读取内参成功");
            }
            catch (Exception)
            {
            }
        }

        public void RunCalibCamPar()
        {

        }
        /// <summary>
        ///自动标定固定相机
        /// </summary>
        /// <returns></returns>
        public bool RunAutoTCalib(string path, int i, int cont, HTuple hWindowID, HTuple pose3D, HObject image)
        {
            return false;
            if (i == 0)
            {
                Errs = "";
                if (!ReadCamPar(path + "\\固定相机\\final_campar.dat", this.TCalibPaht, true))
                {
                    MessageBox.Show("读取相机标定板错误");
                    return false;
                }
            }
            Vision.Disp_message(hWindowID, i + "\\", 20, 20);
            RunCalib(image, pose3D, i, true, hWindowID);
            if (i >= cont)
            {
                HOperatorSet.CalibrateHandEye(calibDataID, out HTuple errs);//手眼标定操作
                HTuple CalObjInCamPose;
                HOperatorSet.GetCalibData(calibDataID, "camera", 0, "params", out tCamParam);//获取相机内部参数
                HOperatorSet.GetCalibData(calibDataID, "camera", 0, "base_in_cam_pose", out this.tBaseInCamPose);//获取相机的工具坐标
                HOperatorSet.GetCalibData(calibDataID, "calib_obj", 0, "obj_in_tool_pose", out this.tCalibInCamPose);//获取标定板目标的工具坐标
                HOperatorSet.QueryCalibDataObservIndices(calibDataID, "camera", 0, out HTuple CalibObjIdx, out HTuple PoseIds);
                for (int it = 0; it < PoseIds.Length; it++)
                {
                    HOperatorSet.ReadImage(out image, path + "\\T\\" + PoseIds[it] + ".bmp");
                    HOperatorSet.DispObj(image, hWindowID);
                    Vision.Disp_message(hWindowID, it + "\\" + PoseIds.Length, 20, 20);
                    HOperatorSet.GetCalibData(calibDataID, "tool", PoseIds[it], "tool_in_base_pose", out HTuple ToolInBasePose);
                    CalObjInCamPose = Calc_calplate_pose_stationarycam(tCalibInCamPose, tBaseInCamPose, ToolInBasePose);
                    Disp3DCoordSystem(tCamParam, CalObjInCamPose, 0.01, hWindowID);
                    HOperatorSet.WaitSeconds(0.5);
                }
                HTuple hTuple = new HTuple();
                if (ReadCamCalib(image, this.TCalibPaht, true))
                {
                    AutoCalibPoint.Disp3DCoordSystem(TCamParam, tCalibInCamPose, 0.01, hWindowID);
                    hTuple.Append(tCalibInCamPose);
                }
                else
                {
                    MessageBox.Show("读取出错");
                }
                SaveCalib(path + "\\", true);
                hTuple.Append("平移位置单位:" + errs.TupleSelect(0).TupleMult(1000).TupleString("0.04f") + "mm|" + errs.TupleSelect(2).TupleMult(1000).TupleString("0.04f") + "mm");
                hTuple.Append("旋转角度单位:" + errs.TupleSelect(1).TupleString("0.04f") + "|" + errs.TupleSelect(3).TupleString("0.04f"));
                Vision.Disp_message(hWindowID, hTuple, 20, 20);
                Errs = hTuple;
                HOperatorSet.ClearCalibData(calibDataID);
            }
            return false;
        }

        /// <summary>
        /// 自动标定移动相机
        /// </summary>
        /// <returns></returns>
        public bool RunAutoMCalib(string path, int i, int cont, HTuple hWindowID, HTuple pose3D, HObject image)
        {
            return false;
            if (i == 0)
            {
                if (!ReadCamPar(path + "\\移动相机\\final_campar.dat", this.MCalibPaht, false))
                {
                    MessageBox.Show("读取相机标定板错误");
                    return false;
                }
            }

            this.Object = RunCalib(image, pose3D, i, false, hWindowID);
            if (i >= cont)
            {
                HOperatorSet.CalibrateHandEye(calibDataID, out HTuple errs);//手眼标定操作
                HTuple CalObjInCamPose;
                HOperatorSet.GetCalibData(calibDataID, "camera", 0, "params", out camParam);//获取相机内部参数
                HOperatorSet.GetCalibData(calibDataID, "camera", 0, "tool_in_cam_pose", out ToolInCamPose);//获取相机的工具坐标
                HOperatorSet.GetCalibData(calibDataID, "calib_obj", 0, "obj_in_base_pose", out calibInCamPose);//获取对象目标的基础坐标
                HOperatorSet.QueryCalibDataObservIndices(calibDataID, "camera", 0, out HTuple CalibObjIdx, out HTuple PoseIds);
                for (int iC = 0; iC < PoseIds.Length; iC++)
                {
                    HOperatorSet.ReadImage(out image, path + "\\M\\" + PoseIds[iC] + ".bmp");
                    HOperatorSet.DispObj(image, hWindowID);
                    Vision.Disp_message(hWindowID, iC + "\\" + PoseIds.Length, 20, 20);
                    HOperatorSet.GetCalibData(calibDataID, "tool", PoseIds[iC], "tool_in_base_pose", out HTuple ToolInBasePose);
                    CalObjInCamPose = calc_calplate_pose_movingcam(calibInCamPose, ToolInCamPose, ToolInBasePose);
                    Disp3DCoordSystem(camParam, CalObjInCamPose, 0.01, hWindowID);
                    HOperatorSet.WaitSeconds(0.5);
                }
                HOperatorSet.DispObj(image, hWindowID);
                HTuple hTuple = new HTuple();
                if (ReadCamCalib(image, this.MCalibPaht, false))
                {
                    AutoCalibPoint.Disp3DCoordSystem(camParam, calibInCamPose, 0.01, hWindowID);
                    hTuple.Append(calibInCamPose);
                }
                else
                {
                    MessageBox.Show("读取出错");
                }
                SaveCalib(path + "\\", false);
                hTuple.Append("平移位置单位:" + errs.TupleSelect(0).TupleMult(1000).TupleString("0.04f") + "mm|" + errs.TupleSelect(2).TupleMult(1000).TupleString("0.04f") + "mm");
                hTuple.Append("旋转角度单位:" + errs.TupleSelect(1).TupleString("0.04f") + "|" + errs.TupleSelect(3).TupleString("0.04f"));
                Vision.Disp_message(hWindowID, hTuple);
                Errs = hTuple;
                HOperatorSet.ClearCalibData(calibDataID);
            }
            return false;

        }

        /// <summary>
        /// 读取图像并标定
        /// </summary>
        /// <param name="iamgePath"></param>
        /// <param name="camPath"></param>
        /// <param name="hWindowID"></param>
        public bool RunUPCalib(string iamgePath, string camPath, string caltabPath, bool misT, HTuple hWindowID = null)
        {
            try
            {
                if (!ReadCamPar(camPath, caltabPath, misT))
                {
                    MessageBox.Show("读取相机标定板错误");
                    return false;
                }
                lPaths = Vision2.ErosProjcetDLL.FileCon.FileConStatic.GetFilesArrayPath(iamgePath, ".bmp");
                Array.Sort(lPaths, new CustomFilesNameComparer());
                lPoses = Directory.GetFiles(iamgePath).Where(item => item.EndsWith(".dat", StringComparison.Ordinal)).ToArray();
                Array.Sort(lPoses, new CustomFilesNameComparer());
                if (lPaths.Length != lPoses.Length)
                {
                    MessageBox.Show("图片与位置参数不相等");
                    return false;
                }
                HObject image;
                for (int i = 0; i < lPaths.Length; i++)
                {
                    try
                    {
                        HOperatorSet.ReadImage(out image, lPaths[i]);
                        HOperatorSet.DispObj(image, hWindowID);
                        HOperatorSet.ReadPose(lPoses[i], out HTuple ps);
                        Vision.Disp_message(hWindowID, i + "\\" + lPaths.Length);
                        RunCalib(image, ps, i, misT, hWindowID);
                        image.Dispose();
                    }
                    catch (Exception ex)
                    {
                    }
                    HOperatorSet.WaitSeconds(0.5);
                }
                string mesage = Check_hand_eye_calibration_input_poses(calibDataID, 0.05, 0.005);
                if (mesage != "")
                {
                }
                HOperatorSet.CalibrateHandEye(calibDataID, out HTuple errs);//手眼标定操作
                HTuple CalObjInCamPose;
                if (misT)
                {
                    HOperatorSet.GetCalibData(calibDataID, "camera", 0, "params", out tCamParam);//获取相机内部参数
                    HOperatorSet.GetCalibData(calibDataID, "camera", 0, "base_in_cam_pose", out this.tBaseInCamPose);//获取相机的工具坐标
                    HOperatorSet.GetCalibData(calibDataID, "calib_obj", 0, "obj_in_tool_pose", out this.tCalibInCamPose);//获取标定板目标的工具坐标
                    HOperatorSet.QueryCalibDataObservIndices(calibDataID, "camera", 0, out HTuple CalibObjIdx, out HTuple PoseIds);
                    for (int i = 0; i < PoseIds.Length; i++)
                    {
                        HOperatorSet.ReadImage(out image, iamgePath + "\\" + PoseIds[i] + ".bmp");
                        HOperatorSet.DispObj(image, hWindowID);
                        Vision.Disp_message(hWindowID, i + "\\" + PoseIds.Length);
                        HOperatorSet.GetCalibData(calibDataID, "tool", PoseIds[i], "tool_in_base_pose", out HTuple ToolInBasePose);
                        CalObjInCamPose = Calc_calplate_pose_stationarycam(tCalibInCamPose, tBaseInCamPose, ToolInBasePose);
                        Disp3DCoordSystem(tCamParam, CalObjInCamPose, 0.01, hWindowID);
                        HOperatorSet.WaitSeconds(0.5);
                    }
                }
                else
                {

                    HOperatorSet.GetCalibData(calibDataID, "camera", 0, "params", out camParam);//获取相机内部参数
                    HOperatorSet.GetCalibData(calibDataID, "camera", 0, "tool_in_cam_pose", out ToolInCamPose);//获取相机的工具坐标
                    HOperatorSet.GetCalibData(calibDataID, "calib_obj", 0, "obj_in_base_pose", out calibInCamPose);//获取对象目标的基础坐标
                    HOperatorSet.QueryCalibDataObservIndices(calibDataID, "camera", 0, out HTuple CalibObjIdx, out HTuple PoseIds);
                    for (int i = 0; i < PoseIds.Length; i++)
                    {
                        HOperatorSet.ReadImage(out image, iamgePath + "\\" + PoseIds[i] + ".bmp");
                        HOperatorSet.DispObj(image, hWindowID);
                        Vision.Disp_message(hWindowID, i + "\\" + PoseIds.Length);
                        HOperatorSet.GetCalibData(calibDataID, "tool", PoseIds[i], "tool_in_base_pose", out HTuple ToolInBasePose);
                        CalObjInCamPose = calc_calplate_pose_movingcam(calibInCamPose, ToolInCamPose, ToolInBasePose);
                        Disp3DCoordSystem(camParam, CalObjInCamPose, 0.01, hWindowID);
                        HOperatorSet.WaitSeconds(0.5);
                    }
                }

                SaveCalib(iamgePath, misT);
                HTuple hTuple = new HTuple();
                HOperatorSet.GetCalibData(calibDataID, "model", "general", "camera_calib_error", out HTuple CamCalibError);
                hTuple.Append("相机内参精度" + CamCalibError);
                hTuple.Append("平移位置单位:" + errs.TupleSelect(0).TupleMult(1000).TupleString("0.04f") + "mm|" + errs.TupleSelect(2).TupleMult(1000).TupleString("0.04f"));
                hTuple.Append("旋转角度单位:" + errs.TupleSelect(1).TupleString("0.04f") + "|" + errs.TupleSelect(3).TupleString("0.04f"));
                Vision.Disp_message(hWindowID, hTuple);
                Errs = hTuple;
                HOperatorSet.ClearCalibData(calibDataID);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            HOperatorSet.ClearCalibData(calibDataID);
            return false;
        }
        HObject Object = new HObject();
        HObject ObjectY = new HObject();
        HObject ObjectZ = new HObject();
        HObject ObjectX = new HObject();
        public HObject GeT3d()
        {
            return Object;
        }
        public HalconRun.ObjectColor Get3DX()
        {
            return new HalconRun.ObjectColor() { _HObject = ObjectX, HobjectColot = "red" };
        }
        public HalconRun.ObjectColor Get3DY()
        {
            return new HalconRun.ObjectColor() { _HObject = ObjectY, HobjectColot = "green" };
        }
        public HalconRun.ObjectColor Get3DZ()
        {

            return new HalconRun.ObjectColor() { _HObject = ObjectZ, HobjectColot = "blue" };

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="calibPaht"></param>
        /// <param name="misT"></param>
        /// <returns></returns>
        public bool ReadCamCalib(HObject image, string calibPaht, bool misT)
        {
            try
            {
                if (misT)
                {
                    HOperatorSet.CreateCalibData("calibration_object", 1, 1, out calibDataID);
                    HOperatorSet.SetCalibDataCamParam(calibDataID, 0, new HTuple(), tCamParam);
                    HOperatorSet.SetCalibDataCalibObject(calibDataID, 0, calibPaht);
                    HOperatorSet.FindCalibObject(image, calibDataID, 0, 0, 0, "sigma", 10);
                    HOperatorSet.GetCalibDataObservPoints(calibDataID, 0, 0, 0, out HTuple row, out HTuple col, out HTuple index, out tCalibInCamPose);
                }
                else
                {
                    HOperatorSet.CreateCalibData("calibration_object", 1, 1, out calibDataID);
                    HOperatorSet.SetCalibDataCamParam(calibDataID, 0, new HTuple(), camParam);
                    HOperatorSet.SetCalibDataCalibObject(calibDataID, 0, calibPaht);
                    HOperatorSet.FindCalibObject(image, calibDataID, 0, 0, 0, "sigma", 10);
                    HOperatorSet.GetCalibDataObservPoints(calibDataID, 0, 0, 0, out HTuple row, out HTuple col, out HTuple index, out calibInCamPose);
                }
                HOperatorSet.ClearCalibData(calibDataID);
                return true;
            }
            catch (Exception ex)
            {
            }
            HOperatorSet.ClearCalibData(calibDataID);
            return false;
        }
        /// <summary>
        /// 初始化读取相机参数和标定板参数
        /// </summary>
        /// <param name="camPath">相机参数地址</param>
        /// <param name="path">标定板参数地址</param>
        /// <param name="misT">true固定相机，false移动相机</param>
        /// <returns></returns>
        public bool ReadCamPar(string camPath, string path, bool misT)
        {
            try
            {
                try
                {
                    HOperatorSet.ClearCalibData(calibDataID);
                }
                catch (Exception)
                {
                }
                if (misT)
                {
                    HOperatorSet.ReadCamPar(camPath, out tCamParam);
                    HOperatorSet.CreateCalibData("hand_eye_stationary_cam", 1, 1, out calibDataID);
                    HOperatorSet.SetCalibDataCamParam(calibDataID, 0, "area_scan_division", tCamParam);
                }
                else
                {
                    HOperatorSet.ReadCamPar(camPath, out camParam);
                    HOperatorSet.CreateCalibData("hand_eye_moving_cam", 1, 1, out calibDataID);
                    HOperatorSet.SetCalibDataCamParam(calibDataID, 0, "area_scan_division", camParam);
                }
                HOperatorSet.SetCalibDataCalibObject(calibDataID, 0, path);
                HOperatorSet.SetCalibData(calibDataID, "model", "general", "optimization_method", "nonlinear");
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }
        /// <summary>
        /// 计算像素在标定后的3D世界坐标
        /// </summary>
        /// <param name="calibMode">定位模式</param>
        /// <param name="rows">2个位置确定角度</param>
        /// <param name="cols">2个位置确定角度</param>
        /// <param name="toolInBasePose">机械手当前位置</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <param name="halconRun">窗口句柄</param>
        /// <returns>成功返回true</returns>
        public bool Run(CalibMode calibMode, HTuple rows, HTuple cols, HTuple toolInBasePose, out HTuple x, out HTuple y, out HTuple z, out HTuple u, out HTuple v, out HTuple w, HTuple phi = null, HalconRun halconRun = null)
        {
            z = x = y = u = v = w = null;
            if (calibMode == CalibMode.移动放置)
            {
                return RunMode3(rows, cols, phi, toolInBasePose, out x, out y, out z, out u, out v, out w, halconRun);
            }
            else if (calibMode == CalibMode.固定相机)
            {
                return RunMode2(rows, cols, toolInBasePose, out x, out y, out z, out u, out v, out w, halconRun);
            }
            else
            {
                return RunMode1(rows, cols, toolInBasePose, out x, out y, out z, out u, out v, out w, halconRun);
            }

        }

        /// <summary>
        /// 移动抓取
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="toolInBasePose"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        bool RunMode1(HTuple row, HTuple col, HTuple toolInBasePose, out HTuple x, out HTuple y, out HTuple z, out HTuple u, out HTuple v, out HTuple w, HalconRun halconRun = null)
        {
            z = x = y = u = v = w = null;
            try
            {
                HTuple CalObjInCamPose = calc_calplate_pose_movingcam(calibInCamPose, ToolInCamPose, toolInBasePose);
                HOperatorSet.PoseInvert(ToolInCamPose, out HTuple CamInToolPose); //对指定的三维坐标系进行反转
                HOperatorSet.ImagePointsToWorldPlane(camParam, CalObjInCamPose, row, col, "m", out HTuple x0, out HTuple y0);
                HOperatorSet.PoseToHomMat3d(CalObjInCamPose, out HTuple HomMat3D);
                HTuple tuz = HTuple.TupleGenConst(x0.Length, 0);
                HOperatorSet.AffineTransPoint3d(HomMat3D, x0, y0, tuz, out HTuple Qx, out HTuple Qy, out HTuple Qz);
                //获取目标点在tool0下的位置
                HOperatorSet.PoseToHomMat3d(CamInToolPose, out HTuple Tool_H_Cam);
                HOperatorSet.AffineTransPoint3d(Tool_H_Cam, Qx, Qy, Qz, out HTuple Qx1, out HTuple Qy1, out HTuple Qz1);
                //* 获取目标点在base下的位置
                HOperatorSet.PoseToHomMat3d(toolInBasePose, out HTuple Base_H_Tool);
                HOperatorSet.AffineTransPoint3d(Base_H_Tool, Qx1, Qy1, Qz1, out HTuple Qx2, out HTuple Qy2, out HTuple Qz2);
                x = Qx2.TupleMult(1000);
                y = Qy2.TupleMult(1000);
                z = Qz2.TupleMult(1000);
                u = 0;
                v = 0;
                w = 0;
                return true;
            }
            catch (System.Exception ex)
            {
            }
            return false;
        }

        /// <summary>
        /// 固定相机拍照获得动态Tool
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="toolInBasePose"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        bool RunMode2(HTuple row, HTuple col, HTuple toolInBasePose, out HTuple x, out HTuple y, out HTuple z, out HTuple u, out HTuple v, out HTuple w, HalconRun halconRun = null)
        {
            z = x = y = u = v = w = null;
            if (RunSetTool(row, col, toolInBasePose, halconRun))
            {
                if (PoseToXYZUVW(Tool1Base, out Single sx, out Single sy, out Single sz, out Single su, out Single sv, out Single sw))
                {
                    x = sx;
                    y = sy;
                    z = sz;
                    u = su;
                    v = sv;
                    w = sw;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 固定加放置相机
        /// </summary>
        /// <param name="rows">像素Rows</param>
        /// <param name="cols">像素cols</param>
        /// <param name="toolInBasePose">机械手当前位置</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <param name="hWindID">窗口句柄</param>
        /// <returns>成功返回true</returns>
        bool RunMode3(HTuple rows, HTuple cols, HTuple phi, HTuple toolInBasePose, out HTuple x, out HTuple y, out HTuple z, out HTuple u, out HTuple v, out HTuple w, HalconRun halconRun = null)
        {
            z = x = y = u = v = w = 0;
            HOperatorSet.ImagePointsToWorldPlane(camParam, calibInCamPose, rows, cols, "m", out HTuple x0, out HTuple y0);
            HOperatorSet.AngleLl(0, 0, 0.1, 0, x0[0], y0[0], x0[1], y0[1], out HTuple angle);
            HOperatorSet.CreatePose(x0[0], y0[0], 0, 180, 0, angle.TupleDeg(), "Rp+T", "abg", "point", out HTuple Tool1InCaliPose);
            HOperatorSet.ConvertPoseType(Tool1InCaliPose, "Rp+T", "gba", "point", out Tool1InCaliPose);
            HOperatorSet.PoseCompose(calibInCamPose, Tool1InCaliPose, out HTuple Tool1InCamPose);
            HOperatorSet.ConvertPoseType(Tool1InCamPose, "Rp+T", "abg", "point", out Tool1InCamPose);
            Disp3DCoordSystem(camParam, Tool1InCamPose, 0.05, halconRun);

            HOperatorSet.PoseInvert(ToolInCamPoseT, out HTuple CamInTool0Pose);
            HOperatorSet.PoseCompose(toolInBasePose, CamInTool0Pose, out HTuple CamInBasePose);
            HOperatorSet.PoseCompose(CamInBasePose, Tool1InCamPose, out CamInBasePose);
            HTuple hTuple = PoseComp(Tool1Base, CamInBasePose);
            if (hTuple != null)
            {
                if (PoseToXYZUVW(hTuple, out Single sx, out Single sy, out Single sz, out Single su, out Single sv, out Single sw))
                {
                    x = sx;
                    y = sy;
                    z = sz;
                    u = su;
                    v = sv;
                    w = sw;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 计算像素在标定后的3D世界坐标
        /// </summary>
        /// <param name="calibMode">定位模式</param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="toolInBasePose">机械手当前位置</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>成功返回true</returns>
        public bool Run(CalibMode calibMode, HTuple row, HTuple col, HTuple toolInBasePose, out HTuple x, out HTuple y, HTuple phi = null)
        {
            return Run(calibMode, row, col, toolInBasePose, out x, out y, out HTuple z, out HTuple u, out HTuple v, out HTuple w, phi);
        }

        /// <summary>
        /// 计算动态Tool
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="row2"></param>
        /// <param name="col2"></param>
        /// <param name="toolInBasePose">机械手位置</param>
        /// <returns></returns>
        public bool RunSetTool(HTuple row, HTuple col, HTuple toolInBasePose, HalconRun halcon)
        {
            try
            {
                HOperatorSet.ImagePointsToWorldPlane(tCamParam, tCalibInCamPose, row, col, "m", out HTuple x1, out HTuple y1);
                HTuple angle = 0;
                if (x1.Length > 2)
                {
                    HOperatorSet.AngleLl(0, 0, 0.1, 0, x1[1], y1[1], x1[2], y1[2], out angle);
                }
                HOperatorSet.CreatePose(x1[0], y1[0], 0, 0, 0, angle.TupleDeg(), "Rp+T", "abg", "point", out HTuple Tool1InCaliPose);
                HOperatorSet.ConvertPoseType(Tool1InCaliPose, "Rp+T", "gba", "point", out Tool1InCaliPose);
                HOperatorSet.PoseCompose(tCalibInCamPose, Tool1InCaliPose, out HTuple Tool1InCamPose);
                HOperatorSet.ConvertPoseType(Tool1InCamPose, "Rp+T", "abg", "point", out Tool1InCamPose);
                Disp3DCoordSystem(TCamParam, Tool1InCamPose, 0.05, halcon);
                HOperatorSet.PoseInvert(toolInBasePose, out HTuple BaseInTool0Pose);
                HOperatorSet.PoseInvert(tBaseInCamPose, out HTuple CamInBasePose);
                HOperatorSet.PoseCompose(BaseInTool0Pose, CamInBasePose, out HTuple CamInTool0Pose);
                HOperatorSet.PoseCompose(CamInTool0Pose, Tool1InCamPose, out CamInTool0Pose);
                Tool1Base = CamInTool0Pose;
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }
        /// <summary>
        /// Pose转3D
        /// </summary>
        /// <param name="pose"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="w"></param>
        /// <returns>成功返回Ture</returns>
        public bool PoseToXYZUVW(HTuple pose, out Single x, out Single y, out Single z, out Single u, out Single v, out Single w)
        {
            x = y = z = u = v = w = 0;
            try
            {
                if (pose.Length >= 7)
                {
                    x = (Single)pose.TupleSelect(0).TupleMult(1000).D;
                    y = (Single)pose.TupleSelect(1).TupleMult(1000).D;
                    z = (Single)pose.TupleSelect(2).TupleMult(1000).D;
                    u = (Single)pose.TupleSelect(5).D;
                    if (u > 180)
                    {
                        u = u - 360;
                    }
                    v = (Single)pose.TupleSelect(4).D;
                    if (v > 180)
                    {
                        v = v - 360;
                    }
                    w = (Single)pose.TupleSelect(3).D;
                    if (w > 180)
                    {
                        w = w - 360;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
            }
            return false;
        }
        /// <summary>
        /// 合并2个位置获得绝对位置
        /// </summary>
        /// <param name="poseTool">抓取物体Tool</param>
        /// <param name="objpose">反正绝对位置Pose</param>
        /// <returns>返回绝对位置,为Null时计算失败</returns>
        public HTuple PoseComp(HTuple poseTool, HTuple objpose)
        {
            HTuple hTuple = null;
            try
            {
                HOperatorSet.PoseInvert(poseTool, out hTuple);
                HOperatorSet.PoseCompose(objpose, hTuple, out hTuple);
            }
            catch (Exception)
            {
            }
            return hTuple;
        }

        /// <summary>
        /// 标定单张图像
        /// </summary>
        /// <param name="image">图像</param>
        /// <param name="ToolInBasePose">基础3D位置</param>
        /// <param name="i">标定的序号，第几张</param>
        public HObject RunCalib(HObject image, HTuple ToolInBasePose, int i, bool misT, HTuple hWindowID = null)
        {
            HObject hObject = new HObject();
            hObject.GenEmptyObj();
            HObject corss = new HObject();
            try
            {
                HOperatorSet.FindCalibObject(image, calibDataID, 0, 0, i, new HTuple(), new HTuple());
                HOperatorSet.GetCalibDataObservContours(out hObject, calibDataID, "caltab", 0, 0, i);
                HOperatorSet.GetCalibDataObservPoints(calibDataID, 0, 0, i, out HTuple row, out HTuple col, out HTuple ind, out HTuple pose);
                HOperatorSet.GenCrossContourXld(out corss, row, col, 40, 0);
                HOperatorSet.SetCalibData(calibDataID, "tool", i, "tool_in_base_pose", ToolInBasePose);
                if (hWindowID != null)
                {
                    HOperatorSet.SetColor(hWindowID, "red");
                    HOperatorSet.DispObj(hObject, hWindowID);
                    HOperatorSet.SetColor(hWindowID, "green");
                    HOperatorSet.DispObj(corss, hWindowID);
                    if (misT)
                    {
                        Disp3DCoordSystem(TCamParam, pose, 0.01, out this.ObjectX, out this.ObjectY, out this.ObjectZ);
                    }
                    else
                    {
                        Disp3DCoordSystem(camParam, pose, 0.01, out this.ObjectX, out this.ObjectY, out this.ObjectZ);
                    }
                    HOperatorSet.DispObj(ObjectX, hWindowID);
                    HOperatorSet.DispObj(ObjectY, hWindowID);
                    HOperatorSet.DispObj(ObjectZ, hWindowID);
                    //HOperatorSet.SetColor(hWindowID, "rad");
                    this.Object = hObject.ConcatObj(corss);
                }
            }
            catch (Exception ex)
            {
                Vision.Disp_message(hWindowID, "标定失败" + ex.Message);
                Object = null;
            }
            return Object;
        }
        /// <summary>
        /// 合并翻转坐标
        /// </summary>
        /// <param name="CalibObjInBasePose">标定参考机器人坐标</param>
        /// <param name="ToolInCamPose">相机工具坐标</param>
        /// <param name="ToolInBasePose">机器人坐标</param>
        /// <returns></returns>
        public static HTuple calc_calplate_pose_movingcam(HTuple CalibObjInBasePose, HTuple ToolInCamPose, HTuple ToolInBasePose)
        {
            HTuple hTuple = new HTuple();
            HOperatorSet.PoseInvert(ToolInBasePose, out hTuple);
            HOperatorSet.PoseCompose(ToolInCamPose, hTuple, out HTuple BaseInCamPose);
            HOperatorSet.PoseCompose(BaseInCamPose, CalibObjInBasePose, out hTuple);
            HOperatorSet.ConvertPoseType(hTuple, "Rp+T", "gba", "point", out hTuple);
            return hTuple;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static HTuple Calc_calplate_pose_stationarycam(HTuple ObjInToolPose, HTuple BaseInCamPose, HTuple ToolInBasePose)
        {
            HTuple ToolInCamPose = new HTuple();
            HOperatorSet.PoseCompose(BaseInCamPose, ToolInBasePose, out ToolInCamPose);
            HOperatorSet.PoseCompose(ToolInCamPose, ObjInToolPose, out ToolInCamPose);
            return ToolInCamPose;
        }
        /// <summary>
        /// 检测是校准是否合格
        /// </summary>
        /// <param name="calibDataID">校准数据模型的句柄。</param>
        /// <param name="RotationTolerance">旋转公差弧度，建议值: 0.02, 0.03, 0.04, 0.05, 0.06, 0.08, 0.1</param>
        /// <param name="TranslationTolerance">输入的平移部分的公差构成[m]。建议值: [0.001,0.002,0.003,0.004,0.005,0.006,0.007,0.008,0.009,0.01]</param>
        /// <returns>返回信息，准确返回空</returns>
        public static string Check_hand_eye_calibration_input_poses(int calibDataID, double RotationTolerance, double TranslationTolerance)
        {
            string dat = "";
            return dat;
        }

        // Chapter: Calibration / Camera Parameters
        // Short Description: Get the value of a specified camera parameter from the camera parameter tuple. 
        public static void Get_cam_par_data(HTuple hv_CameraParam, HTuple hv_ParamName, out HTuple hv_ParamValue)
        {



            // Local iconic variables 

            // Local control variables 

            HTuple hv_CameraType = null, hv_CameraParamNames = null;
            HTuple hv_Index = null, hv_ParamNameInd = new HTuple();
            HTuple hv_I = new HTuple();
            // Initialize local and output iconic variables 
            //get_cam_par_data returns in ParamValue the value of the
            //parameter that is given in ParamName from the tuple of
            //camera parameters that is given in CameraParam.
            //
            //Get the parameter names that correspond to the
            //elements in the input camera parameter tuple.
            get_cam_par_names(hv_CameraParam, out hv_CameraType, out hv_CameraParamNames);
            //
            //Find the index of the requested camera data and return
            //the corresponding value.
            hv_ParamValue = new HTuple();
            for (hv_Index = 0; (int)hv_Index <= (int)((new HTuple(hv_ParamName.TupleLength())) - 1); hv_Index = (int)hv_Index + 1)
            {
                hv_ParamNameInd = hv_ParamName.TupleSelect(hv_Index);
                if ((int)(new HTuple(hv_ParamNameInd.TupleEqual("camera_type"))) != 0)
                {
                    hv_ParamValue = hv_ParamValue.TupleConcat(hv_CameraType);
                    continue;
                }
                hv_I = hv_CameraParamNames.TupleFind(hv_ParamNameInd);
                if ((int)(new HTuple(hv_I.TupleNotEqual(-1))) != 0)
                {
                    hv_ParamValue = hv_ParamValue.TupleConcat(hv_CameraParam.TupleSelect(hv_I));
                }
                else
                {
                    throw new HalconException("Unknown camera parameter " + hv_ParamNameInd);
                }
            }

            return;
        }

        // Chapter: Calibration / Camera Parameters
        // Short Description: Get the names of the parameters in a camera parameter tuple. 
        public static void get_cam_par_names(HTuple hv_CameraParam, out HTuple hv_CameraType,
            out HTuple hv_ParamNames)
        {
            // Local iconic variables 
            // Local control variables 
            HTuple hv_CameraParamAreaScanDivision = null;
            HTuple hv_CameraParamAreaScanPolynomial = null, hv_CameraParamAreaScanTelecentricDivision = null;
            HTuple hv_CameraParamAreaScanTelecentricPolynomial = null;
            HTuple hv_CameraParamAreaScanTiltDivision = null, hv_CameraParamAreaScanTiltPolynomial = null;
            HTuple hv_CameraParamAreaScanImageSideTelecentricTiltDivision = null;
            HTuple hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial = null;
            HTuple hv_CameraParamAreaScanBilateralTelecentricTiltDivision = null;
            HTuple hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial = null;
            HTuple hv_CameraParamAreaScanObjectSideTelecentricTiltDivision = null;
            HTuple hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial = null;
            HTuple hv_CameraParamLinesScan = null, hv_CameraParamAreaScanTiltDivisionLegacy = null;
            HTuple hv_CameraParamAreaScanTiltPolynomialLegacy = null;
            HTuple hv_CameraParamAreaScanTelecentricDivisionLegacy = null;
            HTuple hv_CameraParamAreaScanTelecentricPolynomialLegacy = null;
            HTuple hv_CameraParamAreaScanBilateralTelecentricTiltDivisionLegacy = null;
            HTuple hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy = null;
            // Initialize local and output iconic variables 
            hv_CameraType = new HTuple();
            hv_ParamNames = new HTuple();
            //get_cam_par_names returns for each element in the camera
            //parameter tuple that is passed in CameraParam the name
            //of the respective camera parameter. The parameter names
            //are returned in ParamNames. Additionally, the camera
            //type is returned in CameraType. Alternatively, instead of
            //the camera parameters, the camera type can be passed in
            //CameraParam in form of one of the following strings:
            //  - 'area_scan_division'
            //  - 'area_scan_polynomial'
            //  - 'area_scan_tilt_division'
            //  - 'area_scan_tilt_polynomial'
            //  - 'area_scan_telecentric_division'
            //  - 'area_scan_telecentric_polynomial'
            //  - 'area_scan_tilt_bilateral_telecentric_division'
            //  - 'area_scan_tilt_bilateral_telecentric_polynomial'
            //  - 'area_scan_tilt_object_side_telecentric_division'
            //  - 'area_scan_tilt_object_side_telecentric_polynomial'
            //  - 'line_scan'
            //
            hv_CameraParamAreaScanDivision = new HTuple();
            hv_CameraParamAreaScanDivision[0] = "focus";
            hv_CameraParamAreaScanDivision[1] = "kappa";
            hv_CameraParamAreaScanDivision[2] = "sx";
            hv_CameraParamAreaScanDivision[3] = "sy";
            hv_CameraParamAreaScanDivision[4] = "cx";
            hv_CameraParamAreaScanDivision[5] = "cy";
            hv_CameraParamAreaScanDivision[6] = "image_width";
            hv_CameraParamAreaScanDivision[7] = "image_height";
            hv_CameraParamAreaScanPolynomial = new HTuple();
            hv_CameraParamAreaScanPolynomial[0] = "focus";
            hv_CameraParamAreaScanPolynomial[1] = "k1";
            hv_CameraParamAreaScanPolynomial[2] = "k2";
            hv_CameraParamAreaScanPolynomial[3] = "k3";
            hv_CameraParamAreaScanPolynomial[4] = "p1";
            hv_CameraParamAreaScanPolynomial[5] = "p2";
            hv_CameraParamAreaScanPolynomial[6] = "sx";
            hv_CameraParamAreaScanPolynomial[7] = "sy";
            hv_CameraParamAreaScanPolynomial[8] = "cx";
            hv_CameraParamAreaScanPolynomial[9] = "cy";
            hv_CameraParamAreaScanPolynomial[10] = "image_width";
            hv_CameraParamAreaScanPolynomial[11] = "image_height";
            hv_CameraParamAreaScanTelecentricDivision = new HTuple();
            hv_CameraParamAreaScanTelecentricDivision[0] = "magnification";
            hv_CameraParamAreaScanTelecentricDivision[1] = "kappa";
            hv_CameraParamAreaScanTelecentricDivision[2] = "sx";
            hv_CameraParamAreaScanTelecentricDivision[3] = "sy";
            hv_CameraParamAreaScanTelecentricDivision[4] = "cx";
            hv_CameraParamAreaScanTelecentricDivision[5] = "cy";
            hv_CameraParamAreaScanTelecentricDivision[6] = "image_width";
            hv_CameraParamAreaScanTelecentricDivision[7] = "image_height";
            hv_CameraParamAreaScanTelecentricPolynomial = new HTuple();
            hv_CameraParamAreaScanTelecentricPolynomial[0] = "magnification";
            hv_CameraParamAreaScanTelecentricPolynomial[1] = "k1";
            hv_CameraParamAreaScanTelecentricPolynomial[2] = "k2";
            hv_CameraParamAreaScanTelecentricPolynomial[3] = "k3";
            hv_CameraParamAreaScanTelecentricPolynomial[4] = "p1";
            hv_CameraParamAreaScanTelecentricPolynomial[5] = "p2";
            hv_CameraParamAreaScanTelecentricPolynomial[6] = "sx";
            hv_CameraParamAreaScanTelecentricPolynomial[7] = "sy";
            hv_CameraParamAreaScanTelecentricPolynomial[8] = "cx";
            hv_CameraParamAreaScanTelecentricPolynomial[9] = "cy";
            hv_CameraParamAreaScanTelecentricPolynomial[10] = "image_width";
            hv_CameraParamAreaScanTelecentricPolynomial[11] = "image_height";
            hv_CameraParamAreaScanTiltDivision = new HTuple();
            hv_CameraParamAreaScanTiltDivision[0] = "focus";
            hv_CameraParamAreaScanTiltDivision[1] = "kappa";
            hv_CameraParamAreaScanTiltDivision[2] = "image_plane_dist";
            hv_CameraParamAreaScanTiltDivision[3] = "tilt";
            hv_CameraParamAreaScanTiltDivision[4] = "rot";
            hv_CameraParamAreaScanTiltDivision[5] = "sx";
            hv_CameraParamAreaScanTiltDivision[6] = "sy";
            hv_CameraParamAreaScanTiltDivision[7] = "cx";
            hv_CameraParamAreaScanTiltDivision[8] = "cy";
            hv_CameraParamAreaScanTiltDivision[9] = "image_width";
            hv_CameraParamAreaScanTiltDivision[10] = "image_height";
            hv_CameraParamAreaScanTiltPolynomial = new HTuple();
            hv_CameraParamAreaScanTiltPolynomial[0] = "focus";
            hv_CameraParamAreaScanTiltPolynomial[1] = "k1";
            hv_CameraParamAreaScanTiltPolynomial[2] = "k2";
            hv_CameraParamAreaScanTiltPolynomial[3] = "k3";
            hv_CameraParamAreaScanTiltPolynomial[4] = "p1";
            hv_CameraParamAreaScanTiltPolynomial[5] = "p2";
            hv_CameraParamAreaScanTiltPolynomial[6] = "image_plane_dist";
            hv_CameraParamAreaScanTiltPolynomial[7] = "tilt";
            hv_CameraParamAreaScanTiltPolynomial[8] = "rot";
            hv_CameraParamAreaScanTiltPolynomial[9] = "sx";
            hv_CameraParamAreaScanTiltPolynomial[10] = "sy";
            hv_CameraParamAreaScanTiltPolynomial[11] = "cx";
            hv_CameraParamAreaScanTiltPolynomial[12] = "cy";
            hv_CameraParamAreaScanTiltPolynomial[13] = "image_width";
            hv_CameraParamAreaScanTiltPolynomial[14] = "image_height";
            hv_CameraParamAreaScanImageSideTelecentricTiltDivision = new HTuple();
            hv_CameraParamAreaScanImageSideTelecentricTiltDivision[0] = "focus";
            hv_CameraParamAreaScanImageSideTelecentricTiltDivision[1] = "kappa";
            hv_CameraParamAreaScanImageSideTelecentricTiltDivision[2] = "tilt";
            hv_CameraParamAreaScanImageSideTelecentricTiltDivision[3] = "rot";
            hv_CameraParamAreaScanImageSideTelecentricTiltDivision[4] = "sx";
            hv_CameraParamAreaScanImageSideTelecentricTiltDivision[5] = "sy";
            hv_CameraParamAreaScanImageSideTelecentricTiltDivision[6] = "cx";
            hv_CameraParamAreaScanImageSideTelecentricTiltDivision[7] = "cy";
            hv_CameraParamAreaScanImageSideTelecentricTiltDivision[8] = "image_width";
            hv_CameraParamAreaScanImageSideTelecentricTiltDivision[9] = "image_height";
            hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial = new HTuple();
            hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial[0] = "focus";
            hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial[1] = "k1";
            hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial[2] = "k2";
            hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial[3] = "k3";
            hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial[4] = "p1";
            hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial[5] = "p2";
            hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial[6] = "tilt";
            hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial[7] = "rot";
            hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial[8] = "sx";
            hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial[9] = "sy";
            hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial[10] = "cx";
            hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial[11] = "cy";
            hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial[12] = "image_width";
            hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial[13] = "image_height";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivision = new HTuple();
            hv_CameraParamAreaScanBilateralTelecentricTiltDivision[0] = "magnification";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivision[1] = "kappa";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivision[2] = "tilt";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivision[3] = "rot";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivision[4] = "sx";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivision[5] = "sy";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivision[6] = "cx";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivision[7] = "cy";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivision[8] = "image_width";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivision[9] = "image_height";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial = new HTuple();
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial[0] = "magnification";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial[1] = "k1";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial[2] = "k2";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial[3] = "k3";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial[4] = "p1";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial[5] = "p2";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial[6] = "tilt";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial[7] = "rot";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial[8] = "sx";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial[9] = "sy";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial[10] = "cx";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial[11] = "cy";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial[12] = "image_width";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial[13] = "image_height";
            hv_CameraParamAreaScanObjectSideTelecentricTiltDivision = new HTuple();
            hv_CameraParamAreaScanObjectSideTelecentricTiltDivision[0] = "magnification";
            hv_CameraParamAreaScanObjectSideTelecentricTiltDivision[1] = "kappa";
            hv_CameraParamAreaScanObjectSideTelecentricTiltDivision[2] = "image_plane_dist";
            hv_CameraParamAreaScanObjectSideTelecentricTiltDivision[3] = "tilt";
            hv_CameraParamAreaScanObjectSideTelecentricTiltDivision[4] = "rot";
            hv_CameraParamAreaScanObjectSideTelecentricTiltDivision[5] = "sx";
            hv_CameraParamAreaScanObjectSideTelecentricTiltDivision[6] = "sy";
            hv_CameraParamAreaScanObjectSideTelecentricTiltDivision[7] = "cx";
            hv_CameraParamAreaScanObjectSideTelecentricTiltDivision[8] = "cy";
            hv_CameraParamAreaScanObjectSideTelecentricTiltDivision[9] = "image_width";
            hv_CameraParamAreaScanObjectSideTelecentricTiltDivision[10] = "image_height";
            hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial = new HTuple();
            hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial[0] = "magnification";
            hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial[1] = "k1";
            hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial[2] = "k2";
            hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial[3] = "k3";
            hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial[4] = "p1";
            hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial[5] = "p2";
            hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial[6] = "image_plane_dist";
            hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial[7] = "tilt";
            hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial[8] = "rot";
            hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial[9] = "sx";
            hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial[10] = "sy";
            hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial[11] = "cx";
            hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial[12] = "cy";
            hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial[13] = "image_width";
            hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial[14] = "image_height";
            hv_CameraParamLinesScan = new HTuple();
            hv_CameraParamLinesScan[0] = "focus";
            hv_CameraParamLinesScan[1] = "kappa";
            hv_CameraParamLinesScan[2] = "sx";
            hv_CameraParamLinesScan[3] = "sy";
            hv_CameraParamLinesScan[4] = "cx";
            hv_CameraParamLinesScan[5] = "cy";
            hv_CameraParamLinesScan[6] = "image_width";
            hv_CameraParamLinesScan[7] = "image_height";
            hv_CameraParamLinesScan[8] = "vx";
            hv_CameraParamLinesScan[9] = "vy";
            hv_CameraParamLinesScan[10] = "vz";
            //Legacy parameter names
            hv_CameraParamAreaScanTiltDivisionLegacy = new HTuple();
            hv_CameraParamAreaScanTiltDivisionLegacy[0] = "focus";
            hv_CameraParamAreaScanTiltDivisionLegacy[1] = "kappa";
            hv_CameraParamAreaScanTiltDivisionLegacy[2] = "tilt";
            hv_CameraParamAreaScanTiltDivisionLegacy[3] = "rot";
            hv_CameraParamAreaScanTiltDivisionLegacy[4] = "sx";
            hv_CameraParamAreaScanTiltDivisionLegacy[5] = "sy";
            hv_CameraParamAreaScanTiltDivisionLegacy[6] = "cx";
            hv_CameraParamAreaScanTiltDivisionLegacy[7] = "cy";
            hv_CameraParamAreaScanTiltDivisionLegacy[8] = "image_width";
            hv_CameraParamAreaScanTiltDivisionLegacy[9] = "image_height";
            hv_CameraParamAreaScanTiltPolynomialLegacy = new HTuple();
            hv_CameraParamAreaScanTiltPolynomialLegacy[0] = "focus";
            hv_CameraParamAreaScanTiltPolynomialLegacy[1] = "k1";
            hv_CameraParamAreaScanTiltPolynomialLegacy[2] = "k2";
            hv_CameraParamAreaScanTiltPolynomialLegacy[3] = "k3";
            hv_CameraParamAreaScanTiltPolynomialLegacy[4] = "p1";
            hv_CameraParamAreaScanTiltPolynomialLegacy[5] = "p2";
            hv_CameraParamAreaScanTiltPolynomialLegacy[6] = "tilt";
            hv_CameraParamAreaScanTiltPolynomialLegacy[7] = "rot";
            hv_CameraParamAreaScanTiltPolynomialLegacy[8] = "sx";
            hv_CameraParamAreaScanTiltPolynomialLegacy[9] = "sy";
            hv_CameraParamAreaScanTiltPolynomialLegacy[10] = "cx";
            hv_CameraParamAreaScanTiltPolynomialLegacy[11] = "cy";
            hv_CameraParamAreaScanTiltPolynomialLegacy[12] = "image_width";
            hv_CameraParamAreaScanTiltPolynomialLegacy[13] = "image_height";
            hv_CameraParamAreaScanTelecentricDivisionLegacy = new HTuple();
            hv_CameraParamAreaScanTelecentricDivisionLegacy[0] = "focus";
            hv_CameraParamAreaScanTelecentricDivisionLegacy[1] = "kappa";
            hv_CameraParamAreaScanTelecentricDivisionLegacy[2] = "sx";
            hv_CameraParamAreaScanTelecentricDivisionLegacy[3] = "sy";
            hv_CameraParamAreaScanTelecentricDivisionLegacy[4] = "cx";
            hv_CameraParamAreaScanTelecentricDivisionLegacy[5] = "cy";
            hv_CameraParamAreaScanTelecentricDivisionLegacy[6] = "image_width";
            hv_CameraParamAreaScanTelecentricDivisionLegacy[7] = "image_height";
            hv_CameraParamAreaScanTelecentricPolynomialLegacy = new HTuple();
            hv_CameraParamAreaScanTelecentricPolynomialLegacy[0] = "focus";
            hv_CameraParamAreaScanTelecentricPolynomialLegacy[1] = "k1";
            hv_CameraParamAreaScanTelecentricPolynomialLegacy[2] = "k2";
            hv_CameraParamAreaScanTelecentricPolynomialLegacy[3] = "k3";
            hv_CameraParamAreaScanTelecentricPolynomialLegacy[4] = "p1";
            hv_CameraParamAreaScanTelecentricPolynomialLegacy[5] = "p2";
            hv_CameraParamAreaScanTelecentricPolynomialLegacy[6] = "sx";
            hv_CameraParamAreaScanTelecentricPolynomialLegacy[7] = "sy";
            hv_CameraParamAreaScanTelecentricPolynomialLegacy[8] = "cx";
            hv_CameraParamAreaScanTelecentricPolynomialLegacy[9] = "cy";
            hv_CameraParamAreaScanTelecentricPolynomialLegacy[10] = "image_width";
            hv_CameraParamAreaScanTelecentricPolynomialLegacy[11] = "image_height";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivisionLegacy = new HTuple();
            hv_CameraParamAreaScanBilateralTelecentricTiltDivisionLegacy[0] = "focus";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivisionLegacy[1] = "kappa";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivisionLegacy[2] = "tilt";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivisionLegacy[3] = "rot";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivisionLegacy[4] = "sx";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivisionLegacy[5] = "sy";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivisionLegacy[6] = "cx";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivisionLegacy[7] = "cy";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivisionLegacy[8] = "image_width";
            hv_CameraParamAreaScanBilateralTelecentricTiltDivisionLegacy[9] = "image_height";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy = new HTuple();
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy[0] = "focus";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy[1] = "k1";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy[2] = "k2";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy[3] = "k3";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy[4] = "p1";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy[5] = "p2";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy[6] = "tilt";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy[7] = "rot";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy[8] = "sx";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy[9] = "sy";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy[10] = "cx";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy[11] = "cy";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy[12] = "image_width";
            hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy[13] = "image_height";
            //
            //If the camera type is passed in CameraParam
            if ((int)((new HTuple((new HTuple(hv_CameraParam.TupleLength())).TupleEqual(1))).TupleAnd(
                ((hv_CameraParam.TupleSelect(0))).TupleIsString())) != 0)
            {
                hv_CameraType = hv_CameraParam.TupleSelect(0);
                if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_division"))) != 0)
                {
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanDivision);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_polynomial"))) != 0)
                {
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanPolynomial);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_telecentric_division"))) != 0)
                {
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanTelecentricDivision);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_telecentric_polynomial"))) != 0)
                {
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanTelecentricPolynomial);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_tilt_division"))) != 0)
                {
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanTiltDivision);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_tilt_polynomial"))) != 0)
                {
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanTiltPolynomial);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_tilt_image_side_telecentric_division"))) != 0)
                {
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanImageSideTelecentricTiltDivision);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_tilt_image_side_telecentric_polynomial"))) != 0)
                {
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_tilt_bilateral_telecentric_division"))) != 0)
                {
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanBilateralTelecentricTiltDivision);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_tilt_bilateral_telecentric_polynomial"))) != 0)
                {
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_tilt_object_side_telecentric_division"))) != 0)
                {
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanObjectSideTelecentricTiltDivision);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_tilt_object_side_telecentric_polynomial"))) != 0)
                {
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("line_scan"))) != 0)
                {
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamLinesScan);
                }
                else
                {
                    throw new HalconException(("Unknown camera type '" + hv_CameraType) + "' passed in CameraParam.");
                }

                return;
            }
            //
            //If the camera parameters are passed in CameraParam
            if ((int)(((((hv_CameraParam.TupleSelect(0))).TupleIsString())).TupleNot()) != 0)
            {
                //Format of camera parameters for HALCON 12 and earlier
                switch ((new HTuple(hv_CameraParam.TupleLength()
                    )).I)
                {
                    //
                    //Area Scan
                    case 8:
                        //CameraType: 'area_scan_division' or 'area_scan_telecentric_division'
                        if ((int)(new HTuple(((hv_CameraParam.TupleSelect(0))).TupleNotEqual(0.0))) != 0)
                        {
                            hv_ParamNames = hv_CameraParamAreaScanDivision.Clone();
                            hv_CameraType = "area_scan_division";
                        }
                        else
                        {
                            hv_ParamNames = hv_CameraParamAreaScanTelecentricDivisionLegacy.Clone();
                            hv_CameraType = "area_scan_telecentric_division";
                        }
                        break;
                    case 10:
                        //CameraType: 'area_scan_tilt_division' or 'area_scan_telecentric_tilt_division'
                        if ((int)(new HTuple(((hv_CameraParam.TupleSelect(0))).TupleNotEqual(0.0))) != 0)
                        {
                            hv_ParamNames = hv_CameraParamAreaScanTiltDivisionLegacy.Clone();
                            hv_CameraType = "area_scan_tilt_division";
                        }
                        else
                        {
                            hv_ParamNames = hv_CameraParamAreaScanBilateralTelecentricTiltDivisionLegacy.Clone();
                            hv_CameraType = "area_scan_tilt_bilateral_telecentric_division";
                        }
                        break;
                    case 12:
                        //CameraType: 'area_scan_polynomial' or 'area_scan_telecentric_polynomial'
                        if ((int)(new HTuple(((hv_CameraParam.TupleSelect(0))).TupleNotEqual(0.0))) != 0)
                        {
                            hv_ParamNames = hv_CameraParamAreaScanPolynomial.Clone();
                            hv_CameraType = "area_scan_polynomial";
                        }
                        else
                        {
                            hv_ParamNames = hv_CameraParamAreaScanTelecentricPolynomialLegacy.Clone();
                            hv_CameraType = "area_scan_telecentric_polynomial";
                        }
                        break;
                    case 14:
                        //CameraType: 'area_scan_tilt_polynomial' or 'area_scan_telecentric_tilt_polynomial'
                        if ((int)(new HTuple(((hv_CameraParam.TupleSelect(0))).TupleNotEqual(0.0))) != 0)
                        {
                            hv_ParamNames = hv_CameraParamAreaScanTiltPolynomialLegacy.Clone();
                            hv_CameraType = "area_scan_tilt_polynomial";
                        }
                        else
                        {
                            hv_ParamNames = hv_CameraParamAreaScanBilateralTelecentricTiltPolynomialLegacy.Clone();
                            hv_CameraType = "area_scan_tilt_bilateral_telecentric_polynomial";
                        }
                        break;
                    //
                    //Line Scan
                    case 11:
                        //CameraType: 'line_scan'
                        hv_ParamNames = hv_CameraParamLinesScan.Clone();
                        hv_CameraType = "line_scan";
                        break;
                    default:
                        throw new HalconException("Wrong number of values in CameraParam.");

                }
            }
            else
            {
                //Format of camera parameters since HALCON 13
                hv_CameraType = hv_CameraParam.TupleSelect(0);
                if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_division"))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_CameraParam.TupleLength())).TupleNotEqual(
                        9))) != 0)
                    {
                        throw new HalconException("Wrong number of values in CameraParam.");
                    }
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanDivision);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_polynomial"))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_CameraParam.TupleLength())).TupleNotEqual(
                        13))) != 0)
                    {
                        throw new HalconException("Wrong number of values in CameraParam.");
                    }
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanPolynomial);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_telecentric_division"))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_CameraParam.TupleLength())).TupleNotEqual(
                        9))) != 0)
                    {
                        throw new HalconException("Wrong number of values in CameraParam.");
                    }
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanTelecentricDivision);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_telecentric_polynomial"))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_CameraParam.TupleLength())).TupleNotEqual(
                        13))) != 0)
                    {
                        throw new HalconException("Wrong number of values in CameraParam.");
                    }
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanTelecentricPolynomial);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_tilt_division"))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_CameraParam.TupleLength())).TupleNotEqual(
                        12))) != 0)
                    {
                        throw new HalconException("Wrong number of values in CameraParam.");
                    }
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanTiltDivision);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_tilt_polynomial"))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_CameraParam.TupleLength())).TupleNotEqual(
                        16))) != 0)
                    {
                        throw new HalconException("Wrong number of values in CameraParam.");
                    }
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanTiltPolynomial);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_tilt_image_side_telecentric_division"))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_CameraParam.TupleLength())).TupleNotEqual(
                        11))) != 0)
                    {
                        throw new HalconException("Wrong number of values in CameraParam.");
                    }
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanImageSideTelecentricTiltDivision);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_tilt_image_side_telecentric_polynomial"))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_CameraParam.TupleLength())).TupleNotEqual(
                        15))) != 0)
                    {
                        throw new HalconException("Wrong number of values in CameraParam.");
                    }
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanImageSideTelecentricTiltPolynomial);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_tilt_bilateral_telecentric_division"))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_CameraParam.TupleLength())).TupleNotEqual(
                        11))) != 0)
                    {
                        throw new HalconException("Wrong number of values in CameraParam.");
                    }
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanBilateralTelecentricTiltDivision);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_tilt_bilateral_telecentric_polynomial"))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_CameraParam.TupleLength())).TupleNotEqual(
                        15))) != 0)
                    {
                        throw new HalconException("Wrong number of values in CameraParam.");
                    }
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanBilateralTelecentricTiltPolynomial);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_tilt_object_side_telecentric_division"))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_CameraParam.TupleLength())).TupleNotEqual(
                        12))) != 0)
                    {
                        throw new HalconException("Wrong number of values in CameraParam.");
                    }
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanObjectSideTelecentricTiltDivision);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("area_scan_tilt_object_side_telecentric_polynomial"))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_CameraParam.TupleLength())).TupleNotEqual(
                        16))) != 0)
                    {
                        throw new HalconException("Wrong number of values in CameraParam.");
                    }
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamAreaScanObjectSideTelecentricTiltPolynomial);
                }
                else if ((int)(new HTuple(hv_CameraType.TupleEqual("line_scan"))) != 0)
                {
                    if ((int)(new HTuple((new HTuple(hv_CameraParam.TupleLength())).TupleNotEqual(
                        12))) != 0)
                    {
                        throw new HalconException("Wrong number of values in CameraParam.");
                    }
                    hv_ParamNames = new HTuple();
                    hv_ParamNames[0] = "camera_type";
                    hv_ParamNames = hv_ParamNames.TupleConcat(hv_CameraParamLinesScan);
                }
                else
                {
                    throw new HalconException("Unknown camera type in CameraParam.");
                }
            }

            return;
        }

        // Chapter: Graphics / Output
        // Short Description: Display the axes of a 3d coordinate system 
        public static HObject Disp3DCoordSystem(HTuple hv_CamParam, HTuple hv_Pose,
            HTuple hv_CoordAxesLength, HTuple hv_WindowHandle = null)
        {
            // Local iconic variables 
            // Local control variables 
            HTuple hv_CameraType = null, hv_IsTelecentric = null;
            HTuple hv_TransWorld2Cam = null, hv_OrigCamX = null, hv_OrigCamY = null;
            HTuple hv_OrigCamZ = null, hv_Row0 = null, hv_Column0 = null;
            HTuple hv_X = null, hv_Y = null, hv_Z = null, hv_RowAxX = null;
            HTuple hv_ColumnAxX = null, hv_RowAxY = null, hv_ColumnAxY = null;
            HTuple hv_RowAxZ = null, hv_ColumnAxZ = null, hv_Distance = null;
            HTuple hv_HeadLength = null;
            // Initialize local and output iconic variables 

            try
            {
                //This procedure displays a 3D coordinate system.
                //It needs the procedure gen_arrow_contour_xld.
                //
                //Input parameters:
                //WindowHandle: The window where the coordinate system shall be displayed
                //CamParam: The camera paramters
                //Pose: The pose to be displayed
                //CoordAxesLength: The length of the coordinate axes in world coordinates
                //
                //Check, if Pose is a correct pose tuple.
                if ((int)(new HTuple((new HTuple(hv_Pose.TupleLength())).TupleNotEqual(7))) != 0)
                {
                    //ho_Arrows.Dispose();

                    return new HObject();
                }
                Get_cam_par_data(hv_CamParam, "camera_type", out hv_CameraType);
                hv_IsTelecentric = new HTuple(((hv_CameraType.TupleStrstr("telecentric"))).TupleNotEqual(
                    -1));
                if ((int)((new HTuple(((hv_Pose.TupleSelect(2))).TupleEqual(0.0))).TupleAnd(
                    hv_IsTelecentric.TupleNot())) != 0)
                {
                    //For projective cameras:
                    //Poses with Z position zero cannot be projected
                    //(that would lead to a division by zero error).
                    //ho_Arrows.Dispose();
                    return new HObject();
                }
                //Convert to pose to a transformation matrix
                HOperatorSet.PoseToHomMat3d(hv_Pose, out hv_TransWorld2Cam);
                //Project the world origin into the image
                HOperatorSet.AffineTransPoint3d(hv_TransWorld2Cam, 0, 0, 0, out hv_OrigCamX,
                    out hv_OrigCamY, out hv_OrigCamZ);
                HOperatorSet.Project3dPoint(hv_OrigCamX, hv_OrigCamY, hv_OrigCamZ, hv_CamParam,
                    out hv_Row0, out hv_Column0);
                //Project the coordinate axes into the image
                HOperatorSet.AffineTransPoint3d(hv_TransWorld2Cam, hv_CoordAxesLength, 0, 0,
                    out hv_X, out hv_Y, out hv_Z);
                HOperatorSet.Project3dPoint(hv_X, hv_Y, hv_Z, hv_CamParam, out hv_RowAxX, out hv_ColumnAxX);
                HOperatorSet.AffineTransPoint3d(hv_TransWorld2Cam, 0, hv_CoordAxesLength, 0,
                    out hv_X, out hv_Y, out hv_Z);
                HOperatorSet.Project3dPoint(hv_X, hv_Y, hv_Z, hv_CamParam, out hv_RowAxY, out hv_ColumnAxY);
                HOperatorSet.AffineTransPoint3d(hv_TransWorld2Cam, 0, 0, hv_CoordAxesLength,
                    out hv_X, out hv_Y, out hv_Z);
                HOperatorSet.Project3dPoint(hv_X, hv_Y, hv_Z, hv_CamParam, out hv_RowAxZ, out hv_ColumnAxZ);
                //
                //Generate an XLD contour for each axis
                HOperatorSet.DistancePp(((hv_Row0.TupleConcat(hv_Row0))).TupleConcat(hv_Row0),
                    ((hv_Column0.TupleConcat(hv_Column0))).TupleConcat(hv_Column0), ((hv_RowAxX.TupleConcat(
                    hv_RowAxY))).TupleConcat(hv_RowAxZ), ((hv_ColumnAxX.TupleConcat(hv_ColumnAxY))).TupleConcat(
                    hv_ColumnAxZ), out hv_Distance);
                hv_HeadLength = (((((((hv_Distance.TupleMax()) / 12.0)).TupleConcat(5.0))).TupleMax()
                    )).TupleInt();

                //Vision.Gen_arrow_contour_xld(out ho_Arrows, ((hv_Row0.TupleConcat(hv_Row0))).TupleConcat(
                //      hv_Row0), ((hv_Column0.TupleConcat(hv_Column0))).TupleConcat(hv_Column0),
                //      ((hv_RowAxX.TupleConcat(hv_RowAxY))).TupleConcat(hv_RowAxZ), ((hv_ColumnAxX.TupleConcat(
                //      hv_ColumnAxY))).TupleConcat(hv_ColumnAxZ), hv_HeadLength, hv_HeadLength);

                Vision.Gen_arrow_contour_xld(out HObject ho_ArrowsX, hv_Row0, hv_Column0, hv_RowAxX, hv_ColumnAxX, hv_HeadLength, hv_HeadLength);
                Vision.Gen_arrow_contour_xld(out HObject ho_ArrowsY, hv_Row0, hv_Column0, hv_RowAxY, hv_ColumnAxY, hv_HeadLength, hv_HeadLength);
                Vision.Gen_arrow_contour_xld(out HObject ho_ArrowsZ, hv_Row0, hv_Column0, hv_RowAxZ, hv_ColumnAxZ, hv_HeadLength, hv_HeadLength);

                if (hv_WindowHandle != null)
                {
                    HOperatorSet.SetColor(hv_WindowHandle, "red");
                    HOperatorSet.DispObj(ho_ArrowsX, hv_WindowHandle);
                    HOperatorSet.SetColor(hv_WindowHandle, "green");
                    HOperatorSet.DispObj(ho_ArrowsY, hv_WindowHandle);
                    HOperatorSet.SetColor(hv_WindowHandle, "blue");
                    HOperatorSet.DispObj(ho_ArrowsZ, hv_WindowHandle);
                    Vision.Disp_message(hv_WindowHandle, "X", hv_RowAxX + 3, hv_ColumnAxX + 3, false,
              "red", "box");
                    Vision.Disp_message(hv_WindowHandle, "Y", hv_RowAxY + 3, hv_ColumnAxY + 3, false,
                        "green", "box");
                    Vision.Disp_message(hv_WindowHandle, "Z", hv_RowAxZ + 3, (hv_ColumnAxZ + 3) + 3, false,
                       "blue", "box");
                }


                return ho_ArrowsX.ConcatObj(ho_ArrowsY).ConcatObj(ho_ArrowsZ);
            }
            catch (HalconException HDevExpDefaultException)
            {
                //ho_Arrows.Dispose();

                throw HDevExpDefaultException;
            }

        }

        public static void Disp3DCoordSystem(HTuple hv_CamParam, HTuple hv_Pose,
        HTuple hv_CoordAxesLength, out HObject ho_ArrowsX, out HObject ho_ArrowsY, out HObject ho_ArrowsZ, HalconRun halconRun = null)
        {
            // Local iconic variables 
            // Local control variables 
            HTuple hv_CameraType = null, hv_IsTelecentric = null;
            HTuple hv_TransWorld2Cam = null, hv_OrigCamX = null, hv_OrigCamY = null;
            HTuple hv_OrigCamZ = null, hv_Row0 = null, hv_Column0 = null;
            HTuple hv_X = null, hv_Y = null, hv_Z = null, hv_RowAxX = null;
            HTuple hv_ColumnAxX = null, hv_RowAxY = null, hv_ColumnAxY = null;
            HTuple hv_RowAxZ = null, hv_ColumnAxZ = null, hv_Distance = null;
            HTuple hv_HeadLength = null;
            ho_ArrowsX = new HObject();
            ho_ArrowsY = new HObject();
            ho_ArrowsZ = new HObject();
            // Initialize local and output iconic variables 
            try
            {
                //
                //Check, if Pose is a correct pose tuple.
                if ((int)(new HTuple((new HTuple(hv_Pose.TupleLength())).TupleNotEqual(7))) != 0)
                {
                    //ho_Arrows.Dispose();
                    return;
                }
                Get_cam_par_data(hv_CamParam, "camera_type", out hv_CameraType);
                hv_IsTelecentric = new HTuple(((hv_CameraType.TupleStrstr("telecentric"))).TupleNotEqual(
                    -1));
                if ((int)((new HTuple(((hv_Pose.TupleSelect(2))).TupleEqual(0.0))).TupleAnd(
                    hv_IsTelecentric.TupleNot())) != 0)
                {
                    //For projective cameras:
                    //Poses with Z position zero cannot be projected
                    //(that would lead to a division by zero error).
                    //ho_Arrows.Dispose();
                    return;
                }
                //Convert to pose to a transformation matrix
                HOperatorSet.PoseToHomMat3d(hv_Pose, out hv_TransWorld2Cam);
                //Project the world origin into the image
                HOperatorSet.AffineTransPoint3d(hv_TransWorld2Cam, 0, 0, 0, out hv_OrigCamX,
                    out hv_OrigCamY, out hv_OrigCamZ);
                HOperatorSet.Project3dPoint(hv_OrigCamX, hv_OrigCamY, hv_OrigCamZ, hv_CamParam,
                    out hv_Row0, out hv_Column0);
                //Project the coordinate axes into the image
                HOperatorSet.AffineTransPoint3d(hv_TransWorld2Cam, hv_CoordAxesLength, 0, 0,
                    out hv_X, out hv_Y, out hv_Z);
                HOperatorSet.Project3dPoint(hv_X, hv_Y, hv_Z, hv_CamParam, out hv_RowAxX, out hv_ColumnAxX);
                HOperatorSet.AffineTransPoint3d(hv_TransWorld2Cam, 0, hv_CoordAxesLength, 0,
                    out hv_X, out hv_Y, out hv_Z);
                HOperatorSet.Project3dPoint(hv_X, hv_Y, hv_Z, hv_CamParam, out hv_RowAxY, out hv_ColumnAxY);
                HOperatorSet.AffineTransPoint3d(hv_TransWorld2Cam, 0, 0, hv_CoordAxesLength,
                    out hv_X, out hv_Y, out hv_Z);
                HOperatorSet.Project3dPoint(hv_X, hv_Y, hv_Z, hv_CamParam, out hv_RowAxZ, out hv_ColumnAxZ);
                //
                //Generate an XLD contour for each axis
                HOperatorSet.DistancePp(((hv_Row0.TupleConcat(hv_Row0))).TupleConcat(hv_Row0),
                    ((hv_Column0.TupleConcat(hv_Column0))).TupleConcat(hv_Column0), ((hv_RowAxX.TupleConcat(
                    hv_RowAxY))).TupleConcat(hv_RowAxZ), ((hv_ColumnAxX.TupleConcat(hv_ColumnAxY))).TupleConcat(
                    hv_ColumnAxZ), out hv_Distance);
                hv_HeadLength = (((((((hv_Distance.TupleMax()) / 12.0)).TupleConcat(5.0))).TupleMax()
                    )).TupleInt();

                //Vision.Gen_arrow_contour_xld(out ho_Arrows, ((hv_Row0.TupleConcat(hv_Row0))).TupleConcat(
                //      hv_Row0), ((hv_Column0.TupleConcat(hv_Column0))).TupleConcat(hv_Column0),
                //      ((hv_RowAxX.TupleConcat(hv_RowAxY))).TupleConcat(hv_RowAxZ), ((hv_ColumnAxX.TupleConcat(
                //      hv_ColumnAxY))).TupleConcat(hv_ColumnAxZ), hv_HeadLength, hv_HeadLength);

                Vision.Gen_arrow_contour_xld(out ho_ArrowsX, hv_Row0, hv_Column0, hv_RowAxX, hv_ColumnAxX, hv_HeadLength, hv_HeadLength);
                Vision.Gen_arrow_contour_xld(out ho_ArrowsY, hv_Row0, hv_Column0, hv_RowAxY, hv_ColumnAxY, hv_HeadLength, hv_HeadLength);
                Vision.Gen_arrow_contour_xld(out ho_ArrowsZ, hv_Row0, hv_Column0, hv_RowAxZ, hv_ColumnAxZ, hv_HeadLength, hv_HeadLength);
                if (halconRun != null)
                {
                    halconRun.GetOneImageR().AddImageMassage(hv_RowAxX + 3, hv_ColumnAxX + 3, "X", ColorResult.red);
                    halconRun.GetOneImageR().AddImageMassage(hv_RowAxY + 3, hv_ColumnAxY + 3, "Y", ColorResult.green);
                    halconRun.GetOneImageR().AddImageMassage(hv_RowAxZ + 3, hv_ColumnAxZ + 3, "Z", ColorResult.yellow);
                }
                //  if (hv_WindowHandle != null)
                //  {
                //      HOperatorSet.SetColor(hv_WindowHandle, "red");
                //      HOperatorSet.DispObj(ho_ArrowsX, hv_WindowHandle);
                //      HOperatorSet.SetColor(hv_WindowHandle, "green");
                //      HOperatorSet.DispObj(ho_ArrowsY, hv_WindowHandle);
                //      HOperatorSet.SetColor(hv_WindowHandle, "blue");
                //      HOperatorSet.DispObj(ho_ArrowsZ, hv_WindowHandle);
                //      Vision.Disp_message(hv_WindowHandle, "X", hv_RowAxX + 3, hv_ColumnAxX + 3, false,
                //"red", "box");
                //      Vision.Disp_message(hv_WindowHandle, "Y", hv_RowAxY + 3, hv_ColumnAxY + 3, false,
                //          "green", "box");
                //      Vision.Disp_message(hv_WindowHandle, "Z", hv_RowAxZ + 3, (hv_ColumnAxZ + 3) + 3, false,
                //         "blue", "box");
                //  }

            }
            catch (HalconException HDevExpDefaultException)
            {
                //ho_Arrows.Dispose();
                throw HDevExpDefaultException;
            }

        }
        public static void Disp3DCoordSystem(HTuple hv_CamParam, HTuple hv_Pose,
    HTuple hv_CoordAxesLength, HalconRun halconRun)
        {
            // Local iconic variables 
            // Local control variables 
            HTuple hv_CameraType = null, hv_IsTelecentric = null;
            HTuple hv_TransWorld2Cam = null, hv_OrigCamX = null, hv_OrigCamY = null;
            HTuple hv_OrigCamZ = null, hv_Row0 = null, hv_Column0 = null;
            HTuple hv_X = null, hv_Y = null, hv_Z = null, hv_RowAxX = null;
            HTuple hv_ColumnAxX = null, hv_RowAxY = null, hv_ColumnAxY = null;
            HTuple hv_RowAxZ = null, hv_ColumnAxZ = null, hv_Distance = null;
            HTuple hv_HeadLength = null;
            HObject ho_ArrowsX = new HObject();
            HObject ho_ArrowsY = new HObject();
            HObject ho_ArrowsZ = new HObject();
            // Initialize local and output iconic variables 
            try
            {
                //
                //Check, if Pose is a correct pose tuple.
                if ((int)(new HTuple((new HTuple(hv_Pose.TupleLength())).TupleNotEqual(7))) != 0)
                {
                    //ho_Arrows.Dispose();
                    return;
                }
                Get_cam_par_data(hv_CamParam, "camera_type", out hv_CameraType);
                hv_IsTelecentric = new HTuple(((hv_CameraType.TupleStrstr("telecentric"))).TupleNotEqual(
                    -1));
                if ((int)((new HTuple(((hv_Pose.TupleSelect(2))).TupleEqual(0.0))).TupleAnd(
                    hv_IsTelecentric.TupleNot())) != 0)
                {
                    //For projective cameras:
                    //Poses with Z position zero cannot be projected
                    //(that would lead to a division by zero error).
                    //ho_Arrows.Dispose();
                    return;
                }
                //Convert to pose to a transformation matrix
                HOperatorSet.PoseToHomMat3d(hv_Pose, out hv_TransWorld2Cam);
                //Project the world origin into the image
                HOperatorSet.AffineTransPoint3d(hv_TransWorld2Cam, 0, 0, 0, out hv_OrigCamX,
                    out hv_OrigCamY, out hv_OrigCamZ);
                HOperatorSet.Project3dPoint(hv_OrigCamX, hv_OrigCamY, hv_OrigCamZ, hv_CamParam,
                    out hv_Row0, out hv_Column0);
                //Project the coordinate axes into the image
                HOperatorSet.AffineTransPoint3d(hv_TransWorld2Cam, hv_CoordAxesLength, 0, 0,
                    out hv_X, out hv_Y, out hv_Z);
                HOperatorSet.Project3dPoint(hv_X, hv_Y, hv_Z, hv_CamParam, out hv_RowAxX, out hv_ColumnAxX);
                HOperatorSet.AffineTransPoint3d(hv_TransWorld2Cam, 0, hv_CoordAxesLength, 0,
                    out hv_X, out hv_Y, out hv_Z);
                HOperatorSet.Project3dPoint(hv_X, hv_Y, hv_Z, hv_CamParam, out hv_RowAxY, out hv_ColumnAxY);
                HOperatorSet.AffineTransPoint3d(hv_TransWorld2Cam, 0, 0, hv_CoordAxesLength,
                    out hv_X, out hv_Y, out hv_Z);
                HOperatorSet.Project3dPoint(hv_X, hv_Y, hv_Z, hv_CamParam, out hv_RowAxZ, out hv_ColumnAxZ);
                //
                //Generate an XLD contour for each axis
                HOperatorSet.DistancePp(((hv_Row0.TupleConcat(hv_Row0))).TupleConcat(hv_Row0),
                    ((hv_Column0.TupleConcat(hv_Column0))).TupleConcat(hv_Column0), ((hv_RowAxX.TupleConcat(
                    hv_RowAxY))).TupleConcat(hv_RowAxZ), ((hv_ColumnAxX.TupleConcat(hv_ColumnAxY))).TupleConcat(
                    hv_ColumnAxZ), out hv_Distance);
                hv_HeadLength = (((((((hv_Distance.TupleMax()) / 12.0)).TupleConcat(5.0))).TupleMax()
                    )).TupleInt();

                //Vision.Gen_arrow_contour_xld(out ho_Arrows, ((hv_Row0.TupleConcat(hv_Row0))).TupleConcat(
                //      hv_Row0), ((hv_Column0.TupleConcat(hv_Column0))).TupleConcat(hv_Column0),
                //      ((hv_RowAxX.TupleConcat(hv_RowAxY))).TupleConcat(hv_RowAxZ), ((hv_ColumnAxX.TupleConcat(
                //      hv_ColumnAxY))).TupleConcat(hv_ColumnAxZ), hv_HeadLength, hv_HeadLength);

                Vision.Gen_arrow_contour_xld(out ho_ArrowsX, hv_Row0, hv_Column0, hv_RowAxX, hv_ColumnAxX, hv_HeadLength, hv_HeadLength);
                Vision.Gen_arrow_contour_xld(out ho_ArrowsY, hv_Row0, hv_Column0, hv_RowAxY, hv_ColumnAxY, hv_HeadLength, hv_HeadLength);
                Vision.Gen_arrow_contour_xld(out ho_ArrowsZ, hv_Row0, hv_Column0, hv_RowAxZ, hv_ColumnAxZ, hv_HeadLength, hv_HeadLength);
                if (halconRun != null)
                {
                    halconRun.GetOneImageR().AddObj(ho_ArrowsX, ColorResult.red);
                    halconRun.GetOneImageR().AddObj(ho_ArrowsY, ColorResult.green);
                    halconRun.GetOneImageR().AddObj(ho_ArrowsZ, ColorResult.yellow);
                    halconRun.GetOneImageR().AddImageMassage(hv_RowAxX + 3, hv_ColumnAxX + 3, "X", ColorResult.red);
                    halconRun.GetOneImageR().AddImageMassage(hv_RowAxY + 3, hv_ColumnAxY + 3, "Y", ColorResult.green);
                    halconRun.GetOneImageR().AddImageMassage(hv_RowAxZ + 3, hv_ColumnAxZ + 3, "Z", ColorResult.yellow);
                }
            }
            catch (HalconException HDevExpDefaultException)
            {
                //ho_Arrows.Dispose();
                throw HDevExpDefaultException;
            }

        }

        public Control GetThisControl()
        {
            return null;
        }
    }
}