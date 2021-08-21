using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Vision2.ErosProjcetDLL.Project
{
    public class PLCIntEnum
    {
        public Dictionary<int, string> kaayValue { get; set; } = new Dictionary<int, string>();

        public string this[int index]
        {
            get
            {
                if (kaayValue.ContainsKey(index))
                {
                    return kaayValue[index];
                }
                return "未定义状态";
            }
            set
            {
                if (kaayValue.ContainsKey(index))
                {
                    kaayValue[index] = value;
                }
                else
                {
                    kaayValue.Add(index, value);
                }
            }
        }

        public void RemovAll()
        {
            kaayValue.Clear();
        }

        public void UpDataGiev(DataGridView dataGridView)
        {
            dataGridView.Rows.Clear();
            int dnumve = 0;
            foreach (var item in kaayValue)
            {
                dnumve = dataGridView.Rows.Add();
                dataGridView.Rows[dnumve].Cells[0].Value = item.Key;
                dataGridView.Rows[dnumve].Cells[1].Value = item.Value;
            }
        }

        public void SetDataGiev(DataGridView dataGridView)
        {
            Dictionary<int, string> keyValuePairs = new Dictionary<int, string>();
            try
            {
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    if (dataGridView.Rows[i].Cells[0].Value != null && dataGridView.Rows[i].Cells[1].Value != null &&
                        dataGridView.Rows[i].Cells[0].Value.ToString() != "" && dataGridView.Rows[i].Cells[1].Value.ToString() != ""
                        )
                    {
                        if (int.TryParse(dataGridView.Rows[i].Cells[0].Value.ToString(), out int keyInt) && !keyValuePairs.ContainsKey(keyInt))
                        {
                            keyValuePairs.Add(keyInt, dataGridView.Rows[i].Cells[1].Value.ToString());
                        }
                    }
                }
                kaayValue = keyValuePairs;
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("更改失败:" + ex.Message);
            }
        }
    }
}