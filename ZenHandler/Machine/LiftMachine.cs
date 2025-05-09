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
        LIFT_L_Z = 0, LIFT_R_Z, LIFT_F_X, LIFT_B_X
    };
    public class LiftMachine : MotionControl.MotorController
    {
        public int MotorCnt { get; private set; } = 4;

        //LEFT Z / RIGHT Z / GANTRY FRONT X / GANTRY BACK X / 
        public MotionControl.MotorAxis[] MotorAxes; // 배열 선언

        public string[] axisName = { "FRONT_X", "BACK_X", "LEFT_Z", "RIGHT_Z"};

        private MotorDefine.eMotorType[] motorType = { MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR };
        private AXT_MOTION_LEVEL_MODE[] AXT_SET_LIMIT = { AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW };
        private AXT_MOTION_LEVEL_MODE[] AXT_SET_SERVO_ALARM = { AXT_MOTION_LEVEL_MODE.HIGH, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW };
        private AXT_MOTION_HOME_DETECT[] MOTOR_HOME_SENSOR = { AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor };
        private AXT_MOTION_MOVE_DIR[] MOTOR_HOME_DIR = { AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW };


        private static double[] MaxSpeeds = { 100.0, 100.0, 200.0, 200.0};
        private double[] OrgFirstVel = { 20000.0, 20000.0, 20000.0, 20000.0};
        private double[] OrgSecondVel = { 5000.0, 5000.0, 10000.0, 10000.0};
        private double[] OrgThirdVel = { 2500.0, 2500.0, 5000.0, 5000.0};


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

            //MotorAxes = new MotionControl.MotorAxis[] { Gantry_X_F, Gantry_X_B, LoadLift_Z_L, LoadLift_Z_R};
            //MotorCnt = MotorAxes.Length;
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
        public bool GetTraySlidePos(int index)                  //슬라이드 정위치 확인
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
        public bool GetIsTrayOnTop(int index)              //LEFT , RIGHT 상단 TRAY 유무 확인
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
        
        public override void StopAuto()
        {
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
            if (AutoUnitThread.GetThreadRun() == true)
            {
                return false;
            }

            if (MotorAxes[(int)Machine.eLift.LIFT_B_X].OrgState == false || MotorAxes[(int)Machine.eLift.LIFT_F_X].OrgState == false ||
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
            if (AutoUnitThread.GetThreadRun() == true)
            {
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
                if (this.RunState != OperationState.PreparationComplete)
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
                    AutoUnitThread.m_nCurrentStep = Math.Abs(AutoUnitThread.m_nCurrentStep);

                    RunState = OperationState.AutoRunning;
                }
                else
                {
                    rtn = false;
                }
            }
            else
            {
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
