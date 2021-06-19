using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vision2.vision.HalconRunFile.RunProgramFile;
using HalconDotNet;
using System.ComponentModel;
using Vision2.ErosProjcetDLL.UI.PropertyGrid;

namespace Vision2.vision.Library
{
     public  class LibraryVisionBase : DicHtuple
     {
        public int Row { get; set; }

        public int Col { get; set; }

        public double Angle { get; set; }

        [DescriptionAttribute("根据目标仿射定位。"), Category("定位"), DisplayName("定位目标名称"),
          TypeConverter(typeof(ErosConverter)),  ErosConverter.ThisDropDown("HomNameList", false, "")]
        public string LibraryName { get; set; }

        public List<string> HomNameList
        {
            get
            {
                    List<string> listS = new List<string>();
                    foreach (var item in Vision.GetLibrary())
                    {
                        if (item.Value is ModelVision)
                        {
                            listS.Add(item.Key);
                        }
                    }
                    return listS;
            }
        }

        public int NGNumber = 0;

        public bool RestBool;

        public bool Run( OneResultOBj oneResultO)
        {
            AoiObj aoiObj = new AoiObj();
            HOperatorSet.Union1(Vision.GetLibrary()[LibraryName].AOIObj, out  aoiObj.Aoi);

            HOperatorSet.Union1(Vision.GetLibrary()[LibraryName].DrawObj, out aoiObj.Drow);

            HOperatorSet.AreaCenter(aoiObj.Aoi, out HTuple area, out HTuple row, out HTuple col);
            HOperatorSet.VectorAngleToRigid(row, col, 0, Row, Col, Angle, out HTuple hom2dt);
            HOperatorSet.AffineTransRegion(aoiObj.Aoi, out aoiObj.Aoi, hom2dt, "nearest_neighbor");
            HOperatorSet.AffineTransRegion(aoiObj.Drow, out aoiObj.Drow, hom2dt, "nearest_neighbor");
            oneResultO.AddObj(aoiObj.Aoi, ColorResult.blue);
            oneResultO.AddObj(aoiObj.Drow, ColorResult.yellow);
            RestBool = Vision.GetLibrary()[LibraryName].Run( oneResultO,0,aoiObj);

            return RestBool;
        }



    }
}
