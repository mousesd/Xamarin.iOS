using System;
using System.Collections.Generic;

using Foundation;
using UIKit;

namespace iOSApp3
{
    public class WaterfalllCollectionSource : UICollectionViewDataSource
    {
        public WaterfallCollectionView CollectionView { get; set; }
        public List<int> Numbers { get; set; } = new List<int>();

        public WaterfalllCollectionSource(WaterfallCollectionView collectionView)
        {
            this.CollectionView = collectionView;
            for (int num = 0; num < 100; num++)
                this.Numbers.Add(num);
        }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return this.Numbers.Count;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            if (!(collectionView.DequeueReusableCell("Cell", indexPath) is TextCollecitonViewCell cell))
                return null;

            cell.Title = this.Numbers[(int)indexPath.Item].ToString();
            return cell;
        }

        public override void MoveItem(UICollectionView collectionView, NSIndexPath sourceIndexPath, NSIndexPath destinationIndexPath)
        {
            var item = this.Numbers[(int)sourceIndexPath.Item];
            this.Numbers.RemoveAt((int)sourceIndexPath.Item);
            this.Numbers.Insert((int)destinationIndexPath.Item, item);
        }
    }
}
