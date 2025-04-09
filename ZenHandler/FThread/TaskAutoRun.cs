using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Drawing;

namespace ApsMotionControl.FThread
{
    public class TaskAutoRun
    {
        private CancellationTokenSource _cancellationTokenSource;
        private Task _task;
        private bool _isPaused = false;
        private Process.PcbProcess RunProcess = new Process.PcbProcess();
        private Process.ReadyProcess readyProcess = new Process.ReadyProcess();
        public TaskAutoRun()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public bool Start()
        {
            if (_task != null && !_task.IsCompleted)
            {
                Globalo.LogPrint("PcbPrecess", "작업이 이미 실행 중입니다.");
                return false;
            }
            if (_cancellationTokenSource != null)
            {
                Stop(); // 기존 작업이 있다면 먼저 종료
            }

            _cancellationTokenSource = new CancellationTokenSource();
            _task = Task.Run(() => RunTask(_cancellationTokenSource.Token), _cancellationTokenSource.Token);
            if(_task.Status == TaskStatus.Faulted || _task.Status == TaskStatus.Canceled)
            {
                return false;
            }
            return true;
        }

        private async Task RunTask(CancellationToken token)
        {
            Globalo.LogPrint("PcbPrecess", "Auto Run Task Start!");
            try
            {
                while (!token.IsCancellationRequested)  //취소 요청이 발생했는지 확인하는 속성
                {
                    if (_isPaused)
                    {
                        await Task.Delay(100); // 일시 정지 상태에서 대기
                        continue;
                    }
                    if (Globalo.taskWork.m_nCurrentStep >= Globalo.taskWork.m_nStartStep && Globalo.taskWork.m_nCurrentStep < Globalo.taskWork.m_nEndStep)
                    {
                        if (ProgramState.CurrentState == OperationState.Stopped)
                        {
                            //Stop Auto Process
                            Globalo.MainForm.StopAutoProcess();
                            return;
                        }
                        if (Globalo.taskWork.m_nCurrentStep >= 20000 && Globalo.taskWork.m_nCurrentStep < 30000)
                        {
                            Globalo.taskWork.m_nCurrentStep = readyProcess.AutoReady(Globalo.taskWork.m_nCurrentStep, token);
                        }


                    }
                    else if (Globalo.taskWork.m_nCurrentStep == -1)
                    {
                        Globalo.LogPrint("RunProcess", "Auto Run Task Stop!");
                        Globalo.MainForm.StopAutoProcess();
                    }
                    else if (Globalo.taskWork.m_nCurrentStep < 0)
                    {
                        Globalo.LogPrint("RunProcess", "Auto Run Task Pause!");
                        Globalo.MainForm.PauseAutoProcess();
                    }
                    else
                    {
                        Globalo.LogPrint("RunProcess", "Auto Run Task Stop!!");
                        _cancellationTokenSource.Cancel();
                    }
                    await Task.Delay(10, token);
                }
            }
            catch (OperationCanceledException)
            {
                Globalo.LogPrint("RunProcess", "Task Cancel");
            }

            Globalo.LogPrint("RunProcess", "Task End!");
            if(ProgramState.CurrentState != OperationState.PreparationComplete)
            {
                Globalo.MainForm.StopAutoProcess();
            }
            

        }

        // 작업 일시 정지
        public void Pause()
        {
            if (_isPaused)
            {
                Globalo.LogPrint("RunProcess", "이미 일시 정지 상태입니다.");
                return;
            }

            Globalo.LogPrint("RunProcess", "작업 일시 정지...");
            _isPaused = true;
        }

        // 작업 재개
        public void Resume()
        {
            if (!_isPaused)
            {
                Globalo.LogPrint("RunProcess", "작업이 이미 실행 중입니다.");
                return;
            }
            Globalo.LogPrint("RunProcess", "작업 재개...");
            _isPaused = false;
        }

        // 작업 중단
        public bool Stop()
        {
            bool completedInTime = false;
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
            }
            
            if (_task != null)
            {
                try
                {
                    completedInTime = _task.Wait(300); // 작업이 완료될 때까지 동기적으로 대기
                }
                catch (AggregateException ex)
                {
                    Globalo.LogPrint("RunProcess", "AggregateException");
                    // Task가 취소되었을 경우 정상적인 예외 처리

                    if (ex.InnerException is OperationCanceledException)
                    {
                        // 정상적인 취소 예외이므로 무시
                    }
                    else
                    {
                        throw; // 그 외 예외는 다시 던지기
                    }
                }
                finally
                {
                    _task = null;
                    
                }
            }
            Globalo.LogPrint("PcbPrecess", "작업 중단...");


            return completedInTime;
        }
    }
}
