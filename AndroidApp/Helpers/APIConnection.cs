using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AndroidApp.Helpers
{
    public static class APIConnection
    {
        public static string URL
        {
            get
            {
                return "http://" + _ip + "/api/";
            }
        }
        public static string IP
        {
            set
            {
                _ip = value;
            }
        }
        private static string _ip = "10.0.2.2";

        public static async void LoadOptionsAsync(this List<AssetItem> listAssetItem)
        {
            
            string url = URL + "AssetItems";
            var httpClient = new HttpClient();
            try
            {
                var resultJson = await httpClient.GetStringAsync(url);
                var result = JsonConvert.DeserializeObject<AssetItem[]>(resultJson);
                listAssetItem.Clear();
                for (int i = 0; i < result.Length; i++)
                {
                    listAssetItem.Add(result[i]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
        public static async void LoadOptionsAsync(this List<Department> departments)
        {

            string url = URL + "Departments";
            var httpClient = new HttpClient();
            try
            {
                var resultJson = await httpClient.GetStringAsync(url);
                var result = JsonConvert.DeserializeObject<Department[]>(resultJson);
                departments.Clear();
                for (int i = 0; i < result.Length; i++)
                {
                    departments.Add(result[i]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

    }
}