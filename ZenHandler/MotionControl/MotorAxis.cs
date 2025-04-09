using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//0. [MotorController.cs]
//1. [Base.cs]
//0. [MotorController.cs]



//0.공통 모터 기능 - 속도 , 가감속, 분해능, 리밋/홈 센서 접점 
//1. (추상 클래스) 공통 기능? 조그 이동? 티칭 위치 이동  , 정지, 원점 잡기?
//2.이재기 , 매거진, 리프트, pcb축 - pcb축이면 소켓 업다운 등도 넣기 

namespace ZenHandler.MotionControl
{
    public class MotorAxis
    {
        //MotorController
        //
        public int m_lAxisNo { get; protected set; } = 0;            // 축 번호 MOTOR_PCB_X = 0, MOTOR_PCB_Y,
        public string Name { get; protected set; } = "";
        public MotorDefine.eMotorType Type { get; protected set; }                 //LINEAR, STEPING
        public double CommnadPos { get; protected set; }        //현재 위치 AxmStatusGetCmdPos : STEPING
        public double ActualPos { get; protected set; }         //현재 위치 AxmStatusGetActPos : LINEAR , SERVO
        public double CommandVelocity { get; protected set; }   //지정한 축의 구동 속도 AxmStatusReadVel
        public int Velocity { get; set; }                       //속도 = Move 속도 , Jog 속도 나눠야 될 수도
        public int Acceleration { get; protected set; }         //가속
        public int Deceleration { get; protected set; }         //감속
        public int Resolution { get; protected set; }
        public bool OrgState { get; protected set; }             //원점 상태
        public bool RunState { get; protected set; }             //동작 상태
        //
        //
        public int MaxSpeed { get; protected set; }             //1000
        public int HomeMoveDir { get; protected set; }          //DIR_CW= 0x1, 시계방향/ DIR_CCW= 0x0, 반시계방향
        public int HomeDetect { get; protected set; }           //HomeSensor, PosEndLimit, NegEndLimit

        // dwAbsRelMode : (0)POS_ABS_MODE - 현재 위치와 상관없이 지정한 위치로 절대좌표 이동합니다.
        //                (1)POS_REL_MODE - 현재 위치에서 지정한 양만큼 상대좌표 이동합니다.
        //(uint)AXT_MOTION_ABSREL.POS_ABS_MODE / POS_REL_MODE
        public MotorAxis(int axisNumber, string name)
        {
            this.m_lAxisNo = axisNumber;
            this.Name = name;
        }
        public virtual void ServoOn()
        {
            uint duOnOff = 1;
            CAXM.AxmSignalServoOn(m_lAxisNo, duOnOff);
        }
        public virtual void ServoOff()
        {
            uint duOnOff = 0;
            CAXM.AxmSignalServoOn(m_lAxisNo, duOnOff);
        }
        public virtual void Stop(int type = 0)
        {
            CAXM.AxmMoveSStop(m_lAxisNo);       //감속 정지

            //AxmMoveStop(int nAxisNo, double dDecel) : 설정한 감속도로 감속 정지
            //AxmMoveEStop(int nAxisNo) : 급 정지
            //AxmMoveSStop(int nAxisNo) : 감속 정지

            //AxmMoveStopEx : xx 사용 
        }

        public virtual bool GetAmpEnable()
        {
            uint duLevel = (uint)AXT_USE.ENABLE;

            // 현재의 Servo-On 신호의 출력 상태를 반환
            CAXM.AxmSignalIsServoOn(this.m_lAxisNo, ref duLevel);
            if (duLevel == (uint)AXT_MOTION_SIGNAL_LEVEL.ACTIVE)
            {
                return true;
            }

            OrgState = false;

            return false;
        }
        public virtual bool AmpEnable()
        {
            // int nUseAxis = 0;
            uint duLevel = (uint)AXT_USE.ENABLE;

            CAXM.AxmSignalIsServoOn(this.m_lAxisNo, ref duLevel);

            if (duLevel == (uint)AXT_USE.ENABLE)
            {
                return true;
            }

            OrgState = false;

            CAXM.AxmSignalServoOn(this.m_lAxisNo, (uint)AXT_USE.ENABLE);

            Thread.Sleep(300);

            CAXM.AxmSignalIsServoOn(this.m_lAxisNo, ref duLevel);
            if (duLevel != (uint)AXT_USE.ENABLE)
            {
                return false;
            }
            return true;
        }
        public virtual bool AmpDisable()
        {
            OrgState = false;

            CAXM.AxmMoveStop(this.m_lAxisNo, this.Deceleration);
            CAXM.AxmSignalServoOn(this.m_lAxisNo, (uint)AXT_USE.DISABLE);

            //Thread.Sleep(300);
            //if (MOTOR_TYPE[nAxis] == STEPING)
            //{
            //AxmSignalServoAlarmReset(nUseAxis, ENABLE);
            //}

            return true;
        }
        public virtual bool GetServoState()
        {
            uint duOnOff = 0;
            CAXM.AxmSignalIsServoOn(m_lAxisNo, ref duOnOff);

            if (duOnOff == (uint)AXT_USE.ENABLE)
            {
                return true;
            }
            return false;
        }
        public void EpoxyDraw()
        {
            Console.WriteLine("도포 실행");
        }
        public virtual bool GetStopAxis()
        {
            uint dwRetVal = 0;
            uint dwStatus = 0;
            dwRetVal = CAXM.AxmStatusReadInMotion(this.m_lAxisNo, ref dwStatus);
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

            dwRetVal = CAXM.AxmSignalGetLimit(this.m_lAxisNo, ref dwStatus, ref dwPositiveLevel, ref dwNegativeLevel);
            if (dwRetVal != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                return false;
            }
            if (dwPositiveLevel == (uint)AXT_MOTION_LEVEL_MODE.UNUSED)
            {
                return false;
            }


            dwRetVal = CAXM.AxmSignalReadLimit(this.m_lAxisNo, ref dwPositiveLevel, ref dwNegativeLevel);
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
        public double GetEncoderPos()
        {
            double dPos = 0.0;

            if (this.Type == MotorDefine.eMotorType.LINEAR)
            {
                //리니어,서보
                CAXM.AxmStatusGetActPos(this.m_lAxisNo, ref dPos);
            }
            else
            {
                //스테핑
                CAXM.AxmStatusGetCmdPos(this.m_lAxisNo, ref dPos);
            }

            dPos /= Resolution;
            return dPos;
        }
        public virtual bool GetHomeSensor()
        {
            uint dwStatus = 0;
            uint dwRetVal = 0;

            dwRetVal = CAXM.AxmHomeReadSignal(this.m_lAxisNo, ref dwStatus);
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
        public virtual bool GetNegaSensor()
        {
            uint dwStatus = 0;
            uint dwRetVal = 0;
            uint dwPositiveLevel = 0;
            uint dwNegativeLevel = 0;

            dwRetVal = CAXM.AxmSignalGetLimit(this.m_lAxisNo, ref dwStatus, ref dwPositiveLevel, ref dwNegativeLevel);
            if (dwRetVal != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                return false;
            }
            if (dwNegativeLevel == (uint)AXT_MOTION_LEVEL_MODE.UNUSED)
            {
                return false;
            }


            dwRetVal = CAXM.AxmSignalReadLimit(this.m_lAxisNo, ref dwPositiveLevel, ref dwNegativeLevel);
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
        public virtual bool GetAmpFault()
        {
            uint dwStatus = 0;
            uint dwRetVal = 0;


            dwRetVal = CAXM.AxmSignalGetServoAlarm(this.m_lAxisNo, ref dwStatus);
            if (dwRetVal != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                return true;
            }

            if (dwStatus == (uint)AXT_MOTION_LEVEL_MODE.UNUSED)
            {
                return true;
            }



            // 축의 알람 신호 확인
            //Alarm 입력이 On(1) 되면 해당 축의 모션작업은 급정지 하게 된다.

            dwRetVal = CAXM.AxmSignalReadServoAlarm(this.m_lAxisNo, ref dwStatus);
            if (dwRetVal != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                OrgState = false;
                return true;
            }

            if (dwStatus == (uint)AXT_MOTION_SIGNAL_LEVEL.ACTIVE)
            {
                OrgState = false;
                return true;
            }

            return false;
        }
        public virtual bool AmpFaultReset()
        {

            uint duLevel = 0;
            CAXM.AxmMoveStop(this.m_lAxisNo, this.Deceleration);
            CAXM.AxmSignalServoOn(this.m_lAxisNo, (uint)AXT_USE.DISABLE);

            CAXM.AxmSignalServoAlarmReset(this.m_lAxisNo, (uint)AXT_USE.ENABLE);

            Thread.Sleep(100);

            CAXM.AxmSignalServoAlarmReset(this.m_lAxisNo, (uint)AXT_USE.DISABLE);
            CAXM.AxmSignalServoOn(this.m_lAxisNo, (uint)AXT_USE.ENABLE);

            Thread.Sleep(100);

            CAXM.AxmSignalIsServoOn(this.m_lAxisNo, ref duLevel);
            if (duLevel != (uint)AXT_USE.ENABLE)
            {
                return false;
            }

            return true;
        }

        public void EnableMotor() { Console.WriteLine("모터 활성화"); }
        public void DisableMotor() { Console.WriteLine("모터 비활성화"); }
    }
}
