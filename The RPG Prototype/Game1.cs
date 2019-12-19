﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace The_RPG_Prototype
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public static string GameVersion = "Indev 0.0.1a";

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont Square;
        bool shouldBeFullScreen;

        Actor playerActor;
        Player player;
        Tileset tileset;
        Texture2D tilesetTexture;
        public static Texture2D pixel;

        public const float gravityX = 0f;
        public const float gravityY = 360f;

        public static float deltaTime;

        Camera2D cam;
        MouseState mouseState;
        Vector2 mouseScreenPos;
        Vector2 mouseWorldPos;

        public static KeyboardState keyboardState;
        public static KeyboardState previousKeyboardState;

        public static bool debugOverlay;

        public static int initialResolutionX;
        public static int initialResolutionY;

        private int blockSize;

        int totalFrames = 0;
        float frameElapsedTime = 0.0f;
        int fps = 0;

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

            playerActor = new Actor();
            player = playerActor.player;

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
            Texture2D idleTexture = Content.Load<Texture2D>("Spritesheets/Adventurer_Idle_50x37");
            Texture2D runningTexture = Content.Load<Texture2D>("Spritesheets/Adventurer_Run_50x37");
            Texture2D crouchTexture = Content.Load<Texture2D>("Spritesheets/Adventurer_Crouch_50x37");
            Texture2D jumpTexture = Content.Load<Texture2D>("Spritesheets/Adventurer_Jump_50x37");
            Texture2D jumpChargeTexture = Content.Load<Texture2D>("Spritesheets/Adventurer_JumpCharge_50x37");
            Texture2D fallTexture = Content.Load<Texture2D>("Spritesheets/Adventurer_Fall_50x37");
            
            Square = Content.Load<SpriteFont>("Square");
            player.LoadContent(idleTexture, runningTexture, crouchTexture, jumpTexture, jumpChargeTexture, fallTexture);
            tilesetTexture = Content.Load<Texture2D>("Tilesets/Spritesheet");
            tileset = new Tileset(tilesetTexture, 1, 2, 32);
            pixel = Content.Load<Texture2D>("white pixel");
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
                if (cam.Zoom < 4f)
                {
                    cam.Zoom = 4f;
                }
            } else if (keyboardState.IsKeyDown(Keys.OemMinus))
            {
                if (cam.Zoom > 2f)
                {
                    cam.Zoom = 2f;
                }
            }

            // TODO: Add your update logic here
            mouseState = Mouse.GetState();
            mouseScreenPos = new Vector2( mouseState.Position.X, mouseState.Position.Y);
            mouseWorldPos = mouseScreenPos + (player.transform.position - new Vector2(graphics.PreferredBackBufferWidth/2f, graphics.PreferredBackBufferHeight/2f));
            Vector2 camTarget = (mouseWorldPos + player.transform.position)*.5f;
            Vector2 normalizedCamTarget = camTarget - cam.Pos;
            //normalizedCamTarget.Normalize();
            cam.Move((normalizedCamTarget), .09f);

            player.Update(gameTime, keyboardState, previousKeyboardState);
            previousKeyboardState = keyboardState;

            frameElapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (frameElapsedTime >= 1000f)
            {
                fps = totalFrames;
                totalFrames = 0;
                frameElapsedTime = 0;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            totalFrames++;
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
            tileset.Draw(spriteBatch, new Vector2(0, 17));
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.DrawString(Square, "The RPG Prototype - " + GameVersion, new Vector2(10f, 10f), Color.White);
            spriteBatch.End();

            if (debugOverlay)
            {
                DebugScreen();
            }

            base.Draw(gameTime);
        }

        void DebugScreen()
        {
            float titlePosY = 10f;
            float spacing = 20f;
            float n = 2f;
            spriteBatch.Begin();
            spriteBatch.DrawString(Square, string.Format("FPS: {0}", fps), new Vector2(10f, titlePosY + spacing * n), Color.LightGray);
            n+=2;
            spriteBatch.DrawString(Square, "Blocks" , new Vector2(10f, titlePosY + spacing*n), Color.LightGray);
            n++;
            spriteBatch.DrawString(Square, "Mouse X (" + mouseWorldPos.X / blockSize + ")", new Vector2(10f, titlePosY + spacing * n), Color.LightGray);
            n++;
            spriteBatch.DrawString(Square, "Mouse Y (" + mouseWorldPos.Y / blockSize + ")", new Vector2(10f, titlePosY + spacing * n), Color.LightGray);
            n++;
            spriteBatch.DrawString(Square, "X (" + player.transform.position.X / blockSize + ")", new Vector2(10f, titlePosY + spacing * n), Color.LightGray);
            n++;
            spriteBatch.DrawString(Square, "Y (" + player.transform.position.Y / blockSize + ")", new Vector2(10f, titlePosY + spacing * n), Color.LightGray);
            n += 2f;

            spriteBatch.DrawString(Square, "Units", new Vector2(10f, titlePosY + spacing * n), Color.LightGray);
            n++;
            spriteBatch.DrawString(Square, "Mouse X: " + mouseWorldPos.X, new Vector2(10f, titlePosY + spacing * n), Color.LightGray);
            n++;
            spriteBatch.DrawString(Square, "Mouse Y:" + mouseWorldPos.Y, new Vector2(10f, titlePosY + spacing * n), Color.LightGray);
            n++;
            spriteBatch.DrawString(Square, "X: " + player.transform.position.X, new Vector2(10f, titlePosY + spacing * n), Color.LightGray);
            n++;
            spriteBatch.DrawString(Square, "Y: " + player.transform.position.Y, new Vector2(10f, titlePosY + spacing * n), Color.LightGray);
            n += 2f;

            spriteBatch.DrawString(Square, "Is Grounded: " + player.rigidbody.isGrounded, new Vector2(10f, titlePosY + spacing * n), Color.LightGray);
            n++;
            spriteBatch.End();
        }
    }
}
