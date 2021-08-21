using ErosSocket.DebugPLC.Robot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Vision2.ErosProjcetDLL.Excel;
using Vision2.ErosProjcetDLL.Project;

namespace ErosSocket.DebugPLC
{
    /// <summary>
    /// 执行组合接口
    /// </summary>
    public interface IAxisGrub
    {
        /// <summary>
        /// 执行计时
        /// </summary>
        System.Diagnostics.Stopwatch Watch { get; }

        string Execnte_Code { get; set; }

        /// <summary>
        ///
        /// </summary>
        string Name { get; }

        [Editor(typeof(PLC.ListCyinderControl.Editor), typeof(UITypeEditor))]
        List<string> ListCylin { get; set; }

        List<string> ListPLCIO { get; set; }

        string PointFileName { get; set; }

        DebugRobot DebugFormShow();

        string PRunName { get; set; }

        /// <summary>
        /// 安全Z位置
        /// </summary>
        double JoupZ { get; set; }

        /// <summary>
        /// 速度
        /// </summary>
        double Seelp { get; set; }

        /// <summary>
        /// 轴数量
        /// </summary>
        SByte AxisNumber { get; }

        /// <summary>
        /// 回原点超时
        /// </summary>
        int HomeTime { get; set; }

        /// <summary>
        /// 运行超时时间
        /// </summary>
        int SetTime { get; set; }

        /// <summary>
        /// 停止中
        /// </summary>
        bool Stoping { get; }

        /// <summary>
        /// 暂停中
        /// </summary>
        bool Pauseing { get; }

        /// <summary>
        /// 忙碌中
        /// </summary>
        bool Busy { get; set; }

        /// <summary>
        /// 已同步原点
        /// </summary>
        bool IsHome { get; }

        /// <summary>
        /// 使能
        /// </summary>
        bool enabled { get; }

        /// <summary>
        /// 错误
        /// </summary>
        bool Aralming { get; }

        /// <summary>
        /// 错误代码
        /// </summary>
        int ErrCode { get; }

        /// <summary>
        /// 错误信息
        /// </summary>
        string ErrMeaessge { get; }

        /// <summary>
        /// 寸动模式
        /// </summary>
        bool JogMode { get; set; }

        sbyte Tool { get; set; }

        /// <summary>
        /// 暂停或继续
        /// </summary>
        /// <param name="pause">等于True时继续</param>
        void Pause(bool pause = false);

        /// <summary>
        /// 使能
        /// </summary>
        void Enabled();

        /// <summary>
        /// 复位
        /// </summary>
        void Reset();

        /// <summary>
        /// 回原点
        /// </summary>
        void SetHome();

        /// <summary>
        /// 停止
        /// </summary>
        void Stop();

        /// <summary>
        /// 抱闸
        /// </summary>
        /// <param name="isDeal">等于false是解除抱闸</param>
        void Dand_type_brake(bool isDeal = true);

        /// <summary>
        ///
        /// </summary>
        /// <param name="runCode">轨迹代码</param>
        /// <param name="fileName"></param>
        /// <param name="pointName"></param>
        void SetPoint(string runCode, string pointName);

        /// <summary>
        ///
        /// </summary>
        /// <param name="runCode">轨迹代码</param>
        /// <param name="fileName"></param>
        /// <param name="pointName"></param>
        void SetPoint(string runCode, string fileName, string pointName);

        /// <summary>
        ///
        /// </summary>
        /// <param name="runCode"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <param name="w"></param>
        void SetPoint(string runCode, double x, double y, double? z, double? u, double? v, double? w);

        /// <summary>
        ///
        /// </summary>
        /// <param name="runCode"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="u"></param>
        void SetPoint(string runCode, double x, double y, double? z, double? u);

        /// <summary>
        ///
        /// </summary>
        /// <param name="runCode"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        void SetPoint(string runCode, double x, double y, double? z);

        /// <summary>
        /// 根据代码移动位置
        /// </summary>
        /// <param name="runCode"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="u"></param>
        /// <returns>成功</returns>
        bool SetPoint(string runCode, double? x, double? y, double? z, double? u);

        /// <summary>
        /// 试教当前位置到点文件
        /// </summary>
        /// <param name="pFileName">点文件名</param>
        /// <param name="pName">点编号</param>
        void SetPointPFile(string pFileName, string pName);

        /// <summary>
        ///
        /// </summary>
        /// <param name="runCode"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void SetPoint(double x, double y);

        void SetPointX(double x);

        void SetPointY(double y);

        void SetPointZ(double z);

        void SetPointU(double? u);

        void SetPointV(double? v);

        void SetPointW(double? w);

        void SetSeep(double seelp, double? addSeelp = null, double? accSeelp = null);

        void GetPoints(out double x, out double y, out double z, out double u, out double v, out double w);

        void GetPoints(out double x, out double y, out double z, out double u);

        void GetPoints(out double x, out double y, out double z);

        void GetSt(out bool ishome, out bool enabled, out bool err, out string mees);

        void GetSt();

        /// <summary>
        /// 获得支持的运动轨迹路线
        /// </summary>
        /// <returns></returns>
        List<string> GetRunCode();

        /// <summary>
        /// 下载指定名的点集合
        /// </summary>
        /// <returns></returns>
        List<PointFile> GetFilePoints(string fileName = null);

        /// <summary>
        /// 上穿指定的点文件并保存
        /// </summary>
        /// <param name="fileName">点文件名</param>
        /// <returns>成功返回true</returns>
        bool LoandPointFile(string fileName);

        UInt16 MemOutW { get; set; }
        UInt16 MemInW1 { get; set; }
        UInt16 MemInW2 { get; set; }

        /// <summary>
        /// 获取单个轴
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IAxis GetAxis(sbyte id);

        /// <summary>
        /// 设置IO
        /// </summary>
        /// <param name="id">IOid号</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        bool SetIOOut(string id, string strvalue);

        /// <summary>
        /// 获取16位IO的值，
        /// </summary>
        /// <param name="id">id号字编号</param>
        /// <param name="isOut">ture为输出，默认false输入</param>
        /// <returns></returns>
        bool[] GetIOOuts(bool isOut = false);
    }

    /// <summary>
    /// 轨迹路径
    /// </summary>
    public interface IAxisGrubXYZU
    {
    }

    /// <summary>
    ///
    /// </summary>
    public class AxisGrubXY
    {
        public StringBuilder builder;

        public string Name { get; set; } = "轨迹1";

        /// <summary>
        /// 执行状态
        /// </summary>
        public string RunStr { get; private set; }

        /// <summary>
        /// 轨迹ID
        /// </summary>
        public UInt16 RunID { get; set; }

        /// <summary>
        /// 步ID
        /// </summary>
        public UInt16 RunCDID { get; private set; }

        /// <summary>
        /// 调试单步
        /// </summary>
        public bool DebugDool { get; private set; }

        /// <summary>
        ///
        /// </summary>
        public bool Stop { get; private set; }

        /// <summary>
        /// 执行中
        /// </summary>
        public bool Runing { get; private set; }

        /// <summary>
        /// 调试下一步
        /// </summary>
        public bool DebugNext { get; set; }

        public void Run()
        {
            if (axisG != null)
            {
                RunStr = "开始执行";
            }
        }

        /// <summary>
        /// 异步执行
        /// </summary>
        public void RunAsyn()
        {
            if (axisG != null)
            {
                Task.Run(() => { Run(); });
            }
        }

        /// <summary>
        /// 返回组合
        /// </summary>
        /// <returns></returns>
        public IAxisGrub GetAxisGrub()
        {
            return axisG;
        }

        /// <summary>
        ///
        /// </summary>
        public void STOP()
        {
            RunStr = "停止";
        }

        private IAxisGrub axisG;

        /// <summary>
        /// 关联组合
        /// </summary>
        /// <param name="axisGrub"></param>
        public void Set(IAxisGrub axisGrub)
        {
            axisG = axisGrub;
        }

        public class Points
        {
            public Points()
            {
            }

            public Points(object x, object y, object z, object u, object v, object w, string runcode)
            {
                X = Convert.ToSingle(x);
                Y = Convert.ToSingle(y);
                Z = Convert.ToSingle(z);
                U = Convert.ToSingle(u);
                V = Convert.ToSingle(v);
                W = Convert.ToSingle(w);
                RunCode = runcode;
            }

            public Points(object x, object y, object z, object u, string runcode)
            {
                X = Convert.ToSingle(x);
                Y = Convert.ToSingle(y);
                Z = Convert.ToSingle(z);
                U = Convert.ToSingle(u);
                RunCode = runcode;
            }

            public Single? X;
            public Single? Y;
            public Single? Z;
            public Single? U;
            public Single? V;
            public Single? W;
            public string RunCode;
        }

        public List<Points> ListPoints = new List<Points>();

        public System.Windows.Forms.DataGridView GetPoint(System.Windows.Forms.DataGridView dataGridView)
        {
            try
            {
                dataGridView.Rows.Clear();
                for (int i = 0; i < ListPoints.Count; i++)
                {
                    int d = dataGridView.Rows.Add();
                    dataGridView.Rows[d].Cells[0].Value = ListPoints[i].X;
                    dataGridView.Rows[d].Cells[1].Value = ListPoints[i].Y;
                    dataGridView.Rows[d].Cells[2].Value = ListPoints[i].U;
                    dataGridView.Rows[d].Cells[3].Value = ListPoints[i].Z;
                    dataGridView.Rows[d].Cells[4].Value = ListPoints[i].RunCode;
                }
                return dataGridView;
            }
            catch (Exception)
            {
            }
            return dataGridView;
        }

        /// <summary>
        /// 保存点文件到本地
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool SavePoint(string path = null)
        {
            if (path == null)
            {
                path = ProjectINI.ProjectPathRun + "\\轨迹文件\\";
            }
            if (Name == null)
            {
                Name = "轨迹";
            }
            string pathSd = path + "\\" + Name + "备份1.xls";
            string pathNAMEd = path + "\\" + Name + "写入.xls";
            path = path + "\\" + Name + ".xls";

            if (ListPoints != null)
            {
                List<string> list = new List<string>();
                list.Add("点编号");
                list.Add("X位置");
                list.Add("Y位置");
                list.Add("Z位置");
                list.Add("U位置");
                list.Add("V位置");
                list.Add("W位置");
                list.Add("轨迹代码");
                Npoi.AddWriteColumnToExcel(path, "轨迹文件", list.ToArray());
                for (int i = 0; i < ListPoints.Count; i++)
                {
                    list = new List<string>();
                    list.Add(i.ToString());
                    list.Add(Convert.ToString(ListPoints[i].X));
                    list.Add(Convert.ToString(ListPoints[i].Y));
                    list.Add(Convert.ToString(ListPoints[i].Z));
                    list.Add(Convert.ToString(ListPoints[i].U));
                    list.Add(Convert.ToString(ListPoints[i].V));
                    list.Add(Convert.ToString(ListPoints[i].W));
                    list.Add(Convert.ToString(ListPoints[i].RunCode));
                    Npoi.AddRosWriteToExcel(path, "轨迹文件", list.ToArray());
                }
                if (File.Exists(path))
                {
                    File.Replace(pathNAMEd, path, pathSd);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 读取点位文件
        /// </summary>
        /// <returns></returns>
        public bool ReadPoint(string path = null)
        {
            try
            {
                if (path == null)
                {
                    path = ProjectINI.ProjectPathRun + "\\轨迹文件\\";
                }
                //Directory.CreateDirectory(path);
                string[] lPaths = Directory.GetFiles(path);
                int errn = 0;
                for (int i = 0; i < lPaths.Length; i++)
                {
                    try
                    {
                        DataTable dataTable = Npoi.ReadExcelFile(lPaths[i], "轨迹文件");
                        List<Points> pointFiles = new List<Points>();
                        for (int i2 = 0; i2 < dataTable.Rows.Count; i2++)
                        {
                            Points pointFile = new Points();
                            try
                            {
                                pointFile.X = Convert.ToSingle(dataTable.Rows[i2].ItemArray[1]);
                                pointFile.Y = Convert.ToSingle(dataTable.Rows[i2].ItemArray[2]);
                                pointFile.Z = Convert.ToSingle(dataTable.Rows[i2].ItemArray[3]);
                                pointFile.U = Convert.ToSingle(dataTable.Rows[i2].ItemArray[4]);
                                pointFile.V = Convert.ToSingle(dataTable.Rows[i2].ItemArray[5]);
                                pointFile.W = Convert.ToSingle(dataTable.Rows[i2].ItemArray[6]);
                                pointFile.RunCode = dataTable.Rows[i2].ItemArray[7].ToString();
                            }
                            catch (Exception)
                            {
                            }
                            pointFiles.Add(pointFile);
                        }

                        ListPoints.Clear();
                        ListPoints = pointFiles;
                    }
                    catch (Exception ex)
                    {
                        errn++;
                        //System.Windows.MessageBox.Show("读取失败" + ex.Message);
                    }
                }
                if (errn == 0)
                {
                    return true;
                }
            }
            catch (Exception)
            {
            }

            return false;
        }

        public void SetPint(System.Windows.Forms.DataGridView dataGridView)
        {
            List<Points> list = new List<Points>();
            bool isNull = false;
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                isNull = false;
                for (int ite = 0; ite < dataGridView.Rows[i].Cells.Count; ite++)
                {
                    if (dataGridView.Rows[i].Cells[ite].Value == null || dataGridView.Rows[i].Cells[ite].Value.ToString() == "")
                    {
                        isNull = true;
                        break;
                    }
                }
                if (isNull)
                {
                    continue;
                }
                list.Add(new Points(dataGridView.Rows[i].Cells[0].Value,
                    dataGridView.Rows[i].Cells[1].Value,
                    dataGridView.Rows[i].Cells[2].Value,
                    dataGridView.Rows[i].Cells[3].Value,
                    dataGridView.Rows[i].Cells[4].Value.ToString()
                    ));
            }
            ListPoints = list;
        }
    }
}