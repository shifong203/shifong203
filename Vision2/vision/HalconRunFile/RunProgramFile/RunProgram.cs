﻿using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;
using Vision2.ErosProjcetDLL.UI.PropertyGrid;
using Vision2.vision.HalconRunFile.Controls;
using static Vision2.vision.HalconRunFile.RunProgramFile.HalconRun;
using static Vision2.vision.Vision;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
    [Serializable]
    /// <summary>
    /// 程序接口
    /// </summary>
    public abstract class RunProgram : DicHtuple, Interface_Vision_Control, IHelp
    {
        public RunProgram()
        {
            if (!ListType.Contains(GetType().ToString()))
            {
                ListType.Add(GetType().ToString());
            }
            Type = GetType().ToString();
            NGRoi = new HObject();
            NGRoi.GenEmptyObj();
            AOIObj = new HObject();
            AOIObj.GenEmptyObj();
            DrawObj.GenEmptyObj();
            //ImageRoi.GenEmptyObj();
        }

        /// <summary>
        /// 子类集合，
        /// </summary>
        public static List<string> ListType { get; set; } = new List<string>();

        /// <summary>
        ///
        /// </summary>
        public void SetJsonThisType()
        {
            Type = GetType().ToString();
        }

        /// <summary>
        /// ///
        /// </summary>
        /// <param name="tabControl"></param>
        /// <param name="data"></param>
        public new void UpProperty(PropertyForm control, object data)
        {
            TabPage tabPage;
            Control controlT = GetThisControl();
            tabPage = new TabPage();

            tabPage.Text = tabPage.Name = "调试接面";
            DrawContrlos draw = new DrawContrlos(this);

            tabPage.Controls.Add(draw);
            draw.Dock = DockStyle.Top;
            if (controlT != null)
            {
                tabPage.Controls.Add(controlT);
                controlT.Dock = DockStyle.Fill;
                controlT.BringToFront();
            }
            control.tabControl1.TabPages.Add(tabPage);
            control.tabControl1.SelectedTab = tabPage;
            base.UpProperty(control, data);
            control.Width = 900;
            //
            tabPage = new TabPage();
            //TabControl tabContr = control.tabControl1;
            //tabPage.Text = "区域属性";
            //tabContr.TabPages.Add(tabPage);
            //tabPage.Controls.Add(new ObjDataGr() { Dock = DockStyle.Fill, Tag = this.KeyHObject });
        }
        public Control GetThisControl()
        {
            Panel panel = new Panel();
            Control control = GetControl();
            if (control != null)
            {
                control.Dock = DockStyle.Fill;
                panel.Controls.Add(control);
                DrawContrlos drawContrlos = new DrawContrlos(this) { Dock = DockStyle.Top };
                panel.Controls.Add(drawContrlos);
                control.Tag = drawContrlos;
                panel.Dock = DockStyle.Fill;
                return panel;
            }
            return null;
        }

        /// <summary>
        /// 选择图像类型
        /// </summary>
        public ImageTypeObj ImageTypeOb { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract Control GetControl();
        [Category("图像预处理"), DisplayName("maskWidth宽度"), Description("Emphasize预处理宽度")]
        public byte EmphasizeW { get; set; } = 7;
        [Category("图像预处理"), DisplayName("maskHeight高度"), Description("Emphasize预处理高度")]
        public byte EmphasizeH { get; set; } = 7;
        [Category("图像预处理"), DisplayName("Emphasize"), Description("Emphasize预处理factor")]
        public byte Emphasizefactor { get; set; } = 1;

        [Category("图像预处理"), DisplayName("image_range预处理模式"), Description("预处理模式，image_range")]
        public bool IsImage_range { get; set; }
        [Category("图像预处理"), DisplayName("Emphasize预处理模式"), Description("启用预处理模式")]
        public bool IsEmphasize { get; set; }

        [Category("图像预处理"), DisplayName("缩放最小灰度值"), Description("SeleImageRange")]
        public byte SeleImageRangeMin { get; set; } = 50;
        [Category("图像预处理"), DisplayName("缩放最大灰度值"), Description("SeleImageRange缩放最大灰度")]
        public byte SeleImageRangeMax { get; set; } = 200;

        [Category("图像预处理"), DisplayName("median_image预处理模式"), Description("预处理模式，median_image")]
        public bool IsMedian_image { get; set; }

        [Category("图像预处理"), DisplayName("median_image直径"), Description("预处理模式，median_image")]
        public double Median_imageVa { get; set; } = 1;

        [Category("图像预处理"), DisplayName("开运算预处理模式"), Description("预处理模式，gray_opening_shape")]
        public bool IsOpen_image { get; set; }
        [Category("图像预处理"), DisplayName("减阈比例"), Description("预处理模式，sub_image")]
        public double Sub_Mult { get; set; } = 1;
        [Category("图像预处理"), DisplayName("减阈值"), Description("预处理模式，sub_image")]
        public double Sub_Add { get; set; } = 10;

        public HObject GetEmset(HObject hObject)
        {
            HObject image = hObject;
            try
            {
                if (AOIObj.IsInitialized() && AOIObj.CountObj() >= 1)
                {
                    HOperatorSet.ReduceDomain(image, AOIObj, out image);
                }
      
                if (DrawObj != null&& DrawObj.IsInitialized() && DrawObj.CountObj() >= 1)
                {
                    HOperatorSet.Complement(DrawObj, out HObject hObject1);
                    HOperatorSet.ReduceDomain(image, hObject1, out image);
                }
                if (IsImage_range) Vision.Scale_image_range(image, out image, SeleImageRangeMin, SeleImageRangeMax);
                try
                {
                    if (IsEmphasize) HOperatorSet.Emphasize(image, out image, EmphasizeW, EmphasizeH, Emphasizefactor);

                }
                catch (Exception)       { }
                try
                {
                    if (IsMedian_image) HOperatorSet.MedianImage(image, out image, "circle", Median_imageVa, "mirrored");
                }
                catch (Exception)
                {
                }
                try
                {
                    if (IsOpen_image)
                    {
                        HOperatorSet.GrayOpeningShape(image, out HObject hObject1, 30, 30, "octagon");
                        HOperatorSet.SubImage(image, hObject1,out image,Sub_Mult, Sub_Add);
                    }
                }
                catch (Exception)      { }
                //try
                //{
                //    if (IsMedian_image) HOperatorSet.MedianImage(image, out image, "circle", Median_imageVa, "mirrored");
                //}
                //catch (Exception) { }
            }
            catch (Exception)
            {
            }
            return image;
        }
        /// <summary>
        /// 保存子类实例
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public override void SaveThis(string path)
        {
            if (this.Name == "")
            {
                MessageBox.Show("名称不能为空");
            }
            if (Name == "测量1")
            {
            }
            HalconRun.ClassToJsonSavePath(this, path + "\\" + this.Name + "\\" + this.Name + this.SuffixName);
        }

        [BrowsableAttribute(false)]
        public HObject XLD
        {
            get
            {
                if (NGRoi == null)
                {
                    NGRoi = new HObject();
                    NGRoi.GenEmptyObj();
                }
                if (!NGRoi.IsInitialized())
                {
                    NGRoi.GenEmptyObj();
                }
                return NGRoi;
            }
            set { NGRoi = value; }
        }

        protected HObject NGRoi;

        ///// <summary>
        ///// 图像区域
        ///// </summary>
        //protected HObject ImageRoi = new HObject();

        /// <summary>
        /// 
        /// </summary>
        protected HObject HObjectGreen = new HObject();

        protected HObject HObjectBule = new HObject();

        /// <summary>
        /// 
        /// </summary>
        protected HObject HObjectYellow = new HObject();
        //public void ADDRed(string ngText,  HObject hObject,HObject roi=null) 
        //{
        //    this.GetPThis().AddNGOBJ(new OneRObj() { NGText = ngText, OK = false, NGROI = Vision.XLD_To_Region(hObject), ROI=roi });
        //}
        public void AddYellow(HObject hObject)
        {
            HObjectYellow = HObjectYellow.ConcatObj(hObject);
        }
        public void AddBule(HObject hObject)
        {
            HObjectBule = HObjectBule.ConcatObj(hObject);
        }

        public void AddGreen(HObject hObject)
        {
            HObjectGreen = HObjectGreen.ConcatObj(hObject);
        }
        public bool ResltBool { get; set; }
        /// <summary>
        /// 掩模区域
        /// </summary>
        public HObject DrawObj { get; set; } = new HObject();
        /// <summary>
        /// 检测区域
        /// </summary>
        public HObject AOIObj  = new HObject();

        [DescriptionAttribute("是否在复判栏显示。"), Category("结果显示"), DisplayName("是否显示复判项")]
        public bool ISCompound { get; set; } = true;
        [DescriptionAttribute("NG结果输出的变量名称。"), Category("结果显示"), DisplayName("结果NG文文本")]
        public string NGText { get; set; } = string.Empty;

        /// <summary>
        /// 外部调用程序
        /// </summary>
        /// <param name="halcon"></param>
        /// <returns></returns>
        public bool Run(HalconRun halcon,OneResultOBj oneResultOBj ,int runid = 0)
        {
            if (!Vision.ObjectValided(halcon.Image()))
            {
                return false;
            }
            Watch.Restart();
            this.SetPThis(halcon);
            try
            {
                ResltBool = true;
                this.ErrBool = false;
                this.ClerItem();
                if (NGRoi != null)
                {
                    NGRoi.GenEmptyObj();
                }
                HObjectGreen = new HObject();
                HObjectBule = new HObject();
                HObjectYellow = new HObject();
                HObjectGreen.GenEmptyObj();
                HObjectYellow.GenEmptyObj();
                HObjectBule.GenEmptyObj();
                this.NGNumber = 0;
                ResltBool = RunHProgram(halcon, oneResultOBj,  runid);
                if (ISCompound)
                {
                    if (halcon.TrayRestData.CompoundReseltBool.ContainsKey(this.Name))
                    {
                        halcon.TrayRestData.CompoundReseltBool[this.Name] = ResltBool;
                    }
                    else
                    {
                        halcon.TrayRestData.CompoundReseltBool.Add(this.Name, ResltBool);
                    }
                }
                Dictionary<string, HTuple> sdd = this.SetData();
  
                if (IsDisObj)
                {
                    oneResultOBj.HalconResult.AddObj(HObjectGreen, ColorResult.green);
                }
                oneResultOBj.HalconResult.AddObj(HObjectYellow, ColorResult.yellow);

                oneResultOBj.HalconResult.AddObj(HObjectBule, ColorResult.blue);
                if (ResltBool)
                {
                    if (this.OKName.Contains(","))
                    {

                        ErosSocket.ErosConLink.StaticCon.SetLinkAddressValue(this.OKName, true);
                    }
                    else
                    {
                        ErosSocket.ErosConLink.StaticCon.SetLingkValue(this.OKName, true.ToString(), out string err);
                    }
                }
                else
                {
                    if (this.CoordinateMeassage != Coordinate.Coordinate_Type.Hide)
                    {
                        oneResultOBj.HalconResult.AddMeassge(this.Name + ":执行失败");
                    }
                    if (this.NGName.Contains(","))
                    {
                        ErosSocket.ErosConLink.StaticCon.SetLinkAddressValue(this.NGName, true);
                    }
                    else
                    {
                        ErosSocket.ErosConLink.StaticCon.SetLingkValue(this.NGName, true.ToString(), out string err);
                    }
                }
            }
            catch (Exception ex)
            {
                this.ErrBool = true;
                halcon.ErrLog(this.Name + ",执行错误:", ex);
                ResltBool = false;
            }

            Watch.Stop();
            return ResltBool;
        }

        public virtual string ShowHelpText()
        {
            return "2.2.1";
        }
        public void ShowHelp()
        {
            Help.ShowHelp(null, Application.StartupPath + @"\help.chm", HelpNavigator.Topic, ShowHelpText() + ".htm");
        }
        /// <summary>
        /// 执行程序
        /// </summary>
        /// <param name="halcon">程序主体</param>
        public abstract bool RunHProgram(HalconRun halcon, OneResultOBj oneResultOBj, int runID = 0,string name = null);

        public abstract RunProgram UpSatrt<T>(string path);
        /// <summary>
        /// 读取地址文件创建实例
        /// </summary>
        /// <typeparam name="T">T改为自己的类实例</typeparam>
        /// <param name="Path"></param>
        /// <returns></returns>
        public RunProgram ReadThis<T>(string Path)
        {
            T tshi;
            if (File.Exists(Path + this.SuffixName))
            {
                HalconRun.ReadPathJsonToCalss(Path + this.SuffixName, out tshi);
            }
            else if (File.Exists(Path + ".txt"))
            {
                HalconRun.ReadPathJsonToCalss(Path + ".txt", out tshi);
            }
            else
            {
                return null;
            }
            RunProgram runProgram = tshi as RunProgram;
            return runProgram;
        }
        [DescriptionAttribute("程序类型。"), Category("运行参数"), DisplayName("程序类型")]
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// 返回父结构
        /// </summary>
        /// <returns></returns>
        public HalconRun GetPThis()
        {

            if (Pthis == null)
            {
                foreach (var item in Vision.GetHimageList())
                {

                    foreach (var itemt in item.Value.GetRunProgram().Values)
                    {
                        if (itemt.Name == this.Name)
                        {
                            Pthis = item.Value;
                            return Pthis;
                        }
                    }

                }

            }
            return Pthis;
        }
        public void SetPThis(HalconRun pthis)
        {
            Pthis = pthis;
            foreach (var item in this.Dic_Measure.Keys_Measure)
            {
                item.Value.SetPThis(pthis);
            }
        }
        HalconRun Pthis;
        InterfaceVisionControl InterfaceVisionControl;
        public void SetPd(InterfaceVisionControl pthis)
        {
            InterfaceVisionControl = pthis;
        }
        public InterfaceVisionControl GetPInt()
        {
            return InterfaceVisionControl;
        }
        //[DescriptionAttribute("程序地址。"), Category("运行参数")]
        //public string Path { get; set; } = Application.StartupPath;
        [DescriptionAttribute("决定程序的运行流程，0以下不执行"), Category("运行参数"), DisplayName("执行编号")]
        public float CDID { get; set; } = 1;

        [DescriptionAttribute("运行时间MS。"), Category("结果显示"), DisplayName("运行时间MS")]
        public long RunTime
        {
            get
            {
                if (Watch != null)
                {
                    return Watch.ElapsedMilliseconds;
                }
                return 0;
            }
        }


        //[DescriptionAttribute("错误信息。"), Category("结果显示"), DisplayName("错误信息")]
        //public string ErrText { get; set; }
        [DescriptionAttribute("显示区域。"), Category("结果显示"), DisplayName("是否显示区域")]
        public bool IsDisObj { get; set; } = true;
        [DescriptionAttribute("显示文本。"), Category("结果显示"), DisplayName("是否显示文本")]
        public bool ISShowText { get; set; } = true;


        [DescriptionAttribute("模板颜色。"), Category("结果显示"), DisplayName("区域颜色")]
        public Color COlorES
        {
            get { return colorEs; }
            set { colorEs = value; color = "#" + value.R.ToString("X2") + value.G.ToString("X2") + value.B.ToString("X2") + value.A.ToString("X2"); }
        }

        public HTuple color = "green";
        private Color colorEs = Color.Green;
        //[DescriptionAttribute("像素与实际坐标转换比值。"), Category("转换"), DisplayName("转换系数")]
        //public double Scale { get; set; } = 1;

        [DescriptionAttribute("是否报错。"), Category("结果显示"), DisplayName("错误状态"), ReadOnly(true), Browsable(false)]
        public bool ErrBool { get; set; }

        [DescriptionAttribute("NG数量。"), Category("结果显示"), DisplayName("NG数量"), Browsable(false)]
        public int NGNumber { get; set; }

        [DescriptionAttribute("隐藏信息,图像坐标，机械坐标。3D坐标"), Category("结果显示"), DisplayName("显示文本信息")]
        public Coordinate.Coordinate_Type CoordinateMeassage { get; set; } = new Coordinate.Coordinate_Type();

        [DescriptionAttribute("OK结果输出的变量名称。"), Category("触发器"), DisplayName("结果OK名称"),]
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        public string OKName { get; set; } = string.Empty;

        [DescriptionAttribute("NG结果输出的变量名称。"), Category("触发器"), DisplayName("结果NG名称")]
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        public string NGName { get; set; } = string.Empty;


        [DescriptionAttribute("根据目标仿射定位。"), Category("定位"), DisplayName("定位目标名称"),
            TypeConverter(typeof(ErosConverter)),
        ErosConverter.ThisDropDown("HomNameList", false, "")]
        public string HomName { get; 
            set; } = string.Empty;


        /// <summary>
        /// 返回程序中指定的仿射集合,未找到或未定义仿射返回Null,
        /// </summary>
        /// <param name="halcon"></param>
        /// <returns></returns>
        public List<HTuple> GetHomMatList(HalconRun halcon)
        {
            if (this.HomName != "")
            {
                if (halcon.GDicModelR().ContainsKey(this.HomName))
                {
                    return halcon.GDicModelR()[this.HomName].HomMat;
                }
                return new List<HTuple>();
            }
            else
            {
                HOperatorSet.HomMat2dIdentity(out HTuple coordHanMat2DXY);
                return new List<HTuple> { coordHanMat2DXY };
            }
        }

        public List<string> HomNameList
        {
            get
            {
                HalconRun halconRun = Pthis as HalconRun;
                if (halconRun != null)
                {
                    halconRun = Vision.GetRunNameVision(halconRun.Text);
                    List<string> listS = new List<string>();
                    foreach (var item in halconRun.GetRunProgram())
                    {
                        if (item.Value is ModelVision)
                        {
                            listS.Add(item.Key);
                        }
                    }
                    return listS;
                }
                else
                {
                    //LogErr("获取定位名称失败，父类为Null");
                }
                return null;
            }
        }
        /// <summary>
        /// 运行计时
        /// </summary>
        public System.Diagnostics.Stopwatch Watch = new System.Diagnostics.Stopwatch();

        /// <summary>
        /// 测量
        /// </summary>
        public Dic_Measure Dic_Measure = new Dic_Measure();

        /// <summary>
        /// 区域
        /// </summary>
        public DicHObject KeyHObject { get; set; } = new DicHObject();

        public override void Dispose()
        {
            try
            {
                Dic_Measure.Dispose();
                Watch = null;
                if (NGRoi != null)
                {
                    NGRoi.Dispose();
                }
                //this.SocketClint = null;
            }
            catch (Exception)
            {
            }
            base.Dispose();
        }

        public void Set_Item<T>(T run_Projet)
        {
            throw new NotImplementedException();
        }

        public void UP_Item<T>(T run_Projet)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 绘制区域
        /// </summary>
        /// <param name="HalconRun"></param>
        /// ><param name="DrawObj">要绘制区域</param>
        /// <returns></returns>
        public static HObject DrawHObj(HalconRun HalconRun, HObject drawObj)
        {
            try
            {
                HalconRun.Focus();
                HTuple hv_Button = null;
                HTuple hv_Row = null, hv_Column = null;
                HalconRun.DrawType = 2;
                HalconRun.DrawErasure = false;
                HOperatorSet.SetDraw(HalconRun.hWindowHalcon(), "fill");
                HObject brush_region_affine = new HObject();
                if (HalconRun.Drawing)
                {
                    MessageBox.Show("当前绘制中,请结束绘制");
                    return drawObj;
                }
                HObject final_region = new HObject();
                HOperatorSet.GenCircle(out final_region, 200, 150, Circl_Rire);
                HalconRun.Drawing = true;
                HOperatorSet.SetColor(HalconRun.hWindowHalcon(), Color.Red.Name.ToLower());

                hv_Button = 0;
                // 4为鼠标右键
                while (hv_Button != 4)
                {
                    //一直在循环,需要让halcon控件也响应事件,不然到时候跳出循环,之前的事件会一起爆发触发,
                    Application.DoEvents();
                    try
                    {
                        HOperatorSet.SetSystem("flush_graphic", "false");
                        HOperatorSet.ClearWindow(HalconRun.hWindowHalcon());

                        HOperatorSet.GetMposition(HalconRun.hWindowHalcon(), out hv_Row, out hv_Column, out hv_Button);
                        HObject hObject = new HObject();


                        HOperatorSet.DispObj(HalconRun.Image(), HalconRun.hWindowHalcon());
                        HOperatorSet.SetSystem("flush_graphic", "true");
                        //HOperatorSet.SetSystem("flush_graphic", "true");
                        if (!Vision.ObjectValided(drawObj))
                        {
                            drawObj = new HObject();
                            drawObj.GenEmptyObj();
                        }
                        if (hv_Button == 1)
                        {
                            //HOperatorSet.SetSystem("flush_graphic", "true");
                            switch (HalconRun.DrawType)
                            {
                                case 1:
                                    HOperatorSet.DrawRegion(out hObject, HalconRun.hWindowHalcon());
                                    hv_Button = 4;
                                    break;
                                case 2:
                                    HOperatorSet.GenCircle(out hObject, hv_Row, hv_Column, Circl_Rire);
                                    break;
                                case 3:
                                    HOperatorSet.GenRectangle2(out hObject, hv_Row, hv_Column, new HTuple(90).TupleRad(), Circl_Rire, Circl_Rire);
                                    break;
                                case 4:
                                    HOperatorSet.GenRectangle2(out hObject, hv_Row, hv_Column, new HTuple(90).TupleRad(), Circl_Rire, Circl_Rire);
                                    break;
                                case 5:
                                    HOperatorSet.DrawPoint(HalconRun.hWindowHalcon(), out HTuple row, out HTuple column);
                                    break;
                                default:
                                    HalconRun.Drawing = false;
                                    Vision.Disp_message(HalconRun.hWindowHalcon(), "未选择绘制类型，已取消绘制！");
                                    return drawObj;
                            }
                            HOperatorSet.SetColor(HalconRun.hWindowHalcon(), "red");
                            HOperatorSet.DispObj(hObject, HalconRun.hWindowHalcon());
                            HOperatorSet.SetColor(HalconRun.hWindowHalcon(), "#b2222270");
                            if (HalconRun.DrawErasure)
                            {
                                HOperatorSet.Difference(hObject, drawObj, out hObject);
                            }
                            else
                            {
                                HOperatorSet.Union2(hObject, drawObj, out hObject);
                            }
                            drawObj = hObject;
                            HOperatorSet.DispObj(drawObj, HalconRun.hWindowHalcon());
                        }
                        else
                        {
                            HOperatorSet.GenCircle(out hObject, hv_Row, hv_Column, Circl_Rire);
                            HOperatorSet.SetColor(HalconRun.hWindowHalcon(), "#b2222270");
                            HOperatorSet.DispObj(drawObj, HalconRun.hWindowHalcon());
                            HOperatorSet.SetColor(HalconRun.hWindowHalcon(), "red");
                            HOperatorSet.DispObj(hObject, HalconRun.hWindowHalcon());
                        }
                    }
                    catch (HalconException ex)
                    {
                        hv_Button = 0;
                    }
                }
            }
            catch (Exception)
            {
            }
            HalconRun.Drawing = false;
            HOperatorSet.SetDraw(HalconRun.hWindowHalcon(), "margin");
            HalconRun.DrawErasure = false;
            return drawObj;
        }
        /// <summary>
        /// 绘制
        /// </summary>
        /// <param name="halconRun"></param>
        /// <param name="drawType"></param>
        /// <param name="drawErasure"></param>
        /// <param name="drawObj"></param>
        /// <returns></returns>
        public static HObject DrawHoj(HalconRun halconRun,int drawType,bool drawErasure,HObject drawObj)
        {
            try
            {

                HTuple hv_Button = null;
                HTuple hv_Row = null, hv_Column = null;
                //HalconRun.DrawType = 2;
                //HalconRun.DrawErasure = false;
                HObject brush_region_affine = new HObject();
                if (halconRun.Drawing)
                {
                    MessageBox.Show("当前绘制中,请结束绘制");
                    return drawObj;
                }
                HObject final_region = new HObject();
                HOperatorSet.GenCircle(out final_region, 200, 150, Circl_Rire);
                halconRun.Drawing = true;
                HOperatorSet.SetColor(halconRun.hWindowHalcon(), Color.Red.Name.ToLower());
                halconRun.GetHWindow().Focus();
                hv_Button = 0;
                // 4为鼠标右键
                while (hv_Button != 4)
                {
                    //一直在循环,需要让halcon控件也响应事件,不然到时候跳出循环,之前的事件会一起爆发触发,
                    Application.DoEvents();
                    try
                    {
                        HOperatorSet.SetSystem("flush_graphic", "false");
                        HOperatorSet.ClearWindow(halconRun.hWindowHalcon());

                        HOperatorSet.GetMposition(halconRun.hWindowHalcon(), out hv_Row, out hv_Column, out hv_Button);
                        HObject hObject = new HObject();
                        HOperatorSet.DispObj(halconRun.Image(), halconRun.hWindowHalcon());
                        HOperatorSet.SetSystem("flush_graphic", "true");
                        //HOperatorSet.SetSystem("flush_graphic", "true");
                        if (!Vision.ObjectValided(drawObj))
                        {
                            drawObj = new HObject();
                            drawObj.GenEmptyObj();
                        }
                        if (hv_Button == 1)
                        {
                            //HOperatorSet.SetSystem("flush_graphic", "true");
                            switch (drawType)
                            {
                                case 1:
                                    HOperatorSet.DrawRegion(out hObject, halconRun.hWindowHalcon());
                                    hv_Button = 4;
                                    break;
                                case 2:
                                    HOperatorSet.GenCircle(out hObject, hv_Row, hv_Column, Circl_Rire);
                                    break;
                                case 3:
                                    HOperatorSet.GenRectangle2(out hObject, hv_Row, hv_Column, new HTuple(90).TupleRad(), Circl_Rire, Circl_Rire);
                                    break;
                                case 4:
                                    HOperatorSet.GenRectangle2(out hObject, hv_Row, hv_Column, new HTuple(90).TupleRad(), Circl_Rire, Circl_Rire);
                                    break;
                                case 5:
                                    HOperatorSet.DrawPoint(halconRun.hWindowHalcon(), out HTuple row, out HTuple column);
                                    break;
                                case 6:
                                    HOperatorSet.GenCircle(out hObject, hv_Row, hv_Column, Circl_Rire);
                                    HOperatorSet.ReduceDomain(drawObj, hObject, out drawObj);
                                    break;

                                default:
                                    Vision.Disp_message(halconRun.hWindowHalcon(), "未选择绘制类型，已取消绘制！");
                                    return drawObj;
                            }
                            HOperatorSet.SetColor(halconRun.hWindowHalcon(), "yellow");
                            HOperatorSet.DispObj(hObject, halconRun.hWindowHalcon());
                            HOperatorSet.SetColor(halconRun.hWindowHalcon(), "blue");
                            if (drawType <= 5)
                            {
                                if (drawErasure)
                                {
                                    HOperatorSet.Difference(hObject, drawObj, out hObject);
                                }
                                else
                                {
                                    HOperatorSet.Union2(hObject, drawObj, out hObject);
                                }
                                drawObj = hObject;
                            }

                            HOperatorSet.DispObj(drawObj, halconRun.hWindowHalcon());
                        }
                        else
                        {
                            HOperatorSet.GenCircle(out hObject, hv_Row, hv_Column, Circl_Rire);
                            HOperatorSet.SetColor(halconRun.hWindowHalcon(), "blue");
                            HOperatorSet.DispObj(drawObj, halconRun.hWindowHalcon());
                            HOperatorSet.SetColor(halconRun.hWindowHalcon(), "yellow");
                            HOperatorSet.DispObj(hObject, halconRun.hWindowHalcon());
                        }
                    }
                    catch (HalconException ex)
                    {
                        hv_Button = 0;
                    }
                }
            }
            catch (Exception)
            {
            }
            halconRun.Drawing = false;

            return drawObj;
        }
        /// <summary>
        /// 绘制区域
        /// </summary>
        /// <param name="hWindowHalconID">窗口ID</param>
        /// <param name="Image">图像</param>
        /// <param name="DrawType">绘制类型</param>
        /// <param name="DrawErasure">绘制或擦除</param>
        /// <param name="drawObj">绘制区域</param>
        /// <returns>完成区域</returns>
        public static HObject DrawHObj(HTuple hWindowHalconID, HObject Image, int DrawType, bool DrawErasure, HObject drawObj)
        {
            try
            {

                HTuple hv_Button = null;
                HTuple hv_Row = null, hv_Column = null;
                //HalconRun.DrawType = 2;
                //HalconRun.DrawErasure = false;
                HObject brush_region_affine = new HObject();

                //if (HalconRun.Drawing)
                //{
                //    MessageBox.Show("当前绘制中,请结束绘制");
                //    return drawObj;
                //}
                HObject final_region = new HObject();
                HOperatorSet.GenCircle(out final_region, 200, 150, Circl_Rire);
                //HalconRun.Drawing = true;
                HOperatorSet.SetColor(hWindowHalconID, Color.Red.Name.ToLower());
                //HalconRun.GetHWindow().Focus();
                hv_Button = 0;
                // 4为鼠标右键
                while (hv_Button != 4)
                {
                    //一直在循环,需要让halcon控件也响应事件,不然到时候跳出循环,之前的事件会一起爆发触发,
                    Application.DoEvents();
                    try
                    {
                        HOperatorSet.SetSystem("flush_graphic", "false");
                        HOperatorSet.ClearWindow(hWindowHalconID);

                        HOperatorSet.GetMposition(hWindowHalconID, out hv_Row, out hv_Column, out hv_Button);
                        HObject hObject = new HObject();


                        HOperatorSet.DispObj(Image, hWindowHalconID);
                        HOperatorSet.SetSystem("flush_graphic", "true");
                        //HOperatorSet.SetSystem("flush_graphic", "true");
                        if (!Vision.ObjectValided(drawObj))
                        {
                            drawObj = new HObject();
                            drawObj.GenEmptyObj();
                        }
                        if (hv_Button == 1)
                        {
                            //HOperatorSet.SetSystem("flush_graphic", "true");
                            switch (DrawType)
                            {
                                case 1:
                                    HOperatorSet.DrawRegion(out hObject, hWindowHalconID);
                                    hv_Button = 4;
                                    break;
                                case 2:
                                    HOperatorSet.GenCircle(out hObject, hv_Row, hv_Column, Circl_Rire);
                                    break;
                                case 3:
                                    HOperatorSet.GenRectangle2(out hObject, hv_Row, hv_Column, new HTuple(90).TupleRad(), Circl_Rire, Circl_Rire);
                                    break;
                                case 4:
                                    HOperatorSet.GenRectangle2(out hObject, hv_Row, hv_Column, new HTuple(90).TupleRad(), Circl_Rire, Circl_Rire);
                                    break;
                                case 5:
                                    HOperatorSet.DrawPoint(hWindowHalconID, out HTuple row, out HTuple column);
                                    break;
                                case 6:
                                    HOperatorSet.GenCircle(out hObject, hv_Row, hv_Column, Circl_Rire);
                                    HOperatorSet.ReduceDomain(drawObj, hObject, out drawObj);
                                    break;

                                default:
                                    Vision.Disp_message(hWindowHalconID, "未选择绘制类型，已取消绘制！");
                                    return drawObj;
                            }
                            HOperatorSet.SetColor(hWindowHalconID, "yellow");
                            HOperatorSet.DispObj(hObject, hWindowHalconID);
                            HOperatorSet.SetColor(hWindowHalconID, "blue");
                            if (DrawType <= 5)
                            {
                                if (DrawErasure)
                                {
                                    HOperatorSet.Difference(hObject, drawObj, out hObject);
                                }
                                else
                                {
                                    HOperatorSet.Union2(hObject, drawObj, out hObject);
                                }
                                drawObj = hObject;
                            }

                            HOperatorSet.DispObj(drawObj, hWindowHalconID);
                        }
                        else
                        {
                            HOperatorSet.GenCircle(out hObject, hv_Row, hv_Column, Circl_Rire);
                            HOperatorSet.SetColor(hWindowHalconID, "blue");
                            HOperatorSet.DispObj(drawObj, hWindowHalconID);
                            HOperatorSet.SetColor(hWindowHalconID, "yellow");
                            HOperatorSet.DispObj(hObject, hWindowHalconID);
                        }
                    }
                    catch (HalconException ex)
                    {
                        hv_Button = 0;
                    }
                }
            }
            catch (Exception)
            {
            }
            return drawObj;
        }
        public static double Circl_Rire { get; set; }

        /// <summary>
        /// 擦除区域
        /// </summary>
        /// <param name="HalconRun"></param>
        /// <param name="circl_rea"></param>
        /// <returns></returns>
        public static HObject DrawRmoveObj(HalconRun HalconRun, HObject drawObj)
        {
            try
            {
                HalconRun.Focus();
                HTuple hv_Button = null;
                HTuple hv_Row = null, hv_Column = null;
                if (HalconRun.Drawing)
                {
                    MessageBox.Show("当前绘制中,请结束绘制");
                    return drawObj;
                }
                HalconRun.DrawType = 2;
                HalconRun.DrawErasure = true;
                HObject brush_region_affine = new HObject();
                HObject final_region = new HObject();
                HOperatorSet.GenCircle(out final_region, 200, 150, Circl_Rire);
                HalconRun.Drawing = true;
                HOperatorSet.SetDraw(HalconRun.hWindowHalcon(), "fill");
                HOperatorSet.SetColor(HalconRun.hWindowHalcon(), "red");
                hv_Button = 0;
                // 4为鼠标右键
                while (hv_Button != 4)
                {
                    //一直在循环,需要让halcon控件也响应事件,不然到时候跳出循环,之前的事件会一起爆发触发,
                    Application.DoEvents();
                    try
                    {
                        HOperatorSet.SetSystem("flush_graphic", "false");
                        HOperatorSet.ClearWindow(HalconRun.hWindowHalcon());
                        HOperatorSet.GetMposition(HalconRun.hWindowHalcon(), out hv_Row, out hv_Column, out hv_Button);
                        HObject hObject = new HObject();
                        HOperatorSet.SetSystem("flush_graphic", "true");
                        HOperatorSet.DispObj(HalconRun.Image(), HalconRun.hWindowHalcon());
                        if (!Vision.ObjectValided(drawObj))
                        {
                            drawObj = new HObject();
                            drawObj.GenEmptyObj();
                        }
                        if (hv_Button == 1)
                        {
                            switch (HalconRun.DrawType)
                            {
                                case 1:
                                    HOperatorSet.DrawRegion(out hObject, HalconRun.hWindowHalcon());
                                    hv_Button = 4;
                                    break;
                                case 2:
                                    HOperatorSet.GenCircle(out hObject, hv_Row, hv_Column, Circl_Rire);
                                    break;
                                case 3:
                                    HOperatorSet.GenRectangle2(out hObject, hv_Row, hv_Column, new HTuple(90).TupleRad(), Circl_Rire, Circl_Rire);
                                    break;
                                case 4:
                                    HOperatorSet.GenRectangle2(out hObject, hv_Row, hv_Column, new HTuple(90).TupleRad(), Circl_Rire, Circl_Rire);
                                    break;
                                case 5:
                                    HOperatorSet.DrawPoint(HalconRun.hWindowHalcon(), out HTuple row, out HTuple column);
                                    break;
                                default:
                                    HalconRun.Drawing = false;
                                    Vision.Disp_message(HalconRun.hWindowHalcon(), "未选择绘制类型，已取消绘制！");
                                    return drawObj;
                            }

                            HOperatorSet.SetColor(HalconRun.hWindowHalcon(), "white");
                            HOperatorSet.DispObj(hObject, HalconRun.hWindowHalcon());
                            HOperatorSet.SetColor(HalconRun.hWindowHalcon(), "#b2222270");


                            if (HalconRun.DrawErasure)
                            {
                                HOperatorSet.Difference(drawObj, hObject, out hObject);
                            }
                            else
                            {
                                HOperatorSet.Union2(hObject, drawObj, out hObject);
                            }
                            drawObj = hObject;
                            HOperatorSet.DispObj(drawObj, HalconRun.hWindowHalcon());
                        }
                        else
                        {
                            HOperatorSet.GenCircle(out hObject, hv_Row, hv_Column, Circl_Rire);
                            HOperatorSet.SetColor(HalconRun.hWindowHalcon(), "#b2222270");
                            HOperatorSet.DispObj(drawObj, HalconRun.hWindowHalcon());
                            HOperatorSet.SetColor(HalconRun.hWindowHalcon(), "white");
                            HOperatorSet.DispObj(hObject, HalconRun.hWindowHalcon());
                        }
                    }
                    catch (HalconException ex)
                    {
                        hv_Button = 0;
                    }
                }
            }
            catch (Exception)
            {
            }
            HOperatorSet.SetDraw(HalconRun.hWindowHalcon(), "margin");
            HalconRun.Drawing = false;
            HalconRun.DrawErasure = false;
            return drawObj;
        }
        /// <summary>
        /// 移动区域
        /// </summary>
        /// <param name="halconRun"></param>
        /// <param name="hObject"></param>
        /// <returns></returns>
        public static HObject DragMoveOBJ(HalconRun halconRun, HObject hObject)
        {
            try
            {
                if (halconRun.Drawing)
                {
                    MessageBox.Show("当前绘制中,请结束绘制");
                    return hObject;
                }
                halconRun.Drawing = true;
                halconRun.HobjClear();
                HOperatorSet.SetSystem("flush_graphic", "true");
                int xPos = 0;
                int yPos = 0;
                HTuple ROW = new HTuple();
                HTuple COL = new HTuple();

                bool MoveFlag = false;
                //HOperatorSet.DragRegion1(hObject, out hObject, halconRun.hWindowHalcon());
                try
                {
                    HTuple homMat2d;
                    HTuple hv_Button = null;
                    HTuple hv_Row = null, hv_Column = null;
                    HOperatorSet.AreaCenter(hObject, out HTuple area, out HTuple rows, out HTuple column);
                    HObject hObject1 = new HObject();
                    hObject1 = hObject.Clone();
    

                    HOperatorSet.SetColor(halconRun.hWindowHalcon(), Color.Red.Name.ToLower());
                    halconRun.AddMessage("右键点击区域开始移动，右键结束移动");
                    halconRun.AddOBJ(hObject);
                    halconRun.ShowObj();
                    halconRun.Focus();
                    hv_Button = 0;
                    // 4为鼠标右键
                    while (hv_Button != 4)
                    {
                        //一直在循环,需要让halcon控件也响应事件,不然到时候跳出循环,之前的事件会一起爆发触发,
                        Application.DoEvents();
                        try
                        {
                            HOperatorSet.GetMposition(halconRun.hWindowHalcon(), out hv_Row, out hv_Column, out hv_Button);
                            if (hv_Button == 4)
                            {
                                halconRun.HobjClear();
                                halconRun.AddMessage("结束移动");
                                halconRun.AddOBJ(hObject);
                                halconRun.ShowObj();
                                halconRun.Drawing = false;
                                return hObject;
                            }
                            if (hv_Button==1)
                            {
                                if (!MoveFlag)
                                {
                                    HOperatorSet.GetRegionIndex(hObject, hv_Row, hv_Column, out HTuple index);
                                    if (index == 1)
                                    {
                                        HOperatorSet.AreaCenter(hObject, out area, out rows, out column);
                                        ROW = hv_Row;
                                        COL = hv_Column;
                                        MoveFlag = true;
                                        xPos = hv_Row;//当前x坐标.
                                        yPos = hv_Column;//当前y坐标.
                                    }
                                }
                                else
                                {
                                    ROW = rows + hv_Row - xPos;//设置x坐标.
                                    COL = column + hv_Column - yPos;//设置y坐标.
                                    HOperatorSet.VectorAngleToRigid(rows, column, 0, ROW, COL, 0, out homMat2d);
                                    HOperatorSet.AffineTransRegion(hObject1, out hObject, homMat2d, "nearest_neighbor");
                                    HOperatorSet.SetSystem("flush_graphic", "false");
                                    HOperatorSet.ClearWindow(halconRun.hWindowHalcon());
                                    HOperatorSet.DispObj(halconRun.Image(), halconRun.hWindowHalcon());
                                    HOperatorSet.SetSystem("flush_graphic", "true");
                                    HOperatorSet.DispObj(hObject, halconRun.hWindowHalcon());
                                }
               
                            }
                            else  if(MoveFlag)
                            {
                                halconRun.HobjClear();
                                halconRun.AddMessage("右键点击区域开始移动，右键结束移动");
                                halconRun.AddOBJ(hObject);
                                halconRun.ShowObj();
                                hObject1 = hObject;
                                MoveFlag = false;
                            }
                   

                        }
                        catch (HalconException ex)
                        {
                            hv_Button = 0;
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            catch (Exception EX)
            {
            }
            halconRun.Drawing = false;
            halconRun.Drawing = false;
            return hObject;

        }

        /// <summary>
        /// 移动区域
        /// </summary>
        /// <param name="halconRun"></param>
        /// <param name="hObject"></param>
        /// <returns></returns>
        public static HObject DragMoveOBJS(HalconRun halconRun, HObject hObject)
        {
            try
            {
                if (halconRun.Drawing)
                {
                    MessageBox.Show("当前绘制中,请结束绘制");
                    return hObject;
                }
                halconRun.Drawing = true;
                halconRun.HobjClear();
                HOperatorSet.SetSystem("flush_graphic", "true");
                int xPos = 0;
                int yPos = 0;
                HTuple ROW = new HTuple();
                HTuple COL = new HTuple();

                bool MoveFlag = false;
                //HOperatorSet.DragRegion1(hObject, out hObject, halconRun.hWindowHalcon());
                try
                {
                    HTuple index = 0;
              HTuple homMat2d;
                    HTuple hv_Button = null;
                    HTuple hv_Row = null, hv_Column = null;
                    HOperatorSet.AreaCenter(hObject, out HTuple area, out HTuple rows, out HTuple column);
                    HObject hObject1 = new HObject();
                    HObject hObject2 = new HObject();
                    hObject1 = hObject.Clone();
                    HOperatorSet.SetColor(halconRun.hWindowHalcon(), Color.Red.Name.ToLower());
                    halconRun.AddMessage("右键点击区域开始移动，右键结束移动");
                    halconRun.AddOBJ(hObject);
                    halconRun.ShowObj();
                    halconRun.Focus();
                    hv_Button = 0;
                    // 4为鼠标右键
                    while (hv_Button != 4)
                    {
                        //一直在循环,需要让halcon控件也响应事件,不然到时候跳出循环,之前的事件会一起爆发触发,
                        Application.DoEvents();
                        try
                        {
                            HOperatorSet.GetMposition(halconRun.hWindowHalcon(), out hv_Row, out hv_Column, out hv_Button);
                            if (hv_Button == 4)
                            {
                                halconRun.HobjClear();
                                halconRun.AddMessage("结束移动");
                                halconRun.AddOBJ(hObject);
                                halconRun.ShowObj();
                                halconRun.Drawing = false;
                                return hObject;
                            }
                            if (hv_Button == 1)
                            {
                       
                                if (!MoveFlag)
                                {
                                    HOperatorSet.GetRegionIndex(hObject, hv_Row, hv_Column, out index);
                                    if (index != 0)
                                    {
                                        HOperatorSet.SelectObj(hObject, out  hObject2, index);
                                        HOperatorSet.AreaCenter(hObject2, out area, out rows, out column);
                                        ROW = hv_Row;
                                        COL = hv_Column;
                                        MoveFlag = true;
                                        xPos = hv_Row;//当前x坐标.
                                        yPos = hv_Column;//当前y坐标.
                                    }
                                }
                                else
                                {
                                    ROW = rows + hv_Row - xPos;//设置x坐标.
                                    COL = column + hv_Column - yPos;//设置y坐标.

                                    HOperatorSet.VectorAngleToRigid(rows, column, 0, ROW, COL, 0, out homMat2d);
                                    HOperatorSet.AffineTransRegion(hObject2, out HObject hObject3, homMat2d, "nearest_neighbor");
                                    hObject = hObject1.ReplaceObj(hObject3, index);
                                    HOperatorSet.SetSystem("flush_graphic", "false");
                                    HOperatorSet.ClearWindow(halconRun.hWindowHalcon());
                                    HOperatorSet.DispObj(halconRun.Image(), halconRun.hWindowHalcon());
                                    HOperatorSet.SetSystem("flush_graphic", "true");
                                    HOperatorSet.DispObj(hObject, halconRun.hWindowHalcon());
                                }

                            }
                            else if (MoveFlag)
                            {
                                halconRun.HobjClear();
                                halconRun.AddMessage("右键点击区域开始移动，右键结束移动");
                                halconRun.AddOBJ(hObject);
                                halconRun.ShowObj();
                                hObject1 = hObject;
                                MoveFlag = false;
                            }


                        }
                        catch (HalconException ex)
                        {
                            hv_Button = 0;
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            catch (Exception EX)
            {
            }
            halconRun.Drawing = false;
            halconRun.Drawing = false;
            return hObject;

        }
        /// <summary>
        /// 绘制区域
        /// </summary>
        /// <param name="halcon"></param>
        /// <param name="hObject"></param>
        /// <returns></returns>
        public static HObject DrawModOBJ(HalconRun halcon, HalconRun.EnumDrawType enumDrawType, HObject hObject)
        {
            HTuple hv_RowA = null, hv_ColumnA = null, hv_Phi = null;
            HTuple hv_Length1, hv_Length2;
            try
            {
                halcon.Focus();
                if (halcon.Drawing)
                {
                    MessageBox.Show("当前绘制中,请结束绘制");
                    return hObject;
                }
                halcon.Drawing = true;
                HOperatorSet.SetLineWidth(halcon.hWindowHalcon(), 2);
                HOperatorSet.SetColor(halcon.hWindowHalcon(), "green");
                HOperatorSet.DispObj(halcon.Image(), halcon.hWindowHalcon());
                if (!hObject.IsInitialized())
                {
                    HOperatorSet.GenRectangle2(out hObject, 1000, 1000, 0, 100, 100);
                }
                if (enumDrawType == EnumDrawType.Rectangle2)
                {
                    HOperatorSet.SmallestRectangle2(hObject, out hv_RowA, out hv_ColumnA, out hv_Phi, out hv_Length1, out hv_Length2);
                    if (hv_Length2.Length == 1)
                    {
                        HOperatorSet.DrawRectangle2Mod(halcon.hWindowHalcon(), hv_RowA[0], hv_ColumnA[0], hv_Phi[0], hv_Length1[0], hv_Length2[0],
                            out hv_RowA, out hv_ColumnA, out hv_Phi,
                       out hv_Length1, out hv_Length2);
                    }
                    else
                    {
                        HOperatorSet.DrawRectangle2(halcon.hWindowHalcon(), out hv_RowA, out hv_ColumnA, out hv_Phi,
                       out hv_Length1, out hv_Length2);
                    }
                    HOperatorSet.GenRectangle2(out hObject, hv_RowA, hv_ColumnA, hv_Phi, hv_Length1, hv_Length2);
                }
                else if (enumDrawType == EnumDrawType.Rectangle1)
                {
                    HOperatorSet.SmallestRectangle1(hObject, out hv_RowA, out hv_ColumnA, out hv_Length1, out hv_Length2);
                    if (hv_Length2.Length == 1)
                    {
                        HOperatorSet.DrawRectangle1Mod(halcon.hWindowHalcon(), hv_RowA[0], hv_ColumnA[0], hv_Length1[0], hv_Length2[0],
                            out hv_RowA, out hv_ColumnA, out hv_Length1, out hv_Length2);
                    }
                    else
                    {
                        HOperatorSet.DrawRectangle1(halcon.hWindowHalcon(), out hv_RowA, out hv_ColumnA, out hv_Length1, out hv_Length2);
                    }
                    HOperatorSet.GenRectangle1(out hObject, hv_RowA, hv_ColumnA, hv_Length1, hv_Length2);
                }
                else if (enumDrawType == EnumDrawType.Circle)
                {
                    HOperatorSet.SmallestCircle(hObject, out hv_RowA, out hv_ColumnA, out hv_Length1);
                    if (hv_Length1.Length == 1)
                    {
                        HOperatorSet.DrawCircleMod(halcon.hWindowHalcon(), hv_RowA[0], hv_ColumnA[0], hv_Length1[0], out hv_RowA, out hv_ColumnA, out hv_Length1);
                    }
                    else
                    {
                        HOperatorSet.DrawCircle(halcon.hWindowHalcon(), out hv_RowA, out hv_ColumnA, out hv_Length1);
                    }
                    HOperatorSet.GenCircle(out hObject, hv_RowA, hv_ColumnA, hv_Length1);
                }
                else if (enumDrawType == EnumDrawType.Ellipes)
                {
                    HOperatorSet.SmallestRectangle2(hObject, out hv_RowA, out hv_ColumnA, out hv_Phi, out hv_Length1, out hv_Length2);
                    if (hv_Length1.Length == 1)
                    {
                        HOperatorSet.DrawEllipseMod(halcon.hWindowHalcon(), hv_RowA[0], hv_ColumnA[0], hv_Phi[0], hv_Length1[0], hv_Length2[0],
                            out hv_RowA, out hv_ColumnA, out hv_Phi,
                       out hv_Length1, out hv_Length2);
                    }
                    else
                    {
                        HOperatorSet.DrawEllipse(halcon.hWindowHalcon(), out hv_RowA, out hv_ColumnA, out hv_Phi, out hv_Length1, out hv_Length2);
                    }
                    HOperatorSet.GenEllipse(out hObject, hv_RowA[0], hv_ColumnA[0], hv_Phi[0], hv_Length1[0], hv_Length2[0]);
                }
                halcon.AddOBJ(hObject);
                halcon.ShowObj();
                //HOperatorSet.DispObj(hObject, halcon.hWindowHalcon());
            }
            catch (Exception exception)
            {
            }
            halcon.Drawing = false;
            return hObject;
        }

    }
}