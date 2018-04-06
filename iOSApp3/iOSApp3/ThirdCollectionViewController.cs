using System;

using CoreGraphics;
using UIKit;

namespace iOSApp3
{
    public partial class ThirdCollectionViewController : UICollectionViewController
    {
        public ThirdCollectionViewController (IntPtr handle) : base (handle) { }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            var waterfallLayout = new WaterfallCollectionLayout();
            waterfallLayout.SizeForItem += (collectionView, layout, indexPath) =>
            {
                if (!(collectionView is WaterfallCollectionView view))
                    return CGSize.Empty;

                return new CGSize((this.View.Bounds.Width - 40) / 3, view.Source.Heights[(int)indexPath.Item]);
            };

            this.CollectionView.SetCollectionViewLayout(waterfallLayout, false);
        }
    }
}
