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

        public static double[] MOTOR_MAX_SPEED = { 100.0, 100.0};
        public MotorDefine.eMotorType[] motorType = { MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR };
        public AXT_MOTION_LEVEL_MODE[] AXT_SET_LIMIT = { AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW };
        public AXT_MOTION_LEVEL_MODE[] AXT_SET_SERVO_ALARM = { AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW };


        public double[] OrgFirstVel = { 5000.0, 5000.0 };
        public double[] OrgSecondVel = { 2500.0, 2500.0 };
        public double[] OrgThirdVel = { 500.0, 500.0 };
        public MagazineHandler()// : base("MagazineHandler")
        {
            MagazineY = new MotionControl.MotorAxis((int)MotionControl.MotorSet.eMotorList.L_IN_LIFT, "MagazineY", motorType[0], MOTOR_MAX_SPEED[0], AXT_SET_LIMIT[0], AXT_SET_SERVO_ALARM[0], OrgFirstVel[0], OrgSecondVel[0], OrgThirdVel[0]);
            MagazineZ = new MotionControl.MotorAxis((int)MotionControl.MotorSet.eMotorList.L_OUT_LIFT, "MagazineZ", motorType[1], MOTOR_MAX_SPEED[1], AXT_SET_LIMIT[1], AXT_SET_SERVO_ALARM[1], OrgFirstVel[1], OrgSecondVel[1], OrgThirdVel[1]);

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

        public override bool OriginRun()
        {

            return false;
        }
        public override void ReadyRun()
        {

        }
        public override void AutoRun()
        {

        }


    }
}
