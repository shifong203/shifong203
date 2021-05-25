using HalconDotNet;
using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.UI.PropertyGrid;

namespace Vision2.vision.Calib
{
    public partial class AutoCalibForm : Form
    {
        public AutoCalibForm(HalconRunFile.RunProgramFile.HalconRun halconRun)
        {
            InitializeComponent();
            Halcon = halconRun;
            this.Text = Halcon.Name;
            //vision2UserControl1.UpHalcon(halconRun);
            //halconRunProgram1.UpHalcon(halconRun);
            //visionUserControl2.UpHalcon(halconRun);
            if (Halcon.GetSocket() != null)
            {
                Control control = Halcon.GetSocket().GetDebugControl(this);
                tabPage1.Controls.Add(control);
                halconRun.GetSocket().PassiveEvent += AutoCalibForm_PassiveEvent;
            }
            PuData();
            textBox28.Text = halconRun.Name;
        }

        private string AutoCalibForm_PassiveEvent(byte[] key, ErosSocket.ErosConLink.SocketClint socket, Socket soket)
        {
            try
            {
                if (this.IsDisposed)
                {
                    Halcon.GetSocket().PassiveEvent -= AutoCalibForm_PassiveEvent;
                    return "";
                }
                string dsat = socket.GetEncoding().GetString(key);
                this.Invoke(new Action(() =>
                {
                    textBox27.AppendText(dsat + Environment.NewLine);
                }));
            }
            catch (Exception)
            {

            }
            return "";
        }

        HalconRunFile.RunProgramFile.HalconRun Halcon;

        AutoCalibPoint AutoCali = new AutoCalibPoint();
        private void calibControls1_Load(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string[] dstas = Directory.GetDirectories(AutoCalibPoint.GetFileName());
                if (dstas.Contains(textBox5.Text))
                {
                    MessageBox.Show("文件夹已存在");
                    return;
                }
                PuData();

            }
            catch (Exception)
            {
            }
        }

        string camPath = "";
        private void button3_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = AutoCalibPoint.GetFileName();
            openFileDialog.Filter = "相机内参.dat|*.dat";
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != "")
            {
                camPath = openFileDialog.FileName;
                AutoCali.ReadCamProg(openFileDialog.FileName, checkBox1.Checked);
                if (checkBox1.Checked)
                {
                    textBox7.Text = AutoCali.TCamParam.ToString();
                }
                else
                {
                    textBox7.Text = AutoCali.camParam.ToString();
                }

            }


        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Application.StartupPath + "\\标定板\\";
            openFileDialog.Filter = "标定板.descr|*.descr|标定板.Cpd|*.CPD";
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != "")
            {
                textBox3.Text = openFileDialog.FileName;
            }
            if (checkBox1.Checked)
            {
                AutoCali.TCalibPaht = textBox3.Text;
            }
            else
            {
                AutoCali.MCalibPaht = textBox3.Text;
            }


        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {

                HTuple hTuple = Halcon.GetRobotBaesPose();
                if (hTuple == null)
                {
                    MessageBox.Show("获取坐标失败");
                    return;
                }
                if (!Halcon.SaveImage(AutoCalibPoint.GetFileName() + "\\" + textBox5.Text + "\\" + numericUpDown2.Value.ToString()))
                {
                    MessageBox.Show("保存失败");
                    return;
                }
                HOperatorSet.WritePose(hTuple, AutoCalibPoint.GetFileName() + "\\" + textBox5.Text + "\\" + numericUpDown2.Value.ToString() + ".dat");
                numericUpDown2.Value++;
                PuData();

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
                if (numericUpDown3.Value > numericUpDown1.Value)
                {
                    MessageBox.Show("标定完成");
                    return;
                }
                string path = "";
                if (checkBox1.Checked)
                {
                    path = AutoCalibPoint.GetFileName() + "\\" + textBox5.Text + "\\T\\" + numericUpDown3.Value.ToString();
                }
                else
                {
                    path = AutoCalibPoint.GetFileName() + "\\" + textBox5.Text + "\\M\\" + numericUpDown3.Value.ToString();
                }

                HOperatorSet.ReadImage(out HObject hObject, path + ".bmp");
                Halcon.Image(hObject);
                Halcon.ShowImage();
                HOperatorSet.ReadPose(path + ".dat", out HTuple tuple);
                AutoCali.RunCalib(hObject, tuple, (int)numericUpDown3.Value, checkBox1.Checked, Halcon.hWindowHalcon());
                numericUpDown3.Value++;
                label4.Text = label3.Text = "/" + numericUpDown1.Value.ToString();
                if (numericUpDown3.Value > numericUpDown1.Value)
                {
                    MessageBox.Show("标定完成");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void button8_Click(object sender, EventArgs e)
        {
            PuData();
            if (AutoCali.ReadCamPar(camPath, textBox3.Text, checkBox1.Checked))
            {
                MessageBox.Show("创建成功");
            }
            else
            {
                MessageBox.Show("创建失败");
            }

        }
        HTuple ToolInBasePose;
        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                TreeNode treeNode = treeView1.GetNodeAt(e.Location);
                if (treeNode == null)
                {
                    return;
                }
                if (treeNode.Tag != null)
                {
                    string path = treeNode.Tag.ToString();
                    if (path.EndsWith(".bmp"))
                    {
                        Vision.GetFocusRunHalcon().ReadImage(path);
                    }
                    else if (path.EndsWith(".dat"))
                    {
                        HOperatorSet.ReadPose(path, out ToolInBasePose);
                        textBox9.Text = (treeNode.Text + ":" + ToolInBasePose.ToString() + Environment.NewLine);
                    }
                }
                else
                {
                    if (textBox5.Text == "")
                    {
                        textBox5.Text = treeNode.Text;
                    }

                    if (e.Button == MouseButtons.Right)
                    {
                        textBox5.Text = treeNode.Text;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        string path = "";
        void PuData()
        {
            try
            {
                treeView1.Nodes.Clear();
                //AutoCalibPoint= autoCalib;
                path = AutoCalibPoint.GetFileName();
                Directory.CreateDirectory(path);
                TreeNode treeNode = new TreeNode();
                treeNode.Name = treeNode.Text = "标定文件";
                Vision2.ErosProjcetDLL.FileCon.FileConStatic.GetFilesToTreeNode(treeNode, path);
                treeView1.Nodes.Add(treeNode);
                TreeNode[] treeNodes = treeView1.Nodes.Find(textBox5.Text, true);
                treeNode.Toggle();
                if (treeNodes.Length == 1)
                {
                    treeNodes[0].Toggle();
                }

                treeView2.Nodes.Clear();
                foreach (var item in Vision.Instance.DicCalib3D)
                {
                    TreeNode treeNode2 = new TreeNode();
                    treeNode2.Name = treeNode2.Text = item.Key;
                    treeNode2.Tag = item.Value;
                    treeView2.Nodes.Add(treeNode2);
                }
            }
            catch (Exception)
            {


            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                Task.Run(() =>
                {
                    string path = "";
                    if (checkBox1.Checked)
                    {
                        path = AutoCalibPoint.GetFileName() + textBox5.Text + "\\T";
                    }
                    else
                    {
                        path = AutoCalibPoint.GetFileName() + textBox5.Text + "\\M";
                    }
                    if (AutoCali.RunUPCalib(path, camPath, textBox3.Text, checkBox1.Checked, Halcon.hWindowHalcon()))
                    {
                        if (checkBox1.Checked)
                        {
                            textBox7.Text = AutoCali.TCamParam.ToString();
                            textBox8.Text = AutoCali.tBaseInCamPose.ToString();
                            textBox9.Text = AutoCali.tCalibInCamPose.ToString();
                        }
                        else
                        {
                            textBox7.Text = AutoCali.camParam.ToString();
                            textBox8.Text = AutoCali.ToolInCamPose.ToString();
                            textBox9.Text = AutoCali.calibInCamPose.ToString();
                        }
                        MessageBox.Show("创建成功");
                    }

                });

            }
            catch (Exception)
            {

            }
        }
        bool DRW;
        private void button9_Click(object sender, EventArgs e)
        {
            try
            {
                if (DRW)
                {
                    return;
                }
                DRW = true;
                //Halcon.GetHWindow().Focus();
                HOperatorSet.DrawPointMod(Halcon.hWindowHalcon(), Halcon.Height / 2,
                Halcon.Width / 2, out HTuple ROW, out HTuple COL);

                textBox6.Text = ROW.ToString();
                textBox11.Text = COL.ToString();
                button10.PerformClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            DRW = false;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {

                if (ToolInBasePose == null)
                {
                    MessageBox.Show("未选择机器人坐标");
                }
                if (Single.TryParse(textBox6.Text, out Single rows) && Single.TryParse(textBox11.Text, out Single cols))
                {
                    AutoCali.Run(AutoCalibPoint.CalibMode.移动抓取, rows, cols, ToolInBasePose, out HTuple x, out HTuple y);
                    if (x == null)
                    {
                        MessageBox.Show("转换失败");
                    }
                    textBox10.Text = x.ToString();
                    textBox4.Text = y.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                if (Halcon.GetRobotBaesPose() != null)
                {
                    ToolInBasePose = Halcon.GetRobotBaesPose();
                    textBox9.Text = ToolInBasePose.ToString();
                }

            }
            catch (Exception)
            {

            }
        }




        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                calidControl1.dataGridView1.Rows.Clear();
                calidControl1.dataGridView1.Rows.Add(10);
                Halcon.SendMesage("AutoTool", Halcon.Name, numericUpDown8.Value.ToString(),
                    numericUpDown5.Value.ToString(),
                    numericUpDown7.Value.ToString());
            }
            catch (Exception)
            {
            }

        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                HTuple rows = new HTuple();
                HTuple col = new HTuple();
                HTuple rowsY = new HTuple();
                HTuple colX = new HTuple();
                Coordinate item = Vision.Instance.DicCoordinate[Halcon.CoordinateName];

                for (int i = 0; i < calidControl1.dataGridView1.Rows.Count; i++)
                {
                    if (calidControl1.dataGridView1.Rows[i].Cells[0].Value != null && calidControl1.dataGridView1.Rows[i].Cells[1].Value != null
                            && calidControl1.dataGridView1.Rows[i].Cells[2].Value != null)
                    {
                        Coordinate.CpointXY setd = item.GetPointRctoYX(Convert.ToDouble(calidControl1.dataGridView1.Rows[i].Cells[1].Value),
                            Convert.ToDouble(calidControl1.dataGridView1.Rows[i].Cells[2].Value));
                        rowsY.Append(setd.Y);
                        colX.Append(setd.X);
                        rows.Append(Convert.ToDouble(calidControl1.dataGridView1.Rows[i].Cells[1].Value.ToString()));
                        col.Append(Convert.ToDouble(calidControl1.dataGridView1.Rows[i].Cells[2].Value.ToString()));
                    }
                }
                //Vision.Pts_to_best_circle(out HObject hObject4, rowsY, colX, rowsY.Length, "circle",
                //     out HTuple rowCenter2, out HTuple colCenter2, out HTuple radius2, out HTuple hvStartPhi2,
                //     out HTuple hvENdPhi2, out HTuple porder2, out HTuple hvAngle2);
                Vision.Pts_to_best_circle(out HObject hObject, rows, col, rows.Length, "circle",
                         out HTuple rowCenter, out HTuple colCenter, out HTuple radius, out HTuple hvStartPhi,
                         out HTuple hvENdPhi, out HTuple porder, out HTuple hvAngle);
                HOperatorSet.GenCrossContourXld(out HObject hObject2, rows, col, 60, 0);
                HOperatorSet.GenCrossContourXld(out HObject hObject1, rowCenter, colCenter, 100, 0);
                Halcon.AddOBJ(hObject2);
                Halcon.AddOBJ(hObject);
                Halcon.AddOBJ(hObject1);
                HOperatorSet.GenContourPolygonXld(out HObject hObject3, new HTuple(rowCenter, rows[rows.Length - 1]),
                    new HTuple(colCenter, col[col.Length - 1]));
                Halcon.AddOBJ(hObject3);
                HOperatorSet.GenContourPolygonXld(out HObject hObject5, new HTuple(rowCenter, rowCenter),
                new HTuple(colCenter, col[col.Length - 1]));
                Halcon.AddOBJ(hObject5);
                Halcon.ShowObj();

                HOperatorSet.AffineTransPixel(item.CoordHanMat2DXY, colCenter, rowCenter, out HTuple qx1, out HTuple qy1);
                HOperatorSet.AffineTransPixel(item.CoordHanMat2DXY, col[col.Length - 1], rows[rows.Length - 1], out HTuple qx32, out HTuple qy32);
                Coordinate.CpointXY set = item.GetPointRctoYX(rowCenter, colCenter);
                Coordinate.CpointXY sett = item.GetPointRctoYX(rows[rows.Length - 1], col[col.Length - 1]);
                HOperatorSet.DistancePp(set.X, set.Y, sett.X, sett.Y, out HTuple din);
                if (Halcon.GetRobotXYZUVW(out double? x, out double? y, out double? z, out double? u, out double? v, out double? w))
                {
                }

                //HOperatorSet.HomMat2dRotate(item.CoordHanMat2DXY, new HTuple(u-180).TupleRad(), colCenter2, rowCenter2, out HTuple hommat);
                //HOperatorSet.AffineTransPoint2d(hommat, rows.TupleSelect(rows.Length-1), col.TupleSelect(rows.Length - 1), 
                //    out HTuple qx, out HTuple qy);
                //HOperatorSet.AffineTransPoint2d(hommat, rows.TupleSelect(rows.Length - 1), col.TupleSelect(rows.Length - 1), out qx, out qy);
                //HOperatorSet.HomMat2dTranslate(hommat, qx, qy, out  hommat);
                textBox12.AppendText("中心R:" + rowCenter.ToString() + "中心C：" + colCenter.ToString() + Environment.NewLine);
                textBox12.AppendText("中心X:" + set.X.ToString() + "中心Y：" + set.Y.ToString() + Environment.NewLine);
                textBox12.AppendText("X:" + sett.X.ToString() + "Y：" + sett.Y.ToString() + Environment.NewLine);
                textBox12.AppendText("半径:" + radius.ToString() + "半径MM：" + din.ToString() + Environment.NewLine);
                textBox12.AppendText("机械手角度:" + u.ToString() + Environment.NewLine);
                HTuple ds = sett.X - set.X;

                HTuple dst = set.Y - sett.Y;
                textBox12.AppendText("最后一点X:" + ds.TupleString("0.003f") + "Y:" + dst.TupleString("0.003f") + Environment.NewLine);
                textBox12.AppendText("X:" + (qx1 - qx32).TupleString("0.003f") + "Y:" + (qy1 - qy32).TupleString("0.003f") + Environment.NewLine);

                //HOperatorSet.AffineTransPoint2d(hommat, rowCenter, colCenter, out  qx, out  qy);
                //HOperatorSet.HomMat2dTranslate(item.CoordHanMat2DXY, rowCenter, colCenter, out  hommat);
                //HOperatorSet.HomMat2dRotate(hommat, 0, rowCenter, colCenter, out hommat);

            }
            catch (Exception)
            {


            }

        }

        private void button13_Click(object sender, EventArgs e)
        {
            try
            {

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.InitialDirectory = AutoCalibPoint.GetFileName();
                openFileDialog.Filter = "相机工具.dat|*.dat";
                openFileDialog.ShowDialog();
                if (openFileDialog.FileName != "")
                {
                    HOperatorSet.ReadPose(openFileDialog.FileName, out AutoCali.ToolInCamPose);
                    textBox8.Text = AutoCali.ToolInCamPose.ToString();
                }
            }
            catch (Exception)
            {


            }

        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {

                Halcon = Vision.GetRunNameVision(textBox28.Text);
                Halcon.ShowImage();
                if (AutoCali.ReadCamCalib(Halcon.Image(), textBox3.Text, checkBox1.Checked))
                {
                    if (checkBox1.Checked)
                    {
                        AutoCalibPoint.Disp3DCoordSystem(AutoCali.TCamParam, AutoCali.TCalibInCamPose, 0.01, Halcon);

                        textBox13.Text = AutoCali.TCalibInCamPose.ToString();
                        HOperatorSet.ConvertPoseType(AutoCali.TCalibInCamPose, "Rp+T", "abg", "point", out HTuple posset);
                        textBox13.Text += posset.ToString();
                    }
                    else
                    {
                        AutoCalibPoint.Disp3DCoordSystem(AutoCali.camParam, AutoCali.calibInCamPose, 0.01, Halcon);
                        textBox13.Text = AutoCali.calibInCamPose.ToString();
                        HOperatorSet.ConvertPoseType(AutoCali.calibInCamPose, "Rp+T", "abg", "point", out HTuple posset);
                        textBox13.Text += posset.ToString();
                    }
                }
                else
                {
                    MessageBox.Show("读取出错");
                }



            }
            catch (Exception)
            {
            }
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Vision.Instance.DicCalib3D.ContainsKey(textBox5.Text))
                {
                    Vision.Instance.DicCalib3D.Add(textBox5.Text, AutoCali);
                    MessageBox.Show("添加成功");
                }
                else
                {
                    MessageBox.Show(textBox5.Text + "已存在添加失败！");
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                //HTuple rows= Convert.ToDouble(textBox14.Text);
                //HTuple cols = Convert.ToDouble(textBox16.Text);
                //if (ToolInBasePose==null)
                //{
                //    MessageBox.Show("未获取位置");
                //}
                //AutoCali.Run(AutoCalibPoint.CalibMode.移动抓取, rows, cols,  ToolInBasePose, out HTuple x, out HTuple y,out HTuple z,out HTuple u,out HTuple v,out HTuple w);
                //textBox1.AppendText("X:" + x.ToString() + Environment.NewLine);
                //textBox1.AppendText("Y:" + y.ToString() + Environment.NewLine);
                //textBox1.AppendText("Z:" + z.ToString() + Environment.NewLine);
                //textBox1.AppendText("U:" + u.ToString() + Environment.NewLine);
                //textBox1.AppendText("V:" + v.ToString() + Environment.NewLine);
                //textBox1.AppendText("W:" + w.ToString() + Environment.NewLine);
            }
            catch (Exception tx)
            {
            }

        }

        private void button17_Click(object sender, EventArgs e)
        {
            try
            {
                if (DRW)
                {
                    return;
                }
                DRW = true;

                HOperatorSet.DrawPointMod(Halcon.hWindowHalcon(), Halcon.Height / 2,
                    Halcon.Width / 2, out HTuple ROW, out HTuple COL);
                //textBox14.Text = ROW.ToString();
                //textBox16.Text = COL.ToString();
                //button16.PerformClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            DRW = false;
        }

        private void treeView2_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                TreeNode treeNode = treeView2.GetNodeAt(e.Location);

                if (treeNode == null)
                {
                    return;
                }
                treeView2.SelectedNode = treeNode;
                propertyGrid1.SelectedObject = Vision.Instance.DicCalib3D[treeNode.Text];
                if (treeNode.Tag is Calib.AutoCalibPoint)
                {
                    AutoCali = treeNode.Tag as AutoCalibPoint;
                    groupBox8.Text = "标定对象：" + treeView2.SelectedNode.Text;
                }

                if (e.Button == MouseButtons.Right)
                {

                }

            }
            catch (Exception)
            {

            }

        }

        private void 读取标定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (AutoCali == null)
                {
                    AutoCali = new AutoCalibPoint();
                }

                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
                fbd.Description = "请选择文件夹";
                fbd.SelectedPath = path;
                System.Windows.Forms.DialogResult dialog = FolderBrowserLauncher.ShowFolderBrowser(fbd);
                if (dialog == System.Windows.Forms.DialogResult.OK)
                {
                    if (AutoCali.ReadCalib(fbd.SelectedPath, false))
                    {
                        TreeNode treeNode = new TreeNode();
                        treeNode.Text = treeNode.Name = Path.GetFileNameWithoutExtension(fbd.SelectedPath);

                        if (!Vision.Instance.DicCalib3D.ContainsKey(treeNode.Text))
                        {
                            Vision.Instance.DicCalib3D.Add(treeNode.Text, AutoCali);
                            treeNode.Tag = AutoCali;
                            treeView2.Nodes.Add(treeNode);
                        }
                        else
                        {
                            Vision.Instance.DicCalib3D[treeNode.Text] = AutoCali;
                        }
                        MessageBox.Show("读取成功");
                    }
                    else
                    {
                        MessageBox.Show("读取失败");
                    }
                }
                PuData();
                return;
            }
            catch (Exception)
            {
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    if (MessageBox.Show("确定要删除标定:" + treeView2.SelectedNode.Text, "删除标定", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (Vision.Instance.DicCalib3D.ContainsKey(treeView2.SelectedNode.Text))
                        {
                            Vision.Instance.DicCalib3D.Remove(treeView2.SelectedNode.Text);
                            treeView2.SelectedNode.Remove();

                        }
                        else
                        {
                            MessageBox.Show("不存在标定:" + treeView2.SelectedNode.Text);
                        }
                    }
                }
                catch (Exception)
                {


                }

            }
            catch (Exception)
            {
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            try
            {
                if (Halcon.GetRobotBaesPose() != null)
                {
                    ToolInBasePose = Halcon.GetRobotBaesPose();
                    //textBox17.Text = ToolInBasePose.ToString();
                }

            }
            catch (Exception)
            {

            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            try
            {
                //if (DRW)
                //{
                //    return;
                //}
                //DRW = true;
                //HOperatorSet.DrawPointMod(Halcon.hWindowHalcon(), Halcon.Height / 2,
                //    Halcon.Width / 2, out HTuple ROW, out HTuple COL);
                //textBox2.Text = ROW.ToString();
                //textBox15.Text = COL.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            DRW = false;
        }





        private void button22_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    Halcon.SendMesage("CCD1", textBox26.Text,textBox25.Text,textBox18.Text);
            //}
            //catch (Exception)
            //{
            //}
        }

        private void button23_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    Halcon.SendMesage("CCD2", textBox21.Text, textBox19.Text, textBox24.Text);
            //}
            //catch (Exception)
            //{
            //}
        }

        private void button25_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    Halcon.SendMesage("CCD0",textBox20.Text,textBox22.Text,textBox23.Text);
            //}
            //catch (Exception)
            //{
            //}
        }

        private void button24_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    Halcon.SendMesage("GoOn",numericUpDown8.Value.ToString());
            //}
            //catch (Exception)
            //{
            //}
        }

        private void button26_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    Halcon.SendMesage("GoTOff",numericUpDown4.Value.ToString());
            //}
            //catch (Exception)
            //{
            //}
        }

        private void 读取固定相机标定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                if (AutoCali == null)
                {
                    AutoCali = new AutoCalibPoint();
                }
                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
                fbd.Description = "请选择文件夹";

                fbd.SelectedPath = path + treeView2.SelectedNode.Text + "\\";
                System.Windows.Forms.DialogResult dialog = FolderBrowserLauncher.ShowFolderBrowser(fbd);
                if (dialog == System.Windows.Forms.DialogResult.OK)
                {
                    if (AutoCali.ReadCalib(fbd.SelectedPath, true))
                    {
                        TreeNode treeNode = new TreeNode();
                        treeNode.Text = treeNode.Name = Path.GetFileNameWithoutExtension(fbd.SelectedPath);

                        if (!Vision.Instance.DicCalib3D.ContainsKey(treeNode.Text))
                        {
                            Vision.Instance.DicCalib3D.Add(treeNode.Text, AutoCali);
                            treeNode.Tag = AutoCali;
                            treeView2.Nodes.Add(treeNode);
                        }
                        else
                        {
                            Vision.Instance.DicCalib3D[treeNode.Text] = AutoCali;
                        }
                        MessageBox.Show("读取成功");
                    }
                    else
                    {
                        MessageBox.Show("读取失败");
                    }
                }
                PuData();
                return;
            }
            catch (Exception)
            {
            }
        }

        private void button20_Click_1(object sender, EventArgs e)
        {
            try
            {
                //    HTuple rows = Convert.ToDouble(textBox14.Text);
                //    HTuple cols = Convert.ToDouble(textBox16.Text);
                //    rows.Append(Convert.ToDouble(textBox14.Text));
                //    cols.Append(Convert.ToDouble(textBox16.Text));
                //    if (ToolInBasePose == null)
                //    {
                //        MessageBox.Show("未获取位置");
                //    }
                //    AutoCali.Run(AutoCalibPoint.CalibMode.固定相机, rows, cols, ToolInBasePose, out HTuple x, out HTuple y, out HTuple z, out HTuple u, out HTuple v, out HTuple w);
                //    textBox1.AppendText("X:" + x.ToString() + Environment.NewLine);
                //    textBox1.AppendText("Y:" + y.ToString() + Environment.NewLine);
                //    textBox1.AppendText("Z:" + z.ToString() + Environment.NewLine);
                //    textBox1.AppendText("U:" + u.ToString() + Environment.NewLine);
                //    textBox1.AppendText("V:" + v.ToString() + Environment.NewLine);
                //    textBox1.AppendText("W:" + w.ToString() + Environment.NewLine);
            }
            catch (Exception tx)
            {
            }

        }

        private void button21_Click_1(object sender, EventArgs e)
        {
            try
            {
                //HTuple rows = Convert.ToDouble(textBox14.Text);
                //HTuple cols = Convert.ToDouble(textBox16.Text);
                // rows.Append(Convert.ToDouble(textBox14.Text));
                // cols.Append(Convert.ToDouble(textBox16.Text));
                //if (ToolInBasePose == null)
                //{
                //    MessageBox.Show("未获取位置");
                //}
                //AutoCali.Run(AutoCalibPoint.CalibMode.移动放置, rows, cols, ToolInBasePose, out HTuple x, out HTuple y, out HTuple z, out HTuple u, out HTuple v, out HTuple w);
                //textBox1.AppendText("X:" + x.ToString() + Environment.NewLine);
                //textBox1.AppendText("Y:" + y.ToString() + Environment.NewLine);
                //textBox1.AppendText("Z:" + z.ToString() + Environment.NewLine);
                //textBox1.AppendText("U:" + u.ToString() + Environment.NewLine);
                //textBox1.AppendText("V:" + v.ToString() + Environment.NewLine);
                //textBox1.AppendText("W:" + w.ToString() + Environment.NewLine);
            }
            catch (Exception tx)
            {
            }

        }

        private void button27_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox5.Text == "")
                {
                    MessageBox.Show("未选择标定名");
                    return;
                }
                Halcon.SendMesage("AutoCalib3D", textBox28.Text, numericUpDown9.Value.ToString(),
                    textBox5.Text, numericUpDown10.Value.ToString(),
                    numericUpDown11.Value.ToString(), numericUpDown13.Value.ToString(), numericUpDown12.Value.ToString());
            }
            catch (Exception)
            {
            }
        }

        private void button28_Click(object sender, EventArgs e)
        {
            try
            {
                if (Vision.Instance.DicCalib3D.ContainsKey(textBox5.Text))
                {
                    Vision.Instance.DicCalib3D[textBox5.Text] = AutoCali;
                }
                else
                {
                    Vision.Instance.DicCalib3D.Add(textBox5.Text, AutoCali);
                }
            }
            catch (Exception)
            {
            }
        }

        private void button29_Click(object sender, EventArgs e)
        {

        }

        private void 执行九点标定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Halcon.SendMesage("Calib9", Halcon.Name, "5");
            }
            catch (Exception)
            {
            }
        }

        private void button30_Click(object sender, EventArgs e)
        {
            try
            {
                calidControl1.dataGridView1.Rows.Clear();
                calidControl1.dataGridView1.Rows.Add(10);
                Halcon.SendMesage("Calib9", Halcon.Name, numericUpDown4.Value.ToString(), numericUpDown6.Value.ToString());
            }
            catch (Exception)
            {
            }
        }

        private void AutoCalibForm_Load(object sender, EventArgs e)
        {
            tabPage2.Dispose();
            //tabControl1.
        }
    }
}
