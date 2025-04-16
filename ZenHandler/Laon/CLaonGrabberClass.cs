using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using OpenCvSharp;
using LgitLibrary;
using System.IO;

namespace ZenHandler
{
    public class CLaonGrabberClass : IDisposable
    {
        //public event delLogSender eLogSender;       //외부에서 호출할때 사용

        private int m_nUnit = 0;

        public const int MAX_READ_WRITE_LENGTH = 1024;      //라온 보드 최대 1024 Byte 까지 Read / Write 가능

        public string currentIniFile;
        //제품에서 읽은 eeprom Data 저장 
        //
        //
        public byte[] EEpromReadData; // EEPROM 데이터 읽기
        public Dictionary<ushort, byte> eepromDicData = new Dictionary<ushort, byte>();
        //eepromDicData[0x0001] = 0xA5;
        //eepromDicData.Clear();  //초기화 방법
        //eepromDicData = new Dictionary<ushort, byte>(); //다시 할당
        //eepromDicData.Add(i, byteArray[i]);

        public List<Tuple<ushort, byte>> eepromListData = new List<Tuple<ushort, byte>>();
        //eepromListData.Add(Tuple.Create((ushort)0x0001, (byte)0xA5));
        //eepromListData.Clear(); //초기화 방법
        //eepromListData = new List<Tuple<ushort, byte>>(); //다시 할당


        //if (eepromData1.SequenceEqual(eepromData2))


        private bool m_GrabDllLoadComplete;
        public bool M_GrabDllLoadComplete
        {
            get { return m_GrabDllLoadComplete; }
            set { m_GrabDllLoadComplete = value; }
        }
        private bool m_bOpen;
        public bool M_bOpen
        {
            get{return m_bOpen; }
            set{ m_bOpen = value;}
        }

        private int m_cBoardIndex;
        public int M_cBoardIndex
        {
            get { return m_cBoardIndex; }
            set { m_cBoardIndex = value; }
        }

        public int m_nCurrentCcdState;
        public Mat imageItp;
        public Mat[] m_pImageBuff;
        public Mat[] m_pGrabBuff;
        public byte[] m_pFrameRawBuffer;
        public byte[] m_pFrameBMPBuffer;
        public int m_nWidth;                               // CCD SENSOR SIZE X
        public int m_nHeight;								// CCD SENSOR SIZE Y
        public int nFrameRawSize;
        public int nBmpSize;
        // public ClassLibrary2.ClassLibrary2 GrabberDll = new ClassLibrary2.ClassLibrary2();


        
        public TDATASPEC mGrabSpec;
        private bool disposed = false;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string name, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileInt(string lpAppName, string lpKeyName, int nDefault, string lpFileName);

        public CLaonGrabberClass()
        {
            m_pFrameRawBuffer = null;
            m_pFrameBMPBuffer = null;
            m_pImageBuff = new Mat[3];
            m_pGrabBuff = new Mat[3];

            double[] mydouble = new double[5];
            mydouble[0] = 0.1;
            mydouble[1] = 0.12;
            mydouble[2] = 0.13;
            mydouble[3] = 0.14;
            mydouble[4] = 0.15;
            byte[] myRaw;
            myRaw = null;
            myRaw = new byte[5];
            IntPtr RawPtr = Marshal.UnsafeAddrOfPinnedArrayElement(myRaw, 0);
            IntPtr doublePtr = Marshal.UnsafeAddrOfPinnedArrayElement(mydouble, 0);

            int mLength = mydouble.Length;

            //errorCode = m_pBoard->WriteI2CBurst(SlaveAddr, AddrArr[i], 2, Data + i, 1);
            //errorCode = m_pBoard->ReadI2CBurst(SlaveAddr, checkAddr, 2, pReadData, (unsigned short)1);

            //errorCode = m_pBoard->ReadI2CMultiAddrBurst(SlaveAddress, TempAdress1, AddressByteCount, TempData1, ReadByteCount);
        }
        // 해제할 리소스가 있다면 여기서 정리
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); // GC가 다시 호출하지 않도록 설정
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    // 관리되는 리소스 해제 (예: 이벤트 해제, 파일 닫기)

                    // 관리되는 리소스 해제
                    imageItp?.Dispose();

                    if (m_pImageBuff != null)
                    {
                        foreach (var mat in m_pImageBuff)
                        {
                            mat?.Dispose();
                        }
                    }

                    if (m_pGrabBuff != null)
                    {
                        foreach (var mat in m_pGrabBuff)
                        {
                            mat?.Dispose();
                        }
                    }

                    m_pFrameRawBuffer = null;
                    m_pFrameBMPBuffer = null;
                }

                // 비관리 리소스 해제 (예: 핸들, 포인터 등)
                disposed = true;
            }
        }
        // 소멸자 (Finalizer)
        ~CLaonGrabberClass()
        {
            Dispose(false);
        }
        public void SetUnit(int nUnit)
        {
            m_nUnit = nUnit;
        }
        public bool CloseDevice()
        {
            M_bOpen = false;
            if (Globalo.GrabberDll.mIsGrabStarted() == true)
            {
                //stopGrab
                Globalo.GrabberDll.mGrabStop();
                
            }
            Globalo.GrabberDll.mCloseBoard();
            //closeDevice
            //eLogSender("GrabberDll", $"[CCD]Laon Close");
            m_nCurrentCcdState = (int)CCD_GRAB_MODE.CCD_GRAB_STOP;
            UiconfigLoad();         //CloseDevice
            return true;
        }
        public bool StopGrab()
        {
            if (Globalo.GrabberDll.mIsGrabStarted() == true)
            {
                //stopGrab
                Globalo.GrabberDll.mGrabStop();
            }
            //eLogSender("GrabberDll", $"[CCD]Laon Stop");
            m_nCurrentCcdState = (int)CCD_GRAB_MODE.CCD_GRAB_STOP;
            return true;
        }
        public bool StartGrab()
        {
            m_nCurrentCcdState = (int)CCD_GRAB_MODE.CCD_GRAB_STOP;
            if (Globalo.GrabberDll.mGrabStart() == 0)
            {
                //ok
                m_nCurrentCcdState = (int)CCD_GRAB_MODE.CCD_GRAB_LIVE;
                //eLogSender("GrabberDll", $"[CCD]Laon Start Ok");
            }
            else
            {
                //fail
                //eLogSender("GrabberDll", $"[CCD]Laon Start Fail");
                return false;
            }
            
            return true;
        }
        public int OpenDevice()
        {
            if (M_bOpen)
            {
                return 0;
            }
            m_nCurrentCcdState = (int)CCD_GRAB_MODE.CCD_GRAB_STOP;
            if (Globalo.GrabberDll.mIsGrabStarted() == true)
            {
                //stopGrab
                Globalo.GrabberDll.mGrabStop();
                Globalo.GrabberDll.mCloseBoard();
                //closeDevice
            }
            int nErrorCode = -1;
            nErrorCode = Globalo.GrabberDll.mOpenBoard((sbyte)m_cBoardIndex);
            if(nErrorCode > 0)
            {
                //error
                //eLogSender("GrabberDll",  $"[CCD] Laon Err:" + nErrorCode.ToString());
                return nErrorCode;
            }
            M_bOpen = true;
            //eLogSender("GrabberDll", $"[CCD] Laon Open Ok");
            return 0;
        }
        public void UiconfigSave(string iniName)
        {
            string uiPath = Path.Combine(Data.CPath.SENSOR_INI_DIR, "UIConfig", "UIConfig.ini");

            StringBuilder sb = new StringBuilder(255);
            WritePrivateProfileString("UIConfig", "GrabberINI", iniName, uiPath);

            //GetPrivateProfileString("UIConfig", "GrabberINI", "Default.ini", sb, Globalo.MAX_PATH, uiPath);
            //string mGetFilename = sb.ToString();
        }
        public void UiconfigLoad()
        {
            //D:\EVMS\EEPROM_VERIFY\Model\DEFAULT_MODEL\Initialize\UIConfig
            //string mFilePath = Data.CPath.SENSOR_INI_DIR + "\\UIConfig\\UIConfig.ini";


            //경로 D:\EVMS\EEPROM_VERIFY\Model\ + "모델명" + \Initialize\UIConfig
            //Path.Combine(basePath, year, month, day);



            //string uiPath = Path.Combine(Data.CPath.BASE_MODEL_PATH, Globalo.yamlManager.secsGemDataYaml.MesData.SecGemData.CurrentModelName, "Initialize", "UIConfig", "UIConfig.ini");
            //string modelPath = Path.Combine(Data.CPath.BASE_MODEL_PATH, Globalo.yamlManager.secsGemDataYaml.MesData.SecGemData.CurrentModelName + "\\Initialize\\UIConfig");

            string uiPath = Path.Combine(Data.CPath.SENSOR_INI_DIR,  "UIConfig", "UIConfig.ini");

            StringBuilder sb = new StringBuilder(255);
            GetPrivateProfileString("UIConfig", "GrabberINI", "Default.ini", sb, Globalo.MAX_PATH, uiPath);
            currentIniFile = sb.ToString();


            
            string miniFilePath = Path.Combine(Data.CPath.SENSOR_INI_DIR, currentIniFile);

            try
            {
                unsafe
                {
                    byte[] bytes = Encoding.ASCII.GetBytes(miniFilePath);

                    fixed (byte* p = bytes)
                    {
                        sbyte* sp = (sbyte*)p;
                        Globalo.GrabberDll.mSetINIFile(sp);

                        //SP is now what you want
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"UiconfigLoad 처리 중 예외 발생: {ex.Message}");
                //eLogSender("GrabberDll", $"[CCD] UiconfigLoad Load Fail");
            }
            
        }
        public void SelectSensor()
        {
            string logstr = "";

            m_nWidth = Globalo.GrabberDll.mGetWidth();
            m_nHeight = Globalo.GrabberDll.mGetHeight();

            logstr = $"Width:{m_nWidth} Height {m_nHeight} ";

            //eLogSender("GrabberDll", logstr);
            nFrameRawSize = Globalo.GrabberDll.mGetFrameRawSize(); 
            nBmpSize = Globalo.GrabberDll.mGetFrameBMPSize();

            mGrabSpec.eOutMode = (EOUTMODE)Globalo.GrabberDll.mGetOutMode();
            mGrabSpec.eDataFormat = (EDATAFORMAT)Globalo.GrabberDll.mGetDataFormat();
            mGrabSpec.eSensorType = (ESENSORTYPE)Globalo.GrabberDll.mGetSensorType();

            mGrabSpec.eDemosaicMethod = EDEMOSAICMETHOD.DEMOSAICMETHOD_GRADIENT;
            mGrabSpec.nBlackLevel = 0;
        }
        public void AllocImageBuff()
        {
            // 기존 배열 및 객체 삭제 (null로 설정)
            //m_pFrameRawBuffer = null;
            //m_pFrameBMPBuffer = null;

            //m_pFrameRawBuffer = new byte[nFrameRawSize];
            //m_pFrameBMPBuffer = new byte[nBmpSize];

            //Array.Clear(m_pFrameRawBuffer, 0, nFrameRawSize);
            //Array.Clear(m_pFrameBMPBuffer, 0, nBmpSize);

            if (m_pFrameRawBuffer == null || m_pFrameRawBuffer.Length != nFrameRawSize)
            {
                Array.Resize(ref m_pFrameRawBuffer, nFrameRawSize);
            }
            if (m_pFrameBMPBuffer == null || m_pFrameBMPBuffer.Length != nBmpSize)
            {
                Array.Resize(ref m_pFrameBMPBuffer, nBmpSize);
            }

            foreach (var mat in m_pImageBuff)
            {
                mat?.Dispose();  // Mat 객체는 Dispose() 메서드를 호출하여 해제
            }
            foreach (var mat2 in m_pGrabBuff)
            {
                mat2?.Dispose();  // Mat 객체는 Dispose() 메서드를 호출하여 해제
            }
            imageItp?.Dispose();  // imageItp가 null이 아닌 경우에만 Dispose 호출

            imageItp = new Mat(m_nHeight, m_nWidth, MatType.CV_8UC3);

            m_pImageBuff[0] = new Mat(m_nHeight, m_nWidth, MatType.CV_8UC1);
            m_pImageBuff[1] = new Mat(m_nHeight, m_nWidth, MatType.CV_8UC1);
            m_pImageBuff[2] = new Mat(m_nHeight, m_nWidth, MatType.CV_8UC1);

            m_pGrabBuff[0] = new Mat(m_nHeight, m_nWidth, MatType.CV_8UC3);
            m_pGrabBuff[1] = new Mat(m_nHeight, m_nWidth, MatType.CV_8UC3);
            m_pGrabBuff[2] = new Mat(m_nHeight, m_nWidth, MatType.CV_8UC3);



            Globalo.threadControl.ccdGrabThread.RawInit();
        }
        public void BoardtInitialize()
        {

        }

        public static unsafe bool SensorIdRead_Head_Fn()        //ok 확인완료 250204_1
        {
            /*
                unsigned int errorCode = 0;
	            unsigned short SlaveAddr = 0x24;
	            unsigned short Addr = 0x0000;
	            unsigned short checkAddr = 0x3C06;
	            const int Cmd1Length = 13;
	            unsigned short AddrArr[Cmd1Length] = { 0x3000, 0x3001, 0x3002, 0x3003, 0x3004, 0x3005, 0x3006, 0x3007, 0x3008, 0x3009, 0x300a, 0x300b, 0x80bc };
	            unsigned char Data[Cmd1Length] = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x30, 0x17, 0x01, 0xF2, 0x01};

	            unsigned char pReadData[MAX_PATH];
	            memset(pReadData, 0x00, sizeof(pReadData));
	            int writeDelay = 20;
	            unsigned char test = 0x00;
             */
            int i = 0;
            int errorCode = 0;
            ushort SlaveAddr = 0x24;
            ushort checkAddr = 0x3C06;

            ushort[] AddrArr = { 0x3000, 0x3001, 0x3002, 0x3003, 0x3004, 0x3005, 0x3006, 0x3007, 0x3008, 0x3009, 0x300A, 0x300B, 0x80BC };
            byte[] Data = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x30, 0x17, 0x01, 0xF2, 0x01 };

            byte[] pReadData = new byte[260]; // MAX_PATH 대신 일반적인 크기(예: 260) 사용
            

            int writeDelay = 20;

            const int Cmd1Length = 13;
            for (i = 0; i < Cmd1Length; i++)
            {
                Thread.Sleep(writeDelay);
                fixed (byte* pData = Data)
                {
                    //IntPtr ptr = (IntPtr)pData; // 포인터를 IntPtr로 변환

                    errorCode = Globalo.GrabberDll.mWriteI2CBurst(SlaveAddr, AddrArr[i], 2, pData + i, 1);
                    if (errorCode != 0)
                    {
                        return false;
                    }
                }

            }

            Thread.Sleep(writeDelay);
            Array.Clear(pReadData, 0, pReadData.Length);
            fixed (byte* pData = pReadData)
            {
                errorCode = Globalo.GrabberDll.mReadI2CBurst(SlaveAddr, checkAddr, 2, pData, 1);
                if(pData[0] != 0x99)
                {
                    return false;
                }
            }
            //Command2 진행
            //
            const int Cmd2Length = 13;
            Data[0] = 0x00;
            Data[1] = 0x00;
            Data[2] = 0x00;
            Data[3] = 0x00;
            Data[4] = 0x00;
            Data[5] = 0x06;
            Data[6] = 0x00;
            Data[7] = 0x00;
            Data[8] = 0x3D;
            Data[9] = 0x81;
            Data[10] = 0x01;
            Data[11] = 0x01;
            Data[12] = 0x01;
            for (i = 0; i < Cmd2Length; i++)
            {
                Thread.Sleep(writeDelay);
                fixed (byte* pData = Data)
                {
                    errorCode = Globalo.GrabberDll.mWriteI2CBurst(SlaveAddr, AddrArr[i], 2, pData + i, 1);
                    if (errorCode != 0)
                    {
                        return false;
                    }
                }

            }
            //Command 2 End
            //
            Thread.Sleep(writeDelay);
            //
            Array.Clear(pReadData, 0, pReadData.Length);
            fixed (byte* pData = pReadData)
            {
                errorCode = Globalo.GrabberDll.mReadI2CBurst(SlaveAddr, checkAddr, 2, pData, 1);
                if (pData[0] != 0x99)
                {
                    return false;
                }
            }

            Thread.Sleep(15);

            //Command3 진행
            const int Cmd3Length = 12;
            //
            Data[0] = 0x00;
            Data[1] = 0x00;
            Data[2] = 0x00;
            Data[3] = 0x00;
            Data[4] = 0x00;
            Data[5] = 0x05;
            Data[6] = 0x01;
            Data[7] = 0x00;
            Data[8] = 0x70;
            Data[9] = 0x00;
            Data[10] = 0x10;
            Data[11] = 0x01;

            AddrArr[11] = 0x80bc;

            for (i = 0; i < Cmd3Length; i++)
            {
                Thread.Sleep(writeDelay);
                fixed (byte* pData = Data)
                {
                    errorCode = Globalo.GrabberDll.mWriteI2CBurst(SlaveAddr, AddrArr[i], 2, pData + i, 1);
                    if (errorCode != 0)
                    {
                        return false;
                    }
                }

            }

            //Command 3 End
            //
            Thread.Sleep(writeDelay);
            //
            Array.Clear(pReadData, 0, pReadData.Length);
            fixed (byte* pData = pReadData)
            {
                errorCode = Globalo.GrabberDll.mReadI2CBurst(SlaveAddr, checkAddr, 2, pData, 1);
                if (pData[0] != 0x99)
                {
                    return false;
                }
            }


            //Sensor ID Read (3C07 ~ 3C16)
            const int snReadLength = 16;
            byte[] pSensorData = new byte[260]; // MAX_PATH 대신 일반적인 크기(예: 260) 사용
            Array.Clear(pSensorData, 0, pSensorData.Length);
            string sensor = "";
            fixed (byte* pData = pSensorData)
            {
                errorCode = Globalo.GrabberDll.mReadI2CBurst(SlaveAddr, 0x3C07, 2, pData, snReadLength);
                if(errorCode != 0)
                {
                    return false;
                }

                StringBuilder tempData = new StringBuilder();
                for (i = 0; i < snReadLength; i++)
                {
                    tempData.AppendFormat("{0:X2}", pData[i]); // 2자리 16진수로 변환
                }

                sensor = tempData.ToString();
            }


            string logData = $"Sensor Read : {sensor}";
            Globalo.LogPrint("LaonControl", logData);
            return true;
        }
    }
}
