using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using OpenCvSharp;
using System.Diagnostics;

namespace ZenHandler.FThread
{
    public class CcdGrabThread : BaseThread
    {
        //public event delLogSender eLogSender;       //외부에서 호출할때 사용

        private IntPtr RawPtr;
        private IntPtr BmpPtr;

        private int mWidth;
        private int mHeight;

        private Mat imageItp;
        public CcdGrabThread()
        {
            this.name = "CcdGrabThread";
        }
        public void RawInit()
        {
            RawPtr = Marshal.UnsafeAddrOfPinnedArrayElement(Globalo.mLaonGrabberClass.m_pFrameRawBuffer, 0);
            BmpPtr = Marshal.UnsafeAddrOfPinnedArrayElement(Globalo.mLaonGrabberClass.m_pFrameBMPBuffer, 0);
        }
        protected override void ThreadInit()
        {
            RawInit();
            mWidth = Globalo.GrabberDll.mGetWidth();
            mHeight = Globalo.GrabberDll.mGetHeight();


            imageItp?.Dispose();
            imageItp = new Mat(mHeight, mWidth, MatType.CV_8UC3);       //MatType.CV_8UC3);

            //double dZoomX = 0.0;
            //double dZoomY = 0.0;
            //dZoomX = ((double)Globalo.camControl.CcdPanel.Width / (double)mWidth);
            //dZoomY = ((double)Globalo.camControl.CcdPanel.Height / (double)mHeight);
        }
        //protected override unsafe void ProcessRun()
        protected override unsafe void ThreadRun()
        {
            if (Globalo.mLaonGrabberClass.M_GrabDllLoadComplete == false)
            {
                return;
            }
            try
            {
                if (Globalo.mLaonGrabberClass.M_bOpen == false)
                {
                    return;
                }

                if (Globalo.GrabberDll.mGetFrame((byte*)RawPtr.ToPointer(), (byte*)BmpPtr.ToPointer()) == true)
                {

                    IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(Globalo.mLaonGrabberClass.m_pFrameBMPBuffer, 0);

                    // 데이터를 Mat에 복사
                    Marshal.Copy(Globalo.mLaonGrabberClass.m_pFrameBMPBuffer, 0, Globalo.mLaonGrabberClass.imageItp.Data, mHeight * mWidth * 3);
                    // Grab 버퍼에 저장

                    // imageItp의 데이터를 기존 m_pGrabBuff에 복사 (Clone 대신)
                    Globalo.mLaonGrabberClass.imageItp.CopyTo(Globalo.mLaonGrabberClass.m_pGrabBuff[Globalo.vision.m_nGrabIndex[0]]);

                    Globalo.vision.m_nCvtColorReadyIndex[0] = Globalo.vision.m_nGrabIndex[0];
                    Globalo.vision.m_nGrabIndex[0]++;
                    if (Globalo.vision.m_nGrabIndex[0] >= 3)
                    {
                        Globalo.vision.m_nGrabIndex[0] = 0;
                    }
                }
                
            }
            catch (ThreadInterruptedException err)
            {
                Debug.WriteLine(err);
            }

        }

    }
}
