﻿using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Vision2.vision.HalconRunFile.Controls
{
    public partial class OCRTextModeUserContro : UserControl
    {
        public OCRTextModeUserContro(RunProgramFile.IDrawHalcon halcon_, RunProgramFile.Text_Model text_)
        {
            InitializeComponent();
            SetData(halcon_ as HalconRunFile.RunProgramFile.HalconRun, text_);
            propertyGrid1.SelectedObject = text_;
        }

        private HWindID HWindID = new HWindID();
        private RunProgramFile.Text_Model text_Mode;
        private HalconRunFile.RunProgramFile.HalconRun halcon;

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
                    text_Mode.ListhObjects.Add(text_Mode.DrawRectangle2(halcon));
                    //    text_Mode.ListhObjects.Add(RunProgramFile.RunProgram.DrawModOBJ(halcon,RunProgramFile.HalconRun.EnumDrawType.Rectangle1,new HObject()));
                }
                else
                {
                    text_Mode.ListhObjects[listBox1.SelectedIndex] = text_Mode.DrawRectangle2(halcon);

                    //text_Mode.ListhObjects[listBox1.SelectedIndex] = RunProgramFile. RunProgram.DrawModOBJ(halcon,
                    //RunProgramFile.HalconRun.EnumDrawType.Rectangle2, text_Mode.ListhObjects[listBox1.SelectedIndex]);
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

                HOperatorSet.ReduceDomain(halcon.Image(), text_Mode.ListhObjects[listBox1.SelectedIndex], out HObject hObject);
                HOperatorSet.CropDomain(hObject, out hObject);
                HOperatorSet.RotateImage(hObject, out HObject hObject3, 90, "constant");
                HWindID.Image(hObject3);
                //HWindID.SetImaage(hObject3);
                //HOperatorSet.GetImageSize(hObject3, out HTuple widt, out HTuple hiet);
                //HWindID.SetPerpetualPart(0, 0, widt, hiet);
                HWindID.ShowImage();
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
            pretreatmentUserControl1.SetData(text_, halcon_.GetOneImageR());
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

        private bool isCheave = true;

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
                        //HOperatorSet.HomMat2dRotate(home2d, this.Phi[i2], this.Row[i2], this.Column[i2], out home2d);

                        //HOperatorSet.ReduceDomain(halcon.Image(), text_Mode.ListhObjects[listBox1.SelectedIndex], out HObject hObject);
                        //HOperatorSet.CropDomain(hObject, out hObject);
                        //HOperatorSet.AreaCenter(hObject, out HTuple areas, out HTuple rows, out HTuple colues);
                        //HOperatorSet.SmallestRectangle1(hObject,  out HTuple rows1, out HTuple colues1, out HTuple rows2, out HTuple colues2);
                        //HOperatorSet.GenRectangle1(out HObject hObjecttsd, rows1, colues1, rows2, colues2);
                        //HWindID.OneResIamge.AddObj(hObjecttsd);

                        HOperatorSet.HomMat2dRotate(home2d, text_Mode.Phi[listBox1.SelectedIndex],
                          text_Mode.Row[listBox1.SelectedIndex], text_Mode.Column[listBox1.SelectedIndex], out home2d);
                        HOperatorSet.AffineTransImage(image, out HObject hObject3, home2d, "constant", "false");
                        HOperatorSet.AffineTransRegion(hObject2, out HObject hObject1, home2d, "nearest_neighbor");
                        HOperatorSet.ReduceDomain(hObject3, hObject1, out HObject hObject);
                        HObject hObject4 = text_Mode.FindText(hObject, out string text);
                        HOperatorSet.HomMat2dInvert(home2d, out home2d);
                        if (hObject2 != null)
                        {
                            HOperatorSet.SmallestRectangle1(hObject1, out HTuple row1, out HTuple colu1, out HTuple row2, out HTuple colu2);

                            HWindID.SetImaage(hObject3);
                            HWindID.SetPerpetualPart(row1 - 100, colu1 - 100, row2 + 100, colu2 + 100);
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
                    HWindID.ShowImage();
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
            text_Mode.QRMode = checkBox1.Checked;
        }

        private void pretreatmentUserControl1_Load(object sender, EventArgs e)
        {
        }
    }
}