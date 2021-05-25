using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace Vision2.ErosProjcetDLL.PLCUI
{
    public partial class ErosNewFrom : Form
    {
        /// <summary>
        /// 我们将所有的已经与具体控件关联了的UISizeKnob缓存在这个HashTable中
        /// </summary>
        private Hashtable _HashUISizeKnob;

        /// <summary>
        /// 集合
        /// </summary>
        public Hashtable _HashControl;

        private Control _Owner = new Control();

        private Point downPos = new Point(0, 0);
        private Graphics g = null;
        private bool isDown = false;
        private Point upPos = new Point(100, 100);

        /// <summary>
        /// 大小事件
        /// </summary>
        private MouseHook _MouseHook;

        ///<summary>负责控件移动的类</summary>
        private Hashtable _HashUIMoveKnob;

        private List<Control> ListUi = new List<Control>();

        public ErosNewFrom(Hashtable hashtable)
        {
            InitializeComponent();
            this._HashControl = hashtable;
            _HashUISizeKnob = new Hashtable();
            this._MouseHook = new MouseHook(this);
            this._HashUIMoveKnob = new Hashtable();

            //为了简洁明了，我们在ControlAdded中来设置具体控件和UISizeKnob的关联
            this.ControlAdded += new ControlEventHandler(Form1_ControlAdded);
        }

        //Dictionary<string, Control> keysControl;

        private void Form1_ControlAdded(object sender, ControlEventArgs e)
        {
            if (e.Control.Name != "")
            {
                if (!_HashControl.ContainsKey(e.Control.Name))
                {
                    _HashControl.Add(e.Control.Name, e.Control);
                }
                else
                {
                    MessageBox.Show("添加错误，已存在同名控件：" + e.Control.Name);
                }
            }
            if (!(e.Control is UISizeDot))
            {
                UserControl userControl = e.Control as UserControl;
                this._HashUISizeKnob.Add(e.Control, new UISizeKnob(e.Control));
                this._HashUIMoveKnob.Add(e.Control, new UIMoveKnob(e.Control));
                //点击控件的时候，显示控件的选择
                e.Control.Click += new EventHandler(Control_Click);
            }
        }

        private void Panel_MouseMove(object sender, MouseEventArgs e)
        {
            Panel panel = (Panel)sender;
            if (panel != null)
            {
            }
        }

        private bool isShift;

        private void Control_Click(object sender, EventArgs e)
        {
            try
            {
                if (isShift)
                {
                }
                else
                {
                    foreach (UISizeKnob knob in this._HashUISizeKnob.Values)
                    {
                        knob.ShowUISizeDots(false);
                    }
                    ListUi.Clear();
                }
                ((UISizeKnob)this._HashUISizeKnob[sender]).ShowUISizeDots(true);
                ListUi.Add((Control)sender);
                //我这里仅仅做TextBox的属性演示，如果是其它的控件的话，那么你需要设计不同的ControlProperty（比如TextBoxProperty，ComboBoxProperty）
                if (sender is TextBox)
                {
                    //this.propertyGrid1.SelectedObject = new TextBoxProperty((TextBox)sender);
                }
                else
                {
                    //this.propertyGrid1.SelectedObject = sender;
                }
                Vision2.ErosProjcetDLL.Project.PropertyForm.UPProperty(sender, _HashControl);
            }
            catch { }
        }

        private void ErosNewFrom_Load(object sender, EventArgs e)
        {
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="controlType"></param>
        /// <param name="form"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        public static void CreateControl(string controlType, Form form, int positionX, int positionY)
        {
            try
            {
                string assemblyQualifiedName = typeof(System.Windows.Forms.Form).AssemblyQualifiedName;
                string assemblyInformation = assemblyQualifiedName.Substring(assemblyQualifiedName.IndexOf(","));
                Type ty = Type.GetType(controlType + assemblyInformation);
                Control newControl = (Control)System.Activator.CreateInstance(ty);
                form.SuspendLayout();
                newControl.Location = new System.Drawing.Point(positionX, positionY);
                newControl.Name = ty.Name + form.Controls.Count.ToString();
                form.Controls.Add(newControl);
                form.ResumeLayout();
            }
            catch (Exception ex)
            {
                MessageBox.Show("创建控件错误：" + ex.Message);
            }
        }

        public void XmlForm(XmlNode node)
        {
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //vision.HalconRun.ClassToJsonSavePath(this, Application.StartupPath + "\\HMI\\Hmi");
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
        }

        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateControl("System.Windows.Forms.TextBox", this, Control.MousePosition.X, Control.MousePosition.Y);
        }

        private void 窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.AllowDrop = true;
            this.DragDrop += new System.Windows.Forms.DragEventHandler(Project.ToolForm.UIForm.Control_DragDrop);
            this.DragOver += new System.Windows.Forms.DragEventHandler(Project.ToolForm.UIForm.Control_DragOver);
        }

        private void ErosNewFrom_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                isShift = false;
                if (e.KeyCode == Keys.Delete)
                {
                    foreach (var item in ListUi)
                    {
                        ListUi.Remove(item);

                        ((UISizeKnob)this._HashUISizeKnob[item]).ShowUISizeDots(false);
                        this.Controls.Remove(item);
                        this._HashControl.Remove(item.Name);
                        this._HashUISizeKnob.Remove(item);
                        this._HashUIMoveKnob.Remove(item);
                        item.Dispose();
                    }
                }
                else if (e.KeyCode == Keys.ShiftKey)
                {
                    isShift = true;
                }
                else
                {
                }
            }
            catch (Exception)
            {
            }
        }

        private void ErosNewFrom_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                //如果没有选择控件，那么退出
                g = g ?? this.CreateGraphics();
                downPos = e.Location;
                isDown = true;
            }
            catch (Exception)
            {
            }
        }

        private void ErosNewFrom_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDown)
            {
                upPos = e.Location;
                this.Invalidate();
            }
        }

        private void ErosNewFrom_MouseUp(object sender, MouseEventArgs e)
        {
            this.Invalidate();
            upPos = e.Location;

            isDown = false;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (g == null)
                return;
            g.DrawRectangle(new Pen(Color.Blue, 1), new Rectangle(downPos, new Size(upPos.X - downPos.X, upPos.Y - downPos.Y)));
        }
    }
}