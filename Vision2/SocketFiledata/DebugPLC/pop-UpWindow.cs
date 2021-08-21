using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ErosSocket.DebugPLC
{
    public partial class Pop_UpWindow : Form
    {
        public enum Pop_UpWindows
        {
        }

        public Pop_UpWindow()
        {
            InitializeComponent();
            //if (ErosConLink.DicSocket.Instance.LinkS!=null && ErosConLink.DicSocket.Instance.LinkS=="")
            //{
            //    checkBox1.Visible = false;
            //}

            TopMost = true;
            this.textBox1.Visible = this.groupBox1.Visible = false;
        }

        /// <summary>
        /// 弹出显示信息
        /// </summary>
        /// <param name="text">标题</param>
        /// <param name="FormText">编译文本</param>
        public Pop_UpWindow(string text, string formText) : this()
        {
            //this.Text = FormText;
            this.textBox1.Text = formText;
            this.label2.Text = text;
            //this.Show();
        }

        public void UpWindow(string text, string formText)
        {
            try
            {
                this.Text = formText;
                this.label2.Text = text;
                this.button2.Visible = true;
                button2.Enabled = false;

                Vision2.ErosProjcetDLL.UI.UICon.SwitchToThisWindow(this.Handle, true);
                TopMost = true;
                this.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void UpWindow(string text, string formText, MessageBoxButtons buttons)
        {
            try
            {
                this.Text = formText;
                this.textBox1.Text = formText;
                this.label2.Text = text;
                this.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool cl;
        private int tiemst;
        private int tiemAdd;

        public void UpWindow(string text, string formText, List<string> listItems, string seleItem = null, int tiems = 0)
        {
            try
            {
                checkedListBox1.ItemCheck += CheckedListBox1_ItemCheck;
                this.textBox1.Visible = this.groupBox1.Visible = true;
                for (int i = 0; i < listItems.Count; i++)
                {
                    checkedListBox1.Items.Add(listItems[i]);
                }
                if (tiems < 10)
                {
                    tiems = 10;
                }
                tiemAdd = tiemst = tiems;
                button2.Visible = true;
                if (seleItem != null)
                {
                    if (!listItems.Contains(seleItem))
                    {
                        checkedListBox1.Items.Add(seleItem);
                    }
                    checkedListBox1.SelectedItem = seleItem;
                    checkedListBox1.SetItemChecked(checkedListBox1.SelectedIndex, true);
                    if (tiems >= 5)
                    {
                        timer1.Tick += Timer1_Tick;
                        timer1.Interval = 1000;
                        timer1.Start();
                        label1.Text = "默认处理倒计时:" + tiemAdd;
                        label1.Visible = true;
                        //stopwatch = new System.Diagnostics.Stopwatch();
                        //label1.Text = "默认处理倒计时:" + (tiems - (int)(stopwatch.ElapsedMilliseconds / 1000));
                        //stopwatch.Restart();
                        //Task.Run(() => {
                        //});
                    }
                }
                UpWindow(text, formText);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                cl = false;
                tiemAdd--;

                label1.Text = "默认处理倒计时:" + tiemAdd;

                if (cl)
                {
                    return;
                }
                if (tiemAdd < 0)
                {
                    this.button1.PerformClick();
                    cl = true;
                    return;
                }
                timer1.Start();
            }
            catch (Exception)
            {
            }
        }

        private void CheckedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            try
            {
                //if (e.NewValue == CheckState.Unchecked)
                //{
                //    e.NewValue = CheckState.Checked;
                //    return;
                //}
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                {
                    if (e.Index != i)
                    {
                        checkedListBox1.SetItemChecked(i, false);
                    }
                }
                if (e.NewValue == CheckState.Unchecked)
                {
                    button1.Enabled = false;
                }
                else
                {
                    button1.Enabled = true;
                }
            }
            catch (Exception)
            {
            }
        }

        public static DialogResult ShowDialog(string text, string FormText, MessageBoxButtons buttons, MessageBoxIcon BoxIcon)
        {
            Pop_UpWindow pop_UpWindow = new Pop_UpWindow();
            pop_UpWindow.textBox1.Text = FormText;
            pop_UpWindow.label2.Text = text;
            return pop_UpWindow.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cl = true;
            timer1.Stop();
            if (checkedListBox1.CheckedItems.Count > 0)
            {
                this.Tag = checkedListBox1.CheckedItems[0].ToString();
            }
            else
            {
                this.Tag = "Yes";
            }
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            cl = true;
            this.Tag = "Cancel";
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void Pop_UpWindow_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.F12)
                {
                    if (checkBox1.Checked)
                    {
                        checkBox1.Checked = false;
                    }
                    else
                    {
                        checkBox1.Checked = true;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //if (!checkBox1.Checked)
            //{
            //    ErosSocket.ErosConLink.DicSocket.Instance.SetLinkSTime(false);
            //}
            //else
            //{
            //    ErosSocket.ErosConLink.DicSocket.Instance.SetLinkSTime(true);
            //}
        }
    }
}