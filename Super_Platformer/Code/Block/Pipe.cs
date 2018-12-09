using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Super_Platformer.Code.Mob;
using Super_Platformer.Code.World;

namespace Super_Platformer.Code.Block
{
    /// <summary>
    /// Pipe block, may contain a plant.
    /// </summary>
    public class Pipe : Tile
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="texture"> Texture of the pipe.</param>
        /// <param name="source"> Location of the image.</param>
        /// <param name="position"> Position in the level</param>
        /// <param name="level"> The level the pipe is in.</param>
        public Pipe(Texture2D texture, Rectangle source, Vector2 position, Level level) :
            base(texture, source, position, 16, 16, true, level)
        {
            //
        }

        /// <summary>
        /// Constructor to use when pipe with plant.
        /// </summary>
        /// <param name="texture"> Texture of the pipe.</param>
        /// <param name="source"> Location of the image.</param>
        /// <param name="position"> Position in the level</param>
        /// <param name="level"> The level the pipe is in.</param>
        /// <param name="content"> The content loader of the game.</param>
        public Pipe(Texture2D texture, Rectangle source, Vector2 position, Level level, ContentManager content) :
            base(texture, source, position, 16, 16, true, level)
        {
            PlungerEnemy plungerEnemy = new PlungerEnemy(new Vector2(position.X, position.Y - 20), 16, 21, content, level);
            level.AddEntity(plungerEnemy);
        }
    }
}
