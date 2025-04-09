using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ZenHandler.Data
{
    public class TaskWork
    {
        public int m_nTestFinalResult;

        public int m_nCurrentStep;
        public int m_nStartStep;
        public int m_nEndStep;

        public Rectangle[] rtChartRect = new Rectangle[Globalo.CHART_ROI_COUNT];
        public Rectangle[] rtSfrSmallRect = new Rectangle[Globalo.MTF_ROI_COUNT];

        public string m_szChipID;
        public int EEpromReadTotalCount;

        //Judge Count
        //
        public int Judge_Ok_Count;
        public int Judge_Ng_Count;
        public int Judge_Total_Count;

        // Secs/Gem
        //
        public int bRecv_Client_ObjectIdReport;
        public int bRecv_Client_LotStart;           //착공 확인 신호
        public int bRecv_Client_ApdReport;          //완공 확인 신호

        public int bRecv_Client_CtTimeOut;          //CT TimeOUt 신호
        public string CtTimeOutValue;


        public List<string> vSensorIniList = new List<string>();

        

        public TaskWork()
        {
            int i = 0;
            m_nTestFinalResult = 0;
            m_szChipID = "";
            Judge_Ok_Count = 0;
            Judge_Ng_Count = 0;
            Judge_Total_Count = 0;

            EEpromReadTotalCount = 1;
            bRecv_Client_ObjectIdReport = -1;
            bRecv_Client_ApdReport = -1;
            bRecv_Client_CtTimeOut = -1;
            CtTimeOutValue = "";

            m_nCurrentStep = 0;
            m_nStartStep = 0;
            m_nEndStep = 0;

            
            for (i = 0; i < Globalo.CHART_ROI_COUNT; i++)
            {
                rtChartRect[i] = new Rectangle();
            }
            for (i = 0; i < Globalo.MTF_ROI_COUNT; i++)
            {
                rtSfrSmallRect[i] = new Rectangle();
            }


            vSensorIniList.Clear();
        }
    }
}
