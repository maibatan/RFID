using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AndroidApp.Adapters
{
    public class ExpectedDetailAdapter : RecyclerView.Adapter
    {
        private readonly Context _context;
        private readonly List<Detail> _listDetail;
        private readonly IPopupMenuForAdapter _optionsMenu;
        private readonly IPopupMenuForAdapter _assetItemMenu;
        public ExpectedDetailAdapter(Context context,List<Detail> listDetail, IPopupMenuForAdapter optionsMenu, IPopupMenuForAdapter assetItemMenu)
        {
            _context = context;
            _listDetail = listDetail;
            _assetItemMenu = assetItemMenu;
            _optionsMenu = optionsMenu;
        }

        public override int ItemCount
        {
            get
            {
                return _listDetail.Count;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            ExpectedDetailViewHolder viewHolder = holder as ExpectedDetailViewHolder;
            viewHolder.NumberTextView.Text = position.ToString();
            if(_listDetail[position].AssetItemNavigation != null)
            {
                viewHolder.AssetNameTextView.Text = _listDetail[position].AssetItemNavigation.DisplayName;
            }
            viewHolder.AssetNameTextView.Click += (object sender, EventArgs e) =>
            {
                _assetItemMenu.Show(_context, viewHolder.AssetNameTextView,position);
            };
            viewHolder.OptionsImageView.Click += (object sender, EventArgs e) =>
            {
                _optionsMenu.Show(_context, viewHolder.OptionsImageView,position);
            };
            viewHolder.QualityEditText.Text = _listDetail[position].ExpectedQuality.ToString();
            viewHolder.QualityEditText.AfterTextChanged += (object sender, Android.Text.AfterTextChangedEventArgs e) =>
            {
                Console.WriteLine(viewHolder.QualityEditText.Text.ToString());
                if(viewHolder.QualityEditText.Text.ToString() !="") _listDetail[position].ExpectedQuality = Convert.ToInt16(viewHolder.QualityEditText.Text.ToString());
            };


        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View view = inflater.Inflate(Resource.Layout.expected_detail_item, parent, false);
            return new ExpectedDetailViewHolder(view);
        }
        #region Subclass
        public class ExpectedDetailViewHolder : RecyclerView.ViewHolder
        {
            public TextView NumberTextView { get; }
            public TextView AssetNameTextView { get; }
            public EditText QualityEditText { get; }
            public ImageView OptionsImageView { get; }
            
            public ExpectedDetailViewHolder(View itemView) : base(itemView)
            {
                NumberTextView = itemView.FindViewById<TextView>(Resource.Id.tv_number);
                AssetNameTextView = itemView.FindViewById<TextView>(Resource.Id.tv_asset_name_choose_options);
                QualityEditText = itemView.FindViewById<EditText>(Resource.Id.edit_text_quality);
                OptionsImageView = itemView.FindViewById<ImageView>(Resource.Id.img_btn_options);
            }
        }
        #endregion

    }
}