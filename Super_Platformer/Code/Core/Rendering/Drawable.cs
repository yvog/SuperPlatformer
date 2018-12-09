using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Super_Platformer.Code.Core.Rendering
{
    /// <summary>
    /// Drawable class.
    /// </summary>
    public abstract class Drawable : MonoObject
    {
        /// <summary> Position of the drawable. </summary>
        public Vector2 Position
        {
            get;
            set;
        }

        /// <summary> Width of the drawable. </summary>
        public int Width
        {
            get;
            set;
        }

        /// <summary> Height of the drawable. </summary>
        public int Height
        {
            get;
            set;
        }

        /// <summary> Scale of the drawable. </summary>
        protected int Scale
        {
            get;
            set;
        }

        /// <summary> Origin of the drawable. </summary>
        protected Vector2 Origin
        {
            get;
            set;
        }

        /// <summary> Layer of the drawable. </summary>
        protected float Layer
        {
            get;
            set;
        }

        /// <summary> Visibility of the drawable. </summary>
        public bool Visible
        {
            get;
            set;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="position"> Position of the drawable.</param>
        /// <param name="width"> Width of the drawable.</param>
        /// <param name="height"> Height of the drawable.</param>
        public Drawable(Vector2 position, int width, int height)
        {
            // Set the position.
            Position = position;

            // Set the width.
            Width = width;

            // Set the height.
            Height = height;

            // Set the scale.
            Scale = SuperPlatformerGame.SCALE;

            // Set the origin.
            Origin = Vector2.Zero;

            // Set the layer.
            Layer = 0f;

            // Set visible.
            Visible = true;
        }

        /// <summary>
        /// Update function (IMonoUpdatable).
        /// </summary>
        /// <param name="gameTime"> Game time.</param>
        public abstract override void Update(GameTime gameTime);

        /// <summary>
        /// Render function (IMonoDrawable)
        /// </summary>
        /// <param name="spriteBatch"> Spritebatch to draw to.</param>
        /// <param name="graphics"> GraphicsDevice to use.</param>
        public abstract override void Render(SpriteBatch spriteBatch, GraphicsDevice graphics);
    }
}

