using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Super_Platformer.Code.Core;
using Super_Platformer.Code.Core.Rendering;
using Super_Platformer.Code.Core.Scene;

namespace Super_Platformer.Code.Scene
{

    /// <summary>
    /// Introscene scene.
    /// </summary>
    public class IntroScene : MonoScene
    {
        /// <summary> Activate the next scene after a fixed duration. </summary>
        private TimedEvent _introPhaseTimer;

        /// <summary> Duration of the intro phase.</summary>
        private int _introPhaseDuration;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game">The game this scene belongs to</param>
        public IntroScene(SuperPlatformerGame game) : base(game)
        {
            _introPhaseDuration = 2000;
        }

        /// <summary>
        /// Initialize scene.
        /// </summary>
        public override void Init()
        {
            // Call base init.
            base.Init();

            // Set texture of the scene.
            Texture2D texture = Game.Content.Load<Texture2D>("Images/Screens/Nintendo");

            // Load the nintendo logo.
            MonoSprite logo = new MonoSprite(texture, new Rectangle(0, 0, SuperPlatformerGame.RESOLUTION_X, SuperPlatformerGame.RESOLUTION_Y), Vector2.Zero, SuperPlatformerGame.RESOLUTION_X, SuperPlatformerGame.RESOLUTION_Y);

            // Add the logo to the drawlist.
            Children.Add(logo);

            // Set and start the timer.
            _introPhaseTimer = new TimedEvent(OnIntroPhaseTimerElapsed, _introPhaseDuration);
            _introPhaseTimer.Enable();
        }

        /// <summary>
        /// Function to be called when timer is completed.
        /// </summary>
        private void OnIntroPhaseTimerElapsed()
        {
            // Disable the timer.
            _introPhaseTimer.Disable();

            // Activate MainMenu scene.
            Game.SceneActivator.ActivateScene(new MainMenuScene(Game));
        }

        /// <summary>
        /// Reset the current scene.
        /// </summary>
        public override void Reset()
        {
            _introPhaseTimer.Reset();
        }

        /// <summary>
        /// Update function (IMonoUpdatable).
        /// </summary>
        /// <param name="gameTime"> Game time.</param>
        public override void Update(GameTime gameTime)
        {
            // Update the base.
            base.Update(gameTime);

            // Update the timer.
            _introPhaseTimer.Update(gameTime);
        }
    }
}
