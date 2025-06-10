using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.Data
{
    public class DataManageClass
    {
        public TeachingData teachingData = new TeachingData();
        public IoData ioData = new IoData();
        //public WorkData workData = new WorkData();
        public TaskWork TaskWork = new TaskWork();
        public CMesData mesData = new CMesData();
        public CEEpromData eepromData = new CEEpromData();
    }
    public class CPath
    {
        //BASE
        public const string BASE_PATH = "D:\\EVMS";

        //DATA
        //설비PGM zip파일 Data Directory 파일을 설비PC DATA Directory로 복사 함(덮어쓰기)
        public const string BASE_DATA_PATH = "D:\\EVMS\\Data";


        public const string BASE_OP_PATH = "D:\\EVMS\\OP";
        //ENV - 구동PGM의 설비환경설정파일 보관하는 Directory(삭제되지 않음)
        public const string BASE_ENV_PATH = "D:\\EVMS\\OP\\ENV";

        //BAK - 구동PGM zip파일 Backup
        public const string BASE_BAK_PATH = "D:\\EVMS\\OP\\BAK";

        //EXE - 구동PGM Release 파일이 저장되는 Directory
        //ROP_MP_P1_PAAM_24051000 - 구동PGM zip파일이 압축 해제되는 Directory(모델 변경시 삭제)
        //      - P1_PAAM - 설비모델 (Handler PC #1에서 실행)
        //          - exefile.exe   - 구동PGM 실행파일
        //          - reference.dll - 구동PGM dll 파일 등
        public const string BASE_EXE_PATH = "D:\\EVMS\\OP\\EXE";




        //LOG
        public const string BASE_LOG_PATH = "D:\\EVMS\\OP\\LOG";





        //LAON
        public const string SENSOR_INI_DIR = "D:\\EVMS\\ZenHandler\\Initialize";

        //Mes
        public const string BASE_SECSGEM_PATH = "D:\\EVMS\\ZenHandler\\SecsGem";
        public const string BASE_RECIPE_PATH = "D:\\EVMS\\ZenHandler\\SecsGem\\Recipe";
        public const string BASE_MODEL_PATH = "D:\\EVMS\\ZenHandler\\Model";
        public const string BASE_MODEL_DEFAULT_PATH = "D:\\EVMS\\ZenHandler\\Model\\DEFAULT_MODEL";

        //LOG
        public const string BASE_LOG_EQUIP_PATH = "D:\\EVMS\\OP\\LOG\\EQUIP";               //설비 STEP LOG
        public const string BASE_LOG_MMDDATA_PATH = "D:\\EVMS\\LOG\\MMD_DATA";          //CSV 저장 위치
        public const string BASE_LOG_EEPROMDATA_PATH = "D:\\EVMS\\LOG\\EEPROM_DATA";    //DUMP 저장 위치
        public const string BASE_LOG_ALARM_PATH = "D:\\EVMS\\LOG\\ALARM";               //ALARM 저장 위치

        public const string yamlFilePathModel = "ClientSecGemData.yaml";

        //설정 저장 파일
        public const string yamlTeachingData = "teachingData.yaml";
        public const string yamlFilePathTask = "taskData.yaml";
        public const string yamlFilePathTray = "trayData.yaml";
        public const string yamlFilePathSecGem = "SecGemData.yaml";
        public const string yamlFilePathConfig = "Handler_config.yaml";
        public const string yamlFilePathImage = "imageData.yaml";
        public const string yamlFilePathUgc = "ugcFilePath.yaml";
        public const string yamlFilePathRecipe = "Recipe.yaml";
        public const string yamlFilePathProduct = "products.yaml";
        public const string yamlFilePathUser = "users.yaml";
        public const string yamlFilePathAlarm = "Alarm.yaml";   //ex) Alarm_20250204  하루씩


        //
    }
}
