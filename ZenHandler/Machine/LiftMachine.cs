using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZenHandler.Machine
{
    public class LiftMachine : MotionControl.MotorController
    {
        public int MotorCnt { get; private set; } = 3;  //6개로

        public MotionControl.MotorAxis LoadLift_Z_L;
        public MotionControl.MotorAxis UnLoadLift_Z_L;
        public MotionControl.MotorAxis Gantry_Y_L;

        public MotionControl.MotorAxis LoadLift_Z_R;
        public MotionControl.MotorAxis UnLoadLift_Z_R;
        public MotionControl.MotorAxis Gantry_Y_R;

        public MotionControl.MotorAxis[] MotorAxes; // 배열 선언

        public string[] axisName = { "LoadZ_L", "UnLoadZ_L", "GantryY_L" , "LoadZ_R", "UnLoadZ_R", "GantryY_R" };
        private static double[] MOTOR_MAX_SPEED = { 100.0, 100.0, 200.0, 100.0, 100.0, 200.0 };

        private MotorDefine.eMotorType[] motorType = { MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR };
        private AXT_MOTION_LEVEL_MODE[] AXT_SET_LIMIT = { AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW };
        private AXT_MOTION_LEVEL_MODE[] AXT_SET_SERVO_ALARM = { AXT_MOTION_LEVEL_MODE.HIGH, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW };

        private static AXT_MOTION_HOME_DETECT[] MOTOR_HOME_SENSOR = { AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor };

        private static AXT_MOTION_MOVE_DIR[] MOTOR_HOME_DIR = { AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW };

        private double[] OrgFirstVel = { 20000.0, 20000.0, 20000.0, 20000.0, 20000.0, 20000.0 };
        private double[] OrgSecondVel = { 5000.0, 5000.0, 10000.0, 5000.0, 5000.0, 10000.0 };
        private double[] OrgThirdVel = { 2500.0, 2500.0, 5000.0, 2500.0, 2500.0, 5000.0 };


        public enum eTeachingPosList : int
        {
            WAIT_POS = 0, LOAD_POS, UNLOAD_POS, TOTAL_LIFT_TEACHING_COUNT
        };

        public string[] TeachName = { "WAIT_POS" , "LOAD_POS", "UNLOAD_POS" };


        public string teachingPath = "Teach_Lift.yaml";
        public string taskPath = "Task_Lift.yaml";
        public Data.TeachingConfig teachingConfig = new Data.TeachingConfig();

        //public PickedProduct pickedProduct = new PickedProduct();

        public TrayProduct trayProduct = new TrayProduct();

        public LiftMachine()// : base("LiftModule")
        {
            this.RunState = OperationState.Stopped;
            this.MachineName = this.GetType().Name;

            LoadLift_Z_L = new MotionControl.MotorAxis((int)MotionControl.MotorSet.eLiftMotorList.LOAD_Z_L,
                axisName[0], motorType[0], MOTOR_MAX_SPEED[0], AXT_SET_LIMIT[0], AXT_SET_SERVO_ALARM[0], OrgFirstVel[0], OrgSecondVel[0], OrgThirdVel[0], MOTOR_HOME_SENSOR[0], MOTOR_HOME_DIR[0]);
            UnLoadLift_Z_L = new MotionControl.MotorAxis((int)MotionControl.MotorSet.eLiftMotorList.UNLOAD_Z_L,
                axisName[1], motorType[1], MOTOR_MAX_SPEED[1], AXT_SET_LIMIT[1], AXT_SET_SERVO_ALARM[1], OrgFirstVel[1], OrgSecondVel[1], OrgThirdVel[1], MOTOR_HOME_SENSOR[1], MOTOR_HOME_DIR[1]);
            Gantry_Y_L = new MotionControl.MotorAxis((int)MotionControl.MotorSet.eLiftMotorList.GANTRY_Y_L,
                axisName[2], motorType[2], MOTOR_MAX_SPEED[2], AXT_SET_LIMIT[2], AXT_SET_SERVO_ALARM[2], OrgFirstVel[2], OrgSecondVel[2], OrgThirdVel[2], MOTOR_HOME_SENSOR[2], MOTOR_HOME_DIR[2]);

            LoadLift_Z_R = new MotionControl.MotorAxis((int)MotionControl.MotorSet.eLiftMotorList.LOAD_Z_R,
                axisName[3], motorType[3], MOTOR_MAX_SPEED[3], AXT_SET_LIMIT[3], AXT_SET_SERVO_ALARM[3], OrgFirstVel[3], OrgSecondVel[3], OrgThirdVel[3], MOTOR_HOME_SENSOR[3], MOTOR_HOME_DIR[3]);
            UnLoadLift_Z_R = new MotionControl.MotorAxis((int)MotionControl.MotorSet.eLiftMotorList.UNLOAD_Z_R,
                axisName[4], motorType[4], MOTOR_MAX_SPEED[4], AXT_SET_LIMIT[4], AXT_SET_SERVO_ALARM[4], OrgFirstVel[4], OrgSecondVel[4], OrgThirdVel[4], MOTOR_HOME_SENSOR[4], MOTOR_HOME_DIR[4]);
            Gantry_Y_R = new MotionControl.MotorAxis((int)MotionControl.MotorSet.eLiftMotorList.GANTRY_Y_R,
                axisName[5], motorType[5], MOTOR_MAX_SPEED[5], AXT_SET_LIMIT[5], AXT_SET_SERVO_ALARM[5], OrgFirstVel[5], OrgSecondVel[5], OrgThirdVel[5], MOTOR_HOME_SENSOR[5], MOTOR_HOME_DIR[5]);

            MotorAxes = new MotionControl.MotorAxis[] { LoadLift_Z_L, UnLoadLift_Z_L, Gantry_Y_L, LoadLift_Z_L, UnLoadLift_Z_L, Gantry_Y_L };
            MotorCnt = MotorAxes.Length;

            LoadLift_Z_L.setMotorParameter(10.0, 0.1, 0.1, 1000.0);     //초기 셋 다른 곳에서 다시 해줘야될 듯
            UnLoadLift_Z_L.setMotorParameter(10.0, 0.1, 0.1, 1000.0);
            Gantry_Y_L.setMotorParameter(10.0, 0.1, 0.1, 1000.0);
            LoadLift_Z_R.setMotorParameter(10.0, 0.1, 0.1, 1000.0);
            UnLoadLift_Z_R.setMotorParameter(10.0, 0.1, 0.1, 1000.0);
            Gantry_Y_R.setMotorParameter(10.0, 0.1, 0.1, 1000.0);

            trayProduct = Data.TaskDataYaml.TaskLoad_Lift(taskPath);


        }
        public override void MotorDataSet()
        {
            int i = 0;
            for (i = 0; i < MotorAxes.Length; i++)
            {
                MotorAxes[i].setMotorParameter(teachingConfig.Speed[i], teachingConfig.Accel[i], teachingConfig.Decel[i], teachingConfig.Resolution[i]);
            }

            for (i = 0; i < teachingConfig.Teaching.Count; i++)
            {
                teachingConfig.Teaching[i].Name = TeachName[i];
            }


        }
        public override bool IsMoving()
        {

            return true;
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
