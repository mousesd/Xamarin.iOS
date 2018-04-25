using System;
using CoreGraphics;
using UIKit;

namespace iOSApp4
{
    public partial class ViewController : UIViewController
    {
        public ViewController() { }

        public override void LoadView()
        {
            base.LoadView();

            //# White view covering entire screen
            var contentView = new UIView { BackgroundColor = UIColor.Yellow };
            this.View = contentView;

            //# Vertical UIStackView offset from status bar
            var rect = UIScreen.MainScreen.Bounds;
            rect.Y += 20;
            rect.Height -= 20;

            var vertStackView = new UIStackView(rect) { Axis = UILayoutConstraintAxis.Vertical };
            contentView.Add(vertStackView);

            var horzStackView = new UIStackView
            {
                Axis = UILayoutConstraintAxis.Horizontal,
                Alignment = UIStackViewAlignment.Center,
                Distribution = UIStackViewDistribution.EqualSpacing
            };
            vertStackView.AddArrangedSubview(horzStackView);

            var canvasView = new FingerPaintCanvasView { BackgroundColor = UIColor.White };
            vertStackView.AddArrangedSubview(canvasView);

            horzStackView.AddArrangedSubview(new UILabel(new CGRect(0, 0, 10, 10)));
            var colorModel = new PickerDataModel<UIColor>
            {
                Items =
                {
                    new NamedValue<UIColor>("Red", UIColor.Red),
                    new NamedValue<UIColor>("Green", UIColor.Green),
                    new NamedValue<UIColor>("Blue", UIColor.Blue),
                    new NamedValue<UIColor>("Cyan", UIColor.Cyan),
                    new NamedValue<UIColor>("Magenta", UIColor.Magenta),
                    new NamedValue<UIColor>("Yellow", UIColor.Yellow),
                    new NamedValue<UIColor>("Black", UIColor.Black),
                    new NamedValue<UIColor>("Gray", UIColor.Gray),
                    new NamedValue<UIColor>("White", UIColor.White)
                }
            };
            var colorPicker = new UIPickerView { Model = colorModel };

            var thicknessModel = new PickerDataModel<float>
            {
                Items =
                {
                    new NamedValue<float>("Thin", 2.0F),
                    new NamedValue<float>("Thinish", 5.0F),
                    new NamedValue<float>("Medium", 10.0F),
                    new NamedValue<float>("Thickish", 20.0F),
                    new NamedValue<float>("Thick", 50.0F),
                }
            };
            var thicknessPicker = new UIPickerView { Model = thicknessModel };

            var toolbar = new UIToolbar(new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, 44))
            {
                BarStyle = UIBarStyle.Default,
                Translucent = true
            };

            var font = UIFont.SystemFontOfSize(24);
            var colorTextField = new NoCaretField
            {
                Text = "Red",
                InputView = colorPicker,
                InputAccessoryView = toolbar,
                Font = font
            };
            horzStackView.AddArrangedSubview(colorTextField);
            colorModel.ValueChanged += delegate
            {
                colorTextField.Text = colorModel.SelectedItem.Name;
                canvasView.StrokeColor = colorModel.SelectedItem.Value.CGColor;
            };

            var thicknessTextField = new NoCaretField
            {
                Text = "Thin",
                InputView = thicknessPicker,
                InputAccessoryView = toolbar,
                Font = font
            };
            horzStackView.AddArrangedSubview(thicknessTextField);
            thicknessModel.ValueChanged += delegate
            {
                thicknessTextField.Text = thicknessModel.SelectedItem.Name;
                canvasView.StrokeWidth = thicknessModel.SelectedItem.Value;
            };

            var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
            var doneButtone = new UIBarButtonItem(UIBarButtonSystemItem.Done, (sender, e) =>
            {
                colorTextField.ResignFirstResponder();
                thicknessTextField.ResignFirstResponder();
            });
            toolbar.SetItems(new[] { spacer, doneButtone }, false);

            var button = new UIButton(UIButtonType.RoundedRect) { Font = font };
            horzStackView.AddArrangedSubview(button);

            button.Layer.BorderColor = UIColor.Black.CGColor;
            button.Layer.BorderWidth = 1.0F;
            button.Layer.CornerRadius = 10.0F;
            button.SetTitle("Clear", UIControlState.Normal);
            button.SetTitleColor(UIColor.Black, UIControlState.Normal);
            button.TouchUpInside += (sender, e) => { canvasView.Clear(); };

            horzStackView.AddArrangedSubview(new UILabel(new CGRect(0, 0, 10, 10)));
        }

        #region == Nested classes ==
        private sealed class NoCaretField : UITextField
        {
            public NoCaretField() : base(new CGRect())
            {
                this.BorderStyle = UITextBorderStyle.Line;
            }

            public override CGRect GetCaretRectForPosition(UITextPosition position)
            {
                return new CGRect();
            }
        } 
        #endregion
    }
}
