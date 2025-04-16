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

        public static double[] MOTOR_MAX_SPEED = { 100.0, 100.0, 100.0};
        public MotorDefine.eMotorType[] motorType = { MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR };
        public AXT_MOTION_LEVEL_MODE[] AXT_SET_LIMIT = { AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.HIGH, AXT_MOTION_LEVEL_MODE.LOW };
        public AXT_MOTION_LEVEL_MODE[] AXT_SET_SERVO_ALARM = { AXT_MOTION_LEVEL_MODE.HIGH, AXT_MOTION_LEVEL_MODE.HIGH, AXT_MOTION_LEVEL_MODE.LOW };

        public string processName = "tttt";

        
        //public Dio cylinder;
        //픽업 툴 4개 실린더 Dio 로 지정?

        public TransferMachine()//: base("Machine")
        {
            TransferX = new MotionControl.MotorAxis((int)MotionControl.MotorSet.eMotorList.TRANSFER_X, axisName[0], motorType[0], MOTOR_MAX_SPEED[0], AXT_SET_LIMIT[0], AXT_SET_SERVO_ALARM[0]);
            TransferY = new MotionControl.MotorAxis((int)MotionControl.MotorSet.eMotorList.TRANSFER_Y, axisName[1], motorType[1], MOTOR_MAX_SPEED[1], AXT_SET_LIMIT[1], AXT_SET_SERVO_ALARM[1]);
            TransferZ = new MotionControl.MotorAxis((int)MotionControl.MotorSet.eMotorList.TRANSFER_Z, axisName[2], motorType[2], MOTOR_MAX_SPEED[2], AXT_SET_LIMIT[2], AXT_SET_SERVO_ALARM[2]);

            MotorAxes = new MotionControl.MotorAxis[] { TransferX, TransferY, TransferZ };
            MotorCnt = MotorAxes.Length;

            TransferX.setMotorParameter(10.0, 0.1, 0.1, 1000.0);     //초기 셋 다른 곳에서 다시 해줘야될 듯
            TransferY.setMotorParameter(10.0, 0.1, 0.1, 1000.0);
            TransferZ.setMotorParameter(10.0, 0.1, 0.1, 1000.0);

            this.MachineName = this.GetType().Name;

        }
        public override void MotorDataSet()
        {
            int i = 0;
            for (i = 0; i < MotorAxes.Length; i++)
            {
                MotorAxes[i].setMotorParameter(
                Globalo.yamlManager.teachData.handler.TransferMachine.Speed[i],
                Globalo.yamlManager.teachData.handler.TransferMachine.Accel[i],
                Globalo.yamlManager.teachData.handler.TransferMachine.Decel[i],
                Globalo.yamlManager.teachData.handler.TransferMachine.Resolution[i]);
            }


        }
        public bool ChkXYMotorPos(Data.eTeachPosName teachingPos)
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }

            bool bRtn = false;

            double dXPos = 0.0;
            double dYPos = 0.0;
            double currentXPos = 0.0;
            double currentYPos = 0.0;


            dXPos = Globalo.yamlManager.teachData.handler.TransferMachine.Teaching[(int)teachingPos].Pos[0];
            dYPos = Globalo.yamlManager.teachData.handler.TransferMachine.Teaching[(int)teachingPos].Pos[1];
            

            currentXPos = TransferX.GetEncoderPos();
            currentYPos = TransferY.GetEncoderPos();

            if (dXPos == currentXPos && dYPos == currentYPos)
            {
                bRtn = true;
            }

            return bRtn;
        }
        public bool ChkZMotorPos(Data.eTeachPosName teachingPos)
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            bool bRtn = false;

            double dZPos = 0.0;
            double currentZPos = 0.0;


            dZPos = Globalo.yamlManager.teachData.handler.TransferMachine.Teaching[(int)teachingPos].Pos[2];


            currentZPos = TransferZ.GetEncoderPos();
            if (dZPos == currentZPos)
            {
                bRtn = true;
            }

            return bRtn;
        }
        public bool GetLensGripState(bool bFlag)
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
        public bool GetLoadVacuumState(int index, bool bFlag)
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
                uFlagHigh = upValue & (uint)DioDefine.DIO_IN_ADDR.IN_VACUUM_ON;
                if (uFlagHigh == 1)
                {
                    return true;
                }
            }
            else
            {
                uFlagHigh = upValue & (uint)DioDefine.DIO_IN_ADDR.IN_VACUUM_ON;
                if (uFlagHigh == 0)
                {
                    return true;
                }
            }
            return false;
        }
        public bool GetUnLoadVacuumState(int index, bool bFlag)
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
                uFlagHigh = upValue & (uint)DioDefine.DIO_IN_ADDR.IN_VACUUM_ON;
                if (uFlagHigh == 1)
                {
                    return true;
                }
            }
            else
            {
                uFlagHigh = upValue & (uint)DioDefine.DIO_IN_ADDR.IN_VACUUM_ON;
                if (uFlagHigh == 0)
                {
                    return true;
                }
            }
            return false;
        }
        
        public bool LoadVacuumOn(int index, bool bFlag, bool bWait = false)
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            int lModuleNo = 0;
            int lOffset = 0;
            uint uFlagHigh = 0;
            uint uFlagLow = 0;

            if (bFlag)
            {
                uFlagHigh = (uint)DioDefine.DIO_OUT_ADDR.VACUUM_ON;
                uFlagLow = (uint)DioDefine.DIO_OUT_ADDR.VACUUM_OFF;
            }
            else
            {
                uFlagHigh = (uint)DioDefine.DIO_OUT_ADDR.VACUUM_OFF;
                uFlagLow = (uint)DioDefine.DIO_OUT_ADDR.VACUUM_ON;
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
                        Rtn = GetLoadVacuumState(index, bFlag);
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
        public bool UnLoadVacuumOn(int index, bool bFlag, bool bWait = false)
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            int lModuleNo = 0;
            int lOffset = 0;
            uint uFlagHigh = 0;
            uint uFlagLow = 0;

            if (bFlag)
            {
                uFlagHigh = (uint)DioDefine.DIO_OUT_ADDR.VACUUM_ON;
                uFlagLow = (uint)DioDefine.DIO_OUT_ADDR.VACUUM_OFF;
            }
            else
            {
                uFlagHigh = (uint)DioDefine.DIO_OUT_ADDR.VACUUM_OFF;
                uFlagLow = (uint)DioDefine.DIO_OUT_ADDR.VACUUM_ON;
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
                        Rtn = GetUnLoadVacuumState(index, bFlag);
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
        public bool LensGripOn(int index, bool bFlag, bool bWait = false)
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
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
            TransferX.MotorBreak = true;
            TransferY.MotorBreak = true;
            TransferZ.MotorBreak = true;

            TransferX.Stop();
            TransferY.Stop();
            TransferZ.Stop();
        }

        
        public async Task<bool> MoveFromAbsRel(MotionControl.MotorAxis motorAxis, double dRelPos)
        {
            if (motorAxis.IsMotorBusy == true)
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

                    Globalo.LogPrint("ManualControl", $"[TASK] TransFer X Move End");
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

        //public async Task<bool> TransFer_X_Move(int nPos, double offset)

        public bool TransFer_X_Move(Data.eTeachPosName teachingPos)//int nPos, double offset)
        {
            if (TransferX.IsMotorBusy == true)
            {
                Globalo.LogPrint("ManualControl", $"모터 작업이 이미 실행 중입니다. 기다려 주세요.");
                return false;
            }
            double dPos = 0.0;
           // dPos = Globalo.dataManage.teachingData.PcbTeachData[nPos].dPos[TransferX.m_lAxisNo] + offset;

            bool isSuccess = false;
            try
            {
                isSuccess = SingleAxisMove(TransferX, dPos, AXT_MOTION_ABSREL.POS_ABS_MODE, true);       //<--위치 확인 while 이 안에 넣어도 될듯
                
            }
            catch (Exception ex)
            {
                Globalo.LogPrint("ManualControl", $"모터 이동 실패: {ex.Message}");
                isSuccess = false;
            }
            finally
            {
            }
            Globalo.LogPrint("ManualControl", $"[TRANSFER] X AXIS Move End");

            return isSuccess;
        }

        public bool TransFer_XY_Move(Data.eTeachPosName ePos, bool bWait = false)
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }

            MotionControl.MotorAxis[] multiAxis = { TransferX, TransferY };
            string logStr = "";
            double[] dMultiPos = { 0.0, 0.0 };
            bool bRtn = false;


            bRtn = TransFer_Z_Move(Data.eTeachPosName.WAIT_POS, true);  //TODO:  ??

            if (bRtn == false)
            {
                //Z 축 대기 위치 이동 실패
                logStr = $"Transfer Z축 대기위치 이동 실패";
                Globalo.LogPrint("ManualControl", logStr);
                return false;
            }

            dMultiPos[0] = Globalo.yamlManager.teachData.handler.TransferMachine.Teaching[(int)ePos].Pos[0];     //x Axis
            dMultiPos[1] = Globalo.yamlManager.teachData.handler.TransferMachine.Teaching[(int)ePos].Pos[1];      //y Axis


            bRtn = MultiAxisMove(multiAxis, dMultiPos);


            if (bRtn == false)
            {
                logStr = $"Transfer XY축 {Data.eTeachPosName.WAIT_POS.ToString() } 이동 실패";

                Globalo.LogPrint("ManualControl", logStr);
            }

            bool isSuccess = false;


            if (bWait)
            {
                //이동 위치 확인 
                int step = 100;
                int nTimeTick = 0;

                while (bWait)
                {
                    if (multiAxis[0].MotorBreak) break;
                    if (multiAxis[1].MotorBreak) break;
                    if (multiAxis[2].MotorBreak) break;
                    //위치 도착 확인 , 정지 확인

                    switch (step)
                    {
                        case 100:
                            if (multiAxis[0].GetStopAxis() == true)
                            {
                                Console.WriteLine($"{multiAxis[0].Name } Axis Stop Check");
                                step = 150;
                            }
                            nTimeTick = Environment.TickCount;
                            break;
                        case 150:
                            if (multiAxis[1].GetStopAxis() == true)
                            {
                                Console.WriteLine($"{multiAxis[1].Name } Axis Stop Check");
                                step = 200;
                            }
                            nTimeTick = Environment.TickCount;
                            break;
                        case 200:
                            if ((multiAxis[0].GetEncoderPos() - dMultiPos[0]) < MotionControl.MotorSet.ENCORDER_GAP)
                            {
                                isSuccess = true;
                                Console.WriteLine($"{multiAxis[0].Name } Axis {ePos.ToString()} Move Check");
                                step = 250;
                            }
                            break;
                        case 250:
                            if ((multiAxis[1].GetEncoderPos() - dMultiPos[1]) < MotionControl.MotorSet.ENCORDER_GAP)
                            {
                                isSuccess = true;
                                Console.WriteLine($"{multiAxis[1].Name } Axis {ePos.ToString()} Move Check");
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
                        if (step == 100)        //정지 실패
                        {
                            Console.WriteLine($"{multiAxis[0].Name } Axis Stop Timeout");
                        }
                        else if (step == 150)      //정지 실패
                        {
                            Console.WriteLine($"{multiAxis[1].Name } Axis Stop Timeout");
                        }
                        else if (step == 200)   //위치 이동 실패
                        {
                            Console.WriteLine($"{multiAxis[0].Name } Axis {ePos.ToString()} Move Timeout");
                        }
                        else if (step == 250)   //위치 이동 실패
                        {
                            Console.WriteLine($"{multiAxis[1].Name } Axis {ePos.ToString()} Move Timeout");
                        }
                        isSuccess = false;
                        break;
                    }
                    Thread.Sleep(10);
                }
            }
            return isSuccess;
        }
        public bool TransFer_Z_Move(Data.eTeachPosName ePos, bool bWait = false)
        {
            string logStr = "";
            double dPos = Globalo.yamlManager.teachData.handler.TransferMachine.Teaching[(int)ePos].Pos[2];     //z Axis

            bool bRtn = SingleAxisMove(TransferZ, dPos, AXT_MOTION_ABSREL.POS_ABS_MODE);

            if (bRtn == false)
            {
                logStr = $"Transfer Z axis {ePos.ToString() } 이동 실패";
                return false;
            }

            bool isSuccess = false;
            if (bWait)
            {
                //이동 위치 확인 
                int step = 100;
                int nTimeTick = 0;

                while (bWait)
                {
                    if (TransferZ.MotorBreak) break;
                    //위치 도착 확인 , 정지 확인

                    switch (step)
                    {
                        case 100:
                            if (TransferZ.GetStopAxis() == true)
                            {
                                Console.WriteLine($"{TransferZ.Name } Axis Stop Check");
                                step = 200;
                            }
                            nTimeTick = Environment.TickCount;
                            break;
                        case 200:
                            if ((TransferZ.GetEncoderPos() - dPos) < MotionControl.MotorSet.ENCORDER_GAP)
                            {
                                isSuccess = true;
                                Console.WriteLine($"{TransferZ.Name } Axis {ePos.ToString()} Move Check");
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
                        if (step == 100)        //정지 실패
                        {
                            Console.WriteLine($"{TransferZ.Name } Axis Stop Timeout");
                        }
                        else if (step == 200)   //위치 이동 실패
                        {
                            Console.WriteLine($"{TransferZ.Name } Axis {ePos.ToString()} Move Timeout");
                        }
                        isSuccess = false;
                        break;
                    }
                    Thread.Sleep(10);
                }
            }
            
            return isSuccess;
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
