﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Globalization;

namespace ZenHandler  //ApsMotionControl
{
    public partial class MainForm : Form
    {
        public const int PG_WIDTH = 1920;
        public const int PG_HEIGHT = 1080;

        public const int RunGuideWidth = 330;//164;
        public const int RunButtonWidth = 146;//164;
        

        public const int CamHeight = 330;
        public const int ProductionHeight = 100;
        public const int LogViewHeight = 350;
        
        private int testNum = 0;
        public KeyMessageFilter keyMessageFilter;

        public MainForm()
        {
            InitializeComponent();
            //this.TopMost = true;
            keyMessageFilter = new KeyMessageFilter();
            Application.AddMessageFilter(keyMessageFilter);

            
            Event.EventManager.LanguageChanged += OnLanguageChanged;



            this.Size = new System.Drawing.Size(PG_WIDTH, PG_HEIGHT);
            this.Padding = new Padding(0); // 부모 컨트롤의 여백 제거
            this.Location = new System.Drawing.Point(0, 0);

            Globalo.MainForm = this;
            int dRightPanelW = CenterPanel.Width;
            int dRightPanelH = CenterPanel.Height;



            Globalo.threadControl = new ThreadControl();    //<--log Thread 생성후 로그 출력 가능
            Globalo.mAlarmPanel = new Dlg.AlarmControl(dRightPanelW, dRightPanelH);

            Globalo.yamlManager.AlarmLoad();
            Globalo.yamlManager.secsGemDataYaml.MesLoad();

            string className = typeof(Machine.TransferMachine).Name;

            Globalo.yamlManager.configDataLoad();
            Globalo.yamlManager.taskDataYaml.TaskDataLoad();

            Globalo.yamlManager.RecipeYamlListLoad();

            Globalo.motionManager = new MotionControl.MotionManager();
            Globalo.motionManager.AllMotorParameterSet();

            string fileName = "";
            if (Program.PG_SELECT == HANDLER_PG.FW)
            {
                fileName = string.Format(@"{0}\Fw_Handler_IoMap_v1.3.xlsx", Application.StartupPath);
            }
            else if (Program.PG_SELECT == HANDLER_PG.AOI)
            {
                fileName = string.Format(@"{0}\iomap.xlsx", Application.StartupPath);
            }
            else
            {
                //eeprom
                fileName = string.Format(@"{0}\iomap.xlsx", Application.StartupPath);
            }


            Globalo.dataManage.ioData.ReadEpplusData(fileName);

            // KeyEvent 이벤트 핸들러 추가
            //keyMessageFilter.KeyEvent += KeyMessageFilter_KeyEvent;
            Globalo.mlogControl = new Dlg.LogControl(dRightPanelW, dRightPanelH);

            //모터 초기화
            //
            if (ProgramState.ON_LINE_MOTOR)
            {
                bool rtn = Globalo.motionManager.MotionInit();
                if (rtn == false)
                {
                    //Motor Set Fail!!
                    MessageBox.Show("Motor Set Fail!!");
                }
            }


            Globalo.mMainPanel = new Dlg.MainControl(dRightPanelW, dRightPanelH);
            Globalo.mManualPanel = new Dlg.ManualControl(dRightPanelW, dRightPanelH);
            Globalo.mTeachPanel = new Dlg.TeachingControl(dRightPanelW, dRightPanelH);
            Globalo.mCCdPanel = new Dlg.CCdControl(dRightPanelW, dRightPanelH);
            Globalo.mConfigPanel = new Dlg.ConfigControl(dRightPanelW, dRightPanelH);
            Globalo.mioPanel = new Dlg.IoControl(dRightPanelW, dRightPanelH);
            Globalo.operationPanel = new Dlg.OperationPanel();
            Globalo.productionInfo = new Dlg.ProductionInfo();
            Globalo.trayStateInfo = new Dlg.TrayStateInfo();
            Globalo.socketStateInfo = new Dlg.SocketStateInfo();
            Globalo.pickerInfo = new Dlg.PickerInfo();
            Globalo.tabMenuForm = new Dlg.TabMenuForm(RightPanel.Width, RightPanel.Height);
            //
            this.RightPanel.Controls.Add(Globalo.tabMenuForm);
            
            BTN_TOP_LOG.Location = new System.Drawing.Point(this.TopPanel.Width - BTN_TOP_LOG.Width, 0);
            BTN_TOP_LOG.BackColor = ColorTranslator.FromHtml("#ED6C44");


            Globalo.threadControl.AllThreadStart();

            
            Globalo.yamlManager.vPPRecipeSpecEquip = Globalo.yamlManager.RecipeLoad(Globalo.dataManage.mesData.m_sMesPPID);         //init

            if (Globalo.yamlManager.vPPRecipeSpecEquip == null)
            {
                Globalo.LogPrint("ManualControl", $"[{Globalo.dataManage.mesData.m_sMesPPID}] Recipe Load Fail");
            }



            Globalo.tcpManager = new TcpSocket.TcpManager("127.0.0.1", 2001);
            Globalo.tcpManager.BcrClient.DataReceived += Globalo.motionManager.transferMachine.OnTransferBcrReceived;
            Globalo.mMainPanel.BackColor = ColorTranslator.FromHtml("#F8F3F0");
            Globalo.mCCdPanel.BackColor = ColorTranslator.FromHtml("#F8F3F0");
            Globalo.mConfigPanel.BackColor = ColorTranslator.FromHtml("#F8F3F0");
            Globalo.mAlarmPanel.BackColor = ColorTranslator.FromHtml("#F8F3F0");
            Globalo.mlogControl.BackColor = ColorTranslator.FromHtml("#F8F3F0");

            MainUiSet();

            SerialConnect();    //조명 컨트롤러 , Serial Barcode

            serverStart();      //SECS - GEM 연결

            TopPanel.Paint += new PaintEventHandler(Form_Paint);
            eLogPrint("Main", "PG START");

            Globalo.productionInfo.BcrSet(Globalo.dataManage.TaskWork.m_szChipID);
            Globalo.productionInfo.ProductionInfoSet();
            Globalo.productionInfo.PinCountInfoSet();
            Globalo.pickerInfo.SetLoadPickerInfo();
            Globalo.pickerInfo.SetUnloadPickerInfo();

            Program.SetLanguage(Globalo.yamlManager.configData.DrivingSettings.Language);
        }
        
        private void OnLanguageChanged(object sender, EventArgs e)
        {
            // 이벤트 처리
            //this.Text = Resource.Strings.OP_ORIGIN;
            Console.WriteLine("MainForm - OnLanguageChanged");
        }
        private async void serverStart()
        {
            await Globalo.tcpManager.StartServerAsync();
        }
        public void InitMilLib()
        {
            
            //
            Globalo.vision.AllocMilApplication();
            Globalo.vision.AllocMilCamBuffer();
            Globalo.vision.AllocMilCCdBuffer(0, Globalo.mLaonGrabberClass.m_nWidth, Globalo.mLaonGrabberClass.m_nHeight);

            Globalo.vision.AllocMilCcdDisplay(Globalo.camControl.CcdPanel.Handle);
            Globalo.vision.AllocMilCamDisplay(Globalo.camControl.CamPanel.Handle);

            Globalo.vision.EnableCamOverlay();
            Globalo.vision.EnableCcdOverlay();
            Globalo.vision.DrawOverlay();


            ///Globalo.vision.GrabRun();        //기존 cam grab thread
        }

        private void SerialConnect()
        {
            // 바코드 리더기 Serial Port 설정
            string portData = "";
            bool connectRtn = false;
            


            if(Globalo.yamlManager.configData != null)
            {
                portData = Globalo.yamlManager.configData.SerialPort.Bcr;
            }
            else
            {
                portData = "COM1";
            }

            Globalo.serialPortManager.Barcode = new Serial.SerialCommunicator(portData);
            Globalo.serialPortManager.Barcode.myName = "Bcr";
;            //barcodePort.DataReceived += (sender, data) =>
            //{
            //    Console.WriteLine("Barcode Reader Data: " + data);
            //};
            connectRtn = Globalo.serialPortManager.Barcode.Open();

        }
        
        
        
        private void Form_Paint(object sender, PaintEventArgs e)
        {
            int lineStartY = TopPanel.Height - 1;
            // Graphics 객체 가져오기
            Graphics g = e.Graphics;

            // Pen 객체 생성 (색상과 두께 설정)
            Color color = Color.FromArgb(75, 75, 75);//(86, 86, 86);
            Pen pen = new Pen(color, 2);

            // 라인 그리기 (시작점과 끝점 설정)
            g.DrawLine(pen, 0, lineStartY, TopPanel.Width, lineStartY);

            // 리소스 해제
            pen.Dispose();
        }
        private void MainUiSet()
        {
            //int i = 0;

            //int MainBtnWGap = 4;
            int MainBtnHGap = 2;
            //int MainBtnStartX = 1;
            //int BtnPosX = 0;


            //-----------------------------------------------
            //상단 패널
            //-----------------------------------------------
            TopPanel.BackColor = ColorTranslator.FromHtml("#FAFAFA");
            MainTitleLabel.ForeColor = ColorTranslator.FromHtml("#8F949F");
            MainTitleLabel.BackColor = Color.Transparent;
            MainTitleLabel.Text = "Zen Handler V1 - " + Program.PG_SELECT.ToString();

            //-----------------------------------------------
            int MidPanelHeight = LeftPanel.Height;          //Left Middle 패널 높이
            //int ViewPanelHeight =  LeftPanel.Height - CamHeight - MainBtnHGap - RunButtonHeight - MainBtnHGap;      // 전체 높이에서 -카메라높이 - 버튼 높이 - 생상정보       //로그창 높이
            int nBottomPanelY = RightPanel.Location.Y;     //Bottom 패널 Position Y


            //-----------------------------------------------
            //우측 패널
            //-----------------------------------------------
            RightPanel.BackColor = ColorTranslator.FromHtml("#4C4743");


            //-----------------------------------------------
            //중단 우 패널
            //-----------------------------------------------

            CenterPanel.BackColor = ColorTranslator.FromHtml("#F8F3F0");


            //-----------------------------------------------
            //좌측 패널
            //-----------------------------------------------
            LeftPanel.Controls.Add(Globalo.productionInfo);
            LeftPanel.Controls.Add(Globalo.operationPanel);
            LeftPanel.Controls.Add(Globalo.trayStateInfo);
            LeftPanel.Controls.Add(Globalo.socketStateInfo);
            LeftPanel.Controls.Add(Globalo.pickerInfo);

            Globalo.productionInfo.Location = new System.Drawing.Point(0, 0);
            Globalo.operationPanel.Location = new System.Drawing.Point(LeftPanel.Width - Globalo.operationPanel.Width , Globalo.productionInfo.Height + MainBtnHGap);
            
            Globalo.pickerInfo.Location = new System.Drawing.Point(0, Globalo.operationPanel.Location.Y + MainBtnHGap);
            Globalo.socketStateInfo.Location = new System.Drawing.Point(0, Globalo.pickerInfo.Location.Y + Globalo.pickerInfo.Height + MainBtnHGap);
            Globalo.trayStateInfo.Location = new System.Drawing.Point(Globalo.socketStateInfo.Location.X + Globalo.socketStateInfo.Width+10, Globalo.pickerInfo.Location.Y + Globalo.pickerInfo.Height + MainBtnHGap);

            Globalo.mCCdPanel.Visible = false;
            Globalo.mConfigPanel.Visible = false;
            Globalo.mAlarmPanel.Visible = false;
            Globalo.mlogControl.Visible = false;
            
            CenterPanel.Controls.Add(Globalo.mMainPanel);
            CenterPanel.Controls.Add(Globalo.mManualPanel);
            CenterPanel.Controls.Add(Globalo.mTeachPanel);
            CenterPanel.Controls.Add(Globalo.mioPanel);
            //CenterPanel.Controls.Add(Globalo.mCCdPanel);
            CenterPanel.Controls.Add(Globalo.mConfigPanel);
            CenterPanel.Controls.Add(Globalo.mAlarmPanel);
            CenterPanel.Controls.Add(Globalo.mlogControl);




            
        }
        
        public void MainTitleChange()
        {
            if (ProgramState.CurrentRunMode == ProgramState.eRunMode.ENGINEER)
            {
                //엔지니어 모드일대 상단 배경 색 바꾸자.
                //TopPanel.BackColor = Color.LimeGreen;
                //MainTitleLabel.ForeColor = Color.Black; ;//ColorTranslator.FromHtml("#8F949F");
            }
            else
            {
                //TopPanel.BackColor = Color.MintCream; ;// ColorTranslator.FromHtml("#FAFAFA");
                //MainTitleLabel.ForeColor = Color.DarkCyan;
            }


            string title = ProgramState.PG_TITLE;//// + " - " + ProgramState.CurrentRunMode.ToString() + " MODE";
           
            MainTitleLabel.BackColor = Color.Transparent;
            MainTitleLabel.Text = title;

        }
        
        private void eLogPrint(object oSender, string LogDesc, Globalo.eMessageName bPopUpView = Globalo.eMessageName.M_NULL)
        {
            DateTime dTime = DateTime.Now;
            string LogInfo = $"[{dTime:hh:mm:ss.f}] {LogDesc}";
            Globalo.threadControl.logThread.logQueue.Enqueue(LogInfo);

            if (bPopUpView != Globalo.eMessageName.M_NULL)
            {
                MessagePopUpForm messagePopUp3 = new MessagePopUpForm();
                messagePopUp3.MessageSet(Globalo.eMessageName.M_ERROR, LogDesc);
                messagePopUp3.Show();
            }
        }

        private void BTN_BOTTOM_EXIT_Click(object sender, EventArgs e)
        {
            
        }
        public void ClientConnected(bool state)
        {
            if (state == true)
            {
                BTN_TOP_CLIENT.BackColor = Color.Green;
            }
            else
            {
                BTN_TOP_CLIENT.BackColor = Color.White;
            }
            ProgramState.STATE_CLINET_CONNECT = state;
        }
        public void DriverConnected(bool state)
        {
            if (state == true)
            {
                BTN_TOP_MES.BackColor = Color.Green;
            }
            else
            {
                BTN_TOP_MES.BackColor = Color.White;
            }

            ProgramState.STATE_DRIVER_CONNECT = state;
        }
        public void FuncExit()
        {
            Event.EventManager.RaisePgExit();
            Globalo.threadControl.AllClose();
            Globalo.motionManager.ioController.Close();

            //Time Thread End
            //oGlobal.mDioControl.DioEnd();

            //oGlobal.mtimeThread.mTimeThreadRun = false;
            //if (oGlobal.mtimeThread != null)
            //{
            //oGlobal.mtimeThread.Interrupt();   //스레도 실행 정지
            //oGlobal.mtimeThread.Join();
            //}
            //Vision End
            //oGlobal.vision.ThreadEnd();
            //if (Globalo.GrabberDll.mIsGrabStarted())
            //{
            //    Globalo.GrabberDll.mGrabStop();
            //}
            //Globalo.GrabberDll.mCloseBoard();

            

            if (ProgramState.ON_LINE_MOTOR)
            {
                Globalo.motionManager.MotionClose();
            }

            Globalo.GrabberDll = null;
            //
            //foreach (Form form in Application.OpenForms)
            //{
            //    form.Close();
            //}
            //Globalo.dataManage.teachingData.eLogSender -= eLogPrint;
            //Globalo.motorControl.eLogSender -= eLogPrint;
            //Globalo.dIoControl.eLogSender -= eLogPrint;
            // 다이얼로그
            //
            //mTeachPanel = new Dlg.TeachingControl();
            //mManualPanel = new Dlg.ManualControl();
            // Thread Main
            //

            //Globalo.mLaonGrabberClass.eLogSender -= eLogPrint;
            Globalo.mLaonGrabberClass.Dispose();

            //foreach (var thread in System.Diagnostics.Process.GetCurrentProcess().Threads)
            //{
            //    ((System.Diagnostics.ProcessThread)thread).Dispose();
            //}

            //System.Diagnostics.Process.GetCurrentProcess().Kill();

            Application.Exit();

        }

        

        private void BTN_TOP_LOG_Click(object sender, EventArgs e)
        {

            if (Debugger.IsAttached)
            {
                //string tempLot = "Z23DC24327000030V3WT-13A997-A*";
                string lotname = "Z23DC24327000095V3WT-13A997-A_113410.csv";
                string prefix = "Z23DC24327000095V3WT-13A997-A";

            //    string csvFolderPath = "D:\\EVMS\\LOG\\MMD_DATA\\2025\\02\\28";

            //    // 해당 폴더 안의 모든 파일 리스트 가져오기
            //    string[] files = Directory.GetFiles(csvFolderPath);

            //    // prefix가 포함된 파일명 리스트 필터링
            //    List<string> matchedFiles = files
            //        .Where(file => Path.GetFileName(file).Contains(prefix))
            //        .Select(Path.GetFileName) // 경로가 아닌 파일명만 추출
            //        .ToList();

            //    string earliestFile = matchedFiles
            //        .OrderByDescending(Data.CEEpromData.GetTimeFromFileName) // 시간 기준으로 내림차순 정렬 가장 늦은 시간 출력
            //        //.OrderBy(Data.CEEpromData.GetTimeFromFileName) // // 시간 기준으로 오름차순 정렬
            //        .FirstOrDefault(); // 가장 빠른 시간의 파일 선택




                if (lotname.Contains(prefix))
                {
                    Console.WriteLine("lotname에 prefix가 포함되어 있습니다.");
                }
                else
                {
                    Console.WriteLine("lotname에 prefix가 포함되어 있지 않습니다.");
                }
                testNum++;

                TcpSocket.EquipmentData sendEqipData = new TcpSocket.EquipmentData();

                //sendEqipData.Command = "OBJECT_ID_REPORT";
                //sendEqipData.LotID = "aaaaaaB" + testNum.ToString();

                sendEqipData.Command = "LOT_APD_REPORT";
                sendEqipData.CommandParameter = new List<TcpSocket.EquipmentParameterInfo>();
                for (int i = 0; i < 10; i++)
                {
                    TcpSocket.EquipmentParameterInfo pInfo = new TcpSocket.EquipmentParameterInfo();
                    pInfo.Name = (i+1).ToString();
                    pInfo.Value = (i + 1).ToString() + "value";
                    sendEqipData.CommandParameter.Add(pInfo);
                }
                

                Globalo.tcpManager.SendMessageToClient(sendEqipData);


                //
            }


        }


        
        
        private void BTN_BOTTOM_LIGHT_Click(object sender, EventArgs e)
        {
            //MenuButtonSet(4);
        }

        private void BTN_TOP_MES_Click(object sender, EventArgs e)
        {
            
        }

        
        
        private void BTN_MAIN_JUDGE_RESET_Click(object sender, EventArgs e)
        {
            
        }

        private void BTN_TOP_CCD_Click(object sender, EventArgs e)
        {
            //CCD ON
            //if (ProgramState.CurrentState == OperationState.AutoRunning)
            //{
            //    Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
            //    return;
            //}
            //if (ProgramState.CurrentState == OperationState.ManualTesting)
            //{
            //    Globalo.LogPrint("ManualControl", "[INFO] MANUAL 동작 중 사용 불가", Globalo.eMessageName.M_WARNING);
            //    return;
            //}
            //if (Globalo.threadControl.manualThread.GetThreadRun() == false)
            //{
            //    Globalo.LogPrint("", "[CCD] MANUAL CCD START");
            //    Globalo.threadControl.manualThread.runfn(FThread.ManualThread.eManualType.M_CCD_START);//4);
            //}
        }
        
        

        private void BTN_MAIN_PINCOUNT_RESET_Click(object sender, EventArgs e)
        {
            
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true; // 폼에서 키 이벤트를 받기 위해 설정
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.RemoveMessageFilter(keyMessageFilter);  // 메시지 필터 제거
        }

        private void BTN_TOP_LOG_Click_1(object sender, EventArgs e)
        {
            if (ProgramState.NORINDA_MODE == true)
            {
                // Globalo.motionManager.socketFwMachine.MultiContactUp(0, new int[] { 1, 1, 1, 1 }, true);
                //Globalo.tcpManager.BcrClient.Send("LON\r");

                LeeTest leeTest = new LeeTest();
                leeTest.ShowDialog();
                return;
                //int[] pickerList = { 1, 1, 1, 1 };

                //Globalo.motionManager.transferMachine.LoadMultiPickerUp(pickerList, true);

                //Globalo.motionManager.transferMachine.GetUnloadMultiPickerUp(new int[] { 1, 1, 1, 1 }, true);
                //Globalo.motionManager.transferMachine.pickedProduct.UnloadTrayPos.X = Globalo.motionManager.transferMachine.pickedProduct.LoadTrayPos.X;
                //Globalo.motionManager.transferMachine.pickedProduct.UnloadTrayPos.Y = Globalo.motionManager.transferMachine.pickedProduct.LoadTrayPos.Y;
                //배출위치는 항상 로드하는 위치로 고정시키기

                Globalo.motionManager.transferMachine.pickedProduct = Data.TaskDataYaml.TaskLoad_Transfer(Machine.TransferMachine.taskPath);


                

                

                

                Globalo.motionManager.transferMachine.TransFer_XY_Move(Machine.TransferMachine.eTeachingPosList.LEFT_TRAY_BCR_POS, 0, 0);
                Globalo.motionManager.transferMachine.TransFer_XY_Move(Machine.TransferMachine.eTeachingPosList.LEFT_TRAY_BCR_POS, 1, 0);
                Globalo.motionManager.transferMachine.TransFer_XY_Move(Machine.TransferMachine.eTeachingPosList.LEFT_TRAY_BCR_POS, 2, 0);
                Globalo.motionManager.transferMachine.TransFer_XY_Move(Machine.TransferMachine.eTeachingPosList.LEFT_TRAY_BCR_POS, 3, 0);

                Globalo.motionManager.transferMachine.TransFer_XY_Move(Machine.TransferMachine.eTeachingPosList.LEFT_TRAY_LOAD_POS, 0, 0);
                Globalo.motionManager.transferMachine.TransFer_XY_Move(Machine.TransferMachine.eTeachingPosList.LEFT_TRAY_LOAD_POS, 2, 0);

                Globalo.motionManager.transferMachine.TransFer_XY_Move(Machine.TransferMachine.eTeachingPosList.LEFT_TRAY_LOAD_POS, 0, 1);
                Globalo.motionManager.transferMachine.TransFer_XY_Move(Machine.TransferMachine.eTeachingPosList.LEFT_TRAY_LOAD_POS, 2, 1);


                //배출 시뮬레이션
                int UnloadPosx = Globalo.motionManager.transferMachine.pickedProduct.UnloadTrayPos.X;
                int UnloadPosy = Globalo.motionManager.transferMachine.pickedProduct.UnloadTrayPos.Y;
                int Cnt =  Machine.TransferMachine.UnLoadCount;      //  <--- 배출 개수  ex) 2개

                int StartIndex = UnloadPosx;//// UnloadPosx % Cnt;
                int EndIndex = UnloadPosx + Cnt;/// - StartIndex;
                if (EndIndex > 4)
                {
                    EndIndex = 4;
                }
                int UnloadCnt = EndIndex - StartIndex;
                int[] UnloadPicker = { -1, -1, -1, -1 };
                //
                //항상 0,2 가 아닐수있음 1이나 3일 경우?
                for (int i = UnloadPosx; i < EndIndex; i++)  //x 좌표에서 배출 수만큼 카운트
                {
                    if (Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[i].State == Machine.PickedProductState.Good)
                    {
                        //PIckerList.Add(i);
                        UnloadPicker[i] = 1;
                        Globalo.motionManager.transferMachine.pickedProduct.UnLoadProductInfo[i].State = Machine.PickedProductState.Blank;
                    }
                }
                Console.WriteLine($"피커 다운 범위 : {string.Join(", ", UnloadPicker)}");
                //Globalo.motionManager.transferMachine.LoadMultiPickerUp(LoadTrayOffset, true);  //Socket에서 투입할때 2개이상 사용할 수 있다
                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine($"배출개수 : {Cnt}/{UnloadCnt}, Pos x : {UnloadPosx}");
                Console.WriteLine($"피커 다운 범위 : {StartIndex} ~ {EndIndex}");
                Globalo.motionManager.transferMachine.TransFer_XY_Move(Machine.TransferMachine.eTeachingPosList.LEFT_TRAY_UNLOAD_POS, UnloadPosx, UnloadPosy);
                Globalo.motionManager.transferMachine.UnloadMultiPickerUp(UnloadPicker, true);


                Globalo.motionManager.transferMachine.UnloadTryAdd(UnloadCnt);        //여기서 배출 픽업 위치 로드한 개수만큼 증가


                Globalo.motionManager.transferMachine.TaskSave();

                //for (int i = 0; i < 4; i++)
                //{
                //    Console.WriteLine($"나누기 1: {i/1}, Pos x : {i%1}");
                //}
                //for (int i = 0; i < 4; i+=2)
                //{
                //    Console.WriteLine($"나누기 2: {i / 2}, Pos x : {i % 2}");
                //}
            }
            //
        }
        
    }
}
