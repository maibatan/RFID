
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

namespace AndroidApp.Activities
{
    [Activity(Label = "Inventories Activity")]
    public class InventoriesActivity : AppCompatActivity
    {
        private RecyclerView _recyclerView;
        private InventoryAdapter _adapter;
        private RecyclerView.LayoutManager _layoutManager;
        private Options _options;
        private readonly List<Inventory> _listInventory = new List<Inventory>();
        [Obsolete]
        private ProgressDialog _progressDialog;
        [Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_inventories);
            _progressDialog = new ProgressDialog(this);
            _progressDialog.SetMessage("Loading");
            OnBindView();
            LoadInventoriesAsync();

        }
        #region Methods
        private void OnBindView()
        {
            _options = new Options(new List<string>() { "Scan", "Check" });
            _options.MenuItemClick += ChooseOptions;
            _adapter = new InventoryAdapter(this, _listInventory,_options);
            _layoutManager = new LinearLayoutManager(this);
            _recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView_inventories);
            _recyclerView.HasFixedSize = true;        
            _recyclerView.SetLayoutManager(_layoutManager);     
            _recyclerView.SetAdapter(_adapter);
        }
        private void ChooseOptions(int position, PopupMenu.MenuItemClickEventArgs e)
        {
            if (e.Item.ItemId == 0)
            {
                Intent intent = new Intent(this, typeof(ScanInventoryActivity));
                intent.PutExtra("Id", _listInventory[position].Id);
                StartActivity(intent);
            }
            if (e.Item.ItemId == 1)
            {
                Intent intent = new Intent(this, typeof(DetailsActivity));
                intent.PutExtra("Id", _listInventory[position].Id);
                StartActivity(intent);
            }
        }
        #region Connect API 
        [Obsolete]
        private async void LoadInventoriesAsync()
        {
            Toast.MakeText(this, "Loading data", ToastLength.Long).Show();
            _progressDialog.Show();
            string url = APIConnection.URL + "Inventories";
            var httpClient = new HttpClient();
            try
            {
                var resultJson = await httpClient.GetStringAsync(url);
                var resultProduct = JsonConvert.DeserializeObject<Inventory[]>(resultJson);
                foreach (var inventory in resultProduct)
                {
                    _listInventory.Add(inventory);
                }
                _progressDialog.Hide();
                Toast.MakeText(this, "Load successful", ToastLength.Long).Show();
                
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Long).Show();
                Console.WriteLine(ex.ToString());
            }
            _recyclerView.SetAdapter(_adapter);
        }

        #endregion
        #endregion


    }
}