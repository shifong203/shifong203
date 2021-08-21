

namespace ErosSocket.ErosUI
{
    partial class UserInterfaceControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            LinkPLC linkPLC1 = new LinkPLC();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserInterfaceControl));
            LinkPLC linkPLC2 = new LinkPLC();
            LinkPLC linkPLC3 = new LinkPLC();
            LinkPLC linkPLC4 = new LinkPLC();
          LinkPLC linkPLC5 = new LinkPLC();
          LinkPLC linkPLC6 = new LinkPLC();
           LinkPLC linkPLC7 = new LinkPLC();
            this.Btn_Massge = new PLCBtn();
            this.Btn_Reset = new PLCBtn();
            this.Btn_Pause = new PLCBtn();
            this.Btn_Debug = new PLCBtn();
            this.Btn_Initialize = new PLCBtn();
            this.Btn_Stop = new PLCBtn();
            this.Btn_Start = new  PLCBtn();
            this.SuspendLayout();
            // 
            // Btn_Massge
            // 
            this.Btn_Massge.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
         
            this.Btn_Massge.Font = new System.Drawing.Font("华文中宋", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn_Massge.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            linkPLC1.GetName = null;
         
            linkPLC1.SetName = null;
            linkPLC1.StatusColor = ((System.Collections.Generic.Dictionary<byte, System.Drawing.Color>)(resources.GetObject("linkPLC1.StatusColor")));
            linkPLC1.UpliftClickNames = ((System.Collections.Generic.Dictionary<byte, string>)(resources.GetObject("linkPLC1.UpliftClickNames")));
            linkPLC1.ValueClickTexts = ((System.Collections.Generic.Dictionary<byte, string>)(resources.GetObject("linkPLC1.ValueClickTexts")));
     

            //this.Btn_Massge.KeyValuePairs = linkPLC1;
            this.Btn_Massge.Location = new System.Drawing.Point(295, 67);
            this.Btn_Massge.Name = "Btn_Massge";
            this.Btn_Massge.Size = new System.Drawing.Size(90, 49);
            this.Btn_Massge.TabIndex = 7;
            this.Btn_Massge.Text = "信息";
            this.Btn_Massge.UseVisualStyleBackColor = false;
            this.Btn_Massge.Click += new System.EventHandler(this.Btn_Massge_Click);
            // 
            // Btn_Reset
            // 
            this.Btn_Reset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            this.Btn_Reset.Font = new System.Drawing.Font("华文中宋", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn_Reset.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            linkPLC2.GetName = null;
           
            linkPLC2.SetName = null;
            linkPLC2.StatusColor = ((System.Collections.Generic.Dictionary<byte, System.Drawing.Color>)(resources.GetObject("linkPLC2.StatusColor")));
            linkPLC2.UpliftClickNames = ((System.Collections.Generic.Dictionary<byte, string>)(resources.GetObject("linkPLC2.UpliftClickNames")));
            linkPLC2.ValueClickTexts = ((System.Collections.Generic.Dictionary<byte, string>)(resources.GetObject("linkPLC2.ValueClickTexts")));
      
        
            //this.Btn_Reset.KeyValuePairs = linkPLC2;
            this.Btn_Reset.Location = new System.Drawing.Point(108, 67);
            this.Btn_Reset.Name = "Btn_Reset";
            this.Btn_Reset.Size = new System.Drawing.Size(90, 49);
            this.Btn_Reset.TabIndex = 6;
            this.Btn_Reset.Text = "复位";
            this.Btn_Reset.UseVisualStyleBackColor = false;
            this.Btn_Reset.Click += new System.EventHandler(this.Btn_Reset_Click);
            // 
            // Btn_Pause
            // 
            this.Btn_Pause.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            this.Btn_Pause.Font = new System.Drawing.Font("华文中宋", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn_Pause.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            linkPLC3.GetName = null;

            linkPLC3.SetName = null;
            linkPLC3.StatusColor = ((System.Collections.Generic.Dictionary<byte, System.Drawing.Color>)(resources.GetObject("linkPLC3.StatusColor")));
            linkPLC3.UpliftClickNames = ((System.Collections.Generic.Dictionary<byte, string>)(resources.GetObject("linkPLC3.UpliftClickNames")));
            linkPLC3.ValueClickTexts = ((System.Collections.Generic.Dictionary<byte, string>)(resources.GetObject("linkPLC3.ValueClickTexts")));
    
            //this.Btn_Pause.KeyValuePairs = linkPLC3;
            this.Btn_Pause.Location = new System.Drawing.Point(17, 67);
            this.Btn_Pause.Name = "Btn_Pause";
            this.Btn_Pause.Size = new System.Drawing.Size(90, 49);
            this.Btn_Pause.TabIndex = 5;
            this.Btn_Pause.Text = "暂停";
            this.Btn_Pause.UseVisualStyleBackColor = false;
            this.Btn_Pause.Click += new System.EventHandler(this.Btn_Pause_Click);
            // 
            // Btn_Debug
            // 
            this.Btn_Debug.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            this.Btn_Debug.Font = new System.Drawing.Font("华文中宋", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn_Debug.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            linkPLC4.GetName = null;

            linkPLC4.SetName = null;
            linkPLC4.StatusColor = ((System.Collections.Generic.Dictionary<byte, System.Drawing.Color>)(resources.GetObject("linkPLC4.StatusColor")));
            linkPLC4.UpliftClickNames = ((System.Collections.Generic.Dictionary<byte, string>)(resources.GetObject("linkPLC4.UpliftClickNames")));
            linkPLC4.ValueClickTexts = ((System.Collections.Generic.Dictionary<byte, string>)(resources.GetObject("linkPLC4.ValueClickTexts")));
       
    
            //this.Btn_Debug.KeyValuePairs = linkPLC4;
            this.Btn_Debug.Location = new System.Drawing.Point(204, 67);
            this.Btn_Debug.Name = "Btn_Debug";
            this.Btn_Debug.Size = new System.Drawing.Size(90, 49);
            this.Btn_Debug.TabIndex = 4;
            this.Btn_Debug.Text = "调试";
            this.Btn_Debug.UseVisualStyleBackColor = false;
            this.Btn_Debug.Click += new System.EventHandler(this.Btn_Debug_Click);
            // 
            // Btn_Initialize
            // 
            this.Btn_Initialize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
       
            this.Btn_Initialize.Font = new System.Drawing.Font("华文中宋", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn_Initialize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            linkPLC5.GetName = null;

            linkPLC5.SetName = null;
            linkPLC5.StatusColor = ((System.Collections.Generic.Dictionary<byte, System.Drawing.Color>)(resources.GetObject("linkPLC5.StatusColor")));
            linkPLC5.UpliftClickNames = ((System.Collections.Generic.Dictionary<byte, string>)(resources.GetObject("linkPLC5.UpliftClickNames")));
            linkPLC5.ValueClickTexts = ((System.Collections.Generic.Dictionary<byte, string>)(resources.GetObject("linkPLC5.ValueClickTexts")));


            //this.Btn_Initialize.KeyValuePairs = linkPLC5;
            this.Btn_Initialize.Location = new System.Drawing.Point(204, 13);
            this.Btn_Initialize.Name = "Btn_Initialize";
            this.Btn_Initialize.Size = new System.Drawing.Size(180, 49);
            this.Btn_Initialize.TabIndex = 3;
            this.Btn_Initialize.Text = "初始化";
            this.Btn_Initialize.UseVisualStyleBackColor = false;
            this.Btn_Initialize.Click += new System.EventHandler(this.Btn_Initialize_Click);
            // 
            // Btn_Stop
            // 
            this.Btn_Stop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
   
            this.Btn_Stop.Font = new System.Drawing.Font("华文中宋", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn_Stop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            linkPLC6.GetName = null;
  
            linkPLC6.SetName = null;
            linkPLC6.StatusColor = ((System.Collections.Generic.Dictionary<byte, System.Drawing.Color>)(resources.GetObject("linkPLC6.StatusColor")));
            linkPLC6.UpliftClickNames = ((System.Collections.Generic.Dictionary<byte, string>)(resources.GetObject("linkPLC6.UpliftClickNames")));
            linkPLC6.ValueClickTexts = ((System.Collections.Generic.Dictionary<byte, string>)(resources.GetObject("linkPLC6.ValueClickTexts")));
        
            //this.Btn_Stop.KeyValuePairs = linkPLC6;
            this.Btn_Stop.Location = new System.Drawing.Point(108, 13);
            this.Btn_Stop.Name = "Btn_Stop";
            this.Btn_Stop.Size = new System.Drawing.Size(90, 49);
            this.Btn_Stop.TabIndex = 1;
            this.Btn_Stop.Text = "停止";
            this.Btn_Stop.UseVisualStyleBackColor = false;
            this.Btn_Stop.Click += new System.EventHandler(this.Btn_Stop_Click);
            // 
            // Btn_Start
            // 
            this.Btn_Start.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
      
            this.Btn_Start.Font = new System.Drawing.Font("华文中宋", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Btn_Start.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            linkPLC7.GetName = null;
      
            linkPLC7.SetName = null;
            linkPLC7.StatusColor = ((System.Collections.Generic.Dictionary<byte, System.Drawing.Color>)(resources.GetObject("linkPLC7.StatusColor")));
            linkPLC7.UpliftClickNames = ((System.Collections.Generic.Dictionary<byte, string>)(resources.GetObject("linkPLC7.UpliftClickNames")));
            linkPLC7.ValueClickTexts = ((System.Collections.Generic.Dictionary<byte, string>)(resources.GetObject("linkPLC7.ValueClickTexts")));
          

            //this.Btn_Start.KeyValuePairs = linkPLC7;
            this.Btn_Start.Location = new System.Drawing.Point(17, 13);
            this.Btn_Start.Name = "Btn_Start";
            this.Btn_Start.Size = new System.Drawing.Size(90, 49);
            this.Btn_Start.TabIndex = 0;
            this.Btn_Start.Text = "启动";
            this.Btn_Start.UseVisualStyleBackColor = false;
            this.Btn_Start.Click += new System.EventHandler(this.Btn_Start_Click);
            // 
            // UserInterfaceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.Btn_Massge);
            this.Controls.Add(this.Btn_Reset);
            this.Controls.Add(this.Btn_Pause);
            this.Controls.Add(this.Btn_Debug);
            this.Controls.Add(this.Btn_Initialize);
            this.Controls.Add(this.Btn_Stop);
            this.Controls.Add(this.Btn_Start);
            this.Name = "UserInterfaceControl";
            this.Size = new System.Drawing.Size(397, 130);
            this.ResumeLayout(false);

        }

        #endregion

        public PLCBtn Btn_Start;
        public PLCBtn Btn_Stop;
        public PLCBtn Btn_Initialize;
        public PLCBtn Btn_Debug;
        public PLCBtn Btn_Pause;
        public PLCBtn Btn_Reset;
        public PLCBtn Btn_Massge;
    }
}
