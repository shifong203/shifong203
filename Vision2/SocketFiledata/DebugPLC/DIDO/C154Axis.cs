using System;
using System.Collections.Generic;

namespace ErosSocket.DebugPLC.DIDO
{
    public class C154Axis : IAxis
    {
        public double Scale { get; set; }
        public static Dictionary<string, C154Axis> KeyValuePairs = new Dictionary<string, C154Axis>();
        public int HomeTime { get; set; }

        public C154Axis()
        {
            int d = 0;
            Name = "轴0";
        str:
            if (!KeyValuePairs.ContainsKey(Name))
            {
                KeyValuePairs.Add(Name, this);
            }
            else
            {
                AxisNo = (short)d;
                Name = "轴" + d.ToString();
                d++;
                goto str;
            }
        }

        public double Point { get; set; }
        public double Jog_Distance { get; set; }
        public Single HomeMaxVel { get; set; } = 1500;

        public Single ORGOffset { get; set; }
        public Single HomeStrVal { get; set; } = 1000;
        public Single HomeTacc { get; set; } = 1;
        public Single Ratio { get; set; } = 1;
        public string Name { get; set; } = "";

        public short AxisNo { get; set; }
        public int PlusLimit { get; set; } = 0;
        public int MinusLimit { get; set; } = 1000;

        public bool Initial()
        {
            try
            {
                ReturnCode = MP_C154.c154_set_home_config(AxisNo, HomeMode, 0, 0, 0, 0);//回零方式设定归零运动模式、ORG逻辑、EZ逻辑、EZ计数次数以及ERC输出选项；详细请参阅 4.2.10。
                ReturnCode = MP_C154.c154_set_pls_outmode(AxisNo, 4); //Output type is CW/CCW  输出类型为CW/CCW
                ReturnCode = MP_C154.c154_set_pls_iptmode(AxisNo, 2, 0); //Input Type is 4*AB Phase 输入类型为4*AB相位
                ReturnCode = MP_C154.c154_set_feedback_src(AxisNo, 1); //Feedback source is Internal 反馈源是内部的
                ReturnCode = MP_C154.c154_set_alm(AxisNo, 1, 0); //Feedback source is Internal 反馈源是内部的
                ReturnCode = MP_C154.c154_set_inp(AxisNo, 1, 1);//设定 INP 逻辑与操作模式
                                                                // MP_C154.c154_set_home_config(AxisNo, 4, 0, 0, 0, 0);
                ReturnCode = MP_C154.c154_set_servo(AxisNo, 1);//伺服上电
                ReturnCode = MP_C154.c154_set_move_ratio(AxisNo, Ratio);
                ReturnCode = MP_C154.c154_enable_soft_limit(AxisNo, 1);
                ReturnCode = MP_C154.c154_set_soft_limit(AxisNo, MinusLimit, PlusLimit);

                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        private static short returnCode;

        public static short ReturnCode
        {
            get => returnCode;
            set
            {
                C154Axis.returnCode = value;
                C154_AxisGrub.ErrMesage(value, "");
            }
        }

        public double StrVel { get; set; } = 100;
        public double MaxVel { get; set; } = 200;
        public double Tacc { get; set; } = 10;
        public double Tdec { get; set; } = 10;

        public string Dand_type_brakeID { get; set; } = "none";

        public EnumAxisType AxisType { get; set; }
        public bool IsHome { get; set; }

        public sbyte HomeMode { get; set; } = 1;
        public bool Alarm { get; set; }
        public bool IsEnabled { get; set; }
        public sbyte IsBand_type_brakeNumber { get; set; } = -1;

        public void AddSeelp(double dacc, double strVal, double maxVal, double tacc)
        {
            Tdec = Tacc;
            Tacc = tacc;
            StrVel = strVal;
            MaxVel = maxVal;
        }

        public void Dand_type_brake(bool isDeal)
        {
            try
            {
                if (Dand_type_brakeID.Contains("."))
                {
                    string[] dataStr = Dand_type_brakeID.Split('.');
                    if (short.TryParse(dataStr[0], out short resultID) && short.TryParse(dataStr[1], out short resultCid))
                    {
                        if (isDeal)
                        {
                            ReturnCode = MP_C154.c154_set_gpio_output_ex_CH(resultID, resultCid, 1);   //伺服刹车
                        }
                        else
                        {
                            ReturnCode = MP_C154.c154_set_gpio_output_ex_CH(resultID, resultCid, 0);   //伺服刹车
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        public void Enabled()
        {
            ReturnCode = MP_C154.c154_set_servo(AxisNo, 1);//伺服上电
        }

        public void JogAdd(bool JogPsion, bool jogmode = true, double seepJog = 1)
        {
            try
            {
                MP_C154.c154_tv_move(1, 1000, 6400, 0.1);
                if (JogPsion)
                {
                    ReturnCode = MP_C154.c154_start_tr_move(AxisNo, seepJog * 200, StrVel, 200, 0.1, 0.1); ;//T形
                }
                else
                {
                    ReturnCode = MP_C154.c154_start_tr_move(AxisNo, -seepJog * 200, StrVel, 200, 0.1, 0.1); ;//T形
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Reset()
        {
            try
            {
                short error = 0;
                ReturnCode = MP_C154.c154_get_error_counter(AxisNo, ref error);
                //C154_W32.c154_set_alm(AxisNo, 0, 1);
                ReturnCode = MP_C154.c154_reset_error_counter(AxisNo);
            }
            catch (Exception)
            {
            }
        }

        public void SetHome()
        {
            try
            {
                ReturnCode = MP_C154.c154_home_search(AxisNo, HomeStrVal, HomeMaxVel, HomeTacc, ORGOffset);
                //MP_C154.c154_home_search(2, 1000, -1500, 0.5, 2);
            }
            catch (Exception)
            {
            }
        }

        public bool SetPoint(double? p, double? sleep = null)
        {
            try
            {
                ReturnCode = MP_C154.c154_start_ta_move(AxisNo, p.Value, StrVel, MaxVel, Tacc, Tdec);
                if (ReturnCode == 0)
                {
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        public bool Stop()
        {
            try
            {
                ReturnCode = MP_C154.c154_sd_stop(AxisNo, 100);
            }
            catch (Exception)
            {
            }
            return true;
        }

        public bool GetStatus(out bool enbeled, out bool is_home, out bool error, out bool band_type_brake)
        {
            enbeled = false;
            is_home = false;
            error = false;
            band_type_brake = false;
            short errorID = 0;
            try
            {
                ushort det = 0;
                ReturnCode = MP_C154.c154_get_io_status(AxisNo, ref det);

                ReturnCode = MP_C154.c154_get_error_counter(AxisNo, ref errorID);
                if (errorID != 0)
                {
                    error = true;
                }
                int cmd = 0;
                ReturnCode = MP_C154.c154_get_command(AxisNo, ref cmd);
                double point = 0;
                ReturnCode = MP_C154.c154_get_position(AxisNo, ref point);
                Point = (float)point;

                double pos = 0;
                ReturnCode = MP_C154.c154_get_latch_data(AxisNo, 0, ref pos);

                double seelp = 0;
                ReturnCode = MP_C154.c154_get_current_speed(AxisNo, ref seelp);
                MaxVel = seelp;
                return true;
            }
            catch (Exception)
            {
            }
            return false;
        }

        public bool GetStatus()
        {
            return GetStatus(out bool enabled, out bool ishome, out bool iserror, out bool isband);
        }

        public void AddSeelp()
        {
            throw new NotImplementedException();
        }
    }
}