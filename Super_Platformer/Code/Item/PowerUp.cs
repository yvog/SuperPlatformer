using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Super_Platformer.Code.Core;
using Super_Platformer.Code.Core.Physics;
using Super_Platformer.Code.Mob;
using Super_Platformer.Code.World;
using static Super_Platformer.Code.Core.Physics.CollisionTester;

namespace Super_Platformer.Code.Item
{
    /// <summary>
    /// PowerUp class.
    /// </summary>
    public abstract class PowerUp : MovingEntity
    {
        /// <summary> PowerUp states.</summary>
        protected enum PowerUpState
        {
            /// <summary> Idle state. </summary>
            IDLE,
            /// <summary> Spawning state. </summary>
            SPAWNING,
            /// <summary> Move start state. </summary>
            MOVE_START,
            /// <summary> Droping state. </summary>
            DROPPING,
            /// <summary> Moving state. </summary>
            MOVING
        }

        /// <summary> Speed when spawning.</summary>
        private float _spawnSpeed;

        /// <summary> Start Y-location when spawning.</summary>
        private float _spawnStartPositionY;

        private SoundEffect _spawningSound;

        /// <summary> Current state.</summary>
        protected PowerUpState State
        {
            get;
            set;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="position"> Location in game.</param>
        /// <param name="width"> Width.</param>
        /// <param name="height"> Height.</param>
        /// <param name="level"> Level object.</param>
        /// <param name="content"> ContentManager object.</param>
        public PowerUp(Vector2 position, int width, int height, Level level, ContentManager content) :
            base(position, width, height, level)
        {
            TerminalVelocity = new Vector2(70, 350);

            Solid = false;

            _spawningSound = content.Load<SoundEffect>("Audio/smw_powerupappears");

            _spawnSpeed = 50;
            State = PowerUpState.IDLE;
        }

        /// <summary>
        /// Update function (IMonoUpdatable)
        /// </summary>
        /// <param name="gameTime"> Gametime.</param>
        public override void Update(GameTime gameTime)
        {
            switch (State)
            {
                case PowerUpState.IDLE:
                    //
                    break;
                case PowerUpState.SPAWNING:

                    Gravity = 0;

                    if (Position.Y <= (_spawnStartPositionY - Height))
                    {
                        State = PowerUpState.MOVE_START;
                    }

                    break;
                case PowerUpState.MOVE_START:

                    // Randomnize the move direction after spawning
                    // true -> right
                    // false <- left
                    bool direction = ((new Random()).Next(-1, 1) >= 0);

                    Gravity = Parent.Gravity;

                    // Set velocity
                    velocity.X = TerminalVelocity.X;

                    // Randomnize the move direction after spawning
                    if (!direction)
                    {
                        float invertedVelX = velocity.X * -1;

                        FacingDirection = Facing.LEFT;
                        velocity.X = invertedVelX;
                    }

                    State = PowerUpState.MOVING;

                    Collidable = true;

                    break;
                case PowerUpState.DROPPING:

                    velocity.X = 0;
                    velocity.Y = _spawnSpeed;

                    Gravity = 0;

                    if (Grounded)
                    {
                        State = PowerUpState.MOVING;
                    }

                    Collidable = true;

                    break;
                case PowerUpState.MOVING:

                    velocity.X = TerminalVelocity.X;

                    Gravity = Parent.Gravity;

                    if (FacingDirection == Facing.LEFT)
                    {
                        float invertedVelX = velocity.X * -1;
                        velocity.X = invertedVelX;
                    }

                    break;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Invoked when spawned
        /// </summary>
        public void OnSpawn()
        {
            Parent.Score.IncreaseScore(1000);

            _spawnStartPositionY = Position.Y;
            velocity.Y = -_spawnSpeed;

            if (_spawningSound != null)
            {
                _spawningSound.Play();
            }

            State = PowerUpState.SPAWNING;
        }

        /// <summary>
        /// Invoked when dropped.
        /// </summary>
        public void OnDrop()
        {
            State = PowerUpState.DROPPING;
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
            if (!(ent is Enemy))
            {
                ResolveCollision(ent, penetration, side);

                if (ent.Solid && axis == CollisionTester.Axis.X)
                {
                    TurnAround();
                    State = PowerUpState.MOVING;
                }
            }
        }
    }

}