using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace ZenHandler.Process
{
    public class ManualProcess
    {
        private readonly SynchronizationContext _syncContext;
        private int m_dTickCount;
        //private int nCsvFineRetyCount = 0;
        private int nStep = 1;

        public ManualProcess()
        {
            _syncContext = SynchronizationContext.Current;
            m_dTickCount = 0;
        }
        public bool Manual_EEpromRead()
        {
            bool rtn = true;
            string szLog = "";
            int errorCode = 0;
            nStep = 1;
            while (nStep > 0)
            {
                switch (nStep)
                {
                    case 1:
                        errorCode = Globalo.mLaonGrabberClass.OpenDevice();
                        if (errorCode > 0)
                        {
                            szLog = $"[MANUAL] CCd OpenDevice Fail[STEP : {nStep}]";
                            Globalo.LogPrint("ManualProcess", szLog);
                            nStep = -1;
                            break;
                        }
                        szLog = $"[MANUAL] CCd OpenDevice Ok[STEP : {nStep}]";
                        Globalo.LogPrint("ManualProcess", szLog);

                        m_dTickCount = Environment.TickCount;
                        nStep++;
                        break;
                    case 2:
                        if ((Environment.TickCount - m_dTickCount) > 300)
                        {
                            nStep++;
                        }

                        break;
                    case 3:
                        szLog = $"[MANUAL] EEprom Read Start [STEP : {nStep}]";
                        Globalo.LogPrint("Manualthread", szLog);

                        Globalo.mCCdPanel.EEpromRead();

                        szLog = $"[MANUAL] EEprom Read Complete [STEP : {nStep}]";
                        Globalo.LogPrint("Manualthread", szLog);

                        if (ProgramState.NORINDA_MODE == true)
                        {
                            for (int i = 0; i < 20; i++)
                            {
                                Data.EEpromReadData tempEepData = new Data.EEpromReadData();
                                tempEepData.ITEM_NAME = "test" + i.ToString();
                                tempEepData.ITEM_VALUE = i.ToString();
                                tempEepData.RESULT = "pass";
                                Globalo.dataManage.eepromData.EEpromDataList.Add(tempEepData);
                            }
                            


                            Globalo.dataManage.eepromData.SaveExcelData(Globalo.dataManage.TaskWork.m_szChipID);
                        }
                        nStep++;
                        break;
                    case 4:
                        szLog = $"[MANUAL] EEPROM READ END[STEP : {nStep}]";
                        Globalo.LogPrint("ManualProcess", szLog);
                        nStep = -1;
                        break;
                    default:
                        nStep = -1;
                        break;
                }
            }
            return rtn;
        }
        public bool Manual_CCdRun()
        {
            bool rtn = true;
            string szLog = "";
            int errorCode = 0;
            nStep = 1;
            while (nStep > 0)
            {
                switch (nStep)
                {
                    case 1:
                        errorCode = Globalo.mLaonGrabberClass.OpenDevice();
                        if (errorCode > 0)
                        {
                            szLog = $"[MANUAL] CCd OpenDevice Fail[STEP : {nStep}]";
                            Globalo.LogPrint("ManualProcess", szLog);
                            nStep = -1;
                            break;
                        }
                        szLog = $"[MANUAL] CCd OpenDevice Ok[STEP : {nStep}]";
                        Globalo.LogPrint("ManualProcess", szLog);

                        m_dTickCount = Environment.TickCount;
                        nStep++;
                        break;
                    case 2:
                        if ((Environment.TickCount - m_dTickCount) > 300)
                        {
                            nStep++;
                        }
                            
                        break;
                    case 3:
                        rtn = Globalo.mLaonGrabberClass.StartGrab();
                        if (rtn == false)
                        {
                            szLog = $"[MANUAL] CCd StartGrab Fail[STEP : {nStep}]";
                            Globalo.LogPrint("ManualProcess", szLog);
                            nStep = -1;
                            break;
                        }
                        szLog = $"[MANUAL] CCd StartGrab Ok[STEP : {nStep}]";
                        Globalo.LogPrint("ManualProcess", szLog);

                        szLog = $"[MANUAL] CCD RUN END[STEP : {nStep}]";
                        Globalo.LogPrint("ManualProcess", szLog);
                        nStep = -1;
                        break;
                    default:
                        nStep = -1;
                        break;
                }
            }
            return rtn;
        }
        public bool Manual_Verify()
        {
            bool rtn = true;
            string szLog = "";
            int errorCode = 0;
            nStep = 1;
            while (nStep > 0)
            {
                switch (nStep)
                {
                    case 1:

                        szLog = $"[MANUL] {Globalo.dataManage.TaskWork.m_szChipID} csv File Find [STEP : {nStep}]";
                        Globalo.LogPrint("PcbPrecess", szLog);
                        rtn = Globalo.dataManage.eepromData.LoadExcelData(Globalo.dataManage.TaskWork.m_szChipID);
                        if (rtn == false)
                        {
                            szLog = $"[MANUL] {Globalo.dataManage.TaskWork.m_szChipID} csv Load Fail [STEP : {nStep}]";
                            Globalo.LogPrint("AutoPrecess", szLog, Globalo.eMessageName.M_WARNING);
                            nStep = -1;
                            break;
                        }
                        szLog = $"[MANUL] {Globalo.dataManage.TaskWork.m_szChipID} csv Load Ok[STEP : {nStep}]";
                        Globalo.LogPrint("PcbPrecess", szLog);
                        nStep++;
                        break;
                    case 2:
                        nStep++;
                        break;
                    case 3:
                        szLog = $"[MANUAL] CCd OpenDevice Start[STEP : {nStep}]";
                        Globalo.LogPrint("PcbPrecess", szLog);
                        errorCode = Globalo.mLaonGrabberClass.OpenDevice();
                        if (errorCode > 0)
                        {
                            szLog = $"[MANUAL] CCd OpenDevice Fail[STEP : {nStep}]";
                            Globalo.LogPrint("PcbPrecess", szLog);
                            nStep = -1;
                            break;
                        }
                        szLog = $"[MANUAL] CCd OpenDevice Ok[STEP : {nStep}]";
                        Globalo.LogPrint("PcbPrecess", szLog);
                        nStep++;
                        break;
                    case 4:
                        szLog = $"[MANUAL] EEPROM DATA READ START[STEP : {nStep}]";
                        Globalo.LogPrint("PcbPrecess", szLog);

                        rtn = Data.CEEpromData.EEpromDataRead();
                        if (rtn == true)
                        {
                            szLog = $"[MANUAL] EEPROM DATA READ OK[STEP : {nStep}]";
                            Globalo.LogPrint("PcbPrecess", szLog);


                            //
                            //
                            //EEPROM BINARY FILE 저장

                            string bcrLot = Data.CEEpromData.SanitizeFileName(Globalo.dataManage.TaskWork.m_szChipID);
                            Data.CEEpromData.SaveToBinaryFile(bcrLot, Globalo.dataManage.eepromData.EquipEEpromReadData);

                            szLog = $"[MANUAL] EEPROM BINARY FILE SAVE[STEP : {nStep}]";
                            Globalo.LogPrint("PcbPrecess", szLog);
                        }
                        else
                        {
                            szLog = $"[MANUAL] EEPROM DATA READ FAIL[STEP : {nStep}]";
                            Globalo.LogPrint("PcbPrecess", szLog, Globalo.eMessageName.M_WARNING);
                            nStep = -1;
                            break;
                        }

                        nStep++;
                        break;
                    case 5:
                        szLog = $"[MANUAL] EEPROM DATA VERIFY START[STEP : {nStep}]";
                        Globalo.LogPrint("PcbPrecess", szLog);

                        rtn = Data.CEEpromData.EEpromVerifyRun();


                        _syncContext.Send(_ =>
                        {
                            Globalo.mMainPanel.ShowVerifyResultGrid(Globalo.dataManage.eepromData.CsvRead_MMd_DataList, Globalo.dataManage.eepromData.EEpromDataList);
                        }, null);
                        if (rtn == false)
                        {
                            szLog = $"[MANUAL] EEPROM DATA VERIFY FAIL[STEP : {nStep}]";
                            Globalo.LogPrint("PcbPrecess", szLog, Globalo.eMessageName.M_WARNING);
                            nStep = -1;
                            break;
                        }


                        szLog = $"[MANUAL] EEPROM DATA VERIFY COMPLETE[STEP : {nStep}]";
                        Globalo.LogPrint("PcbPrecess", szLog);
                        nStep++;
                        break;
                    case 6:
                        rtn = Globalo.mLaonGrabberClass.CloseDevice();
                        //if (rtn == false)
                        //{
                        //    szLog = $"[MANUAL] CCd CloseDevice Fail[STEP : {nStep}]";
                        //    Globalo.LogPrint("PcbPrecess", szLog);
                        //    nStep = -1;
                        //    break;
                        //}

                        szLog = $"[MANUAL] CCd CloseDevice Complete[STEP : {nStep}]";
                        Globalo.LogPrint("PcbPrecess", szLog);

                        nStep++;
                        break;
                    case 7:
                        if (ProgramState.NORINDA_MODE == true)
                        {
                            TcpSocket.EquipmentData sendEqipData = new TcpSocket.EquipmentData();
                            sendEqipData.Command = "LOT_APD_REPORT";
                            sendEqipData.LotID = Globalo.dataManage.TaskWork.m_szChipID;
                            sendEqipData.Judge = Globalo.taskWork.m_nTestFinalResult;
                            int tCount = Globalo.dataManage.eepromData.EEpromDataList.Count;
                            for (int i = 0; i < Data.CMesData.APD_REPORT_MAX_COUNT; i++)
                            {
                                if (ProgramState.CurrentState == OperationState.Stopped)
                                {
                                    szLog = $"[MANUAL] Apd Report Stop.[STEP : {nStep}]";
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
                                    pInfo.Value = i.ToString();// "";
                                }

                                sendEqipData.CommandParameter.Add(pInfo);
                            }
                            //eeprom verify 할때 여기에 담긴다.
                            //Globalo.dataManage.eepromData.EEpromDataList.Add(tempEepData);

                            Globalo.tcpManager.SendMessageToClient(sendEqipData);
                        }
                        

                        szLog = $"[MANUAL] EEPROM VERIFY END[STEP : {nStep}]";
                        Globalo.LogPrint("ManualProcess", szLog);
                        nStep = -1;
                        break;
                    default:
                        nStep = -1;
                        break;
                }
                
            }

            return rtn;


        }
    }
}
