//using System;
//using System.ComponentModel;
//using System.Windows.Forms;

//namespace ErosSocket.ErosUI
//{
//    public class PLCBtn : Button
//    {
//        public PLCBtn()
//        {
//        }

//        [CategoryAttribute("状态显示"), DisplayName("显示文本")]
//        public override string Text { get => base.Text; set => base.Text = value; }

//        protected override void OnClick(EventArgs e)
//        {
//            base.OnClick(e);
//            try
//            {
//                //if (SeeName.LingkName != null)
//                //{
//                //    if (StaticCon.SocketClint.ContainsKey(seeName.LingkName))
//                //    {
//                //        LinkPLC.BoolNegate(StaticCon.SocketClint[seeName.LingkName], seeName.ValusFileName);
//                //    }

//                //}
//                //if (seeName.SetName != null && seeName.SetName != "" && seeName.SetName.Contains("."))
//                //{
//                //    if (StaticCon.SocketClint.ContainsKey(seeName.SetName.Split('.')[0]))
//                //    {
//                //        StaticCon.SocketClint[seeName.SetName.Split('.')[0]].SetValues(seeName.SetName.Split('.')[1], seeName.SetName.ToString(), out string err);
//                //    }
//                //}
//            }
//            catch (Exception ex)
//            {
//            }
//        }

//        public void UpRun()
//        {
//            //if (SeeName.LingkName != null)
//            //{
//            //    if (StaticCon.SocketClint.ContainsKey(seeName.LingkName))
//            //    {
//            //        if (StaticCon.SocketClint[seeName.LingkName].KeysValues.DictionaryValueD.ContainsKey(seeName.ValusFileName))
//            //        {
//            //            StaticCon.SocketClint[seeName.LingkName].KeysValues.DictionaryValueD[seeName.ValusFileName].ValueCyEvent += ErosBtn_ValueCyEvent;
//            //        }
//            //    }
//            //}
//        }

//        /// <summary>
//        /// 值改变
//        /// </summary>
//        /// <param name="key"></param>
//        /// <returns></returns>
//        private dynamic ErosBtn_ValueCyEvent(ErosConLink.UClass.ErosValues.ErosValueD key)
//        {
//            try
//            {
//                byte ds = 0;
//                if (key._Type == "Boolean")
//                {
//                    ds = Convert.ToByte(key.Value);
//                }
//                //if (seeName.StatusColor.ContainsKey(ds))
//                //{
//                //    this.BackColor = seeName.StatusColor[ds];
//                //}
//            }
//            catch (Exception ex)
//            {
//            }

//            return false;
//        }

//        //protected override void WndProc(ref Message m)
//        //{
//        //     base.WndProc(ref m);
//        //}
//        public void Updata(dynamic value)
//        {
//            try
//            {
//                //if (seeName.GetName != null && seeName.GetName != "" && seeName.GetName.Contains("."))
//                //{
//                //    if (ErosSocket.ErosConLink.StaticCon.SocketClint.ContainsKey(seeName.GetName.Split('.')[0]))
//                //    {
//                //        string sd = ErosSocket.ErosConLink.StaticCon.SocketClint[seeName.GetName.Split('.')[0]].KeysValues[seeName.GetName.Split('.')[1]].Value.ToStrng();
//                //        byte by = Convert.ToByte(sd);
//                //        if (seeName.StatusColor.ContainsKey(by))
//                //        {
//                //            this.ForeColor = seeName.StatusColor[by];
//                //        }
//                //    }
//                //}
//            }
//            catch (Exception)
//            {
//            }
//        }

//        public override void Refresh()
//        {
//            base.Refresh();
//        }

//        //[DescriptionAttribute("展开以查看链接信息。")]
//        //public LinkPLC SeeName
//        //{
//        //    get { return seeName; }
//        //    set { seeName = value; }
//        //}

//        //private LinkPLC seeName = new LinkPLC();
//    }

//    public class FloatNumTextBox : TextBox
//    {
//        protected override void WndProc(ref Message m)
//        {
//            int WM_CHAR = 0x0102;
//            if (m.Msg == WM_CHAR)
//            {
//                if (((char)m.WParam >= '0') && ((char)m.WParam <= '9') ||
//                (int)m.WParam == (int)Keys.Back || (int)m.WParam == (int)Keys.Delete)
//                {
//                    base.WndProc(ref m);
//                }
//            }
//            else
//            {
//                base.WndProc(ref m);
//            }
//        }
//    }
//}