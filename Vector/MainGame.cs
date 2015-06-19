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

        enum Direction
        {
            Left,
            Right
        }

        int originX;
        int originY;
        int screenWidth;
        int screenHeight;

        bool paused;
        bool PauseReady;

        public MainGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            Player = new Player(ref Graphics);
            Floor = new Wall(ref Graphics);
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

            Player.Initialize(new Point(originX - 5, originY - 448), 5, 10);
            Floor.Initialize(new Rectangle(originX, originY+screenHeight-5,screenWidth,5));

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
            Player.LoadTexture(Content,"man");
            Player.Arrow.LoadTexture(Color.Red);
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
                Player.Collide(GraphicsDevice.Viewport);
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
            } 
            else if (InputManager.LeftReleased()) 
            {
                MovementStack.Remove(Direction.Left);
            }

            if (InputManager.RightPressed() && !MovementStack.Contains(Direction.Right)) 
            {
                MovementStack.Add(Direction.Right);
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatch.Begin(
                SpriteSortMode.FrontToBack, 
                null, 
                SamplerState.LinearWrap,
                null, 
                RasterizerState.CullNone
                );

            if (paused)
            {
                Player.Arrow.DrawRotated(SpriteBatch,(float)Player.Arrow.Angle(),Vector2.Zero);

                SpriteBatch.DrawString(Pixel, "PAUSED", new Vector2(0), Color.Black);
                SpriteBatch.Draw(Cursor, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.Black);
            }

            Player.Draw(SpriteBatch);
            Floor.DrawTiled(SpriteBatch);

            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
