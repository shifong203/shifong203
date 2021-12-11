using HalconDotNet;
using System;
using System.Collections.Generic;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision
{
    /// <summary>
    /// 单次拍照
    /// </summary>
    public class OneResultOBj
    {
        public OneResultOBj()
        {
            Image = new HObject();
            Image.GenEmptyObj();
            HObjectRed = new HObject();
            HObjectRed.GenEmptyObj();
            HObjectGreen = new HObject();
            HObjectGreen.GenEmptyObj();
            HObjectYellow = new HObject();
            HObjectYellow.GenEmptyObj();
            HObjectBlue = new HObject();
            HObjectBlue.GenEmptyObj();
            hObject = new HObject();
            hObject.GenEmptyObj();
            Colrrs = new HObject();
            Colrrs.GenEmptyObj();
        }

        /// <summary>
        /// 是否保存图像
        /// </summary>
        public bool IsSave = true;
        public DateTime CamNewTime;

        public Coordinate CoordinatePXY = new Coordinate();

        /// <summary>
        /// 刷新显示
        /// </summary>
        private HObject hObject = new HObject();

        public HTuple RowsData { get; set; } = new HTuple();
        public HTuple ColumnsData { get; set; } = new HTuple();

        /// <summary>
        /// 执行ID
        /// </summary>
        public int RunID { get; set; }

        /// <summary>
        /// 库ID
        /// </summary>
        public int LiyID { get; set; }
        /// <summary>
        /// 调试
        /// </summary>
        public int DebugID { get; set; }
        /// <summary>
        /// 程序名
        /// </summary>
        public string RunName { get; set; }

        public string NGMestage { get; set; } = "";

        public HTuple Massage { get; set; } = new HTuple();
        public bool ISMassageBack { get; set; }
        public ColorResult ColorResu = ColorResult.green;

        public HTuple OKMassage { get; set; } = new HTuple();
        public HTuple NGMassage { get; set; } = new HTuple();

        public bool IsMoveBool;

        public bool IsXLDOrImage;

        ///// <summary>
        ///// 机器结果
        ///// </summary>
        private bool ResultBool
        {
            get
            {
                foreach (var item in oneContOBJs.DicOnes)
                {
                    if (!item.Value.aOK)
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public bool OK
        {
            get
            {
                foreach (var item in oneContOBJs.DicOnes)
                {
                    if (!item.Value.aOK)
                    {
                        return false;
                    }
                }
                return true;
            }
            set
            {
                if (value)
                {
                    foreach (var item in oneContOBJs.DicOnes)
                    {
                        item.Value.aOK = true;
                    }
                }
                autoOk = value;
            }
        }

        private bool autoOk;

        /// <summary>
        /// 元件集合
        /// </summary>
        private OneCompOBJs oneContOBJs = new OneCompOBJs();

        /// <summary>
        /// 关联元件集合
        /// </summary>
        /// <param name="oneContOB"></param>
        /// <returns></returns>
        public OneCompOBJs GetNgOBJS(OneCompOBJs oneContOB = null)
        {
            if (oneContOB != null)
            {
                oneContOBJs = oneContOB;
            }
            return oneContOBJs;
        }

        public void ReadImage(string path)
        {
            try
            {
                HOperatorSet.ReadImage(out HObject hObject, path);
                Image = hObject;
                CamNewTime = DateTime.Now;
            }
            catch (Exception)
            {
            }
        }

        public void AddOKOBj(OneCompOBJs.OneComponent oneComponent)
        {
            oneContOBJs.Add(oneComponent);
        }

        /// <summary>
        /// 添加NG结果区域数据
        /// </summary>
        /// <param name="rObj"></param>
        public void AddNGOBJ(OneRObj rObj)
        {
            if (!rObj.RestStrings.Contains(rObj.NGText))
            {
                rObj.RestStrings.Add(rObj.NGText);
            }
            oneContOBJs.AddCont(rObj);
        }

        /// <summary>
        ///
        /// </summary>
        private List<HObject> imageS = new List<HObject>();

        /// <summary>
        /// 添加NG结果区域数据
        /// </summary>
        /// <param name="component">元件名称</param>
        /// <param name="nGText">NG信息</param>
        /// <param name="roi">搜索区域</param>
        /// <param name="err">NG区域</param>
        /// <param name="ngText">NG信息集合</param>
        /// <param name="runPa">库名称</param>
        public void AddNGOBJ(string component, string nGText, HObject roi, HObject err,
            HTuple ngText, string runPa = "")
        {
        

            OneRObj rObj = new OneRObj()
            {
                NGText = nGText,
                ComponentID = component,
                ROI = roi,
                NGROI = err,
                RunName = runPa,
            };
            if (roi.IsInitialized())
            {
                HOperatorSet.AreaCenter(roi, out HTuple area, out HTuple row, out HTuple colu);
                if (row.Length > 0)
                {
                    rObj.Row = row.TupleInt();
                    rObj.Col = colu.TupleInt();
                }
            }
            if (ngText != null)
            {
                for (int i = 0; i < ngText.Length; i++)
                {
                    if (!rObj.RestStrings.Contains(ngText[i]))
                    {
                        rObj.RestStrings.Add(ngText[i]);
                    }
                }
            }
            AddNGOBJ(rObj);
        }

        /// <summary>
        /// 添加信息到图像显示
        /// </summary>
        /// <param name="rows">坐标</param>
        /// <param name="columns">坐标</param>
        /// <param name="Massage">信息</param>
        /// <param name="color">颜色</param>
        /// <param name="meblut"></param>
        public void AddImageMassage(HTuple rows, HTuple columns, HTuple Massage, ColorResult color, string meblut = "false")
        {
            if (columns.Length > Massage.Length)
            {
                Massage = HTuple.TupleGenConst(columns.Length, Massage);
            }
            meblut = meblut.ToLower();
            switch (color)
            {
                case ColorResult.red:

                    MaRed.color = "red";
                    MaRed.Rows.Add(rows);
                    MaRed.Columns.Add(columns);
                    MaRed.Massage.Add(Massage);
                    MaRed.MassageBlute = meblut;

                    break;

                case ColorResult.yellow:

                    if (rows.Length == columns.Length && columns.Length == Massage.Length)
                    {
                        MaYellow.color = "yellow";
                        MaYellow.Rows.Add(rows);
                        MaYellow.Columns.Add(columns);
                        MaYellow.Massage.Add(Massage);
                        MaYellow.MassageBlute = meblut;
                    }
                    break;

                case ColorResult.green:
                    if (rows.Length == columns.Length && columns.Length == Massage.Length)
                    {
                        MaGreen.color = "green";
                        MaGreen.Rows.Add(rows);
                        MaGreen.Columns.Add(columns);
                        MaGreen.Massage.Add(Massage);
                        MaGreen.MassageBlute = meblut;
                    }
                    break;

                case ColorResult.blue:
                    if (rows.Length == columns.Length && columns.Length == Massage.Length)
                    {
                        MaBlue.color = "blue";
                        MaBlue.Rows.Add(rows);
                        MaBlue.Columns.Add(columns);
                        MaBlue.Massage.Add(Massage);
                        MaBlue.MassageBlute = meblut;
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        ///  添加信息到图像显示
        /// </summary>
        /// <param name="rows">坐标</param>
        /// <param name="columns">坐标</param>
        /// <param name="Massage">信息</param>
        public void AddImageMassage(HTuple rows, HTuple columns, HTuple Massage)
        {
            string meblut = "false";
            ColorResult color = ColorResult.green;
            if (columns.Length > Massage.Length)
            {
                Massage = HTuple.TupleGenConst(columns.Length, Massage);
            }
            meblut = meblut.ToLower();
            switch (color)
            {
                case ColorResult.red:

                    MaRed.color = "red";
                    MaRed.Rows.Add(rows);
                    MaRed.Columns.Add(columns);
                    MaRed.Massage.Add(Massage);
                    MaRed.MassageBlute = meblut;

                    break;

                case ColorResult.yellow:

                    if (rows.Length == columns.Length && columns.Length == Massage.Length)
                    {
                        MaYellow.color = "yellow";
                        MaYellow.Rows.Add(rows);
                        MaYellow.Columns.Add(columns);
                        MaYellow.Massage.Add(Massage);
                        MaYellow.MassageBlute = meblut;
                    }
                    break;

                case ColorResult.green:
                    if (rows.Length == columns.Length && columns.Length == Massage.Length)
                    {
                        MaGreen.color = "green";
                        MaGreen.Rows.Add(rows);
                        MaGreen.Columns.Add(columns);
                        MaGreen.Massage.Add(Massage);
                        MaGreen.MassageBlute = meblut;
                    }
                    break;

                case ColorResult.blue:
                    if (rows.Length == columns.Length && columns.Length == Massage.Length)
                    {
                        MaBlue.color = "blue";
                        MaBlue.Rows.Add(rows);
                        MaBlue.Columns.Add(columns);
                        MaBlue.Massage.Add(Massage);
                        MaBlue.MassageBlute = meblut;
                    }
                    break;

                default:
                    break;
            }
        }

        public void AddObj(HObject hObject, ColorResult color)
        {
            try
            {
                if (hObject.CountObj() == 0)
                {
                    return;
                }
                switch (color)
                {
                    case ColorResult.red:
                        HObjectRed = HObjectRed.ConcatObj(hObject);
                        break;

                    case ColorResult.yellow:
                        HObjectYellow = HObjectYellow.ConcatObj(hObject);
                        break;

                    case ColorResult.green:
                        HObjectGreen = HObjectGreen.ConcatObj(hObject);
                        break;

                    case ColorResult.blue:
                        HObjectBlue = HObjectBlue.ConcatObj(hObject);
                        break;

                    default:
                        AddObj(hObject, color.ToString());
                        break;
                }
            }
            catch (Exception)
            {
            }
        }

        private HObject Colrrs = new HObject();

        public void SetObjCross(HObject obj,out HTuple row1,out HTuple col1,out HTuple row2,out HTuple col2)
        {
             row1 = new HTuple();
            col1 = new HTuple();
            row2 = new HTuple();
            col2 = new HTuple();
          
            try
            {
                HOperatorSet.GetImageSize(this.Image, out HTuple wid, out HTuple hei);
                double d = (double)wid / (double)hei;
                HOperatorSet.Union1(obj, out HObject hObject1);
                hObject1 = Vision.XLD_To_Region(hObject1);
                HOperatorSet.SmallestRectangle1(hObject1, out  row1, out  col1, out  row2, out col2);
                HOperatorSet.AreaCenter(hObject1, out HTuple area, out HTuple row, out HTuple col);
                if (row1.Length == 1 && hei.Length == 1 && row.Length == 1)
                {
                    HOperatorSet.GenContourPolygonXld(out HObject hObject, new HTuple(row, row), new HTuple(new HTuple(0), col1));
                    HOperatorSet.GenContourPolygonXld(out HObject hObject2, new HTuple(new HTuple(0), row1), new HTuple(col, col));
                    HOperatorSet.GenContourPolygonXld(out HObject hObject3, new HTuple(new HTuple(row), row), new HTuple(col2, wid));
                    HOperatorSet.GenContourPolygonXld(out HObject hObject4, new HTuple(new HTuple(row2), hei), new HTuple(col, col));
                    this.SetCross(hObject.ConcatObj(hObject2).ConcatObj(hObject3).ConcatObj(hObject4));
             
                    //hWindowControl3.HalconWindow.SetPart(row1 - (200 * 1), col1 - (200 * d), row2 + (200 * 1), col2 + (200 * d));
                    //hWindowControl4.HalconWindow.SetPart(row1 - (200 * 1), col1 - (200 * d), row2 + (200 * 1), col2 + (200 * d));
                }
                else
                {
                    hObject1 = Vision.XLD_To_Region(obj);
                    HOperatorSet.AreaCenter(hObject1, out area, out row, out col);
                    HOperatorSet.SmallestRectangle1(hObject1, out row1, out col1, out row2, out col2);
                    if (row1.Length == 1)
                    {
                        //HOperatorSet.AreaCenter(hObject1, out HTuple area, out HTuple row, out HTuple col);
                        HOperatorSet.GenContourPolygonXld(out HObject hObject, new HTuple(row, row), new HTuple(new HTuple(0), col1));
                        HOperatorSet.GenContourPolygonXld(out HObject hObject2, new HTuple(new HTuple(0), row1), new HTuple(col, col));
                        HOperatorSet.GenContourPolygonXld(out HObject hObject3, new HTuple(new HTuple(row), row), new HTuple(col2, wid));
                        HOperatorSet.GenContourPolygonXld(out HObject hObject4, new HTuple(new HTuple(row2), hei), new HTuple(col, col));
                        this.SetCross(hObject.ConcatObj(hObject2).ConcatObj(hObject3).ConcatObj(hObject4));
                      
                        
                        //    hWindowControl3.HalconWindow.SetPart(row1 - (200 * 1), col1 - (200 * d), row2 + (200 * 1), col2 + (200 * d));
                        //    hWindowControl4.HalconWindow.SetPart(row1 - (200 * 1), col1 - (200 * d), row2 + (200 * 1), col2 + (200 * d));
                    }

                }
                row1 = row1 - (200 * 1);
                col1 = col1 - (200 * d);
                row2 = row2 + (200 * 1);
                col2 = col2 + (200 * d);
            }
            catch (Exception)
            {
            }

        }
        public void SetCross(HObject hObject)
        {
            Colrrs = hObject;
        }

        public void AddObj(HObject hObject, HTuple color = null)
        {
            ListHobj.Add(new Hobjt_Colro(hObject, color));
        }

        /// <summary>
        /// 添加带名称的区域
        /// </summary>
        /// <param name="name">区域名称</param>
        /// <param name="hObject">区域</param>
        /// <param name="colr">颜色</param>
        public void AddNameOBJ(string name, HObject hObject, HTuple colr = null)
        {
            try
            {
                if (Dick.ContainsKey(name))
                {
                    Dick[name] = new Hobjt_Colro(hObject, colr);
                }
                else
                {
                    Dick.Add(name, new Hobjt_Colro(hObject, colr));
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 添加带名称的区域
        /// </summary>
        /// <param name="name">区域名称</param>
        /// <param name="hObject">区域</param>
        /// <param name="colr">颜色</param>
        public void AddNameOBJ(string name, HObject hObject, ColorResult colr)
        {
            try
            {
                if (Dick.ContainsKey(name))
                {
                    Dick[name] = new Hobjt_Colro(hObject, colr.ToString());
                }
                else
                {
                    Dick.Add(name, new Hobjt_Colro(hObject, colr.ToString()));
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 添加信息到图像左上方
        /// </summary>
        /// <param name="massage"></param>
        public void AddMeassge(HTuple massage)
        {
            Massage.Append(massage);
        }

        public HalconRun GetHalcon(HalconRun halcon = null)
        {
            if (halcon != null)
            {
                Halcon = halcon;
            }
            return Halcon;
        }

        private HalconRun Halcon;

        public HTuple GetCaliConstMM(HTuple values)
        {
            return Halcon.GetCaliConstMM(values);
        }

        public HTuple GetCaliConstPx(HTuple values)
        {
            return Halcon.GetCaliConstPx(values);
        }

        /// <summary>
        ///
        /// </summary>
        public void ClearAllObj()
        {
            HObjectYellow.GenEmptyObj();
            HObjectGreen.GenEmptyObj();
            HObjectBlue.GenEmptyObj();
            HObjectRed.GenEmptyObj();
            Massage = new HTuple();
            MaGreen = new MassageText();
            MaRed = new MassageText();
            MaYellow = new MassageText();
            MaBlue = new MassageText();
            Colrrs = new HObject();
            Colrrs.GenEmptyObj();
            ListHobj.Clear();
            oneContOBJs.DicOnes.Clear();
        }

        public void ClearImageMassage()
        {
            MaGreen = new MassageText();
            MaRed = new MassageText();
            MaYellow = new MassageText();
            MaBlue = new MassageText();
        }

        public void Dispose()
        {
            try
            {
                Image.Dispose();
                foreach (var item in oneContOBJs.DicOnes)
                {
                    foreach (var itemtd in item.Value.oneRObjs)
                    {
                       // itemtd.ROI.Dispose();
                        //itemtd.NGROI.Dispose();
                    }
                }
                oneContOBJs.DicOnes.Clear();

                //for (int i = 0; i < ListHobj.Count; i++)
                //{
                //    ListHobj[i].Object.Dispose();
                //}
                HObjectYellow.GenEmptyObj();
                HObjectGreen.GenEmptyObj();
                HObjectBlue.GenEmptyObj();
                HObjectRed.GenEmptyObj();
            }
            catch (Exception ex)
            {
            }
        }

        private HObject HObject;

        private void SelectOBJ(HObject hObject, HTuple hWindowHalconID, int rowi, int coli, bool ismove)
        {
            HTuple intd = new HTuple();
            if (ismove)
            {
                try
                {
                    HOperatorSet.GetObjClass(hObject, out HTuple classv);
                    if (classv.Length == 0)
                    {
                        return;
                    }
                    if (classv[0] == "region")
                    {
                        HOperatorSet.GetRegionIndex(hObject, rowi, coli, out intd);
                        if (intd >= 0)
                        {
                            if (HObject == null)
                            {
                                HObject = new HObject();
                                HObject.GenEmptyObj();
                            }
                            HObject = HObject.ConcatObj(hObject.SelectObj(intd));
                            if (hObject.CountObj() != 1)
                            {
                                HOperatorSet.DispObj(hObject.RemoveObj(intd), hWindowHalconID);
                            }
                            return;
                        }
                    }
                    else if (classv[0] == "xld_cont")
                    {
                        if (HObject == null)
                        {
                            HObject = new HObject();
                            HObject.GenEmptyObj();
                        }
                        HOperatorSet.SelectXldPoint(hObject, out HObject hObject1, rowi, coli);
                        HObject = HObject.ConcatObj(hObject1);
                    }
                }
                catch (Exception ex)
                {
                }
            }
            if (hObject.IsInitialized())
            {
                HOperatorSet.DispObj(hObject, hWindowHalconID);
            }
     
        }

        public void ShowAll(HTuple hWindowHalconID, int rowi = 0, int coli = 0, bool ismovet = false)
        {
            try
            {
                HSystem.SetSystem("flush_graphic", "false");
                HOperatorSet.ClearWindow(hWindowHalconID);

                if (Vision.IsObjectValided(this.Image))
                {
                    HOperatorSet.DispObj(Image, hWindowHalconID);
                }
                else
                {
                    //HOperatorSet.ClearWindow(hWindowHalconID);
                }
                HSystem.SetSystem("flush_graphic", "true");
                //

                if (!IsXLDOrImage)
                {
                    ShouOBJ(hWindowHalconID, rowi, coli, ismovet);
                }
                if (hObject == null)
                {
                    hObject = new HObject();
                    hObject.GenEmptyObj();
                }
                HOperatorSet.DispObj(hObject, hWindowHalconID);
            }
            catch (Exception ex)
            {
                ErosProjcetDLL.Project.AlarmText.AddTextNewLine("HalconResult显示故障:" + ex.Message);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hWindowHalconID"></param>
        /// <param name="rowi"></param>
        /// <param name="coli"></param>
        /// <param name="ismovet"></param>
        public void ShouOBJ(HTuple hWindowHalconID, int rowi = 0, int coli = 0, bool ismovet = false)
        {
            try
            {
                if (HObject != null)
                {
                    HObject.Dispose();
                    HObject = new HObject();
                    HObject.GenEmptyObj();
                }
                //HSystem.SetSystem("flush_graphic", "false");

                HOperatorSet.SetColor(hWindowHalconID, "green");
                SelectOBJ(HObjectGreen, hWindowHalconID, rowi, coli, ismovet);
                HOperatorSet.SetColor(hWindowHalconID, "yellow");
                SelectOBJ(HObjectYellow, hWindowHalconID, rowi, coli, ismovet);
                HOperatorSet.DispObj(Colrrs, hWindowHalconID);
                HOperatorSet.SetColor(hWindowHalconID, "blue");
                SelectOBJ(HObjectBlue, hWindowHalconID, rowi, coli, ismovet);
                HOperatorSet.SetColor(hWindowHalconID, "red");
                SelectOBJ(HObjectRed, hWindowHalconID, rowi, coli, ismovet);
                for (int i = 0; i < ListHobj.Count; i++)
                {
                    if (ListHobj[i].Object == null)
                    {
                        break;
                    }
                    HOperatorSet.SetColor(hWindowHalconID, ListHobj[i].Color);
                    SelectOBJ(ListHobj[i].Object, hWindowHalconID, rowi, coli, ismovet);
                }

                foreach (var item in Dick)
                {
                    if (item.Value.Object == null)
                    {
                        break;
                    }
                    HOperatorSet.SetColor(hWindowHalconID, item.Value.Color);
                    SelectOBJ(item.Value.Object, hWindowHalconID, rowi, coli, ismovet);
                }
                SelesShoOBJ(hWindowHalconID);
                foreach (var item in oneContOBJs.DicOnes)
                {
                    foreach (var itemtd in item.Value.oneRObjs)
                    {
                        if (!item.Value.aOK)
                        {
                            if (itemtd.ROI != null)
                            {
                                HOperatorSet.AreaCenter(itemtd.ROI, out HTuple area, out HTuple row, out HTuple column);
                                if (row.Length == 1)
                                {
                                    Vision.Disp_message(hWindowHalconID, itemtd.NGText, row, column, true, "red");
                                }
                               HOperatorSet.SetColor(hWindowHalconID, "red");
                                HOperatorSet.DispObj(itemtd.NGROI, hWindowHalconID);
                                HOperatorSet.SetColor(hWindowHalconID, Vision.Instance.ROIColr.ToString());
                                HOperatorSet.DispObj(itemtd.ROI, hWindowHalconID);
                                Vision.Disp_message(hWindowHalconID, itemtd.messages, itemtd.rows, itemtd.cols, false, "red");
                            }
                        }
                    }
                }
                if (Massage.Length != 0)
                {
                    if (Massage.Length != 1 || Massage.ToString() != "")
                    {
                        HTuple text = Massage;
                        text.Append(NGMassage);
                        text.Append(OKMassage);
                        if (!ResultBool)
                        {
                            Vision.Disp_message(hWindowHalconID, text, 20, 20, true, "red", ISMassageBack.ToString());
                        }
                        else
                        {
                            Vision.Disp_message(hWindowHalconID, text, 20, 20, true, ColorResu.ToString(), ISMassageBack.ToString());
                        }
                    }
                }
                if (!IsMoveBool)
                {
                    MaRed.ShowMassage(hWindowHalconID);
                    MaGreen.ShowMassage(hWindowHalconID);
                    MaBlue.ShowMassage(hWindowHalconID);
                    MaYellow.ShowMassage(hWindowHalconID);
                }
            }
            catch (Exception ex)
            {
                ErosProjcetDLL.Project.AlarmText.AddTextNewLine("HalconResult显示故障:" + ex.StackTrace);
            }
        }

        /// <summary>
        /// 查看细节
        /// </summary>
        /// <param name="hWindowHalconID"></param>
        public void SelesShoOBJ(HTuple hWindowHalconID)
        {
            if (HObject == null)
            {
                return;
            }
            HOperatorSet.GetObjClass(HObject, out HTuple classv);
            HTuple row, colum, ar, height, width, ratio, circularity, compactness, convexity, Rectangularity = null;
            row = colum = ar = height = width = ratio = circularity = compactness = convexity = Rectangularity = null;
            HTuple hTuple = "";
            if (classv.Length == 0)
            {
                return;
            }
            if (classv[0] == "region")
            {
                HOperatorSet.AreaCenter(HObject, out ar, out row, out colum);
                HOperatorSet.HeightWidthRatio(HObject, out height, out width, out ratio);
                HOperatorSet.Circularity(HObject, out circularity);
                HOperatorSet.Compactness(HObject, out compactness);
                HOperatorSet.Convexity(HObject, out convexity);
                HOperatorSet.Rectangularity(HObject, out Rectangularity);
                hTuple = "X" + row + " Y" + colum + " 面积:" + ar + "高" + height + "宽" + width + "比例" + ratio + "圆度" + circularity
                + Environment.NewLine + "紧密度" + compactness + "凸面" + convexity + "长方形" + Rectangularity;
            }
            else if (classv[0] == "xld_cont")
            {
                HOperatorSet.AreaCenterXld(HObject, out ar, out row, out colum, out HTuple hTuple1);
                HOperatorSet.HeightWidthRatioXld(HObject, out height, out width, out ratio);
                HOperatorSet.CircularityXld(HObject, out circularity);
                HOperatorSet.CompactnessXld(HObject, out compactness);
                HOperatorSet.ConvexityXld(HObject, out convexity);
                hTuple = "X" + row + " Y" + colum + " 面积:" + ar + "高" + height + "宽" + width + "比例" + ratio + "圆度" + circularity
   + Environment.NewLine + "紧密度" + compactness + "凸面" + convexity;
            }
            try
            {
                Vision.Disp_message(hWindowHalconID, hTuple, 120, 20, true, "red");
                HOperatorSet.SetColor(hWindowHalconID, "#ff000040");
                HOperatorSet.GenCrossContourXld(out HObject cross, row, colum, 20, 0);
                HOperatorSet.DispObj(cross, hWindowHalconID);
                HOperatorSet.DispObj(HObject, hWindowHalconID);
            }
            catch (Exception)
            {
            }
        }

        public static void ShowImae(HTuple hWindowHalconID, HObject image)
        {
            try
            {
                HSystem.SetSystem("flush_graphic", "false");
                //Massage = new HTuple();
                if (Vision.IsObjectValided(image))
                {
                    HOperatorSet.GetImageSize(image, out HTuple Width, out HTuple Height);
                    HOperatorSet.SetPart(hWindowHalconID, 1, 1, Height - 1, Width - 1);
                    HOperatorSet.ClearWindow(hWindowHalconID);
                    HSystem.SetSystem("flush_graphic", "true");
                    HOperatorSet.DispObj(image, hWindowHalconID);
                    //HOperatorSet.DispObj(image, hWindowHalconID);
                }
            }
            catch (Exception)
            {
            }
        }

        public int Height;

        public int Width;

        /// <summary>
        ///
        /// </summary>
        /// <param name="hObject"></param>
        public void AddImage(HObject hObject)
        {
            imageS.Add(hObject);
        }

        public void ClerImage()
        {
            imageS.Clear();
        }

        /// <summary>
        /// 红色区域
        /// </summary>
        public HObject HObjectRed { get; set; }

        /// <summary>
        /// 主图像
        /// </summary>
        public HObject Image { get; set; } = new HObject();


        public bool ImagePretreatmentDone;

        /// <summary>
        /// 绿色区域
        /// </summary>
        public HObject HObjectGreen { get; set; } = new HObject();

        /// <summary>
        /// 黄色区域
        /// </summary>

        public HObject HObjectYellow { get; set; } = new HObject();

        /// <summary>
        /// 蓝色区域
        /// </summary>
        public HObject HObjectBlue { get; set; } = new HObject();

        public MassageText MaRed { get; set; } = new MassageText();

        public MassageText MaGreen { get; set; } = new MassageText();

        public MassageText MaYellow { get; set; } = new MassageText();

        public MassageText MaBlue { get; set; } = new MassageText();

        public class Hobjt_Colro
        {
            public Hobjt_Colro(HObject hObject, HTuple color = null)
            {
                Object = hObject;
                if (color == null)
                {
                    Color = "green";
                }
                else
                {
                    Color = color;
                }
            }

            public HObject Object = new HObject();
            public HTuple Color = new HTuple("green");
        }

        public Dictionary<string, Hobjt_Colro> GetKeyHobj(Dictionary<string, Hobjt_Colro> keyValuePairs = null)
        {
            if (keyValuePairs != null)
            {
                Dick = keyValuePairs;
            }
            return Dick;
        }

        private Dictionary<string, Hobjt_Colro> Dick = new Dictionary<string, Hobjt_Colro>();
        public List<Hobjt_Colro> GetHobjt_s(List<Hobjt_Colro> hobjt_Colros=null)
        {
            if (hobjt_Colros!=null)
            {
                ListHobj = hobjt_Colros;
            }
            return ListHobj;
        }
        private List<Hobjt_Colro> ListHobj = new List<Hobjt_Colro>();

        ~OneResultOBj()
        {
            //if (HObject != null)
            //{
            //    HObject.Dispose();
            //}
        }
    }
}