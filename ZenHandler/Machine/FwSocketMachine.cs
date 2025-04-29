using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.Machine
{
    public class FwSocketMachine : MotionControl.MotorController
    {
        public int MotorCnt { get; private set; } = 4;

        //실린더 전후진 4개
        //실린더 상승,하강 4개


        public const string teachingPath = "Teach_FwSocket.yaml";
        public const string taskPath = "Task_FwSocket.yaml";
        public Data.TeachingConfig teachingConfig = new Data.TeachingConfig();

        public FwSocketMachine()
        {
            int i = 0;
            this.RunState = OperationState.Stopped;
            this.MachineName = this.GetType().Name;

            

        }
        public override bool TaskSave()
        {
            //bool rtn = Data.TaskDataYaml.TaskSave_Transfer(socketProduct, taskPath);
            //return rtn;
            return false;
        }
        public override void MotorDataSet()
        {

        }
        public override void MovingStop()
        {
            if (CancelToken != null && !CancelToken.IsCancellationRequested)
            {
                CancelToken.Cancel();
            }
        }
        public override bool IsMoving()
        {
            if (AutoUnitThread.GetThreadRun() == true)
            {
                return true;
            }
            return false;
        }
        public override void StopAuto()
        {
            AutoUnitThread.Stop();
            MovingStop();
            RunState = OperationState.Stopped;
            Console.WriteLine($"[INFO] FwSocket Run Stop");

        }
        public override bool OriginRun()
        {
            if (AutoUnitThread.GetThreadRun() == true)
            {
                //motorAutoThread.Stop();
                return false;
            }
            return true;
        }
        public override bool ReadyRun()
        {
            if (AutoUnitThread.GetThreadRun() == true)
            {
                return false;
            }
            this.RunState = OperationState.Preparing;   //TODO: 모터없는 부분이라 확인필요
            AutoUnitThread.m_nCurrentStep = 2000;
            //if (TransferX.OrgState == false || TransferY.OrgState == false || TransferZ.OrgState == false)
            //{
            //    this.RunState = OperationState.OriginRunning;
            //    AutoUnitThread.m_nCurrentStep = 1000;
            //}
            //else
            //{
            //    this.RunState = OperationState.Preparing;
            //    AutoUnitThread.m_nCurrentStep = 2000;
            //}

            AutoUnitThread.m_nEndStep = 3000;
            AutoUnitThread.m_nStartStep = AutoUnitThread.m_nCurrentStep;

            if (AutoUnitThread.GetThreadRun() == true)
            {
                Console.WriteLine($"모터 동작 중입니다.");
                return true;
            }
            bool rtn = AutoUnitThread.Start();
            if (rtn)
            {
                Console.WriteLine($"[READY] Transfer Ready Start");
                Console.WriteLine($"모터 동작 성공.");
            }
            else
            {
                this.RunState = OperationState.Stopped;
                Console.WriteLine($"[READY] Transfer Ready Start Fail");
                Console.WriteLine($"모터 동작 실패.");
            }

            return rtn;
        }
        public override void PauseAuto()
        {
            if (AutoUnitThread.GetThreadRun() == true)
            {
                AutoUnitThread.Pause();
                RunState = OperationState.Paused;
            }
        }
        public override bool AutoRun()
        {
            bool rtn = true;
            if (this.RunState != OperationState.PreparationComplete)
            {
                Globalo.LogPrint("MainForm", "[FWSOCKET] 운전준비가 완료되지 않았습니다.", Globalo.eMessageName.M_WARNING);
                return false;
            }

            if (AutoUnitThread.GetThreadRun() == true)
            {
                Console.WriteLine($"모터 동작 중입니다.");

                if (AutoUnitThread.GetThreadPause() == true)        //일시 정지 상태인지 확인
                {
                    AutoUnitThread.m_nCurrentStep = Math.Abs(AutoUnitThread.m_nCurrentStep);

                    RunState = OperationState.AutoRunning;
                }
                else
                {
                    rtn = false;
                }
            }
            else
            {
                AutoUnitThread.m_nCurrentStep = 3000;
                AutoUnitThread.m_nEndStep = 10000;
                AutoUnitThread.m_nStartStep = AutoUnitThread.m_nCurrentStep;

                rtn = AutoUnitThread.Start();

                if (rtn)
                {
                    RunState = OperationState.AutoRunning;
                    Console.WriteLine($"FWSOCKET 모터 동작 성공.");
                }
                else
                {
                    Console.WriteLine($"FWSOCKET 모터 동작 실패.");
                }
            }
            return rtn;
        }
    }
    
}
