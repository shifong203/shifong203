﻿using System;
using System.Windows.Forms;

namespace Vision2.捷普
{
    public partial class CXAMForm : Form
    {
        public CXAMForm()
        {
            InitializeComponent();
        }

        private void CXAMForm_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = MForm.RestMesEnb;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            MForm.RestMesEnb = checkBox1.Checked;
        }
    }
}