using SoundsOfSpacetime.Mobile.Interfaces;

namespace SoundsOfSpacetime.Mobile.Types
{
    public class Point : IPoint
    {
        public double X { get; set; }

        public double Y { get; set; }

        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
