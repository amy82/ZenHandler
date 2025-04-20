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
                this.m_nCurrentStep = this.parent.processManager.transferFlow.Auto_Waiting(this.m_nCurrentStep);    //제품을 들고있는지, 왼쪽,오른쪽 TRAY 선택
            }
            else if (this.m_nCurrentStep >= 4000 && this.m_nCurrentStep < 5000)
            {
                this.m_nCurrentStep = this.parent.processManager.transferFlow.Auto_LoadInTray(this.m_nCurrentStep);       //제품 로드
            }
            else if (this.m_nCurrentStep >= 5000 && this.m_nCurrentStep < 6000)
            {
                this.m_nCurrentStep = this.parent.processManager.transferFlow.Auto_SocketInsert(this.m_nCurrentStep);     //소켓 투입
            }
            else if (this.m_nCurrentStep >= 6000 && this.m_nCurrentStep < 7000)
            {
                this.m_nCurrentStep = this.parent.processManager.transferFlow.Auto_SocketOutput(this.m_nCurrentStep);     //소켓 배출
            }
            else if (this.m_nCurrentStep >= 7000 && this.m_nCurrentStep < 8000)
            {
                this.m_nCurrentStep = this.parent.processManager.transferFlow.Auto_UnLoadInTray(this.m_nCurrentStep);     //제품 배출
            }
            else if (this.m_nCurrentStep >= 8000 && this.m_nCurrentStep < 9000)
            {
                this.m_nCurrentStep = this.parent.processManager.transferFlow.Auto_Ng_UnLoading(this.m_nCurrentStep);       //ng 배출
            }
            else if (this.m_nCurrentStep >= 10000 && this.m_nCurrentStep < 11000)
            {
                this.m_nCurrentStep = this.parent.processManager.transferFlow.Auto_Cancel(this.m_nCurrentStep);             //투입 취소
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
            if(this.parent.RunState == OperationState.Paused)
            {
                return;     //TODO: 테스트 필요 Paused
            }
            if (this.m_nCurrentStep >= this.m_nStartStep && this.m_nCurrentStep < this.m_nEndStep)
            {
                if (this.parent.MachineName == Globalo.motionManager.transferMachine.GetType().Name)
                {
                    TransferFlow();
                }
                else if (this.parent.MachineName == Globalo.motionManager.magazineHandler.GetType().Name)
                {

                }
                else if (this.parent.MachineName == Globalo.motionManager.liftMachine.GetType().Name)
                {
                    bool rtn = Globalo.motionManager.transferMachine.IsMoving();

                    Console.WriteLine($"{this.parent.MachineName} Process Start: {rtn}");

                    //LiftFlow();

                    rtn = Globalo.motionManager.transferMachine.IsMoving();

                    Console.WriteLine($"{this.parent.MachineName} Process End: {rtn}");
                }
            }
            else if(this.m_nCurrentStep < 0)
            {
                //Pause
                Globalo.motionManager.transferMachine.RunState = OperationState.Paused;
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
