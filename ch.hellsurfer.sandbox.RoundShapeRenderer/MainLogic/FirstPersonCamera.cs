using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ch.hellsurfer.sandbox.RoundShapeRenderer
{
    public static class FirstPersonCamera
    {
        public static bool Active = true;

        private static bool pad = Settings.Control.GamePadControlled;

        public static Matrix View, Projection;
        public static Vector3 Position, Direction;

        private static float pitch, yaw;

        private static Game gameRef;

        public static int mx = 100;
        public static int my = 100;

        public static void Create(Game game)
        {
            gameRef = game;

            Position = Settings.Constants.InitCamPosition;

            Direction = Settings.Constants.InitCamDirection;

            Projection =
                Matrix
                .CreatePerspectiveFieldOfView(
                    MathHelper.PiOver4,
                    gameRef.GraphicsDevice.Viewport.AspectRatio,
                    0.1f,
                    1000f);
            Mouse.SetPosition(0,0);
        }

        public static void Update(GameTime gt)
        {
            if (!(gameRef.IsActive && Active)) return;

            if (pad)
            {
                var state = GamePad.GetState(PlayerIndex.One);

                Look(state);

                Movemenet(state);
            }
            else
            {
                Look(Mouse.GetState());

                Movemenet(Keyboard.GetState());
            }

            Matrices();

            DebugOverlay.Set("position", "{ " + string.Format("{0}, {1}, {2}", Position.X,  Position.Y,  Position.Z) + " }");
            DebugOverlay.Set("direction", "{ " + string.Format("{0}, {1}, {2}", Direction.X,  Direction.Y,  Direction.Z) + " }");
        }

        private static void Look(StatePositionWrapper state)
        {
            yaw += -state.X*(pad?Settings.Control.LOOK_SPEED_PAD:Settings.Control.LOOK_SPEED_MOUSE);
            pitch += -state.Y * (pad?(Settings.Control.INVERTED_LOOK?1:-1)*Settings.Control.LOOK_SPEED_PAD:Settings.Control.LOOK_SPEED_MOUSE);
            pitch = MathHelper.Clamp(pitch, -1.5f, 1.5f);
            
            if(!pad)
            Mouse.SetPosition(mx, my);
        }

        private static void Movemenet(ButtonPressedWrapper state)
        {
            if (state.IsDown(Settings.Control.gForward)||state.IsDown(Settings.Control.kForward))
            {
                Move(Vector3.Forward);
            }

            if (state.IsDown(Settings.Control.gLeft)||state.IsDown(Settings.Control.kLeft))
            {
                Move(Vector3.Left);
            }

            if (state.IsDown(Settings.Control.gBackward)||state.IsDown(Settings.Control.kBackward))
            {
                Move(Vector3.Backward);
            }

            if (state.IsDown(Settings.Control.gRight)||state.IsDown(Settings.Control.kRight))
            {
                Move(Vector3.Right);
            }

            if (state.IsDown(Settings.Control.gDown)||state.IsDown(Settings.Control.kDown))
            {
                Move(Vector3.Down);
            }

            if (state.IsDown(Settings.Control.gUp)||state.IsDown(Settings.Control.kUp))
            {
                Move(Vector3.Up);
            }
        }

        private static void Matrices()
        {
            var look = Matrix.CreateRotationX(pitch)*Matrix.CreateRotationY(yaw);
            
            Direction = Vector3.Transform(Vector3.Forward, look);

            View = 
                Matrix
                .CreateLookAt(
                Position,
                Position + Direction,
                Vector3.Up);
        }

        static void Move(Vector3 v)
        {
            v *= (pad?Settings.Control.MOVE_SPEED_PAD:Settings.Control.MOVE_SPEED_KEYBOARD);
            var rot = Matrix.CreateRotationY(yaw);
            var fw = Vector3.Transform(v, rot);
            Position += fw;
        }

    }

    internal class StatePositionWrapper
    {
        private readonly Vector2 value;
        private readonly bool pad;
        public Vector2 Position { get { return value; } }
        public float X { get { return value.X; } }
        public float Y { get { return value.Y; } }

        public bool Pad { get { return pad; } }
        
        private StatePositionWrapper(float x, float y, bool pad)
        {
            value = new Vector2(x-FirstPersonCamera.mx, y-FirstPersonCamera.my);
            this.pad = pad;
        }

        public static implicit operator StatePositionWrapper(MouseState state)
        {
            return new StatePositionWrapper(state.X, state.Y, false);
        }

        public static implicit operator StatePositionWrapper(GamePadState state)
        {
            return new StatePositionWrapper(state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y, true);
        }
    }

    internal class ButtonPressedWrapper
    {
        private readonly bool pad;

        private readonly KeyboardState kState;
        private readonly GamePadState gState;

        public bool Pad { get { return pad; } }

        private ButtonPressedWrapper(KeyboardState state)
        {
            kState = state;
            pad = false;
        }

        private ButtonPressedWrapper(GamePadState state)
        {
            gState = state;
            pad = true;
        }

        public bool IsDown(Buttons button)
        {
            return gState.IsButtonDown(button);
        }
        public bool IsUp(Buttons button)
        {
            return gState.IsButtonUp(button);
        }
        public bool IsDown(Keys key)
        {
            return kState.IsKeyDown(key);
        }
        public bool IsUp(Keys key)
        {
            return kState.IsKeyUp(key);
        }

        public static implicit operator ButtonPressedWrapper(KeyboardState state)
        {
            return new ButtonPressedWrapper(state);
        }
        public static implicit operator ButtonPressedWrapper(GamePadState state)
        {
            return new ButtonPressedWrapper(state);
        }
    }
}