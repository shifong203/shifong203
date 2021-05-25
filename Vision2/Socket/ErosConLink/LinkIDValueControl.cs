using System;
using System.Windows.Forms;

namespace ErosSocket.ErosConLink
{
    public partial class LinkIDValueControl : UserControl
    {
        public LinkIDValueControl()
        {
            InitializeComponent();
            foreach (var item in DicSocket.Instance.SocketClint.Keys)
            {
                this.comboBox1.Items.Add(item);
            }
            this.comboBox2.Items.AddRange(UClass.GetTypeList().ToArray());
        }
        public LinkIDValueControl(string valuet) : this()
        {
            try
            {
                if (valuet != "")
                {
                    string[] dats = valuet.Split('.');
                    this.comboBox1.SelectedItem = dats[0];
                    if (dats.Length > 1)
                    {
                        this.textBox1.Text = valuet.Remove(0, dats[0].Length + 1);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public LinkIDValueControl(UClass.PLCValue pLC) : this()
        {
            Up(pLC);
        }
        UClass.PLCValue pL;
        private void LinkIDValueControl_Load(object sender, EventArgs e)
        {

        }
        public void Up(UClass.PLCValue pLC)
        {
            try
            {

                pL = pLC;
                this.comboBox1.SelectedItem = pLC.LinkName;
                this.comboBox2.SelectedItem = pLC.TypeStr;
                ErosConLink.StaticCon.GetLingkIDValue(pLC, out dynamic value);

                if (pLC.TypeStr == UClass.Boolean)
                {
                    if (value != null)
                    {
                        button1.Text = value.ToString();
                    }

                    button1.Visible = true;
                    button2.Visible = textBox2.Visible = false;
                }
                else
                {
                    if (value != null)
                    {
                        textBox2.Text = value.ToString();
                    }
                    button2.Visible = textBox2.Visible = true;
                    button1.Visible = false;
                }
                this.textBox1.Text = pLC.Addrea;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (button1.Text == true.ToString())
                {
                    StaticCon.SetLinkAddressValue(comboBox1.SelectedItem.ToString() + "." + textBox1.Text, comboBox2.SelectedItem.ToString(), false.ToString());
                }
                else
                {
                    StaticCon.SetLinkAddressValue(comboBox1.SelectedItem.ToString() + "." + textBox1.Text, comboBox2.SelectedItem.ToString(), true.ToString());
                }
                ErosConLink.StaticCon.GetLingkIDValue(comboBox1.SelectedItem.ToString() + "." + textBox1.Text, comboBox2.SelectedItem.ToString(), out dynamic value);
                if (pL.TypeStr == UClass.Boolean)
                {
                    button1.Text = value.ToString();
                    button1.Visible = true;
                    textBox2.Visible = false;
                }
                else
                {
                    textBox2.Text = value.ToString();
                    textBox2.Visible = true;
                    button1.Visible = false;
                }
            }
            catch (Exception)
            {
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception)
            {
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                StaticCon.SetLinkAddressValue(comboBox1.SelectedItem.ToString() + "." + textBox1.Text, comboBox2.SelectedItem.ToString(), textBox2.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pL != null)
            {
                pL.LinkName = comboBox1.SelectedItem.ToString();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pL != null)
            {
                pL.TypeStr = comboBox2.SelectedItem.ToString();
                if (pL.TypeStr == UClass.Boolean)
                {

                    button1.Visible = true;
                    textBox2.Visible = false;
                }
                else
                {

                    textBox2.Visible = true;
                    button1.Visible = false;
                }
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (pL != null)
            {
                pL.Addrea = textBox1.Text;
            }
        }
    }
}
