using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenHandler.Data
{
    public class _TaskData
    {
        public LOTDATA LotData;
        public PRODUCTION_INFO ProductionInfo;
        public int PintCount;
    }

    //public class _TrayData
    //{
    //    public List<List<int>> LeftLoadTraySlots { get; set; } = new List<List<int>>();
    //    public List<List<int>> RightLoadTraySlots { get; set; } = new List<List<int>>();
    //    public List<List<int>> LeftNgTraySlots { get; set; } = new List<List<int>>();
    //    public List<List<int>> RightNgTraySlots { get; set; } = new List<List<int>>();
    //}


    public class LOTDATA
    {
        public string BarcodeData { get; set; }
    }
    public class PRODUCTION_INFO
    {
        public int OkCount { get; set; }
        public int NgCount { get; set; }
        public int TotalCount { get; set; }
    }
    public class TaskDataYaml
    {
        public _TaskData TaskData { get; private set; }
        //public _TrayData TrayData { get; private set; }


        //public bool TrayDataLoad()
        //{
        //    string filePath = Path.Combine(CPath.BASE_ENV_PATH, CPath.yamlFilePathTray);       //TRAY DATA
        //    try
        //    {
        //        if (!File.Exists(filePath))
        //        {
        //            TrayData = new _TrayData();
        //            return false;
        //        }


        //        TrayData = Data.YamlManager.LoadYaml<_TrayData>(filePath);
        //        if (TaskData == null)
        //        {

        //            return false;
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error loading TrayDataLoad: {ex.Message}");
        //        return false;
        //    }
        //}
        //public bool TrayDataSave()
        //{
        //    string filePath = Path.Combine(CPath.BASE_ENV_PATH, CPath.yamlFilePathTray);       //LOT DATA
        //    try
        //    {
        //        if (!File.Exists(filePath))
        //            return false;

        //        Data.YamlManager.SaveYaml(filePath, TrayData);
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error TrayDataSave: {ex.Message}");
        //        return false;
        //    }
        //}

        public bool TaskDataLoad()
        {
            string filePath = Path.Combine(CPath.BASE_ENV_PATH, CPath.yamlFilePathTask);       //LOT DATA
            
            try
            {
                if (!File.Exists(filePath))
                {
                    TaskData = new _TaskData();
                    TaskData.LotData = new LOTDATA();
                    TaskData.ProductionInfo = new PRODUCTION_INFO();
                    return false;
                }


                TaskData = Data.YamlManager.LoadYaml<_TaskData>(filePath);
                if (TaskData == null)
                {

                    return false;
                }

                Globalo.dataManage.TaskWork.m_szChipID = TaskData.LotData.BarcodeData;
                Globalo.dataManage.TaskWork.Judge_Total_Count = TaskData.ProductionInfo.TotalCount;
                Globalo.dataManage.TaskWork.Judge_Ok_Count = TaskData.ProductionInfo.OkCount;
                Globalo.dataManage.TaskWork.Judge_Ng_Count = TaskData.ProductionInfo.NgCount;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading TaskDataLoad: {ex.Message}");
                return false;
            }
        }
        public bool TaskDataSave()
        {
            string filePath = Path.Combine(CPath.BASE_ENV_PATH, CPath.yamlFilePathTask);       //LOT DATA
            try
            {
                if (!File.Exists(filePath))
                    return false;

                Data.YamlManager.SaveYaml(filePath, TaskData);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error TaskDataSave: {ex.Message}");
                return false;
            }
        }
        //--------------------------------------------------------------------------------------------------------------
        //
        // Transfer
        //
        public static bool TaskSave_Transfer(Machine.PickedProduct data, string fileName)
        {
            string filePath = Path.Combine(CPath.BASE_ENV_PATH, fileName);       //LOT DATA
            try
            {
                Data.YamlManager.SaveYaml(filePath, data);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error TaskDataSave: {ex.Message}");
                return false;
            }

        }
        public static Machine.PickedProduct TaskLoad_Transfer(string fileName)
        {
            string filePath = Path.Combine(CPath.BASE_ENV_PATH, fileName);       //TRAY DATA
            try
            {
                if (!File.Exists(filePath))
                {
                    return new Machine.PickedProduct();
                }


                Machine.PickedProduct data = Data.YamlManager.LoadYaml<Machine.PickedProduct>(filePath);
                if (data == null)
                {

                    return new Machine.PickedProduct();
                }
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading TaskLoad Transfer: {ex.Message}");
                return new Machine.PickedProduct();
            }
        }


        //--------------------------------------------------------------------------------------------------------------
        //
        // Lift
        //
        public static bool TaskSave_Lift(Machine.TrayProduct data, string fileName)
        {
            string filePath = Path.Combine(CPath.BASE_ENV_PATH, fileName);       //LOT DATA
            try
            {
                Data.YamlManager.SaveYaml(filePath, data);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error TrayDataSave: {ex.Message}");
                return false;
            }

        }
        public static Machine.TrayProduct TaskLoad_Lift(string fileName)
        {
            string filePath = Path.Combine(CPath.BASE_ENV_PATH, fileName);       //TRAY DATA
            try
            {
                if (!File.Exists(filePath))
                {
                    return new Machine.TrayProduct();
                }


                Machine.TrayProduct data = Data.YamlManager.LoadYaml<Machine.TrayProduct>(filePath);
                if (data == null)
                {

                    return new Machine.TrayProduct();
                }
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading TaskLoad Transfer: {ex.Message}");
                return new Machine.TrayProduct();
            }
        }
    }//end
    
    
}
