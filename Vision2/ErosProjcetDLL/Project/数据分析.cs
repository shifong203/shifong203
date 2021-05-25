using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using VisionHalcon.ErosUI;

namespace Vision2.ErosProjcetDLL.Project
{
    public partial class 数据分析 : Form
    {

        private RealTimeImageMaker rti;
        private Color backColor = Color.Black;//指定绘制曲线图的背景色  

        public 数据分析()
        {
            InitializeComponent();
            rti = new RealTimeImageMaker(Width / 2, Height / 2, backColor, Color.Green);
            //thread = new Thread(new ThreadStart(Run));
            //thread.Start();
        }

        private void 数据分析_Load(object sender, EventArgs e)
        {
        }

        private void Run()
        {
            while (!this.IsDisposed)
            {
                Image image = rti.GetCurrentCurve();
                Graphics g = CreateGraphics();
                //用指定背景色清除当前窗体上的图象  
                g.Clear(backColor);
                g.DrawImage(image, 0, 0);
                g.Dispose();
                //每秒钟刷新一次  
                Thread.Sleep(1000);
            }
        }

        private List<List<double>> listDatas1 = new List<List<double>>();
        private List<List<double>> listDatas2 = new List<List<double>>();

        //struct PrecisionAnalysis
        //{
        //    public PrecisionAnalysis(int length)
        //    {
        //        this = new PrecisionAnalysis();
        //        datasAccuracy = new List<double>();
        //        datasMen = new List<double>();
        //        datasMin = new List<double>();
        //        datasMax = new List<double>();
        //        for (int i = 0; i < length; i++)
        //        {
        //            datasMin.Add(99999999999);
        //            datasMax.Add(0);
        //        }
        //        datas = new List<List<double>>();
        //    }
        //    public PrecisionAnalysis(List<List<double>> listDatas)
        //    {
        //        this = new PrecisionAnalysis(listDatas[0].Count);
        //        this.Analyze(listDatas);
        //    }
        //    public void Analyze(List<List<double>> listDatas)
        //    {
        //        this = new PrecisionAnalysis(listDatas[0].Count);
        //        List<double>[] time = new List<double>[listDatas.Count];
        //        listDatas.CopyTo(time);
        //        datas.AddRange(time);
        //        for (int i1 = 0; i1 < datas[0].Count(); i1++)
        //        {
        //            double men = 0;
        //            for (int i = 0; i < datas.Count; i++)
        //            {
        //                men = men + datas[i][i1];
        //                if (this.datasMin[i1] > datas[i][i1])
        //                {
        //                    this.datasMin[i1] = datas[i][i1];
        //                }
        //                if (this.datasMax[i1] < datas[i][i1])
        //                {
        //                    this.datasMax[i1] = (datas[i][i1]);
        //                }
        //            }
        //            this.datasMen.Add( men / datas.Count);
        //            this.datasAccuracy.Add((this.datasMax[i1] - this.datasMin[i1])/2);
        //        }

        //    }
        //    public void DataSoer(DataGridView dataGridView)
        //    {
        //        DataTable dt = new DataTable();
        //        DataColumn dataV = new DataColumn();
        //        dataV.Caption ="0";
        //        dataV.ColumnName = "序号";
        //        dt.Columns.Add(dataV);
        //        for (int i = 1; i < datas[0].Count +1; i++)
        //        {
        //            DataColumn dataColumn = new DataColumn();
        //            dataColumn.Caption = i.ToString();
        //            dt.Columns.Add(dataColumn);
        //        }
        //        for (int i = 0; i < datas.Count; i++)
        //        {
        //            DataRow de = dt.NewRow();
        //            de[0] = i + 1;
        //            for (int i1 = 0; i1 < datas[i].Count; i1++)
        //            {
        //                de[i1 + 1] = datas[i][i1];
        //            }
        //            dt.Rows.Add(de);
        //        }
        //        dataGridView.DataSource = dt;
        //    }
        //    public void AnalyzeDataSoer(DataGridView dataGridView)
        //    {
        //        DataTable dt = new DataTable();

        //        for (int i = 0; i < this.datasAccuracy.Count+1; i++)
        //        {
        //            DataColumn dataColumn = new DataColumn();
        //            dataColumn.Caption = i.ToString();
        //            dataColumn.ColumnName = (i + 1).ToString();
        //            dt.Columns.Add(dataColumn);
        //        }
        //         DataRow rowDatasAccuracy = dt.NewRow();
        //         rowDatasAccuracy[0] = "精度±";
        //         DataRow rowDatasMin = dt.NewRow();
        //         rowDatasMin[0] = "最小值";
        //         DataRow rowDatasMen = dt.NewRow();
        //         rowDatasMen[0] = "平均值";
        //         DataRow rowDatasMax = dt.NewRow();
        //         rowDatasMax[0] = "最大值";
        //         for (int i = 0; i < this.datasAccuracy.Count; i++)
        //         {
        //            rowDatasAccuracy[i + 1] = this.datasAccuracy[i];
        //            rowDatasMin[i + 1] = this.datasMin[i];
        //            rowDatasMen[i + 1] = this.datasMen[i];
        //            rowDatasMax[i + 1] = this.datasMax[i];
        //         }
        //         dt.Rows.Add(rowDatasAccuracy);
        //        dt.Rows.Add(rowDatasMin);
        //        dt.Rows.Add(rowDatasMen);
        //        dt.Rows.Add(rowDatasMax);
        //        dataGridView.DataSource = dt;
        //    }
        //    public string ShowStrAnalysis()
        //    {
        //        string accuracy = "精度范围±:";
        //        string minStr = "最小值:";
        //        string maxStr = "最大值:";
        //        string menStr = "平均值:";
        //        for (int i = 0; i < this.datasAccuracy.Count; i++)
        //        {
        //            if (i==this.datasAccuracy.Count-1)
        //            {
        //                accuracy += this.datasAccuracy[i].ToString() + ";";
        //                minStr += this.datasMin[i].ToString() + ";";
        //                maxStr += this.datasMax[i].ToString() + ";";
        //                menStr += this.datasMen[i].ToString() + ";";
        //            }
        //            else
        //            {
        //                accuracy += this.datasAccuracy[i].ToString() + ",";
        //                minStr += this.datasMin[i].ToString() + ",";
        //                maxStr += this.datasMax[i].ToString() + ",";
        //                menStr += this.datasMen[i].ToString() + ",";
        //            }
        //        }
        //        return accuracy + Environment.NewLine+minStr+ Environment.NewLine + menStr+ Environment.NewLine + maxStr+ Environment.NewLine;
        //    }
        //    public List<double> datasMax;
        //    public List<double> datasMin;
        //    public List<double> datasMen;
        //    public List<double> datasAccuracy;
        //    public List<List<double>>datas;
        //}

        private void tsmOpenFileDatas_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "请选择检测数据txt文件可多选";
            openFileDialog.Multiselect = false;
            listDatas1.Clear();
            listDatas2.Clear();

            try
            {
                openFileDialog.InitialDirectory = Application.StartupPath;
                //openFileDialog.Filter = "TXT文件|*(.);";
                openFileDialog.ShowDialog();
                if (openFileDialog.FileName.Length == 0) return;
                string[] txts = File.ReadAllLines(openFileDialog.FileName);
                for (int i = 0; i < txts.Length; i = i + 2)
                {
                    if (txts[i].StartsWith("短波"))
                    {
                        string tiem = txts[i].Remove(0, txts[i].IndexOf('['));
                        tiem = tiem.Trim(']', '[');
                        if (tiem.Contains(","))
                        {
                            string[] tiems = tiem.Split(',');
                            List<double> doblesl = new List<double>();
                            for (int i1 = 0; i1 < tiems.Length; i1++)
                            {
                                doblesl.Add(Convert.ToDouble(tiems[i1]));
                            }
                            listDatas1.Add(doblesl);
                        }
                    }
                    if (txts[i + 1].StartsWith("长波"))
                    {
                        string tiem = txts[i + 1].Remove(0, txts[i].IndexOf('['));
                        tiem = tiem.Trim(']', '[');
                        if (tiem.Contains(","))
                        {
                            string[] tiems = tiem.Split(',');

                            List<double> dobles2 = new List<double>();
                            for (int i1 = 0; i1 < tiems.Length; i1++)
                            {
                                if (double.TryParse(tiems[i1], out double sd))
                                {
                                    dobles2.Add(Convert.ToDouble(tiems[i1]));
                                }
                            }
                            listDatas2.Add(dobles2);
                        }
                    }
                }

                DataTable dt = new DataTable();
                for (int i = 0; i < listDatas1[0].Count + 1; i++)
                {
                    DataColumn dataColumn = new DataColumn();
                    dataColumn.Caption = i.ToString();
                    dt.Columns.Add(dataColumn);
                }
                for (int i = 0; i < listDatas1.Count; i++)
                {
                    DataRow de = dt.NewRow();
                    de[0] = i + 1;
                    for (int i1 = 0; i1 < listDatas1[i].Count; i1++)
                    {
                        de[i1 + 1] = listDatas1[i][i1];
                    }
                    dt.Rows.Add(de);
                }
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }
    }
}