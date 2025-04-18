using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZenHandler.FThread
{
    public class ManualThread : BaseThread
    {
        public enum eManualType : int
        {
            M_EMPTY, M_EEPROM_READ = 1, M_CCD_OPEN = 2, M_CCD_CLOSE = 3, M_CCD_START = 4
                , M_MANUAL_VERIFY = 10
        };
        private Process.ManualProcess manualProcess = new Process.ManualProcess();
        private eManualType nType = eManualType.M_EMPTY;

        public ManualThread()
        {
            this.name = "ManualThread";
        }
        public void runfn(eManualType _nType = eManualType.M_EMPTY)
        {
            nType = _nType;
            base.Start();
        }
        protected override void ThreadInit()
        {
            //ProgramState.CurrentState = OperationState.ManualTesting;
        }
        protected override void ThreadRun()
        {
            Console.WriteLine("추가 작업 실행 중...");
            if (nType == eManualType.M_EMPTY)
            {
                Console.WriteLine("ManualThread ProcessRun out.");
                cts.Cancel();
            }
            Console.WriteLine("ManualThread ProcessRun Run.");

            if (nType == eManualType.M_EEPROM_READ)
            {
                manualProcess.Manual_EEpromRead();
            }
            if (nType == eManualType.M_CCD_OPEN)     //ccd open
            {
                int errorCode = Globalo.mLaonGrabberClass.OpenDevice();

                if (errorCode == 1)
                {
                    Globalo.tcpManager.SendAlarmReport("1003");     //LAONBOARD CONNECT FAIL
                }
                else
                {
                    Globalo.tcpManager.SendAlarmReport("1001");
                }
            }
            if (nType == eManualType.M_CCD_CLOSE)     //ccd Close
            {
                Globalo.mLaonGrabberClass.CloseDevice();
            }
            if (nType == eManualType.M_CCD_START)     //ccd open -> start
            {
                manualProcess.Manual_CCdRun();
            }
            if (nType == eManualType.M_MANUAL_VERIFY)
            {
                //EEPROM VERIFY

                manualProcess.Manual_Verify();
            }


            cts.Cancel();
            Console.WriteLine("ManualThread stopped safely.");


            //ProgramState.CurrentState = OperationState.Stopped;
        }
        //protected override void ProcessRun()
        //{
           // while (!cts.Token.IsCancellationRequested)
            //{
                //if(nType == -1)
                //{
                //    Console.WriteLine("ManualThread ProcessRun out.");
                //    cts.Cancel();
                //    break;
                //}
                //Console.WriteLine("ManualThread ProcessRun Run.");
                //if(nType == 1)
                //{
                //    Globalo.mCCdPanel.EEpromRead();
                //}

                //for (int i = 0; i < 50; i++)  // 긴 반복문
                //{
                //    if (token.IsCancellationRequested)  // 중간에 취소 요청 확인
                //    {
                //        Console.WriteLine($"ManualThread token.IsCancellationRequested");
                //        break;
                //    }

                //    Console.WriteLine($"ManualThread Processing {i}");
                //    Thread.Sleep(300); // 작업 시간 가정
                //}


                //cts.Cancel();

            //}

            //nType = -1;
            //Console.WriteLine("ManualThread stopped safely.");
        //}
    }
}
