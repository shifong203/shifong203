﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace Vision2.Project.formula
{
    public partial class DataButtenModeU : UserControl
    {
        public DataButtenModeU()
        {
            InitializeComponent();
            button6.Enabled = button10.Enabled = false;
        }

        private Mes.OneDataVale dataVale;

        public void Set(Mes.OneDataVale data)
        {
            dataVale = data;
            groupBox1.Text = dataVale.TrayLocation.ToString();
            label1.BackColor = Color.Transparent;
            label1.Text = dataVale.TrayLocation.ToString() + "-";
            if (data.NotNull)
            {
                if (!data.Done)
                {
                    if (data.AutoOK)
                    {
                        label1.Text += "OK";
                        label1.BackColor = Color.Green;
                    }
                    else
                    {
                        button6.Enabled = button10.Enabled = true;
                        label1.Text += "NG";
                        label1.BackColor = Color.Red;
                    }
                }
                else
                {
                    if (data.OK)
                    {
                        label1.Text += "OK";
                        label1.BackColor = Color.Green;
                    }
                    else
                    {
                        label1.Text += "NG";
                        label1.BackColor = Color.Red;
                    }
                }
            }
            label1.Text += "SN:" + dataVale.PanelID;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataVale.PanelID == "")
                {
                    MessageBox.Show("SN未识别");
                    return;
                }
                dataVale.ResetOK();
                groupBox1.Text = dataVale.TrayLocation.ToString();
                label1.Text = "OK" + "SN:" + dataVale.PanelID;
                if (RecipeCompiler.Instance.GetMes() != null)
                {
                    RecipeCompiler.Instance.GetMes().WrietMes(dataVale, Product.ProductionName);
                }
                label1.BackColor = Color.Green;
                button6.Enabled = button10.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                button6.Enabled = button10.Enabled = false;
                dataVale.ResetNG();
                if (RecipeCompiler.Instance.GetMes() != null)
                {
                    RecipeCompiler.Instance.GetMes().WrietMes(dataVale, Product.ProductionName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}