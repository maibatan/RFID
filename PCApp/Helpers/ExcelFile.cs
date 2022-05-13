using OfficeOpenXml;
using SharedLibrary;
using System;
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
            ws.Cells[1,1].Value = inventory.Description;
            ws.Cells[2, 1].Value= inventory.CreatedDate.ToString("hh:mm tt MM/dd/yyyy");
            ws.Cells[3, 1].Value = "No";
            ws.Cells[3, 2].Value = "Item Name";
            ws.Cells[3, 3].Value = "Expected";
            ws.Cells[3, 4].Value = "Physical";
            ws.Cells[3, 5].Value = "Gap";
            for (int i =0; i < inventory.Details.Count; i++)
            {
                ws.Cells[i + 4, 1].Value =(i + 1).ToString();
                ws.Cells[i + 4, 2].Value =details[i].AssetItemNavigation.DisplayName;
                ws.Cells[i + 4, 3].Value= details[i].ExpectedQuality;
                ws.Cells[i + 4, 4].Value = details[i].PhysicalQuality;
                ws.Cells[i + 4, 5].Value = (int)ws.Cells[i + 4, 3].Value - (int)ws.Cells[i + 4, 4].Value;
            }
          
            await package.SaveAsync();
        }
    }
}
