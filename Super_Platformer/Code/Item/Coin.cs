using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Super_Platformer.Code.Core;
using Super_Platformer.Code.Core.Physics;
using Super_Platformer.Code.Core.Rendering;
using Super_Platformer.Code.Core.Spritesheet;
using Super_Platformer.Code.Mob;
using Super_Platformer.Code.Score;
using Super_Platformer.Code.World;
using static Super_Platformer.Code.Core.Physics.CollisionTester;

namespace Super_Platformer.Code.Item
{
    /// <summary>
    /// Coin class.
    /// </summary>
    public class Coin : MovingEntity
    {
        private enum CoinAnimation : int
        {
            ROTATING = 0,
            SPAWNING = 1
        }

        /// <summary> Posible states of the coin. </summary>
        private enum CoinState
        {
            /// <summary> Idle state. </summary>
            IDLE,
            /// <summary> Spawning state. </summary>
            SPAWNING
        }

        /// <summary> Keep track of current state </summary>
        private CoinState _state;

        /// <summary> Start coord of coin on spawn </summary>
        private float _spawnStartY;

        /// <summary> Max movement up the coin gets on spawn </summary>
        private float _maxSpawnDistance;

        /// <summary> Speed the coin gets on spawn. </summary>
        private float _spawnSpeed;

        /// <summary> The score collector.</summary>
        private ScoreCollector _score;

        /// <summary> Disappear sound.</summary>
        private SoundEffect _disappearSound;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="position"> Location in game.</param>
        /// <param name="content"> The content manager to get the texture from.</param>
        /// <param name="level"> The level the coin is in.</param>
        public Coin(Vector2 position, ContentManager content, Level level) :
            base(position, 12, 16, level)
        {
            _score = level.Score;

            Sprite = new MonoSprite(content.Load<Texture2D>("Images/Item/Coin"), new Rectangle(0, 0, 12, 16), position, 12, 16);

            // Make it float.
            Gravity = 0;

            // Coin is not solid.
            Solid = false;

            // Coin does't check for collisions on its own.
            CheckCollisions = false;

            // Set max spawn distance to 20.
            _maxSpawnDistance = 20;

            // Set max spawn speed to 100.
            _spawnSpeed = 100;

            _disappearSound = content.Load<SoundEffect>("Audio/smw_coin");

            // Add rotating animation.
            Animations.Add(new Animation(
                id: (int)CoinAnimation.ROTATING,
                baseFrame: Vector2.Zero,
                frameSize: new Vector2(12, 16),
                rows: 1,
                columns: 4,
                msPerFrame: 125
            ));

            // Add spawning animation.
            Animations.Add(new Animation(
               id: (int)CoinAnimation.SPAWNING,
               baseFrame: Vector2.Zero,
               frameSize: new Vector2(12, 16),
               rows: 1,
               columns: 1,
               msPerFrame: 0
           ));

            // On default start the rotating animation.
            Animations.Play((int)CoinAnimation.ROTATING);
        }

        /// <summary>
        /// Update function (IMonoUpdatable).
        /// </summary>
        /// <param name="gameTime"> Game time.</param>
        public override void Update(GameTime gameTime)
        {
            // Check the state of the coin.
            switch (_state)
            {
                case CoinState.SPAWNING:
                    // Play spawning animation.
                    Animations.Play((int)CoinAnimation.SPAWNING);

                    if (Math.Abs(Position.Y - _spawnStartY) > _maxSpawnDistance)
                    {
                        // Set state to idle.
                        _state = CoinState.IDLE;

                        // Set vertical velocity to the spawnspeed.
                        velocity.Y = -_spawnSpeed;

                        // Set coin to be destroyed.
                        Destroy();
                    }

                    break;
                case CoinState.IDLE:

                    // Play rotation animation.
                    Animations.Play((int)CoinAnimation.ROTATING);

                    break;
            }

            // Update the base class.
            base.Update(gameTime);
        }

        /// <summary>
        /// Actions that sould happen when coin gets out of coinblock.
        /// </summary>
        public void OnFree()
        {
            // Store spawn location.
            _spawnStartY = Position.Y;

            // Set vertical velocity.
            velocity.Y = -_spawnSpeed;

            // Disable collisions.
            Collidable = false;

            // Set terminal velocity.
            TerminalVelocity = new Vector2(0, _spawnSpeed);

            // Set state to spawning.
            _state = CoinState.SPAWNING;

            // Pickup coin.
            OnPickUp();
        }

        /// <summary>
        /// Actions that sould happen when coin gets picked up.
        /// </summary>
        private void OnPickUp()
        {
            // Increase coin counter.
            _score.IncreaseCoins(1);

            // Add score to score counter.
            _score.IncreaseScore(10);

            // Play the disappear sound
            if (_disappearSound != null)
            {
                _disappearSound.Play();
            }
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
            // If player clollides with coin, pick it up and destroy the coin.
            if (ent is Player)
            {
                OnPickUp();

                Destroy();
            }
        }
    }
}