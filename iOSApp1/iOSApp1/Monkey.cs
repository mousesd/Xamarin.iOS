using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

namespace iOSApp1
{
    public class Monkey
    {
        public string Name
        {
            get { return "Monkey"; }
        }

        public UIImage Image
        {
            get { return UIImage.FromBundle("monkey.png"); }
        }
    }
}
