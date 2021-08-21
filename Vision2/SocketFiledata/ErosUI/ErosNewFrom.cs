using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace ErosSocket.ErosUI
{
    public partial class ErosNewFrom : Form
    {
        /// <summary>
        /// 我们将所有的已经与具体控件关联了的UISizeKnob缓存在这个HashTable中
        /// </summary>
        private Hashtable _HashUISizeKnob;

        private Control _Owner = new Control();

        private Point downPos = new Point(0, 0);
        private Graphics g = null;
        private bool isDown = false;
        private Point upPos = new Point(100, 100);

        ///// <summary>
        ///// 大小事件
        ///// </summary>
        //private MouseHook _MouseHook;

        ///<summary>负责控件移动的类</summary>
        private Hashtable _HashUIMoveKnob;

        private List<Control> ListUi = new List<Control>();

        public ErosNewFrom()
        {
            InitializeComponent();
            ToolForm toolForm = new ToolForm();
            toolForm.TopLevel = false;
            toolForm.Dock = DockStyle.Bottom;
            groupBox1.Controls.Add(toolForm);
            toolForm.Show();
            this._HashUISizeKnob = new Hashtable();

            //this._MouseHook = new MouseHook(this);

            this._HashUIMoveKnob = new Hashtable();

            //为了简洁明了，我们在ControlAdded中来设置具体控件和UISizeKnob的关联
            this.ControlAdded += new ControlEventHandler(Form1_ControlAdded);
        }

        private void Form1_ControlAdded(object sender, ControlEventArgs e)
        {
            if (!(e.Control is UISizeDot))
            {
                UserControl userControl = e.Control as UserControl;
                if (userControl != null)
                {
                    Panel panel = new Panel() { BackColor = Color.Transparent, Dock = DockStyle.Fill, };
                    userControl.Controls.Add(panel);
                    userControl.Controls.SetChildIndex(panel, 0);
                    panel.Show();
                    panel.MouseMove += Panel_MouseMove;
                }
                this._HashUISizeKnob.Add(e.Control, new UISizeKnob(e.Control));
                //this._HashUIMoveKnob.Add(e.Control, new UIMoveKnob(e.Control));
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
                    this.propertyGrid1.SelectedObject = new TextBoxProperty((TextBox)sender);
                }
                else
                {
                    this.propertyGrid1.SelectedObject = sender;
                }
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

        private ErosNewFrom erosNewFrom;

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (erosNewFrom == null || erosNewFrom.IsDisposed)
            {
                erosNewFrom = new ErosNewFrom();
            }
            erosNewFrom.Show();
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
        }

        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateControl("System.Windows.Forms.TextBox", this, Control.MousePosition.X, Control.MousePosition.Y);
        }

        private void erosBtn1_Click(object sender, EventArgs e)
        {
            SettingService.Instance.SelectedToolBoxControl = null;
            PLCBtn erosBtn = (PLCBtn)sender;

            //propertyGrid1.SelectedObject = erosBtn.SeeName;
        }

        private void 窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Form1 form1 = new Form1();
            //form1.Show();
        }

        private void elementsEditor1_Load(object sender, EventArgs e)
        {
        }

        private void PropertyGrid1_TextChanged(object sender, EventArgs e)
        {
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void cmdLabel_Click(object sender, EventArgs e)
        {
            SettingService.Instance.SelectedToolBoxControl = new Label();
        }

        private void cmdTextBox_Click(object sender, EventArgs e)
        {
            SettingService.Instance.SelectedToolBoxControl = new TextBox();
        }

        private void cmdComboBox_Click(object sender, EventArgs e)
        {
            SettingService.Instance.SelectedToolBoxControl = new ComboBox();
        }

        private void cmdGroupBox_Click(object sender, EventArgs e)
        {
            SettingService.Instance.SelectedToolBoxControl = new GroupBox();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SettingService.Instance.SelectedToolBoxControl = new ComboBox();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            propertyGrid1.SelectedObject = button1;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.AllowDrop = true;
            this.DragDrop += new System.Windows.Forms.DragEventHandler(ToolForm.UIForm.Control_DragDrop);
            this.DragOver += new System.Windows.Forms.DragEventHandler(ToolForm.UIForm.Control_DragOver);
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