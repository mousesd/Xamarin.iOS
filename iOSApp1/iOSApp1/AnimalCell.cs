using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace iOSApp1
{
    public sealed class AnimalCell : UICollectionViewCell
    {
        private readonly UIImageView imageView;

        public UIImage Image
        {
            set { imageView.Image = value; }
        }

        [Export("initWithFrame:")]
        public AnimalCell(CGRect frame) : base(frame)
        {
            //# TODO: BackgroundView, SelectedBackgroundView가 표시되지 않음
            this.BackgroundView = new UIView { BackgroundColor = UIColor.Orange };
            this.SelectedBackgroundView = new UIView { BackgroundColor = UIColor.Green };
            this.ContentView.Layer.BorderColor = UIColor.LightGray.CGColor;
            this.ContentView.Layer.BorderWidth = 5.0f;
            this.ContentView.BackgroundColor = UIColor.White;

            imageView = new UIImageView(UIImage.FromBundle("placeholder.png"))
            {
                Center = this.ContentView.Center,
                Transform = CGAffineTransform.MakeScale(0.7f, 0.7f)
            };
            this.ContentView.AddSubview(imageView);
        }

        public override bool CanPerform(Selector action, NSObject withSender)
        {
            return action == new Selector("custom");
        }

        [Export("custom")]
        private void Custom()
        {
            Debug.WriteLine("Custom in the cell");
        }
    }
}
