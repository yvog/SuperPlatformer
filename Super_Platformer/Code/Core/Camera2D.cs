using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Super_Platformer.Code.Core
{
    /// <summary>
    /// Camera class that handles the movement of the camera.
    /// </summary>
    public class Camera2D : IMonoUpdateable
    {
        /// <summary> The drawable being followed. </summary>
        private Entity _target;

        /// <summary> Position of the camera. </summary>
        public Vector2 Position
        {
            get;
            private set;
        }

        /// <summary> Origin of this camera. </summary>
        private Vector2 _origin;

        /// <summary> Rotation amount. </summary>
        private float _rotation;

        /// <summary> Zoom level. </summary>
        private float _zoom;

        /// <summary> Maximum position X and Y. </summary>
        public Vector2 MaxPosition
        {
            get;
            private set;
        }

        /// <summary> Scale. </summary>
        private int _scale;

        /// <summary> Viewport of the game. </summary>
        private Viewport _viewport;

        /// <summary> Relative position of deadzone. </summary>
        private float _deadzone;

        /// <summary> Relative postion of anchor. </summary>
        private float _anchor;

        /// <summary> Position the camera should move to. </summary>
        private float _toPositionX;

        /// <summary> Terminal velocity on X axis for camera. </summary>
        private float _terminalVelocityX;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="viewport"> The viewport of the game.</param>
        public Camera2D(Viewport viewport)
        {
            // Set the viewport
            _viewport = viewport;

            // Set the scale.
            _scale = SuperPlatformerGame.SCALE;

            // Set the terminal velocity.
            _terminalVelocityX = 10;

            // No rotation
            _rotation = 0f;

            // No zoom
            _zoom = 1f;

            // Set the origin to X: one-third of the viewport, Y: half of the viewport
            _origin = new Vector2(_viewport.Width * 0.33f, _viewport.Width * 0.5f);

            // X: 0, Y: 0
            Position = Vector2.Zero;

            // Set the relative deadzone position.
            _deadzone = 0.28f;

            // Set the relative anchor position.
            _anchor = 0.40f;
        }

        /// <summary>
        /// Sets the target to follow
        /// </summary>
        /// <param name="target"> Drawable to follow</param>
        public void Follow(Entity target)
        {
            _target = target;
        }

        /// <summary>
        /// Set the bounds of the camera.
        /// </summary>
        /// <param name="pixelSizeX"> Total pixels in x axis.</param>
        /// <param name="pixelSizeY"> Total pixels in y axis.</param>
        public void SetBounds(int pixelSizeX, int pixelSizeY)
        {
            MaxPosition = new Vector2(pixelSizeX - _viewport.Width, pixelSizeY - _viewport.Height);
        }

        /// <summary>
        /// Retrieve the transformed matrix
        /// </summary>
        /// <returns>Matrix transformation</returns>
        public Matrix GetViewMatrix()
        {
            return
                Matrix.CreateTranslation(new Vector3(-Position, 0.0f)) *
                Matrix.CreateTranslation(new Vector3(-_origin, 0.0f)) *
                Matrix.CreateRotationZ(_rotation) *
                Matrix.CreateScale(_zoom, _zoom, 1) *
                Matrix.CreateTranslation(new Vector3(_origin, 0.0f));
        }

        /// <summary>
        /// Check if a object is in the viewport
        /// </summary>
        /// <param name="position"> Top left position of the object.</param>
        /// <param name="width"> Width of the object.</param>
        /// <param name="height"> Height of the object.</param>
        /// <returns></returns>
        public bool IsVisible(Vector2 position, int width, int height)
        {
            return
                ((position.X + width) * _scale) >= Position.X &&
                (position.X * _scale) < (Position.X + _viewport.Width) &&
                ((position.Y + height) * _scale) >= Position.Y &&
                (position.Y * _scale) < (Position.Y + _viewport.Height);
        }

        /// <summary>
        /// Update the camera to its position
        /// </summary>
        /// <param name="gameTime"> Game time</param>
        public void Update(GameTime gameTime)
        {
            // Does the camera follow a drawable?
            if (_target != null)
            {
                // Get current x position of camera.
                float positionX = Position.X;

                // Get current x position of protagonist.
                float targetX = ((_target.Position.X + (_target.Width * 0.5f)) * _scale);

                // Get relative position of protagonist in viewport.
                float targetInViewport = (targetX - _toPositionX) / _viewport.Width; //0 = left, 1 right;

                // Float to determine where the camera should move to.
                float _viewportPosition = 0;

                // check if protagonist is on the right side of the screen.
                if (targetInViewport > 0.5f)
                {
                    // If protagonist moves trough the right anchor, reposition camera to right anchor.
                    if (targetInViewport < (1 - _anchor))
                    {
                        _viewportPosition = _anchor;
                    }
                    // If protagonist moves in the right deadzone, move the camera to the left anchor.
                    else if (targetInViewport >= 1 - _deadzone)
                    {
                        _viewportPosition = 1 - _anchor;
                    }
                }
                else
                {
                    // If protagonist moves trough the left anchor, reposition camera to left anchor.
                    if (targetInViewport > _anchor)
                    {
                        _viewportPosition = 1 - _anchor;
                    }
                    // If protagonist moves in the left deadzone, move the camera to the right anchor.
                    else if (targetInViewport <= _deadzone)
                    {
                        _viewportPosition = _anchor;
                    }
                }

                // Check if camera movement needed.
                if (_viewportPosition != 0)
                {
                    _toPositionX = Math.Max(0, (targetX + (_viewport.Width * _viewportPosition)) - _viewport.Width);
                }

                // If there is a big camera movement use low terminal velocity.
                if (Math.Abs(targetInViewport - ((targetX - positionX) / _viewport.Width)) > .05f)
                {
                    _terminalVelocityX = 14;
                }
                // If there is a small movement use a high terminal velocity.
                else
                {
                    _terminalVelocityX = 20;
                }

                // Calculate the delta time.
                float msPerSecond = 1000;
                float delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds / msPerSecond;

                // Move the camerea smooth.
                positionX = positionX + ((_toPositionX - positionX) * _terminalVelocityX * delta);

                // Move the camera along with player
                Position = new Vector2(positionX, ((float)Math.Ceiling(_target.Position.Y) * _scale) - _origin.Y);

                // Keep the camera position in bounds.
                Position = new Vector2(
                    MathHelper.Clamp(Position.X, 0, MaxPosition.X),
                    MathHelper.Clamp(Position.Y, 0, MaxPosition.Y)
                );
            }
        }
    }
}
