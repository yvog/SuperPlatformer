using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Super_Platformer.Code.Core;
using Super_Platformer.Code.Core.Rendering;
using Super_Platformer.Code.Core.Spritesheet;
using Super_Platformer.Code.World;

namespace Super_Platformer.Code.Mob
{
    /// <summary>
    /// BubbleGumEnemy class.
    /// </summary>
    public class BubbleGumEnemy : Enemy
    {
        private enum BubbleGumEnemyAnimation : int
        {
            WALKING = 0,
            DEATH = 1
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="position"> Location in game.</param>
        /// <param name="width"> Width of the BubbleGumEnemy.</param>
        /// <param name="height"> Height of the BubbleGumEnemy.</param>
        /// <param name="content"> The content manager to laod the texture from.</param>
        /// <param name="level"> The level the BubbleGumEnemy is in.</param>
        public BubbleGumEnemy(Vector2 position, int width, int height, ContentManager content, Level level) :
            base(position, width, height, level)
        {
            Sprite = new MonoSprite(content.Load<Texture2D>("Images/Enemy/BubbleGumEnemy"), new Rectangle(0, 16, 16, 16), position, width, height);

            // Set the terminal velocity.
            TerminalVelocity = new Vector2(70, 350);

            // Add walking animation.
            Animations.Add(new Animation(
                id: (int)BubbleGumEnemyAnimation.WALKING,
                baseFrame: Vector2.Zero,
                frameSize: new Vector2(width, height),
                rows: 1,
                columns: 2,
                msPerFrame: 150
            ));

            // Add death animation.
            Animations.Add(new Animation(
                id: (int)BubbleGumEnemyAnimation.DEATH,
                baseFrame: new Vector2(32, 8),
                frameSize: new Vector2(17, 8),
                rows: 1,
                columns: 1,
                msPerFrame: 0
            ));

            // By default play the walking animation.
            Animations.Play((int)BubbleGumEnemyAnimation.WALKING);

            // Set horizontal velocity to the terminal horizontal velocity.
            velocity.X = TerminalVelocity.X;
        }

        /// <summary>
        /// Function gets called on enemy death.
        /// </summary>
        /// <param name="ent"> Entity that killed this enemy.</param>
        public override void OnDeath(Entity ent)
        {
            EnableDeathTimer();

            // Play death animation.
            Animations.Play((int)BubbleGumEnemyAnimation.DEATH);

            // Disable the start of a new animtaion.
            Animations.Lock();

            // Set position & height.
            int newHeight = 8;
            int heightDifference = Height - newHeight;

            Height = newHeight;
            Position = new Vector2(Position.X, Position.Y + heightDifference);

            // Player can walk through death enemy.
            Solid = false;
            Collidable = false;
            Gravity = 0;

            // Make it stop moving.
            velocity.X = 0;
            velocity.Y = 0;

            // Disable movement.
            AllowMovement = false;
        }
    }

}