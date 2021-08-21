using System;
using System.ComponentModel;
using System.Threading;

namespace Vision2.Project.DebugF.IO
{
    public class C154Cylinder : Vision2.ErosProjcetDLL.Project.INodeNew, ErosSocket.DebugPLC.ICylinder
    {
        /// <summary>
        /// 伸出Q变量名
        /// </summary>
        ///
        [DescriptionAttribute("伸出气缸变量名。"), Category("控制"), DisplayName("伸出Q")]
        public string ProtrudeQ { get; set; }

        /// <summary>
        /// 缩回Q变量名
        /// </summary>
        [DescriptionAttribute("缩回气缸变量名。"), Category("控制"), DisplayName("缩回Q")]
        public string AnastoleQ { get; set; }

        /// <summary>
        /// 伸出I变量名
        /// </summary>
        [DescriptionAttribute("伸出信号变量名。"), Category("控制"), DisplayName("伸出I")]
        public string ProtrudeI { get; set; }

        /// <summary>
        /// 缩回I变量名
        /// </summary>
        [DescriptionAttribute("缩回信号变量名。"), Category("控制"), DisplayName("缩回I")]
        public string AnastoleI { get; set; }

        /// <summary>
        /// 伸出M变量名
        /// </summary>
        [DescriptionAttribute("伸出信号变量名。"), Category("控制"), DisplayName("伸出M")]
        public string ProtrudeM { get; set; }

        /// <summary>
        /// 缩回M变量名
        /// </summary>
        [DescriptionAttribute("缩回信号变量名。"), Category("控制"), DisplayName("缩回M")]
        public string AnastoleM { get; set; }

        /// <summary>
        /// 气缸报警状态
        /// </summary>
        [DescriptionAttribute("缩回信号变量名。"), Category("控制"), DisplayName("气缸报警状态")]
        public string CylinderAlram { get; set; }

        [DescriptionAttribute("单点控制只控制伸出。"), Category("控制"), DisplayName("是否单点控制")]
        public bool ISOne { get; set; }

        /// <summary>
        /// 伸出
        /// </summary>
        /// <param name="isThread">是否等待执行结果</param>
        /// <returns></returns>
        public bool Protrude(bool isThread = true)
        {
            try
            {
                CylinderAlram = "";
                bool done = false;
                if (ToInt())
                {
                    if (!ISOne)
                    {
                        DebugCompiler.GetDoDi().WritDO(AnOut, false);
                    }
                    DebugCompiler.GetDoDi().WritDO(ProtOut, true);

                    Thread thread = new Thread(() =>
                    {
                        Thread.Sleep(1000);
                        int errTime = 0;
                        while (!DebugCompiler.GetDoDi().Int[ProtInt])
                        {
                            Thread.Sleep(10);
                            errTime++;
                            if (errTime > 300)
                            {
                                CylinderAlram = "伸出失败超时";
                                OnAlRam(CylinderAlram);
                                return;
                            }
                        }
                        done = true;
                    });
                    thread.IsBackground = true;
                    thread.Start();

                    if (!isThread)
                    {
                        int errTime = 0;
                        while (!done)
                        {
                            Thread.Sleep(10);
                            errTime++;
                            if (errTime > 300)
                            {
                                OnAlRam(CylinderAlram);
                                return false;
                            }
                        }
                        return true;
                    }
                }
                else
                {
                    CylinderAlram = "伸出失败未设置IO";
                    OnAlRam(CylinderAlram);
                    return false;
                }
            }
            catch (Exception ex)
            {
                //CylinderAlram = "伸出失败"+ex.Message;
                throw (new Exception(ex.Message));
            }
            OnAlRam(CylinderAlram);
            return false;
        }

        /// <summary>
        /// 缩回
        /// </summary>
        /// <param name="isThread">是否等待执行结果</param>
        /// <returns></returns>
        public bool Anastole(bool isThread = true)
        {
            try
            {
                CylinderAlram = "";
                bool done = false;
                if (ToInt())
                {
                    DebugCompiler.GetDoDi().WritDO(ProtOut, false);
                    if (!ISOne)
                    {
                        DebugCompiler.GetDoDi().WritDO(AnOut, true);
                    }
                    if (DebugCompiler.GetDoDi().Int[AnI])
                    {
                        return true;
                    }
                    Thread thread = new Thread(() =>
                    {
                        int errTime = 0;
                        Thread.Sleep(1000);
                        while (!DebugCompiler.GetDoDi().Int[AnI])
                        {
                            Thread.Sleep(10);
                            errTime++;
                            if (errTime > 300)
                            {
                                CylinderAlram = "缩回失败超时";
                                OnAlRam(CylinderAlram);
                                return;
                            }
                        }
                        done = true;
                    });
                    thread.IsBackground = true;
                    thread.Start();
                    if (!isThread)
                    {
                        int errTime = 0;
                        Thread.Sleep(1000);
                        while (!done)
                        {
                            Thread.Sleep(10);
                            errTime++;
                            if (errTime > 300)
                            {
                                OnAlRam(CylinderAlram);
                                return false;
                            }
                        }
                        return true;
                    }
                }
                else
                {
                    CylinderAlram = "缩回失败未设置IO";
                    OnAlRam(CylinderAlram);
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message));
            }
            OnAlRam(CylinderAlram);
            return false;
        }

        private int AnI = -10;
        private int AnOut = -10;

        private int ProtInt = -10;

        private int ProtOut = -10;

        public bool ToInt()
        {
            if (int.TryParse(ProtrudeQ, out ProtOut) && int.TryParse(ProtrudeI, out ProtInt) &&
                (int.TryParse(AnastoleQ, out AnOut) || ISOne) && int.TryParse(AnastoleI, out AnI))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获得缩回
        /// </summary>
        /// <returns></returns>
        public bool AnValue
        {
            get
            {
                int.TryParse(AnastoleI, out AnI);
                if (DebugCompiler.GetDoDi() == null)
                {
                    return false;
                }
                return DebugCompiler.GetDoDi().Int[AnI];
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool PrValue
        {
            get
            {
                int.TryParse(ProtrudeI, out ProtInt);
                if (DebugCompiler.GetDoDi() == null)
                {
                    return false;
                }
                return DebugCompiler.GetDoDi().Int[ProtInt];
            }
        }
    }
}