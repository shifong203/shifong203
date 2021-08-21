using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Vision2.ErosProjcetDLL.Project;

namespace Vision2.ErosProjcetDLL.PLCUI
{
    public class HMI : ProjectC, ProjectNodet.IClickNodeProject
    {
        public HMI(string name = ".HMI")
        {
            Name = name;
        }

        public HMI()
        {
            ErosNewFrom = new ErosNewFrom(keyValueControl);
        }

        public override string Name
        {
            get { return name; }

            set
            {
                name = value;
                ErosNewFrom.Text = value;
            }
        }

        private Hashtable keyValueControl = new Hashtable();
        private string name = "HMI";
        private ErosNewFrom ErosNewFrom;

        public override void DoubleClickUpForm(TabPage tabPage, object data = null)
        {
            ErosNewFrom.Dock = DockStyle.Fill;
            ErosNewFrom.TopLevel = false;
            ErosNewFrom.Show();
            tabPage.Controls.Add(ErosNewFrom);
        }

        //public override void UpProperty(PropertyForm pertyForm, object data = null)
        //{
        //    try
        //    {
        //        base.UpProperty(pertyForm, data);
        //    }
        //    catch (System.Exception)
        //    {
        //    }
        //}

        public Control GetThisControl()
        {
            return null;
        }

        public override void initialization()
        {
            foreach (var item in HControl)
            {
                ErosNewFrom.Controls.Add(item.Value.NewControl());
            }
            base.initialization();
        }

        public void SaveControl()
        {
            HControl.Clear();
            foreach (var item in keyValueControl.Values)
            {
                Control control = item as Control;
                if (control != null)
                {
                    HControl.Add(control.Name, new PLCUI.HControl().RHControl(control));
                }
            }
        }

        public Dictionary<string, HControl> HControl
        {
            get;
            set;
        } = new Dictionary<string, HControl>();
    }

    public class HControl
    {
        public Point Location = new Point();
        public Size Size = new Size();
        public Color BackColor = new Color();
        public Color ForeColor = new Color();
        public string Text = string.Empty;
        public string Name = string.Empty;
        public string _Type = string.Empty;
        public DockStyle Dock = DockStyle.None;

        public Control NewControl()
        {
            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                Control control = new Control();
                dynamic obj = control.GetType().Assembly.CreateInstance(_Type);
                if (obj != null)
                {
                    control = obj;
                    control.Text = this.Text;
                    control.Name = this.Name;
                    control.ForeColor = this.ForeColor;
                    control.BackColor = this.BackColor;
                    control.Size = this.Size;
                    control.Location = this.Location;
                    control.Dock = this.Dock;
                    return control;
                }
                else
                {
                    string[] dat = _Type.Split('.');
                    obj = System.Reflection.Assembly.Load(dat[0]).CreateInstance(_Type, false);
                    if (obj != null)
                    {
                        control = obj;
                        control.Text = this.Text;
                        control.Name = this.Name;
                        control.ForeColor = this.ForeColor;
                        control.BackColor = this.BackColor;
                        control.Size = this.Size;
                        control.Location = this.Location;
                        control.Dock = this.Dock;
                        //if (obj is ExpandoObject)
                        //{
                        //    if (((IDictionary<string, object>)obj).ContainsKey("KeyValuePairs"))
                        //    {
                        //        obj.KeyValuePairs = this.KeyValuePairs;
                        //    }
                        //}
                        //if (obj.GetType().GetProperty("KeyValuePairs") != null)
                        //{
                        //    obj.KeyValuePairs = this.KeyValuePairs;
                        //}
                        return control;
                    }
                }
                MessageBox.Show("未创建成功的控件:" + _Type + "," + Name);
            }
            catch (Exception EX)
            {
                MessageBox.Show("未创建成功的控件:" + _Type + "," + Name + EX.Message);
            }

            return new Control();
        }

        public HControl RHControl(Control control)
        {
            return new PLCUI.HControl()
            {
                Location = control.Location,
                Size = control.Size,
                BackColor = control.BackColor,
                ForeColor = control.ForeColor,
                Name = control.Name,
                Text = control.Text,
                _Type = control.GetType().ToString(),
                Dock = control.Dock,
                //KeyValuePairs = Project.ProjectCon.GetPropertyExist(control,"KeyValuePairs")
            };
        }
    }
}