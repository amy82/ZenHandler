using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.Machine
{
    public class MagazineHandler : MotionControl.MotorController
    {
        public int MotorCnt { get; private set; } = 3;

        private MotionControl.MotorAxis MagazineY;
        private MotionControl.MotorAxis MagazineZ;

        public MotionControl.MotorAxis[] MotorAxes; // 배열 선언

        public static double[] MOTOR_MAX_SPEED = { 100.0, 100.0};
        public MotorDefine.eMotorType[] motorType = { MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR };
        public AXT_MOTION_LEVEL_MODE[] AXT_SET_LIMIT = { AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW };
        public AXT_MOTION_LEVEL_MODE[] AXT_SET_SERVO_ALARM = { AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW };
        public static AXT_MOTION_HOME_DETECT[] MOTOR_HOME_SENSOR = { AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor };
        public static AXT_MOTION_MOVE_DIR[] MOTOR_HOME_DIR = { AXT_MOTION_MOVE_DIR.DIR_CCW, AXT_MOTION_MOVE_DIR.DIR_CCW };

        public double[] OrgFirstVel = { 5000.0, 5000.0 };
        public double[] OrgSecondVel = { 2500.0, 2500.0 };
        public double[] OrgThirdVel = { 500.0, 500.0 };

        public enum eTeachingPosList : int
        {
            WAIT_POS = 0, LEFT_LOAD_POS, LEFT_UNLOAD_POS, TOTAL_MAGAZINE_TEACHING_COUNT
        };
        public string teachingPath = "Teach_Magazine.yaml";
        public Data.TeachingConfig teachingConfig = new Data.TeachingConfig();

        public MagazineHandler()// : base("MagazineHandler")
        {
            this.RunState = OperationState.Stopped;
            this.MachineName = this.GetType().Name;

            MagazineY = new MotionControl.MotorAxis((int)MotionControl.MotorSet.eLiftMotorList.L_IN_LIFT, "MagazineY", motorType[0], 
                MOTOR_MAX_SPEED[0], AXT_SET_LIMIT[0], AXT_SET_SERVO_ALARM[0], OrgFirstVel[0], OrgSecondVel[0], OrgThirdVel[0], MOTOR_HOME_SENSOR[0], MOTOR_HOME_DIR[0]);

            MagazineZ = new MotionControl.MotorAxis((int)MotionControl.MotorSet.eLiftMotorList.L_OUT_LIFT, "MagazineZ", motorType[1], 
                MOTOR_MAX_SPEED[1], AXT_SET_LIMIT[1], AXT_SET_SERVO_ALARM[1], OrgFirstVel[1], OrgSecondVel[1], OrgThirdVel[1], MOTOR_HOME_SENSOR[1], MOTOR_HOME_DIR[1]);

            MotorAxes = new MotionControl.MotorAxis[] { MagazineY, MagazineZ };
            MotorCnt = MotorAxes.Length;

            MagazineY.setMotorParameter(10.0, 0.1, 0.1, 1000.0);     //초기 셋 다른 곳에서 다시 해줘야될 듯
            MagazineZ.setMotorParameter(10.0, 0.1, 0.1, 1000.0);


            this.MachineName = this.GetType().Name;
        }
        public override bool IsMoving()
        {
            return true;
        }
        public override void StopAuto()
        {
            motorAutoThread.Stop();

        }
        public override void MotorDataSet()
        {
            int i = 0;
            for (i = 0; i < MotorAxes.Length; i++)
            {
                MotorAxes[i].setMotorParameter(teachingConfig.Speed[i], teachingConfig.Accel[i], teachingConfig.Decel[i], teachingConfig.Resolution[i]);
            }
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
        public override void PauseAuto()
        {

            return;
        }
        public override bool ReadyRun()
        {
            return true;
        }
        public override bool AutoRun()
        {
            return true;
        }


    }
}
