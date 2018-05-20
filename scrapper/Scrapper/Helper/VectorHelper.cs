using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace scrapper.Scrapper.Helper
{
    public static class VectorHelper
    {
        public static float RadianToDegree = 180F / (float)Math.PI;
        public static float DegreeToRadian = (float)Math.PI / 180F;

        public static Vector2 Rotate(this Vector2 vec, float radian)
        {
            float cosA = (float)System.Math.Cos(radian);
            float sinA = (float)System.Math.Sin(radian);

            float tmpX = vec.X * cosA - vec.Y * sinA;
            float tmpY = vec.Y * cosA + vec.X * sinA;

            vec.X = tmpX;
            vec.Y = tmpY;

            return vec;
        }

        public static float internalAngle(this Vector2 v1, Vector2 v2)
        {
            var n1 = new Vector2(v1.X, v1.Y);
            var n2 = new Vector2(v2.X, v2.Y);
            n1.Normalize();
            n2.Normalize();
            return (float)Math.Acos(dot(n1, n2));
        }

        public static float dot(this Vector2 v1, Vector2 v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }
    }
}
