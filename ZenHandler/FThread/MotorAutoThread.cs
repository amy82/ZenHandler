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
            this.name = this.parent.MachineName + " MotorAutoThread";
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
                this.m_nCurrentStep = this.parent.processManager.transferFlow.Auto_BcrLoadInTray(this.m_nCurrentStep);       //바코드 스캔 + 제품 로드
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
                this.m_nCurrentStep = this.parent.processManager.transferFlow.Auto_UnLoadInTray(this.m_nCurrentStep);     //제품 TRAY 배출
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
            else if (this.m_nCurrentStep >= 2000 && this.m_nCurrentStep < 3000)
            {
                this.m_nCurrentStep = this.parent.processManager.liftFlow.AutoReady(this.m_nCurrentStep);
            }

            //1. Left TRAY 투입 Flow - Gantry Left 이동후 진행
            //2. Left ---->  Right TRAY 이동 Flow -  - Gantry Right 이동후 진행
            //3. RIGHT TRAY 배출 Flow
            //4??
        }
        private void MagazineFlow()
        {
            if (this.m_nCurrentStep >= 1000 && this.m_nCurrentStep < 2000)
            {
                this.m_nCurrentStep = this.parent.processManager.magazineFlow.HomeProcess(this.m_nCurrentStep);
            }
            else if (this.m_nCurrentStep >= 2000 && this.m_nCurrentStep < 3000)
            {
                this.m_nCurrentStep = this.parent.processManager.magazineFlow.AutoReady(this.m_nCurrentStep);
            }

            //1.L,R 공통 TRAY Load <-- Magazine Flow (Magazine Load 위치로 Z축 이동후 -> Tray 꺼내서 투입 위치로 Z축 이동)
            //2.L,R 공통 TRAY Unload --> Magazine Flow (Magazine 원래 위치로 Z축 이동후 -> Y축 이동해서 집어넣기)
            //3.??
        }

        private void SocketFlow()
        {
            if (this.m_nCurrentStep >= 1000 && this.m_nCurrentStep < 2000)
            {
                if (Program.PG_SELECT == HANDLER_PG.FW)
                {
                    this.m_nCurrentStep = this.parent.processManager.fwSocketFlow.HomeProcess(this.m_nCurrentStep);
                }
                if (Program.PG_SELECT == HANDLER_PG.AOI)
                {
                    this.m_nCurrentStep = this.parent.processManager.aoiSocketFlow.HomeProcess(this.m_nCurrentStep);
                }
                if (Program.PG_SELECT == HANDLER_PG.EEPROM)
                {
                    this.m_nCurrentStep = this.parent.processManager.eepromSocketFlow.HomeProcess(this.m_nCurrentStep);
                }
                    
            }else if (this.m_nCurrentStep >= 2000 && this.m_nCurrentStep < 3000)
            {
                if (Program.PG_SELECT == HANDLER_PG.FW)
                {
                    this.m_nCurrentStep = this.parent.processManager.fwSocketFlow.AutoReady(this.m_nCurrentStep);
                }
                if (Program.PG_SELECT == HANDLER_PG.AOI)
                {
                    this.m_nCurrentStep = this.parent.processManager.aoiSocketFlow.AutoReady(this.m_nCurrentStep);
                }
                if (Program.PG_SELECT == HANDLER_PG.EEPROM)
                {
                    this.m_nCurrentStep = this.parent.processManager.eepromSocketFlow.AutoReady(this.m_nCurrentStep);
                }

            }

            //EEprom  , Aoi 설비만 Socket 모터 있음 
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
                    MagazineFlow();
                }
                else if (this.parent.MachineName == Globalo.motionManager.liftMachine.GetType().Name)
                {
                    bool rtn = Globalo.motionManager.transferMachine.IsMoving();

                    Console.WriteLine($"{this.parent.MachineName} Process Start: {rtn}");

                    LiftFlow();

                    rtn = Globalo.motionManager.transferMachine.IsMoving();

                    Console.WriteLine($"{this.parent.MachineName} Process End: {rtn}");
                }
                else if (this.parent.MachineName == Globalo.motionManager.socketAoiMachine.GetType().Name ||        //TODO: 확인 필요
                    this.parent.MachineName == Globalo.motionManager.socketFwMachine.GetType().Name ||
                    this.parent.MachineName == Globalo.motionManager.socketEEpromMachine.GetType().Name)
                {
                    SocketFlow();
                }

            }
            else if(this.m_nCurrentStep < 0)
            {
                //Pause
                if (this.parent.MachineName == Globalo.motionManager.transferMachine.GetType().Name)
                {
                    Globalo.motionManager.transferMachine.RunState = OperationState.Paused;
                }
                else if (this.parent.MachineName == Globalo.motionManager.magazineHandler.GetType().Name)
                {
                    Globalo.motionManager.magazineHandler.RunState = OperationState.Paused;
                }
                else if (this.parent.MachineName == Globalo.motionManager.liftMachine.GetType().Name)
                {
                    Globalo.motionManager.liftMachine.RunState = OperationState.Paused;
                }
                else if (this.parent.MachineName == Globalo.motionManager.socketAoiMachine.GetType().Name)
                {
                    Globalo.motionManager.socketAoiMachine.RunState = OperationState.Paused;
                }
                else if (this.parent.MachineName == Globalo.motionManager.socketFwMachine.GetType().Name)
                {
                    Globalo.motionManager.socketFwMachine.RunState = OperationState.Paused;
                }
                else if (this.parent.MachineName == Globalo.motionManager.socketEEpromMachine.GetType().Name)
                {
                    Globalo.motionManager.socketEEpromMachine.RunState = OperationState.Paused;
                }

                Console.WriteLine($"{this.parent.MachineName} Process Pause");
                this.Pause();
            }
            else
            {
                //stop
                m_nStartStep = 0;
                m_nEndStep = 0;
                this.Stop();

                Console.WriteLine($"{this.parent.MachineName} Process Stop");
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
