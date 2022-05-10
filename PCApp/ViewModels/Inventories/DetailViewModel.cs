using Newtonsoft.Json;
using PCApp.Helpers;
using PCApp.Popup;
using SharedLibrary;
using System;
using System.Collections.ObjectModel;
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
        public ICommand DeleteCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand SaveCommad { get; set; }
        public ICommand PrintCommand { get; set; }
        public DetailViewModel(int id)
        {
            DetailViewCommand = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                ProgressBarEnable = Visibility.Visible;
                await LoadDataAsync(id);
                ProgressBarEnable = Visibility.Hidden;
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
