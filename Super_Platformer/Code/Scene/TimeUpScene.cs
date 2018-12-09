using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Super_Platformer.Code.Core;
using Super_Platformer.Code.Core.Rendering;
using Super_Platformer.Code.Core.Scene;

namespace Super_Platformer.Code.Scene
{
    /// <summary>
    /// TimeUpScene scene.
    /// </summary>
    public class TimeUpScene : MonoScene
    {
        /// <summary> Activate the next scene after a fixed duration. </summary>
        private TimedEvent _durationTimer;

        /// <summary> The fixed duration.</summary>
        private int _duration;

        private SoundEffect _sound;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="game"> The game this scene belongs to.</param>
        public TimeUpScene(SuperPlatformerGame game) : base(game)
        {
            _duration = 5000;
        }

        /// <summary>
        /// Initialize scene.
        /// </summary>
        public override void Init()
        {
            // Call base init.
            base.Init();

            // Set texture of the scene.
            Texture2D texture = Game.Content.Load<Texture2D>("Images/Screens/TimeUp");

            // Load the nintendo logo.
            MonoSprite logo = new MonoSprite(texture, new Rectangle(0, 0, SuperPlatformerGame.RESOLUTION_X, SuperPlatformerGame.RESOLUTION_Y), Vector2.Zero, SuperPlatformerGame.RESOLUTION_X, SuperPlatformerGame.RESOLUTION_Y);
            
            _sound = Game.Content.Load<SoundEffect>("Audio/smw_gameover");

            if (_sound != null)
            {
                _sound.Play();
            }

            // Add the logo to the drawlist.
            Children.Add(logo);

            // Set and start the timer.
            _durationTimer = new TimedEvent(OnTimerElapsed, _duration);
            _durationTimer.Enable();
        }

        /// <summary>
        /// Function to be called when timer is completed.
        /// </summary>
        private void OnTimerElapsed()
        {
            // Disable the timer.
            _durationTimer.Disable();

            // Activate MainMenu scene.
            Game.SceneActivator.ActivateScene(new MainMenuScene(Game));
        }

        /// <summary>
        /// Reset the current scene.
        /// </summary>
        public override void Reset()
        {
            _durationTimer.Reset();
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
            _durationTimer.Update(gameTime);
        }
    }
}
