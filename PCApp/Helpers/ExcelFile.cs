using OfficeOpenXml;
using SharedLibrary;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace PCApp.Helpers
{
    public class ExcelFile
    {
        public static async Task Export(FileInfo file, Inventory inventory, ObservableCollection<Detail> details)
        {
            if(file.Exists)
            {
                file.Delete();
            }
            using var package = new ExcelPackage(file);
            var ws = package.Workbook.Worksheets.Add("Inventory");
            int startRow = 1;
            int startCol = 1;
            ws.Cells[startRow,startCol,startRow,5].Value = inventory.Description;
            ws.Cells[startRow, startCol, startRow, 5].Merge = true;
            ws.Cells[startRow+1, startCol, startRow+1, 5].Value =  inventory.Status ? "Checked" : "Not checked";
            ws.Cells[startRow+1, startCol, startRow+1, 5].Merge = true;
            ws.Cells[startRow+2, startCol, startRow +2, 5].Value= inventory.CreatedDate.ToString("hh:mm tt MM/dd/yyyy");
            ws.Cells[startRow + 2, startCol, startRow +2, 5].Merge = true;
            ws.Cells[startRow+3, startCol].Value = "No";
            ws.Cells[startRow + 3, startCol+1].Value = "Item Name";
            ws.Cells[startRow + 3, startCol+2].Value = "Expected";
            ws.Cells[startRow + 3, startCol+3].Value = "Physical";
            ws.Cells[startRow + 3, startCol+4].Value = "Gap";
            for (int i =0; i < inventory.Details.Count; i++)
            {
                ws.Cells[i + startRow + 3, startCol].Value =(i + 1).ToString();
                ws.Cells[i + startRow + 3, startCol +1].Value =details[i].AssetItemNavigation.DisplayName;
                ws.Cells[i + startRow + 3, startCol +2].Value= details[i].ExpectedQuality;
                ws.Cells[i + startRow + 3, startCol +3].Value = details[i].PhysicalQuality;
                ws.Cells[i + startRow + 3, startCol +4].Value = (int)ws.Cells[i + 4, 3].Value - (int)ws.Cells[i + 4, 4].Value;
            }
          
            await package.SaveAsync();
        }
    }
}
