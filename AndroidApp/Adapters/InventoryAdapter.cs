using Android.Content;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using SharedLibrary;
using System;
using System.Collections.Generic;

namespace AndroidApp.Adapters
{

    public class InventoryAdapter : RecyclerView.Adapter
    {
        private readonly List<Inventory> _listInventory;
        private readonly IPopupMenuForAdapter _popupMenu;
        private readonly Context _context;
        public InventoryAdapter(Context context,List<Inventory> listInventory, IPopupMenuForAdapter popupMenu)
        {
            _popupMenu = popupMenu;
            _listInventory = listInventory;
            _context = context;
        }

        public override int ItemCount
        {
            get
            {
                return _listInventory.Count;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            InventoryViewHolder reportViewHolder = holder as InventoryViewHolder;
            reportViewHolder.LabelTextView.Text = _listInventory[position].Description;
            reportViewHolder.DateTextView.Text = _listInventory[position].CreatedDate.ToString("hh:mm tt MM/dd/yyyy");
            reportViewHolder.StatusTextView.Text = _listInventory[position].Status ? "Checked" : "Not checked";
            reportViewHolder.OptionsImageButton.Click += (object sender, EventArgs e) =>
            {
                _popupMenu.Show(_context,reportViewHolder.OptionsImageButton,position);
            };

        }   

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View view = inflater.Inflate(Resource.Layout.inventory_item, parent, false);
            return new InventoryViewHolder(view);
        }
       
        #region Subclass
        public class InventoryViewHolder : RecyclerView.ViewHolder
        {
            public TextView LabelTextView { get; }
            public TextView DateTextView { get; }
            public TextView StatusTextView { get; }
            public ImageView OptionsImageButton { get; }
            public InventoryViewHolder(View itemView) : base(itemView)
            {
                LabelTextView = itemView.FindViewById<TextView>(Resource.Id.tv_label);
                DateTextView = itemView.FindViewById<TextView>(Resource.Id.tv_createdDate);
                StatusTextView = itemView.FindViewById<TextView>(Resource.Id.tv_status_inventory);
                OptionsImageButton = itemView.FindViewById<ImageView>(Resource.Id.img_btn_options);

            }
        }
        #endregion
    }


}