using Microsoft.Xna.Framework;

namespace Super_Platformer.Code.Core
{
    /// <summary>
    /// Update interface.
    /// </summary>
    public interface IMonoUpdateable
    {
        /// <summary>
        /// Update function.
        /// </summary>
        /// <param name="gameTime"> Game time.</param>
        void Update(GameTime gameTime);
    }
}