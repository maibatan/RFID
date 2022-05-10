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

namespace AndroidApp.Helpers
{
    public class Options : IPopupMenuForAdapter
    {
        
        public Action<int,PopupMenu.MenuItemClickEventArgs> MenuItemClick
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
        private Action<int,PopupMenu.MenuItemClickEventArgs> _menuItemClick;
        private readonly List<string> _options = new List<string>();
        public Options(List<string> options)
        {
            _options = options;
        }
        public void Show(Context context, View view,int position)
        {
            PopupMenu popupMenu = new PopupMenu(context, view);
            popupMenu.MenuItemClick += (object sender, PopupMenu.MenuItemClickEventArgs e) =>
            {
                _menuItemClick?.Invoke(position, e);
            };
            for (int i = 0; i < _options.Count; i++)
            {
                popupMenu.Menu.Add(0, i, i, _options[i]);
            }
            popupMenu.Show();
        }
    }
}