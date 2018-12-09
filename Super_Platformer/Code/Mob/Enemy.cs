using Microsoft.Xna.Framework;
using Super_Platformer.Code.Core;
using Super_Platformer.Code.Core.Physics;
using Super_Platformer.Code.Item;
using Super_Platformer.Code.World;
using static Super_Platformer.Code.Core.Physics.CollisionTester;

namespace Super_Platformer.Code.Mob
{
    /// <summary>
    /// Enemy class.
    /// </summary>
    public abstract class Enemy : MovingEntity
    {
        /// <summary> Determine if this enemy move. </summary>
        protected bool AllowMovement
        {
            get;
            set;
        }

        /// <summary> Timer destroys enemy after 2 seconds death. </summary>
        private TimedEvent _deathTimer;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="position"> Location in game.</param>
        /// <param name="width"> Width of the enemy.</param>
        /// <param name="height"> Height of the enemy.</param>
        /// <param name="level"> The level the Enemy is in.</param>
        public Enemy(Vector2 position, int width, int height, Level level) :
            base(position, width, height, level)
        {
            // Padding is by default x = 0, y = 1.
            Padding = new Vector2(0, 1);

            // Enemies are by default solid.
            Solid = true;

            // Enemies are by default collidable.
            Collidable = true;

            // Enemies can move by default.
            AllowMovement = true;

            // Set deathtimer.
            _deathTimer = new TimedEvent(Destroy, 2000);
        }

        /// <summary>
        /// Update function (IMonoUpdatable).
        /// </summary>
        /// <param name="gameTime"> Game time.</param>
        public override void Update(GameTime gameTime)
        {
            // Calculate delta time.
            float delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;

            // Update deathtimer.
            _deathTimer.Update(gameTime);

            // Check if enemy can move.
            if (AllowMovement)
            {
                // Set velocity of enemy to terminal velocity.
                velocity.X = TerminalVelocity.X;

                // If enemy is going to the left reverse velocity.
                if (FacingDirection == Facing.LEFT)
                {
                    velocity.X *= -1;
                }
            }
            else
            {
                // If a enemy can't move set velocity to 0.
                velocity.X = 0;
            }

            // Apply gravity.
            velocity.Y += Gravity * delta;

            // Update base.
            base.Update(gameTime);
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
            // Check if collider is player.
            if (ent is Player)
            {
                // Check if player came from top.
                bool fromTop = (side == CollisionTester.CollisionSide.TOP && (fromBounds.Bottom - Bounds.Top) <= 0 && axis == CollisionTester.Axis.Y);

                // Get player.
                Player player = (Player)ent;

                // If player came from top and player is not invulnerable.
                if (fromTop && !player.Invulnerable)
                {
                    // Call enemy on death.
                    OnDeath(player);

                    EnableDeathTimer();

                    // Make the player jump.
                    player.Jump();
                }
                else // Player is not comming from top.
                {
                    // Check if enemy is solid.
                    if (Solid)
                    {
                        // Call in player the on enemy collision function.
                        player.OnEnemyCollision();
                    }
                }
            }
            // If colliding with solid but not with player.
            else if (ent.Solid && axis == CollisionTester.Axis.X)
            {
                // Turn enemy around.
                TurnAround();
            }

            // If enemy is not colliding with power up resolve collision.
            if (!(ent is PowerUp))
            {
                ResolveCollision(ent, penetration, side);
            }
        }

        /// <summary>
        /// Starts the enemey death timer.
        /// </summary>
        protected void EnableDeathTimer()
        {
            // Enable death timer if it is not running.
            if (!_deathTimer.Running)
            {
                _deathTimer.Enable();
            }
        }

        /// <summary>
        /// Function gets called on enemy death.
        /// </summary>
        /// <param name="ent"> Entity that killed this enemy.</param>
        public abstract void OnDeath(Entity ent);
    }
}
