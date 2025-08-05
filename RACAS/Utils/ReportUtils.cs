using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace RACAS.Utils
{
    public class ReportUtils
    {
        public static ExcelWorksheet createExcelHeader(List<String> headers, ExcelWorksheet worksheet, int line)
        {
            for (int i = 0; i < headers.Count; i++)
            {
                worksheet.Cells[line, i+1].Value = headers[i];
            }

            using (var range = worksheet.Cells[line, 1, line, headers.Count])
            {
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                range.Style.Font.Color.SetColor(Color.Black);
                range.Style.Font.Bold = true;
            }

            return worksheet;
        }

        public static ExcelWorksheet createLine(List<object> content, ExcelWorksheet worksheet, int line)
        {
            for (int i = 0; i < content.Count; i++)
            {
                worksheet.Cells[line, i + 1].Value = content[i];
            }
            return worksheet;
        }
    }
}
