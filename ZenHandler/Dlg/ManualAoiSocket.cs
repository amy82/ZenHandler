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

        private Button[] Aoi_L_MotorX_BtnArr = new Button[5];
        private Button[] Aoi_L_MotorZ_BtnArr = new Button[3];

        private Button[] Aoi_R_MotorX_BtnArr = new Button[5];
        private Button[] Aoi_R_MotorZ_BtnArr = new Button[3];


        private Button[] Aoi_L_SensorBtnArr = new Button[6];
        private Button[] Aoi_R_SensorBtnArr = new Button[6];

        protected CancellationTokenSource cts;  //TODO: <--이름 변경하고 사용가능하게
        private bool isMovingMagazine;

        private int CurrentAoiMotor = 0;        //0 = LFET , 1 = RIGHT
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
            Aoi_L_MotorX_BtnArr[0] = button_ManualAoi_Socket_L_Wait_Pos_X;
            Aoi_L_MotorX_BtnArr[1] = button_ManualAoi_Socket_L_Load_Pos_X;
            Aoi_L_MotorX_BtnArr[2] = button_ManualAoi_Socket_L_Unload_Pos_X;
            Aoi_L_MotorX_BtnArr[3] = button_ManualAoi_Socket_L_TestL_Pos_X;
            Aoi_L_MotorX_BtnArr[4] = button_ManualAoi_Socket_L_TestR_Pos_X;
            //
            Aoi_L_MotorZ_BtnArr[0] = button_ManualAoi_Socket_L_Wait_Pos_Z;
            Aoi_L_MotorZ_BtnArr[1] = button_ManualAoi_Socket_L_H_In_Pos_Z;
            Aoi_L_MotorZ_BtnArr[2] = button_ManualAoi_Socket_L_H_Out_Pos_Z;


            Aoi_R_MotorX_BtnArr[0] = button_ManualAoi_Socket_R_Wait_Pos_X;
            Aoi_R_MotorX_BtnArr[1] = button_ManualAoi_Socket_R_Load_Pos_X;
            Aoi_R_MotorX_BtnArr[2] = button_ManualAoi_Socket_R_Unload_Pos_X;
            Aoi_R_MotorX_BtnArr[3] = button_ManualAoi_Socket_R_TestL_Pos_X;
            Aoi_R_MotorX_BtnArr[4] = button_ManualAoi_Socket_R_TestR_Pos_X;

            Aoi_R_MotorZ_BtnArr[0] = button_ManualAoi_Socket_R_Wait_Pos_Z;
            Aoi_R_MotorZ_BtnArr[1] = button_ManualAoi_Socket_R_H_In_Pos_Z;
            Aoi_R_MotorZ_BtnArr[2] = button_ManualAoi_Socket_R_H_Out_Pos_Z;

            for (i = 0; i < Aoi_L_MotorX_BtnArr.Length; i++)
            {
                Aoi_L_MotorX_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                Aoi_L_MotorX_BtnArr[i].ForeColor = Color.White;

                Aoi_L_MotorX_BtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }
            for (i = 0; i < Aoi_L_MotorZ_BtnArr.Length; i++)
            {
                Aoi_L_MotorZ_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                Aoi_L_MotorZ_BtnArr[i].ForeColor = Color.White;

                Aoi_L_MotorZ_BtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }
            for (i = 0; i < Aoi_R_MotorX_BtnArr.Length; i++)
            {
                Aoi_R_MotorX_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                Aoi_R_MotorX_BtnArr[i].ForeColor = Color.White;

                Aoi_R_MotorX_BtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }
            for (i = 0; i < Aoi_R_MotorZ_BtnArr.Length; i++)
            {
                Aoi_R_MotorZ_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                Aoi_R_MotorZ_BtnArr[i].ForeColor = Color.White;

                Aoi_R_MotorZ_BtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }


            //MotorBtnArr[0].BackColor = ColorTranslator.FromHtml("#4C4743");   //모터 위치 이동 완료시 색


            Aoi_L_SensorBtnArr[0] = button_ManualAoi_Socket_Left_L_Piece_Detect;
            Aoi_L_SensorBtnArr[1] = button_ManualAoi_Socket_Left_L_Vacuum_On;
            Aoi_L_SensorBtnArr[2] = button_ManualAoi_Socket_Left_L_Vacuum_Off;

            Aoi_L_SensorBtnArr[3] = button_ManualAoi_Socket_Left_R_Piece_Detect;
            Aoi_L_SensorBtnArr[4] = button_ManualAoi_Socket_Left_R_Vacuum_On;
            Aoi_L_SensorBtnArr[5] = button_ManualAoi_Socket_Left_R_Vacuum_Off;



            for (i = 0; i < Aoi_L_SensorBtnArr.Length; i++)
            {
                Aoi_L_SensorBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                Aoi_L_SensorBtnArr[i].ForeColor = Color.White;
                Aoi_L_SensorBtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
            }



            Aoi_R_SensorBtnArr[0] = button_ManualAoi_Socket_Right_L_Piece_Detect;
            Aoi_R_SensorBtnArr[1] = button_ManualAoi_Socket_Right_L_Vacuum_On;
            Aoi_R_SensorBtnArr[2] = button_ManualAoi_Socket_Right_L_Vacuum_Off;

            Aoi_R_SensorBtnArr[3] = button_ManualAoi_Socket_Right_R_Piece_Detect;
            Aoi_R_SensorBtnArr[4] = button_ManualAoi_Socket_Right_R_Vacuum_On;
            Aoi_R_SensorBtnArr[5] = button_ManualAoi_Socket_Right_R_Vacuum_Off;

            for (i = 0; i < Aoi_R_SensorBtnArr.Length; i++)
            {
                Aoi_R_SensorBtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                Aoi_R_SensorBtnArr[i].ForeColor = Color.White;
                Aoi_R_SensorBtnArr[i].FlatAppearance.BorderColor = ColorTranslator.FromHtml("#BBBBBB");
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


        private async void Manual_Z_Move(Machine.AoiSocketMachine.eTeachingAoiPosList ePos, Machine.eAoiSocket aoiMotor)
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
                Globalo.LogPrint("", "AOI SOCKET Z AXIS MOTOR RUNNING.", Globalo.eMessageName.M_INFO);
                Console.WriteLine("Z motor running...");
                return;
            }
            Globalo.motionManager.magazineHandler.RunState = OperationState.Stopped;
            isMovingMagazine = true;        //<---이동후 기다리지 않으면 바로 true로 바껴서 얘로만 체크하면 위험

            cts?.Dispose();
            cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            
            string logstr = $"[MANUAL] AOI SOCKET Z AXIS {ePos.ToString()} Move";
            Globalo.LogPrint("", logstr);


            try
            {
                Task<bool> motorTask = Task.Run(() =>
                {
                    Console.WriteLine(" ------------------> ManualZ_Move");
                    bool rtn = Globalo.motionManager.socketAoiMachine.Socket_Z_Move(ePos, aoiMotor);
                    bool bComplete = true;

                    int nTimeTick = Environment.TickCount;
                    while (rtn)
                    {
                        if (bManualStopKey) break;
                        bComplete = Globalo.motionManager.socketAoiMachine.ChkMotorPos(ePos, aoiMotor);

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
                    logstr = $"[MANUAL] AOI SOCKET Z AXIS {ePos.ToString()} Move Complete";
                }
                else
                {
                    Console.WriteLine("Move fail");
                    logstr = $"[MANUAL] AOI SOCKET Z AXIS {ePos.ToString()} Move Fail";
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


        private async void Manual_X_Move(Machine.AoiSocketMachine.eTeachingAoiPosList ePos, Machine.eAoiSocket aoiMotor)
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
                Globalo.LogPrint("", "AOI SOCKET X AXIS MOTOR RUNNING.", Globalo.eMessageName.M_INFO);
                return;
            }
            Globalo.motionManager.magazineHandler.RunState = OperationState.Stopped;

            isMovingMagazine = true;//<---이동후 기다리지 않으면 바로 true로 바껴서 얘로만 체크하면 위험

            string logstr = $"[MANUAL] AOI SOCKET X  AXIS {ePos.ToString()} Move";
            Globalo.LogPrint("", logstr);


            try
            {
                cts?.Dispose();
                cts = new CancellationTokenSource();
                CancellationToken token = cts.Token;

                Task<bool> motorTask = Task.Run(() =>
                {
                    Console.WriteLine(" ------------------> Manual_X_Move");

                    bool rtn = Globalo.motionManager.socketAoiMachine.Socket_X_Move(ePos, aoiMotor);
                    bool bComplete = true;

                    int nTimeTick = Environment.TickCount;
                    while (rtn)
                    {
                        if (bManualStopKey) break;

                        bComplete = Globalo.motionManager.socketAoiMachine.ChkMotorPos(ePos, aoiMotor);
                        if (bComplete)
                        {
                            //위치 확인 완료
                            Console.WriteLine(" ===> SocketX Move Complete");
                            break;
                        }
                        if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MANUAL_MOVE_TIMEOUT)
                        {
                            bComplete = false;
                            Console.WriteLine(" ===> SocketX Move TIMEOUT");
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
                    logstr = $"[MANUAL] AOI SOCKET X AXIS {ePos.ToString()} Move Complete";
                    Globalo.LogPrint("", logstr);
                }
                else
                {
                    Console.WriteLine("Move fail");
                    logstr = $"[MANUAL] AOI SOCKET X AXIS {ePos.ToString()} Move Fail";
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
        #region [AOI SOCKET X MOTOR MOVE]
        
        private void button_ManualAoi_Socket_L_Wait_Pos_X_Click(object sender, EventArgs e)
        {
            Manual_X_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.WAIT_POS, Machine.eAoiSocket.SOCKET_L_X);
        }
        private void button_ManualAoi_Socket_L_Load_Pos_X_Click(object sender, EventArgs e)
        {
            Manual_X_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.LOAD_POS, Machine.eAoiSocket.SOCKET_L_X);
        }
        private void button_ManualAoi_Socket_L_Unload_Pos_X_Click(object sender, EventArgs e)
        {
            Manual_X_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.UN_LOAD_POS, Machine.eAoiSocket.SOCKET_L_X);
        }
        private void button_ManualAoi_Socket_L_TestL_Pos_X_Click(object sender, EventArgs e)
        {
            Manual_X_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.CAPTURE_L_POS, Machine.eAoiSocket.SOCKET_L_X);
        }
        private void button_ManualAoi_Socket_L_TestR_Pos_X_Click(object sender, EventArgs e)
        {
            Manual_X_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.CAPTURE_R_POS, Machine.eAoiSocket.SOCKET_L_X);
        }

        private void button_ManualAoi_Socket_R_Wait_Pos_X_Click(object sender, EventArgs e)
        {
            Manual_X_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.WAIT_POS, Machine.eAoiSocket.SOCKET_R_X);
        }

        private void button_ManualAoi_Socket_R_Load_Pos_X_Click(object sender, EventArgs e)
        {
            Manual_X_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.WAIT_POS, Machine.eAoiSocket.SOCKET_R_X);
        }

        private void button_ManualAoi_Socket_R_Unload_Pos_X_Click(object sender, EventArgs e)
        {
            Manual_X_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.WAIT_POS, Machine.eAoiSocket.SOCKET_R_X);
        }

        private void button_ManualAoi_Socket_R_TestL_Pos_X_Click(object sender, EventArgs e)
        {
            Manual_X_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.WAIT_POS, Machine.eAoiSocket.SOCKET_R_X);
        }

        private void button_ManualAoi_Socket_R_TestR_Pos_X_Click(object sender, EventArgs e)
        {
            Manual_X_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.WAIT_POS, Machine.eAoiSocket.SOCKET_R_X);
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
        #region [AOI SOCKET Z MOTOR MOVE]

        private void button_ManualAoi_Socket_L_Wait_Pos_Z_Click(object sender, EventArgs e)
        {
            Manual_Z_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.WAIT_POS, Machine.eAoiSocket.SOCKET_L_Z);
        }
        private void button_ManualAoi_Socket_L_H_In_Pos_Z_Click(object sender, EventArgs e)
        {
            Manual_Z_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.HOUSING_IN_POS, Machine.eAoiSocket.SOCKET_L_Z);
        }
        private void button_ManualAoi_Socket_L_H_Out_Pos_Z_Click(object sender, EventArgs e)
        {
            Manual_Z_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.HOUSING_OUT_POS, Machine.eAoiSocket.SOCKET_L_Z);
        }

        private void button_ManualAoi_Socket_R_Wait_Pos_Z_Click(object sender, EventArgs e)
        {
            Manual_Z_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.WAIT_POS, Machine.eAoiSocket.SOCKET_R_Z);
        }

        private void button_ManualAoi_Socket_R_H_In_Pos_Z_Click(object sender, EventArgs e)
        {
            Manual_Z_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.HOUSING_IN_POS, Machine.eAoiSocket.SOCKET_R_Z);
        }

        private void button_ManualAoi_Socket_R_H_Out_Pos_Z_Click(object sender, EventArgs e)
        {
            Manual_Z_Move(Machine.AoiSocketMachine.eTeachingAoiPosList.HOUSING_OUT_POS, Machine.eAoiSocket.SOCKET_R_Z);
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
            for (i = 0; i < Aoi_L_MotorX_BtnArr.Length; i++)
            {
                index = i;
                Machine.AoiSocketMachine.eTeachingAoiPosList pos = (Machine.AoiSocketMachine.eTeachingAoiPosList)index;

                
                if (Globalo.motionManager.socketAoiMachine.ChkMotorPos(pos, Machine.eAoiSocket.SOCKET_L_X) == true)
                {
                    Aoi_L_MotorX_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
                }
                else
                {
                    Aoi_L_MotorX_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                }
            }

            
            for (i = 0; i < Aoi_L_MotorZ_BtnArr.Length; i++)
            {
                
                //2칸 건너띄어야함
                if (i < 1)
                {
                    index = i;
                }
                else
                {
                    index = i + 4;
                }
                Machine.AoiSocketMachine.eTeachingAoiPosList pos = (Machine.AoiSocketMachine.eTeachingAoiPosList)index;

                if (Globalo.motionManager.socketAoiMachine.ChkMotorPos(pos, Machine.eAoiSocket.SOCKET_L_Z) == true)
                {
                    Aoi_L_MotorZ_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
                }
                else
                {
                    Aoi_L_MotorZ_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                }
            }



            if (Globalo.motionManager.socketAoiMachine.GetIsProductInSocket(0, 0, true))
            {
                Aoi_L_SensorBtnArr[0].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
            }
            else
            {
                Aoi_L_SensorBtnArr[0].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
            }
            if (Globalo.motionManager.socketAoiMachine.GetVacuumOn(0, 0, true))
            {
                Aoi_L_SensorBtnArr[1].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
            }
            else
            {
                Aoi_L_SensorBtnArr[1].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
            }

            if (Globalo.motionManager.socketAoiMachine.GetIsProductInSocket(0, 1, true))
            {
                Aoi_L_SensorBtnArr[3].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
            }
            else
            {
                Aoi_L_SensorBtnArr[3].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
            }
            if (Globalo.motionManager.socketAoiMachine.GetVacuumOn(0, 2, true))
            {
                Aoi_L_SensorBtnArr[4].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
            }
            else
            {
                Aoi_L_SensorBtnArr[4].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
            }


            //------------------------------------------------------------------------------------------------------
            //
            //
            //  RIGHT SOCKET
            //
            //
            //------------------------------------------------------------------------------------------------------
            for (i = 0; i < Aoi_R_MotorX_BtnArr.Length; i++)
            {
                index = i;
                Machine.AoiSocketMachine.eTeachingAoiPosList pos = (Machine.AoiSocketMachine.eTeachingAoiPosList)index;


                if (Globalo.motionManager.socketAoiMachine.ChkMotorPos(pos, Machine.eAoiSocket.SOCKET_R_X) == true)
                {
                    Aoi_R_MotorX_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
                }
                else
                {
                    Aoi_R_MotorX_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                }
            }

            for (i = 0; i < Aoi_R_MotorZ_BtnArr.Length; i++)
            {

                //2칸 건너띄어야함
                if (i < 1)
                {
                    index = i;
                }
                else
                {
                    index = i + 4;
                }
                Machine.AoiSocketMachine.eTeachingAoiPosList pos = (Machine.AoiSocketMachine.eTeachingAoiPosList)index;

                if (Globalo.motionManager.socketAoiMachine.ChkMotorPos(pos, Machine.eAoiSocket.SOCKET_L_Z) == true)
                {
                    Aoi_R_MotorZ_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_ON);
                }
                else
                {
                    Aoi_L_MotorZ_BtnArr[i].BackColor = ColorTranslator.FromHtml(ButtonColor.MANUAL_BTN_OFF);
                }
            }
        }

       
    }
}
