﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ErosSocket.DebugPLC
{
    public partial class PLCIntEnumUserControl : UserControl
    {
        Dictionary<string, PLCIntEnum> item;
        public PLCIntEnumUserControl(Dictionary<string, PLCIntEnum>  dicPLCINT)
        {
            InitializeComponent();
            item = dicPLCINT;
            treeView1.ContextMenuStrip = new ContextMenuStrip();
            dataGridView1.ContextMenuStrip = contextMenuStrip1;
            foreach (var item in dicPLCINT)
            {
                TreeNode treeNod = new TreeNode();
                treeNod.Name = treeNod.Text = item.Key;
                treeNod.Tag = item.Value;
                treeView1.Nodes.Add(treeNod);
            }
            treeView1.ContextMenuStrip.Items.Add("添加枚举").Click += PLCIntEnumUserControl_Click;

        }

        private void PLCIntEnumUserControl_Click(object sender, EventArgs e)
        {
            try
            {
                string sd = Microsoft.VisualBasic.Interaction.InputBox("请输入表名！", "创建变量表", "", 100, 100);
                strat:
                if (sd.Length == 0)
                {
                    return;
                }
                if (item.ContainsKey(sd))
                {
                    sd = Microsoft.VisualBasic.Interaction.InputBox("变量表已存在，请重新输入！", "创建变量表", "", 100, 100);
                    goto strat;
                }
                if (sd!="")
                {
                    item.Add(sd, new PLCIntEnum());
                    TreeNode treeNode = new TreeNode();
                    treeNode.Text = treeNode.Name = sd;
                    treeNode.Tag = item[sd];
                    treeView1.Nodes.Add(treeNode);
                }

            }
            catch (Exception)
            {

            }
        }


        private void 导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Exlce文件|*xls;*.xlsx|Xml文件|*.Xml";
                openFileDialog.ShowDialog();
                if (openFileDialog.FileName!="")
                {
                    ErosProjcetDLL.Excel.Npoi.DataGridViewExportExcel(openFileDialog.FileName, treeView1.SelectedNode.Text, dataGridView1);
                }

            }
            catch (Exception)
            {


            }
        }

        private void 导入Exl表格ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Exlce文件|*xls;*.xlsx|Xml文件|*.Xml";
                openFileDialog.ShowDialog();
                if (openFileDialog.FileName != "")
                {
                    dataGridView1.Rows.Clear();
                    ErosProjcetDLL.Excel.Npoi.UpDataExclec(openFileDialog.FileName, dataGridView1);
             
                    if (treeView1.SelectedNode.Tag is PLCIntEnum)
                    {
                        PLCIntEnum pLCIntEnum = (PLCIntEnum)treeView1.SelectedNode.Tag;
                        pLCIntEnum.SetDataGiev(dataGridView1);
                    }
     
                }
            }
            catch (Exception)
            {

             
            }
        }

        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                TreeNode getNode = treeView1.GetNodeAt(e.X, e.Y);
                if (getNode.Tag is PLCIntEnum)
                {
                    PLCIntEnum pLCIntEnum = (PLCIntEnum)getNode.Tag;
                    pLCIntEnum.UpDataGiev(dataGridView1);
                }

            }
            catch (Exception)
            {
            }
        }
    }
}