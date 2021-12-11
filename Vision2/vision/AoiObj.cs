using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vision2.vision
{

    /// <summary>
    /// 库数据
    /// </summary>
    public class AoiObj
    {
        public bool RestBool;

        public HObject GetAOI(HObject aoiObj, HObject draw = null)
        {
            try
            {
                SelseAoi = aoiObj;
                HOperatorSet.Union1(aoiObj, out HObject selseAoi);
                HOperatorSet.AreaCenter(selseAoi, out HTuple area, out AoiRow, out AoiCol);

                if (IsLibrary)
                {
                    if (draw != null)
                    {
                        HOperatorSet.Union1(draw, out Drow);
                    }
                    if (AoiRow.Length == 1)
                    {
                        HOperatorSet.VectorAngleToRigid(AoiRow, AoiCol, 0, AoiRow, AoiCol, new HTuple(Angle).TupleRad(), out HTuple hom2dt);
                        Homt2D = hom2dt;
                        HOperatorSet.AffineTransRegion(SelseAoi, out SelseAoi, hom2dt, "nearest_neighbor");
                        if (draw != null)
                        {
                            HOperatorSet.AffineTransRegion(Drow, out Drow, hom2dt, "nearest_neighbor");
                        }
                    }
                }
                else
                {
                    CiName = "";
                }
            }
            catch (Exception)
            {
            }
            return SelseAoi;
        }

        public AoiObj()
        {
            NGErr.GenEmptyObj();
        }

        public AoiObj(int runid) : this()
        {
            DebugID = runid;
        }

        public HObject NGErr = new HObject();
        /// <summary>
        /// 搜索区域
        /// </summary>
        public HObject SelseAoi = new HObject();

        /// <summary>
        /// 掩模区域
        /// </summary>
        public HObject Drow = new HObject();

        /// <summary>
        /// 元件名
        /// </summary>
        public string CiName = "";

        /// <summary>
        /// 程序名
        /// </summary>
        public string RPName = "";

        /// <summary>
        /// 搜索坐标
        /// </summary>
        public HTuple AoiRow;

        /// <summary>
        /// 搜索坐标
        /// </summary>
        public HTuple AoiCol;

        /// <summary>
        /// 角度
        /// </summary>
        public double Angle;

        /// <summary>
        ///
        /// </summary>
        public HTuple Homt2D;

        /// <summary>
        ///
        /// </summary>
        public bool IsLibrary = false;

        /// <summary>
        /// 调试ID
        /// </summary>
        public int DebugID;
    }

}
