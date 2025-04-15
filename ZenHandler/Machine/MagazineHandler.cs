using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.Machine
{
    public class MagazineHandler : MotionControl.MotorController
    {
        private MotionControl.MotorAxis MagazineY;
        private MotionControl.MotorAxis MagazineZ;

        public MotorDefine.eMotorType[] motorType = { MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR };

        public MagazineHandler()// : base("MagazineHandler")
        {
            MagazineY = new MotionControl.MotorAxis((int)MotionControl.MotorSet.eMotorList.L_IN_LIFT, "MagazineY", motorType[0]);
            MagazineZ = new MotionControl.MotorAxis((int)MotionControl.MotorSet.eMotorList.L_OUT_LIFT, "MagazineZ", motorType[1]);

            //TransferX = new MotionControl.MotorAxis((int)MotionControl.MotorSet.eMotorList.TRANSFER_X, axisName[0], motorType[0]);
            this.MachineName = this.GetType().Name;
        }
        public override bool IsMoving()
        {
            return true;
        }
        public override void MotorDataSet()
        {
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
        public override void MoveToPosition(int position)   //abstract
        {
            Console.WriteLine($"매거진 {position} 위치로 이동");
        }

        public override void OriginRun()
        {

        }
        public override void ReadyRun()
        {

        }
        public override void AutoRun()
        {

        }


    }
}
