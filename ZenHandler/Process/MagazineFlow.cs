using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.Process
{
    public class MagazineFlow
    {
        public MagazineFlow()
        {

        }
        public int AutoReady(int nStep)                 //  운전준비(2000 ~ 3000)
        {
            string szLog = "";
            bool bRtn = false;
            int nRetStep = nStep;
            switch (nStep)
            {
                case 2000:

                    break;
            }
            return nRetStep;
        }
        public int HomeProcess(int nStep)                 //  원점(1000 ~ 2000)
        {
            uint duState = 0;
            bool m_bHomeProc = true;
            bool m_bHomeError = false;
            uint duRetCode = 0;
            string szLog = "";
            bool bRtn = false;
            double dSpeed = 0.0;
            double dAcc = 0.3;
            int nRetStep = nStep;
            switch (nStep)
            {
                case 1000:
                    nRetStep = 1900;
                    break;
                case 1900:
                    Globalo.motionManager.magazineHandler.RunState = OperationState.OriginDone;
                    szLog = $"[ORIGIN] MAGAZINE UNIT 전체 원점 위치 이동 완료 [STEP : {nStep}]";
                    Globalo.LogPrint("ManualControl", szLog);
                    //원점 복귀 완료
                    nRetStep = 2000;
                    break;
                default:
                    //[ORIGIN] STEP ERR
                    nRetStep = -1;
                    break;
            }
            return nRetStep;
        }
    }
}
