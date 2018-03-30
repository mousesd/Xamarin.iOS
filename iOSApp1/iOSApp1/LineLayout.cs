using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using CoreAnimation;
using Foundation;
using UIKit;

namespace iOSApp1
{
    public sealed class LineLayout : UICollectionViewFlowLayout
    {
        public LineLayout()
        {
            this.ItemSize = new CGSize(200.0f, 200.0f);
            this.ScrollDirection = UICollectionViewScrollDirection.Horizontal;
            this.SectionInset = new UIEdgeInsets(400, 0, 400, 0);
            this.MinimumLineSpacing = 50.0f;
        }

        public override bool ShouldInvalidateLayoutForBoundsChange(CGRect newBounds)
        {
            return true;
        }

        public override CGPoint TargetContentOffset(CGPoint proposedContentOffset, CGPoint scrollingVelocity)
        {
            float offsetAdjust = float.MaxValue;
            float horizontalCenter = (float)(proposedContentOffset.X + this.CollectionView.Bounds.Width / 2);
            var targetRect = new CGRect(proposedContentOffset.X, 0.0f, this.CollectionView.Bounds.Width
                , this.CollectionView.Bounds.Height);

            var attributes = base.LayoutAttributesForElementsInRect(targetRect);
            foreach (var attribute in attributes)
            {
                float itemHorizontalCenter = (float)attribute.Center.X;
                if (Math.Abs(itemHorizontalCenter - horizontalCenter) < Math.Abs(offsetAdjust))
                    offsetAdjust = itemHorizontalCenter - horizontalCenter;
            }
            return new CGPoint(proposedContentOffset.X + offsetAdjust, proposedContentOffset.Y);
        }

        public override UICollectionViewLayoutAttributes[] LayoutAttributesForElementsInRect(CGRect rect)
        {
            var attributes = base.LayoutAttributesForElementsInRect(rect);
            var visibleRect = new CGRect(this.CollectionView.ContentOffset, this.CollectionView.Bounds.Size);
            foreach (var attribute in attributes)
            {
                if (attribute.Frame.IntersectsWith(rect))
                {
                    float distance = (float)(visibleRect.GetMidX() - attribute.Center.X);
                    float nomalizeDistance = distance / 200;
                    if (Math.Abs(distance) < 200)
                    {
                        float zoom = 1 + 0.3f * (1 - Math.Abs(nomalizeDistance));
                        attribute.Transform3D = CATransform3D.MakeScale(zoom, zoom, 1.0f);
                        attribute.ZIndex = 1;
                    }
                }
            }

            return attributes;
        }
    }
}
