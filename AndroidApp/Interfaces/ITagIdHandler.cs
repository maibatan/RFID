using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AndroidApp.Interfaces
{
    public interface ITagIdHandler
    {
        public void AddTagIdToList(string tag);
    }
}