using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Super_Platformer.Code.Core
{
    /// <summary>
    /// MonoObject class.
    /// </summary>
    public abstract class MonoObject : IMonoDrawable, IMonoUpdateable
    {
        /// <summary> Keeps track if the input is enabled. </summary>
        public bool InputEnabled
        {
            get;
            private set;
        }

        /// <summary> Can the object can be destroyed. </summary>
        public bool Destroyable
        {
            get;
            private set;
        }

        /// <summary>
        /// Enable the input.
        /// </summary>
        public void EnableInput()
        {
            InputEnabled = true;
        }


        /// <summary>
        /// Disable the input.
        /// </summary>
        public void IgnoreInput()
        {
            InputEnabled = false;
        }

        /// <summary>
        /// Reset funciton.
        /// </summary>
        public virtual void Reset()
        {
            // Nothing to reset here
        }

        /// <summary>
        /// Destroy object.
        /// </summary>
        public void Destroy()
        {
            Destroyable = true;
        }

        /// <summary>
        /// Render its children
        /// </summary>
        /// <param name="spriteBatch"> Spritebatch to draw to.</param>
        /// <param name="graphics"> GraphicsDevice to use.</param>
        public abstract void Render(SpriteBatch spriteBatch, GraphicsDevice graphics);

        /// <summary>
        /// Update function (IMonoUpdatable).
        /// </summary>
        /// <param name="gameTime"> Game time.</param>
        public abstract void Update(GameTime gameTime);

    }
}

