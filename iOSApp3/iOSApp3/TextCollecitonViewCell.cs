using System;
using UIKit;

namespace iOSApp3
{
    public partial class TextCollecitonViewCell : UICollectionViewCell
    {
        public string Title
        {
            get { return TextLabel.Text; }
            set { TextLabel.Text = value; }
        }

        public TextCollecitonViewCell (IntPtr handle) : base (handle) { }
    }
}
