using Android.Content;
using Android.Views;
using Android.Widget;
using SharedLibrary;
using System;
using System.Collections.Generic;

namespace AndroidApp.Helpers
{
    public class AssetItemOptions : IPopupMenuForAdapter
    {
        public Action<int, PopupMenu.MenuItemClickEventArgs> MenuItemClick
        {
            get
            {
                return _menuItemClick;
            }
            set
            {
                _menuItemClick = value;
            }
        }
        private Action<int, PopupMenu.MenuItemClickEventArgs> _menuItemClick;
        private readonly List<AssetItem> _listAssetItem = new List<AssetItem>();
        public AssetItemOptions(List<AssetItem> assetItems)
        {
            _listAssetItem = assetItems;
        }
        public void Show(Context context, View view,int position)
        {
            PopupMenu popupMenu = new PopupMenu(context, view);
            popupMenu.MenuItemClick += (object sender, PopupMenu.MenuItemClickEventArgs e) =>
            {
                _menuItemClick?.Invoke(position, e);
            };
            popupMenu.Menu.Clear();
            for (int i = 0; i < _listAssetItem.Count; i++)
            {
                popupMenu.Menu.Add(0, _listAssetItem[i].Id, _listAssetItem[i].Id, _listAssetItem[i].DisplayName);
            }
            popupMenu.Show();
        }


    }
}