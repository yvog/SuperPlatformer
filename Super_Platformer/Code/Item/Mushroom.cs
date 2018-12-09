using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Super_Platformer.Code.Core;
using Super_Platformer.Code.Core.Physics;
using Super_Platformer.Code.Core.Rendering;
using Super_Platformer.Code.Mob;
using Super_Platformer.Code.World;
using static Super_Platformer.Code.Core.Physics.CollisionTester;

namespace Super_Platformer.Code.Item
{
    /// <summary>
    /// Grow mushroom.
    /// </summary>
    public class Mushroom : PowerUp
    {
        /// <summary> The content manager.</summary>
        private ContentManager _content;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="position"> Location in game.</param>
        /// <param name="width"> Width of the mushroom.</param>
        /// <param name="height"> Height of the mushroom.</param>
        /// <param name="content"> The content manager to get the texutre from.</param>
        /// <param name="level"> The level the mushroom is in.</param>
        public Mushroom(Vector2 position, int width, int height, ContentManager content, Level level) :
            base(position, width, height, level, content)
        {
            _content = content;

            Sprite = new MonoSprite(content.Load<Texture2D>("Images/Item/Mushroom"), new Rectangle(0, 0, 16, 16), position, width, height);

            Collidable = false;
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
            // Check if the collider is a player.
            if (ent is Player)
            {
                // Get the player.
                Player _ent = (Player)ent;

                // Disable collision for this mushroom.
                Collidable = false;
                // Make mushroom none solid.
                Solid = false;

                // Add score.
                Parent.Score.IncreaseScore(1000);

                // If player is small make the player grow.
                if (_ent.PowerState == Player.SizeState.SMALL)
                {
                    _ent.Grow();
                }
                // If the player is big.
                else
                {
                    // Add the mushroom to the HUD.
                    Parent.Display.AddPowerUp(new Mushroom(Vector2.Zero, Width, Height, _content, Parent));
                }

                Destroy();
            }

            // Call base collision.
            base.OnCollision(ent, penetration, side, axis, fromBounds);
        }
    }
}

