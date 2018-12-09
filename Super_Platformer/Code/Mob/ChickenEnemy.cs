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
    /// ChickenEnemy class.
    /// </summary>
    public class ChickenEnemy : Enemy
    {
        private enum ChickenEnemyAnimation : int
        {
            WALKING = 0,
            TURNING = 1,
            DEATH = 2
        }

        /// <summary> Content.</summary>
        private ContentManager _content;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="position"> Location in game.</param>
        /// <param name="width"> Width of the ChickenEnemy.</param>
        /// <param name="height"> Height of the ChickenEnemy.</param>
        /// <param name="content"> The content manager.</param>
        /// <param name="level"> The level the ChickenEnemy is in.</param>
        public ChickenEnemy(Vector2 position, int width, int height, ContentManager content, Level level) :
            base(position, width, height, level)
        {
            Padding = new Vector2(1, 10);
            _content = content;

            Sprite = new MonoSprite(content.Load<Texture2D>("Images/Enemy/ChickenEnemy"), new Rectangle(0, 27, width, height), position, width, height);

            // Set terminal velocity.
            TerminalVelocity = new Vector2(70, 350);

            // Add walking animation.
            Animations.Add(new Animation(
                id: (int)ChickenEnemyAnimation.WALKING,
                baseFrame: new Vector2(16, 0),
                frameSize: new Vector2(width, height),
                rows: 1,
                columns: 2,
                msPerFrame: 150
            ));

            // Add turning animation.
            Animations.Add(new Animation(
                id: (int)ChickenEnemyAnimation.TURNING,
                baseFrame: Vector2.Zero,
                frameSize: new Vector2(width, height),
                rows: 1,
                columns: 1,
                msPerFrame: 175
            ));

            // Add death animation.
            Animations.Add(new Animation(
               id: (int)ChickenEnemyAnimation.DEATH,
               baseFrame: new Vector2(48, 0),
               frameSize: new Vector2(16, 15),
               rows: 1,
               columns: 1,
               msPerFrame: 0
           ));

            // By default play the walking animation.
            Animations.Play((int)ChickenEnemyAnimation.WALKING);

            // Set horizontal velocity to the terminal horizontal velocity.
            velocity.X = TerminalVelocity.X;
        }

        /// <summary>
        /// Function gets called on enemy death.
        /// </summary>
        /// <param name="ent"> Entity that killed this enemy.</param>
        public override void OnDeath(Entity ent)
        {
            // Set not solid and disable collisions.
            Solid = false;
            Collidable = false;

            // Start death timer.
            EnableDeathTimer();

            // Add shell on position of death.
            if (!(ent is EggEnemy))
            {
                Parent.AddEntity(new EggEnemy(new Vector2(Position.X, Position.Y + 11), 16, 16, _content, Parent));
            }

            // Play death animation.
            Animations.Play((int)ChickenEnemyAnimation.DEATH);

            // Disable the start of a new animtaion.
            Animations.Lock();

            // Call jump.
            Jump();

            // Set width and height to death sprite size.
            Height = 16;
            Width = 15;
        }

        /// <summary>
        /// Gets called when a entity should turn.
        /// </summary>
        public override void TurnAround()
        {
            // Play the turning animation once.
            Animations.PlayOnce((int)ChickenEnemyAnimation.TURNING);

            // Call base turn around.
            base.TurnAround();
        }
    }

}