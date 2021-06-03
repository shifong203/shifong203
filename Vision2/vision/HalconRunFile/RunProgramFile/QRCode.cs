using HalconDotNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.UI.PropertyGrid;
using Vision2.Project.DebugF;
using Vision2.vision.HalconRunFile.Controls;
using static Vision2.vision.Vision;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
    /// <summary>
    /// 二维码识别
    /// </summary>
    public class QRCode : RunProgram
    {
        #region 重写父类方法

        public override string FileName => "QRCode";

        /// <summary>
        /// 初始化读取实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Path"></param>
        /// <returns></returns>
        public override RunProgram UpSatrt<T>(string Path)
        {
            Instance = RradModel(Path);
            TimeOut = timeout;
            return Instance;
        }
        public override Control GetControl()
        {
            return new QRCodeControl1(this, this.GetPThis() as HalconRun) { Dock = DockStyle.Fill };
        }

        /// <summary>
        /// 初始化读取模板
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public QRCode RradModel(string path)
        {

            Directory.CreateDirectory(Path.GetDirectoryName(path));
            try
            {
                if (File.Exists(path + this.SuffixName))
                {
                    string stxt = File.ReadAllText(path + this.SuffixName);//以字符串形式读取文件
                    Instance = JsonConvert.DeserializeObject<QRCode>(stxt);
                }
                else
                {
                    MessageBox.Show("读取二维码文件失败，文件" + path + this.SuffixName + "不存在");
                }
                try
                {
                    HOperatorSet.ClearDataCode2dModel(Instance.ID);
                }
                catch { }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
            if (File.Exists(path + ".dcm"))
            {
                HOperatorSet.ReadDataCode2dModel(path + ".dcm", out ID);
                Instance.ID = ID;
                Instance.SetDataProgram();
            }
            else
            {
                MessageBox.Show("缺少二维码模板文件:" + path + ".dcm");
            }

            return Instance;
        }

        /// <summary>
        /// 保存实例
        /// </summary>
        /// <param name="path"></param>
        public override void SaveThis(string path)
        {
            base.SaveThis(path);
            try
            {
                System.IO.Directory.CreateDirectory(path + "\\" + this.Name);

                HOperatorSet.WriteDataCode2dModel(this.ID, path + "\\" + this.Name + "\\" + this.Name + ".dcm");


            }
            catch (Exception ex)
            {
                MessageBox.Show("保存出错：" + ex.Message + ";保存地址：" + path + "\\" + this.Name + "\\" + this.Name + ".dcm");
            }
        }

        #endregion 重写父类方法
        public class OneQR
        {


            public int numer = 1;
            [Description("element_size_min基本条码元素的最小尺寸(也称为“模块”" +
                "或“窄条”，默认2;条码的最小尺寸，指条码宽度和间距，" +
                "大码应设大一点，减少处理时间"), Category("一维码参数"), DisplayName("最小窄条宽度,指条码宽度和间距")]
            public double element_size_min { get; set; } = -10;
            [Description("element_size_max基本条码元素的最大尺寸(也称为“模块”" +
                "或“窄条”默认8;条码的最大尺寸，不能过小也不能过大"), Category("一维码参数"), DisplayName("最大窄条宽度")]
            public double element_size_max { get; set; } = -10;
            [Description("barcode_width_min最小条码宽度。该参数的值以像素为单位定义。" +
                "条码的宽度取决于许多因素"), Category("一维码参数"), DisplayName("最小条码宽度")]
            public double barcode_width_min { get; set; } = -10;
            [Description("Check_char是否验证校验位，'absent'不检查校验和，" +
                "'present'检查校验和、['absent', 'present', 'preserved']"),
                Category("一维码参数"), DisplayName("校验和")]
            [TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDown("", false, "", "absent", "present", "preserved")]
            public string Check_char { get; set; } = "absent";


            [Description("barcode_height_min该参数的值以像素为单位定义。默认值为-1，" +
                "这意味着条形码阅读器自动从其他参数获取合理的高度。 " +
                "对于非常平和非常高的条码，手动调整这个参数是必要的。" +
                "对于高度小于16像素的条码，用户应自行设置其各自的高度。" +
                " 注意，最小值是8像素。如果条形码非常高，即70像素以上，" +
                "手动调整到各自的高度会导致后续查找和读取操作的加速。"), 
                Category("一维码参数"), DisplayName("最小条码高度")]
            public double barcode_height_min { get; set; } = -10;

            [Description("num_scanlines扫描(候选)条形码时使用的最大扫描线数。解码时所用扫描线的最大数目，" +
                "设置为0表示自动确定，一般设置为2-30"), Category("一维码参数"), DisplayName("最大扫描线数")]
            public double num_scanlines { get; set; } = -10;

            [Description("min_identical_scanlines有了这个参数，可以降低误读条码或在没有条码的地方误读的概率。" +
                "该参数指定返回相同数据以成功读取条形码的解码扫描线的最小数目。" +
                "如果将此参数设置为0则认为已使用第一个成功解码的扫描线对条形码进行解码"), 
                Category("一维码参数"), DisplayName("解码扫描线的最小数目")]
            public double min_identical_scanlines { get; set; } = -10;

            [Description("meas_thresh_abs如果将扫描线放置在灰度值动态范围为零或很小的图像区域(如灰度值都在255附近的白色区域)，" +
                "那么基于‘meas_thresh’的边缘检测阈值会计算得非常小。这通常会导致检测大量的假边。" +
                "'meas_thresh_abs'用于防止这种错误检测。如果基于'meas_thresh'的阈值小于'meas_thresh_abs'的值，" +
                "则使用后者作为阈值。默认情况下，'meas_thresh_abs'设置为5.0。" +
                "较大的值可能更适合具有高噪声水平的图像。" + "另一方面，在对比度很弱的无噪声图像中，" +
                "该参数可能会干扰对真实边缘的检测，" +
                "因此可能需要将其设置为0.0来减少甚至完全禁用该参数。"), Category("一维码参数"), DisplayName("边缘阈值")]
            public double meas_thresh_abs { get; set; } = -10;


            public double meas_thresh { get; set; } = -10;
            [Description("persistence设置为1，则会保留中间结果，评估条码印刷质量时会用到"), 
                Category("一维码参数"), DisplayName("保留结果")]
            public double persistence { get; set; } = -1;
            [Description("contrast_min条码元素的前景和背景之间的最小对比。将该参数设置为大于5的值将帮助操作符" +
                "find_bar_code优化候选区域搜索。" +
                "find_bar_code将拒绝对比度值小于'contrast_min'的所有候选区域。" +
                "因此，设置较高的'contrast_min'值将提高find_bar_code的运行时性能。" +
                "但是，请注意，所有对比值低于'contrast_min'的条形码将不会被读取。计算的对比值是一个近似值，以加快执行时间。" +
                "尝试为'contrast_min'设置一个较低的阈值，以便找到可能被拒绝的条形码，并且其对比度值接近指定的'contrast_min'。"),
                Category("一维码参数"), DisplayName("最小对比度")]
            public double contrast_min { get; set; } = -10;

            [Description("start_stop_tolerance容许误差值，可设置为'low'或者'high'，设置为'high'可能造成误判"), 
                Category("一维码参数"), DisplayName("容许误差")]
            [TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDown("", false, "", "low", "high")]
            public string start_stop_tolerance { get; set; } = "high";
            [Description(" element_height_min 条码的最小高度，默认值-1表示自动推测条码高度，该参数对速度影响大"), 
                Category("一维码参数"), DisplayName("条码的最小高度")]
            public int element_height_min { get; set; } = -1;

            [Description("quiet_zone强制验证条形码符号的静区。当启用时，当在检测到的条码序列的左或右安静区域内检测到意外的条码时，" +
                "扫描线将被拒绝。使用'quiet_zone'='true'时，静区必须至少与相应的条形码标准指定的宽度相同。" +
                "参考值'false', 'true', 'tolerant', 1, 2, 3, 4, 5"), Category("一维码参数"), DisplayName("强制验证静区")]
            [TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDown("", false, "", "false","true", "tolerant", "1", "2", "3", "4", "5")]
            public string quiet_zone { get; set; } = "true";

            [Description("图像缩放比例，参考值 1, 2, 3, 4, 5"), Category("一维码参数"), DisplayName("图像缩放")]
            public int ImageZoomSize { get; set; } = 1;
            public void SetParam(HTuple id)
            {
                try
                {
                    try
                    {
                        if (Check_char != "")
                        {
                            HOperatorSet.SetBarCodeParam(id, "Check_char", Check_char);
                        }
                    }
                    catch { }
                    try
                    {

                        if (quiet_zone != "")
                        {
                            HOperatorSet.SetBarCodeParam(id, "quiet_zone", quiet_zone);
                        }
                    }
                    catch { }
                    try
                    {
                        if (element_height_min >= 0)
                        {
                            HOperatorSet.SetBarCodeParam(id, "element_height_min", element_height_min);
                        }
                    }
                    catch { }
                    try
                    {
                        if (start_stop_tolerance != "")
                        {
                            HOperatorSet.SetBarCodeParam(id, "start_stop_tolerance", start_stop_tolerance);
                        }
                    }
                    catch { }
                    try
                    {
                        if (element_size_min >= 0)
                        {
                            HOperatorSet.SetBarCodeParam(id, "element_size_min", element_size_min);
                        }
                    }
                    catch { }
                    try
                    {
                        if (contrast_min >= 0)
                        {
                            HOperatorSet.SetBarCodeParam(id, "contrast_min", contrast_min);
                        }
                    }
                    catch { }
                    try
                    {
                        if (element_size_max >= 0)
                        {
                            HOperatorSet.SetBarCodeParam(id, "element_size_max", element_size_max);
                        }
                    }
                    catch { }
                    try
                    {
                        if (barcode_width_min >= 0)
                        {
                            HOperatorSet.SetBarCodeParam(id, "barcode_width_min", barcode_width_min);
                        }
                    }
                    catch { }
                    try
                    {
                        if (barcode_height_min >= 0)
                        {
                            HOperatorSet.SetBarCodeParam(id, "barcode_height_min", barcode_height_min);
                        }
                    }
                    catch { }
                    try
                    {
                        if (num_scanlines >= 0)
                        {
                            HOperatorSet.SetBarCodeParam(id, "num_scanlines", num_scanlines);
                        }
                    }
                    catch { }
                    try
                    {
                        if (min_identical_scanlines >= 0)
                        {
                            HOperatorSet.SetBarCodeParam(id, "min_identical_scanlines", min_identical_scanlines);
                        }
                    }
                    catch 
                    {
                    }
                    try
                    {
                        if (meas_thresh_abs >= 0)
                    {
                        HOperatorSet.SetBarCodeParam(id, "meas_thresh_abs", meas_thresh_abs);
                        }
                    }
                    catch
                    {
                    }
                    try
                    {
                        if (meas_thresh >= 0)
                    {
                        HOperatorSet.SetBarCodeParam(id, "meas_thresh", meas_thresh);
                        }
                    }
                    catch
                    {
                    }
                    try
                    {
                        if (persistence >= 0)
                        {
                            HOperatorSet.SetBarCodeParam(id, "persistence", persistence);
                        }
                    }
                    catch
                    {
                    }
                }
                catch (Exception ex)
                {
                }
            }

            public void Find1DCode(HObject image, out HObject hObjecXLD, out HTuple text)
            {
                text = new HTuple();
                hObjecXLD = new HObject();
                HObject hObject = image;
                try
                {
                    if (ImageZoomSize!=1)
                    {
                        HOperatorSet.GetImageSize(image, out HTuple width, out HTuple height);
                        HOperatorSet.ZoomImageSize(image, out  hObject, width /ImageZoomSize, height / ImageZoomSize, "constant");
                    }
                    HOperatorSet.CreateBarCodeModel(new HTuple(), new HTuple(), out HTuple id);
                    //HOperatorSet.CreateBarCodeModel("train", "", out HTuple id);
                    HOperatorSet.SetBarCodeParam(id, "stop_after_result_num", numer);
                    SetParam(id);
                    HOperatorSet.FindBarCode(hObject, out hObjecXLD, id, "auto", out text);
                    if (ImageZoomSize != 1)
                    {
                        HOperatorSet.HomMat2dIdentity(out HTuple dase);
                        HOperatorSet.HomMat2dScale(dase, ImageZoomSize, ImageZoomSize, 1, 1, out HTuple hTuple);
                        HOperatorSet.AffineTransRegion(hObjecXLD, out hObjecXLD, hTuple, "nearest_neighbor");
                    }
                    
                    HOperatorSet.ClearBarCodeModel(id);
                }
                catch (Exception ex)
                {
                    ErosProjcetDLL.Project.AlarmText.AddTextNewLine(ex.Message);
                }
            }
        }
        public void SetParam(HTuple id)
        {
            try
            {
                //if (timeout >= 0)
                //{
                //    HOperatorSet.SetDataCode2dParam(this.ID, "timeout", timeout);
                //}
                //if (contrast_min >= 0)
                //{

                //    HOperatorSet.SetDataCode2dParam(id, "contrast_min", contrast_min);
                //}
                //HOperatorSet.SetDataCode2dParam(this.ID, "polarity", isPolarity);
                //HOperatorSet.SetDataCode2dParam(id, "persistence", -1);
            }
            catch (Exception)
            {
            }
        }

        public OneQR One_QR { get; set; } = new OneQR();
        public int XNumber { get; set; } = 4;
        public int YNumber { get; set; } = 5;
        /// <summary>
        /// 起点
        /// </summary>
        public int XLocation { get; set; } = 100;
        /// <summary>
        /// 起点
        /// </summary>
        public int YLocation { get; set; } = 100;

        public int Height { get; set; } = 100;
        /// <summary>
        /// 间距
        /// </summary>
        public int XInterval { get; set; } = 100;
        /// <summary>
        /// 间距
        /// </summary>
        public int YInterval { get; set; } = 100;
        public HTuple YRows { get; set; }
        public HTuple XCols { get; set; }
        #region 运行识别

        /// <summary>
        /// 二维码ID
        /// </summary>
        HTuple ID = new HTuple(-1);

        [DescriptionAttribute("控制操作符行为的(可选)参数的名称。'stop_after_result_num', 'train'"),
            Category("识别参数"), DisplayName("参数操作符"), TypeConverter(typeof(ErosConverter)),
         ErosConverter.ThisDropDown("", "", "train", "stop_after_result_num")]
        public string GenParamName { get; set; } = "";

        /// <summary>
        /// 码数量
        /// </summary>
        public int IDValue { get; set; } = 100;

        [Category("识别参数"), DisplayName("识别超时"), Description("无法识别超时时间，ms")]
        public int TimeOut
        {
            get { return timeout; }
            set
            {
                timeout = value;
                try
                {

                }
                catch (Exception)
                {
                }
            }
        }

        private int timeout = 1000;

        [Category("识别参数"), DisplayName("对比度"), Description("最小对比度")]
        public sbyte Contrast_min
        {
            get { return contrast_min; }
            set
            {
                contrast_min = value;
            }
        }
        sbyte contrast_min;


        /// <summary>
        ///0=左上横向
        ///1=左上竖向
        ///2=右上横向
        ///3=右上竖向
        ///4=左下竖向
        ///5=左下横向
        ///6=右下横向
        ///7=右下竖向
        /// </summary>
        public int MatrixType { get; set; }

        /// <summary>
        /// 黑白底色
        /// </summary>
        [Category("识别参数"), DisplayName("黑白底色"), Description("light_on_dark为黑低白码，dark_on_light白底黑码,any全部"),
            TypeConverter(typeof(ErosConverter)),
      ErosConverter.ThisDropDown("", "dark_on_light", "light_on_dark", "any")]
        public string IsPolarity
        {
            get { return isPolarity; }
            set
            {
                isPolarity = value;
            }
        }
        string isPolarity = "any";
        /// <summary>
        /// 模板最小像素大小
        /// </summary>
        [Category("识别参数"), DisplayName("模板最小像素大小"), Description("单个模板最小像素最大100，最小1")]
        public int Module_size_min
        {
            get { return module_size_min; }
            set
            {
                module_size_min = value;
            }
        }
        int module_size_min = 1;

        /// <summary>
        /// 模板最小像素大小
        /// </summary>
        [Category("识别参数"), DisplayName("模板最大像素大小"), Description("单个模板最大像素最大100，最小1")]
        public int Module_size_mam
        {
            get { return module_size_mam; }
            set
            {
                module_size_mam = value;
            }
        }
        int module_size_mam = 100;

        /// <summary>
        /// 是否分割识别0全图，1点矩阵识别，2分割识别
        /// </summary>
        [Category("识别参数"), DisplayName("是否分割识别"), Description("确定是否使用分割识别扫码")]
        public int DiscernType { get; set; }

        /// <summary>
        /// 是否分割识别0全图，1点矩阵识别，2分割识别
        /// </summary>
        [Category("识别参数"), DisplayName("重复码处理"), Description("重复码剔除，或报警")]
        public int QRCOntEn { get; set; }



        [Category("码类型"), DisplayName("2维/1维码"), Description("2D=false,1D为True")]
        public bool Is2D { get; set; }



        #endregion 运行识别
        #region 扫码输出
        public StringBuilder DecodedDataString = new StringBuilder();
        public override string ShowHelpText()
        {
            return "2.2创建扫码识别";
        }
        [DescriptionAttribute("成功解码后输出格式。Auto解码,Find查询码，SetCode码写入过程信息"),
         Category("扫码输出"), DisplayName("输出格式"), TypeConverter(typeof(ErosConverter)),
         ErosConverter.ThisDropDown("", "Auto", "Find", "SetCode")]
        public string DecodedDataType
        {
            get
            { return decodedDataType; }
            set { decodedDataType = value; }
        }
        private string decodedDataType = "Auto";
        [DescriptionAttribute("是否多个产品码"),
        Category("扫码输出"), DisplayName("多个产品码")]
        public bool IsCont { get; set; }
        [DescriptionAttribute("二维码解码数量"),
                Category("扫码输出"), DisplayName("码数量")]
        public int NumberInt { get { return number; } set { number = value; } }
        private int number;
        [DescriptionAttribute("输出二维码变量名。"), Category("触发器"), DisplayName("输出二维码变量名称")]
        [Editor(typeof(ErosSocket.ErosConLink.LinkName_ValuesNameUserControl.Editor), typeof(UITypeEditor))]
        public string QRStringName { get; set; } = string.Empty;
        #endregion 扫码输出
        #region 创建二维码参数

        [DescriptionAttribute("二维码类型:Aztec Code, Data Matrix ECC 200, GS1 Aztec Code, GS1 DataMatrix," +
        "GS1 QR Code, Micro QR Code, PDF417, QR Code"),
          Category("创建参数"), DisplayName("二维码类型"), TypeConverter(typeof(ErosConverter)),
    ErosConverter.ThisDropDown("Aztec Code", "Data Matrix ECC 200", "GS1 Aztec Code", "GS1 DataMatrix",
         "GS1 QR Code", "Micro QR Code", "PDF417", "QR Code")]
        public string SymbolType { get; set; } = "Data Matrix ECC 200";

        /// <summary>
        /// 创建模式
        /// </summary>
        [DescriptionAttribute("二维码参数值:标准模式standard_recognition,加强模式enhanced_recognition,最强模式maximum_recognition"),
        Category("创建参数"), DisplayName("创建模式"), TypeConverter(typeof(ErosConverter)),
  ErosConverter.ThisDropDown("", "", "standard_recognition", "enhanced_recognition", "maximum_recognition")]
        public string CreateGenParamValue { get; set; } = "";

        private QRCode Instance;

        #endregion 创建二维码参数
        [Category("扫码输出"), DisplayName("分隔符"), Description("多个二维码分割")]
        public char SiptS { get; set; } = ',';
        [Category("扫码输出"), DisplayName("匹配的字段"), Description("二维码匹配的字段名")]
        public string FindQRCODE { get; set; } = "";
        [Category("扫码输出"), DisplayName("查询的字段"), Description("二维码匹配查询的字段名")]
        public string FindQr { get; set; } = "";
        [DescriptionAttribute("码的字符长度限制"),
           Category("扫码输出"), DisplayName("码长度")]
        public int QRCont { get; set; }
        [DescriptionAttribute(""),
         Category("训练"), DisplayName("训练次数")]
        /// <summary>
        /// 训练次数
        /// </summary>
        public int ISCont { get; set; }
        [DescriptionAttribute(""),
        Category("托盘"), DisplayName("码托盘数量")]

        public int TrayNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> QRText = new List<string>();
        public List<int> TrayIDS { get; set; } = new List<int>();
        public int TrayIDNumber { get; set; } = -1;

        /// <summary>
        /// 分割识别
        /// </summary>
        /// <param name="halcon"></param>
        private void threadAnyCodeString(HalconRun halcon,HObject hObject)
        {
            number = 0;
            foreach (var item in this.KeyHObject.DirectoryHObject)
            {
                try
                {
                    HObject reduceDomainObj = new HObject();
                    HOperatorSet.ReduceDomain(hObject, item.Value, out reduceDomainObj);
                    FindS(reduceDomainObj,halcon);
                }
                catch (Exception ex)
                {
                    halcon.ErrLog(this.Name + ":" + item.Key, ex.Message);
                }
            }

            //decodedDataStrings = decodedDataStrings.tr(SiptS);
        }
        /// <summary>
        /// 二维码ID
        /// </summary>
        List<HTuple> IDs = new List<HTuple>();
        HTuple resultHandles = new HTuple();
        public void FindS(HObject hObject,HalconRun halcon)
        {
            HTuple row, colu = null, area;
            HObject hObject2 = new HObject();
            HObject hObject3 = new HObject();
            HObject hOQERoi = new HObject();
            HObject hObjectImage= hObject;
            HTuple textS = new HTuple();
            QRText = new List<string>();
            try
            {
                hOQERoi.GenEmptyObj();
                hObject3.GenEmptyObj();
                hObject2.GenEmptyObj();
                if (DiscernType == 1)
                {
                    if (this.Rows != null && this.Rows.Length > 0)
                    {
                        HOperatorSet.GenRectangle1(out HObject hObject4, 0, 0, 2, 2);
                        HOperatorSet.GenRectangle1(out hObject3, this.Rows - Height, Cols - this.Height, Rows + this.Height, Cols + this.Height);
                        hObject3 = hObject3.ConcatObj(hObject4);
                        HOperatorSet.Union1(hObject3, out hObject2);
                        HOperatorSet.ReduceDomain(hObject, hObject2, out hObject);
                        HOperatorSet.CropDomain(hObject, out  hObjectImage);
                    }
                }
                HTuple text = FindDatacode2d(hObjectImage, ID, out HObject hObject1);
                if (text.Length != 0)
                {
                    textS.Append(text.ToSArr());
                    NumberInt = NumberInt + text.Length;
      
                    if (hObject1.GetObjClass() == "xld_cont")
                    {
                        HOperatorSet.SmallestRectangle2Xld(hObject1, out HTuple row1, out HTuple column1, out HTuple phi, out HTuple lengt1, out HTuple length2);
                        HOperatorSet.GenRectangle2(out hObject1, row1, column1, phi, lengt1, length2);
                        //HOperatorSet.Union1(hObject1, out hObject1);
                        //HOperatorSet.Connection(hObject1, out hObject1);
                    }
                    hOQERoi = hOQERoi.ConcatObj(hObject1);
                    if (this.ISShowText)
                    {
                        this.GetPThis().AddMessage("第1次" + text.Length);
                    }
                    AddGreen(hObject1);
                    if (DiscernType == 1)
                    {
                        //if (this.Rows != null)
                        //{
                        //    HOperatorSet.DilationCircle(hObject1, out HObject hObject4, Height/2);
                        //    HOperatorSet.Connection(hObject4, out hObject2);
                        //    HOperatorSet.Difference(hObject2, hObject4, out hObject2);
                        //    HOperatorSet.SelectShape(hObject2, out hObject2, "area", "and", (this.Height * 2 * this.Height * 2) - 100, 9999999999999);
                        //}
                    }
                    try
                    {
                        hObject2 = hObject1;
                        if (NumberInt != IDValue)
                        {
                            if (IDs == null)
                            {
                                IDs = new List<HTuple>();
                            }
                            for (int i = 0; i < ISCont; i++)
                            {
                                if (NumberInt == IDValue)
                                {
                                    break;
                                }
                                HTuple tupleStr = "";
                                //HOperatorSet.DilationCircle(hObject2, out  hObject2, Height / 2);
                                HOperatorSet.Union1(hObject2, out hObject2);
                                //halcon.AddOBJ(hObject2,ColorResult.red);
                                HOperatorSet.Complement(hObject2, out HObject hObject4);
                                HOperatorSet.ReduceDomain(hObjectImage, hObject4, out hObjectImage);
                                HOperatorSet.CropDomain(hObjectImage, out hObjectImage);
                                //halcon.Image(hObjectImage);
                                if (IDs.Count<=i)
                                {
                                    HOperatorSet.CreateDataCode2dModel(this.SymbolType, new HTuple(), new HTuple(), out HTuple hTuple1);
                                    IDs.Add(hTuple1);
                                    HOperatorSet.FindDataCode2d(hObjectImage, out hObject1, IDs[i], "train", "all", out resultHandles, out tupleStr);
                                }
                                if (resultHandles.Length == 0)
                                {
                                    break;
                                }
                                HOperatorSet.FindDataCode2d(hObjectImage, out hObject2, IDs[i], "stop_after_result_num", IDValue.ToString(), out resultHandles, out HTuple tupleStr2);
                                if (resultHandles.Length == 0)
                                {
                                    break;
                                }
                                textS.Append(tupleStr2);
                                NumberInt = NumberInt + tupleStr2.Length;
                                if (hObject2.GetObjClass() == "xld_cont")
                                {
                                    HOperatorSet.SmallestRectangle2Xld(hObject2, out HTuple row1, out HTuple column1, out HTuple phi, out HTuple lengt1, out HTuple length2);
                                    HOperatorSet.GenRectangle2(out  hObject2, row1, column1, phi, lengt1, length2);
                                }
                                AddYellow(hObject2);
                                hOQERoi = hOQERoi.ConcatObj(hObject2);
                                if (this.ISShowText)
                                {
                                    this.GetPThis().AddMessage("第" + (i + 2) + ":" + tupleStr2.Length);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    HOperatorSet.AreaCenter(hOQERoi, out area, out row, out colu);
         
                    if (DiscernType == 1)
                    {  
                        if (Rows != null && this.Rows.Length > 0)
                        {
                            HOperatorSet.DilationCircle(hOQERoi, out HObject hObject4, Height / 2);
                            //HOperatorSet.Connection(hObject4, out hObject2);
                            HOperatorSet.Difference(hObject3, hObject4, out hObject2);
                            HOperatorSet.SelectShape(hObject2, out hObject2, "area", "and", (this.Height * 2 * this.Height * 2) - 300, 9999999999999);
                            if (hObject2.CountObj() != 0)
                            {
                                this.GetPThis().AddOBJ(hObject2, ColorResult.red);
                            }
                            string[] lixetCR = new string[Rows.Length];
                            string err = "";
                            int det = hObject3.CountObj();
                            for (int i = 0; i < QRText.Count; i++)
                            {
                                HOperatorSet.GetRegionIndex(hObject3, row.TupleSelect(i).TupleInt(), colu.TupleSelect(i).TupleInt(), out HTuple intdex);
                                try
                                {
                                    string daq = QRText[i];
                                    QRText[i] = "";
                                    if (intdex.Length == 1)
                                    {
                                        if (QRText.Contains(daq))
                                        {
                                            //err += QRText[i]+";";
                                            QRText[i] = "";
                                        }
                                        lixetCR[intdex - 1] = daq;
                                        this.GetPThis().AddMessageIamge(row[i], colu[i], intdex.ToString());
                                        DecodedDataString.Append(QRText[i] + this.SiptS);
                                    }
                                    else
                                    {
                                        if (QRText.Contains(QRText[i]))
                                        {
                                            //err += QRText[i] + ";";
                                        }
                                        if (det > 0)
                                        {
                                            err += i + ";";
                                        }
                                    }
                                    QRText[i] = daq;
                                }
                                catch (Exception ex)
                                {
                                    err += i + ex.Message;
                                }
                            }
                            if (err != "")
                            {
                                this.LogErr("二维码位置错误:" + err);
                            }
                            //QRText.Clear();
                            //QRText.AddRange(lixetCR);
                            HTuple index = new HTuple();
                            for (int i = 0; i < Rows.Length; i++)
                            {
                                index.Append(i + 1);
                            }
                            halcon.AddMessageIamge(Rows, Cols, index);
                        }

                    }
                    else
                    {
                        SrotQR(hOQERoi, textS, out  hOQERoi, out textS);
                    }
                    //重复剔除
                    if (QRCOntEn==1)
                    {
                        HOperatorSet.AreaCenter(hOQERoi, out area, out row, out colu);
                        HTuple indexS = new HTuple();
                        for (int i = 0; i < row.Length; i++)
                        {
                            if (textS.TupleSelect(i) == "")
                            {
                                continue;
                            }
                            string txQ = textS.TupleSelect(i);
                            textS[i] = "";
                            HTuple indx = textS.TupleFind(txQ);
                            if (indx[0] < 0)
                            {
                                textS[i] = txQ;
                                continue;
                            }
                            indexS.Append(indx);
                            for (int i2 = 0; i2 < indx.Length; i2++)
                            {
                                textS[indx.TupleSelect(i2)] = "";
                            }
                            textS[i] = txQ;
                        }
                        if (indexS.Length!=0)
                        {
                            HTuple de = indexS + 1;
                            HOperatorSet.SelectObj(hOQERoi, out HObject hObject4, de);
                            HOperatorSet.DilationCircle(hObject4, out hObject4, 20);
                            HOperatorSet.RemoveObj(hOQERoi, out hOQERoi, de);
                            textS = textS.TupleRemove(indexS);
                            AddBule(hObject4);
                            //halcon.AddMessage("重复数量" +indexS.Length);
                            ErosProjcetDLL.Project.AlarmText.AddTextNewLine("重复数量" + indexS.Length +":"+ indexS.ToString());
                        }
                    }

                    if (DiscernType == 0)
                    {
                        HOperatorSet.AreaCenter(hOQERoi, out area, out row, out colu);
                        //AddGreen(hOQERoi);
                        HTuple index = new HTuple();
                        for (int i = 0; i < row.Length; i++)
                        {
                            index.Append(i + 1);
                        }
                        halcon.AddMessageIamge(row, colu, index);
                    }
                }
                else
                {
                    this.GetPThis().AddOBJ(hObject3, ColorResult.red);
                }
                QRText .AddRange( textS.ToSArr());
                string data = "";
                for (int i = 0; i < QRText.Count; i++)
                {
                    DecodedDataString.Append(QRText[i] + this.SiptS);
                }
                data = DecodedDataString.ToString().Trim(SiptS);
                DecodedDataString.Clear();
                DecodedDataString.Append(data);
            }
            catch (Exception ex)
            {
                this.LogErr(ex.StackTrace.Remove(0,ex.StackTrace.Length-100));
            }

        }


        public HTuple Rows { get; set; } = new HTuple();

        public HTuple Cols { get; set; } = new HTuple();

        public List<bool> IsEt { get; set; } = new List<bool>();
        /// <summary>
        /// 训练次数
        /// </summary>

        public int ThraQR = 1;

        /// <summary>
        /// 执行程序
        /// </summary>
        /// <param name="halcon"></param>
        /// <returns></returns>
        public override bool RunHProgram(HalconRun halcon, OneResultOBj oneResultOBj, int id, string name = null)
        {
            NumberInt = 0;
            DecodedDataString.Clear();
            HTuple tupleStr = new HTuple();
            try
            {
                QRText.Clear();
                          //GenParamName = "stop_after_result_num";
                HObject hObject3;
                hObject3 = this.GetEmset(halcon.Image());
                SetDataProgram();
                //SetParam(ID);
                if (DiscernType == 2)
                {
                    if (this.KeyHObject != null)
                    {
                        threadAnyCodeString(halcon,hObject3);
                    }
                }
                else
                {
                    HObject hObjectCont = new HObject();
                    HObject hObject = new HObject();
                    if (GenParamName != "train")
                    {
                        FindS(hObject3,halcon);
                    }
                    else
                    {
                        TrainQRCode(hObject3, halcon, out HObject hObject1);
                    }
                    halcon.AddMessage("识别数量" + QRText.Count);
                }
                number = QRText.Count;
                //image.Dispose();
                if (NGRoi.IsInitialized())
                {
                    HOperatorSet.CountObj(NGRoi, out HTuple hTuple);
                }
                string dataT = "";
                if (number!= this.IDValue)
                {
                    halcon.AddMessage("识别数量" + number + "/" + this.IDValue);
                }
                if (number > 0)
                {
                    if (this.DecodedDataType == "Find")
                    {
                        dataT = "QRcodeFind" + SiptS + DecodedDataString + SiptS;
                        string[] time = DecodedDataString.ToString().Split(SiptS);
                        for (int i = 0; i < time.Length; i++)
                        {
                            if (time[i] == "")
                            {
                                continue;
                            }
                            List<string> listD = Project.ProcessControl.ProcessUser.GetPidPrag(FindQRCODE, time[i], FindQr);
                            for (int it = 0; it < listD.Count; it++)
                            {
                                Project.ProcessControl.ProcessUser.SetCodeProValue(listD[it].Split('=')[0], "过程", ((HalconRun)this.GetPThis()).Name);
                                dataT += listD[it] + SiptS;
                            }
                        }
                    }
                    else
                    {
                        dataT += "QRcode" + SiptS + DecodedDataString;
                    }
                    if (QRStringName.Contains("."))
                    {
                        Vision.TriggerSetup(this.QRStringName, DecodedDataString.ToString().TrimEnd(SiptS));
                    }
                    else
                    {
                        Vision.TriggerSetup(this.QRStringName, dataT.TrimEnd(SiptS));
                    }

                     if (this.DecodedDataType == "Auto")
                    {
                        if (this.QRText.Count == 1)
                        {
                            if (IsCont)
                            {
                                Project.formula.UserFormulaContrsl.StaticAddQRCode(DecodedDataString.ToString(), (int)halcon.RunID);
                            }
                            else
                            {
                                Project.formula.UserFormulaContrsl.StaticAddQRCode(DecodedDataString.ToString());
                            }
                        }
                        else
                        {
                            DebugCompiler.GetTrayDataUserControl().SetValue(this.QRText, this.TrayIDS);
                            halcon.SendMesage(this.QRText.ToArray());

                            if (IDValue == NumberInt)
                            {
                                return true;
                            }
                            return false;

                        }
                    }
                    return true;
                }
                else
                {
                    if (IsCont)
                    {
                        Project.formula.UserFormulaContrsl.StaticAddQRCode(DecodedDataString.ToString(), (int)halcon.RunID);
                    }
                    if (!QRStringName.Contains("."))
                    {
                        if (FindQRCODE != "")
                        {
                            dataT = "QRcodeFind" + SiptS + "NG";
                        }
                        else
                        {
                            dataT = "QRcode" + SiptS + "NG";
                        }
                        Vision.TriggerSetup(this.QRStringName, dataT);
                    }

                }
            }
            catch (Exception ex)
            {
                LogErr(ex.Message);
                throw new Exception(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// 扫码识别
        /// </summary>
        /// <param name="hObject"></param>
        /// <returns></returns>
        private HTuple FindDatacode2d(HObject hObject, HTuple id, out HObject hObjecXLD)
        {
            HTuple tupleStr = new HTuple();
            HTuple tupleStr2 = new HTuple();
            hObjecXLD = new HObject();
            HObject hObjecXLD2 = new HObject();
            if (Is2D)
            {
                HTuple text = new HTuple();
                One_QR.numer = this.IDValue;
                One_QR.Find1DCode(hObject, out hObjecXLD, out text);
                tupleStr = text;
                if (QRCont > 0)
                {
                    if (tupleStr.S.Length == QRCont)
                    {
                        return tupleStr;
                    }
                    return new HTuple();
                }
                return tupleStr;
            }
            try
            {
                if (GenParamName == "train")
                {
                    HOperatorSet.FindDataCode2d(hObject, out hObjecXLD, id, GenParamName, "all", out resultHandles, out tupleStr);
                    if (GenParamName == "train")
                    {
                        HOperatorSet.FindDataCode2d(hObject, out hObjecXLD2, id, "stop_after_result_num", "500", out resultHandles, out tupleStr2);
                    }
                    if (tupleStr2.Length != 0)
                    {
                        tupleStr = tupleStr2;
                        hObjecXLD = hObjecXLD2;
                    }
                }
                else
                {
                    HOperatorSet.FindDataCode2d(hObject, out hObjecXLD, id, "stop_after_result_num","500", out resultHandles, out tupleStr);
                }
                //HOperatorSet.GetDataCode2dObjects(out HObject hObject1, resultHandles,)
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            //HOperatorSet.FillUp(hObjecXLD, out hObjecXLD);
            return tupleStr;
        }

        HObject XLDT = new HObject();
        /// <summary>
        /// 训练二维码
        /// </summary>
        /// <param name="image"></param>
        /// <param name="halcon"></param>
        /// <param name="trainNumber">训练次数</param>
        /// <param name="hObject"></param>
        /// <returns></returns>
        public HTuple TrainQRCode(HObject image, HalconRun halcon,  out HObject hObject, List<HObject> iamges=null, List<HObject> objs = null)
        {
            HTuple tupleStr = new HTuple();
            hObject = new HObject();
            HObject hObjectCont = new HObject();
            this.IsEt.Clear();
            hObject.GenEmptyObj();
            XLDT.GenEmptyObj();
            HOperatorSet.GetImageSize(image, out HTuple wi, out HTuple heit);
            //HOperatorSet.GenRectangle1(out HObject ImageRelation, 0, 0, heit, wi);
            if (ThraQR == 0)
            {
                ThraQR = 5;
            }
            try
            {
                HObject ImageRelation = this.DrawObj;
                string err = "";
                HTuple row, colu, area;
                for (int i = 0; i < ThraQR; i++)
                {
                    this.Watch.Restart();
                    HOperatorSet.Union1(ImageRelation, out hObjectCont);
                    HOperatorSet.Complement(hObjectCont, out hObjectCont);
                    HOperatorSet.ReduceDomain(image, hObjectCont, out image);
                    HOperatorSet.CropDomain(image, out image);
                    HTuple data = FindDatacode2d(image, ID, out hObject);
                    this.Watch.Stop();
                    halcon.AddMessage("第" + (i + 1) + "\\" + ThraQR + "次数量:" + data.Length + ";时间S:" + this.Watch.ElapsedMilliseconds / 1000);
                    if (data.Length == 0)
                    {
                        break;
                    }
                    if (hObject.GetObjClass() == "xld_cont")
                    {
                        HOperatorSet.GenRegionContourXld(hObject, out hObject, "filled");
                    }
                    HOperatorSet.DilationCircle(hObject, out hObject, 10);
                    ImageRelation = ImageRelation.ConcatObj(hObject);
                    XLDT = XLDT.ConcatObj(hObject);
                    tupleStr.Append(data);
                    if (iamges != null)
                    {
                        iamges.Add (image);
                    }
                    if (objs != null)
                    {
                        objs.Add(hObject);
                    }
                }
                int det = XLDT.CountObj();
                HTuple lixetCR = new HTuple();
                HOperatorSet.TupleGenConst(tupleStr.Length, "", out lixetCR);

                QRText.Clear();
                //QRText.AddRange(tupleStr.ToSArr());
      
                HTuple Row2 = new HTuple();
                HTuple cols2 = new HTuple();
     
                HObject hObject2 = XLDT;
                ///排序
                HOperatorSet.SortRegion(hObject2, out XLDT, "character", "true", "row");
                HOperatorSet.AreaCenter(hObject2, out area, out row, out colu);
                for (int i = 0; i < row.Length; i++)
                {
                    HOperatorSet.GetRegionIndex(XLDT, row.TupleSelect(i).TupleInt(), colu.TupleSelect(i).TupleInt(), out HTuple intdex);
                    try
                    {
                        if (intdex.Length == 1)
                        {
                            lixetCR[intdex - 1] = tupleStr[i];
                        }
                    }
                    catch (Exception ex)
                    {         err += i + ex.Message; 
                    }
                }
                //码文本、区域排序结束
                //码重复剔除
                //HOperatorSet.AreaCenter(hObject2, out area, out row, out colu);
                HObject repetitionObj = new HObject();
                HTuple repetitionIndex = new HTuple();
                repetitionObj.GenEmptyObj();
                for (int i2 = 0; i2 < lixetCR.Length; i2++)
                {
                    if (lixetCR[i2]=="")
                    {
                        continue;
                    }
                    string dseT = lixetCR[i2];
                    lixetCR[i2] = "";
                    HTuple repIndexs = lixetCR.TupleFind(dseT);
                    if (repIndexs!=-1)
                    {
                        repetitionIndex.Append(repIndexs);
                        for (int i = 0; i < repIndexs.Length; i++)
                        {
                            lixetCR[repIndexs.TupleSelect(i)]= "";
                        }
                    }
                    lixetCR[i2] = dseT;
                }
                HOperatorSet.SelectObj(XLDT, out hObject2, repetitionIndex + 1);
                halcon.AddOBJ(hObject2,ColorResult.red);
                HOperatorSet.RemoveObj(XLDT, out XLDT, repetitionIndex+1);
                lixetCR= lixetCR.TupleRemove(repetitionIndex);
                HOperatorSet.AreaCenter(XLDT, out area, out Row2, out cols2);
                HTuple ids = new HTuple();
                Rows = Row2;
                Cols = cols2;
                QRText.Clear();
                QRText.AddRange(lixetCR.ToSArr());
    
                SrotCode(halcon);
                number = XLDT.CountObj();
            }
            catch (Exception ex)
            {
                LogErr(ex);
            }
            return tupleStr;
        }
        /// <summary>
        /// 创建二维码模板
        /// </summary>
        /// <param name="halcon"></param>
        public void CreateDataCode2dModel(HalconRun halcon)
        {
            try
            {

                HOperatorSet.CreateDataCode2dModel(this.SymbolType, new HTuple(), new HTuple(), out ID);
                HOperatorSet.SetDataCode2dParam(this.ID, "default_parameters", CreateGenParamValue);

                this.SetDataProgram();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void SetDataProgram()
        {
            try
            {
                HOperatorSet.SetDataCode2dParam(this.ID, "timeout", timeout);
     
                HOperatorSet.SetDataCode2dParam(this.ID, "module_size_man", module_size_mam);
                HOperatorSet.SetDataCode2dParam(this.ID, "module_size_min", module_size_min);
                HOperatorSet.SetDataCode2dParam(this.ID, "polarity", isPolarity);
         
              
            }
            catch (Exception)
            {
            }
        }
        public void SrotCode(HalconRun halcon)
        {
            try
            {
                this.TrayIDS.Clear();
                HTuple area = new HTuple();
                HTuple ids = new HTuple();
                HTuple Row2 = new HTuple();
                HTuple cols2 = new HTuple();
                HTuple idxs = new HTuple();
                HOperatorSet.GenRectangle1(out HObject hObject2, this.Rows - Height, Cols - this.Height, Rows + this.Height, Cols + this.Height);
                switch (MatrixType)
                {
                    case 0:
                        HOperatorSet.SortRegion(hObject2, out hObject2, "character", "true", "row");
                        HOperatorSet.AreaCenter(hObject2, out area, out Row2, out cols2); break; ;
                    case 1:
                        HOperatorSet.SortRegion(hObject2, out hObject2, "character", "true", "column");
                        HOperatorSet.AreaCenter(hObject2, out area, out Row2, out cols2); break; ;
                    case 2:
                        HOperatorSet.SortRegion(hObject2, out hObject2, "character", "false", "row");
                        while (hObject2.CountObj() != 0)
                        {
                            HOperatorSet.AreaCenter(hObject2, out area, out HTuple Row22, out HTuple cols22);
                            HOperatorSet.SelectShape(hObject2, out HObject hObject, "row", "and", Row22.TupleMin() - this.Height, Row22.TupleMin() + this.Height);
                            HOperatorSet.SortRegion(hObject, out hObject, "character", "false", "row");
                            HOperatorSet.AreaCenter(hObject, out area, out HTuple Row1, out HTuple cols1);
                            Row2.Append(Row1);
                            cols2.Append(cols1);
                            HOperatorSet.SelectShape(hObject2, out hObject2, "row", "and", Row22.TupleMin() + this.Height, Row22.TupleMax() + this.Height);
                        }
                        break;
                    case 3:
                        HOperatorSet.SortRegion(hObject2, out hObject2, "character", "true", "column");
                        while (hObject2.CountObj() != 0)
                        {
                            HOperatorSet.AreaCenter(hObject2, out area, out HTuple Row22, out HTuple cols22);
                            HOperatorSet.SelectShape(hObject2, out HObject hObject, "column", "and", cols22.TupleMax() - this.Height, cols22.TupleMax() + this.Height);
                            HOperatorSet.SortRegion(hObject, out hObject, "character", "true", "column");
                            HOperatorSet.AreaCenter(hObject, out area, out HTuple Row1, out HTuple cols1);
                            Row2.Append(Row1);
                            cols2.Append(cols1);
                            HOperatorSet.SelectShape(hObject2, out hObject2, "column", "and", cols22.TupleMin() - this.Height, cols22.TupleMax() - this.Height);
                        }
                        break;
                    case 4:
                        HOperatorSet.SortRegion(hObject2, out hObject2, "character", "true", "row");
                        while (hObject2.CountObj() != 0)
                        {
                            HOperatorSet.AreaCenter(hObject2, out area, out HTuple Row22, out HTuple cols22);
                            HOperatorSet.SelectShape(hObject2, out HObject hObject, "row", "and", Row22.TupleMax() - this.Height, Row22.TupleMax() + this.Height);
                            HOperatorSet.SortRegion(hObject, out hObject, "character", "true", "row");
                            HOperatorSet.AreaCenter(hObject, out area, out HTuple Row1, out HTuple cols1);
                            Row2.Append(Row1);
                            cols2.Append(cols1);
                            HOperatorSet.SelectShape(hObject2, out hObject2, "row", "and", Row22.TupleMin() - this.Height, Row22.TupleMax() - this.Height);
                        }
                        break;
                    case 5:
                        HOperatorSet.SortRegion(hObject2, out hObject2, "character", "true", "column");
                        while (hObject2.CountObj() != 0)
                        {
                            HOperatorSet.AreaCenter(hObject2, out area, out HTuple Row22, out HTuple cols22);
                            HOperatorSet.SelectShape(hObject2, out HObject hObject, "column", "and", cols22.TupleMin() - this.Height, cols22.TupleMin() + this.Height);
                            HOperatorSet.SortRegion(hObject, out hObject, "character", "false", "column");
                            HOperatorSet.AreaCenter(hObject, out area, out HTuple Row1, out HTuple cols1);
                            Row2.Append(Row1);
                            cols2.Append(cols1);
                            HOperatorSet.SelectShape(hObject2, out hObject2, "column", "and", cols22.TupleMin() + this.Height, cols22.TupleMax() + this.Height);
                        }
                        break;


                    case 6:
                        HOperatorSet.SortRegion(hObject2, out hObject2, "character", "false", "row");
                        HOperatorSet.AreaCenter(hObject2, out area, out Row2, out cols2);

                        break;
                    case 7:
                        HOperatorSet.SortRegion(hObject2, out hObject2, "character", "false", "column");
                        HOperatorSet.AreaCenter(hObject2, out area, out Row2, out cols2);
                        break;
                    default:
                        break;
                }

                for (int i = 0; i < Row2.Length; i++)
                {
                    this.TrayIDS.Add(i + 1);
                }
                while (this.IsEt.Count > Row2.Length)
                {
                    this.IsEt.RemoveAt(this.IsEt.Count);
                }
                while (this.IsEt.Count < Row2.Length)
                {
                    this.IsEt.Add(true);
                }


                halcon.AddMessageIamge(Row2+80, cols2, new HTuple(this.TrayIDS.ToArray()));
                Rows = Row2;
                Cols = cols2;

                HOperatorSet.GenRectangle1(out hObject2, this.Rows - Height, Cols - this.Height, Rows + this.Height, Cols + this.Height);
                halcon.AddOBJ(hObject2);
            }
            catch (Exception ex)
            {


            }
        }
        public void SrotQR(HObject hObject2, HTuple text,out HObject QrObj,out HTuple QtText )
        {
            QrObj = new HObject();
            QrObj.GenEmptyObj();
            QtText = new HTuple();
            HObject hObject1 = new HObject();
            hObject1.GenEmptyObj();
            try
            {
                HTuple area = new HTuple();
                HTuple ids = new HTuple();
                HTuple Row2 = new HTuple();
                HTuple cols2 = new HTuple();
                HOperatorSet.AreaCenter(hObject2, out area, out HTuple row, out HTuple column);
                switch (MatrixType)
                {
                    case 0:
                        HOperatorSet.SortRegion(hObject2, out hObject2, "character", "true", "row");
                        break; 
                    case 1:
                        HOperatorSet.SortRegion(hObject2, out hObject2, "character", "true", "column");
                        break;
                    case 2:
                        HOperatorSet.SortRegion(hObject2, out hObject2, "character", "false", "row");
                        while (hObject2.CountObj() != 0)
                        {
                            HOperatorSet.AreaCenter(hObject2, out area, out HTuple Row22, out HTuple cols22);
                            HOperatorSet.SelectShape(hObject2, out HObject hObject, "row", "and", Row22.TupleMin() - this.Height, Row22.TupleMin() + this.Height);
                            HOperatorSet.SortRegion(hObject, out hObject, "character", "false", "row");
                            HOperatorSet.AreaCenter(hObject, out area, out HTuple Row1, out HTuple cols1);
                            hObject1 = hObject1.ConcatObj(hObject);
                            HOperatorSet.SelectShape(hObject2, out hObject2, "row", "and", Row22.TupleMin() + this.Height, Row22.TupleMax() + this.Height);
                        }
                        hObject2 = hObject1;
                        break;
                    case 3:
                        HOperatorSet.SortRegion(hObject2, out hObject2, "character", "true", "column");
                        while (hObject2.CountObj() != 0)
                        {
                            HOperatorSet.AreaCenter(hObject2, out area, out HTuple Row22, out HTuple cols22);
                            HOperatorSet.SelectShape(hObject2, out HObject hObject, "column", "and", cols22.TupleMax() - this.Height, cols22.TupleMax() + this.Height);
                            HOperatorSet.SortRegion(hObject, out hObject, "character", "true", "column");
                            HOperatorSet.AreaCenter(hObject, out area, out HTuple Row1, out HTuple cols1);
                            hObject1 = hObject1.ConcatObj(hObject);
                            HOperatorSet.SelectShape(hObject2, out hObject2, "column", "and", cols22.TupleMin() - this.Height, cols22.TupleMax() - this.Height);
                        }
                        hObject2 = hObject1;
                        break;
                    case 4:
                        HOperatorSet.SortRegion(hObject2, out hObject2, "character", "true", "row");
                        while (hObject2.CountObj() != 0)
                        {
                            HOperatorSet.AreaCenter(hObject2, out area, out HTuple Row22, out HTuple cols22);
                            HOperatorSet.SelectShape(hObject2, out HObject hObject, "row", "and", Row22.TupleMax() - this.Height, Row22.TupleMax() + this.Height);
                            HOperatorSet.SortRegion(hObject, out hObject, "character", "true", "row");
                            HOperatorSet.AreaCenter(hObject, out area, out HTuple Row1, out HTuple cols1);
                            hObject1 = hObject1.ConcatObj(hObject);
                            HOperatorSet.SelectShape(hObject2, out hObject2, "row", "and", Row22.TupleMin() - this.Height, Row22.TupleMax() - this.Height);
                        }
                        hObject2 = hObject1;
                        break;
                    case 5:
                        HOperatorSet.SortRegion(hObject2, out hObject2, "character", "true", "column");
                        while (hObject2.CountObj() != 0)
                        {
                            HOperatorSet.AreaCenter(hObject2, out area, out HTuple Row22, out HTuple cols22);
                            HOperatorSet.SelectShape(hObject2, out HObject hObject, "column", "and", cols22.TupleMin() - this.Height, cols22.TupleMin() + this.Height);
                            HOperatorSet.SortRegion(hObject, out hObject, "character", "false", "column");
                            HOperatorSet.AreaCenter(hObject, out area, out HTuple Row1, out HTuple cols1);
                            hObject1 = hObject1.ConcatObj(hObject);
                            HOperatorSet.SelectShape(hObject2, out hObject2, "column", "and", cols22.TupleMin() + this.Height, cols22.TupleMax() + this.Height);
                        }
                       
                        hObject2 = hObject1;
                        int dwe = hObject2.CountObj();
                        break;
                    case 6:
                        HOperatorSet.SortRegion(hObject2, out hObject2, "character", "false", "row");
                        break;
                    case 7:
                        HOperatorSet.SortRegion(hObject2, out hObject2, "character", "false", "column");
                        break;
                    default:
                        break;
                }
                
                //HOperatorSet.AreaCenter(hObject2, out area, out Row2, out cols2);
                HOperatorSet.TupleGenConst(row.Length, "",out QtText);
                for (int i = 0; i < row.Length; i++)
                {
                    HOperatorSet.GetRegionIndex(hObject2, row.TupleSelect(i).TupleInt(), column.TupleSelect(i).TupleInt(), out HTuple index);
                    if (index.Length==1)
                    {
                        QtText[index - 1] = text[i];
                    }
                    else
                    {
                        ErosProjcetDLL.Project.AlarmText.AddTextNewLine(i+ "码排序错误"+index.ToString());
                    }
                }
                QrObj = hObject2;
                //排序结束
     
            }
            catch (Exception ex)
            {
            }

        }
    }


}
