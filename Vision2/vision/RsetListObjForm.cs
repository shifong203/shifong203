﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision2.Project.formula;
using Vision2.Project.Mes;
using Vision2.vision.HalconRunFile.RunProgramFile;
using ErosSocket.DebugPLC.Robot;

namespace Vision2.vision
{
    public partial class RsetListObjForm : Form
    {
        public RsetListObjForm()
        {
            InitializeComponent();
        }
        HWindID HWindd = new HWindID();
        HWindID HWindIDt = new HWindID();
        static Queue<DataVale> trayDataVales = new Queue<DataVale>();
 
        static DataVale data;
        public void SetImage()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dat"></param>
        public void ShowImage(DataVale dat)
        {
            try
            {
                hWindowControl1.Focus();
                if (HWindd == null)
                {
                    HWindd = new HWindID();
                    HWindd.Initialize(hWindowControl1);
                }
                if (HWindIDt == null)
                {
                    HWindIDt = new HWindID();
                    HWindIDt.Initialize(hWindowControl2);
                }
                this.label4.Text = "";
                if (dat.ResuOBj != null)
                {
                    dat.OK = true;
                    foreach (var item in dat.ResuOBj)
                    {
                        for (int i = 0; i < item.NGObj.Count; i++)
                        {
                            //dat.Data1.AddRange(new MaxMinValue (){ item.NGObj[i].);
                            if (trayDataVales.Count() != 0)
                            {
                                item.NGObj[i].OK = true;
                            }
                            //if (!item.NGObj[i].ResultBool)
                            //{
                            //    dat.OK = false;
                            //}
                        }
                    }
                    if (dat.ResuOBj != null && !dat.OK)
                    {

                        if (!trayDataVales.Contains(dat))
                        {
                            RecipeCompiler.AddOKNumber(false);
                            trayDataVales.Enqueue(dat);
                        }
                        label3.Text = RecipeCompiler.Instance.GetSPC();
                        this.Text = "复判窗口剩余:" + trayDataVales.Count;
                        ErosProjcetDLL.UI.UICon.SwitchToThisWindow(RestObjImage.RestObjImageFrom.Handle, true);
                        RestObjImage.RestObjImageFrom.Show();
                    }
                    else
                    {
                        if (dat.OK)
                        {
                            RecipeCompiler.AddOKNumber(true);
                            //dat.RsetOK = true;
                            dat.PanelID = dat.PanelID;
                            UserFormulaContrsl.WeirtAll(dat);
                        }
                    }
                }
                label3.Text = RecipeCompiler.Instance.GetSPC();
                Vision.OneProductVale = new DataVale();
                //Vision.Instance.OneProductValeClert();
                HWindIDt.ShowImage();
            }
            catch (Exception ex)
            {
                ErosProjcetDLL.Project.AlarmText.AddTextNewLine("复判窗口:" + ex.Message, Color.Red);
            }
        }
        public static void Clser()
        {
            try
            {
                trayDataVales.Clear();
            }
            catch (Exception)
            {
            }
        }

        private void hWindowControl3_HMouseMove(object sender, HalconDotNet.HMouseEventArgs e)
        {

        }
        void UpData()
        {
            double dee = HWindd.HeigthImage / HWindd.WidthImage;
            hWindowControl1.Height = (int)(hWindowControl1.Width * dee);
            if (panel2.Height < hWindowControl1.Height)
            {
                hWindowControl1.Height = panel2.Height;
            }
            label4.Text = "SN:" + data.PanelID + Environment.NewLine +
               "相机:" + HWindd.HalconResult.RunName + "." + HWindd.HalconResult.RunID + Environment.NewLine
                   + "NG信息:" + HWindd.HalconResult.NGMestage;
            if (HWindd.HalconResult.PoxintID != "")
            {
                label4.Text += ";位号:" + HWindd.HalconResult.PoxintID + Environment.NewLine;
            }
            try
            {
                HWindIDt.ClearObj();
                string nareName = Vision.GetRunNameVision(HWindd.HalconResult.RunName).ReNmae[HWindd.HalconResult.RunID - 1];
                string[] datas = nareName.Split('.');
                HWindIDt.SetImaage(RecipeCompiler.GetProductEX().Key_Navigation_Picture[datas[0]].GetHObject());
                HWindIDt.HalconResult.AddObj(RecipeCompiler.GetProductEX().Key_Navigation_Picture[datas[0]].KeyRoi[datas[1]]);
            }
            catch (Exception ex)
            {
            }
            HWindIDt.ShowImage();
            HWindd.ShowImage();
        }
        int det = 0;
        private void RsetListObjForm_Load(object sender, EventArgs e)
        {
            try
            {
                ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView1);
                Thread thread = new Thread(() =>
                {
                    try
                    {
                        //Vision.GetRunNameVision(data.Result.RunName).EventShow += RestObjImage_EventShow; 
                        while (!this.IsDisposed)
                        {
                            try
                            {

                                if (data == null || data.Done)
                                {
                                    if (trayDataVales.Count != 0)
                                    {

                                        data = trayDataVales.Dequeue();
                                        det = 0;
                                        this.Invoke(new Action(() =>
                                        {
                                            this.Text = "复判窗口剩余:" + trayDataVales.Count;
                                            if (data.ImagePlus != null)
                                            {
                                                HWindd.SetImaage(data.ImagePlus);
                                            }
                                            dataGridView1.Rows.Clear();
                                            foreach (var item in data.ResuOBj)
                                            {
                                                for (int i = 0; i < item.NGObj.Count; i++)
                                                {
                                                    if (!item.NGObj[i].OK)
                                                    {
                                                        int dt = dataGridView1.Rows.Add();
                                                        dataGridView1.Rows[dt].Cells[0].Value = item.RunName + ":" + item.RunID;
                                                        dataGridView1.Rows[dt].Cells[0].Tag = item.NGObj[i];
                                                        dataGridView1.Rows[dt].Cells[1].Value = item.NGObj[i].NGText;
                                                    }
                                                }
                                            }
                                            string[] datStr = dataGridView1.Rows[0].Cells[0].Value.ToString().Split(':');
                                            HalconResult halconResult = dataGridView1.Rows[0].Cells[0].Tag as HalconResult;
                                            dataGridView1.Rows[det].DefaultCellStyle.BackColor = Color.Yellow;
                                            label1.Text = "NG";
                                            int idnxet = int.Parse(datStr[1]) - 1;
                                            label1.BackColor = Color.Red;
                                            HWindd.SetImaage(halconResult.Image);
                                            HWindd.HalconResult = halconResult;
                                            UpData();
                                            ErosProjcetDLL.UI.UICon.SwitchToThisWindow(RestObjImage.RestObjImageFrom.Handle, true);
                                            RestObjImage.RestObjImageFrom.Show();
                                        }));

                                    }
                                }
                                Thread.Sleep(100);
                            }
                            catch (Exception ex)
                            {
                                ErosProjcetDLL.Project.AlarmText.AddTextNewLine("复判窗口:" + ex.Message, Color.Red);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErosProjcetDLL.Project.AlarmText.AddTextNewLine("复判窗口:" + ex.Message, Color.Red);
                    }
                });
                ;
                thread.IsBackground = true;
                thread.Start();
            }
            catch (Exception)
            {
            }
        }
    }
}