using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace The_RPG_Prototype
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont Square;
        bool shouldBeFullScreen;

        Player player;
        Tileset tileset;
        Texture2D tilesetTexture;

        public const float gravityX = 0f;
        public const float gravityY = 9.8f;

        public static float deltaTime;

        Camera2D cam;
        MouseState mouseState;
        Vector2 mouseScreenPos;
        Vector2 mouseWorldPos;

        public static KeyboardState keyboardState;
        public static KeyboardState previousKeyboardState;

        public bool debugOverlay;

        public static int initialResolutionX;
        public static int initialResolutionY;

        private int blockSize;

        // Work in progress
        //public enum Direction
        //{
        //    Horizontal,
        //    Vertical
        //}

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            shouldBeFullScreen = true;
            debugOverlay = true;
            blockSize = 32;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            initialResolutionX = graphics.PreferredBackBufferWidth;
            initialResolutionY = graphics.PreferredBackBufferHeight;

            this.IsMouseVisible = true;
            if (shouldBeFullScreen)
            {
                graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
                graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
                graphics.IsFullScreen = true;
                graphics.ApplyChanges();
            }

            player = new Player(0f, 0f, Keys.A, Keys.D, Keys.S, Keys.Space);

            cam = new Camera2D
            {
                Pos = new Vector2((float)graphics.PreferredBackBufferWidth / 2, (float)graphics.PreferredBackBufferHeight / 2),
                Zoom = 2.0f
            };

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            Texture2D idleTexture = Content.Load<Texture2D>("Nioda_Idle");
            Texture2D runningTexture = Content.Load<Texture2D>("Nioda_Running");
            Square = Content.Load<SpriteFont>("Square");
            player.LoadContent(idleTexture, runningTexture);
            tilesetTexture = Content.Load<Texture2D>("Tilesets/Spritesheet");
            tileset = new Tileset(tilesetTexture, 1, 2, 32);
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
            deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            keyboardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            if (keyboardState.IsKeyDown(Keys.F3) && previousKeyboardState.IsKeyUp(Keys.F3))
            {
                debugOverlay = !debugOverlay;
            }
            if (keyboardState.IsKeyDown(Keys.F11) && previousKeyboardState.IsKeyUp(Keys.F11))
            {
                if (graphics.IsFullScreen)
                {
                    graphics.PreferredBackBufferWidth = initialResolutionX;
                    graphics.PreferredBackBufferHeight = initialResolutionY;
                } else
                {
                    graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
                    graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
                }
                graphics.IsFullScreen = !graphics.IsFullScreen;
                graphics.ApplyChanges();
            }

            if (keyboardState.IsKeyDown(Keys.OemPlus))
            {
                if (cam.Zoom < 2f)
                {
                    cam.Zoom = 2f;
                }
            } else if (keyboardState.IsKeyDown(Keys.OemMinus))
            {
                if (cam.Zoom > 1f)
                {
                    cam.Zoom = 1f;
                }
            }

            // TODO: Add your update logic here
            player.Update(gameTime, keyboardState);

            mouseState = Mouse.GetState();
            mouseScreenPos = new Vector2( mouseState.Position.X, mouseState.Position.Y);
            mouseWorldPos = mouseScreenPos + (player.transform.position - new Vector2(graphics.PreferredBackBufferWidth/2f, graphics.PreferredBackBufferHeight/2f));
            Vector2 camTarget = (mouseWorldPos + player.transform.position)*.5f;
            Vector2 normalizedCamTarget = camTarget - cam.Pos;
            //normalizedCamTarget.Normalize();
            cam.Move((normalizedCamTarget), .09f);

            previousKeyboardState = keyboardState;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.BackToFront,
                        BlendState.AlphaBlend,
                        SamplerState.PointClamp,
                        null,
                        null,
                        null,
                        cam.get_transformation(GraphicsDevice /*Send the variable that has your graphic device here*/));
            player.Draw(spriteBatch);
            tileset.Draw(spriteBatch, new Vector2(0, 64));
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.DrawString(Square, "The RPG Prototype - Early Indev", new Vector2(10f, 10f), Color.White);
            spriteBatch.End();

            if (debugOverlay)
            {
                DebugScreen();
            }

            base.Draw(gameTime);
        }

        void DebugScreen()
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(Square, "Blocks" , new Vector2(10f, 40f), Color.LightGray);
            spriteBatch.DrawString(Square, "Mouse X (" + mouseWorldPos.X / blockSize + ")", new Vector2(10f, 70f), Color.LightGray);
            spriteBatch.DrawString(Square, "Mouse Y (" + mouseWorldPos.Y / blockSize + ")", new Vector2(10f, 100f), Color.LightGray);
            spriteBatch.DrawString(Square, "X (" + player.transform.position.X / blockSize + ")", new Vector2(10f, 130f), Color.LightGray);
            spriteBatch.DrawString(Square, "Y (" + player.transform.position.Y / blockSize + ")", new Vector2(10f, 160f), Color.LightGray);

            spriteBatch.DrawString(Square, "Units", new Vector2(10f, 210f), Color.LightGray);
            spriteBatch.DrawString(Square, "Mouse X: " + mouseWorldPos.X, new Vector2(10f, 240f), Color.LightGray);
            spriteBatch.DrawString(Square, "Mouse Y:" + mouseWorldPos.Y, new Vector2(10f, 270f), Color.LightGray);
            spriteBatch.DrawString(Square, "X: " + player.transform.position.X, new Vector2(10f, 300f), Color.LightGray);
            spriteBatch.DrawString(Square, "Y: " + player.transform.position.Y, new Vector2(10f, 330f), Color.LightGray);
            spriteBatch.End();
        }
    }
}
