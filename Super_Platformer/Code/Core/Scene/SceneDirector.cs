using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Super_Platformer.Code.Core.Scene
{
    /// <summary>
    /// Scene director manages the active scene.
    /// </summary>
    public class SceneDirector : MonoObject
    {
        /// <summary> The activated scene. </summary>
        private MonoScene _activeScene;

        /// <summary>
        /// Activate a new scene by tag
        /// </summary>
        /// <param name="scene"> The scene to be activated.</param>
        public void ActivateScene(MonoScene scene)
        {
            // Initialize the scene.
            scene.Init();

            // Set the active scene to provided scene.
            _activeScene = scene;
        }

        /// <summary>
        /// Update the activated scene
        /// </summary>
        /// <param name="gameTime">Game Time</param>
        public override void Update(GameTime gameTime)
        {
            // Update the active scene.
            _activeScene.Update(gameTime);
        }

        /// <summary>
        /// Render the activated scene.
        /// </summary>
        /// <param name="spriteBatch"> Spritebatch to draw to.</param>
        /// <param name="graphics"> GraphicsDevice to use.</param>
        public override void Render(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            // Render the active scene.
            _activeScene.Render(spriteBatch, graphics);
        }
    }
}

