using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Vision2.Project.Mes;

namespace Vision2.Project.formula
{
    public partial class ButtenModeUI : UserControl
    {
        public ButtenModeUI()
        {
            InitializeComponent();
        }

        static ErosSocket.DebugPLC.Robot.TrayRobot dataVales = new ErosSocket.DebugPLC.Robot.TrayRobot(3,1);
        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                dataVales.GetDataVales()[0].Done = true;
                dataVales.GetDataVales()[0].AutoOK = true;
                dataVales.GetDataVales()[0].OK = true;
                RecipeCompiler.AlterNumber(true, dataVales.GetDataVales()[0].ResuOBj[0].RunID - 1);

                if (RecipeCompiler.Instance.GetMes() != null)
                {
                    RecipeCompiler.Instance.GetMes().WrietMes(dataVales, Project.formula.Product.ProductionName);
                }

            }
            catch (Exception)
            {
            }
            button10.Enabled = false;
            button6.Enabled = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                dataVales.GetDataVales()[0].Done = true;
                dataVales.GetDataVales()[0].AutoOK = false;
                //dataVales[0].RsetOK = false;
                RecipeCompiler.AlterNumber(true, dataVales.GetDataVales()[0].ResuOBj[0].RunID - 1);

                if (RecipeCompiler.Instance.GetMes() != null)
                {
                    RecipeCompiler.Instance.GetMes().WrietMes(dataVales, Project.formula.Product.ProductionName);
                }
            }
            catch (Exception)
            {
            }
            button10.Enabled = false;
            button6.Enabled = false;
        }
    }
}
