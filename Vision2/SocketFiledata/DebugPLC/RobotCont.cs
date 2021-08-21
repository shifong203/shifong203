using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using ServerSocket;

namespace ErosSocket.DebugPLC
{
        public static class RobotControl
        {

            public static MyServerSocket ServiceForRobor = new MyServerSocket("192.168.0.2", 2000);
            // public static bool is_RobotConected = ServiceForRobor.Is_Connected;
            public static void PC_ToConect_Robot()
            {
                if (ServiceForRobor == null)
                {
                    ServiceForRobor = new MyServerSocket("192.168.0.2", 2000);
                }
                ServiceForRobor.ToClientConnect();
            }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSendData"></param>
        /// <returns></returns>
            private static bool sendData(string strSendData)
            {
                ServiceForRobor.ServerReceiveContentStr = "";
                strSendData = strSendData + "\r\n";
                if (!ServiceForRobor.Is_Connected)
                {
                    // is_RobotConected = ServiceForRobor.Is_Connected;

                   return false;
                }
                //is_RobotConected = ServiceForRobor.Is_Connected;
                ServiceForRobor.SendData(strSendData);
                return true;
            }
            public static string ReceiveData()
            {
                return ServiceForRobor.ServerReceiveContentStr.Replace("\r\n", ""); ;
            }

            //---------------------------------------------------
            static Stopwatch st = new Stopwatch();
            static string strCurent_X = "";
            static string strCurent_Y = "";
            static string strCurent_Z = "";
            static string strCurent_U = "";
            static string strCurent_V = "";
            static string strCurent_W = "";
 
            static bool ReceiveRobotDataJudge(string RobotBackStr)
            {

                st.Reset();
                st.Start();

                int overTime = 20000;

                if (is_Tin_Or_Flux)
                {
                    overTime = 40000;
                    is_Tin_Or_Flux = false;
                }
                while (ReceiveData() != RobotBackStr)
                {
                    if (!ServiceForRobor.Is_Connected)
                    {
                        break;
                    }
                    if (st.ElapsedMilliseconds > overTime)
                    {
                       break;
                    }
                }
                st.Stop();
                st.Reset();

                string str = ReceiveData();
                if (str == RobotBackStr)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public static bool SetMotorsOn()
            {
                sendData("SetMotorsOn");
                return ReceiveRobotDataJudge("Motors_On");
            }

            public static bool SetMotorsOff()
            {
                sendData("SetMotorsOff");
                return ReceiveRobotDataJudge("Motors_Off");
            }

            public static bool SetPowerHigh()
            {
                sendData("SetPower high");
                return ReceiveRobotDataJudge("Power_High");
            }

            public static bool SetPowerLow()
            {
                sendData("SetPower Low");
                return ReceiveRobotDataJudge("Power_Low");
            }

            public static bool Reset()
            {
                sendData("Reset");
                return ReceiveRobotDataJudge("Reseted");
            }
            #region

            public static bool Resume()
            {
                sendData("Resume");
                return ReceiveRobotDataJudge("Resumed");
            }
            public static bool Pause()
            {
                sendData("Halt");
                return ReceiveRobotDataJudge("Halted");
            }
            #endregion
            public static bool ContinueStepMove(string strAxis, string strDir, string speedPercent, string StepCount)
            {
                 //VisionControl.DoEvents();
                sendData("StepMove" + " " + strAxis + " " + strDir + " " + speedPercent + " " + StepCount);
                return ReceiveRobotDataJudge("StepMoveDone");
            }
            private static bool TeachPoint(string robotPointfilm, string pointNo, ref string strCurent_X, ref string strCurent_Y, ref string strCurent_Z,
                ref string strCurent_U, ref string strCurent_V, ref string strCurent_W)
            {
                pointNo = (Convert.ToInt32(pointNo) + 1).ToString();
                sendData("TeachPoint" + " " + robotPointfilm + " " + pointNo);
                char[] separator = { ' ' };
                bool is_OverTime = false;
                st.Reset();
                st.Start();
                while (ReceiveData().Length < 10)
                {
                    if (st.ElapsedMilliseconds > 500)
                    {
                        is_OverTime = true;
                        break;
                    }
                }
                st.Stop();
                st.Reset();


                string[] receiveDataAnalys = ReceiveData().Split(separator, StringSplitOptions.RemoveEmptyEntries);

                if (is_OverTime)
                {
                 //   Message.TeachRobotPointBackDataOverTime();
                    return false;
                }
                if (receiveDataAnalys.Count() >= 6)
                {
                    strCurent_X = receiveDataAnalys[0];
                    strCurent_Y = receiveDataAnalys[1];
                    strCurent_Z = receiveDataAnalys[2];
                    strCurent_U = receiveDataAnalys[3];
                    strCurent_V = receiveDataAnalys[4];
                    strCurent_W = receiveDataAnalys[5];
                    return true;
                }
                else
                {
                    return false;
                }

            }
            public static bool MoveAppoint(string speed, string acceVelocity, string retardVelocity, string pointNO)
            {
              //  VisionControl.DoEvents();
                string str = (Convert.ToInt32(pointNO) + 1).ToString();
                sendData("MoveAppoint" + " " + speed + " " + acceVelocity + " " + retardVelocity + " " + str);
                return ReceiveRobotDataJudge("AppointMoveDone");
            }


            public static bool RobotPointsfilmSet(string pointfilmName, string pointNO, string X_Value, string Y_Value, string Z_Value, string U_Value, string V_Value, string W_Value)
            {
                sendData("PointsfilmSet" + " " + pointfilmName + " " + pointNO + " " + X_Value + " " + Y_Value + " " + Z_Value +
                    " " + U_Value + " " + V_Value + " " + W_Value);
                return ReceiveRobotDataJudge("SinglePointSetOK");
            }
            private static bool readDataBaseCoord(string productReceipeName, string row_th, ref string[] strAllCoordValue)
            {
               // VisionControl.DoEvents();
                string str = "";
                char[] seprator = { '|' };
      //          MyRobotPointXML.RobotPoint_XML_List_Read(productReceipeName, row_th, ref str);
                if (str == "")
                {
                    return false;
                }
                strAllCoordValue = str.Split(seprator, StringSplitOptions.RemoveEmptyEntries);
                return true;

            }
     //       static DelProcessBarDisp ProcessBarDisp;
            static string temRembProductReceipe = "";
            public static void InitialRobotPointsfilm(string productReceipeName, bool is_PointEditSave = false)
            {
            //    if (!is_PointEditSave)
            //    {
            //        if (productReceipeName == temRembProductReceipe)
            //        {
            //            return;
            //        }
            //    }
            //    temRembProductReceipe = productReceipeName;
            //    string[] strAllCoordValue = new string[6];
            // //   InitialHint hintInterface = new InitialHint(ref ProcessBarDisp);
            ////    hintInterface.Show();
            //    int pointCount = 50;
            //    //if (MyFile.JudeIsSpecilProduct(productReceipeName))
            //    //{
            //    //    pointCount = 51;
            //    //}
            //    for (int i = 0; i < pointCount; i++)
            //    {
            //    //    if (i < 50)
            //    //    {
            //    //        ProcessBarDisp(i * 2);
            //    //    }

            //        if (!RobotControl.readDataBaseCoord(productReceipeName, i.ToString(), ref strAllCoordValue))
            //        {
            //            //Message.RobotPointReadDataBaseFail();

            //            break;
            //        }
            //        if (!RobotControl.RobotPointsfilmSet("Points.PTS", (i + 1).ToString(), strAllCoordValue[0], strAllCoordValue[1],
            //            strAllCoordValue[2], strAllCoordValue[3], strAllCoordValue[4], strAllCoordValue[5]))
            //        {
            //            //Message.RobotInitalPointsFail();
            //            break;
            //        }

            //    }

            //    RobotControl.SetPallet(productReceipeName, (int)RobotPointEnum.RobotPointName._Cmtray1_3);

            //    RobotControl.SetPallet(productReceipeName, (int)RobotPointEnum.RobotPointName._Cmtray2_3);

            //    RobotControl.SetPallet(productReceipeName, (int)RobotPointEnum.RobotPointName._CmtrayNG_3);

            //    RobotControl.SetPallet(productReceipeName, (int)RobotPointEnum.RobotPointName._CmtrayCln_3);
            //    ProcessBarDisp(100);
            //    //Thread.Sleep(200);
            //    hintInterface.Close();
            //    hintInterface.Dispose();

            }
            static void GetPalletRowAndColum(string strProductRecipe, ref string strRow, ref string strColum)
            {
                //string[] strProductParam = new string[33];
                //string strReadData = "";
                //char[] separator = { systemConstant.strParamSeprator };
                //MyProdcutParamXML.ReadParamInformation(strProductRecipe, ref strReadData);
                //strProductParam = strReadData.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                //strRow = strProductParam[11];
                //strColum = strProductParam[12];
            }

            public static bool TeachPoint(string strProductRecipe, string strPointNO = "-1", bool is_SetPallat = false)
            {
                //if (strProductRecipe.Trim() == "")
                //{
                //    Message.ProductNameIsEmpty();
                //    return false;
                //}

                //string strAllAxisCoordValue = "";
                //if (!RobotControl.TeachPoint("Points.pts", strPointNO, ref strCurent_X, ref strCurent_Y, ref strCurent_Z,
                //        ref strCurent_U, ref strCurent_V, ref strCurent_W))
                //{
                //    return false;
                //}
                //strAllAxisCoordValue = strCurent_X + systemConstant.strRobotPointSeprator +
                //    strCurent_Y + systemConstant.strRobotPointSeprator + strCurent_Z + systemConstant.strRobotPointSeprator
                //    + strCurent_U + systemConstant.strRobotPointSeprator + strCurent_V + systemConstant.strRobotPointSeprator
                //    + strCurent_W;

                //MyRobotPointXML.RobotPoint_XML_List_Save(strProductRecipe, strPointNO,
                //    strAllAxisCoordValue);
                //if (is_SetPallat)
                //{
                //    return SetPallet(strProductRecipe, ManualControlFrom.trayPointNO);
                //}
                return true;
            }

            public static bool SetPallet(string strProductRecipe, int trayPointNo)
            {
                string strRow = "";
                string strColum = "";
                //-----------------------------------------------------------------------------------------------------------------------
                //if (trayPointNo >= (int)RobotPointEnum.RobotPointName._Cmtray1_1 && trayPointNo <= (int)RobotPointEnum.RobotPointName._Cmtray1_3)
                //{
                //    GetPalletRowAndColum(strProductRecipe, ref strRow, ref strColum);
                //    sendData("SetPallet Set_1Pallet" + " " + strRow + " " + strColum);
                //    ManualControlFrom.trayPointNO = 0;
                //    return ReceiveRobotDataJudge("SetPalletOK");
                //}
                //if (trayPointNo >= (int)RobotPointEnum.RobotPointName._Cmtray2_1 && trayPointNo <= (int)RobotPointEnum.RobotPointName._Cmtray2_3)
                //{
                //    GetPalletRowAndColum(strProductRecipe, ref strRow, ref strColum);
                //    sendData("SetPallet Set_2Pallet" + " " + strRow + " " + strColum);
                //    ManualControlFrom.trayPointNO = 0;
                //    return ReceiveRobotDataJudge("SetPalletOK");
                //}
                //if (trayPointNo >= (int)RobotPointEnum.RobotPointName._CmtrayCln_1 && trayPointNo <= (int)RobotPointEnum.RobotPointName._CmtrayCln_3)
                //{
                //    GetPalletRowAndColum(strProductRecipe, ref strRow, ref strColum);
                //    sendData("SetPallet SetClnPallet" + " " + strRow + " " + strColum);
                //    ManualControlFrom.trayPointNO = 0;
                //    return ReceiveRobotDataJudge("SetPalletOK");
                //}
                //if (trayPointNo >= (int)RobotPointEnum.RobotPointName._CmtrayNG_1 && trayPointNo <= (int)RobotPointEnum.RobotPointName._CmtrayNG_3)
                //{
                //    GetPalletRowAndColum(strProductRecipe, ref strRow, ref strColum);
                //    sendData("SetPallet SetNGPallet" + " " + strRow + " " + strColum);
                //    ManualControlFrom.trayPointNO = 0;
                //    return ReceiveRobotDataJudge("SetPalletOK");
                //}
                return true;

            }

            public static void TeachCmDectPoint()
            {
                //ArrayList productRecipe = new ArrayList();
                //string strAllAxisCoordValue = "";
                //if (!RobotControl.TeachPoint("Points.pts", "12", ref strCurent_X, ref strCurent_Y, ref strCurent_Z,
                //                  ref strCurent_U, ref strCurent_V, ref strCurent_W))
                //{
                //    Message.TeachPointFail();
                //    return;
                //}
                //strAllAxisCoordValue = strCurent_X + systemConstant.strRobotPointSeprator +
                //   strCurent_Y + systemConstant.strRobotPointSeprator + strCurent_Z + systemConstant.strRobotPointSeprator
                //   + strCurent_U + systemConstant.strRobotPointSeprator + strCurent_V + systemConstant.strRobotPointSeprator
                //   + strCurent_W;

                //MyProductXML.ReadAllProductRecipeName(ref productRecipe);
                //foreach (string item in productRecipe)
                //{
                //    MyRobotPointXML.RobotPoint_XML_List_Save(item, "12",
                //        strAllAxisCoordValue);
                //}
                //Message.TeachPointOK();
                //GC.Collect();
            }

            public static bool TeachCCD_CenterPoint(int CCDNo)
            {
                int pointNo = CCDNo + 94;
                if (!RobotControl.TeachPoint("Points.pts", pointNo.ToString(), ref strCurent_X, ref strCurent_Y, ref strCurent_Z,
                                            ref strCurent_U, ref strCurent_V, ref strCurent_W))
                {

                    return false;
                }
                return true;
            }

            public static bool TeachCCDTool_ToMarkCentrePoint(int CCDNo)
            {
                int pointNo = CCDNo + 96;
                if (!RobotControl.TeachPoint("Points.pts", pointNo.ToString(), ref strCurent_X, ref strCurent_Y, ref strCurent_Z,
                                            ref strCurent_U, ref strCurent_V, ref strCurent_W))
                {

                    return false;
                }
                return true;
            }

            public static bool TeachCCDRefPoint(int CCDNo)
            {
                sendData("TeachToolCCD" + (CCDNo + 1).ToString() + "RefPoint");

                return ReceiveRobotDataJudge("TeachToolCCDRefPointOK");
            }

            public static bool TeachCalibVisionPixelCoord(string strCCDNo, string strPointNo, string strPixel_x, string strPixel_y)
            {
                sendData("TeachCalibCCDPixel" + " " + strCCDNo + " " + strPointNo + " " + strPixel_x + " " + strPixel_y);
                return ReceiveRobotDataJudge("TeachCalibCCDPixelOK");
            }
            public static bool TeachCalibRobotPoint(string strCCDNo, string strPointNo, string strRobot_x, string strRobot_y, string strRobot_z, string strRobot_u,
                string strRobot_v, string strRobot_w)
            {
                sendData("TeachCalibRobotPoint" + " " + strCCDNo + " " + strPointNo + " " + strRobot_x + " " + strRobot_y + " " + strRobot_z + " " + strRobot_u + " "
                    + strRobot_v + " " + strRobot_w);
                return ReceiveRobotDataJudge("TeachCalibRobotPointOK");
            }

            public static bool MoveToCmDecPoint(bool Jog = false)
            {
            //    if (Jog)
            //    {
            //        return RobotControl.MoveAppoint("10", "5", "10", ((int)RobotPointEnum.RobotPointName._CamDetIC).ToString());
            //    }
            //    return RobotControl.MoveAppoint("70", "30", "30", ((int)RobotPointEnum.RobotPointName._CamDetIC).ToString());
            return false;
        }

            public static bool CurrentUseToolSwitch(int toolNO)
            {
                sendData("SwitchTool" + " " + toolNO.ToString());

                return ReceiveRobotDataJudge("SwitchToolOK");
            }

            static bool TeachPoint_Tool(int toolIndex, int u_Status)
            {
                sendData("TeachToolPoint" + " " + toolIndex.ToString() + " " + u_Status.ToString());
                return ReceiveRobotDataJudge("TeachToolPointOK");
            }

            public static bool TeachPoint_CCD1_Tool(int u_Status)
            {
                return TeachPoint_Tool(4, u_Status);
            }

            public static bool TeachPoint_CCD2_Tool(int u_Status)
            {
                return TeachPoint_Tool(2, u_Status);
            }

            public static bool SetTool(int toolindex)
            {
                char[] separator = { ' ' };
                bool is_OverTime = false;
                //if (toolindex == 1)
                //{
                //    RobotTool_Nozzle.x = 0;
                //    RobotTool_Nozzle.y = 0;
                //    RobotTool_Nozzle.z = 0;
                //    RobotTool_Nozzle.u = 0;
                //    RobotTool_Nozzle.v = 0;
                //    RobotTool_Nozzle.w = 0;
                //}
                //else if (toolindex == 2)
                //{
                //    RobotTool_CCD2.x = 0;
                //    RobotTool_CCD2.y = 0;
                //    RobotTool_CCD2.z = 0;
                //    RobotTool_CCD2.u = 0;
                //    RobotTool_CCD2.v = 0;
                //    RobotTool_CCD2.w = 0;
                //}
                //else if (toolindex == 4)
                //{
                //    RobotTool_CCD1.x = 0;
                //    RobotTool_CCD1.y = 0;
                //    RobotTool_CCD1.z = 0;
                //    RobotTool_CCD1.u = 0;
                //    RobotTool_CCD1.v = 0;
                //    RobotTool_CCD1.w = 0;
                //}
                sendData("SetTool" + toolindex.ToString());

                st.Reset();
                st.Start();
                while (ReceiveData().Length < 10)
                {
                    if (st.ElapsedMilliseconds > 500)
                    {
                        is_OverTime = true;
                        break;
                    }
                }
                st.Stop();
                st.Reset();
                //if (is_OverTime)
                //{
                //    Message.TeachRobotPointBackDataOverTime();
                //    return false;
                //}
                string[] receiveDataAnalys = ReceiveData().Split(separator, StringSplitOptions.RemoveEmptyEntries);
            //if (receiveDataAnalys.Count() >= 6)
            //{
            //    if (toolindex == 1)
            //    {
            //        RobotTool_Nozzle.x = Convert.ToDouble(receiveDataAnalys[0]);
            //        RobotTool_Nozzle.y = Convert.ToDouble(receiveDataAnalys[1]);
            //        RobotTool_Nozzle.z = Convert.ToDouble(receiveDataAnalys[2]);
            //        RobotTool_Nozzle.u = Convert.ToDouble(receiveDataAnalys[3]);
            //        RobotTool_Nozzle.v = Convert.ToDouble(receiveDataAnalys[4]);
            //        RobotTool_Nozzle.w = Convert.ToDouble(receiveDataAnalys[5]);
            //    }
            //    else if (toolindex == 2)
            //    {
            //        RobotTool_CCD2.x = Convert.ToDouble(receiveDataAnalys[0]);
            //        RobotTool_CCD2.y = Convert.ToDouble(receiveDataAnalys[1]);
            //        RobotTool_CCD2.z = Convert.ToDouble(receiveDataAnalys[2]);
            //        RobotTool_CCD2.u = Convert.ToDouble(receiveDataAnalys[3]);
            //        RobotTool_CCD2.v = Convert.ToDouble(receiveDataAnalys[4]);
            //        RobotTool_CCD2.w = Convert.ToDouble(receiveDataAnalys[5]);
            //    }
            //    else if (toolindex == 4)
            //    {
            //        RobotTool_CCD1.x = Convert.ToDouble(receiveDataAnalys[0]);
            //        RobotTool_CCD1.y = Convert.ToDouble(receiveDataAnalys[1]);
            //        RobotTool_CCD1.z = Convert.ToDouble(receiveDataAnalys[2]);
            //        RobotTool_CCD1.u = Convert.ToDouble(receiveDataAnalys[3]);
            //        RobotTool_CCD1.v = Convert.ToDouble(receiveDataAnalys[4]);
            //        RobotTool_CCD1.w = Convert.ToDouble(receiveDataAnalys[5]);
            //    }
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            return false;
            }

            public static bool SetICTool(double x_Vision, double y_Vision, double u_Vision, double x_Robot, double y_Robot, double u_Robot, ref double[] toolICCoord)
            {

                sendData("SetICTool" + " " + x_Vision.ToString() + " " + y_Vision.ToString() + " " + u_Vision.ToString() + " " + x_Robot.ToString() + " " + y_Robot.ToString() + " " + u_Robot.ToString());

                char[] separator = { ' ' };
                bool is_OverTime = false;
                st.Reset();
                st.Start();
                while (ReceiveData().Length < 10)
                {
                    if (st.ElapsedMilliseconds > 1000)
                    {
                        is_OverTime = true;
                        break;
                    }
                }
                st.Stop();
                st.Reset();


                string[] strReceiveDataAnalys = ReceiveData().Split(separator, StringSplitOptions.RemoveEmptyEntries);

                if (is_OverTime)
                {
                    //Message.TeachRobotPointBackDataOverTime();
                    return false;
                }
                if (strReceiveDataAnalys.Count() >= 6)
                {
                    toolICCoord[0] = Convert.ToDouble(strReceiveDataAnalys[0]);
                    toolICCoord[1] = Convert.ToDouble(strReceiveDataAnalys[1]);
                    toolICCoord[2] = Convert.ToDouble(strReceiveDataAnalys[2]);
                    toolICCoord[3] = Convert.ToDouble(strReceiveDataAnalys[3]);
                    toolICCoord[4] = Convert.ToDouble(strReceiveDataAnalys[4]);
                    toolICCoord[5] = Convert.ToDouble(strReceiveDataAnalys[5]);
                    return true;
                }
                else
                {
                    return false;
                }
                //if (MainWindow.bSkip_Robot)
                //{
                //    Tool_Product.x = 0;
                //    Tool_Product.y = 0;
                //    Tool_Product.z = 0;
                //    Tool_Product.u = 0;
                //    Tool_Product.v = 0;
                //    Tool_Product.w = 0;

                //    return 0;
                //}
                //char[] separator = new char[2];
                //separator[0] = ' ';
                //separator[1] = '\r';
                //string[] temp = new string[12];
                //if (temp.Length >= 5)
                //{
                //    temp = m_RobotEthernet.ReciveString.Split(separator);
                //    Tool_Product.x = Convert.ToDouble(temp[1]);
                //    Tool_Product.y = Convert.ToDouble(temp[2]);
                //    Tool_Product.z = Convert.ToDouble(temp[3]);
                //    Tool_Product.u = Convert.ToDouble(temp[4]);
                //    Tool_Product.v = Convert.ToDouble(temp[5]);
                //    Tool_Product.w = Convert.ToDouble(temp[6]);
                //}
                //else
                //{

                //}
                //return 0;
            }

            //public static bool Teach_Tool3BasePoint(RobotPointEnum.ToolIC_U index, double u_vision)
            //{
            //    sendData("TeachTool3BasePoint" + " " + " " + index.ToString() + " " + u_vision.ToString());

            //    return ReceiveRobotDataJudge("TeachTool3BasePointOK");
            //}

            public static bool MoveToCCD1Mark(string strU_Status, string speed, string acceVelocity, string retardVelocity)
            {
             //   VisionControl.DoEvents();
                sendData("MoveToCCD1Mark" + " " + strU_Status + " " + speed + " " + acceVelocity + " " + retardVelocity);
                return ReceiveRobotDataJudge("MoveToCCD1MarkOK");
            }
            public static bool MoveToCCD2Mark(string strU_Status, string speed, string acceVelocity, string retardVelocity)
            {
            //    VisionControl.DoEvents();
                sendData("MoveToCCD2Mark" + " " + strU_Status + " " + speed + " " + acceVelocity + " " + retardVelocity);
                return ReceiveRobotDataJudge("MoveToCCD2MarkOK");
            }

            public static bool MoveToCCD1MarkCenter(string speed, string acceVelocity, string retardVelocity)
            {
             //   VisionControl.DoEvents();
                sendData("MoveToCCD1MarkCenter" + " " + speed + " " + acceVelocity + " " + retardVelocity);
                return ReceiveRobotDataJudge("MoveToCCD1MarkCenterOK");
            }
            public static bool MoveToCCD2MarkCenter(string speed, string acceVelocity, string retardVelocity)
            {
           //     VisionControl.DoEvents();
                sendData("MoveToCCD2MarkCenter" + " " + speed + " " + acceVelocity + " " + retardVelocity);
                return ReceiveRobotDataJudge("MoveToCCD2MarkCenterOK");
            }
            public static bool MoveToCCD2DectPoint(string speed, string acceVelocity, string retardVelocity)
            {
             //   VisionControl.DoEvents();
                sendData("MoveToCCD2DectPoint" + " " + speed + " " + acceVelocity + " " + retardVelocity);
                return ReceiveRobotDataJudge("MoveToCCD2DectPointOK");
            }

            public static bool MoveCCDCalibPoint(string strCCNo, string strDir, string strStepDistance, ref string[] strRobotCoord)
            {
             //   VisionControl.DoEvents();
                bool is_OverTime = false;
                sendData("MoveCCDCalib" + " " + strCCNo + " " + strDir + " " + strStepDistance);
                char[] separator = { ' ' };
                string[] strReceiveData = null;
                st.Reset();
                st.Start();

                while (true)
                {
                    strReceiveData = ReceiveData().Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    if (strReceiveData.Length > 5)
                    {
                        break;
                    }
                    //if (st.ElapsedMilliseconds > 7000)
                    //{
                    //    Message.MessageDisplay(MainForm.delMessageDisplay, Message.SystemWarnInfo.RobotReceiveDataOverTime);
                    //    is_OverTime = true;
                    //    break;
                    //}
                }
                st.Stop();
                st.Reset();
                if (is_OverTime)
                {
                    return false;
                }
                strRobotCoord = strReceiveData;
                return true;
            }

            public static bool CalcCalibCCD(int selecetCCDNo, ref string[] strCalibResult)
            {
                sendData("CalibCCD" + (selecetCCDNo + 1).ToString());
                char[] separator = { ' ' };
                string[] strReceiveData = null;
                bool is_OverTime = false;
                st.Reset();
                st.Start();

                while (true)
                {
                    strReceiveData = ReceiveData().Split(separator, StringSplitOptions.RemoveEmptyEntries);
                    if (strReceiveData.Length > 5)
                    {
                        break;
                    }
                    if (st.ElapsedMilliseconds > 7000)
                    {
                        is_OverTime = true;
                     //   Message.MessageDisplay(MainForm.delMessageDisplay, Message.SystemWarnInfo.RobotReceiveDataOverTime);
                        break;
                    }
                }
                if (is_OverTime)
                {
                    return false;
                }
                st.Stop();
                st.Reset();
                strCalibResult = strReceiveData;
                return true;
            }

            public static bool MoveCCDAppointTrayPosition(string strSpeed, string strAcceVelocity, string strRetardVeloc, string strTrayNo, string strPocketNo)
            {
                if (strTrayNo != "4")
                {
                    if (!MoveNozzleToSafePosition(strSpeed, strAcceVelocity, strRetardVeloc))
                    {
                        return false;
                    }
                }
             //   VisionControl.DoEvents();
                sendData("MoveCCDToTrayPocketNo" + " " + strSpeed + " " + strAcceVelocity + " " + strRetardVeloc + " " + strTrayNo + " " + strPocketNo);
                return ReceiveRobotDataJudge("MoveCCDToTrayPocketNoOK");
            }

            //public static bool MoveNozzleToPickUpPos(string strProductRecipeName, ICData PickupICParm, string strSpeed, string strAcceVelocity, string strRetardVelocity, string strTrayNo, string strPocketNo)
            //{
            //  //  VisionControl.DoEvents();
            //    if (!MoveCCDAppointTrayPosition(strSpeed, strAcceVelocity, strRetardVelocity, strTrayNo, strPocketNo))
            //    {
            //  //      Message.MoveToPocketFail();
            //        return false;
            //    }


            //  //  if (!VisionControl.LoadPatMaxTool_PickupIC(strProductRecipeName))
            //  //  {
            //  ////      Message.LoadFileTool_IsNull();
            //  //      return false;
            //  //  }
            //    Thread.Sleep(30);

            //    //if (!VisionControl.PickupIC_Check(PickupICParm, VisionControl.Acquire_CCD(0)))
            //    //{
            //    //    Message.PickupModeDectFail();
            //    //    return false;
            //    //}

            //    sendData("Move_Nozzle_Pick_UpPos" + " " + strSpeed + " " + strAcceVelocity + " " + strRetardVelocity + " " + PickupICParm.x_Vision.ToString()
            //        + " " + PickupICParm.y_Vision + " " + PickupICParm.u_Vision);

            //    return ReceiveRobotDataJudge("Move_Nozzle_Pick_UpPosOK");
            //}

            public static bool MoveNozzleToPickupICPos(string speed_h, string acceVelocity_h, string retardVelocity_h, string speed_L, string acceVelocity_L,
                string retardVelocity_L, string strMoveNozzleDownType, string offset)
            {
            //   VisionControl.DoEvents();
                sendData("MoveNozzleToDown" + " " + speed_h + " " + acceVelocity_h + " " + retardVelocity_h + " " + speed_L + " " + acceVelocity_L + " "
                    + retardVelocity_L + " " + strMoveNozzleDownType + " " + offset);
                return ReceiveRobotDataJudge("MoveNozzleToDownOK");
            }
            public static bool RobotHome()
            {
                sendData("Home");
                return ReceiveRobotDataJudge("HomeOK");
            }

            public static bool MoveNozzleToDownICPostion(string speed_h, string acceVelocity_h, string retardVelocity_h, string strMoveNozzleDownType)
            {
                #region
                string speed_L = "0";
                string acceVelocity_L = "0";
                string retardVelocity_L = "0";
                #endregion
               // VisionControl.DoEvents();
                sendData("MoveNozzleToDown" + " " + speed_h + " " + acceVelocity_h + " " + retardVelocity_h + " " + speed_L + " " + acceVelocity_L + " "
                    + retardVelocity_L + " " + strMoveNozzleDownType + " " + "0");
                return ReceiveRobotDataJudge("MoveNozzleToDownOK");
            }

            public static bool MoveNozzleToDownICPlace(string speed, string acceVelocity, string retardVelocity, string x_PMAl_Pocket, string y_PMAl_Pocket, string u_PMAl_Pocket, string u_PMAl_IC)
            {
        //        VisionControl.DoEvents();
                sendData("MoveNozzleToDownICPlace" + " " + speed + " " + acceVelocity + " " + retardVelocity + " " + x_PMAl_Pocket + " " + y_PMAl_Pocket + " " + u_PMAl_Pocket + " " + u_PMAl_IC);
                return ReceiveRobotDataJudge("MoveNozzleToDownICPlaceOK");
            }

            public static bool GetCurrentRobotPoint(ref double[] receiveDataAnalys)
            {
                sendData("GetCurrentRobotPoint");
                char[] separator = { ' ' };
                bool is_OverTime = false;
                st.Reset();
                st.Start();
                while (ReceiveData().Length < 10)
                {
                    if (st.ElapsedMilliseconds > 1000)
                    {
                        is_OverTime = true;
                        break;
                    }
                }
                st.Stop();
                st.Reset();


                string[] strReceiveDataAnalys = ReceiveData().Split(separator, StringSplitOptions.RemoveEmptyEntries);


                if (is_OverTime)
                {
                //    Message.TeachRobotPointBackDataOverTime();
                    return false;
                }
                if (strReceiveDataAnalys.Count() >= 6)
                {
                    receiveDataAnalys[0] = Convert.ToDouble(strReceiveDataAnalys[0]);
                    receiveDataAnalys[1] = Convert.ToDouble(strReceiveDataAnalys[1]);
                    receiveDataAnalys[2] = Convert.ToDouble(strReceiveDataAnalys[2]);
                    receiveDataAnalys[3] = Convert.ToDouble(strReceiveDataAnalys[3]);
                    receiveDataAnalys[4] = Convert.ToDouble(strReceiveDataAnalys[4]);
                    receiveDataAnalys[5] = Convert.ToDouble(strReceiveDataAnalys[5]);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public static bool MoveNozzleToSafePosition(string speed, string acceVelocity, string retardVelocity)
            {
            //    VisionControl.DoEvents();
                sendData("MoveNozzleToSafePosition" + " " + speed + " " + acceVelocity + " " + retardVelocity);
                return ReceiveRobotDataJudge("MoveNozzleToSafePositionOK");
            }

            //public static bool TeachTool_IC(string strProductName, CheckICAlignData _ICAlignData, string speed, string acceVelocity, string retardVelocity,
            //    ref double AlignU_Vision, bool teachU = false, RobotPointEnum.ToolIC_U index = RobotPointEnum.ToolIC_U.Nothing)
            //{
            //    double[] _Align_vision = new double[3];
            //    double[] _Align_robot = new double[6];
            //    if (!MoveNozzleToSafePosition(speed, acceVelocity, retardVelocity))
            //    {
            //        Message.RobotMoveSafeFail();
            //        return false;
            //    }

            //    if (!VisionControl.LoadPMAlignTool_IC(strProductName))
            //    {
            //        Message.ICLocationToolLoadFail();
            //        return false;
            //    }



            //    if (!MoveToCCD2DectPoint(speed, acceVelocity, retardVelocity))
            //    {
            //        Message.MoveToDectPositionFail();
            //        return false;
            //    }
            //    Thread.Sleep(100);

            //    if (!VisionControl.ICAlignment(VisionControl.Acquire_CCD(1), _ICAlignData))
            //    {
            //        Message.ICModeMacthFail();
            //        return false;
            //    }
            //    _Align_vision[0] = _ICAlignData.x_Vision;
            //    _Align_vision[1] = _ICAlignData.y_Vision;
            //    _Align_vision[2] = _ICAlignData.u_Vision;
            //    AlignU_Vision = _Align_vision[2];

            //    if (!CurrentUseToolSwitch(0))
            //    {
            //        Message.SwitchToolFail();
            //        return false;
            //    }
            //    if (!GetCurrentRobotPoint(ref _Align_robot))
            //    {
            //        Message.GetCurrentPositionFail();
            //        return false;
            //    }
            //    double[] ICToolCoord = new double[6];
            //    if (!SetICTool(_Align_vision[0], _Align_vision[1], _Align_vision[2], _Align_robot[0], _Align_robot[1], _Align_robot[3], ref ICToolCoord))
            //    {
            //        Message.SetICToolFail();
            //        return false;
            //    }
            //    if (teachU)
            //    {
            //        if (!Teach_Tool3BasePoint(index, _ICAlignData.u_Vision))
            //        {
            //            Message.TeachModeWorkBaseUFail();
            //            return false;
            //        }
            //    }
            //    if (!MoveNozzleToSafePosition(speed, acceVelocity, retardVelocity))
            //    {
            //        Message.RobotMoveSafeFail();
            //        return false;
            //    }
            //    return true;
            //}

            public static bool RunSingleFlux(string speed, string acceVelocity, string retardVelocity, string feedingSpeed, string feedingAcceleration, string IC_side_th, string x_offset, string z_offset, string u_vision)
            {
                sendData("RunSingleFlux" + " " + speed + " " + acceVelocity + " " + retardVelocity + " " + feedingSpeed + " " + feedingAcceleration + " " + IC_side_th + " " + x_offset + " " + z_offset + " " + u_vision);
                return ReceiveRobotDataJudge("RunSingleFluxOK");
            }
            //,string feedingSpeed,string feedingAcceleration,string feededTime,

            public static bool RunFlux(string speed, string acceVelocity, string retardVelocity, string feedingSpeed, string feedingAcceleration, string sideCount, string x_offset, string z_offset, string u_vision)//FluxDir fluxDir,
            {
                is_Tin_Or_Flux = true;

                sendData("RunFlux" + " " + speed + " " + acceVelocity + " " + retardVelocity + " " + feedingSpeed + " " + feedingAcceleration + " " + sideCount + " " + x_offset + " " + z_offset + " " + u_vision);
                return ReceiveRobotDataJudge("RunFluxOK");
            }

            public static bool RunSingleTin(string speed, string acceVelocity, string retardVelocity, string feedingSpeed, string feedingAcceleration, string feededSpeed, string feededAcceleration,
                 string delaytime, string strTinIndex, string IC_side_th, string x_offset, string z_offset, string u_vision)
            {
                is_Tin_Or_Flux = true;

                sendData("RunSingleTin" + " " + speed + " " + acceVelocity + " " + retardVelocity + " " + feedingSpeed + " " + feedingAcceleration + " " + feededSpeed + " " + feededAcceleration + " " + delaytime + " " + strTinIndex
                    + " " + IC_side_th + " " + x_offset + " " + z_offset + " " + u_vision);
                return ReceiveRobotDataJudge("RunSingleTinOK");
            }
            static bool is_Tin_Or_Flux = false;

            public static bool RunTin(string speed, string acceVelocity, string retardVelocity, string feedingSpeed, string feedingAcceleration, string feededSpeed, string feededAcceleration,
                 string delaytime, string strTinIndex, string IC_sideCount, string x_offset, string z_offset, string u_vision)
            {
                is_Tin_Or_Flux = true;
                sendData("RunTin" + " " + speed + " " + acceVelocity + " " + retardVelocity + " " + feedingSpeed + " " + feedingAcceleration + " " + feededSpeed + " " + feededAcceleration + " " + delaytime + " " + strTinIndex
                                 + " " + IC_sideCount + " " + x_offset + " " + z_offset + " " + u_vision);
                return ReceiveRobotDataJudge("RunTinOK");
            }
            public static bool DebugMovePreFlux(string speed, string acceVelocity, string retardVelocity, string sideIndex, string x_offset, string z_offset, string u_vision)
            {
                sendData("DebugMovePreFlux" + " " + speed + " " + acceVelocity + " " + retardVelocity + " " + sideIndex + " " + x_offset + " " + z_offset + " " + u_vision);
                return ReceiveRobotDataJudge("DebugMovePreFluxOK");
            }
            public static bool DebugMovePreTin(string speed, string acceVelocity, string retardVelocity, string strTin_Index, string sideIndex, string x_offset, string z_offset, string u_vision)
            {
                sendData("DebugMovePreTin" + " " + speed + " " + acceVelocity + " " + retardVelocity + " " + strTin_Index + " " + sideIndex + " " + x_offset + " " + z_offset + " " + u_vision);
                return ReceiveRobotDataJudge("DebugMovePreTinOK");
            }
        }
    

}
