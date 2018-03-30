using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using UIKit;

namespace iOSApp1
{
    public sealed class Header : UICollectionReusableView
    {
        private readonly UILabel label;

        public string Text
        {
            get { return label.Text; }
            set
            {
                label.Text = value;
                this.SetNeedsDisplay();
            }
        }

        [Export("initWithFrame:")]
        public Header(CGRect frame) : base(frame)
        {
            label = new UILabel { Frame = frame, BackgroundColor = UIColor.Yellow };
            this.AddSubview(label);
        }
    }
}
