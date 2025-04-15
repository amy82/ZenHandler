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
        public Machine.MagazineHandler magazineHandler;
        public Machine.LiftModule liftModule;
        #region test
        //test 1
        //test 2
        #endregion

        public MotionManager()
        {
            ioController = new IOController();

            transferMachine = new Machine.TransferMachine();        //TODO: motor , io 모두 설정되고나서 해야될수도
            magazineHandler = new Machine.MagazineHandler();
            liftModule = new Machine.LiftModule();
        }

        public void AllMotorParameterSet()
        {
            transferMachine.MotorDataSet();
            magazineHandler.MotorDataSet();
            liftModule.MotorDataSet();
        }
        public void AllMotorStop()
        {
            if (ProgramState.ON_LINE_MOTOR)
            {
                transferMachine.MovingStop();
                magazineHandler.MovingStop();
                liftModule.MovingStop();

            }
           
        }
        public void MotionClose()
        {
            Axl_Close();
            ioController.Close();
        }
        public bool MotionInit()
        {
            bool bAxlInit = true;
            if (CAXL.AxlOpen(7) == (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                //초기화 성공
                bConnected = true;

                Axl_Init();
                ioController.DioInit();

                //여기서 Machine 초기화

                

                int length = transferMachine.MotorAxes.Length;
                for (int i = 0; i < length; i++)
                {
                    if (New_Axisconfig(transferMachine.MotorAxes[i]) > 0)   //0 = ok
                    {
                        //fail
                        bAxlInit = false;
                    }
                }

                //Socket Machine

                //Magazine Machine

                //Lift Machine
                //length = liftModule.MotorAxes.Length;
            }
            else
            {
                bAxlInit = false;
                Globalo.LogPrint("", "Motion Intialize Fail..!!", Globalo.eMessageName.M_ERROR);
            }
            

            return bAxlInit;
        }
        
        
        private bool Axl_Init()
        {
            int lBoardCount = 0;
            int i = 0;
            int SetTotalMotorCnt = MotorSet.MAX_MOTOR_COUNT;
            // ※ [CAUTION] 아래와 다른 Mot파일(모션 설정파일)을 사용할 경우 경로를 변경하십시요.
            //String szFilePath = "C:\\Program Files\\EzSoftware RM\\EzSoftware\\MotionDefault.mot";
            //++ AXL(AjineXtek Library)을 사용가능하게 하고 장착된 보드들을 초기화합니다.



            CAXL.AxlGetBoardCount(ref lBoardCount);

            if (lBoardCount < 1)
            {
                Globalo.LogPrint("", "Motion board recognition failure", Globalo.eMessageName.M_ERROR);
                return false;
            }
            Globalo.LogPrint("", "Motion board recognition completed");

            //++ 유효한 전체 모션축수를 반환합니다.

            CAXM.AxmInfoGetAxisCount(ref m_lAxisCounts);

            
            if (m_lAxisCounts < SetTotalMotorCnt)
            {
                Globalo.LogPrint("", $"Motor drive number mismatch[{m_lAxisCounts}/{SetTotalMotorCnt}]", Globalo.eMessageName.M_ERROR);
                return false;
            }

            AmpDisableAll();



            //bool bAxlInit = true;

            //for (i = 0; i < (int)MotorSet.eMotorList.MAX_MOTOR_LIST_COUNT; i++)
            //{
            //    if (Axl_Axisconfig(i) != 0)
            //    {
            //        //fail
            //        bAxlInit = false;
            //    }
            //}



            return true;
        }

        public int New_Axisconfig(MotionControl.MotorAxis motorAxis)
        {
            int i = 0;
            uint duRetCode = 0;
            int nFailCount = 0;
            string logstr = "";

            string sMotorName = motorAxis.Name;
            int nUseAxis = motorAxis.m_lAxisNo;
            double nMaxSpeed = motorAxis.MaxSpeed;
            double dResol = motorAxis.Resolution;
            MotorDefine.eMotorType nMotorType = motorAxis.Type;
            AXT_MOTION_LEVEL_MODE setLimit = motorAxis.AxtSetLimit;
            AXT_MOTION_LEVEL_MODE setServoAlarm = motorAxis.AxtSetServoAlarm;

            duRetCode = CAXM.AxmInfoIsInvalidAxisNo(nUseAxis);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                nFailCount++;
                logstr = "AxmInfoIsInvalidAxisNo Fail";
                Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                //해당축이 사용할 수 있는 축인지 확인한다
                //eLogSender("CAxlMotion", $"[{sMotorName}]모터 드라이브 인식 실패", Globalo.eMessageName.M_ERROR);
            }

            duRetCode = CAXM.AxmMotSetMoveUnitPerPulse(nUseAxis, 1, 1);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                nFailCount++;
                logstr = "AxmMotSetMoveUnitPerPulse Fail";
                Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                //Unit / Pulse = 1 : 1이면 pulse/ sec 초당 펄스수가 되는데 4500 rpm에 맞추고 싶다면 4500 / 60초 는 75회전 / 1초가 된다.
                //모터가 1회전에 몇 펄스인지 알아야 된다. 이것은 Encoder에 Z상을 검색해보면 알수있다.
                //만약 1회전:1800 펄스라고 가정하면 75 x 1800 = 135000 펄스가 필요하게 된다.


                //eLogSender("CAxlMotion", $"[{sMotorName}]모터 드라이브 전자기어비 초기화 실패", Globalo.eMessageName.M_ERROR);
            }
            duRetCode = CAXM.AxmMotSetMinVel(nUseAxis, 1);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                nFailCount++;
                logstr = "AxmMotSetMinVel Fail";
                Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                //특별한 경우가 아니면 이 함수를 사용할 필요는 없지만 기본적으로 초기속도는 1로 설정된다.
                //스텝모터를 사용할 경우 기동 초기속도를 설정하여 탈조 현상을 없앨 수 있다.

                //eLogSender("CAxlMotion", $"[{sMotorName}]모터 드라이브 최소 속도 설정 실패", Globalo.eMessageName.M_ERROR);
            }
            duRetCode = CAXM.AxmMotSetAccelUnit(nUseAxis, (uint)AXT_MOTION_ACC_UNIT.SEC);//SEC UNIT_SEC2
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                nFailCount++;
                logstr = "AxmMotSetAccelUnit Fail";
                Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                //eLogSender("CAxlMotion", $"[{sMotorName}]모터 드라이브 가속도 단위 설정 실패", Globalo.eMessageName.M_ERROR);
            }
            duRetCode = CAXM.AxmMotSetProfileMode(nUseAxis, (uint)AXT_MOTION_PROFILE_MODE.ASYM_S_CURVE_MODE);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                nFailCount++;
                logstr = "AxmMotSetProfileMode Fail";
                Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
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
                nFailCount++;
                logstr = "AxmMotSetDecelJerk Fail";
                Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                //eLogSender("CAxlMotion", $"[{sMotorName}]모터 지정축 가속 저크값 설정 실패", Globalo.eMessageName.M_ERROR);
            }

            duRetCode = CAXM.AxmMotSetAbsRelMode(nUseAxis, (uint)AXT_MOTION_ABSREL.POS_ABS_MODE);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                nFailCount++;
                logstr = "AxmMotSetAbsRelMode Fail";
                Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                //eLogSender("CAxlMotion", $"[{sMotorName}]모터 절대 위치 이동 모드 설정 실패", Globalo.eMessageName.M_ERROR);
            }

            duRetCode = CAXM.AxmHomeSetSignalLevel(nUseAxis, (uint)AXT_MOTION_LEVEL_MODE.HIGH);
            duRetCode = CAXM.AxmHomeGetSignalLevel(nUseAxis, ref dwHomLevel);
            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                nFailCount++;
                logstr = "AxmHomeSetSignalLevel Fail";
                Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                //eLogSender("CAxlMotion", $"[{sMotorName}]모터 HOME LEVEL 설정 실패", Globalo.eMessageName.M_ERROR);
            }

            duRetCode = CAXM.AxmMotSetMaxVel(nUseAxis, nMaxSpeed * dResol);

            if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
            {
                nFailCount++;
                logstr = "AxmMotSetMaxVel Fail";
                Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                //eLogSender("CAxlMotion", $"[{sMotorName}]모터 최고 속도 설정 실패", Globalo.eMessageName.M_ERROR);
            }

            if (nMotorType == MotorDefine.eMotorType.LINEAR)
            {

                duRetCode = CAXM.AxmSignalSetInpos(nUseAxis, (uint)AXT_MOTION_LEVEL_MODE.HIGH);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    nFailCount++;
                    logstr = "[LINEAR] AxmSignalSetInpos Fail";
                    Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 Inposition 설정 실패", Globalo.eMessageName.M_ERROR);
                }
                duRetCode = CAXM.AxmSignalSetStop(nUseAxis, (uint)AXT_MOTION_STOPMODE.EMERGENCY_STOP, (uint)AXT_MOTION_LEVEL_MODE.LOW);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    nFailCount++;
                    logstr = "[LINEAR] AxmSignalSetStop Fail";
                    Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 비상 정지 설정 실패", Globalo.eMessageName.M_ERROR);
                }

                duRetCode = CAXM.AxmSignalSetLimit(nUseAxis, (uint)AXT_MOTION_STOPMODE.EMERGENCY_STOP, (uint)setLimit, (uint)setLimit);


                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    nFailCount++;
                    logstr = "[LINEAR] AxmSignalSetLimit Fail";
                    Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 Limit 감지 정지 설정 실패", Globalo.eMessageName.M_ERROR);
                }

                duRetCode = CAXM.AxmSignalSetServoAlarm(nUseAxis, (uint)setServoAlarm);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    nFailCount++;
                    logstr = "[LINEAR] AxmSignalSetServoAlarm Fail";
                    Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 SERVER Alarm 감지 설정 실패", Globalo.eMessageName.M_ERROR);
                }

            }
            else if (nMotorType == MotorDefine.eMotorType.STEPING)
            {
                duRetCode = CAXM.AxmSignalSetInpos(nUseAxis, (uint)AXT_MOTION_LEVEL_MODE.UNUSED);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    nFailCount++;
                    logstr = "[STEPING] AxmSignalSetServoAlarm Fail";
                    Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 Inposition 설정 실패", Globalo.eMessageName.M_ERROR);
                }
                duRetCode = CAXM.AxmMotSetPulseOutMethod(nUseAxis, (uint)AXT_MOTION_PULSE_OUTPUT.TwoCcwCwHigh);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    nFailCount++;
                    logstr = "[STEPING] AxmMotSetPulseOutMethod Fail";
                    Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 CW/CCW 설정 실패", Globalo.eMessageName.M_ERROR);
                }
                duRetCode = CAXM.AxmSignalSetStop(nUseAxis, (uint)AXT_MOTION_STOPMODE.EMERGENCY_STOP, (uint)AXT_MOTION_LEVEL_MODE.HIGH);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    nFailCount++;
                    logstr = "[STEPING] AxmSignalSetStop Fail";
                    Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                    //_stprintf_s
                }
                //pcb z만 high , high

                duRetCode = CAXM.AxmSignalSetLimit(nUseAxis, (uint)AXT_MOTION_STOPMODE.EMERGENCY_STOP, (uint)setLimit, (uint)setLimit);

                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    nFailCount++;
                    logstr = "[STEPING] AxmSignalSetLimit Fail";
                    Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 비상 정지 설정 실패", Globalo.eMessageName.M_ERROR);
                }
                duRetCode = CAXM.AxmMotSetEncInputMethod(nUseAxis, (uint)AXT_MOTION_EXTERNAL_COUNTER_INPUT.ObverseSqr4Mode);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    nFailCount++;
                    logstr = "[STEPING] AxmMotSetEncInputMethod Fail";
                    Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 4체배 설정 실패", Globalo.eMessageName.M_ERROR);
                }
                duRetCode = CAXM.AxmSignalSetServoOnLevel(nUseAxis, (uint)AXT_MOTION_LEVEL_MODE.HIGH);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    nFailCount++;
                    logstr = "[STEPING] AxmSignalSetServoOnLevel Fail";
                    Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 Servo On Level 설정 실패", Globalo.eMessageName.M_ERROR);
                }
                duRetCode = CAXM.AxmSignalSetServoAlarm(nUseAxis, (uint)setServoAlarm);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    nFailCount++;
                    logstr = "[STEPING] AxmSignalSetServoAlarm Fail";
                    Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);

                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 SERVER Alarm 감지 설정 실패", Globalo.eMessageName.M_ERROR);
                }
            }
            return nFailCount++; ;
        }
        public int Axl_Axisconfig(int axisId)
        {
            int i = 0;
            int nUseAxis = 0;
            int nMaxSpeed = 0;
            uint duRetCode = 0;
            int nFailCount = 0;
            MotorDefine.eMotorType nMotorType;


           
            double dResol = 0.0;
            string sMotorName = "";
            string[] motorNames = Enum.GetNames(typeof(MotorSet.eMotorList));
            string logstr = "";

            double[] resolList = { 2000.0, 2000.0, 10000.0 };
            for (i = 0; i < MotorSet.MAX_MOTOR_COUNT; i++)          //Axl_Axisconfig
            {
                if (i == -1)
                {
                    continue; //사용 안하는 축 번호 있을 경우 사용
                }
                nUseAxis = i;
                sMotorName = motorNames[nUseAxis];
                nMotorType = MotorSet.MOTOR_TYPE[i];
                nMaxSpeed = MotorSet.MOTOR_MAX_SPEED[i];
                dResol = resolList[i];



                //duRetCode = CAXM.AxmInterruptSetAxisEnable(nUseAxis, (uint)AXT_USE.DISABLE);      //사용 x
                //if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                //{
                //    eLogSender("CAxlMotion", $"AxmInterruptSetAxisEnable Fail");//, Globalo.eMessageName.M_ERROR);
                //}

                duRetCode = CAXM.AxmInfoIsInvalidAxisNo(nUseAxis);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    nFailCount++;
                    logstr = "AxmInfoIsInvalidAxisNo Fail";
                    Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                    //해당축이 사용할 수 있는 축인지 확인한다
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 드라이브 인식 실패", Globalo.eMessageName.M_ERROR);
                }

                duRetCode = CAXM.AxmMotSetMoveUnitPerPulse(nUseAxis, 1, 1);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    nFailCount++;
                    logstr = "AxmMotSetMoveUnitPerPulse Fail";
                    Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                    //Unit / Pulse = 1 : 1이면 pulse/ sec 초당 펄스수가 되는데 4500 rpm에 맞추고 싶다면 4500 / 60초 는 75회전 / 1초가 된다.
                    //모터가 1회전에 몇 펄스인지 알아야 된다. 이것은 Encoder에 Z상을 검색해보면 알수있다.
                    //만약 1회전:1800 펄스라고 가정하면 75 x 1800 = 135000 펄스가 필요하게 된다.


                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 드라이브 전자기어비 초기화 실패", Globalo.eMessageName.M_ERROR);
                }
                duRetCode = CAXM.AxmMotSetMinVel(nUseAxis, 1);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    nFailCount++;
                    logstr = "AxmMotSetMinVel Fail";
                    Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                    //특별한 경우가 아니면 이 함수를 사용할 필요는 없지만 기본적으로 초기속도는 1로 설정된다.
                    //스텝모터를 사용할 경우 기동 초기속도를 설정하여 탈조 현상을 없앨 수 있다.

                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 드라이브 최소 속도 설정 실패", Globalo.eMessageName.M_ERROR);
                }
                duRetCode = CAXM.AxmMotSetAccelUnit(nUseAxis, (uint)AXT_MOTION_ACC_UNIT.SEC);//SEC UNIT_SEC2
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    nFailCount++;
                    logstr = "AxmMotSetAccelUnit Fail";
                    Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 드라이브 가속도 단위 설정 실패", Globalo.eMessageName.M_ERROR);
                }
                duRetCode = CAXM.AxmMotSetProfileMode(nUseAxis, (uint)AXT_MOTION_PROFILE_MODE.ASYM_S_CURVE_MODE);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    nFailCount++;
                    logstr = "AxmMotSetProfileMode Fail";
                    Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
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
                    nFailCount++;
                    logstr = "AxmMotSetDecelJerk Fail";
                    Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 지정축 가속 저크값 설정 실패", Globalo.eMessageName.M_ERROR);
                }

                duRetCode = CAXM.AxmMotSetAbsRelMode(nUseAxis, (uint)AXT_MOTION_ABSREL.POS_ABS_MODE);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    nFailCount++;
                    logstr = "AxmMotSetAbsRelMode Fail";
                    Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 절대 위치 이동 모드 설정 실패", Globalo.eMessageName.M_ERROR);
                }

                duRetCode = CAXM.AxmHomeSetSignalLevel(nUseAxis, (uint)AXT_MOTION_LEVEL_MODE.HIGH);
                duRetCode = CAXM.AxmHomeGetSignalLevel(nUseAxis, ref dwHomLevel);
                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    nFailCount++;
                    logstr = "AxmHomeSetSignalLevel Fail";
                    Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 HOME LEVEL 설정 실패", Globalo.eMessageName.M_ERROR);
                }

                duRetCode = CAXM.AxmMotSetMaxVel(nUseAxis, nMaxSpeed * dResol);

                if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                {
                    nFailCount++;
                    logstr = "AxmMotSetMaxVel Fail";
                    Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                    //eLogSender("CAxlMotion", $"[{sMotorName}]모터 최고 속도 설정 실패", Globalo.eMessageName.M_ERROR);
                }

                if (nMotorType == MotorDefine.eMotorType.LINEAR) 
                {

                    duRetCode = CAXM.AxmSignalSetInpos(nUseAxis, (uint)AXT_MOTION_LEVEL_MODE.HIGH);
                    if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                    {
                        nFailCount++;
                        logstr = "[LINEAR] AxmSignalSetInpos Fail";
                        Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                        //eLogSender("CAxlMotion", $"[{sMotorName}]모터 Inposition 설정 실패", Globalo.eMessageName.M_ERROR);
                    }
                    duRetCode = CAXM.AxmSignalSetStop(nUseAxis, (uint)AXT_MOTION_STOPMODE.EMERGENCY_STOP, (uint)AXT_MOTION_LEVEL_MODE.LOW);
                    if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                    {
                        nFailCount++;
                        logstr = "[LINEAR] AxmSignalSetStop Fail";
                        Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                        //eLogSender("CAxlMotion", $"[{sMotorName}]모터 비상 정지 설정 실패", Globalo.eMessageName.M_ERROR);
                    }

                    if(i == 0 || i == 2)
                    {
                        duRetCode = CAXM.AxmSignalSetLimit(nUseAxis, (uint)AXT_MOTION_STOPMODE.EMERGENCY_STOP, (uint)AXT_MOTION_LEVEL_MODE.LOW, (uint)AXT_MOTION_LEVEL_MODE.LOW);
                    }
                    else
                    {
                        duRetCode = CAXM.AxmSignalSetLimit(nUseAxis, (uint)AXT_MOTION_STOPMODE.EMERGENCY_STOP, (uint)AXT_MOTION_LEVEL_MODE.HIGH, (uint)AXT_MOTION_LEVEL_MODE.HIGH);
                    }
                    


                    if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                    {
                        nFailCount++;
                        logstr = "[LINEAR] AxmSignalSetLimit Fail";
                        Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                        //eLogSender("CAxlMotion", $"[{sMotorName}]모터 Limit 감지 정지 설정 실패", Globalo.eMessageName.M_ERROR);
                    }
                    if (i == 2)
                    {
                        duRetCode = CAXM.AxmSignalSetServoAlarm(nUseAxis, (uint)AXT_MOTION_LEVEL_MODE.LOW);
                    }
                    else
                    {
                        duRetCode = CAXM.AxmSignalSetServoAlarm(nUseAxis, (uint)AXT_MOTION_LEVEL_MODE.HIGH);
                    }
                    if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                    {
                        nFailCount++;
                        logstr = "[LINEAR] AxmSignalSetServoAlarm Fail";
                        Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                        //eLogSender("CAxlMotion", $"[{sMotorName}]모터 SERVER Alarm 감지 설정 실패", Globalo.eMessageName.M_ERROR);
                    }
                }
                else if (nMotorType == MotorDefine.eMotorType.STEPING)
                {
                    duRetCode = CAXM.AxmSignalSetInpos(nUseAxis, (uint)AXT_MOTION_LEVEL_MODE.UNUSED);
                    if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                    {
                        nFailCount++;
                        logstr = "[STEPING] AxmSignalSetServoAlarm Fail";
                        Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                        //eLogSender("CAxlMotion", $"[{sMotorName}]모터 Inposition 설정 실패", Globalo.eMessageName.M_ERROR);
                    }
                    duRetCode = CAXM.AxmMotSetPulseOutMethod(nUseAxis, (uint)AXT_MOTION_PULSE_OUTPUT.TwoCcwCwHigh);
                    if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                    {
                        nFailCount++;
                        logstr = "[STEPING] AxmMotSetPulseOutMethod Fail";
                        Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                        //eLogSender("CAxlMotion", $"[{sMotorName}]모터 CW/CCW 설정 실패", Globalo.eMessageName.M_ERROR);
                    }
                    duRetCode = CAXM.AxmSignalSetStop(nUseAxis, (uint)AXT_MOTION_STOPMODE.EMERGENCY_STOP, (uint)AXT_MOTION_LEVEL_MODE.HIGH);
                    if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                    {
                        nFailCount++;
                        logstr = "[STEPING] AxmSignalSetStop Fail";
                        Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                        //_stprintf_s
                    }
                    //pcb z만 high , high

                    if (nUseAxis == 0)      //(int)MotorSet.eMotorList.PCB_Z)
                    {
                        duRetCode = CAXM.AxmSignalSetLimit(nUseAxis, (uint)AXT_MOTION_STOPMODE.EMERGENCY_STOP, (uint)AXT_MOTION_LEVEL_MODE.HIGH, (uint)AXT_MOTION_LEVEL_MODE.HIGH);
                    }
                    else
                    {
                        duRetCode = CAXM.AxmSignalSetLimit(nUseAxis, (uint)AXT_MOTION_STOPMODE.EMERGENCY_STOP, (uint)AXT_MOTION_LEVEL_MODE.LOW, (uint)AXT_MOTION_LEVEL_MODE.LOW);
                    }

                    if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                    {
                        nFailCount++;
                        logstr = "[STEPING] AxmSignalSetLimit Fail";
                        Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                        //eLogSender("CAxlMotion", $"[{sMotorName}]모터 비상 정지 설정 실패", Globalo.eMessageName.M_ERROR);
                    }
                    duRetCode = CAXM.AxmMotSetEncInputMethod(nUseAxis, (uint)AXT_MOTION_EXTERNAL_COUNTER_INPUT.ObverseSqr4Mode);
                    if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                    {
                        nFailCount++;
                        logstr = "[STEPING] AxmMotSetEncInputMethod Fail";
                        Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                        //eLogSender("CAxlMotion", $"[{sMotorName}]모터 4체배 설정 실패", Globalo.eMessageName.M_ERROR);
                    }
                    duRetCode = CAXM.AxmSignalSetServoOnLevel(nUseAxis, (uint)AXT_MOTION_LEVEL_MODE.HIGH);
                    if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                    {
                        nFailCount++;
                        logstr = "[STEPING] AxmSignalSetServoOnLevel Fail";
                        Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                        //eLogSender("CAxlMotion", $"[{sMotorName}]모터 Servo On Level 설정 실패", Globalo.eMessageName.M_ERROR);
                    }
                    duRetCode = CAXM.AxmSignalSetServoAlarm(nUseAxis, (uint)AXT_MOTION_LEVEL_MODE.HIGH);
                    if (duRetCode != (uint)AXT_FUNC_RESULT.AXT_RT_SUCCESS)
                    {
                        nFailCount++;
                        logstr = "[STEPING] AxmSignalSetServoAlarm Fail";
                        Globalo.LogPrint("axm", logstr, Globalo.eMessageName.M_ERROR);
                        //eLogSender("CAxlMotion", $"[{sMotorName}]모터 SERVER Alarm 감지 설정 실패", Globalo.eMessageName.M_ERROR);
                    }
                }
                
            }//for end




            return nFailCount;
        }

        
        public bool AmpDisableAll()
        {
            int i = 0;
            double dDecel = 0.0;

            for (i = 0; i < MotorSet.MAX_MOTOR_COUNT; i++)      //AmpDisableAll
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

            for (i = 0; i < MotorSet.MAX_MOTOR_COUNT; i++)      //AmpDisableAll
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
            for (i = 0; i < MotorSet.MAX_MOTOR_COUNT; i++)      //Axl_Close
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
