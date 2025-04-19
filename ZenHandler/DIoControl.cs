using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace ZenHandler
{
    public class DIoControl
    {
        //public event delLogSender eLogSender;       //외부에서 호출할때 사용

        
        public bool mDioThreadRun = true;
        public int nInModuleCount = 0;
        public int nOutModuleCount = 0;
        public uint[] m_dwDIn = new uint[5];   //모듈 개수
        public uint[,] m_dwDOut = new uint[5, 4];    //모듈개수 , Offset 0 ~ 3
        public int[] ArrInModuleCh = new int[5];
        public int[] ArrOutModuleCh = new int[5];


        public int dCurReadModuleCh = 0;     //현재 in io 채널
        public int dCurOutModuleCh = 0;     //현재 in io 채널

        private System.Windows.Forms.Timer DioTimer;
        public DIoControl()
        {
            if (ProgramState.ON_LINE_MOTOR == false)
            {
                nOutModuleCount = 1;
                nInModuleCount = 3;
                nOutModuleCount = 3;
            }
            DioTimer = new System.Windows.Forms.Timer();
            DioTimer.Interval = 10; // 1초 (1000밀리초) 간격 설정
            DioTimer.Tick += new EventHandler(Dio_Timer_Tick);
        }
        /*
                uint testio = 0xFF;

                testio &= ~((uint)DioDefine.DIO_OUT_ADDR.BUZZER1 | (uint)DioDefine.DIO_OUT_ADDR.BUZZER3);       //testio에서 BUZZER1 하고 BUZZER3을 끄다.
                testio |= ((uint)DioDefine.DIO_OUT_ADDR.BUZZER1 | (uint)DioDefine.DIO_OUT_ADDR.BUZZER3);        //testio에서 BUZZER1 하고 BUZZER3 만 켜다.


         
         */
        //
        //
        //
        public void DioInit()
        {
            bool rtn = OpenDevice();
            if (rtn == true)
            {
                //io get Thread 시작
                //Thread Start
                ReadByteOut(1, 0); Thread.Sleep(10);
                ReadByteOut(1, 1); Thread.Sleep(10);
                ReadByteOut(1, 2); Thread.Sleep(10);
                ReadByteOut(1, 3);
            }
        }
        private bool OpenDevice()
        {
            if (Globalo.motionManager.bConnected)//CAXL.AxlOpen(7) == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
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
                                        case AXT_MODULE.AXT_SIO_DI32: strData = String.Format("[{0:D2}:{1:D2}] SIO-DI32", nBoardNo, i); break;
                                        case AXT_MODULE.AXT_SIO_DO32P: strData = String.Format("[{0:D2}:{1:D2}] SIO-DO32P", nBoardNo, i); break;
                                        case AXT_MODULE.AXT_SIO_DB32P: strData = String.Format("[{0:D2}:{1:D2}] SIO-DB32P", nBoardNo, i); break;
                                        case AXT_MODULE.AXT_SIO_DO32T: strData = String.Format("[{0:D2}:{1:D2}] SIO-DO32T", nBoardNo, i); break;
                                        case AXT_MODULE.AXT_SIO_DB32T: strData = String.Format("[{0:D2}:{1:D2}] SIO-DB32T", nBoardNo, i); break;
                                        case AXT_MODULE.AXT_SIO_RDI32: strData = String.Format("[{0:D2}:{1:D2}] SIO_RDI32", nBoardNo, i); break;
                                        case AXT_MODULE.AXT_SIO_RDO32: strData = String.Format("[{0:D2}:{1:D2}] SIO_RDO32", nBoardNo, i); break;
                                        case AXT_MODULE.AXT_SIO_RDB128MLII: strData = String.Format("[{0:D2}:{1:D2}] SIO-RDB128MLII", nBoardNo, i); break;
                                        case AXT_MODULE.AXT_SIO_RSIMPLEIOMLII: strData = String.Format("[{0:D2}:{1:D2}] SIO-RSIMPLEIOMLII", nBoardNo, i); break;
                                        case AXT_MODULE.AXT_SIO_RDO16AMLII: strData = String.Format("[{0:D2}:{1:D2}] SIO-RDO16AMLII", nBoardNo, i); break;
                                        case AXT_MODULE.AXT_SIO_RDO16BMLII: strData = String.Format("[{0:D2}:{1:D2}] SIO-RDO16BMLII", nBoardNo, i); break;
                                        case AXT_MODULE.AXT_SIO_RDB96MLII: strData = String.Format("[{0:D2}:{1:D2}] SIO-RDB96MLII", nBoardNo, i); break;
                                        case AXT_MODULE.AXT_SIO_RDO32RTEX:
                                            ArrOutModuleCh[nOutModuleCount] = i;
                                            nOutModuleCount++;
                                            strData = String.Format("[{0:D2}:{1:D2}] SIO-RDO32RTEX", nBoardNo, i);
                                            break;
                                        case AXT_MODULE.AXT_SIO_RDI32RTEX:
                                            ArrInModuleCh[nInModuleCount] = i;
                                            nInModuleCount++;
                                            strData = String.Format("[{0:D2}:{1:D2}] SIO-RDI32RTEX", nBoardNo, i);
                                            break;
                                        case AXT_MODULE.AXT_SIO_RDB32RTEX: strData = String.Format("[{0:D2}:{1:D2}] SIO-RDB32RTEX", nBoardNo, i); break;
                                        case AXT_MODULE.AXT_SIO_DI32_P: strData = String.Format("[{0:D2}:{1:D2}] SIO-DI32_P", nBoardNo, i); break;
                                        case AXT_MODULE.AXT_SIO_DO32T_P: strData = String.Format("[{0:D2}:{1:D2}] SIO-DO32T_P", nBoardNo, i); break;
                                        case AXT_MODULE.AXT_SIO_RDB32T: strData = String.Format("[{0:D2}:{1:D2}] SIO-RDB32T", nBoardNo, i); break;
                                    }
                                    if (i % 2 == 0)
                                    {
                                        //eLogSender("Diocontrols",strData);
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
                        //MessageBox.Show("Module not exist.");
                        //eLogSender("Diocontrols", "Dio Module Open not exist");
                        return false;
                    }
                }
            }
            return false;
        }
        private void Dio_Timer_Tick(object sender, EventArgs e)
        {

        }
        public bool ReadDWordIn(int nModuleNo)
        {
            uint dwInputVal = 0;
            int nIndex = (nModuleNo / 2);
            if (CAXD.AxdiReadInportDword(nModuleNo, 0, ref dwInputVal) == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                m_dwDIn[nIndex] = dwInputVal;
                return true;
            }
            return false;
        }
        public bool ReadByteOut(int nModuleNo, int dOffset)
        {
            uint dwInputVal = 0;
            
            if (CAXD.AxdoReadOutportByte(nModuleNo, dOffset, ref dwInputVal) == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                int nIndex = (nModuleNo / 2);
                m_dwDOut[nIndex, dOffset] = dwInputVal;
                return true;
            }
            return false;
        }
        public bool DioWriteOutportByte(int nIndex, int nOffset, uint uAddr)
        {
            int uModuleID = nIndex;
            uint uValue = 0;

            uValue = m_dwDOut[uModuleID, nOffset];

            //if (bSignal)
            //{
            //    uValue = uValue | (uAddr);
            //}
            //else
            //{
            //    uValue = uValue & ~(uAddr);
            //}

            uint uStatus = CAXD.AxdoWriteOutportByte(uModuleID, nIndex, uAddr);

            if (uStatus == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                return false;
            }
            return false;
        }
        public bool DioWriteOutportByte(int nIndex, int nOffset, uint uOnAddr, uint uOffAddr)
        {
            int uModuleID = nIndex;
            uint uValue = 0;
            
            uValue = m_dwDOut[uModuleID, nOffset];


            uValue = uValue | (uOnAddr);        //uValue 에서 uOnAddr를 켠다.  OR(|) 연산
            uValue = uValue & ~(uOffAddr);      //uOffAddr을 반대로 바꾸고 ,uValue에서 uOffAddr를 반대로 변경  AND(&) 연산


            uint uStatus = CAXD.AxdoWriteOutportByte(uModuleID, nIndex, uValue);
            //CAXD.AxdoWriteOutportBit(uModuleID, nIndex, uValue);

            if (uStatus == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                return false;
            }
            return false;
        }
        private bool SelectHighIndex(int nIndex, uint uValue)
        {
            int nModuleCount = 0;
            int SelectedIndex = 0;

            CAXD.AxdInfoGetModuleCount(ref nModuleCount);

            if (nModuleCount > 0)
            {
                int nBoardNo = 0;
                int nModulePos = 0;
                uint uModuleID = 0;

                CAXD.AxdInfoGetModule(SelectedIndex, ref nBoardNo, ref nModulePos, ref uModuleID);

                switch ((AXT_MODULE)uModuleID)
                {
                    case AXT_MODULE.AXT_SIO_DO32P:
                    case AXT_MODULE.AXT_SIO_DO32T:
                    case AXT_MODULE.AXT_SIO_RDO32:
                        CAXD.AxdoWriteOutportBit(SelectedIndex, nIndex, uValue);
                        break;

                    default:
                        return false;
                }
            }

            return true;
        }
        public bool Setbuzzer(bool bOn, int nType)
        {
            int uModuleID = 1;
            int nOffset = 0;

            uint uValue = 0;
            uint dwInputVal = 0;
            uValue = m_dwDOut[uModuleID, nOffset];

            if (CAXD.AxdoReadOutportByte(uModuleID, nOffset, ref dwInputVal) == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {

                switch (nType)
                {
                    case 0:
                        dwInputVal &= ~((uint)DioDefine.DIO_OUT_ADDR_CH0.BUZZER1 | (uint)DioDefine.DIO_OUT_ADDR_CH0.BUZZER2
                                        | (uint)DioDefine.DIO_OUT_ADDR_CH0.BUZZER3 | (uint)DioDefine.DIO_OUT_ADDR_CH0.BUZZER4);     //dwInputVal BUZZER1,2,3,4를 끄다.
                        break;
                    case 1:
                        dwInputVal |= ((uint)DioDefine.DIO_OUT_ADDR_CH0.BUZZER1);                                               //dwInputVal BUZZER1 만 켜다.
                        dwInputVal &= ~((uint)DioDefine.DIO_OUT_ADDR_CH0.BUZZER2 | 
                            (uint)DioDefine.DIO_OUT_ADDR_CH0.BUZZER3 | (uint)DioDefine.DIO_OUT_ADDR_CH0.BUZZER4);//dwInputVal BUZZER 2,3,4를 끄다.
                        break;
                    case 2:
                        dwInputVal |= ((uint)DioDefine.DIO_OUT_ADDR_CH0.BUZZER2);
                        dwInputVal &= ~((uint)DioDefine.DIO_OUT_ADDR_CH0.BUZZER1 | (uint)DioDefine.DIO_OUT_ADDR_CH0.BUZZER3 | (uint)DioDefine.DIO_OUT_ADDR_CH0.BUZZER4);
                        break;
                    case 3:
                        dwInputVal |= ((uint)DioDefine.DIO_OUT_ADDR_CH0.BUZZER3);
                        dwInputVal &= ~((uint)DioDefine.DIO_OUT_ADDR_CH0.BUZZER1 | (uint)DioDefine.DIO_OUT_ADDR_CH0.BUZZER2| (uint)DioDefine.DIO_OUT_ADDR_CH0.BUZZER4);
                        break;
                    case 4:
                        dwInputVal |= ((uint)DioDefine.DIO_OUT_ADDR_CH0.BUZZER4);
                        dwInputVal &= ~((uint)DioDefine.DIO_OUT_ADDR_CH0.BUZZER1 | (uint)DioDefine.DIO_OUT_ADDR_CH0.BUZZER2| (uint)DioDefine.DIO_OUT_ADDR_CH0.BUZZER3);
                        break;
                    default:
                        dwInputVal &= ~((uint)DioDefine.DIO_OUT_ADDR_CH0.BUZZER1 | (uint)DioDefine.DIO_OUT_ADDR_CH0.BUZZER2
                                        | (uint)DioDefine.DIO_OUT_ADDR_CH0.BUZZER3 | (uint)DioDefine.DIO_OUT_ADDR_CH0.BUZZER4);       //testio에서 BUZZER1 하고 BUZZER3을 끄다.
                        break;
                }


                if (this.DioWriteOutportByte(uModuleID, nOffset, dwInputVal) == false)
                {
                    return false; //out Write Fail
                }

                return true;
            }
            
            return false; //out Read Fail
        }
        public void DioEnd()
        {
            DioTimer.Stop();

        }
    }
}
