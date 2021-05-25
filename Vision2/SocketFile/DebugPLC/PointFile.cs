using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Excel;

namespace ErosSocket.DebugPLC
{
    public class PointFile
    {
        public static PointFile ToPointFile(string paStr)
        {
            PointFile pointFile = new PointFile();



            return pointFile;
        }
        static Dictionary<string, List<PointFile>> FilePoints;
        static AxisGrubXY axisGrubXY = new AxisGrubXY();
        public Int16 P;
        public string Name;
        public double? X;
        public double? Y;
        public double? Z;
        public double? U;
        public double? V;
        public double? W;
        public sbyte Local;
        public sbyte Hand;
        public sbyte Elbow;
        public sbyte Wrist;
        public sbyte J1Flag;
        public sbyte J4Flag;
        public sbyte J6Flag;
        public string PointSt;
        public string Pstring
        {
            get
            {
                char sipt = ',';
                string itmes = this.P.ToString() + sipt + this.Name + sipt + this.X + sipt + this.Y + sipt + this.Z + sipt +
             Convert.ToSingle(this.U).ToString("0.00") + sipt +
             Convert.ToSingle(this.V).ToString("0.00") + sipt +
             Convert.ToSingle(this.W).ToString("0.00") + sipt +
             this.Hand + sipt + this.Elbow + sipt + this.Wrist + sipt +
               this.J1Flag + sipt + this.J4Flag + sipt + this.J6Flag;
                return itmes;
            }
        }

        public static AxisGrubXY GetAxisGrubXY()
        {
            return axisGrubXY;
        }
        /// <summary>
        /// 返回点数据
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, List<PointFile>> GetPointFile()
        {
            if (FilePoints.ContainsKey(""))
            {
                FilePoints.Remove("");
            }
            return FilePoints;
        }
        /// <summary>
        /// 点文件内的点名是否存在
        /// </summary>
        /// <param name="fileNamePi"></param>
        /// <param name="name">名称</param>
        /// <returns>返回true</returns>
        public static bool IsPointContainsKey(string fileNamePi, string name)
        {
            if (!FilePoints.ContainsKey(fileNamePi))
            {
                return true;
            }
            for (int i = 0; i < GetPointFile(fileNamePi).Count; i++)
            {
                if (GetPointFile(fileNamePi)[i].Name == "")
                {
                    GetPointFile(fileNamePi).RemoveAt(i);
                }
                if (GetPointFile(fileNamePi)[i].Name == name)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获得点文件内，指定点名称的位置
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="PointName">点名</param>
        /// <returns></returns>
        public static PointFile GetPointName(string fileName, string PointName)
        {
            for (int i = 0; i < GetPointFile(fileName).Count; i++)
            {
                if (GetPointFile(fileName)[i].Name == PointName)
                {
                    return GetPointFile(fileName)[i];
                }
            }
            return new PointFile();
        }
        /// <summary>
        /// 查找点文件指定的编号点，
        /// </summary>
        /// <param name="fileName">点文件</param>
        /// <param name="pid">点编号</param>
        /// <returns>不存在返回Null</returns>
        public static PointFile GetPointP(string fileName, uint pid)
        {
            if (GetPointFile(fileName) == null)
            {
                return null;
            }
            var itme = from li in GetPointFile(fileName)
                       where li.P == pid
                       select li;
            foreach (var item in itme)
            {
                return item as PointFile;
            }

            return itme as PointFile;
        }
        /// <summary>
        /// 获得点文件内点集合
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static List<PointFile> GetPointFile(string fileName)
        {
            if (FilePoints.ContainsKey(fileName))
            {
                return FilePoints[fileName];
            }
            return null;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="pointNmae"></param>
        public static void Remove(string fileName, string pointNmae)
        {
            var itme = from li in PointFile.GetPointFile(fileName)
                       where li.Name == pointNmae
                       select li;
            try
            {
                foreach (var item in itme)
                {
                    PointFile pointFilet = item as PointFile;
                    PointFile.GetPointFile(fileName).Remove(pointFilet);
                }
            }
            catch (Exception)
            {

            }

        }
        /// <summary>
        /// 保存点文件到本地
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool SavePoint(string path = null)
        {

            if (path == null)
            {
                path = Vision2.ErosProjcetDLL.Project.ProjectINI.In.ProjectPathRun + "\\点位文件\\";
            }
            if (FilePoints != null)
            {
                foreach (var item in FilePoints)
                {
                    try
                    {
                        item.Value.ToArray();
                        if (File.Exists(path + item.Key + ".xls"))
                        {
                            File.Delete(path + item.Key + ".xls");
                        }
                        List<string> list = new List<string>();
                        list.Add("点编号");
                        list.Add("点名称");
                        list.Add("X位置");
                        list.Add("Y位置");
                        list.Add("Z位置");
                        list.Add("U位置");
                        list.Add("V位置");
                        list.Add("W位置");
                        list.Add("Local");
                        list.Add("Hand");
                        list.Add("Elbow");
                        list.Add("Wrist");
                        list.Add("J1F");
                        list.Add("J4F");
                        list.Add("J6F");
                        list.Add("点描述");
                        Npoi.AddWriteColumnToExcel(path + item.Key + ".xls", "点文件", list.ToArray());
                        for (int i = 0; i < item.Value.Count; i++)
                        {
                            list = new List<string>();
                            list.Add(item.Value[i].P.ToString());
                            list.Add(Convert.ToString(item.Value[i].Name));
                            list.Add(Convert.ToString(item.Value[i].X));
                            list.Add(Convert.ToString(item.Value[i].Y));
                            list.Add(Convert.ToString(item.Value[i].Z));
                            list.Add(Convert.ToString(item.Value[i].U));
                            list.Add(Convert.ToString(item.Value[i].V));
                            list.Add(Convert.ToString(item.Value[i].W));

                            list.Add(item.Value[i].Local.ToString());
                            list.Add(Convert.ToString(item.Value[i].Hand));
                            list.Add(Convert.ToString(item.Value[i].Elbow));
                            list.Add(Convert.ToString(item.Value[i].Wrist));
                            list.Add(Convert.ToString(item.Value[i].J1Flag));
                            list.Add(Convert.ToString(item.Value[i].J4Flag));
                            list.Add(Convert.ToString(item.Value[i].J6Flag));
                            if (item.Value[i].PointSt != null)
                            {
                                list.Add(item.Value[i].PointSt.ToString());
                            }
                            else
                            {
                                list.Add("");
                            }
                            Npoi.AddRosWriteToExcel(path + item.Key + ".xls", "点文件", list.ToArray());
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("读取失败" + ex.Message);
                    }
                }
                axisGrubXY.SavePoint(path + "轨迹文件");
                return true;
            }
            return false;
        }
        /// <summary>
        /// 读取点位文件
        /// </summary>
        /// <returns></returns>
        public static bool ReadPoint(string path = null)
        {
            if (path == null)
            {
                path = Vision2.ErosProjcetDLL.Project.ProjectINI.In.ProjectPathRun + "\\点位文件\\";
            }
            Directory.CreateDirectory(path);
            string[] lPaths = Directory.GetFiles(path);
            FilePoints = new Dictionary<string, List<PointFile>>();
            int errn = 0;
            for (int i = 0; i < lPaths.Length; i++)
            {
                try
                {
                    DataTable dataTable = Npoi.ReadExcelFile(lPaths[i], "点文件");
                    if (dataTable!=null)
                    {
                        List<PointFile> pointFiles = new List<PointFile>();
                        for (int i2 = 0; i2 < dataTable.Rows.Count; i2++)
                        {
                            PointFile pointFile = new PointFile();
                            try
                            {
                                pointFile.P = Convert.ToInt16(dataTable.Rows[i2].ItemArray[0]);
                                pointFile.Name = dataTable.Rows[i2].ItemArray[1].ToString();
                                pointFile.X = Convert.ToSingle(dataTable.Rows[i2].ItemArray[2]);
                                pointFile.Y = Convert.ToSingle(dataTable.Rows[i2].ItemArray[3]);
                                pointFile.Z = Convert.ToSingle(dataTable.Rows[i2].ItemArray[4]);
                                pointFile.U = Convert.ToSingle(dataTable.Rows[i2].ItemArray[5]);
                                pointFile.V = Convert.ToSingle(dataTable.Rows[i2].ItemArray[6]);
                                pointFile.W = Convert.ToSingle(dataTable.Rows[i2].ItemArray[7]);
                                pointFile.Local = Convert.ToSByte(dataTable.Rows[i2].ItemArray[8]);
                                pointFile.Hand = Convert.ToSByte(dataTable.Rows[i2].ItemArray[9]);
                                pointFile.Elbow = Convert.ToSByte(dataTable.Rows[i2].ItemArray[10]);
                                pointFile.Wrist = Convert.ToSByte(dataTable.Rows[i2].ItemArray[11]);
                                pointFile.J1Flag = Convert.ToSByte(dataTable.Rows[i2].ItemArray[12]);
                                pointFile.J4Flag = Convert.ToSByte(dataTable.Rows[i2].ItemArray[13]);
                                pointFile.J6Flag = Convert.ToSByte(dataTable.Rows[i2].ItemArray[14]);
                                pointFile.PointSt = Convert.ToString(dataTable.Rows[i2].ItemArray[15].ToString());
                            }
                            catch (Exception ex)
                            {
                            }
                            pointFiles.Add(pointFile);
                        }
                        FilePoints.Add(Path.GetFileNameWithoutExtension(lPaths[i]), pointFiles);
                    }
         
                }
                catch (Exception ex)
                {
                    errn++;
                    MessageBox.Show("读取失败" + ex.Message);
                }
            }

            axisGrubXY.ReadPoint(path + "轨迹文件\\");
            if (errn == 0)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 更新点表格
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dataGridView"></param>
        public static void GetPointDataG(string name, DataGridView dataGridView)
        {


            if (FilePoints == null)
            {
                FilePoints = new Dictionary<string, List<PointFile>>();
            }
            else
            {
                var itme = from li in FilePoints[name]
                           orderby li.P ascending
                           select li;
                List<PointFile> ListP = new List<PointFile>();

                ListP.AddRange(itme);
                FilePoints[name] = ListP;

                GetPointDataG(FilePoints[name], dataGridView);
            }


        }
        public static void GetPointDataG(List<PointFile> ListPiles, DataGridView dataGridView)
        {
            dataGridView.Rows.Clear();

            if (dataGridView.Columns.Count >= 15)
            {
                int idx = 0;

                foreach (var item in ListPiles)
                {
                    idx = dataGridView.Rows.Add();
                    dataGridView.Rows[idx].Cells[0].Value = item.P;
                    dataGridView.Rows[idx].Cells[1].Value = item.Name;
                    dataGridView.Rows[idx].Cells[2].Value = item.X;
                    dataGridView.Rows[idx].Cells[3].Value = item.Y;
                    dataGridView.Rows[idx].Cells[4].Value = item.Z;
                    dataGridView.Rows[idx].Cells[5].Value = item.U;
                    dataGridView.Rows[idx].Cells[6].Value = item.V;
                    dataGridView.Rows[idx].Cells[7].Value = item.W;
                    dataGridView.Rows[idx].Cells[8].Value = item.Local.ToString();
                    dataGridView.Rows[idx].Cells[9].Value = item.Hand.ToString();
                    dataGridView.Rows[idx].Cells[10].Value = item.Elbow.ToString();
                    dataGridView.Rows[idx].Cells[11].Value = item.Wrist.ToString();
                    dataGridView.Rows[idx].Cells[12].Value = item.J1Flag.ToString();
                    dataGridView.Rows[idx].Cells[13].Value = item.J4Flag.ToString();
                    dataGridView.Rows[idx].Cells[14].Value = item.J6Flag.ToString();
                    dataGridView.Rows[idx].Cells[15].Value = item.PointSt;
                    idx++;
                }
                //         dataGridView.Rows.Add();
                //if (dataGridView.Rows.Count <= ListPiles.Count)
                //{

                //    idx = dataGridView.Rows.AddCopies(dataGridView.Rows.Count-1,  ListPiles.Count- dataGridView.Rows.Count );
                //}

                //    while (dataGridView.Rows.Count > ListPiles.Count())
                //    {
                //        dataGridView.Rows.RemoveAt(dataGridView.Rows.Count - 1);
                //    }

                //for (int i = 0; i < ListPiles.Count; i++)
                //{

                //    dataGridView.Rows[i].Cells[0].Value = ListPiles[i].P;
                //    dataGridView.Rows[i].Cells[1].Value = ListPiles[i].Name;
                //    dataGridView.Rows[i].Cells[2].Value = ListPiles[i].X;
                //    dataGridView.Rows[i].Cells[3].Value = ListPiles[i].Y;
                //    dataGridView.Rows[i].Cells[4].Value = ListPiles[i].Z;
                //    dataGridView.Rows[i].Cells[5].Value = ListPiles[i].U;
                //    dataGridView.Rows[i].Cells[6].Value = ListPiles[i].V;
                //    dataGridView.Rows[i].Cells[7].Value = ListPiles[i].W;
                //    dataGridView.Rows[i].Cells[8].Value = ListPiles[i].Local.ToString();
                //    dataGridView.Rows[i].Cells[9].Value = ListPiles[i].Hand.ToString();
                //    dataGridView.Rows[i].Cells[10].Value = ListPiles[i].Elbow.ToString();
                //    dataGridView.Rows[i].Cells[11].Value = ListPiles[i].Wrist.ToString();
                //    dataGridView.Rows[i].Cells[12].Value = ListPiles[i].J1Flag.ToString();
                //    dataGridView.Rows[i].Cells[13].Value = ListPiles[i].J4Flag.ToString();
                //    dataGridView.Rows[i].Cells[14].Value = ListPiles[i].J6Flag.ToString();
                //    dataGridView.Rows[i].Cells[15].Value = ListPiles[i].PointSt;

                //}
            }
        }
        /// <summary>
        /// 将表格保存到点文件
        /// </summary>
        /// <param name="name"></param>
        /// <param name="dataGridView"></param>
        public static void SetPointDatag(string name, DataGridView dataGridView)
        {
            if (name == null || name == "")
            {
                System.Windows.Forms.MessageBox.Show("名称为空");
                return;
            }
            if (dataGridView.Columns.Count >= 9)
            {
                if (FilePoints == null)
                {
                    FilePoints = new Dictionary<string, List<PointFile>>(); ;
                }
                List<PointFile> list = new List<PointFile>();
                for (int idx = 0; idx < dataGridView.Rows.Count; idx++)
                {
                    try
                    {
                        if (dataGridView.Rows[idx].Cells[0].Value == null)
                        {
                            continue;
                        }
                        dataGridView.Rows[idx].Cells[0].Style.BackColor = System.Drawing.Color.White;
                        PointFile pointFile = new PointFile();

                        pointFile.P = Convert.ToInt16(dataGridView.Rows[idx].Cells[0].Value);
                        //dataGridView.Rows[idx].DefaultCellStyle.BackColor = System.Drawing.Color.White;
                        if (dataGridView.Rows[idx].Cells[1].Value == null)
                        {
                            list.Add(pointFile);
                            continue;
                        }
                        else
                        {
                            dataGridView.Rows[idx].Cells[1].Style.BackColor = System.Drawing.Color.White;
                            pointFile.Name = dataGridView.Rows[idx].Cells[1].Value.ToString();
                        }
                        if (pointFile.Name == "")
                        {
                            continue;
                        }
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (list[i].Name == pointFile.Name)
                            {
                                MessageBox.Show("点名称重复" + pointFile.Name);
                                return;
                            }
                        }
                        float d = 0f;
                        if (dataGridView.Rows[idx].Cells[2].Value != null && Single.TryParse(dataGridView.Rows[idx].Cells[2].Value.ToString(), out d))
                        {
                            pointFile.X = Convert.ToSingle(dataGridView.Rows[idx].Cells[2].Value.ToString());
                            dataGridView.Rows[idx].Cells[2].Style.BackColor = System.Drawing.Color.White;
                        }
                        else
                        {
                            dataGridView.Rows[idx].Cells[2].Style.BackColor = System.Drawing.Color.Red;
                            pointFile.X = null;
                        }
                        if (dataGridView.Rows[idx].Cells[3].Value != null && Single.TryParse(dataGridView.Rows[idx].Cells[3].Value.ToString(), out d))
                        {
                            pointFile.Y = Convert.ToSingle(dataGridView.Rows[idx].Cells[3].Value.ToString());
                            dataGridView.Rows[idx].Cells[3].Style.BackColor = System.Drawing.Color.White;
                        }
                        else
                        {
                            dataGridView.Rows[idx].Cells[3].Style.BackColor = System.Drawing.Color.Red;
                            pointFile.Y = null;
                        }
                        if (dataGridView.Rows[idx].Cells[4].Value != null && Single.TryParse(dataGridView.Rows[idx].Cells[4].Value.ToString(), out d))
                        {
                            pointFile.Z = Convert.ToSingle(dataGridView.Rows[idx].Cells[4].Value.ToString());
                            dataGridView.Rows[idx].Cells[4].Style.BackColor = System.Drawing.Color.White;
                        }
                        else
                        {
                            dataGridView.Rows[idx].Cells[4].Style.BackColor = System.Drawing.Color.Red;
                            pointFile.Z = null;
                        }
                        if (dataGridView.Rows[idx].Cells[5].Value != null && Single.TryParse(dataGridView.Rows[idx].Cells[5].Value.ToString(), out d))
                        {
                            pointFile.U = Convert.ToSingle(dataGridView.Rows[idx].Cells[5].Value.ToString());
                            dataGridView.Rows[idx].Cells[5].Style.BackColor = System.Drawing.Color.White;
                        }
                        else
                        {
                            dataGridView.Rows[idx].Cells[5].Style.BackColor = System.Drawing.Color.Red;
                            pointFile.U = null;
                        }
                        if (dataGridView.Rows[idx].Cells[6].Value != null && Single.TryParse(dataGridView.Rows[idx].Cells[6].Value.ToString(), out d))
                        {
                            pointFile.V = Convert.ToSingle(dataGridView.Rows[idx].Cells[6].Value.ToString());
                            dataGridView.Rows[idx].Cells[6].Style.BackColor = System.Drawing.Color.White;
                        }
                        else
                        {
                            dataGridView.Rows[idx].Cells[6].Style.BackColor = System.Drawing.Color.Red;
                            pointFile.V = null;
                        }
                        if (dataGridView.Rows[idx].Cells[7].Value != null && Single.TryParse(dataGridView.Rows[idx].Cells[7].Value.ToString(), out d))
                        {
                            pointFile.W = Convert.ToSingle(dataGridView.Rows[idx].Cells[7].Value.ToString());
                            dataGridView.Rows[idx].Cells[7].Style.BackColor = System.Drawing.Color.White;
                        }
                        else
                        {
                            dataGridView.Rows[idx].Cells[7].Style.BackColor = System.Drawing.Color.Red;
                            pointFile.W = null;
                        }
                        pointFile.Local = Convert.ToSByte(dataGridView.Rows[idx].Cells[8].Value);
                        pointFile.Hand = Convert.ToSByte(dataGridView.Rows[idx].Cells[9].Value);
                        pointFile.Elbow = Convert.ToSByte(dataGridView.Rows[idx].Cells[10].Value);
                        pointFile.Wrist = Convert.ToSByte(dataGridView.Rows[idx].Cells[11].Value);
                        pointFile.J1Flag = Convert.ToSByte(dataGridView.Rows[idx].Cells[12].Value);
                        pointFile.J4Flag = Convert.ToSByte(dataGridView.Rows[idx].Cells[13].Value);
                        pointFile.J6Flag = Convert.ToSByte(dataGridView.Rows[idx].Cells[14].Value);
                        if (dataGridView.Rows[idx].Cells[15].Value != null)
                        {
                            pointFile.PointSt = dataGridView.Rows[idx].Cells[15].Value.ToString();
                        }
                        list.Add(pointFile);
                    }
                    catch (Exception)
                    {


                    }

                }
                if (FilePoints.ContainsKey(name))
                {
                    FilePoints.Remove(name);
                }
                FilePoints.Add(name, list);
            }
        }
    }
}
