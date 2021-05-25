namespace Vision2.ErosProjcetDLL.PLCUI
{
    /// <summary>
    /// 子控件接口
    /// </summary>
    public class SettingService
    {
        private static SettingService _Instance;
        private System.Windows.Forms.Control _SelectedToolBoxControl;

        /// <summary>
        /// 在Form中，选择了某个要添加的控件后，这里保存这个控件的一个新实例
        /// </summary>
        public System.Windows.Forms.Control SelectedToolBoxControl
        {
            get { return this._SelectedToolBoxControl; }
            set { this._SelectedToolBoxControl = value; }
        }

        private SettingService()
        { }

        /// <summary>
        /// 这里使用单件
        /// </summary>
        public static SettingService Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new SettingService();
                }
                return _Instance;
            }
        }
    }
}