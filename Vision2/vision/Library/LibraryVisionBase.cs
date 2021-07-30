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

        public Control GetControl(HalconRun halconRun=null)
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
        public HObject GetRoi()
        {
            RunProgram run = GetRun();
            if (run != null)
            {
                HOperatorSet.SmallestRectangle2(run.AOIObj, out HTuple row, out HTuple col, out HTuple phi, out HTuple leng1, out HTuple length2);
                if (row.Length == 1)
                {
                    Length1 = leng1.TupleInt();
                    Length2 = length2.TupleInt();
                }
            }
         
            HOperatorSet.GenRectangle2(out HObject recaoi, Row, Col, new HTuple(Angle).TupleRad(), Length1, Length2);
            return recaoi;
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

        public AoiObj Run( OneResultOBj oneResultO)
        {
            AoiObj aoiObj = new AoiObj();
            aoiObj.AoiRow = Row;
            aoiObj.AoiCol = Col;
            aoiObj.Angle = Angle;
            aoiObj.IsLibrary = true;
            aoiObj.CiName = Name;
            RestBool = false;
            try
            {
                    if (LibraryName ==null)
                    {
                        aoiObj.RestBool = true;
                        return aoiObj;
                    }
                    if (LibraryName!="")
                    {
                        aoiObj.RPName = LibraryName;
                        HOperatorSet.Union1(Vision.GetLibrary()[LibraryName].AOIObj, out aoiObj.SelseAoi);
                        HOperatorSet.Union1(Vision.GetLibrary()[LibraryName].DrawObj, out aoiObj.Drow);
                        HOperatorSet.AreaCenter(aoiObj.SelseAoi, out HTuple area, out HTuple row, out HTuple col);
                        if (row.Length==1)
                        {
                            HOperatorSet.VectorAngleToRigid(row, col, 0, Row, Col, new HTuple(Angle).TupleRad(), out HTuple hom2dt);
                             aoiObj.Homt2D = hom2dt;
                            HOperatorSet.AffineTransRegion(aoiObj.SelseAoi, out aoiObj.SelseAoi, hom2dt, "nearest_neighbor");
                            HOperatorSet.AffineTransRegion(aoiObj.Drow, out aoiObj.Drow, hom2dt, "nearest_neighbor");
                        }
                        //oneResultO.AddObj(aoiObj.Aoi, ColorResult.blue);
                        //oneResultO.AddObj(aoiObj.Drow, ColorResult.yellow);
                        RestBool = Vision.GetLibrary()[LibraryName].Run(oneResultO, aoiObj);
                    }
                    else
                    {
                        aoiObj.RestBool = true;
                    }
            }
            catch (Exception ex)
            { }
            return aoiObj;
        }

        public AoiObj Run(IDrawHalcon drawHalcon )
        {
            AoiObj aoiObj = new AoiObj();
            RestBool = false;
            try
            {
                if (LibraryName == null)
                {
                    return aoiObj;
                }
                if (LibraryName != "")
                {
                    HOperatorSet.Union1(Vision.GetLibrary()[LibraryName].AOIObj, out aoiObj.SelseAoi);
                    HOperatorSet.Union1(Vision.GetLibrary()[LibraryName].DrawObj, out aoiObj.Drow);
                    HOperatorSet.AreaCenter(aoiObj.SelseAoi, out HTuple area, out HTuple row, out HTuple col);
                    HOperatorSet.VectorAngleToRigid(row, col, 0, Row, Col, new HTuple(Angle).TupleRad(), out HTuple hom2dt);
                    HOperatorSet.AffineTransRegion(aoiObj.SelseAoi, out aoiObj.SelseAoi, hom2dt, "nearest_neighbor");
                    HOperatorSet.AffineTransRegion(aoiObj.Drow, out aoiObj.Drow, hom2dt, "nearest_neighbor");
                    drawHalcon.AddObj(aoiObj.SelseAoi, ColorResult.blue);
                    drawHalcon.AddObj(aoiObj.Drow, ColorResult.yellow);
                    OneResultOBj oneResultOBj = new OneResultOBj();
                    oneResultOBj.Image = drawHalcon.Image();
                    RestBool = Vision.GetLibrary()[LibraryName].Run(oneResultOBj, aoiObj);
                    aoiObj.RestBool = RestBool;
                }
            }
            catch (Exception ex)
            {
            }
            return aoiObj;
        }

        public AoiObj GetAOI()
        {
            AoiObj aoiObj = new AoiObj();
            try
            {
                HOperatorSet.Union1(Vision.GetLibrary()[LibraryName].AOIObj, out aoiObj.SelseAoi);
                HOperatorSet.Union1(Vision.GetLibrary()[LibraryName].DrawObj, out aoiObj.Drow);
                HOperatorSet.AreaCenter(aoiObj.SelseAoi, out HTuple area, out HTuple row, out HTuple col);
                HOperatorSet.VectorAngleToRigid(row, col, 0, Row, Col, new HTuple(Angle).TupleRad(), out HTuple hom2dt);
                HOperatorSet.AffineTransRegion(aoiObj.SelseAoi, out aoiObj.SelseAoi, hom2dt, "nearest_neighbor");
                HOperatorSet.AffineTransRegion(aoiObj.Drow, out aoiObj.Drow, hom2dt, "nearest_neighbor");
            }
            catch (Exception)
            {
            }
            return aoiObj;
        }

    }
}
