using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;
using Vision2.Project.DebugF.IO;
using Vision2.vision.RestVisionForm;

namespace Vision2.vision
{
    public partial class FormRestfDataIamge : Form
    {
        public FormRestfDataIamge()
        {
            InitializeComponent();
        }

        public Dictionary<string, OBJData> keyValuePairs = new Dictionary<string, OBJData>();

        public HWindID HWind = new HWindID();

        /// <summary>
        /// 远程
        /// </summary>
        public class OBJData
        {
            public HObject ImageMode = new HObject();
            public Socket socket;
            public PrestC prest1;
            public TrayDataUserControl trayDataUser;

            public bool ReadPCImage(string path)
            {
                try
                {
                    if (ErosSocket.ErosConLink.StaticCon.connectState(prest1.PCNamePath, prest1.LogeName, prest1.LogePass))
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                }
                return false;
            }
        }

        public Queue<PrestImageData> tsLst = new Queue<PrestImageData>();

        public void SetData(OBJData OBJDat)
        {
            try
            {
                if (OBJDat != null)
                {
                    if (OBJDat.ReadPCImage(OBJDat.prest1.PCNamePath))
                    {
                        if (System.IO.File.Exists(OBJDat.prest1.PCNamePath + OBJDat.prest1.ImageModePath))
                        {
                            HOperatorSet.ReadImage(out OBJDat.ImageMode, OBJDat.prest1.PCNamePath + OBJDat.prest1.ImageModePath);
                        }
                    }
                    HWind.SetImaage(OBJDat.ImageMode);
                    HWind.ShowObj();
                    HOperatorSet.GetImageSize(OBJDat.ImageMode, out width, out height);
                    HWind.HobjClear();
                    HWind.SetImaage(OBJDat.ImageMode);
                    this.Text = "线体:" + OBJDat.prest1.LineName + "产品:" + OBJDat.prest1.Name;
                }
            }
            catch (Exception)
            {
            }
        }

        public HObject ImageNG;

        public void AddText(string text)
        {
            this.Invoke(new Action(() =>
            {
                richTextBox1.AppendText(text + Environment.NewLine);
            }));
        }

        private void FormRestfDataIamge_Load(object sender, EventArgs e)
        {
            try
            {
                this.WindowState = FormWindowState.Maximized;
                HWind.Initialize(hWindowControl1);
                HOperatorSet.SetDraw(hWindowControl2.HalconWindow, "margin");
                HOperatorSet.SetLineWidth(hWindowControl2.HalconWindow, Vision.Instance.LineWidth);
                Vision.SetFont(hWindowControl2.HalconWindow);
                Vision.SetFont(hWindowControl3.HalconWindow);
                HOperatorSet.SetDraw(hWindowControl3.HalconWindow, "margin");
                HOperatorSet.SetLineWidth(hWindowControl3.HalconWindow, Vision.Instance.LineWidth);
                Thread thread = new Thread(() =>
                {
                    while (true)
                    {
                        try
                        {
                            if (data != null)
                            {
                                if (data.Done)
                                {
                                    if (tsLst.Count != 0)
                                    {
                                        data = tsLst.Peek();
                                        HWind.HobjClear();
                                        if (!keyValuePairs[data.LinkName].ReadPCImage(keyValuePairs[data.LinkName].prest1.PCNamePath))
                                        {
                                            AddText(data.LinkName + ":连接！" + data.ImagePath);
                                        }

                                        if (System.IO.File.Exists(data.ImagePath + ".jpg"))
                                        {
                                            HOperatorSet.ReadImage(out ImageNG, data.ImagePath + ".jpg");
                                        }
                                        HWind.SetImaage(ImageNG);
                                    }
                                    listBox1.Items.Clear();
                                    List<string> vs = new List<string>();
                                    for (int i = 0; i < tsLst.Count; i++)
                                    {
                                        vs.Add(tsLst.ToArray()[i].LinkName + "." + tsLst.ToArray()[i].PaleSN);
                                    }
                                    listBox1.Items.AddRange(vs.ToArray());
                                }
                                else
                                {
                                    upd();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.StackTrace);
                        }
                        Thread.Sleep(10);
                    }
                });
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception)
            {
            }
        }

        public ErosSocket.DebugPLC.Robot.TrayRobot trayRobot;
        private HTuple width;
        private HTuple height;

        private PrestImageData data;

        public void SetImageData(PrestImageData prest)
        {
            try
            {
                if (prest != null)
                {
                    if (data == null)
                    {
                        data = prest;
                    }

                    tsLst.Enqueue(prest);
                    listBox1.Items.Clear();
                    List<string> vs = new List<string>();
                    for (int i = 0; i < tsLst.Count; i++)
                    {
                        vs.Add(tsLst.ToArray()[i].LinkName + "." + tsLst.ToArray()[i].PName + "." + tsLst.ToArray()[i].PaleSN);
                    }
                    listBox1.Items.AddRange(vs.ToArray());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }

        private bool OKFR;
        private bool NGRF;

        public bool ReseDone;

        public void upd()
        {
            try
            {
                int number = data.Key1Xld.Count();
                int numbt = 0;
                //keyValuePairs[data.LinkName].trayDataUser.RestValue();
                keyValuePairs[data.LinkName].trayDataUser.SetValue(data.TrayID, false);
                keyValuePairs[data.LinkName].trayDataUser.SetValue(data.TrayID, data.PaleSN);

                if (keyValuePairs[data.LinkName] != null)
                {
                    if (!keyValuePairs[data.LinkName].ReadPCImage(keyValuePairs[data.LinkName].prest1.PCNamePath))
                    {
                        AddText(data.LinkName + ":连接！" + data.ImagePath);
                    }

                    if (System.IO.File.Exists(data.ImagePath))
                    {
                        HOperatorSet.ReadImage(out ImageNG, data.ImagePath);
                    }
                    else
                    {
                        ImageNG = keyValuePairs[data.LinkName].ImageMode;
                        AddText("读取失败:" + data.LinkName + ":" + data.PaleSN);
                    }
                }
                HOperatorSet.GetImageSize(ImageNG, out width, out height);
                HWind.SetImaage(ImageNG);
                foreach (var item in data.Key1Xld)
                {
                    HWind.OneResIamge.ClearAllObj();
                    HWind.OneResIamge.AddMeassge(data.TrayID);
                    HWind.OneResIamge.AddMeassge(data.PaleSN);
                    numbt++;
                    HOperatorSet.SmallestRectangle1(item.Value.XLd, out HTuple row1, out HTuple col1, out HTuple row2, out HTuple col2);
                    OKFR = NGRF = false;
                    this.Invoke(new Action(() =>
                    {
                        label3.Text = "当前位置:" + (data.TrayX * data.TrayY) + "/" + data.TrayID + Environment.NewLine
                     + "NG数量:" + data.Key1Xld.Count + "/" + numbt + Environment.NewLine +
                       "线:" + data.LinkName + Environment.NewLine +
                       "产品:" + data.PName + Environment.NewLine +
                        "SN:" + data.PaleSN + Environment.NewLine +
                        "元件:" + item.Key;
                        button1.Enabled = true;
                        button2.Enabled = true;
                    }));
                    foreach (var item2 in data.Key1Xld)
                    {
                        if (item.Key != item2.Key)
                        {
                            HWind.OneResIamge.AddObj(item2.Value.XLd, ColorResult.green);
                        }
                    }
                    hWindowControl2.HalconWindow.ClearWindow();
                    hWindowControl3.HalconWindow.ClearWindow();
                    HWind.OneResIamge.AddObj(item.Value.XLd, ColorResult.yellow);
                    ReseDone = true;
                    HOperatorSet.AreaCenter(item.Value.XLd, out HTuple area, out HTuple row, out HTuple col);
                    HOperatorSet.GenContourPolygonXld(out HObject hObject, new HTuple(row, row), new HTuple(new HTuple(0), col1));
                    HOperatorSet.GenContourPolygonXld(out HObject hObject2, new HTuple(new HTuple(0), row1), new HTuple(col, col));
                    HOperatorSet.GenContourPolygonXld(out HObject hObject3, new HTuple(new HTuple(row), row), new HTuple(col2, width));
                    HOperatorSet.GenContourPolygonXld(out HObject hObject4, new HTuple(new HTuple(row2), height), new HTuple(col, col));
                    HWind.OneResIamge.SetCross(hObject.ConcatObj(hObject2).ConcatObj(hObject3).ConcatObj(hObject4));
                    HWind.ShowObj();
                    while (ReseDone)
                    {
                        try
                        {
                            HSystem.SetSystem("flush_graphic", "false");
                            hWindowControl2.HalconWindow.SetPart(row1 - 200, col1 - 200, row2 + 200, col2 + 200);
                            hWindowControl3.HalconWindow.SetPart(row1 - 200, col1 - 200, row2 + 200, col2 + 200);
                            if (keyValuePairs[data.LinkName].prest1 != null)
                            {
                                hWindowControl2.HalconWindow.DispObj(keyValuePairs[data.LinkName].ImageMode);
                            }
                            hWindowControl3.HalconWindow.DispObj(ImageNG);
                            Vision.Disp_message(hWindowControl2.HalconWindow, item.Key, 20, 20, true);
                            vision.Vision.Disp_message(hWindowControl3.HalconWindow, item.Key, 20, 20, true);
                            HSystem.SetSystem("flush_graphic", "true");
                        }
                        catch (Exception ex)
                        {
                        }
                        Thread.Sleep(10);
                    }
                    item.Value.Done = true;
                    if (NGRF)
                    {
                        item.Value.OK = false;
                    }
                    else if (OKFR)
                    {
                        item.Value.OK = true;
                        number--;
                    }
                }
                if (number == 0)
                {
                    data.OK = true;
                    keyValuePairs[data.LinkName].trayDataUser.SetValue(data.TrayID, true);
                }
                else
                {
                    data.OK = false;
                }
                data.Done = true;
                data.RestNG = false;
            stru:
                try
                {
                    if (keyValuePairs.ContainsKey(data.LinkName))
                    {
                        keyValuePairs[data.LinkName].socket.Send(NummberSPC.ObjectToBytes(data));
                    }
                }
                catch (Exception ex)
                {
                    if (MessageBox.Show(data.LinkName + "通信断开", "是否跳过？", MessageBoxButtons.OKCancel) != DialogResult.OK)
                    {
                        goto stru;
                    }
                }
                if (tsLst.Count != 0)
                {
                    tsLst.Dequeue();
                }
                HWind.OneResIamge.ClearAllObj();
                HWind.ShowObj();
                hWindowControl2.HalconWindow.ClearWindow();
                hWindowControl3.HalconWindow.ClearWindow();
                this.Invoke(new Action(() =>
                {
                    if (data.OK)
                    {
                        label3.Text = "当前位置:" + (data.TrayX * data.TrayY) + "/" + data.TrayID + Environment.NewLine +
                                  "线:" + data.LinkName + Environment.NewLine +
                                  "产品:" + data.PName + Environment.NewLine +
                                   "SN:" + data.PaleSN + Environment.NewLine +
                                   "结果:OK";
                    }
                    else
                    {
                        label3.Text = "当前位置:" + (data.TrayX * data.TrayY) + "/" + data.TrayID + Environment.NewLine +
                                  "线:" + data.LinkName + Environment.NewLine +
                                  "产品:" + data.PName + Environment.NewLine +
                                   "SN:" + data.PaleSN + Environment.NewLine +
                                   "结果:NG";
                    }

                    button1.Enabled = false;
                    button2.Enabled = false;
                }));
                //ShowOBj();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (ReseDone)
                {
                    ReseDone = false;
                    OKFR = true;
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
                if (ReseDone)
                {
                    ReseDone = false;
                    NGRF = true;
                }
            }
            catch (Exception)
            {
            }
        }

        private void FormRestfDataIamge_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("是否退出程序？", "退出程序", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                ProjectINI.In.Clros();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void FormRestfDataIamge_KeyDown(object sender, KeyEventArgs e)
        {
            if (ReseDone)
            {
                if (e.KeyData == Keys.Space)
                {
                    ReseDone = false;
                    OKFR = true;
                }
                if (e.KeyCode == Keys.OemMinus)
                {
                    ReseDone = false;
                    NGRF = true;
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!Project.MainForm1.MainFormF.Visible)
            {
                Project.MainForm1.MainFormF.Show();
            }
            else
            {
                Project.MainForm1.MainFormF.Hide();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox2.SelectedItem == null)
                {
                    return;
                }

                System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
                watch.Start();

                if (ErosSocket.ErosConLink.StaticCon.connectState(keyValuePairs[listBox2.SelectedItem.ToString()].prest1.PCNamePath, "hd", "100"))
                {
                    if (System.IO.Directory.Exists(keyValuePairs[listBox2.SelectedItem.ToString()].prest1.PCNamePath))
                    {
                        if (System.IO.File.Exists(keyValuePairs[listBox2.SelectedItem.ToString()].prest1.PCNamePath + keyValuePairs[listBox2.SelectedItem.ToString()].prest1.ImageModePath))
                        {
                            HOperatorSet.ReadImage(out HObject hObject, keyValuePairs[listBox2.SelectedItem.ToString()].prest1.PCNamePath + keyValuePairs[listBox2.SelectedItem.ToString()].prest1.ImageModePath);
                            HWind.SetImaage(hObject);
                            HWind.OneResIamge.AddMeassge(watch.ElapsedMilliseconds);
                            HWind.ShowObj();
                        }
                    }
                }
                watch.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                propertyGrid1.SelectedObject = keyValuePairs[listBox2.SelectedItem.ToString()].prest1;
            }
            catch (Exception)
            {
            }
        }
    }
}