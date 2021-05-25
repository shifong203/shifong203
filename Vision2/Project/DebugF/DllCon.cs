using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;

namespace Vision2.Project.DebugF
{
    public partial class DllCon : System.Windows.Forms.UserControl
    {
        public DllCon(List<DllUers> dllUers)
        {
            InitializeComponent();

            DllS = dllUers;
            UpData();
        }
        List<DllUers> DllS;
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedItem == null)
                {
                    return;
                }
                foreach (var item in DllS)
                {
                    if (listBox1.SelectedItem.ToString() == item.Name)
                    {
                        item.LoadDll(textBox3.Text);
                        return;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Dll文件|*.dll|exe文件|*.exe";
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != "")
            {
                foreach (var item in DllS)
                {
                    if (item.Name == Path.GetFileName(openFileDialog.FileName))
                    {
                        MessageBox.Show(openFileDialog.FileName + "文件已存在");
                        return;
                    }
                }
                DllUers dllUers = new DllUers();
                dllUers._Path = openFileDialog.FileName;
                dllUers.Name = Path.GetFileName(openFileDialog.FileName);
                DllS.Add(dllUers);
                UpData();
            }
        }
        void UpData()
        {
            listBox1.Items.Clear();
            foreach (var item in DllS)
            {
                listBox1.Items.Add(item.Name);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                return;
            }
            foreach (var item in DllS)
            {
                if (listBox1.SelectedItem.ToString() == item.Name)
                {
                    if (textBox2.Text == "")
                    {
                        item.MetHod(textBox1.Text, null);
                    }
                    else
                    {
                        item.MetHod(textBox1.Text, textBox2.Text.Split(','));
                    }

                    return;
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                return;
            }
            foreach (var item in DllS)
            {
                if (listBox1.SelectedItem.ToString() == item.Name)
                {
                    propertyGrid1.SelectedObject = item.GetDllS();
                    return;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                return;
            }
            foreach (var item in DllS)
            {
                if (listBox1.SelectedItem.ToString() == item.Name)
                {
                    propertyGrid1.SelectedObject = item.GetObjDll();
                    ToolForm.ThisForm.AddKeyConvert(item.Name, item.GetObjDll());
                    return;
                }
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
    }
}
