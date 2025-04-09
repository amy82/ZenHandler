using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;       //<- Marshal
using System.Threading;
using OpenCvSharp;
using Matrox.MatroxImagingLibrary;
using System.Diagnostics;

namespace ZenHandler.FThread
{
    
    public class CcdColorThread : BaseThread
    {
        //public event delLogSender eLogSender;       //외부에서 호출할때 사용
        //public bool mCcdColorThreadRun = false;
        //private Thread thread = null;
        //private bool m_bPause = false;
        //private CancellationTokenSource cts;
        private byte[] bytes2;

        private double dZoomX;
        private double dZoomY;
        public CcdColorThread()
        {
            this.name = "CcdColorThread";
        }
        protected override void ThreadInit()
        {
            //IntPtr RawPtr = Marshal.UnsafeAddrOfPinnedArrayElement(Globalo.mLaonGrabberClass.m_pFrameRawBuffer, 0);
            //IntPtr BmpPtr = Marshal.UnsafeAddrOfPinnedArrayElement(Globalo.mLaonGrabberClass.m_pFrameBMPBuffer, 0);

            int mWidth = Globalo.GrabberDll.mGetWidth();
            int mHeight = Globalo.GrabberDll.mGetHeight();


            dZoomX = ((double)Globalo.camControl.CcdPanel.Width / (double)mWidth);
            dZoomY = ((double)Globalo.camControl.CcdPanel.Height / (double)mHeight);
            bytes2 = new byte[mWidth * mHeight];
        }
        //protected override void ProcessRun()
        protected override void ThreadRun()
        {
            if (Globalo.mLaonGrabberClass.M_GrabDllLoadComplete == false)
            {
                return;
            }





            //Mat imageItp = new Mat(mHeight, mWidth, MatType.CV_8UC3);//MatType.CV_8UC3);



            //mCcdColorThreadRun = true;
            //while (mCcdColorThreadRun)
            //while (!cts.Token.IsCancellationRequested)
            //{
            if (Globalo.mLaonGrabberClass.M_bOpen == false) return ;

            if (Globalo.vision.m_nGrabIndex[0] == Globalo.vision.m_nCvtColorReadyIndex[0])
            {
                Thread.Sleep(10);
                return;
            }

            if (Globalo.vision.m_nCvtColorReadyIndex[0] < 0 || Globalo.vision.m_nCvtColorReadyIndex[0] >= 3)
            {
                Thread.Sleep(10);
                return;
            }


            //0 = B , 1 = G , 2 = R
            //Cv2.Split 이걸쓰면 메모리 상승
            //Cv2.Split(oGlobal.mLaonGrabberClass.m_pGrabBuff[oGlobal.vision.m_nCvtColorReadyIndex[0]], out oGlobal.mLaonGrabberClass.m_pImageBuff);
            Cv2.ExtractChannel(Globalo.mLaonGrabberClass.m_pGrabBuff[Globalo.vision.m_nCvtColorReadyIndex[0]], Globalo.mLaonGrabberClass.m_pImageBuff[0], 0);
            Cv2.ExtractChannel(Globalo.mLaonGrabberClass.m_pGrabBuff[Globalo.vision.m_nCvtColorReadyIndex[0]], Globalo.mLaonGrabberClass.m_pImageBuff[1], 1);
            Cv2.ExtractChannel(Globalo.mLaonGrabberClass.m_pGrabBuff[Globalo.vision.m_nCvtColorReadyIndex[0]], Globalo.mLaonGrabberClass.m_pImageBuff[2], 2);

            //Cv2.Split(m_pGrabBuff[oGlobal.vision.m_nCvtColorReadyIndex[0]], out m_pImageBuff);
            //byte* data = (byte*)oGlobal.mLaonGrabberClass.m_pImageBuff[2].DataPointer;


            //Cv2.ImEncode(".bmp", oGlobal.mLaonGrabberClass.m_pImageBuff[2], out bytes2);
            //oGlobal.mLaonGrabberClass.m_pImageBuff[2].GetArray(0,0, bytes2);
            //IntPtr ptr = oGlobal.mLaonGrabberClass.m_pImageBuff[2];
            // int size = Marshal.SizeOf(ptr);

            Marshal.Copy(Globalo.mLaonGrabberClass.m_pImageBuff[2].Data, bytes2, 0, bytes2.Length); // Mat 데이터를 바이트 배열로 복사
            MIL.MbufPut(Globalo.vision.m_MilCcdProcChild[0, 0], bytes2);
            Marshal.Copy(Globalo.mLaonGrabberClass.m_pImageBuff[1].Data, bytes2, 0, bytes2.Length); // Mat 데이터를 바이트 배열로 복사
            MIL.MbufPut(Globalo.vision.m_MilCcdProcChild[0, 1], bytes2);
            Marshal.Copy(Globalo.mLaonGrabberClass.m_pImageBuff[0].Data, bytes2, 0, bytes2.Length); // Mat 데이터를 바이트 배열로 복사
            MIL.MbufPut(Globalo.vision.m_MilCcdProcChild[0, 2], bytes2);
            
            
            MIL.MimResize(Globalo.vision.m_MilCcdProcImage[0], Globalo.vision.m_MilSmallImage[0], dZoomX, dZoomY, MIL.M_DEFAULT);
        }
       
    }

    
}
