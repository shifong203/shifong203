using System.ComponentModel;
using System.Drawing.Design;
using Vision2.ErosProjcetDLL.Project;

namespace ErosSocket.DebugPLC.PLC
{
    public class PLCIO : INodeNew
    {
        public bool ISOut { get; set; }

        public string TypeStr { get; set; }
        [DescriptionAttribute("链接输出地址。"), Category("触发器"), DisplayName("输出地址")]
        [Editor(typeof(ErosSocket.ErosConLink.LinkNameAddreControl.Editor), typeof(UITypeEditor))]
        public string AddID { get; set; }

    }
}
