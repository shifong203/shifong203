

namespace Vision2.EPSON_Robot_Remote_TCPIP
{
    internal class EPSON_Robot_TCPIP : ErosSocket.ErosConLink.SocketClint
    {


        public void link()
        {

        }



        public override void Receive()
        {
        }

        public void Login(string passWrod)
        {
            if (this.Send("Login" + passWrod))
            {
                Receive();
            }
        }
    }
}