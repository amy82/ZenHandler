using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZenHandler.Process
{
    public class PcbProcess
    {
        private readonly SynchronizationContext _syncContext;
        private int m_dTickCount;
        //private int nCsvFineRetyCount = 0;
        //public int[] SensorSet;
        public int[] SensorSet = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public int[] OrgOnGoing = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        //private bool autoPause = true;

        //System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer { Interval = 5000 }; // 3초 후 바코드 스캔된 것처럼 처리
        public PcbProcess()
        {
            //SensorSet = new int[4];
            //textCmdPos.Text = String.Format("{0:0.000}", dCmdPosition);
            _syncContext = SynchronizationContext.Current;
            m_dTickCount = 0;
        }

        public int Auto_Loading(int nStep)       //로딩(30000 ~ 40000)
        {
            string szLog = "";
            int nRetStep = nStep;
            switch (nStep)
            {
                case 30000:
                    _syncContext.Send(_ =>
                    {
                        Globalo.camControl.setOverlayText("READY", Color.YellowGreen);         //초기화
                    }, null);
                    Globalo.taskWork.m_nTestFinalResult = 1;        //초기화
                    nRetStep = 30050;
                    break;

                case 30050:
                    //IDLE REPORT 
                    //TcpSocket.EquipmentData reportData = new TcpSocket.EquipmentData();
                    //reportData.Command = "IDLE_REPORT";         //Auto_Loading
                    //Globalo.tcpManager.SendMessageToClient(reportData);
                    
                    //szLog = $"[AUTO] IDLE - Process State Changed SEND [STEP : {nRetStep}]";
                    //Globalo.LogPrint("LotProcess", szLog);
                    nRetStep = 30100;
                    break;

                case 30100:
                    if (Globalo.yamlManager.configData.DrivingSettings.PinCountUse == true)
                    {
                        if (Globalo.yamlManager.taskDataYaml.TaskData.PintCount > Globalo.yamlManager.configData.DrivingSettings.PinCountMax)
                        {
                            Globalo.tcpManager.SendAlarmReport("1010");
                            szLog = $"[AUTO] PIN COUNT CHECK OVER: {Globalo.yamlManager.taskDataYaml.TaskData.PintCount} / {Globalo.yamlManager.configData.DrivingSettings.PinCountMax} [STEP : {nRetStep}]";
                            Globalo.LogPrint("LotProcess", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep = -30100;

                            break;
                        }
                        else
                        {
                            szLog = $"[AUTO] PIN COUNT CHECK OK : {Globalo.yamlManager.taskDataYaml.TaskData.PintCount} / {Globalo.yamlManager.configData.DrivingSettings.PinCountMax} [STEP : {nRetStep}]";
                            Globalo.LogPrint("LotProcess", szLog);
                            nRetStep = 30150;
                        }
                    }
                    else
                    {
                        szLog = $"[AUTO] PIN COUNT CHECK PASS [STEP : {nRetStep}]";
                        Globalo.LogPrint("LotProcess", szLog);
                        nRetStep = 30150;
                    }
                    Globalo.yamlManager.taskDataYaml.TaskData.PintCount++;
                    Globalo.yamlManager.taskDataYaml.TaskDataSave();

                    _syncContext.Send(_ =>
                    {
                        Globalo.productionInfo.PinCountInfoSet();
                    }, null);
                    break;
                case 30150:
                    nRetStep = 30200;
                    break;
                case 30200:
                    // 바코드 이벤트를 발생시키는 시뮬레이션

                    //timer.Tick += (sender, args) =>
                    //{
                    //    timer.Stop();
                    //    Globalo.serialPortManager.Barcode.SimulateScan("121212");
                    //};
                    //timer.Start();
                    nRetStep = 30250;
                    break;
                case 30250:
                    //_syncContext?.Post(_ => Globalo.MessageAskPopup("제품 투입후 진행해주세요!"), null);
                    DialogResult result = DialogResult.None;
                    _syncContext.Send(_ =>
                    {
                        result = Globalo.MessageAskPopup("제품 투입 후 진행해주세요!\n(SPACE KEY START)");
                    }, null);


                    if (result == DialogResult.Yes)
                    {
                        

                        nRetStep = 39000;
                    }
                    else
                    {
                        szLog = $"[AUTO] 자동운전 일시정지[STEP : {nStep}]";
                        Globalo.LogPrint("PcbPrecess", szLog);
                        nRetStep = -30250;
                        break;
                    }
                    break;
                case 39000:
                    _syncContext.Send(_ =>
                    {
                        Globalo.camControl.setOverlayText("START", Color.Green);         //초기화
                    }, null);
                    
                    szLog = $"[AUTO] START [STEP : {nRetStep}]";
                    Globalo.LogPrint("LotProcess", szLog);
                    nRetStep = 40000;
                    break;
            }
            return nRetStep;
        }
        public int Auto_Mes_Secenario(int nStep)
        {
            //bool rtn = true;
            string szLog = "";
            const int UniqueNum = 40000;
            int nRetStep = nStep;
            switch (nStep)
            {
                case UniqueNum:

                    nRetStep = 40050;
                    break;
                case 40050:
                    //Object Id Report

                    Globalo.taskWork.bRecv_Client_ObjectIdReport = -1;
                    Globalo.taskWork.bRecv_Client_CtTimeOut = -1;

                    TcpSocket.EquipmentData sendEqipData = new TcpSocket.EquipmentData();
                    sendEqipData.Command = "OBJECT_ID_REPORT";
                    sendEqipData.LotID = Globalo.dataManage.TaskWork.m_szChipID;

                    Globalo.tcpManager.SendMessageToClient(sendEqipData);


                    szLog = $"[AUTO] OBJECT_ID_REPORT SEND [STEP : {nRetStep}]";
                    Globalo.LogPrint("LotProcess", szLog);

                    nRetStep = 40100;
                    m_dTickCount = Environment.TickCount;
                    break;

                case 40100:
                    if (Globalo.taskWork.bRecv_Client_ObjectIdReport == 0)
                    {
                        //ok
                        szLog = $"[AUTO] OBJECT_ID_REPORT Acknowledge [STEP : {nRetStep}]";
                        Globalo.LogPrint("LotProcess", szLog);
                        nRetStep = 40200;
                    }
                    else if (Globalo.taskWork.bRecv_Client_ObjectIdReport == 1)
                    {
                        //fail client 문제
                        //object id report 리트라이
                        DialogResult result = DialogResult.None;
                        _syncContext.Send(_ =>
                        {
                            result = Globalo.MessageAskPopup("OBJECT ID REPORT RETRY?");
                        }, null);

                        if (result == DialogResult.Yes)
                        {
                            nRetStep = 40050;
                            break;
                        }
                        nRetStep = -40050;  
                    }
                    else if ((Environment.TickCount - m_dTickCount) > 100000)
                    {
                        //time out
                        szLog = $"[AUTO] OBJECT_ID_REPORT TIMEOUT [STEP : {nRetStep}]";
                        Globalo.LogPrint("LotProcess", szLog, Globalo.eMessageName.M_ERROR);

                        nRetStep = -40050;
                    }
                    
                    break;
                case 40200:
                    Globalo.taskWork.bRecv_Client_LotStart = -1;        //착공 완료 여부
                    m_dTickCount = Environment.TickCount;
                    nRetStep = 40300;
                    break;
                case 40300:
                    if (Globalo.taskWork.bRecv_Client_LotStart == 0)        //0일때만 정상 착공 상태
                    {
                        szLog = $"[AUTO] LOT START COMPLETE [STEP : {nRetStep}]";
                        Globalo.LogPrint("LotProcess", szLog);
                        nRetStep = 40400;
                        break;
                    }
                    else if (Globalo.taskWork.bRecv_Client_LotStart == 1)       //LGIT_PP_SELECT   - 레시피가 다른 경우
                    {
                        //[1] LGIT_PP_SELECT 사용중인 레시피명과 다를 때

                        szLog = $"[LGIT_LOT_ID_FAIL] " +
                            $"RECIPE:{Globalo.dataManage.mesData.vPPUploadFail.RECIPEID} " +
                            $"Code:{ Globalo.dataManage.mesData.vPPUploadFail.CODE } " +
                            $"Text:{ Globalo.dataManage.mesData.vPPUploadFail.TEXT } " +
                            $"\n재시도 하시겠습니까?";

                        DialogResult result = DialogResult.None;
                        _syncContext.Send(_ =>
                        {
                            result = Globalo.MessageAskPopup(szLog);
                        }, null);

                        if (result == DialogResult.Yes)
                        {
                            nRetStep = 40050;
                            break;
                        }
                        else
                        {
                            //_stprintf_s(szLog, SIZE_OF_1K, _T("[AUTO] RECIPE ID 확인 실패 [STEP : %d]"), nStep);
                            szLog = $"[AUTO] 사용중인 RECIPE ID 와 다릅니다.  [STEP : {nRetStep}]";
                            Globalo.LogPrint("AutoPrecess", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep = -40050;
                            break;
                        }

                    }
                    else if (Globalo.taskWork.bRecv_Client_LotStart == 2)       //LGIT_LOT_ID_FAIL
                    {
                        //[2] LGIT_LOT_ID_FAIL

                        string _lotid = Globalo.dataManage.TaskWork.m_szChipID;

                        if (Globalo.dataManage.mesData.vLotIdFail.Children.Count > 0)
                        {
                            _lotid = Globalo.dataManage.mesData.vLotIdFail.Children[0].value;
                        }
                        szLog = $"[LGIT_LOT_ID_FAIL] " +
                            $"LOT ID:{_lotid} " +
                            $"Code:{ Globalo.dataManage.mesData.vLotIdFail.CODE } " +
                            $"Text:{ Globalo.dataManage.mesData.vLotIdFail.TEXT } " +
                            $"\n재시도 하시겠습니까?";
                        DialogResult result = DialogResult.None;
                        _syncContext.Send(_ =>
                        {
                            result = Globalo.MessageAskPopup(szLog);
                        }, null);

                        if (result == DialogResult.Yes)
                        {
                            nRetStep = 40050;
                            break;
                        }
                        else
                        {
                            szLog = $"[AUTO] LOT ID: {_lotid} Cancel. by Host [STEP : {nRetStep}]";
                            Globalo.LogPrint("AutoPrecess", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep = -40050;
                        }
                    }
                    else if (Globalo.taskWork.bRecv_Client_LotStart == 3)           //LGIT_PP_UPLOAD_FAIL
                    {
                        //[3] LGIT_PP_UPLOAD_FAIL

                        szLog = $"[LGIT_PP_UPLOAD_FAIL] RECIPE ID:{Globalo.dataManage.mesData.vPPUploadFail.RECIPEID} " +
                            $"\nText:{ Globalo.dataManage.mesData.vPPUploadFail.TEXT } " +
                            $"Code:{ Globalo.dataManage.mesData.vPPUploadFail.CODE } " +
                            $"\n재시도 하시겠습니까?";
                        DialogResult result = DialogResult.None;
                        _syncContext.Send(_ =>
                        {
                            result = Globalo.MessageAskPopup(szLog);
                        }, null);

                        if (result == DialogResult.Yes)
                        {
                            nRetStep = 40050;
                            break;
                        }
                        else
                        {
                            szLog = $"[AUTO] PP Upload Fail Pause [STEP : {nRetStep}]";
                            Globalo.LogPrint("AutoPrecess", szLog, Globalo.eMessageName.M_ERROR);
                            nRetStep = -40050;
                        }

                    }
                    else if (Globalo.taskWork.bRecv_Client_LotStart == 4)           //LGIT_EEPROM_DATA (csv 파일 저장 실패)
                    {
                        //[4] LGIT_EEPROM_DATA CSV 파일 저장 실패

                        szLog = $"[LGIT_EEPROM_DATA] CSV FILE SAVE FAIL \n재시도 하시겠습니까?";
                        DialogResult result = DialogResult.None;
                        _syncContext.Send(_ =>
                        {
                            result = Globalo.MessageAskPopup(szLog);
                        }, null);

                        if (result == DialogResult.Yes)
                        {
                            nRetStep = 40050;
                            break;
                        }
                        else
                        {
                            szLog = $"[AUTO] EEprom Data Csv Save Fail Pause [STEP : {nRetStep}]";
                            Globalo.LogPrint("AutoPrecess", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep = -40050;
                        }
                    }
                    else if (Globalo.taskWork.bRecv_Client_LotStart == 5)           //LGIT_EEPROM_FAIL
                    {
                        szLog = $"[LGIT_EEPROM_FAIL] LOT ID:{Globalo.dataManage.mesData.rEEprom_Fail.LotIdValue} " +
                            $"\nText:{ Globalo.dataManage.mesData.rEEprom_Fail.TextValue } " +
                            $"Code:{ Globalo.dataManage.mesData.rEEprom_Fail.CodeValue } ";
                        //$"\n재시도 하시겠습니까?";
                        //Globalo.dataManage.mesData.rEEprom_Fail.LotIdValue = data.LotID;
                        //Globalo.dataManage.mesData.rEEprom_Fail.CodeValue = data.ErrCode;
                        //Globalo.dataManage.mesData.rEEprom_Fail.TextValue = data.ErrText;

                        szLog = $"[AUTO] EEprom Fail Pause [STEP : {nRetStep}]";
                        Globalo.LogPrint("AutoPrecess", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep = -40050;
                    }
                    else if (Globalo.taskWork.bRecv_Client_LotStart == 6)           //LOT_PROCESSING_STARTED ack != 0
                    {
                        //팝업
                        szLog = $"[{Globalo.dataManage.TaskWork.m_szChipID}] Lot Processing Started Fail Recv \n재시도 하시겠습니까?";
                        DialogResult result = DialogResult.None;
                        _syncContext.Send(_ =>
                        {
                            result = Globalo.MessageAskPopup(szLog);
                        }, null);

                        if (result == DialogResult.Yes)
                        {
                            nRetStep = 40050;
                            break;
                        }
                        else
                        {
                            szLog = $"[AUTO] Lot Processing Started Fail Pause [STEP : {nRetStep}]";
                            Globalo.LogPrint("AutoPrecess", szLog, Globalo.eMessageName.M_WARNING);
                            nRetStep = -40050;
                        }
                    }
                    else if (Globalo.taskWork.bRecv_Client_CtTimeOut > 0)
                    {
                        szLog = $"[AUTO] {Globalo.taskWork.CtTimeOutValue} [STEP : {nRetStep}]";
                        Globalo.LogPrint("AutoPrecess", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep = -40050;
                    }
                    else if ((Environment.TickCount - m_dTickCount) > 600000)
                    {
                        //time out
                        szLog = $"[AUTO] LOT START TIMEOUT [STEP : {nRetStep}]";
                        Globalo.LogPrint("LotProcess", szLog, Globalo.eMessageName.M_ERROR);

                        nRetStep = -40050;
                    }
                   
                    break;
                case 40400:

                    nRetStep = 40500;
                    break;
                case 40500:
                    nRetStep = 49000;
                    break;

                case 49000:

                    nRetStep = 50000;
                    break;
            }
            return nRetStep;
        }

        public int Auto_EEpromVerify(int nStep)       //로딩(40000 ~ 50000)
        {
            bool rtn = true;
            string szLog = "";
            int nRetStep = nStep;
            int errorCode = 0;
            switch (nStep)
            {
                case 50000:
                    
                    
                    nRetStep = 50050;
                    break;
                case 50050:
                    //MMD DATA LOAD
                    //
                    szLog = $"[AUTO] {Globalo.dataManage.TaskWork.m_szChipID} csv File Find[STEP : {nStep}]";
                    Globalo.LogPrint("PcbPrecess", szLog);

                    rtn = Globalo.dataManage.eepromData.LoadExcelData(Globalo.dataManage.TaskWork.m_szChipID);
                    if (rtn == false)
                    {
                        //로드 실패
                        Globalo.tcpManager.SendAlarmReport("1006");
                        szLog = $"[AUTO] {Globalo.dataManage.TaskWork.m_szChipID} csv Load Fail [STEP : {nRetStep}]";
                        Globalo.LogPrint("AutoPrecess", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep = -50050;
                        break;
                    }

                    szLog = $"[AUTO] {Globalo.dataManage.TaskWork.m_szChipID} csv Load Ok[STEP : {nStep}]";
                    Globalo.LogPrint("PcbPrecess", szLog);
                    nRetStep = 50100;
                    break;

                case 50100:
                    //영상 open 여기 
                    errorCode = Globalo.mLaonGrabberClass.OpenDevice();
                    if(errorCode > 0)
                    {
                        if (errorCode == 1)
                        {
                            Globalo.tcpManager.SendAlarmReport("1003");     //LAONBOARD CONNECT FAIL
                        }
                        else
                        {
                            Globalo.tcpManager.SendAlarmReport("1001");
                        }
                        
                        szLog = $"[AUTO] CCd OpenDevice Fail[STEP : {nStep}]";
                        Globalo.LogPrint("PcbPrecess", szLog);
                        nRetStep = -50100;
                        break;
                    }

                    szLog = $"[AUTO] CCd OpenDevice Ok[STEP : {nStep}]";
                    Globalo.LogPrint("PcbPrecess", szLog);

                    nRetStep = 50200;
                    break;
                case 50200:
                    nRetStep = 50300;
                    break;
                case 50300:
                    //영상 Grab Start ?? 선택 가능하게
                    if (Globalo.yamlManager.configData.DrivingSettings.ImageGrabUse == true)
                    {
                        rtn = Globalo.mLaonGrabberClass.StartGrab();
                        if (rtn == false)
                        {
                            szLog = $"[AUTO] CCd StartGrab Fail[STEP : {nStep}]";
                            Globalo.LogPrint("PcbPrecess", szLog);
                            nRetStep = -50100;
                            break;
                        }

                        szLog = $"[AUTO] CCd StartGrab Ok[STEP : {nStep}]";
                        Globalo.LogPrint("PcbPrecess", szLog);
                    }
                    else
                    {
                        szLog = $"[AUTO] CCd Grab Pass[STEP : {nStep}]";
                        Globalo.LogPrint("PcbPrecess", szLog);
                    }
                    nRetStep = 50400;
                    break;
                case 50400:
                    //
                    //제품에서 EEPROM READ
                    //
                    szLog = $"[AUTO] EEPROM DATA READ START[STEP : {nStep}]";
                    Globalo.LogPrint("PcbPrecess", szLog);

                    rtn = Data.CEEpromData.EEpromDataRead();
                    if(rtn == true)
                    {
                        szLog = $"[AUTO] EEPROM DATA READ OK[STEP : {nStep}]";
                        Globalo.LogPrint("PcbPrecess", szLog);

                        //
                        //
                        //EEPROM BINARY FILE 저장

                        string bcrLot = Data.CEEpromData.SanitizeFileName(Globalo.dataManage.TaskWork.m_szChipID);
                        Data.CEEpromData.SaveToBinaryFile(bcrLot, Globalo.dataManage.eepromData.EquipEEpromReadData);

                        szLog = $"[AUTO] EEPROM BINARY FILE SAVE[STEP : {nStep}]";
                        Globalo.LogPrint("PcbPrecess", szLog);

                        nRetStep = 50500;
                    }
                    else
                    {
                        Globalo.tcpManager.SendAlarmReport("1004");
                        szLog = $"[AUTO] EEPROM DATA READ FAIL[STEP : {nStep}]";
                        Globalo.LogPrint("PcbPrecess", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep = -50500;
                    }
                    
                    break;
                case 50500:
                    nRetStep = 51500;
                    break;
                case 51500:

                    szLog = $"[AUTO] EEPROM DATA VERIFY START[STEP : {nStep}]";
                    Globalo.LogPrint("PcbPrecess", szLog);

                    rtn = Data.CEEpromData.EEpromVerifyRun();
                    if(rtn == false)
                    {
                        ////Globalo.tcpManager.SendAlarmReport("1009");     //verify fail은 안보내도될듯
                        Globalo.taskWork.m_nTestFinalResult = 0;        //verify Fail
                    }
                    
                    _syncContext.Send(_ =>
                    {
                        Globalo.mMainPanel.ShowVerifyResultGrid(Globalo.dataManage.eepromData.CsvRead_MMd_DataList, Globalo.dataManage.eepromData.EEpromDataList);
                    }, null);

                    szLog = $"[AUTO] EEPROM DATA VERIFY COMPLETE[STEP : {nStep}]";
                    Globalo.LogPrint("PcbPrecess", szLog);
                    nRetStep = 52500;
                    break;
                case 52500:

                    nRetStep = 53000;
                    break;
                case 53000:
                    rtn = Globalo.mLaonGrabberClass.CloseDevice();
                    if (rtn == false)
                    {
                        Globalo.tcpManager.SendAlarmReport("1002");
                        szLog = $"[AUTO] CCd CloseDevice Fail[STEP : {nStep}]";

                        Globalo.LogPrint("PcbPrecess", szLog);
                        //nRetStep = -53000;
                        //break;
                    }
                    else
                    {
                        szLog = $"[AUTO] CCd CloseDevice Ok[STEP : {nStep}]";
                        Globalo.LogPrint("PcbPrecess", szLog);
                    }

                    
                    nRetStep = 59000;
                    break;
                case 59000:
                    //EEPROM DATA SAVE
                    rtn = Globalo.dataManage.eepromData.SaveExcelData(Globalo.dataManage.TaskWork.m_szChipID);
                    if (rtn)
                    {
                        szLog = $"[AUTO] {Globalo.dataManage.TaskWork.m_szChipID} EEprom Data Csv File Save ok[STEP : {nStep}]";
                        Globalo.LogPrint("PcbPrecess", szLog);
                    }
                    else
                    {
                        szLog = $"[AUTO] {Globalo.dataManage.TaskWork.m_szChipID} EEprom Data Csv File Save Fail[STEP : {nStep}]";
                        Globalo.LogPrint("PcbPrecess", szLog);
                    }
                    nRetStep = 60000;
                    break;
            }
            return nRetStep;
        }

        public int Auto_Final(int nStep)       //로딩(60000 ~ 70000)
        {
            string szLog = "";
            int nRetStep = nStep;
            //bool result = false;
            switch (nStep)
            {
                case 60000:
                    Globalo.taskWork.bRecv_Client_CtTimeOut = -1;
                    Globalo.taskWork.bRecv_Client_ApdReport = -1;
                    nRetStep = 60500;
                    break;
                case 60500:
                    nRetStep = 61500;
                    break;
                case 61500:
                    nRetStep = 62000;
                    break;
                case 62000:
                    //완공 APD REPORT
                    
                    TcpSocket.EquipmentData sendEqipData = new TcpSocket.EquipmentData();
                    sendEqipData.Command = "LOT_APD_REPORT";
                    sendEqipData.LotID = Globalo.dataManage.TaskWork.m_szChipID;
                    sendEqipData.Judge = Globalo.taskWork.m_nTestFinalResult;

                    int tCount = Globalo.dataManage.eepromData.EEpromDataList.Count;

                    for (int i = 0; i < Data.CMesData.APD_REPORT_MAX_COUNT; i++)
                    {
                        if (ProgramState.CurrentState == OperationState.Stopped)
                        {
                            szLog = $"[AUTO] Apd Report Stop.[STEP : {nStep}]";
                            Globalo.LogPrint("PcbPrecess", szLog);
                            break;
                        }
                        TcpSocket.EquipmentParameterInfo pInfo = new TcpSocket.EquipmentParameterInfo();
                        if (i < tCount)
                        {
                            pInfo.Name = Globalo.dataManage.eepromData.EEpromDataList[i].ITEM_NAME;
                            pInfo.Value = Globalo.dataManage.eepromData.EEpromDataList[i].ITEM_VALUE;
                        }
                        else
                        {
                            pInfo.Name = "PASS_MODEL";
                            pInfo.Value = "";
                        }
                        
                        sendEqipData.CommandParameter.Add(pInfo);
                    }
                    
                    //eeprom verify 할때 여기에 담긴다.
                    //Globalo.dataManage.eepromData.EEpromDataList.Add(tempEepData);

                    Globalo.tcpManager.SendMessageToClient(sendEqipData);


                    szLog = $"[AUTO] LOT_APD_REPORT SEND [STEP : {nRetStep}]";
                    Globalo.LogPrint("LotProcess", szLog);
                    nRetStep = 62500;

                    m_dTickCount = Environment.TickCount;
                    break;
                case 62500:
                    if (Globalo.taskWork.bRecv_Client_ApdReport == 0)               //APS_LOT_COMPLETE_CMD
                    {
                        szLog = $"[AUTO] Lot Processing Completed OK [STEP : {nRetStep}]";
                        Globalo.LogPrint("LotProcess", szLog);
                        nRetStep = 63000;
                        break;
                    }
                    else if (Globalo.taskWork.bRecv_Client_ApdReport == 1)      //Lot Processing Completed Fail
                    {
                        Globalo.taskWork.m_nTestFinalResult = 0;

                        //LOT_PROCESSING_COMPLETED_REPORT_10710 보내고 ACK 값이 0으로 안온 상황
                        //_stprintf_s(szLog, SIZE_OF_1K, _T("[AUTO] Lot Processing Completed Fail [STEP : %d]"), nStep);
                        //일시정지 없이 팝업만 띄워주면된다.
                        szLog = $"[AUTO] Lot Processing Completed Fail [STEP : {nRetStep}]";
                        Globalo.LogPrint("LotProcess", szLog, Globalo.eMessageName.M_INFO);         //일시정지 없이 팝업만 띄워주면된다.
                        nRetStep = 63000;
                        break;
                    }
                    else if (Globalo.taskWork.bRecv_Client_CtTimeOut == 1)      //Lot APD CT TimeOut
                    {
                        Globalo.taskWork.m_nTestFinalResult = 0;
                        // 1. APD 보내고 리턴이 안온 상황
                        //_stprintf_s(szLog, SIZE_OF_1K, _T("[AUTO] Lot APD Ct TimeOut [STEP : %d]"), nStep);

                        szLog = $"[AUTO] APD CT TimeOut [STEP : {nRetStep}]";
                        Globalo.LogPrint("LotProcess", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep = -62500;
                        break;
                    }
                    else if (Globalo.taskWork.bRecv_Client_CtTimeOut == 2)      //Lot Processing Completed CT TimeOut
                    {
                        Globalo.taskWork.m_nTestFinalResult = 0;

                        // 2. LOT_PROCESSING_COMPLETED_REPORT_10710 보내고 리턴이 안온 상황
                        //_stprintf_s(szLog, SIZE_OF_1K, _T("[AUTO] Lot Processing Completed Ct TimeOut [STEP : %d]"), nStep);

                        szLog = $"[AUTO] Lot Processing Completed CT TimeOut [STEP : {nRetStep}]";
                        Globalo.LogPrint("LotProcess", szLog, Globalo.eMessageName.M_WARNING);
                        nRetStep = -62500;
                        break;
                    }
                    else if ((Environment.TickCount - m_dTickCount) > 100000)
                    {
                        //time out
                        szLog = $"[AUTO] LOT APD TIMEOUT [STEP : {nRetStep}]";
                        Globalo.LogPrint("LotProcess", szLog, Globalo.eMessageName.M_WARNING);

                        nRetStep = -62000;
                    }
                    
                    break;
                case 63000:

                    nRetStep = 64000;
                    break;
                case 64000:

                    nRetStep = 69000;
                    break;
                case 69000:

                    Globalo.yamlManager.taskDataYaml.TaskData.ProductionInfo.TotalCount++;

                    _syncContext.Send(_ =>
                    {
                        if (Globalo.taskWork.m_nTestFinalResult == 1)
                        {
                            Globalo.camControl.setOverlayText("PASS", Color.Green);
                            Globalo.yamlManager.taskDataYaml.TaskData.ProductionInfo.OkCount++;
                        }
                        else
                        {
                            Globalo.camControl.setOverlayText("FAIL", Color.Red);
                            Globalo.yamlManager.taskDataYaml.TaskData.ProductionInfo.NgCount++;
                        }

                        Globalo.yamlManager.taskDataYaml.TaskDataSave();
                        Globalo.productionInfo.ProductionInfoSet();
                    }, null);


                    nRetStep = 30000;
                    break;
            }
            return nRetStep;
        }

        //
        //
        //
    }
    
}


