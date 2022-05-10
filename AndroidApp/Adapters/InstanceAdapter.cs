
using Android.Content;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using SharedLibrary;
using System;
using System.Collections.Generic;

namespace AndroidApp.Adapters
{
    public class InstanceAdapter : RecyclerView.Adapter
    {
        private readonly Context _context;
        private readonly List<Instance> _listInstance;
        private readonly IPopupMenuForAdapter _assetItemMenu;
        private readonly IPopupMenuForAdapter _departmentMenu;
        public InstanceAdapter(Context context, List<Instance> listInstance, IPopupMenuForAdapter assetItemMenu, IPopupMenuForAdapter departmentMenu)
        {
            _context = context;
            _assetItemMenu = assetItemMenu;
            _departmentMenu = departmentMenu;
            _listInstance = listInstance;
        }

        public override int ItemCount
        {
            get
            {
                return _listInstance.Count;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            ScanNewItemViewHolder viewHolder = holder as ScanNewItemViewHolder;
            viewHolder.TagTextView.Text = _listInstance[position].TagId;
            if(_listInstance[position].AssetItemNavigation != null)
            {
                viewHolder.AssetItemTextView.Text = _listInstance[position].AssetItemNavigation.DisplayName;
            }
            if (_listInstance[position].DepartmentNavigation != null)
            {
                viewHolder.DeparmentTextView.Text = _listInstance[position].DepartmentNavigation.DisplayName;
            }
            if(_listInstance[position].Id == 0)
            {
                viewHolder.NewTextView.Text = "New";
                viewHolder.AssetItemTextView.Click += (object sender, EventArgs e) =>
                {
                    _assetItemMenu.Show(_context, viewHolder.AssetItemTextView, position);
                };
                viewHolder.DeparmentTextView.Click += (object sender, EventArgs e) =>
                {
                    _departmentMenu.Show(_context, viewHolder.DeparmentTextView, position);

                };
            }
            else viewHolder.NewTextView.Text = "";

        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View view = inflater.Inflate(Resource.Layout.instance_item, parent, false);
            return new ScanNewItemViewHolder(view);
        }
        #region Subclass
        public class ScanNewItemViewHolder : RecyclerView.ViewHolder
        {
            public TextView TagTextView { get; }
            public TextView AssetItemTextView { get; }
            public TextView DeparmentTextView { get; }
            public TextView NewTextView { get; }
            public ScanNewItemViewHolder(View itemView) : base(itemView)
            {
                TagTextView = itemView.FindViewById<TextView>(Resource.Id.tv_tag_id_scan);
                AssetItemTextView = itemView.FindViewById<TextView>(Resource.Id.tv_asset_name_options);
                DeparmentTextView = itemView.FindViewById<TextView>(Resource.Id.tv_deperment_options);
                NewTextView = itemView.FindViewById<TextView>(Resource.Id.tv_is_new);
            }
        }
        #endregion
    }
}