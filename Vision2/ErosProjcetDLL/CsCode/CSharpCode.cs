using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Vision2.ErosProjcetDLL.CsCode
{
    public class CSharpCode
    {
        private Assembly objAssembly;
        private CompilerParameters objCompilerParameters;
        private dynamic objHelloWorld;

        public List<string> ListDll { get; set; } = new List<string> { "System.dll", "System.Windows.Forms.dll" ,
             "HalconDotNet.dll"};

        /// <summary>
        /// 编译程序集
        /// </summary>
        /// <param name="source">代码</param>
        /// <param name="errString">错误信息</param>
        /// <returns>程序集</returns>
        public Assembly CompileAssmblyFromSource(string source, out string errString)
        {
            errString = "";
            objAssembly = null;
            // 1.CSharpCodePrivoder

            CSharpCodeProvider objCSharpCodePrivoder = new CSharpCodeProvider();

            // 2.ICodeComplier
            ICodeCompiler objICodeCompiler = objCSharpCodePrivoder.CreateCompiler();

            // 3.CompilerParameters
            if (objCompilerParameters == null)
            {
                objCompilerParameters = new CompilerParameters();
            }

            objCompilerParameters.ReferencedAssemblies.AddRange(ListDll.ToArray());
            objCompilerParameters.GenerateExecutable = false;
            objCompilerParameters.GenerateInMemory = true;
            // 4.CompilerResults
            CompilerResults cr = objICodeCompiler.CompileAssemblyFromSource(objCompilerParameters, source);

            if (cr.Errors.HasErrors)
            {
                errString += "编译错误:" + Environment.NewLine;
                foreach (CompilerError err in cr.Errors)
                {
                    errString += err.ErrorText;
                }
            }
            else
            {
                objAssembly = cr.CompiledAssembly;
                errString += "编译成功;" + Environment.NewLine;
                return objAssembly;
            }
            return null;
        }

        public object DebugCode(string className, string method, out string errString)
        {
            errString = "";
            if (method == "")
            {
                errString += "未输入方法名" + Environment.NewLine;
            }
            if (className == "")
            {
                errString += "未输入类名" + Environment.NewLine;
                return null;
            }
            if (objAssembly != null)
            {
                //通过反射，调用HelloWorld的实例
                objHelloWorld = objAssembly.CreateInstance(className);
                if (objHelloWorld != null)
                {
                    if (method == "New")
                    {
                        return objHelloWorld;
                    }
                    MethodInfo objMI = objHelloWorld.GetType().GetMethod(method);
                    return objMI.Invoke(objHelloWorld, null);
                }
                errString += "未找到对象" + Environment.NewLine;
            }
            return null;
        }

        public object DebugCode(string className, string method, out string errString, params object[] data)
        {
            errString = "";
            if (method == "")
            {
                errString += "未输入方法名" + Environment.NewLine;
            }
            if (className == "")
            {
                errString += "未输入类名" + Environment.NewLine;
                return null;
            }
            if (objAssembly != null)
            {
                //通过反射，调用HelloWorld的实例
                if (objHelloWorld == null)
                {
                    objHelloWorld = objAssembly.CreateInstance(className);
                }
                if (objHelloWorld != null)
                {
                    List<Type> sd = new List<Type>();
                    ParameterModifier[] parameterModifiers = new ParameterModifier[data.Length];
                    for (int i = 0; i < data.Length; i++)
                    {
                        sd.Add(data[i].GetType());
                        parameterModifiers[i] = new ParameterModifier() { };
                    }
                    MethodInfo objMI = objHelloWorld.GetType().GetMethod(method, sd.ToArray());
                    if (objMI != null)
                    {
                        return objMI.Invoke(objHelloWorld, data);
                    }
                }
                errString += "未找到对象" + Environment.NewLine;
            }
            return null;
        }

        public bool AddDll(string dll)
        {
            if (ListDll.Contains(dll))
            {
                return false;
            }
            objCompilerParameters.ReferencedAssemblies.Add(dll);
            ListDll.Add(dll);
            return false;
        }

        public bool AddDll(string[] dlls)
        {
            for (int i = 0; i < dlls.Length; i++)
            {
                if (ListDll.Contains(dlls[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static string GenerateCode()
        {
            string code = "";
            if (File.Exists(@"TextFile1.txt"))
            {
                code = File.ReadAllText(@"TextFile1.txt");
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("using System;" + Environment.NewLine);
                sb.Append("using System.Windows.Forms;" + Environment.NewLine);
                sb.Append("using HalconDotNet;" + Environment.NewLine);
                sb.Append("namespace DynamicCodeGenerate");
                sb.Append(Environment.NewLine);
                sb.Append("{");
                sb.Append(Environment.NewLine);
                sb.Append("    public class Halcon");
                sb.Append(Environment.NewLine);
                sb.Append("    {");
                sb.Append(Environment.NewLine);
                sb.Append("        public string OutPut(string data)");
                sb.Append(Environment.NewLine);
                sb.Append("        {");
                sb.Append(Environment.NewLine);
                sb.Append("MessageBox.Show(\"Hello world!\");" + Environment.NewLine);
                sb.Append("             return \"Hello world!\";");
                sb.Append(Environment.NewLine);
                sb.Append("        }");
                sb.Append(Environment.NewLine);
                sb.Append("        public string    ReadImage(string path,HTuple hWindowHalconID)" + Environment.NewLine);
                sb.Append("        {" + Environment.NewLine);
                //    HOperatorSet.ReadImage(out this.imag, path);
                //this.ResultOBj = new HalconResult();
                //if (Widgth < 0)
                //{
                //HOperatorSet.GetImageSize(this.Image(), out HTuple width, out HTuple heigth);
                //this.Width = width;
                //this.Height = heigth;
                //HOperatorSet.SetPart(this.hWindowHalcon(), 0, 0, Height - 1, Width - 1);
                //HOperatorSet.DispObj(this.Image(), this.hWindowHalcon());
                //Hobjet Image
                sb.Append("             HObject Image=new HObject();" + Environment.NewLine);
                sb.Append("             HOperatorSet.ReadImage(out Image, path); HTuple width=new     HTuple(); HTuple heigth=new     HTuple();" + Environment.NewLine);
                sb.Append("             HOperatorSet.GetImageSize(Image, out  width, out  heigth);" + Environment.NewLine);
                sb.Append("             HOperatorSet.SetPart(hWindowHalconID, 0, 0, heigth - 1, width - 1);" + Environment.NewLine);
                sb.Append("             HOperatorSet.DispObj(Image, hWindowHalconID);" + Environment.NewLine);
                sb.Append("             return \"Hello world!\";" + Environment.NewLine);
                sb.Append("        }");
                sb.Append(Environment.NewLine);
                sb.Append("    }");
                sb.Append(Environment.NewLine);
                sb.Append("}");

                code = sb.ToString();
                //Console.WriteLine(code);
                //Console.WriteLine();
            }

            return code;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public Assembly GetAssembly()
        {
            return objAssembly;
        }
    }
}