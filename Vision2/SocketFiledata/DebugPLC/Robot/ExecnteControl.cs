using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ErosSocket.DebugPLC.Robot
{
    public partial class ExecnteControl : UserControl
    {
        public ExecnteControl()
        {
            InitializeComponent();
        }

        public ExecnteControl(IAxisGrub axis) : this()
        {
            axisGrub = axis;
            richTextBox1.Text = axisGrub.Execnte_Code;
            if (DebugComp.GetThis().IsOffline_model)
            {
                tsButton5.BackColor = Color.Green;
            }
            else
            {
                tsButton5.BackColor = Color.Wheat;
            }
        }

        private IAxisGrub axisGrub;
        private IAxis AXIS;

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private int nextD = 0;
        private int lenhy;
        private bool buse;
        public System.Diagnostics.Stopwatch Watch { get; set; }

        private void tsButton4_Click(object sender, EventArgs e)
        {
            try
            {
                if (true)
                {
                    execnte_Program_Control1.XVale = 50;
                }
                //axisGrub.Execnte_Code = richTextBox1.Text;
                richTextBox1.SelectAll();
                richTextBox1.SelectionColor = Color.Black;
                richTextBox1.SelectionLength = 0;

                if (buse)
                {
                    richTextBox1.ForeColor = Color.Red;
                    buse = false;
                    return;
                }
                if (!DebugComp.GetThis().Single_step_mode)
                {
                    nextD = 0;
                    lenhy = 0;
                    axisGrub = null;
                }
                buse = true;
                Watch = new System.Diagnostics.Stopwatch();
                Watch.Start();
                Task.Run(() =>
                {
                    while (buse)
                    {
                        tsButton3.Text = (Watch.ElapsedMilliseconds / 1000.0f).ToString("0.00");
                    }
                });

                Task.Run(() =>
                 {
                     bool errr = false;

                     for (int i = nextD; i < richTextBox1.Lines.Length; i++)
                     {
                         //System.Threading.Thread.Sleep(10);
                         richTextBox1.SelectionStart = lenhy;
                         richTextBox1.SelectionLength = richTextBox1.Lines[i].Length + 1;
                         richTextBox1.SelectionColor = Color.Black;
                         string datas = richTextBox1.Lines[i].Trim(' ');
                         string[] items = datas.Split(' ');
                         if (items.Length == 1)
                         {
                             continue;
                         }
                         //if (DebugComp.GetThis().DicC154.ContainsKey(items[0]))
                         //{
                         //    //axisGrub = DebugComp.GetThis().DicC154[items[0]];
                         //}
                         //else
                         //{
                         if (DIDO.C154Axis.KeyValuePairs.ContainsKey(items[0]))
                         {
                             AXIS = DIDO.C154Axis.KeyValuePairs[items[0]];
                         }
                         //}
                         if (axisGrub == null)
                         {
                             string cmd = "";
                             string data = "";
                             if (AXIS != null && items.Length >= 2)
                             {
                                 if (items.Length == 3)
                                 {
                                     cmd = items[1].ToLower().ToLower();
                                     data = items[2].ToLower().ToLower();
                                 }
                                 else if (items.Length == 2)
                                 {
                                     cmd = items[0].ToLower().ToLower();
                                     data = items[1].ToLower().ToLower();
                                 }

                                 if (cmd == ("move"))
                                 {
                                 }
                                 else if (cmd == ("go"))
                                 {
                                     if (Single.TryParse(data, out Single POS))
                                     {
                                         if (DebugComp.GetThis().IsOffline_model)
                                         {
                                             if (AXIS.AxisType == EnumAxisType.X)
                                             {
                                                 execnte_Program_Control1.SetXValue(POS);
                                             }
                                             else if (AXIS.AxisType == EnumAxisType.Y)
                                             {
                                                 execnte_Program_Control1.SetYValue(POS);
                                             }
                                             richTextBox1.SelectionColor = Color.GreenYellow;
                                         }
                                         else if (AXIS.SetPoint(POS))
                                         {
                                             richTextBox1.SelectionColor = Color.GreenYellow;
                                         }
                                         else
                                         {
                                             errr = true;
                                         }
                                     }
                                     else
                                     {
                                         errr = true;
                                     }
                                     //
                                 }
                                 else if (cmd == ("speed"))
                                 {
                                     items = datas.Split(' ');
                                     if (Single.TryParse(items[2], out Single POS))
                                     {
                                         if (DebugComp.GetThis().IsOffline_model)
                                         {
                                             if (AXIS.AxisType == EnumAxisType.X)
                                             {
                                                 execnte_Program_Control1.XVale = POS;
                                             }
                                             else if (AXIS.AxisType == EnumAxisType.Y)
                                             {
                                                 execnte_Program_Control1.YVale = POS;
                                             }
                                             richTextBox1.SelectionColor = Color.GreenYellow;
                                         }
                                         else if (AXIS.SetPoint(POS))
                                         {
                                             richTextBox1.SelectionColor = Color.GreenYellow;
                                         }
                                         else
                                         {
                                             errr = true;
                                         }
                                     }
                                     else
                                     {
                                         errr = true;
                                     }
                                 }
                             }
                             else
                             {
                                 errr = true;
                             }
                         }
                         else
                         {
                             if (datas.ToLower().StartsWith("move"))
                             {
                                 items = datas.Split(' ');
                                 Single? x = null;
                                 Single? y = null;
                                 Single? z = null;
                                 Single? u = null;
                                 for (int ite = 0; ite < items.Length; ite++)
                                 {
                                     if (items[ite].ToUpper().StartsWith("X"))
                                     {
                                         x = Convert.ToSingle(items[ite].Remove(0, 1));
                                     }
                                     else if (items[ite].ToUpper().StartsWith("Y"))
                                     {
                                         y = Convert.ToSingle(items[ite].Remove(0, 1));
                                     }
                                     else if (items[ite].ToUpper().StartsWith("Z"))
                                     {
                                         z = Convert.ToSingle(items[ite].Remove(0, 1));
                                     }
                                     else if (items[ite].ToUpper().StartsWith("U"))
                                     {
                                         u = Convert.ToSingle(items[ite].Remove(0, 1));
                                     }
                                 }

                                 if (axisGrub.SetPoint("Move", x, y, z, u))
                                 {
                                     richTextBox1.SelectionColor = Color.GreenYellow;
                                 }
                                 else
                                 {
                                     errr = true;
                                 }
                             }
                             else if (datas.ToLower().StartsWith("go"))
                             {
                                 items = datas.Split(' ');
                                 Single? x = null;
                                 Single? y = null;
                                 Single? z = null;
                                 Single? u = null;

                                 for (int ite = 0; ite < items.Length; ite++)
                                 {
                                     if (items[ite].ToUpper().StartsWith("X"))
                                     {
                                         x = Convert.ToSingle(items[ite].Remove(0, 1));
                                     }
                                     else if (items[ite].ToUpper().StartsWith("Y"))
                                     {
                                         y = Convert.ToSingle(items[ite].Remove(0, 1));
                                     }
                                     else if (items[ite].ToUpper().StartsWith("Z"))
                                     {
                                         z = Convert.ToSingle(items[ite].Remove(0, 1));
                                     }
                                     else if (items[ite].ToUpper().StartsWith("U"))
                                     {
                                         u = Convert.ToSingle(items[ite].Remove(0, 1));
                                     }
                                     else if (items[ite].Contains("="))
                                     {
                                         string[] datase = items[ite].Split('=');
                                         if (datase.Length == 2)
                                         {
                                             this.axisGrub.SetIOOut(datase[0], datase[1]);
                                         }
                                     }
                                 }
                                 if (axisGrub.SetPoint("go", x, y, z, u))
                                 {
                                     richTextBox1.SelectionColor = Color.GreenYellow;
                                 }
                                 else
                                 {
                                     errr = true;
                                 }
                                 //
                             }
                             else if (datas.Contains("="))
                             {
                                 string[] datase = datas.Split('=');
                                 if (datase.Length == 2)
                                 {
                                     this.axisGrub.SetIOOut(datase[0], datase[1]);
                                 }
                             }
                         }

                         if (errr)
                         {
                             richTextBox1.SelectionStart = lenhy;
                             richTextBox1.SelectionLength = richTextBox1.Lines[i].Length + 1;
                             richTextBox1.SelectionColor = Color.Red;
                             buse = false;
                             return;
                         }
                         lenhy += richTextBox1.Lines[i].Length + 1;

                         if (DebugComp.GetThis().Single_step_mode)
                         {
                             nextD = i + 1;
                             return;
                         }
                     }
                     nextD = 0;
                     lenhy = 0;
                     buse = false;
                     richTextBox1.SelectionColor = Color.Black;
                 });
            }
            catch (Exception)
            {
                buse = false;
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void tsButton5_Click(object sender, EventArgs e)
        {
            if (DebugComp.GetThis().IsOffline_model)
            {
                DebugComp.GetThis().IsOffline_model = false;
                tsButton5.BackColor = Color.Wheat;
            }
            else
            {
                DebugComp.GetThis().IsOffline_model = true;
                tsButton5.BackColor = Color.Green;
            }
        }

        private void tsButton2_Click(object sender, EventArgs e)
        {
            DebugComp.GetThis().Stop = true;
        }

        private void tsButton3_Click(object sender, EventArgs e)
        {
        }

        private void tsButton1_Click(object sender, EventArgs e)
        {
            if (!DebugComp.GetThis().Single_step_mode)
            {
                DebugComp.GetThis().Single_step_mode = true;
                tsButton1.BackColor = Color.Green;
            }
            else
            {
                DebugComp.GetThis().Single_step_mode = false;
                tsButton1.BackColor = Color.Wheat;
            }
        }
    }
}