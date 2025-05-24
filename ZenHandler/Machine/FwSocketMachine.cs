using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.Machine
{
    public class FwSocketMachine : MotionControl.MotorController
    {
        public int MotorCnt { get; private set; } = 0;

        //실린더 전후진 4개
        //실린더 상승,하강 4개

        //소켓 4개씩 4 세트 = 총 16개

        public const string teachingPath = "Teach_FwSocket.yaml";
        public const string taskPath = "Task_FwSocket.yaml";
        //public Data.TeachingConfig teachingConfig = new Data.TeachingConfig();
        public SocketProduct socketProduct = new SocketProduct();

        public FwSocketMachine()
        {
            int i = 0;
            this.RunState = OperationState.Stopped;
            this.MachineName = this.GetType().Name;


            socketProduct = Data.TaskDataYaml.TaskLoad_Socket(taskPath);
            if (socketProduct.SocketInfo_A.Count < 1)
            {
                socketProduct.SocketInfo_A.Add(new SocketProductInfo());
                socketProduct.SocketInfo_A.Add(new SocketProductInfo());
                socketProduct.SocketInfo_A.Add(new SocketProductInfo());
                socketProduct.SocketInfo_A.Add(new SocketProductInfo());
            }
            if (socketProduct.SocketInfo_B.Count < 1)
            {
                socketProduct.SocketInfo_B.Add(new SocketProductInfo());
                socketProduct.SocketInfo_B.Add(new SocketProductInfo());
                socketProduct.SocketInfo_B.Add(new SocketProductInfo());
                socketProduct.SocketInfo_B.Add(new SocketProductInfo());
            }
            if (socketProduct.SocketInfo_C.Count < 1)
            {
                socketProduct.SocketInfo_C.Add(new SocketProductInfo());
                socketProduct.SocketInfo_C.Add(new SocketProductInfo());
                socketProduct.SocketInfo_C.Add(new SocketProductInfo());
                socketProduct.SocketInfo_C.Add(new SocketProductInfo());
            }
            if (socketProduct.SocketInfo_D.Count < 1)
            {
                socketProduct.SocketInfo_D.Add(new SocketProductInfo());
                socketProduct.SocketInfo_D.Add(new SocketProductInfo());
                socketProduct.SocketInfo_D.Add(new SocketProductInfo());
                socketProduct.SocketInfo_D.Add(new SocketProductInfo());
            }
        }
        public override bool TaskSave()
        {
            bool rtn = Data.TaskDataYaml.TaskSave_Socket(socketProduct, taskPath);
            return rtn;
        }
        public override void MotorDataSet()
        {
            //Fw Socket Motor xxxx
        }
        #region Fw Socket Machine Io 동작
        public bool GetIsProductInSocket(int GroupNo,  int index, bool bFlag, bool bWait = false)      //각 소켓의 제품 유무 확인 센서
        {
            //GroupNo = 앞2 , 뒤2 4Set
            return false;
        }
        public bool GetIsContactForward(int GroupNo, int index, bool bFlag, bool bWait = false)      //각 소켓의 푸셔 전/후진 확인 센서
        {
            //GroupNo = 앞2 , 뒤2 4Set
            return false;
        }
        public bool GetIsContactUp(int GroupNo, int index, bool bFlag, bool bWait = false)      //각 소켓의 푸셔 상/하강 확인 센서
        {
            //GroupNo = 앞2 , 뒤2 4Set
            return false;
        }
        public bool GetIsFlipperRotated(int GroupNo, int index, bool bFlag, bool bWait = false)      //각 소켓의 로테이션 회전 상태 확인
        {
            //GroupNo = 앞2 , 뒤2 4Set
            return false;
        }
        public bool GetIsFlipperUp(int GroupNo, int index, bool bFlag, bool bWait = false)      //각 소켓의 로테이션 실린더 상/하강 상태
        {
            //GroupNo = 앞2 , 뒤2 4Set
            return false;
        }

        public bool FlipperGrip(int GroupNo , int index, bool bFlag, bool bWait = false)        //로테이션 그립 언그립
        {
            bool isSuccess = false;
            //index = -1 이면 전체 동작?
            return isSuccess;
        }
        public bool FlipperRotate(int GroupNo, int index, bool bFlag, bool bWait = false)       //로테이션 회전 동작
        {
            bool isSuccess = false;
            //index = -1 이면 전체 동작?
            return isSuccess;
        }
        public bool FlipperUp(int GroupNo, int index, bool bFlag, bool bWait = false)       //로테이션 상승,하강 동작
        {
            bool isSuccess = false;
            //index = -1 이면 전체 동작?
            return isSuccess;
        }

        public bool ContactPusherUp(int GroupNo, int index, bool bFlag, bool bWait = false)       //컨택 푸셔 상승,하강 동작
        {
            bool isSuccess = false;
            //index = -1 이면 전체 동작?
            return isSuccess;
        }
        public bool ContactPusherFor(int GroupNo, int index, bool bFlag, bool bWait = false)       //컨택 푸셔 전진 후진 동작
        {
            bool isSuccess = false;
            //index = -1 이면 전체 동작?
            return isSuccess;
        }

        public bool MultiContactUp(int SetNo, int[] socketList, bool bFlag, bool bWait = false)
        {
            //SetNo = Socket Set A(0), B(1), C(2), D(3)
            bool isSuccess = false;
            int lModuleNo = 5;
            int lOffset = 0;
            uint uFlagHigh = 0;
            uint uFlagLow = 0;
            int i = 0;
            if (SetNo == 0 || SetNo == 1)
            {
                lModuleNo = 5;
            }
            else
            {
                lModuleNo = 7;
            }
            for (i = 0; i < socketList.Length; i++)
            {
                int nUse = socketList[i];
                if (nUse == 0)
                {
                    continue;
                }

                switch (i)
                {
                    case 0:
                        if(SetNo== 0)
                        {
                            if (bFlag)
                            {
                                uFlagHigh |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH2.OUT5_A_SOCKET_CONTACT_UP1);
                                uFlagLow |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH2.OUT5_A_SOCKET_CONTACT_DOWN1);
                            }
                            else
                            {
                                uFlagLow |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH2.OUT5_A_SOCKET_CONTACT_UP1);
                                uFlagHigh |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH2.OUT5_A_SOCKET_CONTACT_DOWN1);
                            }
                        }
                        else if (SetNo == 1)
                        {
                            if (bFlag)
                            {
                                uFlagHigh |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH2.OUT5_B_SOCKET_CONTACT_UP1);
                                uFlagLow |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH2.OUT5_B_SOCKET_CONTACT_DOWN1);
                            }
                            else
                            {
                                uFlagLow |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH2.OUT5_B_SOCKET_CONTACT_UP1);
                                uFlagHigh |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH2.OUT5_B_SOCKET_CONTACT_DOWN1);
                            }
                        }
                        else if (SetNo == 2)
                        {
                            if (bFlag)
                            {
                                uFlagHigh |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH3.OUT7_C_SOCKET_CONTACT_UP1);
                                uFlagLow |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH3.OUT7_C_SOCKET_CONTACT_DOWN1);
                            }
                            else
                            {
                                uFlagLow |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH3.OUT7_C_SOCKET_CONTACT_UP1);
                                uFlagHigh |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH3.OUT7_C_SOCKET_CONTACT_DOWN1);
                            }
                        }
                        else if (SetNo == 3)
                        {
                            if (bFlag)
                            {
                                uFlagHigh |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH3.OUT7_D_SOCKET_CONTACT_UP1);
                                uFlagLow |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH3.OUT7_D_SOCKET_CONTACT_DOWN1);
                            }
                            else
                            {
                                uFlagLow |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH3.OUT7_D_SOCKET_CONTACT_UP1);
                                uFlagHigh |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH3.OUT7_D_SOCKET_CONTACT_DOWN1);
                            }
                        }


                        break;
                    case 1:
                        if (SetNo == 0)
                        {
                            if (bFlag)
                            {
                                uFlagHigh |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH2.OUT5_A_SOCKET_CONTACT_UP2);
                                uFlagLow |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH2.OUT5_A_SOCKET_CONTACT_DOWN2);
                            }
                            else
                            {
                                uFlagLow |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH2.OUT5_A_SOCKET_CONTACT_UP2);
                                uFlagHigh |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH2.OUT5_A_SOCKET_CONTACT_DOWN2);
                            }
                        }
                        else if (SetNo == 1)
                        {
                            if (bFlag)
                            {
                                uFlagHigh |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH2.OUT5_B_SOCKET_CONTACT_UP2);
                                uFlagLow |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH2.OUT5_B_SOCKET_CONTACT_DOWN2);
                            }
                            else
                            {
                                uFlagLow |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH2.OUT5_B_SOCKET_CONTACT_UP2);
                                uFlagHigh |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH2.OUT5_B_SOCKET_CONTACT_DOWN2);
                            }
                        }
                        else if (SetNo == 2)
                        {
                            if (bFlag)
                            {
                                uFlagHigh |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH3.OUT7_C_SOCKET_CONTACT_UP2);
                                uFlagLow |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH3.OUT7_C_SOCKET_CONTACT_DOWN2);
                            }
                            else
                            {
                                uFlagLow |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH3.OUT7_C_SOCKET_CONTACT_UP2);
                                uFlagHigh |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH3.OUT7_C_SOCKET_CONTACT_DOWN2);
                            }
                        }
                        else if (SetNo == 3)
                        {
                            if (bFlag)
                            {
                                uFlagHigh |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH3.OUT7_C_SOCKET_CONTACT_UP2);
                                uFlagLow |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH3.OUT7_C_SOCKET_CONTACT_DOWN2);
                            }
                            else
                            {
                                uFlagLow |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH3.OUT7_C_SOCKET_CONTACT_UP2);
                                uFlagHigh |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH3.OUT7_C_SOCKET_CONTACT_DOWN2);
                            }
                        }

                        break;
                    case 2:
                        if (SetNo == 0)
                        {
                            if (bFlag)
                            {
                                uFlagHigh |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH2.OUT5_A_SOCKET_CONTACT_UP3);
                                uFlagLow |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH2.OUT5_A_SOCKET_CONTACT_DOWN3);
                            }
                            else
                            {
                                uFlagLow |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH2.OUT5_A_SOCKET_CONTACT_UP3);
                                uFlagHigh |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH2.OUT5_A_SOCKET_CONTACT_DOWN3);
                            }
                        }
                        else if (SetNo == 1)
                        {
                            if (bFlag)
                            {
                                uFlagHigh |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH2.OUT5_A_SOCKET_CONTACT_UP3);
                                uFlagLow |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH2.OUT5_A_SOCKET_CONTACT_DOWN3);
                            }
                            else
                            {
                                uFlagLow |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH2.OUT5_A_SOCKET_CONTACT_UP3);
                                uFlagHigh |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH2.OUT5_A_SOCKET_CONTACT_DOWN3);
                            }
                        }
                        else if (SetNo == 2)
                        {
                            if (bFlag)
                            {
                                uFlagHigh |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH3.OUT7_C_SOCKET_CONTACT_UP3);
                                uFlagLow |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH3.OUT7_C_SOCKET_CONTACT_DOWN3);
                            }
                            else
                            {
                                uFlagLow |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH3.OUT7_C_SOCKET_CONTACT_UP3);
                                uFlagHigh |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH3.OUT7_C_SOCKET_CONTACT_DOWN3);
                            }
                        }
                        else if (SetNo == 3)
                        {
                            if (bFlag)
                            {
                                uFlagHigh |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH3.OUT7_D_SOCKET_CONTACT_UP3);
                                uFlagLow |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH3.OUT7_D_SOCKET_CONTACT_DOWN3);
                            }
                            else
                            {
                                uFlagLow |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH3.OUT7_D_SOCKET_CONTACT_UP3);
                                uFlagHigh |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH3.OUT7_D_SOCKET_CONTACT_DOWN3);
                            }
                        }

                        break;
                    case 3:
                        if (SetNo == 0)
                        {
                            if (bFlag)
                            {
                                uFlagHigh |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH2.OUT5_A_SOCKET_CONTACT_UP4);
                                uFlagLow |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH2.OUT5_A_SOCKET_CONTACT_DOWN4);
                            }
                            else
                            {
                                uFlagLow |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH2.OUT5_A_SOCKET_CONTACT_UP4);
                                uFlagHigh |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH2.OUT5_A_SOCKET_CONTACT_DOWN4);
                            }
                        }
                        else if (SetNo == 1)
                        {
                            if (bFlag)
                            {
                                uFlagHigh |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH2.OUT5_A_SOCKET_CONTACT_UP4);
                                uFlagLow |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH2.OUT5_A_SOCKET_CONTACT_DOWN4);
                            }
                            else
                            {
                                uFlagLow |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH2.OUT5_A_SOCKET_CONTACT_UP4);
                                uFlagHigh |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH2.OUT5_A_SOCKET_CONTACT_DOWN4);
                            }
                        }
                        else if (SetNo == 2)
                        {
                            if (bFlag)
                            {
                                uFlagHigh |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH3.OUT7_C_SOCKET_CONTACT_UP4);
                                uFlagLow |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH3.OUT7_C_SOCKET_CONTACT_DOWN4);
                            }
                            else
                            {
                                uFlagLow |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH3.OUT7_C_SOCKET_CONTACT_UP4);
                                uFlagHigh |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH3.OUT7_C_SOCKET_CONTACT_DOWN4);
                            }
                        }
                        else if (SetNo == 3)
                        {
                            if (bFlag)
                            {
                                uFlagHigh |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH3.OUT7_D_SOCKET_CONTACT_UP4);
                                uFlagLow |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH3.OUT7_D_SOCKET_CONTACT_DOWN4);
                            }
                            else
                            {
                                uFlagLow |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH3.OUT7_D_SOCKET_CONTACT_UP4);
                                uFlagHigh |= (uint)(MotionControl.FwDioDefine.DIO_OUT_ADDR_CH3.OUT7_D_SOCKET_CONTACT_DOWN4);
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
                Console.WriteLine($" Multi ContactUp MOVE FAIL");
                return false;
            }

            return true;
        }
        #endregion
        public override void MovingStop()
        {
            if (CancelToken != null && !CancelToken.IsCancellationRequested)
            {
                CancelToken.Cancel();
            }
        }
        public override bool IsMoving()
        {
            if (AutoUnitThread.GetThreadRun() == true)
            {
                return true;
            }
            return false;
        }
        public override void StopAuto()
        {
            AutoUnitThread.Stop();
            MovingStop();
            RunState = OperationState.Stopped;
            Console.WriteLine($"[INFO] FwSocket Run Stop");

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
                szLog = $"[ORIGIN] Fw Socket Origin Start";
                Console.WriteLine($"[ORIGIN] Fw Socket Origin Start");
                Globalo.LogPrint("MainForm", szLog);
            }
            else
            {
                this.RunState = OperationState.Stopped;
                Console.WriteLine($"[ORIGIN] Fw Socket Origin Start Fail");
                szLog = $"[ORIGIN] Fw Socket Origin Start Fail";
                Globalo.LogPrint("MainForm", szLog);
            }
            return rtn;
        }
        public override bool ReadyRun()
        {
            if (this.RunState != OperationState.Stopped)
            {
                Globalo.LogPrint("MainForm", "[FW SOCKET] 설비 정지상태가 아닙니다.", Globalo.eMessageName.M_WARNING);
                return false;
            }
            if (AutoUnitThread.GetThreadRun() == true)
            {
                Globalo.LogPrint("MainForm", "[FW SOCKET] 설비 정지상태가 아닙니다..", Globalo.eMessageName.M_WARNING);
                return false;
            }
            this.RunState = OperationState.Preparing;   //TODO: 모터없는 부분이라 확인필요
            AutoUnitThread.m_nCurrentStep = 1000;
            //if (TransferX.OrgState == false || TransferY.OrgState == false || TransferZ.OrgState == false)
            //{
            //    this.RunState = OperationState.OriginRunning;
            //    AutoUnitThread.m_nCurrentStep = 1000;
            //}
            //else
            //{
            //    this.RunState = OperationState.Preparing;
            //    AutoUnitThread.m_nCurrentStep = 2000;
            //}

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
                    Globalo.LogPrint("MainForm", "[FW SOCKET] 운전준비가 완료되지 않았습니다.", Globalo.eMessageName.M_WARNING);
                    return false;
                }
            }

            if (AutoUnitThread.GetThreadRun() == true)
            {
                Console.WriteLine($"모터 동작 중입니다.");

                if (AutoUnitThread.GetThreadPause() == true)        //일시 정지 상태인지 확인
                {
                    AutoUnitThread.m_nCurrentStep = Math.Abs(AutoUnitThread.m_nCurrentStep);
                    AutoUnitThread.Resume();
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
                    Console.WriteLine($"FWSOCKET 모터 동작 성공.");
                }
                else
                {
                    Console.WriteLine($"FWSOCKET 모터 동작 실패.");
                }
            }
            return rtn;
        }
    }
    
}
