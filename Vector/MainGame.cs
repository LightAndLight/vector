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
        ArrayList MovementStack;

        int originX;
        int originY;
        int screenWidth;
        int screenHeight;

        bool paused;
        bool pauseReady;

        public MainGame()
        {
            Graphics = new GraphicsDeviceManager(this);
            Player = new Player(ref Graphics);
            Floor = new Wall(ref Graphics);

            MovementStack = new ArrayList();

            paused = false;
            pauseReady = true;

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
                Player.Update();

                Player.Arrow.Direction = Player.Velocity;
                Player.Arrow.Position = new Point(Player.Position.X+Player.Bounds.Width/2,Player.Position.Y+Player.Bounds.Height/2);
            }

            base.Update(gameTime);
        }

        private void HandleInputs()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyUp(Keys.P))
            {
                pauseReady = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.P) && pauseReady)
            {
                if (paused && !Player.Arrow.Dragging)
                {
                    Player.Velocity = Player.Arrow.Direction;
                }

                paused = !paused;
                pauseReady = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A) && !MovementStack.Contains(Keys.A))
            {
                MovementStack.Add(Keys.A);
            } 
            else if (Keyboard.GetState().IsKeyUp(Keys.A) && MovementStack.Contains(Keys.A))
            {
                MovementStack.Remove(Keys.A);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D) && !MovementStack.Contains(Keys.D))
            {
                MovementStack.Add(Keys.D);
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.D) && MovementStack.Contains(Keys.D))
            {
                MovementStack.Remove(Keys.D);
            }

            if (MovementStack.Count > 0)
            {
                switch ((Keys) MovementStack[MovementStack.Count-1])
                {
                    case Keys.A:
                        Player.MoveLeft();
                        break;

                    case Keys.D:
                        Player.MoveRight();
                        break;
                }
            }

            if (Floor.Bounds.Top <= Player.Bounds.Bottom)
            {
                if (MovementStack.Count == 0)
                {
                    Player.Stop();
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    Player.Jump();
                }
            }

            if (paused)
            {
                Rectangle arrowEnd = new Rectangle((int)Player.Arrow.End.X-10, (int)Player.Arrow.End.Y-10, 20, 20);
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    if (arrowEnd.Contains(Mouse.GetState().Position) && !Player.Arrow.Dragging)
                    {
                        Player.Arrow.Dragging = true;
                    } else if (Player.Arrow.Dragging) {
                        Player.Arrow.End = Mouse.GetState().Position;
                    } 
                }

                if (Mouse.GetState().LeftButton == ButtonState.Released && Player.Arrow.Dragging)
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
                Player.Arrow.DrawRotated(SpriteBatch,(float)Player.Arrow.Angle,Vector2.Zero);

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
