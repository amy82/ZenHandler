using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZenHandler.TcpSocket
{
    public class TcpManager
    {
        private readonly SynchronizationContext _syncContext;
        //private readonly List<TcpClientHandler> _clients = new List<TcpClientHandler>();
        
        private TcpServer _server;
        private CancellationTokenSource _cts;
        public TcpManager(string ip, int port)
        {
            _syncContext = SynchronizationContext.Current;
            _server = new TcpServer(ip, port);
            //_server.OnMessageReceived += OnMessageReceived; // 서버의 메시지 수신 이벤트 구독
            _server.OnMessageReceivedAsync += HandleClientMessageAsync;
            _cts = new CancellationTokenSource();
        }
        public async void SendMessageToClient(TcpSocket.EquipmentData equipData)//string message)
        {
            if (_server.bClientConnectedState() == false)
            {
                return;
            }
            //await _server.SendMessageAsync(message);   //클라이언트 하나만 허용  secGemApp
            //
            string jsonData = JsonConvert.SerializeObject(equipData);
            await _server.BroadcastMessageAsync(jsonData);
        }
        // 서버 시작
        public async Task StartServerAsync()
        {
            await _server.StartAsync(_cts.Token);
        }

        // 서버 중지
        public void StopServer()
        {
            _cts.Cancel();
            _server.Stop();
        }
        public void SendAlarmReport(string nAlarmID)
        {
            TcpSocket.EquipmentData sendEqipData = new TcpSocket.EquipmentData();
            sendEqipData.Command = "APS_ALARM_CMD";
            if (nAlarmID == "1001" || nAlarmID == "1003" || nAlarmID == "1004" || nAlarmID == "1007")
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
        private void hostMessageParse(EquipmentData data)
        {
            int i = 0;
            int j = 0;
            int result = -1;
            int cnt = data.CommandParameter.Count;
            string logData = $"[Recv] Client Command: {data.Command} [{data.DataID}] [{cnt}] ";
            Globalo.LogPrint("TcpManager", logData);

            //Console.WriteLine($"장비 ID: {data.EQPID}, 레시피 ID: {data.RECIPEID}");
            //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            //
            if (data.Command == "APS_LOT_START_CMD")
            {
                //착공 진행 신호
                Globalo.taskWork.bRecv_Client_LotStart = data.Judge;   //Only 0 = ok
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

                _syncContext.Send(_ =>
                {
                    Globalo.productionInfo.ProcessSet(stateInfo);
                   //Globalo.MainForm.textBox_ProcessState.Text = stateInfo;
                   
                }, null);
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

            //if (data.Command == CMD_POPUP_MESSAGE.cpFmt_PPReq.ToString())           //XXXX
            //{
            //    //설비에서 레시피 파라미터 down Req 했을 때 들어온다.
            //    //
            //    //Ask 팝업인지 Show 팝업인지 구분해야된다.
            //    //
            //    //팝업 띄워야된다. 2종료
            //    //1. 변경 할거냐? Yes or No
            //    //2. _stprintf_s(szLog, SIZE_OF_1K, _T("[%s] RECIPE EMPTY!"), fmtPpCollection->PPID);
            //    //

            //    string message1 = data.ErrText;
            //    if (data.ErrCode == "Ask")
            //    {
            //        //int result = >MessageBox(_T("Apply Host Recipe Parameter Value?"), _T("Apply Host Recipe Confirm!!!"));
            //        bool Rtn = false;
            //        Rtn = Globalo.ShowAskMessageDialog("Apply Host Recipe Parameter Value?");
            //        if (Rtn)
            //        {
            //            //client로 변경했다고 보내줘야된다.

            //            TcpSocket.EquipmentData FmtData = new TcpSocket.EquipmentData();
            //            FmtData.Command = CMD_POPUP_MESSAGE.cpFmt_PPReq.ToString();
            //            Globalo.tcpManager.SendMessageToClient(FmtData);
            //        }
            //        else
            //        {
            //            //아무 것도 한해도 된다.
            //        }
            //    }
            //    else
            //    {
            //        //_stprintf_s(szLog, SIZE_OF_1K, _T("[%s] RECIPE EMPTY!"), fmtPpCollection->PPID);

            //        Globalo.LogPrint("ManualCMainFormontrol", message1, Globalo.eMessageName.M_WARNING);
            //    }
            //}
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
                Globalo.productionInfo.ShowRecipeName();

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
                    Globalo.productionInfo.ShowModelName();
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
        private async Task HandleClientMessageAsync(string receivedData)
        {
            Console.WriteLine($"TcpManager에서 처리한 메시지: {receivedData}");

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
                EquipmentData data = serializer.Deserialize<EquipmentData>(reader);

                try
                {
                    hostMessageParse(data);
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
