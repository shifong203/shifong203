using ErosSocket.DebugPLC;
using System.Collections.Generic;

namespace Vision2.Project.DebugF.IO
{
    public interface IAxisGrub
    {
        sbyte Is_braking { get; set; }

        List<XYZPoint> XyzPoints { get; set; }

        /// <summary>
        /// 轴集合
        /// </summary>
        List<Axis> AxisS { get; set; }

        Dictionary<string, List<string>> AxisGrot { get; set; }

        List<Axis> GetAxisGrotName(string name);

        Axis GetAxisGrotNameEx(string name, EnumAxisType enumAxisType);

        bool SetXYZ1Points(string groupName, int outTime = 0, float? xp = null, float? yp = null, float? zp = null, bool isMove = false, float? jumpZ = 0);

        void GetAxisGroupPoints(string groupName, out double? xp, out double? yp, out double? zp);
    }
}