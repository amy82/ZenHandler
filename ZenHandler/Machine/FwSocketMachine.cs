using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.Machine
{
    public class FwSocketMachine : MotionControl.MotorController
    {
        public event Action<MotionControl.SocketReqArgs, int> OnFwSocketCall;   //Fw 공급요청
        public int MotorCnt { get; private set; } = 0;

        //실린더 전후진 4개
        //실린더 상승,하강 4개

        //소켓 4개씩 4 세트 = 총 16개

        public const string teachingPath = "Teach_FwSocket.yaml";
        public const string taskPath = "Task_FwSocket.yaml";
        //public Data.TeachingConfig teachingConfig = new Data.TeachingConfig();

        public FwSocketProduct socketProduct = new FwSocketProduct();

        public bool[] IsTesting = { false, false, false, false };      //검사 진행중

        public int[] Tester_A_Result = { -1, -1, -1, -1 };
        public int[] Tester_B_Result = { -1, -1, -1, -1 };
        public int[] Tester_C_Result = { -1, -1, -1, -1 };
        public int[] Tester_D_Result = { -1, -1, -1, -1 };
        public FwSocketMachine()
        {
            int i = 0;
            this.RunState = OperationState.Stopped;
            this.MachineName = this.GetType().Name;


            socketProduct = Data.TaskDataYaml.TaskLoad_FwSocket(taskPath);
            for (i = 0; i < 4; i++)
            {
                if (socketProduct.FwSocketInfo[i].Count < 1)
                {
                    socketProduct.FwSocketInfo[i].Add(new FwSocketProductInfo());
                    socketProduct.FwSocketInfo[i].Add(new FwSocketProductInfo());
                    socketProduct.FwSocketInfo[i].Add(new FwSocketProductInfo());
                    socketProduct.FwSocketInfo[i].Add(new FwSocketProductInfo());
                }
            }

        }
        public override bool TaskSave()
        {
            bool rtn = Data.TaskDataYaml.TaskSave_FwSocket(socketProduct, taskPath);
            return rtn;
        }
        public void RaiseProductCall(MotionControl.SocketReqArgs nReq)   //int[] nReq)
        {
            OnFwSocketCall?.Invoke(nReq, -1);
        }
        public override void MotorDataSet()
        {
            //Fw Socket Motor xxxx
        }
        #region Fw Socket Machine Io 동작

        public bool GetIsProductInSocket(int GroupNo,  int index, bool bFlag, bool bWait = false)      //각 소켓의 제품 유무 확인 센서
        {
            //GroupNo = 앞2 , 뒤2 4Set
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            int lModuleNo = 0;
            int lOffset = 0;
            if (GroupNo == 0)
            {
                lModuleNo = 8;
                lOffset = 3;
            }
            if (GroupNo == 1)
            {
                lModuleNo = 10;
                lOffset = 3;
            }
            if (GroupNo == 2)
            {
                lModuleNo = 12;
                lOffset = 3;
            }
            if (GroupNo == 3)
            {
                lModuleNo = 14;
                lOffset = 3;
            }

            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];

            uFlagHigh = upValue & Globalo.motionManager._dio.GetInGoodDetect(GroupNo, index);
            if (uFlagHigh == 1)
            {
                return true;
            }

            return false;
        }
        public bool GetIsContactForward(int GroupNo, int index, bool bFlag, bool bWait = false)      //각 소켓의 푸셔 전/후진 확인 센서
        {
            //GroupNo = 앞2 , 뒤2 4Set
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            int i = 0;
            int lModuleNo = 0;
            int lOffset = 0;
            if (GroupNo == 0)
            {
                lModuleNo = 4;
                lOffset = 1;
            }
            if (GroupNo == 1)
            {
                lModuleNo = 4;
                lOffset = 3;
            }
            if (GroupNo == 2)
            {
                lModuleNo = 6;
                lOffset = 1;
            }
            if (GroupNo == 3)
            {
                lModuleNo = 6;
                lOffset = 3;
            }
            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];


            if (bFlag)
            {
                uFlagHigh |= Globalo.motionManager._dio.GetInContactForBack(GroupNo, index, true);
            }
            else
            {
                uFlagHigh |= Globalo.motionManager._dio.GetInContactForBack(GroupNo, index, false);
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
        public bool GetIsContactUp(int GroupNo, int index, bool bFlag, bool bWait = false)      //각 소켓의 푸셔 상/하강 확인 센서
        {
            //GroupNo = 앞2 , 뒤2 4Set
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            int i = 0;
            int lModuleNo = 0;
            int lOffset = 0;
            if (GroupNo == 0)
            {
                lModuleNo = 4;
                lOffset = 0;
            }
            if (GroupNo == 1)
            {
                lModuleNo = 4;
                lOffset = 2;
            }
            if (GroupNo == 2)
            {
                lModuleNo = 6;
                lOffset = 0;
            }
            if (GroupNo == 3)
            {
                lModuleNo = 6;
                lOffset = 2;
            }
            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];


            if (bFlag)
            {
                uFlagHigh |= Globalo.motionManager._dio.GetInContactUpDown(GroupNo, index, true);
            }
            else
            {
                uFlagHigh |= Globalo.motionManager._dio.GetInContactUpDown(GroupNo, index, false);
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
        public bool GetIsFlipperTurn(int GroupNo, int index, bool bFlag, bool bWait = false)      //각 소켓의 로테이션 회전 상태 확인
        {
            //GroupNo = 앞2 , 뒤2 4Set
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            int i = 0;
            int lModuleNo = 0;
            int lOffset = 0;
            if (GroupNo == 0)
            {
                lModuleNo = 8;
                lOffset = 1;
            }
            if (GroupNo == 1)
            {
                lModuleNo = 10;
                lOffset = 1;
            }

            if (GroupNo == 2)
            {
                lModuleNo = 12;
                lOffset = 1;
            }
            if (GroupNo == 3)
            {
                lModuleNo = 14;
                lOffset = 1;
            }
            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];


            if (bFlag)
            {
                uFlagHigh |= Globalo.motionManager._dio.GetInRotateTurn(GroupNo, index, true);
            }
            else
            {
                uFlagHigh |= Globalo.motionManager._dio.GetInContactUpDown(GroupNo, index, false);
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
        public bool GetIsFlipperUp(int GroupNo, int index, bool bFlag, bool bWait = false)      //각 소켓의 로테이션 실린더 상/하강 상태
        {
            //GroupNo = 앞2 , 뒤2 4Set
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            int i = 0;
            int lModuleNo = 0;
            int lOffset = 0;
            if (GroupNo == 0)
            {
                lModuleNo = 8;
                lOffset = 0;
            }
            if (GroupNo == 1)
            {
                lModuleNo = 10;
                lOffset = 0;
            }

            if (GroupNo == 2)
            {
                lModuleNo = 12;
                lOffset = 0;
            }
            if (GroupNo == 3)
            {
                lModuleNo = 14;
                lOffset = 0;
            }
            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];


            if (bFlag)
            {
                uFlagHigh |= Globalo.motionManager._dio.GetInRotateUpDown(GroupNo, index, true);
            }
            else
            {
                uFlagHigh |= Globalo.motionManager._dio.GetInRotateUpDown(GroupNo, index, false);
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

        public bool FlipperGrip(int GroupNo , int index, bool bFlag, bool bWait = false)        //로테이션 그립 언그립
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            int lModuleNo = 0;
            int lOffset = 0;
            if (GroupNo == 0)
            {
                lModuleNo = 9;
                lOffset = 2;
            }
            if (GroupNo == 1)
            {
                lModuleNo = 11;
                lOffset = 2;
            }

            if (GroupNo == 2)
            {
                lModuleNo = 13;
                lOffset = 2;
            }
            if (GroupNo == 3)
            {
                lModuleNo = 15;
                lOffset = 2;
            }

            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];

            uFlagHigh = upValue & Globalo.motionManager._dio.GetOutRotateGrip(GroupNo, index, bFlag);
            if (uFlagHigh == 1)
            {
                return true;
            }
            return false;
        }
        public bool FlipperTurn(int GroupNo, int index, bool bFlag, bool bWait = false)       //로테이션 회전 동작
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            int lModuleNo = 0;
            int lOffset = 0;
            if (GroupNo == 0)
            {
                lModuleNo = 9;
                lOffset = 1;
            }
            if (GroupNo == 1)
            {
                lModuleNo = 11;
                lOffset = 1;
            }

            if (GroupNo == 2)
            {
                lModuleNo = 13;
                lOffset = 1;
            }
            if (GroupNo == 3)
            {
                lModuleNo = 15;
                lOffset = 1;
            }

            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];

            uFlagHigh = upValue & Globalo.motionManager._dio.GetOutRotateTurn(GroupNo, index, bFlag);
            if (uFlagHigh == 1)
            {
                return true;
            }
            return false;
        }
        public bool FlipperUp(int GroupNo, int index, bool bFlag, bool bWait = false)       //로테이션 상승,하강 동작
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            int lModuleNo = 0;
            int lOffset = 0;
            if (GroupNo == 0)
            {
                lModuleNo = 9;
                lOffset = 0;
            }
            if (GroupNo == 1)
            {
                lModuleNo = 11;
                lOffset = 0;
            }

            if (GroupNo == 2)
            {
                lModuleNo = 13;
                lOffset = 0;
            }
            if (GroupNo == 3)
            {
                lModuleNo = 15;
                lOffset = 0;
            }

            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];

            uFlagHigh = upValue & Globalo.motionManager._dio.GetOutRotateUpDown(GroupNo, index, bFlag);
            if (uFlagHigh == 1)
            {
                return true;
            }
            return false;
        }

        public bool ContactPusherUp(int GroupNo, int index, bool bFlag, bool bWait = false)       //컨택 푸셔 상승,하강 동작
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            int lModuleNo = 0;
            int lOffset = 0;
            if (GroupNo == 0)
            {
                lModuleNo = 5;
                lOffset = 0;
            }
            if (GroupNo == 1)
            {
                lModuleNo = 5;
                lOffset = 2;
            }

            if (GroupNo == 2)
            {
                lModuleNo = 7;
                lOffset = 0;
            }
            if (GroupNo == 3)
            {
                lModuleNo = 7;
                lOffset = 2;
            }

            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];

            uFlagHigh = upValue & Globalo.motionManager._dio.GetOutContactUpDown(GroupNo, index, bFlag);
            if (uFlagHigh == 1)
            {
                return true;
            }
            return false;
        }
        public bool ContactPusherFor(int GroupNo, int index, bool bFlag, bool bWait = false)       //컨택 푸셔 전진 후진 동작
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                return true;
            }
            int lModuleNo = 0;
            int lOffset = 0;
            if (GroupNo == 0)
            {
                lModuleNo = 5;
                lOffset = 1;
            }
            if (GroupNo == 1)
            {
                lModuleNo = 5;
                lOffset = 3;
            }

            if (GroupNo == 2)
            {
                lModuleNo = 7;
                lOffset = 1;
            }
            if (GroupNo == 3)
            {
                lModuleNo = 7;
                lOffset = 3;
            }

            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];

            uFlagHigh = upValue & Globalo.motionManager._dio.GetOutContactForBack(GroupNo, index, bFlag);
            if (uFlagHigh == 1)
            {
                return true;
            }
            return false;
        }
        public bool MultiContactFor(int GroupNo, bool bFlag, bool bWait = false)
        {
            bool isSuccess = false;
            uint uFlagHigh = 0;
            uint uFlagLow = 0;

            int lModuleNo = 0;
            int lOffset = 0;
            if (GroupNo == 0)
            {
                lModuleNo = 5;
                lOffset = 1;
            }
            if (GroupNo == 1)
            {
                lModuleNo = 5;
                lOffset = 3;
            }

            if (GroupNo == 2)
            {
                lModuleNo = 7;
                lOffset = 1;
            }
            if (GroupNo == 3)
            {
                lModuleNo = 7;
                lOffset = 3;
            }

            if (bFlag)
            {
                uFlagHigh |= Globalo.motionManager._dio.GetOutContactForBack(GroupNo, 0, true);
                uFlagHigh |= Globalo.motionManager._dio.GetOutContactForBack(GroupNo, 1, true);
                uFlagHigh |= Globalo.motionManager._dio.GetOutContactForBack(GroupNo, 2, true);
                uFlagHigh |= Globalo.motionManager._dio.GetOutContactForBack(GroupNo, 3, true);

                uFlagLow |= Globalo.motionManager._dio.GetOutContactForBack(GroupNo, 0, false);
                uFlagLow |= Globalo.motionManager._dio.GetOutContactForBack(GroupNo, 1, false);
                uFlagLow |= Globalo.motionManager._dio.GetOutContactForBack(GroupNo, 2, false);
                uFlagLow |= Globalo.motionManager._dio.GetOutContactForBack(GroupNo, 3, false);
            }
            else
            {
                uFlagHigh |= Globalo.motionManager._dio.GetOutContactForBack(GroupNo, 0, false);
                uFlagHigh |= Globalo.motionManager._dio.GetOutContactForBack(GroupNo, 1, false);
                uFlagHigh |= Globalo.motionManager._dio.GetOutContactForBack(GroupNo, 2, false);
                uFlagHigh |= Globalo.motionManager._dio.GetOutContactForBack(GroupNo, 3, false);

                uFlagLow |= Globalo.motionManager._dio.GetOutContactForBack(GroupNo, 0, true);
                uFlagLow |= Globalo.motionManager._dio.GetOutContactForBack(GroupNo, 1, true);
                uFlagLow |= Globalo.motionManager._dio.GetOutContactForBack(GroupNo, 2, true);
                uFlagLow |= Globalo.motionManager._dio.GetOutContactForBack(GroupNo, 3, true);
            }

            isSuccess = Globalo.motionManager.ioController.DioWriteOutportByte(lModuleNo, lOffset, uFlagHigh, uFlagLow);
            if (isSuccess == false)
            {
                Console.WriteLine($" MultiContactUp MOVE FAIL");
                return isSuccess;
            }

            return isSuccess;
        }
        public bool MultiFlipperTurn(int GroupNo, bool bFlag, bool bWait = false)
        {
            bool isSuccess = false;
            uint uFlagHigh = 0;
            uint uFlagLow = 0;

            int lModuleNo = 0;
            int lOffset = 0;
            if (GroupNo == 0)
            {
                lModuleNo = 9;
                lOffset = 1;
            }
            if (GroupNo == 1)
            {
                lModuleNo = 11;
                lOffset = 1;
            }

            if (GroupNo == 2)
            {
                lModuleNo = 13;
                lOffset = 1;
            }
            if (GroupNo == 3)
            {
                lModuleNo = 15;
                lOffset = 1;
            }

            if (bFlag)
            {
                uFlagHigh |= Globalo.motionManager._dio.GetOutRotateTurn(GroupNo, 0, true);
                uFlagHigh |= Globalo.motionManager._dio.GetOutRotateTurn(GroupNo, 1, true);
                uFlagHigh |= Globalo.motionManager._dio.GetOutRotateTurn(GroupNo, 2, true);
                uFlagHigh |= Globalo.motionManager._dio.GetOutRotateTurn(GroupNo, 3, true);

                uFlagLow |= Globalo.motionManager._dio.GetOutRotateTurn(GroupNo, 0, false);
                uFlagLow |= Globalo.motionManager._dio.GetOutRotateTurn(GroupNo, 1, false);
                uFlagLow |= Globalo.motionManager._dio.GetOutRotateTurn(GroupNo, 2, false);
                uFlagLow |= Globalo.motionManager._dio.GetOutRotateTurn(GroupNo, 3, false);
            }
            else
            {
                uFlagHigh |= Globalo.motionManager._dio.GetOutRotateTurn(GroupNo, 0, false);
                uFlagHigh |= Globalo.motionManager._dio.GetOutRotateTurn(GroupNo, 1, false);
                uFlagHigh |= Globalo.motionManager._dio.GetOutRotateTurn(GroupNo, 2, false);
                uFlagHigh |= Globalo.motionManager._dio.GetOutRotateTurn(GroupNo, 3, false);

                uFlagLow |= Globalo.motionManager._dio.GetOutRotateTurn(GroupNo, 0, true);
                uFlagLow |= Globalo.motionManager._dio.GetOutRotateTurn(GroupNo, 1, true);
                uFlagLow |= Globalo.motionManager._dio.GetOutRotateTurn(GroupNo, 2, true);
                uFlagLow |= Globalo.motionManager._dio.GetOutRotateTurn(GroupNo, 3, true);
            }

            isSuccess = Globalo.motionManager.ioController.DioWriteOutportByte(lModuleNo, lOffset, uFlagHigh, uFlagLow);
            if (isSuccess == false)
            {
                Console.WriteLine($" MultiContactUp MOVE FAIL");
                return isSuccess;
            }

            return isSuccess;
        }
        public bool MultiFlipperUp(int GroupNo, bool bFlag, bool bWait = false)
        {
            bool isSuccess = false;
            uint uFlagHigh = 0;
            uint uFlagLow = 0;

            int lModuleNo = 0;
            int lOffset = 0;
            if (GroupNo == 0)
            {
                lModuleNo = 9;
                lOffset = 0;
            }
            if (GroupNo == 1)
            {
                lModuleNo = 11;
                lOffset = 0;
            }

            if (GroupNo == 2)
            {
                lModuleNo = 13;
                lOffset = 0;
            }
            if (GroupNo == 3)
            {
                lModuleNo = 15;
                lOffset = 0;
            }

            if (bFlag)
            {
                uFlagHigh |= Globalo.motionManager._dio.GetOutRotateUpDown(GroupNo, 0, true);
                uFlagHigh |= Globalo.motionManager._dio.GetOutRotateUpDown(GroupNo, 1, true);
                uFlagHigh |= Globalo.motionManager._dio.GetOutRotateUpDown(GroupNo, 2, true);
                uFlagHigh |= Globalo.motionManager._dio.GetOutRotateUpDown(GroupNo, 3, true);

                uFlagLow |= Globalo.motionManager._dio.GetOutRotateUpDown(GroupNo, 0, false);
                uFlagLow |= Globalo.motionManager._dio.GetOutRotateUpDown(GroupNo, 1, false);
                uFlagLow |= Globalo.motionManager._dio.GetOutRotateUpDown(GroupNo, 2, false);
                uFlagLow |= Globalo.motionManager._dio.GetOutRotateUpDown(GroupNo, 3, false);
            }
            else
            {
                uFlagHigh |= Globalo.motionManager._dio.GetOutRotateUpDown(GroupNo, 0, false);
                uFlagHigh |= Globalo.motionManager._dio.GetOutRotateUpDown(GroupNo, 1, false);
                uFlagHigh |= Globalo.motionManager._dio.GetOutRotateUpDown(GroupNo, 2, false);
                uFlagHigh |= Globalo.motionManager._dio.GetOutRotateUpDown(GroupNo, 3, false);

                uFlagLow |= Globalo.motionManager._dio.GetOutRotateUpDown(GroupNo, 0, true);
                uFlagLow |= Globalo.motionManager._dio.GetOutRotateUpDown(GroupNo, 1, true);
                uFlagLow |= Globalo.motionManager._dio.GetOutRotateUpDown(GroupNo, 2, true);
                uFlagLow |= Globalo.motionManager._dio.GetOutRotateUpDown(GroupNo, 3, true);
            }

            isSuccess = Globalo.motionManager.ioController.DioWriteOutportByte(lModuleNo, lOffset, uFlagHigh, uFlagLow);
            if (isSuccess == false)
            {
                Console.WriteLine($" MultiFlipperUp MOVE FAIL");
                return isSuccess;
            }

            return isSuccess;
        }
        public bool MultiFlipperGrip(int GroupNo, bool bFlag, bool bWait = false)
        {
            bool isSuccess = false;
            uint uFlagHigh = 0;
            uint uFlagLow = 0;

            int lModuleNo = 0;
            int lOffset = 0;
            if (GroupNo == 0)
            {
                lModuleNo = 9;
                lOffset = 2;
            }
            if (GroupNo == 1)
            {
                lModuleNo = 11;
                lOffset = 2;
            }

            if (GroupNo == 2)
            {
                lModuleNo = 13;
                lOffset = 2;
            }
            if (GroupNo == 3)
            {
                lModuleNo = 15;
                lOffset = 2;
            }

            if (bFlag)
            {
                uFlagHigh |= Globalo.motionManager._dio.GetOutRotateGrip(GroupNo, 0, true);
                uFlagHigh |= Globalo.motionManager._dio.GetOutRotateGrip(GroupNo, 1, true);
                uFlagHigh |= Globalo.motionManager._dio.GetOutRotateGrip(GroupNo, 2, true);
                uFlagHigh |= Globalo.motionManager._dio.GetOutRotateGrip(GroupNo, 3, true);

                uFlagLow |= Globalo.motionManager._dio.GetOutRotateGrip(GroupNo, 0, false);
                uFlagLow |= Globalo.motionManager._dio.GetOutRotateGrip(GroupNo, 1, false);
                uFlagLow |= Globalo.motionManager._dio.GetOutRotateGrip(GroupNo, 2, false);
                uFlagLow |= Globalo.motionManager._dio.GetOutRotateGrip(GroupNo, 3, false);
            }
            else
            {
                uFlagHigh |= Globalo.motionManager._dio.GetOutRotateGrip(GroupNo, 0, false);
                uFlagHigh |= Globalo.motionManager._dio.GetOutRotateGrip(GroupNo, 1, false);
                uFlagHigh |= Globalo.motionManager._dio.GetOutRotateGrip(GroupNo, 2, false);
                uFlagHigh |= Globalo.motionManager._dio.GetOutRotateGrip(GroupNo, 3, false);

                uFlagLow |= Globalo.motionManager._dio.GetOutRotateGrip(GroupNo, 0, true);
                uFlagLow |= Globalo.motionManager._dio.GetOutRotateGrip(GroupNo, 1, true);
                uFlagLow |= Globalo.motionManager._dio.GetOutRotateGrip(GroupNo, 2, true);
                uFlagLow |= Globalo.motionManager._dio.GetOutRotateGrip(GroupNo, 3, true);
            }

            isSuccess = Globalo.motionManager.ioController.DioWriteOutportByte(lModuleNo, lOffset, uFlagHigh, uFlagLow);
            if (isSuccess == false)
            {
                Console.WriteLine($" MultiFlipperUp MOVE FAIL");
                return isSuccess;
            }

            return isSuccess;
        }

        public bool MultiContactUp(int GroupNo,  bool bFlag, bool bWait = false)//int[] socketList,
        {
            bool isSuccess = false;
            uint uFlagHigh = 0;
            uint uFlagLow = 0;

            int lModuleNo = 0;
            int lOffset = 0;
            if (GroupNo == 0)
            {
                lModuleNo = 5;
                lOffset = 0;
            }
            if (GroupNo == 1)
            {
                lModuleNo = 5;
                lOffset = 2;
            }

            if (GroupNo == 2)
            {
                lModuleNo = 7;
                lOffset = 0;
            }
            if (GroupNo == 3)
            {
                lModuleNo = 7;
                lOffset = 2;
            }

            if (bFlag)
            {
                uFlagHigh |= Globalo.motionManager._dio.GetOutContactUpDown(GroupNo, 0, true);
                uFlagHigh |= Globalo.motionManager._dio.GetOutContactUpDown(GroupNo, 1, true);
                uFlagHigh |= Globalo.motionManager._dio.GetOutContactUpDown(GroupNo, 2, true);
                uFlagHigh |= Globalo.motionManager._dio.GetOutContactUpDown(GroupNo, 3, true);

                uFlagLow |= Globalo.motionManager._dio.GetOutContactUpDown(GroupNo, 0, false);
                uFlagLow |= Globalo.motionManager._dio.GetOutContactUpDown(GroupNo, 1, false);
                uFlagLow |= Globalo.motionManager._dio.GetOutContactUpDown(GroupNo, 2, false);
                uFlagLow |= Globalo.motionManager._dio.GetOutContactUpDown(GroupNo, 3, false);
            }
            else
            {
                uFlagHigh |= Globalo.motionManager._dio.GetOutContactUpDown(GroupNo, 0, false);
                uFlagHigh |= Globalo.motionManager._dio.GetOutContactUpDown(GroupNo, 1, false);
                uFlagHigh |= Globalo.motionManager._dio.GetOutContactUpDown(GroupNo, 2, false);
                uFlagHigh |= Globalo.motionManager._dio.GetOutContactUpDown(GroupNo, 3, false);

                uFlagLow |= Globalo.motionManager._dio.GetOutContactUpDown(GroupNo, 0, true);
                uFlagLow |= Globalo.motionManager._dio.GetOutContactUpDown(GroupNo, 1, true);
                uFlagLow |= Globalo.motionManager._dio.GetOutContactUpDown(GroupNo, 2, true);
                uFlagLow |= Globalo.motionManager._dio.GetOutContactUpDown(GroupNo, 3, true);
            }

            isSuccess = Globalo.motionManager.ioController.DioWriteOutportByte(lModuleNo, lOffset, uFlagHigh, uFlagLow);
            if (isSuccess == false)
            {
                Console.WriteLine($" MultiFlipperUp MOVE FAIL");
                return isSuccess;
            }

            return isSuccess;
        }
        public bool GetMultiContactUp(int GroupNo, bool bFlag, bool bWait = false)
        {
            //GroupNo = 앞2 , 뒤2 4Set
            int i = 0;
            int lModuleNo = 0;
            int lOffset = 0;
            if (GroupNo == 0)
            {
                lModuleNo = 4;
                lOffset = 0;
            }
            if (GroupNo == 1)
            {
                lModuleNo = 4;
                lOffset = 2;
            }
            if (GroupNo == 2)
            {
                lModuleNo = 6;
                lOffset = 0;
            }
            if (GroupNo == 3)
            {
                lModuleNo = 6;
                lOffset = 2;
            }
            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];


            if (bFlag)
            {
                uFlagHigh |= Globalo.motionManager._dio.GetInContactUpDown(GroupNo, 0, true);
                uFlagHigh |= Globalo.motionManager._dio.GetInContactUpDown(GroupNo, 1, true);
                uFlagHigh |= Globalo.motionManager._dio.GetInContactUpDown(GroupNo, 2, true);
                uFlagHigh |= Globalo.motionManager._dio.GetInContactUpDown(GroupNo, 3, true);
            }
            else
            {
                uFlagHigh |= Globalo.motionManager._dio.GetInContactUpDown(GroupNo, 0, false);
                uFlagHigh |= Globalo.motionManager._dio.GetInContactUpDown(GroupNo, 1, false);
                uFlagHigh |= Globalo.motionManager._dio.GetInContactUpDown(GroupNo, 2, false);
                uFlagHigh |= Globalo.motionManager._dio.GetInContactUpDown(GroupNo, 3, false);
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
        public bool GetMultiContactFor(int GroupNo, bool bFlag, bool bWait = false)
        {
            //GroupNo = 앞2 , 뒤2 4Set
            int i = 0;
            int lModuleNo = 0;
            int lOffset = 0;
            if (GroupNo == 0)
            {
                lModuleNo = 4;
                lOffset = 1;
            }
            if (GroupNo == 1)
            {
                lModuleNo = 4;
                lOffset = 3;
            }
            if (GroupNo == 2)
            {
                lModuleNo = 6;
                lOffset = 1;
            }
            if (GroupNo == 3)
            {
                lModuleNo = 6;
                lOffset = 3;
            }
            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];


            if (bFlag)
            {
                uFlagHigh |= Globalo.motionManager._dio.GetInContactForBack(GroupNo, 0, true);
                uFlagHigh |= Globalo.motionManager._dio.GetInContactForBack(GroupNo, 1, true);
                uFlagHigh |= Globalo.motionManager._dio.GetInContactForBack(GroupNo, 2, true);
                uFlagHigh |= Globalo.motionManager._dio.GetInContactForBack(GroupNo, 3, true);
            }
            else
            {
                uFlagHigh |= Globalo.motionManager._dio.GetInContactForBack(GroupNo, 0, false);
                uFlagHigh |= Globalo.motionManager._dio.GetInContactForBack(GroupNo, 1, false);
                uFlagHigh |= Globalo.motionManager._dio.GetInContactForBack(GroupNo, 2, false);
                uFlagHigh |= Globalo.motionManager._dio.GetInContactForBack(GroupNo, 3, false);
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
        public bool GetMultiFlipperUp(int GroupNo, bool bFlag, bool bWait = false)
        {
            //GroupNo = 앞2 , 뒤2 4Set
            int i = 0;
            int lModuleNo = 0;
            int lOffset = 0;
            if (GroupNo == 0)
            {
                lModuleNo = 8;
                lOffset = 1;
            }
            if (GroupNo == 1)
            {
                lModuleNo = 10;
                lOffset = 1;
            }
            if (GroupNo == 2)
            {
                lModuleNo = 12;
                lOffset = 1;
            }
            if (GroupNo == 3)
            {
                lModuleNo = 14;
                lOffset = 1;
            }
            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];


            if (bFlag)
            {
                uFlagHigh |= Globalo.motionManager._dio.GetInRotateUpDown(GroupNo, 0, true);
                uFlagHigh |= Globalo.motionManager._dio.GetInRotateUpDown(GroupNo, 1, true);
                uFlagHigh |= Globalo.motionManager._dio.GetInRotateUpDown(GroupNo, 2, true);
                uFlagHigh |= Globalo.motionManager._dio.GetInRotateUpDown(GroupNo, 3, true);
            }
            else
            {
                uFlagHigh |= Globalo.motionManager._dio.GetInRotateUpDown(GroupNo, 0, false);
                uFlagHigh |= Globalo.motionManager._dio.GetInRotateUpDown(GroupNo, 1, false);
                uFlagHigh |= Globalo.motionManager._dio.GetInRotateUpDown(GroupNo, 2, false);
                uFlagHigh |= Globalo.motionManager._dio.GetInRotateUpDown(GroupNo, 3, false);
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
        public bool GetMultiFlipperTurn(int GroupNo, bool bFlag, bool bWait = false)
        {
            //GroupNo = 앞2 , 뒤2 4Set
            int i = 0;
            int lModuleNo = 0;
            int lOffset = 0;
            if (GroupNo == 0)
            {
                lModuleNo = 8;
                lOffset = 1;
            }
            if (GroupNo == 1)
            {
                lModuleNo = 10;
                lOffset = 1;
            }
            if (GroupNo == 2)
            {
                lModuleNo = 12;
                lOffset = 1;
            }
            if (GroupNo == 3)
            {
                lModuleNo = 14;
                lOffset = 1;
            }
            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];


            if (bFlag)
            {
                uFlagHigh |= Globalo.motionManager._dio.GetInRotateTurn(GroupNo, 0, true);
                uFlagHigh |= Globalo.motionManager._dio.GetInRotateTurn(GroupNo, 1, true);
                uFlagHigh |= Globalo.motionManager._dio.GetInRotateTurn(GroupNo, 2, true);
                uFlagHigh |= Globalo.motionManager._dio.GetInRotateTurn(GroupNo, 3, true);
            }
            else
            {
                uFlagHigh |= Globalo.motionManager._dio.GetInRotateTurn(GroupNo, 0, false);
                uFlagHigh |= Globalo.motionManager._dio.GetInRotateTurn(GroupNo, 1, false);
                uFlagHigh |= Globalo.motionManager._dio.GetInRotateTurn(GroupNo, 2, false);
                uFlagHigh |= Globalo.motionManager._dio.GetInRotateTurn(GroupNo, 3, false);
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
        public bool GetMultiFlipperGrip(int GroupNo, bool bFlag, bool bWait = false)
        {
            //GroupNo = 앞2 , 뒤2 4Set
            int i = 0;
            int lModuleNo = 0;
            int lOffset = 0;
            if (GroupNo == 0)
            {
                lModuleNo = 8;
                lOffset = 2;
            }
            if (GroupNo == 1)
            {
                lModuleNo = 10;
                lOffset = 2;
            }
            if (GroupNo == 2)
            {
                lModuleNo = 12;
                lOffset = 2;
            }
            if (GroupNo == 3)
            {
                lModuleNo = 14;
                lOffset = 2;
            }
            uint uFlagHigh = 0;
            uint upValue = Globalo.motionManager.ioController.m_dwDInDict[lModuleNo][lOffset];


            if (bFlag)
            {
                uFlagHigh |= Globalo.motionManager._dio.GetInRotateGrip(GroupNo, 0, true);
                uFlagHigh |= Globalo.motionManager._dio.GetInRotateGrip(GroupNo, 1, true);
                uFlagHigh |= Globalo.motionManager._dio.GetInRotateGrip(GroupNo, 2, true);
                uFlagHigh |= Globalo.motionManager._dio.GetInRotateGrip(GroupNo, 3, true);
            }
            else
            {
                uFlagHigh |= Globalo.motionManager._dio.GetInRotateGrip(GroupNo, 0, false);
                uFlagHigh |= Globalo.motionManager._dio.GetInRotateGrip(GroupNo, 1, false);
                uFlagHigh |= Globalo.motionManager._dio.GetInRotateGrip(GroupNo, 2, false);
                uFlagHigh |= Globalo.motionManager._dio.GetInRotateGrip(GroupNo, 3, false);
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
                    AutoUnitThread.m_nSocketStep[0] = Math.Abs(AutoUnitThread.m_nSocketStep[0]);
                    AutoUnitThread.m_nSocketStep[1] = Math.Abs(AutoUnitThread.m_nSocketStep[1]);
                    AutoUnitThread.m_nSocketStep[2] = Math.Abs(AutoUnitThread.m_nSocketStep[2]);
                    AutoUnitThread.m_nSocketStep[3] = Math.Abs(AutoUnitThread.m_nSocketStep[3]);
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
                AutoUnitThread.m_nSocketStep[0] = 100;
                AutoUnitThread.m_nSocketStep[1] = 100;
                AutoUnitThread.m_nSocketStep[2] = 100;
                AutoUnitThread.m_nSocketStep[3] = 100;
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
