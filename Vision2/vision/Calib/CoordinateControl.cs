using HalconDotNet;
using System;
using System.Windows.Forms;

namespace Vision2.vision.Calib
{
    public partial class CoordinateControl : UserControl
    {
        /// <summary>
        /// 空间
        /// </summary>
        private Coordinate _Coordinate;

        public CoordinateControl(Coordinate coordinate)
        {
            InitializeComponent();
            _Coordinate = coordinate;
            try
            {
                if (_Coordinate.Rows != null)
                {
                    for (int i = 0; i < _Coordinate.Rows.Length; i++)
                    {
                        dataGridView1.Rows.Add();
                    }
                    for (int i = 0; i < _Coordinate.Rows.Length; i++)
                    {
                        dataGridView1.Rows[i].Cells[0].Value = _Coordinate.Rows.TupleSelect(i);
                    }
                    for (int i = 0; i < _Coordinate.Columns.Length; i++)
                    {
                        dataGridView1.Rows[i].Cells[1].Value = _Coordinate.Columns.TupleSelect(i);
                    }

                    for (int i = 0; i < _Coordinate.Xs.Length; i++)
                    {
                        dataGridView1.Rows[i].Cells[2].Value = _Coordinate.Xs.TupleSelect(i);
                    }
                    for (int i = 0; i < _Coordinate.Ys.Length; i++)
                    {
                        dataGridView1.Rows[i].Cells[3].Value = _Coordinate.Ys.TupleSelect(i);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            try
            {
                HTuple rowP = new HTuple();
                HTuple ColP = new HTuple();
                HTuple Xmm = new HTuple();
                HTuple Ymm = new HTuple();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells[1].Value == null)
                    {
                        break;
                    }
                    rowP.Append(double.Parse(dataGridView1.Rows[i].Cells[0].Value.ToString()));
                    ColP.Append(double.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString()));
                    Xmm.Append(double.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString()));
                    Ymm.Append(double.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString()));
                }

                _Coordinate.VectorToHomMat2d(rowP, ColP, Ymm, Xmm);
                HTuple HomMat = _Coordinate.CoordHanMat2DXY;
                HOperatorSet.HomMat2dToAffinePar(_Coordinate.CoordHanMat2DXY, out HTuple sx, out HTuple sy, out HTuple phi, out HTuple theta, out HTuple tx, out HTuple ty);
                Vision2.ErosProjcetDLL.Project.AlarmText.AddText(string.Format("1={0},2={1},3={2},4={3},5={4},6={5}" + Environment.NewLine, HomMat.TupleSelect(0),
                    HomMat.TupleSelect(1), HomMat.TupleSelect(2), HomMat.TupleSelect(3), HomMat.TupleSelect(4), HomMat.TupleSelect(5)));
                Vision2.ErosProjcetDLL.Project.AlarmText.AddText(string.Format("机械彷射斜切X:{0}斜切Y:{1}角度旋转:{2}斜度:{3}偏移X:{4}偏移Y:{5}" + Environment.NewLine, sx, sy, phi.TupleDeg(), theta.TupleDeg(), tx, ty));

                MessageBox.Show("计算完成");
            }
            catch (Exception)
            {
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                HOperatorSet.GenCrossContourXld(out HObject cross, _Coordinate.Rows, _Coordinate.Columns, 40, 0);
                Vision.GetFocusRunHalcon().AddObj(cross);
                for (int i = 0; i < _Coordinate.Rows; i++)
                {
                    Vision.GetFocusRunHalcon().GetOneImageR().AddImageMassage(_Coordinate.Rows.TupleSelect(i), _Coordinate.Columns.TupleSelect(i), (i + 1).ToString());
                }
            }
            catch (Exception)
            {
            }
        }
    }
}