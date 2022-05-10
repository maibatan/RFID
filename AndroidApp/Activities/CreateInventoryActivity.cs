using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidApp.Adapters;
using AndroidApp.Helpers;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using Newtonsoft.Json;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AndroidApp.Activities
{
    [Activity(Label = "Create Inventory Activity")]
    public class CreateInventoryActivity : AppCompatActivity
    {
        #region View Fields 
        private EditText _editText;
        private Button _addButton;
        private Button _saveButton;
        private RecyclerView _recyclerView;
        private ExpectedDetailAdapter _adapter;
        private RecyclerView.LayoutManager _layoutManager;
        private Options _options;
        private AssetItemOptions _assetItemOptions;
        #endregion
        private readonly List<AssetItem> _listAssetItem = new List<AssetItem>();
        private readonly List<Detail> _listDetail = new List<Detail>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);      
            SetContentView(Resource.Layout.activity_create_inventory);
            OnBindView();
            _listAssetItem.LoadOptionsAsync();
        }
        #region Methods
        private void OnBindView()
        {
            _assetItemOptions = new AssetItemOptions(_listAssetItem);
            _assetItemOptions.MenuItemClick += ChooseAssetItem;
            _options = new Options(new List<string>() { "Add", "Delete" });
            _options.MenuItemClick += ChooseOptions;     
            _adapter = new ExpectedDetailAdapter(this, _listDetail, _options, _assetItemOptions);
            _layoutManager = new LinearLayoutManager(this);
            _recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView_expected_detail);
            _recyclerView.SetLayoutManager(_layoutManager);
            _recyclerView.SetAdapter(_adapter);
            _editText = FindViewById<EditText>(Resource.Id.editText_description);
            _editText.Click += (object sender, EventArgs e) =>
            {
               Toast.MakeText(this, "Click edit text", ToastLength.Long).Show();
            };
            _saveButton = FindViewById<Button>(Resource.Id.btn_save_inventory);
            _saveButton.Click += Save;
            _addButton = FindViewById<Button>(Resource.Id.btn_add_detail);
            _addButton.Click += AddDetail;
        }

        private void ChooseAssetItem(int position, PopupMenu.MenuItemClickEventArgs e)
        {
            _listDetail[position].AssetItemId = e.Item.ItemId;
            _listDetail[position].AssetItemNavigation = _listAssetItem.Find(x => x.Id == e.Item.ItemId);
            _recyclerView.SetAdapter(_adapter);
        }
        private void ChooseOptions(int position, PopupMenu.MenuItemClickEventArgs e)
        {
            if (e.Item.ItemId == 0)
            {
                _listDetail.Add(new Detail());
            }
            if (e.Item.ItemId == 1)
            {
                _listDetail.RemoveAt(position);
            }
            _recyclerView.SetAdapter(_adapter);
        }

        private void AddDetail(object sender, EventArgs e)
        {
            _listDetail.Add(new Detail());
            _recyclerView.SetAdapter(_adapter);
        }
        private void Save(object sender, EventArgs e)
        {
            if (_editText.Text.Trim() == "")
            {
                Toast.MakeText(this, "Inventory must have description", ToastLength.Long).Show();
                return;
            }
            _saveButton.Enabled = false;
            ProcessData(_listDetail);
            SaveAsync();
        }
       
        private async void SaveAsync()
        {
            Toast.MakeText(this, "Saving data", ToastLength.Long).Show();
            var result = await PostInventoryAsync(_editText.Text);
            if (result == null) return;
            await PostDetailsAsync(result.Id, _listDetail);
            Toast.MakeText(this, "Save successful", ToastLength.Long).Show();
            Intent intent = new Intent(this, typeof(InventoriesActivity));
            StartActivity(intent);
        }
        private void ProcessData(List<Detail> details)
        {
            for (int i = 0; i < details.Count; i++)
            {
                if(details[i].AssetItemId == 0 || details[i].ExpectedQuality == 0)
                {
                    details.RemoveAt(i);
                }
            }
            _recyclerView.SetAdapter(_adapter);
        }
        #endregion
        #region Connect API
        private async Task<Inventory> PostInventoryAsync(string description)
        {
            string url = APIConnection.URL + "Inventories";
            var httpClient = new HttpClient();
            try
            {
                var jsonData = JsonConvert.SerializeObject(new Inventory() {Description = description ,CreatedDate = DateTime.Now});
                var data = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, data);
                string result = await response.Content.ReadAsStringAsync();
                Console.WriteLine(result);
                var resultProduct = JsonConvert.DeserializeObject<Inventory>(result);
                return resultProduct;

            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Long).Show();
                Console.WriteLine(ex.ToString());
            }
            return null;
        }
        private async Task PostDetailsAsync(int id,List<Detail> details)
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
                Toast.MakeText(this, ex.ToString(), ToastLength.Long).Show();
                Console.WriteLine(ex.ToString());
            }
        }
      
        #endregion
    }
}