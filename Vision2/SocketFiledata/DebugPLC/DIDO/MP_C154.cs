using System;
using System.Runtime.InteropServices;

namespace ErosSocket.DebugPLC.DIDO
{
    public class MP_C154
    {
        // System Section 6.3
        /// <summary>
        /// 卡片初始化
        /// </summary>
        /// <param name="CardID_InBit">显示系统上的卡片编码，此数值以位方式解读，位  n 用来表示编码为 n 的卡片是否存在，例如数值若是 0x12，则系统 上卡片的编码(card_id) 分别为 1、 4。</param>
        /// <param name="Manual_ID">卡编号</param>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_initial(ref System.UInt16 CardID_InBit, System.Int16 Manual_ID);

        /// <summary>
        /// 释放卡片
        /// </summary>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_close();

        /// <summary>
        ///
        /// </summary>
        /// <param name="CardId"></param>
        /// <param name="firmware_ver"></param>
        /// <param name="driver_ver"></param>
        /// <param name="dll_ver"></param>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_get_version(System.Int16 CardId, ref System.Int16 firmware_ver, ref System.Int32 driver_ver, ref System.Int32 dll_ver);

        /// <summary>
        ///
        /// </summary>
        /// <param name="CardId"></param>
        /// <param name="old_secu_code"></param>
        /// <param name="new_secu_code"></param>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_set_security_key(System.Int16 CardId, System.Int16 old_secu_code, System.Int16 new_secu_code);

        /// <summary>
        ///
        /// </summary>
        /// <param name="CardId"></param>
        /// <param name="secu_code"></param>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_check_security_key(System.Int16 CardId, System.Int16 secu_code);

        /// <summary>
        ///
        /// </summary>
        /// <param name="CardId"></param>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_reset_security_key(System.Int16 CardId);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_config_from_file();

        //Pulse Input/Output Configuration Section 6.4
        /// <summary>
        ///
        /// </summary>
        /// <param name="AxisNo"></param>
        /// <param name="pls_outmode"></param>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_set_pls_outmode(System.Int16 AxisNo, System.Int16 pls_outmode);

        /// <summary>
        ///
        /// </summary>
        /// <param name="AxisNo"></param>
        /// <param name="pls_iptmode"></param>
        /// <param name="pls_logic"></param>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_set_pls_iptmode(System.Int16 AxisNo, System.Int16 pls_iptmode, System.Int16 pls_logic);

        /// <summary>
        ///
        /// </summary>
        /// <param name="AxisNo"></param>
        /// <param name="Src"></param>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_set_feedback_src(System.Int16 AxisNo, System.Int16 Src);

        /// <summary>
        ///
        /// </summary>
        /// <param name="AxisNo"></param>
        /// <param name="StrVel"></param>
        /// <param name="MaxVel"></param>
        /// <param name="Tacc"></param>
        /// <returns></returns>

        //Velocity mode motion Section 6.5
        [DllImport("C154.dll")] public static extern Int16 c154_tv_move(System.Int16 AxisNo, System.Double StrVel, System.Double MaxVel, System.Double Tacc);

        /// <summary>
        ///
        /// </summary>
        /// <param name="AxisNo"></param>
        /// <param name="StrVel"></param>
        /// <param name="MaxVel"></param>
        /// <param name="Tacc"></param>
        /// <param name="SVacc"></param>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_sv_move(System.Int16 AxisNo, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double SVacc);

        /// <summary>
        /// 停止
        /// </summary>
        /// <param name="AxisNo"></param>
        /// <param name="Tdec">停止速度</param>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_sd_stop(System.Int16 AxisNo, System.Double Tdec);

        /// <summary>
        ///
        /// </summary>
        /// <param name="AxisNo"></param>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_emg_stop(System.Int16 AxisNo);

        /// <summary>
        ///
        /// </summary>
        /// <param name="AxisNo"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_get_current_speed(System.Int16 AxisNo, ref System.Double speed);

        /// <summary>
        ///
        /// </summary>
        /// <param name="CAxisNo"></param>
        /// <param name="NewVelPercent"></param>
        /// <param name="Time"></param>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_speed_override(System.Int16 CAxisNo, System.Double NewVelPercent, System.Double Time);

        /// <summary>
        ///
        /// </summary>
        /// <param name="AxisNo"></param>
        /// <param name="OvrdSpeed"></param>
        /// <param name="Enable"></param>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_set_max_override_speed(System.Int16 AxisNo, System.Double OvrdSpeed, System.Int16 Enable);

        /// <summary>
        ///
        /// </summary>
        /// <param name="AxisNo"></param>
        /// <param name="Dist"></param>
        /// <param name="StrVel"></param>
        /// <param name="MaxVel"></param>
        /// <param name="Tacc"></param>
        /// <param name="Tdec"></param>
        /// <returns></returns>
        //Single Axis Position Mode Section 6.6
        [DllImport("C154.dll")] public static extern Int16 c154_start_tr_move(System.Int16 AxisNo, System.Double Dist, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec);

        /// <summary>
        ///
        /// </summary>
        /// <param name="AxisNo"></param>
        /// <param name="Pos"></param>
        /// <param name="StrVel"></param>
        /// <param name="MaxVel"></param>
        /// <param name="Tacc"></param>
        /// <param name="Tdec"></param>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_start_ta_move(System.Int16 AxisNo, System.Double Pos, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec);

        /// <summary>
        ///
        /// </summary>
        /// <param name="AxisNo"></param>
        /// <param name="Dist"></param>
        /// <param name="StrVel"></param>
        /// <param name="MaxVel"></param>
        /// <param name="Tacc"></param>
        /// <param name="Tdec"></param>
        /// <param name="SVacc"></param>
        /// <param name="SVdec"></param>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_start_sr_move(System.Int16 AxisNo, System.Double Dist, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec, System.Double SVacc, System.Double SVdec);

        /// <summary>
        ///
        /// </summary>
        /// <param name="AxisNo"></param>
        /// <param name="Pos"></param>
        /// <param name="StrVel"></param>
        /// <param name="MaxVel"></param>
        /// <param name="Tacc"></param>
        /// <param name="Tdec"></param>
        /// <param name="SVacc"></param>
        /// <param name="SVdec"></param>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_start_sa_move(System.Int16 AxisNo, System.Double Pos, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec, System.Double SVacc, System.Double SVdec);

        /// <summary>
        ///
        /// </summary>
        /// <param name="AxisNo"></param>
        /// <param name="move_ratio"></param>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_set_move_ratio(System.Int16 AxisNo, System.Double move_ratio);

        /// <summary>
        ///
        /// </summary>
        /// <param name="CardId"></param>
        /// <param name="DistX"></param>
        /// <param name="DistY"></param>
        /// <param name="StrVel"></param>
        /// <param name="MaxVel"></param>
        /// <param name="Tacc"></param>
        /// <param name="Tdec"></param>
        /// <returns></returns>
        //Linear Interpolated Motion Section 6.7
        // Two Axes Linear Interpolation function
        [DllImport("C154.dll")] public static extern Int16 c154_start_tr_move_xy(System.Int16 CardId, System.Double DistX, System.Double DistY, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec);

        /// <summary>
        ///
        /// </summary>
        /// <param name="CardId"></param>
        /// <param name="DistX"></param>
        /// <param name="DistY"></param>
        /// <param name="StrVel"></param>
        /// <param name="MaxVel"></param>
        /// <param name="Tacc"></param>
        /// <param name="Tdec"></param>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_start_tr_move_zu(System.Int16 CardId, System.Double DistX, System.Double DistY, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec);

        /// <summary>
        ///
        /// </summary>
        /// <param name="CardId"></param>
        /// <param name="PosX"></param>
        /// <param name="PosY"></param>
        /// <param name="StrVel"></param>
        /// <param name="MaxVel"></param>
        /// <param name="Tacc"></param>
        /// <param name="Tdec"></param>
        /// <returns></returns>

        [DllImport("C154.dll")] public static extern Int16 c154_start_ta_move_xy(System.Int16 CardId, System.Double PosX, System.Double PosY, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec);

        /// <summary>
        ///
        /// </summary>
        /// <param name="CardId"></param>
        /// <param name="PosX"></param>
        /// <param name="PosY"></param>
        /// <param name="StrVel"></param>
        /// <param name="MaxVel"></param>
        /// <param name="Tacc"></param>
        /// <param name="Tdec"></param>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_start_ta_move_zu(System.Int16 CardId, System.Double PosX, System.Double PosY, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec);

        /// <summary>
        ///
        /// </summary>
        /// <param name="CardId"></param>
        /// <param name="DistX"></param>
        /// <param name="DistY"></param>
        /// <param name="StrVel"></param>
        /// <param name="MaxVel"></param>
        /// <param name="Tacc"></param>
        /// <param name="Tdec"></param>
        /// <param name="SVacc"></param>
        /// <param name="SVdec"></param>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_start_sr_move_xy(System.Int16 CardId, System.Double DistX, System.Double DistY, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec, System.Double SVacc, System.Double SVdec);

        /// <summary>
        ///
        /// </summary>
        /// <param name="CardId"></param>
        /// <param name="DistX"></param>
        /// <param name="DistY"></param>
        /// <param name="StrVel"></param>
        /// <param name="MaxVel"></param>
        /// <param name="Tacc"></param>
        /// <param name="Tdec"></param>
        /// <param name="SVacc"></param>
        /// <param name="SVdec"></param>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_start_sr_move_zu(System.Int16 CardId, System.Double DistX, System.Double DistY, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec, System.Double SVacc, System.Double SVdec);

        /// <summary>
        ///
        /// </summary>
        /// <param name="CardId"></param>
        /// <param name="PosX"></param>
        /// <param name="PosY"></param>
        /// <param name="StrVel"></param>
        /// <param name="MaxVel"></param>
        /// <param name="Tacc"></param>
        /// <param name="Tdec"></param>
        /// <param name="SVacc"></param>
        /// <param name="SVdec"></param>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_start_sa_move_xy(System.Int16 CardId, System.Double PosX, System.Double PosY, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec, System.Double SVacc, System.Double SVdec);

        /// <summary>
        ///
        /// </summary>
        /// <param name="CardId"></param>
        /// <param name="PosX"></param>
        /// <param name="PosY"></param>
        /// <param name="StrVel"></param>
        /// <param name="MaxVel"></param>
        /// <param name="Tacc"></param>
        /// <param name="Tdec"></param>
        /// <param name="SVacc"></param>
        /// <param name="SVdec"></param>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_start_sa_move_zu(System.Int16 CardId, System.Double PosX, System.Double PosY, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec, System.Double SVacc, System.Double SVdec);

        /// <summary>
        ///
        /// </summary>
        /// <param name="AxisArray"></param>
        /// <param name="DistArray"></param>
        /// <param name="StrVel"></param>
        /// <param name="MaxVel"></param>
        /// <param name="Tacc"></param>
        /// <param name="Tdec"></param>
        /// <returns></returns>
        //Any 2 of former or later 4 axes linear interpolation function
        [DllImport("C154.dll")] public static extern Int16 c154_start_tr_line2(ref System.Int16 AxisArray, ref System.Double DistArray, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec);

        /// <summary>
        ///
        /// </summary>
        /// <param name="AxisArray"></param>
        /// <param name="PosArray"></param>
        /// <param name="StrVel"></param>
        /// <param name="MaxVel"></param>
        /// <param name="Tacc"></param>
        /// <param name="Tdec"></param>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_start_ta_line2(ref System.Int16 AxisArray, ref System.Double PosArray, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec);

        /// <summary>
        ///
        /// </summary>
        /// <param name="AxisArray"></param>
        /// <param name="DistArray"></param>
        /// <param name="StrVel"></param>
        /// <param name="MaxVel"></param>
        /// <param name="Tacc"></param>
        /// <param name="Tdec"></param>
        /// <param name="SVacc"></param>
        /// <param name="SVdec"></param>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_start_sr_line2(ref System.Int16 AxisArray, ref System.Double DistArray, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec, System.Double SVacc, System.Double SVdec);

        /// <summary>
        ///
        /// </summary>
        /// <param name="AxisArray"></param>
        /// <param name="PosArray"></param>
        /// <param name="StrVel"></param>
        /// <param name="MaxVel"></param>
        /// <param name="Tacc"></param>
        /// <param name="Tdec"></param>
        /// <param name="SVacc"></param>
        /// <param name="SVdec"></param>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_start_sa_line2(ref System.Int16 AxisArray, ref System.Double PosArray, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec, System.Double SVacc, System.Double SVdec);

        //Any 3 of former or later 4 axes linear interpolation function
        [DllImport("C154.dll")] public static extern Int16 c154_start_tr_line3(ref System.Int16 AxisArray, ref System.Double DistArray, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec);

        [DllImport("C154.dll")] public static extern Int16 c154_start_ta_line3(ref System.Int16 AxisArray, ref System.Double PosArray, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec);

        [DllImport("C154.dll")] public static extern Int16 c154_start_sr_line3(ref System.Int16 AxisArray, ref System.Double DistArray, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec, System.Double SVacc, System.Double SVdec);

        [DllImport("C154.dll")] public static extern Int16 c154_start_sa_line3(ref System.Int16 AxisArray, ref System.Double PosArray, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec, System.Double SVacc, System.Double SVdec);

        //Former or later 4 Axes linear interpolation function
        [DllImport("C154.dll")] public static extern Int16 c154_start_tr_line4(ref System.Int16 AxisArray, ref System.Double DistArray, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec);

        [DllImport("C154.dll")] public static extern Int16 c154_start_ta_line4(ref System.Int16 AxisArray, ref System.Double PosArray, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec);

        [DllImport("C154.dll")] public static extern Int16 c154_start_sr_line4(ref System.Int16 AxisArray, ref System.Double DistArray, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec, System.Double SVacc, System.Double SVdec);

        [DllImport("C154.dll")] public static extern Int16 c154_start_sa_line4(ref System.Int16 AxisArray, ref System.Double PosArray, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec, System.Double SVacc, System.Double SVdec);

        //Circular Interpolation Motion Section 6.8
        // Two Axes Arc Interpolation function
        [DllImport("C154.dll")] public static extern Int16 c154_start_tr_arc_xy(System.Int16 CardId, System.Double OffsetCx, System.Double OffsetCy, System.Double OffsetEx, System.Double OffsetEy, System.Int16 CW_CCW, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec);

        [DllImport("C154.dll")] public static extern Int16 c154_start_ta_arc_xy(System.Int16 CardId, System.Double Cx, System.Double Cy, System.Double Ex, System.Double Ey, System.Int16 CW_CCW, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec);

        [DllImport("C154.dll")] public static extern Int16 c154_start_sr_arc_xy(System.Int16 CardId, System.Double OffsetCx, System.Double OffsetCy, System.Double OffsetEx, System.Double OffsetEy, System.Int16 CW_CCW, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec, System.Double SVacc, System.Double SVdec);

        [DllImport("C154.dll")] public static extern Int16 c154_start_sa_arc_xy(System.Int16 CardId, System.Double Cx, System.Double Cy, System.Double Ex, System.Double Ey, System.Int16 CW_CCW, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec, System.Double SVacc, System.Double SVdec);

        [DllImport("C154.dll")] public static extern Int16 c154_start_tr_arc_zu(System.Int16 CardId, System.Double OffsetCx, System.Double OffsetCy, System.Double OffsetEx, System.Double OffsetEy, System.Int16 CW_CCW, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec);

        [DllImport("C154.dll")] public static extern Int16 c154_start_ta_arc_zu(System.Int16 CardId, System.Double Cx, System.Double Cy, System.Double Ex, System.Double Ey, System.Int16 CW_CCW, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec);

        [DllImport("C154.dll")] public static extern Int16 c154_start_sr_arc_zu(System.Int16 CardId, System.Double OffsetCx, System.Double OffsetCy, System.Double OffsetEx, System.Double OffsetEy, System.Int16 CW_CCW, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec, System.Double SVacc, System.Double SVdec);

        [DllImport("C154.dll")] public static extern Int16 c154_start_sa_arc_zu(System.Int16 CardId, System.Double Cx, System.Double Cy, System.Double Ex, System.Double Ey, System.Int16 CW_CCW, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec, System.Double SVacc, System.Double SVdec);

        [DllImport("C154.dll")] public static extern Int16 c154_start_tr_arc2(ref System.Int16 AxisArray, ref System.Double OffsetCenter, ref System.Double OffsetEnd, System.Int16 CW_CCW, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec);

        [DllImport("C154.dll")] public static extern Int16 c154_start_ta_arc2(ref System.Int16 AxisArray, ref System.Double CenterPos, ref System.Double EndPos, System.Int16 CW_CCW, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec);

        [DllImport("C154.dll")] public static extern Int16 c154_start_sr_arc2(ref System.Int16 AxisArray, ref System.Double OffsetCenter, ref System.Double OffsetEnd, System.Int16 CW_CCW, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec, System.Double SVacc, System.Double SVdec);

        [DllImport("C154.dll")] public static extern Int16 c154_start_sa_arc2(ref System.Int16 AxisArray, ref System.Double CenterPos, ref System.Double EndPos, System.Int16 CW_CCW, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double Tdec, System.Double SVacc, System.Double SVdec);

        //Home Return Mode Section 6.10回原点的方式6.10
        /// <summary>
        /// 回原点的方式
        /// </summary>
        /// <param name="AxisNo"></param>
        /// <param name="home_mode"></param>
        /// <param name="org_logic"></param>
        /// <param name="ez_logic"></param>
        /// <param name="ez_count"></param>
        /// <param name="erc_out"></param>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_set_home_config(System.Int16 AxisNo, System.Int16 home_mode, System.Int16 org_logic, System.Int16 ez_logic, System.Int16 ez_count, System.Int16 erc_out);

        [DllImport("C154.dll")] public static extern Int16 c154_home_move(System.Int16 AxisNo, System.Double StrVel, System.Double MaxVel, System.Double Tacc);

        /// <summary>
        ///
        /// </summary>
        /// <param name="AxisNo"></param>
        /// <param name="StrVel"></param>
        /// <param name="MaxVel"></param>
        /// <param name="Tacc"></param>
        /// <param name="ORGOffset"></param>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_home_search(System.Int16 AxisNo, System.Double StrVel, System.Double MaxVel, System.Double Tacc, System.Double ORGOffset);

        //Manual Pulser Motion Section 6.11
        [DllImport("C154.dll")] public static extern Int16 c154_set_pulser_iptmode(System.Int16 AxisNo, System.Int16 InputMode, System.Int16 Inverse);

        [DllImport("C154.dll")] public static extern Int16 c154_disable_pulser_input(System.Int16 AxisNo, System.UInt16 Disable);

        [DllImport("C154.dll")] public static extern Int16 c154_pulser_vmove(System.Int16 AxisNo, System.Double SpeedLimit);

        [DllImport("C154.dll")] public static extern Int16 c154_pulser_pmove(System.Int16 AxisNo, System.Double Dist, System.Double SpeedLimit);

        [DllImport("C154.dll")] public static extern Int16 c154_set_pulser_ratio(System.Int16 AxisNo, System.Int16 DivF, System.Int16 MultiF);

        //Motion Status Section 6.12
        [DllImport("C154.dll")] public static extern Int16 c154_motion_done(System.Int16 AxisNo);

        //Motion Interface I/O Section 6.13
        [DllImport("C154.dll")] public static extern Int16 c154_set_servo(System.Int16 AxisNo, System.Int16 on_off);

        [DllImport("C154.dll")] public static extern Int16 c154_set_clr_mode(System.Int16 AxisNo, System.Int16 clr_mode, System.Int16 targetCounterInBit);

        [DllImport("C154.dll")] public static extern Int16 c154_set_inp(System.Int16 AxisNo, System.Int16 inp_enable, System.Int16 inp_logic);

        [DllImport("C154.dll")] public static extern Int16 c154_set_alm(System.Int16 AxisNo, System.Int16 alm_logic, System.Int16 alm_mode);

        [DllImport("C154.dll")] public static extern Int16 c154_set_erc(System.Int16 AxisNo, System.Int16 erc_logic, System.Int16 erc_pulse_width, System.Int16 erc_mode);

        [DllImport("C154.dll")] public static extern Int16 c154_set_erc_out(System.Int16 AxisNo);

        [DllImport("C154.dll")] public static extern Int16 c154_clr_erc(System.Int16 AxisNo);

        [DllImport("C154.dll")] public static extern Int16 c154_set_sd(System.Int16 AxisNo, System.Int16 sd_logic, System.Int16 sd_latch, System.Int16 sd_mode);

        [DllImport("C154.dll")] public static extern Int16 c154_enable_sd(System.Int16 AxisNo, System.Int16 enable);

        [DllImport("C154.dll")] public static extern Int16 c154_set_limit_logic(System.Int16 AxisNo, System.Int16 Logic);

        [DllImport("C154.dll")] public static extern Int16 c154_set_limit_mode(System.Int16 AxisNo, System.Int16 limit_mode);

        [DllImport("C154.dll")] public static extern Int16 c154_get_io_status(System.Int16 AxisNo, ref System.UInt16 io_sts);

        //Interrupt Control Section 6.14
        [DllImport("C154.dll")] public static extern Int16 c154_int_control(System.Int16 CardId, System.Int16 intFlag);

        [DllImport("C154.dll")] public static extern Int16 c154_wait_error_interrupt(System.Int16 AxisNo, System.Int32 TimeOut_ms);

        [DllImport("C154.dll")] public static extern Int16 c154_wait_motion_interrupt(System.Int16 AxisNo, System.Int16 IntFactorBitNo, System.Int32 TimeOut_ms);

        [DllImport("C154.dll")] public static extern Int16 c154_set_motion_int_factor(System.Int16 AxisNo, System.UInt32 int_factor);

        //Position Control and Counters Section 6.15
        [DllImport("C154.dll")] public static extern Int16 c154_get_position(System.Int16 AxisNo, ref System.Double Pos);

        [DllImport("C154.dll")] public static extern Int16 c154_set_position(System.Int16 AxisNo, System.Double Pos);

        [DllImport("C154.dll")] public static extern Int16 c154_get_command(System.Int16 AxisNo, ref System.Int32 Cmd);

        [DllImport("C154.dll")] public static extern Int16 c154_set_command(System.Int16 AxisNo, System.Int32 Cmd);

        [DllImport("C154.dll")] public static extern Int16 c154_get_error_counter(System.Int16 AxisNo, ref System.Int16 error);

        [DllImport("C154.dll")] public static extern Int16 c154_reset_error_counter(System.Int16 AxisNo);

        [DllImport("C154.dll")] public static extern Int16 c154_set_general_counter(System.Int16 AxisNo, System.Int16 CntSrc, System.Double CntValue);

        [DllImport("C154.dll")] public static extern Int16 c154_get_general_counter(System.Int16 AxisNo, ref System.Double CntValue);

        [DllImport("C154.dll")] public static extern Int16 c154_set_latch_source(System.Int16 AxisNo, System.Int16 LtcSrc);

        [DllImport("C154.dll")] public static extern Int16 c154_set_ltc_logic(System.Int16 AxisNo, System.Int16 LtcLogic);

        [DllImport("C154.dll")] public static extern Int16 c154_get_latch_data(System.Int16 AxisNo, System.Int16 CounterNo, ref System.Double Pos);

        //Continuous Motion Section 6.17
        [DllImport("C154.dll")] public static extern Int16 c154_set_continuous_move(System.Int16 AxisNo, System.Int16 Enable);

        [DllImport("C154.dll")] public static extern Int16 c154_check_continuous_buffer(System.Int16 AxisNo);

        [DllImport("C154.dll")] public static extern Int16 c154_dwell_move(System.Int16 AxisNo, System.Double milliSecond);

        //Multiple Axes Simultaneous Operation Section 6.18
        [DllImport("C154.dll")] public static extern Int16 c154_set_tr_move_all(System.Int16 TotalAxes, ref System.Int16 AxisArray, ref System.Double DistA, ref System.Double StrVelA, ref System.Double MaxVelA, ref System.Double TaccA, ref System.Double TdecA);

        [DllImport("C154.dll")] public static extern Int16 c154_set_sa_move_all(System.Int16 TotalAx, ref System.Int16 AxisArray, ref System.Double PosA, ref System.Double StrVelA, ref System.Double MaxVelA, ref System.Double TaccA, ref System.Double TdecA, ref System.Double SVaccA, ref System.Double SVdecA);

        [DllImport("C154.dll")] public static extern Int16 c154_set_ta_move_all(System.Int16 TotalAx, ref System.Int16 AxisArray, ref System.Double PosA, ref System.Double StrVelA, ref System.Double MaxVelA, ref System.Double TaccA, ref System.Double TdecA);

        [DllImport("C154.dll")] public static extern Int16 c154_set_sr_move_all(System.Int16 TotalAx, ref System.Int16 AxisArray, ref System.Double DistA, ref System.Double StrVelA, ref System.Double MaxVelA, ref System.Double TaccA, ref System.Double TdecA, ref System.Double SVaccA, ref System.Double SVdecA);

        [DllImport("C154.dll")] public static extern Int16 c154_start_move_all(System.Int16 FirstAxisNo);

        [DllImport("C154.dll")] public static extern Int16 c154_stop_move_all(System.Int16 FirstAxisNo);

        [DllImport("C154.dll")] public static extern Int16 c154_set_sync_stop_mode(System.Int16 AxisNo, System.Int16 stop_mode);

        [DllImport("C154.dll")] public static extern Int16 c154_set_sync_option(System.Int16 AxisNo, System.Int16 sync_stop_on, System.Int16 cstop_output_on, System.Int16 sync_option1, System.Int16 sync_option2);

        [DllImport("C154.dll")] public static extern Int16 c154_set_sync_signal_source(System.Int16 AxisNo, System.Int16 sync_axis);

        [DllImport("C154.dll")] public static extern Int16 c154_set_sync_signal_mode(System.Int16 AxisNo, System.Int16 mode);

        //General-purposed Input/Output Section 6.19
        [DllImport("C154.dll")] public static extern Int16 c154_set_gpio_output(System.Int16 CardId, System.Int16 DoValue);

        [DllImport("C154.dll")] public static extern Int16 c154_get_gpio_output(System.Int16 CardId, ref System.Int16 DoValue);

        [DllImport("C154.dll")] public static extern Int16 c154_get_gpio_input(System.Int16 CardId, ref System.Int16 DiValue);

        [DllImport("C154.dll")] public static extern Int16 c154_set_gpio_input_function(System.Int16 CardId, System.Int16 Channel, System.Int16 Select, System.Int16 Logic);

        [DllImport("C154.dll")] public static extern Int16 c154_get_gpio_input_ex(System.Int16 CardId, ref System.Int16 Value);

        [DllImport("C154.dll")] public static extern Int16 c154_set_gpio_output_ex(System.Int16 CardId, System.Int16 Value);

        [DllImport("C154.dll")] public static extern Int16 c154_get_gpio_output_ex(System.Int16 CardId, ref System.Int16 Value);

        /// <summary>
        /// 读取输入信号
        /// </summary>
        /// <param name="CardId"></param>
        /// <param name="Channel"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_get_gpio_input_ex_CH(System.Int16 CardId, System.Int16 Channel, ref System.Int16 Value);

        /// <summary>
        ///
        /// </summary>
        /// <param name="CardId"></param>
        /// <param name="Channel"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_set_gpio_output_ex_CH(System.Int16 CardId, System.Int16 Channel, System.Int16 Value);

        /// <summary>
        ///
        /// </summary>
        /// <param name="CardId"></param>
        /// <param name="Channel"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        [DllImport("C154.dll")] public static extern Int16 c154_get_gpio_output_ex_CH(System.Int16 CardId, System.Int16 Channel, ref System.Int16 Value);

        //Soft Limit 6.20
        [DllImport("C154.dll")] public static extern Int16 c154_disable_soft_limit(System.Int16 AxisNo);

        [DllImport("C154.dll")] public static extern Int16 c154_enable_soft_limit(System.Int16 AxisNo, System.Int16 Action);

        [DllImport("C154.dll")] public static extern Int16 c154_set_soft_limit(System.Int16 AxisNo, System.Int32 PlusLimit, System.Int32 MinusLimit);

        //Backlas Compensation / Vibration Suppression 6.21
        [DllImport("C154.dll")] public static extern Int16 c154_backlash_comp(System.Int16 AxisNo, System.Int16 CompPulse, System.Int16 Mode);

        [DllImport("C154.dll")] public static extern Int16 c154_suppress_vibration(System.Int16 AxisNo, System.UInt16 ReverseTime, System.UInt16 ForwardTime);

        [DllImport("C154.dll")] public static extern Int16 c154_set_fa_speed(System.Int16 AxisNo, System.Double FA_Speed);
    }
}