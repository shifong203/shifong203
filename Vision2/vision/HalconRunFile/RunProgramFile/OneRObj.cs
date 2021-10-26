using HalconDotNet;
using System.Collections.Generic;
using Vision2.Project.formula;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
    public class MassageText
    {
        public List<HTuple> Rows = new List<HTuple>();

        public List<HTuple> Columns = new List<HTuple>();

        public List<HTuple> Massage = new List<HTuple>();

        public string MassageBlute = "false";

        public string color = "red";

        public void ShowMassage(HTuple hWindowHalconID)
        {
            for (int i = 0; i < Massage.Count; i++)
            {
                if (Massage[i].Length != 0)
                {
                    Vision.Disp_message(hWindowHalconID, Massage[i], Rows[i], Columns[i], false, color, MassageBlute);
                }
            }
        }

        public void AddImageMassage(HTuple rows, HTuple columns, HTuple Massage)
        {
            if (columns.Length != Massage.Length)
            {
                Massage = HTuple.TupleGenConst(columns.Length, Massage);
            }
            if (rows.Length == columns.Length && columns.Length == Massage.Length)
            {
                this.color = color = "red";
                this.Rows.Add(rows);
                this.Columns.Add(columns);
                this.Massage.Add(Massage);
            }
        }
    }

    /// <summary>
    /// 单个缺陷
    /// </summary>
    public class OneRObj
    {
        public HObject ROI;

        public HObject NGROI = new HObject();

        public int Row;
        public int Col;

        public OneRObj()
        {
            NGROI.GenEmptyObj();
            ROI = new HObject();
            ROI.GenEmptyObj();
        }

        public OneRObj(OneRObj oneRObj)
        {
            this.ComponentID = oneRObj.ComponentID;
            this.RestText = oneRObj.RestText;
            if (!oneRObj.aOK)
            {
                this.NGText = oneRObj.NGText;
            }
            aOK = oneRObj.aOK;
            this.dataMinMax = oneRObj.dataMinMax;
            this.Done = oneRObj.Done;
            this.NGROI = oneRObj.NGROI;
            this.ROI = oneRObj.ROI;

            //OneOdata[] oneOdat= new OneOdata[] { };
            //oneRObj.oneOdatas.CopyTo(oneOdat);
            //oneOdatas.AddRange(oneOdat);
            this.RestStrings = oneRObj.RestStrings;
        }

        /// <summary>
        /// 参数集合
        /// </summary>
        public DataMinMax dataMinMax = new DataMinMax();

        public bool aOK;

        public bool Done;

        public string RunName = "";

        /// <summary>
        /// 元件名称
        /// </summary>
        public string ComponentID { get; set; } = "";

        /// <summary>
        /// NG选项
        /// </summary>
        public string NGText
        {
            get { return ngText; }
            set
            {
                if (CNGText == null)
                {
                    CNGText = value;
                }
                ngText = value;
            }
        }

        private string ngText = "";

        /// <summary>
        /// 机器NG判断
        /// </summary>
        public string CNGText;

        /// <summary>
        /// 复判缺陷
        /// </summary>
        public string RestText = "";

        public List<string> RestStrings = new List<string>();

        public void RAddOK()
        {
            try
            {
                //if (ROI != null)
                //{
                //    ROI.GenEmptyObj();
                //}
                //if (NGROI != null)
                //{
                //    NGROI.GenEmptyObj();
                //}
            }
            catch (System.Exception)
            {
            }
            RestText = "OK";

            aOK = true;
            Done = true;
        }

        public void RAddNG(string restText)
        {
            //if (ROI != null)
            //{
            //    ROI.GenEmptyObj();
            //}
            //if (NGROI != null)
            //{
            //    NGROI.GenEmptyObj();
            //}
            RestText = restText;
            Done = true;
            aOK = false;
        }
    }

    /// <summary>
    /// 元件集合
    /// </summary>
    public class OneCompOBJs
    {
        public void AddCont(OneRObj oneRObj)
        {
            try
            {
                if (!DicOnes.ContainsKey(oneRObj.ComponentID))
                {
                    DicOnes.Add(oneRObj.ComponentID, new OneComponent() { ComponentID = oneRObj.ComponentID });
                }
                DicOnes[oneRObj.ComponentID].Row = oneRObj.Row;
                DicOnes[oneRObj.ComponentID].Col = oneRObj.Col;
                DicOnes[oneRObj.ComponentID].oneRObjs.Add(oneRObj);
            }
            catch (System.Exception)
            {
            }
        }

        public void Add(OneComponent oneRObj)
        {
            if (!DicOnes.ContainsKey(oneRObj.ComponentID))
            {
                DicOnes.Add(oneRObj.ComponentID, oneRObj);
            }
            else
            {
            }
        }

        /// <summary>
        ///
        /// </summary>
        public Dictionary<string, OneComponent> DicOnes = new Dictionary<string, OneComponent>();

        /// <summary>
        /// 单个元件,_缺陷集合
        /// </summary>
        public class OneComponent
        {
            public List<OneRObj> oneRObjs = new List<OneRObj>();

            public int Row;
            public int Col;

            public void AddNgObj(string ngText, string conam, HTuple restString, HObject roi, HObject NGerr, DataMinMax dataMinMax)
            {
                OneRObj oneRObj = new OneRObj() { NGText = ngText, ComponentID = conam, ROI = roi, NGROI = NGerr, dataMinMax = dataMinMax };
                oneRObj.RestStrings.AddRange(restString.ToSArr());
                oneRObjs.Add(oneRObj);
            }

            public bool aOK
            {
                get
                {
                    for (int i = 0; i < oneRObjs.Count; i++)
                    {
                        if (!oneRObjs[i].aOK) return false;
                    }
                    return true;
                }
                set
                {
                    for (int i = 0; i < oneRObjs.Count; i++)
                    {
                        oneRObjs[i].aOK = value;
                    }
                }
            }

            public string ComponentID { get; set; } = "";

            public bool Done
            {
                get
                {
                    for (int i = 0; i < oneRObjs.Count; i++)
                    {
                        if (!oneRObjs[i].Done)
                            return false;
                    }
                    return true;
                }

                set
                {
                    for (int i = 0; i < oneRObjs.Count; i++)
                    {
                        oneRObjs[i].Done = value;
                    }
                }
            }

            public string NGText
            {
                get
                {
                    string data = "";
                    foreach (var item in oneRObjs)
                    {
                        if (!item.Done)
                        {
                            return item.NGText;
                        }
                        data += item.NGText + ";";
                    }
                    return data.Trim(';');
                }
            }

            public string CNGText
            {
                get
                {
                    string data = "";
                    foreach (var item in oneRObjs)
                    {
                        if (!item.Done)
                        {
                            return item.CNGText;
                        }
                        data += item.CNGText + ";";
                    }
                    return data.Trim(';');
                }
            }

            /// <summary>
            ///
            /// </summary>
            public List<string> RestStrings
            {
                get
                {
                    //List<string> vs = new List<string>();
                    foreach (var item in oneRObjs)
                    {
                        for (int i = 0; i < item.RestStrings.Count; i++)
                        {
                            if (!restStrings.Contains(item.RestStrings[i]))
                            {
                                restStrings.Add(item.RestStrings[i]);
                            }
                        }
                    }
                    return restStrings;
                }
                set { restStrings = value; }
            }

            /// <summary>
            ///
            /// </summary>
            private List<string> restStrings = new List<string>();

            /// <summary>
            ///
            /// </summary>
            public string RestText
            {
                get
                {
                    string data = "";
                    if (aOK)
                    {
                        data = "OK;";
                    }
                    foreach (var item in oneRObjs)
                    {
                        if (!item.Done)
                        {
                            return item.RestText;
                        }
                        data += item.RestText + ";";
                    }
                    return data.Trim(';');
                }
            }

            public HObject NGROI
            {
                get
                {
                    foreach (var item in oneRObjs)
                    {
                        if (!item.Done)
                        {
                            return item.NGROI;
                        }
                    }
                    HObject hObject = new HObject();
                    hObject.GenEmptyObj();
                    return hObject;
                }
            }

            public HObject ROI
            {
                get
                {
                    foreach (var item in oneRObjs)
                    {
                        if (!item.Done)
                        {
                            return item.ROI;
                        }
                    }
                    HObject hObject = new HObject();
                    hObject.GenEmptyObj();
                    return hObject;
                }
            }

            public void RAddOK()
            {
                foreach (var item in oneRObjs)
                {
                    if (!item.Done)
                    {
                        item.RAddOK();
                        //item.OK = true;
                        //item.RestText = "OK";
                        //item.Done = true;
                        break;
                    }
                }
            }

            public void RAddNG(string restText)
            {
                foreach (var item in oneRObjs)
                {
                    if (!item.Done)
                    {
                        item.RAddNG(restText);
                        //item.OK = false;
                        //item.RestText = restText;
                        //item.Done = true;
                        break;
                    }
                }
            }
        }
    }
}