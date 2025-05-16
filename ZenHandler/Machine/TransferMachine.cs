using System;
using System.Collections.Generic;
using System.Drawing;
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
        public MotionControl.MotorSet.TrayPosition TrayPosition;        //Tray Load 위치 , Lift에서는 Right만 배출 , Magazine는 LEFT , RIGHT 동시 투입/배출
        public int MotorCnt { get; private set; } = 3;
        
        public MotionControl.MotorAxis[] MotorAxes; // 배열 선언
        public string[] axisName = { "TransferX", "TransferY", "TransferZ" };
        
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
            TOTAL_TRANSFER_TEACHING_COUNT
        };

        public string[] TeachName = { 
            "WAIT_POS",
            "LEFT_TRAY_BCR_POS", "RIGHT_TRAY_BCR_POS",
            "L_TRAY_LOAD_POS", "L_TRAY_UNLOAD_POS",
            "R_TRAY_LOAD_POS", "R_TRAY_UNLOAD_POS",
            "SOCKET_A_LOAD", "SOCKET_A_UNLOAD", "SOCKET_B_LOAD", "SOCKET_B_UNLOAD","SOCKET_C_LOAD", "SOCKET_C_UNLOAD", "SOCKET_D_LOAD", "SOCKET_D_UNLOAD",
            "NG_A_LOAD", "NG_A_UNLOAD","NG_B_LOAD", "NG_B_UNLOAD"
        };
        public DateTime uphStartTime;

        public const string teachingPath = "Teach_Transfer.yaml";
        public const string taskPath = "Task_Transfer.yaml";
        public const string LayoutPath = "Task_Product_Layout.yaml";

        public Data.TeachingConfig teachingConfig = new Data.TeachingConfig();
        public PickedProduct pickedProduct = new PickedProduct();
        public ProductLayout productLayout = new ProductLayout();
        public int NoSocketPos;     //투입 , 배출요청하는 소켓 index
        public string CurrentScanBcr = "";
        public const int UnLoadCount = 2;
        //TODO:  픽업 상태 로드 4개 , 배출 4개 / blank , LOAD , BCR OK , PASS , NG(DEFECT 1 , 2 , 3 , 4)
        //public Dio cylinder;
        //픽업 툴 4개 실린더 Dio 로 지정?

        public TransferMachine()//: base("Machine")
        {
            int i = 0;
            this.RunState = OperationState.Stopped;
            this.MachineName = this.GetType().Name;
            //MotorAxes = new MotionControl.MotorAxis[] { TransferX, TransferY, TransferZ };
            //MotorCnt = MotorAxes.Length;
            MotorAxes = new MotionControl.MotorAxis[MotorCnt];

            for (i = 0; i < MotorCnt; i++)
            {
                int index = (int)MotionControl.MotorSet.ValidTransferMotors[i];
                MotorAxes[i] = new MotionControl.MotorAxis(index,
                axisName[i], MotionControl.MotorSet.TransferMotorType[i], 
                MaxSpeeds[i], MotionControl.MotorSet.TransferAXT_SET_LIMIT[i], MotionControl.MotorSet.TransferAXT_SET_SERVO_ALARM[i], OrgFirstVel[i], OrgSecondVel[i], OrgThirdVel[i],
                MotionControl.MotorSet.TransferMOTOR_HOME_SENSOR[i], MotionControl.MotorSet.TransferMOTOR_HOME_DIR[i]);

                //초기 셋 다른 곳에서 다시 해줘야될 듯
                MotorAxes[i].setMotorParameter(10.0, 0.1, 0.1, 1000.0);//(double vel , double acc , double dec , double resol)

                if(this.MotorUse == false)
                {
                    MotorAxes[i].NoUse = true;
                }
            }
            for (i = 0; i < 4; i++)
            {
                pickedProduct.LoadProductInfo.Add(new ProductInfo(i));
                pickedProduct.UnLoadProductInfo.Add(new ProductInfo(i));

                // [pickedProduct] 담고있는 정보
                //1.Index 
                //2.바코드
                //3.제품 상태 (Blank , 양품, 불량 등)
            }

            pickedProduct = Data.TaskDataYaml.TaskLoad_Transfer(taskPath);
            productLayout = Data.TaskDataYaml.TaskLoad_Layout(LayoutPath);

            TrayPosition = MotionControl.MotorSet.TrayPosition.Right;

            uphStartTime = DateTime.Now;
            //double elapsedMinutes = (DateTime.Now - uphStartTime).TotalSeconds;//TotalMinutes;
            TimeSpan elapsed = DateTime.Now - uphStartTime;
            double elapsedMinutes = elapsed.TotalMinutes;
            double elapsedSeconds = elapsed.TotalSeconds;

            NoSocketPos = -1;

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
                if (i < TeachName.Length)
                {
                    teachingConfig.Teaching[i].Name = TeachName[i];
                }
            }
   

        }
        public void OnTransferBcrReceived(string data)
        {
            CurrentScanBcr = data;
            Console.WriteLine($"On Transfer BcrReceived:({CurrentScanBcr})");
        }
        public void LoadTryAdd(int LoadCnt = 1)
        {
            int currentPosx = this.pickedProduct.LoadTrayPos.X;
            int currentPosy = this.pickedProduct.LoadTrayPos.Y;

            int MaxXCount = this.productLayout.TotalTrayPos.X;
            int MaxYCount = this.productLayout.TotalTrayPos.Y;
            Console.WriteLine($"Current Load X : {currentPosx} / {MaxXCount}");
            Console.WriteLine($"Current Load Y : {currentPosy} / {MaxYCount}");

            //배출 위치는 로드하는 위치로 지정?
            //제품 로드하면서 첫 배출 위치를 설정하는 함수

            this.pickedProduct.UnloadTrayPos.X = currentPosx;      //TODO: 배출 위치는 어떻게 관리? Y축 라인으로 해야할듯
            this.pickedProduct.UnloadTrayPos.Y = currentPosy;
            //
            //
            //
            this.pickedProduct.LoadTrayPos.X += LoadCnt;

            if (this.pickedProduct.LoadTrayPos.X >= MaxXCount)
            {
                this.pickedProduct.LoadTrayPos.X = 0;
                this.pickedProduct.LoadTrayPos.Y++;
            }

            if (this.pickedProduct.LoadTrayPos.Y >= MaxYCount)
            {
                this.pickedProduct.LoadTrayPos.Y = 0;
            }
            int nextPosx = this.pickedProduct.LoadTrayPos.X;
            int nextPosy = this.pickedProduct.LoadTrayPos.Y;


            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine($"Next Load X : {currentPosx} > {nextPosx}");
            Console.WriteLine($"Next Load Y : {currentPosy} > {nextPosy}");
            
        }
        public void UnloadTryAdd(int UnloadCnt)
        {
            int currentPosx = this.pickedProduct.UnloadTrayPos.X;
            int currentPosy = this.pickedProduct.UnloadTrayPos.Y;

            //여기는 배출하는 과정에 배출 개수에 따라 배출 위치 재설정하는 함수
            this.pickedProduct.UnloadTrayPos.X += UnloadCnt;

            if (this.pickedProduct.UnloadTrayPos.X >= this.productLayout.TotalTrayPos.X)
            {
                this.pickedProduct.UnloadTrayPos.X = 0;
                this.pickedProduct.UnloadTrayPos.Y++;
            }
            if (this.pickedProduct.UnloadTrayPos.Y == 1)
            {
                OnTrayChangedCall?.Invoke(TrayPosition);    //Gantry -----> Pusher로 이동 요청
                TrayPosition = MotionControl.MotorSet.TrayPosition.Right;
            }
            if (this.pickedProduct.UnloadTrayPos.Y >= this.productLayout.TotalTrayPos.Y)
            {
                this.pickedProduct.UnloadTrayPos.Y = 0;

                Console.WriteLine($"Tray Change req");
                //TODO: TrayPosition 을 LEFT , RIGHT 변경해줘야된다.
                //Tray 교체 요청
                OnTrayChangedCall?.Invoke(TrayPosition); // 어떤 트레이 비었는지 전달

                TrayPosition = MotionControl.MotorSet.TrayPosition.Left;        //우측 배출 요청하고 Gantry 위에서 제품 로드 LEFT 변경
            }
            int nextPosx = this.pickedProduct.UnloadTrayPos.X;
            int nextPosy = this.pickedProduct.UnloadTrayPos.Y;


            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine($"배출 X : {currentPosx} > {nextPosx}");
            Console.WriteLine($"배출 Y : {currentPosy} > {nextPosy}");
        }
        public void CheckTrayState()
        {
            //State = TransferUnitState.TrayEmpty;

            OnTrayChangedCall?.Invoke(TrayPosition); // 어떤 트레이 비었는지 전달
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
        
        
    #region Transfer Io 동작
        public bool GetLoadPickerUpState(int index, bool bFlag)
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            int lModuleNo = 1;
            int lOffset = 0;

            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];
            if(index == 0)
            {
                if (bFlag)
                {
                    uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_LOAD_PICKER_UP1;
                    if (uFlagHigh == 1)
                    {
                        return true;
                    }
                }
                else
                {
                    uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_LOAD_PICKER_DOWN1;
                    if (uFlagHigh == 1)
                    {
                        return true;
                    }
                }
            }
            else if (index == 1)
            {
                if (bFlag)
                {
                    uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_LOAD_PICKER_UP2;
                    if (uFlagHigh == 1)
                    {
                        return true;
                    }
                }
                else
                {
                    uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_LOAD_PICKER_DOWN2;
                    if (uFlagHigh == 1)
                    {
                        return true;
                    }
                }
            }
            else if (index == 2)
            {
                if (bFlag)
                {
                    uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_LOAD_PICKER_UP3;
                    if (uFlagHigh == 1)
                    {
                        return true;
                    }
                }
                else
                {
                    uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_LOAD_PICKER_DOWN3;
                    if (uFlagHigh == 1)
                    {
                        return true;
                    }
                }
            }
            else if (index == 3)
            {
                if (bFlag)
                {
                    uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_LOAD_PICKER_UP4;
                    if (uFlagHigh == 1)
                    {
                        return true;
                    }
                }
                else
                {
                    uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_LOAD_PICKER_DOWN4;
                    if (uFlagHigh == 1)
                    {
                        return true;
                    }
                }
            }
            
            return false;

        }

        public bool GetUnLoadPickerUpState(int index, bool bFlag)
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            int lModuleNo = 1;
            int lOffset = 0;

            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];
            if (index == 0)
            {
                if (bFlag)
                {
                    uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_UNLOAD_PICKER_UP1;
                    if (uFlagHigh == 1)
                    {
                        return true;
                    }
                }
                else
                {
                    uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_UNLOAD_PICKER_DOWN1;
                    if (uFlagHigh == 1)
                    {
                        return true;
                    }
                }
            }
            else if (index == 1)
            {
                if (bFlag)
                {
                    uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_UNLOAD_PICKER_UP2;
                    if (uFlagHigh == 1)
                    {
                        return true;
                    }
                }
                else
                {
                    uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_UNLOAD_PICKER_DOWN2;
                    if (uFlagHigh == 1)
                    {
                        return true;
                    }
                }
            }
            else if (index == 2)
            {
                if (bFlag)
                {
                    uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_UNLOAD_PICKER_UP3;
                    if (uFlagHigh == 1)
                    {
                        return true;
                    }
                }
                else
                {
                    uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_UNLOAD_PICKER_DOWN3;
                    if (uFlagHigh == 1)
                    {
                        return true;
                    }
                }
            }
            else if (index == 3)
            {
                if (bFlag)
                {
                    uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_UNLOAD_PICKER_UP4;
                    if (uFlagHigh == 1)
                    {
                        return true;
                    }
                }
                else
                {
                    uFlagHigh = upValue & (uint)MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_UNLOAD_PICKER_DOWN4;
                    if (uFlagHigh == 1)
                    {
                        return true;
                    }
                }
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

        public bool GetLoadMultiPickerUp(int[] pickerList, bool bFlag, bool bWait = false)
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            if (pickerList.Length != 4)      //항상 4개
            {
                Console.WriteLine("GetLoadMultiPickerUp Length Fail [{pickerList.Length}]");
                return false;
            }
            int i = 0;
            int lModuleNo = 0;
            int lOffset = 0;

            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];


            for (i = 0; i < pickerList.Length; i++)
            {
                bool chk = false;
                if (pickerList[i] == 1)
                {
                    chk = true;
                }
                switch (i)
                {
                    case 0:
                        if (chk)
                        {
                            if (bFlag)
                            {
                                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_LOAD_PICKER_UP1);
                            }
                            else
                            {
                                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_LOAD_PICKER_DOWN1);
                            }
                        }


                        break;
                    case 1:
                        if (chk)
                        {
                            if (bFlag)
                            {
                                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_LOAD_PICKER_UP2);
                            }
                            else
                            {
                                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_LOAD_PICKER_DOWN2);
                            }
                        }

                        break;
                    case 2:
                        if (chk)
                        {
                            if (bFlag)
                            {
                                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_LOAD_PICKER_UP3);
                            }
                            else
                            {
                                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_LOAD_PICKER_DOWN3);
                            }
                        }
                        break;
                    case 3:
                        if (chk)
                        {
                            if (bFlag)
                            {
                                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_LOAD_PICKER_UP4);
                            }
                            else
                            {
                                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_LOAD_PICKER_DOWN4);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            if (bFlag)
            {
                uFlagHigh = upValue & uFlagHigh;        //TODO: IO 되는지 확인 필요
                if (uFlagHigh == 1)
                {
                    return true;
                }
            }
            else
            {
                uFlagHigh = upValue & uFlagHigh;
                if (uFlagHigh == 0)
                {
                    return true;
                }
            }
            return false;
        }
        public bool GetUnloadMultiPickerUp(int[] pickerList, bool bFlag, bool bWait = false)
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            if (pickerList.Length != 4)      //항상 4개
            {
                Console.WriteLine("GetUnloadMultiPickerUp Length Fail [{pickerList.Length}]");
                return false;
            }
            int i = 0;
            int lModuleNo = 0;
            int lOffset = 0;

            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];


            for (i = 0; i < pickerList.Length; i++)
            {
                bool chk = false;
                if (pickerList[i] == 1)
                {
                    chk = true;
                }
                switch (i)
                {
                    case 0:
                        if (chk)
                        {
                            if (bFlag)
                            {
                                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_UNLOAD_PICKER_UP1);
                            }
                            else
                            {
                                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_UNLOAD_PICKER_DOWN1);
                            }
                        }


                        break;
                    case 1:
                        if (chk)
                        {
                            if (bFlag)
                            {
                                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_UNLOAD_PICKER_UP2);
                            }
                            else
                            {
                                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_UNLOAD_PICKER_DOWN2);
                            }
                        }

                        break;
                    case 2:
                        if (chk)
                        {
                            if (bFlag)
                            {
                                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_UNLOAD_PICKER_UP3);
                            }
                            else
                            {
                                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_UNLOAD_PICKER_DOWN3);
                            }
                        }
                        break;
                    case 3:
                        if (chk)
                        {
                            if (bFlag)
                            {
                                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_UNLOAD_PICKER_UP4);
                            }
                            else
                            {
                                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_IN_ADDR_CH1.IN_UNLOAD_PICKER_DOWN4);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            if (bFlag)
            {
                uFlagHigh = upValue & uFlagHigh;        //TODO: IO 되는지 확인 필요
                if (uFlagHigh == 1)
                {
                    return true;
                }
            }
            else
            {
                uFlagHigh = upValue & uFlagHigh;
                if (uFlagHigh == 0)
                {
                    return true;
                }
            }
            return false;
        }
        public bool LoadMultiPickerUp(int[] pickerList , bool bFlag, bool bWait = false)
        {
            
            //pickerList = 1로 들어오는 Picker만 반응하는 방식 xxxxx
            //동작해야되는 피커 번호만 들어옴 동시 동작 개수 만큼
            bool isSuccess = false;
            int lModuleNo = 2;
            int lOffset = 0;
            uint uFlagHigh = 0;
            uint uFlagLow = 0;
            int i = 0;
            
            for (i = 0; i < pickerList.Length; i++)
            {
                bool chk = false;
                int index = pickerList[i];

                switch (index)
                {
                    case 0:
                        if (bFlag)
                        {
                            uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_OUT_ADDR_CH1.LOAD_PICKER_UP1);
                            uFlagLow |= (uint)(MotionControl.DioDefine.DIO_OUT_ADDR_CH1.LOAD_PICKER_DOWN1);
                        }
                        else
                        {
                            uFlagLow |= (uint)(MotionControl.DioDefine.DIO_OUT_ADDR_CH1.LOAD_PICKER_UP1);
                            uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_OUT_ADDR_CH1.LOAD_PICKER_DOWN1);
                        }

                        break;
                    case 1:
                        if (bFlag)
                        {
                            uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_OUT_ADDR_CH1.LOAD_PICKER_UP2);
                            uFlagLow |= (uint)(MotionControl.DioDefine.DIO_OUT_ADDR_CH1.LOAD_PICKER_DOWN2);
                        }
                        else
                        {
                            uFlagLow |= (uint)(MotionControl.DioDefine.DIO_OUT_ADDR_CH1.LOAD_PICKER_UP2);
                            uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_OUT_ADDR_CH1.LOAD_PICKER_DOWN2);
                        }
                        break;
                    case 2:
                        if (bFlag)
                        {
                            uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_OUT_ADDR_CH1.LOAD_PICKER_UP3);
                            uFlagLow |= (uint)(MotionControl.DioDefine.DIO_OUT_ADDR_CH1.LOAD_PICKER_DOWN3);
                        }
                        else
                        {
                            uFlagLow |= (uint)(MotionControl.DioDefine.DIO_OUT_ADDR_CH1.LOAD_PICKER_UP3);
                            uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_OUT_ADDR_CH1.LOAD_PICKER_DOWN3);
                        }
                        break;
                    case 3:
                        if (bFlag)
                        {
                            uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_OUT_ADDR_CH1.LOAD_PICKER_UP4);
                            uFlagLow |= (uint)(MotionControl.DioDefine.DIO_OUT_ADDR_CH1.LOAD_PICKER_DOWN4);
                        }
                        else
                        {
                            uFlagLow |= (uint)(MotionControl.DioDefine.DIO_OUT_ADDR_CH1.LOAD_PICKER_UP4);
                            uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_OUT_ADDR_CH1.LOAD_PICKER_DOWN4);
                        }
                        break;
                    default:
                        break;
                }
            }
            isSuccess = Globalo.motionManager.ioController.DioWriteOutportByte(lModuleNo, lOffset, uFlagHigh, uFlagLow);
            if (isSuccess == false)
            {
                Console.WriteLine($" LoadMultiPickerUp MOVE FAIL");
                return isSuccess;
            }

            return isSuccess;
        }
        public bool UnloadMultiPickerUp(int[] pickerList, bool bFlag, bool bWait = false)
        {
            if (ProgramState.NORINDA_MODE == false)
            {
                if (ProgramState.ON_LINE_MOTOR == false)
                {
                    return true;
                }
            }
            if (pickerList.Length != 4)      //항상 4개
            {
                Console.WriteLine("UnloadMultiPickerUp Length Fail [{pickerList.Length}]");
                return false;
            }
            //pickerList = 1로 들어오는 Picker만 반응하는 방식

            bool isSuccess = false;
            int lModuleNo = 2;
            int lOffset = 0;
            uint uFlagHigh = 0;
            uint uFlagLow = 0;
            int i = 0;

            for (i = 0; i < pickerList.Length; i++)
            {
                bool chk = false;
                if (pickerList[i] == 1)
                {
                    chk = true;
                }
                switch (i)
                {
                    case 0:
                        if (chk)
                        {
                            if (bFlag)
                            {
                                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_OUT_ADDR_CH1.UNLOAD_PICKER_UP1);
                                uFlagLow |= (uint)(MotionControl.DioDefine.DIO_OUT_ADDR_CH1.UNLOAD_PICKER_DOWN1);
                            }
                            else
                            {
                                uFlagLow |= (uint)(MotionControl.DioDefine.DIO_OUT_ADDR_CH1.UNLOAD_PICKER_UP1);
                                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_OUT_ADDR_CH1.UNLOAD_PICKER_DOWN1);
                            }
                        }


                        break;
                    case 1:
                        if (chk)
                        {
                            if (bFlag)
                            {
                                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_OUT_ADDR_CH1.UNLOAD_PICKER_UP2);
                                uFlagLow |= (uint)(MotionControl.DioDefine.DIO_OUT_ADDR_CH1.UNLOAD_PICKER_DOWN2);
                            }
                            else
                            {
                                uFlagLow |= (uint)(MotionControl.DioDefine.DIO_OUT_ADDR_CH1.UNLOAD_PICKER_UP2);
                                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_OUT_ADDR_CH1.UNLOAD_PICKER_DOWN2);
                            }
                        }

                        break;
                    case 2:
                        if (chk)
                        {
                            if (bFlag)
                            {
                                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_OUT_ADDR_CH1.UNLOAD_PICKER_UP3);
                                uFlagLow |= (uint)(MotionControl.DioDefine.DIO_OUT_ADDR_CH1.UNLOAD_PICKER_DOWN3);
                            }
                            else
                            {
                                uFlagLow |= (uint)(MotionControl.DioDefine.DIO_OUT_ADDR_CH1.UNLOAD_PICKER_UP3);
                                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_OUT_ADDR_CH1.UNLOAD_PICKER_DOWN3);
                            }
                        }
                        break;
                    case 3:
                        if (chk)
                        {
                            if (bFlag)
                            {
                                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_OUT_ADDR_CH1.UNLOAD_PICKER_UP4);
                                uFlagLow |= (uint)(MotionControl.DioDefine.DIO_OUT_ADDR_CH1.UNLOAD_PICKER_DOWN4);
                            }
                            else
                            {
                                uFlagLow |= (uint)(MotionControl.DioDefine.DIO_OUT_ADDR_CH1.UNLOAD_PICKER_UP4);
                                uFlagHigh |= (uint)(MotionControl.DioDefine.DIO_OUT_ADDR_CH1.UNLOAD_PICKER_DOWN4);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            isSuccess = Globalo.motionManager.ioController.DioWriteOutportByte(lModuleNo, lOffset, uFlagHigh, uFlagLow);
            if (isSuccess == false)
            {
                Console.WriteLine($" UnloadMultiPickerUp MOVE FAIL");
                return isSuccess;
            }

            return isSuccess;
        }
        public bool LoadPickerUp(int index, bool bFlag, bool bWait = false)
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            int lModuleNo = 2;
            int lOffset = 0;
            uint uFlagHigh = 0;
            uint uFlagLow = 0;
            if(index == 0)
            {
                if (bFlag)
                {
                    uFlagHigh = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH1.LOAD_PICKER_UP1;
                    uFlagLow = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH1.LOAD_PICKER_DOWN1;
                }
                else
                {
                    uFlagHigh = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH1.LOAD_PICKER_DOWN1;
                    uFlagLow = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH1.LOAD_PICKER_UP1;
                }
            }
            else if (index == 1)
            {
                if (bFlag)
                {
                    uFlagHigh = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH1.LOAD_PICKER_UP2;
                    uFlagLow = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH1.LOAD_PICKER_DOWN2;
                }
                else
                {
                    uFlagHigh = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH1.LOAD_PICKER_DOWN2;
                    uFlagLow = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH1.LOAD_PICKER_UP2;
                }
            }
            else if (index == 2)
            {
                if (bFlag)
                {
                    uFlagHigh = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH1.LOAD_PICKER_UP3;
                    uFlagLow = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH1.LOAD_PICKER_DOWN3;
                }
                else
                {
                    uFlagHigh = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH1.LOAD_PICKER_DOWN3;
                    uFlagLow = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH1.LOAD_PICKER_UP3;
                }
            }
            else if (index == 3)
            {
                if (bFlag)
                {
                    uFlagHigh = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH1.LOAD_PICKER_UP4;
                    uFlagLow = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH1.LOAD_PICKER_DOWN4;
                }
                else
                {
                    uFlagHigh = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH1.LOAD_PICKER_DOWN4;
                    uFlagLow = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH1.LOAD_PICKER_UP4;
                }
            }
            

            bool Rtn = Globalo.motionManager.ioController.DioWriteOutportByte(lModuleNo, lOffset, uFlagHigh, uFlagLow);
            if (Rtn == false)
            {
                Console.WriteLine($"#{index} LOAD PICKER MOVE FAIL");
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

            if (bFlag == false)
            {
                Thread.Sleep(300);
                //off 일때 파기를 꺼줘야된다.
                uFlagLow = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH0.VACUUM_OFF;
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
                uFlagHigh = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH0.LENS_GRIP_FOR;
                uFlagLow = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH0.LENS_GRIP_BACK;
            }
            else
            {
                uFlagHigh = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH0.LENS_GRIP_BACK;
                uFlagLow = (uint)MotionControl.DioDefine.DIO_OUT_ADDR_CH0.LENS_GRIP_FOR;
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


#endregion

    #region Transfer Motor 동작
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


            dXPos = this.teachingConfig.Teaching[(int)teachingPos].Pos[(int)eTransfer.TRANSFER_X];
            dYPos = this.teachingConfig.Teaching[(int)teachingPos].Pos[(int)eTransfer.TRANSFER_Y];


            currentXPos = MotorAxes[(int)eTransfer.TRANSFER_X].EncoderPos;
            currentYPos = MotorAxes[(int)eTransfer.TRANSFER_Y].EncoderPos;

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


            dZTeachingPos = this.teachingConfig.Teaching[(int)teachingPos].Pos[(int)eTransfer.TRANSFER_Z];
            currentZPos = MotorAxes[(int)eTransfer.TRANSFER_Z].EncoderPos;

            if (dZTeachingPos == currentZPos)
            {
                return true;
            }

            return false;
        }
        public bool TransFer_X_Move(eTeachingPosList ePos, bool bWait = true)
        {
            if (this.MotorUse == false)
            {
                Console.WriteLine("No Use Machine");
                return true;
            }
            if (MotorAxes[(int)eTransfer.TRANSFER_X].IsMotorBusy == true)
            {
                Globalo.LogPrint("ManualControl", $"모터 작업이 이미 실행 중입니다. 기다려 주세요.");
                return false;
            }
            double dPos = this.teachingConfig.Teaching[(int)ePos].Pos[(int)eTransfer.TRANSFER_X];

            bool isSuccess = true;
            try
            {
                isSuccess = MotorAxes[(int)eTransfer.TRANSFER_X].MoveAxis(dPos, AXT_MOTION_ABSREL.POS_ABS_MODE, bWait);

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
            if (this.MotorUse == false)
            {
                Console.WriteLine("No Use Machine");
                return true;
            }
            bool isSuccess = true;
            string logStr = "";
            double dPos = this.teachingConfig.Teaching[(int)ePos].Pos[(int)eTransfer.TRANSFER_Z];     //z Axis
            try
            {
                isSuccess = MotorAxes[(int)eTransfer.TRANSFER_Z].MoveAxis(dPos, AXT_MOTION_ABSREL.POS_ABS_MODE, bWait);
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
        public bool TransFer_XY_Move(eTeachingPosList ePos, int TrayX = 0, int TrayY = 0,  bool bWait = true)  //Picket Index , Tray or Socekt or Ng , 
        {
            //TODO: PickerNo 는 없애고 CountX로 써도될듯 확인필요.
            if (this.MotorUse == false)
            {
                Console.WriteLine("No Use Machine");
                return true;
            }
            string logStr = "";
            bool isSuccess = false;
            isSuccess = ChkZMotorPos(eTeachingPosList.WAIT_POS);
            if (isSuccess == false)
            {
                //Z 축 대기 위치 이동 실패
                logStr = $"Transfer Z축 대기위치 확인 실패";
                Globalo.LogPrint("ManualControl", logStr);
                return isSuccess;
            }

            MotionControl.MotorAxis[] multiAxis = { MotorAxes[(int)eTransfer.TRANSFER_X], MotorAxes[(int)eTransfer.TRANSFER_Y] };
            double[] dMultiPos = { 0.0, 0.0 };
            double[] dOffsetPos = { 0.0, 0.0 };
            int PickerNo = TrayX;
            if (PickerNo < 0 || PickerNo > 3)
            {
                logStr = $"Transfer Picker Index Err";
                Globalo.LogPrint("ManualControl", logStr);
                return false;
            }

            dMultiPos[0] = this.teachingConfig.Teaching[(int)ePos].Pos[(int)eTransfer.TRANSFER_X];     //x Axis
            dMultiPos[1] = this.teachingConfig.Teaching[(int)ePos].Pos[(int)eTransfer.TRANSFER_Y];      //y Axis

            //리비안 물류
            //if (g_clMotorSet.MoveTransferMotorX(TRANS_ALIGN_POS, TaskWork.m_stTrayWorkPos.nTrayX[PCB_TRAY]) == false)
            //PickerNo = 피커 번호 (0, 1, 2, 3)
            //OffsetX = 피커별 Offset x
            //OffsetY = 피커별 Offset y
            //GapX = Tray , Socket , Ng 가로 간격
            //GapY = Tray , Socket , Ng 세로 간격

            //TODO: LEFT , RIGHT 각 TRAY 몇 번째 진행인지 저장돼야된다.
            //MEMO: 로드 위치이동 방식 - 1번 피커부터 바깥에서 부터 바코드스캔후 2칸씩 이동하며 들어온다.
            //double targetx = LOAD POS X + (Tray x 간격 * Tray X Index) + (1번피커와의 간격 Offset X)
            //double targetx: 15.5 = 10.5 + (5.0 * 0) + (0.0);      //1번 피커
            //double targetx: 25.5 = 10.5 + (5.0 * 1) + (10.0);     //2번 피커
            //double targetx: 40.5 = 10.5 + (5.0 * 2) + (20.0);     //3번 피커
            //double targetx: 55.5= 10.5 + (5.0 * 3) + (30.0);      //4번 피커

            //MEMO: 배출 위치이동 방식 - 각자 Tray 배출 위치에서 티칭 - 1번이든 , 3번이든, Tray 간격은 필요 없을 듯

            if (ePos == eTeachingPosList.LEFT_TRAY_BCR_POS || ePos == eTeachingPosList.RIGHT_TRAY_BCR_POS)
            {
                //바코드 스캔 위치
                //
                dOffsetPos[0] = (this.productLayout.TrayGap.GapX * TrayX); //( X 간격 * 가로 위치)  10.0 곱하기 (0, 1, 2, 3)
                dOffsetPos[1] = (this.productLayout.TrayGap.GapY * TrayY);//( Y 간격 * 세로 위치)  10.0 곱하기 (0 ~ 전체 Tray Y 개수)
            }
            else if (ePos == eTeachingPosList.LEFT_TRAY_LOAD_POS || ePos == eTeachingPosList.RIGHT_TRAY_LOAD_POS)
            {
                //TRAY 위 제품 로드 위치 - 1번 피커부터 바깥부터, Tray 간격 1칸 + 피커 1칸 간격 씩 이동
                //
                dOffsetPos[0] = (this.productLayout.TrayGap.GapX * TrayX) + this.productLayout.LoadTrayOffset[PickerNo].OffsetX;
                dOffsetPos[1] = (this.productLayout.TrayGap.GapY * TrayY) + this.productLayout.LoadTrayOffset[PickerNo].OffsetY;
            }
            else if (ePos == eTeachingPosList.LEFT_TRAY_UNLOAD_POS || ePos == eTeachingPosList.RIGHT_TRAY_UNLOAD_POS)
            {
                //MEMO: FW 모델은 TRAY 에 배출할때 무조건 하나씩 배출해야된다.
                //TRAY 위 제품 배출 위치 - 티칭위치가 각자 바로 피커 하강해도 되는 위치라서 Tray 간격은 필요 없을듯 
                //
                dOffsetPos[0] = this.productLayout.UnLoadTrayOffset[PickerNo].OffsetX;
                dOffsetPos[1] = this.productLayout.UnLoadTrayOffset[PickerNo].OffsetY;
                //dOffsetPos[0] = (Globalo.motionManager.transferMachine.productLayout.TrayGap.GapX * TrayX) + Globalo.motionManager.transferMachine.productLayout.UnLoadTrayOffset[PickerNo].OffsetX;
                //dOffsetPos[1] = (Globalo.motionManager.transferMachine.productLayout.TrayGap.GapY * TrayY) + Globalo.motionManager.transferMachine.productLayout.UnLoadTrayOffset[PickerNo].OffsetY;
            }
            else if (ePos == eTeachingPosList.SOCKET_A_LOAD || ePos == eTeachingPosList.SOCKET_B_LOAD ||
                ePos == eTeachingPosList.SOCKET_C_LOAD || ePos == eTeachingPosList.SOCKET_D_LOAD)
            {
                //Socket에 제품 투입 / 배출 위치
                //Socket쪽에는 피커 전체 동시 동작 Fx = 4 , EE = 4 , Aoi = 2
                //
                dOffsetPos[0] = (this.productLayout.SocketGap.GapX * TrayX) + this.productLayout.LoadTrayOffset[PickerNo].OffsetX;
                dOffsetPos[1] = (this.productLayout.SocketGap.GapY * TrayY) + this.productLayout.LoadTrayOffset[PickerNo].OffsetY;
            }
            else if (ePos == eTeachingPosList.SOCKET_A_UNLOAD || ePos == eTeachingPosList.SOCKET_B_UNLOAD ||
                ePos == eTeachingPosList.SOCKET_C_UNLOAD || ePos == eTeachingPosList.SOCKET_D_UNLOAD)
            {
                //Socket에 제품 투입 / 배출 위치
                //
                dOffsetPos[0] = (this.productLayout.SocketGap.GapX * TrayX) + this.productLayout.UnLoadTrayOffset[PickerNo].OffsetX;
                dOffsetPos[1] = (this.productLayout.SocketGap.GapY * TrayY) + this.productLayout.UnLoadTrayOffset[PickerNo].OffsetY;
            }
            else if (ePos == eTeachingPosList.NG_A_UNLOAD || ePos == eTeachingPosList.NG_B_UNLOAD)
            {
                //MEMO: Ng는 전모델 픽업 하나씩 내려놔야된다.
                dOffsetPos[0] = (this.productLayout.NgGap.GapX * TrayX) + this.productLayout.NgOffset[PickerNo].OffsetX;
                dOffsetPos[1] = (this.productLayout.NgGap.GapY * TrayY) + this.productLayout.NgOffset[PickerNo].OffsetY;
            }
            else
            {
                dOffsetPos[0] = 0.0;
                dOffsetPos[1] = 0.0;
            }
            dMultiPos[0] += dOffsetPos[0];      
            dMultiPos[1] += dOffsetPos[1];

            Console.WriteLine($"[{ePos.ToString()} x Pos({TrayX}):{dMultiPos[0]},  y Pos({TrayY}):{dMultiPos[1]}]"); 
            //ex) Bcr X Pos = (Bcr Pos + (x 가로 간격 * 가로 위치))
            //ex) in Tray 제품 로드 X Pos = (Tray Load Pos + (x 가로 간격 * 가로 위치))  = (100.0 + ((Tray 가로 간격) * (0 or 2)) : 2개씩 배출시 

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
#endregion

    #region Transfer Auto Run 동작
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
                this.RunState = OperationState.OriginRunning;
                szLog = $"[ORIGIN] Transfer Origin Start";
                Console.WriteLine($"[ORIGIN] Transfer Origin Start");
                Globalo.LogPrint("MainForm", szLog);
            }
            else
            {
                this.RunState = OperationState.Stopped;
                Console.WriteLine($"[ORIGIN] Transfer Origin Start Fail");
                szLog = $"[ORIGIN] Transfer Origin Start Fail";
                Globalo.LogPrint("MainForm", szLog);
            }
            return rtn;
        }
        public override bool ReadyRun()
        {
            if (this.RunState != OperationState.Stopped && this.RunState != OperationState.OriginDone)
            {
                Globalo.LogPrint("MainForm", "[TRANSFER] 설비 정지상태가 아닙니다.", Globalo.eMessageName.M_WARNING);
                return false;
            }
            if (AutoUnitThread.GetThreadRun() == true)
            {
                Globalo.LogPrint("MainForm", "[TRANSFER] 설비 정지상태가 아닙니다..", Globalo.eMessageName.M_WARNING);
                return false;
            }

            if (MotorAxes[(int)Machine.eTransfer.TRANSFER_X].OrgState == false || 
                MotorAxes[(int)Machine.eTransfer.TRANSFER_Y].OrgState == false || 
                MotorAxes[(int)Machine.eTransfer.TRANSFER_Z].OrgState == false)
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
            if (this.RunState != OperationState.Paused)
            {
                if (this.RunState != OperationState.Standby)
                {
                    Globalo.LogPrint("MainForm", "[TRANSFER] 운전준비가 완료되지 않았습니다.", Globalo.eMessageName.M_WARNING);
                    return false;
                }
            }
            

            if (AutoUnitThread.GetThreadRun() == true)
            {
                Console.WriteLine($"모터 동작 중입니다.");

                if (AutoUnitThread.GetThreadPause() == true)        //일시 정지 상태인지 확인
                {
                    AutoUnitThread.Resume();
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
#endregion
    }
}
