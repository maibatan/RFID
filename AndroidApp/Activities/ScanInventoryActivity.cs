using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using AndroidApp.Adapters;
using AndroidApp.Helpers;
using AndroidApp.Interfaces;
using AndroidApp.Services;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using Newtonsoft.Json;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AndroidApp.Activities
{
    [Activity(Label = "Scan Inventory Activity")]
    public class ScanInventoryActivity : AppCompatActivity, ITagIdHandler
    {
        #region View Fields 
        private Button _scanButton;
        private Button _saveButton;
        private RecyclerView _recyclerView;
        private ScanDetailAdapter _adapter;
        private RecyclerView.LayoutManager _layoutManager;
        [Obsolete]
        private ProgressDialog _progressDialog;
        #endregion
        #region Fields
        private Inventory _inventory;     
        private readonly List<Detail> _listDetail = new List<Detail>();
        private readonly List<Instance> _readInstances = new List<Instance>();
        #endregion
        #region Android Methods
        [Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_scan);
            _progressDialog = new ProgressDialog(this);
            OnBindView();
            Toast.MakeText(this, "Loading data", ToastLength.Long).Show();
            var _idInventory = Intent.GetIntExtra("Id", 0);
            LoadDataAsync(_idInventory);
            
            
        }
        [Obsolete]
        protected override void OnPause()
        {
            base.OnPause();
            _scanButton.Text = "Start";
            RFIDService.Stop();
        }
        #endregion
        #region Methods
        public void AddTagIdToList(string result)
        {           
            Console.WriteLine("Tag ID : " + result);
            _inventory.Status = true;
            AddTagIdAsync(result);      
        }
        private async void AddTagIdAsync(string tagId)
        {
            var readInstance = _readInstances.Find(x => x.TagId == tagId);
            if (readInstance != null) return;
            readInstance = await GetInstanceIdAsync(tagId);
            _readInstances.Add(readInstance);
            var detailIndex = _listDetail.FindIndex(x => x.AssetItemId == readInstance.AssetItemId);
            if (detailIndex < 0) return;
            _listDetail[detailIndex].PhysicalQuality += 1;
            _adapter.NotifyItemChanged(detailIndex);
        }
        [Obsolete]
        private void Scan(object sender, EventArgs e)
        {
            _adapter.IsScan = true;
            RFIDService.StartScan(this);
            if (RFIDService.IsRun)
            {
                Toast.MakeText(this, "Start scan", ToastLength.Long).Show();
                _scanButton.Text = "Stop";
            }
            else
            {
                Toast.MakeText(this, "Stop scan", ToastLength.Long).Show();
                _scanButton.Text = "Start";
            }
        }

        [Obsolete]
        private void Save(object sender, EventArgs e)
        {
            _saveButton.Enabled = false;
            SaveAsync();
        }

        [Obsolete]
        private async void SaveAsync()
        {
            Toast.MakeText(this, "Saving data", ToastLength.Long).Show();
            _progressDialog.SetMessage("Saving");
            _progressDialog.Show();
            for (int i=0;i < _listDetail.Count;i++)
            {
                await UpdateDetailAsync(_listDetail[i]);
            }
            await UpdateInventoryAsync(_inventory);
            _progressDialog.Hide();
            Toast.MakeText(this, "Save successful", ToastLength.Long).Show();
            Intent intent = new Intent(this, typeof(DetailsActivity));
            intent.PutExtra("Id", _inventory.Id);
            StartActivity(intent);
            _saveButton.Enabled = true;
        }

        [Obsolete]
        private void OnBindView()
        {

            _recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView_physical_detail);
            _layoutManager = new LinearLayoutManager(this);
            _recyclerView.SetLayoutManager(_layoutManager);
            _adapter = new ScanDetailAdapter(_listDetail);
            _recyclerView.SetAdapter(_adapter);
            _scanButton = FindViewById<Button>(Resource.Id.btn_scan);
            _scanButton.Click += Scan;
            _saveButton = FindViewById<Button>(Resource.Id.btn_save);
            _saveButton.Click += Save;
        }

        #region API Connect
        [Obsolete]
        private async void LoadDataAsync(int id)
        {
            Toast.MakeText(this, "Loading data", ToastLength.Long).Show();
            _progressDialog.SetMessage("Loading");
            _progressDialog.Show();
            string url = APIConnection.URL + "Inventories/" + id;
            var httpClient = new HttpClient();
            try
            {
                var resultJson = await httpClient.GetStringAsync(url);
                Console.WriteLine(resultJson);
                var resultProduct = JsonConvert.DeserializeObject<Inventory>(resultJson);
                _inventory = resultProduct;
                foreach (var detail in resultProduct.Details)
                {
                    detail.PhysicalQuality = 0;
                    _listDetail.Add(detail);
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

        private async Task<Instance> GetInstanceIdAsync(string tagId)
        {
            string url = APIConnection.URL + "Instances/" + tagId;
            var httpClient = new HttpClient();
            try
            {
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var resultJson = await httpClient.GetStringAsync(url);
                    Console.WriteLine(resultJson);
                    var result = JsonConvert.DeserializeObject<Instance>(resultJson);
                    return result;
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Long).Show();
                Console.WriteLine(ex.ToString());
            }
            return new Instance() { TagId = tagId };
        }

        private async Task UpdateDetailAsync(Detail detail)
        {
            string url = APIConnection.URL+ "Details/"+detail.Id;
            var httpClient = new HttpClient();
            try
            {
                var updateData = new Detail()
                {
                    Id = detail.Id,
                    InventoryId = detail.InventoryId,
                    AssetItemId = detail.AssetItemId,
                    ExpectedQuality = detail.ExpectedQuality,
                    PhysicalQuality = detail.PhysicalQuality
                };
                var jsonData = JsonConvert.SerializeObject(updateData);
                var data = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync(url, data);
                string result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Long).Show();
                Console.WriteLine(ex.ToString());
            }
        }
        private async Task UpdateInventoryAsync(Inventory inventory)
        {
            string url = APIConnection.URL + "Inventories/"+inventory.Id;
            var httpClient = new HttpClient();
            try
            {
                var updateData = new Inventory()
                {
                    Id = inventory.Id,
                    Description = inventory.Description,
                    CreatedDate = inventory.CreatedDate,
                    Status = inventory.Status
                };
                var jsonData = JsonConvert.SerializeObject(updateData);
                var data = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync(url, data);
                string result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Long).Show();
                Console.WriteLine(ex.ToString());
            }
        }
        #endregion
        #endregion
    }
}