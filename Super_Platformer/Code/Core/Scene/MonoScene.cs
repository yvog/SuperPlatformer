using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Super_Platformer.Code.Core.Scene
{
    /// <summary>
    /// Base class of a scene.
    /// </summary>
    public abstract class MonoScene : IMonoDrawable, IMonoUpdateable
    {
        /// <summary> The game this scene belongs to. </summary>
        protected SuperPlatformerGame Game
        {
            get;
            private set;
        }

        /// <summary> The children of this scene. </summary>
        protected List<MonoObject> Children;

        /// <summary> Indicates whether this scene was initialized or not.</summary>
        public bool Initialized
        {
            get;
            private set;
        }

        /// <summary> The scene's background color.</summary>
        protected Color BackgroundColor
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public MonoScene(SuperPlatformerGame game)
        {
            Game = game;
            Children = new List<MonoObject>();
            BackgroundColor = Color.Black;
        }

        /// <summary>
        /// Initialize this scene.
        /// </summary>
        public virtual void Init()
        {
            Initialized = true;
        }

        /// <summary>
        /// Reset the scene.
        /// </summary>
        public virtual void Reset()
        {
            //
        }

        /// <summary>
        /// Update its children.
        /// </summary>
        /// <param name="gameTime"> The Gametime.</param>
        public virtual void Update(GameTime gameTime)
        {
            Children.ForEach(c =>
            {
                c.Update(gameTime);
            });
        }

        /// <summary>
        /// Render scene's children
        /// </summary>
        /// <param name="spriteBatch"> Spritebatch to draw with.</param>
        /// <param name="graphics"> GraphicsDevice to use.</param>
        public virtual void Render(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            // Clear the screen
            graphics.Clear(BackgroundColor);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // Render all childeren.
            Children.ForEach((child) =>
            {
                child.Render(spriteBatch, graphics);
            });

            spriteBatch.End();
        }
    }
}

