using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZenHandler.Machine
{
    public enum eLift : int
    {
        LIFT_L_Z = 0, LIFT_R_Z, GANTRYX_L, GANTRYX_R
    };

    public enum eLiftSensor : int
    {
        LIFT_TOPSTOP_POS = 0, LIFT_READY_POS, LIFT_HOME_POS
    };
    public class LiftMachine : MotionControl.MotorController
    {
        public int MotorCnt { get; private set; } = 4;

        //LEFT Z / RIGHT Z / GANTRY FRONT X / GANTRY BACK X / 
        public MotionControl.MotorAxis[] MotorAxes; // 배열 선언

        public string[] axisName = { "FRONT_X", "BACK_X", "LEFT_Z", "RIGHT_Z"};

        private MotorDefine.eMotorType[] motorType = { MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR};
        private AXT_MOTION_LEVEL_MODE[] AXT_SET_LIMIT = { AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW };
        private AXT_MOTION_LEVEL_MODE[] AXT_SET_SERVO_ALARM = { AXT_MOTION_LEVEL_MODE.HIGH, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW };
        private AXT_MOTION_HOME_DETECT[] MOTOR_HOME_SENSOR = { AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.NegEndLimit, AXT_MOTION_HOME_DETECT.NegEndLimit };
        private AXT_MOTION_MOVE_DIR[] MOTOR_HOME_DIR = { AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW};


        private static double[] MaxSpeeds = { 100.0, 100.0, 100.0, 100.0};
        private double[] OrgFirstVel = { 20000.0, 20000.0, 20000.0, 20000.0};
        private double[] OrgSecondVel = { 5000.0, 5000.0, 5000.0, 5000.0 };
        private double[] OrgThirdVel = { 2500.0, 2500.0, 2500.0, 2500.0 };

        public bool[] IsLiftOnTray = { false, false };      //리프트 위 Tray 유무 확인
        public bool[] IsTopLoadOnTray = { false, false };      //상단 Gantry , Pusher Tray 로드 확인

        public bool IsLoadingInputTray = false;         //투입 LIft 투입중
        public bool IsUnloadingOutputTray = false;      //배출 Lift 배출중

        public enum eTeachingPosList : int
        {
            WAIT_POS = 0, LOAD_POS, UNLOAD_POS, TOTAL_LIFT_TEACHING_COUNT
        };

        public string[] TeachName = { "WAIT_POS" , "LOAD_POS", "UNLOAD_POS" };


        public const string teachingPath = "Teach_Lift.yaml";
        public const string taskPath = "Task_Lift.yaml";
        public Data.TeachingConfig teachingConfig = new Data.TeachingConfig();

        //public PickedProduct pickedProduct = new PickedProduct();

        public TrayProduct trayProduct = new TrayProduct();

        public LiftMachine()// : base("LiftModule")
        {
            int i = 0;
            this.RunState = OperationState.Stopped;
            this.MachineName = this.GetType().Name;
            
            MotorAxes = new MotionControl.MotorAxis[MotorCnt];
            for (i = 0; i < MotorCnt; i++)
            {
                int index = (int)MotionControl.MotorSet.ValidLiftMotors[i];
                MotorAxes[i] = new MotionControl.MotorAxis(index,
                axisName[i], motorType[i], MaxSpeeds[i], AXT_SET_LIMIT[i], AXT_SET_SERVO_ALARM[i], OrgFirstVel[i], OrgSecondVel[i], OrgThirdVel[i],
                MOTOR_HOME_SENSOR[i], MOTOR_HOME_DIR[i]);


                //초기 셋 다른 곳에서 다시 해줘야될 듯
                MotorAxes[i].setMotorParameter(10.0, 0.1, 0.1, 1000.0);//(double vel , double acc , double dec , double resol)
                if (this.MotorUse == false)
                {
                    MotorAxes[i].NoUse = true;
                }
            }

            trayProduct = Data.TaskDataYaml.TaskLoad_Lift(taskPath);


        }

        public override bool TaskSave()
        {
            bool rtn = Data.TaskDataYaml.TaskSave_Lift(trayProduct, taskPath);
            return rtn;
        }
        public override void MotorDataSet()
        {
            int i = 0;
            for (i = 0; i < MotorAxes.Length; i++)
            {
                MotorAxes[i].setMotorParameter(teachingConfig.Speed[i], teachingConfig.Accel[i], teachingConfig.Decel[i], teachingConfig.Resolution[i]);
            }

            for (i = 0; i < teachingConfig.Teaching.Count; i++)
            {
                if (i < TeachName.Length)
                {
                    teachingConfig.Teaching[i].Name = TeachName[i];
                }
            }


        }
        #region Lift Machine Io 동작
        public bool GetGantryCenteringFor(bool bFlag, bool bWait = false)
        {
            return false;
        }
        public bool GantryCenteringFor(bool bFlag, bool bWait = false)        //GANTRY 모서리 센터링 전후진
        {
            bool isSuccess = false;

            return isSuccess;
        }
        public bool GetGantryClampFor(bool bFlag, bool bWait = false)
        {
            return false;
        }

        public bool GantryClampFor(bool bFlag, bool bWait = false)        //GANTRY 좌우 전후진 클램프
        {
            bool isSuccess = false;

            return isSuccess;
        }
        public bool GetPUsherUp(bool bFlag, bool bWait = false)
        {
            return false;
        }

        public bool PusherUp(bool bFlag, bool bWait = false)        //푸셔 상승 , 하강
        {
            bool isSuccess = false;

            return isSuccess;
        }
        public bool GetPUsherFor(bool bFlag, bool bWait = false)
        {
            return false;
        }
        public bool PusherFor(bool bFlag, bool bWait = false)       //푸셔 전진 , 후진
        {
            bool isSuccess = false;

            return isSuccess;
        }
        public bool GetTraySlidePos(int index)          //슬라이드 정위치 확인
        {
            return false;
        }
        public bool GetTopTouchSensor(int index)        //TRAY 교체시 리프트 정지 센서
        {
            return false;
        }
        public bool GetMiddleWaitSensor(int index)        //리프트 대기 위치 확인 센서
        {
            return false;
        }
        public bool GetIsLoadTrayOnTop(int index)            //GANTRY, PUSHER 위 TRAY 유무 확인
        {
            return false;
        }
        public bool GetIsTrayOnSlide(int index)              //LEFT , RIGHT SLIDE 위 TRAY 유무 확인
        {
            return false;
        }
        #endregion
        public override bool IsMoving()
        {
            if (AutoUnitThread.GetThreadRun() == true)
            {
                return true;
            }

            for (int i = 0; i < MotorAxes.Length; i++)
            {
                if (MotorAxes[i].GetStopAxis() == false)
                {
                    return true;
                }
            }
            return true;
        }
        #region LIFT Motor 동작
        public bool ChkGantryXMotorPos(eTeachingPosList teachingPos)
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }

            double dXPos = 0.0;
            double dYPos = 0.0;
            double currentXPos = 0.0;
            double currentYPos = 0.0;


            dXPos = Globalo.motionManager.liftMachine.teachingConfig.Teaching[(int)teachingPos].Pos[(int)eLift.GANTRYX_L];
            dYPos = Globalo.motionManager.liftMachine.teachingConfig.Teaching[(int)teachingPos].Pos[(int)eLift.GANTRYX_R];


            currentXPos = MotorAxes[(int)eLift.GANTRYX_L].EncoderPos;
            currentYPos = MotorAxes[(int)eLift.GANTRYX_R].EncoderPos;

            if (dXPos == currentXPos && dYPos == currentYPos)
            {
                return true;
            }

            return false;
        }
        public bool LIft_Z_Move_SersonDetected(eLift motorAxis, eLiftSensor Sensor, bool bWait = true)
        {
            if (this.MotorUse == false)
            {
                Console.WriteLine("No Use Machine");
                return true;
            }

            string logStr = "";
            bool isSuccess = false;
            int moveDic = 1;        //1이면 상승 , 그외 하강
            if (Sensor == eLiftSensor.LIFT_READY_POS)
            {
                moveDic = 1;
                bWait = true;       //무조건 대기
                if (MotorAxes[(int)motorAxis].GetHomeSensor() == true)
                {
                    return true;
                }
            }
            else if (Sensor == eLiftSensor.LIFT_TOPSTOP_POS)
            {
                moveDic = 1;
                bWait = true;       //무조건 대기
                //상단 정지 감지 센서
                if (GetTopTouchSensor((int)motorAxis) == true)
                {
                    return true;
                }
            }
            else if (Sensor == eLiftSensor.LIFT_HOME_POS)
            {
                moveDic = -1;
                if (MotorAxes[(int)motorAxis].GetNegaSensor() == true)
                {
                    return true;
                }
            }

            double dSpeed = MotorAxes[(int)motorAxis].Velocity;

            isSuccess = MotorAxes[(int)motorAxis].JogMove(moveDic, dSpeed);

            if (bWait == false)
            {
                return isSuccess;
            }
            int step = 100;
            int nTimeTick = 0;
            int SkipChk = 0;
            while (true)
            {
                if (MotorAxes[(int)motorAxis].MotorBreak)
                {
                    break;
                }

                switch (step)
                {
                    case 100:
                        isSuccess = false;
                        nTimeTick = Environment.TickCount;
                        step = 200;
                        break;
                    case 200:
                        if (Sensor == eLiftSensor.LIFT_READY_POS)
                        {
                            if (MotorAxes[(int)motorAxis].GetHomeSensor() == true)
                            {
                                MotorAxes[(int)motorAxis].Stop(1);
                                isSuccess = true;
                                step = 1000;
                            }
                        }
                        else if (Sensor == eLiftSensor.LIFT_TOPSTOP_POS)
                        {
                            //상단 정지 감지 센서
                            //g_clDioControl.ReadDIn(4);   //TODO:   <-----------필요?
                            if (GetTopTouchSensor((int)motorAxis) == true)
                            {
                                MotorAxes[(int)motorAxis].Stop(1);
                                isSuccess = true;
                                step = 1000;
                            }
                        }
                        else if (Sensor == eLiftSensor.LIFT_HOME_POS)
                        {
                            if (MotorAxes[(int)motorAxis].GetNegaSensor() == true)
                            {
                                MotorAxes[(int)motorAxis].Stop(1);
                                isSuccess = true;
                                step = 1000;
                            }
                        }
                        break;

                    default:
                        break;
                }
                if (step >= 1000)
                {
                    break;
                }

                if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.LIFT_MOVE_TIMEOUT)
                {
                    isSuccess = false;
                    Console.WriteLine($"Lift Motor Stop Timeout ");
                    break;
                }
                Thread.Sleep(10);
            }
            return isSuccess;
        }
        public bool Gantry_X_Move(eTeachingPosList ePos, bool bWait = true)  //Picket Index , Tray or Socekt or Ng , 
        {
            //TODO: PickerNo 는 없애고 CountX로 써도될듯 확인필요.
            if (this.MotorUse == false)
            {
                Console.WriteLine("No Use Machine");
                return true;
            }
            string logStr = "";
            bool isSuccess = false;


            MotionControl.MotorAxis[] multiAxis = { MotorAxes[(int)eLift.GANTRYX_L], MotorAxes[(int)eLift.GANTRYX_R] };
            double[] dMultiPos = { 0.0, 0.0 };
            double[] dOffsetPos = { 0.0, 0.0 };

            dMultiPos[0] = Globalo.motionManager.liftMachine.teachingConfig.Teaching[(int)ePos].Pos[(int)eLift.GANTRYX_L];     //F x Axis
            dMultiPos[1] = Globalo.motionManager.liftMachine.teachingConfig.Teaching[(int)ePos].Pos[(int)eLift.GANTRYX_R];      //F x Axis

            Console.WriteLine($"[{ePos.ToString()} :{dMultiPos[0]}, :{dMultiPos[1]}]");

            isSuccess = MultiAxisMove(multiAxis, dMultiPos, bWait);

            if (isSuccess == false)
            {
                logStr = $"GANTRY X축 {ePos.ToString() } 이동 실패";

                Globalo.LogPrint("ManualControl", logStr);
            }

            return isSuccess;
        }
        #endregion
        public override void StopAuto()
        {
            if (processManager.liftFlow.CancelTokenLift != null && !processManager.liftFlow.CancelTokenLift.IsCancellationRequested)
            {
                processManager.liftFlow.CancelTokenLift.Cancel();
            }
           
            AutoUnitThread.Stop();
            MovingStop();
            RunState = OperationState.Stopped;
            Console.WriteLine($"[INFO] Lift Run Stop");

        }

        public override void MovingStop()
        {
            if (CancelToken != null && !CancelToken.IsCancellationRequested)
            {
                CancelToken.Cancel();
            }
            for (int i = 0; i < MotorAxes.Length; i++)
            {
                MotorAxes[i].MotorBreak = true;
                MotorAxes[i].Stop();
            }

        }
        public override bool OriginRun()
        {
            if (AutoUnitThread.GetThreadRun() == true)
            {
                return false;
            }
            string szLog = "";

            this.RunState = OperationState.OriginRunning;
            AutoUnitThread.m_nCurrentStep = 1000;          //ORG
            AutoUnitThread.m_nEndStep = 2000;

            AutoUnitThread.m_nStartStep = AutoUnitThread.m_nCurrentStep;

            bool rtn = AutoUnitThread.Start();
            if (rtn)
            {
                szLog = $"[ORIGIN] Lift Socket Origin Start";
                Console.WriteLine($"[ORIGIN] Lift Socket Origin Start");
                Globalo.LogPrint("MainForm", szLog);
            }
            else
            {
                this.RunState = OperationState.Stopped;
                Console.WriteLine($"[ORIGIN] Lift Socket Origin Start Fail");
                szLog = $"[ORIGIN] Lift Socket Origin Start Fail";
                Globalo.LogPrint("MainForm", szLog);
            }
            return rtn;
        }
        public override bool ReadyRun()
        {
            if (this.RunState != OperationState.Stopped)
            {
                Globalo.LogPrint("MainForm", "[LIFT] 설비 정지상태가 아닙니다.", Globalo.eMessageName.M_WARNING);
                return false;
            }
            if (AutoUnitThread.GetThreadRun() == true)
            {
                Globalo.LogPrint("MainForm", "[LIFT] 설비 정지상태가 아닙니다..", Globalo.eMessageName.M_WARNING);
                return false;
            }

            if (MotorAxes[(int)Machine.eLift.GANTRYX_L].OrgState == false || MotorAxes[(int)Machine.eLift.GANTRYX_R].OrgState == false ||
                MotorAxes[(int)Machine.eLift.LIFT_L_Z].OrgState == false|| MotorAxes[(int)Machine.eLift.LIFT_R_Z].OrgState == false)
            {
                this.RunState = OperationState.OriginRunning;
                AutoUnitThread.m_nCurrentStep = 1000;
            }
            else
            {
                this.RunState = OperationState.Preparing;
                AutoUnitThread.m_nCurrentStep = 2000;
            }

            AutoUnitThread.m_nEndStep = 3000;
            AutoUnitThread.m_nStartStep = AutoUnitThread.m_nCurrentStep;

            if (AutoUnitThread.GetThreadRun() == true)
            {
                Console.WriteLine($"모터 동작 중입니다.");
                return true;
            }
            bool rtn = AutoUnitThread.Start();
            if (rtn)
            {
                Console.WriteLine($"[READY] Lift Ready Start");
                Console.WriteLine($"모터 동작 성공.");
            }
            else
            {
                this.RunState = OperationState.Stopped;
                Console.WriteLine($"[READY] Lift Ready Start Fail");
                Console.WriteLine($"모터 동작 실패.");
            }

            return rtn;
        }
        public override void PauseAuto()
        {
            if (AutoUnitThread.GetThreadRun() == true)  // //TODO: 리프트상승중 일시정됐을때 모터 정지 or 계속 체크
            {
                //TODO: LIFT는 일시 정지하면 모터 정지시키기 ? 그럼 연이어 자동 안될듯
                processManager.liftFlow.pauseEvent.Reset();
                AutoUnitThread.Pause();
                RunState = OperationState.Paused;
            }
            return;
        }
        public override bool AutoRun()
        {
            bool rtn = true;
            if (this.RunState != OperationState.Paused)
            {
                if (this.RunState != OperationState.Standby)
                {
                    Globalo.LogPrint("MainForm", "[LIFT] 운전준비가 완료되지 않았습니다.", Globalo.eMessageName.M_WARNING);
                    return false;
                }
            }

            if (AutoUnitThread.GetThreadRun() == true)
            {
                Console.WriteLine($"모터 동작 중입니다.");

                if (AutoUnitThread.GetThreadPause() == true)        //일시 정지 상태인지 확인
                {
                    if (this.processManager.liftFlow.LoadTrayTask != null &&
                        this.processManager.liftFlow.LoadTrayTask.IsCompleted == false)
                    {
                        bool isSet = processManager.liftFlow.pauseEvent.IsSet;      //일시정지 체크
                        if (isSet)
                        {
                            //isSet= true진행중
                            //isSet= false 일시정지중
                            Console.WriteLine($"Task 자동 운전 중입니다. {isSet}");
                            return false;
                        }

                    }
                    AutoUnitThread.m_nCurrentStep = Math.Abs(AutoUnitThread.m_nCurrentStep);
                    AutoUnitThread.Resume();
                    processManager.liftFlow.pauseEvent.Set();
                    RunState = OperationState.AutoRunning;
                }
                return true;
            }
            else
            {
                //this.processManager.liftFlow.LoadTrayTask.Status == TaskStatus.Running)
                if (this.processManager.liftFlow.LoadTrayTask != null && this.processManager.liftFlow.LoadTrayTask.IsCompleted == false)
                {
                    //MessageBox.Show("motorTask 동작중입니다.");
                    return false;
                }

                AutoUnitThread.m_nCurrentStep = 3000;
                AutoUnitThread.m_nEndStep = 10000;
                AutoUnitThread.m_nStartStep = AutoUnitThread.m_nCurrentStep;

                rtn = AutoUnitThread.Start();

                if (rtn)
                {
                    RunState = OperationState.AutoRunning;
                    Console.WriteLine($"LIFT 모터 동작 성공.");
                }
                else
                {
                    Console.WriteLine($"LIFT 모터 동작 실패.");
                }
            }
            return rtn;
        }

    }
}
