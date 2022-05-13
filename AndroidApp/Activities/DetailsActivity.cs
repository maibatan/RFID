using Android.App;
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
using System.Threading.Tasks;

namespace AndroidApp.Activities
{
    [Activity(Label = "Details Activity")]
    public class DetailsActivity : AppCompatActivity
    {
        #region View Fields
        private RecyclerView _recyclerView;
        private DetailAdapter _adapter;
        private RecyclerView.LayoutManager _layoutManager;
        private TextView _descriptionTextView;
        private TextView _statusTextView;
        [Obsolete]
        private ProgressDialog _progressDialog;
        #endregion
        #region Fields
        private int _idInventory;
        private readonly List<Detail> _listDetail = new List<Detail>();
        #endregion
        #region Android Methods
        [Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_details);
            _progressDialog = new ProgressDialog(this);
            _progressDialog.SetMessage("Loading");
            OnBindView();
            _idInventory = Intent.GetIntExtra("Id", 0);
            LoadDataAsync(_idInventory);

        }


        #endregion
        #region Methods
        private void OnBindView()
        {
            _recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView_inventory_details);
            _layoutManager = new LinearLayoutManager(this);
            _recyclerView.SetLayoutManager(_layoutManager);
            _adapter = new DetailAdapter(_listDetail);
            _recyclerView.SetAdapter(_adapter);
            _descriptionTextView = FindViewById<TextView>(Resource.Id.tv_inventory_details_name);
            _statusTextView = FindViewById<TextView>(Resource.Id.tv_inventory_details_status);
        }
        #region Connect API
        [Obsolete]
        private async void LoadDataAsync(int id)
        {
            Toast.MakeText(this, "Loading data", ToastLength.Long).Show();
            _progressDialog.Show();
            string url = APIConnection.URL + "Inventories/" + id;
            var httpClient = new HttpClient();
            try
            {
                var resultJson = await httpClient.GetStringAsync(url);
                Console.WriteLine(resultJson);
                var resultProduct = JsonConvert.DeserializeObject<Inventory>(resultJson);
                _descriptionTextView.Text = resultProduct.Description;
                _statusTextView.Text = resultProduct.Status ? "Checked" : "Not checked";
                foreach(var detail in resultProduct.Details)
                {
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

        #endregion
        #endregion
    }
}