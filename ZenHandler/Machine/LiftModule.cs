using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZenHandler.Machine
{
    public class LiftModule : MotionControl.MotorController
    {
        public LiftModule()// : base("LiftModule")
        {
            this.RunState = OperationState.Stopped;
            this.MachineName = this.GetType().Name;

        }
        public override bool IsMoving()
        {

            return true;
        }
        public override void MotorDataSet()
        {
        }
        public override void StopAuto()
        {
            motorAutoThread.Stop();

        }
        public override void MovingStop()
        {
            if (cts != null && !cts.IsCancellationRequested)
            {
                cts.Cancel();
            }
            //TransferZ.motorBreak = true;          //예제코드

            //TransferZ.Stop();                 //예제코드

        }
        public override void MoveToPosition(int position)
        {
            Console.WriteLine($"매거진 {position} 위치로 이동");
        }
        public override bool OriginRun()
        {
            motorAutoThread.m_nCurrentStep = 1000;

            motorAutoThread.m_nStartStep = 1000;
            motorAutoThread.m_nEndStep = 20000;

            if (motorAutoThread.GetThreadRun() == true)
            {
                Console.WriteLine($"원점 동작 중입니다.");

                motorAutoThread.Stop();
                Thread.Sleep(300);
            }
            bool rtn = motorAutoThread.Start();


            return rtn;
        }
        public override bool ReadyRun()
        {


            return true;
        }
        public override void PauseAuto()
        {

            return;
        }
        public override bool AutoRun()
        {

            return true;
        }

    }
}
