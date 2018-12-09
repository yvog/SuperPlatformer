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
    /// ValueBlock class.
    /// </summary>
    public class ValueBlock : MovingTile
    {
        private enum ValueBlockAnimations : int
        {
            SOLID = 0,
            WOOD = 1
        }

        /// <summary> Amount of coins. </summary>
        private int _coins;

        /// <summary> The content manager. </summary>
        private ContentManager _content;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="position"> Location in game.</param>
        /// <param name="width"> Width.</param>
        /// <param name="height"> Height.</param>
        /// <param name="totalCoins"> Total amount of coins.</param>
        /// <param name="level"> The level this block belongs to.</param>
        /// <param name="content"> The content manager.</param>
        public ValueBlock(Vector2 position, int width, int height, int totalCoins, ContentManager content, Level level) :
            base(position, width, height, level)
        {
            _content = content;

            // Let the block float
            Gravity = 0;

            // Texture
            Sprite = new MonoSprite(content.Load<Texture2D>("Images/Tile/Blocks"), new Rectangle(0, 16, 16, 16), position, width, height);

            // Set amount of coins.
            _coins = totalCoins;

            // Coin does't check for collisions on its own.
            CheckCollisions = false;

            // Add solid animation.
            Animations.Add(new Animation(
                id: (int)ValueBlockAnimations.SOLID,
                baseFrame: new Vector2(0, 16),
                frameSize: new Vector2(width, height),
                rows: 1,
                columns: 1,
                msPerFrame: 125
            ));

            // Add wood animation.
            Animations.Add(new Animation(
                id: (int)ValueBlockAnimations.WOOD,
                baseFrame: new Vector2(64, 0),
                frameSize: new Vector2(width, height),
                rows: 1,
                columns: 1,
                msPerFrame: 50
            ));

            // By default play solid animation.
            Animations.Play((int)ValueBlockAnimations.SOLID);
        }

        /// <summary>
        /// Update function (IMonoUpdatable).
        /// </summary>
        /// <param name="gameTime"> Game time.</param>
        public override void Update(GameTime gameTime)
        {
            // If no more coins change to wood texture.
            if (_coins <= 0)
            {
                Animations.Play((int)ValueBlockAnimations.WOOD);
            }

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
            // Check if collision is a player and came from the bottom.
            if (ent is Player && side == CollisionTester.CollisionSide.BOTTOM && axis == CollisionTester.Axis.Y)
            {
                // Check if there are any coins left.
                if (_coins > 0)
                {
                    // Remove a coin.
                    _coins--;

                    int halfHeight = (Height / 2);

                    // Create a new coin.
                    Coin coin = new Coin(new Vector2(Position.X + 2, Position.Y - halfHeight), _content, Parent);

                    // Coin should perform on free action.
                    coin.OnFree();

                    // Add coin to the level.
                    Parent.AddEntity(coin);

                    // Set the state to BUMP_START.
                    State = TileState.BUMP_START;
                }
            }
        }

    }
}



