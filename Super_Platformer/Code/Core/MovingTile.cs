using Microsoft.Xna.Framework;
using Super_Platformer.Code.World;

namespace Super_Platformer.Code.Core
{
    /// <summary>
    /// MovingTile class.
    /// </summary>
    public abstract class MovingTile : MovingEntity
    {
        /// <summary> All the states a moving tile can be in. </summary>
        public enum TileState
        {
            /// <summary> Bump start state. </summary>
            BUMP_START,
            /// <summary> Bumping up state. </summary>
            BUMPING_UP,
            /// <summary> Bumping down state. </summary>
            BUMPING_DOWN,
            /// <summary> Idle state. </summary>
            IDLE
        }

        /// <summary> Current state.</summary>
        public TileState State
        {
            get;
            protected set;
        }

        /// <summary> Speed of bumping.</summary>
        protected float BumpSpeed
        {
            get;
            set;
        }

        /// <summary> Maximum bump distance.</summary>
        protected float BumpDistance
        {
            get;
            set;
        }

        /// <summary> Location where bumping started.</summary>
        protected Vector2 BumpStartPosition
        {
            get;
            set;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="position"> Location in game.</param>
        /// <param name="width"> Width.</param>
        /// <param name="height"> Height.</param>
        /// <param name="level"> The level the Moving Tile is in.</param>
        public MovingTile(Vector2 position, int width, int height, Level level) :
            base(position, width, height, level)
        {
            State = TileState.IDLE;
            BumpDistance = 5f;
            BumpSpeed = 70f;
            TerminalVelocity = new Vector2(70, 70);
            Padding = new Vector2(0, 0);
        }

        /// <summary>
        /// Update function (IMonoUpdatable).
        /// </summary>
        /// <param name="gameTime"> Game time.</param>
        public override void Update(GameTime gameTime)
        {
            switch (State)
            {
                case TileState.BUMP_START:

                    BumpStartPosition = Position;
                    State = TileState.BUMPING_UP;

                    break;
                case TileState.BUMPING_UP:

                    velocity.Y = -BumpSpeed;

                    if (Position.Y <= (BumpStartPosition.Y - BumpDistance))
                    {
                        State = TileState.BUMPING_DOWN;
                    }

                    break;
                case TileState.BUMPING_DOWN:

                    velocity.Y = BumpSpeed;

                    if (Position.Y >= BumpStartPosition.Y)
                    {
                        Position = BumpStartPosition;
                        velocity.Y = 0;
                        State = TileState.IDLE;
                    }

                    break;
                default:

                    velocity.Y = 0;

                    break;
            }

            base.Update(gameTime);
        }

    }
}
