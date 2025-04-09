using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZenHandler.FThread
{
    public class TimeThread : BaseThread
    {
        private Label timeLabel;
        private Label dateLabel;
        //private Thread thread;
        public bool threadTimeRun = false;
        private DateTime dTime;
        private int m_dTimeTickCount;
        public TimeThread()
        {
            this.name = "TimeThread";
            
            m_dTimeTickCount = 0;
        }

        protected override void ThreadInit()
        {
            this.timeLabel = Globalo.tabMenuForm.TimeLabel;
            this.dateLabel = Globalo.tabMenuForm.DateLabel;
        }
        protected override void ThreadRun()
        {
            
            try
            {
                if ((Environment.TickCount - m_dTimeTickCount) > 500)
                {
                    if (this.timeLabel != null && this.dateLabel != null)
                    {
                        dTime = DateTime.Now;
                        string sTime = $"{dTime:HH : mm : ss}";

                        if (this.timeLabel.InvokeRequired)
                        {
                            this.timeLabel.Invoke(new MethodInvoker(delegate { this.timeLabel.Text = sTime; }));

                        }
                        else
                        {
                            this.timeLabel.Text = sTime;
                        }
                        string sData = $"{dTime:yy / MM / dd}";
                        if (this.dateLabel.InvokeRequired)
                        {
                            this.dateLabel.Invoke(new MethodInvoker(delegate { this.dateLabel.Text = sData; }));

                        }
                        else
                        {
                            this.dateLabel.Text = sData;
                        }
                    }

                    m_dTimeTickCount = Environment.TickCount;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"ThreadRun 처리 중 예외 발생: {ex.Message}");
            }
        }

    }
}
