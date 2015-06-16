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
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player player;
        Wall floor;
        Arrow arrow;
        SpriteFont pixel;
        Texture2D cursor;
        ArrayList movementStack;

        int originX;
        int originY;
        int screenWidth;
        int screenHeight;

        bool paused;
        bool pauseReady;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            player = new Player();
            floor = new Wall();
            arrow = new Arrow();

            movementStack = new ArrayList();

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

            //player.Initialize(originX + screenWidth/2, originY + screenHeight/2);
            player.Initialize(originX - 5, originY - 448);
            floor.Initialize(originX, originY+screenHeight-5,screenWidth,5);
            arrow.Initialize(CreateTexture(1, 1, Color.Red), player.Velocity, player.Position, 2, 8);

            base.Initialize();
        }

        private Texture2D CreateTexture(int width, int height, Color color)
        {
            Texture2D texture = new Texture2D(graphics.GraphicsDevice, width, height);
            Color[] data = new Color[width*height];
            for (int i = 0; i < data.Length; i++) {
                data[i] = color;
            }
            texture.SetData(data);

            return texture;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            cursor = Content.Load<Texture2D>("cursor");
            pixel = Content.Load<SpriteFont>("Pixel");
            player.LoadContent(Content);
            floor.LoadContent(Content);
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
                player.Update();
                HandleCollisions();

                arrow.Direction = player.Velocity;
                arrow.Position = new Vector2(player.Position.X+player.Bounds.Width/2,player.Position.Y+player.Bounds.Height/2);
            }

            base.Update(gameTime);
        }

        private void HandleInputs()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyUp(Keys.P) && !pauseReady)
            {
                pauseReady = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.P) && pauseReady)
            {
                if (paused && !arrow.Dragging)
                {
                    player.Velocity = arrow.Direction;
                }

                paused = !paused;
                pauseReady = false;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A) && !movementStack.Contains(Keys.A))
            {
                movementStack.Add(Keys.A);
            } 
            else if (Keyboard.GetState().IsKeyUp(Keys.A) && movementStack.Contains(Keys.A))
            {
                movementStack.Remove(Keys.A);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D) && !movementStack.Contains(Keys.D))
            {
                movementStack.Add(Keys.D);
            }
            else if (Keyboard.GetState().IsKeyUp(Keys.D) && movementStack.Contains(Keys.D))
            {
                movementStack.Remove(Keys.D);
            }

            if (movementStack.Count > 0)
            {
                switch ((Keys) movementStack[movementStack.Count-1])
                {
                    case Keys.A:
                        player.MoveLeft();
                        break;

                    case Keys.D:
                        player.MoveRight();
                        break;
                }
            }

            if (floor.Bounds.Top <= player.Bounds.Bottom)
            {
                if (movementStack.Count == 0)
                {
                    player.Stop();
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    player.Jump();
                }
            }

            if (paused)
            {
                Rectangle arrowEnd = new Rectangle((int)arrow.End.X-10, (int)arrow.End.Y-10, 20, 20);
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    if (arrowEnd.Contains(Mouse.GetState().Position) && !arrow.Dragging)
                    {
                        arrow.Dragging = true;
                    } else if (arrow.Dragging) {
                        arrow.End = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                    } 
                }

                if (Mouse.GetState().LeftButton == ButtonState.Released && arrow.Dragging)
                {
                    arrow.Dragging = false;
                }
            }
        }

        private void HandleCollisions()
        {
            if (floor.Bounds.Top <= player.Bounds.Bottom)
            {
                player.Bounds = new Rectangle(
                    player.Bounds.X,
                    floor.Bounds.Top-player.Bounds.Height,
                    player.Bounds.Width, 
                    player.Bounds.Height
                    );

                if (player.Velocity.Y > 0)
                {
                    player.Velocity = new Vector2(player.Velocity.X, 0);
                }
            }

            if (player.Bounds.Left < originX)
            {
                player.Bounds = new Rectangle(
                    originX,
                    player.Bounds.Y,
                    player.Bounds.Width, 
                    player.Bounds.Height
                    );

                player.Stop();
            }
            else if (player.Bounds.Right > originX+screenWidth)
            {
                player.Bounds = new Rectangle(
                    originX+screenWidth-player.Bounds.Width,
                    player.Bounds.Y,
                    player.Bounds.Width, 
                    player.Bounds.Height
                    );

                player.Stop();
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(
                SpriteSortMode.FrontToBack, 
                null, 
                SamplerState.LinearWrap,
                null, 
                RasterizerState.CullNone
                );

            if (paused)
            {
                arrow.Draw(spriteBatch);

                spriteBatch.DrawString(pixel, "PAUSED", new Vector2(0), Color.Black);
                spriteBatch.Draw(cursor, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.Black);
            }

            player.Draw(spriteBatch);
            floor.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
