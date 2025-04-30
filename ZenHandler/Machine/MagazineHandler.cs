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

        public MotionControl.MotorAxis MagazineY_L;
        public MotionControl.MotorAxis MagazineZ_L;
        public MotionControl.MotorAxis MagazineY_R;
        public MotionControl.MotorAxis MagazineZ_R;

        public MotionControl.MotorAxis[] MotorAxes; // 배열 선언

        public string[] axisName = { "MagazineY_L", "MagazineZ_L", "MagazineY_R", "MagazineZ_R" };

        private MotorDefine.eMotorType[] motorType = { MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR };
        private AXT_MOTION_LEVEL_MODE[] AXT_SET_LIMIT = { AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW };
        private AXT_MOTION_LEVEL_MODE[] AXT_SET_SERVO_ALARM = { AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW };
        private AXT_MOTION_HOME_DETECT[] MOTOR_HOME_SENSOR = { AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor };
        private AXT_MOTION_MOVE_DIR[] MOTOR_HOME_DIR = { AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW };

        public static double[] MaxSpeeds = { 100.0, 100.0, 100.0, 100.0 };
        public double[] OrgFirstVel = { 5000.0, 5000.0, 5000.0, 5000.0 };
        public double[] OrgSecondVel = { 2500.0, 2500.0, 2500.0, 2500.0 };
        public double[] OrgThirdVel = { 500.0, 500.0, 500.0, 500.0 };

        public enum eTeachingPosList : int
        {
            WAIT_POS = 0, 
            LEFT_TRAY_LOAD_POS, LEFT_TRAY_UNLOAD_POS,
            STACK1_L, STACK2_L, STACK3_L, STACK4_L, STACK5_L,
            STACK1_R, STACK2_R, STACK3_R, STACK4_R, STACK5_R,
            TOTAL_MAGAZINE_TEACHING_COUNT
        };

        public string[] TeachName = { "WAIT_POS",
            "LEFT_TRAY_LOAD", "LEFT_TRAY_UNLOAD",
            "STACK1_L","STACK2_L","STACK3_L","STACK4_L","STACK5_L",
            "STACK1_R","STACK2_R","STACK3_R","STACK4_R","STACK5_R",
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

            MotorAxes = new MotionControl.MotorAxis[] { MagazineY_L, MagazineZ_L, MagazineY_R, MagazineZ_R };
            MotorCnt = MotorAxes.Length;

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
            for (i = 0; i < MotorAxes.Length; i++)
            {
                MotorAxes[i].setMotorParameter(teachingConfig.Speed[i], teachingConfig.Accel[i], teachingConfig.Decel[i], teachingConfig.Resolution[i]);
            }
            for (i = 0; i < teachingConfig.Teaching.Count; i++)
            {
                teachingConfig.Teaching[i].Name = TeachName[i];
            }

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

            return false;
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
            if (AutoUnitThread.GetThreadRun() == true)
            {
                return false;
            }
            if (MagazineY_L.OrgState == false || MagazineZ_L.OrgState == false || MagazineY_R.OrgState == false || MagazineZ_R.OrgState == false)
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
                Console.WriteLine($"[READY] Transfer Ready Start");
                Console.WriteLine($"모터 동작 성공.");
            }
            else
            {
                this.RunState = OperationState.Stopped;
                Console.WriteLine($"[READY] Transfer Ready Start Fail");
                Console.WriteLine($"모터 동작 실패.");
            }

            return rtn;
        }
        public override bool AutoRun()
        {
            bool rtn = true;
            if (this.RunState != OperationState.PreparationComplete)
            {
                Globalo.LogPrint("MainForm", "[MAGAZINE] 운전준비가 완료되지 않았습니다.", Globalo.eMessageName.M_WARNING);
                return false;
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
