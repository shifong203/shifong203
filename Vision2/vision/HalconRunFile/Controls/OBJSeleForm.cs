using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using HslCommunication.BasicFramework;

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class OBJSeleForm : Form
    {
        public OBJSeleForm()
        {
            InitializeComponent();
            ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView1);
            HObjectErr.GenEmptyObj();
        }
        HWindID HWind = new HWindID();

        public void ShowImage(OneResultOBj oneResultOBj)
        {
            //HWindID.OneResIamge = oneResultOBj;
            HWind.SetImaage(oneResultOBj.Image);
        }

        public HObject HObjectErr = new HObject();
        HTuple index = new HTuple();
        HTuple rows = new HTuple();
        HTuple cols = new HTuple();

        public void AddErrObj(HObject hObject)
        {
            HObjectErr = hObject;
            HWind.AddObj(HObjectErr);
            try
            {
                HOperatorSet.AreaCenter(hObject, out HTuple area, out  rows, out  cols);
                HOperatorSet.SmallestRectangle1(hObject, out HTuple row1, out HTuple column1, out HTuple row2, out HTuple col2);
                HOperatorSet.SmallestRectangle2(hObject, out rows, out cols, out HTuple phi, out HTuple length1, out HTuple length2);
                HOperatorSet.Circularity(hObject, out HTuple circularity);
                HOperatorSet.HeightWidthRatio(hObject, out HTuple height, out HTuple width, out HTuple ratio);
                HOperatorSet.Compactness(hObject, out HTuple compatness);
                HOperatorSet.Contlength(hObject, out HTuple contLength);
                HOperatorSet.Rectangularity(hObject, out HTuple rectangularity);
                HOperatorSet.SmallestCircle(hObject, out HTuple rowC, out HTuple coluC, out HTuple radius);
                HOperatorSet.InnerCircle(hObject, out rowC, out coluC, out HTuple InnerRadius);
                HOperatorSet.Roundness(hObject, out HTuple distance, out HTuple sigma, out HTuple roundness, out HTuple sides);
                HOperatorSet.InnerRectangle1(hObject, out HTuple inRow1, out HTuple inCol1, out HTuple inRow2, out HTuple inCol2);
                HOperatorSet.ConnectAndHoles(hObject, out HTuple numConnected, out HTuple numHoles);

                //Dictionary<String, Student> students = new Dictionary<String, Student>();
                dataGridView1.DataSource = null;
                //dataGridView1.AutoGenerateColumns = false;
                if (area.Length != 0)
                {
                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("面积", Type.GetType("System.String"));
                    dataTable.Columns.Add("宽", Type.GetType("System.String"));
                    dataTable.Columns.Add("高", Type.GetType("System.String"));
                    dataTable.Columns.Add("宽高比", Type.GetType("System.String"));
                    dataTable.Columns.Add("轮廓长度", Type.GetType("System.String"));
                    dataTable.Columns.Add("row", Type.GetType("System.String"));
                    dataTable.Columns.Add("col", Type.GetType("System.String"));
                    dataTable.Columns.Add("圆度", Type.GetType("System.String"));
                    dataTable.Columns.Add("length1", Type.GetType("System.String"));
                    dataTable.Columns.Add("length2", Type.GetType("System.String"));
                    dataTable.Columns.Add("phi", Type.GetType("System.String"));
                    dataTable.Columns.Add("紧密", Type.GetType("System.String"));
                    dataTable.Columns.Add("矩形度", Type.GetType("System.String"));
                    dataTable.Columns.Add("外圆半径", Type.GetType("System.String"));
                    dataTable.Columns.Add("内圆半径", Type.GetType("System.String"));
                    dataTable.Columns.Add("中心平均距离", Type.GetType("System.String"));
                    dataTable.Columns.Add("距离的标准差", Type.GetType("System.String"));
                    dataTable.Columns.Add("园度因子", Type.GetType("System.String"));
                    dataTable.Columns.Add("多边形边数", Type.GetType("System.String"));
                    dataTable.Columns.Add("row1", Type.GetType("System.String"));
                    dataTable.Columns.Add("col1", Type.GetType("System.String"));
                    dataTable.Columns.Add("row2", Type.GetType("System.String"));
                    dataTable.Columns.Add("col2", Type.GetType("System.String"));
                    dataTable.Columns.Add("inRow1", Type.GetType("System.String"));
                    dataTable.Columns.Add("inCol1", Type.GetType("System.String"));
                    dataTable.Columns.Add("inRow2", Type.GetType("System.String"));
                    dataTable.Columns.Add("inCol2", Type.GetType("System.String"));
                    dataTable.Columns.Add("区域空洞", Type.GetType("System.String"));
                    dataTable.Columns.Add("连接数量", Type.GetType("System.String"));
            
                    //dataGridView1.Rows.Add(area.Length);
                    for (int i = 0; i < area.Length; i++)
                    {
                        DataRow newRow = dataTable.NewRow();
                        TabPage newtabPage = new TabPage();
                        newRow["面积"] = area.TupleSelect(i);
                        newRow["宽"] = width.TupleSelect(i).D.ToString("0.0");
                        newRow["高"] = height.TupleSelect(i).D.ToString("0.0");
                        newRow["宽高比"] = ratio.TupleSelect(i).D.ToString("0.0");
                        newRow["外圆半径"] = radius.TupleSelect(i).D.ToString("0.0");
                        newRow["内圆半径"] = InnerRadius.TupleSelect(i).D.ToString("0.0");
                        newRow["圆度"] = circularity.TupleSelect(i).D.ToString("0.00");
                        newRow["row"] = rows.TupleSelect(i).D.ToString("0.0");
                        newRow["col"] = cols.TupleSelect(i).D.ToString("0.0");
                        newRow["length1"] = length1.TupleSelect(i).D.ToString("0.0");
                        newRow["length2"] = length2.TupleSelect(i).D.ToString("0.0");
                        newRow["phi"] = phi.TupleSelect(i).TupleDeg().D.ToString("0.0");
                        newRow["紧密"] = compatness.TupleSelect(i).D.ToString("0.0");
                        newRow["轮廓长度"] = contLength.TupleSelect(i).D.ToString("0.0");
                        newRow["矩形度"] = rectangularity.TupleSelect(i).D.ToString("0.0");
                        newRow["中心平均距离"] = distance.TupleSelect(i).D.ToString("0.0");
                        newRow["距离的标准差"] = sigma.TupleSelect(i).D.ToString("0.0");
                        newRow["园度因子"] = roundness.TupleSelect(i).D.ToString("0.0");
                        newRow["多边形边数"] = sides.TupleSelect(i).D.ToString("0.0");
                    
                        newRow["row1"] = row1.TupleSelect(i).D.ToString("0.0");
                        newRow["col1"] = column1.TupleSelect(i).D.ToString("0.0");
                        newRow["row2"] = row2.TupleSelect(i).D.ToString("0.0");
                        newRow["col2"] = col2.TupleSelect(i).D.ToString("0.0");
                        newRow["inRow1"] = inRow1.TupleSelect(i).D.ToString("0.0");
                        newRow["inCol1"] = inCol1.TupleSelect(i).D.ToString("0.0");
                        newRow["inRow2"] = inRow2.TupleSelect(i).D.ToString("0.0");
                        newRow["inCol2"] = inCol2.TupleSelect(i).D.ToString("0.0");
                        newRow["区域空洞"] = numHoles.TupleSelect(i).D.ToString("0.0");
                        newRow["连接数量"] = numConnected.TupleSelect(i).D.ToString("0.0");
                        //tpgListReadW.TabPages.Add(newtabPage);
                        dataTable.Rows.Add(newRow);
                        ////DictionaryEntry dictionaryEntry = new DictionaryEntry("","");
                        //dataGridView1.Rows[i].Cells[0].Value = area.TupleSelect(i);
                        //dataGridView1.Rows[i].Cells[1].Value = row.TupleSelect(i).D . ToString("0.0");
                        //dataGridView1.Rows[i].Cells[2].Value = column.TupleSelect(i).D.ToString("0.0");
                        //dataGridView1.Rows[i].Cells[3].Value = circularity.TupleSelect(i).D.ToString("0.00");
                        //dataGridView1.Rows[i].Cells[4].Value = length1.TupleSelect(i).D.ToString("0.0");
                        //dataGridView1.Rows[i].Cells[5].Value = length2.TupleSelect(i).D.ToString("0.0");
                        //dataGridView1.Rows[i].Cells[6].Value = phi.TupleSelect(i).TupleDeg().D.ToString("0.0");
                        index.Append(i+1);
                        //Al.Add(dictionaryEntry);
                    }
                    dataGridView1.DataSource = dataTable;

                    HWind.OneResIamge.AddImageMassage(rows, cols, index);
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void OBJSeleForm_Load(object sender, EventArgs e)
        {
            HWind.Initialize(hWindowControl1);
            HWind.OneResIamge.ColorResu = ColorResult.blue;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                HWind.HobjClear();
                HOperatorSet.SelectObj(HObjectErr, out HObject hObject,  e.RowIndex + 1);
                HWind.OneResIamge.GetKeyHobj().Clear();
                HWind.OneResIamge.AddNameOBJ("sele", hObject, Color.Red.Name.ToLower());
                HWind.AddObj(HObjectErr.RemoveObj(e.RowIndex+1));
                HOperatorSet.SmallestRectangle2(hObject, out HTuple row, out HTuple column, out HTuple phi, out HTuple length1, out HTuple length2);
                HOperatorSet.AreaCenter(hObject, out HTuple area, out row, out HTuple col);
                HOperatorSet.SmallestRectangle1(hObject, out HTuple row1, out HTuple col1, out HTuple row2, out HTuple col2);
                HWindID.SetPart(hWindowControl1.HalconWindow, row.TupleInt(), column.TupleInt(),length1.TupleInt()+100);
                if (e.ColumnIndex>=19&&e.ColumnIndex<=26)
                {
                    HOperatorSet.GenRectangle1(out HObject hObject1, row1, col1, row2, col2);
                    HWind.OneResIamge.AddNameOBJ("Rectangle1", hObject1, Color.Yellow.Name.ToLower());
                    HOperatorSet.InnerRectangle1(hObject, out  row1, out  col1, out  row2, out  col2);
                    HOperatorSet.GenRectangle1(out  hObject1, row1, col1, row2, col2);
                    HWind.OneResIamge.AddNameOBJ("InnerRectangle1", hObject1, Color.Blue.Name.ToLower());
                }
                else if (e.ColumnIndex >= 12 && e.ColumnIndex <= 17)
                {
                    HOperatorSet.SmallestCircle(hObject, out row, out col, out HTuple radius);
                    HOperatorSet.GenCircle(out HObject hObject1, row, col,radius);
          
                    HWind.OneResIamge.AddNameOBJ("Circle", hObject1, Color.Yellow.Name.ToLower());
                    HOperatorSet.InnerCircle(hObject, out row, out col,  out radius);
                    HOperatorSet.GenCircle(out hObject1, row, col, radius);
                    HWind.OneResIamge.AddNameOBJ("InnerCircle", hObject1, Color.Blue.Name.ToLower());
                }
                else if (e.ColumnIndex >= 8 && e.ColumnIndex <= 10)
                {
                    HOperatorSet.GenRectangle2(out HObject hObject1, row, col, phi,length1,length2);
                    HWind.OneResIamge.AddNameOBJ("Rectangle2", hObject1, Color.Yellow.Name.ToLower());
                }
                HWind.OneResIamge.AddImageMassage(rows, cols, index);
                HWind.OneResIamge.ISMassageBack = true;
                     HTuple txes = new HTuple();
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    txes.Append(dataGridView1.Columns[i].HeaderText+ ":"+dataGridView1.Rows[e.RowIndex].Cells[i].Value.ToString());
                }
                HWind.AddMeassge(txes);
                //     HWindID.OneResIamge.AddImageMassage(inets, inecol, e.RowIndex+1);
                HWind.ShowObj();
            }
            catch (Exception)
            {

            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

           
        }

        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            
        }

        private void toolStripCheckbox2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripCheckbox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripCheckbox3_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            AddErrObj(HObjectErr);
        }
    }
}
