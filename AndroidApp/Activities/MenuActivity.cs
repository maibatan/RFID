using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using AndroidApp.Services;
namespace AndroidApp.Activities
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MenuActivity : AppCompatActivity
    {
        private ImageView _iventoryButton;
        private ImageView _registerInstanceButton;
        private ImageView _configurationButton;

        [Obsolete]
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //await RFIDService.InitializeReader();
            SetContentView(Resource.Layout.activity_menu);
            OnBindView();

        }
        private void OnBindView()
        {
            _iventoryButton = FindViewById<ImageView>(Resource.Id.img_btn_inventories);
            _iventoryButton.Click += CheckInventories;
            _registerInstanceButton = FindViewById<ImageView>(Resource.Id.img_btn_register_instance);
            _registerInstanceButton.Click += RegisterInstance;
            _configurationButton = FindViewById<ImageView>(Resource.Id.img_btn_configuration);
            _configurationButton.Click += Configure;

        }

        private void CheckInventories(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(InventoriesActivity));
            StartActivity(intent);
        }
        
        private void RegisterInstance(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(RegisterInstanceActivity));
            StartActivity(intent);
        }
        private void Configure(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(ConfigurationActivity));
            StartActivity(intent);
        }

    }

}