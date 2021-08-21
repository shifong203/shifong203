using System.Collections.Generic;
using System.Linq;

namespace Vision2.ErosProjcetDLL.Code
{
    public class Code : ICode
    {
        public Dictionary<string, string> Ketd { get { return ketd; } set { ketd = value; } }
        private Dictionary<string, string> ketd = new Dictionary<string, string>();
        public Dictionary<string, string> PModet { get { return pModet; } set { pModet = value; } }
        private Dictionary<string, string> pModet = new Dictionary<string, string>();
        public Dictionary<string, string> PProgma { get { return pProgma; } set { pProgma = value; } }
        private Dictionary<string, string> pProgma = new Dictionary<string, string>();
        public string CodeStr { get; set; }
        public string Name { get; set; }
        public List<string> Lines { get; set; }

        public Tmo GetTmo(string code)
        {
            Tmo tmo = new Tmo();
            List<string> p = new List<string>();
            ketd = new Dictionary<string, string>();
            ketd.Add("GO", "");
            ketd.Add("move", "");

            var detee = from n in this.ketd
                        where n.Key.ToLower().StartsWith(code.ToLower())
                        select n;
            foreach (var item in detee)
            {
                p.Add(item.Key);
            }

            tmo.PProgma = p.ToArray();
            // from te in ketd.Keys
            //select te.StartsWith(code)
            //tmo.Ketd = te;
            return tmo;
        }
    }
}