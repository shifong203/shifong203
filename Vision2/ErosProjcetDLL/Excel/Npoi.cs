using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Vision2.ErosProjcetDLL.Excel
{
    public class Npoi
    {
        /// <summary>
        /// 读取Excel
        /// </summary>
        /// <param name="filePath"></param>
        public static DataTable ReadFromExcelFile(string filePath, string sheetNmae)
        {
            IWorkbook wk = null;
            DataTable data = new DataTable();
            string extension = System.IO.Path.GetExtension(filePath);
            try
            {
                FileStream fs = File.OpenRead(filePath);
                if (extension.Equals(".xls"))
                {
                    //把xls文件中的数据写入wk中
                    wk = new HSSFWorkbook(fs);
                }
                else
                {
                    //把xlsx文件中的数据写入wk中
                    wk = new XSSFWorkbook(fs);
                }
                fs.Close();
                //读取当前表数据
                ISheet sheet = wk.GetSheet(sheetNmae);

                IRow row = sheet.GetRow(0);  //读取当前行数据
                if (row != null)
                {
                    for (int j = 0; j < row.LastCellNum; j++)
                    {
                        //读取该行的第j列数据
                        DataColumn column = new DataColumn();
                        string value = row.GetCell(j).ToString();
                        column.ColumnName = value;
                        data.Columns.Add(column);
                    }
                }
                //LastRowNum 是当前表的总行数-1（注意）
                //int offset = 0;
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    row = sheet.GetRow(i);  //读取当前行数据
                    DataRow dataRow = data.NewRow();
                    if (row != null)
                    {
                        //LastCellNum 是当前行的总列数
                        for (int j = 0; j < row.LastCellNum; j++)
                        {
                            //读取该行的第j列数据
                            if (j == 19)
                            {

                            }
                            dataRow[i] = row.GetCell(j);
                        }
                        data.Rows.Add(dataRow);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("读取Excel错误：" + e.Message);
            }
            return data;
        }
        /// <summary>
        /// 读取Excel表格指定的表单文件,64位软件读取时第一行数据作为列名；
        /// </summary>
        /// <param name="filePath">文件地址</param>
        /// <param name="sheetName">表名</param>
        /// <returns></returns>
        public static DataTable ReadExcelFile(string filePath, string sheetName)
        {
            DataTable dt = new DataTable();
            return ImportExcel(filePath, sheetName);
        }     /// <summary>
              /// 读取Excel表格指定的表单文件,64位软件读取时第一行数据作为列名；
              /// </summary>
              /// <param name="filePath">文件地址</param>
              /// <param name="sheetName">表名</param>
              /// <returns></returns>
        public static DataTable ReadExcelFile(string filePath, int sheetSel)
        {
            DataTable dt = new DataTable();
            return ImportExcel(filePath, sheetSel);
        }

        /// <summary>
        /// 根据Excel格式读取Excel
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="type">Excel格式枚举类型，xls/xlsx</param>
        /// <param name="sheetName">表名，默认取第一张</param>
        /// <returns>DataTable</returns>
        private static DataTable ImportExcel(string filePath, string sheetName)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            Stream stream = File.OpenRead(filePath);
            DataTable dt = new DataTable();
            IWorkbook workbook;
            try
            {
                //xls使用HSSFWorkbook类实现，xlsx使用XSSFWorkbook类实现
                if (filePath.EndsWith(".xls"))
                {
                    //把xls文件中的数据写入wk中
                    workbook = new HSSFWorkbook(stream);
                }
                else
                {
                    //把xlsx文件中的数据写入wk中
                    workbook = new XSSFWorkbook(stream);
                }

                stream.Close();
                stream.Dispose();
                ISheet sheet = null;
                //获取工作表 默认取第一张
                if (string.IsNullOrWhiteSpace(sheetName))
                    sheet = workbook.GetSheetAt(0);
                else
                    sheet = workbook.GetSheet(sheetName);

                if (sheet == null)
                    return null;
                IEnumerator rows = sheet.GetRowEnumerator();
                #region 获取表头
                IRow headerRow = sheet.GetRow(0);
                int cellCount = headerRow.LastCellNum;
                for (int j = 0; j < cellCount; j++)
                {
                    ICell cell = headerRow.GetCell(j);
                    if (cell != null)
                    {
                        dt.Columns.Add(cell.ToString());
                    }
                    else
                    {
                        dt.Columns.Add("");
                    }
                }
                #endregion
                #region 获取内容
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    DataRow dataRow = dt.NewRow();

                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            //判断单元格是否为日期格式
                            if (row.GetCell(j).CellType == NPOI.SS.UserModel.CellType.Numeric && HSSFDateUtil.IsCellDateFormatted(row.GetCell(j)))
                            {
                                if (row.GetCell(j).DateCellValue.Year >= 1970)
                                {
                                    dataRow[j] = row.GetCell(j).DateCellValue.ToString();
                                }
                                else
                                {
                                    dataRow[j] = row.GetCell(j).ToString();

                                }
                            }
                            else
                            {
                                dataRow[j] = row.GetCell(j).ToString();
                            }
                        }
                    }
                    dt.Rows.Add(dataRow);
                }
                #endregion

            }
            catch (Exception ex)
            {
                dt = null;
                ErosProjcetDLL.Project.AlarmText.AddTextNewLine(ex.Message);
            }
            finally
            {
                //if (stream != null)
                //{
                //    stream.Close();
                //    stream.Dispose();
                //}
            }
            return dt;
        }

        /// <summary>
        /// 根据Excel格式读取Excel
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <param name="type">Excel格式枚举类型，xls/xlsx</param>
        /// <param name="sheetName">表名，默认取第一张</param>
        /// <returns>DataTable</returns>
        private static DataTable ImportExcel(string filePath, int sheetSele)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            Stream stream = File.OpenRead(filePath);
            DataTable dt = new DataTable();
            IWorkbook workbook;
            try
            {
                //xls使用HSSFWorkbook类实现，xlsx使用XSSFWorkbook类实现
                if (filePath.EndsWith(".xls"))
                {
                    //把xls文件中的数据写入wk中
                    workbook = new HSSFWorkbook(stream);
                }
                else
                {
                    //把xlsx文件中的数据写入wk中
                    workbook = new XSSFWorkbook(stream);
                }

                stream.Close();
                stream.Dispose();
                ISheet sheet = null;
                //获取工作表 默认取第一张
                 sheet = workbook.GetSheetAt(sheetSele);
                if (sheet == null)
                    return null;
                IEnumerator rows = sheet.GetRowEnumerator();
                #region 获取表头
                IRow headerRow = sheet.GetRow(0);
                int cellCount = headerRow.LastCellNum;
                for (int j = 0; j < cellCount; j++)
                {
                    ICell cell = headerRow.GetCell(j);
                    if (cell != null)
                    {
                        dt.Columns.Add(cell.ToString());
                    }
                    else
                    {
                        dt.Columns.Add("");
                    }
                }
                #endregion
                #region 获取内容
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    DataRow dataRow = dt.NewRow();

                    for (int j = row.FirstCellNum; j < cellCount; j++)
                    {
                        if (row.GetCell(j) != null)
                        {
                            //判断单元格是否为日期格式
                            if (row.GetCell(j).CellType == NPOI.SS.UserModel.CellType.Numeric && HSSFDateUtil.IsCellDateFormatted(row.GetCell(j)))
                            {
                                if (row.GetCell(j).DateCellValue.Year >= 1970)
                                {
                                    dataRow[j] = row.GetCell(j).DateCellValue.ToString();
                                }
                                else
                                {
                                    dataRow[j] = row.GetCell(j).ToString();

                                }
                            }
                            else
                            {
                                dataRow[j] = row.GetCell(j).ToString();
                            }
                        }
                    }
                    dt.Rows.Add(dataRow);
                }
                #endregion

            }
            catch (Exception ex)
            {
                dt = null;
            }
            finally
            {
                //if (stream != null)
                //{
                //    stream.Close();
                //    stream.Dispose();
                //}
            }
            return dt;
        }
        /// <summary>
        /// 读取文本文件
        /// </summary>
        /// <param name="filePath">地址</param>
        /// <param name="text">读取的文本</param>
        /// <returns>成功返回Ture</returns>
        public static bool ReadText(string filePath, out string text)
        {
            text = "";
            try
            {
                if (File.Exists(filePath))
                {
                    text = File.ReadAllText(filePath);
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }
        /// <summary>
        /// 读取文本文件
        /// </summary>
        /// <param name="filePath">地址</param>
        /// <param name="text">读取的文本</param>
        /// <returns>成功返回Ture</returns>
        public static bool ReadText(string filePath, out List<string> text)
        {
            text = new List<string>();
            try
            {
                if (File.Exists(filePath))
                {
                    text.AddRange(File.ReadAllLines(filePath));
                    return true;
                }
            }
            catch (Exception)
            {
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="text"></param>
        public static void AddWriteCSV(string path, params string[] text)
        {//保存参数记录

            if (!path.Contains("."))
            {
                path = path + ".CSV";
            }
            System.IO.Directory.CreateDirectory(Path.GetDirectoryName(path));
            try
            {
                //创建文件流写入对象,绑定文件流对象
                //创建数据对象
                StringBuilder sb = new StringBuilder();
                string[] ste = new string[] { };
                if (System.IO.File.Exists(path))//文件不存在时,创建新文件,并写入文件标题
                {
                    ste = File.ReadAllLines(path);
                }
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                //创建文件流对象
                string tet = "";
                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i] == "")
                    {
                        text[i] = " ";
                    }
                    tet += text[i] + ',';
                }
                for (int i = 0; i < ste.Length; i++)
                {
                    sb.AppendLine(ste[i]);
                }

                //sb.Append("RunTime").Append(",").Append("BarCode").Append(",").Append("OverStation");
                sb.AppendLine(tet);

                //把标题内容写入到文件流中
                sw.Write(sb);
                sw.Flush();
                sw.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
            }
        }

        public static void AddText(String path, params string[] text)
        {
            System.IO.Directory.CreateDirectory(Path.GetDirectoryName(path));
            try
            {
                if (!path.Contains("."))
                {
                    path = path + ".txt";
                }
                //创建文件流写入对象,绑定文件流对象
                //创建数据对象

                using (StreamWriter fs = new StreamWriter(path, true))
                {
                    for (int i = 0; i < text.Length; i++)
                    {
                        fs.Write(text[i]);
                    }

                }

                //StringBuilder sb = new StringBuilder();
                //string[] ste = new string[] { };
                //if (System.IO.File.Exists(path))//文件不存在时,创建新文件,并写入文件标题
                //{
                //    ste = File.ReadAllLines(path);
                //}
                //FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                //StreamWriter sw = new StreamWriter(fs, Encoding.UTF8); 

                //for (int i = 0; i < ste.Length; i++)
                //{
                //    sb.AppendLine(ste[i]);
                //}
                ////创建文件流对象
                //string tet = "";
                ////sb.Append("RunTime").Append(",").Append("BarCode").Append(",").Append("OverStation");


                ////把标题内容写入到文件流中
                //sw.Write(sb);
                //sw.Flush();
                //sw.Close();
                //fs.Close();
            }
            catch (Exception ex)
            {
            }
        }
        public static void AddTextLine(String path, params string[] text)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            try
            {
                //创建文件流写入对象,绑定文件流对象
                //创建数据对象
                using (StreamWriter fs = new StreamWriter(path, true))
                {
                    string ted = "";
                    for (int i = 0; i < text.Length; i++)
                    {
                        ted += text[i] + ',';
                    }
                    fs.WriteLine(ted.TrimEnd(','));
                }
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="text"></param>
        /// <param name="Extension"></param>
        public static void WriteF(string path, List<string> text, string Extension = ".CSV")
        {
            if (!path.EndsWith(Extension))
            {
                path = path + Extension;
            }

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            System.IO.Directory.CreateDirectory(Path.GetDirectoryName(path));
            try
            {
                //创建文件流写入对象,绑定文件流对象
                //创建数据对象
                StringBuilder sb = new StringBuilder();
                string[] ste = new string[] { };
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                //创建文件流对象
                string tet = "";
                for (int i = 0; i < ste.Length; i++)
                {
                    sb.AppendLine(ste[i]);
                }
                for (int i = 0; i < text.Count; i++)
                {
                    sb.AppendLine(text[i]);
                }
                //把标题内容写入到文件流中
                sw.Write(sb);
                sw.Flush();
                sw.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
            }
        }
        /// <summary>
        /// 读取Excel表格
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static DataTable ReadExcelFile(string filePath)
        {
            DataTable dt = new DataTable();
            List<string> listCoumnsName = new List<string>();
            try
            {
                //this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                string strConn = "";
                if (!File.Exists(filePath))
                {
                    return null;
                }
                if (IntPtr.Size == 4)
                {
                    // 32-bit
                    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + "Extended Properties=Excel 8.0;";
                }
                else if (IntPtr.Size == 8)
                {
                    strConn = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + filePath + ";Extended Properties='Excel 12.0; HDR=NO; IMEX=1'"; //此連接可以操作.xls與.xlsx文件
                }
                if (System.IO.Path.GetExtension(filePath).ToLower().Contains(".xls"))
                {
                    //打开Excel的连接，设置连接对象
                    OleDbConnection conne = new OleDbConnection(strConn);
                    conne.Open();
                    DataTable sheetNames = conne.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                    foreach (DataRow dr in sheetNames.Rows)
                    {
                        try
                        {
                            //当前选中的工作表前几行数据，获取数据列
                            OleDbDataAdapter oada3 = new OleDbDataAdapter("select top 5 * from [" + dr[2].ToString() + "]", strConn);
                            DataTable ds = new DataTable();
                            oada3.Fill(ds);
                            //将列加载到树节点上
                            for (int i = 0; i < ds.Columns.Count; i++)
                            {
                                listCoumnsName.Add(ds.Columns[i].ColumnName.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        //存储需要查询数据的列
                        string strName = "";
                        //遍历列名，每个列名用逗号隔开
                        for (int i = 0; i < listCoumnsName.Count; i++)
                        {
                            strName = strName + "[" + listCoumnsName[i] + "],";
                        }
                        strName = strName.Substring(0, strName.Length - 1);
                        //获取用户没有删掉留下来的列，读取这些列的数据
                        //建立Excel连接
                        //读取数据
                        OleDbDataAdapter oada = new OleDbDataAdapter("select  " + strName + " from [" + dr[2].ToString() + "]", strConn);
                        //填入DataTable
                        oada.Fill(dt);
                        if (IntPtr.Size == 8)
                        {
                            for (int i = 0; i < listCoumnsName.Count; i++)
                            {
                                if (listCoumnsName[i] != null)
                                {
                                    dt.Columns[listCoumnsName[i]].ColumnName = dt.Rows[0][listCoumnsName[i]].ToString();
                                }
                            }
                            dt.Rows.RemoveAt(0);
                        }

                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("读取Excel错误：" + e.Message);
            }
            return dt;
        }

        public static bool GetTables(OleDbConnection conn)
        {
            int result = 0;
            DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });


            if (schemaTable != null)
            {

                for (Int32 row = 0; row < schemaTable.Rows.Count; row++)
                {
                    string col_name = schemaTable.Rows[row]["TABLE_NAME"].ToString();
                    if (col_name == "MyChooseStock")
                    {
                        result++;
                    }
                }
            }
            if (result == 0)
                return false;
            return true;
        }

        /// <summary>
        /// 获取cell的数据，并设置为对应的数据类型
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public object GetCellValue(ICell cell)
        {
            object value = null;
            try
            {
                if (cell.CellType != CellType.Blank)
                {
                    switch (cell.CellType)
                    {
                        case CellType.Numeric:
                            // Date comes here
                            if (DateUtil.IsCellDateFormatted(cell))
                            {
                                value = cell.DateCellValue;
                            }
                            else
                            {
                                // Numeric type
                                value = cell.NumericCellValue;
                            }
                            break;

                        case CellType.Boolean:
                            // Boolean type
                            value = cell.BooleanCellValue;
                            break;

                        case CellType.Formula:
                            value = cell.CellFormula;
                            break;

                        default:
                            // String type
                            value = cell.StringCellValue;
                            break;
                    }
                }
            }
            catch (Exception)
            {
                value = "";
            }

            return value;
        }
        //根据数据类型设置不同类型的cell
        public static void SetCellValue(ICell cell, object obj)
        {


            if (obj == null)
            {
                cell.SetCellValue(string.Empty);
                return;
            }

            if (double.TryParse(obj.ToString(), out double ds))
            {
                cell.SetCellValue(ds);
            }
            else if (obj.GetType() == typeof(int))
            {
                cell.SetCellValue((int)obj);
            }
            //else if (obj.GetType() == typeof(double))
            //{
            //    cell.SetCellValue((double)obj);
            //}
            else if (obj.GetType() == typeof(IRichTextString))
            {
                cell.SetCellValue((IRichTextString)obj);
            }
            else if (obj.GetType() == typeof(string))
            {
                cell.SetCellValue(obj.ToString());
            }
            else if (obj.GetType() == typeof(DateTime))
            {
                cell.SetCellValue((DateTime)obj);
            }
            else if (obj.GetType() == typeof(bool))
            {
                cell.SetCellValue((bool)obj);
            }
            else
            {
                cell.SetCellValue(obj.ToString());
            }
        }

        /// <summary>
        /// 写行到Excle，不存在则创建
        /// </summary>
        /// <param name="filePath">地址</param>
        /// <param name="sheetName">表名</param>
        /// <param name="Values">参数</param>
        public static void AddRosWriteToExcel(string filePath, string sheetName, params object[] Values)
        {
            //创建工作薄
            try
            {
                if (!filePath.Contains("."))
                {
                    filePath += ".xls";
                }
                IWorkbook wb;
                string extension = System.IO.Path.GetExtension(filePath);
                if (File.Exists(filePath))
                {
                    FileStream fs = File.OpenRead(filePath);
                    //根据指定的文件格式创建对应的类
                    if (extension.Equals(".xls"))
                    {
                        wb = new HSSFWorkbook(fs);
                    }
                    else
                    {
                        wb = new XSSFWorkbook(fs);
                    }
                    fs.Close();
                }
                else
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    if (extension.Equals(".xls"))
                    {
                        wb = new HSSFWorkbook();
                    }
                    else
                    {
                        wb = new XSSFWorkbook();
                    }
                }
                //ICellStyle style1 = wb.CreateCellStyle();//样式
                //style1.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;//文字水平对齐方式
                //style1.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;//文字垂直对齐方式
                //设置边框
                //style1.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                //style1.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                //style1.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                //style1.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                //style1.WrapText = true;//自动换行
                //ICellStyle style2 = wb.CreateCellStyle();//样式
                //IFont font1 = wb.CreateFont();//字体
                //font1.FontName = "楷体";
                //font1.Color = HSSFColor.Red.Index;//字体颜色
                //font1.Boldweight = (short)FontBoldWeight.Normal;//字体加粗样式
                //style2.SetFont(font1);//样式里的字体设置具体的字体样式
                //设置背景色
                //style2.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Yellow.Index;
                //style2.FillPattern = FillPattern.SolidForeground;
                //style2.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Yellow.Index;
                //style2.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;//文字水平对齐方式
                //style2.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;//文字垂直对齐方式

                //ICellStyle dateStyle = wb.CreateCellStyle();//样式
                //dateStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;//文字水平对齐方式
                //dateStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;//文字垂直对齐方式
                //设置数据显示格式
                //IDataFormat dataFormatCustom = wb.CreateDataFormat();
                //dateStyle.DataFormat = dataFormatCustom.GetFormat("yyyy-MM-dd HH:mm:ss");
                //创建一个表单
                ISheet sheet = wb.GetSheet(sheetName);
                if (sheet == null)
                {
                    sheet = wb.CreateSheet(sheetName);
                    //设置列宽
                    int[] columnWidth = { 10, 10, 10, 10 };
                    for (int i = 0; i < columnWidth.Length; i++)
                    {
                        //设置列宽度，256*字符数，因为单位是1/256个字符
                        sheet.SetColumnWidth(i, 256 * columnWidth[i]);
                    }
                }
                //日期可以直接传字符串，NPOI会自动识别//如果是DateTime类型，则要设置CellStyle.DataFormat，否则会显示为数字
                IRow row;
                ICell cell;
                row = sheet.CreateRow(sheet.LastRowNum + 1);//创建第i行
                if (Values.Length == 1)
                {
                    double[] vs = Values[0] as double[];

                    if (vs != null)
                    {
                        for (int j = 0; j < vs.Length; j++)
                        {
                            cell = row.CreateCell(j);//创建第j列
                                                     //cell.CellStyle = j % 2 == 0 ? style1 : style2;
                                                     //根据数据类型设置不同类型的cell
                                                     //object obj = data[i, j];
                            SetCellValue(cell, vs[j]);
                            //如果是日期，则设置日期显示的格式
                            //if (obj.GetType() == typeof(DateTime))
                            //{
                            ////    cell.CellStyle = dateStyle;
                            //}
                            //如果要根据内容自动调整列宽，需要先setCellValue再调用
                            sheet.AutoSizeColumn(j);
                        }
                    }
                    else
                    {
                        Single[] vss = Values[0] as Single[];
                        if (vss != null)
                        {
                            for (int j = 0; j < vss.Length; j++)
                            {
                                cell = row.CreateCell(j);//创建第j列
                                                         //cell.CellStyle = j % 2 == 0 ? style1 : style2;
                                                         //根据数据类型设置不同类型的cell
                                                         //object obj = data[i, j];
                                SetCellValue(cell, vss[j]);
                                //如果是日期，则设置日期显示的格式
                                //if (obj.GetType() == typeof(DateTime))
                                //{
                                ////    cell.CellStyle = dateStyle;
                                //}
                                //如果要根据内容自动调整列宽，需要先setCellValue再调用
                                sheet.AutoSizeColumn(j);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < Values.Length; j++)
                            {
                                cell = row.CreateCell(j);//创建第j列
                                                         //cell.CellStyle = j % 2 == 0 ? style1 : style2;
                                                         //根据数据类型设置不同类型的cell
                                                         //object obj = data[i, j];
                                SetCellValue(cell, Values[j]);
                                //如果是日期，则设置日期显示的格式
                                //if (obj.GetType() == typeof(DateTime))
                                //{
                                ////    cell.CellStyle = dateStyle;
                                //}
                                //如果要根据内容自动调整列宽，需要先setCellValue再调用
                                sheet.AutoSizeColumn(j);
                            }

                        }

                    }
                }
                else
                {
                    for (int j = 0; j < Values.Length; j++)
                    {
                        cell = row.CreateCell(j);//创建第j列
                                                 //cell.CellStyle = j % 2 == 0 ? style1 : style2;
                                                 //根据数据类型设置不同类型的cell
                                                 //object obj = data[i, j];
                        SetCellValue(cell, Values[j]);
                        //如果是日期，则设置日期显示的格式
                        //if (obj.GetType() == typeof(DateTime))
                        //{
                        ////    cell.CellStyle = dateStyle;
                        //}
                        //如果要根据内容自动调整列宽，需要先setCellValue再调用
                        sheet.AutoSizeColumn(j);
                    }
                }

                //合并单元格，如果要合并的单元格中都有数据，只会保留左上角的
                //CellRangeAddress(0, 2, 0, 0)，合并0-2行，0-0列的单元格
                //CellRangeAddress region = new CellRangeAddress(0, 0, 0, 0);
                //sheet.AddMergedRegion(region);
                FileStream fst = File.OpenWrite(filePath);
                wb.Write(fst);//向打开的这个Excel文件中写入表单并保存。
                fst.Close();
            }
            catch (Exception ex)
            {
                throw (new Exception("写数据错误" + ex.Message));

            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="sheetName"></param>
        /// <param name="Values"></param>
        public static void AddRosWriteToExcel(string filePath, string sheetName, params double[] Values)
        {
            //创建工作薄
            try
            {
                if (!filePath.Contains("."))
                {
                    filePath += ".xls";
                }

                IWorkbook wb;
                string extension = System.IO.Path.GetExtension(filePath);
                if (File.Exists(filePath))
                {
                    FileStream fs = File.OpenRead(filePath);
                    //根据指定的文件格式创建对应的类
                    if (extension.Equals(".xls"))
                    {
                        wb = new HSSFWorkbook(fs);
                    }
                    else
                    {
                        wb = new XSSFWorkbook(fs);
                    }
                    fs.Close();
                }
                else
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    if (extension.Equals(".xls"))
                    {
                        wb = new HSSFWorkbook();
                    }
                    else
                    {
                        wb = new XSSFWorkbook();
                    }
                }

                //创建一个表单
                ISheet sheet = wb.GetSheet(sheetName);
                if (sheet == null)
                {
                    sheet = wb.CreateSheet(sheetName);
                    //设置列宽
                    int[] columnWidth = { 10, 10, 10, 10 };
                    for (int i = 0; i < columnWidth.Length; i++)
                    {
                        //设置列宽度，256*字符数，因为单位是1/256个字符
                        sheet.SetColumnWidth(i, 256 * columnWidth[i]);
                    }
                }
                //日期可以直接传字符串，NPOI会自动识别//如果是DateTime类型，则要设置CellStyle.DataFormat，否则会显示为数字
                IRow row;
                ICell cell;
                row = sheet.CreateRow(sheet.LastRowNum + 1);//创建第i行
                for (int j = 0; j < Values.Length; j++)
                {
                    cell = row.CreateCell(j);//创建第j列
                                             //cell.CellStyle = j % 2 == 0 ? style1 : style2;
                                             //根据数据类型设置不同类型的cell
                                             //object obj = data[i, j];
                    SetCellValue(cell, Values[j]);
                    //如果是日期，则设置日期显示的格式
                    //if (obj.GetType() == typeof(DateTime))
                    //{
                    ////    cell.CellStyle = dateStyle;
                    //}
                    //如果要根据内容自动调整列宽，需要先setCellValue再调用
                    sheet.AutoSizeColumn(j);
                }
                //合并单元格，如果要合并的单元格中都有数据，只会保留左上角的
                //CellRangeAddress(0, 2, 0, 0)，合并0-2行，0-0列的单元格
                //CellRangeAddress region = new CellRangeAddress(0, 0, 0, 0);
                //sheet.AddMergedRegion(region);
                FileStream fst = File.OpenWrite(filePath);
                wb.Write(fst);//向打开的这个Excel文件中写入表单并保存。
                fst.Close();
            }
            catch (Exception ex)
            {
                throw (new Exception("写数据错误" + ex.Message));
            }
        }
        public static void AddRosSWriteToExcel(string filePath, string sheetName, List<double[]> Values)
        {
            //创建工作薄
            try
            {
                if (!filePath.Contains("."))
                {
                    filePath += ".xls";
                }

                IWorkbook wb;
                string extension = System.IO.Path.GetExtension(filePath);
                if (File.Exists(filePath))
                {
                    FileStream fs = File.OpenRead(filePath);
                    //根据指定的文件格式创建对应的类
                    if (extension.Equals(".xls"))
                    {
                        wb = new HSSFWorkbook(fs);
                    }
                    else
                    {
                        wb = new XSSFWorkbook(fs);
                    }
                    fs.Close();
                }
                else
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    if (extension.Equals(".xls"))
                    {
                        wb = new HSSFWorkbook();
                    }
                    else
                    {
                        wb = new XSSFWorkbook();
                    }
                }

                //创建一个表单
                ISheet sheet = wb.GetSheet(sheetName);
                if (sheet == null)
                {
                    sheet = wb.CreateSheet(sheetName);
                    //设置列宽
                    int[] columnWidth = { 10, 10, 10, 10 };
                    for (int i = 0; i < columnWidth.Length; i++)
                    {
                        //设置列宽度，256*字符数，因为单位是1/256个字符
                        sheet.SetColumnWidth(i, 256 * columnWidth[i]);
                    }
                }
                //日期可以直接传字符串，NPOI会自动识别//如果是DateTime类型，则要设置CellStyle.DataFormat，否则会显示为数字
                IRow row;
                ICell cell;

                for (int i = 0; i < Values.Count; i++)
                {
                    row = sheet.CreateRow(sheet.LastRowNum + 1);//创建第i行
                    for (int j = 0; j < Values[i].Length; j++)
                    {
                        cell = row.CreateCell(j);//创建第j列
                                                 //cell.CellStyle = j % 2 == 0 ? style1 : style2;
                                                 //根据数据类型设置不同类型的cell
                                                 //object obj = data[i, j];
                        SetCellValue(cell, Values[i][j]);
                        //如果是日期，则设置日期显示的格式
                        //if (obj.GetType() == typeof(DateTime))
                        //{
                        ////    cell.CellStyle = dateStyle;
                        //}
                        //如果要根据内容自动调整列宽，需要先setCellValue再调用
                        sheet.AutoSizeColumn(j);
                    }
                }
                //合并单元格，如果要合并的单元格中都有数据，只会保留左上角的
                //CellRangeAddress(0, 2, 0, 0)，合并0-2行，0-0列的单元格
                //CellRangeAddress region = new CellRangeAddress(0, 0, 0, 0);
                //sheet.AddMergedRegion(region);
                FileStream fst = File.OpenWrite(filePath);
                wb.Write(fst);//向打开的这个Excel文件中写入表单并保存。
                fst.Close();
            }
            catch (Exception ex)
            {
                throw (new Exception("写数据错误" + ex.Message));
            }

        }
        /// <summary>
        /// 写行到Excle，不存在则创建
        /// </summary>
        /// <param name="filePath">地址</param>
        /// <param name="sheetName">表名</param>
        /// <param name="Values">参数</param>
        public static void AddRosSWriteToExcel(string filePath, string sheetName, List<object[]> Values)
        {
            //创建工作薄
            try
            {
                if (!filePath.Contains("."))
                {
                    filePath += ".xls";
                }
                IWorkbook wb;
                string extension = System.IO.Path.GetExtension(filePath);
                if (File.Exists(filePath))
                {
                    FileStream fs = File.OpenRead(filePath);
                    //根据指定的文件格式创建对应的类
                    if (extension.Equals(".xls"))
                    {
                        wb = new HSSFWorkbook(fs);
                    }
                    else
                    {
                        wb = new XSSFWorkbook(fs);
                    }
                    fs.Close();
                }
                else
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    if (extension.Equals(".xls"))
                    {
                        wb = new HSSFWorkbook();
                    }
                    else
                    {
                        wb = new XSSFWorkbook();
                    }
                }
                //ICellStyle style1 = wb.CreateCellStyle();//样式
                //style1.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;//文字水平对齐方式
                //style1.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;//文字垂直对齐方式
                //设置边框
                //style1.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                //style1.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                //style1.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                //style1.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                //style1.WrapText = true;//自动换行
                //ICellStyle style2 = wb.CreateCellStyle();//样式
                //IFont font1 = wb.CreateFont();//字体
                //font1.FontName = "楷体";
                //font1.Color = HSSFColor.Red.Index;//字体颜色
                //font1.Boldweight = (short)FontBoldWeight.Normal;//字体加粗样式
                //style2.SetFont(font1);//样式里的字体设置具体的字体样式
                //设置背景色
                //style2.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Yellow.Index;
                //style2.FillPattern = FillPattern.SolidForeground;
                //style2.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.Yellow.Index;
                //style2.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;//文字水平对齐方式
                //style2.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;//文字垂直对齐方式

                //ICellStyle dateStyle = wb.CreateCellStyle();//样式
                //dateStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;//文字水平对齐方式
                //dateStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;//文字垂直对齐方式
                //设置数据显示格式
                //IDataFormat dataFormatCustom = wb.CreateDataFormat();
                //dateStyle.DataFormat = dataFormatCustom.GetFormat("yyyy-MM-dd HH:mm:ss");
                //创建一个表单
                ISheet sheet = wb.GetSheet(sheetName);
                if (sheet == null)
                {
                    sheet = wb.CreateSheet(sheetName);
                    //设置列宽
                    int[] columnWidth = { 10, 10, 10, 10 };
                    for (int i = 0; i < columnWidth.Length; i++)
                    {
                        //设置列宽度，256*字符数，因为单位是1/256个字符
                        sheet.SetColumnWidth(i, 256 * columnWidth[i]);
                    }
                }
                //日期可以直接传字符串，NPOI会自动识别//如果是DateTime类型，则要设置CellStyle.DataFormat，否则会显示为数字
                IRow row;
                ICell cell;
                for (int i = 0; i < Values.Count; i++)
                {


                    row = sheet.CreateRow(sheet.LastRowNum + 1);//创建第i行

                    if (Values[i].Length == 1)
                    {
                        double[] vs = Values[i][0] as double[];
                        if (vs != null)
                        {
                            for (int j = 0; j < vs.Length; j++)
                            {
                                cell = row.CreateCell(j);//创建第j列
                                                         //cell.CellStyle = j % 2 == 0 ? style1 : style2;
                                                         //根据数据类型设置不同类型的cell
                                                         //object obj = data[i, j];
                                SetCellValue(cell, vs[j]);
                                //如果是日期，则设置日期显示的格式
                                //if (obj.GetType() == typeof(DateTime))
                                //{
                                ////    cell.CellStyle = dateStyle;
                                //}
                                //如果要根据内容自动调整列宽，需要先setCellValue再调用
                                sheet.AutoSizeColumn(j);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < Values[i].Length; j++)
                            {
                                cell = row.CreateCell(j);//创建第j列
                                                         //cell.CellStyle = j % 2 == 0 ? style1 : style2;
                                                         //根据数据类型设置不同类型的cell
                                                         //object obj = data[i, j];
                                SetCellValue(cell, Values[i][j]);
                                //如果是日期，则设置日期显示的格式
                                //if (obj.GetType() == typeof(DateTime))
                                //{
                                ////    cell.CellStyle = dateStyle;
                                //}
                                //如果要根据内容自动调整列宽，需要先setCellValue再调用
                                sheet.AutoSizeColumn(j);
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < Values[i].Length; j++)
                        {
                            cell = row.CreateCell(j);//创建第j列
                                                     //cell.CellStyle = j % 2 == 0 ? style1 : style2;
                                                     //根据数据类型设置不同类型的cell
                                                     //object obj = data[i, j];
                            SetCellValue(cell, Values[i][j]);
                            //如果是日期，则设置日期显示的格式
                            //if (obj.GetType() == typeof(DateTime))
                            //{
                            ////    cell.CellStyle = dateStyle;
                            //}
                            //如果要根据内容自动调整列宽，需要先setCellValue再调用
                            sheet.AutoSizeColumn(j);
                        }
                    }
                }
                //合并单元格，如果要合并的单元格中都有数据，只会保留左上角的
                //CellRangeAddress(0, 2, 0, 0)，合并0-2行，0-0列的单元格
                //CellRangeAddress region = new CellRangeAddress(0, 0, 0, 0);
                //sheet.AddMergedRegion(region);
                FileStream fst = File.OpenWrite(filePath);
                wb.Write(fst);//向打开的这个Excel文件中写入表单并保存。
                fst.Close();
            }
            catch (Exception ex)
            {
                throw (new Exception("写数据错误" + ex.Message));
            }

        }
        /// <summary>
        /// 更改列名不存在则创建
        /// </summary>
        /// <param name="filePath">地址</param>
        /// <param name="sheetName">表名称</param>
        /// <param name="columnNames">列名</param>
        public static void AddWriteColumnToExcel(string filePath, string sheetName, params string[] columnNames)
        {
            //创建工作薄
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                if (!filePath.Contains("."))
                {
                    filePath += ".xls";
                }
                IWorkbook wb;
                string extension = System.IO.Path.GetExtension(filePath);
                if (File.Exists(filePath))
                {
                    FileStream fs = File.OpenRead(filePath);
                    //根据指定的文件格式创建对应的类
                    if (extension.Equals(".xls"))
                    {
                        wb = new HSSFWorkbook(fs);
                    }
                    else
                    {
                        wb = new XSSFWorkbook(fs);
                    }
                    fs.Close();
                }
                else
                {
                    if (extension.Equals(".xls"))
                    {
                        wb = new HSSFWorkbook();
                    }
                    else
                    {
                        wb = new XSSFWorkbook();
                    }
                }
                ISheet sheet = wb.GetSheet(sheetName);
                //创建一个表单
                if (sheet == null)
                {
                    sheet = wb.CreateSheet(sheetName);
                    //设置列宽
                    int[] columnWidth = { 10, 10, 10, 10 };
                    for (int i = 0; i < columnWidth.Length; i++)
                    {
                        //设置列宽度，256*字符数，因为单位是1/256个字符
                        sheet.SetColumnWidth(i, 256 * columnWidth[i]);
                    }
                }

                //测试数据
                int rowCount = 1;
                //object[,] data = {
                //   //      {"名称", "结果", "最大误差", "边界数量"},  //日期可以直接传字符串，NPOI会自动识别//如果是DateTime类型，则要设置CellStyle.DataFormat，否则会显示为数字
                //};

                IRow row;
                ICell cell;
                row = sheet.CreateRow(0);//创建第i行
                for (int j = 0; j < columnNames.Length; j++)
                {
                    cell = row.CreateCell(j);//创建第j列
                    SetCellValue(cell, columnNames[j]);
                    sheet.AutoSizeColumn(j);
                }

                //合并单元格，如果要合并的单元格中都有数据，只会保留左上角的
                //CellRangeAddress(0, 2, 0, 0)，合并0-2行，0-0列的单元格
                CellRangeAddress region = new CellRangeAddress(0, 0, 0, 0);
                sheet.AddMergedRegion(region);

                FileStream fst = File.OpenWrite(filePath);
                wb.Write(fst);//向打开的这个Excel文件中写入表单并保存。
                fst.Close();
            }
            catch (Exception ex)
            {
                throw (new Exception("写数据错误" + ex.Message));
            }
        }

        /// <summary>
        /// 读取Exclec表格到TabControl并以表格创建TabPage 将数据填充到 DataGridView
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static void UpDataExclec(string path, TabControl tabControl1)
        {
            List<string> listCoumnsName = new List<string>();
            tabControl1.Controls.Clear();
            try
            {
                //this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                string strConn = "";
                if (!File.Exists(path))
                {
                    return;
                }
                if (IntPtr.Size == 4)
                {
                    // 32-bit
                    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + path + ";" + "Extended Properties=Excel 8.0;";
                }
                else if (IntPtr.Size == 8)
                {
                    strConn = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + path + ";Extended Properties='Excel 12.0; HDR=NO; IMEX=1'"; //此連接可以操作.xls與.xlsx文件
                }
                if (System.IO.Path.GetExtension(path).ToLower().Contains(".xls"))
                {
                    //打开Excel的连接，设置连接对象
                    OleDbConnection conne = new OleDbConnection(strConn);
                    conne.Open();
                    tabControl1.TabPages.Clear();
                    DataTable sheetNames = conne.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                    //遍历Excel文件获取Excel工作表，并将所有工作表名称加载到comboBox控件中
                    foreach (DataRow dr in sheetNames.Rows)
                    {
                        //添加工作表名称
                        TabPage tabPage = new TabPage();
                        tabPage.Text = dr[2].ToString();
                        tabPage.Name = dr[2].ToString();
                        tabPage.AutoScroll = true;
                        tabControl1.TabPages.Add(tabPage);
                        try
                        {
                            //当前选中的工作表前几行数据，获取数据列
                            OleDbDataAdapter oada3 = new OleDbDataAdapter("select top 5 * from [" + dr[2].ToString() + "]", strConn);
                            DataTable ds = new DataTable();
                            oada3.Fill(ds);
                            //将列加载到树节点上
                            for (int i = 0; i < ds.Columns.Count; i++)
                            {
                                listCoumnsName.Add(ds.Columns[i].ColumnName.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        //存储需要查询数据的列
                        string strName = "";
                        //遍历列名，每个列名用逗号隔开
                        for (int i = 0; i < listCoumnsName.Count; i++)
                        {
                            strName = strName + "[" + listCoumnsName[i] + "],";
                        }
                        strName = strName.Substring(0, strName.Length - 1);
                        //获取用户没有删掉留下来的列，读取这些列的数据
                        //建立Excel连接
                        //读取数据
                        OleDbDataAdapter oada = new OleDbDataAdapter("select  " + strName + " from [" + tabPage.Text + "]", strConn);
                        DataTable dt = new DataTable();

                        //填入DataTable
                        oada.Fill(dt);
                        if (IntPtr.Size == 8)
                        {
                            for (int i = 0; i < listCoumnsName.Count; i++)
                            {
                                if (listCoumnsName[i] != null)
                                {
                                    dt.Columns[listCoumnsName[i]].ColumnName = dt.Rows[0][listCoumnsName[i]].ToString();
                                }
                            }
                        }
                        DataGridView dataGridView = new DataGridView();
                        dataGridView.AllowUserToAddRows = false;
                        tabPage.Controls.Add(dataGridView);
                        dataGridView.Name = tabPage.Text;
                        dataGridView.DataSource = dt;
                        dataGridView.Dock = DockStyle.Fill;
                        dataGridView.Left = 0;
                        //dataGridView.Size= new System.Drawing.Size() { Height = tabPage.Size.Height, Width = tabPage.Size.Width * 4 };
                        dataGridView.RowHeadersVisible = false;
                        dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                        dataGridView.ScrollBars = ScrollBars.Both;
                        dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                        ContextMenuStrip contextMenu = new ContextMenuStrip();
                        dataGridView.ContextMenuStrip = contextMenu;
                        ToolStripItem toolStripItem = contextMenu.Items.Add("打开图片");
                        ToolStripItem toolStripItem2 = contextMenu.Items.Add("计算偏差");
                    }
                    conne.Close();
                }
                else
                {
                    MessageBox.Show("excel 格式不正确！");
                }
                listCoumnsName.Clear();
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return;
            //设置鼠标指针状态为默认状态
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        public static void UpDataExclec(string path, int CoumnsNameIndex, DataGridView dataGridView1)
        {
            List<string> listCoumnsName = new List<string>();
            List<string> CoumnsName = new List<string>();
            try
            {
                //this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                string strConn = "";
                if (!File.Exists(path))
                {
                    MessageBox.Show("文件不存在:" + path);
                    return;
                }
                if (IntPtr.Size == 4)
                {
                    // 32-bit
                    strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + path + ";" + "Extended Properties=Excel 8.0;";
                }
                else if (IntPtr.Size == 8)
                {
                    strConn = "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + path + ";Extended Properties='Excel 12.0; HDR=NO; IMEX=1'"; //此連接可以操作.xls與.xlsx文件
                }
                if (System.IO.Path.GetExtension(path).ToLower() != ".xls")
                {
                    //打开Excel的连接，设置连接对象
                    OleDbConnection conne = new OleDbConnection(strConn);
                    conne.Open();

                    DataTable sheetNames = conne.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                    //遍历Excel文件获取Excel工作表，并将所有工作表名称加载到comboBox控件中
                    foreach (DataRow dr in sheetNames.Rows)
                    {
                        //添加工作表名称
                        try
                        {
                            //当前选中的工作表前几行数据，获取数据列
                            OleDbDataAdapter oada3 = new OleDbDataAdapter("select top 5 * from [" + dr[2].ToString() + "]", strConn);
                            DataTable ds = new DataTable();
                            oada3.Fill(ds);
                            //将列加载到树节点上
                            for (int i = 0; i < ds.Columns.Count; i++)
                            {
                                listCoumnsName.Add(ds.Columns[i].ColumnName.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        //存储需要查询数据的列
                        string strName = "";
                        //遍历列名，每个列名用逗号隔开
                        for (int i = 0; i < listCoumnsName.Count; i++)
                        {
                            strName = strName + "[" + listCoumnsName[i] + "],";
                        }
                        strName = strName.Substring(0, strName.Length - 1);
                        //获取用户没有删掉留下来的列，读取这些列的数据
                        //建立Excel连接
                        //读取数据
                        OleDbDataAdapter oada = new OleDbDataAdapter("select  " + strName + " from [" + dr[2].ToString() + "]", strConn);
                        DataTable dt = new DataTable();
                        oada.Fill(dt);
                        int index = 0;
                        dataGridView1.Rows.Clear();
                        dataGridView1.Columns.Clear();
                        foreach (System.Data.DataRow item in dt.Rows)
                        {
                            if (CoumnsNameIndex == index)
                            {
                                for (int i = 0; i < item.ItemArray.Length; i++)
                                {
                                    DataGridViewTextBoxColumn dataGridTextBoxColumn = new DataGridViewTextBoxColumn();
                                    dataGridTextBoxColumn.HeaderText = item.ItemArray[i].ToString();
                                    dataGridView1.Columns.Add(dataGridTextBoxColumn);
                                }
                            }
                            else if (index > CoumnsNameIndex)
                            {
                                int ds = dataGridView1.Rows.Add();
                                for (int i = 0; i < item.ItemArray.Length; i++)
                                {
                                    if (dataGridView1.Columns.Count < i)
                                    {
                                        DataGridViewTextBoxColumn dataGridTextBoxColumn = new DataGridViewTextBoxColumn();

                                        dataGridView1.Columns.Add(dataGridTextBoxColumn);
                                    }
                                    dataGridView1.Rows[ds].Cells[i].Value = item.ItemArray[i];
                                }
                            }
                            index++;
                        }
                        dataGridView1.EndEdit();
                    }
                }
                else
                {
                    MessageBox.Show("excel 格式不正确！");
                }
                listCoumnsName.Clear();
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return;
            //设置鼠标指针状态为默认状态
        }

        /// <summary>
        /// 从DataGridView导出到Excle表格
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="sheetName"></param>
        /// <param name="dataGridView"></param>
        public static void DataGridViewExportExcel(string filePath, string sheetName, DataGridView dataGridView)
        {
            try
            {
                List<string> list = new List<string>();
                for (int i = 0; i < dataGridView.ColumnCount; i++)
                {
                    list.Add(dataGridView.Columns[i].HeaderText);
                }
                AddWriteColumnToExcel(filePath, sheetName, list.ToArray());
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    List<object> listObj = new List<object>();
                    for (int i2 = 0; i2 < dataGridView.Rows[i].Cells.Count; i2++)
                    {
                        if (dataGridView.Rows[i].Cells[i2].Value == null)
                        {
                            listObj.Add(null);
                            continue;
                        }
                        if (Double.TryParse(dataGridView.Rows[i].Cells[i2].Value.ToString(), out double dewsd))
                        {
                            listObj.Add(dewsd);
                        }
                        else
                        {
                            listObj.Add(dataGridView.Rows[i].Cells[i2].Value.ToString());
                        }

                    }
                    AddRosWriteToExcel(filePath, sheetName, listObj.ToArray());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("DataGridView导出到Excel错误：" + ex.Message);
            }
        }


        /// <summary>
        /// 读取Excel生成类，标题为字段标签
        /// </summary>
        /// <param name="path">读取的Excel文件地址</param>
        /// <returns>返回的类</returns>
        public static T GetPahtLoad<T>(string path)
        {

            return default(T);
        }

        /// <summary>
        /// 根据DisplayName写对象到Excel文件地址,对象必须实现DisplayName并不可重复，并且是非引用类型，或List<string>\Dic<string,string>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="path"></param>
        public static void WriteObjDisplayNameToExcel<T>(T obj, string sheetName, string path)
        {
            //取得属性集合
            PropertyInfo[] pi = obj.GetType().GetProperties();
            string[] dispNames = Dynamic.ErosDynamic.GetDisPlayNames_Values(obj, out string[] values);
            AddWriteColumnToExcel(path, sheetName, dispNames);
            AddRosWriteToExcel(path, sheetName, values);
        }


        #region ini 文件读写函数

        //再一种声明，使用string作为缓冲区的类型同char[]
        /// <summary>
        /// 读取INI文件中指定的Key的值
        /// </summary>
        /// <param name="lpAppName">节点名称。如果为null,则读取INI中所有节点名称,每个节点名称之间用\0分隔</param>
        /// <param name="lpKeyName">Key名称。如果为null,则读取INI中指定节点中的所有KEY,每个KEY之间用\0分隔</param>
        /// <param name="lpDefault">读取失败时的默认值</param>
        /// <param name="lpReturnedString">读取的内容缓冲区，读取之后，多余的地方使用\0填充</param>
        /// <param name="nSize">内容缓冲区的长度</param>
        /// <param name="lpFileName">INI文件名</param>
        /// <returns>实际读取到的长度</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, [In, Out] char[] lpReturnedString, uint nSize, string lpFileName);

        //另一种声明方式,使用 StringBuilder 作为缓冲区类型的缺点是不能接受\0字符，会将\0及其后的字符截断,
        //所以对于lpAppName或lpKeyName为null的情况就不适用
        /// <summary>
        /// 读取INI文件中指定的Key的值
        /// </summary>
        /// <param name="lpAppName">节点名称。如果为null,则读取INI中所有节点名称,每个节点名称之间用\0分隔</param>
        /// <param name="lpKeyName">Key名称。如果为null,则读取INI中指定节点中的所有KEY,每个KEY之间用\0分隔</param>
        /// <param name="lpDefault">读取失败时的默认值</param>
        /// <param name="lpReturnedString">读取的内容缓冲区，读取之后，多余的地方使用\0填充</param>
        /// <param name="nSize">内容缓冲区的长度</param>
        /// <param name="lpFileName">INI文件名</param>
        /// <returns>实际读取到的长度</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);

        /// <summary>
        /// 读取INI文件中指定的Key的值
        /// </summary>
        /// <param name="lpAppName">节点名称。如果为null,则读取INI中所有节点名称,每个节点名称之间用\0分隔</param>
        /// <param name="lpKeyName">Key名称。如果为null,则读取INI中指定节点中的所有KEY,每个KEY之间用\0分隔</param>
        /// <param name="lpDefault">读取失败时的默认值</param>
        /// <param name="lpReturnedString">读取的内容缓冲区，读取之后，多余的地方使用\0填充</param>
        /// <param name="nSize">内容缓冲区的长度</param>
        /// <param name="lpFileName">INI文件名</param>
        /// <returns>实际读取到的长度</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, string lpReturnedString, uint nSize, string lpFileName);

        /// <summary>
        /// 将指定的键和值写到指定的节点，如果已经存在则替换
        /// </summary>
        /// <param name="lpAppName">节点名称</param>
        /// <param name="lpKeyName">键名称。如果为null，则删除指定的节点及其所有的项目</param>
        /// <param name="lpString">值内容。如果为null，则删除指定节点中指定的键。</param>
        /// <param name="lpFileName">INI文件</param>
        /// <returns>操作是否成功</returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        #endregion ini 文件读写函数

    }
}