﻿// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace iOSApp3
{
    [Register ("TextCollectionViewCell")]
    partial class TextCollectionViewCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TextLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (TextLabel != null) {
                TextLabel.Dispose ();
                TextLabel = null;
            }
        }
    }
}