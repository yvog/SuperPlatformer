using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Super_Platformer.Code.Block;
using Super_Platformer.Code.Core;
using Super_Platformer.Code.Core.Physics;
using Super_Platformer.Code.Core.Rendering;
using Super_Platformer.Code.Event;
using Super_Platformer.Code.Mob;
using Super_Platformer.Code.Scene;
using Super_Platformer.Code.Score;
using Super_Platformer.Code.UI;
using static Super_Platformer.Code.Core.Physics.CollisionTester;

namespace Super_Platformer.Code.World
{
    /// <summary>
    /// Level class.
    /// </summary>
    public class Level : MonoObject
    {
        /// <summary> Type of the player.</summary>
        public enum PlayerType
        {
            /// <summary> protagonist player type. </summary>
            PROTAGONIST
        }

        /// <summary> Amount of time left.</summary>
        public int CurrentTime
        {
            get;
            private set;
        }

        /// <summary> Score collector.</summary>
        public ScoreCollector Score
        {
            get;
            private set;
        }

        /// <summary> Loads the content of the level. </summary>
        private LevelLoader _loader;

        /// <summary> Type of the player.</summary>
        public PlayerType TypePlayer
        {
            get;
            private set;
        }

        /// <summary> Gravity.</summary>
        public float Gravity
        {
            get;
            private set;
        }

        /// <summary> Level title.</summary>
        private string _title;

        /// <summary> Level scale.</summary>
        private int _scale;

        /// <summary> Level tilesize.</summary>
        private int _tileSize;

        /// <summary> Camera.</summary>
        private Camera2D _camera;

        /// <summary> List of entities to add.</summary>
        private List<Entity> _pendingEntities;

        /// <summary> List of background tiles.</summary>
        private List<Tile> _backgroundTiles;

        /// <summary> List of all entities in the level.</summary>
        private List<Entity> _entities;

        /// <summary> List of decorations.</summary>
        private List<MonoSprite> _decorations;

        /// <summary> Parallax level background.</summary>
        private ParallaxBackground _background;

        /// <summary> The collision tester.</summary>
        private CollisionTester _collisions;

        /// <summary> The Head Up Display.</summary>
        public HUD Display
        {
            get;
        }

        /// <summary> Spawn position.</summary>
        private Vector2 _spawn;

        /// <summary> Time to respawn.</summary>
        private TimedEvent _respawnTimer;

        /// <summary> Gets invoked every second to decrease the time.</summary>
        private TimedEvent _time;

        /// <summary> Wait X seconds before switching to level end scene.</summary>
        private TimedEvent _endTimer;

        /// <summary> Player object.</summary>
        public Player Player
        {
            get;
        }

        /// <summary> Number of pixels X.</summary>
        public int PixelSizeX
        {
            get;
            private set;
        }

        /// <summary> Number of pixels Y.</summary>
        public int PixelSizeY
        {
            get;
            private set;
        }

        /// <summary> Viewport of the game.</summary>
        private Viewport _viewport;

        /// <summary> List of all colliders during the frame. </summary>
        private List<Entity> _collidables;

        /// <summary> Event on ended.</summary>
        public event EventHandler<EventArgs> OnEnd;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="title"> Level title.</param>
        /// <param name="game"> Game.</param>
        public Level(string title, SuperPlatformerGame game)
        {
            CurrentTime = 0;
            Gravity = 550;

            // Create the lists.
            _entities = new List<Entity>();
            _pendingEntities = new List<Entity>();
            _decorations = new List<MonoSprite>();
            _backgroundTiles = new List<Tile>();

            // Time moves on.
            _time = new TimedEvent(OnTimeTick, 1000);
            _time.Enable();

            // Time to respawn
            _respawnTimer = new TimedEvent(OnRespawnTimerElapsed, 3000);

            // Wait X seconds before switching to end level scene on level finished.
            _endTimer = new TimedEvent(() =>
            {
                game.SceneActivator.ActivateScene(new EndLevelScene(game));
            }, 4000);

            _scale = SuperPlatformerGame.SCALE;

            _viewport = game.GraphicsDevice.Viewport;

            _title = title;

            _collisions = new CollisionTester();

            _camera = new Camera2D(_viewport);

            Player = new Player(game.Content.Load<Texture2D>("Images/Player/Protagonist"), Vector2.Zero, 16, 22, game.KeyboardDevice, this, game.Content);

            // Listen to some player events
            Player.OnSizeChanged += OnPlayerSizeChange;
            Player.OnDied += OnPlayerDeath;

            Score = new ScoreCollector();

            TypePlayer = PlayerType.PROTAGONIST;

            Display = new HUD(this, _camera, game.Content);

            _collidables = new List<Entity>();

            _camera.Follow(Player);
        }

        /// <summary>
        /// Callback every second.
        /// </summary>
        private void OnTimeTick()
        {
            CurrentTime--;
            _time.Enable();
        }

        /// <summary>
        /// Test collisions for the given entity on the given axis.
        /// </summary>
        /// <param name="ent"> The entity to check collisions for.</param>
        /// <param name="axis"> The axis to check collisions on.</param>
        public void TestCollisions(Entity ent, CollisionTester.Axis axis)
        {
            if (_collidables.Count == 0)
            {
                // Concatenate all entities of the current frame.
                _collidables = _backgroundTiles.Concat(_entities).ToList();

                _collidables.RemoveAll(e => !e.Collidable);
            }

            // Get nearest collidables
            List<Entity> checkableColliders = _collidables.OrderBy(e => Vector2.Distance(ent.Position, e.Position)).Take(12).ToList();

            foreach (Entity c in checkableColliders)
            {
                if (c != ent && c.IsCollidable(ent, axis) && _collisions.TestAABB(ent, c))
                {
                    // Get test results
                    CollisionTestResult result = _collisions.Result;
                    Entity collider = result.Collider;

                    // Resolve the collision occured
                    ent.OnCollision(collider, result.Penetration, result.Side, axis, collider.PreviousBounds);
                    collider.OnCollision(ent, result.Penetration, result.OppositeSide, axis, ent.PreviousBounds);
                }
            }
        }

        /// <summary>
        /// Add an entity to the level.
        /// </summary>
        /// <param name="ent"> The entity.</param>
        public void AddEntity(Entity ent)
        {
            _pendingEntities.Add(ent);
        }

        /// <summary>
        /// Update function (IMonoUpdatable).
        /// </summary>
        /// <param name="gameTime"> Game time.</param>
        public override void Update(GameTime gameTime)
        {
            // Time moves on
            _time.Update(gameTime);

            // Update end timer
            _endTimer.Update(gameTime);

            // Update respawn timer
            _respawnTimer.Update(gameTime);

            // Update entities
            _entities.ForEach((ent) =>
            {
                ent.Update(gameTime);
            });

            if (_pendingEntities.Count > 0)
            {
                // Insert pending entities to our entity list
                _pendingEntities.ForEach(e =>
                {
                    _entities.Insert(0, e);
                });

                // Clear the pending entities list
                _pendingEntities.Clear();
            }

            _camera.Update(gameTime);

            _background.Update(gameTime);

            // Remove all destroyed objects from our entities list
            _entities.RemoveAll((entity) => entity.Destroyable);

            // Dispose all colliders of the current frame.
            _collidables.Clear();
        }

        /// <summary>
        /// Render function (IMonoDrawable)
        /// </summary>
        /// <param name="spriteBatch"> Spritebatch to draw to.</param>
        /// <param name="graphics"> GraphicsDevice to use.</param>
        public override void Render(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _camera.GetViewMatrix());

            _background.Render(spriteBatch, graphics);

            // render background
            _backgroundTiles.Where(e => _camera.IsVisible(e.Position, e.Width, e.Height)).ToList().ForEach((tile) =>
            {
                tile.Render(spriteBatch, graphics);
            });

            // render decorations
            _decorations.Where(e => _camera.IsVisible(e.Position, e.Width, e.Height)).ToList().ForEach((decoration) =>
            {
                decoration.Render(spriteBatch, graphics);
            });

            // render entities
            _entities.Where(e => _camera.IsVisible(e.Position, e.Width, e.Height)).ToList().ForEach((entity) =>
            {
                entity.Render(spriteBatch, graphics);
            });

            spriteBatch.End();

            Display.Render(spriteBatch, graphics);
        }

        /// <summary>
        /// Is the time up?
        /// </summary>
        /// <returns>Boolean TimeUp.</returns>
        public bool TimeUp()
        {
            return (CurrentTime <= 0);
        }

        /// <summary>
        /// Load the level.
        /// </summary>
        /// <param name="game"> The game object.</param>
        /// <param name="fileName"> The filename of the level file.</param>
        public void Load(SuperPlatformerGame game, string fileName)
        {
            _loader = new LevelLoader(_title, this, game);

            _loader.Parse(@"Resources/Data/" + fileName + ".json");

            // Load background
            Texture2D texture = game.Content.Load<Texture2D>(_loader.GetBackgroundPath());

            MonoSprite backgroundImageLeft = new MonoSprite(texture, new Rectangle(0, 0, 512, 432), Vector2.Zero, 512, 432);
            MonoSprite backgroundImageRight = new MonoSprite(texture, new Rectangle(0, 0, 512, 432), Vector2.Zero, 512, 432);

            _background = new ParallaxBackground(_camera, backgroundImageLeft, backgroundImageRight);

            // Set the current time to the given level duration
            CurrentTime = _loader.GetTotalTime();

            // Set the given tilesize
            _tileSize = _loader.GetTileSize();

            // Initialize the background tiles
            _loader.InitializeBackgroundTiles(out _backgroundTiles);

            // Initialize the decorations
            _loader.InitializeDecoration(out _decorations);

            // Set the player spawn point
            _spawn = _loader.GetSpawnPoint();

            // Initialize the player's position
            Player.Position = _spawn;

            // Load entities
            LoadEntities();

            // Set pixel sizes
            PixelSizeX = (_loader.TilesX * _tileSize) * (int)_scale;
            PixelSizeY = (_loader.TilesY * _tileSize) * (int)_scale;

            // Set camera bounds
            _camera.SetBounds(PixelSizeX, PixelSizeY);
        }

        /// <summary>
        /// Load entities.
        /// </summary>
        private void LoadEntities()
        {
            _entities = new List<Entity>();

            // Set the current time to the given level duration
            CurrentTime = _loader.GetTotalTime();

            // Initialize blocks
            List<Entity> blocks = _loader.InitializeBlocks();
            blocks.ForEach(AddEntity);

            // Initialize coins
            List<Entity> coins = _loader.InitializeCoins();
            coins.ForEach(AddEntity);

            // Initialize the enemies
            List<Entity> enemies = _loader.InitializeEnemies();
            enemies.ForEach(AddEntity);

            // Add the finish line
            FinishLine finishLine = _loader.GetFinishLine();
            _entities.Add(finishLine);

            // Listen to finishLine's on closed event
            finishLine.OnClosed += OnFinished;

            _entities.Add(Player);
        }

        /// <summary>
        /// Event when player's size changed.
        /// </summary>
        /// <param name="sender"> The sender.</param>
        /// <param name="e"> The event arguments.</param>
        private void OnPlayerSizeChange(object sender, SizeChangeEventArgs e)
        {
            if (e.State == Player.SizeState.SMALL)
            {
                Display.DropPowerUp();
            }
        }

        /// <summary>
        /// Event when player died.
        /// </summary>
        /// <param name="sender"> The sender.</param>
        /// <param name="e"> The event arguments.</param>
        private void OnPlayerDeath(object sender, EventArgs e)
        {
            _respawnTimer.Reset();
            _respawnTimer.Enable();
        }

        /// <summary>
        /// Make a respawn happen.
        /// </summary>
        private void OnRespawnTimerElapsed()
        {
            if (Player.Lives > 0)
            {
                LoadEntities();

                Score.Reset();

                Player.Position = _spawn;
                Player.OnRespawn();
            }
            else
            {
                OnEnd(this, new EventArgs());
            }
        }

        /// <summary>
        /// Activated when player finished the level.
        /// </summary>
        /// <param name="sender"> The sender.</param>
        /// <param name="e"> The event arguments.</param>
        private void OnFinished(object sender, EventArgs e)
        {
            MediaPlayer.Stop();
            Player.OnPeace();
            _endTimer.Enable();
            _time.Disable();
        }
    }

}