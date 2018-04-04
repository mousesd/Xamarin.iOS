using Foundation;
using UIKit;

namespace iOSApp3
{
    public class WaterfallCollectionDelegate : UICollectionViewDelegate
    {
        public WaterfallCollectionView CollectionView { get; set; }

        public WaterfallCollectionDelegate(WaterfallCollectionView collectionView)
        {
            this.CollectionView = collectionView;
        }

        public override bool ShouldHighlightItem(UICollectionView collectionView, NSIndexPath indexPath)
        {
            return true;
        }

        public override void ItemHighlighted(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = collectionView.CellForItem(indexPath);
            cell.ContentView.BackgroundColor = UIColor.FromRGB(183, 208, 57);
        }

        public override void ItemUnhighlighted(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = collectionView.CellForItem(indexPath);
            cell.ContentView.BackgroundColor = UIColor.FromRGB(164, 205, 255);
        }
    }
}