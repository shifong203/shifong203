using System.ComponentModel;
using System.Text;
using System.Threading;

namespace ErosSocket.ErosConLink
{
    public class SECS_GEM : SocketClint
    {
        public SECS_GEM()
        {
            PassiveEvent += SECS_GEM_PassiveEvent;
        }

        private string SECS_GEM_PassiveEvent(byte[] key, SocketClint socket, System.Net.Sockets.Socket clint)
        {
            string datas = Encoding.UTF8.GetString(key);
            if (datas == "Linktest.req.")
            {
                this.Send("Linktest.rsp.");
                this.Send("Linktest.req.");
            }
            return "";
        }

        [DisplayNameAttribute("心跳周期S"), Category("运行参数")]
        /// <summary>
        ///
        /// </summary>
        public int CycleTime { get; set; } = 20;

        public override bool AsynLink(bool isCycle)
        {
            return base.AsynLink();
        }

        public override bool GetValues()
        {
            this.Send("Linktest.rsp.");
            Thread.Sleep(CycleTime * 1000);
            return false;
        }


    }
}