using System;
using System.Collections.Generic;
using Vision2.Project.Mes;
using Vision2.vision.HalconRunFile.RunProgramFile;
namespace Vision2.Project.formula
{
    /// <summary>
    /// 附加数据
    /// </summary>
    public class LinkData
    {

        public string LinkName { get; set; } = "";

        public void SetData( List <DataMinMax> data)
        {
            ListDatV = data;
        }

        public void Set()
        {
            if (LinkName == null)
            {
                LinkName = "";
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        public void AddData(string text)
        {
            //DataStr.Add(text);
            string[] date = text.Split(',');
            for (int i = RecipeCompiler.Instance.DataLinkStrat; i < date.Length; i++)
            {
                if (double.TryParse(date[i], out double result))
                {
                    //ListDatV[i].ValueStrs=
                    //ListDatV.Add(result);
                }
            }
            EventAddValue?.Invoke(ListDatV);
        }
        public void AddData(List<double> datas)
        {
               AddObj(datas);
               EventAddValue?.Invoke(ListDatV);
        }
         public void AddObj( List<string> vaset  )
         {
            int idex = 0;
            int idex2 = 0;
            for (int i = 0; i < vaset.Count; i++)
            {
                if (ListDatV[idex2].Reference_Name.Count <= idex)
                {
                    idex2++;
                    idex = 0;
                }
                if (ListDatV[idex2].ValueStrs.Count <= idex)
                {
                    ListDatV[idex2].ValueStrs.Add(vaset[i]);
                }
                else
                {
                    ListDatV[idex2].ValueStrs[idex] = vaset[i];
                }
                double? d = null;
                if (double.TryParse(vaset[i], out double valued))
                {
                    d = valued;
                }
                if (ListDatV[idex2].doubleV.Count <= idex)
                {
                    ListDatV[idex2].doubleV.Add(d);
                }
                else
                {
                    ListDatV[idex2].doubleV[idex] = d;
                }
                idex++;
            }
         }
        public void AddObj(List<double> vaset)
        {
            int idex = 0;
            int idex2 = 0;
            for (int i = 0; i < vaset.Count; i++)
            {
                if (ListDatV[idex2].Reference_Name.Count <= idex)
                {
                    idex2++;
                    idex = 0;
                }
                if (ListDatV[idex2].ValueStrs.Count <= idex)
                {
                    ListDatV[idex2].ValueStrs.Add(vaset[i].ToString());
                }
                else
                {
                    ListDatV[idex2].ValueStrs[idex] = vaset[i].ToString();
                }
                if (ListDatV[idex2].doubleV.Count <= idex)
                {
                    ListDatV[idex2].doubleV.Add(vaset[i]);
                }
                else
                {
                    ListDatV[idex2].doubleV[idex] = vaset[i];
                }
                idex++;
            }
        }
        /// <summary>
        /// 添加数据到元件
        /// </summary>
        /// <param name="index">元件序号</param>
        /// <param name="datas">元件数据</param>
        public void AddData(int index,List<string> datas)
        {
            int idex = 0;
            int idex2 = index;
            for (int i = index; i < ListDatV.Count; i++)
            {
                if (ListDatV[i].ValueStrs.Count>= ListDatV[i].Reference_Name.Count)
                {
                    idex2++;
                }
                else
                {
                    break;
                }
            }
            if (idex2 >= ListDatV.Count)
            {
                idex2 = 0;
                for (int i = 0; i < ListDatV.Count; i++)
                {
                    ListDatV[i].Clear();
                }
            }
            for (int i = 0; i < datas.Count; i++)
            {
                if (ListDatV[idex2].Reference_Name.Count <= idex)
                {
                    //break;
                }
                if (ListDatV[idex2].ValueStrs.Count <= idex)
                {
                    ListDatV[idex2].ValueStrs.Add(datas[i]);
                }
                else
                {
                    ListDatV[idex2].ValueStrs[idex] = datas[i];
                }
                double? d = null;
                if (double.TryParse(datas[i], out double valued))
                {
                    d = valued;
                }
                if (ListDatV[idex2].doubleV.Count <= idex)
                {
                    ListDatV[idex2].doubleV.Add(d);
                }
                else
                {
                    ListDatV[idex2].doubleV[idex] = d;
                }
                idex++;
            }
            EventAddValue?.Invoke(ListDatV);
        }

        public void AddData(double datas)
        {
            AddObj(new List<double>() { datas });
            EventAddValue?.Invoke(ListDatV);
        }
        public void OnEnver()
        {
            try
            {
                EventAddValue?.Invoke(ListDatV);
            }
            catch (Exception) {}
        }

        public void Clear()
        {
            for (int i = 0; i < ListDatV.Count; i++)
            {
                ListDatV[i].Clear();
            }
          
                  EventAddValue?.Invoke(ListDatV);
        }
        public bool GetChet(List<float> xs, List<float> ys, List<float> zs)
        {
            bool valset = true;
            for (int i = 0; i < CheCalssT.Count; i++)
            {
                if (!CheCalssT[i].GetChet(zs, xs, ys))
                {
                    valset = false;
                }
            }
            return valset;
        }
        public bool GetChet()
        {
            bool valset = true;
            if (IsChe)
            {
            }
            else
            {
                for (int i = 0; i < ListDatV.Count; i++)
                {
                    if (!ListDatV[i].GetRsetOK())
                    {
                        return false;
                    }
                }
            }
            return valset;
        }
        public bool GetChet(int number)
        {
            try
            {
                return ListDatV[number].GetRsetOK();
            }
            catch (Exception) {  }
            return false;
        }

        public DataMinMax GetMaxMinValue(int number)
        {
            try
            {
                if (number >= 0)
                {
                    return ListDatV[number];
                }
            }
            catch (Exception ex){ }
            return null;
        }
        ///// <summary>
        ///// 
        ///// </summary>
        //public List<double> Values = new List<double>();
        /// <summary>
        /// 
        /// </summary>
        List<string> DataStr = new List<string>();
        public bool IsChe { get; set; }
        public List<CheCalss> CheCalssT = new List<CheCalss>();
        public class CheCalss
        {
            public int StrartNumber { get; set; }
            public int EndNumber { get; set; }
            public double MaxValue { get; set; }

            public double Value;

            public bool GetChet(List<float> zs, List<float> xs, List<float> ys)
            {
                int sTA = StrartNumber - 1;
                if (sTA < 0)
                {
                    sTA = 0;
                }
                float[] z = new float[EndNumber];
                float[] x = new float[EndNumber];
                float[] y = new float[EndNumber];

                Array.Copy(xs.ToArray(), sTA, x, 0, EndNumber);
                Array.Copy(ys.ToArray(), sTA, y, 0, EndNumber);
                Array.Copy(zs.ToArray(), sTA, z, 0, EndNumber);

                Value = DataHandxxxx.Flatness(DataHandxxxx.Plane(x, y, z, z.Length), x, y, z);
                Value = FlatnessResult(z, x, y, z.Length);
                if (Value > MaxValue)
                {
                    return false;
                }
                return true;
            }
            float fun(float[] m, Point3D A)
            {
                return m[0] + m[1] * A.x + m[2] * A.y - A.z;
            }
            public struct coordinate
            {
                public coordinate(int CONT)
                {
                    x = new float[CONT];
                    y = new float[CONT];
                    z = new float[CONT];
                }
                public float[] x;
                public float[] y;
                public float[] z;
            };
            public class Point3D
            {

                public float x;
                public float y;
                public float z;
            };
            public double FlatnessResult(float[] fOriginal, float[] fx, float[] fy, int iDataNumber)
            {
                coordinate Axis = new coordinate(fOriginal.Length);
                coordinate all_axis = new coordinate(fOriginal.Length);
                int point_num;
                int j;
                int[] flag = new int[256];
                int flag2;
                int high_flag;
                float point_x, point_y, point_z;
                float[] higher = new float[256];
                double top_higher;
                float d, divisor, d1, divisor2;
                float[] a1 = new float[3] { 0.0F, 0.0F, 0.0F };
                float[] a2 = new float[3] { 0.0F, 0.0F, 0.0F };
                float[] a3 = new float[3] { 0.0F, 0.0F, 0.0F };
                float[] a4 = new float[3] { 0.0F, 0.0F, 0.0F };
                float m, m1, m2, m3;
                float A, B, C;
                for (int i = 0; i < iDataNumber; i++)
                {
                    Axis.x[i] = fx[i];
                    Axis.y[i] = fy[i];
                    Axis.z[i] = fOriginal[i];
                }
                all_axis = Axis;
                point_num = iDataNumber;
                if (point_num < 4 || point_num > 256)
                {
                    return -2;
                }

                for (int i = 0; i < point_num; i++)
                {
                    a1[0] = a1[0] + all_axis.x[i] * all_axis.x[i];
                    a1[1] = a1[1] + all_axis.x[i] * all_axis.y[i];
                    a1[2] = a1[2] + all_axis.x[i];
                    a2[0] = a2[0] + all_axis.x[i] * all_axis.y[i];
                    a2[1] = a2[1] + all_axis.y[i] * all_axis.y[i];
                    a2[2] = a2[2] + all_axis.y[i];
                    a3[0] = a3[0] + all_axis.x[i];
                    a3[1] = a3[1] + all_axis.y[i];
                    a3[2] = point_num;
                    a4[0] = a4[0] + all_axis.x[i] * all_axis.z[i];
                    a4[1] = a4[1] + all_axis.y[i] * all_axis.z[i];
                    a4[2] = a4[2] + all_axis.z[i];
                }

                m = a1[0] * a2[1] * a3[2] + a2[0] * a3[1] * a1[2] + a3[0] * a1[1] * a2[2] - a2[0] * a1[1] * a3[2] - a1[0] * a3[1] * a2[2] - a3[0] * a2[1] * a1[2];
                m1 = a4[0] * a2[1] * a3[2] + a2[0] * a3[1] * a4[2] + a3[0] * a4[1] * a2[2] - a2[0] * a4[1] * a3[2] - a4[0] * a3[1] * a2[2] - a3[0] * a2[1] * a4[2];
                m2 = a1[0] * a4[1] * a3[2] + a4[0] * a3[1] * a1[2] + a3[0] * a1[1] * a4[2] - a4[0] * a1[1] * a3[2] - a1[0] * a3[1] * a4[2] - a3[0] * a4[1] * a1[2];
                m3 = a1[0] * a2[1] * a4[2] + a2[0] * a4[1] * a1[2] + a4[0] * a1[1] * a2[2] - a2[0] * a1[1] * a4[2] - a1[0] * a4[1] * a2[2] - a4[0] * a2[1] * a1[2];
                A = m1 / m;
                B = m2 / m;
                C = m3 / m;
                divisor2 = (float)(A * A + B * B + 1);
                divisor = divisor2;

                for (int i = 0; i < point_num; i++)
                {
                    higher[i] = Math.Abs(all_axis.z[i] - A * (all_axis.x[i]) - B * (all_axis.y[i]) - C) / divisor;
                    flag[i] = i;
                }

                top_higher = higher[0];
                flag2 = flag[0];

                for (int i = 0; i < point_num; i++)
                {
                    if (top_higher < higher[i])
                    {
                        top_higher = higher[i];
                        flag2 = flag[i];
                    }
                }

                point_x = all_axis.x[flag2];
                point_y = all_axis.y[flag2];
                point_z = all_axis.z[flag2];
                C = point_z - A * point_x - B * point_y;
                HalconDotNet.HTuple valuse = new HalconDotNet.HTuple();
                for (int i = 0; i < point_num; i++)
                {
                    higher[i] = Math.Abs(all_axis.z[i] - A * (all_axis.x[i]) - B * (all_axis.y[i]) - C) / divisor;
                    valuse.Append(higher[i]);
                }


                //top_higher = higher[0];
                top_higher = (valuse.TupleMax());
                //for ( int   i = 0; i < point_num; i++)
                //{
                //    if (top_higher < higher[i])
                //    {
                //        top_higher = higher[i];
                //    }
                //}

                return top_higher;
            }
            public bool GetChet()
            {
                if (Value > MaxValue)
                {
                    return false;
                }
                return true;
            }
            void Least_squares(Point3D[] v_Point, out float[] M)
            {
                List<float[]> c = new List<float[]>();
                M = new float[3];


                for (int i = 0; i < 3; i++)
                {
                    c.Add(new float[4]);
                }
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        c[i][j] = 0;
                    }
                }
                c[0][0] = v_Point.Length;
                for (int i = 0; i < v_Point.Length; i++)
                {
                    c[0][1] = c[0][1] + v_Point[i].x;
                    c[0][2] = c[0][2] + v_Point[i].y;
                    c[0][3] = c[0][3] + v_Point[i].z;
                    c[1][1] = c[1][1] + v_Point[i].x * v_Point[i].x;
                    c[1][2] = c[1][2] + v_Point[i].x * v_Point[i].y;
                    c[1][3] = c[1][3] + v_Point[i].x * v_Point[i].z;
                    c[2][2] = c[2][2] + v_Point[i].y * v_Point[i].y;
                    c[2][3] = c[2][3] + v_Point[i].y * v_Point[i].z;
                }
                c[1][0] = c[0][1];
                c[2][0] = c[0][2];
                c[2][1] = c[1][2];

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        Vision2.ErosProjcetDLL.Project.AlarmText.AddTextNewLine(c[i][j].ToString());
                    }
                }
                column_principle_gauss(3, c);

                for (int i = 0; i < 3; i++)
                {
                    M[i] = c[i][3];
                }

                //for (int i = 0; i < 3; i++)
                //{
                //    delete[] c[i];
                //    c[i] = NULL;
                //}
                //delete[] c;
                //c = NULL;
            }
            //列主元高斯消去法求逆矩阵
            //void column_principle_gauss(float a[][N+1])
            void column_principle_gauss(int N, List<float[]> a)
            {
                int k = 0, i = 0, r = 0, j = 0;
                float t;
                for (k = 0; k < N - 1; k++)
                {
                    for (i = k; i < N; i++)
                    {
                        r = i;
                        t = (float)Math.Abs(a[r][k]);
                        if (Math.Abs(a[i][k]) > t)
                        {
                            r = i;
                        }
                    }
                    if (a[r][k] == 0)
                    {
                        break;
                    }
                    for (j = k; j < N + 1; j++)
                    {
                        t = a[r][j];
                        a[r][j] = a[k][j];
                        a[k][j] = t;
                    }
                    for (i = k + 1; i < N; i++)
                    {
                        for (j = k + 1; j < N + 1; j++)
                        {
                            a[i][j] = a[i][j] - a[i][k] / a[k][k] * a[k][j];
                        }
                    }
                }

                float he = 0;
                for (k = N - 1; k >= 0; k--)
                {
                    he = 0;
                    for (j = k + 1; j < N; j++)
                    {
                        he = he + a[k][j] * a[j][N];
                    }
                    a[k][N] = (a[k][N] - he) / a[k][k];
                }
            }
            public static class DataHandxxxx
            {

                /// <summary>
                /// 求拟合平面
                /// </summary>
                /// <param name="x">x坐标</param>
                /// <param name="y">y坐标</param>
                /// <param name="z">z坐标</param>
                /// <param name="docCount">计算点数</param>
                /// <return name="a">x系数、y系数、常数</return>
                public static float[] Plane(float[] x, float[] y, float[] z, int docCount)
                {
                    float[] a = new float[3] { 0, 0, 0 };
                    if (docCount < 3) return a;
                    float xi = 0; float yi = 0; float zi = 0;
                    float xi_xi = 0; float yi_yi = 0; float zi_zi = 0;
                    float xi_yi = 0; float xi_zi = 0; float yi_zi = 0;
                    #region  矩阵方程对角线求法            
                    for (int i = 0; i < docCount; i++)
                    {
                        xi = xi + x[i];
                        yi = yi + y[i];
                        zi = zi + z[i];
                        xi_xi = xi_xi + x[i] * x[i];
                        yi_yi = yi_yi + y[i] * y[i];
                        zi_zi = zi_zi + z[i] * z[i];
                        xi_yi = xi_yi + x[i] * y[i];
                        xi_zi = xi_zi + x[i] * z[i];
                        yi_zi = yi_zi + y[i] * z[i];
                    }
                    float D = (xi_xi * yi_yi * docCount + xi_yi * yi * xi + xi_yi * yi * xi) - (xi * yi_yi * xi + xi_yi * xi_yi * docCount + yi * yi * xi_xi);
                    float Dx = (xi_zi * yi_yi * docCount + xi_yi * yi * zi + yi_zi * yi * xi) - (xi * yi_yi * zi + xi_yi * yi_zi * docCount + yi * yi * xi_zi);
                    float Dy = (xi_xi * yi_zi * docCount + xi_zi * yi * xi + xi_yi * zi * xi) - (xi * yi_zi * xi + xi_zi * xi_yi * docCount + yi * zi * xi_xi);
                    float Dc = (xi_xi * yi_yi * zi + xi_yi * yi_zi * xi + xi_yi * yi * xi_zi) - (xi_zi * yi_yi * xi + xi_yi * xi_yi * zi + yi_zi * yi * xi_xi);
                    a[0] = Dx / D;    //x坐标系数
                    a[1] = Dy / D;    //y坐标系数
                    a[2] = Dc / D;    //常数
                    #endregion
                    #region 逆矩阵求法
                    //float[][] nijuzheng = new float[3][];
                    //nijuzheng[0] = new float[] { 0, 0, 0 };
                    //nijuzheng[1] = new float[] { 0, 0, 0 };
                    //nijuzheng[2] = new float[] { 0, 0, 0 };
                    //double[] juzheng2 = new double[] { xi_zi, yi_zi, zi };
                    //if (D != 0)
                    //{
                    //    nijuzheng[0][0] = (1) * (yi_yi * docCount - yi * yi) / D;
                    //    nijuzheng[0][1] = (-1) * (xi_yi * docCount - yi * xi) / D;
                    //    nijuzheng[0][2] = (1) * (xi_yi * yi - yi_yi * xi) / D;
                    //    nijuzheng[1][0] = (-1) * (xi_yi * docCount - xi * yi) / D;
                    //    nijuzheng[1][1] = (1) * (xi_xi * docCount - xi * xi) / D;
                    //    nijuzheng[1][2] = (-1) * (xi_xi * yi - xi_yi * xi) / D;
                    //    nijuzheng[2][0] = (1) * (xi_yi * yi - xi * yi_yi) / D;
                    //    nijuzheng[2][1] = (-1) * (xi_xi * yi - xi * xi_yi) / D;
                    //    nijuzheng[2][2] = (1) * (xi_xi * yi_yi - xi_yi * xi_yi) / D;

                    //    for (int i = 0; i < 3; i++)
                    //    {
                    //        for (int j = 0; j < 3; j++)
                    //        {
                    //            if (i == 0)
                    //            {
                    //                a[0] += (nijuzheng[i][j] * juzheng2[j]);
                    //            }
                    //            else if (i == 1)
                    //            {
                    //                a[1] += (nijuzheng[i][j] * juzheng2[j]);
                    //            }
                    //            else if (i == 2)
                    //            {
                    //                a[2] += (nijuzheng[i][j] * juzheng2[j]);
                    //            }
                    //        }
                    //    }
                    //}
                    #endregion

                    return a;
                }

                /// <summary>
                /// 平面度--平面一侧各取实测点z数值到拟合平面z的理论数值的最大距离
                /// </summary>
                /// <param name="coefficient">拟合平面系数</param>
                /// <param name="x">被测点x数组</param>
                /// <param name="y">被测点y数组</param>
                /// <param name="z">被测点z数组</param>
                /// <returns>平面度</returns>
                public static float Flatness(float[] coefficient, float[] x, float[] y, float[] z)
                {
                    float[] theoryZ = new float[z.Length];
                    List<float> DnToMaxArray = new List<float>();
                    for (int i = 0; i < z.Length; i++)
                    {
                        theoryZ[i] = (coefficient[0] * x[i]) + (coefficient[1] * y[i]) - z[i] + coefficient[2];   //指定坐标在拟合平面上的理论z方向数值                          
                        DnToMaxArray.Add(Math.Abs(theoryZ[i]) / Convert.ToSingle(Math.Sqrt((coefficient[0] * coefficient[0]) + (coefficient[1] * coefficient[1]) + 1)));   //点面距离                        
                    }
                    DnToMaxArray.Sort();
                    return DnToMaxArray[DnToMaxArray.Count - 1] - DnToMaxArray[0];
                }
                /// <summary>
                /// 厚度--z坐标相加
                /// </summary>
                /// <param name="z1"></param>
                /// <param name="z2"></param>
                /// <returns></returns>
                public static float Thickness(float z1, float z2)
                {
                    return z1 + z2;
                }
            }
        }
        public event ChangeValue EventAddValue;
        /// <summary>
        /// 值改变委托
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public delegate void ChangeValue(List<DataMinMax> text);
        /// <summary>
        /// 
        /// </summary>
        public List<DataMinMax> ListDatV = new List<DataMinMax>();
        //public int Number { get; set; }
        //public List<double> Reference_ValueMin { get; set; } = new List<double>();
        //public List<double> Reference_ValueMax { get; set; } = new List<double>();
        ///// <summary>
        ///// 点位名称
        ///// </summary>
        //public List<string> Reference_Name { get; set; } = new List<string>();
        //public List<string> PLCID { get; set; } = new List<string>();

        //public List<string> PointXID { get; set; } = new List<string>();
        //public List<string> PointYID { get; set; } = new List<string>();
    }
    public class DataMinMax
    {
        public List<string> GetStringData()
        {
            List<string> vs = new List<string>();
            vs.Add(ComponentName + ":"+RunNameOBJ+":"+Reference_Name.Count+";");
            for (int i2 = 0; i2 < Reference_Name.Count; i2++)
            {
                vs.Add( (i2+1) +":"+ Reference_Name[i2] + ":" +Reference_ValueMin[i2] + ">" + ValueStrs + "<" + Reference_ValueMax[i2]);
            }
            return vs;
        }


        public  OneRObj GetOneRObj()
        {
            OneRObj oneRObj = new OneRObj()
            {
                NGText = RunNameOBJ,
                ComponentID = ComponentName,
                RestText = ComponentName,
                RestStrings = Reference_Name,
            };
            if (RunNameOBJ != null && RunNameOBJ.Contains("."))
            {
                string[] vs =RunNameOBJ.Split('.');
                if (vs.Length == 2)
                {
                    oneRObj.NGROI = RecipeCompiler.GetProductEX().Key_Navigation_Picture[vs[0]].KeyRoi[vs[1]].Clone();
                    oneRObj.ROI = RecipeCompiler.GetProductEX().Key_Navigation_Picture[vs[0]].KeyRoi[vs[1]].Clone();
                }
            }
            oneRObj.OK = GetRsetOK();
            oneRObj.dataMinMax = this;
            return oneRObj;
        }
        /// <summary>
        /// 轨迹区域名称
        /// </summary>
        public string RunNameOBJ { get; set; } = "";
        /// <summary>
        /// 元件名称
        /// </summary>
        public string ComponentName { get; set; } = "";

        /// <summary>
        /// 最小标准值
        /// </summary>
        public List<double> Reference_ValueMin { get; set; } = new List<double>();
        public List<double> Reference_ValueMax { get; set; } = new List<double>();
        /// <summary>
        /// 数据点名称
        /// </summary>
        public List<string> Reference_Name { get; set; } = new List<string>();

        public List<string> ValueStrs
        {
            get { return valueStrs; }
            set {

                if (doubleV.Count < valueStrs.Count)
                {
                    doubleV.Add(valueStrs.Count - doubleV.Count);
                }
                else if (doubleV.Count > valueStrs.Count)
                {
                    doubleV.RemoveRange(valueStrs.Count-1, doubleV.Count- valueStrs.Count);
                }
                for (int i = 0; i < ValueStrs.Count; i++)
                {
                  
                    if (double.TryParse(ValueStrs[i],out double vael))
                    {
                        doubleV[i] = vael;
                    }
                    else
                    {
                        doubleV[i] = null;
                    }
                }
            }
        }

        List<string> valueStrs=new List<string>();
        public List< double?> doubleV { get; set; } = new List<double?>();
        public bool GetRsetOK()
        {
            if (Done)
            {
                return OK;
            }
            for (int i = 0; i < Reference_ValueMin.Count; i++)
            {
                if (GetRsetNumber(i)!=0)
                {
                    return false;
                }
            }
            return true;
        }
        public void SetResetOK()
        {
            Done = true;
            OK = true;
        }
        public void SetResetNG()
        {
            Done = true;
            OK = false;
        }

        public bool OK { get; set; }

        public bool Done { get; set; }

        public int  GetRset()
        {
            if (Reference_ValueMin.Count>ValueStrs.Count)
            {
                return 0;
            }
            for (int i = 0; i < Reference_ValueMin.Count; i++)
            {
                if (GetRsetNumber(i) != 0)
                {
                    return -2;
                }
            }
            return 1;
        }
        /// <summary>
        /// 获取标准值
        /// </summary>
        /// <param name="indxe"></param>
        /// <returns>返回0=OK，-1=空值,返回-2小于下限，返回-3大于上限</returns>
        public int  GetRsetNumber(int indxe)
        {
            if (indxe >= doubleV.Count)
            {
                return -1;
            }
            if (doubleV[indxe]==null)
            {
                return -4;
            }
            if (Reference_ValueMin[indxe] >  doubleV[indxe] )
            {
                return -2;
            }
            else if (Reference_ValueMax[indxe] < doubleV[indxe])
            {
                return -3;
            }
            return 0;
        }
        public void AddData(string name,double value,double datamin,double datamxa)
        {
            Reference_Name.Add(name);
            Reference_ValueMin.Add(datamin);
            Reference_ValueMax.Add(datamxa);
            ValueStrs.Add(value.ToString());
            doubleV.Add(value);
        }
        public void Clear()
        {
            ValueStrs.Clear();
            doubleV.Clear();
        }
    }
}
