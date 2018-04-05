using System;
using Foundation;
using UIKit;

namespace iOSApp3
{
    [Register("WaterfallCollectionView")]
    public partial class WaterfallCollectionView : UICollectionView
    {
        public WaterfallCollectionView (IntPtr handle) : base (handle) { }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            this.DataSource = new WaterfalllCollectionSource(this);
            this.Delegate = new WaterfallCollectionDelegate(this);
        }
    }
}
