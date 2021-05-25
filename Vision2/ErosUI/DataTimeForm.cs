using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Vision2.ErosUI
{
    public partial class DataTimeForm : Form
    {
        public DataTimeForm(string path)
        {
            InitializeComponent();
            try
            {
                if (File.Exists(path))
                {
                    string[] strs = File.ReadAllLines(path);  //以每一行读取数据
                    int dw = 0;
                    for (int i = 0; i < strs.Length; i++)
                    {
                        if (strs[i].Length > 5)
                        {
                            dataGridView1.Rows.Add();
                            string[] datas = strs[i].Split(',');
                            dataGridView1.Rows[dw].Cells[0].Value = datas[0];
                            for (int i1 = 1; i1 < datas.Length; i1++)
                            {
                                if (datas[i1].Contains('='))
                                {
                                    dataGridView1.Rows[dw].Cells[i1].Value = datas[i1].Split('=')[1];
                                }
                            }
                            dw++;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void DataTime_Load(object sender, EventArgs e)
        {
        }
    }
}