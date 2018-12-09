using System;
using Microsoft.Xna.Framework;
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
    /// PlungerEnemy class.
    /// </summary>
    public class PlungerEnemy : Enemy
    {
        private enum PlungerEnemyAnimation : int
        {
            BITING = 0
        }

        /// <summary> Store the spawn position. </summary>
        private Vector2 _spawnPosition;

        /// <summary> Timer for changing directions. </summary>
        private TimedEvent _turnTimer;

        /// <summary> Original height. </summary>
        private int _startHeight;

        /// <summary> Directions the plant can move in. </summary>
        private enum Direction : int
        {
            /// <summary> Moving Down. </summary>
            Down = 0,
            /// <summary> Moving Up. </summary>
            Up = 1
        }

        /// <summary> Store the current direction. </summary>
        private Direction _direction;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="position"> Position in the level.</param>
        /// <param name="width"> Width of the PlungerEnemy.</param>
        /// <param name="height"> Height of the PlungerEnemy.</param>
        /// <param name="content"> Content manager of the game.</param>
        /// <param name="level"> The level the PlungerEnemy is in.</param>
        public PlungerEnemy(Vector2 position, int width, int height, ContentManager content, Level level) : base(position, width, height, level)
        {
            Sprite = new MonoSprite(content.Load<Texture2D>("Images/Enemy/PlungerEnemy"), new Rectangle(0, 0, width, height), position, width, height);

            // Remove gravity.
            Gravity = 0;

            // PlungerEnemy is not solid.
            Solid = false;

            // PlungerEnemy shouldn't check for collisions.
            CheckCollisions = false;

            // Set default direction.
            _direction = Direction.Down;

            // Enemy doesnt walk.
            AllowMovement = false;

            // Set spawn position.
            _spawnPosition = position;

            // Set default height.
            _startHeight = height;

            // Set some padding
            Padding = new Vector2(3, 3);

            // This enemy is invunreble.
            Invulnerable = true;

            // Add biting animation.
            Animations.Add(new Animation(
                id: (int)PlungerEnemyAnimation.BITING,
                baseFrame: Vector2.Zero,
                frameSize: new Vector2(width, height),
                rows: 1,
                columns: 2,
                msPerFrame: 150
            ));

            // Start biting animation.
            Animations.Play((int)PlungerEnemyAnimation.BITING);

            // Create timer.
            _turnTimer = new TimedEvent(Turn, 2250);

            // Start timer.
            _turnTimer.Enable();
        }

        /// <summary>
        /// Set direction in opisite direction.
        /// </summary>
        private void Turn()
        {
            // Switch direction.
            _direction = (Direction)(((int)_direction + 1) % 2);

            // Restart timer. 
            _turnTimer.Enable();
        }

        /// <summary>
        /// Update method.
        /// </summary>
        /// <param name="gameTime"> Game time.</param>
        public override void Update(GameTime gameTime)
        {
            float delta = ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000);

            // Update timer.
            _turnTimer.Update(gameTime);

            // Vector2 to edit.
            Vector2 position = Position;

            // Check state.
            switch (_direction)
            {
                // going up state.
                case Direction.Up:

                    // Check if position is below end position.
                    if (position.Y > _spawnPosition.Y)
                    {
                        // Move gradualy to end position.
                        position.Y -= _startHeight * delta;
                    }

                    // Clamp to end position.
                    position.Y = Math.Max(position.Y, _spawnPosition.Y);

                    break;

                // Going down state
                case Direction.Down:

                    // Check if position is above end position.
                    if (position.Y < _spawnPosition.Y + _startHeight)
                    {
                        // Move gradualy to end position.
                        position.Y += _startHeight * delta;
                    }

                    position.Y = Math.Min(position.Y, _spawnPosition.Y + _startHeight);

                    break;
            }

            // Calculate visible part of sprite for drawing and hitbox.
            Height = (int)Math.Round(_startHeight - (position.Y - _spawnPosition.Y));

            // Set position to new position.
            Position = position;

            // Update base.
            base.Update(gameTime);
        }


        /// <summary>
        /// Method gets called when enemy dies
        /// </summary>
        /// <param name="ent"> Entity that killed this enemy.</param>
        public override void OnDeath(Entity ent)
        {
            // Only a shell can kill this one.
            if (ent is EggEnemy)
            {
                Destroy();
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
            // Dont allow player from side to kill it.
            if (ent is Player)
            {
                ((Player)ent).OnEnemyCollision();
            }
        }
    }
}
