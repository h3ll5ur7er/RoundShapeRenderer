using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ch.hellsurfer.sandbox.RoundShapeRenderer
{
    public static partial class Settings
    {
        public static class Control
        {
            public static readonly bool GamePadControlled = GamePad.GetState(PlayerIndex.One).IsConnected;

            public const bool INVERTED_LOOK = false;

            public const float MOVE_SPEED_KEYBOARD = 0.2f;
            public const float LOOK_SPEED_MOUSE = 0.002f;
            public const float TURN_SPEED_KEYBOARD = 0.02f;

            public const float MOVE_SPEED_PAD = 0.5f;
            public const float LOOK_SPEED_PAD = 0.03f;
            public const float TURN_SPEED_PAD = 0.02f;


            //Keyboard controls
            public const Keys kForward = Keys.W;
            public const Keys kBackward = Keys.S;
            public const Keys kLeft = Keys.A;
            public const Keys kRight = Keys.D;
            public const Keys kUp = Keys.Space;
            public const Keys kDown = Keys.LeftShift;

            public const Keys kFullScreen = Keys.F11;
            public const Keys kQuit = Keys.Escape;
            public const Keys kDebug = Keys.F3;
            public const Keys kWireframe = Keys.Q;
            public const Keys kNextModel = Keys.E;

            public const Keys kUndo = Keys.R;
            public const Keys kCursor = Keys.M;


            //GamePad Controls
            public const Buttons gForward = Buttons.LeftThumbstickUp;
            public const Buttons gBackward = Buttons.LeftThumbstickDown;
            public const Buttons gLeft = Buttons.LeftThumbstickLeft;
            public const Buttons gRight = Buttons.LeftThumbstickRight;
            public const Buttons gUp = Buttons.A;
            public const Buttons gDown = Buttons.B;

            public const Buttons gFullScreen = Buttons.LeftStick;
            public const Buttons gQuit = Buttons.Back;
            public const Buttons gDebug = Buttons.Start;
            public const Buttons gWireframe = Buttons.RightStick;
            public const Buttons gNextModel = Buttons.X;

            public const Buttons gUndo = Buttons.Y;
            public const Buttons gCursor = Buttons.LeftShoulder;
        }

    }
}