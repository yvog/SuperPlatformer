using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Super_Platformer.Code.Core.Scene;
using Super_Platformer.Code.World;

namespace Super_Platformer.Code.Scene
{
    /// <summary>
    /// Scene that loads a level.
    /// </summary>
    public class LevelScene : MonoScene
    {
        /// <summary> Tag of the level. </summary>
        private string _levelTag;

        /// <summary> Current level loaded. </summary>
        private Level _currentLevel;

        /// <summary> Background song. </summary>
        private Song _backgroundSong;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="game"> The game.</param>
        /// <param name="levelTag"> The level tag.</param>
        public LevelScene(SuperPlatformerGame game, string levelTag) : base(game)
        {
            _levelTag = levelTag;
            BackgroundColor = Color.Bisque;
        }

        /// <summary>
        /// Initialize the level.
        /// </summary>
        public override void Init()
        {
            // Call the init from its parent.
            base.Init();

            // Create the current level.
            _currentLevel = new Level(_levelTag, Game);

            // Load the current level.
            _currentLevel.Load(Game, _levelTag);

            _backgroundSong = Game.Content.Load<Song>("Audio/smw_overworld");

            MediaPlayer.Play(_backgroundSong);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.6f;

            // Add the current level to the monoscene.
            Children.Add(_currentLevel);

            _currentLevel.OnEnd += OnLevelEnded;
        }

        private void OnLevelEnded(object sender, EventArgs e)
        {
            MediaPlayer.Stop();
            Game.SceneActivator.ActivateScene(new GameOverScene(Game));
        }

        /// <summary>
        /// Update function (IMonoUpdatable).
        /// </summary>
        /// <param name="gameTime"> Game time.</param>
        public override void Update(GameTime gameTime)
        {
            // Update its base.
            base.Update(gameTime);

            // Check if the time is up.
            if (_currentLevel.TimeUp())
            {
                MediaPlayer.Stop();
                Game.SceneActivator.ActivateScene(new TimeUpScene(Game));
            }
        }

        /// <summary>
        /// Render scene's children
        /// </summary>
        /// <param name="spriteBatch"> Spritebatch to draw with.</param>
        /// <param name="graphics"> GraphicsDevice to use.</param>
        public override void Render(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            // Clear the screen
            graphics.Clear(BackgroundColor);

            // Render all childeren.
            Children.ForEach((child) =>
            {
                child.Render(spriteBatch, graphics);
            });
        }
    }
}
