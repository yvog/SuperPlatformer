using System;
using System.Linq;

namespace Super_Platformer.Code.Core.Physics
{
    /// <summary> 
    /// CollisionTester executes collision tests.
    /// </summary>
    public class CollisionTester
    {
        /// <summary>
        /// The four sides of a rectangle
        /// </summary>
        public enum CollisionSide : int
        {
            /// <summary> Top Side. </summary>
            TOP = 0,
            /// <summary> Right Side. </summary>
            RIGHT = 1,
            /// <summary> Bottom Side. </summary>
            BOTTOM = 2,
            /// <summary> Left Side. </summary>
            LEFT = 3
        }

        /// <summary>
        /// 2D matrix axis
        /// </summary>
        public enum Axis
        {
            /// <summary> Horizontal Axis. </summary>
            X,
            /// <summary> Vertical Axis. </summary>
            Y
        }

        /// <summary>
        /// Latest test result
        /// </summary>
        public CollisionTestResult Result
        {
            get;
            private set;
        }

        /// <summary>
        /// Penetration amount cache
        /// </summary>
        private int[] _penetrations;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CollisionTester()
        {
            Result = new CollisionTestResult();
            _penetrations = new int[4];
        }

        /// <summary>
        /// Test a possible collision between two entities.
        /// </summary>
        /// <param name="ent"> Entity to check collisions for.</param>
        /// <param name="candidate"> List of possible colliders.</param>
        /// <returns>Returns whether the given entity collided or not</returns>
        public bool TestAABB(Entity ent, Entity candidate)
        {
            // Does the entity intersect this candidate?
            if (ent.Bounds.Intersects(candidate.Bounds))
            {
                int penetration;
                CollisionSide side;
                CollisionSide oppositeSide;

                // Calculate penetration per side
                _penetrations[(int)CollisionSide.TOP] = Math.Abs(ent.Bounds.Top - candidate.Bounds.Bottom);
                _penetrations[(int)CollisionSide.RIGHT] = Math.Abs(candidate.Bounds.Left - ent.Bounds.Right);
                _penetrations[(int)CollisionSide.BOTTOM] = Math.Abs(ent.Bounds.Bottom - candidate.Bounds.Top);
                _penetrations[(int)CollisionSide.LEFT] = Math.Abs(ent.Bounds.Left - candidate.Bounds.Right);

                // The minimum value of the above calculated values is the penetrating one
                penetration = _penetrations.Min();

                // Which side was it?
                side = (CollisionSide)Array.IndexOf(_penetrations, penetration);
                oppositeSide = InvertCollisionSide(side);

                // Save our test result
                Result.Write(candidate, penetration, side, oppositeSide);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Inverts the given collision side
        /// </summary>
        /// <param name="side"> The collision side.</param>
        /// <returns>Returns the opposite side of the given side</returns>
        private CollisionSide InvertCollisionSide(CollisionSide side)
        {
            int inversion = ((int)side + 2) % 4;

            return (CollisionSide)inversion;
        }

    }
}
