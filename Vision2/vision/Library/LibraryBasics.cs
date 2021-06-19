using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vision2.vision.HalconRunFile.RunProgramFile;

namespace Vision2.vision.Library
{
  public  class LibraryBasics
  {

      public static string PathStr = ErosProjcetDLL.Project.ProjectINI.In.ProjectPathRun + "\\Library\\Vision\\" + Project.formula.Product.ProductionName + "\\";

      public static string RunPath = ErosProjcetDLL.Project.ProjectINI.In.ProjectPathRun + "\\Library\\Vision\\";
      public static Dictionary<string, RunProgram> ReadDic(string paths)
      {
            Dictionary<string, RunProgram> keyValuePairs = new Dictionary<string, RunProgram>();
            Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集
            string staSET = "                                                          ";
            ErosProjcetDLL.Excel.Npoi.GetPrivateProfileString("视觉库", null, "", staSET, 500, paths + "\\Library.ini");
            string[] vs = staSET.ToString().Split('\0');
            StringBuilder stdt = new StringBuilder(100);
          
            for (int i = 0; i < vs.Length; i++)
            {
                if (vs[i].Trim() != "")
                {
                    ErosProjcetDLL.Excel.Npoi.GetPrivateProfileString("视觉库", vs[i], "", stdt, 500, paths + "\\Library.ini");
                    string taext= stdt.ToString();
                    if (taext=="")
                    {
                        continue;
                    }
                    string name = vs[i];
                    try
                    {
                        string ntype = taext.Split('.')[taext.Split('.').Length-1];
                        dynamic obj = assembly.CreateInstance(taext); // 创建类的实例                            
                        if (obj == null)
                        {
                             obj = assembly.CreateInstance(ntype); // 创建类的实例    
                        }
                        keyValuePairs.Add(ntype, obj.UpSatrt<RunProgram>(paths + "\\" + name + "\\" + name));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(name + "读取错误:" + ex.Message);
                    }
                }
            }
            return keyValuePairs;
        }
      public static void SaveLibraryVision(string path, Dictionary<string, RunProgram> dicLibrary)
      {
            foreach (var item in dicLibrary)
            {
                item.Value.SaveThis(path);
                ErosProjcetDLL.Excel.Npoi.WritePrivateProfileString("视觉库", item.Value.Name, item.Value.GetType().ToString(), path + "\\Library.ini");
            }
      }


    }






}
