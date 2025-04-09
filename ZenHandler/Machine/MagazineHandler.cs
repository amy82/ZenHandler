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
        public MagazineHandler()// : base("MagazineHandler")
        {
            MagazineY = new MotionControl.MotorAxis((int)MotorControl.ePcbMotor.PCB_TX, "MagazineY");
            MagazineZ = new MotionControl.MotorAxis((int)MotorControl.ePcbMotor.PCB_TY, "MagazineZ");

            this.MachineName = this.GetType().Name;
        }
        public override bool IsMoving()
        {
            return true;
        }
        public override void MovingStop()
        {
            this.cts?.Cancel();
            this.motorBreak = true; //MovingStop
        }
        public override void MoveToPosition(int position)   //abstract
        {
            Console.WriteLine($"매거진 {position} 위치로 이동");
        }
        public override bool JogMove(int direction, int Speed)  //virtual
        {
            Console.WriteLine($"커스텀 조그 이동: 속도 조절 후 방향 {direction}");
            return true;
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
