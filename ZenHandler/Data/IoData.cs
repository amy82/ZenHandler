using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml;
using System.IO;
//using Excel = Microsoft.Office.Interop.Excel;

namespace ZenHandler.Data
{
    public class IoData
    {
        public DataTable dataTable = new DataTable();
        public IoData()
        {

        }

        public void ReadEpplusData(string fileName)
        {
            // Excel 패키지의 라이센스 설정
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            //var filePath = "path_to_your_file.xlsx";
            FileInfo fileInfo = new FileInfo(fileName);

            dataTable.Columns.Add(new DataColumn("IN", typeof(string)));
            dataTable.Columns.Add(new DataColumn("OUT", typeof(string)));
            DataRow row;
            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                int rowCount = worksheet.Dimension.Rows;
                int colCount = worksheet.Dimension.Columns;
                for (int j = 2; j <= rowCount; j++)
                {
                    String temp = "";
                    row = dataTable.NewRow();

                    object obj = worksheet.Cells[j, 4].Value;// (worksheet.Cells[j, 4] as Excel.Range).Value2;

                    if (obj != null)
                    {
                        temp = obj.ToString();
                    }
                    row[0] = temp;
                    //
                    temp = "";
                    obj = worksheet.Cells[j, 9].Value;// (worksheet.Cells[j, 9] as Excel.Range).Value2;
                    if (obj != null)
                    {
                        temp = obj.ToString();
                    }
                    row[1] = temp;
                    dataTable.Rows.Add(row);
                    
                }
                //for (int nRow = 1; nRow <= rowCount; nRow++)
                //{
                //    for (int nCol = 1; nCol <= colCount; nCol++)
                //    {
                //        string nstr = worksheet.Cells[2, nCol].Value.ToString();
                //        Console.Write(worksheet.Cells[nRow, nCol].Value?.ToString() + "\t");
                //    }
                //    Console.WriteLine();
                //}
            }
        }
        public void ReadExcelData(string fileName)
        {
        //    application = new Excel.Application();
        //    application.DisplayAlerts = false;  // 알림 비활성화

        //    workbook = application.Workbooks.Open(Filename: @fileName);
        //    worksheet = workbook.Worksheets.get_Item("sheet1");    //물류IO
        //    application.Visible = false;
        //    range = worksheet.UsedRange;

        //    //4 =IN FUNCTION
        //    //8 = OUT FUNCTION 
        //    //두 줄만 가져오는 방식
        //    dataTable.Columns.Add(new DataColumn("IN", typeof(string)));
        //    dataTable.Columns.Add(new DataColumn("OUT", typeof(string)));
        //    DataRow row;
        //    // for (int i = 1; i <= range.Columns.Count; ++i)// for (int i = 1; i <= range.Rows.Count; ++i)
        //    //  {
        //    for (int j = 2; j <= range.Rows.Count; ++j)// for (int j = 1; j <= range.Columns.Count; ++j)
        //    {
        //        String temp = "";
        //        row = dataTable.NewRow();
        //        //  if (i == 4)// || i == 9)
        //        {
        //            object obj = (range.Cells[j, 4] as Excel.Range).Value2;

        //            if (obj != null)
        //            {
        //                temp = obj.ToString();
        //            }
        //            row[0] = temp;
        //            //
        //            temp = "";
        //            obj = (range.Cells[j, 9] as Excel.Range).Value2;
        //            if (obj != null)
        //            {
        //                temp = obj.ToString();
        //            }
        //            row[1] = temp;
        //            dataTable.Rows.Add(row);
        //        }
        //    }


        //    //foreach (var process in Process.GetProcessesByName("EXCEL"))
        //    //{
        //    //    Trace.WriteLine(process.ProcessName);
        //    //    //process.Kill();
        //    //}
        //    foreach (var process in Process.GetProcesses())
        //    {
        //        Console.WriteLine("프로세스 이름: " + process.ProcessName);
        //        Console.WriteLine("프로세스 Title: " + process.MainWindowTitle);
        //        //if (process.ProcessName.Contains("EXCEL", StringComparison.OrdinalIgnoreCase))
        //        //{
        //        //    // 프로세스 이름을 로그에 출력합니다.
        //        //    Console.WriteLine("프로세스 이름: " + process.ProcessName);
        //        //}
        //    }
        //    // 객체 해제 순서
        //    try
        //    {
        //        //throw new ArgumentException("exception");

        //        if (range != null)
        //        {
        //            DeleteObject(range);
        //            range = null;
        //        }

        //        if (worksheet != null)
        //        {
        //            DeleteObject(worksheet);
        //            worksheet = null;
        //        }
        //        if (workbook != null)
        //        {
        //            application.Workbooks.Close();
        //            workbook.Close(false);  // SaveChanges: false
        //            DeleteObject(workbook);
        //            workbook = null;
        //        }

                
        //    }
        //    catch (Exception)
        //    {
        //    }
        //    finally
        //    {
        //        //app.Workbooks.Close();
        //        //app.Quit();
        //        if (application != null)
        //        {
                    
        //            application.Quit();
        //            DeleteObject(application);
        //            application = null;
        //        }
        //    }
        //    //DeleteObject(worksheet);
        //    //application.Quit();
        //    //workbook.Close(false, Type.Missing, Type.Missing);
        //    //DeleteObject(workbook);


        //    //DeleteObject(application);

        //    // 모든 Excel 프로세스 종료

        }
    }
}
