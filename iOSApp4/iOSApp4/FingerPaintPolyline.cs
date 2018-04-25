using CoreGraphics;

namespace iOSApp4
{
    public class FingerPaintPolyline
    {
        public CGColor Color { get; set; }
        public float StrokeWidth { get; set; }
        public CGPath Path { get; }

        public FingerPaintPolyline()
        {
            this.Path = new CGPath();
        }
    }
}