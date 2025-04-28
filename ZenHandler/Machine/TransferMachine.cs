using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZenHandler.Machine
{
    public enum eTransfer : int
    {
        TRANSFER_X = 0, TRANSFER_Y, TRANSFER_Z
    };
    
    public class TransferMachine : MotionControl.MotorController
    {
        public event Action<MotionControl.MotorSet.TrayPosition> OnTrayChangedCall;
        public MotionControl.MotorSet.TrayPosition Position;// { get; }
        public int MotorCnt { get; private set; } = 3;

        public MotionControl.MotorAxis TransferX;
        public MotionControl.MotorAxis TransferY;
        public MotionControl.MotorAxis TransferZ;

        public MotionControl.MotorAxis[] MotorAxes; // 배열 선언
        public string[] axisName = { "TransferX", "TransferY", "TransferZ" };
        
        private MotorDefine.eMotorType[] motorType = { MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR };
        private AXT_MOTION_LEVEL_MODE[] AXT_SET_LIMIT = { AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.HIGH, AXT_MOTION_LEVEL_MODE.LOW };
        private AXT_MOTION_LEVEL_MODE[] AXT_SET_SERVO_ALARM = { AXT_MOTION_LEVEL_MODE.HIGH, AXT_MOTION_LEVEL_MODE.HIGH, AXT_MOTION_LEVEL_MODE.LOW };
        private AXT_MOTION_HOME_DETECT[] MOTOR_HOME_SENSOR = {AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor};
        private AXT_MOTION_MOVE_DIR[] MOTOR_HOME_DIR = {AXT_MOTION_MOVE_DIR.DIR_CCW, AXT_MOTION_MOVE_DIR.DIR_CCW, AXT_MOTION_MOVE_DIR.DIR_CW};

        private static double[] MaxSpeeds = { 200.0, 500.0, 50.0 };
        private double[] OrgFirstVel = { 20000.0, 20000.0, 20000.0 };   //수치 조심
        private double[] OrgSecondVel = { 10000.0, 10000.0, 5000.0 };
        private double[] OrgThirdVel = { 5000.0, 5000.0, 2500.0 };

        //대기위치 1개
        //티칭위치 피커별로 로드 Tray x1: y:1 위치 하나씩 4개 * 리프트 2개 = 8개
        //티칭위치 피커별로 배출 Tray x1: y:1 위치 하나씩 4개 * 리프트 2개 = 8개
        //
        //소켓
        //eeprom - 4개 2세트
        //aoi - 2개 2세트
        //fw - 4개 4세트
        //
        //
        //NG tray (가로4개 2Set)  피커 4개 * 2Set
        //
        //대기위치 1개
        //LEFT TRAY 로드 위치 1개
        //LEFT TRAY 배출 위치 1개
        //RIGHT TRAY 로드 위치 1개
        //RIGHT TRAY 배출 위치 1개
        //소켓 n개의 세트
        //eep - 소켓 1 투입 / 배출 2개
        //eep - 소켓 2 투입 / 배출 2개
        //aoi - 소켓 1 투입 / 배출 2개
        //aoi - 소켓 2 투입 / 배출 2개
        //fw - 소켓 8개 투입 4 + 배출 4
        //
        //Ng 2개

        //tray 간 x축, y축 Offset
        //Socket 간 x축, y축 Offset

        //모터 이동 방식
        //최종 이동 위치 = 피커 1의 저장된 티칭 위치 + Try x/y Gap + Picker 별 Offset x/y;
        public enum eTeachingPosList : int
        {
            WAIT_POS = 0,
            LEFT_TRAY_BCR_POS, RIGHT_TRAY_BCR_POS,
            LEFT_TRAY_LOAD_POS, LEFT_TRAY_UNLOAD_POS, 
            RIGHT_TRAY_LOAD_POS, RIGHT_TRAY_UNLOAD_POS,
            SOCKET_A_LOAD, SOCKET_A_UNLOAD, SOCKET_B_LOAD, SOCKET_B_UNLOAD, SOCKET_C_LOAD, SOCKET_C_UNLOAD, SOCKET_D_LOAD, SOCKET_D_UNLOAD,
            NG_A_LOAD, NG_A_UNLOAD, NG_B_LOAD, NG_B_UNLOAD,
            TOTAL_TRANSFER_TEACHING_COUNT};

        public string[] TeachName = { 
            "WAIT_POS",
            "LEFT_TRAY_BCR_POS", "RIGHT_TRAY_BCR_POS",
            "L_TRAY_LOAD_POS", "L_TRAY_UNLOAD_POS",
            "R_TRAY_LOAD_POS", "R_TRAY_UNLOAD_POS",
            "SOCKET_A_LOAD", "SOCKET_A_UNLOAD", "SOCKET_B_LOAD", "SOCKET_B_UNLOAD","SOCKET_C_LOAD", "SOCKET_C_UNLOAD", "SOCKET_D_LOAD", "SOCKET_D_UNLOAD",
            "NG_A_UNLOAD", "NG_B_UNLOAD", "NG_C_UNLOAD", "NG_D_UNLOAD"};


        public const string teachingPath = "Teach_Transfer.yaml";
        public const string taskPath = "Task_Transfer.yaml";
        public const string LayoutPath = "Task_Product_Layout.yaml";

        public Data.TeachingConfig teachingConfig = new Data.TeachingConfig();
        public PickedProduct pickedProduct = new PickedProduct();
        public ProductLayout productLayout = new ProductLayout();

        //TODO:  픽업 상태 로드 4개 , 배출 4개 / blank , LOAD , BCR OK , PASS , NG(DEFECT 1 , 2 , 3 , 4)
        //public Dio cylinder;
        //픽업 툴 4개 실린더 Dio 로 지정?

        public TransferMachine()//: base("Machine")
        {
            int i = 0;
            this.RunState = OperationState.Stopped;
            this.MachineName = this.GetType().Name;
            MotorAxes = new MotionControl.MotorAxis[] { TransferX, TransferY, TransferZ };
            MotorCnt = MotorAxes.Length;


            for (i = 0; i < MotorCnt; i++)
            {
                int index = (int)MotionControl.MotorSet.ValidTransferMotors[i];
                MotorAxes[i] = new MotionControl.MotorAxis(index,
                axisName[i], motorType[i], MaxSpeeds[i], AXT_SET_LIMIT[i], AXT_SET_SERVO_ALARM[i], OrgFirstVel[i], OrgSecondVel[i], OrgThirdVel[i],
                MOTOR_HOME_SENSOR[i], MOTOR_HOME_DIR[i]);

                //초기 셋 다른 곳에서 다시 해줘야될 듯
                MotorAxes[i].setMotorParameter(10.0, 0.1, 0.1, 1000.0);//(double vel , double acc , double dec , double resol)
            }


            for (i = 0; i < 4; i++)
            {
                pickedProduct.LoadProductInfo.Add(new ProductInfo(i));
                pickedProduct.UnLoadProductInfo.Add(new ProductInfo(i));
            }

            pickedProduct = Data.TaskDataYaml.TaskLoad_Transfer(taskPath);
            productLayout = Data.TaskDataYaml.TaskLoad_Layout(LayoutPath);


            Position = MotionControl.MotorSet.TrayPosition.Left;
            //
        }

        public override bool TaskSave()     //Picket 상태 저장
        {
            bool rtn = Data.TaskDataYaml.TaskSave_Transfer(pickedProduct, taskPath);
            return rtn;
        }
        public override void MotorDataSet() //모터 설정 저장
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
        private void CheckTrayState()
        {
            //State = TransferUnitState.TrayEmpty;
            //OnTrayChangedCall?.Invoke(Position); // 어떤 트레이 비었는지 전달
        }
        public bool SetPicker(UnitPicker Picker, PickedProductState State , int index)
        {
            if (Picker == UnitPicker.LOAD)
            {
                pickedProduct.LoadProductInfo[index].State = State;
            }
            else
            {
                pickedProduct.UnLoadProductInfo[index].State = State;
            }
            Data.TaskDataYaml.TaskSave_Transfer(pickedProduct, taskPath);
            return true;
        }

        public PickedProductState GetPickerState(UnitPicker Picker, int index)
        {
            PickedProductState myState;
            if (Picker == UnitPicker.LOAD)
            {
                myState = pickedProduct.LoadProductInfo[index].State;
            }
            else
            {
                myState = pickedProduct.UnLoadProductInfo[index].State;
            }

            return myState;
        }
        public bool ChkXYMotorPos(eTeachingPosList teachingPos)
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }

            double dXPos = 0.0;
            double dYPos = 0.0;
            double currentXPos = 0.0;
            double currentYPos = 0.0;


            dXPos = Globalo.motionManager.transferMachine.teachingConfig.Teaching[(int)teachingPos].Pos[(int)eTransfer.TRANSFER_X];
            dYPos = Globalo.motionManager.transferMachine.teachingConfig.Teaching[(int)teachingPos].Pos[(int)eTransfer.TRANSFER_Y];
            

            currentXPos = TransferX.EncoderPos;
            currentYPos = TransferY.EncoderPos;

            if (dXPos == currentXPos && dYPos == currentYPos)
            {
                return true;
            }

            return false;
        }
        public bool ChkZMotorPos(eTeachingPosList teachingPos)
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            double dZTeachingPos = 0.0;
            double currentZPos = 0.0;


            dZTeachingPos = Globalo.motionManager.transferMachine.teachingConfig.Teaching[(int)teachingPos].Pos[(int)eTransfer.TRANSFER_Z];
            currentZPos = TransferZ.EncoderPos;
            
            if (dZTeachingPos == currentZPos)
            {
                return true;
            }

            return false;
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
                uFlagHigh = upValue & (uint)DioDefine.DIO_IN_ADDR_CH0.IN_LENS_GRIP_FOR;
                if (uFlagHigh == 1)
                {
                    return true;
                }
            }
            else
            {
                uFlagHigh = upValue & (uint)DioDefine.DIO_IN_ADDR_CH0.IN_LENS_GRIP_BACK;
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
                uFlagHigh = upValue & (uint)DioDefine.DIO_IN_ADDR_CH0.IN_VACUUM_ON;
                if (uFlagHigh == 1)
                {
                    return true;
                }
            }
            else
            {
                uFlagHigh = upValue & (uint)DioDefine.DIO_IN_ADDR_CH0.IN_VACUUM_ON;
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
                uFlagHigh = upValue & (uint)DioDefine.DIO_IN_ADDR_CH0.IN_VACUUM_ON;
                if (uFlagHigh == 1)
                {
                    return true;
                }
            }
            else
            {
                uFlagHigh = upValue & (uint)DioDefine.DIO_IN_ADDR_CH0.IN_VACUUM_ON;
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
                uFlagHigh = (uint)DioDefine.DIO_OUT_ADDR_CH0.VACUUM_ON;
                uFlagLow = (uint)DioDefine.DIO_OUT_ADDR_CH0.VACUUM_OFF;
            }
            else
            {
                uFlagHigh = (uint)DioDefine.DIO_OUT_ADDR_CH0.VACUUM_OFF;
                uFlagLow = (uint)DioDefine.DIO_OUT_ADDR_CH0.VACUUM_ON;
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
                uFlagLow = (uint)DioDefine.DIO_OUT_ADDR_CH0.VACUUM_OFF;
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
                uFlagHigh = (uint)DioDefine.DIO_OUT_ADDR_CH0.VACUUM_ON;
                uFlagLow = (uint)DioDefine.DIO_OUT_ADDR_CH0.VACUUM_OFF;
            }
            else
            {
                uFlagHigh = (uint)DioDefine.DIO_OUT_ADDR_CH0.VACUUM_OFF;
                uFlagLow = (uint)DioDefine.DIO_OUT_ADDR_CH0.VACUUM_ON;
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
                uFlagHigh = (uint)DioDefine.DIO_OUT_ADDR_CH0.LENS_GRIP_FOR;
                uFlagLow = (uint)DioDefine.DIO_OUT_ADDR_CH0.LENS_GRIP_BACK;
            }
            else
            {
                uFlagHigh = (uint)DioDefine.DIO_OUT_ADDR_CH0.LENS_GRIP_BACK;
                uFlagLow = (uint)DioDefine.DIO_OUT_ADDR_CH0.LENS_GRIP_FOR;
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

        public bool TransFer_X_Move(eTeachingPosList ePos, bool bWait = true)
        {
            if (TransferX.IsMotorBusy == true)
            {
                Globalo.LogPrint("ManualControl", $"모터 작업이 이미 실행 중입니다. 기다려 주세요.");
                return false;
            }
            double dPos = Globalo.motionManager.transferMachine.teachingConfig.Teaching[(int)ePos].Pos[(int)eTransfer.TRANSFER_X];

            bool isSuccess = true;
            try
            {
                isSuccess = TransferX.MoveAxis(dPos, AXT_MOTION_ABSREL.POS_ABS_MODE, bWait);

            }
            catch (Exception ex)
            {
                Globalo.LogPrint("ManualControl", $"TransFer_X_Move Exception: {ex.Message}");
                isSuccess = false;
            }
            finally
            {
            }
            Globalo.LogPrint("ManualControl", $"[TRANSFER] X AXIS Move End");

            return isSuccess;
        }
        public bool TransFer_Z_Move(eTeachingPosList ePos, bool bWait = true)
        {
            bool isSuccess = true;
            string logStr = "";
            double dPos = Globalo.motionManager.transferMachine.teachingConfig.Teaching[(int)ePos].Pos[(int)eTransfer.TRANSFER_Z];     //z Axis
            try
            {
                isSuccess = TransferZ.MoveAxis(dPos, AXT_MOTION_ABSREL.POS_ABS_MODE, bWait);
            }
            catch (Exception ex)
            {
                Globalo.LogPrint("ManualControl", $"TransFer_Z_Move Exception: {ex.Message}");
                isSuccess = false;
            }
            
            
            if (isSuccess == false)
            {
                logStr = $"Transfer Z axis {ePos.ToString() } 이동 실패";
            }

            return isSuccess;
        }
        public bool TransFer_XY_Move(eTeachingPosList ePos, int PickerNo = 0, int CountX = 0, int CountY = 0,  bool bWait = true)  //Picket Index , Tray or Socekt or Ng , 
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }

            string logStr = "";
            MotionControl.MotorAxis[] multiAxis = { TransferX, TransferY };
            double[] dMultiPos = { 0.0, 0.0 };
            double[] dOffsetPos = { 0.0, 0.0 };
            bool isSuccess = false;

            isSuccess = ChkZMotorPos(eTeachingPosList.WAIT_POS);
            if (isSuccess == false)
            {
                //Z 축 대기 위치 이동 실패
                logStr = $"Transfer Z축 대기위치 확인 실패";
                Globalo.LogPrint("ManualControl", logStr);
                return false;
            }
            if(PickerNo < 0 || PickerNo > 3)
            {
                logStr = $"Transfer Picker Index Err";
                Globalo.LogPrint("ManualControl", logStr);
                return false;
            }
            dMultiPos[0] = Globalo.motionManager.transferMachine.teachingConfig.Teaching[(int)ePos].Pos[(int)eTransfer.TRANSFER_X];     //x Axis
            dMultiPos[1] = Globalo.motionManager.transferMachine.teachingConfig.Teaching[(int)ePos].Pos[(int)eTransfer.TRANSFER_Y];      //y Axis

            //리비안 물류
            //if (g_clMotorSet.MoveTransferMotorX(TRANS_ALIGN_POS, TaskWork.m_stTrayWorkPos.nTrayX[PCB_TRAY]) == false)
            //PickerNo = 피커 번호 (0, 1, 2, 3)
            //OffsetX = 피커별 Offset x
            //OffsetY = 피커별 Offset y
            //GapX = Tray , Socket , Ng 가로 간격
            //GapY = Tray , Socket , Ng 세로 간격

            //TODO: LEFT , RIGHT 각 TRAY 몇 번째 진행인지 저장돼야된다.

            if (ePos == eTeachingPosList.LEFT_TRAY_BCR_POS || 
                ePos == eTeachingPosList.RIGHT_TRAY_BCR_POS)
            {
                dOffsetPos[0] = (Globalo.motionManager.transferMachine.productLayout.TrayGap.GapX * CountX);
                dOffsetPos[1] = (Globalo.motionManager.transferMachine.productLayout.TrayGap.GapY * CountY);
            }
            else if (ePos == eTeachingPosList.LEFT_TRAY_LOAD_POS || ePos == eTeachingPosList.RIGHT_TRAY_LOAD_POS)
            {
                dOffsetPos[0] = (Globalo.motionManager.transferMachine.productLayout.TrayGap.GapX * CountX) + Globalo.motionManager.transferMachine.productLayout.LoadTrayOffset[PickerNo].OffsetX;
                dOffsetPos[1] = (Globalo.motionManager.transferMachine.productLayout.TrayGap.GapY * CountY) + Globalo.motionManager.transferMachine.productLayout.LoadTrayOffset[PickerNo].OffsetY;
            }
            else if (ePos == eTeachingPosList.LEFT_TRAY_UNLOAD_POS || ePos == eTeachingPosList.RIGHT_TRAY_UNLOAD_POS)
            {
                dOffsetPos[0] = (Globalo.motionManager.transferMachine.productLayout.TrayGap.GapX * CountX) + Globalo.motionManager.transferMachine.productLayout.UnLoadTrayOffset[PickerNo].OffsetX;
                dOffsetPos[1] = (Globalo.motionManager.transferMachine.productLayout.TrayGap.GapY * CountY) + Globalo.motionManager.transferMachine.productLayout.UnLoadTrayOffset[PickerNo].OffsetY;
            }
            else if (ePos == eTeachingPosList.SOCKET_A_LOAD || ePos == eTeachingPosList.SOCKET_B_LOAD ||
                ePos == eTeachingPosList.SOCKET_C_LOAD || ePos == eTeachingPosList.SOCKET_D_LOAD ||
                ePos == eTeachingPosList.SOCKET_A_UNLOAD || ePos == eTeachingPosList.SOCKET_B_UNLOAD ||
                ePos == eTeachingPosList.SOCKET_C_UNLOAD || ePos == eTeachingPosList.SOCKET_D_UNLOAD)
            {
                dOffsetPos[0] = 0.0;// Globalo.motionManager.transferMachine.productLayout.SocketGap.GapX; //소켓은 피커 4개 동시에 투입, 배출 이라서 필요없나?
                dOffsetPos[1] = 0.0;//Globalo.motionManager.transferMachine.productLayout.SocketGap.GapY;
            }
            else if (ePos == eTeachingPosList.NG_A_LOAD)
            {
                dOffsetPos[0] = Globalo.motionManager.transferMachine.productLayout.NgGap.GapX;
                dOffsetPos[1] = Globalo.motionManager.transferMachine.productLayout.NgGap.GapY;
            }
            else
            {
                dOffsetPos[0] = 0.0;
                dOffsetPos[1] = 0.0;
            }
            dMultiPos[0] += dOffsetPos[0];
            dMultiPos[1] += dOffsetPos[1];

            isSuccess = MultiAxisMove(multiAxis, dMultiPos, bWait);

            if (isSuccess == false)
            {
                logStr = $"Transfer XY축 {ePos.ToString() } 이동 실패";

                Globalo.LogPrint("ManualControl", logStr);
            }

            return isSuccess;
        }

        public override bool IsMoving()
        {
            if(AutoUnitThread.GetThreadRun() == true )
            {
                return true;
            }

            for (int i = 0; i < MotorAxes.Length; i++)
            {
                if(MotorAxes[i].GetStopAxis() == false)
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
            Console.WriteLine($"[ORIGIN] Transfer Run Stop");

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
            if (AutoUnitThread.GetThreadRun() == true)
            {
                return false;
            }

            if (TransferX.OrgState == false || TransferY.OrgState == false || TransferZ.OrgState == false)
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
                Globalo.LogPrint("MainForm", "[TRANSFER] 운전준비가 완료되지 않았습니다.", Globalo.eMessageName.M_WARNING);
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
