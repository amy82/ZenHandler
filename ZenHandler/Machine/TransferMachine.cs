using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZenHandler.Machine
{
    public class TransferMachine : MotionControl.MotorController
    {
        public int MotorCnt { get; private set; } = 3;

        private MotionControl.MotorAxis TransferX;
        private MotionControl.MotorAxis TransferY;
        private MotionControl.MotorAxis TransferZ;

        public MotionControl.MotorAxis[] MotorAxes; // 배열 선언


        public string[] axisName = { "TransferX", "TransferY", "TransferZ" };
        public string[] TeachingPos = { "Wait", "Load", "UnLoad" };
        

        public string processName = "tttt";

        
        //public Dio cylinder;
        //픽업 툴 4개 실린더 Dio 로 지정?

        public TransferMachine()//: base("Machine")
        {
            TransferX = new MotionControl.MotorAxis((int)MotionControl.MotorSet.eMotorList.TRANSFER_X, axisName[0]);
            TransferY = new MotionControl.MotorAxis((int)MotionControl.MotorSet.eMotorList.TRANSFER_Y, axisName[1]);
            TransferZ = new MotionControl.MotorAxis((int)MotionControl.MotorSet.eMotorList.TRANSFER_Z, axisName[2]);

            MotorAxes = new MotionControl.MotorAxis[] { TransferX, TransferY, TransferZ };

            this.MachineName = this.GetType().Name;

            //transferThread = new FThread.TransferThread();
            //TransferX.ServoOn();
        }
        public bool GetLensGripState(bool bFlag)
        {
            int lModuleNo = 0;
            int lOffset = 0;

            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];
            if (bFlag)
            {
                uFlagHigh = upValue & (uint)DioDefine.DIO_IN_ADDR.IN_LENS_GRIP_FOR;
                if (uFlagHigh == 1)
                {
                    return true;
                }
            }
            else
            {
                uFlagHigh = upValue & (uint)DioDefine.DIO_IN_ADDR.IN_LENS_GRIP_BACK;
                if (uFlagHigh == 1)
                {
                    return true;
                }
            }
            return false;

        }
        public bool LensGripOn(bool bFlag, bool bWait = false)
        {
            int lModuleNo = 0;
            int lOffset = 0;
            uint uFlagHigh = 0;
            uint uFlagLow = 0;

            if (bFlag)
            {
                uFlagHigh = (uint)DioDefine.DIO_OUT_ADDR.LENS_GRIP_FOR;
                uFlagLow = (uint)DioDefine.DIO_OUT_ADDR.LENS_GRIP_BACK;
            }
            else
            {
                uFlagHigh = (uint)DioDefine.DIO_OUT_ADDR.LENS_GRIP_BACK;
                uFlagLow = (uint)DioDefine.DIO_OUT_ADDR.LENS_GRIP_FOR;
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
                        Rtn = GetLensGripState(bFlag);
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

        public override void MovingStop()
        {
            if (cts != null && !cts.IsCancellationRequested)
            {
                cts.Cancel();
            }
            TransferX.motorBreak = true;
            TransferY.motorBreak = true;
            TransferZ.motorBreak = true;

            TransferX.Stop();
            TransferY.Stop();
            TransferZ.Stop();
        }

        
        public async Task<bool> MoveFromAbsRel(MotionControl.MotorAxis motorAxis, double dRelPos)
        {
            if (motorAxis.isMotorBusy == true)
            {
                //Console.WriteLine("모터 작업이 이미 실행 중입니다. 기다려 주세요.");
                Globalo.LogPrint("ManualControl", $"모터 작업이 이미 실행 중입니다. 기다려 주세요.");
                return false;
            }
            cts?.Dispose();
            cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            int i = 0;

            double dPos = 0.0;

            dPos = dRelPos;
            if (dPos > 10.0)
            {
                dPos = 10.0;
            }

            if (dPos < -10.0)
            {
                dPos = -10.0;
            }

            bool bRtn = false;

            bool isSuccess = false;
            try
            {
                await Task.Run(() =>
                {

                    isSuccess = SingleAxisMove(motorAxis, dPos, AXT_MOTION_ABSREL.POS_REL_MODE, true);       //<--위치 확인 while 이 안에 넣어도 될듯

                    if (bRtn)
                    {
                        //while (bRtn)
                        //{
                        //    if (cts.Token.IsCancellationRequested)
                        //    {
                        //        //Console.WriteLine("취소 요청 감지됨");
                        //        Globalo.LogPrint("ManualControl", $"취소 요청 감지됨");
                        //        cts.Token.ThrowIfCancellationRequested(); // 예외 던지기 (catch로 감) 취소 요청 시 예외 발생
                        //    }
                        //    break;
                        //}
                        // 작업 정상 종료
                    }

                    Globalo.LogPrint("ManualControl", $"[TASK] TransFer_X_Move End");
                }, token);
            }
            catch (OperationCanceledException)
            {
                bRtn = false;
                //Console.WriteLine($"모터 작업이 취소되었습니다: {i}");
                Globalo.LogPrint("ManualControl", $"모터 작업이 취소되었습니다: {i}");
                isSuccess = false;
            }
            catch (Exception ex)
            {
                // 그 외 예외 처리
                //Console.WriteLine($"모터 이동 실패: {ex.Message}");
                Globalo.LogPrint("ManualControl", $"모터 이동 실패: {ex.Message}");
                isSuccess = false;
            }
            finally
            {
                // 리소스 정리
                cts?.Dispose();  // cts가 null이 아닐 때만 Dispose 호출
                ////cts = null;      // cts를 null로 설정하여 다음 작업에서 새로 생성할 수 있게
            }

            Globalo.LogPrint("ManualControl", $"[FUNCTION] MoveFromAbsRel End");
            return isSuccess;
        }
        public async Task<bool> TransFer_X_Move(int nPos, double offset)
        {
            //bool aaaaa = cts.Token.IsCancellationRequested;     //최초 false 이 속성은 취소 요청이 발생했는지 여부 Cancel()을 호출하거나,
            //Token.ThrowIfCancellationRequested()가 호출되면 true로 

            //bool acccc = cts.Token.CanBeCanceled;               //최초 true CancellationTokenSource**에서 취소가 가능한 상태인지
            //CancellationTokenSource가 Cancel()을 호출하거나 Dispose()가 호출되기 전까지 true 상태를 유지합니다.

            if (TransferX.isMotorBusy == true)
            {
                //Console.WriteLine("모터 작업이 이미 실행 중입니다. 기다려 주세요.");
                Globalo.LogPrint("ManualControl", $"모터 작업이 이미 실행 중입니다. 기다려 주세요.");
                return false;
            }
            cts?.Dispose();
            cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            double dPos = 0.0;
            int i = 0;
            dPos = Globalo.dataManage.teachingData.PcbTeachData[nPos].dPos[TransferX.m_lAxisNo] + offset;
            bool bRtn = false;

            bool isSuccess = false;
            try
            {
                await Task.Run(() =>
                {

                    isSuccess = SingleAxisMove(TransferX, dPos, AXT_MOTION_ABSREL.POS_ABS_MODE, true);       //<--위치 확인 while 이 안에 넣어도 될듯

                    if (bRtn)
                    {
                        //while (bRtn)
                        //{
                        //    if (cts.Token.IsCancellationRequested)
                        //    {
                        //        //Console.WriteLine("취소 요청 감지됨");
                        //        Globalo.LogPrint("ManualControl", $"취소 요청 감지됨");
                        //        cts.Token.ThrowIfCancellationRequested(); // 예외 던지기 (catch로 감) 취소 요청 시 예외 발생
                        //    }
                        //    break;
                        //}
                        // 작업 정상 종료
                    }
                    Globalo.LogPrint("ManualControl", $"[TASK] TransFer_X_Move End");
                }, token);
            }
            catch (OperationCanceledException)
            {
                bRtn = false;
                //Console.WriteLine($"모터 작업이 취소되었습니다: {i}");
                Globalo.LogPrint("ManualControl", $"모터 작업이 취소되었습니다: {i}");
                isSuccess = false;
                //MessageBox.Show("모터 작업이 취소되었습니다");
            }
            catch (Exception ex)
            {
                // 그 외 예외 처리
                //Console.WriteLine($"모터 이동 실패: {ex.Message}");
                Globalo.LogPrint("ManualControl", $"모터 이동 실패: {ex.Message}");
                isSuccess = false;
            }
            finally
            {
                // 리소스 정리
                cts?.Dispose();  // cts가 null이 아닐 때만 Dispose 호출
                ////cts = null;      // cts를 null로 설정하여 다음 작업에서 새로 생성할 수 있게
            }
 
            Globalo.LogPrint("ManualControl", $"[FUNCTION] TransFer_X_Move End");
            return isSuccess;
        }

        public void TransFer_XY_Move()
        {
            MotionControl.MotorAxis[] multiAxis = { TransferX, TransferY };

            double[] dMultiPos = { 0.0, 0.0 };

            bool bRtn = MultiAxisMove(multiAxis, dMultiPos);
            if (bRtn)
            {

            }
            else
            {

            }
        }
        public async Task MoveMotorAndWaitAsync()
        {
            // 취소 토큰 준비
            cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            int i = 0;
            try
            {
                await Task.Run(() =>
                {
                    while (true)
                    {
                        // 취소 요청 시 예외 발생
                        token.ThrowIfCancellationRequested();
                        i++;
                        Console.WriteLine($"현재 위치: {i}");

                        Thread.Sleep(300); // 너무 빠르게 읽지 않도록 약간 sleep

                        if (i > 10)
                        {
                            break;
                        }
                }
                }, token);
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("모터 작업이 취소되었습니다");
            }


            Console.WriteLine($"MoveMotorAndWaitAsync End 위치: {i}");
        }
        public void TransFer_XYZ_Move()
        {
            MotionControl.MotorAxis[] multiAxis = { TransferX, TransferY, TransferZ };
            double[] dMultiPos = { 0.0, 0.0 };

            bool bRtn = MultiAxisMove(multiAxis, dMultiPos);
            if (bRtn)
            {

            }
            else
            {

            }
        }


        public void SingleMoveToPosition(int position)
        {
            int i = 0;
            //GetAmpFault 확인

            //GetAmpEnable 확인


            for (i = 0; i < 3; i++)
            {
                /*
                    nAxis[0] = MOTOR_PCB_X;
	                nAxis[1] = MOTOR_PCB_Y;
	                nAxis[2] = MOTOR_PCB_TH;

	                dPos[0] = g_clModelData[nUnit].m_stTeachData[nPosi].dPos[MOTOR_PCB_X] + dOffsetX;
	                dPos[1] = g_clModelData[nUnit].m_stTeachData[nPosi].dPos[MOTOR_PCB_Y] + dOffsetY;
	                dPos[2] = g_clModelData[nUnit].m_stTeachData[nPosi].dPos[MOTOR_PCB_TH] + dOffsetTh;

                */
            }

            //this.MultiAxisMove(0);


            //if (this->MoveAxisMulti(nUnit, 3, nAxis, dPos) == false)

            //if (bWait == true)
            //      while(1)

            //duRetCode = CAXM.AxmMoveMultiPos((int)m_lMoveMultiAxesCount, m_lMoveMultiAxes, dMultiPos, dMultiVel, dMultiAcc, dMultiDec);
            //duRetCode = CAXM.AxmMoveStartMultiPos((int)m_lMoveMultiAxesCount, m_lMoveMultiAxes, dMultiPos, dMultiVel, dMultiAcc, dMultiDec);
        }


        public override void MoveToPosition(int position)
        {
            Console.WriteLine($"Transfer name : {TransferX.Name}");
            Console.WriteLine($"Transfer 이동축 {position} 위치로 이동");
        }
        
        public override bool IsMoving()
        {
            if(motorAutoThread.GetThreadRun() == true)
            {
                return true;
            }
            return false;
        }
        
        public override void OriginRun()
        {
            motorAutoThread.m_nCurrentStep = 1000;

            motorAutoThread.m_nStartStep = motorAutoThread.m_nCurrentStep;
            motorAutoThread.m_nEndStep = 2000;

            if (motorAutoThread.GetThreadRun() == true)
            {
                Console.WriteLine($"모터 동작 중입니다.");

                motorAutoThread.Stop();
                Thread.Sleep(300);
            }
            bool rtn = motorAutoThread.Start();
            if(rtn)
            {
                Console.WriteLine($"모터 동작 성공.");
            }
            else
            {
                Console.WriteLine($"모터 동작 실패.");
            }

        }
        public override void ReadyRun()
        {
            motorAutoThread.m_nCurrentStep = 2000;
            motorAutoThread.m_nStartStep = motorAutoThread.m_nCurrentStep;
            motorAutoThread.m_nEndStep = 3000;

            if (motorAutoThread.GetThreadRun() == true)
            {
                Console.WriteLine($"모터 동작 중입니다.");

                motorAutoThread.Stop();
                Thread.Sleep(300);
            }
            bool rtn = motorAutoThread.Start();
            if (rtn)
            {
                Console.WriteLine($"모터 동작 성공.");
            }
            else
            {
                Console.WriteLine($"모터 동작 실패.");
            }
        }
        public override void AutoRun()
        {
            motorAutoThread.m_nCurrentStep = 3000;
            motorAutoThread.m_nStartStep = motorAutoThread.m_nCurrentStep;
            motorAutoThread.m_nEndStep = 10000;

            if (motorAutoThread.GetThreadRun() == true)
            {
                Console.WriteLine($"모터 동작 중입니다.");

                motorAutoThread.Stop();
                Thread.Sleep(300);
            }
            bool rtn = motorAutoThread.Start();
            if (rtn)
            {
                Console.WriteLine($"모터 동작 성공.");
            }
            else
            {
                Console.WriteLine($"모터 동작 실패.");
            }
        }


        //if (g_clMotorSet.MovePcbZMotor(m_nUnit, WAIT_POS, 0.0, true) == false)
    }
}
