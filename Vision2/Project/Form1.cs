using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

using System.Drawing;
using System.Text;

namespace NokidaE.Project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            This = this;
            ErosProjcetDLL.UI.DataGridViewF.StCon.AddCon(dataGridView1);
        }
        public static Form1 This;
        public static List<DataBeas> ListData = new List<DataBeas>();
        public DataBeas data;
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListData.Clear();

        }

        private void 模拟ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
        public static void AddPaneID(string Pane)
        {
            try
            {
                if (This != null)
                {
                    This.data = new DataBeas();
                    ListData.Add(This.data);
                    This.labelPaneID.Text = "PaneID:" + Pane;
                    This.label2.Text = "入站时间:" + DateTime.Now.ToString();
                    This.label7.Text = "";
                    This.data.PaneID = Pane;
                    This.data.InTimeStr = DateTime.Now.ToString();
                    This.labelPaneID.BackColor = Color.GreenYellow;
                }

            }
            catch (Exception)
            {

            }
    
        }
        public static void AddCarrierID(string Carrier)
        {
            try
            {
                if (This != null)
                {
                    This.labelCarrierID.Text = "CarrierID:" + Carrier;
                    This.data.CarrierID = Carrier;
                }
            }
            catch (Exception)
            {

            }
       
        }
        public static void AddMagazineID(string MagazineID)
        {
            try
            {
                if (This != null)
                {
                    This.labelMagazineID.Text = "MagazineID:" + MagazineID;
                    This.data.MagazineID = MagazineID;
                }
            }
            catch (Exception)
            {
            }
     
        }

        public static void FindID(string rselt)
        {
            try
            {
                if (This != null)
                {
                    if (rselt == "OK")
                    {
                        This.pictureBox5.Image = global::NokidaE.Properties.Resources.accept;
                    }
                    else
                    {
                        This.pictureBox5.Image = global::NokidaE.Properties.Resources.security;
                    }

                    This.label7.Text = rselt;
                    This.label3.Text = "出站时间:" + DateTime.Now.ToString();
                    This.data.Result = rselt;
                    This.data.OutTimeStr = DateTime.Now.ToString();
                    This.dataGridView1.DataSource = null ;
                    This.dataGridView1.DataSource = ListData;
          
                }
            }
            catch (Exception)
            {

    
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ListData = new List<DataBeas>();
            dataGridView1.AutoGenerateColumns = false;
            Column1.DataPropertyName = "InTimeStr";
            Column2.DataPropertyName = "OutTimeStr";
            Column3.DataPropertyName = "MagazineID";
            Column4.DataPropertyName = "PaneID";
            Column5.DataPropertyName = "CarrierID";
            Column6.DataPropertyName = "Result";
        
            This.label7.Text = "";
        }
        simulateQRForm simulateQRForm = new simulateQRForm();
        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            ErosProjcetDLL.UI.UICon.WindosFormerShow(ref simulateQRForm);
        }
        /// <summary> 
        /// IList接口（包括一维数组，ArrayList等） 
        /// </summary> 
        /// <returns></returns> 
        private ArrayList DataBindingByList1()
        {
            ArrayList Al = new ArrayList();
            //Al.Add(new PersonInfo("a", "-1"));
            //Al.Add(new PersonInfo("b", "-2"));
            //Al.Add(new PersonInfo("c", "-3"));
            return Al;
        }

        /// <summary> 
        /// IList接口（包括一维数组，ArrayList等） 
        /// </summary> 
        /// <returns></returns> 
        private ArrayList DataBindingByList2()
        {
            ArrayList list = new ArrayList();
            for (int i = 0; i < 10; i++)
            {
                list.Add(new DictionaryEntry(i.ToString(), i.ToString() + "_List"));
            }
            return list;
        }


        /// <summary> 
        /// IListSource接口（DataTable、DataSet等） 
        /// </summary> 
        /// <returns></returns> 
        private DataTable DataBindingByDataTable()
        {
            DataTable dt = new DataTable();
            DataColumn dc1 = new DataColumn("Name");
            DataColumn dc2 = new DataColumn("Value");

            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);

            for (int i = 1; i <= 10; i++)
            {
                DataRow dr = dt.NewRow();
                dr[0] = i;
                dr[1] = i.ToString() + "_DataTable";
                dt.Rows.Add(dr);
            }

            return dt;
        }


        /// <summary> 
        /// IBindingListView接口（如BindingSource类） 
        /// </summary> 
        /// <returns></returns> 
        private BindingSource DataBindingByBindingSource()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            for (int i = 0; i < 10; i++)
            {
                dic.Add(i.ToString(), i.ToString() + "_Dictionary");
            }
            return new BindingSource(dic, null);
        }

        public class DataBeas
        {
            public string InTimeStr { get; set; }
            public string OutTimeStr { get; set; }
            public string MagazineID { get; set; }
            public string PaneID { get; set; }
            public string CarrierID { get; set; }
            public string Result { get; set; }

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
