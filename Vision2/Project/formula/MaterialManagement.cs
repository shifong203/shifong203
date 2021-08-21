using System.ComponentModel;
using Vision2.ErosProjcetDLL.UI.PropertyGrid;

namespace Vision2.Project.formula
{
    /// <summary>
    /// 物料类
    /// </summary>
    public class MaterialManagement : Vision2.ErosProjcetDLL.Project.INodeNew
    {
        [Description("首关键字>表示从结尾开始，无表示从首位，ProductName匹配配方名，或其他固定字符"), Category("物料信息"), DisplayName("二维码匹配"),
          TypeConverter(typeof(ErosConverter)), ErosConverter.ThisDropDownAttribute("", false, ">ProductName", "ProductName")]
        public string QRCODE { get; set; } = "ProductName";

        /// <summary>
        /// 匹配物料
        /// </summary>
        /// <param name="qRCode">关键二维码</param>
        /// <returns>true成功，false失败</returns>
        public bool MatchTheMaterial(string qRCode)
        {
            if (QRCODE.StartsWith(">"))
            {
                if (QRCODE == ">ProductName")
                {
                    if (qRCode.EndsWith(Product.ProductionName))
                    {
                        return true;
                    }
                }
                else
                {
                    if (qRCode.EndsWith(QRCODE.Remove(0, 1)))
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (QRCODE == "ProductName")
                {
                    if (qRCode.StartsWith(Product.ProductionName))
                    {
                        return true;
                    }
                }
                else
                {
                    if (qRCode.StartsWith(QRCODE))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}