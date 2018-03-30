using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using UIKit;

namespace iOSApp1
{
    public class CircleLayout : UICollectionViewLayout
    {
        private static readonly int CellCount = 20;
        private static readonly NSString DecorationViewId = new NSString(nameof(DecorationView));

        private float radius;
        private CGPoint center;

        public CircleLayout()
        {
            this.RegisterClassForDecorationView(typeof(DecorationView), DecorationViewId);
        }

        public override void PrepareLayout()
        {
            base.PrepareLayout();

            var size = this.CollectionView.Frame.Size;
            center = new CGPoint(size.Width / 2.0f, size.Height / 2.0f);
            radius = (float)Math.Min(size.Width, size.Height) / 2.5f;
        }

        public override CGSize CollectionViewContentSize
        {
            get { return this.CollectionView.Frame.Size; }
        }

        public override bool ShouldInvalidateLayoutForBoundsChange(CGRect newBounds)
        {
            return true;
        }

        public override UICollectionViewLayoutAttributes LayoutAttributesForItem(NSIndexPath indexPath)
        {
            var attribute = UICollectionViewLayoutAttributes.CreateForCell(indexPath);
            attribute.Size = new CGSize(70.0f, 70.0f);
            attribute.Center = new CGPoint(center.X + radius * (float)Math.Cos(2 * indexPath.Row * Math.PI / CellCount)
                , center.Y + radius * (float)Math.Sin(2 * indexPath.Row * Math.PI / CellCount));
            return attribute;
        }

        public override UICollectionViewLayoutAttributes[] LayoutAttributesForElementsInRect(CGRect rect)
        {
            var attributes = new UICollectionViewLayoutAttributes[CellCount + 1];
            for (int index = 0; index < CellCount; index++)
            {
                var indexPath = NSIndexPath.FromItemSection(index, 0);
                attributes[index] = this.LayoutAttributesForItem(indexPath);
            }

            var decorationAttr = UICollectionViewLayoutAttributes
                .CreateForDecorationView(DecorationViewId, NSIndexPath.FromItemSection(0, 0));
            decorationAttr.Size = this.CollectionView.Frame.Size;
            decorationAttr.Center = this.CollectionView.Center;
            decorationAttr.ZIndex = -1;
            attributes[CellCount] = decorationAttr;

            return attributes;
        }
    }

    public sealed class DecorationView : UICollectionReusableView
    {
        [Export("initWithFrame:")]
        public DecorationView(CGRect frame) : base(frame)
        {
            this.BackgroundColor = UIColor.Red;
        }
    }
}
