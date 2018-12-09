using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Super_Platformer.Code.Core;
using Super_Platformer.Code.Core.Physics;
using Super_Platformer.Code.Core.Rendering;
using Super_Platformer.Code.Core.Spritesheet;
using Super_Platformer.Code.World;
using static Super_Platformer.Code.Core.Physics.CollisionTester;

namespace Super_Platformer.Code.Mob
{
    /// <summary>
    /// EggEnemy enemy class.
    /// </summary>
    public class EggEnemy : Enemy
    {
        private enum EggEnemyAnimation : int
        {
            IDLE = 0,
            MOVING = 1
        }

        private SoundEffect _startMovingSound;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="position"> Location in game.</param>
        /// <param name="width"> Width of the EggEnemy.</param>
        /// <param name="height"> Height of the EggEnemy.</param>
        /// <param name="content"> Content manager of the game.</param>
        /// <param name="level"> The level the EggEnemy is in.</param>
        public EggEnemy(Vector2 position, int width, int height, ContentManager content, Level level) :
            base(position, width, height, level)
        {
            Sprite = new MonoSprite(content.Load<Texture2D>("Images/Enemy/EggEnemy"), new Rectangle(32, 0, width, height), position, width, height);

            // EggEnemy is invulnerable.
            Invulnerable = true;

            // By default shell lays still.
            AllowMovement = false;

            // Set the terminal velocity of the shell.
            TerminalVelocity = new Vector2(110f, 150f);

            _startMovingSound = content.Load<SoundEffect>("Audio/smw_kick");

            // Add idle animation.
            Animations.Add(new Animation(
                id: (int)EggEnemyAnimation.IDLE,
                baseFrame: Vector2.Zero,
                frameSize: new Vector2(width, height),
                rows: 1,
                columns: 1,
                msPerFrame: 50
            ));

            // Add moving animation.
            Animations.Add(new Animation(
                id: (int)EggEnemyAnimation.MOVING,
                baseFrame: Vector2.Zero,
                frameSize: new Vector2(width, height),
                rows: 1,
                columns: 4,
                msPerFrame: 85
            ));

            // Play the idle animation.
            Animations.Play((int)EggEnemyAnimation.IDLE);
        }

        /// <summary>
        /// Function gets called on enemy death.
        /// </summary>
        /// <param name="ent"> Entity that killed this enemy.</param>

        public override void OnDeath(Entity ent)
        {
            // This one is really, really strong
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
                // Check if player comes from top.
                bool fromTop = (side == CollisionTester.CollisionSide.TOP && (fromBounds.Bottom - Bounds.Top) <= 0 && axis == CollisionTester.Axis.Y);

                // Get player.
                Player player = (Player)ent;

                // If player comes top and player is not invulnerable.
                if (fromTop && !player.Invulnerable)
                {
                    // If shell is moving make it idle, if it is idle make it moving.
                    AllowMovement = !AllowMovement;

                    // Start in opposite direction where it came from.
                    TurnAround();

                    // Play sound.
                    if (_startMovingSound != null)
                    {
                        _startMovingSound.Play();
                    }

                    // Make the palyer jump.
                    player.Jump();
                }
                // If player is not from top and shell is moving and player is not invurnelable.
                else if (Math.Abs(velocity.X) > 0 && !player.Invulnerable)
                {
                    // Call in player the on enemy collision function
                    ((Player)ent).OnEnemyCollision();
                }

                // If the player walks to it from the side and the shell is not moving.
                if ((int)velocity.X == 0 && axis == CollisionTester.Axis.X)
                {
                    // Make the shell move.
                    AllowMovement = true;

                    // Play sound.
                    if (_startMovingSound != null)
                    {
                        _startMovingSound.Play();
                    }

                    // Make shell move away from player.
                    if (player.Position.X < Position.X)
                    {
                        // If player is to the left make shell go to right.
                        FacingDirection = Facing.RIGHT;
                    }
                    else
                    {
                        // If player is to the right make shell go to left.
                        FacingDirection = Facing.LEFT;
                    }
                }
            }
            else
            {
                // Resolve collision.
                ResolveCollision(ent, penetration, side);
            }

            // Check if collision axis is X axis.
            if (axis == CollisionTester.Axis.X && ent.Solid)
            {
                // If ent is enemy and shell is moving.
                if (ent is Enemy && !(ent is EggEnemy) && AllowMovement)
                {
                    // Get enemy.
                    Enemy enemy = (Enemy)ent;

                    // If enemy is not invulnerable.
                    if (!enemy.Invulnerable)
                    {
                        // Kill enemy.
                        enemy.OnDeath(this);
                    }
                    else
                    {
                        TurnAround();
                    }
                }
                // If entity is not player turn around.
                else if (!(ent is Player))
                {
                    TurnAround();
                }
            }

            // Check if shell is moving.
            if (AllowMovement)
            {
                // Play moving animation.
                Animations.Play((int)EggEnemyAnimation.MOVING);
            }
            else
            {
                // Play idle animation.
                Animations.Play((int)EggEnemyAnimation.IDLE);
            }
        }
    }
}
