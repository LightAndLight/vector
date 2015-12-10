using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Vector
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// 
    public class MainGame : Game
    {
        GraphicsDeviceManager GraphicsManager;
        SpriteBatch SpriteBatch;
        SpriteFont Pixel;
        Texture2D Cursor;
        InputManager InputManager;

        public RenderTarget2D RenderTarget { get; private set; }

        Player Player;
        Arrow Arrow;
        PowerBar PowerBar;
        LinkedList<Sprite> Sprites;

        int originX;
        int originY;
        int screenWidth;
        int screenHeight;

        bool paused;

        const int PLAYER_MOVE_SPEED = 2;
        const int PLAYER_JUMP_SPEED = 8;
        const int POWER_BAR_CAPACITY = 100;

        public MainGame()
        {
            GraphicsManager = new GraphicsDeviceManager(this);
            GraphicsManager.PreferMultiSampling = false;

            paused = false;

            Player = new Player("man", new Point(5,5), PLAYER_MOVE_SPEED, PLAYER_JUMP_SPEED);
            Arrow = new Arrow("arrow", Player.Bounds.Location, 10, Player.Velocity);
            PowerBar = new PowerBar("barborder", new Point(380, 10), "bar", POWER_BAR_CAPACITY);

            Sprites = new LinkedList<Sprite>();
            Sprites.AddLast(Player);
            Sprites.AddLast(Arrow);
            Sprites.AddLast(PowerBar);

            InputManager = new InputManager();

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            originX = GraphicsDevice.Viewport.TitleSafeArea.X;
            originY = GraphicsDevice.Viewport.TitleSafeArea.Y;
            screenWidth = GraphicsDevice.Viewport.TitleSafeArea.Width;
            screenHeight = GraphicsDevice.Viewport.TitleSafeArea.Height;

            RenderTarget = new RenderTarget2D(GraphicsDevice, screenWidth / 2, screenHeight / 2);

            SpriteBatch = new SpriteBatch(GraphicsDevice);

            // Create a new SpriteBatch, which can be used to draw textures.
            Cursor = Content.Load<Texture2D>("Cursor");
            Pixel = Content.Load<SpriteFont>("Pixel");

            foreach (Sprite sprite in Sprites)
            {
                sprite.LoadTexture(Content);
            }
        }

        private void HandleInputs()
        {
            if (InputManager.Exit)
            {
                Exit();
            }

            if (!paused)
            {
                if (InputManager.MoveLeft)
                {
                    Player.MoveLeft();
                }
                else if (InputManager.MoveRight)
                {
                    Player.MoveRight();
                }
                else
                {
                    Player.Stop();
                }

                if (InputManager.Jump)
                {
                    Player.Jump();
                }
            }
            else
            {
                Rectangle endRegion = new Rectangle(Arrow.EndPoint.X - 10, Arrow.EndPoint.Y - 10, 20, 20);
                if (!Arrow.Dragging && InputManager.MouseDown && endRegion.Contains(InputManager.MousePosition))
                {
                    Arrow.StartDragging(Player.Velocity);
                }
                else if (Arrow.Dragging && InputManager.MouseUp)
                {
                    Arrow.StopDragging();
                    Player.Velocity = Arrow.Velocity;
                }
            }

            if (InputManager.TogglePause())
            {
                PowerBar.Visible = Arrow.Visible = paused = !paused;
                PowerBar.TogglePause();
            }

            if (Arrow.Dragging)
            {
                Arrow.EndPoint = InputManager.MousePosition;
                PowerBar.CurrentLevel -= Arrow.Change;
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 
        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();
            HandleInputs();

            if (!paused)
            {
                Player.CollideWithScreen(RenderTarget);
                Player.Update();

                Arrow.Velocity = Player.Velocity;
                Arrow.Bounds = new Rectangle(Player.Bounds.Center, Arrow.Bounds.Size);
            }

            base.Update(gameTime);
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

            foreach (Sprite sprite in Sprites) {
                sprite.Draw(SpriteBatch);
            }

            if (paused)
            {
                SpriteBatch.DrawString(Pixel, "PAUSED", new Vector2(0), Color.Black);
                SpriteBatch.Draw(Cursor, new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.Black);
            }

            SpriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null);
            SpriteBatch.Draw(RenderTarget, new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
