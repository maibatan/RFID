using Newtonsoft.Json;
using PCApp.Helpers;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PCApp.ViewModels.Inventories
{
    public class NewInventoryViewModel : BaseViewModel
    {
        private ObservableCollection<Detail> _listDetail;
        public ObservableCollection<Detail> ListDetail { get => _listDetail; set { _listDetail = value; OnPropertyChanged(); } }
        private int _id;
        public int Id { get => _id; set { _id = value; OnPropertyChanged(); } }
        private string _inventoryDescription;
        public string InventoryDescription { get => _inventoryDescription; set { _inventoryDescription = value; OnPropertyChanged(); } }
        private Visibility _ProgressBarEnable = Visibility.Hidden;
        public Visibility ProgressBarEnable { get { return _ProgressBarEnable; } set { _ProgressBarEnable = value; OnPropertyChanged(); } }
        public ICommand AddCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        public NewInventoryViewModel()
        {
            _listDetail = new ObservableCollection<Detail>();
            SaveCommand = new RelayCommand<object>((p) =>
            {
                return true;
            },  (p) =>
            {
                (Application.Current.MainWindow.DataContext as MainViewModel).CloseDialog();
            });
            AddCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                (Application.Current.MainWindow.DataContext as MainViewModel).OpenDiaLog(new NewDetailViewModel());
            });
        }


 
    }
}
