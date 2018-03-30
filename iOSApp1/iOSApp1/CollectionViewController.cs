using System;
using System.Collections.Generic;
using System.Diagnostics;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace iOSApp1
{
    public partial class CollectionViewController : UICollectionViewController
    {
        //# NOTE: 이 앱은 화면크기 때문에 iPad에서 실행해야 정상적으로 동작
        private static readonly string AnimalCellId = nameof(AnimalCell);
        private static readonly string HeaderId = nameof(Header);

        private readonly List<Monkey> animals;

        public CollectionViewController(UICollectionViewLayout layout) : base(layout)
        {
            animals = new List<Monkey>();
            for (int count = 0; count < 100; count++)
                animals.Add(new Monkey());
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.CollectionView.RegisterClassForCell(typeof(AnimalCell), AnimalCellId);
            this.CollectionView.RegisterClassForSupplementaryView(typeof(Header)
                , UICollectionElementKindSection.Header
                , HeaderId);

            UIMenuController.SharedMenuController.MenuItems = new[]
            {
                new UIMenuItem("Custom", new Selector("custom"))
            };
        }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return animals?.Count ?? 0;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = (AnimalCell)collectionView.DequeueReusableCell(AnimalCellId, indexPath);
            var monkey = animals[indexPath.Row];
            cell.Image = monkey.Image;
            return cell;
        }

        public override UICollectionReusableView GetViewForSupplementaryElement(UICollectionView collectionView
            , NSString elementKind, NSIndexPath indexPath)
        {
            var view = (Header)collectionView.DequeueReusableSupplementaryView(elementKind, HeaderId, indexPath);
            view.Text = "Supplementary View";
            return view;
        }

        public override void ItemHighlighted(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = collectionView.CellForItem(indexPath);
            cell.ContentView.BackgroundColor = UIColor.Yellow;
        }

        public override void ItemUnhighlighted(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = collectionView.CellForItem(indexPath);
            cell.ContentView.BackgroundColor = UIColor.White;
        }

        public override bool ShouldShowMenu(UICollectionView collectionView, NSIndexPath indexPath)
        {
            return true;
        }

        public override bool CanPerformAction(UICollectionView collectionView, Selector action, NSIndexPath indexPath, NSObject sender)
        {
            return action == new Selector("custom");
        }

        public override void PerformAction(UICollectionView collectionView, Selector action, NSIndexPath indexPath, NSObject sender)
        {
            //# TODO: 이 Callback 메서드가 호출되지 않음!
            Debug.WriteLine("Code to perform action");
        }
    }
}
