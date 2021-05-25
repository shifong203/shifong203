
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Vision2.Project.DebugF
{
    public class DllUers
    {
        #region 声明动态载入DLL的参数
        byte[] filesByte;
        Assembly assembly;
        Type type;
        #endregion
        [DescriptionAttribute("外部Dll地址。"), Category("外部Dll"), DisplayName("外部Dll地址")]
        public string _Path { get; set; } = "";
        [DescriptionAttribute("Dll名称。"), Category("外部Dll"), DisplayName("名称")]
        public string Name { get; set; } = "";
        [DescriptionAttribute("是否启用外部Dll。"), Category("外部Dll"), DisplayName("启用Dll")]
        public bool IsEn { get; set; }

        [DescriptionAttribute("生成类集合。"), Category("外部Dll"), DisplayName("生成类集合")]
        public Dictionary<string, string> DicObjClass { get; set; } = new Dictionary<string, string>();

        Dictionary<string, dynamic> keyObjDlls = new Dictionary<string, dynamic>();
        /// <summary>
        /// DLL动态对象
        /// </summary>
        dynamic ObjDll { get; set; }

        public dynamic GetObjDll()
        {
            return ObjDll;
        }

        public Assembly GetDllS()
        {
            if (assembly == null)
            {
                if (!File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + "//" + Name))
                {
                    File.Copy(_Path, Path.GetDirectoryName(Application.ExecutablePath) + "//" + Name);
                }
                else if (File.Exists(_Path))
                {
                    File.Copy(_Path, Path.GetDirectoryName(Application.ExecutablePath) + "//" + Name, true);
                }
                filesByte = File.ReadAllBytes(Path.GetDirectoryName(Application.ExecutablePath) + "//" + Name);
                assembly = Assembly.Load(filesByte);
            }
            return assembly;
        }
        [DescriptionAttribute("调用方法名称。"), Category("外部Dll"), DisplayName("调用方法名")]
        public string MetHodStr { get; set; }

        dynamic RetnMetHod { get; set; }

        /// <summary>
        /// 加载DLL并生成
        /// </summary>
        public dynamic LoadDll(string ClassName)//
        {
            string pathst = ClassName;
            SafeInvoke(() =>
            {

                //if (!File.Exists(Path.GetDirectoryName(Application.ExecutablePath) + "//" + Name))
                //{
                //    File.Copy(pathst, Path.GetDirectoryName(Application.ExecutablePath) + "//" + Name);
                //}
                //else if (File.Exists(pathst))
                //{
                //    File.Copy(pathst, Path.GetDirectoryName(Application.ExecutablePath) + "//" + Name, true);
                //}
                if (File.Exists(pathst))
                {
                    filesByte = File.ReadAllBytes(pathst);
                    assembly = Assembly.Load(filesByte);
                }

            });
            return assembly;
        }
        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="ClassName"></param>
        /// <returns></returns>
        public dynamic New(string ClassName)
        {

            type = assembly.GetType(ClassName);
            if (type != null)
            {
                ObjDll = System.Activator.CreateInstance(type);

            }
            else
            {
                ObjDll = null;
                MessageBox.Show("创建失败");
            }
            return ObjDll;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dllText"></param>
        /// <returns></returns>
        public dynamic LoadText(string dllText)
        {
            dynamic nd = null;
            SafeInvoke(() =>
            {
                //foreach (var item in dllText.Split(';'))    
                //{
                //    assembly.GetType(item.);
                //}
            });
            return nd;
        }

        /// <summary>
        /// 通用的异常处理
        /// </summary>
        /// <param name="act">对应任何的逻辑</param>
        public static void SafeInvoke(Action act)
        {
            try
            {
                act.Invoke();
            }
            catch (Exception ex)//按异常类型区分处理
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 调用指定名称的方法
        /// </summary>
        /// <param name="MetHodName">方法名</param>
        /// <param name="Params">参数</param>
        public dynamic MetHod(string MetHodName, params object[] Params)
        {
            try
            {
                if (type != null)
                {

                    return type.InvokeMember(MetHodName, BindingFlags.Default | BindingFlags.InvokeMethod, null, ObjDll, new object[] { });
                    //MethodInfo Mymethodinfoa = type.GetMethod(MetHodName);
                    //if (Mymethodinfoa!=null)
                    //{
                    //    RetnMetHod= Mymethodinfoa.Invoke(ObjDll, Params);
                    //}
                }
            }
            catch (Exception)
            {
            }
            return null;
        }
    }
}
