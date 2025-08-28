using ClosedXML.Excel;
using eGranjaCAT.Application.ExportMappings;


namespace eGranjaCAT.Infrastructure.Services
{
    public class ExcelService : IExcelService
    {
        public async Task<MemoryStream> GenerateExcelAsync<T>(IEnumerable<T> data, List<ExcelColumnMap<T>> maps, string sheetName)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(sheetName);

            int totalColumns = maps.Count;

            var titleCell = worksheet.Range(1, 1, 1, totalColumns).Merge();
            titleCell.Value = "eGranjaCAT";
            titleCell.Style.Font.Bold = true;
            titleCell.Style.Font.FontSize = 20;
            titleCell.Style.Font.FontColor = XLColor.White;
            titleCell.Style.Fill.BackgroundColor = XLColor.Green;
            titleCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            titleCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;


            var exportDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            var leftCell = worksheet.Range(2, 1, 2, totalColumns / 2).Merge();
            leftCell.Value = $"Exportació: {exportDate}";
            leftCell.Style.Font.Italic = true;
            leftCell.Style.Font.FontSize = 11;
            leftCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            leftCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            var rightCell = worksheet.Range(2, (totalColumns / 2) + 1, 2, totalColumns).Merge();
            rightCell.Value = "SIGE Porcí propi";
            rightCell.Style.Font.Italic = true;
            rightCell.Style.Font.FontSize = 11;
            rightCell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            rightCell.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            int tableStartRow = 4;

            for (int col = 0; col < totalColumns; col++)
            {
                var cell = worksheet.Cell(tableStartRow, col + 1);
                cell.Value = maps[col].Header;

                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.LightGray;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                cell.Style.Border.TopBorder = XLBorderStyleValues.Thin;
                cell.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                cell.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                cell.Style.Border.RightBorder = XLBorderStyleValues.Thin;

                cell.Style.Border.TopBorderColor = XLColor.Black;
                cell.Style.Border.BottomBorderColor = XLColor.Black;
                cell.Style.Border.LeftBorderColor = XLColor.Black;
                cell.Style.Border.RightBorderColor = XLColor.Black;
            }


            int row = tableStartRow + 1;
            foreach (var item in data)
            {
                for (int col = 0; col < totalColumns; col++)
                {
                    var value = maps[col].ValueSelector(item);
                    var cell = worksheet.Cell(row, col + 1);
                    cell.Value = value?.ToString() ?? "";

                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    cell.Style.Font.FontName = "Arial";
                    cell.Style.Font.FontSize = 12;
                    cell.Style.Fill.BackgroundColor = XLColor.White;

                    cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    cell.Style.Border.OutsideBorderColor = XLColor.Black;
                }

                row++;
            }

            worksheet.Columns().AdjustToContents();

            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;
            return stream;
        }
    }
}