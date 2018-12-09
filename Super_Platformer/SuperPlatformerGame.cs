using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Super_Platformer.Code.Core.Input;
using Super_Platformer.Code.Core.Scene;
using Super_Platformer.Code.Scene;

#if DEBUG_INFO
using Super_Platformer.Code.Core;
#endif

namespace Super_Platformer
{
    /// <summary>
    /// SuperPlatformerGame class.
    /// </summary>
    public class SuperPlatformerGame : Game
    {
        /// <summary> Graphics Device Manager object. </summary>
        private GraphicsDeviceManager graphics;

        /// <summary> Spritebatch object. </summary>
        private SpriteBatch spriteBatch;

        /// <summary> SceneDirector object. </summary>
        public SceneDirector SceneActivator
        {
            get;
            private set;
        }

        /// <summary> Keyboard input.</summary>
        public KeyboardInput KeyboardDevice
        {
            get;
            private set;
        }

#if DEBUG_INFO
        /// <summary> FLAG: enable to show fps on screen. </summary>
        public const bool SHOW_FPS = true;

        /// <summary> FLAG: enable to draw collisionboxes. </summary>
        public const bool SHOW_BOUNDINGBOXES = true;

        /// <summary> Fps counter class, used to measure fps. </summary>
        private FpsCounter _fps;
#endif

        /// <summary> Horizontal Resolution. </summary>
        public const int RESOLUTION_X = 256;

        /// <summary> Vertical Resolution. </summary>
        public const int RESOLUTION_Y = 224;

        /// <summary> Scale </summary>
        public const int SCALE = 3; // If a feature doesn't work, this is the dark magic that destroys it!

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SuperPlatformerGame()
        {
            // Get graphicsDeviceManager.
            graphics = new GraphicsDeviceManager(this);

            // No fullscreen.
            graphics.IsFullScreen = false;

            // Set backbufferwidth.
            graphics.PreferredBackBufferWidth = RESOLUTION_X * SCALE;
            graphics.PreferredBackBufferHeight = RESOLUTION_Y * SCALE;

            // V-sync disabled.
            graphics.SynchronizeWithVerticalRetrace = false;

            // Disable fixed update.
            IsFixedTimeStep = false;

            // Show mouse.
            IsMouseVisible = true;
            
            // Create SceneDirector.
            SceneActivator = new SceneDirector();

            // Create keyboard input class.
            KeyboardDevice = new KeyboardInput();

            // Set root directory to content.
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
            // Update base.
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Get the spritebatch.
            spriteBatch = new SpriteBatch(GraphicsDevice);

#if DEBUG_INFO
            // Load debug font.
            SpriteFont font = Content.Load<SpriteFont>("Font/DebugFont");

            // If debug mode + SHOW_FPS create fps counter.
            if (SHOW_FPS)
            {
                _fps = new FpsCounter(font);
            }
#endif

            // Start intro scene.
            SceneActivator.ActivateScene(new IntroScene(this));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            //
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Check if game should close.
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            /*
             * If an update takes too long, we want to set the elapsed time to 41ms
             *  this is to prevent strange physics based bugs
             *  The game can play on lower framerate
             *  But the updates will be done on 24 fps calculations.
             */
            // Check gametime is more than 41 milliseconds (24fps).
            if (gameTime.ElapsedGameTime.TotalMilliseconds > 41f)
            {
                // set gametime to 41 milliseconds.
                gameTime.ElapsedGameTime = new System.TimeSpan(0, 0, 0, 0, 41);
            }

            // Update keyboard device.
            KeyboardDevice.Update(gameTime);

            // Update scene activator.
            SceneActivator.Update(gameTime);

            // Call keyboard endframe.
            KeyboardDevice.EndFrame();

#if DEBUG_INFO
            // check if debug + SHOW_FPS
            if (SHOW_FPS)
            {
                // Update fps counter.
                _fps.Update(gameTime);
            }
#endif
            // Update base.
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime"> Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // Render everything in scene activator.
            SceneActivator.Render(spriteBatch, GraphicsDevice);

#if  DEBUG_INFO
            // Check if debug and showfps.
            if (SHOW_FPS)
            {
                // Render fps to screne.
                _fps.Render(spriteBatch, GraphicsDevice);
            }
#endif
            // Call base draw.
            base.Draw(gameTime);
        }
    }
}
