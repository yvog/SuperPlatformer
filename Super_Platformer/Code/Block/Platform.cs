using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Super_Platformer.Code.Core;
using Super_Platformer.Code.Core.Physics;
using Super_Platformer.Code.World;
using static Super_Platformer.Code.Core.Physics.CollisionTester;

namespace Super_Platformer.Code.Block
{
    /// <summary>
    /// Platform class.
    /// </summary>
    public class Platform : Tile
    {
        /// <summary> Should the platform collide on the left. </summary>
        public bool CollideLeft
        {
            private get;
            set;
        }

        /// <summary> Should the platform collide on the right. </summary>
        public bool CollideRight
        {
            private get;
            set;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="texture"> Texture of this platform.</param>
        /// <param name="source"> Location of the image.</param>
        /// <param name="position"> Location in game.</param>
        /// <param name="width"> Width of the platform.</param>
        /// <param name="height"> Height of the platform.</param>
        /// <param name="collidable"> Collidable state of the platform.</param>
        /// <param name="level"> The level the platform is in.</param>
        public Platform(Texture2D texture, Rectangle source, Vector2 position, int width, int height, bool collidable, Level level) :
            base(texture, source, position, width, height, collidable, level)
        {
            //
        }

        /// <summary>
        /// Returns if the Platform is collidable.
        /// </summary>
        /// <param name="ent"> Entity this object collides with.</param>
        /// <param name="axis"> The axis that is being checked.</param>
        /// <returns>True if the platform is collidable.</returns>
        public override bool IsCollidable(Entity ent, CollisionTester.Axis axis)
        {
            Rectangle fromBounds = ent.PreviousBounds;

            return (
                Collidable && (                                 // Is the platform collidable in general?
                axis == CollisionTester.Axis.Y &&                               // Is the axis being checked Axis.Y?
                (fromBounds.Bottom - Bounds.Top) <= 0) ||       // Does the player comes from above?

                (axis == CollisionTester.Axis.X &&                              // Is the axis being checked Axis.X?
                CollideLeft &&                                  // Should you collide on the Left side
                (fromBounds.Right - Bounds.Left) <= 0 &&        // Does the player comes from left?
                (fromBounds.Bottom - Bounds.Bottom) <= 0) ||    // Does the player comes from above?

                (axis == CollisionTester.Axis.X &&                              // Is the axis being checked Axis.X?
                CollideRight &&                                 // Should you collide on the Right side
                (fromBounds.Left - Bounds.Right) <= 0 &&        // Does the player comes from right?
                (fromBounds.Bottom - Bounds.Bottom) <= 0)       // Does the player comes from above?
            );
        }
    }
}