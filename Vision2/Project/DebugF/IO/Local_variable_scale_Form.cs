using System;
using System.Linq;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.UI.DataGridViewF;

namespace Vision2.Project.DebugF.IO
{
    public partial class Local_variable_scale_Form : Form
    {

        public Local_variable_scale_Form(ErosSocket.ErosConLink.UClass.ErosValues erosValueDs)
        {
            InitializeComponent();
            dataGridView1.DoubleBuffered(true);
            SetData(erosValueDs);
            BindingSource bindSource = new BindingSource();
            bindSource.DataSource = keyValues.DictionaryValueD.Values.ToList();
            dataGridView1.DataSource = bindSource;
        }



        ErosSocket.ErosConLink.UClass.ErosValues keyValues;
        public void SetData(ErosSocket.ErosConLink.UClass.ErosValues erosValueDs)
        {
            keyValues = erosValueDs;
            dataGridView1.AutoGenerateColumns = false;
            BindingSource bindSource = new BindingSource();
            //bindSource.DataSource = keyValues.DictionaryValueD.Values.ToList();
            //dataGridView1.DataSource = bindSource;
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                keyValues.Remove(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells[0].Value.ToString());
                BindingSource bindSource = new BindingSource();
                //bindSource.DataSource = keyValues.DictionaryValueD.Values.ToList();
                //dataGridView1.DataSource = bindSource;
            }
            catch (Exception)
            {
            }
        }

        private void 初始化变量表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("初始化值", "初始化本量表?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                foreach (var item in keyValues.DictionaryValueD)
                {
                    item.Value.InitialValue();
                }
                BindingSource bindSource = new BindingSource();
                //bindSource.DataSource = keyValues.DictionaryValueD.Values.ToList();
                //dataGridView1.DataSource = bindSource;
            }
        }

        private void 当前值为快照值ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("快照值", "设置当前值为快照值?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                foreach (var item in keyValues.DictionaryValueD.Values)
                {
                    item.Value.SnapshootValueStr = item.Value.Value.ToString();
                }
                //BindingSource bindSource = new BindingSource();
                //bindSource.DataSource = keyValues.DictionaryValueD.Values.ToList();
                //dataGridView1.DataSource = bindSource;
            }
        }

        private void 快照值为当前置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("当前值", "设置快照值为当前值?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                foreach (var item in keyValues.DictionaryValueD)
                {
                    item.Value.ValueSnapshot();
                }
                //BindingSource bindSource = new BindingSource();
                //bindSource.DataSource = keyValues.DictionaryValueD.Values.ToList();
                //dataGridView1.DataSource = bindSource;
            }
        }

        private void dataGridView1_Paint(object sender, PaintEventArgs e)
        {
            dataGridView1.Update();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {

            }
        }
    }
}
