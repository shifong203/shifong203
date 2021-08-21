using HalconDotNet;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision.HalconRunFile.Controls
{
    /// <summary>
    /// 叠加显示
    /// </summary>
    public partial class ListObjHalcon : UserControl
    {
        public HalconRun halcon;

        /// <summary>
        ///
        /// </summary>
        /// <param name="halconR"></param>
        public ListObjHalcon(HalconRun halconR)
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.TabStop = false;
            this._movable = true;
            halcon = halconR;
            //halcon.EventShowObj += Halcon_EventShowObj;
        }

        private bool _movable;

        private Pen pen = new Pen(Color.Black);

        /// <summary>
        /// UISizeDot的边框颜色
        /// </summary>
        public Color BorderColor
        {
            get { return pen.Color; }
            set
            {
                this.pen = new Pen(value);
                this.Refresh();
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            // TODO: 在此处添加自定义绘制代码
            //this.BackColor = Color.White;
            pe.Graphics.DrawRectangle(pen, 0, 0, this.Width - 1, this.Height - 1);
            // 调用基类 OnPaint
            base.OnPaint(pe);
        }

        /// <summary>
        ///
        /// </summary>
        public bool Movable
        {
            get { return this._movable; }
            set { this._movable = value; }
        }

        private void ListObjHalcon_Load(object sender, EventArgs e)
        {
            //UISizeKnob = new ErosSocket.ErosUI.UISizeKnob(this.Parent);
        }

        private void ListObjHalcon_Enter(object sender, EventArgs e)
        {
            //UISizeKnob.ShowUISizeDots(true);
        }

        private void ListObjHalcon_Leave(object sender, EventArgs e)
        {
            //UISizeKnob.ShowUISizeDots(false);
        }

        //public ErosSocket.ErosUI.UISizeKnob UISizeKnob;

        //private Dictionary<string, HalconRun.ObjectColorTi> dic = new Dictionary<string, HalconRun.ObjectColorTi>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="halcon"></param>
        /// <param name="objName"></param>
        /// <returns></returns>
        public HObject Halcon_EventShowObj(HalconRun halcon, string objName)
        {
            try
            {
                if (this.Visible)
                {
                    Thread thread = new Thread(DataGrev);
                    void DataGrev()
                    {
                        try
                        {
                            int ds = 0;

                            dataGridView1.Rows.Clear();
                            //int dss = halcon.DicObjColorTi.DirectoryHObject.Keys.Count;
                            ds = dataGridView1.Rows.Add("Imgae");
                            //dataGridView1.Rows[ds].Cells[4].Value = halcon.GetShowListObj("Imgae");
                            ds = dataGridView1.Rows.Add("结果区域");

                            //foreach (var item in halcon.TKHobject.DirectoryHObject)
                            //{
                            //    if (dic.ContainsKey("TK." + item.Key))
                            //    {
                            //    }
                            //    dataGridView1.Rows[ds].DefaultCellStyle.BackColor = Color.SeaShell;
                            //    if (item.Value.HobjectColot.ToString().Length == 8)
                            //    {
                            //        dataGridView1.Rows[ds].Cells[2].Style.BackColor = System.Drawing.Color.FromArgb(System.Int32.Parse(item.Value.HobjectColot.ToString().Substring(8, 2),
                            //            System.Globalization.NumberStyles.AllowHexSpecifier)
                            //      , System.Int32.Parse(item.Value.HobjectColot.ToString().Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier),
                            //      System.Int32.Parse(item.Value.HobjectColot.ToString().Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier),
                            //      System.Int32.Parse(item.Value.HobjectColot.ToString().Substring(6, 2), System.Globalization.NumberStyles.AllowHexSpecifier));
                            //    }
                            //    else
                            //    {
                            //        dataGridView1.Rows[ds].Cells[2].Style.BackColor = System.Drawing.Color.FromName(item.Value.HobjectColot);
                            //    }
                            //    dataGridView1.Rows[ds].Cells[2].Value = item.Value.HobjectColot;
                            //    dataGridView1.Rows[ds].Cells[3].Value = item.Value._HObject.CountObj();

                            //    int dst = halcon.GetShowListObj("TK." + item.Key);
                            //    if (dst > 0)
                            //    {
                            //        dataGridView1.Rows[ds].Cells[4].Value = dst;
                            //    }
                            //}
                            //foreach (var item in halcon.KeyHObject.DirectoryHObject)
                            //{
                            //    ds = dataGridView1.Rows.Add("KHobject." + item.Key);
                            //    dataGridView1.Rows[ds].DefaultCellStyle.BackColor = Color.Beige;
                            //    if (item.Value.HobjectColot.ToString().Length == 8)
                            //    {
                            //        dataGridView1.Rows[ds].Cells[2].Style.BackColor = System.Drawing.Color.FromArgb(System.Int32.Parse(item.Value.HobjectColot.ToString().Substring(8, 2),
                            //            System.Globalization.NumberStyles.AllowHexSpecifier)
                            //      , System.Int32.Parse(item.Value.HobjectColot.ToString().Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier),
                            //      System.Int32.Parse(item.Value.HobjectColot.ToString().Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier),
                            //      System.Int32.Parse(item.Value.HobjectColot.ToString().Substring(6, 2), System.Globalization.NumberStyles.AllowHexSpecifier));
                            //    }
                            //    else
                            //    {
                            //        dataGridView1.Rows[ds].Cells[2].Style.BackColor = System.Drawing.Color.FromName(item.Value.HobjectColot);
                            //    }
                            //    dataGridView1.Rows[ds].Cells[2].Value = item.Value.HobjectColot;
                            //    dataGridView1.Rows[ds].Cells[3].Value = item.Value._HObject.CountObj();
                            //    int dst = halcon.GetShowListObj("TK." + item.Key);
                            //    if (dst > 0)
                            //    {
                            //        dataGridView1.Rows[ds].Cells[4].Value = dst;
                            //    }
                            //}
                        }
                        catch (Exception)
                        {
                        }
                    }
                    thread.IsBackground = true;
                    thread.Start();
                }
            }
            catch (Exception)
            {
            }
            return new HObject();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            toolStripButton1.BackColor = colorDialog1.Color;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void dataGridView1_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            e.Row.HeaderCell.Value = string.Format("{0}", e.Row.Index + 1);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedCells.Count != 1) return;

                int rowst = dataGridView1.SelectedCells[0].RowIndex;
                if (dataGridView1.SelectedCells[0].ColumnIndex == 0 && dataGridView1.SelectedCells[0].Value != null)
                {
                    string dss = dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[2].Style.BackColor.Name;
                    //(0, 0)单元格的背景色为粉色
                    string HexColor = "#" + dataGridView1.Rows[rowst].Cells[2].Style.BackColor.R.ToString("X2") + dataGridView1.Rows[rowst].Cells[2].Style.BackColor.G.ToString("X2")
                    + dataGridView1.Rows[rowst].Cells[2].Style.BackColor.B.ToString("X2") + dataGridView1.Rows[rowst].Cells[2].Style.BackColor.A.ToString("X2");

                    //int ds = halcon.SetShowListObj(dataGridView1.Rows[rowst].Cells[0].Value.ToString(),
                    //    Convert.ToBoolean(dataGridView1.Rows[rowst].Cells[1].EditedFormattedValue), HexColor.ToUpper());
                    //if (ds < 0)
                    //{
                    //    dataGridView1.Rows[rowst].Cells[4].Value = "";
                    //}
                    //else
                    //{
                    //    dataGridView1.Rows[rowst].Cells[4].Value = ds;
                    //}
                }
                else if (dataGridView1.SelectedCells[0].ColumnIndex == 2)
                {
                    DialogResult dialogResult = colorDialog1.ShowDialog();

                    if (dialogResult == DialogResult.OK)
                    {
                        dataGridView1.Rows[rowst].Cells[2].Style.BackColor = colorDialog1.Color;
                        string HexColor = "#" + dataGridView1.Rows[rowst].Cells[2].Style.BackColor.R.ToString("X2") + dataGridView1.Rows[rowst].Cells[2].Style.BackColor.G.ToString("X2")
                + dataGridView1.Rows[rowst].Cells[2].Style.BackColor.B.ToString("X2") + dataGridView1.Rows[rowst].Cells[2].Style.BackColor.A.ToString("X2");
                        //halcon.SetObjColor(dataGridView1.Rows[rowst].Cells[0].Value.ToString(), HexColor);
                    }

                    dataGridView1.ClearSelection();
                }
            }
            catch (Exception)
            {
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            halcon.ListObjCler();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1[4, i].Value = "";
            }
        }
    }
}