using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace ZenHandler.Data
{
    public enum eTeachPosName : int
    {
        WAIT_POS = 0, LEFT_TRAY_LOAD_POS, RIGHT_TRAY_LOAD_POS, SOCKET_POS1, SOCKET_POS2, SOCKET_POS3, SOCKET_POS4
    };
    
    public class TeachingPos
    {
        public string Name { get; set; }
        public List<double> Pos { get; set; }
    }
    public class TeachingDataList
    {
        public List<double> Speed { get; set; }
        public List<double> Accel { get; set; }
        public List<double> Decel { get; set; }
        public List<double> Resolution { get; set; }
        public List<double> MaxSpeed { get; set; }
        public List<TeachingPos> Teaching { get; set; }
    }

    public class HandlerTeachingData
    {
        public TeachingDataList TransferMachine { get; set; } = new TeachingDataList();
        public TeachingDataList MagazineHandler { get; set; } = new TeachingDataList();
        public TeachingDataList LiftMachine { get; set; } = new TeachingDataList();
    }

    // Flow 스타일 시퀀스를 위한 커스텀 TypeConverter
    public class FlowStyleDoubleListConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return type == typeof(List<double>);
        }

        public object ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
        {
            throw new NotImplementedException("Deserialization not implemented.");
        }
        public void WriteYaml(IEmitter emitter, object value, Type type, ObjectSerializer serializer)
        {
            var list = (List<double>)value;
            emitter.Emit(new SequenceStart(null, null, false, SequenceStyle.Flow));
            foreach (var item in list)
            {
                emitter.Emit(new Scalar(null, null, item.ToString(), ScalarStyle.Any, true, false));
            }
            emitter.Emit(new SequenceEnd());
        }
    }
    public class TeachingDataYaml
    {
        public HandlerTeachingData teachingHandlerData;

        public bool LoadTeaching()
        {
            string filePath = Path.Combine(CPath.BASE_ENV_PATH, CPath.yamlTeachingData);
            try
            {
                if (!File.Exists(filePath))
                    return false;

                teachingHandlerData = Data.YamlManager.LoadYaml<HandlerTeachingData>(filePath);

                if (teachingHandlerData == null)
                {
                    Globalo.LogPrint("TeachingDataYaml", "TEACHING DATA LOAD FAIL", Globalo.eMessageName.M_ERROR);
                    return false;
                }

                Globalo.LogPrint("TeachingDataYaml", "TEACHING DATA LOAD COMPLETE!");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading MesLoad: {ex.Message}");
                return false;
            }
        }

        public bool SaveTeaching()
        {
            string filePath = Path.Combine(CPath.BASE_ENV_PATH, CPath.yamlTeachingData);
            try
            {
                if (!File.Exists(filePath))
                    return false;

                //Data.YamlManager.SaveYaml(filePath, teachingHandlerData);
                SaveFlowYaml(filePath, teachingHandlerData);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Save YAML: {ex.Message}");
                return false;
            }
        }
        public static void SaveFlowYaml(string filePath, HandlerTeachingData data)
        {
            var serializer = new SerializerBuilder()
               // .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .WithTypeConverter(new FlowStyleDoubleListConverter())
                .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
                .Build();

            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, data);
            }

            Console.WriteLine($"YAML 저장 완료: {filePath}");
        }
    }
    
}
