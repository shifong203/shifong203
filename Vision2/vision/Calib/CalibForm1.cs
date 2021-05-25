using System;
using System.Windows.Forms;

namespace Vision2.vision.Calib
{
    public partial class CalibForm1 : Form
    {
        public CalibForm1()
        {
            InitializeComponent();
            ThisForm = this;
            for (int i = 0; i < 9; i++)
            {
                dataGridView1.Rows.Add();
            }
            foreach (var item in Vision.Instance.DicCoordinate)
            {
                listBox1.Items.Add(item.Key);
            }
        }

        private int d;
        public static CalibForm1 ThisForm = new CalibForm1();

        public void AddPoint(string data)
        {
            MethodInvoker methodInvoker = ThreadFilesVison;
            this.Invoke(methodInvoker);
            void ThreadFilesVison()
            {
                richTextBox1.AppendText(data + Environment.NewLine);
            }
        }

        public void AddPoint(string rowName, string value3, string value4)
        {
            MethodInvoker methodInvoker = ThreadFilesVison;
            this.Invoke(methodInvoker);
            void ThreadFilesVison()
            {
                d = Convert.ToInt16(rowName.Substring(rowName.Length - 1)) - 1;
                dataGridView1.Rows[d].Cells[0].Value = rowName;
                dataGridView1.Rows[d].Cells[3].Value = value3;
                dataGridView1.Rows[d].Cells[4].Value = value4;
            }
        }

        public void AddPointXX(double row, double col)
        {
            MethodInvoker methodInvoker = ThreadFilesVison;
            this.Invoke(methodInvoker);
            void ThreadFilesVison()
            {
                dataGridView1.Rows[d].Cells[1].Value = row;
                dataGridView1.Rows[d].Cells[2].Value = col;
                richTextBox1.AppendText("Row:" + row + "col:" + col + Environment.NewLine);
            }
        }

        public void AddCoordinate(Coordinate coordinate)
        {
            Coordinate = coordinate;
            Coordinate.Mat2dPar();
        }

        private Coordinate Coordinate;

        private void CalibForm1_Load(object sender, EventArgs e)
        {
            listBox2.Items.Add("新空间");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Coordinate != null && textBox1.Text != "")
            {
                if (!Vision.Instance.DicCoordinate.ContainsKey(textBox1.Text))
                {
                    Vision.Instance.DicCoordinate.Add(textBox1.Text, Coordinate);
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show(textBox1.Text + "已存在！是否覆盖", "覆盖空间", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Vision.Instance.DicCoordinate[textBox1.Text] = Coordinate;
                    }
                }
                listBox1.Items.Clear();
                foreach (var item in Vision.Instance.DicCoordinate)
                {
                    listBox1.Items.Add(item.Key);
                }
            }
            else if (Coordinate == null)
            {
                MessageBox.Show("空间不存在!");
            }
            else if (textBox1.Text == "")
            {
                MessageBox.Show("请输入名称");
            }
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
            if (Coordinate != null)
            {
                Coordinate.Mat2dPar();
                propertyGrid1.SelectedObject = Coordinate;
            }
        }
    }
}