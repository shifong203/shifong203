using HalconDotNet;
using System;
using System.Collections.Generic;
using Vision2.Project.Mes;
using ErosSocket.DebugPLC.Robot;
using static Vision2.vision.HalconRunFile.RunProgramFile.RunProgram;
using Vision2.Project.formula;
using static Vision2.vision.Vision;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
    /// <summary>
    /// 结果类
    /// </summary>
    public class HalconResult
    {
        public HalconResult()
        {
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
        HObject hObject = new HObject();

        public HTuple RowsData { get; set; } = new HTuple();
        public HTuple ColumnsData { get; set; } = new HTuple();

        ///// <summary>
        ///// 机器结果
        ///// </summary>
        bool ResultBool
        {
            get
            {
                for (int i = 0; i < ListNGObj.Count; i++)
                {
                    if (!ListNGObj[i].OK)
                    {
                        return false;
                    }
                }
                return true;
            } 
        }

        /// <summary>
        /// 结果集合
        /// </summary>
        public List<OneRObj> ListNGObj = new List<OneRObj>();

        public bool IsMoveBool;

        public bool IsXLDOrImage;

        public class MassageText
        {
            public List<HTuple> Rows = new List<HTuple>();
            public List<HTuple> Columns = new List<HTuple>();
            public List<HTuple> Massage = new List<HTuple>();
            public string MassageBlute = "false";

            public string color = "red";
            public void ShowMassage(HTuple hWindowHalconID)
            {
                for (int i = 0; i < Massage.Count; i++)
                {
                    if (Massage[i].Length != 0)
                    {
                        Vision.Disp_message(hWindowHalconID, Massage[i], Rows[i], Columns[i], false, color, MassageBlute);
                    }
                }
            }
            public void AddImageMassage(HTuple rows, HTuple columns, HTuple Massage)
            {
                if (columns.Length != Massage.Length)
                {
                    Massage = HTuple.TupleGenConst(columns.Length, Massage);
                }
                if (rows.Length == columns.Length && columns.Length == Massage.Length)
                {
                    this.color = color = "red";
                    this.Rows.Add(rows);
                    this.Columns.Add(columns);
                    this.Massage.Add(Massage);
                }
            }
        }


        public string NGMestage { get; set; } = "";
        /// <summary>
        /// 位名
        /// </summary>
        public string PoxintID { get; set; } = "";
        /// <summary>
        /// 程序名
        /// </summary>
        public string RunName { get; set; } = "";

        /// <summary>
        /// 程序号
        /// </summary>
        public int RunID { get; set; }

        public int Heith;

        public int Width;

        public HObject HObjectRed { get; set; } 
        public HObject Image { get; set; }
        public HObject HObjectGreen { get; set; } = new HObject();

        public HObject HObjectYellow { get; set; } = new HObject();
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
                    Color = "red";
                }
                else
                {
                    Color = color;
                }

            }
            public HObject Object = new HObject();
            public HTuple Color = new HTuple("red");
        }

        List<Hobjt_Colro> ListHobj = new List<Hobjt_Colro>();

        public void AddNGObj(OneRObj rObj)
        {
            ListNGObj.Add(rObj);
        }

        //public void AddData(DataMinMax maxMinValue)
        //{
        //    Data.Add(maxMinValue);
        //}

        public HTuple Massage { get; set; } = new HTuple();
        public HTuple OKMassage { get; set; } = new HTuple();
        public HTuple NGMassage { get; set; } = new HTuple();
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
        public void AddImageMassage(HTuple rows, HTuple columns, HTuple Massage)
        {
            string meblut = "false";
            ColorResult color = ColorResult.red;
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
                if (hObject.CountObj()==0)
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
        HObject Colrrs = new HObject();
        public void SetCross(HObject hObject)
        {
            Colrrs = hObject;
        }
        //public void AddData(MaxMinValue maxMinValue)
        //{
        //    Data.Add(maxMinValue);
        //}
        public void AddObj(HObject hObject, HTuple color = null)
        {
            ListHobj.Add(new Hobjt_Colro(hObject, color));
        }
        public void AddMeassge(HTuple massage)
        {
            Massage.Append(massage);
        }

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
            ListNGObj.Clear();
        }
        public void Dispose()
        {
            try
            {
                Image.Dispose();
                for (int i = 0; i < ListHobj.Count; i++)
                {
                    ListHobj[i].Object.Dispose();
                }
                HObjectYellow.GenEmptyObj();
                HObjectGreen.GenEmptyObj();
                HObjectBlue.GenEmptyObj();
                HObjectRed.GenEmptyObj();
            }
            catch (Exception ex)
            {
            }
        }
        HObject HObject;
        void SelectOBJ(HObject hObject, HTuple hWindowHalconID, int rowi, int coli, bool ismove)
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
            HOperatorSet.DispObj(hObject, hWindowHalconID);
        }
        public void ShowAll(HTuple hWindowHalconID, int rowi = 0, int coli = 0, bool ismovet = false)
        {
            try
            {
                HSystem.SetSystem("flush_graphic", "false");
                HOperatorSet.ClearWindow(hWindowHalconID);

                if (Vision.ObjectValided(this.Image))
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
                ErosProjcetDLL.Project.AlarmText.AddTextNewLine("HalconResult显示故障:"+ex.Message);
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
     
                for (int i = 0; i < ListNGObj.Count; i++)
                {
                    if (ListNGObj[i].ROI!=null)
                    {
                        HOperatorSet.AreaCenter(ListNGObj[i].ROI, out HTuple area, out HTuple row, out HTuple column);
                        if (row.Length == 1)
                        {
                            Vision.Disp_message(hWindowHalconID, ListNGObj[i].NGText, row, column, true, "red");
                        }
                        HOperatorSet.SetColor(hWindowHalconID, "red");
                        HOperatorSet.DispObj(ListNGObj[i].NGROI, hWindowHalconID);
                        HOperatorSet.SetColor(hWindowHalconID, Vision.Instance.ROIColr.ToString());
                        HOperatorSet.DispObj(ListNGObj[i].ROI, hWindowHalconID);
                    }
                }
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
                SelesShoOBJ(hWindowHalconID);
                if (Massage.Length != 0)
                {
                    if (Massage.Length != 1 || Massage.ToString() != "")
                    {
                        HTuple text = Massage;
                        text.Append(NGMassage);
                        text.Append(OKMassage);
                        if (this.RunName != null)
                        {
                            //switch (Vision.GetSaveImageInfo(this.RunName).DispTextType)
                            //{
                            //    case 0:
                            //        break;
                            //    case 1:
                            //        if (ResultBool)
                            //        {
                            //            text = "OK";
                            //        }
                            //        else
                            //        {
                            //            text = "NG";
                            //        }
                            //        break;
                            //    default:
                            //        break;
                            //}
                        }
                        if (!ResultBool)
                        {
                            Vision.Disp_message(hWindowHalconID, text, 20, 20, true, "red");
                        }
                        else
                        {
                            Vision.Disp_message(hWindowHalconID, text, 20, 20, true, "green");
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
                if (Vision.ObjectValided(image))
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
        ~HalconResult()
        {
            if (HObject != null)
            {
                HObject.Dispose();
            }
        }
    }

    public class OneOdata
    {
        public HObject ROI = new HObject();
        public HObject NGROI = new HObject();
        public OneOdata()
        {
            NGROI.GenEmptyObj();
            ROI.GenEmptyObj();
        }
        /// <summary>
        /// NG选项
        /// </summary>
        public string NGText;
        /// <summary>
        /// 复判缺陷
        /// </summary>
        public string RestText = "";

        public List<string> RestStrings = new List<string>();

        public bool OK;

        public bool Done;

    }
    /// <summary>
    /// 单个元件
    /// </summary>
    public class OneRObj
    {
        public HObject ROI;

        public HObject NGROI = new HObject();
        public OneRObj()
        {
            NGROI.GenEmptyObj();
        }
        public DataMinMax dataMinMax = new DataMinMax();

        public List<OneOdata> oneOdatas = new List<OneOdata>();

        public bool OK;

        public bool Done;

        public string ComponentID { get; set; } = "";
        /// <summary>
        /// NG选项
        /// </summary>
        public string NGText;
        /// <summary>
        /// 复判缺陷
        /// </summary>
        public string RestText = "";

        public List<string> RestStrings = new List<string>();

        public void RAddOK()
        {
            if (ROI!=null)
            {
                ROI.GenEmptyObj();
            }
            if (NGROI != null)
            {
                NGROI.GenEmptyObj();
            }
            RestText = "OK";
            OK = true;
            Done = true;
        }
        public void RAddNG()
        {
            ROI.GenEmptyObj();
            Done = true;
            OK = false;
        }


    }
}
