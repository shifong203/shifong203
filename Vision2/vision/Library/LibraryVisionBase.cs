using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vision2.vision.HalconRunFile.RunProgramFile;
using HalconDotNet;
using System.ComponentModel;
using Vision2.ErosProjcetDLL.UI.PropertyGrid;
using System.Windows.Forms;

namespace Vision2.vision.Library
{
     public  class LibraryVisionBase : DicHtuple
     {

        public Control GetControl(HalconRun halconRun)
        {
            try
            {
                return Vision.GetLibrary()[LibraryName].GetControl(halconRun);
            }
            catch (Exception ex)
            { }
            return null;
        }
        public RunProgram GetRun()
        {
            try
            {
                return Vision.GetLibrary()[LibraryName];
            }
            catch (Exception ex)
            { }
            return null;
        }

        public int RunID = 1;
        public double Row { get; set; }

        public double Col { get; set; }

        public double Angle { get; set; }

        public string ToolDone { get; set; } = "";

        public int Length1 { get; set; } = 100;
        public int Length2 { get; set; } = 50;

        [DescriptionAttribute("调用的库名称。"), Category("定位"), DisplayName("库名称"),
          TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDown("LibraryListName", false, "")]
        public string LibraryName { get; set; } = "";

        public List<string> LibraryListName
        {
            get
            {
                    List<string> listS = new List<string>();
                    foreach (var item in Vision.GetLibrary())
                    {
                        if (item.Value is RunProgram)
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
            RestBool = false;
            try
            {
                if (LibraryName ==null)
                {
                    return false;
                }
                    if (LibraryName!="")
                {
                    HOperatorSet.Union1(Vision.GetLibrary()[LibraryName].AOIObj, out aoiObj.Aoi);
                    HOperatorSet.Union1(Vision.GetLibrary()[LibraryName].DrawObj, out aoiObj.Drow);
                    HOperatorSet.AreaCenter(aoiObj.Aoi, out HTuple area, out HTuple row, out HTuple col);
                    HOperatorSet.VectorAngleToRigid(row, col, 0, Row, Col, Angle, out HTuple hom2dt);
                    HOperatorSet.AffineTransRegion(aoiObj.Aoi, out aoiObj.Aoi, hom2dt, "nearest_neighbor");
                    HOperatorSet.AffineTransRegion(aoiObj.Drow, out aoiObj.Drow, hom2dt, "nearest_neighbor");
                    oneResultO.AddObj(aoiObj.Aoi, ColorResult.blue);
                    oneResultO.AddObj(aoiObj.Drow, ColorResult.yellow);
                    RestBool = Vision.GetLibrary()[LibraryName].Run(oneResultO, aoiObj);

                }
              

            }
            catch (Exception ex)
            {
            }
            return RestBool;
        }

        public AoiObj GetAOI()
        {
            AoiObj aoiObj = new AoiObj();
            try
            {
                HOperatorSet.Union1(Vision.GetLibrary()[LibraryName].AOIObj, out aoiObj.Aoi);
                HOperatorSet.Union1(Vision.GetLibrary()[LibraryName].DrawObj, out aoiObj.Drow);
                HOperatorSet.AreaCenter(aoiObj.Aoi, out HTuple area, out HTuple row, out HTuple col);
                HOperatorSet.VectorAngleToRigid(row, col, 0, Row, Col, Angle, out HTuple hom2dt);
                HOperatorSet.AffineTransRegion(aoiObj.Aoi, out aoiObj.Aoi, hom2dt, "nearest_neighbor");
                HOperatorSet.AffineTransRegion(aoiObj.Drow, out aoiObj.Drow, hom2dt, "nearest_neighbor");
            }
            catch (Exception)
            {
            }
            return aoiObj;
        }

    }
}
