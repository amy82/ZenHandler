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
        //public IoData ioData = new IoData();
        //public WorkData workData = new WorkData();
        public TaskWork TaskWork = new TaskWork();
        public CMesData mesData = new CMesData();
        public CEEpromData eepromData = new CEEpromData();



    }
    public class CPath
    {
        //BASE
        public const string BASE_PATH = "D:\\EVMS\\ZenHandler";

        //CONFIG
        public const string BASE_DATA_PATH = "D:\\EVMS\\ZenHandler\\Data";

        //LAON
        public const string SENSOR_INI_DIR = "D:\\EVMS\\ZenHandler\\Initialize";

        //Mes
        public const string BASE_SECSGEM_PATH = "D:\\EVMS\\ZenHandler\\SecsGem";
        public const string BASE_RECIPE_PATH = "D:\\EVMS\\ZenHandler\\SecsGem\\Recipe";
        public const string BASE_MODEL_PATH = "D:\\EVMS\\ZenHandler\\Model";
        public const string BASE_MODEL_DEFAULT_PATH = "D:\\EVMS\\ZenHandler\\Model\\DEFAULT_MODEL";

        //LOG
        public const string BASE_LOG_PATH = "D:\\EVMS\\LOG";
        public const string BASE_LOG_EQUIP_PATH = "D:\\EVMS\\LOG\\EQUIP";               //설비 STEP LOG
        public const string BASE_LOG_MMDDATA_PATH = "D:\\EVMS\\LOG\\MMD_DATA";          //CSV 저장 위치
        public const string BASE_LOG_EEPROMDATA_PATH = "D:\\EVMS\\LOG\\EEPROM_DATA";    //DUMP 저장 위치
        public const string BASE_LOG_ALARM_PATH = "D:\\EVMS\\LOG\\ALARM";               //ALARM 저장 위치



        //설정 저장 파일
        public const string yamlTeachingData = "teachingData.yaml";
        public const string yamlFilePathTask = "taskData.yaml";
        public const string yamlFilePathSecGem = "SecGemData.yaml";
        public const string yamlFilePathConfig = "equip_config.yaml";
        public const string yamlFilePathImage = "imageData.yaml";
        public const string yamlFilePathUgc = "ugcFilePath.yaml";
        public const string yamlFilePathRecipe = "Recipe.yaml";
        public const string yamlFilePathProduct = "products.yaml";
        public const string yamlFilePathUser = "users.yaml";
        public const string yamlFilePathAlarm = "Alarm.yaml";   //ex) Alarm_20250204  하루씩


        //
    }
}
