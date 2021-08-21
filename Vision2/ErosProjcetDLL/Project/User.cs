using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Vision2.ErosProjcetDLL.Project
{
    public class User
    {
        public User()
        {
            ListRightGroup.Add("基本权限");
            ListRightGroup.Add("管理权限");
            ListRightGroup.Add("工程师");
            UserRightGroup.Add("操作员", new List<string> { "基本权限" });
        }

        /// <summary>
        /// 链接委托
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public delegate void DelegateLog(bool isLog);

        /// <summary>
        /// 登录事件
        /// </summary>
        public event DelegateLog EventLog;

        /// <summary>
        /// 匹配权限
        /// </summary>
        /// <param name="permissions">权限</param>
        /// <returns>是否匹配</returns>
        public static bool MatchThePermissions(string permissions)
        {
            if (ProjectINI.In.UserRight.Contains(permissions))

            {
                return true;
            }
            return false;
        }

        public static void Del()
        {
            ProjectINI.In.UserRight.Clear();
            ProjectINI.In.UserName = "未登陆";
            //MainForm1.MainFormF.Up();
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWrod"></param>
        /// <returns></returns>
        public static bool Loge(string userName, string passWrod)
        {
            ProjectINI.In.UserRight.Clear();
            ProjectINI.In.UserName = "未登陆";
            if (ProjectINI.In.User.UserPassWords.ContainsKey(userName))
            {
                if (ProjectINI.In.User.UserPassWords[userName].passWord == passWrod)
                {
                    if (ProjectINI.In.User.UserRightGroup.ContainsKey(ProjectINI.In.User.UserPassWords[userName].UserRightGroup))
                    {
                        ProjectINI.In.UserRight.AddRange(ProjectINI.In.User.UserRightGroup[ProjectINI.In.User.UserPassWords[userName].UserRightGroup].ToArray());
                    }
                    ProjectINI.In.Right = ProjectINI.In.User.UserPassWords[userName].UserRightGroup;
                    ProjectINI.In.UserName = ProjectINI.In.User.UserPassWords[userName].uersName;
                    ProjectINI.In.UserID = ProjectINI.In.User.UserPassWords[userName].UserID;
                    if (ProjectINI.In.User.EventLog != null)
                    {
                        ProjectINI.In.User.EventLog.Invoke(true);
                    }

                    return true;
                }
                else
                {
                    MessageBox.Show("用户名密码错误！");
                }
            }
            else
            {
                MessageBox.Show("用户名密码错误！");
            }
            if (ProjectINI.In.User.EventLog != null)
            {
                ProjectINI.In.User.EventLog.Invoke(false);
            }

            return false;
        }

        /// <summary>
        /// 用户名与密码
        /// </summary>
        public Dictionary<string, Users> UserPassWords
        {
            get
            {
                if (keyValuePairs.ContainsKey("Eros"))
                {
                    keyValuePairs["Eros"].name = "Eros";
                    keyValuePairs["Eros"].uersName = "Eros";
                    keyValuePairs["Eros"].passWord = "ErosEE1988";
                    keyValuePairs["Eros"].UserRightGroup = "工程师";
                    keyValuePairs["Eros"].UserDepartment = "工程师";
                }
                else
                {
                    keyValuePairs.Add("Eros", new Users());
                    keyValuePairs["Eros"].name = "Eros";
                    keyValuePairs["Eros"].uersName = "Eros";
                    keyValuePairs["Eros"].passWord = "ErosEE1988";
                    keyValuePairs["Eros"].UserRightGroup = "工程师";
                    keyValuePairs["Eros"].UserDepartment = "工程师";
                }
                if (keyValuePairs.ContainsKey("UserAdmin"))
                {
                    keyValuePairs["UserAdmin"].name = "UserAdmin";
                    keyValuePairs["UserAdmin"].uersName = "UserAdmin";
                    keyValuePairs["UserAdmin"].passWord = "UserAdminTO";
                    keyValuePairs["UserAdmin"].UserRightGroup = "管理员";
                    keyValuePairs["UserAdmin"].UserDepartment = "技术部";
                }
                else
                {
                    keyValuePairs.Add("UserAdmin", new Users());
                    keyValuePairs["UserAdmin"].name = "UserAdmin";
                    keyValuePairs["UserAdmin"].uersName = "UserAdmin";
                    keyValuePairs["UserAdmin"].passWord = "UserAdminTO";
                    keyValuePairs["UserAdmin"].UserRightGroup = "管理员";
                    keyValuePairs["UserAdmin"].UserDepartment = "技术部";
                }
                return keyValuePairs;
            }
            set
            {
                if (!keyValuePairs.ContainsKey("Eros"))
                {
                    keyValuePairs.Add("Eros", new Users());
                }
                keyValuePairs["Eros"].name = "Eros";
                keyValuePairs["Eros"].uersName = "Eros";
                keyValuePairs["Eros"].passWord = "ErosEE1988";
                keyValuePairs["Eros"].UserRightGroup = "工程师";
                keyValuePairs["Eros"].UserDepartment = "工程师";
                if (!keyValuePairs.ContainsKey("UserAdmin"))
                {
                    keyValuePairs.Add("UserAdmin", new Users());
                }
                keyValuePairs["UserAdmin"].name = "UserAdmin";
                keyValuePairs["UserAdmin"].uersName = "UserAdmin";
                keyValuePairs["UserAdmin"].passWord = "UserAdminTO";
                keyValuePairs["UserAdmin"].UserRightGroup = "管理员";
                keyValuePairs["UserAdmin"].UserDepartment = "技术部";
                keyValuePairs = value;
            }
        }

        private Dictionary<string, Users> keyValuePairs = new Dictionary<string, Users>();

        public class Users
        {
            /// <summary>
            /// 名称
            /// </summary>
            public string name;

            /// <summary>
            /// 用户名
            /// </summary>
            public string uersName;

            /// <summary>
            /// 密码
            /// </summary>
            public string passWord;

            /// <summary>
            /// 权限组
            /// </summary>
            public string UserRightGroup;

            /// <summary>
            /// 工号
            /// </summary>
            public string UserID;

            /// <summary>
            /// 部门
            /// </summary>
            public string UserDepartment;
        }

        /// <summary>
        /// 权限组
        /// </summary>
        public Dictionary<string, List<string>> UserRightGroup
        {
            get
            {
                if (userRightGroup.ContainsKey("工程师"))
                {
                    userRightGroup["工程师"] = ListRightGroup.ToList();
                }
                else
                {
                    userRightGroup.Add("工程师", ListRightGroup.ToList());
                }
                if (userRightGroup.ContainsKey("管理员"))
                {
                    userRightGroup["管理员"] = ListRightGroup.ToList();
                }
                else
                {
                    userRightGroup.Add("管理员", ListRightGroup.ToList());
                }
                userRightGroup["管理员"].Remove("工程师");
                return userRightGroup;
            }

            set { userRightGroup = value; }
        }

        /// <summary>
        ///
        /// </summary>
        private Dictionary<string, List<string>> userRightGroup = new Dictionary<string, List<string>>();

        /// <summary>
        /// 权限集合
        /// </summary>
        public List<string> ListRightGroup
        {
            get
            {
                if (listRightGroup.Contains(""))
                {
                    listRightGroup.Remove("");
                }
                HashSet<string> hs = new HashSet<string>(listRightGroup);
                listRightGroup = hs.ToList();
                return listRightGroup;
            }
            set { }
        }

        private List<string> listRightGroup = new List<string>();

        /// <summary>
        /// 部门集合
        /// </summary>
        public List<string> ListDepartmentGroup
        {
            get
            {
                if (listDepartmentGroup.Contains(""))
                {
                    listDepartmentGroup.Remove("");
                }
                HashSet<string> hs = new HashSet<string>(listDepartmentGroup);
                listDepartmentGroup = hs.ToList();
                return listDepartmentGroup;
            }
            set { }
        }

        private List<string> listDepartmentGroup = new List<string>();
    }
}