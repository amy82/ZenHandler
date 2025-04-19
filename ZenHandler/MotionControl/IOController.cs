using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZenHandler.MotionControl
{
    public class IOController
    {
        //private object[] _locks; // 여러 개의 락 객체 배열
        private Dictionary<int, object> _locks = new Dictionary<int, object>();
        //Dio
        public int DiModuleCount = 0;
        public int DoModuleCount = 0;

        protected CancellationTokenSource ctsMotion;
        public Dictionary<int, uint[]> m_dwDInDict = new Dictionary<int, uint[]>();
        public Dictionary<int, uint[]> m_dwDOutDict = new Dictionary<int, uint[]>();        //모듈번호가 1,3,5 순차적이지 않아서 Dictionary 로 변경

        //type 1 = 렌즈 그립 In(2) , Out(2)
        //type 2 = 부저 out(4)
        //type 3 = 타워램프 out(3)
        //type 4 = 도어 in (N개) , Out(N개)
        //type 5 = lg Door Out(1)
        //type 6 = Start Button In(2)   


        public IOController()
        {
            m_dwDInDict.Clear();
            m_dwDOutDict.Clear();
        }
        public void Close()
        {
            ctsMotion?.Cancel();
        }
        public async void MotionTaskRun()
        {
            await TaskReadDio();
        }
        public async Task<bool> TaskReadDio()
        {
            ctsMotion?.Dispose();

            ctsMotion = new CancellationTokenSource();
            CancellationToken token = ctsMotion.Token;

            //bool isSuccess = false;
            try
            {
                await Task.Run(() =>
                {
                    while (true)
                    {
                        if (ctsMotion.Token.IsCancellationRequested)
                        {
                            break;
                            //Console.WriteLine("취소 요청 감지됨");
                            //Globalo.LogPrint("ManualControl", $"취소 요청 감지됨");
                            //ctsMotion.Token.ThrowIfCancellationRequested(); // 예외 던지기 (catch로 감) 취소 요청 시 예외 발생
                        }
                        foreach (var inModule in m_dwDInDict)
                        {
                            int subCnt = inModule.Value.Length;
                            for (int i = 0; i < subCnt; i++)
                            {
                                ///ReadByteIn(inModule.Key, i);
                                ReadDWordIn(inModule.Key);
                                Thread.Sleep(10);
                            }
                        }
                        Thread.Sleep(10);
                    }
                    // 작업 정상 종료
                }, token);
            }
            catch (OperationCanceledException)
            {
                Globalo.LogPrint("ManualControl", $"TaskReadDio Cancel");
            }
            catch (Exception e)
            {
                // 그 외 예외 처리
                Globalo.LogPrint("ManualControl", $"TaskReadDio Fail-{e.Message}");
            }
            finally
            {
                // 리소스 정리
                ctsMotion?.Dispose();  // cts가 null이 아닐 때만 Dispose 호출
                ////ctsMotion = null;      // cts를 null로 설정하여 다음 작업에서 새로 생성할 수 있게
            }

            Globalo.LogPrint("ManualControl", $"[TASK] TaskReadDio End");
            return true;
        }
        public void DioInit()
        {
            bool rtn = OpenDevice();

            if (rtn == false)
            {
                return;
            }
            else
            {
                int i = 0;
                foreach (var ModuleNo in m_dwDOutDict)      //foreach (var ModuleNo in OutModuleList)
                {
                    int subCnt = ModuleNo.Value.Length;
                    for (i = 0; i < subCnt; i++)
                    {
                        ReadByteOut(ModuleNo.Key, i);

                        Thread.Sleep(10);
                    }
                }
                foreach (int key in m_dwDOutDict.Keys)
                {
                    _locks[key] = new object(); // 각 인덱스에 개별 객체 할당
                }
                int OutCount = m_dwDOutDict.Count;
                if (OutCount > 0)
                {
                    //_locks = new object[OutCount]; // 배열 크기 설정
                    //for (i = 0; i < OutCount; i++)
                    //{
                    //    _locks[i] = new object(); // 각 인덱스에 개별 객체 할당
                    //}
                    
                }

                MotionTaskRun();        //in 신호 얻기 Task

            }
        }
        public bool ReadBitIn(int nModuleNo, int offset, bool bWait = false)
        {
            uint uValue = 0;
            //offset 0 ~ 31

            uint uStatus = CAXD.AxdiReadInportBit(nModuleNo, offset, ref uValue);

            if (uStatus == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                if (uValue == 1)
                {
                    return true;
                }
            }
            return false;
        }
        private bool ReadDWordIn(int nModuleNo)
        {
            uint dwInputVal = 0;

            uint uStatus = CAXD.AxdiReadInportDword(nModuleNo, 0, ref dwInputVal);
            if (uStatus == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                //Globalo.motionManager.m_dwDIn[nIndex] = dwInputVal;

                m_dwDInDict[nModuleNo][0] = dwInputVal;
                return true;
            }
            return false;
        }
        private bool ReadByteIn(int nModuleNo, int dOffset)
        {
            uint dwInputVal = 0;
            uint uStatus = CAXD.AxdiReadInportByte(nModuleNo, dOffset, ref dwInputVal);

            if (uStatus == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                m_dwDInDict[nModuleNo][dOffset] = dwInputVal;
                return true;
            }
            return false;
        }
        private bool ReadByteOut(int nModuleNo, int dOffset)
        {
            uint dwInputVal = 0;

            uint uStatus = CAXD.AxdoReadOutportByte(nModuleNo, dOffset, ref dwInputVal);

            if (uStatus == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                m_dwDOutDict[nModuleNo][dOffset] = dwInputVal;
                return true;
            }
            return false;
        }
        public bool DioWriteOutportByte(int lModuleNo, int nOffset, uint uAddr, bool bSignal)
        {
            //ex) io 신호 하나 on,off
            int uModuleID = lModuleNo;
            uint uValue = 0;

            uValue = m_dwDOutDict[uModuleID][nOffset];

            if (bSignal)
            {
                uValue = uValue | (uAddr);
            }
            else
            {
                uValue = uValue & ~(uAddr);
            }

            uint uStatus = CAXD.AxdoWriteOutportByte(uModuleID, nOffset, uValue);

            if (uStatus == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                m_dwDOutDict[uModuleID][nOffset] = uValue;
                return false;
            }
            return false;
        }
        public bool DioWriteOutportByte(int lModuleNo, int nOffset, uint uOnAddr, uint uOffAddr)
        {
            //ex) io 신호 두개 1번 = On , 2번 = Off

            lock (_locks[lModuleNo]) //한 번에 하나의 스레드만 실행 가능
            {
                int uModuleID = lModuleNo;
                uint uValue = 0;

                uValue = m_dwDOutDict[uModuleID][nOffset];


                uValue = uValue | (uOnAddr);
                uValue = uValue & ~(uOffAddr);


                uint uStatus = CAXD.AxdoWriteOutportByte(uModuleID, nOffset, uValue);
                if (uStatus == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    m_dwDOutDict[uModuleID][nOffset] = uValue;
                    return true;
                }
            }
            return false;
        }
        private bool OpenDevice()
        {
            uint uStatus = 0;

            if (CAXD.AxdInfoIsDIOModule(ref uStatus) == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                if ((AXT_EXISTENCE)uStatus == AXT_EXISTENCE.STATUS_EXIST)
                {
                    int nModuleCount = 0;
                    if (CAXD.AxdInfoGetModuleCount(ref nModuleCount) == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                    {
                        short i = 0;
                        int nBoardNo = 0;
                        int nModulePos = 0;
                        uint uModuleID = 0;
                        string strData = "";

                        for (i = 0; i < nModuleCount; i++)
                        {
                            if (CAXD.AxdInfoGetModule(i, ref nBoardNo, ref nModulePos, ref uModuleID) == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                            {
                                switch ((AXT_MODULE)uModuleID)
                                {
                                    case AXT_MODULE.AXT_SIO_RDI32RTEX:  // Digital IN  32점
                                        //InModuleList.Add(i);
                                        m_dwDInDict.Add(i, new uint[1]);
                                        strData = String.Format("[{0:D2}:{1:D2}] SIO-RDI32RTEX", nBoardNo, i);
                                        break;

                                    case AXT_MODULE.AXT_SIO_RDO32RTEX:  // Digital OUT  32점
                                        //OutModuleList.Add(i);
                                        m_dwDOutDict.Add(i, new uint[4]);
                                        strData = String.Format("[{0:D2}:{1:D2}] SIO-RDO32RTEX", nBoardNo, i);
                                        break;

                                    case AXT_MODULE.AXT_SIO_RDB32RTEX:  //Digital IN/OUT 16 + 16 = 32점
                                        //InModuleList.Add(i);
                                        //OutModuleList.Add(i);
                                        m_dwDInDict.Add(i, new uint[1]);
                                        m_dwDOutDict.Add(i, new uint[2]);
                                        strData = String.Format("[{0:D2}:{1:D2}] SIO-RDB32RTEX", nBoardNo, i);
                                        break;
                                        //m_dwDInList.Add(0);
                                        //m_dwDOutList.Add(new uint[2]);
                                        //m_dwDInList.Add(0);
                                        //m_dwDOutList.Add(new uint[4]);
                                        //case AXT_MODULE.AXT_SIO_DI32: strData = String.Format("[{0:D2}:{1:D2}] SIO-DI32", nBoardNo, i); break;
                                        //case AXT_MODULE.AXT_SIO_DO32P: strData = String.Format("[{0:D2}:{1:D2}] SIO-DO32P", nBoardNo, i); break;
                                        //case AXT_MODULE.AXT_SIO_DB32P: strData = String.Format("[{0:D2}:{1:D2}] SIO-DB32P", nBoardNo, i); break;
                                        //case AXT_MODULE.AXT_SIO_DO32T: strData = String.Format("[{0:D2}:{1:D2}] SIO-DO32T", nBoardNo, i); break;
                                        //case AXT_MODULE.AXT_SIO_DB32T: strData = String.Format("[{0:D2}:{1:D2}] SIO-DB32T", nBoardNo, i); break;
                                        //case AXT_MODULE.AXT_SIO_RDI32: strData = String.Format("[{0:D2}:{1:D2}] SIO_RDI32", nBoardNo, i); break;
                                        //case AXT_MODULE.AXT_SIO_RDO32: strData = String.Format("[{0:D2}:{1:D2}] SIO_RDO32", nBoardNo, i); break;
                                        //case AXT_MODULE.AXT_SIO_RDB128MLII: strData = String.Format("[{0:D2}:{1:D2}] SIO-RDB128MLII", nBoardNo, i); break;
                                        //case AXT_MODULE.AXT_SIO_RSIMPLEIOMLII: strData = String.Format("[{0:D2}:{1:D2}] SIO-RSIMPLEIOMLII", nBoardNo, i); break;
                                        //case AXT_MODULE.AXT_SIO_RDO16AMLII: strData = String.Format("[{0:D2}:{1:D2}] SIO-RDO16AMLII", nBoardNo, i); break;
                                        //case AXT_MODULE.AXT_SIO_RDO16BMLII: strData = String.Format("[{0:D2}:{1:D2}] SIO-RDO16BMLII", nBoardNo, i); break;
                                        //case AXT_MODULE.AXT_SIO_RDB96MLII: strData = String.Format("[{0:D2}:{1:D2}] SIO-RDB96MLII", nBoardNo, i); break;
                                        //case AXT_MODULE.AXT_SIO_RDB32RTEX: strData = String.Format("[{0:D2}:{1:D2}] SIO-RDB32RTEX", nBoardNo, i); break;
                                        //case AXT_MODULE.AXT_SIO_DI32_P: strData = String.Format("[{0:D2}:{1:D2}] SIO-DI32_P", nBoardNo, i); break;
                                        //case AXT_MODULE.AXT_SIO_DO32T_P: strData = String.Format("[{0:D2}:{1:D2}] SIO-DO32T_P", nBoardNo, i); break;
                                        //case AXT_MODULE.AXT_SIO_RDB32T: strData = String.Format("[{0:D2}:{1:D2}] SIO-RDB32T", nBoardNo, i); break;
                                }
                                if (i % 2 == 0)
                                {
                                    //eLogSender("Diocontrols", strData);
                                    //inTxt.Text = strData;
                                    //inIndexTxt.Text = (mdata.dCurReadModuleCh + 1).ToString() + " / " + nInModuleCount.ToString();
                                }
                                else
                                {
                                    //eLogSender("Diocontrols", strData);
                                    //OutTxt.Text = strData;
                                    //OutIndexTxt.Text = (mdata.dCurOutModuleCh + 1).ToString() + " / " + nOutModuleCount.ToString();
                                }
                                //comboModule.Items.Add(strData);
                            }
                        }
                        // ModuleName = strData;


                        //comboModule.SelectedIndex = 0;
                        return true;
                    }
                }
                else
                {
                    Console.WriteLine("Module not exist.");
                    //MessageBox.Show("Module not exist.");
                    //eLogSender("Diocontrols", "Dio Module Open not exist");
                    return false;
                }
            }

            return false;
        }
    }
}
