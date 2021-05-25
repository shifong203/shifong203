using ErosSocket.DebugPLC;
using ErosSocket.DebugPLC.DIDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NokidaE.Project.DebugF.IO
{
    public class C154 : IDIDO, IAxisGrub
    {
        public bool[] Di
        {
            get
            {
                return di;
            }
            private set
            {
                di = value;
            }
        }
        bool[] di = new bool[16];
        public bool[] DO
        {
            get { return dob; }
            private set { dob = value; }
        }

        bool[] dob = new bool[16];

        public string[] DO_Name { get; set; } = new string[16];
        public string[] DI_Name { get; set; } = new string[16];


        public bool IsInitialBool { get;private set; }
        public sbyte Is_braking { get ; set ; }
        public List<XYZPoint> XyzPoints { get; set; } = new List<XYZPoint>();



        /// <summary>
        /// 轴集合
        /// </summary>
        public List<Axis> AxisS { get; set; } = new List<Axis>();
        public Dictionary<string, List<string>> AxisGrot { get; set; } = new Dictionary<string, List<string>>();

        /// <summary>
        /// 轴组
        /// </summary>
        Dictionary<string, List<Axis>> axisGrot;

        public bool ReadDO(int number)
        {
            return false;
        }

        public bool ReadDI(int number)
        {
            return false;
        }

        public bool WritDO(int intex, int inde, bool value)
        {
            if (value)
            {
                C154_AxisGrub.ReturnCode = MP_C154.c154_set_gpio_output_ex_CH((sbyte)intex, (short)inde, 1);
            }
            else
            {
                C154_AxisGrub.ReturnCode = MP_C154.c154_set_gpio_output_ex_CH((sbyte)intex, (short)inde, 0);
            }
            return false;
        }

        public bool WritDO(int intex, bool value)
        {
            return WritDO(0, intex, value);
        }

        public List<Axis> GetAxisGrotName(string name)
        {
            try
            {
                if (axisGrot == null)
                {
                    return null;
                }
                List<Axis> list = new List<Axis>();
                if (AxisGrot.ContainsKey(name))
                {
                    foreach (var item in AxisGrot[name])
                    {
                        for (int i = 0; i < AxisS.Count; i++)
                        {
                            if (item == AxisS[i].Name)
                            {
                                list.Add(AxisS[i]);
                                break;
                            }
                        }
                    }
                    return list;
                    //return axisGrot[name];
                }
            }
            catch (Exception)
            {


            }

            return null;
        }

        public Axis GetAxisGrotNameEx(string name, EnumAxisType enumAxisType)
        {
            try
            {
                if (AxisGrot.ContainsKey(name))
                {
                    for (int i = 0; i < AxisGrot[name].Count; i++)
                    {
                        for (int i2 = 0; i2 < AxisS.Count; i2++)
                        {
                            if (AxisGrot[name][i] == AxisS[i2].Name && AxisS[i2].AxisType == enumAxisType)
                            {
                                return AxisS[i2];
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return null;
        }

        public bool SetXYZ1Points(string groupName, int outTime = 0, float? xp = null, float? yp = null, float? zp = null, bool isMove = false, float? jumpZ = 0)
        {
            throw new NotImplementedException();
        }

        public void GetAxisGroupPoints(string groupName, out double? xp, out double? yp, out double? zp)
        {
            throw new NotImplementedException();
        }
    }
}
