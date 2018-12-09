using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Super_Platformer.Code.Core;
using Super_Platformer.Code.Core.Rendering;
using Super_Platformer.Code.Core.Scene;

namespace Super_Platformer.Code.Scene
{

    /// <summary>
    /// EndLevelScene scene.
    /// </summary>
    public class EndLevelScene : MonoScene
    {
        /// <summary> Activate the next scene after a fixed duration. </summary>
        private TimedEvent _endDurationTimer;

        /// <summary> Duration of the intro phase.</summary>
        private int _endDuration;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game"> The game this scene belongs to.</param>
        public EndLevelScene(SuperPlatformerGame game) : base(game)
        {
            _endDuration = 3500;
        }

        /// <summary>
        /// Initialize scene.
        /// </summary>
        public override void Init()
        {
            // Call base init.
            base.Init();

            // Set texture of the scene.
            Texture2D texture = Game.Content.Load<Texture2D>("Images/Screens/CourseClear");

            // Load the nintendo logo.
            MonoSprite logo = new MonoSprite(texture, new Rectangle(0, 0, SuperPlatformerGame.RESOLUTION_X, SuperPlatformerGame.RESOLUTION_Y), Vector2.Zero, SuperPlatformerGame.RESOLUTION_X, SuperPlatformerGame.RESOLUTION_Y);

            // Add the logo to the drawlist.
            Children.Add(logo);

            // Set and start the timer.
            _endDurationTimer = new TimedEvent(OnTimerElapsed, _endDuration);
            _endDurationTimer.Enable();
        }

        /// <summary>
        /// Function to be called when timer is completed.
        /// </summary>
        private void OnTimerElapsed()
        {
            // Disable the timer.
            _endDurationTimer.Disable();

            // Activate MainMenu scene.
            Game.SceneActivator.ActivateScene(new MainMenuScene(Game));
        }

        /// <summary>
        /// Reset the current scene.
        /// </summary>
        public override void Reset()
        {
            _endDurationTimer.Reset();
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
            _endDurationTimer.Update(gameTime);
        }
    }
}
