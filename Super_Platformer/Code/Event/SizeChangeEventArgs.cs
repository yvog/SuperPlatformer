using System;
using Super_Platformer.Code.Mob;

namespace Super_Platformer.Code.Event
{
    /// <summary>
    /// SizeChangeEventArgs class.
    /// </summary>
    public class SizeChangeEventArgs : EventArgs
    {
        /// <summary>
        /// The new player
        /// 
        ///  state.
        /// </summary>
        public Player.SizeState State
        {
            get;
            private set;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="state"> New player state.</param>
        public SizeChangeEventArgs(Player.SizeState state)
        {
            State = state;
        }
    }
}