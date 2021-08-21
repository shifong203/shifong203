using System.Collections.Generic;

namespace Vision2.ErosProjcetDLL.Code
{
    public interface ICode
    {
        Dictionary<string, string> Ketd { get; set; }
        Dictionary<string, string> PModet { get; set; }
        Dictionary<string, string> PProgma { get; set; }

        Tmo GetTmo(string code);

        string CodeStr { get; set; }
        List<string> Lines { get; set; }
    }

    public class Tmo
    {
        public string[] Ketd = new string[] { };
        public string[] PModet = new string[] { };
        public string[] PProgma = new string[] { };
    }
}