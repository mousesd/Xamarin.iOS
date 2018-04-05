using Foundation;
using System;
using UIKit;

namespace iOSApp3
{
    public partial class SecondCollectionViewController : UICollectionViewController
    {
        public SecondCollectionViewController (IntPtr handle) : base (handle) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var longPressGesture = new UILongPressGestureRecognizer(gesture =>
            {
                switch (gesture.State)
                {
                    case UIGestureRecognizerState.Began:
                        var indexPath = this.CollectionView.IndexPathForItemAtPoint(gesture.LocationInView(this.View));
                        if (indexPath != null)
                            this.CollectionView.BeginInteractiveMovementForItem(indexPath);
                        break;

                    case UIGestureRecognizerState.Changed:
                        this.CollectionView.UpdateInteractiveMovement(gesture.LocationInView(this.View));
                        break;

                    case UIGestureRecognizerState.Ended:
                        this.CollectionView.EndInteractiveMovement();
                        break;

                    default:
                        this.CollectionView.CancelInteractiveMovement();
                        break;
                }
            });
            this.CollectionView.AddGestureRecognizer(longPressGesture);
        }
    }
}
