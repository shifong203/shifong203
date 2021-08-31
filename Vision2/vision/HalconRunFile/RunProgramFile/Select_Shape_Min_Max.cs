using HalconDotNet;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
    /// <summary>
    /// 赛选类
    /// </summary>
    public class Select_shape_Min_Max
    {
        public Select_shape_Min_Max()
        {
            AddSelectType(Enum_Select_Type.area, 100, 999999);
        }

        public void AddDataG(DataGridView dataGridView)
        {
            int de = dataGridView.Rows.Add();
            dataGridView.Rows[de].Cells[0].Value = Select_Type;
            dataGridView.Rows[de].Cells[1].Value = Min;
            dataGridView.Rows[de].Cells[2].Value = Max;
        }

        public static List<string> GetList()
        {
            List<string> listValues = new List<string>();
            listValues.Add("area");
            listValues.Add("height");
            listValues.Add("width");
            listValues.Add("radius");
            listValues.Add("column");
            listValues.Add("row");
            listValues.Add("ra");
            listValues.Add("rb");
            listValues.Add("ratio");
            listValues.Add("orientation");
            listValues.Add("bulkiness");
            listValues.Add("circularity");
            listValues.Add("convexity");
            listValues.Add("dist_deviation");
            listValues.Add("inner_height");
            listValues.Add("inner_width");
            listValues.Add("inner_radius");
            listValues.Add("area_holes");
            return listValues;
        }

        public enum Enum_Select_Type
        {
            area = 0,
            height = 1,
            width = 2,
            area_holes = 3,
            radius = 4,
            column = 5,
            row = 6,
            ra = 7,
            rb = 8,
            ratio = 9,
            orientation = 10,
            bulkiness = 11,
            circularity = 12,
            convexity = 13,
            dist_deviation = 14,
            inner_height = 15,
            inner_width = 16,
            inner_radius = 17,
            roundness = 18,
            num_sides = 19,
            max_diameter = 20,
            euler_number = 21,
            rectangularity = 22,
        }

        public Enum_Select_Type Select_type { get; set; }

        public void AddSelectType(Enum_Select_Type enum_Select_Type, double min, double max)
        {
            AddSelectType(enum_Select_Type.ToString(), min, max);
        }

        public void AddSelectType(string enum_Select_Type, double min, double max)
        {
            Select_Type.Append(enum_Select_Type);
            Min.Append(min);
            Max.Append(max);
        }

        public bool AddSelectType(string enum_Select_Type, string min, string max)
        {
            if (double.TryParse(min, out double minRelset) && double.TryParse(max, out double maxrelst))
            {
                Select_Type.Append(enum_Select_Type);
                Min.Append(minRelset);
                Max.Append(maxrelst);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Clear()
        {
            Select_Type = new HTuple();
            Min = new HTuple();
            Max = new HTuple();
        }

        public HTuple Select_Type { get; set; } = new HTuple();
        public bool and_OR { get; set; }
        public HTuple Min { get; set; } = new HTuple();
        public HTuple Max { get; set; } = new HTuple();

        public HObject select_shape(HObject hObject, int runid = -1)
        {
            HObject hObject1;
            string Operation = "and";
            if (and_OR)
            {
                Operation = "or";
            }
            if (Select_Type.Length > Min.Length)
            {
                Select_Type = Select_Type.TupleRemove(Select_Type.Length - 1);
            }
            if (Max.Length > Min.Length)
            {
                Max = Max.TupleRemove(Max.Length - 1);
            }
            //HOperatorSet.Connection(hObject, out hObject);
            if (runid >= 0)
            {
                HOperatorSet.SelectShape(hObject, out hObject1, Select_Type[runid], Operation, Min[runid], Max[runid]);
            }
            else
            {
                //Select_Type = "arae";
                HOperatorSet.SelectShape(hObject, out hObject1, Select_Type, Operation, Min, Max);
            }
            return hObject1;
        }
    }
}