using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZenHandler.FThread
{
    public class DIoThread : BaseThread
    {
        public DIoThread()
        {
            this.name = "DIoThread";
        }
        //protected override void ProcessRun()
        protected override void ThreadInit()
        {

        }
        protected override void ThreadRun()
        {
            //Globalo.dIoControl.ReadDWordIn(0);

            //BASE_THREAD_INTERVAL
            //while (!cts.Token.IsCancellationRequested)
            //{
            //    Globalo.dIoControl.ReadDWordIn(0);
            //    Thread.Sleep(10);
            //}
        }
    }
}
