using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using AndroidApp.Helpers;
using AndroidX.AppCompat.App;
using System;
namespace AndroidApp.Activities
{
    [Activity(Label = "Configuration Activity")]
    public class ConfigurationActivity : AppCompatActivity
    {
        private EditText _ipAddressEditText;
        private Button _saveButton;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_configuration);
            OnBindView();
        }

        private void OnBindView()
        {
            _ipAddressEditText = FindViewById<EditText>(Resource.Id.edit_text_ip);
            _saveButton = FindViewById<Button>(Resource.Id.btn_save_config);
            _saveButton.Click += SaveConfiguration;
        }
        private void SaveConfiguration(object sender, EventArgs e)
        {
            
            APIConnection.IP=_ipAddressEditText.Text.ToString();
            Toast.MakeText(this, "Save successful", ToastLength.Long).Show();
        }
    }
}