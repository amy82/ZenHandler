using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Matrox.MatroxImagingLibrary;
using System.Windows.Forms;
using System.Drawing;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using OpenCvSharp;

namespace ZenHandler
{
    public struct STRUC_MIL_BOX
    {
        public int nWidth;
        public DashStyle nStyle;
        public Rectangle clRectBox;
        public Color color;//COLORREF color;
    }
    public struct STRUC_MIL_TEXT
    {
        public string szFontName;
        public string szText;
        public System.Drawing.Point clPoint;
        public int clFontSize;
        public Color color;
    }
    public struct STRUC_MIL_CROSS
    {
        public int nWidth;
        public int nSize;
        public System.Drawing.Point clPoint;
        public Color color;
    }
    public class CMilDrawBox
    {
        private List<STRUC_MIL_BOX> m_clArrayBox = new List<STRUC_MIL_BOX>();
        public CMilDrawBox()
        {
        }
        ~CMilDrawBox()
        {
            m_clArrayBox.Clear();
        }
        public void AddList(int nLeft, int nTop, int nRight, int nBottom, int nWidth, int nStyle, Color color)//COLORREF color);
        {

        }
        public void AddList(Rectangle clRect, int nWidth, DashStyle nStyle, Color color)
        {
            STRUC_MIL_BOX stMilBox = new STRUC_MIL_BOX();

            stMilBox.clRectBox = clRect;
            stMilBox.nWidth = nWidth;
            stMilBox.nStyle = nStyle;
            stMilBox.color = color;

            m_clArrayBox.Add(stMilBox);
        }
        public void Draw(IntPtr hOverlayDC, double dZoomX, double dZoomY)  //HDC hOverlayDC
        {
            STRUC_MIL_BOX stMilBox;
            int nCount;
            int i;

            nCount = (int)m_clArrayBox.Count();

            if (!hOverlayDC.Equals(IntPtr.Zero))
            {
                if(nCount > 0)
                {
                    using (Graphics DrawingGraphics = Graphics.FromHdc(hOverlayDC))
                    {
                        for (i = 0; i < nCount; i++)
                        {
                            stMilBox = m_clArrayBox[i];
                            using (Pen DrawingPen = new Pen(stMilBox.color))
                            {
                                DrawingPen.DashStyle = stMilBox.nStyle;
                                DrawingPen.Width = stMilBox.nWidth;
                                //
                                int x1 = (int)((stMilBox.clRectBox.X * dZoomX) + 0.5);
                                int x2 = (int)((stMilBox.clRectBox.Width * dZoomX) + 0.5);
                                int y1 = (int)((stMilBox.clRectBox.Y * dZoomY) + 0.5);
                                int y2 = (int)((stMilBox.clRectBox.Height * dZoomY) + 0.5);

                                DrawingGraphics.DrawRectangle(DrawingPen, new Rectangle(x1, y1, x2, y2));
                            }
                        }
                    }
                }
                
            }
        }
        
        public void RemoveAll()
        {
            // m_clArrayBox.RemoveAll();
            m_clArrayBox.Clear();
        }
    };
    public class CMilDrawText
    {
        public List<STRUC_MIL_TEXT> m_clArrayText = new List<STRUC_MIL_TEXT>();
        public CMilDrawText()
        {
        }
        ~CMilDrawText()
        {
            m_clArrayText.Clear();
        }
	    public void AddList(int nPosX, int nPosY, int nSizeX, int nSizeY, string szText, string szFontName, Color color)
        {

        }
        public void AddList(System.Drawing.Point clPoint, int nFontSize, string szText, string szFontName, Color color)
        {
            STRUC_MIL_TEXT stMilText = new STRUC_MIL_TEXT();

            stMilText.clPoint = clPoint;
            stMilText.clFontSize = nFontSize;
            stMilText.szText = szText;
            stMilText.szFontName = szFontName;
            stMilText.color = color;

            m_clArrayText.Add(stMilText);
        }
        public void Draw(IntPtr hOverlayDC, double dZoomX, double dZoomY)
        {
            STRUC_MIL_TEXT stMilText;

            int nCount;
            int i;

            nCount = (int)m_clArrayText.Count();

            if (!hOverlayDC.Equals(IntPtr.Zero))
            {
                if (nCount > 0)
                {
                    using (Graphics DrawingGraphics = Graphics.FromHdc(hOverlayDC))
                    {
                        using (Pen DrawingPen = new Pen(Color.Blue))
                        {
                            for (i = 0; i < nCount; i++)
                            {
                                stMilText = m_clArrayText[i];
                                using (SolidBrush mBrush = new SolidBrush(stMilText.color))
                                {
                                    using (Font OverlayFont = new Font(FontFamily.GenericSansSerif, stMilText.clFontSize, FontStyle.Bold))
                                    {
                                        int x = (int)((stMilText.clPoint.X * dZoomX) + 0.5);
                                        int y = (int)((stMilText.clPoint.Y * dZoomY) + 0.5);

                                        DrawingGraphics.DrawString(stMilText.szText, OverlayFont, mBrush, x, y);
                                    }
                                }
                            }
                        }
                    }
                }

            }
        }
        public void RemoveAll()
        {
            m_clArrayText.Clear();
        }

    };
    public class CMilDrawCross
    {
        public List<STRUC_MIL_CROSS> m_clArrayCross = new List<STRUC_MIL_CROSS>();
 
        public CMilDrawCross()
        {

        }
        ~CMilDrawCross()
        {
            m_clArrayCross.Clear();
        }

	    public void AddList(int nPosX, int nPosY, int nSize, int nWidth, Color color)
        {

        }
        public void AddList(System.Drawing.Point clPoint, int nSize, int nWidth, Color color)
        {
            STRUC_MIL_CROSS stMilCross;

            stMilCross.clPoint = clPoint;
            stMilCross.nSize = nSize;
            stMilCross.nWidth = nWidth;
            stMilCross.color = color;

            m_clArrayCross.Add(stMilCross);
        }
        public void Draw(IntPtr hOverlayDC, double dZoomX, double dZoomY)
        {
            STRUC_MIL_CROSS stMilCross;
            int nCount;
            int i;
            System.Drawing.Point clCrossPos = new System.Drawing.Point();
            nCount = (int)m_clArrayCross.Count();

            if (!hOverlayDC.Equals(IntPtr.Zero))
            {
                if (nCount > 0)
                {
                    using (Graphics DrawingGraphics = Graphics.FromHdc(hOverlayDC))
                    {
                        // Draw a blue cross.
                        for (i = 0; i < nCount; i++)
                        {
                            stMilCross = m_clArrayCross[i];
                            using (Pen DrawingPen = new Pen(stMilCross.color))
                            {
                                clCrossPos.X = stMilCross.clPoint.X + stMilCross.nSize;
                                clCrossPos.Y = stMilCross.clPoint.Y + stMilCross.nSize;


                                int x1 = (int)((stMilCross.clPoint.X - stMilCross.nSize) * dZoomX + 0.5);
                                int x2 = (int)((stMilCross.clPoint.X + stMilCross.nSize) * dZoomX + 0.5);
                                int y1 = (int)((stMilCross.clPoint.Y * dZoomY) + 0.5);

                                int x3 = (int)((stMilCross.clPoint.X) * dZoomX + 0.5);
                                int y2 = (int)((stMilCross.clPoint.Y - stMilCross.nSize) * dZoomY + 0.5);
                                int y3 = (int)((stMilCross.clPoint.Y + stMilCross.nSize) * dZoomY + 0.5);


                                DrawingGraphics.DrawLine(DrawingPen, new System.Drawing.Point(x1, y1), new System.Drawing.Point(x2, y1));
                                DrawingGraphics.DrawLine(DrawingPen, new System.Drawing.Point(x3, y2), new System.Drawing.Point(x3, y3));


                            }
                        }
                       
                    }
                }

            }
        }
        public void RemoveAll()
        {
            m_clArrayCross.Clear();
        }
    };

    public class Vision
    {
        //public event delLogSender eLogSender;       //외부에서 호출할때 사용
        //private bool bGrabRun = true;
        //private UserPanel myPic;
        public bool CAM_LOAD_CHECK = false;
        private int MAX_CAMERA_COUNT = 0;
        public int CAM_SIZE_X = 1280;//2000;
        public int CAM_SIZE_Y = 1024;//1500;
        public int SMALL_CAM_SIZE_X = 800;
        public int SMALL_CAM_SIZE_Y = 600;
        public int CamCount { get; set; }
        //Cam
        private double m_CamReduceFactorX;
        public double M_CamReduceFactorX
        {
            get { return m_CamReduceFactorX; }
        }
        private double m_CamReduceFactorY;
        public double M_CamReduceFactorY
        {
            get { return m_CamReduceFactorY; }
        }
        private double m_CamExpandFactorX;
        public double M_CamExpandFactorX
        {
            get { return m_CamExpandFactorX; }
        }
        private double m_CamExpandFactorY;
        public double M_CamExpandFactorY
        {
            get { return m_CamExpandFactorY; }
        }
        //ccd
        private double m_CcdReduceFactorX;
        public double M_CcdReduceFactorX
        {
            get { return m_CcdReduceFactorX; }
        }
        private double m_CcdReduceFactorY;
        public double M_CcdReduceFactorY
        {
            get { return m_CcdReduceFactorY; }
        }

        private double m_CcdExpandFactorX;
        public double M_CcdExpandFactorX
        {
            get { return m_CcdExpandFactorX; }
        }
        private double m_CcdExpandFactorY;
        public double M_CcdExpandFactorY
        {
            get { return m_CcdExpandFactorY; }
        }
        //

        public CMilDrawBox[] m_clMilDrawBox = new CMilDrawBox[1];
        public CMilDrawText[] m_clMilDrawText = new CMilDrawText[1];
        public CMilDrawCross[] m_clMilDrawCross = new CMilDrawCross[1];

        public MIL_ID MilApplication = MIL.M_NULL;         // Application identifier.
        public MIL_ID MilSystem = MIL.M_NULL;              // System identifier.
        //
        public MIL_ID[] MilCamDisplay;                        // Display identifier.
        public MIL_ID[] MilCcdDisplay;                        // Display identifier.
        public MIL_ID MilDigitizer = MIL.M_NULL;           // Digitizer identifier.
        //
        //
        //ALIGN CAMERA
        //
        public MIL_ID[] MilCamGrabImage;
        public MIL_ID[] MilCamGrabImageChild;
        public MIL_ID[] MilCamSmallImage;
	    public MIL_ID[] MilCamSmallImageChild;
        public MIL_ID[] MilCamOverlay;
        public MIL_INT[] MilCamTransparent;
        //
        //CCD
        public MIL_ID[] m_MilCcdGrabImage;
        public MIL_ID[] m_MilSmallImage;
        public MIL_ID[] m_MilCcdProcImage;
        public MIL_ID[,] m_MilCcdProcChild;
        public MIL_ID[] MilCcdOverlay;
        public MIL_INT[] MilCcdTransparent;
        //public byte[,] m_pImgBuff;

        public byte[][][] m_pImgBuff = new byte[1][][];
        //
        public int[] m_nGrabIndex = new int[1];
        public int[] m_nCvtColorReadyIndex = new int[1];
        //MIL_INT nMilCount;
        MIL_INT m_nMilSizeX;
        MIL_INT m_nMilSizeY;
        //
        Thread thread = null;
        
        public Vision()//Form _form)
        {
            // MainForm = _form;

            m_nGrabIndex[0] = 0;
            m_nCvtColorReadyIndex[0] = 0;

            m_clMilDrawBox[0] = new CMilDrawBox();
            m_clMilDrawText[0] = new CMilDrawText();
            m_clMilDrawCross[0] = new CMilDrawCross();
        }

        ~Vision()
        {
            
        }
        public void UISet(int mDistWidth, int mDistHeight)
        {
            SMALL_CAM_SIZE_X = mDistWidth;
            SMALL_CAM_SIZE_Y = mDistHeight;

            m_CamReduceFactorX = ((double)mDistWidth / (double)CAM_SIZE_X);
            m_CamReduceFactorY = ((double)mDistHeight / (double)CAM_SIZE_Y);

            m_CamExpandFactorX = ((double)CAM_SIZE_X / (double)mDistWidth);
            m_CamExpandFactorY = ((double)CAM_SIZE_Y / (double)mDistHeight);

            m_CcdReduceFactorX = ((double)mDistWidth / (double)Globalo.mLaonGrabberClass.m_nWidth);
            m_CcdReduceFactorY = ((double)mDistHeight / (double)Globalo.mLaonGrabberClass.m_nHeight);

            m_CcdExpandFactorX = ((double)Globalo.mLaonGrabberClass.m_nWidth / (double)mDistWidth);
            m_CcdExpandFactorY = ((double)Globalo.mLaonGrabberClass.m_nHeight / (double)mDistHeight);


            ///myPic = new UserPanel();
            //myPic.Location = new Point(0, 0);
            //myPic.Size = new Size(SMALL_CAM_SIZE_X, SMALL_CAM_SIZE_Y);


            //MainForm.Controls.Add(myPic);
        }
        public void DrawOverlayAll(int nUnit, int DisplayMode)
        {
            if (ProgramState.ON_LINE_MIL == false)
            {
                return;
            }
            double dZoomX = 0.0;
            double dZoomY = 0.0;
            //HDC hOverlayDC;
            IntPtr hOverlayDC = IntPtr.Zero;

            if (DisplayMode == 0)
            {
                dZoomX = 1.0;// m_CamReduceFactorX;
                dZoomY = 1.0;// m_CamReduceFactorY;
                MIL.MbufControl(MilCamOverlay[0], MIL.M_DC_ALLOC, MIL.M_DEFAULT);
                hOverlayDC = (IntPtr)MIL.MbufInquire(MilCamOverlay[0], MIL.M_DC_HANDLE, MIL.M_NULL);
            }
            else
            {
                dZoomX = m_CcdReduceFactorX;
                dZoomY = m_CcdReduceFactorY;
                MIL.MbufControl(MilCcdOverlay[0], MIL.M_DC_ALLOC, MIL.M_DEFAULT);
                hOverlayDC = (IntPtr)MIL.MbufInquire(MilCcdOverlay[0], MIL.M_DC_HANDLE, MIL.M_NULL);
            }


            m_clMilDrawBox[nUnit].Draw(hOverlayDC, dZoomX, dZoomY);
            m_clMilDrawText[nUnit].Draw(hOverlayDC, dZoomX, dZoomY);
            m_clMilDrawCross[nUnit].Draw(hOverlayDC, dZoomX, dZoomY);

            if (DisplayMode == 0)
            {
                MIL.MbufControl(MilCamOverlay[0], MIL.M_DC_FREE, MIL.M_DEFAULT);
                MIL.MbufControl(MilCamOverlay[0], MIL.M_MODIFIED, MIL.M_DEFAULT);
            }
            else
            {
                MIL.MbufControl(MilCcdOverlay[0], MIL.M_DC_FREE, MIL.M_DEFAULT);
                MIL.MbufControl(MilCcdOverlay[0], MIL.M_MODIFIED, MIL.M_DEFAULT);
            }
        }
        public void ClearOverlay(int nUnit, int DisplayMode)
        {
            if (ProgramState.ON_LINE_MIL == false)
            {
                return;
            }
            m_clMilDrawBox[nUnit].RemoveAll();
            m_clMilDrawText[nUnit].RemoveAll();
            m_clMilDrawCross[nUnit].RemoveAll();
            if (DisplayMode == 0)
            {
                MIL.MbufClear(MilCamOverlay[0], MilCamTransparent[0]);
            }
            else
            {
                MIL.MbufClear(MilCcdOverlay[0], MilCcdTransparent[0]);
            }
        }
        public void DrawOverlayBox(int nUnit, int nCamIndex, Rectangle clRect, Color color, int nWid, DashStyle nStyles, int DisplayMode)
        {
            if (ProgramState.ON_LINE_MIL == false)
            {
                return;
            }
            double dZoomX = 0.0;
            double dZoomY = 0.0;
            IntPtr hOverlayDC = IntPtr.Zero;

            if (DisplayMode == 0)
            {
                dZoomX = 1.0;
                dZoomY = 1.0;
                MIL.MbufControl(MilCamOverlay[0], MIL.M_DC_ALLOC, MIL.M_DEFAULT);
                hOverlayDC = (IntPtr)MIL.MbufInquire(MilCamOverlay[0], MIL.M_DC_HANDLE, MIL.M_NULL);
            }
            else
            {
                dZoomX = m_CcdReduceFactorX;
                dZoomY = m_CcdReduceFactorY;
                MIL.MbufControl(MilCcdOverlay[0], MIL.M_DC_ALLOC, MIL.M_DEFAULT);
                hOverlayDC = (IntPtr)MIL.MbufInquire(MilCcdOverlay[0], MIL.M_DC_HANDLE, MIL.M_NULL);
            }
            

            //
            if (!hOverlayDC.Equals(IntPtr.Zero))
            {
                // Attach the device context
                //CDC NewDC;
                //NewDC.Attach(hOverlayDC);
                //NewDC.SetBkMode(TRANSPARENT);
                //NewDC.SelectStockObject(NULL_BRUSH);
                using (Graphics DrawingGraphics = Graphics.FromHdc(hOverlayDC))
                {
                    // Draw a blue cross.
                    using (Pen DrawingPen = new Pen(Color.Blue))
                    {
                        DrawingPen.DashStyle = nStyles;
                        DrawingPen.Width = nWid;
                        int x1 = (int)((clRect.X * dZoomX) + 0.5);
                        int x2 = (int)((clRect.Width * dZoomX) + 0.5);
                        int y1 = (int)((clRect.Y * dZoomY) + 0.5);
                        int y2 = (int)((clRect.Height * dZoomY) + 0.5);

                        DrawingGraphics.DrawRectangle(DrawingPen, new Rectangle(x1, y1, x2, y2));

                        //DrawingGraphics.DrawLine(DrawingPen, 0, (int)(CAM_SIZE_Y / 2), CAM_SIZE_X, (int)(CAM_SIZE_Y / 2));
                        //DrawingGraphics.DrawLine(DrawingPen, (int)(CAM_SIZE_X / 2), 0, (int)(CAM_SIZE_X / 2), CAM_SIZE_Y);
                        // Prepare transparent text annotations.
                        // Define the Brushes and fonts used to draw text
                        using (SolidBrush LeftBrush = new SolidBrush(Color.Red))
                        {
                            using (SolidBrush RightBrush = new SolidBrush(Color.Yellow))
                            {
                                using (Font OverlayFont = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold))
                                {
                                    // Write text in the overlay image
                                    //SizeF GDITextSize = DrawingGraphics.MeasureString("GDI Overlay Text", OverlayFont);
                                    //DrawingGraphics.DrawString("GDI Overlay Text", OverlayFont, LeftBrush, System.Convert.ToInt32(CAM_SIZE_X / 4 - GDITextSize.Width / 2), System.Convert.ToInt32(CAM_SIZE_Y * 3 / 4 - GDITextSize.Height / 2));
                                    //DrawingGraphics.DrawString("GDI Overlay Text", OverlayFont, RightBrush, System.Convert.ToInt32(CAM_SIZE_X * 3 / 4 - GDITextSize.Width / 2), System.Convert.ToInt32(CAM_SIZE_Y * 3 / 4 - GDITextSize.Height / 2));
                                }
                            }
                        }
                    }
                }
            }
            if (DisplayMode == 0)
            {
                MIL.MbufControl(MilCamOverlay[0], MIL.M_DC_FREE, MIL.M_DEFAULT);
                MIL.MbufControl(MilCamOverlay[0], MIL.M_MODIFIED, MIL.M_DEFAULT);
            }
            else
            {
                MIL.MbufControl(MilCcdOverlay[0], MIL.M_DC_FREE, MIL.M_DEFAULT);
                MIL.MbufControl(MilCcdOverlay[0], MIL.M_MODIFIED, MIL.M_DEFAULT);
            }
            
        }
        public void DrawMOverlayBox(int nUnit, int nCamIndex, Rectangle clRect, Color color, int nWid, bool bRealDraw, DashStyle nStyles, int DisplayMode)
        {
            if (ProgramState.ON_LINE_MIL == false)
            {
                return;
            }
            if (bRealDraw)
            {
                DrawOverlayBox(nCamIndex, nCamIndex, clRect, color, nWid, nStyles, DisplayMode);
            }
            else
            {
                m_clMilDrawBox[nCamIndex].AddList(clRect, nWid, nStyles, color);
            }
        }

        //-----------------------------------------------------------------------------
        //
        //	Overlay 텍스트 추가
        //
        //-----------------------------------------------------------------------------
        public void DrawMOverlayText(int nCamIndex, int nX, int nY, string szText, Color color, string szFontName, int mSize, bool bRealDraw, int DisplayMode)
        {
            if (ProgramState.ON_LINE_MIL == false)
            {
                return;
            }
            System.Drawing.Point clPointPos = new System.Drawing.Point();

            clPointPos.X = nX;
            clPointPos.Y = nY;

            if (bRealDraw)
            {
                DrawOverlayText(nCamIndex, clPointPos, szText, color, szFontName, mSize, DisplayMode);
            }
            else
            {
                m_clMilDrawText[nCamIndex].AddList(clPointPos, mSize, szText, szFontName, color);
            }
        }
        public void DrawOverlayText(int nUnit, System.Drawing.Point clPoint, string szText, Color color, string szFontName, int nFs, int DisplayMode)
        {
            if (ProgramState.ON_LINE_MIL == false)
            {
                return;
            }
            int x;
            int y;
            double dZoomX = 0.0;
            double dZoomY = 0.0;

            IntPtr hOverlayDC = IntPtr.Zero;

            dZoomX = m_CamReduceFactorX;
            dZoomY = m_CamReduceFactorY;

            x = (int)((clPoint.X * dZoomX) + 0.5);
            y = (int)((clPoint.Y * dZoomY) + 0.5);
            if(DisplayMode == 0)
            {
                MIL.MbufControl(MilCamOverlay[0], MIL.M_DC_ALLOC, MIL.M_DEFAULT);
                hOverlayDC = (IntPtr)MIL.MbufInquire(MilCamOverlay[0], MIL.M_DC_HANDLE, MIL.M_NULL);
            }
            else
            {
                MIL.MbufControl(MilCcdOverlay[0], MIL.M_DC_ALLOC, MIL.M_DEFAULT);
                hOverlayDC = (IntPtr)MIL.MbufInquire(MilCcdOverlay[0], MIL.M_DC_HANDLE, MIL.M_NULL);
            }
            

            if (!hOverlayDC.Equals(IntPtr.Zero))
            {
                using (Graphics DrawingGraphics = Graphics.FromHdc(hOverlayDC))
                {
                    // Draw a blue cross.
                    using (Pen DrawingPen = new Pen(Color.Blue))
                    {
                        // Prepare transparent text annotations.
                        // Define the Brushes and fonts used to draw text
                        using (SolidBrush LeftBrush = new SolidBrush(Color.Red))
                        {
                            using (SolidBrush RightBrush = new SolidBrush(Color.Yellow))
                            {
                                using (Font OverlayFont = new Font(FontFamily.GenericSansSerif, nFs, FontStyle.Bold))
                                {
                                    // Write text in the overlay image
                                    DrawingGraphics.DrawString("CCD ROI TEST", OverlayFont, LeftBrush, x, y);
                                }
                            }
                        }
                    }
                }
            }
            if (DisplayMode == 0)
            {
                MIL.MbufControl(MilCamOverlay[0], MIL.M_DC_FREE, MIL.M_DEFAULT);
                MIL.MbufControl(MilCamOverlay[0], MIL.M_MODIFIED, MIL.M_DEFAULT);
            }
            else
            {
                MIL.MbufControl(MilCcdOverlay[0], MIL.M_DC_FREE, MIL.M_DEFAULT);
                MIL.MbufControl(MilCcdOverlay[0], MIL.M_MODIFIED, MIL.M_DEFAULT);
            }
        }
        //-----------------------------------------------------------------------------
        //
        //	Overlay Cross 추가
        //
        //-----------------------------------------------------------------------------
        public void DrawOverlayCross(int nUnit, int nCamIndex, System.Drawing.Point clPointPos, int nWdt, Color color, int nWid, int nStyles, int DisplayMode)
        {
            if (ProgramState.ON_LINE_MIL == false)
            {
                return;
            }
        }
        public void DrawMOverlayCross(int nUnit, int nCamIndex, System.Drawing.Point clPointPos, int nLength, Color color, int nWid, bool bRealDraw/* = TRUE*/, int nStyles/* = PS_SOLID*/)
        {
            if (ProgramState.ON_LINE_MIL == false)
            {
                return;
            }
            if (bRealDraw)
            {
                DrawOverlayCross(nCamIndex, nCamIndex, clPointPos, nLength, color, nWid, nStyles,0);
            }
            else
            {
                m_clMilDrawCross[nCamIndex].AddList(clPointPos, nLength, nWid, color);
            }
        }

        public void GrabRun()
        {
            //Thread Run
            //if (CAM_LOAD_CHECK)
            //{
            //    thread = new Thread(Run);
            //    thread.Start();
            //}
            
        }
        public void Run()
        {
            //try
            //{
            //    while (bGrabRun)
            //    {
                    
            //        MIL.MdigGrab(MilDigitizer, MilCamGrabImage[0]);
            //        MIL.MdigGrabWait(MilDigitizer, MIL.M_GRAB_END);// M_GRAB_END); M_GRAB_FRAME_END
            //        MIL.MimResize(MilCamGrabImageChild[0], MilCamSmallImageChild[0], m_CamReduceFactorX, m_CamReduceFactorY, MIL.M_DEFAULT);
            //        //MIL.MbufExport("d:\\milimage.bmp", MIL.M_BMP, MilCamGrabImage[0]);
            //        Thread.Sleep(10);
            //    }

            //}
            //catch (ThreadInterruptedException err)
            //{
            //    Debug.WriteLine(err);
            //}
            //finally
            //{
            //    Debug.WriteLine("time 리소스 지우기");
            //}

            
        }
        
        public void AllocMilApplication()
        {
            if (ProgramState.ON_LINE_MIL == false)
            {
                return;
            }
            int i = 0;
            MIL.MappAlloc(MIL.M_NULL, MIL.M_DEFAULT, ref MilApplication);//MIL.MappAlloc(MIL.M_DEFAULT, ref MilApplication);
            // Allocate defaults.
            MIL.MappControl(MIL.M_ERROR, MIL.M_PRINT_DISABLE);


            MIL.MsysAlloc(MIL.M_SYSTEM_GIGE_VISION, MIL.M_DEV0, MIL.M_COMPLETE, ref MilSystem);
            MAX_CAMERA_COUNT = (int)MIL.MsysInquire(MilSystem, MIL.M_DIGITIZER_NUM, MIL.M_NULL);
            if(MAX_CAMERA_COUNT > 0)
            {
                //
                MIL.MdigAlloc(MilSystem, MIL.M_DEFAULT, ("Henckel.dcf"), MIL.M_DEFAULT, ref MilDigitizer);
                MIL.MdigControl(MilDigitizer, MIL.M_GRAB_MODE, MIL.M_ASYNCHRONOUS); //M_SYNCHRONOUS); M_SYNCHRONOUS  M_ASYNCHRONOUS
                MIL.MdigControl(MilDigitizer, MIL.M_GRAB_TIMEOUT, 1000);

                MIL.MdigInquire(MilDigitizer, MIL.M_SIZE_X, ref m_nMilSizeX);
                MIL.MdigInquire(MilDigitizer, MIL.M_SIZE_Y, ref m_nMilSizeY);

                CAM_LOAD_CHECK = true;
            }
            else
            {
                m_nMilSizeX = CAM_SIZE_X;
                m_nMilSizeY = CAM_SIZE_Y;
                MAX_CAMERA_COUNT = 1;
            }
        
            MilCcdDisplay = new MIL_ID[MAX_CAMERA_COUNT];
            MilCamDisplay = new MIL_ID[MAX_CAMERA_COUNT];
            MilCamGrabImage = new MIL_ID[MAX_CAMERA_COUNT];
            MilCamGrabImageChild = new MIL_ID[MAX_CAMERA_COUNT];
            MilCamSmallImage = new MIL_ID[MAX_CAMERA_COUNT];
            MilCamSmallImageChild = new MIL_ID[MAX_CAMERA_COUNT];

            MilCamOverlay = new MIL_ID[MAX_CAMERA_COUNT];
            MilCamTransparent = new MIL_INT[MAX_CAMERA_COUNT];


            MilCcdOverlay = new MIL_ID[MAX_CAMERA_COUNT];
            MilCcdTransparent = new MIL_INT[MAX_CAMERA_COUNT];

            m_MilCcdGrabImage = new MIL_ID[1];
            m_MilSmallImage = new MIL_ID[1];
            m_MilCcdProcImage = new MIL_ID[1];
            m_MilCcdProcChild = new MIL_ID[1,3];


            m_pImgBuff[i] = new byte[3][]; // 복사할 데이터

            for (i = 0; i < MAX_CAMERA_COUNT; i++)
            {
                MilCcdDisplay[i] = MIL.M_NULL;
                MilCamDisplay[i] = MIL.M_NULL;
                MilCamGrabImage[i] = MIL.M_NULL;
                MilCamGrabImageChild[i] = MIL.M_NULL;
                MilCamSmallImage[i] = MIL.M_NULL;
                MilCamSmallImageChild[i] = MIL.M_NULL;
                MilCamOverlay[i] = MIL.M_NULL;
                MilCamTransparent[i] = MIL.M_NULL;

                MilCcdOverlay[i] = MIL.M_NULL;
                MilCcdTransparent[i] = MIL.M_NULL;
                
                m_MilCcdGrabImage[i] = MIL.M_NULL;
                m_MilSmallImage[i] = MIL.M_NULL;
                m_MilCcdProcImage[i] = MIL.M_NULL;
                m_MilCcdProcChild[i, 0] = MIL.M_NULL;
                m_MilCcdProcChild[i, 1] = MIL.M_NULL;
                m_MilCcdProcChild[i, 2] = MIL.M_NULL;
            }


        }
        public void AllocMilCCdBuffer(int nIndex, int nimageWidth, int nimageHeight) //Laon
        {
            if (ProgramState.ON_LINE_MIL == false)
            {
                return;
            }
            int i = 0;
            long lColorBand = MIL.M_RED;

            if (m_MilCcdGrabImage[nIndex] != MIL.M_NULL)
            {
                MIL.MbufFree(m_MilCcdGrabImage[nIndex]);  // 기존 메모리 해제
            }
            MIL.MbufAllocColor(MilSystem, 3, nimageWidth, nimageHeight, (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref m_MilCcdGrabImage[nIndex]);
            if (m_MilCcdGrabImage[nIndex] == MIL.M_NULL)
            {
                return;
            }
            MIL.MbufClear(m_MilCcdGrabImage[nIndex], 0);

            if (m_MilCcdProcImage[nIndex] != MIL.M_NULL)
            {
                for (i = 0; i < 3; i++)
                {
                    if (m_MilCcdProcChild[nIndex, i] != MIL.M_NULL)
                    {
                        MIL.MbufFree(m_MilCcdProcChild[nIndex, i]);  // 기존 메모리 해제
                    }
                }
                MIL.MbufFree(m_MilCcdProcImage[nIndex]);  // 기존 메모리 해제
            }
            MIL.MbufAllocColor(MilSystem, 3, nimageWidth, nimageHeight, (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref m_MilCcdProcImage[nIndex]);
            if (m_MilCcdProcImage[nIndex] == MIL.M_NULL)
            {
                return;
            }

            MIL.MbufClear(m_MilCcdProcImage[nIndex], 0);
            IntPtr[] visionImagePtr = new IntPtr[3];

            
            for (i = 0; i < 3; i++)
            {
                if (m_pImgBuff[0][i] != null)
                {
                    m_pImgBuff[0][i] = null;
                }
            }
            if (m_pImgBuff[0] != null)
            {
                m_pImgBuff[0] = null; // 복사할 데이터
            }
            m_pImgBuff[0] = new byte[3][]; // 복사할 데이터

            for (i = 0; i < 3; i++)
            {
                switch (i)
                {
                    case 0: lColorBand = MIL.M_RED; break;
                    case 1: lColorBand = MIL.M_GREEN; break;
                    case 2: lColorBand = MIL.M_BLUE; break;
                }
                
                MIL.MbufChildColor(m_MilCcdProcImage[nIndex], lColorBand, ref m_MilCcdProcChild[nIndex , i]);
                if (m_MilCcdProcChild[nIndex , i] == MIL.M_NULL)
                {
                    return;
                }

                MIL.MbufClear(m_MilCcdProcChild[nIndex, i], 0);

                int bufferSize = nimageWidth * nimageHeight; // 할당할 메모리의 크기

                

                m_pImgBuff[0][i] = new byte[bufferSize];
                IntPtr bufferPtr = Marshal.AllocCoTaskMem(bufferSize); // native heap으로부터 메모리를 할당받아,

                Marshal.Copy(bufferPtr, m_pImgBuff[0][i], 0, bufferSize);

                

                MIL_ID CompressedBufferId = MIL.M_NULL;
                MIL.MbufInquire(m_MilCcdProcChild[nIndex, i], MIL.M_HOST_ADDRESS, ref CompressedBufferId);// m_pImgBuff[0][i]);
                Marshal.Copy(CompressedBufferId, m_pImgBuff[0][i], 0, bufferSize);

                
            }
            if (m_MilSmallImage[0] != MIL.M_NULL)
            {
                MIL.MbufFree(m_MilSmallImage[0]);  // 기존 메모리 해제
            }

            MIL.MbufAllocColor(MilSystem, 3, Globalo.camControl.CcdPanel.Width, Globalo.camControl.CcdPanel.Height, (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref m_MilSmallImage[0]);
            MIL.MbufClear(m_MilSmallImage[0], 0);
        }
        public void AllocMilCamBuffer() //Align Cam
        {
            if (ProgramState.ON_LINE_MIL == false)
            {
                return;
            }
            long lBufferAttributes;
            if (CAM_LOAD_CHECK)
            {
                lBufferAttributes = MIL.M_IMAGE + MIL.M_GRAB + MIL.M_PROC;
            }
            else
            {
                lBufferAttributes = MIL.M_IMAGE + MIL.M_PROC;
            }
                

            MIL.MbufAlloc2d(MilSystem, CAM_SIZE_X, CAM_SIZE_Y, (8 + MIL.M_UNSIGNED), lBufferAttributes, ref MilCamGrabImage[0]);
            //           
            MIL.MbufClear(MilCamGrabImage[0], MIL.M_COLOR_BLACK);

            MIL.MbufChild2d(MilCamGrabImage[0], 0, 0, CAM_SIZE_X, CAM_SIZE_Y, ref MilCamGrabImageChild[0]);
            MIL.MbufClear(MilCamGrabImageChild[0], MIL.M_COLOR_BLACK);
            //
            MIL.MbufAlloc2d(MilSystem, SMALL_CAM_SIZE_X, SMALL_CAM_SIZE_Y, (8 + MIL.M_UNSIGNED), MIL.M_IMAGE + MIL.M_PROC + MIL.M_DISP, ref MilCamSmallImage[0]);
            MIL.MbufClear(MilCamSmallImage[0], MIL.M_COLOR_BLACK);

            MIL.MbufChild2d(MilCamSmallImage[0], 0, 0, SMALL_CAM_SIZE_X, SMALL_CAM_SIZE_Y, ref MilCamSmallImageChild[0]);
            MIL.MbufClear(MilCamSmallImageChild[0], MIL.M_COLOR_BLACK);

        }
        public void AllocMilCcdDisplay(IntPtr myPicHandler)//PictureControl myPic)
        {
            if (ProgramState.ON_LINE_MIL == false)
            {
                return;
            }
            MIL.MdispAlloc(MilSystem, MIL.M_DEFAULT, "M_DEFAULT", MIL.M_DEFAULT, ref MilCcdDisplay[0]);
            if (MilCcdDisplay[0] != MIL.M_NULL)
            {
                MIL_INT DisplayType = MIL.MdispInquire(MilCcdDisplay[0], MIL.M_DISPLAY_TYPE, MIL.M_NULL);

                if (DisplayType != MIL.M_WINDOWED)
                {
                    MIL.MdispFree(MilCcdDisplay[0]);
                    MilCcdDisplay[0] = MIL.M_NULL;

                    return;
                }
                MIL.MdispSelectWindow(MilCcdDisplay[0], m_MilSmallImage[0], myPicHandler);
            }
        }
        public void AllocMilCamDisplay(IntPtr myPicHandler)//Panel myPic)
        {
            if (ProgramState.ON_LINE_MIL == false)
            {
                return;
            }
            MIL.MdispAlloc(MilSystem, MIL.M_DEV0, "M_DEFAULT", MIL.M_DEFAULT, ref MilCamDisplay[0]);

            if (MilCamDisplay[0] != MIL.M_NULL)
            {
                MIL_INT DisplayType = MIL.MdispInquire(MilCamDisplay[0], MIL.M_DISPLAY_TYPE, MIL.M_NULL);

                if (DisplayType != MIL.M_WINDOWED)
                {
                    MIL.MdispFree(MilCamDisplay[0]);
                    MilCamDisplay[0] = MIL.M_NULL;

                    return;
                }

                MIL.MdispSelectWindow(MilCamDisplay[0], MilCamSmallImageChild[0], myPicHandler);//MilCamSmallImageChild[0], myPic.Handle);
            }
        }
        public void EnableCcdOverlay()
        {
            if (ProgramState.ON_LINE_MIL == false)
            {
                return;
            }
            MIL_INT DisplayType = MIL.MdispInquire(MilCcdDisplay[0], MIL.M_DISPLAY_TYPE, MIL.M_NULL);
            if (DisplayType == (MIL.M_WINDOWED | MIL.M_USER_WINDOW))
            {
                MIL.MdispControl(MilCcdDisplay[0], MIL.M_OVERLAY, MIL.M_ENABLE);
                MIL.MdispInquire(MilCcdDisplay[0], MIL.M_OVERLAY_ID, ref MilCcdOverlay[0]);
                MIL.MdispControl(MilCcdDisplay[0], MIL.M_OVERLAY_CLEAR, MIL.M_DEFAULT);
                MIL.MdispControl(MilCcdDisplay[0], MIL.M_OVERLAY_SHOW, MIL.M_ENABLE);
                MIL.MdispInquire(MilCcdDisplay[0], MIL.M_TRANSPARENT_COLOR, ref MilCcdTransparent[0]);
                MIL.MgraControl(MIL.M_DEFAULT, MIL.M_BACKGROUND_MODE, MIL.M_TRANSPARENT);
            }
        }
        
        public void EnableCamOverlay()
        {
            if (ProgramState.ON_LINE_MIL == false)
            {
                return;
            }
            MIL_INT DisplayType = MIL.MdispInquire(MilCamDisplay[0], MIL.M_DISPLAY_TYPE, MIL.M_NULL);
            if (DisplayType == (MIL.M_WINDOWED | MIL.M_USER_WINDOW))
            {
                MIL.MdispControl(MilCamDisplay[0], MIL.M_OVERLAY, MIL.M_ENABLE);
                MIL.MdispInquire(MilCamDisplay[0], MIL.M_OVERLAY_ID, ref MilCamOverlay[0]);
                MIL.MdispControl(MilCamDisplay[0], MIL.M_OVERLAY_CLEAR, MIL.M_DEFAULT);
                MIL.MdispControl(MilCamDisplay[0], MIL.M_OVERLAY_SHOW, MIL.M_ENABLE);
                MIL.MdispInquire(MilCamDisplay[0], MIL.M_TRANSPARENT_COLOR, ref MilCamTransparent[0]);
                //MIL.MbufClear(MilCamOverlay[0], (MIL_DOUBLE) MilCamTransparent[0]);
                MIL.MgraControl(MIL.M_DEFAULT, MIL.M_BACKGROUND_MODE, MIL.M_TRANSPARENT);
            }
        }
        public void DrawOverlay()
        {
            if (ProgramState.ON_LINE_MIL == false)
            {
                return;
            }
            IntPtr hCustomDC = IntPtr.Zero;
            // Draw MIL overlay annotations.
            //*********************************

            // Print a white string in the overlay image buffer.
            MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_WHITE);
            MIL.MgraText(MIL.M_DEFAULT, MilCamOverlay[0], CAM_SIZE_X / 9, CAM_SIZE_Y / 5, " -------------------- ");
            MIL.MgraText(MIL.M_DEFAULT, MilCamOverlay[0], CAM_SIZE_X / 9, CAM_SIZE_Y / 5 + 25, " - MIL Overlay Text - ");
            MIL.MgraText(MIL.M_DEFAULT, MilCamOverlay[0], CAM_SIZE_X / 9, CAM_SIZE_Y / 5 + 50, " -------------------- ");

            // Print a green string in the overlay image buffer.
            MIL.MgraColor(MIL.M_DEFAULT, MIL.M_COLOR_GREEN);
            MIL.MgraText(MIL.M_DEFAULT, MilCamOverlay[0], CAM_SIZE_X * 11 / 18, CAM_SIZE_Y / 5, " ---------------------");
            MIL.MgraText(MIL.M_DEFAULT, MilCamOverlay[0], CAM_SIZE_X * 11 / 18, CAM_SIZE_Y / 5 + 25, " - MIL Overlay Text - ");
            MIL.MgraText(MIL.M_DEFAULT, MilCamOverlay[0], CAM_SIZE_X * 11 / 18, CAM_SIZE_Y / 5 + 50, " ---------------------");

            // Re-enable the overlay display after all annotations are done.
            //MIL.MdispControl(MilCamDisplay, MIL.M_OVERLAY_SHOW, MIL.M_ENABLE);

            // Draw GDI color overlay annotation.
            //***********************************

            // Set the graphic text background to transparent.
            //MIL.MgraControl(MIL.M_DEFAULT, MIL.M_BACKGROUND_MODE, MIL.M_TRANSPARENT);

            // Create a device context to draw in the overlay buffer with GDI.
            MIL.MbufControl(MilCamOverlay[0], MIL.M_DC_ALLOC, MIL.M_DEFAULT);

            // Inquire the device context.
            hCustomDC = (IntPtr)MIL.MbufInquire(MilCamOverlay[0], MIL.M_DC_HANDLE, MIL.M_NULL);

            MIL.MappControl(MIL.M_DEFAULT, MIL.M_ERROR, MIL.M_PRINT_ENABLE);

            // Perform operation if GDI drawing is supported.
            if (!hCustomDC.Equals(IntPtr.Zero))
            {
                // NOTE : The using blocks will automatically call the Dipose method on the GDI objects.
                //        This ensures that resources are freed even if an exception occurs.

                // Create a System.Drawing.Graphics object from the Device context
                using (Graphics DrawingGraphics = Graphics.FromHdc(hCustomDC))
                {
                    // Draw a blue cross.
                    using (Pen DrawingPen = new Pen(Color.Blue))
                    {
                        // Draw a blue cross in the overlay image
                        DrawingGraphics.DrawLine(DrawingPen, 0, (int)(CAM_SIZE_Y / 2), CAM_SIZE_X, (int)(CAM_SIZE_Y / 2));
                        DrawingGraphics.DrawLine(DrawingPen, (int)(CAM_SIZE_X / 2), 0, (int)(CAM_SIZE_X / 2), CAM_SIZE_Y);

                        // Prepare transparent text annotations.
                        // Define the Brushes and fonts used to draw text
                        using (SolidBrush LeftBrush = new SolidBrush(Color.Red))
                        {
                            using (SolidBrush RightBrush = new SolidBrush(Color.Yellow))
                            {
                                using (Font OverlayFont = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold))
                                {
                                    // Write text in the overlay image
                                    SizeF GDITextSize = DrawingGraphics.MeasureString("GDI Overlay Text", OverlayFont);
                                    DrawingGraphics.DrawString("GDI Overlay Text", OverlayFont, LeftBrush, System.Convert.ToInt32(CAM_SIZE_X / 4 - GDITextSize.Width / 2), System.Convert.ToInt32(CAM_SIZE_Y * 3 / 4 - GDITextSize.Height / 2));
                                    DrawingGraphics.DrawString("GDI Overlay Text", OverlayFont, RightBrush, System.Convert.ToInt32(CAM_SIZE_X * 3 / 4 - GDITextSize.Width / 2), System.Convert.ToInt32(CAM_SIZE_Y * 3 / 4 - GDITextSize.Height / 2));
                                }
                            }
                        }
                    }
                }
                //   // Delete device context.
                MIL.MbufControl(MilCamOverlay[0], MIL.M_DC_FREE, MIL.M_DEFAULT);

                //   // Signal MIL that the overlay buffer was modified.
                MIL.MbufControl(MilCamOverlay[0], MIL.M_MODIFIED, MIL.M_DEFAULT);
            }
        }
        public int FindRectPos(ref byte[] ChartImage, int nIndex)
        {
            int i = nIndex;
            //int nOffsetX = Globalo.dataManage.workData.m_clSfrInfo.m_nSizeX[nIndex] / 4;
            //int nOffsetY = Globalo.dataManage.workData.m_clSfrInfo.m_nSizeY[nIndex] / 4;

            //int nSearchX = Globalo.dataManage.workData.m_clSfrInfo.m_clPtOffset[nIndex].X - nOffsetX;
            //int nSearchY = Globalo.dataManage.workData.m_clSfrInfo.m_clPtOffset[nIndex].Y - nOffsetY;
            //int nWidth = Globalo.dataManage.workData.m_clSfrInfo.m_nSizeX[nIndex] + nOffsetX * 2;
            //int nHeight = Globalo.dataManage.workData.m_clSfrInfo.m_nSizeY[nIndex] + nOffsetY * 2;

            //Mat mOrgImage = new Mat(Globalo.mLaonGrabberClass.m_nHeight, Globalo.mLaonGrabberClass.m_nWidth, MatType.CV_8UC1, ChartImage);
            Mat mOrgImage = new Mat(Globalo.mLaonGrabberClass.m_nHeight, Globalo.mLaonGrabberClass.m_nWidth, MatType.CV_8UC1);//, ChartImage);
            mOrgImage.SetArray<byte>(ChartImage); // 배열 데이터를 Mat에 복사

            //Marshal.Copy(ChartImage,  0, mOrgImage.Data, oGlobal.mLaonGrabberClass.m_nHeight * oGlobal.mLaonGrabberClass.m_nWidth);
            //Rect Roirect = new Rect(nSearchX, nSearchY, nWidth, nHeight);
            //Mat imgRoi = mOrgImage.SubMat(Roirect);
            //Cv2.NamedWindow("imgRoi", WindowFlags.Normal);
            //Cv2.ImShow("imgRoi", imgRoi);
            //Cv2.WaitKey(0);
            //Cv2.DestroyAllWindows();


            //Mat colorImage = new Mat(Globalo.mLaonGrabberClass.m_nHeight, Globalo.mLaonGrabberClass.m_nWidth, MatType.CV_8UC3);
            //Cv2.CvtColor(imgRoi, colorImage, ColorConversionCodes.GRAY2BGR); // 1채널을 3채널로 변환

            ////Conversion to grayscale.
            //Mat gray = new Mat();
            //if (imgRoi.Channels() == 3)
            //{
            //    gray = imgRoi.CvtColor(ColorConversionCodes.BGR2GRAY);//3채널을 1채널로 변환
            //}
            //else
            //{
            //    gray = imgRoi.Clone();
            //}

            ////Blurring to reduce high frequency noise to make our contour detection process more accurate.
            //Mat blurred = new Mat();
            //blurred = gray.GaussianBlur(new OpenCvSharp.Size(5, 5), 0);
            ////Cv2.NamedWindow("blurred", WindowFlags.Normal);
            ////Cv2.ImShow("blurred", blurred);
            ////Cv2.WaitKey(0);
            ////Cv2.DestroyAllWindows();

            ////Binarization of the image.             
            //Mat threshold = new Mat();
            //threshold = blurred.Threshold(60, 255, ThresholdTypes.Binary);
            ////Cv2.NamedWindow("threshold", WindowFlags.Normal);
            ////Cv2.ImShow("threshold", threshold);
            ////Cv2.WaitKey(0);
            ////Cv2.DestroyAllWindows();

            ////find contours
            //OpenCvSharp.Point[][] contours;
            //HierarchyIndex[] hierarchyIndexes;
            //Cv2.FindContours(
            //    image: threshold,
            //    contours: out contours,
            //    hierarchy: out hierarchyIndexes,
            //    mode: RetrievalModes.CComp,//RetrievalModes.External, Tree , CComp
            //    method: ContourApproximationModes.ApproxSimple);



            //int contoursCount = contours.Count();
            //for (i = 0; i < contoursCount; i++)
            //{
            //    if (hierarchyIndexes[i].Parent < 0) continue;
            //    double peri = Cv2.ArcLength(contours[i], true);     //ArcLength = 윤곽선 길이 함수 , true일 경우 시작과 끝 연결된 것으로 간주
            //    OpenCvSharp.Point[] approx = Cv2.ApproxPolyDP(contours[i], 0.02 * peri, true);//0.04 * peri, true);
            //    if (approx.Length != 4)
            //    {
            //        continue;
            //    }
            //    Moments m = Cv2.Moments(contours[i]);
            //    OpenCvSharp.Point pnt = new OpenCvSharp.Point(m.M10 / m.M00, m.M01 / m.M00); //center point
            //    Cv2.Circle(colorImage, pnt, 5, Scalar.Yellow, 5);
                
            //    Rect rect;
                


                



            //    rect = Cv2.BoundingRect(approx);
            //    //string shape = GetShape(c); //*형상구분
            //    string shape = i.ToString();
            //    Cv2.PutText(colorImage, shape, pnt, HersheyFonts.HersheySimplex, 2, Scalar.White, 2);
            //    //hierarchyIndexes[0].Parent 값이 -1이 최외곽 항목이다

            //    string mstrLog = String.Format("[CCD] ChartRect #{0}  peri:{1:0.00}, approxLength:{2:0.00}", nIndex, peri, approx.Length);
            //    eLogSender("FindCircle", mstrLog);
            //    //Cv2.DrawContours(colorImage, contours, -1, Scalar.Blue, 3);//-1 은 전부다 그리기
            //    Cv2.DrawContours(colorImage, contours, i, Scalar.Blue, 3);

            //    //int roiSize = (int)cirlce_result[0].Radius;
            //    double cx = rect.X + nSearchX;
            //    double cy = rect.Y + nSearchY;

            //    Rectangle m_clRect = new Rectangle((int)(cx), (int)(cy), rect.Width, rect.Height);
            //    Globalo.dataManage.TaskWork.rtChartRect[nIndex] = m_clRect;

            //    Globalo.vision.DrawMOverlayBox(0, 0, m_clRect, Color.Blue, 1, true, 0, 1);
            //    System.Drawing.Point CenterP = new System.Drawing.Point(pnt.X + nSearchX, pnt.Y + nSearchY);

            //    Globalo.vision.DrawMOverlayCross(0, 0, CenterP, 10, Color.White, 1, false, 9);

                //Cv2.NamedWindow("colorImage", WindowFlags.Normal);
                //Cv2.ImShow("colorImage", colorImage);
                //Cv2.WaitKey(0);
                //Cv2.DestroyAllWindows();
            //}

            return 0;
        }
        public int FindSfrRectPos(int nIndex, Rectangle Chartroi)
        {
            //int nOffsetX = 0;
            //int nOffsetY = 0;

            //int nIndexLeft = 0;
            //int nIndexRight = 0;
            //int nIndexTop = 0;
            //int nIndexBottom = 0;

            //int iWidth = 0;
            //int iHeight = 0;
            //int mSizeOffsetX = 0;
            //int mSizeOffsetY = 0;

            //mSizeOffsetX = 50;  // model.m_iSize_ROI_X / 2;
            //mSizeOffsetY = 40;  // model.m_iSize_ROI_Y / 2;

            //iWidth = mSizeOffsetX;
            //iHeight = mSizeOffsetY;


            //if (nIndex == 0) { nIndexLeft = 3; nIndexRight = 4; nIndexTop = 1; nIndexBottom = 2; }
            //else if (nIndex == 1) { nIndexLeft = 0; nIndexRight = 5; nIndexTop = 0; nIndexBottom = 6; }
            //else if (nIndex == 2) { nIndexLeft = 7; nIndexRight = 0; nIndexTop = 0; nIndexBottom = 8; }
            //else if (nIndex == 3) { nIndexLeft = 0; nIndexRight = 9; nIndexTop = 10; nIndexBottom = 0; }
            //else if (nIndex == 4) { nIndexLeft = 11; nIndexRight = 0; nIndexTop = 12; nIndexBottom = 0; }
            //else if (nIndex == 5) { nIndexLeft = 0; nIndexRight = 13; nIndexTop = 0; nIndexBottom = 14; }
            //else if (nIndex == 6) { nIndexLeft = 15; nIndexRight = 0; nIndexTop = 0; nIndexBottom = 16; }
            //else if (nIndex == 7) { nIndexLeft = 0; nIndexRight = 17; nIndexTop = 18; nIndexBottom = 0; }
            //else if (nIndex == 8) { nIndexLeft = 19; nIndexRight = 0; nIndexTop = 20; nIndexBottom = 0; }

            ////oGlobal.mTaskWork.rtSfrSmallRect[0]

            //nIndexLeft--;
            //nIndexRight--;
            //nIndexTop--;
            //nIndexBottom--;

            //if (nIndexTop >= 0)
            //{
            //    if (Globalo.dataManage.workData.m_clSfrInfo.m_MTF_Direction[nIndexTop] == 0)
            //    {
            //        iWidth = mSizeOffsetY;
            //        iHeight = mSizeOffsetX;
            //    }
            //    //nOffsetX = 0;// model.m_MTF_ROI_Pos[nIndexTop].x;// RoiHalfWidth / 8;//센터라도 패턴 틀어진 각도때문에 정 센터로 안가서 offset 적용
            //    //nOffsetY = 0;// model.m_MTF_ROI_Pos[nIndexTop].y;// -RoiHalfHeight / 2;

            //    Globalo.dataManage.TaskWork.rtSfrSmallRect[nIndexTop].X = Chartroi.X + (Chartroi.Width / 2) - (iWidth / 2);
            //    Globalo.dataManage.TaskWork.rtSfrSmallRect[nIndexTop].Y = Chartroi.Y - (iHeight / 2);
            //    Globalo.dataManage.TaskWork.rtSfrSmallRect[nIndexTop].Width = iWidth - 1;
            //    Globalo.dataManage.TaskWork.rtSfrSmallRect[nIndexTop].Height = iHeight - 1;


            //    Globalo.vision.DrawMOverlayBox(0, 0, Globalo.dataManage.TaskWork.rtSfrSmallRect[nIndexTop], Color.GreenYellow, 1, false, 0, 1);
            //}
            //if (nIndexBottom >= 0)
            //{
            //    if (Globalo.dataManage.workData.m_clSfrInfo.m_MTF_Direction[nIndexBottom] == 0)
            //    {
            //        iWidth = mSizeOffsetY;
            //        iHeight = mSizeOffsetX;
            //    }
            //    Globalo.dataManage.TaskWork.rtSfrSmallRect[nIndexBottom].X = Chartroi.X + (Chartroi.Width / 2) - (iWidth / 2);
            //    Globalo.dataManage.TaskWork.rtSfrSmallRect[nIndexBottom].Y = Chartroi.Y + Chartroi.Height - (iHeight / 2);
            //    Globalo.dataManage.TaskWork.rtSfrSmallRect[nIndexBottom].Width = iWidth - 1;
            //    Globalo.dataManage.TaskWork.rtSfrSmallRect[nIndexBottom].Height = iHeight - 1;

            //    Globalo.vision.DrawMOverlayBox(0, 0, Globalo.dataManage.TaskWork.rtSfrSmallRect[nIndexBottom], Color.GreenYellow, 1, false, 0, 1);
            //}
            //if (nIndexLeft >= 0)
            //{
            //    if (Globalo.dataManage.workData.m_clSfrInfo.m_MTF_Direction[nIndexLeft] == 0)
            //    {
            //        iWidth = mSizeOffsetY;
            //        iHeight = mSizeOffsetX;
            //    }
            //    Globalo.dataManage.TaskWork.rtSfrSmallRect[nIndexLeft].X = Chartroi.X - (iWidth / 2);
            //    Globalo.dataManage.TaskWork.rtSfrSmallRect[nIndexLeft].Y = Chartroi.Y + (Chartroi.Height / 2) - (iHeight / 2);
            //    Globalo.dataManage.TaskWork.rtSfrSmallRect[nIndexLeft].Width = iWidth - 1;
            //    Globalo.dataManage.TaskWork.rtSfrSmallRect[nIndexLeft].Height = iHeight - 1;

            //    Globalo.vision.DrawMOverlayBox(0, 0, Globalo.dataManage.TaskWork.rtSfrSmallRect[nIndexLeft], Color.GreenYellow, 1, false, 0, 1);
            //}
            //if (nIndexRight >= 0)
            //{
            //    if (Globalo.dataManage.workData.m_clSfrInfo.m_MTF_Direction[nIndexRight] == 0)
            //    {
            //        iWidth = mSizeOffsetY;
            //        iHeight = mSizeOffsetX;
            //    }
            //    Globalo.dataManage.TaskWork.rtSfrSmallRect[nIndexRight].X = Chartroi.X + (Chartroi.Width - (iWidth / 2));
            //    Globalo.dataManage.TaskWork.rtSfrSmallRect[nIndexRight].Y = Chartroi.Y + (Chartroi.Height / 2) - (iHeight / 2);
            //    Globalo.dataManage.TaskWork.rtSfrSmallRect[nIndexRight].Width = iWidth - 1;
            //    Globalo.dataManage.TaskWork.rtSfrSmallRect[nIndexRight].Height = iHeight - 1;

            //    Globalo.vision.DrawMOverlayBox(0, 0, Globalo.dataManage.TaskWork.rtSfrSmallRect[nIndexRight], Color.GreenYellow, 1, false, 0, 1);

            //}
            return 0;
        }
        public int FineCirclePos(ref byte[] ChartImage)
        {
            int mRtn = 0;
            //eLogSender("FindCircle", "[CCD] FIND CIRCLE START");
            //int i = 0;
            ////int nPitch, nSizeX, nSizeY;
            ////nPitch = (int)MIL.MbufInquire(oGlobal.vision.m_MilCcdProcChild[0, 1], MIL.M_PITCH, MIL.M_NULL);
            ////nSizeX = (int)MIL.MbufInquire(oGlobal.vision.m_MilCcdProcChild[0, 1], MIL.M_SIZE_X, MIL.M_NULL);    //1820
            ////nSizeY = (int)MIL.MbufInquire(oGlobal.vision.m_MilCcdProcChild[0, 1], MIL.M_SIZE_Y, MIL.M_NULL);	//940
            ////MIL.MbufGet(oGlobal.vision.m_MilCcdProcChild[0, 1], oGlobal.vision.m_pImgBuff[0][1]);

            //Mat mOrgImage = new Mat(Globalo.mLaonGrabberClass.m_nHeight, Globalo.mLaonGrabberClass.m_nWidth, MatType.CV_8UC1);//, ChartImage);// oGlobal.vision.m_pImgBuff[0][1]);
            //mOrgImage.SetArray<byte>(ChartImage); // 배열 데이터를 Mat에 복사
            //HoughModes mothod = HoughModes.Gradient;
            //double dp = 1.0;        //분해능: 1을 사용할 경우 이미지와 동일한 크기 사용
            //double minDist = 100.0;  //원과 원사이의 최소 거리 입니다.
            //double param1 = 200.0;  //Edge 임계값: Canny Edge의 상위 임계값입니다. 하위 임계값은 상위 임계값의 절반으로 자동으로 설정
            //double param2 = 30.0;   //중심 임계값: CV_HOUGH_GRADIENT에 적용된 중심 hISTOGRAM에 대한 임계값
            //int minRadius = 0;  //검출된 원의 반지름 범위, 0을 입력시 모든 원을 검색합니다.
            //int maxRadius = 0;  //검출된 원의 반지름 범위, 0을 입력시 모든 원을 검색합니다.

            //for (i = 0; i < 4; i++)
            //{
            //    Rect Roirect = new Rect(Globalo.dataManage.workData.m_CircleP[i].X, Globalo.dataManage.workData.m_CircleP[i].Y, Globalo.dataManage.workData.m_CircleP[i].Width, Globalo.dataManage.workData.m_CircleP[i].Height);
            //    Mat imgRoi = mOrgImage.SubMat(Roirect);
            //    List<CircleSegment> cirlce_result = Cv2.HoughCircles(imgRoi, mothod, dp, minDist, param1, param2, minRadius, maxRadius).ToList();
            //    Mat result = new Mat();
            //    imgRoi.CopyTo(result);

            //    if (cirlce_result.Count == 1)
            //    {
            //        int roiSize = (int)cirlce_result[0].Radius;
            //        double cx = cirlce_result[0].Center.X + Globalo.dataManage.workData.m_CircleP[i].X;
            //        double cy = cirlce_result[0].Center.Y + Globalo.dataManage.workData.m_CircleP[i].Y;
            //        string mstrLog = String.Format("[CCD] Circle #{0}  x:{1:0.00}, y:{2:0.00}", i+1, cx, cy);
            //        //String.Format("[{0:D2}:{1:D2}] SIO-DI32", nBoardNo, i)
            //        System.Drawing.Point mPoint = new System.Drawing.Point((int)cx, (int)cy);
            //        Rectangle m_clRect = new Rectangle((int)(cx - roiSize), (int)(cy - roiSize), roiSize * 2, roiSize * 2);

            //        Globalo.vision.DrawMOverlayBox(0, 0, m_clRect, Color.Blue, 1, true, 0, 1);
            //        Globalo.vision.DrawMOverlayCross(0, 0, mPoint, 100, Color.Gray, 1, false, 9);
            //        eLogSender("FindCircle",  mstrLog);
            //        Cv2.Circle(result, cirlce_result[0].Center.ToPoint(), 30, Scalar.Yellow, 10, LineTypes.AntiAlias);
            //        OpenCvSharp.Point line1Pos;
            //        line1Pos.X = (int)(cirlce_result[0].Center.X - roiSize);
            //        line1Pos.Y = (int)(cirlce_result[0].Center.Y);
            //        OpenCvSharp.Point line2Pos;
            //        line2Pos.X = (int)(cirlce_result[0].Center.X + roiSize);
            //        line2Pos.Y = (int)(cirlce_result[0].Center.Y);


            //        Cv2.Line(result, line1Pos, line2Pos, Scalar.Yellow, 2);

            //        line1Pos.X = (int)(cirlce_result[0].Center.X);
            //        line1Pos.Y = (int)(cirlce_result[0].Center.Y - roiSize);
            //        line2Pos.X = (int)(cirlce_result[0].Center.X);
            //        line2Pos.Y = (int)(cirlce_result[0].Center.Y + roiSize);


            //        Cv2.Line(result, line1Pos, line2Pos, Scalar.Yellow, 2);

            //        //Cv2.NamedWindow("resu11lt", WindowFlags.Normal);
            //        //Cv2.ImShow("resu11lt", result);
            //        //Cv2.WaitKey(0);
            //        //Cv2.DestroyAllWindows();
            //    }
            //    else
            //    {
            //        //원 검출 실패
            //        mRtn = i + 1;
            //    }
            //}
            return mRtn;        //0이면 모두 찾았다.
        }
        public void ThreadStart()
        {
            if (ProgramState.ON_LINE_MIL == false)
            {
                return;
            }
            thread = new Thread(Run);
            thread.IsBackground = true;    //부모 종료시 스레드 종료
            thread.Start();
        }
        public void ThreadEnd()
        {
            if (ProgramState.ON_LINE_MIL == false)
            {
                return;
            }
            if (thread != null)
            {
                thread.Interrupt();
                thread.Join();
            }
            // Stop continuous grab.
            MIL.MdigHalt(MilDigitizer);

            // Remove the MIL buffer from the display.
            MIL.MdispSelect(MilCamDisplay[0], MIL.M_NULL);
            MIL.MdispSelect(MilCcdDisplay[0], MIL.M_NULL);

            // Free image.
            MIL.MbufFree(MilCamSmallImageChild[0]);
            MIL.MbufFree(MilCamGrabImageChild[0]);
            MIL.MbufFree(MilCamSmallImage[0]);
            MIL.MbufFree(MilCamGrabImage[0]);
            //
            MIL.MbufFree(m_MilCcdGrabImage[0]);
            MIL.MbufFree(m_MilSmallImage[0]);
            MIL.MbufFree(m_MilCcdProcChild[0, 0]);
            MIL.MbufFree(m_MilCcdProcChild[0, 1]);
            MIL.MbufFree(m_MilCcdProcChild[0, 2]);
            MIL.MbufFree(m_MilCcdProcImage[0]);


            if (MilDigitizer != MIL.M_NULL)
            {
                MIL.MdigFree(MilDigitizer);
            }

            MIL.MdispFree(MilCamDisplay[0]);
            MIL.MdispFree(MilCcdDisplay[0]);
            MIL.MsysFree(MilSystem);
            if (MilApplication != MIL.M_NULL)
            {
                MIL.MappFree(MilApplication);
            }
        }
    }
}
