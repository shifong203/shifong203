using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using Vision2.ConClass;

namespace Vision2.vision.HalconRunFile.PCBFile
{
    public partial class PCBLibraryForm : Form
    {
        public PCBLibraryForm()
        {
            InitializeComponent();
            HWindID.Initialize(hWindowControl1);
            ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView1);
        }

        private HWindID HWindID = new HWindID();

        public PCBLibraryForm(PCBAEX pCBAEX) : this()
        {
            PCBAEX = pCBAEX;
        }

        private PCBAEX PCBAEX;

        public void UPdata()
        {
            try
            {
                //HWindID.WhidowAdd = true;
                propertyGrid1.SelectedObject = PCBAEX.GetPThis().DXFInFoc;
                HWindID.HobjClear();
                dataGridView1.Rows.Clear();
                treeView1.Nodes.Clear();
                foreach (var item in PCBAEX.DictRoi)
                {
                    TreeNode treeNode = treeView1.Nodes.Add(item.Key);
                    treeNode.Tag = item.Value;
                    //HOperatorSet.AffineTransPixel(PCBAEX.GetPThis().DXFInFoc.SetDXF(), item.Value.Row, item.Value.Col,
                    //    out HTuple rowTrans, out HTuple colTrans);
                    HOperatorSet.GenRectangle2(out HObject hObject, item.Value.Row, item.Value.Col,
                       new HTuple(item.Value.Angle).TupleRad(), item.Value.Length1, item.Value.Length2);
                    HWindID.OneResIamge.AddNameOBJ(item.Key, hObject);
                    int det = dataGridView1.Rows.Add();
                    dataGridView1.Rows[det].Cells[0].Value = item.Value.Name;
                    dataGridView1.Rows[det].Cells[1].Value = item.Value.Row;
                    dataGridView1.Rows[det].Cells[2].Value = item.Value.Col;
                    dataGridView1.Rows[det].Cells[3].Value = item.Value.Angle;
                    dataGridView1.Rows[det].Cells[4].Value = item.Value.ToolDone;
                    dataGridView1.Rows[det].Cells[5].Value = item.Value.LibraryName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public class DXFInFo
        {
            [DescriptionAttribute("。"), Category("导入索引"), DisplayName("名称索引")]
            public int NameIndex { get; set; } = 0;

            [DescriptionAttribute("-1不使用。"), Category("导入索引"), DisplayName("库索引")]
            public int LiNameIndex { get; set; } = -1;

            [DescriptionAttribute("-1不使用。"), Category("导入索引"), DisplayName("Row索引")]
            public int RowIndex { get; set; } = 1;

            [DescriptionAttribute("-1不使用。"), Category("导入索引"), DisplayName("Col索引")]
            public int ColIndex { get; set; } = 2;

            [DescriptionAttribute("-1不使用。"), Category("导入索引"), DisplayName("角度索引")]
            public int ApiIndex { get; set; } = 3;

            [DescriptionAttribute("-1不使用。"), Category("导入索引"), DisplayName("上下面索引")]
            public int TurnUpsideDownIndex { get; set; } = 4;

            [DescriptionAttribute(""), Category("原点参数"), DisplayName("坐标旋转")]
            public double Api { get; set; } = 0;

            [DescriptionAttribute(""), Category("原点参数"), DisplayName("原点Row")]
            public double Row { get; set; } = 0;

            [DescriptionAttribute(""), Category("原点参数"), DisplayName("原点Col")]
            public double Col { get; set; } = 0;

            [DescriptionAttribute(""), Category("原点参数"), DisplayName("X比例")]
            public double ScaleX { get; set; } = 1;

            [DescriptionAttribute(""), Category("原点参数"), DisplayName("Y比例")]
            public double ScaleY { get; set; } = 1;

            public HObject DXF { get; set; }
            public HObject DXFMode { get; set; }

            public HObject ReadDxf()
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "请选择dxf文件";
                openFileDialog.Multiselect = false;
                openFileDialog.Filter = "dxf文件|*.dxf;";
                openFileDialog.ShowDialog();
                if (openFileDialog.FileName.Length == 0) return DXF;
                try
                {
                    HOperatorSet.ReadContourXldDxf(out HObject hObject, openFileDialog.FileNames[0], new HTuple(), new HTuple(), out HTuple dxfStratus);
                    DXFMode = hObject;
                    HOperatorSet.AffineTransContourXld(hObject, out hObject, SetDXF());
                    DXF = hObject;
                    //return SetDXF();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
                return DXF;
            }

            public HTuple SetDXF()
            {
                HOperatorSet.HomMat2dIdentity(out HTuple HomMat2DIdentity);
                try
                {
                    HOperatorSet.HomMat2dRotate(HomMat2DIdentity, new HTuple(Api).TupleRad(), 0, 0, out HomMat2DIdentity);
                    HOperatorSet.HomMat2dScale(HomMat2DIdentity, ScaleX, ScaleY, 0, 0, out HomMat2DIdentity);
                    HOperatorSet.HomMat2dTranslate(HomMat2DIdentity, Row, Col, out HomMat2DIdentity);
                    //HOperatorSet.AffineTransContourXld(DXFMode, out HObject hObject, HomMat2DIdentity);
                    //DXF = hObject;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
                return HomMat2DIdentity;
            }
        }

        private void PCBLibraryForm_Load(object sender, EventArgs e)
        {
            try
            {
                HOperatorSet.HomMat2dIdentity(out Home2D);
                if (System.IO.File.Exists(Vision.VisionPath + "Image\\" + PCBAEX.GetPThis().Name + "拼图.jpg"))
                {
                    HOperatorSet.ReadImage(out HObject imaget, Vision.VisionPath + "Image\\" + PCBAEX.GetPThis().Name + "拼图.jpg");
                    HWindID.SetImaage(imaget);
                }
                else
                {
                    MessageBox.Show("未创建大图:" + PCBAEX.GetPThis().Name + "拼图.jpg");
                }

                UPdata();
            }
            catch (Exception)
            {
            }
        }

        private Library.LibraryVisionBase libraryVisionBase;

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (libraryVisionBase != null)
                {
                    this.panel1.Controls.Clear();
                    HWindID.OneResIamge.ClearImageMassage();
                    HOperatorSet.AffineTransPixel(PCBAEX.GetPThis().DXFInFoc.SetDXF(), libraryVisionBase.Row, libraryVisionBase.Col,
                  out HTuple rowTrans, out HTuple colTrans);
                    HOperatorSet.GenRectangle2(out HObject hObject, rowTrans, colTrans,
                       new HTuple(libraryVisionBase.Angle).TupleRad(), libraryVisionBase.Length1, libraryVisionBase.Length2);
                    HWindID.OneResIamge.AddNameOBJ(libraryVisionBase.Name, hObject);
                }
                libraryVisionBase = e.Node.Tag as Library.LibraryVisionBase;
                if (libraryVisionBase != null)
                {
                    HOperatorSet.AffineTransPixel(PCBAEX.GetPThis().DXFInFoc.SetDXF(), libraryVisionBase.Row, libraryVisionBase.Col,
                      out HTuple rowTrans, out HTuple colTrans);
                    HOperatorSet.GenRectangle2(out HObject hObject, rowTrans, colTrans,
                       new HTuple(libraryVisionBase.Angle).TupleRad(), libraryVisionBase.Length1, libraryVisionBase.Length2);
                    HWindID.OneResIamge.AddNameOBJ(libraryVisionBase.Name, hObject, ColorResult.red);
                    HWindID.OneResIamge.AddImageMassage(rowTrans, colTrans, libraryVisionBase.Name);
                    HWindID.SetPart(rowTrans - 500, colTrans - 500, rowTrans + 500, colTrans + 500);
                    propertyGrid2.SelectedObject = libraryVisionBase;
                    libraryVisionBase.Run(HWindID);
                    this.panel1.Controls.Add(libraryVisionBase.GetControl());
                }
                HWindID.ShowObj();
            }
            catch (Exception)
            {
            }
        }

        private void 导入图像ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string path = Vision.VisionPath + "Image\\";
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "请选择图片文件可多选";
                openFileDialog.Multiselect = true;
                openFileDialog.Filter = "图片文件|*.jpg;*.tif;*.tiff;*.gif;*.bmp;*.jpg;*.jpeg;*.jp2;*.png;*.pcx;*.pgm;*.ppm;*.pbm;*.xwd;*.ima;*.hobj";
                openFileDialog.InitialDirectory = path;
                //DialogResult dialogResult= openFileDialog.ShowDialog();
                if (openFileDialog.ShowDialog() != DialogResult.OK) return;
                path = openFileDialog.FileName;
                //+ halconRun.Name + (dataGridView1.SelectedRows[0].Index + 1) + ".bmp";
                if (System.IO.File.Exists(path))
                {
                    HOperatorSet.ReadImage(out HObject hObject, path);
                    HWindID.SetImaage(hObject);
                    HOperatorSet.GetImageSize(hObject, out width, out height);
                    HWindID.ShowImage();
                }
            }
            catch (Exception)
            { }
        }

        private HTuple Home2D;
        private HTuple width;
        private HTuple height;

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.InitialDirectory = ProjectINI.ProjectPathRun ;
            try
            {//"文本文件|*txt.*|C#文件|*.cs|所有文件|*.*";
                openFileDialog.Filter = "Excel文件|*.xls;*.xlsx;*.txt;";
                DialogResult dialog = openFileDialog.ShowDialog();
                if (dialog == DialogResult.OK)
                {
                    if (System.IO.Path.GetExtension(openFileDialog.FileName) == ".txt")
                    {
                        Npoi.ReadText(openFileDialog.FileName, out List<string> text);
                        dataGridView1.Rows.Clear();
                        foreach (var item in text)
                        {
                            string[] ItemArray = System.Text.RegularExpressions.Regex.Split(item, @"\s+");
                            int det = dataGridView1.Rows.Add();
                            if (ItemArray.Length >= 3)
                            {
                                dataGridView1.Rows[det].Cells[0].Value = ItemArray[PCBAEX.GetPThis().DXFInFoc.NameIndex];
                                dataGridView1.Rows[det].Cells[1].Value = ItemArray[PCBAEX.GetPThis().DXFInFoc.RowIndex];
                                dataGridView1.Rows[det].Cells[2].Value = ItemArray[PCBAEX.GetPThis().DXFInFoc.ColIndex];
                                dataGridView1.Rows[det].Cells[3].Value = ItemArray[PCBAEX.GetPThis().DXFInFoc.ApiIndex];
                                dataGridView1.Rows[det].Cells[4].Value = ItemArray[PCBAEX.GetPThis().DXFInFoc.TurnUpsideDownIndex];
                            }
                            if (ItemArray.Length >= 4)
                            {
                                dataGridView1.Rows[det].Cells[5].Value = ItemArray[5];
                                dataGridView1.Rows[det].Cells[6].Value = ItemArray[6];
                            }
                        }
                    }
                    else
                    {
                        DataTable dataTable2 = Npoi.ReadExcelFile(openFileDialog.FileName, 0);
                        if (dataTable2 == null)
                        {
                            MessageBox.Show("参数信息不存在;" + Environment.NewLine);
                        }
                        else
                        {
                            dataGridView1.Rows.Clear();
                            //dataGridView1.Columns.Clear();
                            foreach (var item in dataTable2.Columns)
                            {
                            }
                            foreach (DataRow item1 in dataTable2.Rows)
                            {
                                Library.LibraryVisionBase libraryVisionBase = new Library.LibraryVisionBase();
                                libraryVisionBase.Name = item1.ItemArray[PCBAEX.GetPThis().DXFInFoc.NameIndex].ToString();
                                int det = dataGridView1.Rows.Add();
                                dataGridView1.Rows[det].Cells[0].Value = libraryVisionBase.Name;
                                dataGridView1.Rows[det].Cells[1].Value = item1.ItemArray[PCBAEX.GetPThis().DXFInFoc.RowIndex];
                                dataGridView1.Rows[det].Cells[2].Value = item1.ItemArray[PCBAEX.GetPThis().DXFInFoc.ColIndex];
                                dataGridView1.Rows[det].Cells[3].Value = item1.ItemArray[PCBAEX.GetPThis().DXFInFoc.ApiIndex];
                                dataGridView1.Rows[det].Cells[4].Value = item1.ItemArray[4];
                                dataGridView1.Rows[det].Cells[5].Value = item1.ItemArray[5];
                                dataGridView1.Rows[det].Cells[6].Value = item1.ItemArray[6];
                                libraryVisionBase.Row = double.Parse(item1.ItemArray[PCBAEX.GetPThis().DXFInFoc.RowIndex].ToString());
                                libraryVisionBase.Col = double.Parse(item1.ItemArray[PCBAEX.GetPThis().DXFInFoc.ColIndex].ToString());
                                libraryVisionBase.Angle = double.Parse(item1.ItemArray[PCBAEX.GetPThis().DXFInFoc.ApiIndex].ToString());
                                if (!PCBAEX.DictRoi.ContainsKey(libraryVisionBase.Name))
                                {
                                    PCBAEX.DictRoi.Add(libraryVisionBase.Name, libraryVisionBase);
                                }
                            }
                            UPdata();
                            MessageBox.Show("导入成功");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("导入失败:" + ex.Message);
            }
        }

        private void PCBLibraryForm_Resize(object sender, EventArgs e)
        {
            try
            {
                HTuple sets = width.D / height.D;
                hWindowControl1.Height = (int)((double)hWindowControl1.Width / sets.D);
            }
            catch (Exception ex)
            {
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                propertyGrid1.SelectedObject = PCBAEX.GetPThis().DXFInFoc;
                HWindID.HobjClear();
                treeView1.Nodes.Clear();
                foreach (var item in PCBAEX.DictRoi)
                {
                    TreeNode treeNode = treeView1.Nodes.Add(item.Key);
                    treeNode.Tag = item.Value;
                    HOperatorSet.AffineTransPixel(PCBAEX.GetPThis().DXFInFoc.SetDXF(), item.Value.Row, item.Value.Col,
                        out HTuple rowTrans, out HTuple colTrans);
                    HOperatorSet.GenRectangle2(out HObject hObject, rowTrans, colTrans,
                       new HTuple(item.Value.Angle).TupleRad(), item.Value.Length1, item.Value.Length2);
                    HWindID.OneResIamge.AddNameOBJ(item.Key, hObject);
                }
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            try
            {
                RunProgramFile.RunProgram.DrawPoint(HWindID, out HTuple row, out HTuple col);
                PCBAEX.GetPThis().DXFInFoc.Row = row;
                PCBAEX.GetPThis().DXFInFoc.Col = col;
                UPdata();
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView1.Rows.Clear();

                dataGridView1.Rows.Add(HWindID.OneResIamge.GetKeyHobj().Count);
                int det = 0;
                foreach (var item in HWindID.OneResIamge.GetKeyHobj())
                {
                    HOperatorSet.AreaCenter(item.Value.Object, out HTuple area, out HTuple row, out HTuple column);
                    if (PCBAEX.DictRoi.ContainsKey(item.Key))
                    {
                        PCBAEX.DictRoi[item.Key].Row = row;
                        PCBAEX.DictRoi[item.Key].Col = column;
                    }
                    dataGridView1.Rows[det].Cells[0].Value = item.Key;
                    dataGridView1.Rows[det].Cells[1].Value = row;
                    dataGridView1.Rows[det].Cells[2].Value = column;
                    dataGridView1.Rows[det].Cells[3].Value = PCBAEX.DictRoi[item.Key].Angle;
                    dataGridView1.Rows[det].Cells[4].Value = PCBAEX.DictRoi[item.Key].ToolDone;

                    dataGridView1.Rows[det].Cells[5].Value = PCBAEX.DictRoi[item.Key].LibraryName;
                    det++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    Library.LibraryVisionBase libraryVisionBase;
                    if (!PCBAEX.DictRoi.ContainsKey(dataGridView1.Rows[i].Cells[0].Value.ToString()))
                    {
                        libraryVisionBase = new Library.LibraryVisionBase();
                        libraryVisionBase.Name = dataGridView1.Rows[i].Cells[0].Value.ToString();
                        PCBAEX.DictRoi.Add(libraryVisionBase.Name, libraryVisionBase);
                    }
                    libraryVisionBase = PCBAEX.DictRoi[dataGridView1.Rows[i].Cells[0].Value.ToString()];
                    libraryVisionBase.Name = dataGridView1.Rows[i].Cells[0].Value.ToString();
                    libraryVisionBase.Row = double.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString());
                    libraryVisionBase.Col = double.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString());
                    libraryVisionBase.Angle = double.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString());
                    libraryVisionBase.ToolDone = dataGridView1.Rows[i].Cells[4].Value.ToString();
                }
                UPdata();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog openFileDialog = new SaveFileDialog();
                openFileDialog.Filter = "Exlce文件|*.xls;*.xlsx";

                openFileDialog.FileName = Project.formula.Product.ProductionName + ".xls";
                DialogResult dialogResult = openFileDialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    ErosProjcetDLL.Excel.Npoi.DataGridViewExportExcel(openFileDialog.FileName, Project.formula.Product.ProductionName, dataGridView1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}