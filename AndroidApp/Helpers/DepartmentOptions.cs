using Android.Content;
using Android.Views;
using Android.Widget;
using SharedLibrary;
using System;
using System.Collections.Generic;

namespace AndroidApp.Helpers
{
    public class DepartmentOptions : IPopupMenuForAdapter
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
        private readonly List<Department> _listDepartment = new List<Department>();
        public DepartmentOptions(List<Department> departments)
        {
            _listDepartment = departments;
        }
        public void Show(Context context, View view, int position)
        {
            PopupMenu popupMenu = new PopupMenu(context, view);
            popupMenu.MenuItemClick += (object sender, PopupMenu.MenuItemClickEventArgs e) =>
            {
                _menuItemClick?.Invoke(position, e);
            };
            popupMenu.Menu.Clear();
            for (int i = 0; i < _listDepartment.Count; i++)
            {
                popupMenu.Menu.Add(0, _listDepartment[i].Id, _listDepartment[i].Id, _listDepartment[i].DisplayName);
            }
            popupMenu.Show();
        }


    }
}