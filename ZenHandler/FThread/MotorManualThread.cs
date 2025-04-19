using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZenHandler.FThread
{
    public class MotorManualThread : BaseThread
    {
        private MotionControl.MotorController parent;

        public int m_nCurrentStep = 0;
        public int m_nStartStep = 0;
        public int m_nEndStep = 0;

        public MotorManualThread(MotionControl.MotorController _parent)
        {
            this.parent = _parent;
        }
        private void TransferFlow()
        {
            if (this.m_nCurrentStep >= 1000 && this.m_nCurrentStep < 2000)
            {
            }

        }

        private void LiftFlow()
        {
            if (this.m_nCurrentStep >= 1000 && this.m_nCurrentStep < 2000)
            {
            }
        }
        private void MagazineFlow()
        {
            if (this.m_nCurrentStep >= 1000 && this.m_nCurrentStep < 2000)
            {
            }
        }



        protected override void ThreadRun()
        {
            if (this.m_nCurrentStep >= this.m_nStartStep && this.m_nCurrentStep < this.m_nEndStep)
            {
                if (this.parent.MachineName == "TransferMachine")
                {
                    TransferFlow();
                }

                if (this.parent.MachineName == "LiftModule")
                {
                    //bool rtn = Globalo.motionManager.transferMachine.IsMoving();
                    LiftFlow();
                }
                this.m_nCurrentStep = 0;
            }
            else if(this.m_nCurrentStep < 0)
            {
                //Pause
                this.Pause();
            }
            else
            {
                //stop
                m_nStartStep = 0;
                m_nEndStep = 0;
                this.Stop();
            }
        }
        protected override void ThreadInit()
        {
            Console.WriteLine($"{this.parent.MachineName} ThreadInit");
        }

        protected override void ThreadDestructor()
        {
            Console.WriteLine($"{this.parent.MachineName} ThreadDestructor");
        }
    }
}
