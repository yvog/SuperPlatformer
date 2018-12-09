using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Super_Platformer.Code.Core;
using Super_Platformer.Code.Core.Rendering;
using Super_Platformer.Code.Item;
using Super_Platformer.Code.World;

namespace Super_Platformer.Code.UI
{
    /// <summary>
    /// Heads up display.
    /// </summary>
    public class HUD : IMonoUpdateable, IMonoDrawable
    {
        /// <summary> The level where this HUD is being used. </summary>
        private Level _level;

        /// <summary> The background image. </summary>
        private MonoSprite _background;

        /// <summary> Powerup in hud. </summary>
        private PowerUp _powerUp;

        /// <summary> protagonist/Luigi name image. </summary>
        private MonoSprite _playerName;

        /// <summary> Font to draw text. </summary>
        private SpriteFont _font;

        /// <summary> the camera. (for dropping the mushroom on right position) </summary>
        private Camera2D _camera;

        /// <summary> Position to draw lives. </summary>
        private Vector2 _livesPosition;

        /// <summary> Position to draw time. </summary>
        private Vector2 _currentTimePosition;

        /// <summary> Position to draw coin count. </summary>
        private Vector2 _totalCoinsPosition;

        /// <summary> Position to draw score. </summary>
        private Vector2 _totalScorePosition;

        /// <summary> The scale of the game. </summary>
        private int _scale;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="level"> The level.</param>
        /// <param name="camera"> The camera.</param>
        /// <param name="content"> The content manager.</param>
        public HUD(Level level, Camera2D camera, ContentManager content)
        {
            // Get background texture.
            Texture2D texture = content.Load<Texture2D>("Images/HUD");

            // Set level.
            _level = level;

            // Set camera.
            _camera = camera;

            // Set background texture.
            _background = new MonoSprite(texture, new Rectangle(0, 0, 256, 28), new Vector2(0, 5), 256, 28);

            // Set player name texture.
            _playerName = new MonoSprite(texture, new Rectangle(0, 0, 0, 0), new Vector2(14, 10), 40, 8);

            // rectangle to store location of playername.
            Rectangle playerNameSource;

            // If player is protagonist use this texture.
            if (level.TypePlayer == Level.PlayerType.PROTAGONIST)
            {
                playerNameSource = new Rectangle(0, 28, 40, 8);
            }
            // If player is luigi use this texture.
            else
            {
                playerNameSource = new Rectangle(40, 28, 40, 8);
            }

            // Set player name source.
            _playerName.source = playerNameSource;

            // Set the font.
            _font = content.Load<SpriteFont>("Font/PixelFont");

            // Set the scale.
            _scale = SuperPlatformerGame.SCALE;

            // set locations to draw text.
            _livesPosition = new Vector2(35 * _scale, 19 * _scale);
            _currentTimePosition = new Vector2(153 * _scale, 20 * _scale);
            _totalCoinsPosition = new Vector2(227 * _scale, 10 * _scale);
            _totalScorePosition = new Vector2(200 * _scale, 20 * _scale);
        }

        /// <summary>
        /// Update function (IMonoUpdatable).
        /// </summary>
        /// <param name="gameTime"> Game time.</param>
        public void Update(GameTime gameTime)
        {
            //
        }

        /// <summary>
        /// Drop powerup from hud if there is one.
        /// </summary>
        public void DropPowerUp()
        {
            if (_powerUp != null)
            {
                _powerUp.Position = new Vector2(((_camera.Position.X / _scale) + (SuperPlatformerGame.RESOLUTION_X * 0.5f)), _camera.Position.Y / _scale);

                _powerUp.OnDrop();
                _level.AddEntity(_powerUp);
                _powerUp = null;
            }
        }

        /// <summary>
        /// Add powerup to hud if there is none.
        /// </summary>
        /// <param name="powerUp"> The powerup to add.</param>
        public void AddPowerUp(PowerUp powerUp)
        {
            if (_powerUp == null || _powerUp.GetType() != powerUp.GetType())
            {
                powerUp.Position = new Vector2(120, 11);

                powerUp.SyncTexture();

                _powerUp = powerUp;
            }
        }

        /// <summary>
        /// Render function (IMonoDrawable)
        /// </summary>
        /// <param name="spriteBatch"> Spritebatch to draw to.</param>
        /// <param name="graphics"> GraphicsDevice to use.</param>
        public void Render(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _background.Render(spriteBatch, graphics);
            _playerName.Render(spriteBatch, graphics);

            if (_powerUp != null)
            {
                _powerUp.Render(spriteBatch, graphics);
            }

            RenderText(spriteBatch);

            spriteBatch.End();
        }

        /// <summary>
        /// Render the text to the screen.
        /// </summary>
        /// <param name="spriteBatch"> The spritebatch to use.</param>
        private void RenderText(SpriteBatch spriteBatch)
        {
            // Get lives left.
            string lives = _level.Player.Lives.ToString();
            // Get current time.
            string currentTime = _level.CurrentTime.ToString();
            // Get coins.
            string totalCoins = _level.Score.TotalCoins.ToString();
            // Get score.
            string totalScore = _level.Score.TotalScore.ToString().PadLeft(6, '0');

            // Calculate textscale relative to game scale.
            float textScale = _scale * 0.33f;

            // Draw black outline.
            #region DIRTY BLACK OUTLINE
            int blackSpacing = 1 + (_scale / 2);
            spriteBatch.DrawString(_font, lives, new Vector2(_livesPosition.X - blackSpacing, _livesPosition.Y), Color.Black, 0f, Vector2.Zero, textScale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(_font, lives, new Vector2(_livesPosition.X + blackSpacing, _livesPosition.Y), Color.Black, 0f, Vector2.Zero, textScale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(_font, lives, new Vector2(_livesPosition.X, _livesPosition.Y - blackSpacing), Color.Black, 0f, Vector2.Zero, textScale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(_font, lives, new Vector2(_livesPosition.X, _livesPosition.Y + blackSpacing), Color.Black, 0f, Vector2.Zero, textScale, SpriteEffects.None, 1f);

            spriteBatch.DrawString(_font, currentTime, new Vector2(_currentTimePosition.X - blackSpacing, _currentTimePosition.Y), Color.Black, 0f, Vector2.Zero, textScale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(_font, currentTime, new Vector2(_currentTimePosition.X + blackSpacing, _currentTimePosition.Y), Color.Black, 0f, Vector2.Zero, textScale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(_font, currentTime, new Vector2(_currentTimePosition.X, _currentTimePosition.Y - blackSpacing), Color.Black, 0f, Vector2.Zero, textScale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(_font, currentTime, new Vector2(_currentTimePosition.X, _currentTimePosition.Y + blackSpacing), Color.Black, 0f, Vector2.Zero, textScale, SpriteEffects.None, 1f);

            spriteBatch.DrawString(_font, totalCoins, new Vector2(_totalCoinsPosition.X - blackSpacing, _totalCoinsPosition.Y), Color.Black, 0f, Vector2.Zero, textScale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(_font, totalCoins, new Vector2(_totalCoinsPosition.X + blackSpacing, _totalCoinsPosition.Y), Color.Black, 0f, Vector2.Zero, textScale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(_font, totalCoins, new Vector2(_totalCoinsPosition.X, _totalCoinsPosition.Y - blackSpacing), Color.Black, 0f, Vector2.Zero, textScale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(_font, totalCoins, new Vector2(_totalCoinsPosition.X, _totalCoinsPosition.Y + blackSpacing), Color.Black, 0f, Vector2.Zero, textScale, SpriteEffects.None, 1f);

            spriteBatch.DrawString(_font, totalScore, new Vector2(_totalScorePosition.X - blackSpacing, _totalScorePosition.Y), Color.Black, 0f, Vector2.Zero, textScale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(_font, totalScore, new Vector2(_totalScorePosition.X + blackSpacing, _totalScorePosition.Y), Color.Black, 0f, Vector2.Zero, textScale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(_font, totalScore, new Vector2(_totalScorePosition.X, _totalScorePosition.Y - blackSpacing), Color.Black, 0f, Vector2.Zero, textScale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(_font, totalScore, new Vector2(_totalScorePosition.X, _totalScorePosition.Y + blackSpacing), Color.Black, 0f, Vector2.Zero, textScale, SpriteEffects.None, 1f);

            #endregion

            // Draw white text ontop of black text.
            spriteBatch.DrawString(_font, lives, _livesPosition, Color.White, 0f, Vector2.Zero, textScale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(_font, currentTime, _currentTimePosition, Color.White, 0f, Vector2.Zero, textScale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(_font, totalCoins, _totalCoinsPosition, Color.White, 0f, Vector2.Zero, textScale, SpriteEffects.None, 1f);
            spriteBatch.DrawString(_font, totalScore, _totalScorePosition, Color.White, 0f, Vector2.Zero, textScale, SpriteEffects.None, 1f);
        }
    }
}
