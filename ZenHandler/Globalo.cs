using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;


namespace ZenHandler
{
    public class Globalo
    {
        public enum eMessageName : int
        {
            M_INFO = 0, M_ASK, M_WARNING, M_ERROR, M_VIEW, M_OP_CALL, M_MATERIAL_ID_FAIL, M_TERMINAL_MSG, M_NULL
        };
        public enum BOARDTYPE
        {
            BOARD_TYPE_LAON = 0,
            BOARD_TYPE_POWER,
            BOARD_TYPE_DAQ,
            BOARD_TYPE_LAON1000OP,
            BOARD_TYPE_V5FP
        }

        public static MainForm MainForm;
        ///public LgitLibrary.LgitLibrary LgitDll = new LgitLibrary.LgitLibrary();

        public static LgitLibrary.LgitLibrary GrabberDll = new LgitLibrary.LgitLibrary();

        public static CLaonGrabberClass mLaonGrabberClass = new CLaonGrabberClass();

        public static Data.DataManageClass dataManage = new Data.DataManageClass();
        public static Data.YamlManager yamlManager = new Data.YamlManager();
        public static Data.TaskWork taskWork = new Data.TaskWork();

        public static ThreadControl threadControl;
        public static MotionControl.MotionManager motionManager;
        public static Vision vision = new Vision();

        //우측 메뉴 Form
        public static Dlg.TabMenuForm tabMenuForm;

        //화면 Form
        public static Dlg.TeachingControl mTeachPanel;
        public static Dlg.MainControl mMainPanel;
        public static Dlg.ManualControl mManualPanel;
        public static Dlg.IoControl mioPanel;

        public static Dlg.CCdControl mCCdPanel;
        public static Dlg.ConfigControl mConfigPanel;
        public static Dlg.AlarmControl mAlarmPanel;
        public static Dlg.LogControl mlogControl;
        public static Dlg.CamControl camControl;

        //In Left Panel
        public static Dlg.ProductionInfo productionInfo;
        public static Dlg.OperationPanel operationPanel;
        public static Dlg.TrayStateInfo trayStateInfo;
        public static Dlg.SocketStateInfo socketStateInfo;
        public static Dlg.PickerInfo pickerInfo;

        public static TcpSocket.TcpManager tcpManager;      //secs/gem connect
        public static Serial.SerialPortManager serialPortManager = new Serial.SerialPortManager();


        public const int TabLineY = 56;
        public const int MAX_PATH = 256;
        public const int CHART_ROI_COUNT = 9;
        public const int MTF_ROI_COUNT = 20;
        public const int BASE_THREAD_INTERVAL = 10;
        public static Color GridHeaderBackColor = Color.MediumAquamarine;// LightYellow;//OldLace;//WhiteSmoke; 

        public static int TerminalMessageDialog = 0;
        //Color.WhiteSmoke
        public static void MessageShowPopup(string LogDesc, Globalo.eMessageName bPopUpView = Globalo.eMessageName.M_NULL)
        {
            if (Globalo.MainForm.InvokeRequired)
            {
                Globalo.MainForm.Invoke(new Action(() => MessageShowPopup(LogDesc, bPopUpView)));
            }
            else
            {
                MessagePopUpForm messagePopUp = new MessagePopUpForm();
                messagePopUp.MessageSet(bPopUpView, LogDesc);
                messagePopUp.Show();
            }
        }
        public static DialogResult MessageAskPopup(string LogDesc, string b1 = "YES" , string b2 = "NO")
        {
            MessagePopUpForm messagePopUp3 = new MessagePopUpForm("", b1, b2);
            messagePopUp3.MessageSet(Globalo.eMessageName.M_ASK, LogDesc);

            Globalo.MainForm.keyMessageFilter.RegisterKeyHandler(messagePopUp3.Name.ToString() , messagePopUp3.OnDialogCloseEvent);

            //Globalo.MainForm.keyMessageFilter.KeyEvent += messagePopUp3.OnDialogCloseEvent;

            DialogResult result = messagePopUp3.ShowDialog();

            
            return result;
        }
        public static void LogPrint(object oSender, string LogDesc, Globalo.eMessageName bPopUpView = Globalo.eMessageName.M_NULL)
        {
            DateTime dTime = DateTime.Now;
            string LogInfo = $"[{dTime:hh:mm:ss.f}] {LogDesc}";
            Globalo.threadControl.logThread.logQueue.Enqueue(LogInfo);

            if (bPopUpView != Globalo.eMessageName.M_NULL)
            {
                MessageShowPopup(LogDesc, bPopUpView);
                //MessagePopUpForm messagePopUp = new MessagePopUpForm();
                //messagePopUp.MessageSet(bPopUpView, LogDesc);
                //messagePopUp.Show();

                if (bPopUpView == Globalo.eMessageName.M_WARNING || bPopUpView == Globalo.eMessageName.M_ERROR)
                {
                    Globalo.yamlManager.alarmData.Alarms.Add(new Data.Alarm
                    {
                        No = 1,
                        Time = dTime.ToString("yyyy-MM-dd HH:mm:ss"),     //"2025-02-04 15:00:00"
                        Details = LogDesc
                    });
                    Globalo.yamlManager.AlarmSave();
                    Globalo.mAlarmPanel.RefreshAlarm();
                }
            }
            ///Globalo.yamlManager.alarmData.Alarms.Count; 알람은 여기에 추가하면된다.
        }
        public static bool ShowAskMessageDialog(string LogDesc)
        {
            MessagePopUpForm messagePopUp3 = new MessagePopUpForm("", "YES", "NO");
            messagePopUp3.MessageSet(Globalo.eMessageName.M_ASK, LogDesc);

            Globalo.MainForm.Enabled = false;
            DialogResult result = messagePopUp3.ShowDialog();

            if (result == DialogResult.Yes)
            {
                Globalo.MainForm.Enabled = true;
                return true;
            }
            Globalo.MainForm.Enabled = true;
            return false;
        }
        public static void ShowTerminalMessageDialog(string LogDesc)
        {
            MessagePopUpForm messageForm = new MessagePopUpForm();
            TerminalMessageDialog++;
            if (Globalo.MainForm.InvokeRequired)
            {
                Globalo.MainForm.Invoke(new Action(() => MessageShowPopup(LogDesc, Globalo.eMessageName.M_TERMINAL_MSG)));
            }
            else
            {
                MessagePopUpForm messagePopUp = new MessagePopUpForm("","","Confirm");
                messagePopUp.MessageSet(Globalo.eMessageName.M_TERMINAL_MSG, LogDesc);
                messagePopUp.Show();
            }


        }
        public static void ShowOpCallMessageDialog(string LogDesc)
        {
            MessagePopUpForm messageForm = new MessagePopUpForm();
            messageForm.MessageSet(Globalo.eMessageName.M_OP_CALL,
                LogDesc, "LGIT_OP_CALL", $"OPCALL CODE: {Globalo.dataManage.mesData.rCtrlOp_Call.OpCall_Code}",
                Globalo.dataManage.mesData.rCtrlOp_Call.CallType);
            Globalo.MainForm.Enabled = false;
            //messageForm.Show(); // 새로운 창 계속 생성
            messageForm.ShowDialog();
            Globalo.MainForm.Enabled = true;
        }
        public static void ShowMaterialMessageDialog(string LogDesc)
        {
            MessagePopUpForm messageForm = new MessagePopUpForm();
            LogDesc ="CODE : " 
                    +Globalo.dataManage.mesData.rMaterial_Id_Fail.Code 
                    + "\n"
                    + "TEXT : "
                    + Globalo.dataManage.mesData.rMaterial_Id_Fail.Text;
            messageForm.MessageSet(Globalo.eMessageName.M_MATERIAL_ID_FAIL,
                LogDesc, "LGIT_MATERIAL_ID_FAIL", $"MATERIAL ID: {Globalo.dataManage.mesData.rMaterial_Id_Fail.MaterialId}");

            Globalo.MainForm.Enabled = false;
            messageForm.ShowDialog();
            Globalo.MainForm.Enabled = true;
        }
    }

    public static class ButtonColor
    {
        public static readonly string BTN_ON = "#FFB230";
        public static readonly string BTN_PAUSE_ON = "#FF0000";
        public static readonly string BTN_OFF = "#C3A279";


        public static readonly string MANUAL_BTN_ON = "#4C4743";
        public static readonly string MANUAL_BTN_OFF = "#C3A279"; 
        public static readonly Color BTN_START_ON = Color.BlueViolet;
    }
    public static class SecsGemData
    {
        public static readonly string[] LOT_APD_INFO =
        {
            "BARCODE",
            "SENSOR_SERIAL_ID",
            "CURRENT"
        };

        public static readonly string LGIT_CTRLSTATE_CHG_REQ = "LGIT_CTRLSTATE_CHG_REQ";
        public static readonly string LGIT_OP_CALL = "LGIT_OP_CALL";
        //
        public static readonly string LGIT_SETCODE_OFFLINE_REASON = "LGIT_SETCODE_OFFLINE_REASON";
        public static readonly string LGIT_SETCODE_IDLE_REASON = "LGIT_SETCODE_IDLE_REASON";
        public static readonly string LGIT_SETCODE_MATERIAL_EXCHANGE = "LGIT_SETCODE_MATERIAL_EXCHANGE";
        public static readonly string LGIT_SETCODE_MODEL_LIST = "LGIT_SETCODE_MODEL_LIST";
        //
        public static readonly string LGIT_PP_SELECT = "LGIT_PP_SELECT";
        public static readonly string LGIT_LOT_ID_FAIL = "LGIT_LOT_ID_FAIL";
        public static readonly string LGIT_PP_UPLOAD_CONFIRM = "LGIT_PP_UPLOAD_CONFIRM";
        public static readonly string LGIT_PP_UPLOAD_FAIL = "LGIT_PP_UPLOAD_FAIL";
        public static readonly string LGIT_LOT_START = "LGIT_LOT_START";
        //
        public static readonly string LGIT_MATERIAL_ID_CONFIRM = "LGIT_MATERIAL_ID_CONFIRM";
        public static readonly string LGIT_MATERIAL_ID_FAIL = "LGIT_MATERIAL_ID_FAIL";
        //
        public static readonly string LGIT_EEPROM_DATA = "LGIT_EEPROM_DATA";
        public static readonly string LGIT_EEPROM_FAIL = "LGIT_EEPROM_FAIL";
        //LGIT_EEPROM_DATA
        //client에서 받아서 csv 파일로 저장하고
        //server쪽으로 PASS , FAIL 만 보내준다.
        //
        //
        public static readonly string LGIT_PAUSE = "LGIT_PAUSE";


        public static readonly string MODEL_LIST_CPNAME = "MODELIDLIST";
        public static readonly string SETCODE_MATERIAL_EXCHANGE_CPNAME = "MATERIALINFOLIST";
        public static readonly string IDLE_REASON_CPNAME = "IDLEREASONCODELIST";
        public static readonly string EEPROM_DATA_CPNAME = "EEPROMDATALIST";
    }
}
