using HalconDotNet;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class PretreatmentUserControl : UserControl
    {
        public PretreatmentUserControl()
        {
            InitializeComponent();
        }

        public void SetData(PretreatmentVsiion run,OneResultOBj oneResultOBj)
        {
            try
            {
                OneResul = oneResultOBj;
                   pretreatment = run;
                isCheave = true;
                checkBox4.Checked = pretreatment.IsImage_range;
                checkBox3.Checked = pretreatment.IsEmphasize;
                numericUpDown1.Value = (decimal)pretreatment.Emphasizefactor;
                numericUpDown2.Value = (decimal)pretreatment.EmphasizeH;
                numericUpDown3.Value = (decimal)pretreatment.EmphasizeW;
                trackBar1.Value = (int)numericUpDown1.Value;
                trackBar2.Value = (int)numericUpDown2.Value;
                trackBar3.Value = (int)numericUpDown3.Value;
                numericUpDown11.Value = (decimal)pretreatment.SeleImageRangeMin;
                numericUpDown12.Value = (decimal)pretreatment.SeleImageRangeMax;
                trackBar5.Value = (int)numericUpDown11.Value;
                trackBar4.Value = (int)numericUpDown12.Value;
                checkBox1.Checked = pretreatment.IsMedian_image;
                checkBox7.Checked = pretreatment.IsOpen_image;
                numericUpDown20.Value = (decimal)pretreatment.Sub_Mult;
                numericUpDown21.Value = (decimal)pretreatment.Sub_Add;
                numericUpDown19.Value = (decimal)pretreatment.Median_imageVa;

            }
            catch (Exception)
            {
            }
            isCheave = false;
        }
        public void SetData(RunProgram run,OneResultOBj oneResultOBj)
        {
            RunP = run;
            isCheave = true;
            OneResul = oneResultOBj;
            try
            {
                checkBox4.Checked = pretreatment.IsImage_range;
                checkBox3.Checked = pretreatment.IsEmphasize;
                numericUpDown1.Value = (decimal)pretreatment.Emphasizefactor;
                numericUpDown2.Value = (decimal)pretreatment.EmphasizeH;
                numericUpDown3.Value = (decimal)pretreatment.EmphasizeW;
                trackBar1.Value = (int)numericUpDown1.Value;
                trackBar2.Value = (int)numericUpDown2.Value;
                trackBar3.Value = (int)numericUpDown3.Value;
                numericUpDown11.Value = (decimal)pretreatment.SeleImageRangeMin;
                numericUpDown12.Value = (decimal)pretreatment.SeleImageRangeMax;
                trackBar5.Value = (int)numericUpDown11.Value;
                trackBar4.Value = (int)numericUpDown12.Value;
            }
            catch (Exception)
            {

            }
            isCheave = false;
        }

        public RunProgramFile.RunProgram RunP;
        public OneResultOBj OneResul;
        private PretreatmentVsiion pretreatment;
        private bool isCheave;

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            try
            {
                if (isCheave)
                {
                    return;
                }
                pretreatment.IsEmphasize = checkBox3.Checked;
                pretreatment.IsImage_range = checkBox4.Checked;
                pretreatment.IsMedian_image = checkBox1.Checked;
                pretreatment.IsOpen_image = checkBox7.Checked;
                pretreatment.Median_imageVa = (double)numericUpDown19.Value;
                pretreatment.Sub_Mult = (double)numericUpDown20.Value;
                pretreatment.Sub_Add = (double)numericUpDown21.Value;
                numericUpDown12.Value = trackBar4.Value;
                numericUpDown11.Value = trackBar5.Value;
                pretreatment.SeleImageRangeMax = (byte)numericUpDown12.Value;
                pretreatment.SeleImageRangeMin = (byte)numericUpDown11.Value;
                numericUpDown1.Value = trackBar1.Value;
                numericUpDown2.Value = trackBar2.Value;
                numericUpDown3.Value = trackBar3.Value;
                pretreatment.Emphasizefactor = (byte)numericUpDown1.Value;
                pretreatment.EmphasizeH = (byte)numericUpDown2.Value;
                pretreatment.EmphasizeW = (byte)numericUpDown3.Value;
                HObject image = pretreatment.GetEmset(OneResul.Image);
                hWind.OneResIamge.Image = image;
                hWind.ShowImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
 

        public HWindID hWind;
         System.Diagnostics.Stopwatch Watch = new System.Diagnostics.Stopwatch();
        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    Watch.Restart();
                    HObject image = pretreatment.GetEmset(OneResul.Image);
                    Watch.Stop();
                    hWind.SetImaage(image);
                    hWind.OneResIamge.AddMeassge("运行时间" + Watch.ElapsedMilliseconds);
                    hWind.ShowImage();
                }
                catch (Exception)
                {
                }
            }
            catch (Exception)
            {
            }
        }

        private void label10_Click(object sender, EventArgs e)
        {
        }

        private void label11_Click(object sender, EventArgs e)
        {
        }

        private void PretreatmentUserControl_Load(object sender, EventArgs e)
        {
            try
            {
                hWind = new HWindID();
                hWind.Initialize(hWindowControl1);
            }
            catch (Exception)
            {
            }
        }
  
    }

    public class PretreatmentVsiion
    {
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
        public virtual HObject GetEmset(HObject imageTd, HObject aoiObj, HObject drawObj, HTuple homat2d = null)
        {
            HObject image = imageTd;
            HObject aoiobj = aoiObj;
            HObject drawobj = drawObj;
            try
            {
                if (aoiobj.IsInitialized() && aoiobj.CountObj() >= 1)
                {
                    if (homat2d != null)
                    {
                        HOperatorSet.AffineTransRegion(aoiobj, out aoiobj, homat2d, "nearest_neighbor");
                    }

                    HOperatorSet.ReduceDomain(image, aoiobj, out image);
                }
                if (drawobj != null && drawobj.IsInitialized() && drawobj.CountObj() >= 1)
                {
                    if (homat2d != null)
                    {
                        HOperatorSet.AffineTransRegion(drawobj, out drawobj, homat2d, "nearest_neighbor");
                    }
                    HOperatorSet.Complement(drawobj, out HObject hObject1);
                    HOperatorSet.ReduceDomain(image, hObject1, out image);
                }
                if (IsImage_range) Vision.Scale_image_range(image, out image, SeleImageRangeMin, SeleImageRangeMax);
                try
                {
                    if (IsEmphasize) HOperatorSet.Emphasize(image, out image, EmphasizeW, EmphasizeH, Emphasizefactor);
                }
                catch (Exception) { }
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
                        HOperatorSet.SubImage(image, hObject1, out image, Sub_Mult, Sub_Add);
                    }
                }
                catch (Exception) { }
            }
            catch (Exception)
            {
            }
            return image;
        }
        public virtual HObject GetEmset(HObject imageTd)
        {
            HObject image = imageTd;
            try
            {
                if (IsImage_range) Vision.Scale_image_range(image, out image, SeleImageRangeMin, SeleImageRangeMax);
                try
                {
                    if (IsEmphasize) HOperatorSet.Emphasize(image, out image, EmphasizeW, EmphasizeH, Emphasizefactor);
                }
                catch (Exception) { }
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
                        HOperatorSet.SubImage(image, hObject1, out image, Sub_Mult, Sub_Add);
                    }
                }
                catch (Exception) { }
            }
            catch (Exception)
            {
            }
            return image;
        }

    }
}