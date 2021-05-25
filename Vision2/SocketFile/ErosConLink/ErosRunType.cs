using System;

namespace ErosSocket.ErosConLink
{
    public class ErosRunType
    {
        /// <summary>
        /// 任务类
        /// 根据任务类型执行任务
        /// </summary>
        public class StratRunType
        {
            /// <summary>
            /// 任务类型
            /// </summary>
            public string RunType { get; set; }

            /// <summary>
            /// 任务间隔
            /// </summary>
            public System.Timers.Timer RunTime { get; set; }

            /// <summary>
            /// 目标次数
            /// </summary>
            public Decimal SVICD { get; set; }

            /// <summary>
            /// 执行次数
            /// </summary>
            public Decimal PVICD { get; set; }

            /// <summary>
            /// 任务状态
            /// </summary>
            public string Srewrw { get; set; }

            /// <summary>
            /// 执行
            /// </summary>
            public bool StrataOn { get; set; }

            /// <summary>
            /// 暂停
            /// </summary>
            public bool Perang { get; set; }

            /// <summary>
            /// 关闭标识
            /// </summary>
            private bool closeBool;

            /// <summary>
            /// 委托
            /// </summary>
            public delegate void VoidHandlerDel(); //定义委托类型

            /// <summary>
            /// 任务开始事件
            /// </summary>
            public event VoidHandlerDel EventStratOn;

            /// <summary>
            /// 任务执行中事件
            /// </summary>
            public event VoidHandlerDel EventRuning;

            /// <summary>
            /// 任务结束事件
            /// </summary>
            public event VoidHandlerDel EventDone;

            /// <summary>
            /// 默认
            /// </summary>
            public StratRunType()
            {
                this.closeBool = this.Perang = this.StrataOn = false;
                this.PVICD = 0;
            }

            /// <summary>
            /// 默认间隔执行的任务构造
            /// </summary>
            /// <param name="time">周期</param>
            /// <param name="TimeAutoReset">循环次数，0无限循环</param>
            /// <param name="eventStratOn"></param>
            /// <param name="eventRuning"></param>
            /// <param name="eventDone"></param>
            public StratRunType(Double time, Decimal TimeNum, VoidHandlerDel eventStratOn, VoidHandlerDel eventRuning, VoidHandlerDel eventDone)
            {
                this.closeBool = this.Perang = this.StrataOn = false;
                this.PVICD = 0;
                this.EventStratOn += eventStratOn;
                this.EventRuning += eventRuning;
                this.EventDone += eventDone;//注册事件
                this.OnEventStratOn();//首次任务
                this.RunTime = new System.Timers.Timer();
                this.RunTime.Interval = time;
                this.RunTime.Elapsed += RunTime_Elapsed;
                this.RunTime.AutoReset = true;
                this.SVICD = TimeNum;
            }

            /// <summary>
            /// 任务开始
            /// </summary>
            protected virtual void OnEventStratOn()      // 触发事件私有方法
            {
                if (this.EventStratOn != null)
                {
                    this.EventStratOn(); /* 事件被触发 */
                }
                else
                {
                }
            }

            /// <summary>
            /// 任务中执行
            /// </summary>
            protected virtual void OnRuning()      // 触发事件私有方法
            {
                if (this.EventRuning != null)
                {
                    this.EventRuning(); /* 事件被触发 */
                }
                else
                {
                }
            }

            /// <summary>
            ///任务结束执行
            /// </summary>
            protected virtual void OnEventDone()      // 触发事件私有方法
            {
                if (this.EventDone != null)
                {
                    this.EventDone(); /* 事件被触发 */
                }
                else
                {
                }
            }

            private void RunTime_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
            {
                EventRuning();
                if (SVICD == 0)
                {
                    return;
                }
                PVICD++;
                if (SVICD <= PVICD)
                {
                    this.RunTime.Stop();
                    this.OnEventDone();
                }
            }

            /// <summary>
            /// 执行后台任务虚方法
            /// </summary>
            public virtual void RunStrat<T>(T data, T data2)
            {
            }

            public void Close()
            {
                closeBool = true;
                RunTime.Close();
            }
        }
    }
}