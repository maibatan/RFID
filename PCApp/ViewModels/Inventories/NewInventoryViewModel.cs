﻿using Newtonsoft.Json;
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
        public ICommand AddDetailCommand { get; set; }
        public ICommand DeleteDetailCommand { get; set; }
        public ICommand SaveDetailCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand NewInventoryViewCommand { get; set; }
        public NewInventoryViewModel()
        {
            _listDetail = new ObservableCollection<Detail>();
            SaveDetailCommand = new RelayCommand<object>((p) =>{return true;}, async (p) =>
            {
                if (_inventoryDescription == "")
                {
                    new MsgBox("Must have decription ", MessageType.Warning, MessageButtons.Ok).ShowDialog();
                    return;
                }
                if (_listDetail.Count == 0)
                {
                    new MsgBox("Must have details ", MessageType.Warning, MessageButtons.Ok).ShowDialog();
                    return;
                }
                int id= await PostInventoryAsync(_inventoryDescription);
                if(id == 0)
                {
                    new MsgBox("Can't create inventory ", MessageType.Error, MessageButtons.Ok).ShowDialog();
                    return;
                }
                await PostDetailsAsync(id, _listDetail);
                Clear();
                (Application.Current.MainWindow.DataContext as MainViewModel).CurrentView = new InventoryViewModel();
            });
            AddDetailCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                (Application.Current.MainWindow.DataContext as MainViewModel).OpenDiaLog(new NewDetailViewModel(ListDetail));
            });
            ClearCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Clear();
            });
           
        }
        private void Clear()
        {
            InventoryDescription = "";
            ListDetail.Clear();
        }
        private async Task<int> PostInventoryAsync(string description)
        {
            string url = APIConnection.URL + "Inventories";
            var httpClient = new HttpClient();
            try
            {
                var jsonData = JsonConvert.SerializeObject(new Inventory() { Description = description, CreatedDate = DateTime.Now });
                var data = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, data);
                string result = await response.Content.ReadAsStringAsync();
                Console.WriteLine(result);
                var resultProduct = JsonConvert.DeserializeObject<Inventory>(result);
                return resultProduct.Id;

            }
            catch (Exception ex)
            {
               
                Console.WriteLine(ex.ToString());
            }
            return 0;
        }
        private async Task PostDetailsAsync(int id, ObservableCollection<Detail> details)
        {
            string url = APIConnection.URL + "Details";
            var httpClient = new HttpClient();
            try
            {
                for (int i = 0; i < details.Count; i++)
                {
                    var detail = new Detail()
                    {
                        AssetItemId = details[i].AssetItemId,
                        InventoryId = id,
                        ExpectedQuality = details[i].ExpectedQuality
                    };
                    var jsonData = JsonConvert.SerializeObject(detail);
                    Console.WriteLine(jsonData);
                    var data = new StringContent(jsonData, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(url, data);
                }


            }
            catch (Exception ex)
            {
               
                Console.WriteLine(ex.ToString());
            }
        }


    }
}
