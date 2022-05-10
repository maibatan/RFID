using Android.Content;
using Android.Views;
using Android.Widget;
using System;

namespace AndroidApp
{
    public interface IPopupMenuForAdapter
    {
      
        public Action<int,PopupMenu.MenuItemClickEventArgs> MenuItemClick { get; set; }
        public void Show(Context context,View view, int postion);
    }
}