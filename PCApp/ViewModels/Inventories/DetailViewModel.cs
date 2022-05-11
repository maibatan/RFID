
using Microsoft.Win32;
using Newtonsoft.Json;
using OfficeOpenXml;
using PCApp.Helpers;
using PCApp.Popup;
using SharedLibrary;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static PCApp.Popup.MsgBox;

namespace PCApp.ViewModels.Inventories
{
    public class DetailViewModel : BaseViewModel
    {
        private Inventory _inventory;
        public Inventory Inventory { get => _inventory;  set { _inventory = value; OnPropertyChanged(); } }
        private ObservableCollection<Detail> _listDetail;
        public ObservableCollection<Detail> ListDetail { get => _listDetail; set { _listDetail = value; OnPropertyChanged(); } }
        private Visibility _ProgressBarEnable = Visibility.Hidden;
        public Visibility ProgressBarEnable { get { return _ProgressBarEnable; } set { _ProgressBarEnable = value; OnPropertyChanged(); } }
        public ICommand DetailViewCommand { get; set; }
        public ICommand PrintCommand { get; set; }
        public DetailViewModel(int id)
        {
            DetailViewCommand = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                ProgressBarEnable = Visibility.Visible;
                await LoadDataAsync(id);
                ProgressBarEnable = Visibility.Hidden;
            });
            PrintCommand = new RelayCommand<Window>((p) => { return true; }, async(p) =>
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Title = "Export Excel";
                saveFileDialog.Filter = "Excel (*.xlsx)|*.xlsx|Excel 2003 (*.xls)|*.xls";
                var result = saveFileDialog.ShowDialog();
                if((bool)result)
                {
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    var file = new FileInfo(saveFileDialog.FileName);
                    await ExcelFile.Export(file, _inventory, _listDetail);
                    new MsgBox("Save successful! ", MessageType.Success, MessageButtons.Ok).ShowDialog();
                }
                
            });
        }
        private async Task LoadDataAsync(int id)
        {

            string url = APIConnection.URL + "Inventories/"+id;
            var httpClient = new HttpClient();
            try
            {
                var resultJson = await httpClient.GetStringAsync(url);
                var resultProduct = JsonConvert.DeserializeObject<Inventory>(resultJson);
                Inventory = resultProduct;
                ListDetail = new ObservableCollection<Detail>(resultProduct.Details);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
    }
}
