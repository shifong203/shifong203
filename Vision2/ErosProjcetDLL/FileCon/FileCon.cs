using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Vision2.ErosProjcetDLL.FileCon
{
    public static class FileConStatic
    {
        /// <summary>
        /// 获得文件夹下所以文件
        /// </summary>
        /// <param name="path">地址</param>
        /// <returns></returns>
        public static string[] GetFilesArrayPath(string path)
        {
            List<string> fileslist = new List<string>();
            if (!Directory.Exists(path))
            {
                return fileslist.ToArray();
            }
            string[] files = Directory.GetFiles(path);
            fileslist.AddRange(files);
            var paths = Directory.GetDirectories(path);
            for (int i = 0; i < paths.Length; i++)
            {
                var file = GetFilesArrayPath(paths[i]);
                fileslist.AddRange(file);
            }
            return fileslist.ToArray();
        }

        /// <summary>
        /// 获得文件夹下所有赛选文件
        /// </summary>
        /// <param name="path">文件地址</param>
        /// <param name="sele">赛选后缀名</param>
        /// <returns></returns>
        public static string[] GetFilesArrayPath(string path, string sele)
        {
            return GetFilesArrayPath(path).Where(item => item.EndsWith(sele, StringComparison.Ordinal)).ToArray();
        }

        /// <summary>
        /// 将地址文件夹以及文件递归显示在TreeView上
        /// </summary>
        /// <param name="treeView"></param>
        /// <param name="path"></param>
        public static void GetFilesToTreeNode(TreeNode tree, string path)
        {
            string[] itemPaths = Directory.GetDirectories(path);
            string[] itemPaths1 = Directory.GetFiles(path);

            Array.Sort(itemPaths1, new CustomFilesNameComparer());

            for (int i = 0; i < itemPaths1.Length; i++)
            {
                TreeNode treeNode = new TreeNode();
                treeNode.Name = treeNode.Text = Path.GetFileName(itemPaths1[i]);
                treeNode.Tag = itemPaths1[i];
                tree.Nodes.Add(treeNode);
            }
            for (int i = 0; i < itemPaths.Length; i++)
            {
                TreeNode treeNode = new TreeNode();
                treeNode.Name = treeNode.Text = Path.GetFileName(itemPaths[i]);
                tree.Nodes.Add(treeNode);
                GetFilesToTreeNode(treeNode, itemPaths[i]);
            }
        }

        public class CustomFilesNameComparer : System.Collections.IComparer
        {
            public CustomFilesNameComparer()
            {
            }

            // Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            ///<summary>
            ///比较两个字符串，如果含用数字，则数字按数字的大小来比较。
            ///</summary>
            ///<param name="x"></param>
            ///<param name="y"></param>
            ///<returns></returns>
            public int Compare(Object x, Object y)
            {
                if (x == null || y == null)
                    throw new ArgumentException("Parameters can't be null");
                string fileA = Path.GetFileNameWithoutExtension(x.ToString());
                string fileB = Path.GetFileNameWithoutExtension(y.ToString());
                char[] arr1 = fileA.ToCharArray();
                char[] arr2 = fileB.ToCharArray();
                int i = 0, j = 0;
                while (i < arr1.Length && j < arr2.Length)
                {
                    if (char.IsDigit(arr1[i]) && char.IsDigit(arr2[j]))
                    {
                        string s1 = "", s2 = "";
                        while (i < arr1.Length && char.IsDigit(arr1[i]))
                        {
                            s1 += arr1[i];
                            i++;
                        }
                        while (j < arr2.Length && char.IsDigit(arr2[j]))
                        {
                            s2 += arr2[j];
                            j++;
                        }

                        if (int.Parse(s1) > int.Parse(s2))
                        {
                            return 1;
                        }

                        if (int.Parse(s1) < int.Parse(s2))
                        {
                            return -1;
                        }
                    }
                    else
                    {
                        if (arr1[i] > arr2[j])
                        {
                            return 1;
                        }

                        if (arr1[i] < arr2[j])
                        {
                            return -1;
                        }
                        i++;
                        j++;
                    }
                }
                if (arr1.Length == arr2.Length)
                {
                    return 0;
                }
                else
                {
                    return arr1.Length > arr2.Length ? 1 : -1;
                }
            }
        }

        public class CustomComparer : System.Collections.IComparer
        {
            public CustomComparer()
            {
            }

            private string sele;

            public CustomComparer(string selestr)
            {
                sele = selestr;
            }

            // Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            ///<summary>
            ///比较两个字符串，如果含用数字，则数字按数字的大小来比较。
            ///</summary>
            ///<param name="x"></param>
            ///<param name="y"></param>
            ///<returns></returns>
            public int Compare(Object x, Object y)
            {
                try
                {
                    if (x == null || y == null)
                        throw new ArgumentException("Parameters can't be null");

                    string fileA = x.ToString();
                    string fileB = y.ToString();

                    if (sele != null)
                    {
                        fileA = fileA.ToString().Remove(0, sele.Length);
                        fileB = fileB.ToString().Remove(0, sele.Length);
                    }
                    char[] arr1 = fileA.ToCharArray();
                    char[] arr2 = fileB.ToCharArray();

                    if (double.Parse(fileA) > double.Parse(fileB))
                    {
                        return 1;
                    }
                    if (double.Parse(fileA) == double.Parse(fileB))
                    {
                        return 0;
                    }
                }
                catch (Exception)
                {
                }
                return -1;
            }
        }

        /// <summary>
        /// 获得文件夹下所有赛选文件地址
        /// </summary>
        /// <param name="path">文件夹地址</param>
        /// <param name="sele"></param>
        /// <returns></returns>
        public static Dictionary<string, List<string>> GetFilesDicListPath(string path, string sele)
        {
            string[] seles = sele.Split(',');
            Dictionary<string, List<string>> keyValuePairs = new Dictionary<string, List<string>>();
            if (!Directory.Exists(path))
            {
                return null;
            }
            List<string> fileslist = new List<string>();
            string d = path.Remove(0, path.IndexOf('\\') + 1);
            if (d == "")
            {
                d = path.Replace('\\', ' ');
            }
            string[] files = Directory.GetFiles(path);
            if (files.Length != 0)
            {
                List<string> filesSeles = new List<string>();
                for (int i = 0; i < files.Length; i++)
                {
                    for (int i1 = 0; i1 < seles.Length; i1++)
                    {
                        if (files[i].ToLower().EndsWith(seles[i1].ToLower(), StringComparison.Ordinal))
                        {
                            filesSeles.Add(files[i]);
                            break;
                        }
                    }
                }
                fileslist.AddRange(filesSeles);
                keyValuePairs.Add(d, fileslist);
            }
            var paths = Directory.GetDirectories(path);
            for (int i = 0; i < paths.Length; i++)
            {
                Dictionary<string, List<string>> file = GetFilesDicListPath(paths[i], sele);
                foreach (var item in file)
                {
                    if (!keyValuePairs.ContainsKey(item.Key))
                    {
                        keyValuePairs.Add(item.Key, item.Value);
                    }
                }
            }
            return keyValuePairs;
        }

        public static List<string> GetFilesListPath(string path, string sele)
        {
            List<string> fileslist = new List<string>();
            string[] seles = sele.Split(',');
            if (!Directory.Exists(path))
            {
                return null;
            }

            string d = path.Remove(0, path.IndexOf('\\') + 1);
            if (d == "")
            {
                d = path.Replace('\\', ' ');
            }
            //string[] files = Directory.GetFiles(path);
            var di = new DirectoryInfo(path);//文件夹所在目录
            var fc = new FileComparer();
            FileInfo[] fileList = di.GetFiles();

            Array.Sort(fileList, fc);//按文件创建时间排正序
            List<string> files = new List<string>();
            for (int i = 0; i < fileList.Length; i++)
            {
                files.Add(fileList[i].FullName);
            }

            if (files.Count != 0)
            {
                List<string> filesSeles = new List<string>();
                for (int i = 0; i < files.Count; i++)
                {
                    for (int i1 = 0; i1 < seles.Length; i1++)
                    {
                        if (files[i].ToLower().EndsWith(seles[i1].ToLower(), StringComparison.Ordinal))
                        {
                            filesSeles.Add(files[i]);
                            break;
                        }
                    }
                }
                fileslist.AddRange(filesSeles);
            }
            var paths = Directory.GetDirectories(path);
            for (int i = 0; i < paths.Length; i++)
            {
                fileslist.AddRange(GetFilesListPath(paths[i], sele));
            }
            return fileslist;
        }

        public class FileComparer : IComparer
        {
            /// <summary>
            /// 文件排序
            /// </summary>
            /// <param name="o1"></param>
            /// <param name="o2"></param>
            /// <returns></returns>
            int IComparer.Compare(object o1, object o2)
            {
                FileInfo fi1 = o1 as FileInfo;
                FileInfo fi2 = o2 as FileInfo;
                return fi1.CreationTime.CompareTo(fi2.CreationTime);
            }
        }
    }
}