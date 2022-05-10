using MaterialDesignThemes.Wpf;
using PCApp.ViewModels.Inventories;
using System;
using System.Windows;
using System.Windows.Input;

namespace PCApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {

        public ICommand LoadedWindowCommand { get; set; }
        public ICommand InventoryViewCommand { get; set; }
        public ICommand NewInventoryViewCommand { get; set; }
        public InventoryViewModel InventoryVM { get; set; }
        public NewInventoryViewModel NewInventoryVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainViewModel()
        {
            NewInventoryVM = new NewInventoryViewModel();
            InventoryVM = new InventoryViewModel();
            LoadedWindowCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
              
            });
            InventoryViewCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                CurrentView = InventoryVM;
            });
            NewInventoryViewCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                CurrentView = NewInventoryVM;
            });
        }

        public void OpenDiaLog(object content) => DialogHost.Show(content, "MainDialog");
        public void CloseDialog() => DialogHost.Close("MainDialog");


    }
}