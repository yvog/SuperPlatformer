using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Super_Platformer.Code.Core;
using Super_Platformer.Code.Core.Physics;
using Super_Platformer.Code.Core.Rendering;
using Super_Platformer.Code.Core.Spritesheet;
using Super_Platformer.Code.Item;
using Super_Platformer.Code.Mob;
using Super_Platformer.Code.World;
using static Super_Platformer.Code.Core.Physics.CollisionTester;

namespace Super_Platformer.Code.Block
{
    /// <summary>
    /// GiftBlock class.
    /// </summary>
    public class GiftBlock : MovingTile
    {
        private enum GiftBlockAnimation : int
        {
            ROTATING = 0,
            WOOD = 1
        }

        /// <summary> The item in this mystery block.</summary>
        private PowerUp _item;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="position"> Location in game.</param>
        /// <param name="width"> Width.</param>
        /// <param name="height"> Height.</param>
        /// <param name="item"> Mystery item.</param>
        /// <param name="content"> The content manager.</param>
        /// <param name="level"> The level the giftblock is in.</param>
        public GiftBlock(Vector2 position, int width, int height, PowerUp item, ContentManager content, Level level) :
            base(position, width, height, level)
        {
            Gravity = 0;

            Sprite = new MonoSprite(content.Load<Texture2D>("Images/Tile/Blocks"), new Rectangle(0, 0, 16, 16), position, width, height);

            // The block should not check its own collisions.
            CheckCollisions = false;

            // Add rotating animation.
            Animations.Add(new Animation(
                id: (int)GiftBlockAnimation.ROTATING,
                baseFrame: Vector2.Zero,
                frameSize: new Vector2(width, height),
                rows: 1,
                columns: 4,
                msPerFrame: 150
            ));

            // Add wood animation.
            Animations.Add(new Animation(
                id: (int)GiftBlockAnimation.WOOD,
                baseFrame: new Vector2(64, 0),
                frameSize: new Vector2(width, height),
                rows: 1,
                columns: 1,
                msPerFrame: 50
            ));

            // By default start rotating animation.
            Animations.Play((int)GiftBlockAnimation.ROTATING);

            _item = item;
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
            // Check if the entity colliding is player and came from the bottom.
            if (ent is Player && side == CollisionTester.CollisionSide.BOTTOM && axis == CollisionTester.Axis.Y)
            {
                // Check if there is a powerup inside.
                if (_item != null)
                {
                    // Get the powerup.
                    PowerUp item = _item;

                    // Set the position of the powerup.
                    item.Position = Position;

                    // Call the spawn of the powerup.
                    item.OnSpawn();

                    // Add the powerup to the level.
                    Parent.AddEntity(item);

                    // Play wood animation.
                    Animations.Play((int)GiftBlockAnimation.WOOD);

                    // Remove item.
                    _item = null;
                }
            }
        }

    }
}

