using ErosSocket.DebugPLC;
using ErosSocket.ErosConLink;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Vision2.Project.formula;

namespace Vision2.Project.DebugF
{
    public partial class PLCValuesUI : UserControl
    {
        public PLCValuesUI()
        {

            InitializeComponent();
            //checkBox1.Checked = RecipeCompiler.Instance.Data.IsChe;
        }

        private void PLCValuesUI_Load(object sender, EventArgs e)
        {
            Vision2.ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView1);
            Vision2.ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView2);
            propertyGrid1.Enabled = false;
            try
            {
                propertyGrid1.SelectedObject = RecipeCompiler.Instance.MesDa;
                dataGridView2.Rows.Clear();
                dataGridView2.Rows.Add(RecipeCompiler.Instance.Data.CheCalssT.Count);
                Thread thread = new Thread(
                    () =>
                    {
                        while (true)
                        {
                            try
                            {
                                if (DebugComp.GetThis().ISPLCDebug)
                                {
                                    int dwe = (int)StaticCon.GetSocketClint(RecipeCompiler.Instance.DataLinkName).GetKeyValue("点位数量");
                                    label2.Text = "点数量:" + dwe;
                                    label4.Text = "完成状态:" + StaticCon.GetSocketClint(RecipeCompiler.Instance.DataLinkName).GetKeyValue("完成");
                                    label5.Text = "请求状态:";
                                    if (StaticCon.GetSocketClint(RecipeCompiler.Instance.DataLinkName).GetIDValue("D1210", "Int16", out dynamic valueD))
                                    {
                                        string err = "";
                                        if (valueD == 10)
                                        {
                                            if (ProcessControl.ProcessUser.QRCode == "")
                                            {
                                                StaticCon.GetSocketClint(RecipeCompiler.Instance.DataLinkName).SetIDValue("D1210", "Int16", "21", out  err);
                                            }
                                            else
                                            {
                                                StaticCon.GetSocketClint(RecipeCompiler.Instance.DataLinkName).SetIDValue("D1210", "Int16","20", out  err);
                                            }
                                        }
                                        label5.Text = "请求状态:"+ valueD +err;
                                    }
       
                                    label1.Text = "位移值:" + StaticCon.GetSocketClint(RecipeCompiler.Instance.DataLinkName).GetKeyValue("位移传感器值");
                                    if (StaticCon.GetSocketClint(RecipeCompiler.Instance.DataLinkName).GetKeyValue("完成") == 1)
                                    {
                                        StaticCon.GetSocketClint(RecipeCompiler.Instance.DataLinkName).SetValue("完成", "0", out string err);
                                        List<float> valuse = new List<float>();
                                        List<float> xs = new List<float>();
                                        List<float> ys = new List<float>();
                                        RecipeCompiler.Instance.Data.Clear();
                                        for (int i = 0; i < dwe; i++)
                                        {
                                            try
                                            {
                                                //if (StaticCon.GetSocketClint(RecipeCompiler.Instance.DataLinkName).GetIDValue(RecipeCompiler.Instance.Data.PLCID[i], "Single", out dynamic values))
                                                //{
                                                //    RecipeCompiler.Instance.Data.AddData(values.ToString());
                                                //    valuse.Add(Convert.ToSingle(values));
                                                //}
                                                //else
                                                //{
                                                //    Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine("读取PLC错误z:" + i);
                                                //}
                                                //if (!Single.TryParse(RecipeCompiler.Instance.Data.PointXID[i], out float resFloat))
                                                //{
                                                //    if (StaticCon.GetSocketClint(RecipeCompiler.Instance.DataLinkName).GetIDValue(RecipeCompiler.Instance.Data.PointXID[i], "Single", out values))
                                                //    {
                                                //        xs.Add(Convert.ToSingle(values));
                                                //    }
                                                //    else
                                                //    {
                                                //        Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine("读取PLC错误X:" + i);
                                                //    }
                                                //}
                                                //else
                                                //{
                                                //    xs.Add(resFloat);
                                                //}
                                                //if (!Single.TryParse(RecipeCompiler.Instance.Data.PointYID[i], out resFloat))
                                                //{
                                                //    if (StaticCon.GetSocketClint(RecipeCompiler.Instance.DataLinkName).GetIDValue(RecipeCompiler.Instance.Data.PointYID[i], "Single", out values))
                                                //    {
                                                //        ys.Add(Convert.ToSingle(values));
                                                //    }
                                                //    else
                                                //    {
                                                //        Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine("读取PLC错误X:" + i);
                                                //    }
                                                //}
                                                //else
                                                //{
                                                //    ys.Add(resFloat);
                                                //}
                                            }
                                            catch (Exception)
                                            {
                                            }
                                        }
                                        Updet(valuse, xs, ys, dwe);
                                    }

                                 
                                }
                            }
                            catch (Exception ex)
                            {
                                ErosProjcetDLL.Project.AlarmText.AddTextNewLine("读取PLC错误:" + ex.Message);
                            }
                        }
                    }
                    );
                thread.IsBackground = true;
                thread.Start();


            }
            catch (Exception)
            {

            }
            ErosProjcetDLL.Project.ProjectINI.In.User.EventLog += User_EventLog;
        }

        private void User_EventLog(bool isLog)
        {
            try
            {
                propertyGrid1.Enabled = isLog;
            }
            catch (Exception)
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int dwe = 40;
                List<float> valuse = new List<float>();
                RecipeCompiler.Instance.Data.Clear();
                valuse.Add(3.52f);//              [0,0,3.52],
                //xs.Add(1);
                //ys.Add(0);
                valuse.Add(2.3f);//   [1,0,2.3],

                valuse.Add(-2.69f);// [2,0,-2.69],

                valuse.Add(-2.65f);// [0,1,-2.65],

                valuse.Add(3.25f);//  [1,1,3.25],

                valuse.Add(4.36f);//  [2,1,4.36],

                valuse.Add(-2.36f);// [0,2,-2.36],

                valuse.Add(4.56f);// [1,2,4.56],

                valuse.Add(-3.54f);// [2,2,-3.54],

                valuse.Add(2.36f);// [0,3,2.36],

                valuse.Add(-5.65f);//    [1,3,-5.65],

                valuse.Add(2.59f);//       [2,3,2.59]
                List<float> xs = new List<float>();
                List<float> ys = new List<float>();
                valuse.Clear();
                Random rd = new Random();
                for (int i = 0; i < dwe; i++)
                {
                    //Membership.GeneratePassword(20, 1)


                    valuse.Add(rd.Next(-1, 10));
                    //if (RecipeCompiler.Instance.Data.PointXID[i] == "")
                    //{
                    //    xs.Add(rd.Next(-1, 10));
                    //}
                    //else
                    //{
                    //    if (!Single.TryParse(RecipeCompiler.Instance.Data.PointXID[i], out float resFloat))
                    //    {
                    //        if (StaticCon.GetSocketClint(RecipeCompiler.Instance.DataLinkName).GetIDValue(RecipeCompiler.Instance.Data.PointXID[i], "Single", out dynamic values))
                    //        {
                    //            xs.Add(Convert.ToSingle(values));
                    //        }
                    //        else
                    //        {
                    //            Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine("读取PLC错误X:" + i);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        xs.Add(resFloat);
                    //    }

                    //}
                    //if (RecipeCompiler.Instance.Data.PointYID[i] == "")
                    //{
                    //    ys.Add(rd.Next(-1, 10));
                    //}
                    //else
                    //{
                    //    if (!Single.TryParse(RecipeCompiler.Instance.Data.PointYID[i], out float resFloat))
                    //    {
                    //        if (StaticCon.GetSocketClint(RecipeCompiler.Instance.DataLinkName).GetIDValue(RecipeCompiler.Instance.Data.PointYID[i], "Single", out dynamic values))
                    //        {
                    //            ys.Add(Convert.ToSingle(values));
                    //        }
                    //        else
                    //        {
                    //            Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine("读取PLC错误X:" + i);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        ys.Add(resFloat);
                    //    }
                    //}



                }

                Updet(valuse, xs, ys, valuse.Count);
            }
            catch (Exception)
            {

            }
        }

        void Updet(List<float> valuse, List<float> x, List<float> y, int dwe)
        {
            try
            {
                string QR = ProcessControl.ProcessUser.QRCode;
                if (RecipeCompiler.Instance.GetMes() != null)
                {
                    if (!RecipeCompiler.Instance.GetMes().ReadMes(QR, out string resetMes))
                    {
                        ErosProjcetDLL.Project.AlarmText.AddTextNewLine("Mes结果ng:" + resetMes, Color.Red);
                    }
                    else
                    {
                        ErosProjcetDLL.Project.AlarmText.AddTextNewLine("Mes结果ok:" + resetMes, Color.Green);
                    }
                }
                string da = DateTime.Now.ToString();
                RecipeCompiler.Instance.Data.OnEnver();
                int resto = 0;
                if (RecipeCompiler.Instance.Data.GetChet(x, y, valuse))
                {
                    StaticCon.GetSocketClint(RecipeCompiler.Instance.DataLinkName).SetValue("NG", "1", out string err);
                    RecipeCompiler.AddOKNumber(true);
                    resto = 1;
                }
                else
                {
                    StaticCon.GetSocketClint(RecipeCompiler.Instance.DataLinkName).SetValue("NG", "2", out string err);
                    RecipeCompiler.AddOKNumber(false);
                }
                //List<string> nAMES = new List<string>();
                List<string> ContNAMES = new List<string>();
                ContNAMES.Add("时间");
                ContNAMES.Add("码");
                List<string> ValueStr = new List<string>();
                ValueStr.Add(da);
                ValueStr.Add(QR);

                this.Invoke(new Action(() =>
                {
                    dataGridView1.Rows.Clear();
                    dataGridView1.Rows.Add(dwe);
                    for (int i = 0; i < dwe; i++)
                    {
                        //dataGridView1.Rows[i].Cells[0].Value = RecipeCompiler.Instance.Data.Reference_Name[i];
                        //nAMES.Add(RecipeCompiler.Instance.Data.Reference_Name[i]);
                        //ContNAMES.Add(RecipeCompiler.Instance.Data.Reference_Name[i]);
                        dataGridView1.Rows[i].Cells[1].Value = valuse[i];
                        dataGridView1.Rows[i].Cells[2].Value = x[i];
                        dataGridView1.Rows[i].Cells[3].Value = y[i];
                        if (!RecipeCompiler.Instance.Data.IsChe)
                        {
                            //if (RecipeCompiler.Instance.Data.Reference_ValueMin[i] > RecipeCompiler.Instance.Data.Values[i])
                            //{
                            //    dataGridView1.Rows[i].Cells[2].Style.BackColor = Color.Red;
                            //}
                            //else
                            //{
                            //    dataGridView1.Rows[i].Cells[2].Style.BackColor = Color.Green;
                            //}
                            //if (RecipeCompiler.Instance.Data.Reference_ValueMax[i] < RecipeCompiler.Instance.Data.Values[i])
                            //{
                            //    dataGridView1.Rows[i].Cells[3].Style.BackColor = Color.Red;
                            //}
                            //else
                            //{
                            //    dataGridView1.Rows[i].Cells[3].Style.BackColor = Color.Green;
                            //}
                        }
                        ValueStr.Add(valuse[i].ToString());
                    }
                    if (RecipeCompiler.Instance.Data.IsChe)
                    {
                        for (int i = 0; i < RecipeCompiler.Instance.Data.CheCalssT.Count; i++)
                        {
                            //nAMES.Add("平面度" + i);
                            ContNAMES.Add("平面度" + i);
                            dataGridView2.Rows[i].Cells[0].Value = RecipeCompiler.Instance.Data.CheCalssT[i].Value;
                            dataGridView2.Rows[i].Cells[1].Value = RecipeCompiler.Instance.Data.CheCalssT[i].MaxValue;
                            if (!RecipeCompiler.Instance.Data.CheCalssT[i].GetChet())
                            {
                                dataGridView2.Rows[i].Cells[0].Style.BackColor = Color.Red;
                                int DWET = RecipeCompiler.Instance.Data.CheCalssT[i].StrartNumber - 1;
                                if (DWET < 0)
                                {
                                    DWET = 0;
                                }
                                for (int i2 = DWET; i2 < dwe; i2++)
                                {
                                    if (RecipeCompiler.Instance.Data.CheCalssT[i].EndNumber + DWET <= i2)
                                    {
                                        break;
                                    }
                                    dataGridView1.Rows[i2].Cells[1].Style.BackColor = Color.Red;
                                }
                            }
                            else
                            {
                                int DWET = RecipeCompiler.Instance.Data.CheCalssT[i].StrartNumber - 1;
                                if (DWET < 0)
                                {
                                    DWET = 0;
                                }
                                dataGridView2.Rows[i].Cells[0].Style.BackColor = Color.Green;
                                for (int i2 = DWET; i2 < dwe; i2++)
                                {
                                    if (RecipeCompiler.Instance.Data.CheCalssT[i].EndNumber + DWET <= i2)
                                    {
                                        break;
                                    }
                                    dataGridView1.Rows[i2].Cells[1].Style.BackColor = Color.Green;

                                }
                            }
                            ValueStr.Add(RecipeCompiler.Instance.Data.CheCalssT[i].Value.ToString());
                        }
                    }
                }));
                string dataTime = DateTime.Now.ToLongDateString();
                if (!System.IO.File.Exists(ProcessControl.ProcessUser.GetThis().ExcelPath + "//" + dataTime + ".xls"))
                {
                    ErosProjcetDLL.Excel.Npoi.AddWriteColumnToExcel(ProcessControl.ProcessUser.GetThis().ExcelPath + "//" + dataTime, "数据", ContNAMES.ToArray());
                }
                ErosProjcetDLL.Excel.Npoi.AddRosWriteToExcel(ProcessControl.ProcessUser.GetThis().ExcelPath + "//" + dataTime, "数据", ValueStr.ToArray());
                UserFormulaContrsl.This.textBox1.Text = "";

                if (RecipeCompiler.Instance.GetMes() != null)
                {
                    RecipeCompiler.Instance.GetMes().WrietMesAll(resto, QR, "");
                }
                else
                {
                    ErosProjcetDLL.Project.AlarmText.AddTextNewLine("未定义Mes", Color.Red);
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
                StaticCon.GetSocketClint(RecipeCompiler.Instance.DataLinkName).SetValue("完成", "1", out string err);
            }
            catch (Exception)
            {

            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                RecipeCompiler.Instance.MesDa.UserCode = textBox1.Text;
            }
            catch (Exception)
            {

            }
        }
    }
}
