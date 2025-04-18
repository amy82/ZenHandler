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

        private MotionControl.MotorAxis MagazineY_L;
        private MotionControl.MotorAxis MagazineZ_L;
        private MotionControl.MotorAxis MagazineY_R;
        private MotionControl.MotorAxis MagazineZ_R;

        public MotionControl.MotorAxis[] MotorAxes; // 배열 선언

        public string[] axisName = { "MagazineY_L", "MagazineZ_L", "MagazineY_R", "MagazineZ_R" };

        public static double[] MOTOR_MAX_SPEED = { 100.0, 100.0, 100.0, 100.0 };
        public MotorDefine.eMotorType[] motorType = { MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR };
        public AXT_MOTION_LEVEL_MODE[] AXT_SET_LIMIT = { AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW };
        public AXT_MOTION_LEVEL_MODE[] AXT_SET_SERVO_ALARM = { AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW };
        public static AXT_MOTION_HOME_DETECT[] MOTOR_HOME_SENSOR = { AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor };
        public static AXT_MOTION_MOVE_DIR[] MOTOR_HOME_DIR = { AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW };

        public double[] OrgFirstVel = { 5000.0, 5000.0, 5000.0, 5000.0 };
        public double[] OrgSecondVel = { 2500.0, 2500.0, 2500.0, 2500.0 };
        public double[] OrgThirdVel = { 500.0, 500.0, 500.0, 500.0 };

        public enum eTeachingPosList : int
        {
            WAIT_POS = 0, 
            LEFT_TRAY_LOAD_POS, LEFT_TRAY_UNLOAD_POS,
            STACK1_L, STACK2_L, STACK3_L, STACK4_L, STACK5_L,
            STACK1_R, STACK2_R, STACK3_R, STACK4_R, STACK5_R,
            TOTAL_MAGAZINE_TEACHING_COUNT
        };

        public string[] TeachName = { "WAIT_POS",
            "LEFT_TRAY_LOAD_POS", "LEFT_TRAY_UNLOAD_POS",
            "STACK1_L","STACK2_L","STACK3_L","STACK4_L","STACK5_L",
            "STACK1_R","STACK2_R","STACK3_R","STACK4_R","STACK5_R",
        };

        //TRAY 꺼내는 층별 위치 다 따로 해야될수도
        public string teachingPath = "Teach_Magazine.yaml";
        public string taskPath = "Task_Magazine.yaml";
        public Data.TeachingConfig teachingConfig = new Data.TeachingConfig();
        //public LayerTray pickedProduct = new LayerTray();
        public MagazineTray magazineTray = new MagazineTray();

        public MagazineHandler()// : base("MagazineHandler")
        {
            this.RunState = OperationState.Stopped;
            this.MachineName = this.GetType().Name;

            //     private MotionControl.MotorAxis MagazineY_L;
            //private MotionControl.MotorAxis MagazineZ_L;
            //private MotionControl.MotorAxis MagazineY_R;
            //private MotionControl.MotorAxis MagazineZ_R;

            MagazineY_L = new MotionControl.MotorAxis((int)MotionControl.MotorSet.eMagazineMotorList.L_MAGAZINE_Z,
                axisName[0], motorType[0], MOTOR_MAX_SPEED[0], AXT_SET_LIMIT[0], AXT_SET_SERVO_ALARM[0], OrgFirstVel[0], OrgSecondVel[0], OrgThirdVel[0], MOTOR_HOME_SENSOR[0], MOTOR_HOME_DIR[0]);
            MagazineZ_L = new MotionControl.MotorAxis((int)MotionControl.MotorSet.eMagazineMotorList.L_MAGAZINE_Y,
                axisName[1], motorType[1], MOTOR_MAX_SPEED[1], AXT_SET_LIMIT[1], AXT_SET_SERVO_ALARM[1], OrgFirstVel[1], OrgSecondVel[1], OrgThirdVel[1], MOTOR_HOME_SENSOR[1], MOTOR_HOME_DIR[1]);
            MagazineY_R = new MotionControl.MotorAxis((int)MotionControl.MotorSet.eMagazineMotorList.R_MAGAZINE_Z,
                axisName[2], motorType[2], MOTOR_MAX_SPEED[2], AXT_SET_LIMIT[2], AXT_SET_SERVO_ALARM[2], OrgFirstVel[2], OrgSecondVel[2], OrgThirdVel[2], MOTOR_HOME_SENSOR[2], MOTOR_HOME_DIR[2]);
            MagazineZ_R = new MotionControl.MotorAxis((int)MotionControl.MotorSet.eMagazineMotorList.R_MAGAZINE_Y,
                axisName[3], motorType[3], MOTOR_MAX_SPEED[3], AXT_SET_LIMIT[3], AXT_SET_SERVO_ALARM[3], OrgFirstVel[3], OrgSecondVel[3], OrgThirdVel[3], MOTOR_HOME_SENSOR[3], MOTOR_HOME_DIR[3]);

            MotorAxes = new MotionControl.MotorAxis[] { MagazineY_L, MagazineZ_L, MagazineY_R, MagazineZ_R };
            MotorCnt = MotorAxes.Length;

            MagazineY_L.setMotorParameter(10.0, 0.1, 0.1, 1000.0);     //초기 셋 다른 곳에서 다시 해줘야될 듯
            MagazineZ_L.setMotorParameter(10.0, 0.1, 0.1, 1000.0);
            MagazineY_R.setMotorParameter(10.0, 0.1, 0.1, 1000.0);
            MagazineZ_R.setMotorParameter(10.0, 0.1, 0.1, 1000.0);



            magazineTray = Data.TaskDataYaml.TaskLoad_Magazine(taskPath);
            

        }
        public override bool TaskSave()
        {
            bool rtn = Data.TaskDataYaml.TaskSave_Magazine(magazineTray, taskPath);
            return rtn;
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
