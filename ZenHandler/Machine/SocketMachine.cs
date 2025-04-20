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

        private MotorDefine.eMotorType[] motorType = { MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR, MotorDefine.eMotorType.LINEAR };
        private AXT_MOTION_LEVEL_MODE[] AXT_SET_LIMIT = { AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW };
        private AXT_MOTION_LEVEL_MODE[] AXT_SET_SERVO_ALARM = { AXT_MOTION_LEVEL_MODE.HIGH, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW, AXT_MOTION_LEVEL_MODE.LOW };
        private AXT_MOTION_HOME_DETECT[] MOTOR_HOME_SENSOR = { AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor, AXT_MOTION_HOME_DETECT.HomeSensor };
        private AXT_MOTION_MOVE_DIR[] MOTOR_HOME_DIR = { AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW, AXT_MOTION_MOVE_DIR.DIR_CW };

        private static double[] MaxSpeeds = { 200.0, 200.0, 200.0, 200.0, 200.0 };
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
            int i = 0;
            this.RunState = OperationState.Stopped;
            this.MachineName = this.GetType().Name;

            MotorAxes = new MotionControl.MotorAxis[] { FrontSocketX, BackSocketX, BackSocketY, CamZ_L, CamZ_R };
            MotorCnt = MotorAxes.Length;

            for (i = 0; i < MotorCnt; i++)
            {
                int index = (int)MotionControl.MotorSet.ValidSocketMotors[i];
                MotorAxes[i] = new MotionControl.MotorAxis(index,
                axisName[i], motorType[i], MaxSpeeds[i], AXT_SET_LIMIT[i], AXT_SET_SERVO_ALARM[i], OrgFirstVel[i], OrgSecondVel[i], OrgThirdVel[i],
                MOTOR_HOME_SENSOR[i], MOTOR_HOME_DIR[i]);


                //초기 셋 다른 곳에서 다시 해줘야될 듯
                MotorAxes[i].setMotorParameter(10.0, 0.1, 0.1, 1000.0);//(double vel , double acc , double dec , double resol)
            }


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
            if (CancelToken != null && !CancelToken.IsCancellationRequested)
            {
                CancelToken.Cancel();
            }
            for (int i = 0; i < MotorAxes.Length; i++)
            {
                MotorAxes[i].MotorBreak = true;
                MotorAxes[i].Stop();
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

            Console.WriteLine($"[INFO] Socket Run Stop");

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
            return true;
        }
        public override void PauseAuto()
        {
            if (AutoUnitThread.GetThreadRun() == true)
            {
                AutoUnitThread.Pause();
            }
        }
        public override bool AutoRun()
        {
            bool rtn = true;

            return rtn;
        }
    }
    
}
