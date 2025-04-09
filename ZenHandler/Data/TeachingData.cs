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
        
        


        //public enum eMotor_Name : int
        //{
        //    PCB_X = 0, PCB_Y, PCB_Z, PCB_TH, PCB_TX, PCB_TY
        //};
        //public string[] MOTOR_NAME = { "PcbX", "PcbY", "PcbZ", "PcbTh", "PcbTx", "PcbTy" };

        public enum eTeachPosName : int
        {
            WAIT_POS = 0, LOAD_POS, LASER_POS, OC_POS, CHART_POS
        };
        public string[] TEACH_POS_NAME = { "WAIT_POS", "LOAD_POS", "LASER_POS", "OC_POS", "CHART_POS" };

       // private double[] waitPosArray = { 19.790, 27.066, -10.668, 0.000, 0.000, 0.000 };
        //public double[] WaitPos { get; set; }

        public event delLogSender eLogSender;       //외부에서 호출할때 사용
        public TeachingData()
        {
            int i = 0;

            for (i = 0; i < MAX_TEACHPOS_COUNT; i++)
            {
                PcbTeachData[i].sPosName = "EMPTY_POS";// new string[MotorControl.MAX_MOTOR_COUNT];
                PcbTeachData[i].dPos = new double[MotorControl.MAX_MOTOR_COUNT];
                PcbTeachData[i].dOffSet = new double[MotorControl.MAX_MOTOR_COUNT];
               

                LensTeachData[i].sPosName = "EMPTY_POS";
                LensTeachData[i].dPos = new double[MotorControl.MAX_MOTOR_COUNT];
                LensTeachData[i].dOffSet = new double[MotorControl.MAX_MOTOR_COUNT];
               
            }
            for (i = 0; i < MotorControl.PCB_UNIT_COUNT; i++)
            {
                PcbMotorData.dMotorVel = new int[MotorControl.MAX_MOTOR_COUNT];
                PcbMotorData.dMotorAcc = new double[MotorControl.MAX_MOTOR_COUNT];
                PcbMotorData.dMotorDec = new double[MotorControl.MAX_MOTOR_COUNT];
                PcbMotorData.dMotorResol = new double[MotorControl.MAX_MOTOR_COUNT];
            }

            for (i = 0; i < MotorControl.LENS_UNIT_COUNT; i++)
            {
                LensMotorData.dMotorVel = new int[MotorControl.MAX_MOTOR_COUNT];
                LensMotorData.dMotorAcc = new double[MotorControl.MAX_MOTOR_COUNT];
                LensMotorData.dMotorDec = new double[MotorControl.MAX_MOTOR_COUNT];
                LensMotorData.dMotorResol = new double[MotorControl.MAX_MOTOR_COUNT];
            }
        }
        public void DataLoad()
        {
            int i = 0;
            int j = 0;
            TomlNode node2;
            //eLogSender("TeachingData", "Teaching Data Load");
            using (StreamReader reader = File.OpenText("Teach.toml"))//CPath.BASE_DATA_PATH + "\\Teach.toml"))
            {
                // Parse the table
                TomlTable table = TOML.Parse(reader);

                for (i = 0; i < TEACH_POS_NAME.Length; i++)
                {

                    PcbTeachData[i].sPosName = TEACH_POS_NAME[i];
                    LensTeachData[i].sPosName = TEACH_POS_NAME[i];
 
                   
                    node2 = table["PCB_AXIS"][TEACH_POS_NAME[i]];
                    for (j = 0; j < node2.ChildrenCount; j++)
                    {
                        PcbTeachData[i].dPos[j] = Double.Parse(node2[j]);
                    }

                    node2 = table["LENS_AXIS"][TEACH_POS_NAME[i]];
                    for (j = 0; j < node2.ChildrenCount; j++)
                    {
                        LensTeachData[i].dPos[j] = Double.Parse(node2[j]);

                    }
                }
                //PCB
                //
                node2 = table["PCB_AXIS"]["Velocity"];
                for (i = 0; i < node2.ChildrenCount; i++)
                {
                    PcbMotorData.dMotorVel[i] = int.Parse(node2[i]);

                }
                node2 = table["LENS_AXIS"]["Velocity"];
                for (i = 0; i < node2.ChildrenCount; i++)
                {
                    LensMotorData.dMotorVel[i] = int.Parse(node2[i]);
                }
                node2 = table["PCB_AXIS"]["Resolution"];
                for (i = 0; i < node2.ChildrenCount; i++)
                {
                    PcbMotorData.dMotorResol[i] = Double.Parse(node2[i]);
                }
                node2 = table["LENS_AXIS"]["Resolution"];
                for (i = 0; i < node2.ChildrenCount; i++)
                {
                    LensMotorData.dMotorResol[i] = Double.Parse(node2[i]);
                }
                node2 = table["PCB_AXIS"]["Accel"];
                for (i = 0; i < node2.ChildrenCount; i++)
                {
                    PcbMotorData.dMotorAcc[i] = Double.Parse(node2[i]);
                }
                node2 = table["LENS_AXIS"]["Accel"];
                for (i = 0; i < node2.ChildrenCount; i++)
                {
                    LensMotorData.dMotorAcc[i] = Double.Parse(node2[i]);
                }
                node2 = table["PCB_AXIS"]["Decel"];
                for (i = 0; i < node2.ChildrenCount; i++)
                {
                    PcbMotorData.dMotorDec[i] = Double.Parse(node2[i]);
                }
                node2 = table["LENS_AXIS"]["Decel"];
                for (i = 0; i < node2.ChildrenCount; i++)
                {
                    LensMotorData.dMotorDec[i] = Double.Parse(node2[i]);
                }
                //
                //LENS


            }
        }
        public void DataSave()
        {
            int i = 0;
            int j = 0;
            double dTemp = 0.0;
            // eLogSender("TeachingData", LogDefine.enLogLevel.Info, "Teaching Data Save");
            TomlNode mNode1 = new TomlNode[MAX_TEACHPOS_COUNT];
            TomlNode[] mNodeArray = new TomlNode[MAX_TEACHPOS_COUNT];
            TomlArray array = new TomlArray { 1, 2, 3, 4, 5 };
            STURC_TOML_NODE[] sPcbTomlNode = new STURC_TOML_NODE[MAX_TEACHPOS_COUNT + 4];
            STURC_TOML_NODE[] sLensTomlNode = new STURC_TOML_NODE[MAX_TEACHPOS_COUNT + 4];


            for (i = 0; i < MAX_TEACHPOS_COUNT + 4; i++)
            {
                sPcbTomlNode[i].tomlNodes = new TomlNode[MotorControl.PCB_UNIT_COUNT];
                sLensTomlNode[i].tomlNodes = new TomlNode[MotorControl.LENS_UNIT_COUNT];
                for (j = 0; j < MotorControl.PCB_UNIT_COUNT; j++)
                {
                    if (i == 0) //속도
                    {
                        dTemp = PcbMotorData.dMotorVel[j];
                        sPcbTomlNode[i].tomlNodes[j] = dTemp.ToString();
                        
                    }
                    else if (i == 1)    //분해능
                    {
                        dTemp = PcbMotorData.dMotorResol[j];
                        sPcbTomlNode[i].tomlNodes[j] = dTemp.ToString("0.0##");
                        
                    }
                    else if (i == 2)    //가속도
                    {
                        dTemp = PcbMotorData.dMotorAcc[j];
                        sPcbTomlNode[i].tomlNodes[j] = dTemp.ToString("0.0##");
                       
                    }
                    else if (i == 3)    //감속도
                    {
                        dTemp = PcbMotorData.dMotorDec[j];
                        sPcbTomlNode[i].tomlNodes[j] = dTemp.ToString("0.0##");
                    }
                    else
                    {
                        //티칭 위치
                        dTemp = PcbTeachData[i - 4].dPos[j];
                        sPcbTomlNode[i].tomlNodes[j] = dTemp.ToString("0.0##");
                    }
                }
                for (j = 0; j < MotorControl.LENS_UNIT_COUNT; j++)
                {
                    if (i == 0) //속도
                    {
                        dTemp = LensMotorData.dMotorVel[j];
                        sLensTomlNode[i].tomlNodes[j] = dTemp.ToString();

                    }
                    else if (i == 1)    //분해능
                    {
                        dTemp = LensMotorData.dMotorResol[j];
                        sLensTomlNode[i].tomlNodes[j] = dTemp.ToString("0.0##");

                    }
                    else if (i == 2)    //가속도
                    {
                        dTemp = LensMotorData.dMotorAcc[j];
                        sLensTomlNode[i].tomlNodes[j] = dTemp.ToString("0.0##");

                    }
                    else if (i == 3)    //감속도
                    {
                        dTemp = LensMotorData.dMotorDec[j];
                        sLensTomlNode[i].tomlNodes[j] = dTemp.ToString("0.0##");
                    }
                    else
                    {
                        //티칭 위치
                        dTemp = LensTeachData[i - 4].dPos[j];
                        //dTemp = PcbTeachData[i - 4].dPos[j];
                        sLensTomlNode[i].tomlNodes[j] = dTemp.ToString("0.0##");
                    }
                }
            }

            //public string[] TEACH_POS_NAME = { "WAIT_POS", "LOAD_POS", "LASER_POS", "OC_POS", "CHART_POS" };
            TomlTable toml = new TomlTable
            {
                ["title"] = "TEACHING DATA",
                ["PCB_AXIS"] = new TomlTable
                {
                    IsInline = false,
                    ["Velocity"] = sPcbTomlNode[0].tomlNodes,
                    ["Resolution"] = sPcbTomlNode[1].tomlNodes,
                    ["Accel"] = sPcbTomlNode[2].tomlNodes,
                    ["Decel"] = sPcbTomlNode[3].tomlNodes,
                    //
                    [TEACH_POS_NAME[0]] = sPcbTomlNode[4].tomlNodes,
                    [TEACH_POS_NAME[1]] = sPcbTomlNode[5].tomlNodes,
                    [TEACH_POS_NAME[2]] = sPcbTomlNode[6].tomlNodes,
                    [TEACH_POS_NAME[3]] = sPcbTomlNode[7].tomlNodes,
                    [TEACH_POS_NAME[4]] = sPcbTomlNode[8].tomlNodes
                    //["array"] = new TomlNode[] { 1, 2, 3 }
                }
                ,
                ["LENS_AXIS"] = new TomlTable
                {
                    IsInline = false,
                    ["Velocity"] = sLensTomlNode[0].tomlNodes,
                    ["Resolution"] = sLensTomlNode[1].tomlNodes,
                    ["Accel"] = sLensTomlNode[2].tomlNodes,
                    ["Decel"] = sLensTomlNode[3].tomlNodes,
                    //
                    [TEACH_POS_NAME[0]] = sLensTomlNode[4].tomlNodes,
                    [TEACH_POS_NAME[1]] = sLensTomlNode[5].tomlNodes,
                    [TEACH_POS_NAME[2]] = sLensTomlNode[6].tomlNodes,
                    [TEACH_POS_NAME[3]] = sLensTomlNode[7].tomlNodes,
                    [TEACH_POS_NAME[4]] = sLensTomlNode[8].tomlNodes
                    //["array"] = new TomlNode[] { 1, 2, 3 }
                }
            };
            using (StreamWriter writer = File.CreateText("Teach.toml"))// CPath.BASE_DATA_PATH + "\\Teach.toml"))
            {
                toml.WriteTo(writer);
                // Remember to flush the data if needed!
                writer.Flush();
            }
            /*
             
              int i = 0;
            TomlNode mNode1 = new TomlNode[oGlobal.MAX_MOTOR_COUNT];
            for (i = 0; i < 2; i++)
            {
                mNode1[0] = oGlobal.Maindata.mLensTeachData[i].dPos[0];
                mNode1[1] = oGlobal.Maindata.mLensTeachData[i].dPos[1];
                mNode1[2] = oGlobal.Maindata.mLensTeachData[i].dPos[2];
            }

            TomlNode mNode2 = new TomlNode[oGlobal.MAX_MOTOR_COUNT];
            mNode2[0] = oGlobal.Maindata.mLensTeachData[0].dPos[0];
            mNode2[1] = oGlobal.Maindata.mLensTeachData[0].dPos[1];
            mNode2[2] = oGlobal.Maindata.mLensTeachData[0].dPos[2];
            TomlTable toml = new TomlTable
            {
                ["title"] = "LENS UNIT TEACH",
                ["PCB_AXIS"] = new TomlTable
                {
                    IsInline = false,
                    ["WaitPos"] = new TomlNode[] { 0.11, 0.21, 0.1231 },
                    ["LoadPos"] = new TomlNode[] { 5.1, 6.2, 1.123 }
                },

                ["LENS_AXIS"] = new TomlTable
                {
                    IsInline = false,
                    ["WaitPos"] = mNode1     // new TomlNode[] { mLensTeachData.dPos[0], mLensTeachData.dPos[1], mLensTeachData.dPos[2] },
                    ["LoadPos"] = mNode2        //new TomlNode[] { 1.1, 1.2, 1.123}
                }
            };
            using (StreamWriter writer = File.CreateText(CPath.BASE_DATA_PATH + "\\Teach.toml"))
            {
                toml.WriteTo(writer);
                // Remember to flush the data if needed!
                writer.Flush();
            }
             */


        }
    }
    public delegate void delLogSender(object oSender, string strLog, Globalo.eMessageName bPopUpView = Globalo.eMessageName.M_NULL);    //선언 로그 출력
}
