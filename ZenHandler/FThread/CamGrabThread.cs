using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Matrox.MatroxImagingLibrary;
using System.Threading;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace ZenHandler.FThread
{
    public class CamGrabThread : BaseThread
    {
        public CamGrabThread()
        {
            this.name = "CamGrabThread";
        }
        protected override void ThreadInit()
        {

        }
        protected override void ThreadRun()
        {
            //if (g_clVision.m_nGrabMode[0] == GRAB_LIVE)

            try
            {
                MIL.MdigGrab(Globalo.vision.MilDigitizer, Globalo.vision.MilCamGrabImage[0]);
                MIL.MdigGrabWait(Globalo.vision.MilDigitizer, MIL.M_GRAB_END);// M_GRAB_END); M_GRAB_FRAME_END
                MIL.MimResize(Globalo.vision.MilCamGrabImageChild[0], Globalo.vision.MilCamSmallImageChild[0], Globalo.vision.M_CamReduceFactorX, Globalo.vision.M_CamReduceFactorY, MIL.M_DEFAULT);
                Thread.Sleep(10);

            }
            catch (ThreadInterruptedException err)
            {
                Debug.WriteLine(err);
            }
        }
    }



}
