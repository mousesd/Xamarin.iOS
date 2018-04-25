using System;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;
using Foundation;
using UIKit;

namespace iOSApp4
{
    public class FingerPaintCanvasView : UIView
    {
        private readonly Dictionary<IntPtr, FingerPaintPolyline> fingerIdPolylineDic 
            = new Dictionary<IntPtr, FingerPaintPolyline>();
        private readonly List<FingerPaintPolyline> completedPolylines = new List<FingerPaintPolyline>();

        public CGColor StrokeColor { get; set; } = UIColor.Red.CGColor;

        public float StrokeWidth { get; set; } = 2.0F;

        public override void TouchesBegan(NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            foreach (var touch in touches.Cast<UITouch>())
            {
                var polyline = new FingerPaintPolyline { Color = this.StrokeColor, StrokeWidth = this.StrokeWidth };
                polyline.Path.MoveToPoint(touch.LocationInView(this));
                fingerIdPolylineDic.Add(touch.Handle, polyline);
            }
            this.SetNeedsDisplay();
        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);

            foreach (var touch in touches.Cast<UITouch>())
                fingerIdPolylineDic[touch.Handle].Path.AddLineToPoint(touch.LocationInView(this));
            this.SetNeedsDisplay();
        }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            foreach (var touch in touches.Cast<UITouch>())
            {
                var polyline = fingerIdPolylineDic[touch.Handle];
                fingerIdPolylineDic.Remove(touch.Handle);

                polyline.Path.AddLineToPoint(touch.LocationInView(this));
                completedPolylines.Add(polyline);
            }
            this.SetNeedsDisplay();
        }

        public override void TouchesCancelled(NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);

            foreach (var touch in touches.Cast<UITouch>())
                fingerIdPolylineDic.Remove(touch.Handle);
            this.SetNeedsDisplay();
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            using (var context = UIGraphics.GetCurrentContext())
            {
                //# Stroke settings
                context.SetLineCap(CGLineCap.Round);
                context.SetLineJoin(CGLineJoin.Round);

                //# Draw the completed polylines
                foreach (var polyline in completedPolylines)
                {
                    context.SetStrokeColor(polyline.Color);
                    context.SetLineWidth(polyline.StrokeWidth);
                    context.AddPath(polyline.Path);
                    context.DrawPath(CGPathDrawingMode.Stroke);
                }

                //# Draw the in-progress polylines
                foreach (var polyline in fingerIdPolylineDic.Values)
                {
                    context.SetStrokeColor(polyline.Color);
                    context.SetLineWidth(polyline.StrokeWidth);
                    context.AddPath(polyline.Path);
                    context.DrawPath(CGPathDrawingMode.Stroke);
                }
            }
        }

        public void Clear()
        {
            completedPolylines.Clear();
            this.SetNeedsDisplay();
        }
    }
}
