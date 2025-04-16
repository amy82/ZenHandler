using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZenHandler.Process
{
    public class TransferFlow
    {
        public TransferFlow()
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
                    Console.WriteLine("[ORIGIN] TRANSFER START");
                    nRetStep = 1900;
                    break;
                case 1900:
                    Thread.Sleep(5000);
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
        public int AutoReady(int nStep)					//  운전준비(2000 ~ 3000)
        {
            int nRetStep = nStep;
            switch (nStep)
            {
                case 2000:

                    break;
                case 1900:

                    break;
            }
            return nRetStep;
        }
        public int Auto_PCBLoading(int nStep)       //PCB 로딩(3000 ~ 40000)
        {
            int nRetStep = nStep;
            switch (nStep)
            {
                case 3000:

                    break;
                case 3900:

                    break;
            }
            return nRetStep;
        }
    }
}