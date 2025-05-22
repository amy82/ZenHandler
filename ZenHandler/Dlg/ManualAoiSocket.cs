using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZenHandler.Dlg
{
    public partial class ManualAoiSocket : UserControl
    {
        public bool bManualStopKey;
        //private int MovePos;
        
        private System.Windows.Forms.Timer ManualTimer;

        private Button[] Left_MotorX_BtnArr = new Button[5];
        private Button[] Left_MotorZ_BtnArr = new Button[5];

        private Button[] Right_MotorX_BtnArr = new Button[5];
        private Button[] Right_MotorZ_BtnArr = new Button[5];

        private Button[] AoiL_SensorBtnArr = new Button[4];
        private Button[] AoiR_SensorBtnArr = new Button[4];

        protected CancellationTokenSource cts;  //TODO: <--이름 변경하고 사용가능하게
        private bool isMovingMagazine;

        private int CurrentMagazine = 0;        //0 = LFET , 1 = RIGHT
        //MEMO: 티칭 위치를 어떻게 가져올 것인가
        public ManualAoiSocket()
        {
            InitializeComponent();
            bManualStopKey = false;
            cts = new CancellationTokenSource();

            isMovingMagazine = false;

            ManualTimer = new System.Windows.Forms.Timer();
            ManualTimer.Interval = 300; // 1초 (1000밀리초) 간격 설정
            ManualTimer.Tick += new EventHandler(Manual_Timer_Tick);


            ManualUiSet();
        }
        private void ManualUiSet()
        {
            int i = 0;
            Left_MotorX_BtnArr[0] = button_ManualAoi_Socket_L_Wait_Pos_X;
            Left_MotorX_BtnArr[1] = button_ManualAoi_Socket_L_Load_Pos_X;
            Left_MotorX_BtnArr[2] = button_ManualAoi_Socket_L_Unload_Pos_X;
            Left_MotorX_BtnArr[3] = button_ManualAoi_Socket_L_TestL_Pos_X;
            Left_MotorX_BtnArr[4] = button_ManualAoi_Socket_L_TestR_Pos_X;

            Left_MotorZ_BtnArr[0] = button_ManualAoi_Socket_L_Wait_Pos_Z;
            Left_MotorZ_BtnArr[1] = button_ManualAoi_Socket_L_Load_Pos_Z;
            Left_MotorZ_BtnArr[2] = button_ManualAoi_Socket_L_Unload_Pos_Z;
            Left_MotorZ_BtnArr[3] = button_ManualAoi_Socket_L_H_In_Pos_Z;
            Left_MotorZ_BtnArr[4] = button_ManualAoi_Socket_L_H_Out_Pos_Z;

            Right_MotorX_BtnArr[0] = button_ManualAoi_Socket_R_Wait_Pos_X;
            Right_MotorX_BtnArr[1] = button_ManualAoi_Socket_R_Load_Pos_X;
            Right_MotorX_BtnArr[2] = button_ManualAoi_Socket_R_Unload_Pos_X;
            Right_MotorX_BtnArr[3] = button_ManualAoi_Socket_R_TestL_Pos_X;
            Right_MotorX_BtnArr[4] = button_ManualAoi_Socket_R_TestR_Pos_X;

            Right_MotorZ_BtnArr[0] = button_ManualAoi_Socket_R_Wait_Pos_Z;
            Right_MotorZ_BtnArr[1] = button_ManualAoi_Socket_R_Load_Pos_Z;
            Right_MotorZ_BtnArr[2] = button_ManualAoi_Socket_R_Unload_Pos_Z;
            Right_MotorZ_BtnArr[3] = button_ManualAoi_Socket_R_H_In_Pos_Z;
            Right_MotorZ_BtnArr[4] = button_ManualAoi_Socket_R_H_Out_Pos_Z;

            for (i = 0; i < Left_MotorX_BtnArr.Length; i++)
            {
                Left_MotorX_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                Left_MotorX_BtnArr[i].ForeColor = Color.White;

                Left_MotorX_BtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }
            for (i = 0; i < Left_MotorZ_BtnArr.Length; i++)
            {
                Left_MotorZ_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                Left_MotorZ_BtnArr[i].ForeColor = Color.White;

                Left_MotorZ_BtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }

            for (i = 0; i < Right_MotorX_BtnArr.Length; i++)
            {
                Right_MotorX_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                Right_MotorX_BtnArr[i].ForeColor = Color.White;

                Right_MotorX_BtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }
            for (i = 0; i < Right_MotorZ_BtnArr.Length; i++)
            {
                Right_MotorZ_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                Right_MotorZ_BtnArr[i].ForeColor = Color.White;

                Right_MotorZ_BtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }


            //MotorBtnArr[0].BackColor = ColorTranslator.FromHtml("#4C4743");   //모터 위치 이동 완료시 색


            AoiL_SensorBtnArr[0] = button_ManualAoi_Socket_Left_L_Vacuum_On;
            AoiL_SensorBtnArr[1] = button_ManualAoi_Socket_Left_L_Piece_Detect;
            AoiL_SensorBtnArr[2] = button_ManualAoi_Socket_Left_R_Piece_Detect;
            AoiL_SensorBtnArr[3] = button_ManualAoi_Socket_Left_R_Vacuum_On;

            AoiR_SensorBtnArr[0] = button_ManualAoi_Socket_Right_L_Vacuum_On;
            AoiR_SensorBtnArr[1] = button_ManualAoi_Socket_Right_L_Piece_Detect;
            AoiR_SensorBtnArr[2] = button_ManualAoi_Socket_Right_R_Piece_Detect;
            AoiR_SensorBtnArr[3] = button_ManualAoi_Socket_Right_R_Vacuum_On;


            for (i = 0; i < AoiL_SensorBtnArr.Length; i++)
            {
                AoiL_SensorBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                AoiL_SensorBtnArr[i].ForeColor = Color.White;
                AoiL_SensorBtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }
            for (i = 0; i < AoiR_SensorBtnArr.Length; i++)
            {
                AoiR_SensorBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                AoiR_SensorBtnArr[i].ForeColor = Color.White;
                AoiR_SensorBtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }

            
            


        }




        private void ManualLoadVacuumOn(int index, bool bFlag)
        {
            if (Globalo.motionManager.magazineHandler.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.magazineHandler.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Globalo.motionManager.magazineHandler.RunState = OperationState.Stopped;
            //Globalo.motionManager.magazineHandler.LoadVacuumOn(index, bFlag);
        }
        private void ManualUnLoadVacuumOn(int index, bool bFlag)
        {
            if (Globalo.motionManager.magazineHandler.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.magazineHandler.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            Globalo.motionManager.magazineHandler.RunState = OperationState.Stopped;
            //Globalo.motionManager.magazineHandler.UnLoadVacuumOn(index, bFlag);
        }


        private async void Manual_Z_Move(Machine.MagazineHandler.eTeachingPosList ePos)
        {
            if (Globalo.motionManager.magazineHandler.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.magazineHandler.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.magazineHandler.RunState == OperationState.Preparing)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 운전 준비 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.magazineHandler.RunState == OperationState.OriginRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 원점 동작 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (isMovingMagazine == true || Globalo.motionManager.magazineHandler.IsMoving())
            {
                Globalo.LogPrint("", "MAGAZINE Z AXIS MOTOR RUNNING.", Globalo.eMessageName.M_INFO);
                Console.WriteLine("Z motor running...");
                return;
            }
            Globalo.motionManager.magazineHandler.RunState = OperationState.Stopped;
            isMovingMagazine = true;        //<---이동후 기다리지 않으면 바로 true로 바껴서 얘로만 체크하면 위험

            cts?.Dispose();
            cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            
            string logstr = $"[MANUAL] MAGAZINE Z AXIS {ePos.ToString()} Move";
            Globalo.LogPrint("", logstr);

            Machine.eMagazine magazineMotor;
            if (CurrentMagazine == 0)
            {
                magazineMotor = Machine.eMagazine.MAGAZINE_L_Y;
            }
            else
            {
                magazineMotor = Machine.eMagazine.MAGAZINE_R_Y;
            }
            try
            {
                Task<bool> motorTask = Task.Run(() =>
                {
                    Console.WriteLine(" ------------------> ManualZ_Move");
                    bool rtn = Globalo.motionManager.magazineHandler.Magazine_Z_Move(ePos, magazineMotor);
                    bool bComplete = true;

                    int nTimeTick = Environment.TickCount;
                    while (rtn)
                    {
                        if (bManualStopKey) break;
                        bComplete = Globalo.motionManager.magazineHandler.ChkZMotorPos(ePos, magazineMotor);

                        if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MANUAL_MOVE_TIMEOUT)
                        {
                            bComplete = false;
                            Console.WriteLine(" ===> ManualZ_Move TIMEOUT");
                            break;
                        }
                        Thread.Sleep(10);
                    }

                    return bComplete;
                }, cts.Token);

                bool result = await motorTask;      //여기서 대기했다가 Task 빠져나오면 아래 if문으로 이동한다.
                //TODO: 이때 팝업 SHOW모달로 팝업띄우고 거기에 정지 버튼 추가 하는게 좋을 듯

                if (result)
                {
                    Console.WriteLine("Move okok");
                    logstr = $"[MANUAL] MAGAZINE Z AXIS {ePos.ToString()} Move Complete";
                }
                else
                {
                    Console.WriteLine("Move fail");
                    logstr = $"[MANUAL] MAGAZINE Z AXIS {ePos.ToString()} Move Fail";
                }
                Globalo.LogPrint("", logstr);
            }
            catch (OperationCanceledException)
            {
                Globalo.LogPrint("ManualControl", $"모터 작업이 취소되었습니다");
            }
            catch (Exception ex)
            {
                // 그 외 예외 처리
                Globalo.LogPrint("ManualControl", $"모터 이동 실패: {ex.Message}");
            }
            bManualStopKey = false;
            isMovingMagazine = false;
        }


        private async void Manual_Y_Move(Machine.MagazineHandler.eTeachingPosList ePos)
        {
            if (Globalo.motionManager.magazineHandler.RunState == OperationState.AutoRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 자동 운전 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.magazineHandler.RunState == OperationState.Paused)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 일시 정지 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.magazineHandler.RunState == OperationState.Preparing)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 운전 준비 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (Globalo.motionManager.magazineHandler.RunState == OperationState.OriginRunning)
            {
                Globalo.LogPrint("ManualControl", "[INFO] 원점 동작 중 사용 불가", Globalo.eMessageName.M_WARNING);
                return;
            }
            if (isMovingMagazine || Globalo.motionManager.magazineHandler.IsMoving())
            {
                Console.WriteLine("Y motor running...");
                Globalo.LogPrint("", "MAGAZINE Y AXIS MOTOR RUNNING.", Globalo.eMessageName.M_INFO);
                return;
            }
            Globalo.motionManager.magazineHandler.RunState = OperationState.Stopped;

            isMovingMagazine = true;//<---이동후 기다리지 않으면 바로 true로 바껴서 얘로만 체크하면 위험

            string logstr = $"[MANUAL] MAGAZINE Y  AXIS {ePos.ToString()} Move";
            Globalo.LogPrint("", logstr);

            Machine.eMagazine magazineMotor;
            if (CurrentMagazine == 0)
            {
                magazineMotor = Machine.eMagazine.MAGAZINE_L_Y;
            }
            else
            {
                magazineMotor = Machine.eMagazine.MAGAZINE_R_Y;
            }

            try
            {
                cts?.Dispose();
                cts = new CancellationTokenSource();
                CancellationToken token = cts.Token;

                Task<bool> motorTask = Task.Run(() =>
                {
                    Console.WriteLine(" ------------------> TransFer_XY_Move");

                    bool rtn = Globalo.motionManager.magazineHandler.Magazine_Y_Move(ePos, magazineMotor);
                    bool bComplete = true;

                    int nTimeTick = Environment.TickCount;
                    while (rtn)
                    {
                        if (bManualStopKey) break;

                        bComplete = Globalo.motionManager.magazineHandler.ChkYMotorPos(ePos, magazineMotor);
                        if (bComplete)
                        {
                            //위치 확인 완료
                            Console.WriteLine(" ===> MagazineYMove Complete");
                            break;
                        }
                        if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MANUAL_MOVE_TIMEOUT)
                        {
                            bComplete = false;
                            Console.WriteLine(" ===> MagazineYMove TIMEOUT");
                            break;
                        }
                        Thread.Sleep(10);
                    }

                    return bComplete;
                }, cts.Token);

                bool result = await motorTask;      //여기서 대기했다가 Task 빠져나오면 아래 if문으로 이동한다.
                //TODO: 이때 팝업 SHOW모달로 팝업띄우고 거기에 정지 버튼 추가 하는게 좋을 듯

                if (result)
                {
                    Console.WriteLine("Move okok");
                    logstr = $"[MANUAL] MAGAZINE Y AXIS {ePos.ToString()} Move Complete";
                    Globalo.LogPrint("", logstr);
                }
                else
                {
                    Console.WriteLine("Move fail");
                    logstr = $"[MANUAL] MAGAZINE Y AXIS {ePos.ToString()} Move Fail";
                    Globalo.LogPrint("", logstr);
                }

                bManualStopKey = false;
                isMovingMagazine = false;
            }
            catch (OperationCanceledException)
            {
                Globalo.LogPrint("ManualControl", $"모터 작업이 취소되었습니다");
            }
            catch (Exception ex)
            {
                // 그 외 예외 처리
                Globalo.LogPrint("ManualControl", $"모터 이동 실패: {ex.Message}");
            }

            
        }

        
        public void showPanel()
        {
            if (ProgramState.ON_LINE_MOTOR == true)
            {
                //TeachingTimer.Start();
            }

        }
        public void hidePanel()
        {
            ManualTimer.Stop();
        }

        private void ManualMagazine_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                bManualStopKey = false;
                ManualTimer.Start();
            }
            else
            {
                bManualStopKey = true;      //TODO: 테스트 필요 
                ManualTimer.Stop();
            }
        }
        //-------------------------------------------------------------------------------------------------------------------------------------
        //
        // X, Y 축 
        //
        //
        //
        //
        //
        #region [TRANSFER X,Y MOTOR MOVE]
        
        private void button_Manual_Magazine_Wait_Pos_Y_Click(object sender, EventArgs e)
        {
            Manual_Y_Move(Machine.MagazineHandler.eTeachingPosList.WAIT_POS);
        }

        // X,Y BCR SCAN
        private void button_Manual_Transfer_Left_Bcr_Pos_XY_Click(object sender, EventArgs e)
        {
            Manual_Y_Move(Machine.MagazineHandler.eTeachingPosList.LAYER1);
        }
        private void button_Manual_Transfer_Right_Bcr_Pos_XY_Click(object sender, EventArgs e)
        {
            Manual_Y_Move(Machine.MagazineHandler.eTeachingPosList.LAYER2);
        }
        // X,Y TRAY 제품 로드
        private void BTN_MANUAL_TRANSFER_LEFT_LOAD_POS_XY_Click(object sender, EventArgs e)
        {
            Manual_Y_Move(Machine.MagazineHandler.eTeachingPosList.LAYER3);
        }
        private void BTN_MANUAL_TRANSFER_LEFT_UNLOAD_POS_XY_Click(object sender, EventArgs e)
        {
            Manual_Y_Move(Machine.MagazineHandler.eTeachingPosList.LAYER4);
        }
        
        private void BTN_MANUAL_TRANSFER_RIGHT_LOAD_POS_XY_Click(object sender, EventArgs e)
        {
            Manual_Y_Move(Machine.MagazineHandler.eTeachingPosList.LAYER5);
        }
        
        #endregion
        //-------------------------------------------------------------------------------------------------------------------------------------
        //
        //Z 축 
        //
        //
        //
        //
        //
        #region [TRANSFER Z MOTOR MOVE]

        private void button_Manual_Magazine_Wait_Pos_Z_Click(object sender, EventArgs e)
        {
            Manual_Z_Move(Machine.MagazineHandler.eTeachingPosList.WAIT_POS);
        }
        //Z BCR
        private void button_Manual_Magazine_Layer1_Pos_Z_Click(object sender, EventArgs e)
        {
            Manual_Z_Move(Machine.MagazineHandler.eTeachingPosList.LAYER1);
        }
        private void button_Manual_Magazine_Layer2_Pos_Z_Click(object sender, EventArgs e)
        {
            Manual_Z_Move(Machine.MagazineHandler.eTeachingPosList.LAYER2);
        }
        //Z TRAY 제품 로드
        private void button_Manual_Magazine_Layer3_Pos_Z_Click(object sender, EventArgs e)
        {
            Manual_Z_Move(Machine.MagazineHandler.eTeachingPosList.LAYER3);
        }
        private void button_Manual_Magazine_Layer4_Pos_Z_Click(object sender, EventArgs e)
        {
            Manual_Y_Move(Machine.MagazineHandler.eTeachingPosList.LAYER4);
        }
        private void button_Manual_Magazine_Layer5_Pos_Z_Click(object sender, EventArgs e)
        {
            Manual_Z_Move(Machine.MagazineHandler.eTeachingPosList.LAYER5);
        }
        
        #endregion


        //-------------------------------------------------------------------------------------------------------------------------------------
        //
        //IO 동작
        //
        //
        //
        //
        //

        private void Manual_Timer_Tick(object sender, EventArgs e)
        {
            int i = 0;
            bool bRtn = false;
            //------------------------------------------------------------------------------------------------------
            //
            //
            //  LEFT SOCKET
            //
            //
            //------------------------------------------------------------------------------------------------------
            int index = 0;
            for (i = 0; i < Left_MotorX_BtnArr.Length; i++)
            {
                index = i;
                Machine.AoiSocketMachine.eTeachingAoiPosList pos = (Machine.AoiSocketMachine.eTeachingAoiPosList)index;

                
                if (Globalo.motionManager.socketAoiMachine.ChkMotorPos(pos, Machine.eAoiSocket.SOCKET_L_X) == true)
                {
                    Left_MotorX_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
                }
                else
                {
                    Left_MotorX_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                }
            }

            
            for (i = 0; i < Left_MotorZ_BtnArr.Length; i++)
            {
                
                //2칸 건너띄어야함
                if (i < 3)
                {
                    index = i;
                }
                else
                {
                    index = i + 2;
                }
                Machine.AoiSocketMachine.eTeachingAoiPosList pos = (Machine.AoiSocketMachine.eTeachingAoiPosList)index;

                if (Globalo.motionManager.socketAoiMachine.ChkMotorPos(pos, Machine.eAoiSocket.SOCKET_L_Z) == true)
                {
                    Left_MotorZ_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
                }
                else
                {
                    Left_MotorZ_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                }
            }



            if (Globalo.motionManager.socketAoiMachine.GetIsProductInSocket(0, 0, true))
            {
                AoiL_SensorBtnArr[0].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
            }
            else
            {
                AoiL_SensorBtnArr[0].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
            }
            if (Globalo.motionManager.socketAoiMachine.GetVacuumOn(0, 0, true))
            {
                AoiL_SensorBtnArr[1].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
            }
            else
            {
                AoiL_SensorBtnArr[1].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
            }

            if (Globalo.motionManager.socketAoiMachine.GetIsProductInSocket(0, 1, true))
            {
                AoiL_SensorBtnArr[2].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
            }
            else
            {
                AoiL_SensorBtnArr[2].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
            }
            if (Globalo.motionManager.socketAoiMachine.GetVacuumOn(0, 2, true))
            {
                AoiL_SensorBtnArr[3].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
            }
            else
            {
                AoiL_SensorBtnArr[3].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
            }

            //------------------------------------------------------------------------------------------------------
            //
            //
            //  RIGHT SOCKET
            //
            //
            //------------------------------------------------------------------------------------------------------
            for (i = 0; i < Right_MotorX_BtnArr.Length; i++)
            {
                Machine.MagazineHandler.eTeachingPosList pos = (Machine.MagazineHandler.eTeachingPosList)i;


                if (Globalo.motionManager.magazineHandler.ChkYMotorPos(pos, Machine.eMagazine.MAGAZINE_R_Y) == true)
                {
                    Right_MotorX_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
                }
                else
                {
                    Right_MotorX_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                }
            }

            for (i = 0; i < Right_MotorZ_BtnArr.Length; i++)
            {
                Machine.MagazineHandler.eTeachingPosList pos = (Machine.MagazineHandler.eTeachingPosList)i;

                if (Globalo.motionManager.magazineHandler.ChkZMotorPos(pos, Machine.eMagazine.MAGAZINE_R_Y) == true)
                {
                    Right_MotorZ_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
                }
                else
                {
                    Right_MotorZ_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                }
            }


            if (Globalo.motionManager.socketAoiMachine.GetIsProductInSocket(1, 0, true))
            {
                AoiR_SensorBtnArr[0].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
            }
            else
            {
                AoiR_SensorBtnArr[0].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
            }
            if (Globalo.motionManager.socketAoiMachine.GetVacuumOn(1, 0, true))
            {
                AoiR_SensorBtnArr[1].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
            }
            else
            {
                AoiR_SensorBtnArr[1].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
            }

            if (Globalo.motionManager.socketAoiMachine.GetIsProductInSocket(1, 1, true))
            {
                AoiR_SensorBtnArr[2].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
            }
            else
            {
                AoiR_SensorBtnArr[2].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
            }
            if (Globalo.motionManager.socketAoiMachine.GetVacuumOn(1, 2, true))
            {
                AoiR_SensorBtnArr[3].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
            }
            else
            {
                AoiR_SensorBtnArr[3].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
            }
        }

    }
}
