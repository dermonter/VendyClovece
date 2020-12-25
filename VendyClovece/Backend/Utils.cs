using Microsoft.Xna.Framework;
using System;

namespace VendyClovece.Backend
{
    public class Utils
    {
        public static Vector2 RotateAroundOrigin(Vector2 point, Vector2 origin, float angle)
        {
            float s = (float)Math.Sin(angle);
            float c = (float)Math.Cos(angle);

            // translate point back to origin:
            float px = point.X - origin.X;
            float py = point.Y - origin.Y;

            // rotate point
            float xnew = px * c - py * s;
            float ynew = px * s + py * c;

            // translate point back:
            px = xnew + origin.X;
            py = ynew + origin.Y;
            return new Vector2(px, py);
        }
    }
}
