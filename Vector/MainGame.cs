using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Vector
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// 
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    public class MainGame : Game
    {
        GraphicsDeviceManager Graphics;
        SpriteBatch SpriteBatch;
        Player Player;
        Wall Floor;
        SpriteFont Pixel;
        Texture2D Cursor;
        InputManager InputManager;
        ArrayList MovementStack;
        RenderTarget2D RenderTarget;

        int originX;
        int originY;
        int screenWidth;
        int screenHeight;

        float CapacityUsage = 0.5f;

        bool paused;
        bool PauseReady;
        Vector2 PreviousEnd;
        int PreviousCapacity;

        public MainGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            Graphics.PreferMultiSampling = false;

            Player = new Player();
            Floor = new Wall();

            InputManager = new InputManager();
            MovementStack = new ArrayList();

            paused = false;
            PauseReady = true;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            originX = GraphicsDevice.Viewport.TitleSafeArea.X;
            originY = GraphicsDevice.Viewport.TitleSafeArea.Y;
            screenWidth = GraphicsDevice.Viewport.TitleSafeArea.Width;
            screenHeight = GraphicsDevice.Viewport.TitleSafeArea.Height;

            RenderTarget = new RenderTarget2D(GraphicsDevice, screenWidth / 2, screenHeight / 2);

            Player.Initialize(new Point(0,0), 2, 8);
            Floor.Initialize(new Rectangle(0, screenHeight / 2 - 5, screenWidth / 2, 5));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Cursor = Content.Load<Texture2D>("Cursor");
            Pixel = Content.Load<SpriteFont>("Pixel");

            Player.LoadTexture(Content, "man", new Rectangle(0, 0, 7, 19));
            Player.AddToLibrary("turnleft",new int[] { 0, 1, 2, 3 });
            Player.AddToLibrary("turnright",new int[] { 0, 11, 10, 9 });
            Player.Arrow.LoadTexture(GraphicsDevice, Color.Red);
            Player.PowerBar.LoadTexture(Content, "bar");

            Floor.LoadTexture(Content,"dirt");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            HandleInputs();

            if (!paused)
            {
                Player.Collide<Wall>(Floor);
                Player.Collide(RenderTarget);
                Player.Update();
            }

            base.Update(gameTime);
        }

        private void HandleInputs()
        {
            if (InputManager.Exit())
            {
                Exit();
            }

            if (InputManager.PausePressed() && PauseReady)
            {
                PauseReady = false;
                PreviousEnd = Player.Arrow.End;
                PreviousCapacity = Player.PowerBar.CurrentCapacity;

                if (paused && !Player.Arrow.Dragging)
                {
                    Player.Velocity = Player.Arrow.Direction;
                }

                paused = !paused;
            }

            if (InputManager.PauseReleased())
            {
                PauseReady = true;
            }

            if (InputManager.LeftPressed() && !MovementStack.Contains(Direction.Left)) 
            {
                MovementStack.Add(Direction.Left);
                Player.PlayAnimation("turnleft");
            } 
            else if (InputManager.LeftReleased()) 
            {
                MovementStack.Remove(Direction.Left);
            }

            if (InputManager.RightPressed() && !MovementStack.Contains(Direction.Right)) 
            {
                MovementStack.Add(Direction.Right);
                Player.PlayAnimation("turnright");
            } 
            else if (InputManager.RightReleased()) 
            {
                MovementStack.Remove(Direction.Right);
            }

            if (MovementStack.Count > 0) 
            {
                switch ((Direction) MovementStack[MovementStack.Count - 1]) 
                {
                    case Direction.Left:
                        Player.MoveLeft();
                        break;

                    case Direction.Right:
                        Player.MoveRight();
                        break;
                }
            }

            if (Floor.Bounds.Top <= Player.Bounds.Bottom)
            {
                if (!InputManager.LeftPressed() && !InputManager.RightPressed())
                {
                    Player.Stop();
                }

                if (InputManager.Jump())
                {
                    Player.Jump();
                }
            }

            if (paused)
            {
                Rectangle arrowEnd = new Rectangle((int)Player.Arrow.End.X-10, (int)Player.Arrow.End.Y-10, 20, 20);
                if (InputManager.MouseDown())
                {
                    if (arrowEnd.Contains(InputManager.MousePosition()) && !Player.Arrow.Dragging)
                    {
                        Player.Arrow.Dragging = true;
                    } else if (Player.Arrow.Dragging) {
                        Player.Arrow.End = InputManager.MousePosition().ToVector2();
                        Player.PowerBar.CurrentCapacity = 
                            (int)(PreviousCapacity - CapacityUsage * Vector2.Distance(PreviousEnd, Player.Arrow.End));
                    } 
                }

                if (InputManager.MouseUp() && Player.Arrow.Dragging)
                {
                    Player.Arrow.Dragging = false;
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(RenderTarget);
            GraphicsDevice.Clear(Color.White);

            SpriteBatch.Begin(
                SpriteSortMode.FrontToBack, 
                null, 
                SamplerState.AnisotropicWrap,
                null, 
                RasterizerState.CullNone
                );

            if (paused)
            {
                Player.Arrow.DrawRotated(SpriteBatch,(float)Player.Arrow.Angle(),Vector2.Zero);
                Player.PowerBar.Draw(SpriteBatch);

                SpriteBatch.DrawString(Pixel, "PAUSED", new Vector2(0), Color.Black);
                SpriteBatch.Draw(Cursor, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.Black);
            }

            Player.Draw(SpriteBatch);
            Floor.DrawTiled(SpriteBatch);

            SpriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null);
            SpriteBatch.Draw(RenderTarget, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
