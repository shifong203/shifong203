﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Vision2.ErosProjcetDLL.Project
{
    public partial class ToolForm : Form
    {
        public ToolForm()
        {
            InitializeComponent();
            ThisForm = this;
        }

        public static ToolForm ThisForm;

        private Dictionary<string, List<TreeNode>> keyValuePairs = new Dictionary<string, List<TreeNode>>();

        private void ToolForm_Load(object sender, EventArgs e)
        {
            AddKeyConvert("常用窗体控件", new Button() { Text = "按钮", Name = "Button" });
            AddKeyConvert("常用窗体控件", new Label { Text = "文本显示", Name = "Label" });
            AddKeyConvert("常用窗体控件", new TextBox { Text = "文本框", Name = "TextBox" });
            AddKeyConvert("常用窗体控件", new ComboBox { Text = "下拉框", Name = "ComboBox" });
            AddKeyConvert("常用窗体控件", new ListBox { Text = "集合列表", Name = "ListBox" });

            foreach (var item in keyValuePairs)
            {
                TreeNode treeNodes = new TreeNode();
                treeNodes.Text = item.Key;
                treeNodes.Name = item.Key;
                treeViewTool.Nodes.Add(treeNodes);
                for (int i = 0; i < item.Value.Count; i++)
                {
                    treeNodes.Nodes.Add(item.Value[i]);
                }
            }

            treeViewTool.ExpandAll();
        }

        /// <summary>
        ///添加工具控件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="convert"></param>
        public void AddKeyConvert(string key, Control convert)
        {
            TreeNode treeNodes = new TreeNode();
            treeNodes.ImageIndex = treeNodes.SelectedImageIndex = 1;
            treeNodes.Name = treeNodes.Text = convert.Name;
            treeNodes.Tag = convert;
            if (convert.BackgroundImage != null)
            {
                treeViewTool.ImageList.Images.Add(convert.BackgroundImage);
                treeNodes.SelectedImageIndex = treeNodes.ImageIndex = treeViewTool.ImageList.Images.Count;
            }

            if (keyValuePairs.ContainsKey(key))
            {
                keyValuePairs[key].Add(treeNodes);
            }
            else
            {
                keyValuePairs.Add(key, new List<TreeNode>());
                keyValuePairs[key].Add(treeNodes);
            }
        }

        private void treeViewTool_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void treeViewTool_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                try
                {
                    if (treeViewTool.SelectedNode != null)
                    {
                        DataObject d = new DataObject("Control", treeViewTool.SelectedNode.Tag);
                        treeViewTool.DoDragDrop(d, DragDropEffects.All);
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        public class UIForm
        {
            public static Control NewUI(Control type)
            {
                dynamic obj = type.GetType().Assembly.CreateInstance(type.GetType().ToString());
                return obj;
            }

            private static string FindControl(Control fControl, Type type)
            {
                int i = 1;
                foreach (Control child in fControl.Controls)
                {
                    if (child.GetType() == type)
                    {
                        if ((type.Name + i) == child.Name)
                        {
                            i++;
                        }
                    }
                }
                return type.Name + i;
            }

            /// <summary>
            /// 拖拽放置事件
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            public static void Control_DragDrop(object sender, DragEventArgs e)
            {
                try
                {
                    Control contrForm = sender as Control;
                    Control btn = NewUI((Control)e.Data.GetData("Control"));
                    if (btn != null)
                    {
                        btn.Name = btn.Text = FindControl(contrForm, btn.GetType());
                        btn.Location = contrForm.PointToClient(new Point(e.X, e.Y));
                        contrForm.Controls.Add(btn);
                    }
                }
                catch (Exception)
                {
                }
            }

            public static void Control_DragOver(object sender, DragEventArgs e)
            {
                if (e.AllowedEffect == DragDropEffects.All)
                {
                    e.Effect = DragDropEffects.All;
                }
            }
        }
    }
}