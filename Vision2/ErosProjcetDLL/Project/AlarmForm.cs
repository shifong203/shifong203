using System.Windows.Forms;

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
            this.Hide();
            e.Cancel = true;//拦截，不响应操作
            return;
        }
    }
}
