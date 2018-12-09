using Microsoft.Xna.Framework.Graphics;

namespace Super_Platformer.Code.Core
{
    /// <summary>
    /// Draw/Render interface.
    /// </summary>
    public interface IMonoDrawable
    {
        /// <summary>
        /// Render function.
        /// </summary>
        /// <param name="spriteBatch"> Spritebatch to draw to.</param>
        /// <param name="graphics"> GraphicsDevice to use.</param>
        void Render(SpriteBatch spriteBatch, GraphicsDevice graphics);
    }
}

