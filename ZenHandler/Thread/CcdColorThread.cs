using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Matrox.MatroxImagingLibrary;
using OpenCvSharp;
using System.Threading;
using System.Diagnostics;



namespace ActiveAligApp
{
    class CcdColorThread : BaseThread
    {
        public bool mCcdColorThreadRun = false;
        public CcdColorThread()
        : base()
        {
        }
        public override void RunThread()
        {
            // Do some stuff
            IsBackground = true;
            FuncGrabColorRun();
        }
        public void FuncGrabColorRun()
        {
            if (oGlobal.mLaonGrabberClass.M_GrabDllLoadComplete == false)
            {
                return;
            }

            IntPtr RawPtr = Marshal.UnsafeAddrOfPinnedArrayElement(oGlobal.mLaonGrabberClass.m_pFrameRawBuffer, 0);
            IntPtr BmpPtr = Marshal.UnsafeAddrOfPinnedArrayElement(oGlobal.mLaonGrabberClass.m_pFrameBMPBuffer, 0);



            int mWidth = oGlobal.mLaonGrabberClass.GrabberDll.mGetWidth();
            int mHeight = oGlobal.mLaonGrabberClass.GrabberDll.mGetHeight();
            //Mat imageItp = new Mat(mHeight, mWidth, MatType.CV_8UC3);//MatType.CV_8UC3);

            double dZoomX = 0.0;
            double dZoomY = 0.0;
            dZoomX = ((double)oGlobal.MainForm.mCamSc.mCCdWindow.Width / (double)mWidth);
            dZoomY = ((double)oGlobal.MainForm.mCamSc.mCCdWindow.Height / (double)mHeight);
            byte[] bytes2 = new byte[mWidth * mHeight];
            try
            {
                mCcdColorThreadRun = true;
                while (mCcdColorThreadRun)
                {
                    if (oGlobal.mLaonGrabberClass.M_bOpen == false) continue;

                    if (oGlobal.vision.m_nGrabIndex[0] == oGlobal.vision.m_nCvtColorReadyIndex[0])
                    {
                        Thread.Sleep(50);
                        continue;
                    }

                    if (oGlobal.vision.m_nCvtColorReadyIndex[0] < 0 || oGlobal.vision.m_nCvtColorReadyIndex[0] >= 3)
                    {
                        Thread.Sleep(50);
                        continue;
                    }

                    //0 = B , 1 = G , 2 = R
                    //Cv2.Split 이걸쓰면 메모리 상승
                    //Cv2.Split(oGlobal.mLaonGrabberClass.m_pGrabBuff[oGlobal.vision.m_nCvtColorReadyIndex[0]], out oGlobal.mLaonGrabberClass.m_pImageBuff);
                    Cv2.ExtractChannel(oGlobal.mLaonGrabberClass.m_pGrabBuff[oGlobal.vision.m_nCvtColorReadyIndex[0]], oGlobal.mLaonGrabberClass.m_pImageBuff[0], 0);
                    Cv2.ExtractChannel(oGlobal.mLaonGrabberClass.m_pGrabBuff[oGlobal.vision.m_nCvtColorReadyIndex[0]], oGlobal.mLaonGrabberClass.m_pImageBuff[1], 1);
                    Cv2.ExtractChannel(oGlobal.mLaonGrabberClass.m_pGrabBuff[oGlobal.vision.m_nCvtColorReadyIndex[0]], oGlobal.mLaonGrabberClass.m_pImageBuff[2], 2);

                    //Cv2.Split(m_pGrabBuff[oGlobal.vision.m_nCvtColorReadyIndex[0]], out m_pImageBuff);
                    //byte* data = (byte*)oGlobal.mLaonGrabberClass.m_pImageBuff[2].DataPointer;


                    //Cv2.ImEncode(".bmp", oGlobal.mLaonGrabberClass.m_pImageBuff[2], out bytes2);
                    //oGlobal.mLaonGrabberClass.m_pImageBuff[2].GetArray(0,0, bytes2);
                    //IntPtr ptr = oGlobal.mLaonGrabberClass.m_pImageBuff[2];
                    // int size = Marshal.SizeOf(ptr);


                    Marshal.Copy(oGlobal.mLaonGrabberClass.m_pImageBuff[2].Data, bytes2, 0, bytes2.Length); // Mat 데이터를 바이트 배열로 복사
                    MIL.MbufPut(oGlobal.vision.m_MilCcdProcChild[0, 0], bytes2);
                    Marshal.Copy(oGlobal.mLaonGrabberClass.m_pImageBuff[1].Data, bytes2, 0, bytes2.Length); // Mat 데이터를 바이트 배열로 복사
                    MIL.MbufPut(oGlobal.vision.m_MilCcdProcChild[0, 1], bytes2);
                    Marshal.Copy(oGlobal.mLaonGrabberClass.m_pImageBuff[0].Data, bytes2, 0, bytes2.Length); // Mat 데이터를 바이트 배열로 복사
                    MIL.MbufPut(oGlobal.vision.m_MilCcdProcChild[0, 2], bytes2);


                    MIL.MimResize(oGlobal.vision.m_MilCcdProcImage[0], oGlobal.vision.m_MilSmallImage[0], dZoomX, dZoomY, MIL.M_DEFAULT);


                    //oGlobal.mLaonGrabberClass.m_pImageBuff[2].SaveImage("d:\\m_pImageBuff2.jpg");
                    //oGlobal.mLaonGrabberClass.m_pImageBuff[1].SaveImage("d:\\m_pImageBuff1.jpg");
                    //oGlobal.mLaonGrabberClass.m_pImageBuff[0].SaveImage("d:\\m_pImageBuff0.jpg");

                    //MIL.MbufExport("d:\\Mil1.jpg", MIL.M_BMP, oGlobal.vision.m_MilCcdProcChild[0, 1]);
                }
            }
            catch (ThreadInterruptedException err)
            {
                Debug.WriteLine(err);
            }
            finally
            {
                //Debug.WriteLine("리소스 지우기");
            }
        }
    }
}
