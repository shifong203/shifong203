using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using NokidaE.vision.HalconRunFile.RunProgramFile;
namespace NokidaE.vision.HalconRunFile.Controls
{
    public partial class ConRunProgram : UserControl
    {
        public ConRunProgram()
        {
            InitializeComponent();
            //dataGridView2.CellValueChanged += dataGridView1_CellValueChanged;
       //     ErosProjcetDLL.MainForm1.MainFormF.Controls.Add(PropertyForm);
            this.Controls.Add(PropertyForm);
            PropertyForm.Dock = DockStyle.Right;
            PropertyForm.Show();
        }
        bool isEnb;


        private HalconRun Halcon = new HalconRun();

        public ConRunProgram(HalconRun halcon):this()
        {
           
            Halcon = halcon;
        }

        private void UpData()
        {
            try
            {
                dataGridViewHalcon.Rows.Clear();
                var detee = from objDic in Halcon.GetRunProgram()
                            orderby objDic.Value.CDID ascending
                            select objDic;
                int i = 0;
                i = dataGridViewHalcon.Rows.Add();
                dataGridViewHalcon.Rows[i].Cells[1].Value = Halcon.GetType().Name;
                dataGridViewHalcon.Rows[i].Cells[2].Value = Halcon.GetType().Name;
                dataGridViewHalcon.Rows[i].Cells[3].Value = "单次执行";
                dataGridViewHalcon.Rows[i].Cells[4].Value = Halcon.RunTimeI;
                //tsbRunList.Items.Clear();
                //tsbRunList.Items.Add(Halcon.GetType().Name);
                //tsbRunList.SelectedIndex = 0;
                foreach (var item in detee)
                {
                    i = dataGridViewHalcon.Rows.Add();
                    dataGridViewHalcon.Rows[i].Cells[0].Value = item.Value.CDID;
                    dataGridViewHalcon.Rows[i].Cells[1].Value = item.Key;
                    dataGridViewHalcon.Rows[i].Cells[2].Value = item.Value.GetType().Name;
                    dataGridViewHalcon.Rows[i].Cells[3].Value = "单次执行";
                    dataGridViewHalcon.Rows[i].Cells[4].Value = item.Value.Watch.ElapsedMilliseconds;
                    i++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        ErosProjcetDLL.Project.PropertyForm PropertyForm = new ErosProjcetDLL.Project.PropertyForm();
        private void ConRunProgram_Load(object sender, EventArgs e)
        {
            UpData();
        }

        private void toolStripTextBox2_Click(object sender, EventArgs e)
        {
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void 保存所有ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void dataGridView1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                isEnb = true;
                DataGridView.HitTestInfo hit = dataGridViewHalcon.HitTest(e.X, e.Y);
                if (hit.ColumnIndex!=3)
                {   
                  if (dataGridViewHalcon.SelectedRows[0] != null || dataGridViewHalcon.SelectedRows[0].Cells[0].Value.ToString() != "")
                {
                
                    if (!Halcon.GetRunProgram().ContainsKey(dataGridViewHalcon.SelectedRows[0].Cells[1].Value.ToString()))
                    {
                            PropertyForm.UProperty(Halcon, null);
                        return;
                    }
          
                    if (Halcon.GetRunProgram()[dataGridViewHalcon.SelectedRows[0].Cells[1].Value.ToString()] == null)
                    {
                        return;
                    }
                        PropertyForm.UProperty(Halcon.GetRunProgram()[dataGridViewHalcon.SelectedRows[0].Cells[1].Value.ToString()],Halcon);
                    }
                }

                if (hit.Type == DataGridViewHitTestType.Cell &&hit.ColumnIndex==3 )
                {
                    DataGridViewCell clickedCell =
                        dataGridViewHalcon.Rows[hit.RowIndex].Cells[hit.ColumnIndex];
                    if (hit.RowIndex==0)
                    {
                        Halcon.ShowVision(0,999);
                    }
                    else if (hit.RowIndex>0)
                    {
                        Halcon.ShowVision(dataGridViewHalcon.Rows[hit.RowIndex].Cells[1].Value.ToString());
                    }

                }



            }
            catch (Exception ex)
            {
            }
            isEnb = false;
        }

        string path = "";
        private void btnSaveAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (Halcon == null)
                {
                    MessageBox.Show("未关联执行程序");
                    return;
                }
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "请选择图片文件可多选";
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = "图片文件|*.jpg;*.tif;*.tiff;*.gif;*.bmp;*.jpg;*.jpeg;*.jp2;*.png;*.pcx;*.pgm;*.ppm;*.pbm;*.xwd;*.ima;*.hobj";
                if (!path.StartsWith(Vision.Instance.DicSaveType[Halcon.Name].SavePath))
                {
                    openFileDialog.InitialDirectory = Vision.Instance.DicSaveType[Halcon.Name].SavePath;
                }
                else
                {
                    openFileDialog.InitialDirectory = System.IO. Path.GetDirectoryName(path);
                    openFileDialog.FileName =  System.IO.Path.GetFileName(path);
                }

                //DialogResult dialogResult= openFileDialog.ShowDialog();
                if (openFileDialog.ShowDialog() != DialogResult.OK) return;
                path = openFileDialog.FileName;
                Halcon.ReadImage(path);
                Halcon.ShowImage();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void btnRedraw_Click(object sender, EventArgs e)
        {
            UpData();
        }

        private void 自定义CToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void 选项OToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void tsbRunList_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    groupBox1.Text = Halcon.GetType().Name;
            //    dataGridView2.Rows.Clear();
            //    int i = 0;
            //    //Halcon.GetRunProgram()[dataGridView1.SelectedRows[0].Cells[0].Value.ToString()].KayHTuple.DirectoryHTuple
            //    dataGridView2.Rows.Add();
            //    foreach (var item in Halcon.DirectoryHTup)
            //    {
            //        dataGridView2.Rows[i].Cells[0].Value = item.Key;
            //        dataGridView2.Rows[i].Cells[1].Value = item.Value.DirectoryHTuple;
            //        i++;
            //        dataGridView2.Rows.AddCopy(dataGridView2.Rows.Count - 1);
            //    }
            //}
            //catch (Exception ex)
            //{
            //}
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void tsButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Halcon.GetCam().Run(Halcon);
            }
            catch (Exception)
            {
            }
        }

        private void tsButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (tsButton2.Text=="实时采图")
                {
                    Halcon.GetCam().Key = "One";
                    Halcon.GetCam().ThreadSatring(Halcon);
                }
                else
                {
                    Halcon.GetCam().Stop();
                }
            }
            catch (Exception)
            {
            }
        }
    }
}