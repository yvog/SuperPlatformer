using System;
using Microsoft.Xna.Framework;

namespace Super_Platformer.Code.Core
{
    /// <summary>
    /// Timed event class.
    /// </summary>
    public class TimedEvent : IMonoUpdateable
    {
        /// <summary> Elapsed time. </summary>
        private double _elapsed;
        /// <summary> Time after witch a event should happen. </summary>
        private int _msDelay;
        /// <summary> Callback action. </summary>
        private Action _action;

        /// <summary> Is the timed event running. </summary>
        public bool Running
        {
            get;
            private set;
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="action"> Callback action.</param>
        /// <param name="msDelay"> Delay.</param>
        public TimedEvent(Action action, int msDelay)
        {
            // Set elapsed time to 0.
            _elapsed = 0;
            // Set delay.
            _msDelay = msDelay;
            // Set action.
            _action = action;

            // Default running is false.
            Running = false;
        }

        /// <summary>
        /// Enable the timed event.
        /// </summary>
        public void Enable()
        {
            Running = true;
        }

        /// <summary>
        /// Disable the timed event.
        /// </summary>
        public void Disable()
        {
            Running = false;
        }

        /// <summary>
        /// Reset the timed event.
        /// </summary>
        public void Reset()
        {
            // Set elapsed time to 0.
            _elapsed = 0;
            // Disable the timer.
            Disable();
        }

        /// <summary>
        /// Update function (IMonoUpdatable).
        /// </summary>
        /// <param name="gameTime"> Game time.</param>
        public void Update(GameTime gameTime)
        {
            // Check if the timer is running.
            if (Running)
            {
                // Accumolate the elapsed time.
                _elapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

                // Check if elapsed time is more than the wait delay.
                if (_elapsed >= _msDelay)
                {
                    // Reset the timer.
                    Reset();

                    // Call the action.
                    _action();
                }
            }
        }
    }
}
