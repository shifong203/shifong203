using System;
using System.Drawing;
using System.Windows.Forms;
using NokidaE.ErosUI;

namespace NokidaE.DebugF
{
    public partial class FormTextProgram : Form
    {
        public FormTextProgram()
        {
            InitializeComponent();
            userInterfaceControl1.Btn_Start.Click += Btn_Start_Click; 

   
            userInterfaceControl1.Btn_Debug.Click += Btn_Debug_Click;
            //userInterfaceControl1.Btn_Stop.MouseClick += Btn_Stop_MouseDoubleClick;
         
            timer = new Timer();
            timer.Tick += Timer_Tick;
            timer.Interval = 3000;
        }



      

    
    }
}