using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;

namespace Vision2.Project.DebugF.IO
{
    public partial class RunCodeUserControl : UserControl
    {
        public RunCodeUserControl()
        {
            InitializeComponent();
        }

        RunCodeStr codeStr;
        List<string> TimeStr;
        bool botrr;
        public void SetData(RunCodeStr code)
        {
            codeStr = code;
            try
            {
                botrr = true;
                CheckBox checkBox = toolStripCheckbox1.Control as CheckBox;
                if (checkBox != null)
                {
                    checkBox.Checked = RunCodeStr.IsSimulate;
                }
                if (!codeStr.Single_step)
                {
                    tsButton4.BackColor = Color.Gray;
                }
                else
                {
                    tsButton4.BackColor = Color.Green;
                }
                for (int i = 0; i < codeStr.CodeStr.Count; i++)
                {
                    richTextBox1.AppendText(codeStr.CodeStr[i] + Environment.NewLine);
                }
                TimeStr = new List<string>();
                TimeStr.AddRange(new string[codeStr.CodeStr.Count]);
                codeStr.RunCode += RunCodeOne;
                codeStr.RunDone += CodeStr_RunDone; ;
                showLineNo(codeStr.StepInt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            botrr = false;
        }

        private void CodeStr_RunDone(RunCodeStr.RunErr key)
        {
            toolStripStatusLabel4.Text = "总CT:" + key.RunTime;
        }

        public void RunCodeOne(RunCodeStr.RunErr runErr)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    Invoke(new Action<RunCodeStr.RunErr>(RunCodeOne), runErr);
                    return;
                }

                if (this.IsDisposed)
                {
                    codeStr.RunCode -= RunCodeOne;
                }
                if (runErr.ErrStr != "")
                {
                    richTextBox2.SelectionColor = Color.Red;
                }
                else
                {
                    richTextBox2.SelectionColor = Color.Black;
                }

                richTextBox2.AppendText("行:" + (runErr.RowIndx + 1) + "状态:" + runErr.RunState +"时间:"+ (runErr.StepRunTime / 1000.0).ToString("0.000S") +"错误:"+runErr.ErrStr + Environment.NewLine);
                ////toolStripStatusLabel1.Text = "行:" + runErr.RowIndx;
                toolStripStatusLabel2.Text = (runErr.StepRunTime / 1000.0).ToString("0.000S");
                toolStripStatusLabel4.Text = "总CT:"+codeStr.RunTime.ToString();
                TimeStr[runErr.RowIndx] = runErr.StepRunTime + "ms";
                //toolStripStatusLabel3.Text = "Time:" + runErr.StepRunTime + "ms";
                showLineNoTime(runErr.RowIndx, panel3, runErr.StepRunTime + "ms");
                showLineNo(codeStr.StepInt);
                richTextBox2.SelectionStart = richTextBox2.Text.Length;
                richTextBox2.ScrollToCaret();
            }
            catch (Exception)
            {

            }
        }

        int lineSpace;
        private void showLineNo(int COT)
        {
            if (codeStr.Runing)
            {
                tsButton3.BackColor = Color.Green;
            }
            else
            {
                tsButton3.BackColor = Color.Gray;
            }
            //获得当前坐标信息
            Point p = this.richTextBox1.Location;
            int crntFirstIndex = this.richTextBox1.GetCharIndexFromPosition(p);

            int crntFirstLine = this.richTextBox1.GetLineFromCharIndex(crntFirstIndex);
            //crntFirstLine = this.richTextBox1.Lines.Length;
            Point crntFirstPos = this.richTextBox1.GetPositionFromCharIndex(crntFirstIndex);

            p.Y += this.richTextBox1.Height;

            int crntLastIndex = this.richTextBox1.GetCharIndexFromPosition(p);

            int crntLastLine = this.richTextBox1.GetLineFromCharIndex(crntLastIndex);
            Point crntLastPos = this.richTextBox1.GetPositionFromCharIndex(crntLastIndex);

            //准备画图
            Graphics g = this.panel1.CreateGraphics();

            Font font = new Font(this.richTextBox1.Font, this.richTextBox1.Font.Style);
            //font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            SolidBrush brush = new SolidBrush(Color.Blue);

            //画图开始

            //刷新画布

            Rectangle rect = this.panel1.ClientRectangle;
            brush.Color = this.panel1.BackColor;
            SolidBrush brusht = new SolidBrush(Color.Blue);
            brusht.Color = Color.GreenYellow;

            g.FillRectangle(brush, 0, 0, this.panel1.ClientRectangle.Width, this.panel1.ClientRectangle.Height);
            g.FillRectangle(brusht, 20, 0, 1, this.panel1.ClientRectangle.Height);
            brush.Color = Color.Blue;//重置画笔颜色

            //绘制行号



            if (crntFirstLine != crntLastLine)
            {
                lineSpace = (crntLastPos.Y - crntFirstPos.Y) / (crntLastLine - crntFirstLine);
            }
            else
            {
                //lineSpace = Convert.ToInt32(this.richTextBox1.Font.Size);

            }

            int brushX = this.panel1.ClientRectangle.Width - Convert.ToInt32(font.Size * 2);

            int brushY = crntLastPos.Y + Convert.ToInt32(font.Size * 0.21f);//惊人的算法啊！！


            System.Drawing.Drawing2D.AdjustableArrowCap lineCap =
                new System.Drawing.Drawing2D.AdjustableArrowCap(4, 6, true);
            Pen redArrowPen = new Pen(Color.Red, 4);
            redArrowPen.CustomEndCap = lineCap;

            Graphics gfx = this.panel1.CreateGraphics();




            for (int i = crntLastLine; i > -1; i--)
            {
                if (i == COT)
                {
                    gfx.DrawLine(redArrowPen, brushX - 40, brushY + 5, brushX, brushY + 5);
                    brush.Color = Color.Red;//重置画笔颜色
                }
                else
                {
                    brush.Color = Color.Blue;//重置画笔颜色
                }
                g.DrawString((i + 1).ToString(), font, brush, brushX, brushY);
                brushY -= lineSpace;
            }

            g.Dispose();

            font.Dispose();

            brush.Dispose();
        }

        private void showLineNoTime(int COT, Panel panel, string text)
        {
            try
            {


                //获得当前坐标信息
                Point p = this.richTextBox1.Location;
                int crntFirstIndex = this.richTextBox1.GetCharIndexFromPosition(p);

                int crntFirstLine = this.richTextBox1.GetLineFromCharIndex(crntFirstIndex);
                //crntFirstLine = this.richTextBox1.Lines.Length;
                Point crntFirstPos = this.richTextBox1.GetPositionFromCharIndex(crntFirstIndex);

                p.Y += this.richTextBox1.Height;

                int crntLastIndex = this.richTextBox1.GetCharIndexFromPosition(p);

                int crntLastLine = this.richTextBox1.GetLineFromCharIndex(crntLastIndex);
                Point crntLastPos = this.richTextBox1.GetPositionFromCharIndex(crntLastIndex);

                //准备画图
                Graphics g = panel.CreateGraphics();

                Font font = new Font(this.richTextBox1.Font, this.richTextBox1.Font.Style);
                //font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                SolidBrush brush = new SolidBrush(Color.Blue);

                //画图开始

                //刷新画布

                Rectangle rect = panel.ClientRectangle;
                brush.Color = panel.BackColor;
                SolidBrush brusht = new SolidBrush(Color.Blue);
                brusht.Color = Color.GreenYellow;

                g.FillRectangle(brush, 0, 0, panel.ClientRectangle.Width, panel.ClientRectangle.Height);
                g.FillRectangle(brusht, 20, 0, 1, panel.ClientRectangle.Height);
                brush.Color = Color.Blue;//重置画笔颜色

                //绘制行号


                if (crntFirstLine != crntLastLine)
                {
                    lineSpace = (crntLastPos.Y - crntFirstPos.Y) / (crntLastLine - crntFirstLine);
                }
                else
                {
                    //lineSpace = Convert.ToInt32(this.richTextBox1.Font.Size);
                }

                int brushX = 0;

                int brushY = crntLastPos.Y + Convert.ToInt32(font.Size * 0.21f);//惊人的算法啊！！


                System.Drawing.Drawing2D.AdjustableArrowCap lineCap =
                    new System.Drawing.Drawing2D.AdjustableArrowCap(4, 6, true);
                Pen redArrowPen = new Pen(Color.Red, 4);
                redArrowPen.CustomEndCap = lineCap;

                Graphics gfx = panel.CreateGraphics();

                for (int i = crntLastLine; i > -1; i--)
                {

                    if (i == COT)
                    {
                        //gfx.DrawLine(redArrowPen, brushX - 40, brushY + 5, brushX, brushY + 5);
                        brush.Color = Color.Red;//重置画笔颜色
                        g.DrawString(text, font, brush, brushX, brushY);

                    }
                    else if (TimeStr[i] != null)
                    {

                        brush.Color = Color.Blue;//重置画笔颜色
                        g.DrawString(TimeStr[i], font, brush, brushX, brushY);
                    }
                    brushY -= lineSpace;
                }

                g.Dispose();

                font.Dispose();

                brush.Dispose();
            }
            catch (Exception)
            {

            }
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (!botrr)
            {
                showLineNo(codeStr.StepInt);
            }

        }

        private void tsButton1_Click(object sender, EventArgs e)
        {
            //DebugCompiler.EquipmentStatus = EnumEquipmentStatus.已停止;
            codeStr.Stop();
            codeStr.IfRowInt = 0;
            codeStr.CodeStr = richTextBox1.Lines.ToList();
            TimeStr = new List<string>();
            TimeStr.AddRange(new string[codeStr.CodeStr.Count]);
            richTextBox1.SelectionStart = 0;
            richTextBox1.SelectionLength = richTextBox1.Text.Length;
            richTextBox1.SelectionColor = Color.Black;
            richTextBox2.Text = "";
            richTextBox1.SelectionStart = 0;
            richTextBox1.Select(0, 0);
        }

        private void tsButton3_Click(object sender, EventArgs e)
        {

            Thread thread = new Thread(() =>
            {

                codeStr.Run();
            });
            thread.IsBackground = true;

            thread.Start();


        }


        private void RunCodeUserControl_Load(object sender, EventArgs e)
        {
            try
            {
                if (!ProjectINI.GetUserJurisdiction("工程师"))
                {
                    richTextBox1.Enabled = false;
                }
            }
            catch (Exception)
            {

            }
        }

        private void tsButton2_Click(object sender, EventArgs e)
        {
            codeStr.NextStep = true;
        }

        private void tsButton4_Click(object sender, EventArgs e)
        {
            if (codeStr.Single_step)
            {
                codeStr.Single_step = false;
                tsButton4.BackColor = Color.White;
            }
            else
            {
                codeStr.Single_step = true;
                tsButton4.BackColor = Color.Green;
            }
            //tsButton4.Checked = false;

        }

        private void richTextBox1_VScroll(object sender, EventArgs e)
        {
            showLineNo(codeStr.StepInt);
            showLineNoTime(codeStr.IfRowInt, panel3, "");
        }
        Local_variable_scale_Form local_Variable_Scale_Form;

        private void tsButton5_Click(object sender, EventArgs e)
        {
            if (local_Variable_Scale_Form == null || local_Variable_Scale_Form.IsDisposed)
            {
                local_Variable_Scale_Form = new Local_variable_scale_Form(DebugCompiler.GetThis().DDAxis.KeyVales);
            }
            Vision2.ErosProjcetDLL.UI.UICon.WindosFormerShow(ref local_Variable_Scale_Form);
        }

        private void tsButton7_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, Application.StartupPath + @"\help.chm", HelpNavigator.Topic, "4.1运行脚本.htm");
        }

        private void toolStripCheckbox1_Click(object sender, EventArgs e)
        {
            CheckBox checkBox = toolStripCheckbox1.Control as CheckBox;
            if (checkBox != null)
            {
                RunCodeStr.IsSimulate = checkBox.Checked;
            }
        }

        private void RunCodeUserControl_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (codeStr != null)
                {
                    showLineNo(codeStr.StepInt);
                }
            }
            catch (Exception)
            {
            }
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F5)
                {
                    codeStr.NextStep = true;
                }
            }
            catch (Exception)
            {
            }

        }
    }
}
