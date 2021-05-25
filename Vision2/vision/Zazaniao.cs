using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Vision2.vision
{
    public class ZazaniaoDll
    {
        // 得到应用程序的目录
        public static string GetDirectory()
        {
            string szPath;
            szPath = Directory.GetCurrentDirectory();
            return szPath;
        }

        //目录是否存在检查
        public static bool FolderExist(string path)
        {
            bool bValue = false;
            bValue = Directory.Exists(path);
            return bValue;
        }

        //某个硬盘分区是否存在，分区必须大写
        public static bool DiskExist(string strPath)
        {
            char[] chas = { '\\' };
            string[] str = strPath.Split(chas);
            bool bValue;
            bValue = Directory.Exists(str[0]);
            return bValue;
        }

        //创建文件夹
        public static void CreateAllDirectory(string szPath)
        {
            char[] chas = { '\\' };
            string[] str = szPath.Split(chas);
            string pathN = null, pathN1 = null;
            for (int i = 0; i < str.Length - 1; i++)
            {
                pathN += str[i] + "\\";
                pathN1 = DirectoryExistOrCreate(pathN);
            }
        }

        //获取当前时间
        public static string GetCurrentTimeAsString()
        {
            string Time;
            Time = DateTime.Now.ToString("yyyyMMddHHmmss");
            return Time;
        }

        //文件是否存在
        public static bool FileExist(string FileName)
        {
            bool bValue;
            bValue = File.Exists(FileName);
            return bValue;
        }

        //删除目录
        public static bool DelectDirectory(string DirName)
        {
            if (Directory.Exists(DirName))
            {
                Directory.Delete(DirName);
                return true;
            }
            return false;
        }

        //save Log file
        public static void Log(string szPath, string szText, bool bWirteFirstLine, string szFirstLineText)
        {
            try
            {
                string time1;
                time1 = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

                CreateAllDirectory(szPath);
                if (!File.Exists(szPath))
                {
                    using (FileStream fs = new FileStream(szPath, FileMode.Create, FileAccess.Write))
                    {
                        if (bWirteFirstLine)
                        {
                            StringBuilder sb1 = new StringBuilder();
                            sb1.Append("时间").Append(",").Append(szFirstLineText).Append(",").Append("\n");
                            string str = sb1.ToString();
                            byte[] buffer = Encoding.Default.GetBytes(str);
                            fs.Write(buffer, 0, buffer.Length);
                        }
                    }
                }
                using (StreamWriter sw = new StreamWriter(szPath, true, Encoding.Default))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(time1).Append(",").Append(szText).Append(",");
                    sw.WriteLine(sb);
                }
            }
            catch (Exception)
            {
            }
        }

        public static double Rad(double dAngle)
        {
            double Rad1;
            Rad1 = (dAngle / 180) * Math.PI;
            return Rad1;
        }

        public static double Deg(double dAngle)
        {
            double Deg1;
            Deg1 = (dAngle / Math.PI) * 180;
            return Deg1;
        }

        //文件是否存在，不存在则创建
        public static string DirectoryExistOrCreate(string pathD)
        {
            if (!Directory.Exists(pathD))
            {
                Directory.CreateDirectory(pathD);
            }
            return pathD;
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