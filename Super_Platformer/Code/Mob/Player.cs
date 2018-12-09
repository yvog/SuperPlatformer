using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Super_Platformer.Code.Core;
using Super_Platformer.Code.Core.Input;
using Super_Platformer.Code.Core.Rendering;
using Super_Platformer.Code.Core.Spritesheet;
using Super_Platformer.Code.Event;
using Super_Platformer.Code.World;

namespace Super_Platformer.Code.Mob
{
    /// <summary>
    /// Player class.
    /// </summary>
    public class Player : MovingEntity
    {
        private enum PlayerAnimation : int
        {
            SMALL_LOOKUP = 0,
            SMALL_IDLE = 1,
            SMALL_WALK = 2,
            SMALL_DUCK = 3,
            SMALL_JUMP = 4,
            SMALL_FALL = 5,
            SMALL_PEACE = 6,

            BIG_LOOKUP = 7,
            BIG_IDLE = 8,
            BIG_WALK = 9,
            BIG_DUCK = 10,
            BIG_JUMP = 11,
            BIG_FALL = 12,
            BIG_PEACE = 13,

            DEATH = 14
        }

        /// <summary> Sizes the player can be. </summary>
        public enum SizeState
        {
            /// <summary> Player small size. </summary>
            SMALL,
            /// <summary> Player big size. </summary>
            BIG
        }

        /// <summary> Current power state.</summary>
        public SizeState PowerState
        {
            get;
            private set;
        }

        /// <summary> Allowed to walk or not?</summary>
        private bool _allowWalking;

        /// <summary> Width when small.</summary>
        private int _smallWidth = 16;

        /// <summary> Height when small.</summary>
        private int _smallHeight = 22;

        /// <summary> Jump impulse when small.</summary>
        private float _smallJumpImpulse = 270;

        /// <summary> Width when big.</summary>
        private int _bigWidth = 16;

        /// <summary> Height when big.</summary>
        private int _bigHeight = 32;

        /// <summary> Jump impulse when big.</summary>
        private float _bigJumpImpulse = 290;

        /// <summary> Keyboard device.</summary>
        private KeyboardInput _keyboard;

        /// <summary> Collection of sounds.</summary>
        private Dictionary<string, SoundEffect> _sounds;

        /// <summary> Maximum lives.</summary>
        public int MaxLives
        {
            get;
            set;
        }

        /// <summary> Lives leftover.</summary>
        public int Lives
        {
            get;
            set;
        }

        /// <summary> Amount of time flickered.</summary>
        private int _timesFlickered;

        /// <summary> Interval of flickers.</summary>
        private TimedEvent _flickerTimer;

        /// <summary> Event on size changed.</summary>
        public event EventHandler<SizeChangeEventArgs> OnSizeChanged;

        /// <summary> Event Handler on size changed.</summary>
        public delegate void SizeChangeHandler(object sender, SizeChangeEventArgs e);

        /// <summary> Event on death.</summary>
        public event EventHandler<EventArgs> OnDied;

        /// <summary> Event Handler on death.</summary>
        public delegate void OnDeathEventHandler(object sender, EventArgs e);

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="texture"> Texture.</param>
        /// <param name="position"> Location in game.</param>
        /// <param name="width"> Width.</param>
        /// <param name="height"> Height.</param>
        /// <param name="keyboard"> Keyboard device.</param>
        /// <param name="level"> The level the player is in.</param>
        /// <param name="content"> The content manager.</param>
        public Player(Texture2D texture, Vector2 position, int width, int height, KeyboardInput keyboard, Level level, ContentManager content) :
            base(position, width, height, level)
        {
            Sprite = new MonoSprite(texture, new Rectangle(80, 10, width, height), position, width, height);
            Padding = new Vector2(3, 3);
            Invulnerable = false;
            EnableInput();
            MaxLives = 3;
            Lives = MaxLives;
            _keyboard = keyboard;

            _timesFlickered = 0;
            _flickerTimer = new TimedEvent(Flicker, 80);

            _sounds = new Dictionary<string, SoundEffect>()
            {
                { "jump", content.Load<SoundEffect>("Audio/smw_jump") },
                { "grow", content.Load<SoundEffect>("Audio/smw_powerup") },
                { "shrink", content.Load<SoundEffect>("Audio/smw_powerdown") },
                { "death", content.Load<SoundEffect>("Audio/smw_lostalife") },
                { "peace", content.Load<SoundEffect>("Audio/smw_courseclear") }
            };

            TerminalVelocity = new Vector2(150, 350);
            Acceleration = 290;
            JumpImpulse = _smallJumpImpulse;

            Grounded = false;
            _allowWalking = true;

            Animations.Add(new Animation(
                id: (int)PlayerAnimation.SMALL_LOOKUP,
                baseFrame: new Vector2(114, 10),
                frameSize: new Vector2(width, height),
                rows: 1,
                columns: 1,
                msPerFrame: 50
            ));

            Animations.Add(new Animation(
                id: (int)PlayerAnimation.SMALL_WALK,
                baseFrame: new Vector2(0, 10),
                frameSize: new Vector2(width, height),
                rows: 1,
                columns: 2,
                msPerFrame: 110
            ));

            Animations.Add(new Animation(
                id: (int)PlayerAnimation.SMALL_IDLE,
                baseFrame: new Vector2(0, 10),
                frameSize: new Vector2(width, height),
                rows: 1,
                columns: 1,
                msPerFrame: 50
            ));

            Animations.Add(new Animation(
                id: (int)PlayerAnimation.SMALL_DUCK,
                baseFrame: new Vector2(96, 10),
                frameSize: new Vector2(width, height),
                rows: 1,
                columns: 1,
                msPerFrame: 175
            ));

            Animations.Add(new Animation(
                id: (int)PlayerAnimation.SMALL_JUMP,
                baseFrame: new Vector2(64, 10),
                frameSize: new Vector2(width, height),
                rows: 1,
                columns: 1,
                msPerFrame: 175
            ));

            Animations.Add(new Animation(
                id: (int)PlayerAnimation.SMALL_FALL,
                baseFrame: new Vector2(80, 10),
                frameSize: new Vector2(width, height),
                rows: 1,
                columns: 1,
                msPerFrame: 175
            ));

            Animations.Add(new Animation(
                id: (int)PlayerAnimation.BIG_LOOKUP,
                baseFrame: new Vector2(145, 32),
                frameSize: new Vector2(_bigWidth, _bigHeight),
                rows: 1,
                columns: 1,
                msPerFrame: 50
            ));

            Animations.Add(new Animation(
                id: (int)PlayerAnimation.BIG_WALK,
                baseFrame: new Vector2(0, 32),
                frameSize: new Vector2(_bigWidth, _bigHeight),
                rows: 1,
                columns: 2,
                msPerFrame: 110
            ));

            Animations.Add(new Animation(
                id: (int)PlayerAnimation.BIG_IDLE,
                baseFrame: new Vector2(0, 32),
                frameSize: new Vector2(_bigWidth, _bigHeight),
                rows: 1,
                columns: 1,
                msPerFrame: 175
            ));

            Animations.Add(new Animation(
                id: (int)PlayerAnimation.BIG_DUCK,
                baseFrame: new Vector2(128, 32),
                frameSize: new Vector2(_bigWidth, _bigHeight),
                rows: 1,
                columns: 1,
                msPerFrame: 175
            ));

            Animations.Add(new Animation(
                id: (int)PlayerAnimation.BIG_JUMP,
                baseFrame: new Vector2(96, 32),
                frameSize: new Vector2(_bigWidth, _bigHeight),
                rows: 1,
                columns: 1,
                msPerFrame: 175
            ));

            Animations.Add(new Animation(
                id: (int)PlayerAnimation.BIG_FALL,
                baseFrame: new Vector2(112, 32),
                frameSize: new Vector2(_bigWidth, _bigHeight),
                rows: 1,
                columns: 1,
                msPerFrame: 175
            ));

            Animations.Add(new Animation(
                id: (int)PlayerAnimation.DEATH,
                baseFrame: new Vector2(131, 8),
                frameSize: new Vector2(15, 24),
                rows: 1,
                columns: 2,
                msPerFrame: 135
            ));

            Animations.Add(new Animation(
                 id: (int)PlayerAnimation.SMALL_PEACE,
                 baseFrame: new Vector2(162, 10),
                 frameSize: new Vector2(16, 21),
                 rows: 1,
                 columns: 1,
                 msPerFrame: 0
             ));

            Animations.Add(new Animation(
                 id: (int)PlayerAnimation.BIG_PEACE,
                 baseFrame: new Vector2(162, 36),
                 frameSize: new Vector2(16, 28),
                 rows: 1,
                 columns: 1,
                 msPerFrame: 0
             ));
        }

        /// <summary>
        /// This method kills protagonist.
        /// </summary>
        public void OnDeath()
        {
            velocity = Vector2.Zero;

            Collidable = false;
            Solid = false;
            Invulnerable = true;

            Lives--;

            IgnoreInput();

            Width = _smallWidth;
            Height = _smallHeight;

            Animations.Play((int)PlayerAnimation.DEATH);
            Animations.Lock();

            Jump();

            MediaPlayer.Pause();
            PlaySound("death");

            OnDied?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// This method is called by enemies when they collide with the player.
        /// </summary>
        public void OnEnemyCollision()
        {
            if (!Invulnerable)
            {
                Jump();

                if (PowerState == SizeState.BIG)
                {
                    Shrink();
                }
                else
                {
                    OnDeath();
                }
            }
        }

        /// <summary>
        /// Make the player jump.
        /// </summary>
        public override void Jump()
        {
            base.Jump();

            // Check if player is alive.
            if (Solid)
            {
                PlaySound("jump");
            }
        }

        /// <summary>
        /// This method makes the player go to the big state.
        /// </summary>
        public void Grow()
        {
            PowerState = SizeState.BIG;

            Width = _bigWidth;
            Height = _bigHeight;

            Vector2 position = Position;
            position.Y -= _bigHeight - _smallHeight;
            Position = position;

            JumpImpulse = _bigJumpImpulse;

            OnSizeChanged?.Invoke(this, new SizeChangeEventArgs(PowerState));

            PlaySound("grow");

            _flickerTimer.Enable();
        }

        /// <summary>
        /// This method makes the player go to the small state.
        /// </summary>
        public void Shrink()
        {
            Invulnerable = true;
            Solid = false;

            PowerState = SizeState.SMALL;

            Width = _smallWidth;
            Height = _smallHeight;

            JumpImpulse = _smallJumpImpulse;

            OnSizeChanged?.Invoke(this, new SizeChangeEventArgs(PowerState));

            PlaySound("shrink");

            _flickerTimer.Enable();
        }

        /// <summary>
        /// Update function (IMonoUpdatable)
        /// </summary>
        /// <param name="gameTime"> Game time.</param>
        public override void Update(GameTime gameTime)
        {
            // When not walking, play idle animation
            PlayAnimation(((int)velocity.X == 0) ? PlayerAnimation.SMALL_IDLE : PlayerAnimation.SMALL_WALK);

            if (InputEnabled)
            {
                Control(gameTime);
            }

            _flickerTimer.Update(gameTime);

            base.Update(gameTime);

            if ((Position.Y * Scale) > Parent.PixelSizeY && Collidable)
            {
                OnDeath();
            }
        }

        private void Control(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;

            // Start a jump
            if (_keyboard.KeyDown(Keys.Space) && Grounded)
            {
                Jump();
            }

            if (_allowWalking && _keyboard.KeyUp(Keys.W))
            {
                // Move Left
                if (_keyboard.KeyHeld(Keys.A) && _keyboard.KeyUp(Keys.D))
                {
                    FacingDirection = Facing.LEFT;
                    velocity.X -= Acceleration * delta;
                }

                // Move right
                if (_keyboard.KeyHeld(Keys.D) && _keyboard.KeyUp(Keys.A))
                {
                    FacingDirection = Facing.RIGHT;
                    velocity.X += Acceleration * delta;
                }
            }

            // Friction physics
            if (!_allowWalking || _keyboard.KeyUp(Keys.A) && _keyboard.KeyUp(Keys.D))
            {
                velocity.X *= (float)Math.Pow(Friction, delta);

                if (Math.Abs(velocity.X) <= 6)
                {
                    velocity.X = 0;
                }
            }

            // Set lookup animation
            if (_keyboard.KeyHeld(Keys.W) && Grounded)
            {
                _allowWalking = false;
                PlayAnimation(PlayerAnimation.SMALL_LOOKUP);
            }

            // Set jump or fall animation
            if (!Grounded)
            {
                PlayerAnimation animationKey = (velocity.Y < 0 ? PlayerAnimation.SMALL_JUMP : PlayerAnimation.SMALL_FALL);

                PlayAnimation(animationKey);
            }

            // Duck
            if (_keyboard.KeyHeld(Keys.S) && Grounded)
            {
                _allowWalking = false;
                PlayAnimation(PlayerAnimation.SMALL_DUCK);
            }

            _allowWalking = (_keyboard.KeyUp(Keys.S) && _keyboard.KeyUp(Keys.W));
        }

        /// <summary>
        /// Make the player show peace.
        /// </summary>
        public void OnPeace()
        {
            PlayAnimation(PlayerAnimation.SMALL_PEACE);
            Animations.Lock();

            if (PowerState == SizeState.BIG)
            {
                Width = 16;
                Height = 28;
            }
            else
            {
                Width = 16;
                Height = 21;
            }

            PlaySound("peace");

            SyncBoundingBox();
            SyncTexture();

            velocity.X = 0;
            Invulnerable = true;

            IgnoreInput();
        }

        /// <summary>
        /// Method to call when player respawns.
        /// </summary>
        public void OnRespawn()
        {
            velocity = Vector2.Zero;

            Collidable = true;
            Solid = true;

            Shrink();

            EnableInput();

            MediaPlayer.Resume();

            Animations.Unlock();
            Animations.Play((int)PlayerAnimation.SMALL_IDLE);
        }

        /// <summary>
        /// Let the player flicker.
        /// </summary>
        private void Flicker()
        {
            float maxFlickers = 16;

            _timesFlickered++;

            Sprite.Visible = (_timesFlickered % 2 == 0);

            _flickerTimer.Enable();

            if (_timesFlickered >= maxFlickers)
            {
                _flickerTimer.Disable();
                _timesFlickered = 0;
                Invulnerable = false;
                Solid = true;
            }
        }

        /// <summary>
        /// Play an animation size independently
        /// </summary>
        /// <param name="id"> The animation id.</param>
        private void PlayAnimation(PlayerAnimation id)
        {
            // hack start
            string key = id.ToString();

            if (PowerState == SizeState.BIG)
            {
                key = key.Replace("SMALL_", "BIG_");

                Enum.TryParse(key, out id);
            }
            // hack end

            Animations.Play((int)id);
        }

        /// <summary> Play a sound from the players sound collection.</summary>
        /// <param name="soundKey"></param>
        private void PlaySound(string soundKey)
        {
            if (_sounds.ContainsKey(soundKey) && _sounds[soundKey] != null)
            {
                _sounds[soundKey].Play();
            }
        }
    }
}