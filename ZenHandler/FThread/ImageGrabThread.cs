using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;


namespace ZenHandler.FThread
{
    public class ImageGrabThread : BaseThread
    {
        private IntPtr RawPtr;
        private IntPtr BmpPtr;

        private int mWidth;
        private int mHeight;

        private Mat resizedMat;

        public ImageGrabThread()
        {
            this.name = "CcdGrabThread";

        }
        public void RawInit()
        {
            mWidth = Globalo.GrabberDll.mGetWidth();
            mHeight = Globalo.GrabberDll.mGetHeight();


            resizedMat?.Dispose();
            resizedMat = new Mat(mHeight, mWidth, MatType.CV_8UC3);       //MatType.CV_8UC3);
        }

        protected override void ThreadInit()
        {
            RawPtr = Marshal.UnsafeAddrOfPinnedArrayElement(Globalo.mLaonGrabberClass.m_pFrameRawBuffer, 0);
            BmpPtr = Marshal.UnsafeAddrOfPinnedArrayElement(Globalo.mLaonGrabberClass.m_pFrameBMPBuffer, 0);

            
        }
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
                    Cv2.Resize(Globalo.mLaonGrabberClass.imageItp, resizedMat, new OpenCvSharp.Size((int)(mWidth * Globalo.vision.M_CcdReduceFactorX), (int)(mHeight * Globalo.vision.M_CcdReduceFactorY)));
                    Globalo.camControl.Invoke((MethodInvoker)delegate
                    {

                        Globalo.camControl.pictureBox1.Image?.Dispose();  // 이전 이미지 Dispose
                        Globalo.camControl.pictureBox1.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(resizedMat);// Globalo.mLaonGrabberClass.imageItp);
                    });
                    // byte[] 데이터를 Bitmap으로 변환
                    //using (MemoryStream stream = new MemoryStream(Globalo.mLaonGrabberClass.m_pFrameBMPBuffer))
                    //{
                    //    //Bitmap bitmap = new Bitmap(stream);
                    //    using (Bitmap testBitmap = new Bitmap(stream))
                    //    {
                    //        // UI 스레드에서 PictureBox 갱신
                    //        Globalo.camControl.Invoke((MethodInvoker)delegate
                    //        {

                    //            Globalo.camControl.pictureBox.Image?.Dispose();  // 이전 이미지 Dispose
                    //            Globalo.camControl.pictureBox.Image = (Bitmap)testBitmap.Clone();
                    //        });
                    //    }

                    //}
                }

            }
            catch (ThreadInterruptedException err)
            {
                Debug.WriteLine(err);
            }
        }
    }
}
