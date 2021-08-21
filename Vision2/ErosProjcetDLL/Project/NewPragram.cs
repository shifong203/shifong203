using System.Collections.Generic;
using System.Windows.Forms;

namespace Vision2.ErosProjcetDLL.Project
{
    public partial class NewPragram : Form
    {
        public NewPragram()
        {
            InitializeComponent();
            this.listBox2.Items.Clear();
            dss = ProjectINI.In.GetListRun();
            listBox1.Items.Clear();

            foreach (var item in dss)
            {
                this.listBox2.Items.Add(item.Key);
            }
            if (this.listBox2.Items.Count > 0)
            {
                this.listBox2.SelectedIndex = 0;
            }
        }

        private Dictionary<string, ProjectObj> dss;
        public string NewName = "";

        private void button1_Click(object sender, System.EventArgs e)
        {
            NewName = textBox1.Text;

            this.Close();
        }

        private void listBox2_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (this.listBox2.SelectedItem != null)
            {
                if (listBox2.Items[listBox2.SelectedIndex] != null)
                {
                    ProjectObj project = dss[listBox2.SelectedItem.ToString()];
                    listBox1.Items.Clear();
                    foreach (var item in project.ProjectClass)
                    {
                        listBox1.Items.Add(item.Key);
                    }
                    this.richTextBox1.Text = project.Information;
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (this.listBox1.SelectedItem != null)
            {
                if (listBox1.Items[listBox1.SelectedIndex] != null)
                {
                    ProjectObj project = dss[listBox2.SelectedItem.ToString()];
                    if (project.ProjectClass.ContainsKey(listBox1.SelectedItem.ToString()))
                    {
                        //this.richTextBox1.Text = project.ProjectClass[listBox1.SelectedItem.ToString()].;
                    }
                }
            }
        }
    }
}