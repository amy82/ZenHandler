using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler
{
    public class ThreadControl
    {
        public FThread.LogThread logThread;
        public FThread.TimeThread timeThread;
        
        public FThread.AutoRunthread autoRunthread;

        //CCD thread
        public FThread.CcdColorThread ccdColorThread;
        public FThread.CamGrabThread camGrabThread;
        public FThread.CcdGrabThread ccdGrabThread;

        //test
        public FThread.ImageGrabThread imageGrabThread;


        //ON_LINE_MOTOR
        public FThread.DIoThread dIoThread;

        public FThread.ManualThread manualThread;

        public ThreadControl()
        {
            logThread = new FThread.LogThread();
            timeThread = new FThread.TimeThread();
            autoRunthread = new FThread.AutoRunthread();

            // 이벤트 핸들러 등록
            //autoRunthread.ThreadCompleted += (bool result) =>
            //{
            //    Console.WriteLine($"[Event] Thread 종료됨, 결과: {result}");
            //    //if(Globalo.MainForm != null)
            //    //{
            //    //    Globalo.MainForm.StopAutoProcess();
            //    //}
            //};

            ccdColorThread = new FThread.CcdColorThread();
            ccdGrabThread = new FThread.CcdGrabThread();
            if (ProgramState.ON_LINE_OPENCV_IMAGE)
            {
                imageGrabThread = new FThread.ImageGrabThread();
            }
                
            if (ProgramState.ON_LINE_MIL)
            {
                if (ProgramState.ON_LINE_CAM)
                {
                    camGrabThread = new FThread.CamGrabThread();
                }
            }

            manualThread = new FThread.ManualThread();
        }
        public void AllThreadStart()
        {
            logThread.Start();
            timeThread.Start();
            if (ProgramState.ON_LINE_MOTOR)
            {
                //dIoThread.Start();
            }
            
        }
        public void AllClose()
        {
            logThread.Close();
            timeThread.Close();
            autoRunthread.Close();

            if (ProgramState.ON_LINE_OPENCV_IMAGE)
            {
                imageGrabThread.Close();
            }
            if (ProgramState.ON_LINE_MIL)
            {
                ccdColorThread.Close();
                ccdGrabThread.Close();
            }
            if (ProgramState.ON_LINE_CAM)
            {
                camGrabThread.Close();
            }
            if (ProgramState.ON_LINE_MOTOR)
            {
                //dIoThread.Close();
            }
            manualThread.Close();
        }
    }
}
