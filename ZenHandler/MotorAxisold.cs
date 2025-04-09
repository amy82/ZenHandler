using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ZenHandler
{
    public class MotorAxisold
    {
        /// <summary>
        /// 모터 이름
        /// </summary>
        private readonly string name;
        public string Name { get { return name; } }

        /// <summary>
        /// 모터 Number
        /// </summary>
        private readonly int axisNo;
        public int AxisNo { get { return axisNo; } }

        /// <summary>
        /// 분해능
        /// </summary>
        private readonly double resolution;
        public double Resolution { get { return resolution; } }

        /// <summary>
        /// 모터 타입 - LINEAR , STEPING
        /// </summary>
        private readonly MotorDefine.eMotorType type;
        public MotorDefine.eMotorType Type { get { return type; } }

        /// <summary>
        /// 최고 속도
        /// </summary>
        private readonly int maxSpeed;
        public int MaxSpeed { get { return maxSpeed; } }

        /// <summary>
        /// 원점 방향 (DIR_CW (+) , DIR_CCW(-))
        /// </summary>
        private readonly AXT_MOTION_MOVE_DIR homeDir;
        public AXT_MOTION_MOVE_DIR HomeDir { get { return homeDir; } }

        /// <summary>
        /// 원점 센서 (Home, Minus , Plus)
        /// </summary>
        private readonly AXT_MOTION_HOME_DETECT homeSignal;
        public AXT_MOTION_HOME_DETECT HomeSignal { 
            get { 
                return homeSignal; 
            } 
        }
        /// <summary>
        /// 속도
        /// </summary>
        public double dVelocity { get; private set; }
        /// <summary>
        /// 가속도
        /// </summary>
        public double dAccel { get; private set; }
        /// <summary>
        /// 감속도
        /// </summary>
        public double dDecel { get; private set; }
        /// <summary>
        /// 모터 동작 상태 (런 , 정지)
        /// </summary>
        public bool bState { get; private set; }
        /// <summary>
        /// 원점 상태
        /// </summary>
        public bool bOrgState { get; set; }
        /// 
        public MotorAxisold(int axisNo, string name, int maxSpeed, double resolution, MotorDefine.eMotorType type, AXT_MOTION_MOVE_DIR homeDir, AXT_MOTION_HOME_DETECT homeSignal)
        {
            this.axisNo = axisNo;
            this.name = name;
            this.maxSpeed = maxSpeed;
            this.resolution = resolution;
            this.type = type;
            this.homeDir = homeDir;
            this.homeSignal = homeSignal;

            this.dVelocity = 10.0;
            this.dAccel = 0.25;
            this.dDecel = 0.3;
        }
        /// <summary>
        /// 모터 현재 위치
        /// </summary>
        public double dPos {
            get
            {
                double dCmdPos = 0.0;
                CAXM.AxmStatusGetCmdPos(axisNo, ref dCmdPos);
                return dCmdPos;//
            }
        }
        

        public bool OrgCatch()
        {
            //++ 지정한 축의 지령(Command)위치를 반환합니다.
            //CAXM.AxmStatusGetCmdPos(m_lMoveMultiAxes[lArrayIndex], ref dCmdPos);
            //if (m_dOldCmdPos[lCount] != dCmdPos)
            //{
                //DisplayCmdPos[lCount].Text = String.Format("{0:0.000}", dCmdPos);
                //m_dOldCmdPos[lCount] = dCmdPos;
            //}
            return true;
        }

        public bool JogMove(int direction , double speedMode)
        {
            uint dwRetVal = 0;
            //MOTOR_JOG_LOW = 0.1
            //MOTOR_JOG_MID = 0.5
            //MOTOR_JOG_HIGH = 1.0
            if (this.GetAmpFault() == true)
            {
                return false;
            }
            double dJogSpeed = (dVelocity * resolution) * speedMode;

            dwRetVal = CAXM.AxmMoveVel(this.axisNo, (dJogSpeed * direction), dAccel, dDecel);
            if (dwRetVal != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                return false;
            }

            return true;
        }
        public bool JogStop()
        {
            CAXM.AxmMoveSStop(this.axisNo);

            return true;
        }

        public double GetEncoderPos()
        {
            double dPos = 0.0;

            if (this.type == MotorDefine.eMotorType.LINEAR)
            {
                //리니어,서보
                CAXM.AxmStatusGetActPos(this.axisNo, ref dPos);
            }
            else
            {
                //스테핑
                CAXM.AxmStatusGetCmdPos(this.axisNo, ref dPos);
            }

            dPos /= resolution;
            return dPos;
        }
        public bool GetHomeState(int nUnit)
        {
            return bOrgState;
        }
        
        public bool GetServoOn()
        {
            uint dwLevel = 0;
            CAXM.AxmSignalIsServoOn(this.axisNo, ref dwLevel);
            if (dwLevel == (uint)AXT_USE.ENABLE)
            {
                return true;
            }
            return false;
        }
        public bool MoveFromAbsRel(double dPos, bool bPlus)
        {
            double dCurrPos = 0.0;
            double dSpeed = 50.0;

            if (bPlus == false)
            {
                dPos *= -1.0;
            }
            dCurrPos = this.GetEncoderPos();

            this.MoveAxis(AXT_MOTION_ABSREL.POS_REL_MODE, dPos, dSpeed, false);
            return true;
        }
        //-----------------------------------------------------------------------------
        //
        //	지정 축을 절대 구동 또는 상대 구동으로 이동한다. 
        //
        //-----------------------------------------------------------------------------
        public bool MoveAxis(AXT_MOTION_ABSREL nAbsFlag, double dPos, double dVel, bool bWait)
        {
            double dCurrPos = 0.0;
            double dAcc = 0.0;
            double dDec = 0.0;

            if (this.GetAmpFault() == true)
            {
                return false;
            }
            if (this.GetAmpEnable() == false)
            {
                return false;
            }

            

            if (nAbsFlag == AXT_MOTION_ABSREL.POS_ABS_MODE)
            {
                if (bOrgState == false)
                {
                    return false;
                }
                dCurrPos = this.GetEncoderPos();
                if (Math.Abs(dCurrPos - dPos) < 0.0001)
                {
                    return true;
                }

            }
            else if (nAbsFlag == AXT_MOTION_ABSREL.POS_REL_MODE)
            {
                dPos += this.GetEncoderPos();
            }
            else
            {
                //모터 이동 명령 이상
                return false;
            }
            
            dPos *= resolution;
            if (dPos > 0)
            {
                dPos = (int)(dPos + 0.5);
            }
            dVel = dVel * resolution;	//이동 속도 
            dAcc = dAccel;      //! 가속 
            dDec = dDecel;      //! 감속

            // 설정한 거리만큼 또는 위치까지 이동한다.
            // 지정 축의 절대 좌표/ 상대좌표 로 설정된 위치까지 설정된 속도와 가속율로 구동을 한다.
            // 속도 프로파일은 AxmMotSetProfileMode 함수에서 설정한다.
            // 펄스가 출력되는 시점에서 함수를 벗어난다.
            // AxmMotSetAccelUnit(lAxisNo, 1) 일경우 dAccel -> dAccelTime , dDecel -> dDecelTime 으로 바뀐다.
            CAXM.AxmMoveStartPos(this.axisNo, dPos, dVel, dAcc, dDec);

            return true;
        }
        public bool GetAmpFault()
        {
            uint dwStatus = 0;
            uint dwRetVal = 0;


            dwRetVal = CAXM.AxmSignalGetServoAlarm(this.axisNo, ref dwStatus);
            if (dwRetVal != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                return true;
            }

            if (dwStatus == (uint)AXT_MOTION_LEVEL_MODE.UNUSED)
                return true;


            // 축의 알람 신호 확인
            dwRetVal = CAXM.AxmSignalReadServoAlarm(this.axisNo, ref dwStatus);
            if (dwRetVal != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                bOrgState = false;
                return true;
            }

            if (dwStatus == (uint)AXT_MOTION_SIGNAL_LEVEL.ACTIVE)
            {
                bOrgState = false;
                return true;
            }

            return false;
        }
        public bool GetStopAxis()
        {
            uint dwRetVal = 0;
            uint dwStatus = 0;
            dwRetVal = CAXM.AxmStatusReadInMotion(this.axisNo, ref dwStatus);
            if (dwRetVal != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                return false;
            }

            if (dwStatus != (uint)AXT_MOTION_SIGNAL_LEVEL.ACTIVE)
            {
                return true;
            }
            return false;
        }
        public bool GetPosiSensor()
        {
            uint dwStatus = 0;
            uint dwRetVal = 0;
            uint dwPositiveLevel = 0;
            uint dwNegativeLevel = 0;

            dwRetVal = CAXM.AxmSignalGetLimit(this.axisNo, ref dwStatus, ref dwPositiveLevel, ref dwNegativeLevel);
            if (dwRetVal != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                return false;
            }
            if (dwPositiveLevel == (uint)AXT_MOTION_LEVEL_MODE.UNUSED)
            {
                return false;
            }


            dwRetVal = CAXM.AxmSignalReadLimit(this.axisNo, ref dwPositiveLevel, ref dwNegativeLevel);
            if (dwRetVal != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                return false;
            }
            if (dwPositiveLevel == (uint)AXT_MOTION_SIGNAL_LEVEL.ACTIVE)
            {
                return true;
            }
            return false;
        }
        public bool GetHomeSensor()
        {
            uint dwStatus = 0;
            uint dwRetVal = 0;

            dwRetVal = CAXM.AxmHomeReadSignal(this.axisNo, ref dwStatus);
            if (dwRetVal != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                return false;
            }
            if (dwStatus == (uint)AXT_MOTION_SIGNAL_LEVEL.INACTIVE)
            {
                return false;
            }
            return true;
        }
        public bool GetNegaSensor()
        {
            uint dwStatus = 0;
            uint dwRetVal = 0;
            uint dwPositiveLevel = 0;
            uint dwNegativeLevel = 0;

            dwRetVal = CAXM.AxmSignalGetLimit(this.axisNo, ref dwStatus, ref dwPositiveLevel, ref dwNegativeLevel);
            if (dwRetVal != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                return false;
            }
            if (dwNegativeLevel == (uint)AXT_MOTION_LEVEL_MODE.UNUSED)
            {
                return false;
            }


            dwRetVal = CAXM.AxmSignalReadLimit(this.axisNo, ref dwPositiveLevel, ref dwNegativeLevel);
            if (dwRetVal != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                return false;
            }
            if (dwNegativeLevel == (uint)AXT_MOTION_SIGNAL_LEVEL.ACTIVE)
            {
                return true;
            }
            return false;
        }

        public bool GetAmpEnable()
        {
            uint duLevel = (uint)AXT_USE.ENABLE;

            CAXM.AxmSignalIsServoOn(this.axisNo, ref duLevel);
            if (duLevel == (uint)AXT_MOTION_SIGNAL_LEVEL.ACTIVE)
            {
                return true;
            }

            bOrgState = false;

            return false;
        }
        public bool AmpEnable()
        {
           // int nUseAxis = 0;
            uint duLevel = (uint)AXT_USE.ENABLE;

            CAXM.AxmSignalIsServoOn(this.axisNo, ref duLevel);

            if (duLevel == (uint)AXT_USE.ENABLE)
            {
                return true;
            }

            bOrgState = false;

            CAXM.AxmSignalServoOn(this.axisNo, (uint)AXT_USE.ENABLE);

            Thread.Sleep(300);

            CAXM.AxmSignalIsServoOn(this.axisNo, ref duLevel);
            if (duLevel != (uint)AXT_USE.ENABLE)
            {
                return false;
            }
            return true;
        }
        public bool AmpDisable()
        {
            bOrgState = false;

            CAXM.AxmMoveStop(this.axisNo, dDecel);
            CAXM.AxmSignalServoOn(this.axisNo, (uint)AXT_USE.DISABLE);

            //Thread.Sleep(300);
            //if (MOTOR_TYPE[nAxis] == STEPING)
            //{
            //AxmSignalServoAlarmReset(nUseAxis, ENABLE);
            //}

            return true;
        }
        public bool AmpFaultReset()
        {

            uint duLevel = 0;
            CAXM.AxmMoveStop(this.axisNo, dDecel);
            CAXM.AxmSignalServoOn(this.axisNo, (uint)AXT_USE.DISABLE);

            CAXM.AxmSignalServoAlarmReset(this.axisNo, (uint)AXT_USE.ENABLE);

            Thread.Sleep(100);

            CAXM.AxmSignalServoAlarmReset(this.axisNo, (uint)AXT_USE.DISABLE);
            CAXM.AxmSignalServoOn(this.axisNo, (uint)AXT_USE.ENABLE);

            Thread.Sleep(100);

            CAXM.AxmSignalIsServoOn(this.axisNo, ref duLevel);
            if (duLevel != (uint)AXT_USE.ENABLE)
            {
                return false;
            }

            return true;
        }
    }
}
