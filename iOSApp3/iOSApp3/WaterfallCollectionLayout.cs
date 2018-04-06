using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using CoreGraphics;
using Foundation;
using UIKit;

namespace iOSApp3
{
    [Register("WaterfallCollectionLayout")]
    [SuppressMessage("ReSharper", "SuggestVarOrType_SimpleTypes")]
    public class WaterfallCollectionLayout : UICollectionViewLayout
    {
        #region == Delegates & Events --
        public delegate nfloat WaterfallCollectionFloatDelegate(UICollectionView collectionView,
            WaterfallCollectionLayout layout, nint section);

        public delegate CGSize WaterfallCollectionSizeDelegate(UICollectionView collectionView,
            WaterfallCollectionLayout layout, NSIndexPath indexPath);

        public event WaterfallCollectionFloatDelegate HeightForHeader;
        public event WaterfallCollectionFloatDelegate HeightForFooter;
        public event WaterfallCollectionSizeDelegate SizeForItem; 
        #endregion

        #region == Fields & Properties ==
        private static readonly nfloat UnionSize = 20f;

        private readonly Dictionary<nint, UICollectionViewLayoutAttributes> headersAttributes =
            new Dictionary<nint, UICollectionViewLayoutAttributes>();
        private readonly Dictionary<nint, UICollectionViewLayoutAttributes> footersAttributes =
            new Dictionary<nint, UICollectionViewLayoutAttributes>();
        private readonly List<CGRect> unionRects = new List<CGRect>();
        private readonly List<nfloat> columnHeights = new List<nfloat>();
        private readonly List<UICollectionViewLayoutAttributes> allItemAttributes =
            new List<UICollectionViewLayoutAttributes>();
        private readonly List<List<UICollectionViewLayoutAttributes>> sectionItemAttributes =
            new List<List<UICollectionViewLayoutAttributes>>();

        private int columnCount = 2;
        [Export(nameof(ColumnCount))]
        public int ColumnCount
        {
            get { return columnCount; }
            set
            {
                this.WillChangeValue(nameof(this.ColumnCount));
                columnCount = value;
                this.DidChangeValue(nameof(this.ColumnCount));
                this.InvalidateLayout();
            }
        }

        private nfloat minimumColumnSpacing = 10f;
        [Export(nameof(MinimumColumnSpacing))]
        public nfloat MinimumColumnSpacing
        {
            get { return minimumColumnSpacing; }
            set
            {
                this.WillChangeValue(nameof(this.MinimumColumnSpacing));
                minimumColumnSpacing = value;
                this.DidChangeValue(nameof(this.MinimumColumnSpacing));
                this.InvalidateLayout();
            }
        }

        private nfloat minimumInterItemSpacing = 10f;
        [Export(nameof(MinimumInterItemSpacing))]
        public nfloat MinimumInterItemSpacing
        {
            get { return minimumInterItemSpacing; }
            set
            {
                this.WillChangeValue(nameof(this.MinimumInterItemSpacing));
                minimumInterItemSpacing = value;
                this.DidChangeValue(nameof(this.MinimumInterItemSpacing));
                this.InvalidateLayout();
            }
        }

        private nfloat headerHeight = 0f;
        [Export(nameof(HeaderHeight))]
        public nfloat HeaderHeight
        {
            get { return headerHeight; }
            set
            {
                this.WillChangeValue(nameof(this.HeaderHeight));
                headerHeight = value;
                this.DidChangeValue(nameof(this.HeaderHeight));
                this.InvalidateLayout();
            }
        }

        private nfloat footerHeight = 0f;
        [Export(nameof(FooterHeight))]
        public nfloat FooterHeight
        {
            get { return footerHeight; }
            set
            {
                this.WillChangeValue(nameof(this.FooterHeight));
                footerHeight = value;
                this.DidChangeValue(nameof(this.FooterHeight));
                this.InvalidateLayout();
            }
        }

        private UIEdgeInsets sectionInset = new UIEdgeInsets(0f, 0f, 0f, 0f);
        [Export(nameof(SectionInset))]
        public UIEdgeInsets SectionInset
        {
            get { return sectionInset; }
            set
            {
                this.WillChangeValue(nameof(this.SectionInset));
                sectionInset = value;
                this.DidChangeValue(nameof(this.SectionInset));
                this.InvalidateLayout();
            }
        }

        private WaterfallCollectionRenderDirection itemRenderDirection = WaterfallCollectionRenderDirection.ShortestFirst;
        [Export(nameof(ItemRenderDirection))]
        public WaterfallCollectionRenderDirection ItemRenderDirection
        {
            get { return itemRenderDirection; }
            set
            {
                this.WillChangeValue(nameof(this.ItemRenderDirection));
                itemRenderDirection = value;
                this.DidChangeValue(nameof(this.ItemRenderDirection));
                this.InvalidateLayout();
            }
        } 
        #endregion

        #region == Constructors ==
        public WaterfallCollectionLayout() { }

        public WaterfallCollectionLayout(NSCoder coder) : base(coder) { }
        #endregion

        #region == Override methods ==
        public override void PrepareLayout()
        {
            base.PrepareLayout();

            //# Get the number of sections
            nint numberOfSections = this.CollectionView.NumberOfSections();
            if (numberOfSections == 0)
                return;

            //# Reset colletions
            headersAttributes.Clear();
            footersAttributes.Clear();
            unionRects.Clear();
            columnHeights.Clear();
            allItemAttributes.Clear();
            sectionItemAttributes.Clear();

            //# Initialize column heights
            for (int num = 0; num < this.ColumnCount; num++)
                columnHeights.Add(0);

            //# Process all sections
            nfloat top = 0f;
            for (nint section = 0; section < numberOfSections; ++section)
            {
                //# Calculate width
                nfloat width = this.CollectionView.Bounds.Width - this.SectionInset.Left - this.SectionInset.Right;
                nfloat itemWith = (nfloat)Math.Floor((width - (this.ColumnCount - 1) * this.MinimumColumnSpacing) / this.ColumnCount);

                //# Calculate section header
                nfloat heightHeader = this.HeightForFooter?.Invoke(this.CollectionView, this, section) ?? this.HeaderHeight;
                UICollectionViewLayoutAttributes attributes;
                if (headerHeight > 0)
                {
                    attributes = UICollectionViewLayoutAttributes.CreateForSupplementaryView(
                        UICollectionElementKindSection.Header, NSIndexPath.FromRowSection(0, section));
                    attributes.Frame = new CGRect(0, top, this.CollectionView.Bounds.Width, heightHeader);
                    headersAttributes.Add(section, attributes);
                    allItemAttributes.Add(attributes);
                    top = attributes.Frame.GetMaxY();
                }

                top += this.SectionInset.Top;
                for (int index = 0; index < this.ColumnCount; index++)
                    columnHeights[index] = top;

                //# Calculate section items
                nint itemCount = this.CollectionView.NumberOfItemsInSection(section);
                var itemAttributes = new List<UICollectionViewLayoutAttributes>();

                int columnIndex;
                for (nint num = 0; num < itemCount; num++)
                {
                    var indexPath = NSIndexPath.FromRowSection(num, section);
                    var itemSize = this.SizeForItem?.Invoke(this.CollectionView, this, indexPath) ?? new CGSize(0, 0);

                    columnIndex = this.NextColumnIndexForItem(num);
                    nfloat xOffset = this.sectionInset.Left + (itemWith + this.MinimumColumnSpacing) * columnIndex;
                    nfloat yOffset = columnHeights[columnIndex];
                    nfloat itemHeight = 0.0f;
                    if (itemSize.Height > 0.0f && itemSize.Width > 0.0f)
                        itemHeight = (nfloat)Math.Floor(itemSize.Height * itemWith / itemSize.Width);

                    attributes = UICollectionViewLayoutAttributes.CreateForCell(indexPath);
                    attributes.Frame = new CGRect(xOffset, yOffset, itemWith, itemHeight);
                    itemAttributes.Add(attributes);
                    allItemAttributes.Add(attributes);
                    columnHeights[columnIndex] = attributes.Frame.GetMaxY() + this.MinimumInterItemSpacing;
                }
                sectionItemAttributes.Add(itemAttributes);

                //# Calculate section footer
                columnIndex = this.LongestColumnIndex();
                top = columnHeights[columnIndex] - this.MinimumInterItemSpacing + this.SectionInset.Bottom;
                footerHeight = this.HeightForFooter?.Invoke(this.CollectionView, this, section) ?? this.FooterHeight;
                if (footerHeight > 0)
                {
                    attributes = UICollectionViewLayoutAttributes.CreateForSupplementaryView(
                        UICollectionElementKindSection.Footer, NSIndexPath.FromRowSection(0, section));
                    attributes.Frame = new CGRect(0, top, this.CollectionView.Bounds.Width, footerHeight);
                    footersAttributes.Add(section, attributes);
                    allItemAttributes.Add(attributes);
                    top = attributes.Frame.GetMaxY();
                }

                for (int index = 0; index < this.ColumnCount; index++)
                    columnHeights[index] = top;
            }

            int i = 0;
            int attrCount = allItemAttributes.Count;
            while (i < attrCount)
            {
                var rect1 = allItemAttributes[i].Frame;
                i = (int)Math.Min(i + UnionSize, attrCount) - 1;
                var rect2 = allItemAttributes[i].Frame;
                unionRects.Add(CGRect.Union(rect1, rect2));
                i++;
            }
        }

        public override CGSize CollectionViewContentSize
        {
            get
            {
                if (this.CollectionView.NumberOfSections() == 0)
                    return new CGSize(0f, 0f);

                var contentSize = this.CollectionView.Bounds.Size;
                contentSize.Height = columnHeights[0];
                return contentSize;
            }
        }

        public override UICollectionViewLayoutAttributes LayoutAttributesForItem(NSIndexPath indexPath)
        {
            if (indexPath.Section >= sectionItemAttributes.Count)
                return null;

            if (indexPath.Item >= sectionItemAttributes[indexPath.Section].Count)
                return null;

            var list = sectionItemAttributes[indexPath.Section];
            return list[(int)indexPath.Item];
        }

        public override UICollectionViewLayoutAttributes LayoutAttributesForSupplementaryView(NSString kind, NSIndexPath indexPath)
        {
            var attributes = new UICollectionViewLayoutAttributes();
            switch (kind)
            {
                case "header":
                    attributes = headersAttributes[indexPath.Section];
                    break;
                case "footer":
                    attributes = footersAttributes[indexPath.Section];
                    break;
            }

            return attributes;
        }

        public override UICollectionViewLayoutAttributes[] LayoutAttributesForElementsInRect(CGRect rect)
        {
            int begin = 0;
            int end = unionRects.Count;
            var attributes = new List<UICollectionViewLayoutAttributes>();

            for (int index = 0; index < end; index++)
                if (rect.IntersectsWith(unionRects[index]))
                    begin = index * (int)UnionSize;

            for (int index = end - 1; index >= 0; index--)
            {
                if (rect.IntersectsWith(unionRects[index]))
                {
                    end = Math.Min((index + 1) * (int)UnionSize, allItemAttributes.Count);
                    break;
                }
            }

            for (int index = begin; index < end; index++)
            {
                var attribute = allItemAttributes[index];
                if (rect.IntersectsWith(attribute.Frame))
                    attributes.Add(attribute);
            }

            return attributes.ToArray();
        }

        public override bool ShouldInvalidateLayoutForBoundsChange(CGRect newBounds)
        {
            var oldBounds = this.CollectionView.Bounds;
            return newBounds.Width != oldBounds.Width;
        }
        #endregion

        #region == Methods ==
        private int NextColumnIndexForItem(nint item)
        {
            int index = 0;
            switch (this.ItemRenderDirection)
            {
                case WaterfallCollectionRenderDirection.ShortestFirst:
                    index = this.ShortestColumnIndex();
                    break;
                case WaterfallCollectionRenderDirection.LeftToRight:
                    index = this.ColumnCount;
                    break;
                case WaterfallCollectionRenderDirection.RightToLeft:
                    index = this.ColumnCount - 1 - (int)item / this.ColumnCount;
                    break;
            }
            return index;
        }

        private int ShortestColumnIndex()
        {
            int num = 0;
            int index = 0;
            nfloat shortestHeight = nfloat.MaxValue;

            foreach (nfloat height in columnHeights)
            {
                if (height < shortestHeight)
                {
                    shortestHeight = height;
                    index = num;
                }
                ++num;
            }
            return index;
        }

        private int LongestColumnIndex()
        {
            int num = 0, index = 0;
            nfloat longestHeight = nfloat.MinValue;

            foreach (nfloat height in columnHeights)
            {
                if (height > longestHeight)
                {
                    longestHeight = height;
                    index = num;
                }
                ++num;
            }
            return index;
        }
        #endregion
    }

    public enum WaterfallCollectionRenderDirection
    {
        ShortestFirst,
        LeftToRight,
        RightToLeft
    }
}
