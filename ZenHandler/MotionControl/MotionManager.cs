using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZenHandler.MotionControl
{
    public class MotionManager
    {
        public IOController ioController;       //io 동작

        public int m_lAxisCounts = 0;                          // 제어 가능한 축갯수 선언 및 초기화
        public bool bConnected = false;

        public Machine.TransferMachine transferMachine;
        public Machine.LiftModule liftModule;
        #region test
        //test 1
        //test 2
        #endregion

        public MotionManager()
        {
            ioController = new IOController();

            transferMachine = new Machine.TransferMachine();        //TODO: motor , io 모두 설정되고나서 해야될수도
            liftModule = new Machine.LiftModule();
        }
        public void AllMotorStop()
        {
            transferMachine.MovingStop();
        }
        public void MotionClose()
        {
            Axl_Close();
            ioController.Close();
        }
        public bool MotionInit()
        {
            if (CAXL.AxlOpen(7) == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                //초기화 성공
                bConnected = true;

                Axl_Init();
                ioController.DioInit();
            }
            else
            {
                MessageBox.Show("AxlOpen Error!");
                //MessageBox.Show("Motion Intialize Fail..!!");
                //eLogSender("CAxlMotion", "Motion Intialize Fail..!!", Globalo.eMessageName.M_ERROR);
                return false;
            }
            

            return true;
        }
        
        
        private bool Axl_Init()
        {
            int lBoardCount = 0;
            int i = 0;
            // ※ [CAUTION] 아래와 다른 Mot파일(모션 설정파일)을 사용할 경우 경로를 변경하십시요.
            //String szFilePath = "C:\\Program Files\\EzSoftware RM\\EzSoftware\\MotionDefault.mot";
            //++ AXL(AjineXtek Library)을 사용가능하게 하고 장착된 보드들을 초기화합니다.

            

            CAXL.AxlGetBoardCount(ref lBoardCount);

            if (lBoardCount < 1)
            {
                //MessageBox.Show("모션 보드 인식을 실패");
                //eLogSender("CAxlMotion", $"모션 보드 인식을 실패", Globalo.eMessageName.M_ERROR);
                return false;
            }
            //보드개수 확인 AxlGetBoardCount
            //++ 지정한 Mot파일의 설정값들로 모션보드의 설정값들을 일괄변경 적용합니다.
            //if (CAXM.AxmMotLoadParaAll(szFilePath) != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            //{
            //    MessageBox.Show("Mot File Not Found.");
            //}

            AmpDisableAll();

            bool bAxlInit = true;
            for (i = 0; i < (int)MotorSet.eMotorList.MAX_MOTOR_LIST_COUNT; i++)
            {
                if (Axl_Axisconfig(i) != 0)
                {
                    //fail
                    bAxlInit = false;
                }
            }



            return bAxlInit;
        }


        public int Axl_Axisconfig(int axisId)
        {
            int i = 0;
            int nUseAxis = 0;
            int nMaxSpeed = 0;
            uint duRetCode = 0;
            MotorDefine.eMotorType nMotorType;
            //++ 유효한 전체 모션축수를 반환합니다.

            CAXM.AxmInfoGetAxisCount(ref m_lAxisCounts);
            if (m_lAxisCounts < MotorSet.MAX_MOTOR_COUNT)
            {
                // _stprintf_s(szLog, SIZE_OF_1K, _T("모터 드라이브 개수 불일치[%d / %d]"), lAxisCount, (MAX_MOTOR_COUNT * MAX_UNIT_COUNT));
                //eLogSender("CAxlMotion", $"[{m_lAxisCounts} / {(MAX_MOTOR_COUNT)}] 모터 드라이브 개수 불일치", Globalo.eMessageName.M_ERROR);

                return -4;
            }
            double dResol = 0.0;
            string sMotorName = "";
            string[] motorNames = Enum.GetNames(typeof(MotorSet.eMotorList));

            for (i = 0; i < MotorSet.MAX_MOTOR_COUNT; i++)
            {
                if (i == -1)
                {
                    continue; //사용 안하는 축 번호 있을 경우
                }
                nUseAxis = i;
                sMotorName = motorNames[nUseAxis];
                nMotorType = MotorSet.MOTOR_TYPE[i];
                nMaxSpeed = MotorSet.MOTOR_MAX_SPEED[i];



                //duRetCode = CAXM.AxmInterruptSetAxisEnable(nUseAxis, (uint)AXT_USE.DISABLE);      //사용 x
                //if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                //{
                //    eLogSender("CAxlMotion", $"AxmInterruptSetAxisEnable Fail");//, Globalo.eMessageName.M_ERROR);
                //}
                duRetCode = CAXM.AxmInfoIsInvalidAxisNo(nUseAxis);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 드라이브 인식 실패", Globalo.eMessageName.M_ERROR);
                }
                duRetCode = CAXM.AxmMotSetMoveUnitPerPulse(nUseAxis, 1, 1);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 드라이브 전자기어비 초기화 실패", Globalo.eMessageName.M_ERROR);
                }
                duRetCode = CAXM.AxmMotSetMinVel(nUseAxis, 1);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 드라이브 최소 속도 설정 실패", Globalo.eMessageName.M_ERROR);
                }
                duRetCode = CAXM.AxmMotSetAccelUnit(nUseAxis, (uint)AXT_MOTION_ACC_UNIT.SEC);//SEC UNIT_SEC2
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 드라이브 가속도 단위 설정 실패", Globalo.eMessageName.M_ERROR);
                }
                duRetCode = CAXM.AxmMotSetProfileMode(nUseAxis, (uint)AXT_MOTION_PROFILE_MODE.ASYM_S_CURVE_MODE);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 비대칭 S-Curve 가감속 설정 실패", Globalo.eMessageName.M_ERROR);
                }
                duRetCode = CAXM.AxmMotSetAccelJerk(nUseAxis, 30);
                duRetCode = CAXM.AxmMotSetDecelJerk(nUseAxis, 30);

                double dAccelJerk = 0.0;
                double dDecelJerk = 0.0;
                uint dwHomLevel = 0;
                CAXM.AxmMotGetAccelJerk(nUseAxis, ref dAccelJerk);
                CAXM.AxmMotGetDecelJerk(nUseAxis, ref dDecelJerk);

                if (dAccelJerk != 30.0 || dDecelJerk != 30.0)
                {
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 지정축 가속 저크값 설정 실패", Globalo.eMessageName.M_ERROR);
                }

                duRetCode = CAXM.AxmMotSetAbsRelMode(nUseAxis, (uint)AXT_MOTION_ABSREL.POS_ABS_MODE);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 절대 위치 이동 모드 설정 실패", Globalo.eMessageName.M_ERROR);
                }

                duRetCode = CAXM.AxmHomeSetSignalLevel(nUseAxis, (uint)AXT_MOTION_LEVEL_MODE.HIGH);
                duRetCode = CAXM.AxmHomeGetSignalLevel(nUseAxis, ref dwHomLevel);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 HOME LEVEL 설정 실패", Globalo.eMessageName.M_ERROR);
                }

                duRetCode = CAXM.AxmMotSetMaxVel(nUseAxis, nMaxSpeed * dResol);

                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 최고 속도 설정 실패", Globalo.eMessageName.M_ERROR);
                }
                switch (nMotorType)
                {
                    case MotorDefine.eMotorType.LINEAR:     //LINEAR
                        duRetCode = CAXM.AxmSignalSetInpos(nUseAxis, (uint)AXT_MOTION_LEVEL_MODE.HIGH);
                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            //eLogSender("CAxlMotion", $"[{sMotorName}]모터 Inposition 설정 실패", Globalo.eMessageName.M_ERROR);
                        }
                        duRetCode = CAXM.AxmSignalSetStop(nUseAxis, (uint)AXT_MOTION_STOPMODE.EMERGENCY_STOP, (uint)AXT_MOTION_LEVEL_MODE.LOW);
                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            //eLogSender("CAxlMotion", $"[{sMotorName}]모터 비상 정지 설정 실패", Globalo.eMessageName.M_ERROR);
                        }
                        duRetCode = CAXM.AxmSignalSetLimit(nUseAxis, (uint)AXT_MOTION_STOPMODE.EMERGENCY_STOP, (uint)AXT_MOTION_LEVEL_MODE.HIGH, (uint)AXT_MOTION_LEVEL_MODE.HIGH);
                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            //eLogSender("CAxlMotion", $"[{sMotorName}]모터 Limit 감지 정지 설정 실패", Globalo.eMessageName.M_ERROR);
                        }

                        duRetCode = CAXM.AxmSignalSetServoAlarm(nUseAxis, (uint)AXT_MOTION_LEVEL_MODE.HIGH);
                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            //eLogSender("CAxlMotion", $"[{sMotorName}]모터 SERVER Alarm 감지 설정 실패", Globalo.eMessageName.M_ERROR);
                        }
                        break;
                    //
                    //
                    //Steping
                    //
                    case MotorDefine.eMotorType.STEPING:
                        duRetCode = CAXM.AxmSignalSetInpos(nUseAxis, (uint)AXT_MOTION_LEVEL_MODE.UNUSED);
                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            //eLogSender("CAxlMotion", $"[{sMotorName}]모터 Inposition 설정 실패", Globalo.eMessageName.M_ERROR);
                        }
                        duRetCode = CAXM.AxmMotSetPulseOutMethod(nUseAxis, (uint)AXT_MOTION_PULSE_OUTPUT.TwoCcwCwHigh);
                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            //eLogSender("CAxlMotion", $"[{sMotorName}]모터 CW/CCW 설정 실패", Globalo.eMessageName.M_ERROR);
                        }
                        duRetCode = CAXM.AxmSignalSetStop(nUseAxis, (uint)AXT_MOTION_STOPMODE.EMERGENCY_STOP, (uint)AXT_MOTION_LEVEL_MODE.HIGH);
                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            //_stprintf_s
                        }
                        //pcb z만 high , high
                        if (nUseAxis == 0)//(int)MotorSet.eMotorList.PCB_Z)
                        {
                            duRetCode = CAXM.AxmSignalSetLimit(nUseAxis, (uint)AXT_MOTION_STOPMODE.EMERGENCY_STOP, (uint)AXT_MOTION_LEVEL_MODE.HIGH, (uint)AXT_MOTION_LEVEL_MODE.HIGH);
                        }
                        else
                        {
                            duRetCode = CAXM.AxmSignalSetLimit(nUseAxis, (uint)AXT_MOTION_STOPMODE.EMERGENCY_STOP, (uint)AXT_MOTION_LEVEL_MODE.LOW, (uint)AXT_MOTION_LEVEL_MODE.LOW);
                        }

                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            //eLogSender("CAxlMotion", $"[{sMotorName}]모터 비상 정지 설정 실패", Globalo.eMessageName.M_ERROR);
                        }
                        duRetCode = CAXM.AxmMotSetEncInputMethod(nUseAxis, (uint)AXT_MOTION_EXTERNAL_COUNTER_INPUT.ObverseSqr4Mode);
                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            //eLogSender("CAxlMotion", $"[{sMotorName}]모터 4체배 설정 실패", Globalo.eMessageName.M_ERROR);
                        }
                        duRetCode = CAXM.AxmSignalSetServoOnLevel(nUseAxis, (uint)AXT_MOTION_LEVEL_MODE.HIGH);
                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            //eLogSender("CAxlMotion", $"[{sMotorName}]모터 Servo On Level 설정 실패", Globalo.eMessageName.M_ERROR);
                        }
                        duRetCode = CAXM.AxmSignalSetServoAlarm(nUseAxis, (uint)AXT_MOTION_LEVEL_MODE.HIGH);
                        if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                        {
                            //eLogSender("CAxlMotion", $"[{sMotorName}]모터 SERVER Alarm 감지 설정 실패", Globalo.eMessageName.M_ERROR);
                        }

                        break;
                }//switch end
            }//for end
            return 0;
        }

        
        public bool AmpDisableAll()
        {
            int i = 0;
            double dDecel = 0.0;

            for (i = 0; i < MotorSet.MAX_MOTOR_COUNT; i++)
            {
                if (i == -1)
                {
                    continue; //사용 안하는 축 번호 있을 경우
                }
                CAXM.AxmMoveStop(i, dDecel);
                CAXM.AxmSignalServoOn(i, (uint)AXT_USE.DISABLE);

                if (MotorSet.MOTOR_TYPE[i] == MotorDefine.eMotorType.LINEAR)
                {
                    CAXM.AxmSignalServoAlarmReset(i, (uint)AXT_USE.ENABLE);
                }
            }

            

            Thread.Sleep(500);

            for (i = 0; i < MotorSet.MAX_MOTOR_COUNT; i++)
            {
                if (i == -1)
                {
                    continue; //사용 안하는 축 번호 있을 경우
                }
                if (MotorSet.MOTOR_TYPE[i] == MotorDefine.eMotorType.LINEAR)
                {
                    CAXM.AxmSignalServoAlarmReset(i, (uint)AXT_USE.DISABLE);
                }
            }
            return true;
        }
        private void Axl_Close()
        {
            int i;
            // 모든 모터를 정지한다.
            for (i = 0; i < MotorSet.MAX_MOTOR_COUNT; i++)
            {
                if (i == -1)
                {
                    continue; //사용 안하는 축 번호 있을 경우
                }
                CAXM.AxmMoveSStop(i);
            }

            CAXL.AxlClose();
        }
        
    }
}
