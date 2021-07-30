using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static Vision2.vision.HalconRunFile.RunProgramFile.RunProgram;
using static Vision2.vision.Vision;

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class OCRTextModeUserContro : UserControl
    {
        public OCRTextModeUserContro(RunProgramFile.HalconRun halcon_, RunProgramFile.Text_Model text_)
        {
            InitializeComponent();
            SetData(halcon_, text_);
            propertyGrid1.SelectedObject = text_;

        }
        HWindID HWindID = new HWindID();
        RunProgramFile.Text_Model text_Mode;
        HalconRunFile.RunProgramFile.HalconRun halcon;

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedIndex < 0)
                {
                    listBox1.SelectedIndex = 0;
                }
                if (text_Mode.ListhObjects.Count == 0)
                {
                    text_Mode.ListhObjects.Add(text_Mode.DrawHomObj(halcon));
                }
                else
                {
                    text_Mode.ListhObjects[listBox1.SelectedIndex] = text_Mode.DrawHomObj(halcon, listBox1.SelectedIndex);
                }

            }
            catch (Exception)
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {


                text_Mode.GetHomObj(halcon);


            }
            catch (Exception ex)
            {


            }

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
        public void SetData(RunProgramFile.HalconRun halcon_, RunProgramFile.Text_Model text_)
        {
            isCheave = true;
            text_Mode = text_;
            halcon = halcon_;
            checkBox1.Checked = text_Mode.QRMode;
            HWindID.Initialize(hWindowControl1);
            pretreatmentUserControl1.SetData(text_, HWindID);
            textBox1.Text = text_Mode.ModeText;
            listBox1.Items.Clear();
            for (int i = 0; i < text_Mode.ListhObjects.Count; i++)
            {
                listBox1.Items.Add(i + 1);
            }
            isCheave = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                text_Mode.CreateTextModt(halcon);
            }
            catch (Exception ex)
            {
            }

        }
        bool isCheave = true;
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                halcon.HobjClear();
            
                try
                {
                    HWindID.HobjClear();

                    HWindID.GetHWindowControl().ClearWindow();

                    HWindID.GetHWindowControl().SetColor("red");
                    HOperatorSet.SetDraw(HWindID.GetHWindowControl(), "margin");
                    text_Mode.SetParam();
                    //HOperatorSet.HomMat2dIdentity(out HTuple home2d);
                    //HOperatorSet.HomMat2dRotate(home2d, text_Mode.Phi[listBox1.SelectedIndex], text_Mode.Row[listBox1.SelectedIndex], text_Mode.Column[listBox1.SelectedIndex], out home2d);
                    HObject image = new HObject();
                    image = text_Mode.GetEmset(halcon.Image());

                    List<HTuple> listh = text_Mode.GetHomMatList(halcon.GetOneImageR());
                    for (int i = 0; i < listh.Count; i++)
                    {
                        HOperatorSet.AffineTransRegion(text_Mode.ListhObjects[listBox1.SelectedIndex], out HObject hObject2, listh[i], "nearest_neighbor");
                        HTuple home2d = listh[i];
                        halcon.AddObj(hObject2);
                        HOperatorSet.HomMat2dRotate(home2d, text_Mode.Phi[listBox1.SelectedIndex], text_Mode.Row[listBox1.SelectedIndex], text_Mode.Column[listBox1.SelectedIndex], out home2d);
                        HOperatorSet.AffineTransImage(image, out HObject hObject3, home2d, "constant", "false");
                        HOperatorSet.AffineTransRegion(hObject2, out HObject hObject1, home2d, "nearest_neighbor");
                        HOperatorSet.ReduceDomain(hObject3, hObject1, out HObject hObject);
                        HObject hObject4 = text_Mode.FindText(hObject, out string text);
                        HOperatorSet.HomMat2dInvert(home2d, out home2d);
                        if (hObject2 != null)
                        {
                            HWindID.SetImaage(hObject);
                            HWindID.OneResIamge.AddObj(hObject1);
                            HWindID.OneResIamge.AddObj(hObject4);
                            HOperatorSet.AreaCenter(hObject4, out HTuple area, out HTuple row, out HTuple column);
                            string textStr = "";
                            for (int i2 = 0; i2 < row.Length; i2++)
                            {
                                textStr = text[i2].ToString();
                                HWindID.OneResIamge.AddImageMassage(row[i2] - 100, column[i2], textStr, ColorResult.blue, "true");
                            }
                        }

                    }

                    halcon.ShowObj();

                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.Message);
                }
            }
            catch (Exception)
            {

            }

        }

        private void 删除区域ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                text_Mode.ListhObjects.RemoveAt(listBox1.SelectedIndex);
                text_Mode.Row.TupleRemove(listBox1.SelectedIndex);
                text_Mode.Column.TupleRemove(listBox1.SelectedIndex);
                text_Mode.Phi.TupleRemove(listBox1.SelectedIndex);
                text_Mode.Length1.TupleRemove(listBox1.SelectedIndex);
                text_Mode.Length2.TupleRemove(listBox1.SelectedIndex);

                listBox1.Items.RemoveAt(listBox1.SelectedIndex);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 添加区域ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            text_Mode.ListhObjects.Add(new HObject());
            listBox1.Items.Add(listBox1.Items.Count + 1);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            text_Mode.ModeText = textBox1.Text;
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (isCheave)
            {
                return;
            }
            text_Mode. QRMode =checkBox1.Checked;

        }
    }
}
