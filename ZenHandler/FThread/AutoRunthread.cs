using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZenHandler.FThread
{
    public class AutoRunthread : BaseThread
    {
        //public event delLogSender eLogSender;       //외부에서 호출할때 사용
        


        private Process.PcbProcess RunProcess = new Process.PcbProcess();
        private Process.ReadyProcess readyProcess = new Process.ReadyProcess();
        public AutoRunthread()
        {
            //thread = null;
            this.name = "AutoRunthread";
        }
        protected override void ThreadInit()
        {

        }
        protected override void ThreadRun()
        {
            if (Globalo.taskWork.m_nCurrentStep >= Globalo.taskWork.m_nStartStep && Globalo.taskWork.m_nCurrentStep < Globalo.taskWork.m_nEndStep)
            {
                //if (ProgramState.CurrentState == OperationState.Stopped)
                //{
                //    //Stop Auto Process
                //    Globalo.operationPanel.StopAutoProcess();
                //    return;
                //}
                //// 원점 복귀
                //if (Globalo.taskWork.m_nCurrentStep >= 10000 && Globalo.taskWork.m_nCurrentStep < 20000)
                //{
                //    Globalo.taskWork.m_nCurrentStep = readyProcess.HomeProcess(Globalo.taskWork.m_nCurrentStep);
                //}

                //if (Globalo.taskWork.m_nCurrentStep >= 20000 && Globalo.taskWork.m_nCurrentStep < 30000)
                //{
                //    Globalo.taskWork.m_nCurrentStep = readyProcess.AutoReady(Globalo.taskWork.m_nCurrentStep, cts.Token);
                //}
                if (Globalo.taskWork.m_nCurrentStep >= 30000 && Globalo.taskWork.m_nCurrentStep < 40000)
                {
                    Globalo.taskWork.m_nCurrentStep = RunProcess.Auto_Loading(Globalo.taskWork.m_nCurrentStep);
                }
                else if (Globalo.taskWork.m_nCurrentStep >= 40000 && Globalo.taskWork.m_nCurrentStep < 50000)
                {
                    Globalo.taskWork.m_nCurrentStep = RunProcess.Auto_Mes_Secenario(Globalo.taskWork.m_nCurrentStep);
                }
                else if (Globalo.taskWork.m_nCurrentStep >= 50000 && Globalo.taskWork.m_nCurrentStep < 60000)
                {
                    Globalo.taskWork.m_nCurrentStep = RunProcess.Auto_EEpromVerify(Globalo.taskWork.m_nCurrentStep);
                }
                else if (Globalo.taskWork.m_nCurrentStep >= 60000 && Globalo.taskWork.m_nCurrentStep < 70000)
                {
                    Globalo.taskWork.m_nCurrentStep = RunProcess.Auto_Final(Globalo.taskWork.m_nCurrentStep);
                }
            }
            else if (Globalo.taskWork.m_nCurrentStep < 0)
            {
                if (cts.Token.IsCancellationRequested)
                {
                    //정지 요청시
                    Stop();
                }
                else
                {
                    //일시 정지 요청시
                    Globalo.operationPanel.PauseAutoProcess();
                }
            }
            else
            {
                //Globalo.MainForm.StopAutoProcess();
                // TODO:   STOP 버튼 함수 살려야된다. 250219 왜? 250220
                //cts.Cancel();
                Stop();
            }
        }

    }
}
