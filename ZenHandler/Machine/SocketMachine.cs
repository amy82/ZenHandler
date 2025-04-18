using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.Machine
{
    public class SocketMachine : MotionControl.MotorController
    {
        public int MotorCnt { get; private set; } = 5;

        public MotionControl.MotorAxis FrontSocketX;    //eeprom 공정
        public MotionControl.MotorAxis BackSocketX;    //eeprom 공정
        public MotionControl.MotorAxis BackSocketY;    //eeprom 공정

        public MotionControl.MotorAxis CamZ_L;          //AOI 공정
        public MotionControl.MotorAxis CamZ_R;          //AOI 공정

        public MotionControl.MotorAxis[] MotorAxes; // 배열 선언

        public string[] axisName = { "FrontSocketX", "BackSocketX", "BackSocketY", "CAMZ_L", "CAMZ_R" };

        private static double[] MOTOR_MAX_SPEED = { 200.0, 200.0, 200.0, 200.0, 200.0 };
        private MotorDefine.eMotorType[] motorType = { MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR };
        private AXT_MOTION_LEVEL_MODE[] AXT_SET_LIMIT = { AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW };
        private AXT_MOTION_LEVEL_MODE[] AXT_SET_SERVO_ALARM = { AXT_MOTION_LEVEL_MODE.HIGH, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW };

        private static AXT_MOTION_HOME_DETECT[] MOTOR_HOME_SENSOR = { AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor };

        private static AXT_MOTION_MOVE_DIR[] MOTOR_HOME_DIR = { AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW };

        private double[] OrgFirstVel = { 20000.0, 20000.0, 20000.0, 20000.0, 20000.0 };
        private double[] OrgSecondVel = { 10000.0, 10000.0, 10000.0, 10000.0, 10000.0 };
        private double[] OrgThirdVel = { 5000.0, 5000.0, 5000.0, 5000.0, 5000.0 };

        public enum eTeachingPosList : int
        {
            WAIT_POS = 0, LOAD_POS, UN_LOAD_POS, WRITE_POS, VERIFY_POS, CAPTURE_POS, TOTAL_SOCKET_TEACHING_COUNT
        };
        public string[] TeachName =
        {
            "WAIT_POS", "LOAD_POS", "UN_LOAD_POS", "WRITE_POS", "VERIFY_POS", "CAPTURE_POS"
        };

        public string teachingPath = "Teach_Socket.yaml";
        public string taskPath = "Task_Socket.yaml";
        public Data.TeachingConfig teachingConfig = new Data.TeachingConfig();


        //public SocketProduct socketProduct = new SocketProduct();
        public SocketMachine()
        {
            this.RunState = OperationState.Stopped;
            this.MachineName = this.GetType().Name;

            FrontSocketX = new MotionControl.MotorAxis((int)MotionControl.MotorSet.eSocketMotorList.FRONT_X,
                    axisName[0], motorType[0], MOTOR_MAX_SPEED[0], AXT_SET_LIMIT[0], AXT_SET_SERVO_ALARM[0], OrgFirstVel[0], OrgSecondVel[0], OrgThirdVel[0],
                    MOTOR_HOME_SENSOR[0], MOTOR_HOME_DIR[0]);
            BackSocketX = new MotionControl.MotorAxis((int)MotionControl.MotorSet.eSocketMotorList.BACK_X,
                axisName[1], motorType[1], MOTOR_MAX_SPEED[1], AXT_SET_LIMIT[1], AXT_SET_SERVO_ALARM[1], OrgFirstVel[1], OrgSecondVel[1], OrgThirdVel[1],
                MOTOR_HOME_SENSOR[1], MOTOR_HOME_DIR[1]);
            BackSocketY = new MotionControl.MotorAxis((int)MotionControl.MotorSet.eSocketMotorList.BACK_Y,
                axisName[2], motorType[2], MOTOR_MAX_SPEED[2], AXT_SET_LIMIT[2], AXT_SET_SERVO_ALARM[2], OrgFirstVel[2], OrgSecondVel[2], OrgThirdVel[2],
                MOTOR_HOME_SENSOR[2], MOTOR_HOME_DIR[2]);
            CamZ_L = new MotionControl.MotorAxis((int)MotionControl.MotorSet.eSocketMotorList.CAMZ_L,
                axisName[3], motorType[3], MOTOR_MAX_SPEED[3], AXT_SET_LIMIT[3], AXT_SET_SERVO_ALARM[3], OrgFirstVel[3], OrgSecondVel[3], OrgThirdVel[3],
                MOTOR_HOME_SENSOR[3], MOTOR_HOME_DIR[3]);
            CamZ_R = new MotionControl.MotorAxis((int)MotionControl.MotorSet.eSocketMotorList.CAMZ_R,
                axisName[4], motorType[4], MOTOR_MAX_SPEED[4], AXT_SET_LIMIT[4], AXT_SET_SERVO_ALARM[4], OrgFirstVel[4], OrgSecondVel[4], OrgThirdVel[4],
                MOTOR_HOME_SENSOR[4], MOTOR_HOME_DIR[4]);



            MotorAxes = new MotionControl.MotorAxis[] { FrontSocketX, BackSocketX, BackSocketY, CamZ_L, CamZ_R };
            MotorCnt = MotorAxes.Length;

            FrontSocketX.setMotorParameter(10.0, 0.1, 0.1, 1000.0);     //초기 셋 다른 곳에서 다시 해줘야될 듯
            BackSocketX.setMotorParameter(10.0, 0.1, 0.1, 1000.0);
            BackSocketY.setMotorParameter(10.0, 0.1, 0.1, 1000.0);
            CamZ_L.setMotorParameter(10.0, 0.1, 0.1, 1000.0);
            CamZ_R.setMotorParameter(10.0, 0.1, 0.1, 1000.0);


            //socketProduct = Data.TaskDataYaml.TaskLoad_Socket(taskPath);

        }
        public override bool TaskSave()
        {
            //bool rtn = Data.TaskDataYaml.TaskSave_Transfer(socketProduct, taskPath);
            //return rtn;
            return false;
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
        public override void MovingStop()
        {
            if (cts != null && !cts.IsCancellationRequested)
            {
                cts.Cancel();
            }
            for (int i = 0; i < MotorAxes.Length; i++)
            {
                MotorAxes[i].MotorBreak = true;
                MotorAxes[i].Stop();
            }
        }
        public override void MoveToPosition(int position)
        {
            //Console.WriteLine($"Transfer name : {TransferX.Name}");
            //Console.WriteLine($"Transfer 이동축 {position} 위치로 이동");
        }

        public override bool IsMoving()
        {
            if (motorAutoThread.GetThreadRun() == true)
            {
                return true;
            }
            return false;
        }
        public override void StopAuto()
        {
            motorAutoThread.Stop();
            MovingStop();

            Console.WriteLine($"[INFO] Socket Run Stop");

        }
        public override bool OriginRun()
        {
            if (motorAutoThread.GetThreadRun() == true)
            {
                //motorAutoThread.Stop();
                return false;
            }
            return true;
        }
        public override bool ReadyRun()
        {
            if (motorAutoThread.GetThreadRun() == true)
            {
                return false;
            }
            return true;
        }
        public override void PauseAuto()
        {
            if (motorAutoThread.GetThreadRun() == true)
            {
                motorAutoThread.Pause();
            }
        }
        public override bool AutoRun()
        {
            bool rtn = true;

            return rtn;
        }
    }
    
}
