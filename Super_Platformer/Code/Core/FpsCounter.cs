using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Super_Platformer.Code.Core
{
    /// <summary>
    /// FPS counter class
    /// </summary>
    public class FpsCounter : IMonoUpdateable, IMonoDrawable
    {
        /// <summary> Font to use. </summary>
        private SpriteFont _font;
        /// <summary> Total Frame Count. </summary>
        private int _totalFrames;
        /// <summary> Elapsed time. </summary>
        private float _elapsedTime;
        /// <summary> frames per second. </summary>
        private int _fps;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="font"> Font to use.</param>
        public FpsCounter(SpriteFont font)
        {
            _font = font;
            _totalFrames = 0;
            _elapsedTime = 0;
            _fps = 0;
        }

        /// <summary>
        /// Update function (IMonoUpdatable).
        /// </summary>
        /// <param name="gameTime"> Game time.</param>
        public void Update(GameTime gameTime)
        {
            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_elapsedTime > 1000.0f)
            {
                // Calculate fps
                _fps = (int)(_totalFrames * 1000 / _elapsedTime);
                // Reset counter.
                _totalFrames = 0;
                // Reset timer.
                _elapsedTime = 0;
            }
        }

        /// <summary>
        /// Render function (IMonoDrawable)
        /// </summary>
        /// <param name="spriteBatch"> Spritebatch to draw to.</param>
        /// <param name="graphics"> GraphicsDevice to use.</param>
        public void Render(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            // Only count frames we actualy draw.
            _totalFrames++;

            spriteBatch.Begin();
            // Draw fps on screen.
            spriteBatch.DrawString(_font, "FPS: " + _fps, new Vector2(5, 5), Color.DarkRed);
            spriteBatch.End();
        }
    }
}
