using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZenHandler.FThread
{
    public class MotorAutoThread : BaseThread
    {
        private MotionControl.MotorController parent;
        public int m_nCurrentStep = 0;
        public int m_nStartStep = 0;
        public int m_nEndStep = 0;

        public MotorAutoThread(MotionControl.MotorController _parent)
        {
            this.parent = _parent;
            this.name = "MotorAutoThread";
        }


        private void TransferFlow()
        {
            if (this.m_nCurrentStep >= 1000 && this.m_nCurrentStep < 2000)
            {
                this.m_nCurrentStep = this.parent.processManager.transferFlow.HomeProcess(this.m_nCurrentStep);
            }
            else if (this.m_nCurrentStep >= 2000 && this.m_nCurrentStep < 3000)
            {
                this.m_nCurrentStep = this.parent.processManager.transferFlow.AutoReady(this.m_nCurrentStep);
            }
            else if (this.m_nCurrentStep >= 3000 && this.m_nCurrentStep < 4000)
            {
                this.m_nCurrentStep = this.parent.processManager.transferFlow.Auto_PCBLoading(this.m_nCurrentStep);
            }
        }

        private void LiftFlow()
        {
            if (this.m_nCurrentStep >= 1000 && this.m_nCurrentStep < 2000)
            {
                this.m_nCurrentStep = this.parent.processManager.liftFlow.HomeProcess(this.m_nCurrentStep);
            }
        }
        private void MagazineFlow()
        {
            if (this.m_nCurrentStep >= 1000 && this.m_nCurrentStep < 2000)
            {
                this.m_nCurrentStep = this.parent.processManager.liftFlow.HomeProcess(this.m_nCurrentStep);
            }
        }



        protected override void ThreadRun()
        {
            if (this.m_nCurrentStep >= this.m_nStartStep && this.m_nCurrentStep < this.m_nEndStep)
            {
                if (this.parent.MachineName == "TransferMachine")       //TODO: 여기도 개선 필요 자기자신
                {

                    TransferFlow();
                }

                if (this.parent.MachineName == "LiftModule")
                {
                    bool rtn = Globalo.motionManager.transferMachine.IsMoving();

                    Console.WriteLine($"{this.parent.MachineName} Process Start: {rtn}");

                    LiftFlow();

                    rtn = Globalo.motionManager.transferMachine.IsMoving();

                    Console.WriteLine($"{this.parent.MachineName} Process End: {rtn}");
                }
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
