using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZenHandler.MotionControl
{
    
    public class MotionManager
    {
        public IOController ioController;       //io 동작

        public int m_lAxisCounts = 0;                          // 제어 가능한 축갯수 선언 및 초기화
        public bool bConnected = false;

        public Machine.TransferMachine transferMachine;
        public Machine.MagazineHandler magazineHandler;
        public Machine.LiftMachine liftMachine;


        //SOCKET MACHINE
        //TODO: Socket 머신 하나두고 , 그 아래 소켓 Set마트 Class 추가?
        public Machine.AoiSocketMachine socketAoiMachine;
        public Machine.EEpromSocketMachine socketEEpromMachine;
        public Machine.FwSocketMachine socketFwMachine;

        public int SocketSetCount = 2;      //or 4(fw)
        private bool[] trayEjectRequested = {false , false };

        private int[] socketA_Req_State = {-1, -1, -1, -1 };        //-1 = 초기화 , 0 = 공급 완료, 1 = 공급요청 ,  2 = 배출요청
        private int[] socketB_Requested = { -1, -1, -1, -1 };       //-1 = 초기화 , 0 = 공급 완료, 1 = 공급요청 ,  2 = 배출요청
        private int[] socketC_Requested = { -1, -1, -1, -1 };       //-1 = 초기화 , 0 = 공급 완료, 1 = 공급요청 ,  2 = 배출요청
        private int[] socketD_Requested = { -1, -1, -1, -1 };       //-1 = 초기화 , 0 = 공급 완료, 1 = 공급요청 ,  2 = 배출요청




        //TODO: Set 라서 4개인데 , 개별이면 달라진다. -
        //펌웨어는 Set 로 요청 - 4개씩 4Set
        //EEPROM는 Set 로 요청 - 4개씩 2Set
        //AOI는 Set 로 요청 - 2개씩 2Set


        #region test
        //test 1
        //test 2
        #endregion


        public MotionManager()
        {

            Event.EventManager.PgExitCall += OnPgExit;
            ioController = new IOController();

            transferMachine = new Machine.TransferMachine();        //TODO: motor , io 모두 설정되고나서 해야될수도
            magazineHandler = new Machine.MagazineHandler();
            liftMachine = new Machine.LiftMachine();
            socketAoiMachine = new Machine.AoiSocketMachine();
            socketEEpromMachine = new Machine.EEpromSocketMachine();
            socketFwMachine = new Machine.FwSocketMachine();

            transferMachine.OnTrayChangedCall += OnTrayChengeReq;
            transferMachine.OnSocketReqComplete += OnSocketLoadReq;     //공급, 배출 완료 0으로 변경


            socketEEpromMachine.OnSocketCall += OnSocketLoadReq;        //공급, 배출 요청 1 or 2

            bool LoadChk = true;
            LoadChk = transferMachine.teachingConfig.LoadTeach(Machine.TransferMachine.teachingPath, transferMachine.MotorCnt, (int)Machine.TransferMachine.eTeachingPosList.TOTAL_TRANSFER_TEACHING_COUNT);   //TODO: 티칭 개수만큼 불러와야되는데 파일에 없으면 못 불러온다
            transferMachine.MotorUse = LoadChk;
            LoadChk = magazineHandler.teachingConfig.LoadTeach(Machine.MagazineHandler.teachingPath, magazineHandler.MotorCnt, (int)Machine.MagazineHandler.eTeachingPosList.TOTAL_MAGAZINE_TEACHING_COUNT);
            magazineHandler.MotorUse = LoadChk;
            LoadChk = liftMachine.teachingConfig.LoadTeach(Machine.LiftMachine.teachingPath, liftMachine.MotorCnt, (int)Machine.LiftMachine.eTeachingPosList.TOTAL_LIFT_TEACHING_COUNT);
            liftMachine.MotorUse = LoadChk;
            LoadChk = socketAoiMachine.teachingConfig.LoadTeach(Machine.AoiSocketMachine.teachingPath, socketAoiMachine.MotorCnt, (int)Machine.AoiSocketMachine.eTeachingAoiPosList.TOTAL_AOI_SOCKET_TEACHING_COUNT);
            socketAoiMachine.MotorUse = LoadChk;
            LoadChk = socketEEpromMachine.teachingConfig.LoadTeach(Machine.EEpromSocketMachine.teachingPath, socketEEpromMachine.MotorCnt, (int)Machine.EEpromSocketMachine.eTeachingPosList.TOTAL_SOCKET_TEACHING_COUNT);
            socketEEpromMachine.MotorUse = LoadChk;


            ClearTrayChange(MotorSet.TrayPos.Left);
            ClearTrayChange(MotorSet.TrayPos.Right);

            if (Program.PG_SELECT == HANDLER_PG.FW)
            {
                SocketSetCount = 4;
            }
                
            //FwSocket = Teaching 없음
        }
        private void OnTrayChengeReq(MotorSet.TrayPos position)
        {
            Console.WriteLine($"ToLiftUnitTrayChenge - {position}");

            //Magazine - LEFT , RIGHT 모두 사용
            //Lift - 우측 리프트에서만 배출

            int index = (int)position;
            trayEjectRequested[index] = true;
        }
        public void ClearTrayChange(MotorSet.TrayPos position)
        {
            Console.WriteLine($"ClearTrayChange - {position}");
            int index = (int)position;
            trayEjectRequested[index] = false;
        }
        public bool GetTrayEjectReq(MotorSet.TrayPos position)
        {
            int index = (int)position;
            bool rtn = false;
            rtn = trayEjectRequested[index];
            return rtn;

        }
        //---------------------------------------------------------------------------------------------------------
        //
        //
        //  소켓 공급 / 배출 요청
        //
        //---------------------------------------------------------------------------------------------------------
        private void OnSocketLoadReq(int index, int[] nReq)          //소켓에서 투입 요청
        {
            Console.WriteLine($"OnSocketLoadReq - {index},{nReq}");
            if (index == 0)
            {
                socketA_Req_State = (int[])nReq.Clone();
            }
            else
            {
                socketB_Requested = (int[])nReq.Clone();
            }
            
        }
        public int[] GetSocketReq(int index)//public int GetSocketEjectReq(int index)
        {
            if (index == 0)
            {
                return socketA_Req_State;
            }
            if (index == 1)
            {
                return socketB_Requested;
            }
            if (index == 2)
            {
                return socketC_Requested;
            }
            else
            {
                return socketD_Requested;
            }
            //int nRtn = -1;
            //if (index == 0)
            //{
            //    nRtn = socketA_Requested[no];
            //}
            //if (index == 1)
            //{
            //    nRtn = socketB_Requested[no];
            //}
            //if (index == 2)
            //{
            //    nRtn = socketC_Requested[no];
            //}
            //else
            //{
            //    nRtn = socketD_Requested[no];
            //}
            //return nRtn;
        }
        //public int GetSocketLoadReq(int index)//public int GetSocketLoadReq(int index)
        //{
        //    int rtn = socketA_Requested[index];
        //    return rtn;
        //}

        //---------------------------------------------------------------------------------------------------------
        private void OnPgExit(object sender, EventArgs e)
        {
            Console.WriteLine("MotionManager - OnPgExit");
            transferMachine.StopAuto();
            magazineHandler.StopAuto();
            liftMachine.StopAuto();
            socketAoiMachine.StopAuto();
            socketEEpromMachine.StopAuto();
            socketFwMachine.StopAuto();
            

            transferMachine.MachineClose();
            magazineHandler.MachineClose();
            liftMachine.MachineClose();
            socketAoiMachine.MachineClose();
            socketEEpromMachine.MachineClose();

            socketFwMachine.MachineClose();

        }
        public void AllMotorParameterSet()
        {
            transferMachine.MotorDataSet();
            magazineHandler.MotorDataSet();
            liftMachine.MotorDataSet();
            socketAoiMachine.MotorDataSet();
            socketEEpromMachine.MotorDataSet();
            socketFwMachine.MotorDataSet();
        }
        public void AllMotorStop()
        {
            if (ProgramState.ON_LINE_MOTOR)
            {
                transferMachine.StopAuto();
                magazineHandler.StopAuto();
                liftMachine.StopAuto();
                socketAoiMachine.StopAuto();
                socketEEpromMachine.StopAuto();
                socketFwMachine.StopAuto();
            }
           
        }

        public void MotionClose()
        {
            Axl_Close();
            ioController.Close();
        }

        public bool MotionInit()
        {
            bool bAxlInit = true;
            if (CAXL.AxlOpen(7) == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                //초기화 성공
                bConnected = true;

                bAxlInit = Axl_Init();

                if(bAxlInit == false)
                {
                    return false;
                }
                ioController.DioInit();
                int length = 0;

                //------------------------------------------------------------------------------------------------------------
                //
                // TRANSFER UNIT
                //
                //
                //------------------------------------------------------------------------------------------------------------
                if (transferMachine.MotorUse)
                {
                    length = transferMachine.MotorAxes.Length;
                    for (int i = 0; i < length; i++)
                    {
                        if (New_Axisconfig(transferMachine.MotorAxes[i]) > 0)   //0 = ok
                        {
                            //fail
                            bAxlInit = false;
                        }
                    }
                }
                
                //------------------------------------------------------------------------------------------------------------
                //
                // AOI SOCKET UNIT
                //
                //
                //------------------------------------------------------------------------------------------------------------
                if(socketAoiMachine.MotorUse)
                {
                    length = socketAoiMachine.MotorAxes.Length;
                    for (int i = 0; i < length; i++)
                    {
                        if (New_Axisconfig(socketAoiMachine.MotorAxes[i]) > 0)   //0 = ok
                        {
                            //fail
                            bAxlInit = false;
                        }
                    }
                }
                //------------------------------------------------------------------------------------------------------------
                //
                // EEPROM SOCKET UNIT
                //
                //
                //------------------------------------------------------------------------------------------------------------

                if (socketEEpromMachine.MotorUse)
                {
                    length = socketEEpromMachine.MotorAxes.Length;
                    for (int i = 0; i < length; i++)
                    {
                        if (New_Axisconfig(socketEEpromMachine.MotorAxes[i]) > 0)   //0 = ok
                        {
                            //fail
                            bAxlInit = false;
                        }
                    }
                }
                //------------------------------------------------------------------------------------------------------------
                //
                // FW SOCKET UNIT
                //
                //
                //------------------------------------------------------------------------------------------------------------

                //socketFwMachine = Motor XXXXX 없음

                //------------------------------------------------------------------------------------------------------------
                //
                // MAGAZINE UNIT
                //
                //
                //------------------------------------------------------------------------------------------------------------
                if (magazineHandler.MotorUse)
                {
                    length = magazineHandler.MotorAxes.Length;
                    for (int i = 0; i < length; i++)
                    {
                        if (New_Axisconfig(magazineHandler.MotorAxes[i]) > 0)   //0 = ok
                        {
                            //fail
                            bAxlInit = false;
                        }
                    }
                }
                
                //------------------------------------------------------------------------------------------------------------
                //
                // LIFT UNIT
                //
                //
                //------------------------------------------------------------------------------------------------------------
                if(liftMachine.MotorUse)
                {
                    length = liftMachine.MotorAxes.Length;
                    for (int i = 0; i < length; i++)
                    {
                        if (New_Axisconfig(liftMachine.MotorAxes[i]) > 0)   //0 = ok
                        {
                            //fail
                            bAxlInit = false;
                        }
                    }
                }
            }
            else
            {
                bAxlInit = false;
                Globalo.LogPrint("", "Motion Intialize Fail..!!", Globalo.eMessageName.M_ERROR);
            }
            

            return bAxlInit;
        }
        
        
        private bool Axl_Init()
        {
            int lBoardCount = 0;
            //int i = 0;
            int SetTotalMotorCnt = MotorSet.MAX_MOTOR_COUNT;
            // ※ [CAUTION] 아래와 다른 Mot파일(모션 설정파일)을 사용할 경우 경로를 변경하십시요.
            //String szFilePath = "C:\\Program Files\\EzSoftware RM\\EzSoftware\\MotionDefault.mot";
            //++ AXL(AjineXtek Library)을 사용가능하게 하고 장착된 보드들을 초기화합니다.

            CAXL.AxlGetBoardCount(ref lBoardCount);

            if (lBoardCount < 1)
            {
                Globalo.LogPrint("", "Motion board recognition failure", Globalo.eMessageName.M_ERROR);
                return false;
            }
            Globalo.LogPrint("", "Motion board recognition completed");

            //++ 유효한 전체 모션축수를 반환합니다.

            CAXM.AxmInfoGetAxisCount(ref m_lAxisCounts);

            
            if (m_lAxisCounts < SetTotalMotorCnt)
            {
                Globalo.LogPrint("", $"Motor drive number mismatch[{m_lAxisCounts}/{SetTotalMotorCnt}]", Globalo.eMessageName.M_ERROR);
                return false;
            }

            AmpDisableAll();

            return true;
        }

        public int New_Axisconfig(MotionControl.MotorAxis motorAxis)
        {
            uint duRetCode = 0;
            int nFailCount = 0;
            string logstr = "";

            string sMotorName = motorAxis.Name;
            int nUseAxis = motorAxis.m_lAxisNo;
            double nMaxSpeed = motorAxis.MaxSpeed;
            double dResol = motorAxis.Resolution;
            MotorDefine.eMotorType nMotorType = motorAxis.Type;
            AXT_MOTION_LEVEL_MODE setLimit = motorAxis.AxtSetLimit;
            AXT_MOTION_LEVEL_MODE setServoAlarm = motorAxis.AxtSetServoAlarm;

            duRetCode = CAXM.AxmInfoIsInvalidAxisNo(nUseAxis);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                nFailCount++;
                logstr = "AxmInfoIsInvalidAxisNo Fail";
                Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                //해당축이 사용할 수 있는 축인지 확인한다
                //eLogSender("CAxlMotion", $"[{sMotorName}]모터 드라이브 인식 실패", Globalo.eMessageName.M_ERROR);
            }

            duRetCode = CAXM.AxmMotSetMoveUnitPerPulse(nUseAxis, 1, 1);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                nFailCount++;
                logstr = "AxmMotSetMoveUnitPerPulse Fail";
                Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                //Unit / Pulse = 1 : 1이면 pulse/ sec 초당 펄스수가 되는데 4500 rpm에 맞추고 싶다면 4500 / 60초 는 75회전 / 1초가 된다.
                //모터가 1회전에 몇 펄스인지 알아야 된다. 이것은 Encoder에 Z상을 검색해보면 알수있다.
                //만약 1회전:1800 펄스라고 가정하면 75 x 1800 = 135000 펄스가 필요하게 된다.


                //eLogSender("CAxlMotion", $"[{sMotorName}]모터 드라이브 전자기어비 초기화 실패", Globalo.eMessageName.M_ERROR);
            }
            duRetCode = CAXM.AxmMotSetMinVel(nUseAxis, 1);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                nFailCount++;
                logstr = "AxmMotSetMinVel Fail";
                Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                //특별한 경우가 아니면 이 함수를 사용할 필요는 없지만 기본적으로 초기속도는 1로 설정된다.
                //스텝모터를 사용할 경우 기동 초기속도를 설정하여 탈조 현상을 없앨 수 있다.

                //eLogSender("CAxlMotion", $"[{sMotorName}]모터 드라이브 최소 속도 설정 실패", Globalo.eMessageName.M_ERROR);
            }
            duRetCode = CAXM.AxmMotSetAccelUnit(nUseAxis, (uint)AXT_MOTION_ACC_UNIT.SEC);//SEC UNIT_SEC2
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                nFailCount++;
                logstr = "AxmMotSetAccelUnit Fail";
                Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                //eLogSender("CAxlMotion", $"[{sMotorName}]모터 드라이브 가속도 단위 설정 실패", Globalo.eMessageName.M_ERROR);
            }
            duRetCode = CAXM.AxmMotSetProfileMode(nUseAxis, (uint)AXT_MOTION_PROFILE_MODE.ASYM_S_CURVE_MODE);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                nFailCount++;
                logstr = "AxmMotSetProfileMode Fail";
                Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                //eLogSender("CAxlMotion", $"[{sMotorName}]모터 비대칭 S-Curve 가감속 설정 실패", Globalo.eMessageName.M_ERROR);
            }
            duRetCode = CAXM.AxmMotSetAccelJerk(nUseAxis, 30);
            duRetCode = CAXM.AxmMotSetDecelJerk(nUseAxis, 30);

            double dAccelJerk = 0.0;
            double dDecelJerk = 0.0;
            uint dwHomLevel = 0;
            CAXM.AxmMotGetAccelJerk(nUseAxis, ref dAccelJerk);
            CAXM.AxmMotGetDecelJerk(nUseAxis, ref dDecelJerk);

            if (dAccelJerk != 30.0 || dDecelJerk != 30.0)
            {
                nFailCount++;
                logstr = "AxmMotSetDecelJerk Fail";
                Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                //eLogSender("CAxlMotion", $"[{sMotorName}]모터 지정축 가속 저크값 설정 실패", Globalo.eMessageName.M_ERROR);
            }

            duRetCode = CAXM.AxmMotSetAbsRelMode(nUseAxis, (uint)AXT_MOTION_ABSREL.POS_ABS_MODE);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                nFailCount++;
                logstr = "AxmMotSetAbsRelMode Fail";
                Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                //eLogSender("CAxlMotion", $"[{sMotorName}]모터 절대 위치 이동 모드 설정 실패", Globalo.eMessageName.M_ERROR);
            }

            duRetCode = CAXM.AxmHomeSetSignalLevel(nUseAxis, (uint)AXT_MOTION_LEVEL_MODE.HIGH);
            duRetCode = CAXM.AxmHomeGetSignalLevel(nUseAxis, ref dwHomLevel);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                nFailCount++;
                logstr = "AxmHomeSetSignalLevel Fail";
                Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                //eLogSender("CAxlMotion", $"[{sMotorName}]모터 HOME LEVEL 설정 실패", Globalo.eMessageName.M_ERROR);
            }

            duRetCode = CAXM.AxmMotSetMaxVel(nUseAxis, nMaxSpeed * dResol);

            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                nFailCount++;
                logstr = "AxmMotSetMaxVel Fail";
                Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                //eLogSender("CAxlMotion", $"[{sMotorName}]모터 최고 속도 설정 실패", Globalo.eMessageName.M_ERROR);
            }

            if (nMotorType == MotorDefine.eMotorType.LINEAR)
            {

                duRetCode = CAXM.AxmSignalSetInpos(nUseAxis, (uint)AXT_MOTION_LEVEL_MODE.HIGH);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    nFailCount++;
                    logstr = "[LINEAR] AxmSignalSetInpos Fail";
                    Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 Inposition 설정 실패", Globalo.eMessageName.M_ERROR);
                }
                duRetCode = CAXM.AxmSignalSetStop(nUseAxis, (uint)AXT_MOTION_STOPMODE.EMERGENCY_STOP, (uint)AXT_MOTION_LEVEL_MODE.LOW);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    nFailCount++;
                    logstr = "[LINEAR] AxmSignalSetStop Fail";
                    Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 비상 정지 설정 실패", Globalo.eMessageName.M_ERROR);
                }

                duRetCode = CAXM.AxmSignalSetLimit(nUseAxis, (uint)AXT_MOTION_STOPMODE.EMERGENCY_STOP, (uint)setLimit, (uint)setLimit);


                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    nFailCount++;
                    logstr = "[LINEAR] AxmSignalSetLimit Fail";
                    Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 Limit 감지 정지 설정 실패", Globalo.eMessageName.M_ERROR);
                }

                duRetCode = CAXM.AxmSignalSetServoAlarm(nUseAxis, (uint)setServoAlarm);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    nFailCount++;
                    logstr = "[LINEAR] AxmSignalSetServoAlarm Fail";
                    Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 SERVER Alarm 감지 설정 실패", Globalo.eMessageName.M_ERROR);
                }

            }
            else if (nMotorType == MotorDefine.eMotorType.STEPING)
            {
                duRetCode = CAXM.AxmSignalSetInpos(nUseAxis, (uint)AXT_MOTION_LEVEL_MODE.UNUSED);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    nFailCount++;
                    logstr = "[STEPING] AxmSignalSetServoAlarm Fail";
                    Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 Inposition 설정 실패", Globalo.eMessageName.M_ERROR);
                }
                duRetCode = CAXM.AxmMotSetPulseOutMethod(nUseAxis, (uint)AXT_MOTION_PULSE_OUTPUT.TwoCcwCwHigh);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    nFailCount++;
                    logstr = "[STEPING] AxmMotSetPulseOutMethod Fail";
                    Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 CW/CCW 설정 실패", Globalo.eMessageName.M_ERROR);
                }
                duRetCode = CAXM.AxmSignalSetStop(nUseAxis, (uint)AXT_MOTION_STOPMODE.EMERGENCY_STOP, (uint)AXT_MOTION_LEVEL_MODE.HIGH);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    nFailCount++;
                    logstr = "[STEPING] AxmSignalSetStop Fail";
                    Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                    //_stprintf_s
                }
                //pcb z만 high , high

                duRetCode = CAXM.AxmSignalSetLimit(nUseAxis, (uint)AXT_MOTION_STOPMODE.EMERGENCY_STOP, (uint)setLimit, (uint)setLimit);

                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    nFailCount++;
                    logstr = "[STEPING] AxmSignalSetLimit Fail";
                    Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 비상 정지 설정 실패", Globalo.eMessageName.M_ERROR);
                }
                duRetCode = CAXM.AxmMotSetEncInputMethod(nUseAxis, (uint)AXT_MOTION_EXTERNAL_COUNTER_INPUT.ObverseSqr4Mode);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    nFailCount++;
                    logstr = "[STEPING] AxmMotSetEncInputMethod Fail";
                    Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 4체배 설정 실패", Globalo.eMessageName.M_ERROR);
                }
                duRetCode = CAXM.AxmSignalSetServoOnLevel(nUseAxis, (uint)AXT_MOTION_LEVEL_MODE.HIGH);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    nFailCount++;
                    logstr = "[STEPING] AxmSignalSetServoOnLevel Fail";
                    Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 Servo On Level 설정 실패", Globalo.eMessageName.M_ERROR);
                }
                duRetCode = CAXM.AxmSignalSetServoAlarm(nUseAxis, (uint)setServoAlarm);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    nFailCount++;
                    logstr = "[STEPING] AxmSignalSetServoAlarm Fail";
                    Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);

                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 SERVER Alarm 감지 설정 실패", Globalo.eMessageName.M_ERROR);
                }
            }
            return nFailCount++; ;
        }
        
        public bool AmpDisableAll()
        {
            int i = 0;

            int length = transferMachine.MotorAxes.Length;

            for (i = 0; i < length; i++)
            {
                transferMachine.MotorAxes[i].Stop();
                transferMachine.MotorAxes[i].ServoOff();
                if (transferMachine.MotorAxes[i].Type == MotorDefine.eMotorType.LINEAR)
                {
                    transferMachine.MotorAxes[i].ServoAlarmReset(1);
                }

            }
            Thread.Sleep(500);

            length = transferMachine.MotorAxes.Length;
            for (i = 0; i < length; i++)
            {
                if (transferMachine.MotorAxes[i].Type == MotorDefine.eMotorType.LINEAR)
                {
                    transferMachine.MotorAxes[i].ServoAlarmReset(0);
                }
            }

            return true;
        }
        private void Axl_Close()
        {
            int i;
            // 모든 모터를 정지한다.

            int length = transferMachine.MotorAxes.Length;
            for (i = 0; i < length; i++)
            {
                transferMachine.MotorAxes[i].Stop();
            }

            CAXL.AxlClose();
        }
        
    }
}
