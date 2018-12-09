using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Super_Platformer.Code.Core.Rendering
{
    /// <summary>
    /// Sprite class.
    /// </summary>
    public class MonoSprite : Drawable
    {
        /// <summary> Texture of the sprite. </summary>
        protected Texture2D Texture
        {
            get;
            set;
        }

        /// <summary> Source of the sprite. </summary>
        public Rectangle source;

        /// <summary> Mirror the sprite. </summary>
        public bool Mirror
        {
            get;
            set;
        }

        /// <summary> Rotate the sprite. </summary>
        protected float Rotation
        {
            get;
            set;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="texture"> Texture of sprite.</param>
        /// <param name="source"> Location of the sprite.</param>
        /// <param name="position"> Location in game.</param>
        /// <param name="width"> Width of the sprite.</param>
        /// <param name="height"> Height of the sprite.</param>
        public MonoSprite(Texture2D texture, Rectangle source, Vector2 position, int width, int height) :
            base(position, width, height)
        {
            // Set the texture.
            Texture = texture;

            // Default not mirrord.
            Mirror = false;

            // No rotation.
            Rotation = 0;

            // Set the source.
            this.source = source;
        }

        /// <summary>
        /// Update the sprite, if wanted
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            //   
        }

        /// <summary>
        /// Render the sprite
        /// </summary>
        /// <param name="spriteBatch"> Spritebatch to draw to.</param>
        /// <param name="graphics"> GraphicsDevice to use.</param>
        public override void Render(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            if (Visible)
            {
                SpriteEffects effect = SpriteEffects.None;

                if (Mirror)
                {
                    // Mirror this sprite horizontally by setting a sprite effect
                    effect = SpriteEffects.FlipHorizontally;
                }

                spriteBatch.Draw(Texture, new Vector2((float)Math.Floor(Position.X) * Scale, (float)Math.Floor(Position.Y) * Scale), source, Color.White, Rotation, Origin, Scale, effect, Layer);
            }

        }
    }
}

