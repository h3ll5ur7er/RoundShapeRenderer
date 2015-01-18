using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ch.hellsurfer.sandbox.RoundShapeRenderer
{
    public class Engine : Game
    {
        #region Fields

        public readonly GraphicsDeviceManager graphics;
        private readonly Stack<IUndoableCommand> commandStack = new Stack<IUndoableCommand>();
        private readonly List<SmoothedModel2D> models = new List<SmoothedModel2D>();
        private readonly Matrix world = Matrix.Identity;

        public SpriteBatch spriteBatch;
        public BasicEffect effect;

        private RenderTarget2D renderTarget;

        private KeyboardState kState;
        private GamePadState gState;

        private int currentModelIndex;

        private bool wireframe, cursorVisible;

        private IUndoableCommand quitCommand, fillModeCommand, debugCommand, fullScreenCommand, nextCommand, cursorCommand, undoCommand;

        private Texture2D cursorTexture;

        #endregion Fields

        #region Initialisation

        public Engine()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();

            quitCommand  = CommandFactory.CreateToggleCommand(Exit);
            fillModeCommand  = CommandFactory.CreateToggleCommand(ToggleFillMode);
            debugCommand  = CommandFactory.CreateToggleCommand(ToggleDebug);
            fullScreenCommand  = CommandFactory.CreateToggleCommand(ToggleFullScreen);
            cursorCommand  = CommandFactory.CreateToggleCommand(ToggleCursor);
            nextCommand  = CommandFactory.CreateUndoableCommand(NextModel, PrevModel);

            undoCommand = CommandFactory.CreateToggleCommand(Undo);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            DebugOverlay.LoadContent(Content.Load<SpriteFont>(Settings.Constants.DebugFontPath));

            graphics.PreferredBackBufferWidth = Settings.Constants.BackBufferWidth;
            graphics.PreferredBackBufferHeight = Settings.Constants.BackBufferHeight;
            graphics.ApplyChanges();

            FirstPersonCamera.Create(this);

            spriteBatch = new SpriteBatch(GraphicsDevice);

            effect = new BasicEffect(GraphicsDevice)
            {
                VertexColorEnabled = true
            };

            effect.EnableDefaultLighting();

            GraphicsDevice.RasterizerState = new RasterizerState
            {
                CullMode = Settings.Constants.CULLMODE,
                FillMode = Settings.Constants.FILLMODE
            };

            renderTarget = new RenderTarget2D(
                GraphicsDevice,
                GraphicsDevice.PresentationParameters.BackBufferWidth,
                GraphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24,
                0,
                RenderTargetUsage.PreserveContents);

            BuildModels(1, 1, 1, 6, Settings.Constants.Square);
            BuildModels(2, 2, 1, 6, Settings.Constants.Square);
            BuildModels(3, 1, 2, 6, Settings.Constants.Square);
            BuildModels(4, 2, 2, 6, Settings.Constants.Square);

            BuildModels(5, 1, 1, 6, Settings.Constants.Cross);
            BuildModels(6, 2, 1, 6, Settings.Constants.Cross);
            BuildModels(7, 1, 2, 6, Settings.Constants.Cross);
            BuildModels(8, 2, 2, 6, Settings.Constants.Cross);

            BuildModels(5, 1, 1, 6, Settings.Constants.Custom);
            BuildModels(6, 2, 1, 6, Settings.Constants.Custom);
            BuildModels(7, 1, 2, 6, Settings.Constants.Custom);
            BuildModels(8, 2, 2, 6, Settings.Constants.Custom);

            cursorTexture = Content.Load<Texture2D>("cursor");
        }

        private void BuildModels(int modelId, int refinement, int smoothing, int iterations, Vector3[] vertices)
        {
            for (int i = 0; i < iterations; i++)
            {
                models.Add(new SmoothedModel2D(this, modelId, refinement, smoothing, i, vertices));
            }
        }

        #endregion

        #region XNA-Logic

        #region Refresh

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            HandleInput(gameTime);

            GraphicsDevice.RasterizerState = new RasterizerState
            {
                CullMode = Settings.Constants.CULLMODE,
                FillMode = wireframe ? FillMode.WireFrame : FillMode.Solid
            };

            RefreshDebugInfo(gameTime);
        }

        private void HandleInput(GameTime gameTime)
        {
            if (Settings.Control.GamePadControlled)
            {
                InvokeIfDown(Settings.Control.gQuit, () => Execute(quitCommand));
                InvokeIfDown(Settings.Control.gNextModel, () => Execute(nextCommand));
                InvokeIfDown(Settings.Control.gWireframe, () => Execute(fillModeCommand));
                InvokeIfDown(Settings.Control.gDebug, () => Execute(debugCommand));
                InvokeIfDown(Settings.Control.gFullScreen, () => Execute(fullScreenCommand));
                InvokeIfDown(Settings.Control.gUndo, () => undoCommand.Execute());
                InvokeIfDown(Settings.Control.gCursor, () => Execute(cursorCommand));

                gState = GamePad.GetState(PlayerIndex.One);
            }

            else
            {
                InvokeIfDown(Settings.Control.kQuit, ()=> Execute(quitCommand));
                InvokeIfDown(Settings.Control.kNextModel, ()=> Execute(nextCommand));
                InvokeIfDown(Settings.Control.kWireframe, ()=> Execute(fillModeCommand));
                InvokeIfDown(Settings.Control.kDebug, ()=> Execute(debugCommand));
                InvokeIfDown(Settings.Control.kFullScreen, ()=> Execute(fullScreenCommand));
                InvokeIfDown(Settings.Control.kUndo, ()=> undoCommand.Execute());
                InvokeIfDown(Settings.Control.kCursor, () => Execute(cursorCommand));

                kState = Keyboard.GetState();
            }

            FirstPersonCamera.Update(gameTime);
        }

        private void InvokeIfDown(Buttons button, Action a)
        {
            if (GamePad.GetState(PlayerIndex.One).IsButtonDown(button))
                if (gState.IsButtonUp(button))
                    a();
        }

        private void InvokeIfDown(Keys key, Action a)
        {
            if (Keyboard.GetState().IsKeyDown(key))
                if (kState.IsKeyUp(key))
                    a();
        }

        private void RefreshDebugInfo(GameTime gameTime)
        {
            DebugOverlay.Set("Model", models[currentModelIndex].ToString());
            DebugOverlay.Set("Step", string.Format("{{{0}/{1}}}", (currentModelIndex % 6) + 1, 6));
            DebugOverlay.Set("FillMode", wireframe ? "WireFrame" : "Solid");

            DebugOverlay.Set("PressedKeys", string.Format("{{ {0} }}", string.Join(", ", kState.GetPressedKeys())));
            DebugOverlay.Set("PressedButtons", string.Format("{{ {0}, {1}, {2}}}", string.Join(", ", gState.ThumbSticks), string.Join(", ", gState.Triggers), string.Join(", ", gState.Buttons)));

            DebugOverlay.Set("TotalGameTime", gameTime.TotalGameTime);
            DebugOverlay.Set("ElapsedGameTime", gameTime.ElapsedGameTime);
            DebugOverlay.Set("refreshRate", 1000 / (gameTime.ElapsedGameTime.Milliseconds + 0.000000000001f));
        }

        #endregion Refresh

        #region Render

        protected override void Draw(GameTime gameTime)
        {

            DrawToRT();

            GraphicsDevice.Clear(Color.Transparent);

            DrawLayers(gameTime);

            DebugOverlay.Set("framerate", 1000 / (gameTime.ElapsedGameTime.Milliseconds + 0.000000000001f));

            base.Draw(gameTime);
        }

        private void DrawToRT()
        {
            GraphicsDevice.SetRenderTarget(renderTarget);

            DrawScene();

            GraphicsDevice.SetRenderTargets(null);
        }

        private void DrawScene()
        {
            GraphicsDevice.Clear(Color.Transparent);

            models[currentModelIndex].Render(effect, GraphicsDevice, world);
        }

        private void DrawLayers(GameTime gameTime)
        {
            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.AnisotropicClamp,
                DepthStencilState.Default,
                RasterizerState.CullNone);

            spriteBatch.Draw(
                renderTarget,
                Vector2.Zero,
                null,
                Color.White,
                0,
                Vector2.Zero,
                Vector2.One,
                SpriteEffects.None,
                1);

            DebugOverlay.Draw(gameTime, spriteBatch, 0);

            if(cursorVisible)
                spriteBatch.Draw(
                    cursorTexture,
                    new Rectangle(Settings.Constants.BackBufferWidth/2, Settings.Constants.BackBufferHeight/2, 32, 32),
                    null,
                    Color.White,
                    0f,
                    new Vector2(8, 8),
                    SpriteEffects.None,
                    0f); 

            spriteBatch.End();
        }

        #endregion Render

        #endregion XNA-Logic

        #region Command-Logic

        private void Execute(IUndoableCommand command)
        {
            commandStack.Push(command);
            command.Execute();
        }
        private void Undo()
        {
            if(commandStack.Count>0)
                commandStack.Pop().Undo();
        }

        private void ToggleFillMode()
        {
            wireframe = !wireframe;
        }
        private void ToggleCursor()
        {
            cursorVisible = !cursorVisible;
        }
        private static void ToggleDebug()
        {
            DebugOverlay.Active = !DebugOverlay.Active;
        }
        private void ToggleFullScreen()
        {
            graphics.ToggleFullScreen();
            graphics.ApplyChanges();
        }

        private void NextModel()
        {
            currentModelIndex = (currentModelIndex + 1) % models.Count;
        }
        private void PrevModel()
        {
            currentModelIndex = (currentModelIndex + models.Count - 1) % models.Count;
        }
        
        #endregion
    }
}
