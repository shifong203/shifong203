using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace ErosSocket.DebugPLC.Robot
{
    public class Sequential_programme
    {
        public class ErrorExecute
        {
            public bool error;
            public int errorID;
            public bool isContinue;
            public int CodeStrRow;
            public string errorMesagge;
        }

        public bool Continue_next { get; set; }
        public List<string> CodeStr { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="codeStr"></param>
        /// <param name="codeRow"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Execute_Programme(string codeStr, int codeRow, out ErrorExecute error)
        {
            error = new ErrorExecute();

            return true;
        }

        public void Run_Execute_Programme(bool debug = false)
        {
            ErrorExecute errorExecute = new ErrorExecute();
            for (int i = 0; i < CodeStr.Count; i++)
            {
                if (!Execute_Programme(CodeStr[i], i, out errorExecute))
                {
                    Vision2.ErosProjcetDLL.Project.AlarmText.LogErr("行:" + errorExecute.CodeStrRow + ";故障详细:" + errorExecute.errorMesagge + ";ID" + errorExecute.errorID, "执行脚本故障");
                    if (!errorExecute.isContinue)
                    {
                        return;
                    }
                }
                if (debug)
                {
                    while (!Continue_next)
                    {
                        Thread.Sleep(10);
                    }
                }
            }
        }

        private bool busy;

        public void Run(RichTextBox richTextBox1, ErosSocket.DebugPLC.IAxisGrub axisGrub)
        {
            try
            {
                richTextBox1.SelectAll();
                richTextBox1.SelectionColor = Color.Black;
                richTextBox1.SelectionLength = 0;
                if (busy)
                {
                    richTextBox1.ForeColor = Color.Red;
                    return;
                }
                busy = true;
                Task.Run(() =>
                {
                    int lenhy = 0;
                    for (int i = 0; i < richTextBox1.Lines.Length; i++)
                    {
                        System.Threading.Thread.Sleep(1010);
                        richTextBox1.SelectionStart = lenhy;
                        richTextBox1.SelectionLength = richTextBox1.Lines[i].Length + 1;
                        richTextBox1.SelectionColor = Color.Black;
                        string datas = richTextBox1.Lines[i].Trim(' ');
                        if (datas.ToLower().StartsWith("move"))
                        {
                            string[] items = datas.Split(' ');
                            Single? x = null;
                            Single? y = null;
                            Single? z = null;
                            Single? u = null;
                            for (int ite = 0; ite < items.Length; ite++)
                            {
                                if (items[ite].ToUpper().StartsWith("X"))
                                {
                                    x = Convert.ToSingle(items[ite].Remove(0, 1));
                                }
                                else if (items[ite].ToUpper().StartsWith("Y"))
                                {
                                    y = Convert.ToSingle(items[ite].Remove(0, 1));
                                }
                                else if (items[ite].ToUpper().StartsWith("Z"))
                                {
                                    z = Convert.ToSingle(items[ite].Remove(0, 1));
                                }
                                else if (items[ite].ToUpper().StartsWith("U"))
                                {
                                    u = Convert.ToSingle(items[ite].Remove(0, 1));
                                }
                            }
                            if (axisGrub.SetPoint("Move", x, y, z, u))
                            {
                                richTextBox1.SelectionColor = Color.GreenYellow;
                            }
                            else
                            {
                                richTextBox1.SelectionStart = lenhy;
                                richTextBox1.SelectionLength = richTextBox1.Lines[i].Length + 1;
                                richTextBox1.SelectionColor = Color.Red;

                                busy = false;
                                return;
                            }
                        }
                        else if (datas.ToLower().StartsWith("go"))
                        {
                            string[] items = datas.Split(' ');
                            Single? x = null;
                            Single? y = null;
                            Single? z = null;
                            Single? u = null;

                            for (int ite = 0; ite < items.Length; ite++)
                            {
                                if (items[ite].ToUpper().StartsWith("X"))
                                {
                                    x = Convert.ToSingle(items[ite].Remove(0, 1));
                                }
                                else if (items[ite].ToUpper().StartsWith("Y"))
                                {
                                    y = Convert.ToSingle(items[ite].Remove(0, 1));
                                }
                                else if (items[ite].ToUpper().StartsWith("Z"))
                                {
                                    z = Convert.ToSingle(items[ite].Remove(0, 1));
                                }
                                else if (items[ite].ToUpper().StartsWith("U"))
                                {
                                    u = Convert.ToSingle(items[ite].Remove(0, 1));
                                }
                                else if (items[ite].Contains("="))
                                {
                                    string[] datase = items[ite].Split('=');
                                    if (datase.Length == 2)
                                    {
                                        axisGrub.SetIOOut(datase[0], datase[1]);
                                    }
                                }
                            }
                            if (axisGrub.SetPoint("go", x, y, z, u))
                            {
                                richTextBox1.SelectionColor = Color.GreenYellow;
                            }
                            else
                            {
                                richTextBox1.SelectionStart = lenhy;
                                richTextBox1.SelectionLength = richTextBox1.Lines[i].Length + 1;
                                richTextBox1.SelectionColor = Color.Red;

                                busy = false;
                                return;
                            }
                            //
                        }
                        else if (datas.Contains("="))
                        {
                            string[] datase = datas.Split('=');
                            if (datase.Length == 2)
                            {
                                axisGrub.SetIOOut(datase[0], datase[1]);
                            }
                        }
                        lenhy += richTextBox1.Lines[i].Length + 1;
                    }

                    busy = false;
                });
            }
            catch (Exception)
            {
                busy = false;
            }
        }
    }
}