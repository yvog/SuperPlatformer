using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Super_Platformer.Code.Core;
using Super_Platformer.Code.Core.Rendering;
using Super_Platformer.Code.World;

namespace Super_Platformer.Code.Block
{
    /// <summary>
    /// Tile class.
    /// </summary>
    public class Tile : Entity
    {
        /// <summary> Corner types. </summary>
        public enum Corner : int
        {
            /// <summary> No corner. </summary>
            NONE = 0,
            /// <summary> Left corner. </summary>
            LEFT = 1,
            /// <summary> Right corner. </summary>
            RIGHT = 2
        }

        /// <summary> Corner type. </summary>
        public Corner CornerSide
        {
            get;
            set;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="texture"> Texture of this tile.</param>
        /// <param name="source"> Location of the image.</param>
        /// <param name="position"> Location in game.</param>
        /// <param name="width"> Width of the tile.</param>
        /// <param name="height"> Height of the tile.</param>
        /// <param name="collidable"> Collidable state of the tile.</param>
        /// <param name="level"> The level the Tile is in.</param>
        public Tile(Texture2D texture, Rectangle source, Vector2 position, int width, int height, bool collidable, Level level) :
            base(position, width, height, level)
        {
            // Texture of this tile
            Sprite = new MonoSprite(texture, source, position, width, height);

            // Default cornerside is none.
            CornerSide = Corner.NONE;

            // Tiles should not check for collision on their own.
            CheckCollisions = false;

            Collidable = collidable;
        }
    }
}

