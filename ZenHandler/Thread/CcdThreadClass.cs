using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Matrox.MatroxImagingLibrary;
using System.Runtime.InteropServices;
using OpenCvSharp;
using System.Diagnostics;

namespace ActiveAligApp
{
    class CcdThreadClass : BaseThread
    {
        public bool mCcdThreadRun = false;
        public CcdThreadClass()
        : base()
        {
        }

        public override void RunThread()
        {
            // Do some stuff
            IsBackground = true;
            FuncGrabRun();
        }

        unsafe public void FuncGrabRun()
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
            try
            {
                mCcdThreadRun = true;
                while (mCcdThreadRun)
                {
                    if (oGlobal.mLaonGrabberClass.M_bOpen == false) continue;
                    //
                    if (oGlobal.mLaonGrabberClass.GrabberDll.mGetFrame((byte*)RawPtr.ToPointer(), (byte*)BmpPtr.ToPointer()) == true)
                    {

                        oGlobal.mLaonGrabberClass.imageItp = new Mat(mHeight, mWidth, MatType.CV_8UC3, oGlobal.mLaonGrabberClass.m_pFrameBMPBuffer);

                        //IntPtr matData = oGlobal.mLaonGrabberClass.imageItp.Data;
                        //Marshal.Copy(oGlobal.mLaonGrabberClass.m_pFrameBMPBuffer, 0, matData, (mWidth * mHeight * 3));


                        // Grab 버퍼에 저장
                        oGlobal.mLaonGrabberClass.m_pGrabBuff[oGlobal.vision.m_nGrabIndex[0]] = oGlobal.mLaonGrabberClass.imageItp.Clone();
                        oGlobal.vision.m_nCvtColorReadyIndex[0] = oGlobal.vision.m_nGrabIndex[0];
                        oGlobal.vision.m_nGrabIndex[0]++;
                        if (oGlobal.vision.m_nGrabIndex[0] >= 3)
                        {
                            oGlobal.vision.m_nGrabIndex[0] = 0;
                        }


                        //Cv2.Split(oGlobal.mLaonGrabberClass.m_pGrabBuff[oGlobal.vision.m_nCvtColorReadyIndex[0]], out oGlobal.mLaonGrabberClass.m_pImageBuff);
                        //cvCopy(g_clLaonGrabberWrapper[m_nUnit].m_stMIUDevice.imageItp, g_clLaonGrabberWrapper[m_nUnit].m_pGrabBuff[g_clVision.m_nGrabIndex[m_nUnit]]);

                        //mCamSc.mCCdWindow.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(oGlobal.mLaonGrabberClass.imageItp);

                        //MIL.MbufPut(oGlobal.vision.m_MilCcdProcChild[0, 0], oGlobal.mLaonGrabberClass.m_pImageBuff[2].ToBytes(".jpg"));
                        //MIL.MbufPut(oGlobal.vision.m_MilCcdProcChild[0, 1], oGlobal.mLaonGrabberClass.m_pImageBuff[1].ToBytes(".jpg"));
                        //MIL.MbufPut(oGlobal.vision.m_MilCcdProcChild[0, 2], oGlobal.mLaonGrabberClass.m_pImageBuff[0].ToBytes(".jpg"));

                        //MIL.MimResize(oGlobal.vision.m_MilCcdProcImage[0], oGlobal.vision.m_MilSmallImage[0], dZoomX, dZoomY, MIL.M_DEFAULT);

                        //oGlobal.mLaonGrabberClass.m_pImageBuff[2].SaveImage("d:\\m_pImageBuff2.jpg");
                        //oGlobal.mLaonGrabberClass.m_pImageBuff[1].SaveImage("d:\\m_pImageBuff1.jpg");
                        //oGlobal.mLaonGrabberClass.m_pImageBuff[0].SaveImage("d:\\m_pImageBuff0.jpg");
                        //imageItp = OpenCvSharp.Extensions.WriteableBitmapConverter();
                        //imageItp->imageData = (char*)m_pFrameBMPBuffer;
                        //Array.Copy(imageItp.Data, m_pFrameBMPBuffer, 555);
                        //imageItp.Data = m_pFrameBMPBuffer;
                        //imageItp = Mat.FromImageData(m_pFrameBMPBuffer, OpenCvSharp.LoadMode.Color);
                        //Mat mMat = new Mat(mHeight, mWidth, MatType.CV_8UC3, oGlobal.mLaonGrabberClass.m_pFrameBMPBuffer);
                        //
                        // mMat.SaveImage("d:\\matimage.jpg");
                        // imageItp.SaveImage("d:\\mtestMat.jpg");
                        continue;
                    }
                    Thread.Sleep(5);
                }
            }
            catch (ThreadInterruptedException err)
            {
                Debug.WriteLine(err);
            }
            finally
            {
                Debug.WriteLine("time 리소스 지우기");
            }
        }
    }
}
