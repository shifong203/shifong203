using System;
using System.Collections.Generic;

/// <summary>
/// 生产管理
/// </summary>
namespace Vision2.ProductionManagement
{
    /// <summary>
    /// 生产任务
    /// </summary>
    public class Productive_Task
    {
        private Dictionary<string, Productive_Task> ThisDic = new Dictionary<string, Productive_Task>();

        public DateTime StratTime { get; set; }

        public DateTime EndTime { get; set; }
        //[DescriptionAttribute("显示用户视觉窗体。"), Category("程序"), DisplayName("显示视觉用户控件")]
        //public static bool Visible
        //{
        //    get
        //    {
        //        if (ErosUI.FormTextProgram.ThisStatic != null)
        //        {
        //            if (visible)
        //            {
        //                if (!ErosUI.FormTextProgram.ThisStatic.tabControl1.TabPages.ContainsKey("生产任务"))
        //                {
        //                    vision.UserVisionManagement userVisionManagement = new vision.UserVisionManagement();
        //                    TabPage tabPage = new TabPage();
        //                    userVisionManagement.Dock = DockStyle.Fill;
        //                    tabPage.Controls.Add(userVisionManagement);
        //                    tabPage.Text = "生产任务";
        //                    tabPage.Name = "生产任务";
        //                    ErosUI.FormTextProgram.ThisStatic.tabControl1.TabPages.Add(tabPage);
        //                }
        //            }
        //            else
        //            {
        //                if (ErosUI.FormTextProgram.ThisStatic.tabControl1.TabPages.ContainsKey("生产任务"))
        //                {
        //                    ErosUI.FormTextProgram.ThisStatic.tabControl1.TabPages.RemoveByKey("生产任务");
        //                }
        //            }
        //        }

        //        return visible;
        //    }
        //    set
        //    {
        //        if (ErosUI.FormTextProgram.ThisStatic != null)
        //        {
        //            if (value)
        //            {
        //                if (!ErosUI.FormTextProgram.ThisStatic.tabControl1.TabPages.ContainsKey("生产任务"))
        //                {
        //                    vision.UserVisionManagement userVisionManagement = new vision.UserVisionManagement();

        //                    TabPage tabPage = new TabPage();
        //                    userVisionManagement.Dock = DockStyle.Fill;
        //                    tabPage.Controls.Add(userVisionManagement);
        //                    tabPage.Text = "生产任务";
        //                    tabPage.Name = "生产任务";
        //                    ErosUI.FormTextProgram.ThisStatic.tabControl1.TabPages.Add(tabPage);
        //                }
        //            }
        //            else
        //            {
        //                if (ErosUI.FormTextProgram.ThisStatic.tabControl1.TabPages.ContainsKey("生产任务"))
        //                {
        //                    ErosUI.FormTextProgram.ThisStatic.tabControl1.TabPages.RemoveByKey("生产任务");
        //                }
        //            }
        //        }

        //        visible = value;
        //    }
        //}
        //static bool visible;
    }
}