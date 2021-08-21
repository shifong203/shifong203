using ErosSocket.ErosConLink;
using HalconDotNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;
using Vision2.ErosProjcetDLL.UI.PropertyGrid;
using Vision2.vision.HalconRunFile.Controls;
using static Vision2.vision.HalconRunFile.RunProgramFile.Color_Detection;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
    [Serializable]
    public class ModelVision : RunProgram
    {
        public override string ShowHelpText()
        {
            return "2.2_创建视觉模板";
        }

        public override Control GetControl(IDrawHalcon halcon)
        {
            return new ModelControl(this, halcon) { Dock = DockStyle.Fill };
        }

        public enum CModeRowCol
        {
            Dis = 0,
            Row = 1,
            Col = 2,
            Angle = 3,
            RowCol = 4,
            RowColAngle = 5,
            Cilcre = 6,
            AngleCilcre = 7,
        }

        [Serializable]
        /// <summary>
        /// 定位信息
        /// </summary>
        public class RModelHomMat
        {
            public RModelHomMat()
            {
                NumberT = Row = Col = Angle = Score = Scale = X = Y = U = 0;
                HomMat = new List<HTuple>();
                //ImageHomMat = new List<HTuple>();
                //this.RowCompensate = new HTuple(0.0);
                //this.ColCompenSate = new HTuple(0.0);
                //this.AngleCompenSate = new HTuple(0.0);
                LocationOK = false;
                ModeXld.GenEmptyObj();
            }

            /// <summary>
            /// 跟随模板
            /// </summary>
            private HObject ModeXld = new HObject();

            public HObject GetModeXld(HObject hObject = null)
            {
                if (hObject != null)
                {
                    ModeXld = hObject;
                }
                return ModeXld;
            }

            /// <summary>
            /// 数量
            /// </summary>
            public int NumberT;

            public HTuple Row = new HTuple();

            public HTuple Col = new HTuple();

            /// <summary>
            /// 角度
            /// </summary>
            public HTuple Angle = new HTuple();

            /// <summary>
            /// 角度
            /// </summary>
            public HTuple Phi = new HTuple();

            /// <summary>
            /// 分数
            /// </summary>
            public HTuple Score = new HTuple();

            /// <summary>
            /// 缩放系数
            /// </summary>
            public HTuple Scale = new HTuple();

            /// <summary>
            /// 仿射变换
            /// </summary>
            public List<HTuple> HomMat;

            ///// <summary>
            ///// 仿射变换
            ///// </summary>
            //public List<HTuple> ImageHomMat = new List<HTuple>();
            /// <summary>
            /// 放射模式
            /// </summary>
            public string HomMatMode = "区域放射";

            /// <summary>
            /// 放射XLD
            /// </summary>
            public List<HObject> AffAffineTransContourXld(HObject intHObject)
            {
                List<HObject> listObj = new List<HObject>();
                try
                {
                    for (int i = 0; i < this.HomMat.Count; i++)
                    {
                        HObject contoursAffineTrans = new HObject();
                        if (HomMatMode == "模板区域")
                        {
                            HOperatorSet.HomMat2dIdentity(out HTuple homMat2D);
                            HOperatorSet.AreaCenterPointsXld(intHObject, out HTuple area, out HTuple row, out HTuple column);
                            HOperatorSet.VectorAngleToRigid(row, column, 0, 0, 0, 0, out homMat2D);
                            HOperatorSet.AffineTransContourXld(intHObject, out contoursAffineTrans, homMat2D);
                        }
                        else
                        {
                            HOperatorSet.AffineTransContourXld(intHObject, out contoursAffineTrans, this.HomMat[i]);
                        }
                        listObj.Add(contoursAffineTrans);
                    }
                    return listObj;
                }
                catch (Exception)
                {
                }
                return listObj;
            }

            /// <summary>
            /// 定位OK标志
            /// </summary>
            public bool LocationOK;

            /// <summary>
            /// 转换坐标
            /// </summary>
            public HTuple X = new HTuple();

            public HTuple Y = new HTuple();
            public HTuple U = new HTuple();
            public HTuple Z = new HTuple();
            public HTuple V = new HTuple();
            public HTuple W = new HTuple();
        }

        public ModelVision()
        {
            ID = -1;
            OriginU = OriginX = OriginY = m_HomMat2D = 0;
            Type = GetType().ToString();
            modelVision = this;
            this.OrignXLD.GenEmptyObj();
            this.ROIModeXLD.GenEmptyObj();
            Create_ModelRegr = new HObject();
            Create_ModelRegr.GenEmptyObj();
            this.AOIObj.GenEmptyObj();
        }

        ~ModelVision()
        {
            AOIObj.Dispose();
            OrignXLD.Dispose();
            ROIModeXLD.Dispose();
            try
            {
                HOperatorSet.ClearShapeModel(this.ID);
            }
            catch (Exception)
            {
            }
        }

        public HObject Create_ModelRegr { get; set; }

        public override void Dispose()
        {
            OrignXLD.Dispose();
            ROIModeXLD.Dispose();
            try
            {
                HOperatorSet.ClearShapeModel(this.ID);
            }
            catch (Exception)
            {
            }
            base.Dispose();
        }

        private ModelVision modelVision;

        [Description("3D坐标彷射参数"), Category("坐标系统"), DisplayName("标定模式")]
        public Calib.AutoCalibPoint.CalibMode calibMode { get; set; }

        [Description("3D坐标彷射参数"), Category("坐标系统"), DisplayName("坐标系统"),
       TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("ListCoordinate3DName")]
        public string Coordinate3DName { get; set; } = "";

        [DescriptionAttribute("机器人名称。"), Category("触发器"), DisplayName("机器人名称")]
        [Editor(typeof(LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        public string ReadRunIDName { get; set; } = string.Empty;

        /// <summary>
        /// 坐标集合名称
        /// </summary>
        public static List<string> ListCoordinate3DName
        {
            get
            {
                return Vision.Instance.DicCalib3D.Keys.ToList();
            }
        }

        [DescriptionAttribute("模板彷射模式。"), Category("结果显示"), DisplayName("模板偏移方式")]
        public CModeRowCol CMode
        {
            get { return cMode; }
            set
            {
                DialogResult dr;
                switch (value)
                {
                    case CModeRowCol.Dis:
                        break;

                    case CModeRowCol.Row:
                        if (!this.Dic_Measure.Keys_Measure.ContainsKey("Row"))
                        {
                            dr = MessageBox.Show("使用测量偏移,将创建[Row]测量已确定模板偏移方向", "新建测量程序", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                            if (dr != DialogResult.OK)
                            {
                                return;
                            }
                            this.Dic_Measure.Add("Row");
                            this.Dic_Measure["Row"].ISMatHat = this.Dic_Measure["Row"].Enabled = true;
                            this.Dic_Measure["Row"].Measure_Type = Measure.MeasureType.Line;
                        }
                        break;

                    case CModeRowCol.Col:
                        if (!this.Dic_Measure.Keys_Measure.ContainsKey("Col"))
                        {
                            dr = MessageBox.Show("使用测量偏移,将创建[Col]测量已确定模板偏移方向", "新建测量程序", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                            if (dr != DialogResult.OK)
                            {
                                return;
                            }
                            this.Dic_Measure.Add("Col");
                        }
                        break;

                    case CModeRowCol.Angle:
                        break;

                    case CModeRowCol.RowCol:
                        if (!this.Dic_Measure.Keys_Measure.ContainsKey("左侧测量") || !this.Dic_Measure.Keys_Measure.ContainsKey("右侧测量") ||
                        !this.Dic_Measure.Keys_Measure.ContainsKey("上测测量")
                         || !this.Dic_Measure.Keys_Measure.ContainsKey("下测测量"))
                        {
                            dr = MessageBox.Show("使用原点同步,将创建[左侧测量、右侧测量、上测测量、下测测量]测量已确定模板中心", "新建测量程序", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                            if (dr != DialogResult.OK)
                            {
                                return;
                            }
                            if (!this.Dic_Measure.Keys_Measure.ContainsKey("左侧测量"))
                            {
                                this.Dic_Measure.Add("左侧测量");
                            }
                            if (!this.Dic_Measure.Keys_Measure.ContainsKey("右侧测量"))
                            {
                                this.Dic_Measure.Add("右侧测量");
                            }
                            if (!this.Dic_Measure.Keys_Measure.ContainsKey("上测测量"))
                            {
                                this.Dic_Measure.Add("上测测量");
                            }
                            if (!this.Dic_Measure.Keys_Measure.ContainsKey("下测测量"))
                            {
                                this.Dic_Measure.Add("下测测量");
                            }
                            this.Dic_Measure["左侧测量"].ISMatHat = this.Dic_Measure["左侧测量"].Enabled =
                            this.Dic_Measure["右侧测量"].ISMatHat = this.Dic_Measure["右侧测量"].Enabled =
                            this.Dic_Measure["下测测量"].ISMatHat = this.Dic_Measure["下测测量"].Enabled =
                            this.Dic_Measure["上测测量"].ISMatHat = this.Dic_Measure["上测测量"].Enabled = true;
                            this.Dic_Measure["左侧测量"].Measure_Type = this.Dic_Measure["下测测量"].Measure_Type =
                            this.Dic_Measure["右侧测量"].Measure_Type = this.Dic_Measure["上测测量"].Measure_Type = Measure.MeasureType.Line;
                            this.Dic_Measure["左侧测量"].MeasurePointNumber = this.Dic_Measure["右侧测量"].MeasurePointNumber =
                            this.Dic_Measure["下测测量"].MeasurePointNumber = this.Dic_Measure["上测测量"].MeasurePointNumber = 20;
                            this.Dic_Measure["左侧测量"].Min_Measure_Point_Number = this.Dic_Measure["右侧测量"].Min_Measure_Point_Number =
                            this.Dic_Measure["下测测量"].Min_Measure_Point_Number = this.Dic_Measure["上测测量"].Min_Measure_Point_Number = 10;

                            ProjectNodet.FormThis.UpProject();
                        }
                        break;

                    case CModeRowCol.RowColAngle:
                        break;

                    case CModeRowCol.Cilcre:

                        foreach (var item in this.Dic_Measure.Keys_Measure)
                        {
                            if (item.Key.StartsWith("Cilcre"))
                            {
                                cMode = value;
                                return;
                            }
                        }
                        dr = MessageBox.Show("使用测量偏移,将创建[Cilcre]测量已确定模板中心", "新建测量程序", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (dr == DialogResult.OK)
                        {
                            this.Dic_Measure.Add("Cilcre");
                            this.Dic_Measure["Cilcre"].Measure_Type = Measure.MeasureType.Cilcre;
                        }
                        break;
                }
                cMode = value;
            }
        }

        private CModeRowCol cMode = new CModeRowCol();

        [DescriptionAttribute("模板彷射模式。图片仿射模式、模板区域仿射模式，和绘制仿射模式"), Category("结果显示"), DisplayName("原点Col")]
        [TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("", true, "图片", "模板区域", "绘制区域")]
        public string Mode { get; set; } = "";

        [DescriptionAttribute("是否显示模板绘制中心点。"), Category("结果显示"), DisplayName("显示模板绘制中心点")]
        public bool ISPoint { get; set; } = true;

        [DescriptionAttribute("显示模板分数。"), Category("结果显示"), DisplayName("显示模板分数")]
        public bool IsSeae { get; set; } = true;

        [DescriptionAttribute("显示模板角度。"), Category("结果显示"), DisplayName("显示模板角度")]
        public bool IsAngle { get; set; } = true;

        [DescriptionAttribute("NG显示园。"), Category("结果显示"), DisplayName("显示园")]
        public bool IsCircle { get; set; } = true;

        [DescriptionAttribute("中心大小。"), Category("结果显示"), DisplayName("中心大小")]
        public int PointSize { get; set; } = 60;

        public bool IsDip { get; set; }
        public List<DIP> DIPs { get; set; } = new List<DIP>();

        public class DIP
        {
            public string Name { get; set; } = "";
            public double Row { get; set; }
            public double Col { get; set; }
            public double Angle { get; set; }
            public double DistanceMax { get; set; } = 100;
            public double AngleMax { get; set; } = 50;
        }

        public Text_Model.Text_Mo HTextMod = new Text_Model.Text_Mo();

        public Variation_Model Variation_Model = new Variation_Model();

        public Dictionary<string, Color_classify> ColorDic { get; set; } = new Dictionary<string, Color_classify>();

        [DescriptionAttribute("同步模板中心Col。"), Category("原点处理"), DisplayName("原点Col")]
        public double SetOriginCol { get; set; }

        [DescriptionAttribute("同步模板中心Row。"), Category("原点处理"), DisplayName("原点Row")]
        public double SetOriginRow { get; set; }

        [DescriptionAttribute("原点位置col。"), Category("原点处理"), DisplayName("原点Y")]
        public HTuple OriginY { get; set; } = new HTuple();

        [DescriptionAttribute("原点位置row。"), Category("原点处理"), DisplayName("原点X")]
        public HTuple OriginX { get; set; } = new HTuple();

        [DescriptionAttribute("原点位置U。"), Category("原点处理"), DisplayName("原点U")]
        public HTuple OriginU { get; set; } = new HTuple();

        [DescriptionAttribute("模板中心偏移Y。"), Category("原点处理"), DisplayName("原点叠加偏移Y")]
        public double OriginYAdd { get; set; }

        [DescriptionAttribute("模板中心偏移X。"), Category("原点处理"), DisplayName("原点叠加偏移X")]
        public double OriginXAdd { get; set; }

        [DescriptionAttribute("模板中心偏移U。"), Category("原点处理"), DisplayName("原点叠加偏移U")]
        public double OriginUAdd { get; set; }

        private HTuple ID = new HTuple();

        [DescriptionAttribute("最小得分。"), Category("定位参数"), DisplayName("最小匹配分数")]
        public double ScoreD { get; set; } = 0.6;

        [DescriptionAttribute("最小缩放。"), Category("定位参数"), DisplayName("最小缩放范围")]
        public double MinScaelD { get; set; } = 0.9;

        [DescriptionAttribute("最大缩放。"), Category("定位参数"), DisplayName("最大缩放范围")]
        public double MaxScaelD { get; set; } = 1.1;

        [DescriptionAttribute("开始角度。"), Category("定位参数"), DisplayName("搜索开始角度")]
        public double AngleStartDeg
        {
            get
            {
                return AngleStart;
            }

            set { AngleStart = new HTuple(value); }
        }

        private HTuple AngleStart = new HTuple(-10.0);

        [DescriptionAttribute("超时事件MS。"), Category("定位参数"), DisplayName("超时事件")]
        public int Timeout { get; set; } = 2000;

        [DescriptionAttribute("结束角度。"), Category("定位参数"), DisplayName("搜索结束角度")]
        public double AngleExtentDeg
        {
            get { return AngleExtent; }
            set { AngleExtent = new HTuple(value); }
        }

        private HTuple AngleExtent { get; set; } = 20;

        [DescriptionAttribute("角度的分辨率‘auto', 0.0175, 0.0349, 0.0524, 0.0698, 0.0873"), Category("定位参数"), DisplayName("角度分辨率")]
        public string AngleStep
        {
            get { return angleStep; }
            set
            {
                if (double.TryParse(value, out double sdouble))
                {
                    angleStep = value;
                }
                else
                {
                    angleStep = "auto";
                }
            }
        }

        private string angleStep = "auto";

        [DescriptionAttribute(""), Category("2D"), DisplayName("参考点Y")]
        /// <summary>
        ///
        /// </summary>
        public double Ys { get; set; }

        [DescriptionAttribute(""), Category("2D"), DisplayName("参考点X")]
        /// <summary>
        /// 参考点X
        /// </summary>
        public double Xs { get; set; }

        [DescriptionAttribute(""), Category("2D"), DisplayName("使用参考点")]
        /// <summary>
        ///
        /// </summary>
        public bool IsCot { get; set; }

        [DescriptionAttribute("越大越快，定位效果也相对低。最大1"), Category("定位参数"), DisplayName("查找速度")]
        /// <summary>
        /// 贪婪速度，越大越慢效果越好,小块效果降低
        /// </summary>
        public double GreedinessD { get; set; } = 0.7;

        [DescriptionAttribute("重叠系数。0.3~0.9"), Category("定位参数"), DisplayName("最大重叠系数")]
        /// <summary>
        /// 重叠系数
        /// </summary>
        public double MaxOverlapD { get; set; } = 0.5;

        [DescriptionAttribute("最小1"), Category("定位参数"), DisplayName("查找数量上限")]
        /// <summary>
        /// 数量上线
        /// </summary>
        public int NumberI { get; set; } = 1;

        /// <summary>
        /// 目标数量
        /// </summary>
        public int ContNumber { get; set; } = -1;

        [DescriptionAttribute("模板加入偏移的中心X位置"), Category("结果参数"), DisplayName("中心点X")]
        public HTuple X { get; set; }

        [DescriptionAttribute("模板加入偏移的中心Y位置"), Category("结果参数"), DisplayName("中心点Y")]
        public HTuple Y { get; set; }

        [DescriptionAttribute("将X偏移写入的变量名称"), Category("触发器"), DisplayName("结果X偏移位置名称")]
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        public string MXName { get; set; } = string.Empty;

        [DescriptionAttribute("将Y偏移写入的变量名称。"), Category("触发器"), DisplayName("结果Y偏移位置名称")]
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        public string MYName { get; set; } = string.Empty;

        [DescriptionAttribute("将U偏移写入的变量名称。"), Category("触发器"), DisplayName("结果U偏移位置名称")]
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        public string MUName { get; set; } = string.Empty;

        [DescriptionAttribute("将Z偏移写入的变量名称。"), Category("触发器"), DisplayName("结果Z偏移位置名称")]
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        public string MZName { get; set; } = string.Empty;

        /// <summary>
        /// 原点区域
        /// </summary>
        public HObject OrignXLD = new HObject();

        public HObject ROIModeXLD = new HObject();

        public HTuple m_HomMat2D = new HTuple();

        /// <summary>
        /// 图像金字塔级别0-10；
        /// </summary>
        public HTuple pyramid = 0;

        //public HObject arrow = new HObject();

        public HTuple arrowPhi = 0;

        public HTuple ArrowRow1 = new HTuple();

        public HTuple ArrowCol1 = new HTuple();
        public HTuple ArrowRow2 = new HTuple();

        public HTuple ArrowCol2 = new HTuple();

        //Default value: 'least_squares'
        // Suggested values: 'none', 'interpolation', 'least_squares', 'least_squares_high', 'least_squares_very_high', 'max_deformation 1', 'max_deformation 2', 'max_deformation 3', 'max_deformation 4', 'max_deformation 5', 'max_deformation 6'
        /// <summary>
        /// 亚像素精度
        /// </summary>
        public HTuple SubPixel = "least_squares_high";// "least_squares_high";ignore_color_polarity

        public RModelHomMat MRModelHomMat;

        /// <summary>
        /// 读取模板
        /// </summary>
        /// <param name="name"></param>
        /// <param name="name"></param>
        public ModelVision RradModel(string path)
        {
            //ModelVision modelVision = new ModelVision();
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            try
            {
                if (File.Exists(path + this.SuffixName))
                {
                    modelVision = JsonConvert.DeserializeObject<ModelVision>(File.ReadAllText(path + this.SuffixName));  //以字符串形式读取文件
                }
                else
                {
                    modelVision = JsonConvert.DeserializeObject<ModelVision>(File.ReadAllText(path + ".txt"));  //以字符串形式读取文件
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            path = Path.GetDirectoryName(path);
            if (File.Exists(path + "\\模板文件.shm"))
            {
                HOperatorSet.ReadShapeModel(path + "\\模板文件.shm", out HTuple id);
                modelVision.ID = id;
                HOperatorSet.SetShapeModelParam(id, "timeout", Timeout);
            }
            else
            {
                LogErr("缺少模板文件：" + path + "\\" + modelVision.Name + ".dcm");
                MessageBox.Show("缺少模板文件:" + path + "\\" + modelVision.Name + ".dcm");
            }

            HOperatorSet.GetShapeModelContours(out modelVision.OrignXLD, modelVision.ID, 1);

            return modelVision;
        }

        /// <summary>
        /// 写模板到目标地址
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        public bool SaveModel(string path)
        {
            try
            {
                Directory.CreateDirectory(path + "\\" + this.Name + "\\");
                HOperatorSet.WriteShapeModel(this.ID, path + "\\" + this.Name + "\\模板文件.shm");
                return true;
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("保存模板定位失败,名称:" + this.Name + "错误信息:" + ex.Message);
                Vision.Log(ex.ToString());
                return false;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="tshi"></param>
        /// <returns></returns>
        public override RunProgram UpSatrt<T>(string path)
        {
            modelVision = RradModel(path);
            modelVision.Variation_Model.ReadModel(path);
            return modelVision;
        }

        public override void SaveThis(string path)
        {
            base.SaveThis(path);
            SaveModel(path);
            Variation_Model.Write_Variation_Model(path + "\\" + this.Name + "\\" + this.Name);
        }

        public void DrawROI(HalconRun halcon)
        {
            try
            {
                if (halcon.Drawing)
                {
                    halcon.ShowMessage("正在绘制中！", halcon.Width - 200, 20);
                    return;
                }
                halcon.Drawing = true;
                HTuple Row, Column, Phi, Length1, Length2;
                Length1 = 0;
                halcon.ShowMessage("画模板区域，点击鼠标右键确认", halcon.Width / 2, 20);
                if (AOIObj.IsInitialized())
                {
                    HOperatorSet.AreaCenter(AOIObj, out Length1, out HTuple row, out HTuple column);
                }
                else
                {
                    AOIObj.GenEmptyObj();
                }
                //画模板区域
                if (Length1 > 10)
                {
                    HOperatorSet.SmallestRectangle2(this.AOIObj, out Row, out Column, out Phi, out Length1, out Length2);
                    HOperatorSet.DrawRectangle2Mod(halcon.hWindowHalcon(), Row, Column, Phi, Length1, Length2,
                        out Row, out Column, out Phi, out Length1, out Length2);
                }
                else
                {
                    HOperatorSet.DrawRectangle2(halcon.hWindowHalcon(), out Row, out Column, out Phi, out Length1, out Length2);
                }
                HOperatorSet.GenRectangle2(out this.AOIObj, Row, Column, Phi, Length1, Length2);
                halcon.AddObj(this.AOIObj.Clone());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            halcon.Drawing = false;
        }

        /// <summary>
        /// 创建模板
        /// </summary>
        /// <param name="hWindowControl1">窗口</param>
        /// <param name="m_Image">图像</param>
        /// <param name="objDisp">区域</param>
        /// <param name="model">模板</param>
        public void Create_Shape_Model(HalconRun halcon, HObject m_Image, ref HObject objDisp)
        {
            HObject ImageReduced, Cross;
            MRModelHomMat = new RModelHomMat();
            HOperatorSet.GenEmptyObj(out ImageReduced);
            try
            {
                if (!Vision.IsObjectValided(m_Image))
                {
                    MessageBox.Show("没有图像！");
                    return;
                }
                if (DrawObj.IsInitialized())
                {
                    HOperatorSet.Difference(Create_ModelRegr, this.DrawObj, out nGRoi);
                }
                HOperatorSet.ReduceDomain(m_Image, this.nGRoi, out ImageReduced);
                try
                {
                    HOperatorSet.ClearShapeModel(this.ID);
                }
                catch { }
                //SubPixel = "ignore_color_polarity";
                HOperatorSet.CreateScaledShapeModel(ImageReduced, "auto", AngleStart.TupleRad(), AngleExtent.TupleRad(), AngleStep,
                    this.MinScaelD, this.MaxScaelD, "auto", "auto",
                    "ignore_color_polarity", "auto", "auto", out HTuple id);
                this.ID = id;
                HTuple HomMat2D_T;
                HOperatorSet.SetShapeModelParam(this.ID, "timeout", Timeout);
                if (AOIObj.CountObj() > 0)
                {
                    HOperatorSet.ReduceDomain(m_Image, this.AOIObj, out ImageReduced);
                }
                //几何定位
                if (MaxScaelD == 1 && MinScaelD == 1)
                {
                    HOperatorSet.FindShapeModel(ImageReduced, this.ID, AngleStart.TupleRad(), AngleExtent.TupleRad(), this.ScoreD, this.NumberI, MaxOverlapD,
                   SubPixel, pyramid, GreedinessD, out MRModelHomMat.Row, out MRModelHomMat.Col, out MRModelHomMat.Angle,
                   out MRModelHomMat.Score);
                    MRModelHomMat.Scale = 1;
                }
                else
                {
                    HOperatorSet.FindScaledShapeModel(ImageReduced, this.ID, AngleStart.TupleRad(), AngleExtent.TupleRad(), MinScaelD, MaxScaelD, this.ScoreD, this.NumberI, MaxOverlapD,
                  SubPixel, pyramid, GreedinessD, out MRModelHomMat.Row, out MRModelHomMat.Col, out MRModelHomMat.Angle, out MRModelHomMat.Scale,
                  out MRModelHomMat.Score);
                }
                if (MRModelHomMat.Score.Length >= 1)
                {
                    this.OriginY = MRModelHomMat.Row[0];
                    this.OriginX = MRModelHomMat.Col[0];
                    this.OriginU = MRModelHomMat.Angle[0];
                }
                else
                {
                    Vision.Disp_message(halcon.hWindowHalcon(), "创建模板成功,但未找到模板!", 20, 20, true);
                }
                //获得中心点并显示
                HOperatorSet.GenCrossContourXld(out Cross, this.OriginY, this.OriginX, 20, this.OriginU);
                HOperatorSet.DispObj(Cross, halcon.hWindowHalcon());
                //获得模板并显示
                HOperatorSet.GetShapeModelContours(out this.OrignXLD, this.ID, 1);
                HObject hObject1 = Vision.XLD_To_Region(OrignXLD);
                HOperatorSet.Union1(hObject1, out hObject1);

                HOperatorSet.SmallestRectangle2(DrawObj, out HTuple row1, out HTuple colu1, out HTuple phi1, out HTuple length1, out HTuple length2);
                HOperatorSet.SmallestRectangle2(hObject1, out row1, out colu1, out arrowPhi, out length1, out length2);

                HOperatorSet.VectorAngleToRigid(0, 0, 0, this.OriginY, this.OriginX, this.OriginU, out HomMat2D_T);
                HOperatorSet.VectorAngleToRigid(0, 0, 0, this.OriginY, this.OriginX, arrowPhi, out HTuple homat);
                HOperatorSet.AffineTransPoint2d(homat, 0, -length1, out ArrowRow1, out ArrowCol1);
                HOperatorSet.AffineTransPoint2d(homat, 0, length1, out ArrowRow2, out ArrowCol2);
                HOperatorSet.GenRegionLine(out HObject hObject, ArrowRow1, ArrowCol1, ArrowRow2, ArrowCol2);
                halcon.AddObj(hObject, ColorResult.blue);
                //Vision.Gen_arrow_contour_xld(out arrow, 0, -length1, 0, length1);

                HOperatorSet.AffineTransContourXld(this.OrignXLD, out objDisp, HomMat2D_T);
                this.XLD = objDisp.ConcatObj(Cross);
                this.ROIModeXLD = this.XLD.Clone();
                HOperatorSet.DispObj(this.XLD, halcon.hWindowHalcon());
            }
            catch (HalconException hdEx)
            {
                ImageReduced.Dispose();
                Vision.Disp_message(halcon.hWindowHalcon(), "创建模板过程失败!", 100, 20, true);
                Vision.Log(hdEx.Message);
            }
            ImageReduced.Dispose();
        }

        /// <summary>
        /// 在程序集合HalconImage类中创建模板
        /// </summary>
        /// <param name="halcon"></param>
        public void Create_Shape_Model(HalconRun halcon)
        {
            HObject hObject = new HObject();
            Create_Shape_Model(halcon, halcon.Image(), ref hObject);
            halcon.AddObj(hObject);
        }

        public void Create_Scaled_Shap_Model_Xld(HalconRun halcon, HObject xldMode)
        {
            try
            {
                halcon.Drawing = true;
                if (this.ID > -1)
                {
                    try
                    {
                        HOperatorSet.ClearShapeModel(this.ID);
                    }
                    catch (Exception)
                    {
                        this.ID = -1;
                    }
                }
                HTuple id = -1;
                if (this.MinScaelD == 1 && this.MaxScaelD == 1)
                {
                    HOperatorSet.CreateShapeModelXld(xldMode, "auto", AngleStart.TupleRad(), AngleExtent.TupleRad(), AngleStep, "auto", "ignore_local_polarity", 5, out id);
                }
                else
                {
                    HOperatorSet.CreateScaledShapeModelXld(xldMode, "auto", AngleStart.TupleRad(), AngleExtent.TupleRad(), AngleStep, this.MinScaelD, this.MaxScaelD, "auto", "auto", "ignore_local_polarity", 5, out id);
                }
                this.ID = id;
                //几何定位
                HOperatorSet.SetShapeModelParam(this.ID, "timeout", Timeout);
                //几何定位
                if (MaxScaelD == 1 && MinScaelD == 1)
                {
                    HOperatorSet.FindShapeModel(halcon.Image(), this.ID, AngleStart.TupleRad(), AngleExtent.TupleRad(), this.ScoreD, this.NumberI, MaxOverlapD,
                   SubPixel, pyramid, GreedinessD, out MRModelHomMat.Row, out MRModelHomMat.Col, out MRModelHomMat.Angle,
                   out MRModelHomMat.Score);
                    MRModelHomMat.Scale = 1;
                }
                else
                {
                    HOperatorSet.FindScaledShapeModel(halcon.Image(), this.ID, AngleStart.TupleRad(), AngleExtent.TupleRad(), MinScaelD, MaxScaelD, this.ScoreD, this.NumberI, MaxOverlapD,
                  SubPixel, pyramid, GreedinessD, out MRModelHomMat.Row, out MRModelHomMat.Col, out MRModelHomMat.Angle, out MRModelHomMat.Scale,
                  out MRModelHomMat.Score);
                }
                //HOperatorSet.FindShapeModel(halcon.Image(), this.ID, AngleStart.TupleRad(), AngleExtent.TupleRad(),
                //   this.ScoreD, 1, MaxOverlapD, SubPixel, pyramid, GreedinessD, out HTuple Y, out HTuple X, out HTuple U, out HTuple Score);
                if (MRModelHomMat.Score.Length == 1)
                {
                    this.OriginY = MRModelHomMat.Row[0];
                    this.OriginX = MRModelHomMat.Col[0];
                    this.OriginU = MRModelHomMat.Angle[0];
                    //获得模板并显示
                    HOperatorSet.GetShapeModelContours(out this.OrignXLD, this.ID, 1);
                    HObject hObject1 = Vision.XLD_To_Region(OrignXLD);
                    HOperatorSet.Union1(hObject1, out hObject1);
                    HOperatorSet.SmallestRectangle2(hObject1, out HTuple row1, out HTuple colu1, out HTuple phi1, out HTuple length1, out HTuple length2);
                    //Vision.Gen_arrow_contour_xld(out arrow, 0, -length1, 0, length1);
                    HOperatorSet.VectorAngleToRigid(0, 0, 0, this.OriginY, this.OriginX, this.OriginU, out HTuple HomMat2D_T);
                    HOperatorSet.AffineTransContourXld(this.OrignXLD, out HObject objDisp, HomMat2D_T);
                    HOperatorSet.VectorAngleToRigid(0, 0, 0, this.OriginY, this.OriginX, arrowPhi, out HTuple homat);
                    HOperatorSet.AffineTransPoint2d(homat, 0, -length1, out ArrowRow1, out ArrowCol1);
                    HOperatorSet.AffineTransPoint2d(homat, 0, length1, out ArrowRow2, out ArrowCol2);
                    HOperatorSet.GenRegionLine(out HObject hObject, ArrowRow1, ArrowCol1, ArrowRow2, ArrowCol2);
                    halcon.AddObj(hObject, ColorResult.blue);

                    this.ROIModeXLD = objDisp;
                    HOperatorSet.GetShapeModelContours(out this.OrignXLD, this.ID, 1);
                    halcon.AddObj(this.OrignXLD);

                    halcon.AddNGMessage("创建模板成功");
                }
                else
                {
                    MessageBox.Show("创建成功，单未搜索到模板");
                }
                //获得中心点并显示
                //halcon.POnShowObj(halcon, this.Name);
            }
            catch (HalconException hdEx)
            {
                MessageBox.Show(hdEx.Message);
                halcon.AddNGMessage("创建模板过程失败");
                //Vision.Disp_message(halcon.hWindowHalcon(), "创建模板过程失败!",  20, 20, true);
            }
        }

        public static Double ToAngle(double value)
        {
            if (value < 0)
            {
                return value + 360;
            }
            else if (value > 360)
            {
                return value + 360;
            }
            return value;
        }

        public void SetDip(HTuple AoiRow, HTuple AoiCol, HTuple angle, out HObject xld, out HTuple row, out HTuple col, out HTuple phi, out HTuple idT)
        {
            xld = new HObject();
            xld.GenEmptyObj();
            HTuple rows = new HTuple();
            HTuple cols = new HTuple();
            HTuple ridus = new HTuple();
            row = new HTuple();
            col = new HTuple();
            phi = new HTuple();
            idT = new HTuple();
            try
            {
                HTuple hTuple2Rs = new HTuple();
                HTuple hTuple2cs = new HTuple();
                HTuple hTuple3Rs = new HTuple();
                HTuple hTuple3cs = new HTuple();
                for (int i = 0; i < AoiRow.Length; i++)
                {
                    HOperatorSet.VectorAngleToRigid(OriginY, OriginX, 0, AoiRow[i], AoiCol[i], angle.TupleRad()[i], out HTuple hTuple);
                    HOperatorSet.AffineTransPixel(hTuple, ArrowRow1, ArrowCol1, out HTuple hTuple2R, out HTuple hTuple2c);
                    HOperatorSet.AffineTransPixel(hTuple, ArrowRow2, ArrowCol2, out HTuple hTuple3R, out HTuple hTuple3c);

                    hTuple2Rs.Append(hTuple2R);
                    hTuple2cs.Append(hTuple2c);
                    hTuple3Rs.Append(hTuple3R);
                    hTuple3cs.Append(hTuple3c);
                }
                for (int i = 0; i < DIPs.Count; i++)
                {
                    HTuple phiMdt = new HTuple(0);
                    bool Rste = false;
                    Rste = true;
                    HOperatorSet.GenCircle(out HObject hObject, DIPs[i].Row, DIPs[i].Col, this.GetPThis().GetCaliConstPx(DIPs[i].DistanceMax));
                    if (ISPoint)
                    {
                        HOperatorSet.GenCrossContourXld(out HObject hObjectt, DIPs[i].Row, DIPs[i].Col, 50, 0);
                        HObjectGreen = HObjectGreen.ConcatObj(hObjectt);
                    }
                    if (AoiRow.Length == 0)
                    {
                        Rste = false;
                    }
                    else
                    {
                        HTuple indet = new HTuple();
                        for (int id = 0; id < AoiRow.Length; id++)
                        {
                            HOperatorSet.GetRegionIndex(hObject, AoiRow.TupleSelect(id).TupleInt(), AoiCol.TupleSelect(id).TupleInt(), out indet);
                            if (indet.Length != 0)
                            {
                                HOperatorSet.VectorAngleToRigid(OriginY, OriginX, 0, DIPs[i].Row, DIPs[i].Col, new HTuple(DIPs[i].Angle).TupleRad(), out HTuple hTuple);
                                HOperatorSet.AffineTransPixel(hTuple, ArrowRow1, ArrowCol1, out HTuple hTuple2R, out HTuple hTuple2c);
                                HOperatorSet.AffineTransPixel(hTuple, ArrowRow2, ArrowCol2, out HTuple hTuple3R, out HTuple hTuple3c);
                                HOperatorSet.AngleLl(hTuple2Rs[id], hTuple2cs[id], hTuple3Rs[id], hTuple3cs[id], hTuple2R, hTuple2c, hTuple3R, hTuple3c, out HTuple angleT);
                                angleT = angleT.TupleDeg();
                                Vision.Gen_arrow_contour_xld(out HObject HOARROW, hTuple2R, hTuple2c, hTuple3R, hTuple3c);

                                if (angleT.TupleAbs() > DIPs[i].AngleMax)
                                {
                                    phiMdt = angleT;
                                    this.GetPThis().AddObj(HOARROW, ColorResult.red);
                                    Rste = false;
                                }
                                else
                                {
                                    HOperatorSet.RemoveObj(hObject, out hObject, indet);
                                }
                                hTuple2Rs = hTuple2Rs.TupleRemove(id);
                                hTuple2cs = hTuple2cs.TupleRemove(id);
                                hTuple3Rs = hTuple3Rs.TupleRemove(id);
                                hTuple3cs = hTuple3cs.TupleRemove(id);
                                AoiRow = AoiRow.TupleRemove(id);
                                AoiCol = AoiCol.TupleRemove(id);
                                angle = angle.TupleRemove(id);

                                break;
                            }
                        }
                    }
                    if (hObject.CountObj() > 0)
                    {
                        Rste = false;
                    }
                    if (!Rste)
                    {
                        idT.Append(i);
                        phi.Append(phiMdt);
                        row.Append(DIPs[i].Row);
                        col.Append(DIPs[i].Col);
                        if (!IsCircle)
                        {
                            HOperatorSet.VectorAngleToRigid(0, 0, 0, DIPs[i].Row, DIPs[i].Col, new HTuple(DIPs[i].Angle).TupleRad(), out HTuple homMat2D);
                            HOperatorSet.AffineTransContourXld(OrignXLD, out HObject xl, homMat2D);
                            xld = xld.ConcatObj(xl);
                        }
                        else
                        {
                            xld = xld.ConcatObj(hObject);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 执行模板查找，并将图片放射变换
        /// </summary>
        /// <param name="halcon"></param>
        /// <returns></returns>
        public bool FindShapeModel(OneResultOBj halcon, string mode, AoiObj aoiObj)
        {
            MRModelHomMat = new RModelHomMat();
            HTuple message = new HTuple();
            HObject xldM = new HObject();
            this.X = new HTuple();
            this.Y = new HTuple();
            this.m_HomMat2D = new HTuple();
            HTuple phi = null;
            try
            {
                HObject Image = halcon.Image;
                if (aoiObj.SelseAoi.IsInitialized())
                {
                    HOperatorSet.AreaCenter(aoiObj.SelseAoi, out HTuple area, out HTuple row, out HTuple column);
                    if (area > 100) HOperatorSet.ReduceDomain(halcon.Image, aoiObj.SelseAoi, out Image);
                }
                else
                {
                    aoiObj.SelseAoi.GenEmptyObj();
                }
                //几何定位
                if (MaxScaelD == 1 && MinScaelD == 1)
                {
                    HOperatorSet.FindShapeModel(Image, this.ID, AngleStart.TupleRad(), AngleExtent.TupleRad(), this.ScoreD, this.NumberI, MaxOverlapD,
                   SubPixel, pyramid, GreedinessD, out MRModelHomMat.Row, out MRModelHomMat.Col, out MRModelHomMat.Angle,
                   out MRModelHomMat.Score);
                    MRModelHomMat.Scale = 1;
                }
                else
                {
                    HOperatorSet.FindScaledShapeModel(Image, this.ID, AngleStart.TupleRad(), AngleExtent.TupleRad(), MinScaelD, MaxScaelD, this.ScoreD, this.NumberI, MaxOverlapD,
                  SubPixel, pyramid, GreedinessD, out MRModelHomMat.Row, out MRModelHomMat.Col, out MRModelHomMat.Angle, out MRModelHomMat.Scale,
                  out MRModelHomMat.Score);
                }
                if (MRModelHomMat.Row.Length == 0)
                {
                    if (!halcon.GetHalcon().GDicModelR().ContainsKey(this.Name))
                    {
                        halcon.GetHalcon().GDicModelR().Add(this.Name, MRModelHomMat);
                    }
                    else
                    {
                        halcon.GetHalcon().GDicModelR()[this.Name] = MRModelHomMat;
                    }
                    return false;
                }
                MRModelHomMat.Phi = MRModelHomMat.Angle.TupleDeg();
                for (int i = 0; i < MRModelHomMat.Row.Length; i++)
                {
                    HOperatorSet.VectorAngleToRigid(this.OriginY, this.OriginX, this.OriginU, MRModelHomMat.Row[i], MRModelHomMat.Col[i], MRModelHomMat.Angle[i], out HTuple hTuple);
                    MRModelHomMat.HomMat.Add(hTuple);
                }
                //产生跟随仿射矩阵
                if (mode == "模板区域")
                {
                    HOperatorSet.GetShapeModelContours(out this.OrignXLD, this.ID, 1);
                    HTuple homMat2D;
                    for (int i = 0; i < MRModelHomMat.Row.Length; i++)
                    {
                        HOperatorSet.GetShapeModelOrigin(this.ID, out HTuple row, out HTuple col);
                        HOperatorSet.VectorAngleToRigid(0, 0, 0, MRModelHomMat.Row[i], MRModelHomMat.Col[i], MRModelHomMat.Angle[i], out homMat2D);
                        HOperatorSet.HomMat2dScale(homMat2D, MRModelHomMat.Scale[i], MRModelHomMat.Scale[i], MRModelHomMat.Row[i], MRModelHomMat.Col[i], out this.m_HomMat2D);
                        //把模板轮廓添加到显示图形
                        HOperatorSet.AffineTransContourXld(this.OrignXLD, out xldM, this.m_HomMat2D);
                        AddGreen(xldM);
                        //HOperatorSet.HomMat2dRotate(m_HomMat2D, arrowPhi, MRModelHomMat.Row[i], MRModelHomMat.Col[i], out this.m_HomMat2D);
                        //HOperatorSet.AffineTransContourXld(arrow, out xldM, this.m_HomMat2D);
                        //AddBule(xldM);//
                        HOperatorSet.VectorAngleToRigid(this.OriginY, this.OriginX, this.OriginU, MRModelHomMat.Row[i], MRModelHomMat.Col[i], MRModelHomMat.Angle[i], out this.m_HomMat2D);
                        HOperatorSet.HomMat2dScale(m_HomMat2D, MRModelHomMat.Scale[i], MRModelHomMat.Scale[i], MRModelHomMat.Row[i], MRModelHomMat.Col[i], out this.m_HomMat2D);
                        MRModelHomMat.Angle[i] = MRModelHomMat.Angle[i] - this.OriginU;
                        //把模板轮廓添加到显示图形
                        HOperatorSet.AffineTransContourXld(this.ROIModeXLD, out HObject hObject, this.m_HomMat2D);
                        //AddGreen(hObject);
                        hObject.Dispose();
                    }
                }
                else if (mode == "图片")
                {
                    HOperatorSet.VectorAngleToRigid(MRModelHomMat.Row, MRModelHomMat.Col, MRModelHomMat.Angle, this.OriginY, this.OriginX, 0, out this.m_HomMat2D);
                    HOperatorSet.AffineTransImage(halcon.Image, out HObject images, this.m_HomMat2D, "bicubic", "false");
                    halcon.Image = images;
                    halcon.GetHalcon().ImageHdt(images);
                    AddGreen(ROIModeXLD.Clone());
                }//图片彷设
                else /*if (mode=="绘制区域")*/
                {
                    HOperatorSet.TupleRad(this.OriginUAdd, out HTuple hTuple);
                    for (int i = 0; i < MRModelHomMat.Row.Length; i++)
                    {
                        HOperatorSet.VectorAngleToRigid(this.OriginY, this.OriginX, this.OriginU, MRModelHomMat.Row[i], MRModelHomMat.Col[i], MRModelHomMat.Angle[i], out this.m_HomMat2D);
                        HOperatorSet.HomMat2dScale(m_HomMat2D, MRModelHomMat.Scale[i], MRModelHomMat.Scale[i], MRModelHomMat.Row[i], MRModelHomMat.Col[i], out this.m_HomMat2D);
                        MRModelHomMat.Angle[i] = MRModelHomMat.Angle[i] - this.OriginU;
                        //把模板轮廓添加到显示图形
                        HOperatorSet.AffineTransContourXld(this.ROIModeXLD, out HObject hObject, this.m_HomMat2D);
                        hObject.Dispose();
                        //把模板轮廓添加到显示图形
                        HOperatorSet.AffineTransContourXld(this.ROIModeXLD, out xldM, this.m_HomMat2D);
                        AddGreen(xldM);
                    }
                } //区域彷设
                //int ds = xldM.CountObj();
                HOperatorSet.GenCrossContourXld(out HObject cross, MRModelHomMat.Row, MRModelHomMat.Col, PointSize, MRModelHomMat.Angle);
                if (ISPoint)
                {
                    halcon.AddObj(cross);
                }
                MRModelHomMat.GetModeXld(HObjectGreen);
                MRModelHomMat.LocationOK = true;
                MRModelHomMat.NumberT = MRModelHomMat.Score.Length;
                MRModelHomMat.HomMatMode = mode;
                MRModelHomMat.X = new HTuple();
                MRModelHomMat.Y = new HTuple();
                for (int i = 0; i < MRModelHomMat.NumberT; i++)
                {
                    Coordinate.CpointXY cpointt = halcon.CoordinatePXY.GetPointRctoYX(MRModelHomMat.Row.TupleSelect(i), MRModelHomMat.Col.TupleSelect(i));
                    MRModelHomMat.X.Append(cpointt.X);
                    MRModelHomMat.Y.Append(cpointt.Y);
                }
                Coordinate.CpointXY cpoint = halcon.CoordinatePXY.GetPointRctoYX(this.OriginY, this.OriginX);
                if (IsCot)
                {
                    cpoint = halcon.CoordinatePXY.GetPointRctoYX(Ys, Xs);
                }
                for (int i = 0; i < MRModelHomMat.NumberT; i++)
                {
                    MRModelHomMat.X[i] = MRModelHomMat.X.TupleSelect(i).TupleSub(cpoint.X);
                    MRModelHomMat.Y[i] = MRModelHomMat.Y.TupleSelect(i).TupleSub(cpoint.Y);
                }
                if (CoordinateMeassage != Coordinate.Coordinate_Type.Hide)
                {
                    halcon.AddMeassge(this.Name + ":" + MRModelHomMat.LocationOK + ";数量:" + MRModelHomMat.NumberT);
                }
                else if (CoordinateMeassage == Coordinate.Coordinate_Type.PixelRC)
                {
                    halcon.AddImageMassage(MRModelHomMat.Row, MRModelHomMat.Col, MRModelHomMat.Scale + "row:" + MRModelHomMat.Row + "col:" + MRModelHomMat.Col);
                }
                else if (CoordinateMeassage == Coordinate.Coordinate_Type.XYU2D)
                {
                    MRModelHomMat.Y = MRModelHomMat.Y.TupleAdd(this.OriginYAdd);
                    MRModelHomMat.X = MRModelHomMat.X.TupleAdd(this.OriginXAdd);
                    MRModelHomMat.U = MRModelHomMat.Angle.TupleDeg().TupleAdd(this.OriginUAdd);
                    MRModelHomMat.Angle = MRModelHomMat.U.TupleRad();
                    for (int i = 0; i < MRModelHomMat.X.Length; i++)
                    {
                        message.Append("X:" + MRModelHomMat.X.TupleSelect(i).TupleString(".3f") + ";Y:" + MRModelHomMat.Y.TupleSelect(i).TupleString(".3f") + ";U:" +
                       MRModelHomMat.Angle.TupleSelect(i).TupleDeg().TupleString(".3f") + "分数:" + MRModelHomMat.Score.TupleSelect(i).TupleString(".3f") +
                       ";缩放:" + MRModelHomMat.Scale.TupleSelect(i).TupleString(".3f"));
                    }
                }
                else if (CoordinateMeassage == Coordinate.Coordinate_Type.XYZU3D)
                {
                    if (Vision.Instance.DicCalib3D.ContainsKey(this.Coordinate3DName))
                    {
                        HTuple pose = halcon.GetHalcon().GetRobotBaesPose();
                        if (pose == null)
                        {
                            halcon.AddMeassge(this.Name + ":未获取到机械手当前位置");
                            return false;
                        }
                        bool dsfCon = Vision.Instance.DicCalib3D[Coordinate3DName].Run(calibMode,
                            MRModelHomMat.Row, MRModelHomMat.Col, pose, out MRModelHomMat.X, out MRModelHomMat.Y,
                            out MRModelHomMat.Z, out MRModelHomMat.U, out MRModelHomMat.V, out MRModelHomMat.W, phi, halcon.GetHalcon());
                        if (calibMode == Calib.AutoCalibPoint.CalibMode.移动抓取)
                        {
                            MRModelHomMat.U = MRModelHomMat.Angle.TupleDeg();
                        }
                        if (calibMode == Calib.AutoCalibPoint.CalibMode.固定相机)
                        {
                            Vision.Instance.DicCalib3D[Coordinate3DName].Tool1Base[0] = Vision.Instance.DicCalib3D[Coordinate3DName].Tool1Base[0] + (this.OriginXAdd / 1000);
                            Vision.Instance.DicCalib3D[Coordinate3DName].Tool1Base[1] = Vision.Instance.DicCalib3D[Coordinate3DName].Tool1Base[1] + (this.OriginYAdd / 1000);
                            Vision.Instance.DicCalib3D[Coordinate3DName].Tool1Base[5] = Vision.Instance.DicCalib3D[Coordinate3DName].Tool1Base[5] + this.OriginUAdd;
                        }
                        if (dsfCon)
                        {
                            MRModelHomMat.Y = MRModelHomMat.Y.TupleAdd(this.OriginYAdd);
                            MRModelHomMat.X = MRModelHomMat.X.TupleAdd(this.OriginXAdd);
                            MRModelHomMat.U = MRModelHomMat.U.TupleAdd(this.OriginUAdd);
                            for (int i = 0; i < MRModelHomMat.NumberT; i++)
                            {
                                message.Append("X:" + MRModelHomMat.X.TupleSelect(i).TupleString(".3f") +
                                    ";Y:" + MRModelHomMat.Y.TupleSelect(i).TupleString(".3f") +
                                     ";Z:" + MRModelHomMat.Z.TupleSelect(i).TupleString(".3f") +
                                    ";U:" + MRModelHomMat.U.TupleSelect(i).TupleString(".3f") +
                                    ";V:" + MRModelHomMat.V.TupleSelect(i).TupleString(".3f") +
                                    ";W:" + MRModelHomMat.W.TupleSelect(i).TupleString(".3f") +
                                    "分数:" + MRModelHomMat.Score.TupleSelect(i).TupleString(".3f") +
                               ";缩放:" + MRModelHomMat.Scale.TupleSelect(i).TupleString(".3f"));
                            }
                        }
                        else
                        {
                            MRModelHomMat.X = MRModelHomMat.Y = MRModelHomMat.U = 0;
                            halcon.AddMeassge(this.Name + ":计算标定失败");
                        }
                    }
                    else
                    {
                        halcon.AddMeassge(this.Name + ":标定对象不存在，" + this.Coordinate3DName);
                    }
                }
                string data = "";
                if (IsSeae)
                {
                    halcon.AddImageMassage(MRModelHomMat.Row, MRModelHomMat.Col, MRModelHomMat.Scale);
                }
                if (IsAngle)
                {
                    halcon.AddImageMassage(MRModelHomMat.Row + 40, MRModelHomMat.Col, MRModelHomMat.Phi);
                }
                if (data.Length != 0)
                {
                    halcon.AddMeassge(this.Name + data);
                }
            }
            catch (Exception ex)
            {
                this.LogErr(this.Name + "查找模板错误", ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 查找模板并将窗口显示并返回处理结果的集合
        /// </summary>
        /// <param name="halcon">处理集合</param>
        /// <returns></returns>
        public override bool RunHProgram(OneResultOBj oneResultOBj, out List<OneRObj> oneRObjs, AoiObj aoiObj)
        {
            oneRObjs = new List<OneRObj>();
            base.ClerItem();
            //HOperatorSet.SetShapeModelParam(ID, "timeout", Timeout);
            this.SetDefault("模板X");
            this.SetDefault("模板Y");
            this.SetDefault("模板U");
            bool dst = true;
            Watch.Restart();
            if (!this.FindShapeModel(oneResultOBj, this.Mode, aoiObj))
            {
                oneResultOBj.AddNGOBJ(this.Name, "偏移", aoiObj.SelseAoi, nGRoi);
                NGNumber++;
            }
            Watch.Stop();
            if (IsDip)
            {
                SetDip(MRModelHomMat.Row, MRModelHomMat.Col, MRModelHomMat.Phi, out HObject xldt, out HTuple row, out HTuple col, out HTuple phi, out HTuple idt);
                if (xldt.CountObj() > 0)
                {
                    NGNumber++;
                    nGRoi = nGRoi.ConcatObj(xldt);

                    HOperatorSet.GenRectangle2(out HObject hObject1, row, col, HTuple.TupleGenConst(row.Length, 0),
                        HTuple.TupleGenConst(row.Length, Vision.Instance.DilationRectangle1),
                        HTuple.TupleGenConst(row.Length, Vision.Instance.DilationRectangle1));

                    oneResultOBj.AddImageMassage(row, col, phi.TupleString("0.02f") + ":" + DIPs[idt].Name, ColorResult.red);
                    oneResultOBj.AddNGOBJ(this.Name, "偏移", hObject1, nGRoi);
                    dst = false;
                }
            }
            else if (!dst)
            {
                if (nGRoi.IsInitialized())
                {
                    HOperatorSet.AreaCenter(nGRoi, out HTuple area, out HTuple row, out HTuple column);
                    if (area > 100)
                    {
                        NGNumber++;
                        HOperatorSet.GenRectangle2(out HObject hObject1, row, column, HTuple.TupleGenConst(row.Length, 0),
                        HTuple.TupleGenConst(row.Length, Vision.Instance.DilationRectangle1), HTuple.TupleGenConst(row.Length, Vision.Instance.DilationRectangle1));
                        oneResultOBj.AddNGOBJ(this.Name, "偏移", hObject1, nGRoi);
                    }
                }
            }
            if (!Variation_Model.RunPa(oneResultOBj, this, MRModelHomMat.HomMat))
            {
                NGNumber++;
            }
            foreach (var item in ColorDic)
            {
                try
                {
                    if (!item.Value.Enble)
                    {
                        continue;
                    }
                    if (item.Value.IsHomMat)
                    {
                        if (MRModelHomMat != null)
                        {
                            if (item.Value.DrawObj != null && item.Value.DrawObj.IsInitialized())
                            {
                                for (int i = 0; i < MRModelHomMat.NumberT; i++)
                                {
                                    HOperatorSet.AffineTransRegion(item.Value.DrawObj, out HObject hObjectROI, MRModelHomMat.HomMat[i], "nearest_neighbor");
                                    aoiObj.SelseAoi = hObjectROI;
                                    if (!item.Value.Classify(oneResultOBj, aoiObj, this, out HObject hObject))
                                    {
                                        //oneResultOBj.ADDRed(this.Name, item.Value.Name, hObjectROI, hObject);
                                        NGNumber++;
                                    }
                                    if (this.IsDisObj)
                                    {
                                        oneResultOBj.AddObj(hObject, item.Value.color);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        aoiObj.SelseAoi = item.Value.DrawObj;
                        if (!item.Value.Classify(oneResultOBj, aoiObj, this, out HObject hObject))
                        {
                            //    oneResultOBj.ADDRed(this.Name, item.Value.Name, item.Value.DrawObj.Clone(), hObject);
                            NGNumber++;
                        }
                        if (this.IsDisObj)
                        {
                            oneResultOBj.AddObj(hObject, item.Value.color);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            if (oneResultOBj.GetHalcon() != null)
            {
                oneResultOBj.GetHalcon().MRModelHomMat = MRModelHomMat;
                if (!oneResultOBj.GetHalcon().GDicModelR().ContainsKey(this.Name))
                {
                    oneResultOBj.GetHalcon().GDicModelR().Add(this.Name, MRModelHomMat);
                }
                else
                {
                    oneResultOBj.GetHalcon().GDicModelR()[this.Name] = MRModelHomMat;
                }
                string data = "ModelVision,null";
                if (oneResultOBj.GetHalcon().GDicModelR().ContainsKey(this.Name))
                {
                    data = "ModelVision," + this.Name + "," + oneResultOBj.GetHalcon().GDicModelR()[this.Name].NumberT.ToString() + ",";
                    this["模板数量"] = oneResultOBj.GetHalcon().GDicModelR()[this.Name].NumberT;
                    this["模板X"] = oneResultOBj.GetHalcon().GDicModelR()[this.Name].X;
                    this["模板Y"] = oneResultOBj.GetHalcon().GDicModelR()[this.Name].Y;
                    this["模板U"] = oneResultOBj.GetHalcon().GDicModelR()[this.Name].Angle.TupleDeg();
                    this["模板Row"] = oneResultOBj.GetHalcon().GDicModelR()[this.Name].Row;
                    this["模板Col"] = oneResultOBj.GetHalcon().GDicModelR()[this.Name].Col;
                    this["模板Row"] = oneResultOBj.GetHalcon().GDicModelR()[this.Name].Row;
                    this["模板Angle"] = oneResultOBj.GetHalcon().GDicModelR()[this.Name].Angle;
                    this["模板分数"] = oneResultOBj.GetHalcon().GDicModelR()[this.Name].Score;
                }
                if (dst)
                {
                    if (CoordinateMeassage == Coordinate.Coordinate_Type.XYZU3D)
                    {
                        this["模板V"] = MRModelHomMat.V;
                        this["模板Z"] = MRModelHomMat.Z;
                        this["模板W"] = MRModelHomMat.W;
                        if (calibMode == Calib.AutoCalibPoint.CalibMode.移动放置)
                        {
                            for (int i = 0; i < oneResultOBj.GetHalcon().GDicModelR()[this.Name].NumberT; i++)
                            {
                                data += oneResultOBj.GetHalcon().GDicModelR()[this.Name].X.TupleSelect(i).TupleString("0.3f") + "," +
                                    oneResultOBj.GetHalcon().GDicModelR()[this.Name].Y.TupleSelect(i).TupleString("0.3f") + ","
                                    + oneResultOBj.GetHalcon().GDicModelR()[this.Name].Z.TupleSelect(i).TupleString("0.3f") + ","
                                    + oneResultOBj.GetHalcon().GDicModelR()[this.Name].U.TupleSelect(i).TupleString("0.3f") + ","
                                        + oneResultOBj.GetHalcon().GDicModelR()[this.Name].V.TupleSelect(i).TupleString("0.3f") + ","
                                             + oneResultOBj.GetHalcon().GDicModelR()[this.Name].W.TupleSelect(i).TupleString("0.3f");
                            }
                        }
                        else
                        {
                            for (int i = 0; i < oneResultOBj.GetHalcon().GDicModelR()[this.Name].NumberT; i++)
                            {
                                data += oneResultOBj.GetHalcon().GDicModelR()[this.Name].X.TupleSelect(i).TupleString("0.3f") +
                                    "," + oneResultOBj.GetHalcon().GDicModelR()[this.Name].Y.TupleSelect(i).TupleString("0.3f")
                                    + "," + oneResultOBj.GetHalcon().GDicModelR()[this.Name].U.TupleSelect(i).TupleString("0.3f") + ",";
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < oneResultOBj.GetHalcon().GDicModelR()[this.Name].NumberT; i++)
                        {
                            data += oneResultOBj.GetHalcon().GDicModelR()[this.Name].X.TupleSelect(i).TupleString("0.3f") +
                                "," + oneResultOBj.GetHalcon().GDicModelR()[this.Name].Y.TupleSelect(i).TupleString("0.3f")
                                + "," + oneResultOBj.GetHalcon().GDicModelR()[this.Name].Angle.TupleSelect(i).TupleDeg().TupleString("0.3f") + ",";
                        }
                    }
                }
                oneResultOBj.GetHalcon().SendMesage(data.TrimEnd(','));
            }
            if (dst)
            {
                StaticCon.SetLinkAddressValue(this.MXName, "Single", MRModelHomMat.X.TupleString("0.3f"));
                StaticCon.SetLinkAddressValue(this.MYName, "Single", MRModelHomMat.Y.TupleString("0.3f"));
                StaticCon.SetLinkAddressValue(this.MUName, "Single", MRModelHomMat.U.TupleString("0.3f"));
            }
            if (ContNumber >= 0 && ContNumber != MRModelHomMat.NumberT)
            {
                NGNumber++;
            }
            if (NGNumber != 0)
            {
                //oneResultOBj.ADDRed(this.Name, AOIObj, AOIObj);
                return false;
            }
            oneResultOBj.AddOKOBj(new OneCompOBJs.OneComponent() { ComponentID = this.Name });
            return true;
        }

        public void GetPoint(double row, double col, ModelVision.RModelHomMat rModelHomMat, out double rowt, out double colT)
        {
            rowt = 0;
            colT = 0;
            if (Mode == "绘制区域")
            {
                HOperatorSet.VectorAngleToRigid(rModelHomMat.Row[0], rModelHomMat.Col[0], rModelHomMat.Angle[0],
                    this.OriginY, this.OriginX, this.OriginU, out this.m_HomMat2D);
            }
            else if (Mode == "模板区域")
            {
                HOperatorSet.VectorAngleToRigid(rModelHomMat.Row[0], rModelHomMat.Col[0], rModelHomMat.Angle[0],
               0, 0, 0, out this.m_HomMat2D);
            }
            HOperatorSet.HomMat2dScale(m_HomMat2D, rModelHomMat.Scale[0],
           rModelHomMat.Scale[0], rModelHomMat.Row[0], rModelHomMat.Col[0], out this.m_HomMat2D);
            HOperatorSet.AffineTransPoint2d(this.m_HomMat2D, row, col, out HTuple hTupleRow, out HTuple hTupleCol);
            rowt = hTupleRow.D;
            colT = hTupleCol.D;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hamHam2d"></param>
        /// <param name="halcon"></param>
        /// <param name="OutCentreRow"></param>
        /// <param name="OutCentreCol"></param>
        /// <returns></returns>
        private bool Angle(HTuple hamHam2d, HalconRun halcon, out HTuple OutCentreRow, out HTuple OutCentreCol, out HTuple phi)
        {
            phi = OutCentreCol = OutCentreRow = new HTuple();
            if (this.Dic_Measure.Keys_Measure.ContainsKey("angle1") && this.Dic_Measure.Keys_Measure.ContainsKey("angle2"))
            {
                nGRoi = nGRoi.ConcatObj(this.Dic_Measure["angle1"].MeasureObj(halcon, hamHam2d, halcon.GetOneImageR())._HObject);
                nGRoi = nGRoi.ConcatObj(this.Dic_Measure["angle2"].MeasureObj(halcon, hamHam2d, halcon.GetOneImageR())._HObject);
                //      halcon.SetAddObj(this.Name + "Cilcre", this.Dic_Measure["Cilcre"].MeasureObj(halcon, homMat)._HObject);
                if (this.Dic_Measure["angle1"].OutCentreRow != 0 && this.Dic_Measure["angle2"].OutCentreCol != 0)
                {
                    OutCentreRow = this.Dic_Measure["angle1"].OutCentreRow;
                    OutCentreCol = this.Dic_Measure["angle1"].OutCentreCol;
                    OutCentreRow.Append(this.Dic_Measure["angle2"].OutCentreRow);
                    OutCentreCol.Append(this.Dic_Measure["angle2"].OutCentreCol);
                    HOperatorSet.LinePosition(this.Dic_Measure["angle1"].OutCentreRow, this.Dic_Measure["angle1"].OutCentreCol,
                       this.Dic_Measure["angle2"].OutCentreRow, this.Dic_Measure["angle2"].OutCentreCol,
                       out HTuple rowcenter, out HTuple colcenter, out HTuple Length, out phi);

                    phi = phi.TupleDeg();
                    return true;
                }
            }
            else if (this.Dic_Measure.Keys_Measure.ContainsKey("angle"))
            {
                nGRoi = nGRoi.ConcatObj(this.Dic_Measure["angle"].MeasureObj(halcon, hamHam2d, halcon.GetOneImageR())._HObject);

                if (this.Dic_Measure["angle"].IsExist("平行线夹角"))
                {
                    if (this.Dic_Measure["angle"].OutPhi != 0)
                    {
                        phi = this.Dic_Measure["angle"]["平行线夹角"];
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 测量圆形纠正
        /// </summary>
        /// <param name="halcon"></param>
        /// <param name="homMat"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private bool CilcreM(HalconRun halcon, HTuple homMat, out HTuple row, out HTuple col)
        {
            row = new HTuple();
            col = new HTuple();
            int ds = 0;
            var detee = from n in this.Dic_Measure.Keys_Measure
                        where n.Key.StartsWith("Cilcre") && n.Key.Length >= 7
                        orderby int.Parse(n.Key.Remove(0, 6)) ascending
                        select n;
            foreach (var item in detee)
            {
                ds++;
                halcon.AddObj(item.Value.MeasureObj(halcon, homMat, halcon.GetOneImageR())._HObject);
                if (item.Value.OutCentreRow != 0 && item.Value.OutCentreCol != 0)
                {
                    row.Append(item.Value.OutCentreRow);
                    col.Append(item.Value.OutCentreCol);
                }
            }
            if (this.Dic_Measure.Keys_Measure.ContainsKey("Cilcre"))
            {
                ds++;
                halcon.AddObj(this.Dic_Measure.Keys_Measure["Cilcre"].MeasureObj(halcon, homMat, halcon.GetOneImageR())._HObject);
                if (this.Dic_Measure.Keys_Measure["Cilcre"].OutCentreRow != 0 && this.Dic_Measure.Keys_Measure["Cilcre"].OutCentreCol != 0)
                {
                    row.Append(this.Dic_Measure.Keys_Measure["Cilcre"].OutCentreRow);
                    col.Append(this.Dic_Measure.Keys_Measure["Cilcre"].OutCentreCol);
                }
            }
            if (ds == row.Length)
            {
                return true;
            }
            return false;
        }
    }
}