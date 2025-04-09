using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ActiveAligApp
{

    class TimeThreadClass : BaseThread
    {
        public bool mTimeThreadRun = false;
        delegate void TimeTextCallback(string contents);
        public TimeThreadClass()
        : base()
        {
        }

        public override void RunThread()
        {
            IsBackground = true;    //부모 종료시 스레드 종료
            FuncTimeViewRun();
        }
        public void FuncTimeViewRun()
        {
            try
            {
                DateTime dTime;
                string sTime = "";
                mTimeThreadRun = true;
                while (mTimeThreadRun)
                {
                    dTime = DateTime.Now;
                    //sTime = $"{dTime:hh:mm:ss.fff}";
                    sTime = $"{dTime:hh : mm : ss}";
                    if (oGlobal.MainForm.mTimeLabel.InvokeRequired)
                    {
                        oGlobal.MainForm.mTimeLabel.Invoke(new TimeTextCallback(setTimeLabel), sTime);        //<--사용가능 #1
                        //TimeLabel.Invoke(new MethodInvoker(delegate { TimeLabel.Text = sTime; }));//<--사용가능 #2

                    }

                    Thread.Sleep(100);
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
        public void setTimeLabel(string sTime)
        {

            oGlobal.MainForm.mTimeLabel.Text = sTime;
        }


    }
}
