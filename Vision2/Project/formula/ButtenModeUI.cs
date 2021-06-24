using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Vision2.Project.Mes;
using ErosSocket.DebugPLC.Robot;
namespace Vision2.Project.formula
{
    public partial class ButtenModeUI : UserControl ,ITrayRobot
    {
        public ButtenModeUI()
        {
            InitializeComponent();
        }
        public void SetTrayData(TrayData trayData )
        {

            dataVales = trayData;
            try
            {
                this.Controls.Clear();
                for (int i = 0; i < dataVales.Count; i++)
                {
                    DataButtenModeU dataUi = new DataButtenModeU();
                    dataUi.Name = (i + 1).ToString();
                    dataUi.Dock = DockStyle.Top;
                    this.Controls.Add(dataUi);
                    dataUi.Set(dataVales.GetDataVales()[i]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
         TrayData dataVales;

        public void SetValue(int number, bool value, double? valueDouble = null)
        {
      
        }

        public void SetValue(int number, DataVale dataVale)
        {

        }

        public void SetValue(int number, TrayData dataVale)
        {
            try
            {
                for (int i = 0; i < dataVale.Count; i++)
                {

                  Control [] controls=   this.Controls.Find((i + 1).ToString(), false);
                    if (controls.Length!=0)
                    {
                        DataButtenModeU dataUi = controls[0] as DataButtenModeU;
                        dataUi.Name = (i + 1).ToString();

                        dataUi.Set(dataVale.GetDataVales()[i]);
                    }
                }

            }
            catch (Exception)
            {
            }
     
        }

        public void SetPanleSN(List<string> listSN, List<int> tryaid)
        {
          
        }

        public void RestValue()
        {
            try
            {
                this.Invoke(new MethodInvoker(() => {
                    this.Controls.Clear();
                    for (int i = 0; i < dataVales.Count; i++)
                    {
                        DataButtenModeU dataUi = new DataButtenModeU();
                        dataUi.Name = (i + 1).ToString();
                        dataUi.Dock = DockStyle.Top;
                        this.Controls.Add(dataUi);
                        dataUi.Set(dataVales.GetDataVales()[i]);
                    }

                }));

          
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
