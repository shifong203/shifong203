using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace ErosSocket.ErosConLink
{
    public partial class LinkNameListValueNameUserControl1 : UserControl
    {

        public LinkNameListValueNameUserControl1()
        {
            InitializeComponent();
            dataGridView1.CellValueChanged += dataGridView1_CellValueChanged;
            dataGridView1.CurrentCellDirtyStateChanged += dataGridView1_CurrentCellDirtyStateChanged;
        }
        ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
        LinkNameListValueName listN;
        public LinkNameListValueNameUserControl1(LinkNameListValueName listNameValue) : this()
        {
            foreach (var item2 in StaticCon.GetLingkNmaeValues(listNameValue.LinkName))
            {
                ToolStripMenuItem tool = new ToolStripMenuItem();
                tool.Text = item2;
                tool.Click += Tool_Click;
                contextMenuStrip.Items.Add(tool);
            }

            this.Column1.Items.AddRange(Vision2.ErosProjcetDLL.PLCUI.HMIDIC.This.DicPLCEnum.Keys.ToArray());
            listN = listNameValue;
            for (int i = 0; i < listNameValue.ListName.Count; i++)
            {
                int indx = dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells[0].Value = listNameValue.ListName[i];
                if (listNameValue.ListEnumName.Count >= i)
                {
                    dataGridView1.Rows[i].Cells[1].Value = listNameValue.ListEnumName[i];
                }

            }
        }
        private void LinkNameListValueNameUserControl1_Leave(object sender, EventArgs e)
        {
            try
            {
                listN.ListName.Clear();
                listN.ListEnumName.Clear();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    try
                    {
                        if (dataGridView1.Rows[i].Cells[0].Value != null)
                        {
                            string values = dataGridView1.Rows[i].Cells[0].Value.ToString();
                            if (dataGridView1.Rows[i].Cells[1].Value != null)
                            {
                                listN.ListEnumName.Add(dataGridView1.Rows[i].Cells[1].Value.ToString());
                            }
                            else
                            {
                                listN.ListEnumName.Add("");
                            }

                            listN.ListName.Add(values);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
                this.Tag = listN;
            }
            catch (Exception)
            {


            }
        }

        public class LinkNameListValueName
        {
            public List<string> ListName { get; set; } = new List<string>();
            public List<string> ListEnumName { get; set; } = new List<string>();
            public string LinkName { get; set; } = "";
        }
        public class Editor : UITypeEditor
        {
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.DropDown;
            }
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                IWindowsFormsEditorService service = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
                if (service != null)
                {
                    if (value == null)
                    {
                        value = new LinkNameListValueName();
                    }
                    LinkNameListValueNameUserControl1 linkNamesControl = new LinkNameListValueNameUserControl1(value as LinkNameListValueName);
                    service.DropDownControl(linkNamesControl);
                    if (linkNamesControl.Tag != null)
                    {
                        value = linkNamesControl.Tag;
                    }
                }
                return value;
            }
        }

        private void Tool_Click(object sender, EventArgs e)
        {
            try
            {
                ToolStripMenuItem item = (ToolStripMenuItem)sender;

                string[] text = dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[dataGridView1.CurrentCellAddress.X].Value.ToString().Split(',');
                text[text.Length - 1] = item.Text;
                string texts = "";
                for (int i = 0; i < text.Length; i++)
                {
                    texts += text[i];
                    if (text.Length > i + 1)
                    {
                        texts += ",";
                    }
                }
                if (dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[dataGridView1.CurrentCellAddress.X].Value.ToString().Contains("."))
                {
                    dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[dataGridView1.CurrentCellAddress.X].Value += item.Text;
                }
                else
                {
                    dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[dataGridView1.CurrentCellAddress.X].Value = texts;
                }

                int X = dataGridView1.CurrentCellAddress.X;
                int Y = dataGridView1.CurrentCellAddress.Y;
                if ((Y != -1 && X != -1) && dataGridView1.Rows[Y].Cells[X] != null && dataGridView1.Rows[Y].Cells[X].Value != null)
                {
                    ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
                    string[] keys = dataGridView1.Rows[Y].Cells[X].Value.ToString().Split(',');

                    foreach (var item2 in StaticCon.GetLingkNmaeValues(text[0]))
                    {
                        ToolStripMenuItem tool = new ToolStripMenuItem();
                        tool.Text = item2;
                        tool.Click += Tool_Click;
                        contextMenuStrip.Items.Add(tool);
                    }
                    Rectangle rectangle = dataGridView1.GetCellDisplayRectangle(x, y, false);
                    Rectangle rectangle2 = dataGridView1.RectangleToScreen(rectangle);
                    contextMenuStrip.Show(rectangle2.X, rectangle2.Y + this.dataGridView1.Rows[x].Height);
                }
                dataGridView1.EndEdit();
                dataGridView1.BeginEdit(false);

                //dataGridView1.RefreshEdit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        int y;
        int x;
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {

                try
                {
                    Vision2.ErosProjcetDLL.UI.UICon.GetCursorPos(out Vision2.ErosProjcetDLL.UI.UICon.POINT pOINT);

                    int X = dataGridView1.CurrentCellAddress.X;
                    int Y = dataGridView1.CurrentCellAddress.Y;
                    if ((Y != -1 && X != -1) && dataGridView1.Rows[Y].Cells[X] != null && dataGridView1.Rows[Y].Cells[X].Value != null)
                    {

                        if (!StaticCon.GetLingkNmaeValues(listN.LinkName).Contains(dataGridView1.Rows[Y].Cells[X].Value.ToString()))
                        {
                            Rectangle rectangle = dataGridView1.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                            x = e.ColumnIndex;
                            y = e.RowIndex;
                            Rectangle rectangle2 = dataGridView1.RectangleToScreen(rectangle);
                            contextMenuStrip.Show(rectangle2.X, rectangle2.Y + this.dataGridView1.Rows[e.RowIndex].Height);
                            //foreach (var item in StaticCon.GetLingkNames())
                            //{
                            //    ToolStripMenuItem tool = new ToolStripMenuItem();
                            //    tool.Text = item;
                            //    tool.Click += Tool_Click;
                            //    contextMenuStrip.Items.Add(tool);
                            //}
                        }
                        //    string[] keys = dataGridView1.Rows[Y].Cells[X].Value.ToString().Split('.');
                        //    if (keys.Length == 1)
                        //    {
                        //        if (StaticCon.GetLingkNames().Contains(keys[0]))
                        //        {
                        //            foreach (var item2 in StaticCon.GetLingkNmaeValues(keys[0]))
                        //            {
                        //                ToolStripMenuItem tool = new ToolStripMenuItem();
                        //                tool.Text = item2;
                        //                tool.Click += Tool_Click;
                        //                contextMenuStrip.Items.Add(tool);
                        //            }
                        //        }
                        //        else
                        //        {
                        //            //foreach (var item in StaticCon.GetLingkNames())
                        //            //{
                        //            //    ToolStripMenuItem tool = new ToolStripMenuItem();
                        //            //    tool.Text = item;
                        //            //    tool.Click += Tool_Click;
                        //            //    contextMenuStrip.Items.Add(tool);
                        //            //}
                        //        }
                        //    }
                        //    else
                        //    {
                        //        if (StaticCon.GetLingkNames().Contains(keys[0]) || StaticCon.GetLingkNmaeValues(keys[0]).Contains(keys[1]))
                        //        {
                        //            return;
                        //        }
                        //    }
                        //}
                        //else
                        //{

                        //}

                        //contextMenuStrip.Show(pOINT.X, pOINT.Y);
                    }
                }
                catch (Exception es)
                {
                }
            }

        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

    }
}
