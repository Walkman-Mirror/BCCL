using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MSExcel = Microsoft.Office.Interop.Excel;

namespace BCCL.Operations.Excel
{
    /// <summary>
    /// Provides functionality for exporting to XLS
    /// Be sure to add a reference to Excel 11 (2003) or Excel 12 (2007) to your project
    /// </summary>
    public static class ExcelFromDataSet
    {
        #region Public Methods

        public static void ExportXLS(this DataSet dataset, string filename)
        {
            MSExcel.Application app = new MSExcel.Application();
            MSExcel.Workbook wb = app.Workbooks.Add(1);
            //app.Visible = true;
            foreach (DataTable table in dataset.Tables)
            {
                MSExcel.Worksheet ws = (MSExcel.Worksheet)wb.Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                ws.Name = table.TableName;

                string start = "A1";
                string end = ColLetter(table.Columns.Count) + (table.Rows.Count + 1).ToString("0");

                var arr = table.Table2DArray();
                ws.get_Range(start, end).Value2 = arr;

                ws.Cells.Select();
                ws.Cells.AutoFilter(1, Type.Missing, MSExcel.XlAutoFilterOperator.xlAnd, Type.Missing, true);
                //ws.AutoFilter;
                //ws.get_Range(start, end).AutoFilter();
                //ws.get_Range(start, end).AutoFilter(1, Type.Missing, Excel.XlAutoFilterOperator.xlAnd, Type.Missing, true);

                for (int col = 0; col < table.Columns.Count; col++)
                {
                    ws.get_Range(ColLetter(col + 1) + "1", ColLetter(col + 1) + "1").EntireColumn.NumberFormat = numFormat(table.Columns[col].DataType.ToString());
                }

                MSExcel.Range header = (MSExcel.Range)ws.get_Range("A1", ColLetter(table.Columns.Count) + "1");
                header.Activate();
                header.Font.Bold = true;

                app.ActiveWindow.SplitColumn = 0;
                app.ActiveWindow.SplitRow = 1;
                app.ActiveWindow.FreezePanes = true;
            }

            wb.SaveAs(filename, MSExcel.XlFileFormat.xlExcel8, Type.Missing, Type.Missing, Type.Missing, Type.Missing, MSExcel.XlSaveAsAccessMode.xlNoChange, MSExcel.XlSaveConflictResolution.xlLocalSessionChanges, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            wb.Close(false, Type.Missing, Type.Missing);
            app.Quit();
        }

        #endregion

        #region Private Methods

        private static string ColLetter(int col)
        {
            string letter = string.Empty;
            int alpha = 0;
            int remainder = 0;
            alpha = col / 27;
            remainder = col - (alpha * 26);

            if (alpha > 0)
                letter = ((char)(alpha + 64)).ToString();

            if (remainder > 0)
                letter = letter + ((char)(remainder + 64)).ToString();

            return letter;
        }

        private static string numFormat(string type)
        {
            string format = "General";
            switch (type.ToString())
            {
                case "System.Int32": format = "#,##0"; break;
                case "System.Int64": format = "#,##0"; break;
                case "System.Decimal": format = "#,##0.00"; break;
                case "System.Double": format = "#,##0.00"; break;
                case "System.DateTime": format = "mm/dd/yyyy"; break;
                case "Currency": format = "$#,##0.00_);($#,##0.00)"; break;
                case "Accounting": format = "_($* #,##0.00_);_($* (#,##0.00);_($* \"-\"??_);_(@_)"; break;
                case "System.String": format = "@"; break;
            }

            return format;
        }

        private static object[,] Table2DArray(this DataTable table)
        {
            var array = new object[table.Rows.Count + 1, table.Columns.Count];

            for (int j = 0; j < table.Columns.Count; j++)
                array[0, j] = table.Columns[j].ColumnName;

            for (int i = 0; i < table.Rows.Count; ++i)
                for (int j = 0; j < table.Columns.Count; ++j)
                {
                    if (table.Rows[i][j] == null || table.Rows[i][j] == DBNull.Value || string.IsNullOrWhiteSpace((table.Rows[i][j] ?? string.Empty).ToString()))
                    {
                        if (table.Columns[j].DataType.ToString() == "System.Double" ||
                            table.Columns[j].DataType.ToString() == "System.Int32" ||
                            table.Columns[j].DataType.ToString() == "System.Int64" ||
                            table.Columns[j].DataType.ToString() == "System.Single" ||
                            table.Columns[j].DataType.ToString() == "System.Float")
                            array[i + 1, j] = 0;
                        else
                            array[i + 1, j] = string.Empty;
                    }
                    else
                    {
                        if (table.Columns[j].DataType.ToString().Contains("System."))
                            array[i + 1, j] = table.Rows[i][j];
                        else
                            array[i + 1, j] = table.Rows[i][j].ToString();
                    }
                }

            return array;
        }

        #endregion
    }
}
