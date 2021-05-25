using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace Vision2.ErosProjcetDLL.Bytes
{
    /// <summary>0
    /// Byte和File互转帮助类
    /// </summary>
    public class ByteHelper
    {

        /// <summary> 
        /// 将一个object对象序列化，返回一个byte[]         
        /// </summary> 
        /// <param name="obj">能序列化的对象</param>         
        /// <returns></returns> 
        public static byte[] ObjectToBytes(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(ms, obj); return ms.GetBuffer();
            }
        }

        /// <summary> 
        /// 将一个序列化后的byte[]数组还原         
        /// </summary>
        /// <param name="Bytes"></param>         
        /// <returns></returns> 
        public static object BytesToObject(byte[] Bytes)
        {
            using (MemoryStream ms = new MemoryStream(Bytes))
            {
                IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return formatter.Deserialize(ms);
            }
        }
        /// <summary>
        /// 读文件到byte[]
        /// </summary>
        /// <param name="fileName">硬盘文件路径</param>
        /// <returns></returns>
        public static byte[] ReadFileToByte(string fileName)
        {
            FileStream pFileStream = null;
            byte[] pReadByte = new byte[0];
            try
            {
                pFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);

                BinaryReader r = new BinaryReader(pFileStream, GetFileEncodeType(fileName));
                r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开
                pReadByte = r.ReadBytes((int)r.BaseStream.Length);
                return pReadByte;
            }
            catch
            {
                return pReadByte;
            }
            finally
            {
                if (pFileStream != null)
                    pFileStream.Close();
            }
        }
        /// <summary>
        /// 写byte[]到fileName
        /// </summary>
        /// <param name="pReadByte">byte[]</param>
        /// <param name="fileName">保存至硬盘路径</param>
        /// <returns></returns>
        public static bool WriteByteToFile(byte[] pReadByte, string fileName)
        {
            FileStream pFileStream = null;
            try
            {
                //if(File.Exists(fileName))
                //{
                //   MessageBoxResult messageBoxResult=  MessageBox.Show("文件已存在，是否覆盖？", "覆盖文件", MessageBoxButton.YesNo, MessageBoxImage.Question);
                //    if (messageBoxResult!=MessageBoxResult.Yes)
                //    {
                //        return false;
                //    }
                //}
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                pFileStream = new FileStream(fileName, FileMode.OpenOrCreate);
                pFileStream.Write(pReadByte, 0, pReadByte.Length);
            }
            catch
            {
                return false;
            }
            finally
            {
                if (pFileStream != null)
                    pFileStream.Close();
            }
            return true;
        }


        /// <summary>
        /// 字符串转指定CodeHex进制字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] StringHexToByte(string hexString, int codeHex)
        {
            hexString = hexString.Replace(" ", "");
            hexString = hexString.Replace("-", "0");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), codeHex);
            return returnBytes;
        }
        // 16进制字符串转字节数组   格式为 string sendMessage = "00 01 00 00 00 06 FF 05 00 64 00 00";
        private static byte[] HexStrTobyte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2).Trim(), 16);
            return returnBytes;
        }
        // 字节数组转16进制字符串
        public static string ByteToHexStr(byte[] bytes, string ch = " ")
        {
            string str0x = BitConverter.ToString(bytes, 0, bytes.Length).Replace("-", ch);
            return str0x;
        }

        /// <summary>
        /// 读取文本文件的编码格式
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static System.Text.Encoding GetFileEncodeType(string filename)
        {
            using (FileStream fs = new FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
                byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
                byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF };//带BOM
                Encoding reVal = Encoding.Default;
                BinaryReader br = new BinaryReader(fs);
                int length;
                int.TryParse(fs.Length.ToString(), out length);
                byte[] ss = br.ReadBytes(length);
                if (IsUTF8Bytes(ss) ||
                    (ss[0] == UTF8[0] && ss[1] == UTF8[1] && ss[2] == UTF8[2]))
                    reVal = Encoding.UTF8;
                else if (ss[0] == UnicodeBIG[0] && ss[1] == UnicodeBIG[1] && ss[2] == UnicodeBIG[2])
                    reVal = Encoding.BigEndianUnicode;
                else if (ss[0] == Unicode[0] && ss[1] == Unicode[1] && ss[2] == Unicode[2])
                    reVal = Encoding.Unicode;
                br.Close();

                return reVal;
            }
        }

        /// <summary>
        /// 判断是否是不带BOM的UTF8格式
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1;//计算当前正分析的字符应还有的字节数
            byte curByte;//当前分析的字节
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        //判断当前
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        //标记位首位若为非0 则至少以2个1开始，如：110XXXXX.....1111110X
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //若是UTF-8 此时第一位必须为1
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                throw new Exception("非预期的byte格式");
            }
            return true;
        }

    }

}
