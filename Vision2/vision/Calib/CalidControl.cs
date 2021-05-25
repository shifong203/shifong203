using HalconDotNet;
using System;
using System.Windows.Forms;

namespace Vision2.vision.Calib
{
    public partial class CalidControl : UserControl
    {
        /// <summary>
        /// 空间
        /// </summary>
        private Coordinate _Coordinate;

        private int d;
        public CalidControl()
        {
            InitializeComponent();
            ThisForm = this;


            //foreach (var item in Vision.Instance.DicCoordinate)
            //{
            //    listBox1.Items.Add(item.Key);
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_Coordinate != null && textBox1.Text != "")
            {
                if (!Vision.Instance.DicCoordinate.ContainsKey(textBox1.Text))
                {
                    Vision.Instance.DicCoordinate.Add(textBox1.Text, _Coordinate);
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show(textBox1.Text + "已存在！是否覆盖", "覆盖空间", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Vision.Instance.DicCoordinate[textBox1.Text] = _Coordinate;
                    }
                }
                listBox1.Items.Clear();
                foreach (var item in Vision.Instance.DicCoordinate)
                {
                    listBox1.Items.Add(item.Key);
                }
            }
            else if (_Coordinate == null)
            {
                MessageBox.Show("空间不存在!");
            }
            else if (textBox1.Text == "")
            {
                MessageBox.Show("请输入名称");
            }
        }


        public static CalidControl ThisForm = new CalidControl();

        /// <summary>
        /// 添加点数据
        /// </summary>
        /// <param name="data"></param>
        public void AddPoint(string data)
        {
            MethodInvoker methodInvoker = ThreadFilesVison;
            this.Invoke(methodInvoker);
            void ThreadFilesVison()
            {
                richTextBox1.AppendText(data + Environment.NewLine);
            }
        }

        /// <summary>
        /// 添加点数据
        /// </summary>
        /// <param name="rowName">列明</param>
        /// <param name="value1">值1</param>
        /// <param name="value2">值2</param>
        public void AddPoint(string rowName, string value3, string value4)
        {
            MethodInvoker methodInvoker = ThreadFilesVison;
            this.Invoke(methodInvoker);
            void ThreadFilesVison()
            {

                if (int.TryParse(rowName, out d))
                {
                    if (dataGridView1.Rows.Count <= d)
                    {
                        dataGridView1.Rows.Add(3);
                    }
                    dataGridView1.Rows[d].Cells[0].Value = rowName;
                    dataGridView1.Rows[d].Cells[1].Value = value3;
                    dataGridView1.Rows[d].Cells[2].Value = value4;
                }
                else
                {
                    if (dataGridView1.Rows.Count <= d)
                    {
                        dataGridView1.Rows.Add();
                    }
                    d = Convert.ToInt16(rowName.Substring(rowName.Length - 1)) - 1;
                    dataGridView1.Rows[d].Cells[0].Value = rowName;
                    dataGridView1.Rows[d].Cells[3].Value = value3;
                    dataGridView1.Rows[d].Cells[4].Value = value4;
                }

            }
        }

        /// <summary>
        /// 添加图像坐标
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public void AddPointRowsCols(double col, double row)
        {
            MethodInvoker methodInvoker = ThreadFilesVison;
            this.Invoke(methodInvoker);
            void ThreadFilesVison()
            {
                dataGridView1.Rows[d].Cells[1].Value = col;
                dataGridView1.Rows[d].Cells[2].Value = row;
                //richTextBox1.AppendText("Row:" + row + "col:" + col + Environment.NewLine);
            }
        }

        /// <summary>
        /// 添加空间
        /// </summary>
        /// <param name="coordinate"></param>
        public void AddCoordinate(Coordinate coordinate)
        {
            _Coordinate = coordinate;

            HOperatorSet.HomMat2dToAffinePar(_Coordinate.CoordHanMat2d, out HTuple sx, out HTuple sy, out HTuple phi, out HTuple theta, out HTuple tx, out HTuple ty);

            _Coordinate.Mat2dPar();
        }



        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                propertyGrid1.SelectedObject = Vision.Instance.DicCoordinate[listBox1.SelectedItem.ToString()];
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_Coordinate != null)
            {
                _Coordinate.Mat2dPar();
                propertyGrid1.SelectedObject = _Coordinate;
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
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
                    ColP.Append(double.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString()));
                    rowP.Append(double.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString()));
                    Xmm.Append(double.Parse(dataGridView1.Rows[i].Cells[3].Value.ToString()));
                    Ymm.Append(double.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString()));
                }
                _Coordinate = new Coordinate();

                _Coordinate.VectorToHomMat2d(rowP, ColP, Ymm, Xmm);
                HOperatorSet.HomMat2dToAffinePar(_Coordinate.CoordHanMat2DXY, out HTuple sx, out HTuple sy,
                    out HTuple phi, out HTuple theta, out HTuple tx, out HTuple ty);
                //HOperatorSet.AffineTransPixel(_Coordinate.CoordHanMat2d, 0, 0, out HTuple axisX, out HTuple axisY);
                richTextBox1.AppendText(string.Format("斜切X:{0}斜切Y:{1}角度旋转:{2}斜度:{3}偏移X:{4}偏移Y:{5}"
                    + Environment.NewLine, sx, sy, phi.TupleDeg(), theta.TupleDeg(), tx, ty));

                //richTextBox1.AppendText(string.Format("机械原点的像素位置X(Col){0}，Y(Row){1}" + Environment.NewLine, axisX, axisY));
                MessageBox.Show("计算完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show("计算失败：" + ex.Message);

            }
        }

        private void CalidControl_Load(object sender, EventArgs e)
        {

            dataGridView1.Rows.Add(9);

            foreach (var item in Vision.Instance.DicCoordinate)
            {
                listBox1.Items.Add(item.Key);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Click(object sender, EventArgs e)
        {
            try
            {
                propertyGrid1.SelectedObject = _Coordinate;
            }
            catch (Exception)
            {
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}