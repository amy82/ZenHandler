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
        //
        //--------------------------------------------------------------------------------------------------------------
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

        public static bool TaskSave_Layout(Machine.ProductLayout data, string fileName)
        {
            string filePath = Path.Combine(CPath.BASE_ENV_PATH, fileName);       //LOT DATA
            try
            {
                Data.YamlManager.SaveYaml(filePath, data);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error TaskSave_Layout: {ex.Message}");
                return false;
            }

        }
        public static Machine.ProductLayout TaskLoad_Layout(string fileName)
        {
            string filePath = Path.Combine(CPath.BASE_ENV_PATH, fileName);       //TRAY DATA
            try
            {
                if (!File.Exists(filePath))
                {
                    return new Machine.ProductLayout();
                }


                Machine.ProductLayout data = Data.YamlManager.LoadYaml<Machine.ProductLayout>(filePath);
                if (data == null)
                {

                    return new Machine.ProductLayout();
                }
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading TaskLoad_Layout Transfer: {ex.Message}");
                return new Machine.ProductLayout();
            }
        }
        

        //--------------------------------------------------------------------------------------------------------------
        //
        // Lift
        //
        //
        //--------------------------------------------------------------------------------------------------------------
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

        //--------------------------------------------------------------------------------------------------------------
        //
        // Magazine 
        //
        //
        //--------------------------------------------------------------------------------------------------------------
        public static bool TaskSave_Magazine(Machine.MagazineTray data, string fileName)
        {
            string filePath = Path.Combine(CPath.BASE_ENV_PATH, fileName);       //LOT DATA
            try
            {
                Data.YamlManager.SaveYaml(filePath, data);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error MagazineDataSave: {ex.Message}");
                return false;
            }

        }
        public static Machine.MagazineTray TaskLoad_Magazine(string fileName)
        {
            string filePath = Path.Combine(CPath.BASE_ENV_PATH, fileName);       //TRAY DATA
            try
            {
                if (!File.Exists(filePath))
                {
                    return new Machine.MagazineTray();
                }


                Machine.MagazineTray data = Data.YamlManager.LoadYaml<Machine.MagazineTray>(filePath);
                if (data == null)
                {

                    return new Machine.MagazineTray();
                }
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading TaskLoad Magazine: {ex.Message}");
                return new Machine.MagazineTray();
            }
        }


        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------
    }//end


}
