using Foundation;
using System;
using UIKit;

namespace iOSApp3
{
    public partial class TextCollectionViewCell : UICollectionViewCell
    {
        public string Title
        {
            get { return TextLabel.Text; }
            set { TextLabel.Text = value; }
        }

        public TextCollectionViewCell (IntPtr handle) : base (handle) { }
    }
}