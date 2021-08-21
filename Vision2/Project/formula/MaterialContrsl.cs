using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Vision2.Project.formula
{
    public partial class MaterialContrsl : UserControl
    {
        public MaterialContrsl()
        {
            InitializeComponent();
        }

        public MaterialContrsl(List<MaterialManagement> materialManagement) : this()
        {
            MaterialMan = materialManagement;
            for (int i = 0; i < MaterialMan.Count; i++)
            {
                TreeNode treeNode = new TreeNode();
                treeNode.Name = treeNode.Text = MaterialMan[i].Name;
                treeNode.Tag = MaterialMan[i];
                treeView1.Nodes.Add(treeNode);
            }
            this.Tag = MaterialMan;
        }

        private List<MaterialManagement> MaterialMan;

        private void MaterialContrsl_Load(object sender, EventArgs e)
        {
        }

        private void MaterialContrsl_Leave(object sender, EventArgs e)
        {
            this.Tag = MaterialMan;
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
                        value = new List<MaterialManagement>();
                    }
                    List<MaterialManagement> materialManagements = value as List<MaterialManagement>;
                    if (materialManagements != null)
                    {
                        MaterialContrsl linkNamesControl = new MaterialContrsl(materialManagements);
                        service.DropDownControl(linkNamesControl);
                        if (linkNamesControl.Tag != null)
                        {
                            value = linkNamesControl.Tag;
                        }
                    }
                }
                return value;
            }
        }

        private void 新建物料ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                MaterialManagement materialManagement = new MaterialManagement();
                List<string> vs = new List<string>();
                for (int i = 0; i < MaterialMan.Count; i++)
                {
                    vs.Add(MaterialMan[i].Name);
                }
                TreeNode treeNode = materialManagement.NewNodeProject(vs, "新建物料1");
                if (treeNode != null)
                {
                    treeView1.Nodes.Add(treeNode);
                    MaterialMan.Add(materialManagement);
                }
            }
            catch (Exception)
            {
            }
        }

        private void 删除物料ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (treeView1.SelectedNode != null)
                {
                    MaterialMan.Remove(treeView1.SelectedNode.Tag as MaterialManagement);
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
                TreeNode treeNode = treeView1.GetNodeAt(e.Location);
                if (treeNode != null)
                {
                    propertyGrid1.SelectedObject = treeNode.Tag;
                }
            }
            catch (Exception)
            {
            }
        }
    }
}