using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Vision2.ErosProjcetDLL.UI.DataGridViewF
{
    /// <summary>
    /// 可修改 DropDownStyle 的 DataGridViewComboBoxColumn
    /// </summary>
    public class DataGridViewComboBoxColumnEx : DataGridViewComboBoxColumn
    {
        private ComboBoxStyle dropDownStyle;

        /// <summary>
        /// 控制组合框的外观和功能
        /// </summary>
        [Description("控制组合框的外观和功能"), DefaultValue(ComboBoxStyle.DropDownList)]
        public ComboBoxStyle DropDownStyle
        {
            get { return dropDownStyle; }
            set { dropDownStyle = value; }
        }

        public DataGridViewComboBoxColumnEx()
        {
            dropDownStyle = ComboBoxStyle.DropDownList;
        }
    }
    /// <summary>
    /// 自定义可编辑下拉框单元
    /// </summary>
    class DataGridViewComboEditBoxCell : DataGridViewComboBoxCell
    {

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue,
            DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);

            ComboBox comboBox = (ComboBox)base.DataGridView.EditingControl;
            if (comboBox != null)
            {
                comboBox.DropDownStyle = ComboBoxStyle.DropDown;
                comboBox.AutoCompleteMode = AutoCompleteMode.Suggest;
                comboBox.Validating += new CancelEventHandler(comboBox_Validating);
            }
        }

        protected override object GetFormattedValue(object value, int rowIndex,
            ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter,
            TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
        {
            if (value != null && value.ToString().Trim() != string.Empty)
            {
                if (Items.IndexOf(value) == -1)// 如果下拉框中不存在填入的值，则添加到下拉框中
                {
                    Items.Add(value);
                    // 添加到该列所有单元所绑定的下拉列表中
                    DataGridViewComboBoxColumn col = (DataGridViewComboBoxColumn)OwningColumn;
                    col.Items.Add(value);
                }

            }

            return base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);
        }

        private void comboBox_Validating(object sender, CancelEventArgs e)
        {
            DataGridViewComboBoxEditingControl cbo = (DataGridViewComboBoxEditingControl)sender;
            if (cbo.Text.Trim() == string.Empty)
                return;

            DataGridView grid = cbo.EditingControlDataGridView;
            object value = cbo.Text;

            if (cbo.Items.IndexOf(value) == -1)
            {
                DataGridViewComboBoxColumn cboCol = (DataGridViewComboBoxColumn)grid.Columns[grid.CurrentCell.ColumnIndex];
                // 添加到当前下拉框中以及模版中，避免出现重复项
                if (!cbo.Items.Contains(value))
                {
                    cbo.Items.Add(value);
                    cboCol.Items.Add(value);
                }

                grid.CurrentCell.Value = value;

            }
        }
    }


    public class DataGridViewComboEditBoxColumn : DataGridViewComboBoxColumn
    {


        public DataGridViewComboEditBoxColumn()
        {
            DataGridViewComboEditBoxCell obj = new DataGridViewComboEditBoxCell();

            this.CellTemplate = obj;

        }
    }



    /// <summary>
    /// 可分组的列 (该列必须是 Grid 的第一列)
    /// </summary>
    public class DataGridViewGroupColumn : DataGridViewTextBoxColumn
    {
        public DataGridViewGroupColumn()
        {
            CellTemplate = new DataGridViewGroupCell();
            ReadOnly = true;
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if ((value != null) && !(value is DataGridViewGroupCell))
                {
                    throw new InvalidCastException("Need System.Windows.Forms.DataGridViewGroupCell");
                }
                base.CellTemplate = value;
            }
        }
    }

    /// <summary>
    /// 可分组的单元格
    /// </summary>
    public class DataGridViewGroupCell : DataGridViewTextBoxCell
    {
        #region Variant

        /// <summary>
        /// 标示的宽度
        /// </summary>
        private const int PLUS_WIDTH = 24;

        /// <summary>
        /// 标示的区域
        /// </summary>
        private Rectangle groupPlusRect;

        #endregion Variant

        #region Init

        public DataGridViewGroupCell()
        {
            groupLevel = 1;
        }

        #endregion Init

        #region Property

        private int groupLevel;

        /// <summary>
        /// 组级别(以1开始)
        /// </summary>
        public int GroupLevel
        {
            get { return groupLevel; }
            set { groupLevel = value; }
        }

        private DataGridViewGroupCell parentCell;

        /// <summary>
        /// 父节点
        /// </summary>
        public DataGridViewGroupCell ParentCell
        {
            get
            {
                return parentCell;
            }
            //set
            //{
            //    if (value == null)
            //        throw new NullReferenceException("父节点不可为空");
            //    if (!(value is DataGridViewGroupCell))
            //        throw new ArgumentException("父节点必须为 DataGridViewGroupCell 类型");

            //    parentCell = value;
            //    parentCell.AddChildCell(this);
            //}
        }

        private bool collapsed;

        /// <summary>
        /// 是否收起
        /// </summary>
        public bool Collapsed
        {
            get { return collapsed; }
        }

        private List<DataGridViewGroupCell> childCells = null;

        /// <summary>
        /// 所有的子结点
        /// </summary>
        public DataGridViewGroupCell[] ChildCells
        {
            get
            {
                if (childCells == null)
                    return null;
                return childCells.ToArray();
            }
        }

        /// <summary>
        /// 取得组标示(有+或-号的框)的区域
        /// </summary>
        public Rectangle GroupPlusRect
        {
            get
            {
                return groupPlusRect;
            }
        }

        private bool bPaint = true;

        /// <summary>
        /// 是否重绘
        /// </summary>
        public bool BPaint
        {
            get { return bPaint; }
            set { bPaint = value; }
        }

        #endregion Property

        #region 添加子节点

        /// <summary>
        /// 添加子结点
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public int AddChildCell(DataGridViewGroupCell cell)
        {
            return AddChildCellRange(new DataGridViewGroupCell[] { cell });
        }

        public int AddChildCellRange(DataGridViewGroupCell[] cells)
        {
            bool needRedraw = false;
            if (childCells == null)
            {
                //需要画一个加号
                childCells = new List<DataGridViewGroupCell>();
                needRedraw = true;
            }
            foreach (DataGridViewGroupCell cell in cells)
            {
                childCells.Add(cell);
                cell.groupLevel = groupLevel + 1;
            }

            if (needRedraw)
                DataGridView.InvalidateCell(this);
            return childCells.Count;
        }

        #endregion 添加子节点

        #region 绘制节点

        protected override void Paint(System.Drawing.Graphics graphics, System.Drawing.Rectangle clipBounds, System.Drawing.Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            if (!bPaint)
            {
                base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
                return;
            }
            Pen gridPen = new Pen(DataGridView.GridColor);
            Brush bruBK = new SolidBrush(cellStyle.BackColor);
            int width = groupLevel * PLUS_WIDTH;
            Rectangle rectLeft = new Rectangle(cellBounds.Left, cellBounds.Top - 1, width, cellBounds.Height);
            cellBounds.X += width;
            cellBounds.Width -= width;
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);

            PaintGroupPlus(graphics, gridPen, bruBK, cellStyle, rectLeft, collapsed);

            gridPen.Dispose();
            gridPen = null;

            bruBK.Dispose();
            bruBK = null;
        }

        private void PaintGroupPlus(Graphics graphics, Pen gridPen, Brush bruBK, DataGridViewCellStyle cellStyle, Rectangle rectLeft, bool collapsed)
        {
            graphics.FillRectangle(bruBK, rectLeft);
            int left = rectLeft.Left + (groupLevel - 1) * PLUS_WIDTH;
            //画 Left, Top, Right 三根线
            graphics.DrawLine(gridPen, left, rectLeft.Top, left, rectLeft.Bottom);
            graphics.DrawLine(gridPen, left, rectLeft.Top, rectLeft.Right, rectLeft.Top);
            graphics.DrawLine(gridPen, rectLeft.Right, rectLeft.Top, rectLeft.Right, rectLeft.Bottom);

            //最左边的一条线
            graphics.DrawLine(gridPen, rectLeft.Left, rectLeft.Top, rectLeft.Left, rectLeft.Bottom);

            //如果是该级别的最后一个节点，则需要画一个底线，以便将整个组封闭起来
            bool drawBottomLine = false;
            for (int i = 1; i < groupLevel; i++)
            {
                graphics.DrawLine(gridPen, rectLeft.Left + i * PLUS_WIDTH, rectLeft.Top
                , rectLeft.Left + i * PLUS_WIDTH, rectLeft.Bottom);

                if (!drawBottomLine && IsLastCell(i))
                {
                    graphics.DrawLine(gridPen, rectLeft.Left + (i - 1) * PLUS_WIDTH, rectLeft.Bottom
                    , rectLeft.Left + groupLevel * PLUS_WIDTH, rectLeft.Bottom);
                    drawBottomLine = true;
                }
            }

            //如果有子结点， 则需要画一个方框, 里面有+号或-号
            if (childCells != null && childCells.Count > 0)
            {
                groupPlusRect = new Rectangle((groupLevel - 1) * PLUS_WIDTH + rectLeft.Left + (PLUS_WIDTH - 12) / 2
                    , rectLeft.Top + (rectLeft.Height - 12) / 2, 12, 12);
                graphics.DrawRectangle(gridPen, groupPlusRect);

                graphics.DrawLine(Pens.Black, groupPlusRect.Left + 3, groupPlusRect.Top + groupPlusRect.Height / 2
                , groupPlusRect.Right - 3, groupPlusRect.Top + groupPlusRect.Height / 2);
                if (collapsed)
                {
                    graphics.DrawLine(Pens.Black, groupPlusRect.Left + groupPlusRect.Width / 2, groupPlusRect.Top + 3
                , groupPlusRect.Left + groupPlusRect.Width / 2, groupPlusRect.Bottom - 3);
                }
            }
        }

        #endregion 绘制节点

        #region 判断

        /// <summary>
        /// 该节点是否为某一级节点的最后一个子结点
        /// </summary>
        /// <param name="level">节点层级</param>
        /// <returns></returns>
        private bool IsLastCell(int level)
        {
            int row = DataGridView.Rows.GetNextRow(RowIndex, DataGridViewElementStates.None);
            if (row == -1)
                return true;
            DataGridViewGroupCell cel = DataGridView.Rows[row].Cells[0] as DataGridViewGroupCell;
            return (cel.GroupLevel == level);
        }

        #endregion 判断

        #region 点击 Cell

        protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
        {
            Rectangle rect = DataGridView.GetCellDisplayRectangle(ColumnIndex, RowIndex, false);
            Point pt = new Point(rect.Left + e.Location.X, rect.Top + e.Location.Y);
            if (groupPlusRect.Contains(pt))
            {
                if (collapsed)
                {
                    Expand();
                }
                else
                {
                    Collapse();
                }
            }
            base.OnMouseDown(e);
        }

        #endregion 点击 Cell

        #region 展开/收起节点

        /// <summary>
        /// 展开节点
        /// </summary>
        public void Expand()
        {
            collapsed = false;
            ShowChild(true);
            base.DataGridView.InvalidateCell(this);
        }

        private void ShowChild(bool show)
        {
            if (childCells == null)
                return;
            foreach (DataGridViewGroupCell cel in childCells)
            {
                if (cel.RowIndex == -1)
                {
                    continue;
                }
                DataGridView.Rows[cel.RowIndex].Visible = show;
                if (!cel.collapsed)
                    cel.ShowChild(show);
            }
        }

        /// <summary>
        /// 收起节点
        /// </summary>
        public void Collapse()
        {
            collapsed = true;
            ShowChild(false);
            base.DataGridView.InvalidateCell(this);
        }

        /// <summary>
        /// 展开节点及子结点
        /// </summary>
        public void ExpandAll()
        {
            if (childCells == null)
                return;
            foreach (DataGridViewGroupCell cel in childCells)
            {
                cel.Expand();
                cel.ExpandAll();
            }
        }

        #endregion 展开/收起节点
    }

    public static class ExtensionMethods
    {
        public static void DoubleBuffered(this DataGridView dgv, bool setting)

        {

            Type dgvType = dgv.GetType();

            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",

            BindingFlags.Instance | BindingFlags.NonPublic);

            pi.SetValue(dgv, setting, null);

        }
    }
}