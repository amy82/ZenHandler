using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.Data
{
    public class _SecGemData
    {
        public string RecipeId { get; set; }
        public string CurrentModelName { get; set; }       //현재 사용중인 모델 명
    }
    public class RootSecGem
    {
        public _SecGemData SecGemData { get; set; }
    }

    public class UgcSetFile
    {
        public string ugcFilePath { get; set; }

    }
    public class SecGemDataYaml
    {
        public RootSecGem MesData { get; set; }

        public bool SensorIniLoad()
        {
            Globalo.taskWork.vSensorIniList.Clear();
            //
            string logData = "";
            try
            {
                Globalo.taskWork.vSensorIniList.Clear();
                string iniFilePath = CPath.SENSOR_INI_DIR;

                // 폴더가 존재하는지 확인
                if (Directory.Exists(iniFilePath))
                {
                    // 확장자가 ".ini"인 파일만 찾기
                    string[] files = Directory.GetFiles(iniFilePath, "*.ini");

                    // 파일명만 추출해서 출력
                    foreach (string file in files)
                    {
                        string fileName = Path.GetFileName(file);  // 전체 경로에서 파일명만 추출
                        Globalo.taskWork.vSensorIniList.Add(fileName);

                        logData = $"[{MesData.SecGemData.CurrentModelName}] Ini File: {fileName}";
                        Globalo.LogPrint("TcpManager", logData);
                    }
                    // 리스트에 파일 경로 추가

                }

                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error ModelDataSet: {ex.Message}");
                return false;
            }
            return true;
        }
        public bool ModelDataSet(string modelName)
        {
            //Sensor ini 파일 설정
            //CurrentModelName
            MesData.SecGemData.CurrentModelName = modelName;
            Globalo.yamlManager.secsGemDataYaml.MesSave();


            //모델별로 폴더 만드는 코드 250317
            //사용 안함 x
            //
            //Globalo.taskWork.vSensorIniList.Clear();
            //
            //string modelFolerPath = Path.Combine(CPath.BASE_MODEL_PATH, MesData.SecGemData.CurrentModelName);
            //string logData = "";
            //try
            //{
            //    if (!Directory.Exists(modelFolerPath))
            //    {
            //        //새로 추가된 모델 -> [DEFAULT_MODEL] 폴더 복사해서 추가해라
            //        //BASE_MODEL_DEFAULT_PATH  -> copy ->ModelData.CurrentModelName
            //        CopyDirectory(CPath.BASE_MODEL_DEFAULT_PATH, modelFolerPath);
            //        logData = $"[Model] {MesData.SecGemData.CurrentModelName} Folder Create";
            //        Globalo.LogPrint("TcpManager", logData);
            //    }

            //    Globalo.taskWork.vSensorIniList.Clear();


            //    string iniFilePath = Path.Combine(CPath.BASE_MODEL_PATH, MesData.SecGemData.CurrentModelName, "Initialize");

            //    // 폴더가 존재하는지 확인
            //    if (Directory.Exists(iniFilePath))
            //    {
            //        // 확장자가 ".ini"인 파일만 찾기
            //        string[] files = Directory.GetFiles(iniFilePath, "*.ini");

            //        // 파일명만 추출해서 출력
            //        foreach (string file in files)
            //        {
            //            string fileName = Path.GetFileName(file);  // 전체 경로에서 파일명만 추출
            //            Globalo.taskWork.vSensorIniList.Add(fileName);

            //            logData = $"[{MesData.SecGemData.CurrentModelName}] Ini File: {fileName}";
            //            Globalo.LogPrint("TcpManager", logData);
            //        }
            //        // 리스트에 파일 경로 추가

            //    }

            //    Globalo.mCCdPanel.SetSensorIni();

            //    //vSensorIniList
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error ModelDataSet: {ex.Message}");
            //    return false;
            //}


            return true;
        }
        public bool MesLoad()
        {
            string filePath = Path.Combine(CPath.BASE_SECSGEM_PATH, CPath.yamlFilePathSecGem);
            try
            {
                if (!File.Exists(filePath))
                    return false;

                MesData = Data.YamlManager.LoadYaml<RootSecGem>(filePath);
                if (MesData == null)
                {
                    return false;
                }

                Globalo.dataManage.mesData.m_sMesPPID = MesData.SecGemData.RecipeId;        //yaml Load

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading MesLoad: {ex.Message}");
                return false;
            }
        }

        public bool MesSave()
        {
            string filePath = Path.Combine(CPath.BASE_SECSGEM_PATH, CPath.yamlFilePathSecGem);
            try
            {
                if (!File.Exists(filePath))
                    return false;

                Data.YamlManager.SaveYaml(filePath, MesData);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Save YAML: {ex.Message}");
                return false;
            }
        }
        public void CopyDirectory(string sourceDir, string destinationDir)
        {
            // 대상 폴더가 없으면 생성
            if (!Directory.Exists(destinationDir))
            {
                Directory.CreateDirectory(destinationDir);
            }

            // 현재 폴더의 파일 복사
            foreach (string file in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(destinationDir, fileName);
                File.Copy(file, destFile, true); // 덮어쓰기 허용
            }

            // 하위 폴더 복사 (재귀적으로 수행)
            foreach (string subDir in Directory.GetDirectories(sourceDir))
            {
                string dirName = Path.GetFileName(subDir);
                string destSubDir = Path.Combine(destinationDir, dirName);
                CopyDirectory(subDir, destSubDir); // 재귀 호출
            }
        }
    }
    

}
