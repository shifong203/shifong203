using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision2.Project.formula;

namespace Vision2.Project.Mes
{
    public partial class DefaultMesForm1 : Form
    {
        public DefaultMesForm1()
        {
            InitializeComponent();
        }
        MesInfon mesInfon;
        public DefaultMesForm1(DefaultMes defaultMes ):this()
        {
            mesInfon = defaultMes;
        }
        private void DefaultMesForm1_Load(object sender, EventArgs e)
        {
            propertyGrid2.SelectedObject = mesInfon;
        }
        private string[] imagepsht = new string[] { };
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                listBox2.Items.Clear();
                richTextBox5.Text = "";
                richTextBox3.Text = "";
                richTextBox4.Text = "";
                richTextBox3.AppendText(",位号,结果,SN     ,机检,  MES    ， FVT，" + Environment.NewLine);
                string dataTime = dateTimePicker1.Value.ToString("D");
                string[] files = new string[] { };
                if (Directory.Exists(vision.Vision.GetSaveImageInfo(vision.Vision.GetRunNameVision().Name).SavePath + "\\" + dataTime))
                {
                    imagepsht = ErosProjcetDLL.Project.ProjectINI.GetFilesArrayPath(
                        vision.Vision.GetSaveImageInfo(vision.Vision.GetRunNameVision().Name).SavePath + "\\" + dataTime,
                        vision.Vision.GetSaveImageInfo(vision.Vision.GetRunNameVision().Name).SaveImageType);
                    for (int i = 0; i < imagepsht.Length; i++)
                    {
                        if (imagepsht[i].Contains(textBox2.Text))
                        {
                            listBox2.Items.Add(Path.GetFileNameWithoutExtension(imagepsht[i]));
                        }
                    }
                }

                if (Directory.Exists(RecipeCompiler.Instance.DataPaht + "//Mes记录//" + dataTime))
                {
                    files = Directory.GetFiles(RecipeCompiler.Instance.DataPaht + "//Mes记录//" + dataTime);
                    List<string> mestPahts = new List<string>();

                    for (int i = 0; i < files.Length; i++)
                    {
                        if (files[i].Contains(textBox2.Text))
                        {
                            string[] txte = File.ReadAllLines(files[i]);
                            richTextBox5.AppendText(files[i] + Environment.NewLine);
                            foreach (var item in txte)
                            {
                                richTextBox5.AppendText(item + Environment.NewLine);
                            }
                        }
                    }
                }
                if (Directory.Exists(RecipeCompiler.Instance.DataPaht + "//FVT//" + dataTime))
                {
                    files = Directory.GetFiles(RecipeCompiler.Instance.DataPaht + "//FVT//" + dataTime);
                    for (int i = 0; i < files.Length; i++)
                    {
                        bool iscd = false;
                        string[] txte = File.ReadAllLines(files[i]);
                        for (int j = 0; j < txte.Length; j++)
                        {
                            if (txte[j].Contains(textBox2.Text))
                            {
                                iscd = true;
                            }
                        }
                        if (iscd)
                        {
                            for (int j = 0; j < txte.Length; j++)
                            {
                                richTextBox4.AppendText(txte[j] + Environment.NewLine);
                            }
                        }
                    }
                }

                if (File.Exists(RecipeCompiler.Instance.DataPaht + "//" + dataTime + ".csv"))
                {
                    string[] txte = File.ReadAllLines(RecipeCompiler.Instance.DataPaht + "//" + dataTime + ".csv");
                    List<string> dasts = new List<string>();
                    bool isdta = false;
                    for (int i = 0; i < txte.Length; i++)
                    {
                        if (isdta)
                        {
                            if (!txte[i].StartsWith(" ,"))
                            {
                                foreach (var item in dasts)
                                {
                                    richTextBox3.AppendText(item + Environment.NewLine);
                                }
                                isdta = false;
                            }
                        }
                        else
                        {
                            if (!txte[i].StartsWith(" ,"))
                            {
                                if (dasts.Count > 1)
                                {
                                    dasts.Clear();
                                }
                            }
                        }
                        dasts.Add(txte[i]);
                        if (txte[i].Contains(textBox2.Text))//JSH21342A7BD
                        {
                            isdta = true;
                            //richTextBox3.AppendText(txte[i] + Environment.NewLine);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                mesInfon.ReadMes(textBox1.Text, out string mesData);
                richTextBox1.AppendText(mesData+Environment.NewLine);
            }
            catch (Exception)
            {

          
            }
        }
    }
}
