using Microsoft.Xna.Framework;

namespace Super_Platformer.Code.Core.Spritesheet
{
    /// <summary>
    /// Animation is used to store information about a animation.
    /// </summary>
    public class Animation
    {
        /// <summary> Id of the animation. </summary>
        public int Id
        {
            get;
            private set;
        }

        /// <summary> Coords of the first frame (top left). </summary>
        public Vector2 BaseFrame
        {
            get;
            private set;
        }

        /// <summary> Size of the frames. </summary>
        public Vector2 FrameSize
        {
            get;
            private set;
        }

        /// <summary> Number of rows in the animation. </summary>
        public int Rows
        {
            get;
            private set;
        }

        /// <summary> Number of columns in the animation. </summary>
        public int Columns
        {
            get;
            private set;
        }

        /// <summary> Number of milliseconds per frame. </summary>
        public int MsPerFrame
        {
            get;
            private set;
        }

        /// <summary>
        /// Animation Data Default constructor
        /// </summary>
        /// <param name="id"> The name of the animation.</param>
        /// <param name="baseFrame"> The cords (topleft) of the first frame.</param>
        /// <param name="frameSize"> The size of the frame.</param>
        /// <param name="rows"> Number of rows in the animation.</param>
        /// <param name="columns"> Number of columns in the animation.</param>
        /// <param name="msPerFrame"> Number of milliseconds per frame.</param>
        public Animation(int id, Vector2 baseFrame, Vector2 frameSize, int rows, int columns, int msPerFrame)
        {
            Id = id;
            BaseFrame = baseFrame;
            FrameSize = frameSize;
            Rows = rows;
            Columns = columns;
            MsPerFrame = msPerFrame;
        }
    }
}