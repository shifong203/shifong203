using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision2.Project.formula;

namespace Vision2.Project.Mes
{
    public partial class FvtForm1 : Form
    {
        public FvtForm1(int[] number, string[] SN, string[] rsetsTR)
        {
            InitializeComponent();
            mesJib = RecipeCompiler.Instance.GetMes() as MesJib;
            Sn = SN;
            RsetsTR = rsetsTR;
            Number = number;
        }

        private int[] Number;
        private string[] Sn;
        private string[] RsetsTR;
        private MesJib mesJib;

        private void FvtForm1_Load(object sender, EventArgs e)
        {
            try
            {
                groupBox1.Text = mesJib.MesData.FVTPath;
                groupBox1.Text = mesJib.MesData.FVTPath2;
                dataGridView1.Rows.Clear();
                dataGridView1.Rows.Add(Sn.Length);
                for (int i = 0; i < Sn.Length; i++)
                {
                    dataGridView1.Rows[i].Cells[0].Value = Number[i];
                    dataGridView1.Rows[i].Cells[1].Value = Sn[i];
                    dataGridView1.Rows[i].Cells[2].Value = RsetsTR[i];
                }
                UpdATS();
                int id = 0;
                Task task = new Task(() =>
                {
                    while (!this.IsDisposed)
                    {
                        try
                        {
                            if (mesJib.ReadFvt(Number, Sn, RsetsTR, out String[] FVTSTR))
                            {
                                label1.Text = "读取成功";
                                Thread.Sleep(1000);
                                this.Close();
                                return;
                            }
                            id++;
                            this.Invoke(new Action(() =>
                            {
                                label1.Text = "读取失败" + id;
                                if (this.IsDisposed)
                                {
                                }
                            }));
                            Thread.Sleep(200);
                        }
                        catch (Exception)
                        {
                            break;
                        }
                    }
                });
                task.Start();
            }
            catch (Exception ex)
            {
            }
        }

        private string[] fileS;
        private string[] fileS2;

        public void UpdATS()
        {
            try
            {
                listBox1.Items.Clear();
                listBox2.Items.Clear();
                fileS = Directory.GetFiles(mesJib.MesData.FVTPath, "*.txt");
                for (int i = 0; i < fileS.Length; i++)
                {
                    string snt = Path.GetFileNameWithoutExtension(fileS[i]);
                    listBox2.Items.Add(snt);
                }
                fileS2 = Directory.GetFiles(mesJib.MesData.FVTPath2, "*.txt");
                for (int i = 0; i < fileS2.Length; i++)
                {
                    string snt = Path.GetFileNameWithoutExtension(fileS2[i]);
                    listBox1.Items.Add(snt);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string[] strDatas = File.ReadAllLines(fileS[listBox2.SelectedIndex]);
                richTextBox1.Lines = strDatas;
            }
            catch (Exception)
            {
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string[] strDatas = File.ReadAllLines(fileS2[listBox1.SelectedIndex]);
                richTextBox1.Lines = strDatas;
            }
            catch (Exception)
            {
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (mesJib.ReadFvt(Number, Sn, RsetsTR, out String[] FVTSTR))
                {
                    MessageBox.Show("成功");
                }
                else
                {
                    MessageBox.Show("失败");
                }
            }
            catch (Exception)
            {
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (mesJib.WriteFatData(Number, Sn, RsetsTR))
                {
                    MessageBox.Show("成功");
                }
            }
            catch (Exception)
            {
            }
        }
    }
}