using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.Machine
{
    public enum eEEpromSocket : int
    {
        SOCKET_F_X = 0, SOCKET_B_X
    };
    public class EEpromSocketMachine : MotionControl.MotorController
    {
        public int MotorCnt { get; private set; } = 2;

        //소켓4개 2세트 = 총 8개

        public MotionControl.MotorAxis Front_X;    //eeprom 공정 TOTAL : 2
        public MotionControl.MotorAxis Back_X;    //eeprom 공정

        public MotionControl.MotorAxis[] MotorAxes; // 배열 선언

        public string[] axisName = { "Front_X", "Back_X"};

        private MotorDefine.eMotorType[] motorType = { MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR };
        private AXT_MOTION_LEVEL_MODE[] AXT_SET_LIMIT = { AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW };
        private AXT_MOTION_LEVEL_MODE[] AXT_SET_SERVO_ALARM = { AXT_MOTION_LEVEL_MODE.HIGH, AXT_MOTION_LEVEL_MODE.LOW };
        private AXT_MOTION_HOME_DETECT[] MOTOR_HOME_SENSOR = { AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor};
        private AXT_MOTION_MOVE_DIR[] MOTOR_HOME_DIR = { AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW};

        private static double[] MaxSpeeds = { 200.0, 200.0 };
        private double[] OrgFirstVel = { 20000.0, 20000.0};
        private double[] OrgSecondVel = { 10000.0, 10000.0 };
        private double[] OrgThirdVel = { 5000.0, 5000.0 };

        public enum eTeachingPosList : int
        {
            WAIT_POS = 0, LOAD_POS, UN_LOAD_POS, WRITE_POS, VERIFY_POS, TOTAL_SOCKET_TEACHING_COUNT
        };
        public string[] TeachName =
        {
            "WAIT_POS", "LOAD_POS", "UN_LOAD_POS", "WRITE_POS", "VERIFY_POS"
        };

        public const string teachingPath = "Teach_EEpromSocket.yaml";
        public const string taskPath = "Task_EEpromSocket.yaml";
        public Data.TeachingConfig teachingConfig = new Data.TeachingConfig();


        //public SocketProduct socketProduct = new SocketProduct();
        public EEpromSocketMachine()
        {
            int i = 0;
            this.RunState = OperationState.Stopped;
            this.MachineName = this.GetType().Name;

            MotorAxes = new MotionControl.MotorAxis[] { Front_X, Back_X };
            MotorCnt = MotorAxes.Length;

            for (i = 0; i < MotorCnt; i++)
            {
                int index = (int)MotionControl.MotorSet.ValidEEpromSocketMotors[i];
                MotorAxes[i] = new MotionControl.MotorAxis(index,
                axisName[i], motorType[i], MaxSpeeds[i], AXT_SET_LIMIT[i], AXT_SET_SERVO_ALARM[i], OrgFirstVel[i], OrgSecondVel[i], OrgThirdVel[i],
                MOTOR_HOME_SENSOR[i], MOTOR_HOME_DIR[i]);


                //초기 셋 다른 곳에서 다시 해줘야될 듯
                MotorAxes[i].setMotorParameter(10.0, 0.1, 0.1, 1000.0);
                if (this.MotorUse == false)
                {
                    MotorAxes[i].NoUse = true;
                }
            }


            //socketProduct = Data.TaskDataYaml.TaskLoad_Socket(taskPath);

        }
        public override bool TaskSave()
        {
            //bool rtn = Data.TaskDataYaml.TaskSave_Transfer(socketProduct, taskPath);
            //return rtn;
            return false;
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
            return false;
        }
        public override void StopAuto()
        {
            AutoUnitThread.Stop();
            MovingStop();
            RunState = OperationState.Stopped;
            Console.WriteLine($"[INFO] EEpromSocket Run Stop");

        }
        public override bool OriginRun()
        {
            if (AutoUnitThread.GetThreadRun() == true)
            {
                //motorAutoThread.Stop();
                return false;
            }
            return true;
        }
        public override bool ReadyRun()
        {
            if (AutoUnitThread.GetThreadRun() == true)
            {
                return false;
            }
            if (Front_X.OrgState == false || Back_X.OrgState == false)
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
        public override void PauseAuto()
        {
            if (AutoUnitThread.GetThreadRun() == true)
            {
                AutoUnitThread.Pause();
                RunState = OperationState.Paused;
            }
        }
        public override bool AutoRun()
        {
            bool rtn = true;
            if (this.RunState != OperationState.PreparationComplete)
            {
                Globalo.LogPrint("MainForm", "[EEPROM SOCKET] 운전준비가 완료되지 않았습니다.", Globalo.eMessageName.M_WARNING);
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
                    Console.WriteLine($"EEPROM SOCKET 모터 동작 성공.");
                }
                else
                {
                    Console.WriteLine($"EEPROM SOCKET 모터 동작 실패.");
                }
            }
            return rtn;
        }
    }
    
}
