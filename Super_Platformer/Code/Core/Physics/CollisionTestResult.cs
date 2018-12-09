using static Super_Platformer.Code.Core.Physics.CollisionTester;

namespace Super_Platformer.Code.Core.Physics
{
    /// <summary> 
    /// CollisionTestResult holds test results.
    /// </summary>
    public class CollisionTestResult
    {
        /// <summary> Last collider. </summary>
        public Entity Collider
        {
            get;
            private set;
        }

        /// <summary> Last penetration amount. </summary>
        public int Penetration
        {
            get;
            private set;
        }

        /// <summary> Last collided side. </summary>
        public CollisionTester.CollisionSide Side
        {
            get;
            private set;
        }

        /// <summary> Last collided opposite side. </summary>
        public CollisionTester.CollisionSide OppositeSide
        {
            get;
            private set;
        }

        /// <summary>
        /// Save test results.
        /// </summary>
        /// <param name="collider"> The entity an entity collided with.</param>
        /// <param name="penetration"> The depth of the collision.</param>
        /// <param name="side"> The collision side.</param>
        /// <param name="oppositeSide"> The opposite collision side.</param>
        public void Write(Entity collider, int penetration, CollisionTester.CollisionSide side, CollisionTester.CollisionSide oppositeSide)
        {
            Collider = collider;
            Penetration = penetration;
            Side = side;
            OppositeSide = oppositeSide;
        }
    }
}
