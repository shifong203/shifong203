namespace Vision2.Project.DebugF.IO
{
    public abstract class Run_project
    {
        public abstract bool Initial();

        public abstract bool Run();

        public abstract bool SetHome();

        public abstract bool Stop();

        public abstract bool Pause();

        public bool Pauseing;

        public abstract bool Cont();

        public abstract void Reset();

        public bool IsInitialBool { get; set; }

        public RunCodeStr RunCodeT { get; set; } = new RunCodeStr() { Name = "运行" };

        public RunCodeStr HomeCodeT { get; set; } = new RunCodeStr() { Name = "初始化" };

        public RunCodeStr StopCodeT { get; set; } = new RunCodeStr() { Name = "停止" };

        public RunCodeStr CPKCodeT { get; set; } = new RunCodeStr() { Name = "CPK" };

        /// <summary>
        /// 回原点
        /// </summary>
        public bool Homeing { get; set; }

        /// <summary>
        /// 回原点完成
        /// </summary>

        public bool HomeDone { get; set; }

        /// <summary>
        /// 运行中
        /// </summary>
        public bool Runing { get; set; }
    }
}