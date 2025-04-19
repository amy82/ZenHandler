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
        //Firset Set
        public int m_lAxisNo { get; protected set; } = 0;            // 축 번호 MOTOR_PCB_X = 0, MOTOR_PCB_Y,
        public string Name { get; protected set; } = "";
        public MotorDefine.eMotorType Type { get; protected set; }                 //LINEAR, STEPING
        public AXT_MOTION_LEVEL_MODE AxtSetServoAlarm { get; protected set; }  //AxmSignalSetServoAlarm
        public AXT_MOTION_LEVEL_MODE AxtSetLimit { get; protected set; }  //AxmSignalSetLimit
        public double FirstVel { get; protected set; } = 0.0;
        public double SecondVel { get; protected set; } = 0.0;
        public double ThirdVel { get; protected set; } = 0.0;
        //
        //Second Set
        public double Velocity { get; set; }                        //속도 = Move 속도 , Jog 속도 나눠야 될 수도
        public double Acceleration { get; set; }                    //가속
        public double Deceleration { get; set; }                    //감속
        public double Resolution { get; protected set; }
        public double MaxSpeed { get; protected set; }
        //
        //
        public double EncoderPos
        {
            get
            {
                return GetEncoderPos(); // 호출 시마다 최신값 반환
            }
            //protected set { } // 필요하면 나중에 로직 추가 가능
        }
        public bool OrgState { get; set; }                //원점 상태
        //
        //
        public AXT_MOTION_MOVE_DIR HomeMoveDir { get; protected set; }              //DIR_CW= 0x1, 시계방향/ DIR_CCW= 0x0, 반시계방향
        public AXT_MOTION_HOME_DETECT HomeDetect { get; protected set; }               //HomeSensor, PosEndLimit, NegEndLimit
        //
        //
        public bool IsMotorBusy;        //실행중 체크용 플래그
        public bool MotorBreak;         //while 빠져 나오는 용도
        // dwAbsRelMode : (0)POS_ABS_MODE - 현재 위치와 상관없이 지정한 위치로 절대좌표 이동합니다.
        //                (1)POS_REL_MODE - 현재 위치에서 지정한 양만큼 상대좌표 이동합니다.
        //(uint)AXT_MOTION_ABSREL.POS_ABS_MODE / POS_REL_MODE


        public MotorAxis(int axisNumber, string name, MotorDefine.eMotorType type, double maxSpeed, AXT_MOTION_LEVEL_MODE limit, AXT_MOTION_LEVEL_MODE alarm,
            double firstvel , double secondvel , double thirdvel,
            AXT_MOTION_HOME_DETECT homeDetect , AXT_MOTION_MOVE_DIR homeDir)
        {
            this.m_lAxisNo = axisNumber;
            this.Name = name;
            this.Type = type;
            this.MaxSpeed = maxSpeed;
            this.AxtSetLimit = limit;
            this.AxtSetServoAlarm = alarm;
            this.FirstVel = firstvel;
            this.SecondVel = secondvel;
            this.ThirdVel = thirdvel;
            this.HomeDetect = homeDetect;
            this.HomeMoveDir = homeDir;

            this.MotorBreak = false;     //init
            this.IsMotorBusy = false;
        }
        
        public virtual void setMotorParameter(double vel , double acc , double dec , double resol)
        {
            if (vel < 10)
            {
                vel = 10;
            }

            if (acc < 0.1)
            {
                acc = 0.1;
            }
            if (acc > 3.0)
            {
                acc = 3.0;
            }
            if (dec < 0.1)
            {
                dec = 0.1;
            }
            if (dec > 3.0)
            {
                dec = 3.0;
            }

            if (resol < 1000.0)
            {
                resol = 1000.0;
            }
            Velocity = vel;
            Acceleration = acc;
            Deceleration = dec;
            Resolution = resol;
        }


        public virtual void ServoOn()
        {
            CAXM.AxmSignalServoOn(m_lAxisNo, (uint)AXT_USE.ENABLE);
        }
        public virtual void ServoOff()
        {
            this.OrgState = false;
            CAXM.AxmSignalServoOn(m_lAxisNo, (uint)AXT_USE.DISABLE);
        }
        public virtual void ServoAlarmReset(int dOnOff)
        {
            if (dOnOff == 0)
            {
                CAXM.AxmSignalServoAlarmReset(m_lAxisNo, (uint)AXT_USE.DISABLE);
            }
            else
            {
                CAXM.AxmSignalServoAlarmReset(m_lAxisNo, (uint)AXT_USE.ENABLE);
            }
            
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
        public bool MoveAxisLimit(double dVel, double dAcc, AXT_MOTION_HOME_DETECT Detect, AXT_MOTION_EDGE Edge, AXT_MOTION_STOPMODE StopMode, bool bWait = false)
        {
            uint duRetCode = 0;

            double DVel = dVel * this.Resolution;
            double DAcc = dAcc;


            duRetCode = CAXM.AxmMoveSignalSearch(this.m_lAxisNo, DVel, DAcc, (int)Detect, (int)Edge, (int)StopMode);

            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {

                Console.WriteLine("MotorControl", $"AxmMoveSignalSearch return error[Code:{duRetCode}]", Globalo.eMessageName.M_ERROR);

                return false;
            }

            if (bWait)
            {
                //TODO: 구현 필요 센서 감시 확인 , 특정 센서(ex) 상단감지 감지 이동 함수도 필요
                while (bWait)
                {
                    if (MotorBreak) break;

                    
                }
            }
            return true;
        }

        public bool JogMove(int direction, double Speed)
        {
            //Speed = 0.1 , 0.5 , 1.0 Low , Mid , High
            Console.WriteLine($"방향 {direction}으로 조그 이동");
            double dAcc = 0.0;
            double dDec = 0.0;
            uint dwRetVal = 0;
            double dJogSpeed = 0.0;

            //MOTOR_JOG_LOW = 0.1
            //MOTOR_JOG_MID = 0.5

            //MOTOR_JOG_HIGH = 1.0

            string str = "";

            if (this.GetAmpFault() == true)    //알람 신호 입력 상태 확인
            {
                str = $"{this.Name} Motor ServoAlarm occurs";
                return false;
            }
            if (this.GetAmpEnable() == false)      //Servo-On 상태 확인
            {
                str = $"{this.Name} Motor Servo Off State";
                return false;
            }
            if (this.GetStopAxis() == false)      //정지 상태 확인
            {
                str = $"{this.Name} Motor Stop status check failed";
                return false;
            }
            //if (this.OrgState == false)       //조그라서 빠져도 될듯 
            //{
            //    str = $"{this.Name} Failed to check for return to origin";
            //    return false;
            //}

            if (direction == 1)
            {
                dJogSpeed = (this.Velocity * this.Resolution) * Speed;        //Speed = 0.1 , 0.5 , 1.0
            }
            else
            {
                dJogSpeed = Math.Abs((this.Velocity * this.Resolution) * Speed) * -1;
            }

            if (MotionControl.MotorSet.MOTOR_ACC_TYPE_SEC)
            {
                dAcc = this.Acceleration;      //! 가속 
                dDec = this.Deceleration;      //! 감속
            }
            else
            {
                dAcc = this.Acceleration * (9.8 * 1000 * this.Resolution);      //! 가속 
                dDec = this.Deceleration * (9.8 * 1000 * this.Resolution);      //! 감속
            }

            dwRetVal = CAXM.AxmMoveVel(this.m_lAxisNo, dJogSpeed, dAcc, dDec);

            if (dwRetVal != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                return false;
            }

            return true;
        }
        //-----------------------------------------------------------------------------
        //
        //	지정 축을 절대 구동 또는 상대 구동으로 이동한다. 
        //
        //-----------------------------------------------------------------------------
        public bool MoveAxis(double dPos, AXT_MOTION_ABSREL nAbsFlag, bool bWait)
        {
            bool isSuccess = false;
            double dCurrPos = 0.0;
            double dVel = 0.0;
            double dAcc = 0.0;
            double dDec = 0.0;
            string str = "";

            if (this.GetAmpFault() == true)    //알람 신호 입력 상태 확인
            {
                str = $"{this.Name} Motor ServoAlarm occurs";
                return false;
            }
            if (this.GetAmpEnable() == false)      //Servo-On 상태 확인
            {
                str = $"{this.Name} Motor Servo Off State";
                return false;
            }
            if (this.GetStopAxis() == false)      //정지 상태 확인
            {
                str = $"{this.Name} Motor Stop status check failed";
                return false;
            }
            if (nAbsFlag == AXT_MOTION_ABSREL.POS_ABS_MODE)
            {
                if (this.OrgState == false)       //TODO: + , - 움직이는 거라서 원점 체크 빼도될듯 
                {
                    str = $"{this.Name} Failed to check for return to origin";
                    return false;
                }
            }
            ////
            ////
            ////
            ///
            dVel = this.Velocity * this.Resolution;   //이동 속도 

            if (nAbsFlag == AXT_MOTION_ABSREL.POS_ABS_MODE)
            {
                dVel *= 0.5;
                dCurrPos = this.EncoderPos;

                if (Math.Abs(dCurrPos - dPos) < 0.0001)
                {
                    return true;
                }
            }
            else if (nAbsFlag == AXT_MOTION_ABSREL.POS_REL_MODE)
            {
                dPos += this.EncoderPos;
            }
            else
            {
                str = $"{this.Name} Motor movement command error";
                return false;
            }

            dPos *= this.Resolution;

            if (dPos > 0)
            {
                dPos = (int)(dPos + 0.5);
            }
            


            if (MotionControl.MotorSet.MOTOR_ACC_TYPE_SEC)
            {
                dAcc = this.Acceleration;      //! 가속 
                dDec = this.Deceleration;      //! 감속
            }
            else
            {
                dAcc = this.Acceleration * (9.8 * 1000 * this.Resolution);      //! 가속 
                dDec = this.Deceleration * (9.8 * 1000 * this.Resolution);      //! 감속
            }

            // 설정한 거리만큼 또는 위치까지 이동한다.
            // 지정 축의 절대 좌표/ 상대좌표 로 설정된 위치까지 설정된 속도와 가속율로 구동을 한다.
            // 속도 프로파일은 AxmMotSetProfileMode 함수에서 설정한다.
            // 펄스가 출력되는 시점에서 함수를 벗어난다.
            // AxmMotSetAccelUnit(lAxisNo, 1) 일경우 dAccel -> dAccelTime , dDecel -> dDecelTime 으로 바뀐다.

            this.IsMotorBusy = true;
            uint duRetCode = CAXM.AxmMoveStartPos(this.m_lAxisNo, dPos, dVel, dAcc, dDec);

            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                Console.WriteLine($"AxmMoveStartPos return error[Code:{duRetCode}]");
                return false;
            }
            if (bWait == false)
            {
                return true;
            }
            else
            {
                //이동 위치 확인 
                int step = 100;
                int nTimeTick = 0;

                while (bWait)
                {
                    if (MotorBreak) break;
                    //위치 도착 확인 , 정지 확인

                    switch (step)
                    {
                        case 100:
                            if (this.GetStopAxis() == true)
                            {
                                Console.WriteLine($"{this.Name }Motor Stop Check");
                                step = 200;
                            }
                            nTimeTick = Environment.TickCount;
                            break;
                        case 200:
                            if ((this.EncoderPos - dPos) < MotionControl.MotorSet.ENCORDER_GAP)
                            {
                                isSuccess = true;
                                Console.WriteLine($"{this.Name }Motor Move Check");
                                step = 1000;
                            }
                            break;
                        default:
                            break;
                    }
                    if (step >= 1000)
                    {
                        break;
                    }
                    if (Environment.TickCount - nTimeTick > MotionControl.MotorSet.MOTOR_MOVE_TIMEOUT)
                    {
                        if (step == 100)//정지 실패
                        {
                            Console.WriteLine($"{this.Name }Motor Stop Timeout error[Code:{duRetCode}]");
                        }
                        else if (step == 200)//위치 이동 실패
                        {
                            Console.WriteLine($"{this.Name }Motor Move Timeout error[Code:{duRetCode}]");
                        }
                        isSuccess = false;
                        break;
                    }
                    Thread.Sleep(10);
                }
            }
            IsMotorBusy = false;
            return isSuccess;
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
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return dPos;
            }
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
