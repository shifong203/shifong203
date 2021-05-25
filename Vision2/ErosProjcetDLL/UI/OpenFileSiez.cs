using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Vision2.ErosProjcetDLL.UI
{
    public partial class OpenFileSiez : UserControl
    {
        public List<string> filePathPublic = new List<string>();

        public OpenFileSiez()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 计算文件大小函数(保留两位小数),Size为字节大小
        /// </summary>
        /// <param name="Size">初始文件大小</param>
        /// <returns></returns>
        public static string CountSize(long Size)
        {
            string m_strSize = "";
            long FactSize = 0;
            FactSize = Size;
            if (FactSize < 1024.00)
                m_strSize = FactSize.ToString("F2") + " Byte";
            else if (FactSize >= 1024.00 && FactSize < 1048576)
                m_strSize = (FactSize / 1024.00).ToString("F2") + " K";
            else if (FactSize >= 1048576 && FactSize < 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00).ToString("F2") + " M";
            else if (FactSize >= 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " G";
            return m_strSize;
        }

        /// <summary>
        /// 所给路径中所对应的文件大小
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static long FileSize(string filePath)
        {
            if (Directory.Exists(filePath))
            {
                //定义一个FileInfo对象，是指与filePath所指向的文件相关联，以获取其大小
                FileInfo fileInfo = new FileInfo(filePath);
                return fileInfo.Length;
            }
            return 0;
        }

        /// <summary>
        /// 所给路径中所对应的文件占用空间
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static long FileSpace(string filePath)

        {
            long temp = 0;

            //定义一个FileInfo对象，是指与filePath所指向的文件相关联，以获取其大小

            FileInfo fileInfo = new FileInfo(filePath);

            long clusterSize = GetClusterSize(fileInfo);

            if (fileInfo.Length % clusterSize != 0)

            {
                decimal res = fileInfo.Length / clusterSize;

                int clu = Convert.ToInt32(Math.Ceiling(res)) + 1;

                temp = clusterSize * clu;
            }
            else

            {
                return fileInfo.Length;
            }

            return temp;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static long GetClusterSize(FileInfo file)

        {
            long clusterSize = 0;

            DiskInfo diskInfo = new DiskInfo();

            diskInfo = GetDiskInfo(file.Directory.Root.FullName);

            clusterSize = (diskInfo.BytesPerSector * diskInfo.SectorsPerCluster);

            return clusterSize;
        }

        /// <summary>
        /// 获取指定驱动器的剩余空间总大小(单位为G)
        ///   只需输入代表驱动器的字母即可
        /// </summary>
        /// <param name="str_HardDiskName"></param>
        /// <returns></returns>
        public static long GetHardDiskFreeSpace(char str_HardDiskName)
        {
            long freeSpace = new long();
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
            foreach (System.IO.DriveInfo drive in drives)
            {
                if (drive.Name == str_HardDiskName + ":\\")
                {
                    freeSpace = drive.TotalFreeSpace / (1024 * 1024 * 1024);
                }
            }
            return freeSpace;
        }

        /// <summary>
        /// 获取指定路径目录的占用大小
        /// </summary>
        /// <param name="dirPath">路径</param>
        /// <returns></returns>
        public static long GetDirectoryLength(string dirPath)
        {
            long len = 0;
            //判断该路径是否存在（是否为文件夹）
            if (!Directory.Exists(dirPath))
            {
                //查询文件的大小

                len = FileSize(dirPath);
            }
            else

            {
                //定义一个DirectoryInfo对象

                DirectoryInfo di = new DirectoryInfo(dirPath);

                //通过GetFiles方法，获取di目录中的所有文件的大小

                foreach (FileInfo fi in di.GetFiles())

                {
                    len += fi.Length;
                }

                //获取di中所有的文件夹，并存到一个新的对象数组中，以进行递归

                DirectoryInfo[] dis = di.GetDirectories();

                if (dis.Length > 0)

                {
                    for (int i = 0; i < dis.Length; i++)

                    {
                        len += GetDirectoryLength(dis[i].FullName);
                    }
                }
            }
            return len;
        }

        /// <returns></returns>
        /// <summary>
        ///
        /// </summary>
        /// <param name="rootPathName"></param>
        /// <returns></returns>
        public static DiskInfo GetDiskInfo(string rootPathName)

        {
            DiskInfo diskInfo = new DiskInfo();

            int sectorsPerCluster = 0, bytesPerSector = 0, numberOfFreeClusters = 0, totalNumberOfClusters = 0;

            GetDiskFreeSpace(rootPathName, ref sectorsPerCluster, ref bytesPerSector, ref numberOfFreeClusters, ref totalNumberOfClusters);

            //每簇的扇区数

            diskInfo.SectorsPerCluster = sectorsPerCluster;
            //每扇区字节
            diskInfo.BytesPerSector = bytesPerSector;
            return diskInfo;
        }

        /// <summary>
        /// 导入库   调用windows API获取磁盘空闲空间
        /// </summary>
        /// <param name="rootPathName"></param>
        /// <param name="sectorsPerCluster"></param>
        /// <param name="bytesPerSector"></param>
        /// <param name="numberOfFreeClusters"></param>
        /// <param name="totalNumbeOfClusters"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern bool GetDiskFreeSpace([MarshalAs(UnmanagedType.LPTStr)] string rootPathName,
        ref int sectorsPerCluster, ref int bytesPerSector, ref int numberOfFreeClusters, ref int totalNumbeOfClusters);

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenFileSiez_Load(object sender, EventArgs e)
        {
            try
            {
                int d = 0;
                for (int i = 0; i < filePathPublic.Count; i++)
                {
                    d = dataGridView1.Rows.Add(filePathPublic[i]);
                    dataGridView1.Rows[d].Cells[1].Value = CountSize(FileSize(filePathPublic[i]));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <param name="file">指定文件</param>
        /// <summary>
        /// 硬盘信息
        /// </summary>
        public struct DiskInfo
        {
            public int BytesPerSector;

            //每扇区字节
            public int NumberOfFreeClusters;

            public string RootPathName;

            //每簇的扇区数

            public int SectorsPerCluster;
            public int TotalNumberOfClusters;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
                //dialog.RootFolder = Environment.SpecialFolder.;
                dialog.Description = "请选择Txt所在文件夹";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (string.IsNullOrEmpty(dialog.SelectedPath))
                    {
                        MessageBox.Show(this, "文件夹路径不能为空", "提示");
                        return;
                    }
                    //System.Diagnostics.Process.Start(dialog.SelectedPath);
                    string data = textBox1.Text;
                    List<string> listPath = new List<string>();
                    if (textBox1.Text.Contains(dialog.SelectedPath))
                    {
                        while (data.Contains(Environment.NewLine))
                        {
                            if (!listPath.Contains(data.Substring(0, data.IndexOf('|'))))
                            {
                                textBox1.AppendText(dialog.SelectedPath + "|" + CountSize(GetDirectoryLength(dialog.SelectedPath)) + Environment.NewLine);
                                textBox1.ScrollToCaret();
                                listPath.Add(data.Substring(0, data.IndexOf(Environment.NewLine, StringComparison.Ordinal)));
                            }
                            data = data.Remove(0, data.IndexOf(Environment.NewLine, StringComparison.Ordinal) + Environment.NewLine.Length);
                        }
                    }
                    else
                    {
                        textBox1.AppendText(dialog.SelectedPath + "|" + CountSize(GetDirectoryLength(dialog.SelectedPath)) + Environment.NewLine);
                        textBox1.ScrollToCaret();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}