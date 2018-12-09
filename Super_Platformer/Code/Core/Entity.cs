using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Super_Platformer.Code.Core.Physics;
using Super_Platformer.Code.Core.Rendering;
using Super_Platformer.Code.Core.Spritesheet;
using Super_Platformer.Code.World;
using static Super_Platformer.Code.Core.Physics.CollisionTester;

namespace Super_Platformer.Code.Core
{
    /// <summary>
    /// Entity class.
    /// </summary>
    public abstract class Entity : MonoObject
    {
        /// <summary> AnimationSequencer handles the animations.</summary>
        protected AnimationSequencer Animations
        {
            get;
            set;
        }

        /// <summary> Sprite of the entity.</summary>
        public MonoSprite Sprite
        {
            get;
            protected set;
        }

        /// <summary> Bounds from previous frame.</summary>
        public Rectangle PreviousBounds
        {
            get;
            protected set;
        }

        /// <summary> Bounding box.</summary>
        private Rectangle _bounds;

        /// <summary> Collision bounds of the entity.</summary>
        public Rectangle Bounds
        {
            get
            {
                return _bounds;
            }
            private set
            {
                _bounds = value;
            }
        }

        /// <summary> Amount of space between border and body.</summary>
        protected Vector2 Padding
        {
            get;
            set;
        }

        /// <summary> Is the entity collidable.</summary>
        public bool Collidable
        {
            get;
            protected set;
        }

        /// <summary> Is the entity Solid.</summary>
        public bool Solid
        {
            get;
            protected set;
        }

        /// <summary> Should the entity check its own collisions.</summary>
        public bool CheckCollisions
        {
            get;
            protected set;
        }

        /// <summary> Location in game.</summary>
        public Vector2 Position
        {
            get;
            set;
        }

        /// <summary> Width of the drawable.</summary>
        public int Width
        {
            get;
            set;
        }

        /// <summary> Height of the drawable.</summary>
        public int Height
        {
            get;
            set;
        }

        /// <summary> Parent level of this entity.</summary>
        protected Level Parent
        {
            get;
            private set;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="position"> Location in game.</param>
        /// <param name="width"> Width of the entity.</param>
        /// <param name="height"> Height of the entity.</param>
        /// <param name="level"> The level the Entity is in.</param>
        public Entity(Vector2 position, int width, int height, Level level)
        {
            Parent = level;

            Position = position;

            Width = width;
            Height = height;

            Animations = new AnimationSequencer();
            Bounds = new Rectangle((int)position.X, (int)position.Y, width, height);

            Solid = true;

            CheckCollisions = true;

            Collidable = true;
        }

        /// <summary>
        /// Returns if the entity is collidable.
        /// </summary>
        /// <param name="ent"> Entity this object collides with.</param>
        /// <param name="axis"> The axis that is being checked.</param>
        /// <returns>True if the entity is collidable.</returns>
        public virtual bool IsCollidable(Entity ent, CollisionTester.Axis axis)
        {
            return Collidable;
        }

        /// <summary>
        /// Function gets called when a collision with this object occurs.
        /// </summary>
        /// <param name="ent"> Entity this object collides with.</param>
        /// <param name="penetration"> Amount of penetration.</param>
        /// <param name="side"> The side the objects collide on.</param>
        /// <param name="axis"> The axis that is being checked.</param>
        /// <param name="fromBounds"> Where the colliding entity came from.</param>
        public virtual void OnCollision(Entity ent, int penetration, CollisionTester.CollisionSide side, CollisionTester.Axis axis, Rectangle fromBounds)
        {
            //
        }

        /// <summary>
        /// Update function (IMonoUpdatable)
        /// </summary>
        /// <param name="gameTime">Gametime</param>
        public override void Update(GameTime gameTime)
        {
            UpdateAnimation(gameTime);
        }

        /// <summary>
        /// Updates the sprite with the animation sequencer.
        /// </summary>
        /// <param name="gameTime"> Gametime.</param>
        protected void UpdateAnimation(GameTime gameTime)
        {
            // Check if animation is running.
            if (Animations.Running)
            {
                // Update the animation.
                Animations.Update(gameTime);

                // Set sprite to current frame.
                Sprite.source.X = Animations.GetCurrentFrameX();
                Sprite.source.Y = Animations.GetCurrentFrameY();
            }
        }

        /// <summary>
        /// Sync the bounding box to the current entity position.
        /// </summary>
        protected void SyncBoundingBox()
        {
            // Set the bounding box to the current x position (with padding).
            _bounds.X = (int)(Position.X + Padding.X);

            // Set the bounding box to the current Y position (with padding).
            _bounds.Y = (int)(Position.Y + Padding.Y);

            // Set the bounding box to the current width (with padding).
            _bounds.Width = (int)(Width - Padding.X * 2);

            // Set the bounding box to the current height (with padding).
            _bounds.Height = (int)(Height - Padding.Y);
        }

        /// <summary>
        /// Sync the texture to the current entity position.
        /// </summary>
        public void SyncTexture()
        {
            // Set the current position.
            Sprite.Position = Position;

            // Set the current width and height.
            Sprite.Width = Sprite.source.Width = Width;
            Sprite.Height = Sprite.source.Height = Height;
        }

        /// <summary>
        /// Render function (IMonoDrawable)
        /// </summary>
        /// <param name="spriteBatch"> Spritebatch to draw to.</param>
        /// <param name="graphics"> GraphicsDevice to use.</param>
        public override void Render(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            Sprite.Render(spriteBatch, graphics);
        }

    }

}