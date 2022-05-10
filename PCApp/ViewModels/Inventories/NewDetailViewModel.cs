using Newtonsoft.Json;
using PCApp.Helpers;
using PCApp.Popup;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static PCApp.Popup.MsgBox;

namespace PCApp.ViewModels.Inventories
{
    public class NewDetailViewModel: BaseViewModel
    {
        private ObservableCollection<Department> _listDepartment;
        public ObservableCollection<Department> ListDepartment{ get => _listDepartment; set { _listDepartment = value; OnPropertyChanged(); } }
        private ObservableCollection<AssetItem> _listAssetItem;
        public ObservableCollection<AssetItem> ListAssetItem { get => _listAssetItem; set { _listAssetItem = value; OnPropertyChanged(); } }
        private Department _selectedDepartment;
        public Department SelectedDepartment
        {
            get => _selectedDepartment; 
            set
            {
                _selectedDepartment = value;
                OnPropertyChanged();
            }
        }
        private AssetItem  _selectedAssetItem;
        public AssetItem SelectedAssetItem
        {
            get => _selectedAssetItem; 
            set
            {
                _selectedAssetItem = value;
                OnPropertyChanged();
            }
        }
        private Visibility _ProgressBarEnable = Visibility.Hidden;
        public Visibility ProgressBarEnable { get { return _ProgressBarEnable; } set { _ProgressBarEnable = value; OnPropertyChanged(); } }

        public ICommand DetailViewCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public NewDetailViewModel()
        {
            
        }
       
       
    }
}
