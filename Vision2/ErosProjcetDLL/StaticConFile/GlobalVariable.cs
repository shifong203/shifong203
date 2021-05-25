using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Vision2.ErosProjcetDLL.StaticConFile
{
    public class GlobalVariable
    {
        private Dictionary<string, dynamic> Gkeys = new Dictionary<string, dynamic>();

        public dynamic this[string index]
        {
            get
            {
                if (Gkeys.ContainsKey(index))
                {
                    return Gkeys[index];
                }
                else
                {
                    Gkeys.Add(index, new System.Dynamic.ExpandoObject());
                    return Gkeys[index];
                };
            }
            set
            {
                if (Gkeys.ContainsKey(index))
                {
                    Gkeys[index] = value;
                }
                else
                {
                    Gkeys.Add(index, value);
                }
            }
        }

        public dynamic this[int index]
        {
            get
            {
                if (Gkeys.ContainsKey(index.ToString()))
                {
                    return Gkeys[index.ToString()];
                }
                else
                {
                    Gkeys.Add(index.ToString(), new System.Dynamic.ExpandoObject());
                    return Gkeys[index.ToString()];
                };
            }
            set
            {
                if (Gkeys.ContainsKey(index.ToString()))
                {
                    Gkeys[index.ToString()] = value;
                }
                else
                {
                    Gkeys.Add(index.ToString(), value);
                }
            }
        }

        public DataGridView GetKeysToDataGridView()
        {
            DataGridView dataGridVi = new DataGridView();
            dataGridVi.DataSource = new BindingSource(Gkeys, null);
            return dataGridVi;
        }

        public string[] GetKeys()
        {
            return Gkeys.Keys.ToArray();
        }
    }
}