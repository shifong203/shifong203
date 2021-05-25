using NokidaE.vision;
using System.Windows.Forms;

namespace NokidaE.Project.formula
{
    public partial class ReconvictionOKForm : Form
    {
        public ReconvictionOKForm()
        {
            InitializeComponent();
            hWindID.Initialize(hWindowControl1);
        }
        HWindID hWindID = new HWindID();
        public static ReconvictionOKForm Form
        {
            get
            {
                if (Reconviction == null || Reconviction.IsDisposed)
                {
                    Reconviction = new ReconvictionOKForm();
                }
                return Reconviction;
            }
            set { Reconviction = value; }

        }
        static ReconvictionOKForm Reconviction;

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
                    hWindID.SetImaage(resultOBj.keyValuePairs[item][0].Image);
                    //visionUserC1.UpHalcon();

                }
            }
        }

        public void ShowData(ResetData DATA)
        {
            this.WindowState = FormWindowState.Maximized;
            groupBox2.Controls.Clear();

            for (int i = 0; i < DATA.DataValue.Count; i++)
            {
                if (DATA.DataValue[i].Image!=null)
                {
                    HalconDotNet.HWindowControl hWindowControl = new HalconDotNet.HWindowControl();
                    hWindowControl.Dock = DockStyle.Left;
                    groupBox2.Controls.Add(hWindowControl);
                    HWindID.DispImage(hWindowControl.HalconWindow, DATA.DataValue[i].Image);
                }
            }
            this.Show();
        }


    }
}
