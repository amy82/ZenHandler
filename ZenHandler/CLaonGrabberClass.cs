using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;
namespace ActiveAligApp
{
    public class CLaonGrabberClass
    {
        public event delLogSender eLogSender;       //외부에서 호출할때 사용

        private int m_nUnit = 0;
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
        public LgitLibrary.LgitLibrary GrabberDll = new LgitLibrary.LgitLibrary();
        public _TDATASPEC mGrabSpec;


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
            int mWidth = 100;
            int mHeight = 100;
            int mRoiCount = 5;
            int mLength = mydouble.Length;
            unsafe
            {
                //GrabberDll.g_GetSFR((byte*)RawPtr.ToPointer(), mWidth, mHeight, mRoiCount, (double*)doublePtr, mLength);
            }
            
        }

        public void SetUnit(int nUnit)
        {
            m_nUnit = nUnit;
        }
        public bool CloseDevice()
        {
            M_bOpen = false;
            if (GrabberDll.mIsGrabStarted() == true)
            {
                //stopGrab
                GrabberDll.mGrabStop();
                GrabberDll.mCloseBoard();
                //closeDevice
            }
            m_nCurrentCcdState = (int)CCD_GRAB_MODE.CCD_GRAB_STOP;
            UiconfigLoad();
            return true;
        }
        public bool StopGrab()
        {
            if (GrabberDll.mIsGrabStarted() == true)
            {
                //stopGrab
                GrabberDll.mGrabStop();
            }
            m_nCurrentCcdState = (int)CCD_GRAB_MODE.CCD_GRAB_STOP;
            return true;
        }
        public bool StartGrab()
        {
            m_nCurrentCcdState = (int)CCD_GRAB_MODE.CCD_GRAB_STOP;
            if (GrabberDll.mGrabStart() == 0)
            {
                //ok
            }
            else
            {
                //fail
                return false;
            }

            m_nCurrentCcdState = (int)CCD_GRAB_MODE.CCD_GRAB_LIVE;
            return true;
        }
        public bool OpenDevice()
        {
            if (M_bOpen)
            {
                return true;
            }
            m_nCurrentCcdState = (int)CCD_GRAB_MODE.CCD_GRAB_STOP;
            if (GrabberDll.mIsGrabStarted() == true)
            {
                //stopGrab
                GrabberDll.mGrabStop();
                GrabberDll.mCloseBoard();
                //closeDevice
            }
            int nErrorCode = -1;
            nErrorCode = GrabberDll.mOpenBoard((sbyte)m_cBoardIndex);
            if(nErrorCode > 0)
            {
                //error
                eLogSender("GrabberDll", enLogLevel.Info, $"[CCD]Laon Err:" + nErrorCode.ToString());
                return false;
            }
            M_bOpen = true;
            eLogSender("GrabberDll", enLogLevel.Info, $"[CCD]Laon Open Ok");
            return true;
        }
        public void UiconfigLoad()
        {
            string mFilePath = CPath.MIU_DIR + "\\UIConfig\\UIConfig.ini"; // "Initialize\\M3_TESLA_DES964_M3.ini";
            string mGetFilename = "";
            string miniFilePath = "";
            StringBuilder sb = new StringBuilder(255);
            GetPrivateProfileString("UIConfig", "GrabberINI", "Default.ini", sb, oGlobal.MAX_PATH, mFilePath);
            mGetFilename = sb.ToString();
            miniFilePath = CPath.MIU_DIR + "\\" + mGetFilename;
            unsafe
            {
                byte[] bytes = Encoding.ASCII.GetBytes(miniFilePath);

                fixed (byte* p = bytes)
                {
                    sbyte* sp = (sbyte*)p;
                    GrabberDll.mSetINIFile(sp);
                   
                    //SP is now what you want
                }

            }
        }
        public void SelectSensor()
        {
            m_nWidth = GrabberDll.mGetWidth();
            m_nHeight = GrabberDll.mGetHeight();

            //m_stMIUDevice.iPixelFormat = m_pBoard->GetDataFormat();

            nFrameRawSize = GrabberDll.mGetFrameRawSize(); 
            nBmpSize = GrabberDll.mGetFrameBMPSize();

            eLogSender("GrabberDll", enLogLevel.Info, $"Width:" + m_nWidth.ToString());
            eLogSender("GrabberDll", enLogLevel.Info, $"Height:" + m_nHeight.ToString());
            mGrabSpec.eOutMode = (EOUTMODE)GrabberDll.mGetOutMode();
            mGrabSpec.eDataFormat = (EDATAFORMAT)GrabberDll.mGetDataFormat();
            mGrabSpec.eSensorType = (ESENSORTYPE)GrabberDll.mGetSensorType();


        }
        public void AllocImageBuff()
        {
            m_pFrameRawBuffer = new byte[nFrameRawSize];
            m_pFrameBMPBuffer = new byte[nBmpSize];

            Array.Clear(m_pFrameRawBuffer, 0, nFrameRawSize);
            Array.Clear(m_pFrameBMPBuffer, 0, nBmpSize);

            imageItp = new Mat(m_nHeight, m_nWidth, MatType.CV_8UC3);

            m_pImageBuff[0] = new Mat(m_nHeight, m_nWidth, MatType.CV_8UC1);
            m_pImageBuff[1] = new Mat(m_nHeight, m_nWidth, MatType.CV_8UC1);
            m_pImageBuff[2] = new Mat(m_nHeight, m_nWidth, MatType.CV_8UC1);

            m_pGrabBuff[0] = new Mat(m_nHeight, m_nWidth, MatType.CV_8UC3);
            m_pGrabBuff[1] = new Mat(m_nHeight, m_nWidth, MatType.CV_8UC3);
            m_pGrabBuff[2] = new Mat(m_nHeight, m_nWidth, MatType.CV_8UC3);

            
            //new Mat(mHeight, mWidth, MatType.CV_8UC3, oGlobal.mLaonGrabberClass.m_pFrameBMPBuffer);
        }
        public void BoardtInitialize()
        {

        }
    }
}
