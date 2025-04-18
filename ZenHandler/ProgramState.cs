using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler
{
    public enum OperationState
    {
        Stopped,       // 정지중
        Paused,
        AutoRunning,   // 자동운전중
        Preparing,      // 운전준비중
        PreparationComplete,   //운전준비 완료
        ManualTesting,   //수동 검사 , Manualthread 진행중
        OriginRunning,      // 원점
        OriginDone
    }
    // 글로벌 상태 변수를 관리하는 클래스 정의
    public static class ProgramState
    {
        public static string PG_TITLE = "EEprom Verify";
        public enum eRunMode : int
        {
            OPERATOR , ENGINEER
        };
        // 현재 상태를 나타내는 static 변수


        public static OperationState CurrentState { get; set; } = OperationState.Stopped;       //Unit 별로 나눠야된다.



        public static eRunMode CurrentRunMode { get; set; } = eRunMode.OPERATOR;



        public static bool STATE_DRIVER_CONNECT = false;
        public static bool STATE_CLINET_CONNECT = false;



        public static bool NORINDA_MODE = true;    //TODO: 배포전 변경해야된다.


        public static bool ON_LINE_OPENCV_IMAGE = false;      //Matorx 사용없이 영상 출력

        public static bool ON_LINE_MIL = false;      //true
        public static bool ON_LINE_CAM = false;      //true



        public static bool ON_LINE_MOTOR = false;    //true

        //public static bool ON_LINE_GRABBER = false;      //true

    }
}
