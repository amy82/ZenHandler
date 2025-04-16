using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler
{
    public class MotorDefine
    {
        public enum eMotorType : int
        {
            LINEAR,
            STEPING,
            LINEAR_STEP     //PCB Z 모터 세팅은 리니어로 , 구동은 AxmStatusGetCmdPos로
        };

        public enum eMotorAxis : int
        {
            MOTOR_PCB_X = 0, MOTOR_PCB_Y, MOTOR_PCB_Z, 
            MOTOR_PCB_TH, MOTOR_PCB_TX, MOTOR_PCB_TY
        };
    }
}
