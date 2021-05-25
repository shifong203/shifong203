using NokidaE.vision;
using System.Windows.Forms;

namespace NokidaE.Project.formula
{
    public partial class ReconvictionForm : Form
    {
        public ReconvictionForm()
        {
            InitializeComponent();

        }
        public static ReconvictionForm Form
        {
            get
            {
                if (Reconviction == null || Reconviction.IsDisposed)
                {
                    Reconviction = new ReconvictionForm();
                }
                return Reconviction;
            }
            set { Reconviction = value; }

        }
        static ReconvictionForm Reconviction;

        public void ShowData(ResultOBj resultOBj)
        {
            this.WindowState = FormWindowState.Maximized;
            this.Show();
            bool isct = true;
            foreach (var item in resultOBj.keyValuePairs.Keys)
            {
                if (groupBox2.Controls.ContainsKey(item))
                {
                    Control[] controls = groupBox2.Controls.Find(item, false);
                    GroupBox groupBox = controls[0] as GroupBox;
                    if (groupBox != null)
                    {
                    }
                }
                else
                {

                    HalconDotNet.HWindowControl hWindowControl = new HalconDotNet.HWindowControl();
                    hWindowControl.Dock = DockStyle.Top;

                    GroupBox groupBox = new GroupBox();
                    groupBox.Name = groupBox.Text = item;
                    groupBox2.Controls.Add(groupBox);
                    groupBox.Dock = DockStyle.Left;
                    groupBox.Controls.Add(hWindowControl);

                }
                if (isct)
                {
                    isct = false;
                    visionUserC1.hWindwC = new vision.HalconRunFile.Controls.VisionUserC.HWindwC();
                    visionUserC1.hWindwC.hWindowHalconID = visionUserC1.HalconWindow;
                    visionUserC1.hWindwC.Image(resultOBj.keyValuePairs[item][0].Image);
                    visionUserC1.UpHalcon();

                }
            }




        }

    }
}
