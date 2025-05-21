using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.Machine
{
    public enum eMagazine : int
    {
        MAGAZINE_L_Y = 0, MAGAZINE_L_Z, MAGAZINE_R_Y, MAGAZINE_R_Z
    };
    public class MagazineHandler : MotionControl.MotorController
    {
        public int MotorCnt { get; private set; } = 4;

        public MotionControl.MotorAxis[] MotorAxes; // 배열 선언

        public string[] axisName = { "LEFT_Y", "LEFT_Z", "RIGHT_Y", "RIGHT_Z" };
        //Y축 2개 Home 센서 없음 , -Limit으로 원점 진행

        private MotorDefine.eMotorType[] motorType = { MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR };
        private AXT_MOTION_LEVEL_MODE[] AXT_SET_LIMIT = { AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW };
        private AXT_MOTION_LEVEL_MODE[] AXT_SET_SERVO_ALARM = { AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW };
        private AXT_MOTION_HOME_DETECT[] MOTOR_HOME_SENSOR = { AXT_MOTION_HOME_DETECT.NegEndLimit, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.NegEndLimit, AXT_MOTION_HOME_DETECT.HomeSensor };
        private AXT_MOTION_MOVE_DIR[] MOTOR_HOME_DIR = { AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW };

        public static double[] MaxSpeeds = { 100.0, 100.0, 100.0, 100.0 };
        public double[] OrgFirstVel = { 5000.0, 5000.0, 5000.0, 5000.0 };
        public double[] OrgSecondVel = { 2500.0, 2500.0, 2500.0, 2500.0 };
        public double[] OrgThirdVel = { 500.0, 500.0, 500.0, 500.0 };

        public enum eTeachingPosList : int
        {
            WAIT_POS = 0, 
            LEFT_TRAY_LOAD_POS, LEFT_TRAY_UNLOAD_POS,
            LAYER1, LAYER2, LAYER3, LAYER4, LAYER5,
            TOTAL_MAGAZINE_TEACHING_COUNT
        };

        public string[] TeachName = { "WAIT_POS",
            "LEFT_TRAY_LOAD", "LEFT_TRAY_UNLOAD",
            "LAYER1","LAYER2","LAYER3","LAYER4","LAYER5"
        };

        //TRAY 꺼내는 층별 위치 다 따로 해야될수도
        public const string teachingPath = "Teach_Magazine.yaml";
        public const string taskPath = "Task_Magazine.yaml";
        public Data.TeachingConfig teachingConfig = new Data.TeachingConfig();
        //public LayerTray pickedProduct = new LayerTray();
        public MagazineTray magazineTray = new MagazineTray();

        
        public MagazineHandler()// : base("MagazineHandler")
        {
            int i = 0;
            this.RunState = OperationState.Stopped;
            this.MachineName = this.GetType().Name;

            MotorAxes = new MotionControl.MotorAxis[MotorCnt];
            for (i = 0; i < MotorCnt; i++)
            {
                int index = (int)MotionControl.MotorSet.ValidMagazineMotors[i];
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

            magazineTray = Data.TaskDataYaml.TaskLoad_Magazine(taskPath);
        }
        public override bool TaskSave()
        {
            bool rtn = Data.TaskDataYaml.TaskSave_Magazine(magazineTray, taskPath);
            return rtn;
        }
        #region Magazine Machine Io 동작
        public bool GetMagazineInPosition(int index, bool bFlag, bool bWait = false)       //Magazine 정위치 안착 확인 
        {
            return false;
        }
        public bool GetTrayUndocked(int index, bool bFlag, bool bWait = false)              //Magazine 과 Loader 사이 Tray 감지
        {
            return false;
        }
        public bool GetIsTrayOnLoader(int index, bool bFlag, bool bWait = false)              //Loader 에 Tray 유무 확인
        {
            return false;
        }
        public bool GetIsTrayFrontOfLoader(int index, bool bFlag, bool bWait = false)              //Loader 앞쪽에 Tray 감지 센서  - Magazine에서 Tray 빼기전 유무 확인
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
            Console.WriteLine($"[INFO] Magazine Run Stop");

        }
        public override void MotorDataSet()
        {
            int i = 0;
            if(teachingConfig.Speed.Count < 1)
            {
                Console.WriteLine("magazineHandler Speed Zero");
                return;
            }
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
        #region MAGAZINE Motor 동작
        public bool ChkYMotorPos(eTeachingPosList teachingPos, eMagazine MotorY)
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            double dYTeachingPos = 0.0;
            double currentYPos = 0.0;


            dYTeachingPos = this.teachingConfig.Teaching[(int)teachingPos].Pos[(int)MotorY];    //MAGAZINE_L_Y
            currentYPos = MotorAxes[(int)MotorY].EncoderPos;

            if (dYTeachingPos == currentYPos)
            {
                return true;
            }

            return false;
        }
        public bool ChkZMotorPos(eTeachingPosList teachingPos, eMagazine MotorZ)
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            double dZTeachingPos = 0.0;
            double currentZPos = 0.0;


            dZTeachingPos = this.teachingConfig.Teaching[(int)teachingPos].Pos[(int)MotorZ];
            currentZPos = MotorAxes[(int)MotorZ].EncoderPos;

            if (dZTeachingPos == currentZPos)
            {
                return true;
            }

            return false;
        }
        public bool Magazine_Y_Move(eTeachingPosList ePos, eMagazine MotorY, bool bWait = true)
        {
            if (this.MotorUse == false)
            {
                Console.WriteLine("No Use Machine");
                return true;
            }
            bool isSuccess = true;
            string logStr = "";

            double dPos = this.teachingConfig.Teaching[(int)ePos].Pos[(int)MotorY];     //z Axis
            try
            {
                isSuccess = MotorAxes[(int)MotorY].MoveAxis(dPos, AXT_MOTION_ABSREL.POS_ABS_MODE, bWait);
            }
            catch (Exception ex)
            {
                Globalo.LogPrint("ManualControl", $"Magazine_Y_Move Exception: {ex.Message}");
                isSuccess = false;
            }
            if (isSuccess == false)
            {
                logStr = $"Magazine Y axis {ePos.ToString() } 이동 실패";
            }

            return isSuccess;
        }
        public bool Magazine_Z_Move(eTeachingPosList ePos, eMagazine MotorZ, bool bWait = true)
        {
            if (this.MotorUse == false)
            {
                Console.WriteLine("No Use Machine");
                return true;
            }
            bool isSuccess = true;
            string logStr = "";
            double dPos = this.teachingConfig.Teaching[(int)ePos].Pos[(int)MotorZ];     //z Axis
            try
            {
                isSuccess = MotorAxes[(int)MotorZ].MoveAxis(dPos, AXT_MOTION_ABSREL.POS_ABS_MODE, bWait);
            }
            catch (Exception ex)
            {
                Globalo.LogPrint("ManualControl", $"Magazine_Z_Move Exception: {ex.Message}");
                isSuccess = false;
            }


            if (isSuccess == false)
            {
                logStr = $"Magazine Z axis {ePos.ToString() } 이동 실패";
            }

            return isSuccess;
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
        #endregion

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
                szLog = $"[ORIGIN] Magazine Socket Origin Start";
                Console.WriteLine($"[ORIGIN] Magazine Socket Origin Start");
                Globalo.LogPrint("MainForm", szLog);
            }
            else
            {
                this.RunState = OperationState.Stopped;
                Console.WriteLine($"[ORIGIN] Magazine Socket Origin Start Fail");
                szLog = $"[ORIGIN] Magazine Socket Origin Start Fail";
                Globalo.LogPrint("MainForm", szLog);
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
        public override bool ReadyRun()
        {
            if (this.RunState != OperationState.Stopped)
            {
                Globalo.LogPrint("MainForm", "[MAGAZINE] 설비 정지상태가 아닙니다.", Globalo.eMessageName.M_WARNING);
                return false;
            }
            if (AutoUnitThread.GetThreadRun() == true)
            {
                Globalo.LogPrint("MainForm", "[MAGAZINE] 설비 정지상태가 아닙니다..", Globalo.eMessageName.M_WARNING);
                return false;
            }
            if (MotorAxes[(int)Machine.eMagazine.MAGAZINE_L_Y].OrgState == false || MotorAxes[(int)Machine.eMagazine.MAGAZINE_L_Z].OrgState == false ||
                MotorAxes[(int)Machine.eMagazine.MAGAZINE_R_Y].OrgState == false || MotorAxes[(int)Machine.eMagazine.MAGAZINE_R_Z].OrgState == false)
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
                Console.WriteLine($"[READY] Magazine Ready Start");
                Console.WriteLine($"모터 동작 성공.");
            }
            else
            {
                this.RunState = OperationState.Stopped;
                Console.WriteLine($"[READY] Magazine Ready Start Fail");
                Console.WriteLine($"모터 동작 실패.");
            }

            return rtn;
        }
        public override bool AutoRun()
        {
            bool rtn = true;
            if (this.RunState != OperationState.Paused)
            {
                if (this.RunState != OperationState.Standby)
                {
                    Globalo.LogPrint("MainForm", "[MAGAZINE] 운전준비가 완료되지 않았습니다.", Globalo.eMessageName.M_WARNING);
                    return false;
                }
            }

            if (AutoUnitThread.GetThreadRun() == true)
            {
                Console.WriteLine($"모터 동작 중입니다.");

                if (AutoUnitThread.GetThreadPause() == true)        //일시 정지 상태인지 확인
                {
                    AutoUnitThread.m_nCurrentStep = Math.Abs(AutoUnitThread.m_nCurrentStep);
                    AutoUnitThread.Resume();
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
                    Console.WriteLine($"MAGAZINE 모터 동작 성공.");
                }
                else
                {
                    Console.WriteLine($"MAGAZINE 모터 동작 실패.");
                }
            }
            return rtn;
        }


    }
}
