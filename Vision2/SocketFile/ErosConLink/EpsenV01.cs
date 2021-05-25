namespace ErosSocket.ErosConLink
{
    public class EpsenV01 : SocketClint
    {
        //public override void OnEventArge(byte[] key)
        //{
        //    base.OnEventArge(key);
        //    //Tram(Encoding.UTF8.GetString(key));
        //}
        public EpsenV01()
        {
            PassiveTextBoxEvent += EpsenV01_PassiveTextBoxEvent;
        }

        private System.Windows.Forms.TextBoxBase EpsenV01_PassiveTextBoxEvent(System.Text.Encoding key, byte[] buffrs)
        {
            return ErosProjcetDLL.Project.AlarmText.ThisF.richTextBox1;
        }

        public override void Receive()
        {
            base.Receive();
        }
    }
}