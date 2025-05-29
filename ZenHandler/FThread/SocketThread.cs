using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.FThread
{
    public class SocketThread : BaseThread
    {
        public int m_nCurrentStep = 0;
        public int m_nStartStep = 0;
        public int m_nEndStep = 0;

        public int[] m_nSocketStep = { 0, 0, 0, 0 };
        public int Index;
        public SocketThread(int index = 0)
        {
            this.Index = index;
        }
        protected override void ThreadInit()
        {
            Console.WriteLine($"Socket {this.Index} ThreadInit");
        }

        protected override void ThreadDestructor()
        {
            Console.WriteLine($"Socket {this.Index} ThreadDestructor");
        }
        protected override void ThreadRun()
        {
            if (this.m_nCurrentStep >= this.m_nStartStep && this.m_nCurrentStep < this.m_nEndStep)
            {

            }
            else if (this.m_nCurrentStep < 0)
            {

            }
            else
            {
                //stop
                m_nStartStep = 0;
                m_nEndStep = 0;
                this.Stop();

                Console.WriteLine($"ocket {this.Index} Process Stop");
            }
        }
    }
}
