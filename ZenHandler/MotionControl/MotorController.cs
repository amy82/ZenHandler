using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZenHandler.MotionControl
{
    //class MotionBase
    public abstract class MotorController
    {
        protected FThread.MotorAutoThread AutoUnitThread;
        //protected FThread.MotorManualThread motorManualThread;
        
        public Process.ProcessManager processManager;
        public OperationState RunState = OperationState.Stopped;
        public string MachineName { get; protected set; }
        protected CancellationTokenSource CancelToken;      //TODO: 사용안하는듯?

        ////protected bool isMotorBusy = false; //실행중 체크용 플래그
        //abstract : 추상 메서드
        //반드시 자식 클래스에 구현해야 함, 내용없이 선언가능, 강제 특정 메서드 오버라이딩 유도
        //추상 클래스 안에서만 사용가능 (abstract class)

        //virtual : 가상 메서드
        //기본 구현을 제공하지만, 필요하면 자식 클래스에서 재정의 가능
        //오버라이딩(재정의)하지 않아도 사용가능
        //선택적으로 변경할수 있도록 유도


        public MotorController()//string name
        {
            AutoUnitThread = new FThread.MotorAutoThread(this);        //TODO: MotorAutoThread 쓰레드 종료하는 거 추가해야된다.
            //motorManualThread = new FThread.MotorManualThread(this);

            processManager = new Process.ProcessManager();

            CancelToken = new CancellationTokenSource();
        }

        public abstract void StopAuto();
        public abstract bool AutoRun();
        public abstract void PauseAuto();

        public abstract bool OriginRun();
        public abstract bool ReadyRun();

        public abstract bool IsMoving();
        public abstract void MovingStop();
        public abstract void MotorDataSet();
        public abstract bool TaskSave();


        public virtual void MachineClose()
        {
            AutoUnitThread.Close();
        }
        public virtual bool MultiAxisMove(MotorAxis[] multiAxis, double[] dMultiPos, bool bWait = false)
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            bool isSuccess = false;
            int multiCnt = multiAxis.Count();
            int i = 0;
            string str = "";


            int[] dMultiAxis = new int[multiCnt];
            double[] dMultiCurrPos = new double[multiCnt];
            double[] dMultiVel = new double[multiCnt];
            double[] dMultiAcc = new double[multiCnt];
            double[] dMultiDec = new double[multiCnt];

            int nMotorCount = 0;


            for (i = 0, nMotorCount = 0; i < multiCnt; i++)
            {
                multiAxis[i].IsMotorBusy = true;
                multiAxis[i].MotorBreak = false;

                if (multiAxis[i].GetAmpFault() == true)    //알람 신호 입력 상태 확인
                {
                    str = $"{multiAxis[i].Name} Motor ServoAlarm occurs";
                    return false;
                }
                if (multiAxis[i].GetAmpEnable() == false)      //Servo-On 상태 확인
                {
                    str = $"{multiAxis[i].Name} Motor Servo Off State";
                    return false;
                }
                if (multiAxis[i].GetStopAxis() == false)      //정지 상태 확인
                {
                    str = $"{multiAxis[i].Name} Motor Stop status check failed";
                    return false;
                }
                if (multiAxis[i].OrgState == false)
                {
                    str = $"{multiAxis[i].Name} Failed to check for return to origin";
                    return false;
                }


                dMultiCurrPos[i] = multiAxis[i].EncoderPos;

                if (Math.Abs(dMultiCurrPos[i] - dMultiPos[i]) < 0.0001)
                {
                    continue;
                }

                //

                dMultiAxis[nMotorCount] = multiAxis[i].m_lAxisNo;
                dMultiCurrPos[nMotorCount] = dMultiPos[i] * multiAxis[i].Resolution;

                //if (dMultiCurrPos[nMotorCount] > 0)
                //{
                //    dMultiCurrPos[nMotorCount] = (int)(dMultiPos[i] + 0.5);
                //}

                dMultiVel[nMotorCount] = multiAxis[i].Velocity * multiAxis[i].Resolution; //이동 속도 

                if(MotionControl.MotorSet.MOTOR_ACC_TYPE_SEC)
                {
                    dMultiAcc[nMotorCount] = multiAxis[i].Acceleration;      //! 가속 
                    dMultiDec[nMotorCount] = multiAxis[i].Deceleration;      //! 감속
                }
                else
                {
                    dMultiAcc[nMotorCount] = multiAxis[i].Acceleration * (9.8 * 1000 * multiAxis[i].Resolution);      //! 가속 
                    dMultiDec[nMotorCount] = multiAxis[i].Deceleration * (9.8 * 1000 * multiAxis[i].Resolution);      //! 감속
                }
                


                nMotorCount++;
            }

            if(nMotorCount == 0)
            {
                return true;
            }

            //여러 개의 축에 대해서 현재의 위치에서 지정한 거리만큼 이동을 동시에 시작한다.
            //이 함수를 사용하면 여러 개의 축이 동시에 작업을 시작한다.
            //이 함수는 여러 축이 동기를 맞추어 작업을 시작해야하는경우에 사용한다.
            //펄스 출력이 시작되는 시점에서 함수를 벗어난다.


            //가속도 (가속도의 단위는 Unit/pulse를 1/1로 한경우에 PPS[Pulses/sec^2]) 배열
            //감속도(감속도의 단위는 Unit/pulse를 1/1로 한경우에 PPS[Pulses/sec^2]) 배열

            uint duRetCode = CAXM.AxmMoveStartMultiPos(nMotorCount, dMultiAxis, dMultiCurrPos, dMultiVel, dMultiAcc, dMultiDec);

            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                Console.WriteLine($"AxmMoveStartMultiPos return error[Code:{duRetCode}]");
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
                int SkipChk = 0;
                while (bWait)
                {
                    for (i = 0; i < multiCnt; i++)
                    {
                        if (multiAxis[i].MotorBreak) break;
                    }
                        
                    //위치 도착 확인 , 정지 확인

                    switch (step)
                    {
                        case 100:
                            SkipChk = 0;
                            for (i = 0; i < multiCnt; i++)
                            {
                                if (multiAxis[i].GetStopAxis() == true)
                                {
                                    SkipChk++;
                                }
                            }
                            if(SkipChk == multiCnt)
                            {
                                Console.WriteLine($"{multiAxis[i].Name }Motor Stop Check");
                                step = 200;
                                nTimeTick = Environment.TickCount;
                            }
                            
                            
                            break;
                        case 200:
                            SkipChk = 0;
                            for (i = 0; i < multiCnt; i++)
                            {
                                if ((multiAxis[i].EncoderPos - dMultiPos[i]) < MotionControl.MotorSet.ENCORDER_GAP)
                                {
                                    SkipChk++;
                                    multiAxis[i].IsMotorBusy = false;
                                }
                            }
                            if (SkipChk == multiCnt)
                            {
                                isSuccess = true;
                                Console.WriteLine($"{multiAxis[i].Name }Motor Move Check");
                                step = 1000;
                                nTimeTick = Environment.TickCount;
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
                        isSuccess = false;
                        if (step == 100)//정지 실패
                        {
                            Console.WriteLine($"Motor Stop Timeout error[Code:{duRetCode}]");
                        }
                        else if (step == 200)//위치 이동 실패
                        {
                            Console.WriteLine($"Motor Move Timeout error[Code:{duRetCode}]");
                        }
                        break;
                    }
                    Thread.Sleep(10);
                }
            }
            
            return isSuccess;
        }


#region 확인 후 삭제 [SingleAxisMove]
        public virtual bool SingleAxisMove(MotorAxis nAxis, double dPos, AXT_MOTION_ABSREL nAbsFlag, bool bWait = false)
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            bool isSuccess = false;
            double dCurrPos = 0.0;
            double dVel = 0.0;
            double dAcc = 0.0;
            double dDec = 0.0;
            string str = "";
            nAxis.MotorBreak = false;
            nAxis.IsMotorBusy = true;


            if (nAxis.GetAmpFault() == true)    //알람 신호 입력 상태 확인
            {
                str = $"{nAxis.Name} Motor ServoAlarm occurs";
                return false;
            }
            if (nAxis.GetAmpEnable() == false)      //Servo-On 상태 확인
            {
                str = $"{nAxis.Name} Motor Servo Off State";
                return false;
            }
            if (nAxis.GetStopAxis() == false)      //정지 상태 확인
            {
                str = $"{nAxis.Name} Motor Stop status check failed";
                return false;
            }
            if (nAbsFlag == AXT_MOTION_ABSREL.POS_ABS_MODE)
            {
                if (nAxis.OrgState == false)        //MEMO: REL 상대 위치 이동일때는 원점 확인 필요 없을듯 , ABS = 티칭위치 이동
                {
                    str = $"{nAxis.Name} Failed to check for return to origin";
                    return false;
                }
            }




            if (nAbsFlag == AXT_MOTION_ABSREL.POS_ABS_MODE)
            {
                dCurrPos = nAxis.EncoderPos;

                if (Math.Abs(dCurrPos - dPos) < 0.0001)
                {
                    return true;
                }
            }
            else if (nAbsFlag == AXT_MOTION_ABSREL.POS_REL_MODE)
            {
                dPos += nAxis.EncoderPos;
            }
            else
            {
                str = $"{nAxis.Name} Motor movement command error";
                return false;
            }

            dPos *= nAxis.Resolution;

            if (dPos > 0)
            {
                dPos = (int)(dPos + 0.5);
            }
            dVel = nAxis.Velocity * nAxis.Resolution;   //이동 속도 


            if (MotionControl.MotorSet.MOTOR_ACC_TYPE_SEC)
            {
                dAcc = nAxis.Acceleration;      //! 가속 
                dDec = nAxis.Deceleration;      //! 감속
            }
            else
            {
                dAcc = nAxis.Acceleration * (9.8 * 1000 * nAxis.Resolution);      //! 가속 
                dDec = nAxis.Deceleration * (9.8 * 1000 * nAxis.Resolution);      //! 감속
            }

            // 설정한 거리만큼 또는 위치까지 이동한다.
            // 지정 축의 절대 좌표/ 상대좌표 로 설정된 위치까지 설정된 속도와 가속율로 구동을 한다.
            // 속도 프로파일은 AxmMotSetProfileMode 함수에서 설정한다.
            // 펄스가 출력되는 시점에서 함수를 벗어난다.
            // AxmMotSetAccelUnit(lAxisNo, 1) 일경우 dAccel -> dAccelTime , dDecel -> dDecelTime 으로 바뀐다.


            uint duRetCode = CAXM.AxmMoveStartPos(nAxis.m_lAxisNo, dPos, dVel, dAcc, dDec);

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
                    if (nAxis.MotorBreak) break;
                    //위치 도착 확인 , 정지 확인

                    switch (step)
                    {
                        case 100:
                            if (nAxis.GetStopAxis() == true)
                            {
                                Console.WriteLine($"{nAxis.Name }Motor Stop Check");
                                step = 200;
                            }
                            nTimeTick = Environment.TickCount;
                            break;
                        case 200:
                            if ((nAxis.EncoderPos - dPos) < MotionControl.MotorSet.ENCORDER_GAP)
                            {
                                isSuccess = true;
                                Console.WriteLine($"{nAxis.Name }Motor Move Check");
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
                            Console.WriteLine($"{nAxis.Name }Motor Stop Timeout error[Code:{duRetCode}]");
                        }
                        else if (step == 200)//위치 이동 실패
                        {
                            Console.WriteLine($"{nAxis.Name }Motor Move Timeout error[Code:{duRetCode}]");
                        }
                        isSuccess = false;
                        break;
                    }
                    Thread.Sleep(10);
                }
            }
            nAxis.IsMotorBusy = false;
            return isSuccess;
        }
        #endregion
    }
}
