using System;
using System.Drawing;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;
using static Vision2.vision.HalconRunFile.RunProgramFile.OneCompOBJs;

namespace Vision2.vision.RestVisionForm
{
    public partial class RestOneComUserControl : UserControl
    {
        public RestOneComUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 显示集合委托
        /// </summary>
        /// <param name="hObject"></param>
        public delegate void UPShowObj(int objName);

        /// <summary>
        /// 显示事件
        /// </summary>
        public event UPShowObj EventShowObj;

        private OneComponent oneContOBJs;

        /// <summary>
        /// 单个元件
        /// </summary>
        private OneRObj OneRObjT;

        /// <summary>
        /// 关联元件缺陷集合
        /// </summary>
        /// <param name="oBJs"></param>
        public void UpData(OneComponent oBJs)
        {
            try
            {
                oneContOBJs = oBJs;
                dataGridView1.Rows.Clear();
                dataGridView1.Rows.Add(oneContOBJs.oneRObjs.Count);
                int d = 0;
                foreach (var item in oneContOBJs.oneRObjs)
                {
                    dataGridView1.Rows[d].Cells[0].Value = item.NGText;
                    dataGridView1.Rows[d].Cells[1].Value = item.NGText;
                    d++;
                }
                UpOnes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 刷新显示
        /// </summary>
        public void UpOnes()
        {
            int intd = 0;
            foreach (var item in oneContOBJs.oneRObjs)
            {
                if (!item.Done)
                {
                    dataGridView1.Rows[intd].DefaultCellStyle.BackColor = Color.Yellow;
                    OneRObjT = item;
                    Rectangle rectangle = dataGridView1.GetCellDisplayRectangle(1, intd, false);
                    Rectangle rectangle2 = dataGridView1.RectangleToScreen(rectangle);
                    listBox1.Location = new Point(rectangle.X - 50, rectangle.Y + dataGridView1.ColumnHeadersHeight);
                    listBox1.Visible = true;
                    listBox1.Items.Clear();
                    //listBox1.Items.Add(0+ OneRObjT.NGText);
                    //listBox1.SelectedIndex = 0;
                    for (int i = 0; i < OneRObjT.RestStrings.Count; i++)
                    {
                        if (Vision.Instance.DefectTypeDicEx.ContainsKey(OneRObjT.RestStrings[i]))
                        {
                            string dname = Vision.Instance.DefectTypeDicEx[OneRObjT.RestStrings[i]].DrawbackEnglish;
                            if (!listBox1.Items.Contains(i + dname))
                            {
                                listBox1.Items.Add(i + dname);
                            }
                        }
                        else
                        {
                            if (!listBox1.Items.Contains(i + OneRObjT.RestStrings[i]))
                            {
                                listBox1.Items.Add(i + OneRObjT.RestStrings[i]);
                            }
                        }
                    }
                    listBox1.SelectedIndex = 0;
                    break;
                }
                else
                {
                    if (item.aOK)
                    {
                        dataGridView1.Rows[intd].DefaultCellStyle.BackColor = Color.Green;
                    }
                    else
                    {
                        dataGridView1.Rows[intd].DefaultCellStyle.BackColor = Color.Red;
                    }
                }
                intd++;
            }
        }

        public void SetRest(int ngTextItmeIndex)
        {
            if (ngTextItmeIndex >= 0)
            {
                if (listBox1.Items.Count > ngTextItmeIndex)
                {
                    listBox1.SelectedIndex = ngTextItmeIndex;
                }
                if (ngTextItmeIndex > listBox1.Items.Count)
                {
                    //OneRObjT.RAddNG(listBox1.SelectedItem.ToString());
                }
                else
                {
                    oneContOBJs.RAddNG(OneRObjT.RestStrings[listBox1.SelectedIndex]);
                }
            }
            else
            {
                oneContOBJs.RAddOK();
            }
            UpOnes();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            SetRest(listBox1.SelectedIndex);
            try
            {
                EventShowObj.Invoke(listBox1.SelectedIndex);
            }
            catch (Exception)
            {
            }
        }
    }
}