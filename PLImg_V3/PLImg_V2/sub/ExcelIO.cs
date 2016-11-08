using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace PLImg_V2
{
    public class ExcelIO
    {
        public void ExcelExport(List<object> inputX)
        {
            Excel.Application excelApp = null;
            Excel.Workbook wb = null;
            Excel.Worksheet ws = null;

            try
            {
                excelApp = new Excel.Application();
                wb = excelApp.Workbooks.Add();
                ws = wb.Worksheets.get_Item(1) as Excel.Worksheet;

                Parallel.For(1, inputX.Count + 1, i =>
                {
                    ws.Cells[i, 1] = inputX[i - 1];

                });

                SaveFileDialog savedia = new SaveFileDialog();
                if (savedia.ShowDialog() == DialogResult.OK)
                {
                    wb.SaveAs(savedia.FileName, Excel.XlFileFormat.xlWorkbookNormal);
                    wb.Close(true);
                    excelApp.Quit();
                }
            }
            finally
            {
                // Clean up
                ReleaseExcelObject(ws);
                ReleaseExcelObject(wb);
                ReleaseExcelObject(excelApp);
            }
        }

        public void ExcelExport(List<object> inputX,List<object> inputY)
        {
            Excel.Application excelApp = null;
            Excel.Workbook wb = null;
            Excel.Worksheet ws = null;

            try
            {
                excelApp = new Excel.Application();
                wb = excelApp.Workbooks.Add();
                ws = wb.Worksheets.get_Item(1) as Excel.Worksheet;

                Parallel.For(1, inputX.Count + 1, i =>
                {
                    ws.Cells[i, 1] = inputX[i - 1];
                    ws.Cells[i, 2] = inputY[i - 1];

                });

                SaveFileDialog savedia = new SaveFileDialog();
                if (savedia.ShowDialog() == DialogResult.OK)
                {
                    wb.SaveAs(savedia.FileName, Excel.XlFileFormat.xlWorkbookNormal);
                    wb.Close(true);
                    excelApp.Quit();
                }
            }
            finally
            {
                // Clean up
                ReleaseExcelObject(ws);
                ReleaseExcelObject(wb);
                ReleaseExcelObject(excelApp);
            }
        }

        public void ExcelExport(byte[] inputX)
        {
            Excel.Application excelApp = null;
            Excel.Workbook wb = null;
            Excel.Worksheet ws = null;

            try
            {
                excelApp = new Excel.Application();
                wb = excelApp.Workbooks.Add();
                ws = wb.Worksheets.get_Item(1) as Excel.Worksheet;

                Console.WriteLine(inputX.Length);
                Parallel.For(1, inputX.Length + 1, i =>
                 {
                     ws.Cells[i, 1] = inputX[i - 1];

                 });

                SaveFileDialog savedia = new SaveFileDialog();
                if (savedia.ShowDialog() == DialogResult.OK)
                {
                    wb.SaveAs(savedia.FileName, Excel.XlFileFormat.xlWorkbookNormal);
                    wb.Close(true);
                    excelApp.Quit();
                }
            }
            finally
            {
                ReleaseExcelObject(ws);
                ReleaseExcelObject(wb);
                ReleaseExcelObject(excelApp);
            }
        }

        public void ExcelExport(byte[] inputX, byte[] inputY)
        {
            Excel.Application excelApp = null;
            Excel.Workbook wb = null;
            Excel.Worksheet ws = null;

            try
            {
                excelApp = new Excel.Application();
                wb = excelApp.Workbooks.Add();
                ws = wb.Worksheets.get_Item(1) as Excel.Worksheet;

                Parallel.For(1, inputX.Length + 1, i =>
                {
                    ws.Cells[i, 1] = inputX[i - 1];
                    ws.Cells[i, 2] = inputY[i - 1];
                });

                SaveFileDialog savedia = new SaveFileDialog();
                if (savedia.ShowDialog() == DialogResult.OK)
                {
                    wb.SaveAs(savedia.FileName, Excel.XlFileFormat.xlWorkbookNormal);
                    wb.Close(true);
                    excelApp.Quit();
                }
            }
            finally
            {
                // Clean up
                ReleaseExcelObject(ws);
                ReleaseExcelObject(wb);
                ReleaseExcelObject(excelApp);
            }
        }

        public void ExcelExport(double[] inputX)
        {
            Excel.Application excelApp = null;
            Excel.Workbook wb = null;
            Excel.Worksheet ws = null;

            try
            {
                excelApp = new Excel.Application();
                wb = excelApp.Workbooks.Add();
                ws = wb.Worksheets.get_Item(1) as Excel.Worksheet;

                Parallel.For(1, inputX.Length + 1, i =>
                {
                    ws.Cells[i, 1] = inputX[i - 1];

                });

                SaveFileDialog savedia = new SaveFileDialog();
                if (savedia.ShowDialog() == DialogResult.OK)
                {
                    wb.SaveAs(savedia.FileName, Excel.XlFileFormat.xlWorkbookNormal);
                    wb.Close(true);
                    excelApp.Quit();
                }
            }
            finally
            {
                // Clean up
                ReleaseExcelObject(ws);
                ReleaseExcelObject(wb);
                ReleaseExcelObject(excelApp);
            }
        }

        public void ExcelExport(double[] inputX, double[] inputY)
        {
            Excel.Application excelApp = null;
            Excel.Workbook wb = null;
            Excel.Worksheet ws = null;

            try
            {
                excelApp = new Excel.Application();
                wb = excelApp.Workbooks.Add();
                ws = wb.Worksheets.get_Item(1) as Excel.Worksheet;

                Parallel.For(1, inputX.Length + 1, i =>
                {
                    ws.Cells[i, 1] = inputX[i - 1];
                    ws.Cells[i, 2] = inputY[i - 1];
                });

                SaveFileDialog savedia = new SaveFileDialog();
                if (savedia.ShowDialog() == DialogResult.OK)
                {
                    wb.SaveAs(savedia.FileName, Excel.XlFileFormat.xlWorkbookNormal);
                    wb.Close(true);
                    excelApp.Quit();
                }
            }
            finally
            {
                // Clean up
                ReleaseExcelObject(ws);
                ReleaseExcelObject(wb);
                ReleaseExcelObject(excelApp);
            }
        }

        public void ExcelExportDir(List<object> inputX,string path)
        {
            Excel.Application excelApp = null;
            Excel.Workbook wb = null;
            Excel.Worksheet ws = null;

            try
            {
                excelApp = new Excel.Application();
                wb = excelApp.Workbooks.Add();
                ws = wb.Worksheets.get_Item(1) as Excel.Worksheet;

                Parallel.For(1, inputX.Count + 1, i =>
                {
                    ws.Cells[i, 1] = inputX[i - 1];
                });


                wb.SaveAs(path, Excel.XlFileFormat.xlWorkbookNormal);
                wb.Close(true);
                excelApp.Quit();

            }
            finally
            {
                // Clean up
                ReleaseExcelObject(ws);
                ReleaseExcelObject(wb);
                ReleaseExcelObject(excelApp);
            }
        }

        public void ExcelExportDir(List<object> inputX, List<object> inputY,string path)
        {
            Excel.Application excelApp = null;
            Excel.Workbook wb = null;
            Excel.Worksheet ws = null;

            try
            {
                excelApp = new Excel.Application();
                wb = excelApp.Workbooks.Add();
                ws = wb.Worksheets.get_Item(1) as Excel.Worksheet;
                
                Parallel.For(1, inputX.Count + 1, i =>
                {
                    ws.Cells[i, 1] = inputX[i - 1];
                    ws.Cells[i, 2] = inputY[i - 1];
                });

                SaveFileDialog savedia = new SaveFileDialog();

                wb.SaveAs(path, Excel.XlFileFormat.xlWorkbookNormal);
                wb.Close(true);
                excelApp.Quit();

            }
            finally
            {
                // Clean up
                ReleaseExcelObject(ws);
                ReleaseExcelObject(wb);
                ReleaseExcelObject(excelApp);
            }
        }

        public void ExcelExportDir(byte[] inputX, string path)
        {
            Excel.Application excelApp = null;
            Excel.Workbook wb = null;
            Excel.Worksheet ws = null;

            try
            {
                excelApp = new Excel.Application();
                wb = excelApp.Workbooks.Add();
                ws = wb.Worksheets.get_Item(1) as Excel.Worksheet;

                Parallel.For(1, inputX.Length + 1, i =>
                {
                    ws.Cells[i, 1] = inputX[i - 1];
                });
                wb.SaveAs(path, Excel.XlFileFormat.xlWorkbookNormal);
                wb.Close(true);
                excelApp.Quit();

            }
            finally
            {
                // Clean up
                ReleaseExcelObject(ws);
                ReleaseExcelObject(wb);
                ReleaseExcelObject(excelApp);
            }
        }

        public void ExcelExportDir(double[] inputX, string path)
        {
            Excel.Application excelApp = null;
            Excel.Workbook wb = null;
            Excel.Worksheet ws = null;

            try
            {
                excelApp = new Excel.Application();
                wb = excelApp.Workbooks.Add();
                ws = wb.Worksheets.get_Item(1) as Excel.Worksheet;

                Parallel.For(1, inputX.Length + 1, i =>
                {
                    ws.Cells[i, 1] = inputX[i - 1];
                });


                wb.SaveAs(path, Excel.XlFileFormat.xlWorkbookNormal);
                wb.Close(true);
                excelApp.Quit();

            }
            finally
            {
                // Clean up
                ReleaseExcelObject(ws);
                ReleaseExcelObject(wb);
                ReleaseExcelObject(excelApp);
            }
        }

        public void ExcelExportDir(byte[] inputX, byte[] inputY, string path)
        {
            Excel.Application excelApp = null;
            Excel.Workbook wb = null;
            Excel.Worksheet ws = null;

            try
            {
                excelApp = new Excel.Application();
                wb = excelApp.Workbooks.Add();
                ws = wb.Worksheets.get_Item(1) as Excel.Worksheet;
                
                Parallel.For(1, inputX.Length + 1, i =>
                {
                    ws.Cells[i, 1] = inputX[i - 1];
                    ws.Cells[i, 2] = inputY[i - 1];
                });

                SaveFileDialog savedia = new SaveFileDialog();

                wb.SaveAs(path, Excel.XlFileFormat.xlWorkbookNormal);
                wb.Close(true);
                excelApp.Quit();

            }
            finally
            {
                // Clean up
                ReleaseExcelObject(ws);
                ReleaseExcelObject(wb);
                ReleaseExcelObject(excelApp);
            }
        }

        public void ExcelExportDir(double[] inputX, double[] inputY, string path)
        {
            Excel.Application excelApp = null;
            Excel.Workbook wb = null;
            Excel.Worksheet ws = null;

            try
            {
                excelApp = new Excel.Application();
                wb = excelApp.Workbooks.Add();
                ws = wb.Worksheets.get_Item(1) as Excel.Worksheet;
               
                Parallel.For(1, inputX.Length + 1, i =>
                {
                    ws.Cells[i, 1] = inputX[i - 1];
                    ws.Cells[i, 2] = inputY[i - 1];
                });
                SaveFileDialog savedia = new SaveFileDialog();

                wb.SaveAs(path, Excel.XlFileFormat.xlWorkbookNormal);
                wb.Close(true);
                excelApp.Quit();
            }
            finally
            {
                // Clean up
                ReleaseExcelObject(ws);
                ReleaseExcelObject(wb);
                ReleaseExcelObject(excelApp);
            }
        }

        private void ReleaseExcelObject(object obj)
        {
            try
            {
                if (obj != null)
                {
                    Marshal.ReleaseComObject(obj);
                    obj = null;
                }
            }
            catch (Exception ex)
            {
                obj = null;
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
