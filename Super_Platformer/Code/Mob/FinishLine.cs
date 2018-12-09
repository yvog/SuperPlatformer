using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Super_Platformer.Code.Core;
using Super_Platformer.Code.Core.Physics;
using Super_Platformer.Code.Core.Rendering;
using Super_Platformer.Code.Core.Spritesheet;
using Super_Platformer.Code.World;
using static Super_Platformer.Code.Core.Physics.CollisionTester;

namespace Super_Platformer.Code.Mob
{
    /// <summary>
    /// FinishLine class.
    /// </summary>
    public class FinishLine : Entity
    {
        private enum FinishState : int
        {
            /// <summary> Open State </summary>
            OPEN = 0,
            /// <summary> Close State </summary>
            CLOSED = 1
        }

        private FinishState _state;

        /// <summary> Event on closed.</summary>
        public event EventHandler<EventArgs> OnClosed;

        /// <summary> Event Handler on closed.</summary>
        public delegate void OnClosedHandler(object sender, EventArgs e);

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="position"> Location in game.</param>
        /// <param name="level"> Level object.</param>
        /// <param name="content"> Content manager object.</param>
        public FinishLine(Vector2 position, Level level, ContentManager content) :
            base(position, 32, 64, level)
        {
            Sprite = new MonoSprite(content.Load<Texture2D>("Images/Finish"), new Rectangle(0, 0, Width, Height), position, Width, Height);
            _state = FinishState.OPEN;

            // Add finishing animation
            Animations.Add(new Animation(
                id: (int)FinishState.OPEN,
                baseFrame: Vector2.Zero,
                frameSize: new Vector2(Width, Height),
                rows: 1,
                columns: 4,
                msPerFrame: 150
            ));

            Animations.Add(new Animation(
                id: (int)FinishState.CLOSED,
                baseFrame: new Vector2(0, 64),
                frameSize: new Vector2(Width, Height),
                rows: 1,
                columns: 4,
                msPerFrame: 150
            ));

            Padding = new Vector2(8, 3);
            Solid = false;

            Animations.Play((int)_state);
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
            if (ent is Player && _state == FinishState.OPEN)
            {
                _state = FinishState.CLOSED;

                Animations.Play((int)_state);
                OnClosed?.Invoke(this, new EventArgs());
            }
        }
    }
}
