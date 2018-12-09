using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Super_Platformer.Code.Core.Physics;
using Super_Platformer.Code.World;
using static Super_Platformer.Code.Core.Physics.CollisionTester;

namespace Super_Platformer.Code.Core
{
    /// <summary>
    /// MovingEntity class.
    /// </summary>
    public abstract class MovingEntity : Entity
    {
        /// <summary>
        /// The side the enemy is facing.
        /// </summary>
        protected enum Facing : int
        {
            /// <summary> Facing to the left. </summary>
            LEFT = 0,
            /// <summary> Facing to the right. </summary>
            RIGHT = 1
        }

        /// <summary> Impulse when jumping. </summary>
        protected float JumpImpulse
        {
            get;
            set;
        }

        /// <summary> Maximum velocity. </summary>
        protected Vector2 TerminalVelocity
        {
            get;
            set;
        }

        /// <summary> Gravity value. </summary>
        protected float Gravity
        {
            get;
            set;
        }

        /// <summary> Speed up speed. </summary>
        protected float Acceleration
        {
            get;
            set;
        }

        /// <summary> Current velocity. </summary>
        protected Vector2 velocity;

        /// <summary> Scale. </summary>
        protected int Scale
        {
            get;
            private set;
        }

        /// <summary> Get current velocity. </summary>
        public Vector2 Velocity
        {
            get { return velocity; }
        }

        /// <summary> Can you harm this entity? </summary>
        public bool Invulnerable
        {
            get;
            protected set;
        }

        /// <summary> Entity on ground.</summary>
        protected bool Grounded
        {
            get;
            set;
        }

        /// <summary> Facing of this entity.</summary>
        protected Facing FacingDirection
        {
            get;
            set;
        }

        /// <summary> Deacceleration amount.</summary>
        protected float Friction
        {
            get;
            set;
        }

#if DEBUG_INFO
        Texture2D _pixel;
#endif

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="position"> Location in game.</param>
        /// <param name="width"> Width.</param>
        /// <param name="height"> Height.</param>
        /// <param name="level"> The level the entity is in.</param>
        public MovingEntity(Vector2 position, int width, int height, Level level) :
            base(position, width, height, level)
        {
            FacingDirection = Facing.RIGHT;

            velocity = Vector2.Zero;
            TerminalVelocity = new Vector2(70, 90);

            Gravity = Parent.Gravity;

            JumpImpulse = 250;
            Friction = 0.0396f;

            Grounded = false;
            Invulnerable = false;

            Padding = new Vector2(1, 1);

            Scale = SuperPlatformerGame.SCALE;
        }

        /// <summary>
        /// Gets called when an entity should turn.
        /// </summary>
        public virtual void TurnAround()
        {
            FacingDirection = (Facing)(((int)FacingDirection + 1) % 2);

            if (FacingDirection == Facing.LEFT && velocity.X > 0)
            {
                velocity *= -1;
            }
        }

        /// <summary>
        /// Make the entity jump.
        /// </summary>
        public virtual void Jump()
        {
            velocity.Y = -JumpImpulse;
            Grounded = false;
        }

        /// <summary>
        /// Function gets called when a collision with this object occurs.
        /// </summary>
        /// <param name="ent"> Entity this object collides with.</param>
        /// <param name="penetration"> Amount of penetration.</param>
        /// <param name="side"> The side the objects collide on.</param>
        /// <param name="axis"> The axis that is being checked.</param>
        /// <param name="fromBounds"> Where the colliding entity came from.</param>
        public override void OnCollision(Entity ent, int penetration, CollisionTester.CollisionSide side, CollisionTester.Axis axis, Rectangle fromBounds)
        {
            ResolveCollision(ent, penetration, side);
        }

        /// <summary>
        /// Resolve an occured collision
        /// </summary>
        /// <param name="ent"> The collider.</param>
        /// <param name="penetration"> Depth of the collision.</param>
        /// <param name="side"> Collided side.</param>
        protected void ResolveCollision(Entity ent, int penetration, CollisionTester.CollisionSide side)
        {
            if (ent.Solid)
            {
                Vector2 position = Position;

                if (side == CollisionTester.CollisionSide.TOP)
                {
                    position.Y += penetration;
                    velocity.Y = 0;
                }

                if (side == CollisionTester.CollisionSide.RIGHT)
                {
                    position.X -= penetration;
                    velocity.X = 0;
                }

                if (side == CollisionTester.CollisionSide.BOTTOM)
                {
                    position.Y -= penetration;
                    velocity.Y = 0;
                    Grounded = true;
                }

                if (side == CollisionTester.CollisionSide.LEFT)
                {
                    position.X += penetration;
                    velocity.X = 0;
                }

                Position = position;

                SyncBoundingBox();
            }
        }

        /// <summary>
        /// Mirror the sprite.
        /// </summary>
        protected void SyncFacing()
        {
            Sprite.Mirror = (FacingDirection == Facing.LEFT);
        }

        /// <summary>
        /// Apply updated forces to this entity.
        /// </summary>
        /// <param name="gameTime"> Gametime.</param>
        protected void ApplyPhysics(GameTime gameTime)
        {
            int msPerSecond = 1000;
            float delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds / msPerSecond;

            Vector2 position = Position;

            // Store the original bounding box
            PreviousBounds = Bounds;

            velocity.X = MathHelper.Clamp(velocity.X, -TerminalVelocity.X, TerminalVelocity.X);
            position.X += velocity.X * delta;

            Position = position;

            // Test X-axis collisions
            if (Collidable)
            {
                SyncBoundingBox();

                if (CheckCollisions)
                {
                    Parent.TestCollisions(this, CollisionTester.Axis.X);
                }
            }

            position = Position;

            velocity.Y += Gravity * delta;

            velocity.Y = MathHelper.Clamp(velocity.Y, -TerminalVelocity.Y, TerminalVelocity.Y);

            position.Y += velocity.Y * delta;

            Position = position;

            // Test Y-axis collisions
            if (Collidable)
            {
                SyncBoundingBox();

                if (CheckCollisions)
                {
                    Parent.TestCollisions(this, CollisionTester.Axis.Y);
                }
            }
        }

        /// <summary>
        /// Keep the entity in parent's bounds.
        /// </summary>
        private void KeepInBounds()
        {
            if (Collidable)
            {
                Vector2 oldPosition = Position;

                oldPosition.X = MathHelper.Clamp(Position.X, 0, (Parent.PixelSizeX - Width * Scale) / Scale);

                if (oldPosition != Position)
                {
                    velocity.X = 0;
                }

                Position = oldPosition;
            }
        }

        /// <summary>
        /// Update function (IMonoUpdatable).
        /// </summary>
        /// <param name="gameTime"> Game time.</param>
        public override void Update(GameTime gameTime)
        {
            ApplyPhysics(gameTime);

            KeepInBounds();

            UpdateAnimation(gameTime);

            SyncFacing();
            SyncTexture();
        }

#if DEBUG_INFO
        /// <summary>
        /// Render function (IMonoDrawable)
        /// </summary>
        /// <param name="spriteBatch"> Spritebatch to draw to.</param>
        /// <param name="graphics"> GraphicsDevice to use.</param>
        public override void Render(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            if (SuperPlatformerGame.SHOW_BOUNDINGBOXES)
            {
                if (Height != 0 && Width != 0)
                {
                    DrawBorder(Bounds, 1, Color.Magenta, spriteBatch, graphics);
                }
            }

            base.Render(spriteBatch, graphics);
        }

        private void DrawBorder(Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor, SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            if (_pixel == null)
            {
                _pixel = new Texture2D(graphics, 1, 1, false, SurfaceFormat.Color);
                _pixel.SetData(new[] { Color.White });
            }

            int scale = SuperPlatformerGame.SCALE;

            // Draw top line
            spriteBatch.Draw(_pixel, new Rectangle(rectangleToDraw.X * scale, rectangleToDraw.Y * scale,
                rectangleToDraw.Width * scale, thicknessOfBorder * scale), borderColor);

            // Draw left line
            spriteBatch.Draw(_pixel, new Rectangle(rectangleToDraw.X * scale, rectangleToDraw.Y * scale,
                thicknessOfBorder * scale, rectangleToDraw.Height * scale), borderColor);

            // Draw right line
            spriteBatch.Draw(_pixel, new Rectangle((rectangleToDraw.X + rectangleToDraw.Width - thicknessOfBorder) * scale,
                rectangleToDraw.Y * scale, thicknessOfBorder * scale, rectangleToDraw.Height * scale), borderColor);

            // Draw bottom line
            spriteBatch.Draw(_pixel, new Rectangle(rectangleToDraw.X * scale, (rectangleToDraw.Y + rectangleToDraw.Height - thicknessOfBorder) * scale,
                rectangleToDraw.Width * scale, thicknessOfBorder * scale), borderColor);

        }
#endif
    }
}
