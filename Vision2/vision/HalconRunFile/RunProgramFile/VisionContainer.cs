using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Vision2.ErosProjcetDLL.Project;
using Vision2.vision.HalconRunFile.Controls;
using static Vision2.vision.HalconRunFile.RunProgramFile.HalconRun;

namespace Vision2.vision.HalconRunFile.RunProgramFile
{
    public class VisionContainer : RunProgram, InterfaceVisionControl
    {    /// <summary>
         /// 刷新程序事件
         /// </summary>
        public event DelegateAddRun UpHalconRunProgram;

        public override Control GetControl(HalconRun halcon )
        {
            return new  VisionContainerControl(this);
        }

        public override RunProgram UpSatrt<T>(string path)
        {

            RunProgram visionConta=     base.ReadThis<VisionContainer>(path);
            VisionContainer visionContainer = visionConta as VisionContainer;

            try
            {
                if (visionContainer == null)
                {
                    return null;
                }
                //halcon.TiffeOffsetImageEX
                if (path.Contains("."))
                {
                    path = Path.GetDirectoryName(path);
                }
                if (visionContainer.ListRunName == null || visionContainer.ListRunName.Count == 0)
                {
                    if (Directory.Exists(path + "\\" + visionContainer.Name))
                    {
                        string[] itmeName = Directory.GetDirectories(path + "\\" + visionContainer.Name);

                        for (int i = 0; i < itmeName.Length; i++)
                        {
                            string[] itmesStr = Directory.GetFiles(itmeName[i], "*.dicHtuole");
                            if (itmesStr.Length != 0)
                            {
                                itmeName[i] = itmesStr[0];
                            }
                        }
                        for (int i = 0; i < itmeName.Length; i++)
                        {
                            if (File.Exists(itmeName[i]))
                            {
                                string strdata = File.ReadAllText(itmeName[i]);
                                Newtonsoft.Json.Linq.JObject jo = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(strdata);    //字段对象
                                visionContainer.ListRunName.Add(jo["Name"].ToString(), jo["Type"].ToString());
                            }
                        }
                    }
                }
                //path = path + "\\" + visionContainer.Name + "\\";
                path = Path.GetDirectoryName(path);
                var det = visionContainer.ListRun.ToArray();
                Assembly assembly = Assembly.GetExecutingAssembly(); // 获取当前程序集
                foreach (var item in visionContainer.ListRunName)
                {
                    try
                    {
                        if (item.Value != null)
                        {
                            string ntype = item.Value.Split('.')[item.Value.Split('.').Length - 1];
                            dynamic obj = assembly.CreateInstance(visionContainer.GetType().Namespace + "." + ntype); // 创建类的实例                            
                            if (obj != null)
                            {
                                visionContainer.ListRun[item.Key] = obj.UpSatrt<RunProgram>(path+ "\\"+item.Key + "\\" + item.Key);
                            }
                            else
                            {
                                obj = assembly.CreateInstance(item.Value); // 创建类的实例     
                                visionContainer.ListRun[item.Key] = obj.UpSatrt<RunProgram>(path + "\\" + item.Key + "\\" + item.Key);
                            }
                            if (visionContainer.ListRun[item.Key] == null)
                            {
                                visionContainer.ListRun[item.Key] = obj;
                            }
                            else
                            {
                                obj.Dispose();
                            }
                            visionContainer.ListRun[item.Key].SetPThis(this.GetPThis());
                            visionContainer.ListRun[item.Key].Name = item.Key;
                            visionContainer.ListRun[item.Key].SetPd(this);
                    
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(item.Key + "读取错误:" + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取地址：" + path + "错误！", ex.Message);
            }
            return visionConta;
        }

        public override void SaveThis(string path)
        {
            base.SaveThis(path);
            Dictionary<string, RunProgram> itemS = new Dictionary<string, RunProgram>();

            foreach (var item in this.ListRun)
            {
                try
                {
                    if (!ListRunName.ContainsKey(item.Value.Name))
                    {
                        ListRunName.Add(item.Value.Name, item.Value.Type);
                    }
                    else
                    {
                        ListRunName[item.Value.Name] = item.Value.Type;
                    }
                    itemS.Add(item.Value.Name, item.Value);
                    item.Value.SaveThis(path +"\\"+ this.Name + "\\");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(item.Key + ":保存失败！");
                }
            }
            this.ListRun = itemS;


        }

        public Dictionary<string, bool> keyValues = new Dictionary<string, bool>();
        public override bool RunHProgram( OneResultOBj oneResultOBj, out List<OneRObj> oneRObjs, int runID = 0)
        {
            oneRObjs = new List<OneRObj>();
            if (keyValues==null)
            {
                keyValues = new Dictionary<string, bool>();

            }
            keyValues.Clear();
            //if (name!=null)
            //{
            //    if (ListRun.ContainsKey(name))
            //    {
            //        ListRun[name].RunHProgram(halcon, oneResultOBj,runID);
            //    }
            //}
            //else
            //{
                foreach (var item in ListRun)
                {
                    keyValues.Add(item.Key, item.Value.Run(oneResultOBj));
                    if (!keyValues[item.Key])
                    {
                        ResltBool = false;
                    }
                }
            //}
            
        
            return ResltBool;
        }
        [Browsable(false)]
        public Dictionary<string, string> ListRunName { get; set; } = new Dictionary<string, string>();

        Dictionary<string, RunProgram> ListRun = new Dictionary<string, RunProgram>();


        public Dictionary<string, RunProgram> GetRunProgram()
        {
            return ListRun;
        }
        public ContextMenuStrip GetNewPrajetContextMenuStrip(string name)
        {
            AddRun("添加模板", "模板", typeof(ModelVision));
            AddRun("添加测量", "测量1", typeof(MeasureMlet));
            AddRun("添加图像扫码", "扫码1", typeof(QRCode));
            AddRun("添加二值化检测", "二值化检测1", typeof(Calculate));
            AddRun("添加OCR识别", "OCR识别1", typeof(Text_Model));
            AddRun("添加连接器识别", "连接器识别", typeof(PinT));
            AddRun("添加焊点检测", "焊点检测", typeof(Welding_Spot));
            AddRun("添加焊线检测", "焊线检测", typeof(Wire_Solder));
            AddRun("添加颜色识别", "颜色检测", typeof(Color_Detection));
            AddRun("添加擦针检测", "擦针检测", typeof(Pin_Round_brush_needlecs));
            AddRun("添加PCB检测", "PCB", typeof(PCBA));
            AddRun("添加元件", "元件", typeof(VisionContainer));
            if (contextMenuTT.Items.Find("删除", false).Length == 0)
            {
                ToolStripItem toolStripItemw = contextMenuTT.Items.Add("删除");
                toolStripItemw.Click += ToolStripItemw_Click1;
                toolStripItemw.Name = toolStripItemw.Text;
                void ToolStripItemw_Click1(object sender, EventArgs e)
                {
                    try
                    {
                        //ToolStrip toolStrip  =toolStripItemw.GetCurrentParent();
                        Control control = (sender as ToolStripItem).GetCurrentParent();
                        if ((sender as ToolStripItem).Tag is List<string>)
                        {
                            List<string> list = (sender as ToolStripItem).Tag as List<string>;
                            for (int i = 0; i < list.Count; i++)
                            {
                                if (this.ListRun.ContainsKey(list[i]))
                                {
                                    this.ListRun.Remove(list[i]);

                                    this.ListRunName.Remove(list[i]);
                                }
                                if (this.ListRunName.ContainsKey(list[i]))
                                {
                                    this.ListRunName.Remove(list[i]);
                                }
                            }
                            OnUpHalconRunPro(null);
                        }
                        else
                        {
                            //Vision.GetHimageList().Remove(this.Name);
                            //Vision.Instance.ListHalconName.Remove(this.Name);
                            //Vision.Instance.UpProjectNode();
                        }


                    }
                    catch (Exception)
                    {
                    }
                    //弹出带输入
                }
            }
            if (contextMenuTT.Items.Find("同步到库", false).Length == 0)
            {
                ToolStripItem toolStripItemw = contextMenuTT.Items.Add("同步到库");
                toolStripItemw.Click += ToolStripItemw_Click1;
                toolStripItemw.Name = toolStripItemw.Text;
                void ToolStripItemw_Click1(object sender, EventArgs e)
                {
                    try
                    {
                        //ToolStrip toolStrip  =toolStripItemw.GetCurrentParent();
                        Control control = (sender as ToolStripItem).GetCurrentParent();
                        if ((sender as ToolStripItem).Tag is List<string>)
                        {
                            List<string> list = (sender as ToolStripItem).Tag as List<string>;
                            for (int i = 0; i < list.Count; i++)
                            {
                                this.ListRun[list[i]].SaveThis(ErosProjcetDLL.Project.ProjectINI.In.ProjectPathRun + "\\Library\\Vision\\");
                                if (!Vision.Instance.ListLibrary.ContainsKey(list[i]))
                                {
                                    Vision.Instance.ListLibrary.Add(list[i], this.ListRun[list[i]].GetType().ToString());
                                }
                                ErosProjcetDLL.Excel.Npoi.WritePrivateProfileString("视觉库", list[i], this.ListRun[list[i]].GetType().ToString(), ProjectINI.In.ProjectPathRun + "\\Library\\Vision\\Library.ini");
                            }
                        }
                        else
                        {

                        }
                    }
                    catch (Exception)
                    {
                    }
                    //弹出带输入
                }
            }
            if (contextMenuTT.Items.Find("从库导入", false).Length == 0)
            {
                ToolStripItem toolStripItemw = contextMenuTT.Items.Add("从库导入");
                toolStripItemw.Click += ToolStripItemw_Click1;
                toolStripItemw.Name = toolStripItemw.Text;
                void ToolStripItemw_Click1(object sender, EventArgs e)
                {
                    try
                    {
                        LibraryFormAdd libraryFormAdd = new LibraryFormAdd(this.GetPThis());
                        libraryFormAdd.ShowDialog();

                    }
                    catch (Exception ex)
                    {
                    }
                    //弹出带输入
                }
            }
            return contextMenuTT;
        }
        void AddRun(string newName, string pRName, Type type)
        {
            Vision.AddRunNames(new string[]
                        {
                    newName,
                        });
            if (Vision.FindRunName(newName))
            {
                if (contextMenuTT.Items.Find(newName, false).Length == 0)
                {
                    ToolStripItem toolStripItemw = contextMenuTT.Items.Add(newName);
                    toolStripItemw.Name = toolStripItemw.Text;
                    toolStripItemw.Click += ToolStripItemwT_Click;
                    void ToolStripItemwT_Click(object sender, EventArgs e)
                    {
                        try
                        {
                            //弹出带输入的
                            string sd = Interaction.InputBox("请输入程序名称", "创建程序", pRName, 100, 100);
                            if (sd.Length == 0)
                            {
                                return;
                            }
                            dynamic obj = type.Assembly.CreateInstance(type.FullName);
                            obj.Name = sd;
                            this.AddListRun(sd, obj);
                            OnUpHalconRunPro(obj);
                        }
                        catch (Exception)
                        {

                        }

                    }
                }
            }
            else
            {
                contextMenuTT.Items.RemoveByKey(newName);
            }
        }

    
        /// <summary>
        /// 刷新程序事件
        /// </summary>
        private void OnUpHalconRunPro(RunProgram run)
        {
            UpHalconRunProgram?.Invoke(this.GetPThis(),run);
        }
        /// <summary>
        ///添加程序
        /// </summary>
        /// <param name="name"></param>
        /// <param name="run"></param>
        public RunProgram AddListRun(string name, RunProgram run)
        {
            try
            {
                if (this.ListRun.ContainsKey(name))
                {
                    string dsts = string.Empty;
                    int ds = ProjectINI.GetStrReturnInt(name, out dsts);
                strt:
                    if (this.ListRun.ContainsKey(dsts + (++ds)))
                    {
                        goto strt;
                    }
                    DialogResult dr = MessageBox.Show(name + ":已存在!是否新建《" + dsts + ds + "》？", "新建程序", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr != DialogResult.OK)
                    {
                        return null;
                    }
                    string jsonStr = JsonConvert.SerializeObject(ListRun[name]);
                    dsts = dsts + ds;
                    object DA = JsonConvert.DeserializeObject(jsonStr, run.GetType());
                    ListRun.Add(dsts, DA as RunProgram);
                    ListRun[dsts].Name = dsts;
                    float maxd = 1;
                    foreach (var item in ListRun)
                    {
                        if (item.Value.CDID > maxd)
                        {
                            maxd = item.Value.CDID + 1;
                        }
                    }
                    ListRun[dsts].CDID = maxd;
                    run.Name = dsts ;
                }
                else
                {
                    this.ListRun.Add(name, run);
                
                }
                ListRun[run.Name].SetPThis(this.GetPThis());
                ListRun[run.Name].SetPd(this);
                if (!ListRunName.ContainsKey(run.Name))
                {
                    ListRunName.Add(run.Name, run.GetType().ToString());
                }
            }
            catch (Exception)
            {

            }
            if (Node != null)
            {
                this.UpProjectNode(Node.Parent);
            }


            return run;
        }
    }
}
