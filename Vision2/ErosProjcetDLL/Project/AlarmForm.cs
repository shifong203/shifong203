using System.Windows.Forms;
using Vision2.Project.formula;

namespace Vision2.ErosProjcetDLL.Project
{
    public partial class AlarmForm : Form
    {
        public AlarmForm()
        {
            InitializeComponent();
            this.TopLevel = false;
        }

        public static AlarmForm AlarmFormThis
        {

            get
            {
                if (alarmForm == null || alarmForm.IsDisposed)
                {
                    alarmForm = new AlarmForm();

                    // alarmForm.TopMost = true;
                    //Vision2.Project.MainForm1.MainFormF.Controls.Add(alarmForm);
                }
                return alarmForm;
            }
            set { alarmForm = value; }
        }


        static AlarmForm alarmForm;

        private void AlarmForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                UserFormulaContrsl.This.tabControl1.TabPages.Add(UserFormulaContrsl.This.tabPage4);
                UserFormulaContrsl.This.tabPage4.Controls.Add(AlarmForm.AlarmFormThis);
                AlarmForm.AlarmFormThis.TopLevel = false;
                AlarmForm.AlarmFormThis.FormBorderStyle = FormBorderStyle.None;
                AlarmForm.AlarmFormThis.Dock = DockStyle.Fill;
                AlarmForm.AlarmFormThis.Show();
            }
            catch (System.Exception)
            {  }
            e.Cancel = true;//拦截，不响应操作
            return;
        }

        private void AlarmForm_Load(object sender, System.EventArgs e)
        {

        }
    }
}
