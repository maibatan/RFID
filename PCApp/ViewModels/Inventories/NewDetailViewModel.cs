using Newtonsoft.Json;
using PCApp.Helpers;
using PCApp.Popup;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static PCApp.Popup.MsgBox;

namespace PCApp.ViewModels.Inventories
{
    public class NewDetailViewModel: BaseViewModel
    {
        private ObservableCollection<Detail> _details;
        private bool _assetItemEnable = false;
        public bool AssetItemEnable { get => _assetItemEnable; set { _assetItemEnable = value; OnPropertyChanged(); } }
        private int _quality;
        public int Quality { get => _quality; set { _quality = value; OnPropertyChanged(); } }
        private ObservableCollection<AssetItem> _listAssetItem;
        public ObservableCollection<AssetItem> ListAssetItem { get => _listAssetItem; set { _listAssetItem = value; OnPropertyChanged(); } }

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

        public ICommand NewDetailViewCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CloseCommand { get; set; }
        public NewDetailViewModel(ObservableCollection<Detail> details)
        {
            _details = details;
            NewDetailViewCommand = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                ProgressBarEnable = Visibility.Visible;
                await LoadDataAsync();
                ProgressBarEnable = Visibility.Hidden;
            });
            SaveCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                if (_selectedAssetItem == null)
                {
                    new MsgBox("You must select item ", MessageType.Warning, MessageButtons.Ok).ShowDialog();
                    return;
                }
                if (Quality == 0)
                {
                    new MsgBox("Quality must not equal 0", MessageType.Warning, MessageButtons.Ok).ShowDialog();
                    return;
                }

                Detail newDetail = new Detail()
                {
                    AssetItemId = _selectedAssetItem.Id,
                    AssetItemNavigation = _selectedAssetItem,
                    ExpectedQuality = Quality
                };
                int detailposition = -1;
                for(int i=0; i  < _details.Count;i++)
                {
                    if (_details[i].AssetItemId == _selectedAssetItem.Id) detailposition = i;
                }
                if (detailposition >=0)
                {
                   newDetail.ExpectedQuality += _details[detailposition].ExpectedQuality;
                   _details.RemoveAt(detailposition);
                }
                
                _details.Add(newDetail);
                (Application.Current.MainWindow.DataContext as MainViewModel).CloseDialog();
            });
            CloseCommand = new RelayCommand<object>((p) =>
            {
                return true;
            }, (p) =>
            {
                (Application.Current.MainWindow.DataContext as MainViewModel).CloseDialog();
            });
        }
        private async Task LoadDataAsync()
        {
            string url = APIConnection.URL + "AssetItems";
            var httpClient = new HttpClient();
            try
            {
                var resultJson = await httpClient.GetStringAsync(url);
                var resultProduct = JsonConvert.DeserializeObject<AssetItem[]>(resultJson);
                ListAssetItem = new ObservableCollection<AssetItem>(resultProduct);
                AssetItemEnable = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
       
    }
}
