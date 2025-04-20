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
        public int MotorCnt { get; private set; } = 6;

        public MotionControl.MotorAxis LoadLift_Z_L;
        public MotionControl.MotorAxis UnLoadLift_Z_L;
        public MotionControl.MotorAxis Gantry_Y_L;

        public MotionControl.MotorAxis LoadLift_Z_R;
        public MotionControl.MotorAxis UnLoadLift_Z_R;
        public MotionControl.MotorAxis Gantry_Y_R;

        public MotionControl.MotorAxis[] MotorAxes; // 배열 선언

        public string[] axisName = { "LoadZ_L", "UnLoadZ_L", "GantryY_L" , "LoadZ_R", "UnLoadZ_R", "GantryY_R" };

        private MotorDefine.eMotorType[] motorType = { MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR };
        private AXT_MOTION_LEVEL_MODE[] AXT_SET_LIMIT = { AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW };
        private AXT_MOTION_LEVEL_MODE[] AXT_SET_SERVO_ALARM = { AXT_MOTION_LEVEL_MODE.HIGH, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW };
        private AXT_MOTION_HOME_DETECT[] MOTOR_HOME_SENSOR = { AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor };
        private AXT_MOTION_MOVE_DIR[] MOTOR_HOME_DIR = { AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW };


        private static double[] MaxSpeeds = { 100.0, 100.0, 200.0, 100.0, 100.0, 200.0 };
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
            int i = 0;
            this.RunState = OperationState.Stopped;
            this.MachineName = this.GetType().Name;

            MotorAxes = new MotionControl.MotorAxis[] { LoadLift_Z_L, UnLoadLift_Z_L, Gantry_Y_L, LoadLift_Z_R, UnLoadLift_Z_R, Gantry_Y_R };
            MotorCnt = MotorAxes.Length;

            for (i = 0; i < MotorCnt; i++)
            {
                int index = (int)MotionControl.MotorSet.ValidLiftMotors[i];
                MotorAxes[i] = new MotionControl.MotorAxis(index,
                axisName[i], motorType[i], MaxSpeeds[i], AXT_SET_LIMIT[i], AXT_SET_SERVO_ALARM[i], OrgFirstVel[i], OrgSecondVel[i], OrgThirdVel[i],
                MOTOR_HOME_SENSOR[i], MOTOR_HOME_DIR[i]);


                //초기 셋 다른 곳에서 다시 해줘야될 듯
                MotorAxes[i].setMotorParameter(10.0, 0.1, 0.1, 1000.0);//(double vel , double acc , double dec , double resol)
            }

            trayProduct = Data.TaskDataYaml.TaskLoad_Lift(taskPath);


        }

        public override bool TaskSave()
        {
            bool rtn = Data.TaskDataYaml.TaskSave_Lift(trayProduct, taskPath);
            return rtn;
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
            AutoUnitThread.Stop();

        }
        public override void MovingStop()
        {
            if (CancelToken != null && !CancelToken.IsCancellationRequested)
            {
                CancelToken.Cancel();
            }
            //TransferZ.motorBreak = true;          //예제코드

            //TransferZ.Stop();                 //예제코드

        }
        public override bool OriginRun()
        {
            AutoUnitThread.m_nCurrentStep = 1000;

            AutoUnitThread.m_nStartStep = 1000;
            AutoUnitThread.m_nEndStep = 20000;

            if (AutoUnitThread.GetThreadRun() == true)
            {
                Console.WriteLine($"원점 동작 중입니다.");

                AutoUnitThread.Stop();
                Thread.Sleep(300);
            }
            bool rtn = AutoUnitThread.Start();


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
