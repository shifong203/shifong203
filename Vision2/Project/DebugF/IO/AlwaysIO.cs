namespace Vision2.Project.DebugF.IO
{
    public class AlwaysIO
    {
        /// <summary>
        /// 等待信号等于Value并保持多少秒
        /// </summary>
        /// <param name="diNumber">DI编号</param>
        /// <param name="timeS">时间秒</param>
        /// <param name="Value">等于的值，默认True</param>
        public static void Always(sbyte diNumber, double timeS, bool Value = true)
        {
            if (diNumber < 0)
            {
                return;
            }
            System.Diagnostics.Stopwatch WatchT = new System.Diagnostics.Stopwatch();
            while (true)
            {
                if (Value != DebugCompiler.GetDoDi().Int[diNumber])
                {
                    WatchT.Restart();
                }
                else
                {
                    WatchT.Start();
                }
                if (timeS != 0 && timeS <= WatchT.ElapsedMilliseconds / 1000)
                {
                    break;
                }
                System.Threading.Thread.Sleep(10);
                if (DebugCompiler.EquipmentStatus == ErosSocket.ErosConLink.EnumEquipmentStatus.已停止)
                {
                    return;
                }
            }
        }

        public sbyte Dnumber { get; set; }
        public double TimeS { get; set; } = 0.1;
        public bool ISValue { get; set; } = true;

        public bool Value
        {
            get
            {
                if (Dnumber <= 32)
                {
                    if (DebugCompiler.GetDoDi() == null)
                    {
                        return false;
                    }
                    return DebugCompiler.GetDoDi().Int[Dnumber];
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        public bool TimeValue
        {
            get
            {
                if (ISValue != valtM)
                {
                    return false;
                }
                else
                {
                    if (this.TimeS != 0 && TimeS <= (double)WatchT.ElapsedMilliseconds / 1000)
                    {
                        return true;
                    }
                }
                return false;
                ;
            }
        }

        public double RunTime
        {
            get
            {
                return WatchT.Elapsed.TotalSeconds;
            }
        }

        public bool Enbelr = true;
        private System.Diagnostics.Stopwatch WatchT = new System.Diagnostics.Stopwatch();
        private bool Runing;
        private bool valtM;

        public void Always()
        {
            if (Runing)
            {
                return;
            }
            Runing = true;
            if (Dnumber < 0)
            {
                return;
            }
            DebugCompiler.GetDoDi().Int.EventDs[Dnumber].EventValueCh += Int_EventValueCh;
            Int_EventValueCh(DebugCompiler.GetDoDi().Int[Dnumber], 0);
        }

        private void Int_EventValueCh(bool value, double timeRun)
        {
            WatchT.Restart();

            if (valtM != value)
            {
                valtM = value;
            }
            //if (ISValue != value)
            //{
            //    TimeValue = false;
            //}
            //else
            //{
            //    if (this.TimeS != 0 && TimeS <= (double)timeRun / 1000)
            //    {
            //        TimeValue = true;
            //    }
            //}
        }
    }
}