using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.Machine
{

    public enum eAoiSocket : int
    {
        SOCKET_L_X = 0, SOCKET_L_Z, SOCKET_R_X, SOCKET_R_Z
    };
    public class AoiSocketMachine : MotionControl.MotorController
    {
        public int MotorCnt { get; private set; } = 4;

        //소켓 2개 2세트 = 4개
        
        public MotionControl.MotorAxis[] MotorAxes; // 배열 선언

        public string[] axisName = {"LEFT_X", "LEFT_Z", "RIGHT_X", "RIGHT_Z" };

        private MotorDefine.eMotorType[] motorType = { MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR};
        private AXT_MOTION_LEVEL_MODE[] AXT_SET_LIMIT = { AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW};
        private AXT_MOTION_LEVEL_MODE[] AXT_SET_SERVO_ALARM = { AXT_MOTION_LEVEL_MODE.HIGH, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW};
        private AXT_MOTION_HOME_DETECT[] MOTOR_HOME_SENSOR = { AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor};
        private AXT_MOTION_MOVE_DIR[] MOTOR_HOME_DIR = { AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW };

        private static double[] MaxSpeeds = { 200.0, 200.0, 200.0, 200.0};
        private double[] OrgFirstVel = { 20000.0, 20000.0, 20000.0, 20000.0 };
        private double[] OrgSecondVel = { 10000.0, 10000.0, 10000.0, 10000.0 };
        private double[] OrgThirdVel = { 5000.0, 5000.0, 5000.0, 5000.0};

        public enum eTeachingAoiPosList : int
        {
            WAIT_POS = 0, LOAD_POS, UN_LOAD_POS, CAPTURE_L_POS, CAPTURE_R_POS, HOUSING_IN_POS , HOUSING_OUT_POS, TOTAL_AOI_SOCKET_TEACHING_COUNT
        };
        public string[] TeachName =
        {
            "WAIT_POS", "LOAD_POS", "UN_LOAD_POS", "CAPTURE_L_POS", "CAPTURE_R_POS", "HOUSING_IN_POS", "HOUSING_OUT_POS"
        };

        public const string teachingPath = "Teach_AoiSocket.yaml";
        public const string taskPath = "Task_AoiSocket.yaml";
        public Data.TeachingConfig teachingConfig = new Data.TeachingConfig();


        public SocketProduct socketProduct = new SocketProduct();
        public AoiSocketMachine()
        {
            int i = 0;
            this.RunState = OperationState.Stopped;
            this.MachineName = this.GetType().Name;

            MotorAxes = new MotionControl.MotorAxis[MotorCnt];
            for (i = 0; i < MotorCnt; i++)
            {
                int index = (int)MotionControl.MotorSet.ValidAoiSocketMotors[i];
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

            socketProduct = Data.TaskDataYaml.TaskLoad_Socket(taskPath);
            if (socketProduct.SocketInfo_A.Count < 1)
            {
                socketProduct.SocketInfo_A.Add(new SocketProductInfo());
                socketProduct.SocketInfo_A.Add(new SocketProductInfo());
            }
            if (socketProduct.SocketInfo_B.Count < 1)
            {
                socketProduct.SocketInfo_B.Add(new SocketProductInfo());
                socketProduct.SocketInfo_B.Add(new SocketProductInfo());
            }

        }
        public override bool TaskSave()
        {
            bool rtn = Data.TaskDataYaml.TaskSave_Socket(socketProduct, taskPath);
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
        #region Aoi Socket Machine Io 동작
        public bool GetIsProductInSocket(int GroupNo, int index , bool bFlag , bool bWait = false)      //각 소켓의 제품 유무 확인 센서
        {
            //GroupNo = 좌,우 2Set
            return false;
        }

        public bool GetVacuumOn(int GroupNo, int index, bool bFlag, bool bWait = false)      //각 소켓의 흡착 유무 감지
        {
            //GroupNo = 좌,우 2Set
            return false;
        }
        #endregion

        #region Aoi Socekt Motor 동작
        public bool ChkMotorPos(eTeachingAoiPosList teachingPos, eAoiSocket Motor)
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            double dTeachingPos = 0.0;
            double currentPos = 0.0;

            dTeachingPos = this.teachingConfig.Teaching[(int)teachingPos].Pos[(int)Motor];
            currentPos = MotorAxes[(int)Motor].EncoderPos;

            if (dTeachingPos == currentPos)
            {
                return true;
            }

            return false;
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
        #endregion
        public override void StopAuto()
        {
            AutoUnitThread.Stop();
            MovingStop();
            RunState = OperationState.Stopped;
            Console.WriteLine($"[INFO] AoiSocket Run Stop");

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
                szLog = $"[ORIGIN] Aoi Socket Origin Start";
                Console.WriteLine($"[ORIGIN] Aoi Socket Origin Start");
                Globalo.LogPrint("MainForm", szLog);
            }
            else
            {
                this.RunState = OperationState.Stopped;
                Console.WriteLine($"[ORIGIN] Aoi Socket Origin Start Fail");
                szLog = $"[ORIGIN] Aoi Socket Origin Start Fail";
                Globalo.LogPrint("MainForm", szLog);
            }
            return rtn;
        }
        public override bool ReadyRun()
        {
            if (this.RunState != OperationState.Stopped)
            {
                Globalo.LogPrint("MainForm", "[AOI SOCKET] 설비 정지상태가 아닙니다.", Globalo.eMessageName.M_WARNING);
                return false;
            }
            if (AutoUnitThread.GetThreadRun() == true)
            {
                Globalo.LogPrint("MainForm", "[AOI SOCKET] 설비 정지상태가 아닙니다..", Globalo.eMessageName.M_WARNING);
                return false;
            }
            if (MotorAxes[(int)Machine.eAoiSocket.SOCKET_L_X].OrgState == false || MotorAxes[(int)Machine.eAoiSocket.SOCKET_L_Z].OrgState == false ||
                MotorAxes[(int)Machine.eAoiSocket.SOCKET_R_X].OrgState == false || MotorAxes[(int)Machine.eAoiSocket.SOCKET_R_Z].OrgState == false)
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
                Console.WriteLine($"[READY] Aoi Socket Ready Start");
                Console.WriteLine($"모터 동작 성공.");
            }
            else
            {
                this.RunState = OperationState.Stopped;
                Console.WriteLine($"[READY] Aoi Socket Ready Start Fail");
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
            if (this.RunState != OperationState.Paused)
            {
                if (this.RunState != OperationState.Standby)
                {
                    Globalo.LogPrint("MainForm", "[AOI SOCKET] 운전준비가 완료되지 않았습니다.", Globalo.eMessageName.M_WARNING);
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
                    Console.WriteLine($"AOI SOCKET 모터 동작 성공.");
                }
                else
                {
                    Console.WriteLine($"AOI SOCKET 모터 동작 실패.");
                }
            }
            return rtn;
        }
    }
    
}
