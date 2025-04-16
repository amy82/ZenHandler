using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Tommy;

namespace ZenHandler.Data
{
    public struct STURC_TEACH_DATA
    {
        public string sPosName;
        public double[] dPos;               // 티칭 POS
        public double[] dOffSet;            // 티칭 OFFSET
    }

    public struct STURC_MOTOR_DATA
    {
        public int[] dMotorVel;
        public double[] dMotorAcc;
        public double[] dMotorDec;
        public double[] dMotorResol;
    }
    public struct STURC_TOML_NODE
    {
        public TomlNode[] tomlNodes;
    }
    public class TeachingData
    {

        public const int MAX_TEACHPOS_COUNT = 5;     //티칭 + 기타 상태
        public STURC_TEACH_DATA[] PcbTeachData = new STURC_TEACH_DATA[MAX_TEACHPOS_COUNT];
        public STURC_TEACH_DATA[] LensTeachData = new STURC_TEACH_DATA[MAX_TEACHPOS_COUNT];


        public STURC_MOTOR_DATA PcbMotorData;
        public STURC_MOTOR_DATA LensMotorData;


        public int PositionId { get; set; }
        public List<double> lPos { get; set; }
        
        
        public enum eTeachPosName : int
        {
            WAIT_POS = 0, LOAD_POS, LASER_POS, OC_POS, CHART_POS
        };
        public string[] TEACH_POS_NAME = { "WAIT_POS", "LOAD_POS", "LASER_POS", "OC_POS", "CHART_POS" };

        
        //public event delLogSender eLogSender;       //외부에서 호출할때 사용
        public TeachingData()
        {
            
        }
        public void DataLoad()
        {
           
        }
        public void DataSave()
        {
            

        }
    }
    public delegate void delLogSender(object oSender, string strLog, Globalo.eMessageName bPopUpView = Globalo.eMessageName.M_NULL);    //선언 로그 출력
}
