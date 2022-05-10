using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using SharedLibrary;
using System.Collections.Generic;

namespace AndroidApp.Adapters
{
    
    public class DetailAdapter : RecyclerView.Adapter
    {

        private readonly List<Detail> _listDetail;
        public DetailAdapter(List<Detail> listDetail)
        {
            _listDetail = listDetail;
                  
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
            DetailViewHolder viewHolder = holder as DetailViewHolder;
            viewHolder.AssetItemTextView.Text = _listDetail[position].AssetItemNavigation?.DisplayName;
            viewHolder.ExpectedQualityTextView.Text = _listDetail[position].ExpectedQuality.ToString();
            viewHolder.PhysicalQualityTextView.Text = _listDetail[position].PhysicalQuality.ToString();
            viewHolder.GapQualityTextView.Text = (_listDetail[position].ExpectedQuality- _listDetail[position].PhysicalQuality).ToString();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View view = inflater.Inflate(Resource.Layout.detail_item, parent, false);
            return new DetailViewHolder(view);
        }
        #region Subclass
        public class DetailViewHolder : RecyclerView.ViewHolder
        {
            public TextView AssetItemTextView { get; }
            public TextView ExpectedQualityTextView { get; }
            public TextView PhysicalQualityTextView { get; }
            public TextView GapQualityTextView { get; }
            public DetailViewHolder(View itemView) : base(itemView)
            {
                AssetItemTextView = itemView.FindViewById<TextView>(Resource.Id.column_asset_name);
                ExpectedQualityTextView = itemView.FindViewById<TextView>(Resource.Id.column_expected_quality);
                PhysicalQualityTextView = itemView.FindViewById<TextView>(Resource.Id.column_physical_quality);
                GapQualityTextView = itemView.FindViewById<TextView>(Resource.Id.column_gap_quality);
            }
        }
        #endregion

    }


}