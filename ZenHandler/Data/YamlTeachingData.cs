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
    public class TeachingConfig
    {
        public List<double> Speed { get; set; } = new List<double>();
        public List<double> Accel { get; set; } = new List<double>();
        public List<double> Decel { get; set; } = new List<double>();
        public List<double> Resolution { get; set; } = new List<double>();
        public List<TeachingPos> Teaching { get; set; } = new List<TeachingPos>();

        public bool LoadTeach(string fileName, int axisCount, int teachCnt)      //티칭 분리
        {
            string filePath = Path.Combine(CPath.BASE_ENV_PATH, fileName);
            try
            {
                //if (!File.Exists(filePath))
                //{
                //    return false;
                //}
                var Loaded = Data.YamlManager.LoadYaml<TeachingConfig>(filePath);

                if (Loaded == null)
                {
                    Globalo.LogPrint("TeachingDataYaml", $"{fileName} - TEACHING DATA LOAD FAIL", Globalo.eMessageName.M_ERROR);
                   
                    return false;
                }
                // 값 복사
                this.Speed = Loaded.Speed;
                this.Accel = Loaded.Accel;
                this.Decel = Loaded.Decel;
                this.Resolution = Loaded.Resolution;
                this.Teaching = Loaded.Teaching;
                if (this.Teaching.Count < teachCnt)
                {
                    for (int i = 0; i < teachCnt - this.Teaching.Count; i++)
                    {
                        this.Teaching.Add(new TeachingPos { Name = "Pos", Pos = new List<double> { 10.0, 20.0 } });//TODO: 티칭 개수만큼 더해줘야된다.
                    }
                    Globalo.LogPrint("TeachingDataYaml", $"{fileName} - TEACHING DATA add");
                }
                Globalo.LogPrint("TeachingDataYaml", $"{fileName} - TEACHING DATA LOAD COMPLETE!");
                return true;
            }
            catch (Exception ex)
            {
                for (int i = 0; i < axisCount; i++)
                {
                    this.Speed.Add(10.0);
                    this.Accel.Add(0.1);
                    this.Decel.Add(0.1);
                    this.Resolution.Add(1000);
                    this.Teaching.Add(new TeachingPos { Name = "Pos", Pos = new List<double> { 10.0, 20.0 } });
                    
                }
                Console.WriteLine($"Error loading MesLoad: {ex.Message}");
                return false;
            }
        }
        public bool SaveTeach(string fileName)
        {
            string filePath = Path.Combine(CPath.BASE_ENV_PATH, fileName);
            try
            {
                if (!File.Exists(filePath))
                    return false;

                SaveFlowYaml(filePath, this);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Save YAML: {ex.Message}");
                return false;
            }
        }
        public static void SaveFlowYaml(string filePath, TeachingConfig data)
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

    }
    // Flow 스타일 시퀀스를 위한 커스텀 TypeConverter

    public class TeachingDataYaml
    {
        //public HandlerTeachingData handler;
        
        public TeachingDataYaml()
        {

        }
        
        //public bool LoadTeaching(string fileName = "teachingData")
        //{
        //    string filePath = Path.Combine(CPath.BASE_ENV_PATH, "Teaching_"+fileName + ".yaml");    //CPath.yamlTeachingData);
        //    try
        //    {
        //        if (!File.Exists(filePath))
        //            return false;

        //        handler = Data.YamlManager.LoadYaml<HandlerTeachingData>(filePath);

        //        if (handler == null)
        //        {
        //            Globalo.LogPrint("TeachingDataYaml", "TEACHING DATA LOAD FAIL", Globalo.eMessageName.M_ERROR);
        //            return false;
        //        }

        //        Globalo.LogPrint("TeachingDataYaml", "TEACHING DATA LOAD COMPLETE!");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error loading MesLoad: {ex.Message}");
        //        return false;
        //    }
        //}

        //public bool SaveTeaching()
        //{
        //    string filePath = Path.Combine(CPath.BASE_ENV_PATH, CPath.yamlTeachingData);
        //    try
        //    {
        //        if (!File.Exists(filePath))
        //            return false;

        //        //Data.YamlManager.SaveYaml(filePath, teachingHandlerData);
        //        SaveFlowYaml(filePath, handler);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error Save YAML: {ex.Message}");
        //        return false;
        //    }
        //}

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
    }
    
}
