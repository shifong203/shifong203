using HalconDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Vision2.Project.DebugF.IO;

namespace Vision2.Project.formula
{
    public class ProductEX
    {

        public ProductEX()
        {

        }

        [Description("。"), Category("产品尺寸"), DisplayName("产品宽度")]
        /// <summary>
        /// 产品宽度mm
        /// </summary>
        public int ProductWidth { get; set; } = 100;
        [Description("。"), Category("产品尺寸"), DisplayName("产品高度")]
        /// <summary>
        /// 产品高度mm
        /// </summary>
        public int ProductHeight { get; set; } = 10;
        [Description("。"), Category("产品尺寸"), DisplayName("产品长度")]
        /// <summary>
        /// 产品长度mm
        /// </summary>
        public int ProductLength { get; set; } = 100;
        [Description("。"), Category("产品尺寸"), DisplayName("托盘产品数量")]
        /// <summary>
        /// 产品总数
        /// </summary>
        public int ProductAmount { get; set; } = 1;

        [Description("。"), Category("视觉"), DisplayName("单个产品拍照次数")]
        /// <summary>
        /// 产品总数
        /// </summary>
        public int ProductNumber { get; set; } = 1;

        /// <summary>
        /// 附加测试参数
        /// </summary>
        public List<DataMinMax> ListDicData { get; set; } = new List<DataMinMax>();

        /// <summary>
        /// 点位 集合
        /// </summary>
        public List<XYZPoint> DPoint { get; set; } = new List<XYZPoint>();


        public List<string> GetPointNames()
        {
            List<string> vs = new List<string>();
            for (int i = 0; i < DPoint.Count; i++)
            {
                vs.Add(DPoint[i].Name);
            }
            return vs;
        }
        public XYZPoint GetPoint(string name)
        {

            for (int i = 0; i < DPoint.Count; i++)
            {
                if (DPoint[i].Name == name)
                {
                    return DPoint[i];
                }
            }
            return null;
        }
        public int GetPointIntdx(string name)
        {
            ;
            for (int i = 0; i < DPoint.Count; i++)
            {
                if (DPoint[i].Name == name)
                {
                    return i;
                }
            }
            return -1;
        }

        public Relatively Relativel { get; set; } = new Relatively();

        ///// <summary>
        ///// 轨迹点
        ///// </summary>
        //public List<XYZPoint> RelativelyPoint { get; set; } = new List<XYZPoint>();


        /// <summary>
        /// 导航图面
        /// </summary>
        public Dictionary<string, Navigation_Picture> Key_Navigation_Picture { get; set; } = new Dictionary<string, Navigation_Picture>();

        /// <summary>
        /// 配方参数
        /// </summary>
        public Dictionary<string, string> Produc { get; set; } = new Dictionary<string, string>();
        /// <summary>
        /// 导航图
        /// </summary>
        public class Navigation_Picture
        {

            public Navigation_Picture()
            {
            }
            /// <summary>
            /// 导航图面
            /// </summary>
            public Dictionary<string, HObject> KeyRoi { get; set; } = new Dictionary<string, HObject>();

            HObject ImageTimff;

            public HObject GetHObject()
            {
                if (ImageTimff == null)
                {
                    if (System.IO.File.Exists(ImagePath))
                    {
                        HOperatorSet.ReadImage(out HObject image2, ImagePath);
                        ImageTimff = image2;
                    }
                    else
                    {
                    }
                }
                return ImageTimff;
            }

            public void Cler()
            {
                try
                {
                    if (ImageTimff != null)
                    {
                        ImageTimff.Dispose();
                        ImageTimff = null;
                    }
                }
                catch (Exception)
                {

                }

            }
            public string ImagePath { get; set; } = "";


        }

        /// <summary>
        /// 轨迹
        /// </summary>
        public class Relatively
        {
            public enum EnumPointType
            {
                不启用 = 0,
                扫码点 = 1,
                独立执行 = 2,
                过程点 = 3,
                同时执行 = 4,
            }
            /// <summary>
            /// 起点
            /// </summary>
            public class PointType
            {
                public string PointNmae { get; set; }
                public EnumPointType EnumPointTyp { get; set; }
                public string RelativeLyPiintName { get; set; }
            }

            /// <summary>
            /// 轨迹流程
            /// </summary>
            public List<List<PointType>> ListListPointName { get; set; } = new List<List<PointType>>();

            public int RelativelyId;

            /// <summary>
            /// 轨迹
            /// </summary>

            public Dictionary<string, List<XYZPoint>> DicRelativelyPoint = new Dictionary<string, List<XYZPoint>>();


        }

    }
}
