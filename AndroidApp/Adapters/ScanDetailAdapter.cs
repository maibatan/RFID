using Android.Content;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using SharedLibrary;
using System.Collections.Generic;
namespace AndroidApp.Adapters
{
    
    public class ScanDetailAdapter : RecyclerView.Adapter
    {
        private readonly List<Detail> _listDetail;
        public ScanDetailAdapter(List<Detail> listDetail)
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
            ScanDetailViewHolder detailViewHolder = holder as ScanDetailViewHolder;
            detailViewHolder.PhysicalQualityTextView.Text = _listDetail[position].PhysicalQuality.ToString();
            detailViewHolder.ExpectedQualityTextView.Text = _listDetail[position].ExpectedQuality.ToString();
            if (_listDetail[position].AssetItemNavigation == null) return;
            detailViewHolder.AssetNameTextView.Text = _listDetail[position].AssetItemNavigation.DisplayName;
                
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View view = inflater.Inflate(Resource.Layout.scan_detail_item, parent, false);
            return new ScanDetailViewHolder(view);
        }
        #region Subclass
        public class ScanDetailViewHolder : RecyclerView.ViewHolder
        {
            public TextView AssetNameTextView { get; }
            public TextView PhysicalQualityTextView { get; }
            public TextView ExpectedQualityTextView { get; }
            public ScanDetailViewHolder(View itemView) : base(itemView)
            {
                AssetNameTextView = itemView.FindViewById<TextView>(Resource.Id.tv_asset_name);
                PhysicalQualityTextView = itemView.FindViewById<TextView>(Resource.Id.tv_physical_quality);
                ExpectedQualityTextView = itemView.FindViewById<TextView>(Resource.Id.tv_expected_quality);
            }
        }
        #endregion
    }
}