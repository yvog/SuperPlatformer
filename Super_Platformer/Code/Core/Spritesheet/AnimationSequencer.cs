using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Super_Platformer.Code.Core.Spritesheet
{
    /// <summary>
    /// Animation sequencer is used to play animations.
    /// </summary>
    public class AnimationSequencer : IMonoUpdateable
    {
        /// <summary> The current Row of the animation. </summary>
        private int _currentRow;

        /// <summary> The current Column of the animation. </summary>
        private int _currentColumn;

        /// <summary> If the animation should continue to move. </summary>
        public bool Running
        {
            get;
            private set;
        }

        /// <summary> Keeps track of the elapsed time. </summary>
        private double _accumulator;

        /// <summary> Dictionary that stores all the animations. </summary>
        private Dictionary<int, Animation> _animationSet;

        /// <summary> Current Animation. </summary>
        private Animation _currentAnimation;

        /// <summary> Next Animation. </summary>
        private Animation _nextAnimation;

        /// <summary> Check if you can start a new animation. </summary>
        private bool _unlocked;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public AnimationSequencer()
        {
            _unlocked = true;
            _animationSet = new Dictionary<int, Animation>();
            _currentRow = 0;
            _currentColumn = 0;
            _accumulator = 0;
            Running = false;
        }

        /// <summary>
        /// Pauses the current animation.
        /// </summary>
        public void Pause()
        {
            Running = false;
        }

        /// <summary>
        /// Unpauses the current animation.
        /// </summary>
        public void Unpause()
        {
            // Can we start an animation?
            if (_currentAnimation != null)
            {
                Running = true;
            }
        }

        /// <summary>
        /// Enable to start a new animation.
        /// </summary>
        public void Unlock()
        {
            _unlocked = true;
        }

        /// <summary>
        /// Disable to start a new animation.
        /// </summary>
        public void Lock()
        {
            _unlocked = false;
        }

        /// <summary>
        /// Check if a animation request is valid.
        /// </summary>
        /// <param name="id"> Identification of the animation.</param>
        /// <returns></returns>
        private bool ValidAnimationRequest(int id)
        {
            return (_unlocked && ((_currentAnimation != null && _currentAnimation.Id != id) || _currentAnimation == null));
        }

        /// <summary>
        /// Check if a animation exists.
        /// </summary>
        /// <param name="id"> Identification of the animation.</param>
        /// <returns>If a animation exists.</returns>
        private bool CheckAnimationExistence(int id)
        {
            if (!_animationSet.ContainsKey(id))
            {
                Debug.WriteLine($"AnimationSequencer: couldn't find animation with id: {id}");

                return false;
            }

            return true;
        }

        /// <summary>
        /// Run Animation with the id provided.
        /// </summary>
        /// <param name="id"> Identification of the animation.</param>
        /// <returns>Returns true if the animation is found.</returns>
        public void Play(int id)
        {
            // Is the animation already running and can we start it?
            if (!ValidAnimationRequest(id))
            {
                return;
            }

            // Check if animation exists.
            if (CheckAnimationExistence(id))
            {
                // Get Animation from dictionary and store it in CurrentAnimation.
                _currentAnimation = _animationSet[id];

                Reset();
                Unpause();
            }
        }

        /// <summary>
        /// Play Animation with provided id once.
        /// </summary>
        /// <param name="id"> Identification of the animation.</param>
        public void PlayOnce(int id)
        {
            // Is the animation already running and can we start it?
            if (!ValidAnimationRequest(id))
            {
                return;
            }

            // Check if animation exists.
            if (CheckAnimationExistence(id))
            {
                // Save old animation.
                _nextAnimation = _currentAnimation;

                // Get Animation from dictionary and store it in CurrentAnimation.
                _currentAnimation = _animationSet[id];

                // Start running this animation.
                Unpause();
                Reset();
            }
        }

        /// <summary>
        /// Reset the current animation to its start frame.
        /// </summary>
        public void Reset()
        {
            _currentColumn = 0;
            _currentRow = 0;
            _accumulator = 0;
        }

        /// <summary>
        /// Add animation to the dictionary.
        /// </summary>
        /// <param name="animation">The Animation to add.</param>
        public void Add(Animation animation)
        {
            _animationSet.Add(animation.Id, animation);
        }

        /// <summary>
        /// Retrieve the current frame X coord.
        /// </summary>
        /// <returns>Current X coord.</returns>
        public int GetCurrentFrameX()
        {
            return (int)(_currentAnimation.BaseFrame.X + (_currentColumn * _currentAnimation.FrameSize.X));
        }

        /// <summary>
        /// Retrieve the current frame Y coord.
        /// </summary>
        /// <returns>Current Y coord.</returns>
        public int GetCurrentFrameY()
        {
            return (int)(_currentAnimation.BaseFrame.Y + (_currentRow * _currentAnimation.FrameSize.Y));
        }

        /// <summary>
        /// Update function (IMonoUpdatable).
        /// </summary>
        /// <param name="gameTime"> Game time.</param>
        public void Update(GameTime gameTime)
        {
            // Check if it should update.
            if (Running)
            {
                // Add the Elapsed game time to the accumulator.
                _accumulator += gameTime.ElapsedGameTime.TotalMilliseconds;

                // Check if the current frame should be updated.
                if (_accumulator >= _currentAnimation.MsPerFrame)
                {
                    // Reset accumulator and increace column count.
                    _accumulator = 0;
                    _currentColumn++;

                    // If the column count exceeds the max count reset it and increase row count.
                    if (_currentColumn >= _currentAnimation.Columns)
                    {
                        _currentColumn = 0;
                        _currentRow++;

                        // If the row count exceeds the max count reset it.
                        if (_currentRow >= _currentAnimation.Rows)
                        {
                            // If there is a next animation start it.
                            if (_nextAnimation != null)
                            {
                                _currentAnimation = _nextAnimation;
                                _nextAnimation = null;
                            }

                            _currentRow = 0;
                        }
                    }
                }
            }
        }
    }
}
