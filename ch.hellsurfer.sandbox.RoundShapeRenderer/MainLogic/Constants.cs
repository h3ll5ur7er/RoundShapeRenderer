using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ch.hellsurfer.sandbox.RoundShapeRenderer
{
    public static partial class Settings
    {
        public static class Constants
        {
            public const int BackBufferWidth = 840;
            public const int BackBufferHeight = 525;

            public const string ConsoleFontPath = "Fonts/ConsoleFont";
            public const string DebugFontPath = "Fonts/DebugFont";

            public const CullMode CULLMODE = CullMode.None;
            public const FillMode FILLMODE = FillMode.WireFrame;

            public const int SquareSize = 25;

            public static readonly Vector3[] Square =
            {
                new Vector3(SquareSize, 0, SquareSize),
                new Vector3(-SquareSize, 0, SquareSize),
                new Vector3(-SquareSize, 0, -SquareSize),
                new Vector3(SquareSize, 0, -SquareSize)
            };
            public static readonly Vector3[] Cross =
            {
                new Vector3(- 5, 0, - 5),
                new Vector3(- 5, 0, -15),
                new Vector3(  5, 0, -15),
                new Vector3(  5, 0, - 5),
                new Vector3( 15, 0, - 5),
                new Vector3( 15, 0,   5),
                new Vector3(  5, 0,   5),
                new Vector3(  5, 0,  15),
                new Vector3(- 5, 0,  15),
                new Vector3(- 5, 0,  5),
                new Vector3(-15, 0,  5),
                new Vector3(-15, 0, -5)
            };

            public static readonly Vector3[] Custom =
            {
                  new Vector3(15, 15, 10),
                  new Vector3(15, 0, -10),
                  new Vector3(-15, -10, -10),
                  new Vector3(-15, -15, 10)
            };

            public static readonly Vector3 InitCamPosition = new Vector3(0.0947944447f, 24.9999952f, 2.60018682f);
            public static readonly Vector3 InitCamDirection = new Vector3(0.00136367814f, -0.990658045f, -0.13636288f);

            public const int loopTimeout = 2000;
        }
    }
}