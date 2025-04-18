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

        public MotionControl.MotorAxis TransferX;
        public MotionControl.MotorAxis TransferY;
        public MotionControl.MotorAxis TransferZ;

        public MotionControl.MotorAxis[] MotorAxes; // 배열 선언


        public string[] axisName = { "TransferX", "TransferY", "TransferZ" };
        private static double[] MOTOR_MAX_SPEED = { 200.0, 500.0, 50.0};
        private MotorDefine.eMotorType[] motorType = { MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR };
        private AXT_MOTION_LEVEL_MODE[] AXT_SET_LIMIT = { AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.HIGH, AXT_MOTION_LEVEL_MODE.LOW };
        private AXT_MOTION_LEVEL_MODE[] AXT_SET_SERVO_ALARM = { AXT_MOTION_LEVEL_MODE.HIGH, AXT_MOTION_LEVEL_MODE.HIGH, AXT_MOTION_LEVEL_MODE.LOW };

        private static AXT_MOTION_HOME_DETECT[] MOTOR_HOME_SENSOR = {AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor};

        private static AXT_MOTION_MOVE_DIR[] MOTOR_HOME_DIR = {AXT_MOTION_MOVE_DIR.DIR_CCW, AXT_MOTION_MOVE_DIR.DIR_CCW, AXT_MOTION_MOVE_DIR.DIR_CW};

        private double[] OrgFirstVel = { 20000.0, 20000.0, 20000.0 };
        private double[] OrgSecondVel = { 10000.0, 10000.0, 5000.0 };
        private double[] OrgThirdVel = { 5000.0, 5000.0, 2500.0 };

        public enum eTeachingPosList : int
        {
            WAIT_POS = 0,
            LEFT_TRAY_LOAD_POS, LEFT_TRAY_UNLOAD_POS,
            RIGHT_TRAY_LOAD_POS, RIGHT_TRAY_UNLOAD_POS,
            SOCKET_A1, SOCKET_A2, SOCKET_B1, SOCKET_B2, SOCKET_C1, SOCKET_C2, SOCKET_D1, SOCKET_D2,
            TOTAL_TRANSFER_TEACHING_COUNT
        };
        public string[] TeachName = { "WAIT_POS",
            "L_TRAY_LOAD_POS", "L_TRAY_UNLOAD_POS",
            "R_TRAY_LOAD_POS", "R_TRAY_UNLOAD_POS",
            "SOCKET_A1", "SOCKET_A2", "SOCKET_B1", "SOCKET_B2","SOCKET_C1", "SOCKET_C2", "SOCKET_D1", "SOCKET_D2" };

        public string teachingPath = "Teach_Transfer.yaml";
        public string taskPath = "Task_Transfer.yaml";
        public Data.TeachingConfig teachingConfig = new Data.TeachingConfig();
        public PickedProduct pickedProduct = new PickedProduct();

        //TODO:  픽업 상태 로드 4개 , 배출 4개 / blank , LOAD , BCR OK , PASS , NG(DEFECT 1 , 2 , 3 , 4)
        //public Dio cylinder;
        //픽업 툴 4개 실린더 Dio 로 지정?

        public TransferMachine()//: base("Machine")
        {
            this.RunState = OperationState.Stopped;
            this.MachineName = this.GetType().Name;

            TransferX = new MotionControl.MotorAxis((int)MotionControl.MotorSet.eTransferMotorList.TRANSFER_X, 
                axisName[0], motorType[0], MOTOR_MAX_SPEED[0], AXT_SET_LIMIT[0], AXT_SET_SERVO_ALARM[0], OrgFirstVel[0], OrgSecondVel[0], OrgThirdVel[0],
                MOTOR_HOME_SENSOR[0], MOTOR_HOME_DIR[0]);
            ////
            TransferY = new MotionControl.MotorAxis((int)MotionControl.MotorSet.eTransferMotorList.TRANSFER_Y, 
                axisName[1], motorType[1], MOTOR_MAX_SPEED[1], AXT_SET_LIMIT[1], AXT_SET_SERVO_ALARM[1], OrgFirstVel[1], OrgSecondVel[1], OrgThirdVel[1],
                MOTOR_HOME_SENSOR[1], MOTOR_HOME_DIR[1]);
            ////
            TransferZ = new MotionControl.MotorAxis((int)MotionControl.MotorSet.eTransferMotorList.TRANSFER_Z, 
                axisName[2], motorType[2], MOTOR_MAX_SPEED[2], AXT_SET_LIMIT[2], AXT_SET_SERVO_ALARM[2], OrgFirstVel[2], OrgSecondVel[2], OrgThirdVel[2],
                MOTOR_HOME_SENSOR[2], MOTOR_HOME_DIR[2]);

            MotorAxes = new MotionControl.MotorAxis[] { TransferX, TransferY, TransferZ };
            MotorCnt = MotorAxes.Length;

            TransferX.setMotorParameter(10.0, 0.1, 0.1, 1000.0);     //초기 셋 다른 곳에서 다시 해줘야될 듯
            TransferY.setMotorParameter(10.0, 0.1, 0.1, 1000.0);
            TransferZ.setMotorParameter(10.0, 0.1, 0.1, 1000.0);


            for (int i = 0; i < 4; i++)
            {
                pickedProduct.LoadProductInfo.Add(new ProductInfo(i));
                pickedProduct.UnLoadProductInfo.Add(new ProductInfo(i));
            }
            pickedProduct = Data.TaskDataYaml.TaskLoad_Transfer(taskPath);

            //
        }

        public override bool TaskSave()
        {
            bool rtn = Data.TaskDataYaml.TaskSave_Transfer(pickedProduct, taskPath);
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
                teachingConfig.Teaching[i].Name = TeachName[i];
            }
   

        }
        public bool ChkXYMotorPos(eTeachingPosList teachingPos)
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


            dXPos = Globalo.motionManager.transferMachine.teachingConfig.Teaching[(int)teachingPos].Pos[0];
            dYPos = Globalo.motionManager.transferMachine.teachingConfig.Teaching[(int)teachingPos].Pos[1];
            

            currentXPos = TransferX.GetEncoderPos();
            currentYPos = TransferY.GetEncoderPos();

            if (dXPos == currentXPos && dYPos == currentYPos)
            {
                bRtn = true;
            }

            return bRtn;
        }
        public bool ChkZMotorPos(eTeachingPosList teachingPos)
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            bool bRtn = false;

            double dZPos = 0.0;
            double currentZPos = 0.0;


            dZPos = Globalo.motionManager.transferMachine.teachingConfig.Teaching[(int)teachingPos].Pos[2];


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
            int lModuleNo = 1;
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

            if (bFlag == false)
            {
                Thread.Sleep(300);
                //off 일때 파기를 꺼줘야된다.
                uFlagLow = (uint)DioDefine.DIO_OUT_ADDR.VACUUM_OFF;
                Globalo.motionManager.ioController.DioWriteOutportByte(lModuleNo, lOffset, uFlagLow, false);
                
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

        public bool TransFer_X_Move(eTeachingPosList teachingPos)//int nPos, double offset)
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

        public bool TransFer_XY_Move(eTeachingPosList ePos, bool bWait = false)
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }

            MotionControl.MotorAxis[] multiAxis = { TransferX, TransferY };
            string logStr = "";
            double[] dMultiPos = { 0.0, 0.0 };
            bool bRtn = false;


            bRtn = TransFer_Z_Move(eTeachingPosList.WAIT_POS, true);  //TODO:  ??

            if (bRtn == false)
            {
                //Z 축 대기 위치 이동 실패
                logStr = $"Transfer Z축 대기위치 이동 실패";
                Globalo.LogPrint("ManualControl", logStr);
                return false;
            }

            dMultiPos[0] = Globalo.motionManager.transferMachine.teachingConfig.Teaching[(int)ePos].Pos[0];     //x Axis
            dMultiPos[1] = Globalo.motionManager.transferMachine.teachingConfig.Teaching[(int)ePos].Pos[1];      //y Axis


            bRtn = MultiAxisMove(multiAxis, dMultiPos);


            if (bRtn == false)
            {
                logStr = $"Transfer XY축 {eTeachingPosList.WAIT_POS.ToString() } 이동 실패";

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
        public bool TransFer_Z_Move(eTeachingPosList ePos, bool bWait = false)
        {
            string logStr = "";
            double dPos = Globalo.motionManager.transferMachine.teachingConfig.Teaching[(int)ePos].Pos[2];     //z Axis

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

        public void SingleMoveToPosition(int position)
        {
            int i = 0;
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
        public override void StopAuto()
        {
            motorAutoThread.Stop();
            MovingStop();

            Console.WriteLine($"[ORIGIN] Transfer Run Stop");

        }
        public override bool OriginRun()
        {
            if (motorAutoThread.GetThreadRun() == true)
            {
                //motorAutoThread.Stop();
                return false;
            }


            bool bServoOnChk = true;
            int length = MotorAxes.Length;
            string szLog = "";
            
            for (int i = 0; i < length; i++)
            {
                if(MotorAxes[i].AmpEnable() == false)
                {
                    bServoOnChk = false;
                    szLog = $"[ORIGIN] {MotorAxes[i].Name} AmpEnable Fail]";
                    Globalo.LogPrint("ManualControl", szLog);
                    return false;
                }
            }
            if(bServoOnChk == false)
            {

                return false;
            }
            this.RunState = OperationState.Originning;
            motorAutoThread.m_nCurrentStep = 1000;          //ORG
            motorAutoThread.m_nEndStep = 2000;

            motorAutoThread.m_nStartStep = motorAutoThread.m_nCurrentStep;

            bool rtn = motorAutoThread.Start();
            if(rtn)
            {

                Console.WriteLine($"[ORIGIN] Transfer Origin Start");
            }
            else
            {
                this.RunState = OperationState.Stopped;
                Console.WriteLine($"[ORIGIN] Transfer Origin Start Fail");
            }
            return rtn;
        }
        public override bool ReadyRun()
        {
            if (motorAutoThread.GetThreadRun() == true)
            {
                return false;
            }

            if (TransferX.OrgState == false || TransferY.OrgState == false || TransferZ.OrgState == false)
            {
                this.RunState = OperationState.Originning;
                motorAutoThread.m_nCurrentStep = 1000;
            }
            else
            {
                this.RunState = OperationState.Preparing;
                motorAutoThread.m_nCurrentStep = 2000;
            }

            motorAutoThread.m_nEndStep = 3000;
            motorAutoThread.m_nStartStep = motorAutoThread.m_nCurrentStep;

            if (motorAutoThread.GetThreadRun() == true)
            {
                Console.WriteLine($"모터 동작 중입니다.");
                return true;
            }
            bool rtn = motorAutoThread.Start();
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
            if (motorAutoThread.GetThreadRun() == true)
            {
                motorAutoThread.Pause();
            }
        }
        public override bool AutoRun()
        {
            bool rtn = true;
            if (this.RunState != OperationState.PreparationComplete)
            {
                Globalo.LogPrint("MainForm", "[TRANSFER] 운전준비가 완료되지 않았습니다.", Globalo.eMessageName.M_WARNING);
                return false;
            }
            if (motorAutoThread.GetThreadRun() == true)
            {
                Console.WriteLine($"모터 동작 중입니다.");
                if (motorAutoThread.GetThreadPause() == true)
                {
                    motorAutoThread.m_nCurrentStep = Math.Abs(motorAutoThread.m_nCurrentStep);
                }
                else
                {
                    rtn = false;
                }
            }
            else
            {
                motorAutoThread.m_nCurrentStep = 3000;
                motorAutoThread.m_nEndStep = 10000;
                motorAutoThread.m_nStartStep = motorAutoThread.m_nCurrentStep;

                rtn = motorAutoThread.Start();

                if (rtn)
                {
                    Console.WriteLine($"모터 동작 성공.");
                }
                else
                {
                    Console.WriteLine($"모터 동작 실패.");
                }
            }


           

            return rtn;
        }
    }
}
