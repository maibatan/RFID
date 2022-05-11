using Android.App;
using Android.OS;
using Android.Views;
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
using System.Threading;
using System.Threading.Tasks;

namespace AndroidApp.Activities
{

    [Activity(Label = "Register Instance Activity")]
    public class RegisterInstanceActivity : AppCompatActivity, ITagIdHandler
    {
        #region View Fields 
        private Button _scanButton;
        private Button _saveButton;
        private RecyclerView _recyclerView;
        private InstanceAdapter _adapter;
        private RecyclerView.LayoutManager _layoutManager;
        #endregion
        #region Fields
        private readonly List<Instance> _listInstance = new List<Instance>();
        private readonly List<AssetItem> _listAssetItem = new List<AssetItem>();
        private readonly List<Department> _listDepartment = new List<Department>();
        private AssetItemOptions _assetItemOptions;
        private DepartmentOptions _departmentOptions;
        #endregion
        #region Android Methods
        [Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_scan);
            OnBindView();
            _listAssetItem.LoadOptionsAsync();
            _listDepartment.LoadOptionsAsync();
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
            Console.WriteLine("Read " + result);
            var exist = _listInstance.Find(x => x.TagId == result);
            if (exist != null) return;
            AddTagIdAsync(result);
        }
        private async void AddTagIdAsync(string tagId)
        {
            var result = await GetDataAsync(tagId);    
            _listInstance.Add(result);
            _recyclerView.SetAdapter(_adapter);
        }
        [Obsolete]
        private void Scan(object sender, EventArgs e)
        {
            
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
        private void Save(object sender, EventArgs e)
        {
            _saveButton.Enabled = false;
            ProcessData(_listInstance);
            SaveAsync();
        }
        private async void SaveAsync()
        {

            Toast.MakeText(this, "Saving data", ToastLength.Long).Show();
            for (int i=0; i < _listInstance.Count;i++)
            {
                await PostInstancesAsync(_listInstance[i]);
            }
            Toast.MakeText(this, "Save successful", ToastLength.Long).Show();
            _listInstance.Clear();
            _recyclerView.SetAdapter(_adapter);
            _saveButton.Enabled = true; ;        
        }
        private void ProcessData(List<Instance> instances)
        {

            instances.RemoveAll(x=> x.AssetItemNavigation == null || x.DepartmentNavigation == null || x.Id != 0);
            _recyclerView.SetAdapter(_adapter);
        }
        [Obsolete]
        private void OnBindView()
        {
            _assetItemOptions = new AssetItemOptions(_listAssetItem);
            _assetItemOptions.MenuItemClick += ChooseAssetItem;
            _departmentOptions = new DepartmentOptions(_listDepartment);
            _departmentOptions.MenuItemClick += ChooseDepartment;
            _adapter = new InstanceAdapter(this, _listInstance, _assetItemOptions, _departmentOptions);
            _layoutManager = new LinearLayoutManager(this);
            _recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView_physical_detail);
            _recyclerView.HasFixedSize = true;           
            _recyclerView.SetLayoutManager(_layoutManager);           
            _recyclerView.SetAdapter(_adapter);
            _scanButton = FindViewById<Button>(Resource.Id.btn_scan);
            _scanButton.Click += Scan;
            _saveButton = FindViewById<Button>(Resource.Id.btn_save);
            _saveButton.Click += Save;
        }

        private void ChooseDepartment(int position, PopupMenu.MenuItemClickEventArgs e)
        {
            _listInstance[position].DepartmentId = e.Item.ItemId;
            _listInstance[position].DepartmentNavigation = _listDepartment.Find(x => x.Id == e.Item.ItemId);
            _recyclerView.SetAdapter(_adapter);
        }

        private void ChooseAssetItem(int position, PopupMenu.MenuItemClickEventArgs e)
        {
            _listInstance[position].AssetItemId = e.Item.ItemId;
            _listInstance[position].AssetItemNavigation = _listAssetItem.Find(x => x.Id == e.Item.ItemId);
            _recyclerView.SetAdapter(_adapter);
        }

        #region Connect API
        public async Task<Instance> GetDataAsync(string tagId)
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
        private async Task PostInstancesAsync(Instance instance)
        {
            string url = APIConnection.URL + "Instances";
            var httpClient = new HttpClient();
            try
            {
                var newData = new Instance()
                {
                    Id = instance.Id,
                    AssetItemId = instance.AssetItemId,
                    DepartmentId = instance.DepartmentId,
                    TagId = instance.TagId,
                    IsAvailable = true,
                    CreatedDate = DateTime.Now
                };
                var jsonData = JsonConvert.SerializeObject(newData);
                var data = new StringContent(jsonData, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, data);
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    Console.WriteLine("Save successful");
                }

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