using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Super_Platformer.Code.Core.UI
{
    /// <summary>
    /// Option List Item.
    /// </summary>
    public class OptionListItem : IMonoUpdateable, IMonoDrawable
    {
        /// <summary> Position of the item.</summary>
        public Vector2 Position
        {
            get;
            set;
        }

        /// <summary> Text of the item.</summary>
        public string Text
        {
            get;
            set;
        }

        /// <summary> Color of the item.</summary>
        public Color FillStyle
        {
            get;
            set;
        }

        /// <summary> Indicates whether this item is selected or not.</summary>
        public bool Selected
        {
            get;
            private set;
        }

        /// <summary> Scale of the item.</summary>
        public int Scale
        {
            get;
            set;
        }

        /// <summary> Callback of the item.</summary>
        private Action _callback;

        /// <summary> Font of the item.</summary>
        private SpriteFont _font;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="text"> Text of the item.</param>
        /// <param name="font"> Font of the item.</param>
        /// <param name="fillStyle"> Color of the item.</param>
        /// <param name="callback"> Callback on activation</param>
        public OptionListItem(string text, SpriteFont font, Color fillStyle, Action callback)
        {
            Text = text;
            Selected = false;
            Scale = 1;
            FillStyle = fillStyle;

            _font = font;
            _callback = callback;
        }

        /// <summary>
        /// Invoke the callback.
        /// </summary>
        public void OnAction()
        {
            _callback();
        }

        /// <summary>
        /// Update the item.
        /// </summary>
        /// <param name="gameTime"> Game Time.</param>
        public void Update(GameTime gameTime)
        {
            //
        }

        /// <summary>
        /// Render the item.
        /// </summary>
        /// <param name="spriteBatch"> Spritebatch.</param>
        /// <param name="graphics"> GraphicsDevice.</param>
        public void Render(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            float shrinkFactor = 0.33f;
            float depth = 1f;
            float rotation = 0f;

            spriteBatch.DrawString(_font, Text, Position, FillStyle, rotation, Vector2.Zero, shrinkFactor * Scale, SpriteEffects.None, depth);
        }
    }
}
