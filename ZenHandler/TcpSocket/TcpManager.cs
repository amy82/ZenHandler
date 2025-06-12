using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZenHandler.TcpSocket
{
    public class TcpManager
    {
        private readonly SynchronizationContext _syncContext;
        //private readonly List<TcpClientHandler> _clients = new List<TcpClientHandler>();
        
        public BarcodeClient BcrClient;     //<--- 바코드 연결
        public TcpServer _HandlerServer;    //<-----Tester Client1,2,3,4,5,6,7,8... 이쪽으로 붙기 + Secsgem Client 도

        private CancellationTokenSource _cts;
        public TcpManager(string ip, int port)
        {
            Event.EventManager.PgExitCall += OnPgExitCall;
            _syncContext = SynchronizationContext.Current;
            _HandlerServer = new TcpServer(ip, port);
            //_server.OnMessageReceived += OnMessageReceived; // 서버의 메시지 수신 이벤트 구독
            _HandlerServer.OnMessageReceivedAsync += HandleClientMessageAsync;

            BcrClient = new BarcodeClient();
            _cts = new CancellationTokenSource();
        }
        
        private void OnPgExitCall(object sender, EventArgs e)
        {
            // 이벤트 처리
            Console.WriteLine("TcpManager - OnPgExitCall");
            StopServer();
        }

        public async void SendMessageToClient(TcpSocket.EquipmentData equipData, int clintNum = -1)
        {
            if (_HandlerServer.bClientConnectedState(clintNum) == false)
            {
                return;
            }
            //await _server.SendMessageAsync(message);   //클라이언트 하나만 허용  secGemApp
            //
            string jsonData = JsonConvert.SerializeObject(equipData);
            await _HandlerServer.BroadcastMessageAsync(jsonData, clintNum);
        }

        public async void SendMsgToTester(TcpSocket.MessageWrapper equipData, int clintNum = -1)
        {
            if (_HandlerServer.bClientConnectedState(clintNum) == false || clintNum == -1)
            {
                Console.WriteLine($"bClientConnectedState - {clintNum}");
                return;
            }
            string jsonData = JsonConvert.SerializeObject(equipData);
            await _HandlerServer.BroadcastMessageAsync(jsonData, clintNum);
        }
        // 서버 시작
        public async Task StartServerAsync()
        {
            await _HandlerServer.StartAsync(_cts.Token);
        }

        // 서버 중지
        public void StopServer()
        {
            _cts.Cancel();
            _HandlerServer.Stop();
        }
        public void SendAlarmReport(string nAlarmID)
        {
            TcpSocket.EquipmentData sendEqipData = new TcpSocket.EquipmentData();
            sendEqipData.Command = "APS_ALARM_CMD";
            if (nAlarmID == "1001" || nAlarmID == "1003" || nAlarmID == "1004" || nAlarmID == "1007")//수정필요
            {
                sendEqipData.ErrCode = "H";
            }
            else
            {
                sendEqipData.ErrCode = "L";
            }
            
            sendEqipData.ErrText = nAlarmID;
            SendMessageToClient(sendEqipData);
        }
        private void socketMessageParse(TesterData data, int index)        //index = ip뒷자리
        {
            int i = 0;
            int result = -1;

            string dataName = data.Name;
            string cmd = data.Cmd;
            int nStep = data.Step;

            if (dataName == "EEPROM_WRITE") //0 ~ 3 개별 pc
            {
                int pcNum = index % 4;      //0,1,2,3 반복
                if (index < 4)
                {
                    Globalo.motionManager.socketEEpromMachine.Tester_A_Result[pcNum] = data.States[pcNum];
                }
                
            }
            else if (dataName == "EEPROM_VERIFY") //4 ~ 7 개별 pc
            {
                int pcNum = index % 4;      //0,1,2,3 반복
                if (index < 4)
                {
                    Globalo.motionManager.socketEEpromMachine.Tester_B_Result[pcNum] = data.States[pcNum];
                }
                
            }
            else if (dataName == "AOI")  //2  (aoi)
            {
                if (cmd == "RESP_TEST")      //CMD_TEST 보내고 결과 받기
                {
                    Globalo.motionManager.socketAoiMachine.Test_Req_Result[data.socketNum - 1] = data.result;
                    if (nStep == 0)
                    {
                        //1차 검사 요청에 대한 리턴 ? 2차때Z축 이동 후 측정할수도 있어서
                    }
                    if (nStep == 1)
                    {
                        //2차 검사 요청에 대한 리턴, 
                    }
                }
                if (data.socketNum < 4 && data.socketNum > -1)
                {
                    if (index == 0)     //LEFT 소켓 - L/R
                    {
                        Globalo.motionManager.socketAoiMachine.Tester_A_Result[data.socketNum] = data.States[data.socketNum];
                    }

                    if (index == 1)     //RIGHT 소켓 - L/R
                    {
                        Globalo.motionManager.socketAoiMachine.Tester_B_Result[data.socketNum] = data.States[data.socketNum];
                    }
                }
                
            }
            else if (dataName == "FW")  //4(fw)
            {
                Globalo.motionManager.socketFwMachine.Tester_Result_All[index] = (int[])data.States.Clone();
            }
        }
        private void hostMessageParse(EquipmentData data)
        {
            int i = 0;
            int j = 0;
            int result = -1;
            int cnt = data.CommandParameter.Count;
            string logData = $"[Recv] Client Command: {data.Command} [{data.DataID}] [{cnt}] ";
            Globalo.LogPrint("TcpManager", logData);

            //Console.WriteLine($"장비 ID: {data.EQPID}, 레시피 ID: {data.RECIPEID}");

            //통신 예제
            //REQ_ 요청 (Request)
            //REQ_INSPECT - 검사 요청
            //REQ_LOT_START
            //REQ_APD_REPORT - RESP_LOT_COMPLETE

            //
            //RESP_ 응답 (Response)
            //RESP_MOVE_Z : Z축 이동 완료 응답
            //
            //NOTI_ 알람 / 통지 (Notification)
            //NOTI_LOT_END  : LOT 종료 알림
            //
            //CMD_  명령(Command_ 제어 지시)
            //CMD_MOVE_Z    : Z축 이동 명령
            //CMD_RESET
            //
            //STAT_ 상태전송(sTATUS)
            //STAT_READY
            //STAT_BUSY
            //ERR_  에러 알림(
            //
            /*
             {
              "cmd": "REQ_APD_REPORT",
              "lotId": "LOT20240601",
              "socketIndex": 2,
              "result": "PASS",
              "timestamp": "2025-06-02T14:21:33"
            }
             */
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            //
            //
            if (data.Command == "APS_LOT_START_CMD")
            {
                //TODO: 이때 검사 pc로 보내야될 값 , 받아서 바코드 Lot과 함께 전달해야된다.
                //착공 진행 신호
                Globalo.taskWork.bRecv_Client_LotStart = data.Judge;   //Only 0 = ok
                Globalo.taskWork.SpecialDataParameter = data.CommandParameter.Select(item => item.DeepCopy()).ToList();

            }
            else if (data.Command == "APS_LOT_COMPLETE_CMD")
            {
                //완공 진행 신호
                Globalo.taskWork.bRecv_Client_ApdReport = data.Judge;       //0 = 완공 , 1 = Lot_Processing_Completed_Ack = 1
            }

            if (data.Command == "OBJECT_ID_REPORT_ACK")
            {
                //CLIENT THREAD START 여부
                Globalo.taskWork.bRecv_Client_ObjectIdReport = data.Judge;   //0 = ok , 1 = fail
            }

            if (data.Command == "APS_PROCESS_STATE_INFO")       
            {
                string stateInfo = data.DataID;//ProcessStateInfo 상태 받기 INIT , IDLE , SETUP , READY , EXECUTING , PAUSE
                //ProgramState.STATE_DRIVER_ONLINE = true;
                //if (stateInfo == "OFFLINE")
                //{
                //    ProgramState.STATE_DRIVER_ONLINE = false;
                //}
                //_syncContext.Send(_ =>
                //{
                //   Globalo.MainForm.textBox_ProcessState.Text = stateInfo;
                //}, null);
            }
            if (data.Command == "APS_DRIVER_CMD")
            {
                //UbiGem Drive 연결 상태 받기
                if (data.Judge == 1)
                {
                    //연결 완료
                    Globalo.MainForm.DriverConnected(true);
                }
                else
                {
                    //연결 끊어짐
                    Globalo.MainForm.DriverConnected(false);
                }
            }
            if (data.Command == "CT_TIMEOUT")
            {
                Globalo.taskWork.CtTimeOutValue = data.ErrText;
                if (data.ErrCode == "89")
                {
                    Globalo.taskWork.bRecv_Client_CtTimeOut = 3;  //[LOT] LGIT PP SELECT CT TimeOut
                }
                if (data.ErrCode == "90")
                {
                    Globalo.taskWork.bRecv_Client_CtTimeOut = 4;  //[LOT] (Setup)Process State Change CT TimeOut
                }
                if (data.ErrCode == "91")
                {
                    Globalo.taskWork.bRecv_Client_CtTimeOut = 5;  //[LOT] (Ready)Process State Change CT TimeOut
                }
                if (data.ErrCode == "92")
                {
                    Globalo.taskWork.bRecv_Client_CtTimeOut = 6;  //[LOT] PP-Select Report CT TimeOut
                }
                if (data.ErrCode == "93")
                {
                    Globalo.taskWork.bRecv_Client_CtTimeOut = 7;  //[LOT] Formatted Process Program CT TimeOut
                }
                if (data.ErrCode == "94")
                {
                    Globalo.taskWork.bRecv_Client_CtTimeOut = 8;  //[LOT] PP Upload Confirm CT TimeOut
                }
                if (data.ErrCode == "95")
                {
                    Globalo.taskWork.bRecv_Client_CtTimeOut = 9;  //[LOT] PP Upload Completed CT TimeOut
                }
                if (data.ErrCode == "96")
                {
                    Globalo.taskWork.bRecv_Client_CtTimeOut = 10;  //Lot Start CT TimeOut
                }
                if (data.ErrCode == "97")
                {
                    Globalo.taskWork.bRecv_Client_CtTimeOut = 11;  //EEprom Data CT TimeOut
                }
                if (data.ErrCode == "98")
                {
                    //ProcessComData.Judge = 0;
                    //ProcessComData.ErrText = "[LOT] (Setup)Process State Change CT TimeOut";
                    Globalo.taskWork.bRecv_Client_CtTimeOut = 12; //PROCESS_STATE_CHANGED_REPORT_10401 (SETUP , READY , EXECUTE ,) 보내고 ACK 못 받은 경우
                }
                if (data.ErrCode == "100")  
                {
                    Globalo.taskWork.bRecv_Client_CtTimeOut = 1;    //Lot APD CT TimeOut
                }
                if (data.ErrCode == "101")  
                {
                    Globalo.taskWork.bRecv_Client_CtTimeOut = 2;     //Lot Processing Completed CT TimeOut
                }
            }

            if (data.Command == CMD_POPUP_MESSAGE.cpDefault.ToString())
            {
                string message1 = data.ErrText;
                if (message1.Length > 0)
                {
                    Globalo.LogPrint("ManualCMainFormontrol", message1, Globalo.eMessageName.M_WARNING);
                }
                
            }
            if (data.Command == "CMD_BUZZER")
            {
                //TODO: 부저 ON
            }
            if (data.Command == SecsGemData.LGIT_PP_SELECT)        //정상적일 때 PP SELECT 보내지 않는다.
            {
                Globalo.taskWork.bRecv_Client_LotStart = 1;   //LGIT_PP_SELECT 사용중인 레시피와 다른 경우
            }
            if (data.Command == SecsGemData.LGIT_LOT_ID_FAIL)
            {
                
                Globalo.dataManage.mesData.vLotIdFail.CODE = data.ErrCode;
                Globalo.dataManage.mesData.vLotIdFail.TEXT = data.ErrText;

                foreach (EquipmentParameterInfo paramInfo in data.CommandParameter)
                {
                    Data.RcmdParameter parameter = new Data.RcmdParameter();
                    parameter.name = paramInfo.Name;
                    parameter.value = paramInfo.Value;
                    Globalo.dataManage.mesData.vLotIdFail.Children.Add(parameter);
                }
                Globalo.taskWork.bRecv_Client_LotStart = 2;   //LGIT_LOT_ID_FAIL

            }
            if (data.Command == SecsGemData.LGIT_PP_UPLOAD_FAIL)
            {
                

                Globalo.dataManage.mesData.vPPUploadFail.RECIPEID = data.RecipeID;
                Globalo.dataManage.mesData.vPPUploadFail.CODE = data.ErrCode;
                Globalo.dataManage.mesData.vPPUploadFail.TEXT = data.ErrText;


                foreach (EquipmentParameterInfo paramInfo in data.CommandParameter)
                {
                    Data.RcmdParameter parameter = new Data.RcmdParameter();
                    parameter.name = paramInfo.Name;
                    parameter.value = paramInfo.Value;
                    Globalo.dataManage.mesData.vPPUploadFail.Children.Add(parameter);
                }

                Globalo.taskWork.bRecv_Client_LotStart = 3;   //LGIT_PP_UPLOAD_FAIL
            }

            if (data.Command == SecsGemData.LGIT_EEPROM_FAIL)
            {
                Globalo.dataManage.mesData.rEEprom_Fail.LotIdValue = data.LotID;
                Globalo.dataManage.mesData.rEEprom_Fail.CodeValue = data.ErrCode;
                Globalo.dataManage.mesData.rEEprom_Fail.TextValue = data.ErrText;

                Globalo.taskWork.bRecv_Client_LotStart = 5; //LGIT_EEPROM_FAIL
            }
            else if (data.Command == SecsGemData.LGIT_EEPROM_DATA)
            {
                Globalo.taskWork.bRecv_Client_LotStart = 4;   //LGIT_EEPROM_DATA csv 저장 실패 때만 들어온다.
            }


            
            if (data.Command == "LOT_PROCESSING_STARTED_FAIL")
            {
                //ProcessFailData.Judge = Globalo.dataManage.TaskWork.bRecv_S6F12_Lot_Processing_Started;
                //ProcessFailData.Command = "LOT_PROCESSING_STARTED_FAIL";
                Globalo.taskWork.bRecv_Client_LotStart = 6; //LOT_PROCESSING_STARTED  ack 값이 0이 아닌 값이 들어온 경우
            }

            if (data.Command == "APS_RECIPE_CMD")
            {
                Globalo.dataManage.mesData.m_sMesPPID = data.DataID;      //Recv Client

                Globalo.yamlManager.vPPRecipeSpecEquip = Globalo.yamlManager.RecipeLoad(Globalo.dataManage.mesData.m_sMesPPID); //APS_RECIPE_CMD
                if (Globalo.yamlManager.vPPRecipeSpecEquip == null)
                {
                    Globalo.LogPrint("ManualControl", $"[{Globalo.dataManage.mesData.m_sMesPPID}] Recipe Load Fail");
                }
                //string slaveAddr = Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["SLAVE_ADDRESS"].value;
                Console.WriteLine("slave address: " + Globalo.yamlManager.vPPRecipeSpecEquip.RECIPE.ParamMap["SLAVE_ADDRESS"].value);
                //Globalo.yamlManager.secsGemDataYaml.RecipeDataSet(data.DataID);
                //Globalo.mMainPanel.ShowRecipeName();

                TcpSocket.EquipmentData sendEqipData = new TcpSocket.EquipmentData();
                sendEqipData.Command = "APS_RECIPE_ACK";
                SendMessageToClient(sendEqipData);
            }

            
            if (data.Command == "APS_MODEL_CMD")
            {
                TcpSocket.EquipmentData sendEqipData = new TcpSocket.EquipmentData();
                if (data.DataID.Length > 0)
                {
                    bool rtn = Globalo.yamlManager.secsGemDataYaml.ModelDataSet(data.DataID);    //현재 모델명 변경 , 모델 폴더 없으면 Default 폴더 복사
                    if (rtn)
                    {
                        
                    }
                    //Globalo.mMainPanel.ShowModelName();
                    sendEqipData.Command = "APS_MODEL_ACK";
                }
                else
                {
                    sendEqipData.Command = "APS_MODEL_NAK";
                }

                SendMessageToClient(sendEqipData);

                

                
               
            }
            if (data.Command == "APS_PPID_CMD") //"APS_PPID_REQ")
            {
                TcpSocket.EquipmentData sendEqipData = new TcpSocket.EquipmentData();

                //sendEqipData.Command = "APS_PPID_RES";
                //sendEqipData.DataID = Globalo.dataManage.mesData.m_sMesPPID;

                SendMessageToClient(sendEqipData);
            }


            
            if (data.Command == SecsGemData.LGIT_PP_UPLOAD_CONFIRM) //From Client Recv xxxxxx
            {
                
            }

            if (data.Command == SecsGemData.LGIT_LOT_START)
            {
                Globalo.dataManage.mesData.vLotStart.Clear();
                for (i = 0; i < cnt; i++)
                {
                    if (data.CommandParameter[i].Name == "EQPID")
                    {
                        Globalo.dataManage.mesData.m_sEquipmentID = data.CommandParameter[i].Value;
                    }
                    else if (data.CommandParameter[i].Name == "EQPNAME")
                    {
                        Globalo.dataManage.mesData.m_sEquipmentName = data.CommandParameter[i].Value;
                    }
                    else if (data.CommandParameter[i].Name == "RECIPEID")
                    {
                        Globalo.dataManage.mesData.m_sRecipeId = data.CommandParameter[i].Value;
                    }
                    else if (data.CommandParameter[i].Name == "LOTINFOLIST")
                    {
                        Data.RcmdParameter parameter = new Data.RcmdParameter();
                        foreach (EquipmentParameterInfo paramInfo in data.CommandParameter[i].ChildItem)
                        {
                            parameter.name = paramInfo.Name;
                            parameter.value = paramInfo.Value;
                            parameter.Children = new List<Data.RcmdParameter>();
                            foreach (EquipmentParameterInfo item in paramInfo.ChildItem)
                            {
                                Data.RcmdParameter childPara = new Data.RcmdParameter();
                                childPara.name = item.Name;
                                childPara.value = item.Value;
                                childPara.Children = new List<Data.RcmdParameter>();
                                parameter.Children.Add(childPara);

                                foreach (EquipmentParameterInfo SubItem in item.ChildItem)
                                {
                                    Data.RcmdParameter subchildPara = new Data.RcmdParameter();
                                    subchildPara.name = SubItem.Name;
                                    subchildPara.value = SubItem.Value;
                                    childPara.Children.Add(subchildPara);
                                }
                            }
                        }

                        Globalo.dataManage.mesData.vLotStart.Add(parameter);
                    }
                }
            }
            if (data.Command == SecsGemData.LGIT_MATERIAL_ID_CONFIRM) //From Client Recv xxxxxx
            {
               
            }
            if (data.Command == SecsGemData.LGIT_MATERIAL_ID_FAIL)
            {
                Globalo.dataManage.mesData.rMaterial_Id_Fail.MaterialId = data.MaterialID;
                Globalo.dataManage.mesData.rMaterial_Id_Fail.Code = data.ErrCode;
                Globalo.dataManage.mesData.rMaterial_Id_Fail.Text = data.ErrText;

                
            }
            if (data.Command == SecsGemData.LGIT_SETCODE_MATERIAL_EXCHANGE)     //사용 안하는 듯...
            {
                Globalo.dataManage.mesData.vMaterialExchange.Clear();
                
                for (i = 0; i < cnt; i++)
                {
                    if (data.CommandParameter[i].Name == "EQPID")
                    {
                        Globalo.dataManage.mesData.m_sEquipmentID = data.CommandParameter[i].Value;
                    }
                    else if (data.CommandParameter[i].Name == "EQPNAME")
                    {
                        Globalo.dataManage.mesData.m_sEquipmentName = data.CommandParameter[i].Value;
                    }
                    else if (data.CommandParameter[i].Name == SecsGemData.SETCODE_MATERIAL_EXCHANGE_CPNAME) //"MATERIALINFOLIST"
                    {
                        Data.RcmdParameter parameter = new Data.RcmdParameter();
                        foreach (EquipmentParameterInfo paramInfo in data.CommandParameter[i].ChildItem)
                        {
                            parameter.name = paramInfo.Name;
                            parameter.value = paramInfo.Value;
                            parameter.Children = new List<Data.RcmdParameter>();
                            foreach (EquipmentParameterInfo item in paramInfo.ChildItem)
                            {
                                Data.RcmdParameter childPara = new Data.RcmdParameter();
                                childPara.name = item.Name;
                                childPara.value = item.Value;
                                childPara.Children = new List<Data.RcmdParameter>();
                                parameter.Children.Add(childPara);

                                foreach (EquipmentParameterInfo SubItem in item.ChildItem)
                                {
                                    Data.RcmdParameter subchildPara = new Data.RcmdParameter();
                                    subchildPara.name = SubItem.Name;
                                    subchildPara.value = SubItem.Value;
                                    childPara.Children.Add(subchildPara);
                                }
                            }
                        }
                        Globalo.dataManage.mesData.vMaterialExchange.Add(parameter);
                    }
                }
            }
            if (data.Command == SecsGemData.LGIT_CTRLSTATE_CHG_REQ)
            {
                foreach (EquipmentParameterInfo paramInfo in data.CommandParameter)
                {
                    if (paramInfo.Name == "CONFIRMFLAG")
                    {
                        Globalo.dataManage.mesData.rCtrlState_Chg_Req.ConfirmFlag = paramInfo.Value;
                    }
                    else if (paramInfo.Name == "CONTROLSTATE")
                    {
                        int.TryParse(paramInfo.Value, out result);
                        Globalo.dataManage.mesData.rCtrlState_Chg_Req.ControlState = result;
                    }
                    else if (paramInfo.Name == "CHANGE_CODE")
                    {
                        Globalo.dataManage.mesData.rCtrlState_Chg_Req.Change_Code = paramInfo.Value;
                    }
                    else if (paramInfo.Name == "CHANGE_TEXT")
                    {
                        Globalo.dataManage.mesData.rCtrlState_Chg_Req.Change_Text = paramInfo.Value;
                    }
                    else if (paramInfo.Name == "RESULT_CODE")
                    {
                        Globalo.dataManage.mesData.rCtrlState_Chg_Req.Result_Code = paramInfo.Value;
                    }
                    else if (paramInfo.Name == "RESULT_TEXT")
                    {
                        Globalo.dataManage.mesData.rCtrlState_Chg_Req.Result_Text = paramInfo.Value;
                    }
                }
            }
            if (data.Command == SecsGemData.LGIT_OP_CALL)
            {
                Globalo.dataManage.mesData.rCtrlOp_Call.CallType = data.CallType;
                Globalo.dataManage.mesData.rCtrlOp_Call.OpCall_Code = data.ErrCode;
                Globalo.dataManage.mesData.rCtrlOp_Call.OpCall_Text = data.ErrText;
                    
            }
            
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            //
            if (data.Command == SecsGemData.LGIT_SETCODE_IDLE_REASON)
            {
                for (i = 0; i < cnt; i++)
                {
                    if (data.CommandParameter[i].Name == SecsGemData.IDLE_REASON_CPNAME)
                    {
                        int subCnt = data.CommandParameter[i].ChildItem.Count;
                        if (subCnt > 0)
                        {
                            Globalo.dataManage.mesData.vIdleReason.Clear();
                            for (j = 0; j < subCnt; j++)
                            {
                                Data.RcmdParam1 rcmdP1 = new Data.RcmdParam1();
                                rcmdP1.CpName = data.CommandParameter[i].ChildItem[j].Name;
                                rcmdP1.CepVal = data.CommandParameter[i].ChildItem[j].Value;
                                Globalo.dataManage.mesData.vIdleReason.Add(rcmdP1);
                            }
                        }
                    }
                }
            }
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            //
            if (data.Command == SecsGemData.LGIT_SETCODE_OFFLINE_REASON)
            {
                for (i = 0; i < cnt; i++)
                {
                    if (data.CommandParameter[i].Name == "OFFLINEREASONCODELIST")
                    {
                        int subCnt = data.CommandParameter[i].ChildItem.Count;
                        if (subCnt > 0)
                        {
                            Globalo.dataManage.mesData.vOfflineReason.Clear();
                            for (j = 0; j < subCnt; j++)
                            {
                                Data.RcmdParam1 rcmdP1 = new Data.RcmdParam1();
                                rcmdP1.CpName = data.CommandParameter[i].ChildItem[j].Name;
                                rcmdP1.CepVal = data.CommandParameter[i].ChildItem[j].Value;
                                Globalo.dataManage.mesData.vOfflineReason.Add(rcmdP1);
                            }
                        }
                    }
                }
            }
        }

        // 메시지 수신 시 처리
        private async Task HandleClientMessageAsync(string receivedData, int clientIndex)
        {
            //Console.WriteLine($"TcpManager에서 처리한 메시지: {receivedData}");

            //JsonSerializerSettings settings = new JsonSerializerSettings
            //{
            //    MaxDepth = 128, // 기본값보다 크게 설정
            //    NullValueHandling = NullValueHandling.Ignore
            //};
            //EquipmentData data = JsonConvert.DeserializeObject<EquipmentData>(receivedData, settings);


            Console.WriteLine($"JSON 데이터 길이: {receivedData.Length}");
            using (StreamReader sr = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(receivedData))))
            using (JsonTextReader reader = new JsonTextReader(sr))
            {
                JsonSerializer serializer = new JsonSerializer();
                var wrapper = serializer.Deserialize<MessageWrapper>(reader);

                switch (wrapper.Type)
                {
                    case "EquipmentData":
                        //EquipmentData edata = serializer.Deserialize<EquipmentData>(reader);
                        EquipmentData edata = JsonConvert.DeserializeObject<EquipmentData>(wrapper.Data.ToString());
                        hostMessageParse(edata);
                        break;

                    case "TesterData":
                        //SocketTestState sdata = serializer.Deserialize<SocketTestState>(reader);
                        TesterData socketState = JsonConvert.DeserializeObject<TesterData>(wrapper.Data.ToString());
                        socketMessageParse(socketState, clientIndex);
                        break;
                }
                

                try
                {
                    
                    await Task.Delay(10); // 가짜 비동기 작업 (예: DB 저장)
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"hostMessageParse 처리 중 예외 발생: {ex.Message}");
                }
            }
        }
        private void OnMessageReceived(string receivedData)
        {
            //Console.WriteLine($"TcpManager에서 처리한 메시지: {receivedData}");

            ////JsonSerializerSettings settings = new JsonSerializerSettings
            ////{
            ////    MaxDepth = 128, // 기본값보다 크게 설정
            ////    NullValueHandling = NullValueHandling.Ignore
            ////};
            ////EquipmentData data = JsonConvert.DeserializeObject<EquipmentData>(receivedData, settings);
            //Console.WriteLine($"JSON 데이터 길이: {receivedData.Length}");
            //using (StreamReader sr = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(receivedData))))
            //using (JsonTextReader reader = new JsonTextReader(sr))
            //{
            //    JsonSerializer serializer = new JsonSerializer();
            //    EquipmentData data = serializer.Deserialize<EquipmentData>(reader);

            //    try
            //    {
            //        hostMessageParse(data);
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"hostMessageParse 처리 중 예외 발생: {ex.Message}");
            //    }
            //}
        }
    }
}
