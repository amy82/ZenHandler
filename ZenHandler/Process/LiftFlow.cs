using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.Process
{
    public class LiftFlow
    {
        public LiftFlow()
        {

        }
        public int HomeProcess(int nStep)                 //  원점(1000 ~ 2000)
        {
            //string szLog = "";
            //uint duState = 0;

            //bool bRtn = false;
            //int nLensAxis = 0;
            //bool m_bHomeProc = true;
            //bool m_bHomeError = false;
            //uint duRetCode = 0;
            //double dAcc = 0.3;
            //int i = 0;

            int nRetStep = nStep;
            switch (nStep)
            {
                case 1000:
                    nRetStep = 9000;
                    break;
                case 1900:

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
