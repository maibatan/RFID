using Newtonsoft.Json;
using PCApp.Helpers;
using SharedLibrary;
using System;
using System.Collections.ObjectModel;
using PCApp.Converters;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using PCApp.Popup;
using static PCApp.Popup.MsgBox;

namespace PCApp.ViewModels.Inventories
{
    public class InventoryViewModel : BaseViewModel
    {
        private ObservableCollection<Inventory> _listInventory;
        public ObservableCollection<Inventory> ListInventory { get => _listInventory; set { _listInventory = value; OnPropertyChanged(); } }
        private Visibility _ProgressBarEnable = Visibility.Hidden;
        public Visibility ProgressBarEnable { get { return _ProgressBarEnable; } set { _ProgressBarEnable = value; OnPropertyChanged(); } }
        public ICommand InventoryViewCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand CheckCommand { get; set; }
        public InventoryViewModel()
        {
            InventoryViewCommand = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                ProgressBarEnable = Visibility.Visible;
                await LoadDataAsync();
                ProgressBarEnable = Visibility.Hidden;
            });

            AddCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                (Application.Current.MainWindow.DataContext as MainViewModel).CurrentView =new NewInventoryViewModel();
            });
            DeleteCommand = new RelayCommand<int>((p) => { return true; }, async (id) =>
            {
                bool? Result = new MsgBox("Are you sure want to detele this inventory? ", MessageType.Confirmation, MessageButtons.YesNo).ShowDialog();
                if (Result.Value)
                {
                    await DeleteAsync(id);
                    await LoadDataAsync();
                }
            });
            CheckCommand = new RelayCommand<int>((p) => { return true; }, (id) =>
            {
                (Application.Current.MainWindow.DataContext as MainViewModel).CurrentView = new DetailViewModel(id);
            });

        }
        private async Task DeleteAsync(int id)
        {

            string url = APIConnection.URL + "Inventories/"+id;
            var httpClient = new HttpClient();
            try
            {
                var resultJson = await httpClient.DeleteAsync(url);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
        private async Task LoadDataAsync()
        {
           
            string url = APIConnection.URL + "Inventories";
            var httpClient = new HttpClient();
            try
            {
                var resultJson = await httpClient.GetStringAsync(url);
                var resultProduct = JsonConvert.DeserializeObject<Inventory[]>(resultJson);
                ListInventory = new ObservableCollection<Inventory>(resultProduct);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
    }
}
