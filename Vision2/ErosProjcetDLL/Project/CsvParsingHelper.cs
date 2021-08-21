using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace Vision2.ErosProjcetDLL.Project
{
    public class CsvParsingHelper
    {
        /// <summary>
        /// 将csv文件的数据转成datatable
        /// </summary>
        /// <param name="csvfilePath">csv文件路径</param>
        /// <param name="firstIsRowHead">是否将第一行作为字段名</param>
        /// <returns></returns>
        public static DataTable CsvToDataTable(string csvfilePath, bool firstIsRowHead)
        {
            DataTable dtResult = null;
            if (File.Exists(csvfilePath))
            {
                string csvstr = File.ReadAllText(csvfilePath, Encoding.Default);
                if (!string.IsNullOrEmpty(csvstr))
                {
                    dtResult = ToDataTable(csvstr, firstIsRowHead);
                }
            }
            return dtResult;
        }

        /// <summary>
        /// 将CSV数据转换为DataTable
        /// </summary>
        /// <param name="csv">包含以","分隔的CSV数据的字符串</param>
        /// <param name="isRowHead">是否将第一行作为字段名</param>
        /// <returns></returns>
        private static DataTable ToDataTable(string csv, bool isRowHead)
        {
            DataTable dt = null;
            if (!string.IsNullOrEmpty(csv))
            {
                dt = new DataTable();
                string[] csvRows = csv.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                string[] csvColumns = null;
                if (csvRows != null)
                {
                    if (csvRows.Length > 0)
                    {
                        //第一行作为字段名,添加第一行记录并删除csvRows中的第一行数据
                        if (isRowHead)
                        {
                            csvColumns = FromCsvLine(csvRows[0]);
                            csvRows[0] = null;
                            for (int i = 0; i < csvColumns.Length; i++)
                            {
                                dt.Columns.Add(csvColumns[i]);
                            }
                        }

                        for (int i = 0; i < csvRows.Length; i++)
                        {
                            if (csvRows[i] != null)
                            {
                                csvColumns = FromCsvLine(csvRows[i]);
                                //检查列数是否足够,不足则补充
                                if (dt.Columns.Count < csvColumns.Length)
                                {
                                    int columnCount = csvColumns.Length - dt.Columns.Count;
                                    for (int c = 0; c < columnCount; c++)
                                    {
                                        dt.Columns.Add();
                                    }
                                }
                                dt.Rows.Add(csvColumns);
                            }
                        }
                    }
                }
            }

            return dt;
        }

        /// <summary>
        /// 解析一行CSV数据
        /// </summary>
        /// <param name="csv">csv数据行</param>
        /// <returns></returns>
        public static string[] FromCsvLine(string csv)
        {
            List<string> csvLiAsc = new List<string>();
            List<string> csvLiDesc = new List<string>();

            if (!string.IsNullOrEmpty(csv))
            {
                //顺序查找
                int lastIndex = 0;
                int quotCount = 0;
                //剩余的字符串
                string lstr = string.Empty;
                for (int i = 0; i < csv.Length; i++)
                {
                    if (csv[i] == '"')
                    {
                        quotCount++;
                    }
                    else if (csv[i] == ',' && quotCount % 2 == 0)
                    {
                        csvLiAsc.Add(ReplaceQuote(csv.Substring(lastIndex, i - lastIndex)));
                        lastIndex = i + 1;
                    }
                    if (i == csv.Length - 1 && lastIndex < csv.Length)
                    {
                        lstr = csv.Substring(lastIndex, i - lastIndex + 1);
                    }
                }
                if (!string.IsNullOrEmpty(lstr))
                {
                    //倒序查找
                    lastIndex = 0;
                    quotCount = 0;
                    string revStr = Reverse(lstr);
                    for (int i = 0; i < revStr.Length; i++)
                    {
                        if (revStr[i] == '"')
                        {
                            quotCount++;
                        }
                        else if (revStr[i] == ',' && quotCount % 2 == 0)
                        {
                            csvLiDesc.Add(ReplaceQuote(Reverse(revStr.Substring(lastIndex, i - lastIndex))));
                            lastIndex = i + 1;
                        }
                        if (i == revStr.Length - 1 && lastIndex < revStr.Length)
                        {
                            csvLiDesc.Add(ReplaceQuote(Reverse(revStr.Substring(lastIndex, i - lastIndex + 1))));
                            lastIndex = i + 1;
                        }
                    }
                    string[] tmpStrs = csvLiDesc.ToArray();
                    Array.Reverse(tmpStrs);
                    csvLiAsc.AddRange(tmpStrs);
                }
            }

            return csvLiAsc.ToArray();
        }

        /// <summary>
        /// 反转字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string Reverse(string str)
        {
            string revStr = string.Empty;
            foreach (char chr in str)
            {
                revStr = chr.ToString() + revStr;
            }
            return revStr;
        }

        /// <summary>
        /// 替换CSV中的双引号转义符为正常双引号,并去掉左右双引号
        /// </summary>
        /// <param name="csvValue">csv格式的数据</param>
        /// <returns></returns>
        private static string ReplaceQuote(string csvValue)
        {
            string rtnStr = csvValue;
            if (!string.IsNullOrEmpty(csvValue))
            {
                //首尾都是"
                System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(csvValue, "^\"(.*?)\"$");
                if (m.Success)
                {
                    rtnStr = m.Result("${1}").Replace("\"\"", "\"");
                }
                else
                {
                    rtnStr = rtnStr.Replace("\"\"", "\"");
                }
            }
            return rtnStr;
        }
    }
}