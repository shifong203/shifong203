using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace Vision2.ErosProjcetDLL.Project
{
    public class INodeNew
    {
        [DescriptionAttribute("定义名称。"), Category("信息"), DisplayName("名称")]
        public virtual string Name { get; set; } = "";

        public object NewProjectNode(string name = null)
        {
            if (name == null)
            {
                return this;
            }
            Name = name;
            return this;
        }

        public TreeNode NewNodeProject(List<string> listAxes, string name = null, TreeView treeView = null)
        {
            if (name == null)
            {
                name = ProjectINI.GetStrReturnStr(listAxes[listAxes.Count]);
                //;
            }
            TreeNode treeNode = NewTreeNodeProject(listAxes, name);

            if (treeNode != null)
            {
                treeNode.Tag = NewProjectNode(treeNode.Text);
            }
            return treeNode;
        }

        /// <summary>
        /// 新建对象，并返回节点
        /// </summary>
        /// <param name="listna"></param>
        /// <returns></returns>
        public static TreeNode NewTreeNodeProject(List<string> listAxes, string name)
        {
            string nameStr = Interaction.InputBox("请输入点名称", "新建" + name, name, 100, 100);
        str:
            if (nameStr == "")
            {
                return null;
            }
            if (listAxes.Contains(nameStr))
            {
                nameStr = Interaction.InputBox("请重新输入名称", "名称已存在", nameStr, 100, 100);
                goto str;
            }

            TreeNode treeNod = new TreeNode();
            treeNod.Name = treeNod.Text = nameStr;
            //treeNod.Tag = NewProjectNode(nameStr);
            return treeNod;
        }

        public void OnAlRam(string text)
        {
            if (text == "")
            {
                return;
            }
            Vision2.ErosProjcetDLL.Project.AlarmListBoxt.AddAlarmText(new Vision2.ErosProjcetDLL.Project.AlarmText.alarmStruct() { Name = Name, Text = text, Time = DateTime.Now.ToLongDateString() });
        }

        public virtual void Reset()
        {
            Vision2.ErosProjcetDLL.Project.AlarmListBoxt.RomveAlarm(this.Name);
        }

        public virtual void Remove(TreeNode treeNode)
        {
            treeNode.Remove();
        }
    }
}