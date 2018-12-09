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
    /// FlowerEnemy enemy class.
    /// </summary>
    public class FlowerEnemy : Enemy
    {
        private enum FlowerEnemyAnimation : int
        {
            BITING = 0
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="position"> Location in game.</param>
        /// <param name="width"> Width of the FlowerEnemy.</param>
        /// <param name="height"> Height of the FlowerEnemy.</param>
        /// <param name="content"> The content manager.</param>
        /// <param name="level"> The level the FlowerEnemy is in.</param>
        public FlowerEnemy(Vector2 position, int width, int height, ContentManager content, Level level) :
            base(position, width, height, level)
        {
            Sprite = new MonoSprite(content.Load<Texture2D>("Images/Enemy/FlowerEnemy"), new Rectangle(0, 16, width, height), position, width, height);

            // This enemy is invulnerable.
            Invulnerable = true;

            // Add biting animation.
            Animations.Add(new Animation(
                id: (int)FlowerEnemyAnimation.BITING,
                baseFrame: Vector2.Zero,
                frameSize: new Vector2(width, height),
                rows: 1,
                columns: 2,
                msPerFrame: 150
            ));

            // Start biting animation.
            Animations.Play((int)FlowerEnemyAnimation.BITING);
        }

        /// <summary>
        /// Update function (IMonoUpdatable).
        /// </summary>
        /// <param name="gameTime"> Game time.</param>
        public override void Update(GameTime gameTime)
        {
            // Update animation.
            UpdateAnimation(gameTime);
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
                // Call on enemy collision function of player.
                ((Player)ent).OnEnemyCollision();
            }
        }

        /// <summary>
        /// Function gets called on enemy death.
        /// </summary>
        /// <param name="ent"> Entity that killed this enemy.</param>
        public override void OnDeath(Entity ent)
        {
            // This one is really, really strong.
        }
    }

}