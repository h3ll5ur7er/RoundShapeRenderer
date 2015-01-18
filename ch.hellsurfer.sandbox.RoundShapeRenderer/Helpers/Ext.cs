using System.Linq;
using Microsoft.Xna.Framework;

namespace ch.hellsurfer.sandbox.RoundShapeRenderer
{
    public static class Ext
    {
        public static Color Average(this Color c1, Color c2)
        {
            int r = (c1.R + c2.R) / 2;
            int g = (c1.G + c2.G) / 2;
            int b = (c1.B + c2.B) / 2;
            int a = (c1.A + c2.A) / 2;

            return new Color(r,g,b,a);
        }

        public static Vector3 Average(this Vector3 v1, Vector3 v2)
        {
            return (v1 + v2)/2;
        }

        public static Vector3 Average(this Vector3 v, params Vector3[] other)
        {
            if (other.Length == 0)
                return v;
            var a = other.Aggregate(v, (v1,v2) => v1 + v2);
            return a/(other.Length + 1);
        }

        public static VertexPositionColorNormal Average(this VertexPositionColorNormal v1, VertexPositionColorNormal v2)
        {
            return new VertexPositionColorNormal(
                v1.Position.Average(v2.Position),
                v1.Color.Average(v2.Color),
                v1.Normal.Average(v2.Normal));
        }

        public static Vector2 Norm(this Vector2 v)
        {
            v.Normalize();
            return v;
        }
        public static Vector3 Norm(this Vector3 v)
        {
            v.Normalize();
            return v;
        }
    }
}