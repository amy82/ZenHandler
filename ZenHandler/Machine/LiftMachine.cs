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

        public bool IsLoadingTrayOnGangry = false;         //투입 LIft 투입중 On Gantry
        public bool IsMovingTrayOnPusher = false;         //Pusher로 Tray 이동중

        public bool IsUnloadingOutputTray = false;      //배출 Lift 배출중

        public enum eTeachingPosList : int
        {
            WAIT_POS = 0, LEFT_LOAD_POS, RIGHT_LOAD_POS, TRAY_LOAD_LEFT_POS, TRAY_LOAD_RIGHT_POS, TOTAL_LIFT_TEACHING_COUNT   //UNLOAD_POS
        };
        //아래 두개 나눠야 될수도
        //GANTRY 가 TRAY 받는 위치
        //GANTRY 위에서 제품 로드 하는 위치
        //Item , part , Unit , Goods , Piece , Obj
        public string[] TeachName = { "WAIT_POS" , "L_LOAD_POS", "R_LOAD_POS", "TRAY_LOAD_LEFT", "TRAY_LOAD_RIGHT" };


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


            // 갠트리 구동을 설정한다.
            int m_lAxisNoMaster = (int)eLift.GANTRYX_L;
            int m_lAxisNoSlave = (int)eLift.GANTRYX_R;       // Gantry Master/Slave 축 번호 선언 초기화
            uint duSlaveHmUse = 0;      // TRUE : 마스터 홈센서를 찾은 후 슬레이브 홈센서도 찾음
            double dSlaveHmOffset = 0;       // 마스터와 슬레이브 홈센서들간의 Offset
            double dSlaveHmRange = 10;       // 원점 검색시 마스터 홈센서와 슬레이브 홈센서간의 오차 한계

            uint duRetCode;

            //
            //이 함수를 이용해 Master축을 갠트리 제어로 설정하면 해당 Slave축은 Master축과 동기 시킨다.
            //갠트리 제어 기능을 활성화 시키고 이후 Slave축에 구동명령이나 정지 명령등을 내려도 모두 무시된다.
            //주의사항: 갠트리 ENABLE시 슬레이브축은 모션중 AxmStatusReadMotion 함수로 확인하면 InMotion 중으로 True로 확인되어야 한다.

            //만약 InMotion 이 False되면 Gantry Enable이 되지 않았으므로 알람발생 여부 혹은 Limit 신호 발생 여부를 확인한다.


            duRetCode = CAXM.AxmGantrySetEnable(m_lAxisNoMaster, m_lAxisNoSlave, duSlaveHmUse, dSlaveHmOffset, dSlaveHmRange);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                Console.WriteLine("AxmGantrySetEnable Fail");
                //chkGantryEnable.Checked = false;
                //MessageBox.Show(String.Format("AxmGantrySetEnable return error[Code:{0:d}]", duRetCode));
                //AxmGantrySetEnable 함수를 이용해 마스터, 슬레이브 축을 갠트리 시스템으로 설정하면 (((슬레이브))) 축은 모션중으로 설정 된다.
                //AxmStatusReadInMotion 함수의 Status값은 반드시 1로 읽혀야 된다. 1이 아니면 갠트리 설정이 안 된 것이다.

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
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            int lModuleNo = 0;
            int lOffset = 0;

            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];
            if (bFlag)
            {
                uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH0.IN_LENS_GRIP_FOR;
                if (uFlagHigh == 1)
                {
                    return true;
                }
            }
            else
            {
                uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH0.IN_LENS_GRIP_BACK;
                if (uFlagHigh == 1)
                {
                    return true;
                }
            }
            return false;
        }
        public bool GantryCenteringFor(bool bFlag, bool bWait = false)        //GANTRY 모서리 센터링 전후진
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            int lModuleNo = 0;
            int lOffset = 0;

            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];
            if (bFlag)
            {
                uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH0.IN_LENS_GRIP_FOR;
                if (uFlagHigh == 1)
                {
                    return true;
                }
            }
            else
            {
                uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH0.IN_LENS_GRIP_BACK;
                if (uFlagHigh == 1)
                {
                    return true;
                }
            }
            return false;
        }
        public bool GetGantryClampFor(bool bFlag, bool bWait = false)
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            int lModuleNo = 0;
            int lOffset = 0;

            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];
            if (bFlag)
            {
                uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH0.IN_LENS_GRIP_FOR;
                if (uFlagHigh == 1)
                {
                    return true;
                }
            }
            else
            {
                uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH0.IN_LENS_GRIP_BACK;
                if (uFlagHigh == 1)
                {
                    return true;
                }
            }
            return false;
        }

        public bool GantryClampFor(bool bFlag, bool bWait = false)        //GANTRY 좌우 전후진 클램프
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            int lModuleNo = 0;
            int lOffset = 0;

            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];
            if (bFlag)
            {
                uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH0.IN_LENS_GRIP_FOR;
                if (uFlagHigh == 1)
                {
                    return true;
                }
            }
            else
            {
                uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH0.IN_LENS_GRIP_BACK;
                if (uFlagHigh == 1)
                {
                    return true;
                }
            }
            return false;
        }
        public bool GetPUsherUp(bool bFlag, bool bWait = false)
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            int lModuleNo = 0;
            int lOffset = 0;

            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];
            if (bFlag)
            {
                uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH0.IN_LENS_GRIP_FOR;
                if (uFlagHigh == 1)
                {
                    return true;
                }
            }
            else
            {
                uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH0.IN_LENS_GRIP_BACK;
                if (uFlagHigh == 1)
                {
                    return true;
                }
            }
            return false;
        }

        public bool PusherUp(bool bFlag, bool bWait = false)        //푸셔 상승 , 하강
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            int lModuleNo = 1;
            int lOffset = 0;
            uint uFlagHigh = 0;
            uint uFlagLow = 0;

            if (bFlag)
            {
                uFlagHigh = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH0.VACUUM_ON;
                uFlagLow = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH0.VACUUM_OFF;
            }
            else
            {
                uFlagHigh = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH0.VACUUM_OFF;
                uFlagLow = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH0.VACUUM_ON;
            }

            bool Rtn = Globalo.motionManager.ioController.DioWriteOutportByte(lModuleNo, lOffset, uFlagHigh, uFlagLow);
            if (Rtn == false)
            {
                //LENS GRIP 동작 
                return false;
            }
            bool isSuccess = false;

            if (bWait == false)
            {
                return true;
            }
            else
            {
                if (bWait == false)
                {
                    return false;
                }
                else
                {
                    int nTimeTick = 0;
                    while (bWait)
                    {
                        Rtn = GetPUsherUp(bFlag);
                        if (Rtn == true)
                        {
                            isSuccess = true;
                            break;
                        }

                        nTimeTick = Environment.TickCount;
                        if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                        {
                            isSuccess = false;
                            break;
                        }

                        Thread.Sleep(10);
                    }
                }
            }
            return isSuccess;
        }
        public bool GetPUsherFor(bool bFlag, bool bWait = false)
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            int lModuleNo = 0;
            int lOffset = 0;

            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];
            if (bFlag)
            {
                uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH0.IN_LENS_GRIP_FOR;
                if (uFlagHigh == 1)
                {
                    return true;
                }
            }
            else
            {
                uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH0.IN_LENS_GRIP_BACK;
                if (uFlagHigh == 1)
                {
                    return true;
                }
            }
            return false;
        }
        public bool PusherFor(bool bFlag, bool bWait = false)       //푸셔 전진 , 후진
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            int lModuleNo = 1;
            int lOffset = 0;
            uint uFlagHigh = 0;
            uint uFlagLow = 0;

            if (bFlag)
            {
                uFlagHigh = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH0.VACUUM_ON;
                uFlagLow = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH0.VACUUM_OFF;
            }
            else
            {
                uFlagHigh = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH0.VACUUM_OFF;
                uFlagLow = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH0.VACUUM_ON;
            }

            bool Rtn = Globalo.motionManager.ioController.DioWriteOutportByte(lModuleNo, lOffset, uFlagHigh, uFlagLow);
            if (Rtn == false)
            {
                //LENS GRIP 동작 
                return false;
            }
            bool isSuccess = false;

            if (bWait == false)
            {
                return true;
            }
            else
            {
                if (bWait == false)
                {
                    return false;
                }
                else
                {
                    int nTimeTick = 0;
                    while (bWait)
                    {
                        Rtn = GetPUsherFor(bFlag);
                        if (Rtn == true)
                        {
                            isSuccess = true;
                            break;
                        }

                        nTimeTick = Environment.TickCount;
                        if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                        {
                            isSuccess = false;
                            break;
                        }

                        Thread.Sleep(10);
                    }
                }
            }
            return isSuccess;
        }
        public bool PusherCentringFor(bool bFlag, bool bWait = false)       //푸셔 센터링 전진 , 후진
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            int lModuleNo = 1;
            int lOffset = 0;
            uint uFlagHigh = 0;
            uint uFlagLow = 0;

            if (bFlag)
            {
                uFlagHigh = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH0.VACUUM_ON;
                uFlagLow = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH0.VACUUM_OFF;
            }
            else
            {
                uFlagHigh = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH0.VACUUM_OFF;
                uFlagLow = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH0.VACUUM_ON;
            }

            bool Rtn = Globalo.motionManager.ioController.DioWriteOutportByte(lModuleNo, lOffset, uFlagHigh, uFlagLow);
            if (Rtn == false)
            {
                //LENS GRIP 동작 
                return false;
            }
            bool isSuccess = false;

            if (bWait == false)
            {
                return true;
            }
            else
            {
                if (bWait == false)
                {
                    return false;
                }
                else
                {
                    int nTimeTick = 0;
                    while (bWait)
                    {
                        Rtn = GetPusherCentringFor(bFlag);
                        if (Rtn == true)
                        {
                            isSuccess = true;
                            break;
                        }

                        nTimeTick = Environment.TickCount;
                        if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.IO_TIMEOUT)
                        {
                            isSuccess = false;
                            break;
                        }

                        Thread.Sleep(10);
                    }
                }
            }
            return isSuccess;
        }
        public bool GetPusherCentringFor(bool bFlag, bool bWait = false) //푸셔 센터링 전진 , 후진 센서 확인
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            int lModuleNo = 0;
            int lOffset = 0;

            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];
            if (bFlag)
            {
                uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH0.IN_LENS_GRIP_FOR;
                if (uFlagHigh == 1)
                {
                    return true;
                }
            }
            else
            {
                uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH0.IN_LENS_GRIP_BACK;
                if (uFlagHigh == 1)
                {
                    return true;
                }
            }
            return false;
        }
        public bool GetTraySlidePos(int index)          //슬라이드 정위치 확인
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }

            int lModuleNo = 0;
            int lOffset = 0;

            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];

            if (index == 0)
            {
                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_LOAD_PICKER_UP1);
            }
            else
            {
                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_LOAD_PICKER_UP2);
            }

            uFlagHigh = upValue & uFlagHigh;        //TODO: IO 되는지 확인 필요
            if (uFlagHigh == 1)
            {
                return true;
            }

            return false;
        }
        public bool GetTopTouchSensor(int index)        //TRAY 교체시 리프트 정지 센서
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }

            int lModuleNo = 0;
            int lOffset = 0;

            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];

            if (index == 0)
            {
                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_LOAD_PICKER_UP1);
            }
            else
            {
                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_LOAD_PICKER_UP2);
            }

            uFlagHigh = upValue & uFlagHigh;        //TODO: IO 되는지 확인 필요
            if (uFlagHigh == 1)
            {
                return true;
            }
            return false;
        }
        public bool GetMiddleWaitSensor(int index)        //리프트 대기 위치 확인 센서
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }

            int lModuleNo = 0;
            int lOffset = 0;

            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];

            if (index == 0)
            {
                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_LOAD_PICKER_UP1);
            }
            else
            {
                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_LOAD_PICKER_UP2);
            }

            uFlagHigh = upValue & uFlagHigh;        //TODO: IO 되는지 확인 필요
            if (uFlagHigh == 1)
            {
                return true;
            }
            return false;
        }
        public bool GetIsLoadTrayOnTop(int index)            //GANTRY, PUSHER 위 TRAY 유무 확인
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }

            int lModuleNo = 0;
            int lOffset = 0;

            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];

            if (index == 0)
            {
                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_LOAD_PICKER_UP1);
            }
            else
            {
                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_LOAD_PICKER_UP2);
            }

            uFlagHigh = upValue & uFlagHigh;        //TODO: IO 되는지 확인 필요
            if (uFlagHigh == 1)
            {
                return true;
            }
            return false;
        }
        public bool GetIsTrayOnSlide(int index)              //LEFT , RIGHT SLIDE 위 TRAY 유무 확인
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }

            int lModuleNo = 0;
            int lOffset = 0;

            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];

            if (index == 0)
            {
                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_LOAD_PICKER_UP1);
            }
            else
            {
                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_LOAD_PICKER_UP2);
            }

            uFlagHigh = upValue & uFlagHigh;        //TODO: IO 되는지 확인 필요
            if (uFlagHigh == 1)
            {
                return true;
            }
            return false;
        }
        public bool ChkButtonLoadTray(bool bFlag)     //작업자 : Tary투입후 작업자 Gantry로 Tray 투입 요청
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            int lModuleNo = 0;
            int lOffset = 0;

            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];
            if (bFlag)
            {
                uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH0.IN_VACUUM_ON;
                if (uFlagHigh == 1)
                {
                    return true;
                }
            }
            else
            {
                uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH0.IN_VACUUM_ON;
                if (uFlagHigh == 0)
                {
                    return true;
                }
            }
            return false;
        }
        public bool ChkButtonUnloadTray(bool bFlag)    //작업자 : 배출 Lift 다운 요청
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            int lModuleNo = 0;
            int lOffset = 0;

            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];
            if (bFlag)
            {
                uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH0.IN_VACUUM_ON;
                if (uFlagHigh == 1)
                {
                    return true;
                }
            }
            else
            {
                uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH0.IN_VACUUM_ON;
                if (uFlagHigh == 0)
                {
                    return true;
                }
            }
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
        public bool LIft_Z_Height_Move(eLift motorAxis, double dHeight, bool bWait = true)      //TODO: 확인필요
        {
            if (this.MotorUse == false)
            {
                Console.WriteLine("No Use Machine");
                return true;
            }

            bool isSuccess = true;
            string logStr = "";
            double dPos = dHeight;

            

            try
            {
                isSuccess = this.MotorAxes[(int)motorAxis].MoveAxis(dPos, AXT_MOTION_ABSREL.POS_REL_MODE, bWait);
            }
            catch (Exception ex)
            {
                Globalo.LogPrint("ManualControl", $"{motorAxis.ToString()} Rel Move Exception: {ex.Message}");
                isSuccess = false;
            }


            if (isSuccess == false)
            {
                logStr = $"{motorAxis.ToString() } Rel Move 이동 실패";
            }


            return isSuccess;
        }
        public bool LIft_Z_Move_SersonDetected(eLift motorAxis, eLiftSensor Sensor, bool bWait = true)
        {
            if (this.MotorUse == false)
            {
                Console.WriteLine("No Use Machine");
                return true;
            }

            //string logStr = "";
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
            //int SkipChk = 0;
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
                    this.processManager.liftFlow.nTimeTick = Environment.TickCount;
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
