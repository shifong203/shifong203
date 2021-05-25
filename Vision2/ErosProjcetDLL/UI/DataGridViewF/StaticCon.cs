using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Vision2.ErosProjcetDLL.UI.DataGridViewF
{
    public static class StCon
    {
        /// <summary>
        /// 为控件DataGridView添加行号
        /// </summary>
        /// <param name="dataGrid"></param>
        public static void AddCon(DataGridView dataGrid, int cont = 1)
        {
            dataGrid.RowHeadersVisible = true;
            dataGrid.RowPostPaint += new DataGridViewRowPostPaintEventHandler(DataGridViewEx_RowPostPaint);
            void OnEditingControlShowing(DataGridViewEditingControlShowingEventArgs e)
            {
                if (dataGrid.CurrentCell != null && dataGrid.CurrentCell.OwningColumn is DataGridViewComboBoxColumnEx)
                {
                    DataGridViewComboBoxColumnEx col = dataGrid.CurrentCell.OwningColumn as DataGridViewComboBoxColumnEx;
                    //修改组合框的样式
                    if (col.DropDownStyle != ComboBoxStyle.DropDownList)
                    {
                        ComboBox combo = e.Control as ComboBox;
                        combo.DropDownStyle = col.DropDownStyle;
                        combo.Leave += new EventHandler(combo_Leave);
                    }
                }
                OnEditingControlShowing(e);
            }
            //dataGrid.Leave += combo_Leave;
            /// <summary>
            /// 当焦点离开时，需要将新输入的值加入到组合框的 Items 列表中
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            void combo_Leave(object sender, EventArgs e)
            {
                ComboBox combo = sender as ComboBox;
                if (combo != null)
                {
                    combo.Leave -= new EventHandler(combo_Leave);
                    if (dataGrid.CurrentCell != null && dataGrid.CurrentCell.OwningColumn is DataGridViewComboBoxColumnEx)
                    {
                        DataGridViewComboBoxColumnEx col = dataGrid.CurrentCell.OwningColumn as DataGridViewComboBoxColumnEx;
                        //一定要将新输入的值加入到组合框的值列表中
                        //否则下一步给单元格赋值的时候会报错（因为值不在组合框的值列表中）
                        col.Items.Add(combo.Text);
                        dataGrid.CurrentCell.Value = combo.Text;
                    }
                }

            }
            void DataGridViewEx_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
            {
                string title = (e.RowIndex + cont).ToString();
                Brush bru = Brushes.Black;
                e.Graphics.DrawString(title, dataGrid.DefaultCellStyle.Font,
                    bru, e.RowBounds.Location.X + dataGrid.RowHeadersWidth / 2 - 4, e.RowBounds.Location.Y + 4);

            }
        }


        /// <summary>
        /// 双缓冲，解决闪烁问题
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="flag"></param>
        public static void DoubleBufferedDataGirdView(DataGridView dgv, bool flag)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, flag, null);
        }

    }
}
