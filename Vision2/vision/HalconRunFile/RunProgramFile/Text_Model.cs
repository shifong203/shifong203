using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.UI.PropertyGrid;
using Vision2.vision.HalconRunFile.Controls;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
    public class Text_Model : RunProgram
    {
        private Text_Model Instance;


        HTuple ID { get; set; }
        [DescriptionAttribute("创建模式"),
        Category("识别参数"), DisplayName("创建模式"), TypeConverter(typeof(ErosConverter)),
        ErosConverter.ThisDropDown("", "auto", "manual")]
        public string CreateMode { get; set; } = "auto";
        [DescriptionAttribute("OCR类型选择。默认：Universal_Rej.occ'。可选'Document_Rej.omc'," +
            " 'Document_0-9_Rej.omc', 'Document_0-9A-Z_Rej.omc', 'Document_A-Z+_Rej.omc', 'DotPrint_Rej.omc'," +
            " 'DotPrint_0-9_Rej.omc', 'DotPrint_0-9+_Rej.omc', 'DotPrint_0-9A-Z_Rej.omc', 'DotPrint_A-Z+_Rej.omc', " +
            "'HandWritten_0-9_Rej.omc', 'Industrial_Rej.omc', 'Industrial_0-9_Rej.omc', 'Industrial_0-9+_Rej.omc', " +
            "'Industrial_0-9A-Z_Rej.omc', 'Industrial_A-Z+_Rej.omc', 'OCRA_Rej.omc', 'OCRA_0-9_Rej.omc', " +
            "'OCRA_0-9A-Z_Rej.omc', 'OCRA_A-Z+_Rej.omc', 'OCRB_Rej.omc', 'OCRB_0-9_Rej.omc', 'OCRB_0-9A-Z_Rej.omc'," +
            " 'OCRB_A-Z+_Rej.omc', 'OCRB_passport_Rej.omc', 'Pharma_Rej.omc', 'Pharma_0-9_Rej.omc', 'Pharma_0-9+_Rej.omc'," +
            " 'Pharma_0-9A-Z_Rej.omc', 'SEMI_Rej.omc', 'Universal_Rej.occ', 'Universal_0-9_Rej.occ', " +
            "'Universal_0-9+_Rej.occ', 'Universal_0-9A-Z_Rej.occ', 'Universal_0-9A-Z+_Rej.occ', 'Universal_A-Z+_Rej.occ"),
        Category("识别参数"), DisplayName("OCR类型"), TypeConverter(typeof(ErosConverter)),
        ErosConverter.ThisDropDown("", false, "", "Universal_0-9_NoRej", "Universal_Rej.occ", "Universal_0-9A-Z+_Rej.occ", "Universal_A-Z+_Rej.occ")]
        public string FontName { get; set; } = "Universal_0-9A-Z+_Rej.occ";


        HTuple ResultIDText = new HTuple();

        [Description("返回区域的特征根据创建模式aoto/manual，all_lines：返回所有分段文本行的所有字符。" +
           "['line', LineIndex]:返回LineIndex指定的文本行中的所有字符。例如，['line'， 0] 返回第一行。" +
             "[element,Index]返回位置索引处的字符。例如，['element'， 0]返回第一个字符。+" +
             "manual_all_lines:返回所有分段文本行的所有字符。文本行从上到下排序，从左到右排序。文本行中的字符是从左到右排序的。" +
             "['manual_line',Index]:返回“Index”指定的文本行中的所有字符(例如['manual_line'，0]以返回第一行)。文本行中的字符是从左到右排序的。"),
             Category("识别参数"), DisplayName("返回区域特征"), TypeConverter(typeof(ErosConverter)),
         ErosConverter.ThisDropDown("", "all_lines", "element", "line", "manual_all_lines",
             "manual_compensated_image", "manual_line")]
        public string ResultObjName { get; set; } = "all_lines";
        [Description(" 返回的结果的名称。 'class'OCR信息, 'class_element', 'class_line', 'confidence', 'confidence_element'" +
            "'confidence_line', 'manual_num_lines', 'manual_thresholds', 'num_classes', 'num_lines'" +
            "'polarity', 'polarity_element', 'polarity_line'"),
       Category("识别参数"), DisplayName("返回字符类"), TypeConverter(typeof(ErosConverter)),
       ErosConverter.ThisDropDown("", "class", "class_element", "class_line", "confidence",
       "confidence_element", "confidence_line", "manual_num_lines", "manual_thresholds", "num_classes",
            "num_lines", "polarity", "polarity_element", "polarity_line")]
        public string ResultTextName { get; set; } = "class";




        [Description("最小字符笔画宽度,auto或数字"), Category("识别参数"), DisplayName("笔画宽度")
            , TypeConverter(typeof(ErosConverter)),
       ErosConverter.ThisDropDown("", false, "auto")]
        public string Min_stroke_width { get; set; } = "";
        [Description("字符最小间隔，auto或数字"), Category("识别参数"), DisplayName("字符最小间隔"), TypeConverter(typeof(ErosConverter)),
       ErosConverter.ThisDropDown("", false, "auto")]
        public string Dot_print_min_char_gap { get; set; } = "";
        [Description("笔画最大间隔，auto或数字"), Category("识别参数"), DisplayName("笔画最大间隔"), TypeConverter(typeof(ErosConverter)),
       ErosConverter.ThisDropDown("", false, "auto")]
        public string Dot_print_max_dot_gap { get; set; } = "";
        [Description("识别到的字符"), Category("结果"), DisplayName("识别的OCR")]
        /// <summary>
        /// 识别的字符
        /// </summary>
        public HTuple ResultText { get; set; } = new HTuple();



        [Description("完全匹配的字符"), Category("结果"), DisplayName("目标字符")]
        public string ModeText { get; set; } = "";
        [Description("匹配QR"), Category("结果"), DisplayName("比较QR")]
        public bool QRMode { get; set; }


        [Description("使用训练库识别字符"), Category("识别"), DisplayName("使用训练库")]
        public bool OCRMLP { get; set; } 
        public HTuple Row { get; set; } = new HTuple();

        public HTuple Column { get; set; } = new HTuple();
        public HTuple Phi { get; set; } = new HTuple();
        public HTuple Length1 { get; set; } = new HTuple();

        public HTuple Length2 { get; set; } = new HTuple();


        public List<HObject> ListhObjects { get; set; } = new List<HObject>();

        int Intdex;
        /// <summary>
        /// 初始化读取实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Path"></param>
        /// <returns></returns>
        public override RunProgram UpSatrt<T>(string Path)
        {
            if (File.Exists(Path + this.SuffixName))
            {
                HalconRun.ReadPathJsonToCalss<Text_Model>(Path + this.SuffixName, out this.Instance);
            }
            else if (File.Exists(Path + ".txt"))
            {
                HalconRun.ReadPathJsonToCalss<Text_Model>(Path + ".txt", out this.Instance);
            }
            else
            {
                return null;
            }
            try
            {
                //HOperatorSet.CreateDeepOcr(new HTuple(), new HTuple(), out HTuple deepOcrID);

                HOperatorSet.CreateTextModelReader(Instance.CreateMode, Instance.FontName, out HTuple id);
                this.Instance.ID = id;
                HOperatorSet.ReadOcrClassMlp("Industrial_0-9A-Z_NoRej.omc", out OcrHandle);
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("未安装OCR识别或创建参数错误");
            }

            return Instance;
        }
        HTuple ocrMLPID;
        /// <summary>
        /// 创建识别区
        /// </summary>
        /// <param name="halcon"></param>
        public void CreateTextModt(HalconRun halcon)
        {
            try
            {
                HOperatorSet.ClearTextModel(ID);
            }
            catch (Exception)
            {
            }

            try
            {
                //HOperatorSet.CreateDeepOcr(new HTuple(), new HTuple(), out HTuple deepOcrID);
                //HOperatorSet.QueryAvailableDlDevices(new HTuple("runtime", "runtime"), new HTuple("gpu", "cpu"),out HTuple DLDeviceHandles);
                //if (DLDeviceHandles.Length==0)
                //{
                //    MessageBox.Show("没有找到支持的设备来继续这个示例。");
                //}
                ////将recognition_image_width设置为更大的值，以使示例在没有内存问题的情况下工作。
                //try
                //{
                //    HOperatorSet.SetDeepOcrParam(deepOcrID, "recognition_image_width", 250);
                //}
                //catch (Exception) { }
                //for (int i = 0; i < DLDeviceHandles.Length; i++)
                //{
                //    try
                //    {
                //        HOperatorSet.SetDeepOcrParam(deepOcrID, "device", DLDeviceHandles[i]);
                //        break;
                //    }
                //    catch (Exception) {  }
                //}
                //try
                //{
                //    HOperatorSet.SetDeepOcrParam(deepOcrID, "recognition_image_width", 100);
                //}
                //catch (Exception) { }
                //HOperatorSet.GetDeepOcrParam(deepOcrID, "recognition_alphabet", out HTuple hTupleRes);
                //HOperatorSet.GetDeepOcrParam(deepOcrID, "detection_image_width", out HTuple hTupleWidth);
                //HOperatorSet.GetDeepOcrParam(deepOcrID, "detection_image_height", out HTuple hTupleheight);

                HOperatorSet.CreateTextModelReader(CreateMode, FontName, out HTuple deepOcrID);
                ID = deepOcrID;
                HOperatorSet.ReadOcrClassMlp("Industrial_0-9A-Z_NoRej.omc", out  OcrHandle);
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.Message);
            }

            SetParam();
        }
        HTuple OcrHandle;
        /// <summary>
        /// 
        /// </summary>
        public void SetParam()
        {
            try
            {
                if (double.TryParse(Min_stroke_width, out double value))
                {
                    HOperatorSet.SetTextModelParam(ID, "min_stroke_width", value);
                }
                else
                {
                    HOperatorSet.SetTextModelParam(ID, "min_stroke_width", "auto");
                }
                if (double.TryParse(Dot_print_min_char_gap, out value))
                {

                    HOperatorSet.SetTextModelParam(ID, "dot_print_min_char_gap", value);
                }
                else
                {
                    HOperatorSet.SetTextModelParam(ID, "dot_print_min_char_gap", "auto");
                }
                if (double.TryParse(Dot_print_max_dot_gap, out value))
                {
                    HOperatorSet.SetTextModelParam(ID, "dot_print_max_dot_gap", value);
                }
                else
                {
                    HOperatorSet.SetTextModelParam(ID, "dot_print_max_dot_gap", "auto");
                }
            }
            catch (Exception)
            {
            }

        }
        /// <summary>
        /// 绘制仿射识别区
        /// </summary>
        /// <param name="halcon"></param>
        public HObject DrawHomObj(HalconRun halcon, int intdex = 0)
        {
            Intdex = intdex;

            halcon.DrawIng(Drw, out HObject hObject);

            return hObject;
        }

        HObject Drw(HalconRun halcon)
        {
            HTuple row, column, phi, length1, length2;
            Vision.Disp_message(halcon.hWindowHalcon(), "画字符区域，注意角度与文字方向相同，点击鼠标右键确认", 20, 20, true, "red", "false");
            if (Row.Length <= Intdex)
            {
                HOperatorSet.DrawRectangle2(halcon.hWindowHalcon(), out row, out column, out phi,
                    out length1, out length2);
            }
            else
            {
                HOperatorSet.DrawRectangle2Mod(halcon.hWindowHalcon(), Row[Intdex], Column[Intdex], Phi[Intdex], Length1[Intdex], Length2[Intdex], out row, out column, out phi,
                 out length1, out length2);
            }
            //提示信息
            //画模板区域
            HOperatorSet.GenRectangle2(out HObject homObj, row, column, phi, length1, length2);
            if (Row == null)
            {
                Row = new HTuple();
                Column = new HTuple();
            }
            if (Row.Length <= Intdex)
            {
                Row.Append(new HTuple());
                Column.Append(new HTuple());
                Phi.Append(new HTuple());
                Length1.Append(new HTuple());
                Length2.Append(new HTuple());
            }
            Row[Intdex] = row;
            Column[Intdex] = column;
            Phi[Intdex] = phi;
            Length1[Intdex] = length1;
            Length2[Intdex] = length2;
            HOperatorSet.ReduceDomain(halcon.Image(), homObj, out HObject ImageReduced);
            halcon.AddObj(homObj);
            return homObj;
        }

        public void GetHomObj(HalconRun halcon)
        {
            halcon.HobjClear();
            for (int i = 0; i < this.ListhObjects.Count; i++)
            {
                halcon.AddObj(this.ListhObjects[i]);
            }
            halcon.ShowObj();
        }
        public override bool RunHProgram( OneResultOBj oneResultOBj, out List<OneRObj> oneRObjs, AoiObj aoiObj)
        {
            oneRObjs = new List<OneRObj>();
            SetParam();
            ResultText = new HTuple();
            HObject image = new HObject();
            try
            {
                image = GetEmset(oneResultOBj.Image);
                List<HTuple> listh = this.GetHomMatList(oneResultOBj);
                int errNumer = 0;
                string data = "";
                for (int i2 = 0; i2 < ListhObjects.Count; i2++)
                {
                    if (listh != null)
                    {
                        for (int i = 0; i < listh.Count; i++)
                        {
                            HOperatorSet.AffineTransRegion(ListhObjects[i2], out HObject hObject2, listh[i], "nearest_neighbor");
                            HTuple home2d = listh[i];
                            HOperatorSet.HomMat2dRotate(home2d, this.Phi[i2], this.Row[i2], this.Column[i2], out home2d);
                            HOperatorSet.AffineTransImage(image, out HObject hObject3, home2d, "constant", "false");
                            HOperatorSet.AffineTransRegion(hObject2, out HObject hObject1, home2d, "nearest_neighbor");
                            HOperatorSet.ReduceDomain(hObject3, hObject1, out HObject hObject);
                            HObject hObject4 = FindText(hObject, out string text);
                            HOperatorSet.HomMat2dInvert(home2d, out home2d);
                            HOperatorSet.AffineTransRegion(hObject4, out hObject4, home2d, "nearest_neighbor");
                            if (QRMode)
                            {
                                if (text == Project.ProcessControl.ProcessUser.QRCode)
                                {
                                    oneResultOBj.AddObj(hObject2);
                                    AddGreen(hObject4);
                                    errNumer = 0;
                                }
                                else
                                {
                                    oneResultOBj. AddNGOBJ( this.Name,"字符与码不等", hObject2, hObject4);
                                    errNumer++;
                                }
                            }
                            else
                            {
                                if (ModeText != "")
                                {
                                    if (ModeText != text)
                                    {
                                        errNumer++;
                                        oneResultOBj.AddNGOBJ(Name, "字符不同", hObject2, hObject4);
                                    }
                                    else
                                    {
                                        oneResultOBj.AddObj(hObject2);
                                        AddGreen(hObject4);
                                    }
                                }
                            }
                            hObject.Dispose();
                        }
                        for (int i = 0; i < ResultText.Length; i++)
                        {
                            data += ResultText.TupleSelect(i);
                        }
                    }
                }
                HOperatorSet.AreaCenter(NGRoi, out HTuple area, out HTuple row, out HTuple column);
                string textStr = "";
                for (int i = 0; i < row.Length; i++)
                {
                    textStr = textStr + ResultText[i];
                }
                //halcon.SendMesage("OCR", this.Name, textStr);
                if (errNumer == 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
            }
            return false;
        }
        public HObject FindText(HObject hObject, out string text, HTuple hwindosID = null)
        {
            HObject hObjects = null;
            text = "";
            try
            {
                if (hwindosID != null)
                {
                    HOperatorSet.DispObj(hObject, hwindosID);
                }
                HOperatorSet.FindText(hObject, ID, out HTuple hTuple);
                ResultIDText = hTuple;
                HOperatorSet.GetTextObject(out hObjects, ResultIDText, ResultObjName);
                HOperatorSet.GetTextResult(ResultIDText, ResultTextName, out HTuple hTuple1);
                if (OCRMLP)
                {
                    HOperatorSet.DoOcrWordMlp(hObjects, hObject, OcrHandle, "^[A-Z][A-Z][A-Z][_][A-Z][0-9][_][A-Z][A-Z][A-Z][0-9][0-9][_][0-9]$", 3, 2,
                   out  hTuple1, out HTuple cof, out HTuple word, out HTuple score);
                }
                if (ResultText == null)
                {
                    ResultText = new HTuple();
                }
                ResultText.Append(hTuple1);
                for (int i = 0; i < hTuple1.Length; i++)
                {
                    text += hTuple1.TupleSelect(i);
                }
                HOperatorSet.ClearTextResult(hTuple);
            }
            catch (Exception ex)
            {
            }
            return hObjects;
        }

        public override void Close()
        {
            base.Close();
            HOperatorSet.ClearTextModel(ID);
        }

        public override Control GetControl(HalconRun halcon)
        {
            HalconRun halconRun = this.GetPThis() as HalconRun;
            return new OCRTextModeUserContro(halconRun, this);
        }
        public class Text_Mo
        {     
            HTuple ID { get; set; }
            [DescriptionAttribute("创建模式"),
            Category("识别参数"), DisplayName("创建模式"), TypeConverter(typeof(ErosConverter)),
            ErosConverter.ThisDropDown("", "auto", "manual")]
            public string CreateMode { get; set; } = "auto";
            [DescriptionAttribute("OCR类型选择。默认：Universal_Rej.occ'。可选'Document_Rej.omc'," +
                " 'Document_0-9_Rej.omc', 'Document_0-9A-Z_Rej.omc', 'Document_A-Z+_Rej.omc', 'DotPrint_Rej.omc'," +
                " 'DotPrint_0-9_Rej.omc', 'DotPrint_0-9+_Rej.omc', 'DotPrint_0-9A-Z_Rej.omc', 'DotPrint_A-Z+_Rej.omc', " +
                "'HandWritten_0-9_Rej.omc', 'Industrial_Rej.omc', 'Industrial_0-9_Rej.omc', 'Industrial_0-9+_Rej.omc', " +
                "'Industrial_0-9A-Z_Rej.omc', 'Industrial_A-Z+_Rej.omc', 'OCRA_Rej.omc', 'OCRA_0-9_Rej.omc', " +
                "'OCRA_0-9A-Z_Rej.omc', 'OCRA_A-Z+_Rej.omc', 'OCRB_Rej.omc', 'OCRB_0-9_Rej.omc', 'OCRB_0-9A-Z_Rej.omc'," +
                " 'OCRB_A-Z+_Rej.omc', 'OCRB_passport_Rej.omc', 'Pharma_Rej.omc', 'Pharma_0-9_Rej.omc', 'Pharma_0-9+_Rej.omc'," +
                " 'Pharma_0-9A-Z_Rej.omc', 'SEMI_Rej.omc', 'Universal_Rej.occ', 'Universal_0-9_Rej.occ', " +
                "'Universal_0-9+_Rej.occ', 'Universal_0-9A-Z_Rej.occ', 'Universal_0-9A-Z+_Rej.occ', 'Universal_A-Z+_Rej.occ"),
            Category("识别参数"), DisplayName("OCR类型"), TypeConverter(typeof(ErosConverter)),
            ErosConverter.ThisDropDown("", false, "", "Universal_0-9_NoRej", "Universal_Rej.occ", "Universal_0-9A-Z+_Rej.occ", "Universal_A-Z+_Rej.occ")]
            public string FontName { get; set; } = "Universal_0-9A-Z+_Rej.occ";


            HTuple ResultIDText = new HTuple();

            [Description("返回区域的特征根据创建模式aoto/manual，all_lines：返回所有分段文本行的所有字符。" +
               "['line', LineIndex]:返回LineIndex指定的文本行中的所有字符。例如，['line'， 0] 返回第一行。" +
                 "[element,Index]返回位置索引处的字符。例如，['element'， 0]返回第一个字符。+" +
                 "manual_all_lines:返回所有分段文本行的所有字符。文本行从上到下排序，从左到右排序。文本行中的字符是从左到右排序的。" +
                 "['manual_line',Index]:返回“Index”指定的文本行中的所有字符(例如['manual_line'，0]以返回第一行)。文本行中的字符是从左到右排序的。"),
                 Category("识别参数"), DisplayName("返回区域特征"), TypeConverter(typeof(ErosConverter)),
             ErosConverter.ThisDropDown("", "all_lines", "element", "line", "manual_all_lines",
                 "manual_compensated_image", "manual_line")]
            public string ResultObjName { get; set; } = "all_lines";
            [Description(" 返回的结果的名称。 'class'OCR信息, 'class_element', 'class_line', 'confidence', 'confidence_element'" +
                "'confidence_line', 'manual_num_lines', 'manual_thresholds', 'num_classes', 'num_lines'" +
                "'polarity', 'polarity_element', 'polarity_line'"),
           Category("识别参数"), DisplayName("返回字符类"), TypeConverter(typeof(ErosConverter)),
           ErosConverter.ThisDropDown("", "class", "class_element", "class_line", "confidence",
           "confidence_element", "confidence_line", "manual_num_lines", "manual_thresholds", "num_classes",
                "num_lines", "polarity", "polarity_element", "polarity_line")]
            public string ResultTextName { get; set; } = "class";

            [Description("最小字符笔画宽度,auto或数字"), Category("识别参数"), DisplayName("笔画宽度")
                , TypeConverter(typeof(ErosConverter)),
           ErosConverter.ThisDropDown("", false, "auto")]
            public string Min_stroke_width { get; set; } = "";
            [Description("字符最小间隔，auto或数字"), Category("识别参数"), DisplayName("字符最小间隔"), TypeConverter(typeof(ErosConverter)),
           ErosConverter.ThisDropDown("", false, "auto")]
            public string Dot_print_min_char_gap { get; set; } = "";
            [Description("笔画最大间隔，auto或数字"), Category("识别参数"), DisplayName("笔画最大间隔"), TypeConverter(typeof(ErosConverter)),
           ErosConverter.ThisDropDown("", false, "auto")]
            public string Dot_print_max_dot_gap { get; set; } = "";
            [Description("识别到的字符"), Category("结果"), DisplayName("识别的OCR")]
            /// <summary>
            /// 识别的字符
            /// </summary>
            public HTuple ResultText { get; set; } = new HTuple();
            [Description("完全匹配的字符"), Category("结果"), DisplayName("目标字符")]
            public string ModeText { get; set; } = "";
            public HTuple Row { get; set; } = new HTuple();

            public HTuple Column { get; set; } = new HTuple();
            public HTuple Phi { get; set; } = new HTuple();
            public HTuple Length1 { get; set; } = new HTuple();

            public HTuple Length2 { get; set; } = new HTuple();
            int Intdex;
            public List<HObject> ListhObjects { get; set; } = new List<HObject>();
            public HObject FindText(HObject hObject, out string text, HTuple hwindosID = null)
            {
                HObject hObjects = null;
                text = "";
                try
                {
                    //hObjects = new HObject();
                    if (hwindosID != null)
                    {
                        HOperatorSet.DispObj(hObject, hwindosID);
                    }
                    HOperatorSet.FindText(hObject, ID, out HTuple hTuple);
                    ResultIDText = hTuple;
                    HOperatorSet.GetTextObject(out hObjects, ResultIDText, ResultObjName);

                    HOperatorSet.GetTextResult(ResultIDText, ResultTextName, out HTuple hTuple1);
                    if (ResultText == null)
                    {
                        ResultText = new HTuple();
                    }
                    ResultText.Append(hTuple1);
                    for (int i = 0; i < hTuple1.Length; i++)
                    {
                        text += hTuple1.TupleSelect(i);
                    }

                    HOperatorSet.ClearTextResult(hTuple);

                }
                catch (Exception ex)
                {
                }
                return hObjects;
            }
            public  void UpSatrt(string Path)
            {
              
                try
                {
                    HOperatorSet.CreateTextModelReader(CreateMode,FontName, out HTuple id);
                    ID = id;
                }
                catch (Exception ex)
                {
                    throw new ArgumentNullException("未安装OCR识别或创建参数错误");
                }
            }
            HTuple ocrMLPID;
            /// <summary>
            /// 创建识别区
            /// </summary>
            /// <param name="halcon"></param>
            public void CreateTextModt(HalconRun halcon)
            {
                try
                {
                    HOperatorSet.ClearTextModel(ID);
                }
                catch (Exception)
                {
                }
                try
                {
                    HOperatorSet.ReadOcrClassMlp("Industrial_0-9A-Z_NoRej.omc", out HTuple hTuple);

                    ocrMLPID = hTuple;
                    HOperatorSet.CreateTextModelReader(CreateMode, FontName, out HTuple id);
                    ID = id;
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.Message);
                }
                SetParam();
            }
            HObject Drw(HalconRun halcon)
            {
                HTuple row, column, phi, length1, length2;
                Vision.Disp_message(halcon.hWindowHalcon(), "画字符区域，注意角度与文字方向相同，点击鼠标右键确认", 20, 20, true, "red", "false");
                if (Row.Length <= Intdex)
                {
                    HOperatorSet.DrawRectangle2(halcon.hWindowHalcon(), out row, out column, out phi,
                        out length1, out length2);
                }
                else
                {
                    HOperatorSet.DrawRectangle2Mod(halcon.hWindowHalcon(), Row[Intdex], Column[Intdex], Phi[Intdex], Length1[Intdex], Length2[Intdex], out row, out column, out phi,
                     out length1, out length2);
                }
                //提示信息
                //画模板区域
                //Row = row;
                //Column = column;
                //Phi = phi;
                //Length1 = length1;
                //Length2 = length2;
                HOperatorSet.GenRectangle2(out HObject homObj, row, column, phi, length1, length2);
                if (Row == null)
                {
                    Row = new HTuple();
                    Column = new HTuple();
                }
                if (Row.Length <= Intdex)
                {
                    Row.Append(new HTuple());
                    Column.Append(new HTuple());
                    Phi.Append(new HTuple());
                    Length1.Append(new HTuple());
                    Length2.Append(new HTuple());
                }
                Row[Intdex] = row;
                Column[Intdex] = column;
                Phi[Intdex] = phi;
                Length1[Intdex] = length1;
                Length2[Intdex] = length2;

                HOperatorSet.ReduceDomain(halcon.Image(), homObj, out HObject ImageReduced);
                halcon.AddObj(homObj);
                return homObj;
            }
            /// <summary>
            /// 
            /// </summary>
            public void SetParam()
            {
                try
                {
                    if (double.TryParse(Min_stroke_width, out double value))
                    {
                        HOperatorSet.SetTextModelParam(ID, "min_stroke_width", value);
                    }
                    else
                    {
                        HOperatorSet.SetTextModelParam(ID, "min_stroke_width", "auto");
                    }
                    if (double.TryParse(Dot_print_min_char_gap, out value))
                    {

                        HOperatorSet.SetTextModelParam(ID, "dot_print_min_char_gap", value);
                    }
                    else
                    {
                        HOperatorSet.SetTextModelParam(ID, "dot_print_min_char_gap", "auto");
                    }
                    if (double.TryParse(Dot_print_max_dot_gap, out value))
                    {
                        HOperatorSet.SetTextModelParam(ID, "dot_print_max_dot_gap", value);
                    }
                    else
                    {
                        HOperatorSet.SetTextModelParam(ID, "dot_print_max_dot_gap", "auto");
                    }
                }
                catch (Exception ex)
                {
                }

            }
        }

    }
}
