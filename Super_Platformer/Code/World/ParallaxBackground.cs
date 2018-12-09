using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Super_Platformer.Code.Core;
using Super_Platformer.Code.Core.Rendering;

namespace Super_Platformer.Code.World
{
    /// <summary>
    /// Parallax background class.
    /// </summary>
    public class ParallaxBackground : IMonoDrawable, IMonoUpdateable
    {
        /// <summary> The camera in witch the background is visable. </summary>
        private Camera2D _camera;
        /// <summary> List with background images. </summary>
        private List<MonoSprite> _backgrounds;
        /// <summary> The scale. </summary>
        private int _scale;
        /// <summary> The strength of the parralax effect. </summary>
        private float _parallaxEffect;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="camera"> The camera.</param>
        /// <param name="backgroundLeft"> Left background sprite.</param>
        /// <param name="backgroundRight"> Right background sprite.</param>
        public ParallaxBackground(Camera2D camera, MonoSprite backgroundLeft, MonoSprite backgroundRight)
        {
            // Set the scale.
            _scale = SuperPlatformerGame.SCALE;

            // Set the paralax effect strength.
            _parallaxEffect = 3f;

            // Set the camera.
            _camera = camera;

            // Create the backgrounds list.
            _backgrounds = new List<MonoSprite>();

            // Add the backgrounds.
            _backgrounds.Add(backgroundLeft);
            _backgrounds.Add(backgroundRight);
        }

        /// <summary>
        /// Update function (IMonoUpdatable).
        /// </summary>
        /// <param name="gameTime"> Game time.</param>
        public void Update(GameTime gameTime)
        {
            float bgCenterX = (_backgrounds[0].Width * 0.5f);

            // Calculate times camera is passed over background.
            int timesMoved = (int)(Math.Floor(_camera.Position.X / (_scale * _parallaxEffect)) / bgCenterX);

            // Store the position
            Vector2 position = _backgrounds[0].Position;

            // Set background x position to camera position / parallax effect + times the camera moved passed the background.
            position.X = ((float)Math.Floor(_camera.Position.X / (_scale * _parallaxEffect))) + (timesMoved * _backgrounds[0].Width);

            // Set background x position to camera position / parallax effect.
            position.Y = ((float)Math.Floor(_camera.Position.Y / (_scale * _parallaxEffect)));

            // Apply the new position
            _backgrounds[0].Position = position;

            // Set background right to behind background left.
            position.X = _backgrounds[0].Position.X + _backgrounds[0].Width;

            // background y cord is the same.
            position.Y = _backgrounds[0].Position.Y;

            // Apply the new position
            _backgrounds[1].Position = position;
        }

        /// <summary>
        /// Render function (IMonoDrawable)
        /// </summary>
        /// <param name="spriteBatch"> Spritebatch to draw to.</param>
        /// <param name="graphics"> GraphicsDevice to use.</param>
        public void Render(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            _backgrounds[0].Render(spriteBatch, graphics);
            _backgrounds[1].Render(spriteBatch, graphics);
        }
    }
}