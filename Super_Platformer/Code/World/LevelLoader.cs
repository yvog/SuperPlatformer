using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Super_Platformer.Code.Block;
using Super_Platformer.Code.Core;
using Super_Platformer.Code.Core.Rendering;
using Super_Platformer.Code.Item;
using Super_Platformer.Code.Mob;

namespace Super_Platformer.Code.World
{
    /// <summary>
    /// Level loader class.
    /// </summary>
    public class LevelLoader
    {
        /// <summary> Background tile types.</summary>
        private enum BackgroundTileType : int
        {
            /// <summary> Grass background tile. </summary>
            GRASS = 1,
            /// <summary> Dirt background tile. </summary>
            DIRT = 2
        }

        /// <summary> Block types.</summary>
        private enum BlockType : int
        {
            /// <summary> Coinblock block. </summary>
            COINBLOCK = 1,
            /// <summary> Gift block. </summary>
            GIFTBLOCK = 2,
            /// <summary> Stone block. </summary>
            STONE = 3,
            /// <summary> Cloud block. </summary>
            CLOUD = 4,
            /// <summary> Wood block. </summary>
            WOOD = 5,
            /// <summary> Pipe block. </summary>
            PIPE = 6,
            /// <summary> pipe with plunger. </summary>
            PLUNGER_PIPE = 7,
        }

        /// <summary> Enemy types.</summary>
        private enum EnemyType : int
        {
            /// <summary> BUBBLEGUM_ENEMY enemy. </summary>
            BUBBLEGUM_ENEMY = 0,
            /// <summary> CHICKEN_ENEMY enemy. </summary>
            CHICKEN_ENEMY = 1,
            /// <summary> FLOWER_ENEMY enemy. </summary>
            FLOWER_ENEMY = 2,
        }

        /// <summary> Decoration types.</summary>
        private enum DecorationType : int
        {
            /// <summary> Sign Decoration. </summary>
            SIGN = 0,
            /// <summary> Bush Decoration. </summary>
            BUSH = 1
        }

        /// <summary> Number of tiles X.</summary>
        public int TilesX
        {
            get;
            private set;
        }

        /// <summary> Number of tiles Y.</summary>
        public int TilesY
        {
            get;
            private set;
        }

        /// <summary> The game object.</summary>
        private SuperPlatformerGame _game;

        /// <summary> Tag to know the level to load.</summary>
        private string _levelTag;

        /// <summary> Parent.</summary>
        private Level _level;

        /// <summary> Level data.</summary>
        private JToken _levelData;

        /// <summary> Entity data from the level data.</summary>
        private JObject _entityData;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="levelTag"> Level tag.</param>
        /// <param name="level"> Level object.</param>
        /// <param name="game"> Game object.</param>
        public LevelLoader(string levelTag, Level level, SuperPlatformerGame game)
        {
            _levelTag = levelTag;
            _level = level;
            _game = game;
        }

        /// <summary>
        /// Parse the given file path.
        /// </summary>
        /// <param name="path"> The file path.</param>
        public void Parse(string path)
        {
            // Get de file contents
            string levelContents = File.ReadAllText(path);

            // Parse the level data into a JToken
            _levelData = JObject.Parse(levelContents).GetValue(_levelTag);

            // Parse the entity data of the level data into a JObject
            _entityData = JObject.Parse(_levelData["entities"].ToString());
        }

        /// <summary>
        /// Retrieve the level background file path.
        /// </summary>
        /// <returns>Background file path.</returns>
        public string GetBackgroundPath()
        {
            return $"Images/Background/{_levelData["background"]}";
        }

        /// <summary>
        /// Get the spawn point.
        /// </summary>
        /// <returns>Spawn point.</returns>
        public Vector2 GetSpawnPoint()
        {
            int tileSize = GetTileSize();

            return new Vector2(((float)_levelData["spawnPointX"]) * tileSize, ((float)_levelData["spawnPointY"]) * tileSize);
        }

        /// <summary>
        /// Retrieve a finish line.
        /// </summary>
        /// <returns>FinishLine object.</returns>
        public FinishLine GetFinishLine()
        {
            JToken data = _levelData["finish"];
            int tileSize = GetTileSize();

            return new FinishLine(new Vector2((float)data["x"] * tileSize, (float)data["y"] * tileSize), _level, _game.Content);
        }

        /// <summary>
        /// Get the tile size.
        /// </summary>
        /// <returns>Tile size.</returns>
        public int GetTileSize()
        {
            return (int)_levelData["tileSize"];
        }

        /// <summary>
        /// Get the total amount of time.
        /// </summary>
        /// <returns>Total amount of time.</returns>
        public int GetTotalTime()
        {
            return (int)_levelData["time"];
        }

        /// <summary>
        /// Initialize the background tiles.
        /// </summary>
        /// <param name="backgroundTiles"> List of background tiles.</param>
        public void InitializeBackgroundTiles(out List<Tile> backgroundTiles)
        {
            // Create the background tiles list.
            backgroundTiles = new List<Tile>();

            // Load the texture.
            Texture2D texture = _game.Content.Load<Texture2D>("Images/Tile/Blocks");

            // Retrieve the tilesize.
            int tileSize = GetTileSize();

            // Generate an array with background tile type IDs.
            BackgroundTileType[,] tiles = JsonConvert.DeserializeObject<BackgroundTileType[,]>(_levelData["tiles"].ToString());

            TilesY = tiles.GetLength(0);
            TilesX = tiles.GetLength(1);

            // Convert the tile IDs to a real object.
            int x;
            int y;
            Tile tile;

            // Convert those IDs to a real object.
            for (y = 0; y < TilesY; y++)
            {
                for (x = 0; x < TilesX; x++)
                {
                    tile = null;

                    switch (tiles[y, x])
                    {
                        case BackgroundTileType.GRASS:
                            tile = new Platform(texture, new Rectangle(0, 48, 16, 16), new Vector2(x * tileSize, y * tileSize), 16, 16, true, _level);
                            break;

                        case BackgroundTileType.DIRT:
                            tile = new Tile(texture, new Rectangle(0, 64, 16, 16), new Vector2(x * tileSize, y * tileSize), 16, 16, false, _level);
                            break;
                    }

                    if (tile != null)
                    {
                        backgroundTiles.Add(tile);
                    }
                }
            }

            // Set the neighbour aware background frames.
            SetBackgroundFrames(backgroundTiles);
        }

        /// <summary>
        /// Set background frames.
        /// </summary>
        /// <param name="tiles"> The list of tiles.</param>
        private void SetBackgroundFrames(List<Tile> tiles)
        {
            int tileSize = GetTileSize();

            tiles.OrderBy(e => e.Position.Y).ToList().ForEach((tile) =>
            {
                bool right = tiles.All((s) => !(tile.Bounds.Right == s.Bounds.Left && tile.Bounds.Top == s.Bounds.Top)); // Check if there is no tile to the left.
                bool left = tiles.All((s) => !(tile.Bounds.Left == s.Bounds.Right && tile.Bounds.Top == s.Bounds.Top));  // Check if there is no tile to the right.

                if (!(tile is Platform) && !right && !left) // only check dirt if corner above
                {
                    if (tiles.Where((s) => (tile.Bounds.Right == s.Bounds.Right) && tile.Bounds.Top == s.Bounds.Bottom).Count() > 0) // Check if tile above.
                    {
                        Tile t = tiles.Where((s) => (tile.Bounds.Right == s.Bounds.Right) && tile.Bounds.Top == s.Bounds.Bottom).First();
                        right = (t.CornerSide == Tile.Corner.RIGHT);
                        left = (t.CornerSide == Tile.Corner.LEFT);
                    }
                }

                // Middle grass/dirt, texture 1.
                int index = 1;
                // Dirt on row 4.
                int rowindex = 4;

                // Right grass/dirt, texture 2.
                if (right)
                {
                    index = 2;
                    tile.CornerSide = Tile.Corner.RIGHT;
                }

                // Left grass/dirt, texture 0.
                if (left)
                {
                    index = 0;
                    tile.CornerSide = Tile.Corner.LEFT;
                }

                // Platforms (grass) check if corner
                if (tile is Platform)
                {
                    // Check if grass on top.
                    bool top = tiles.Any((s) => tile.Bounds.Bottom == s.Bounds.Top && tile.Bounds.Left == s.Bounds.Left && s is Platform);
                    // Check if grass on bottom.
                    bool bottom = tiles.Any((s) => tile.Bounds.Top == s.Bounds.Bottom && tile.Bounds.Left == s.Bounds.Left && s is Platform);
                    // Check if left or right (false on right)
                    bool leftcorner = tiles.Any((s) => tile.Bounds.Left == s.Bounds.Right && tile.Bounds.Top == s.Bounds.Top && s is Platform);

                    // Grass on row 3
                    rowindex = 3;

                    // Check if left corner piece
                    if ((top || bottom) && leftcorner)
                    {
                        // Left corner, texture 3.
                        index = 4;
                        // Add side collision to top piece.
                        ((Platform)tile).CollideRight = top;
                    }
                    else if ((top || bottom) && !leftcorner) // Check right left corner piece.
                    {
                        // Right corner, texture 3.
                        index = 3;

                        // Add side collision to top piece.
                        ((Platform)tile).CollideLeft = top;
                    }

                    // Bottom grass corners on row 4.
                    if (bottom) rowindex = 4;

                    // Check if right from grass is dirt and this tile is not a bottom corner.
                    if (!bottom && tiles.Where((s) => tile.Bounds.Top == s.Bounds.Top && tile.Bounds.Right == s.Bounds.Left).Any((s) => !(s is Platform)))
                    {
                        tile.CornerSide = Tile.Corner.RIGHT;
                        rowindex = 2;
                        index = 4;
                    }
                    // Check if left from grass is dirt and this tile is not a not bottom corner.
                    else if (!bottom && tiles.Where((s) => tile.Bounds.Top == s.Bounds.Top && tile.Bounds.Left == s.Bounds.Right).Any((s) => !(s is Platform)))
                    {
                        tile.CornerSide = Tile.Corner.LEFT;
                        rowindex = 2;
                        index = 3;
                    }

                }

                tile.Sprite.source = new Rectangle(tileSize * index, tileSize * rowindex, tileSize, tileSize);
            });
        }

        /// <summary>
        /// Initialize the blocks.
        /// </summary>
        /// <returns>List of block objects.</returns>
        public List<Entity> InitializeBlocks()
        {
            // Deserialize the blocks data.
            JToken[] blocksData = JsonConvert.DeserializeObject<JToken[]>(_entityData.GetValue("blocks").ToString());
            List<Entity> blocks = new List<Entity>();

            Texture2D texture = _game.Content.Load<Texture2D>("Images/Tile/Blocks");

            int tileSize = GetTileSize();

            int blockId;
            int positionX;
            int positionY;

            int i;
            JToken blockData;

            // Convert the data to real objects.
            for (i = 0; i < blocksData.Length; i++)
            {
                blockData = blocksData[i];

                blockId = (int)blockData["id"];
                positionX = (int)blockData["x"] * tileSize;
                positionY = (int)blockData["y"] * tileSize;

                Entity block = null;

                switch ((BlockType)blockId)
                {
                    case BlockType.COINBLOCK:
                        block = new ValueBlock(new Vector2(positionX, positionY), 16, 16, (int)blockData["coins"], _game.Content, _level);
                        break;

                    case BlockType.GIFTBLOCK:
                        PowerUp item = new Mushroom(Vector2.Zero, 16, 16, _game.Content, _level);
                        block = new GiftBlock(new Vector2(positionX, positionY), 16, 16, item, _game.Content, _level);
                        break;

                    case BlockType.STONE:
                        block = new Tile(texture, new Rectangle(64, 16, 16, 16), new Vector2(positionX, positionY), 16, 16, true, _level);
                        break;

                    case BlockType.CLOUD:
                        block = new Platform(texture, new Rectangle(32, 32, 16, 16), new Vector2(positionX, positionY), 16, 16, true, _level);
                        break;

                    case BlockType.WOOD:
                        block = new Tile(texture, new Rectangle(64, 0, 16, 16), new Vector2(positionX, positionY), 16, 16, true, _level);
                        break;

                    case BlockType.PIPE:

                        int column = 0;

                        if ((bool)blockData["top"])
                        {
                            column = 16;
                        }

                        block = new Pipe(texture, new Rectangle(column, 32, 16, 16), new Vector2(positionX, positionY), _level);

                        break;

                    case BlockType.PLUNGER_PIPE:
                        block = new Pipe(texture, new Rectangle(16, 32, 16, 16), new Vector2(positionX, positionY), _level, _game.Content);
                        break;
                }

                if (block != null)
                {
                    blocks.Add(block);
                }
            }

            return blocks;
        }

        /// <summary>
        /// Initialize coins.
        /// </summary>
        /// <returns>List of coins.</returns>
        public List<Entity> InitializeCoins()
        {
            JToken[] coinsData = JsonConvert.DeserializeObject<JToken[]>(_entityData.GetValue("coins").ToString());

            int tileSize = GetTileSize();
            int translationX = 2;

            int i;
            JToken coinData;

            List<Entity> coins = new List<Entity>();

            for (i = 0; i < coinsData.Length; i++)
            {
                coinData = coinsData[i];

                coins.Add(new Coin(new Vector2(
                    (int)coinData["x"] * tileSize + translationX,
                    (int)coinData["y"] * tileSize
                ), _game.Content, _level));
            }

            return coins;
        }

        /// <summary>
        /// Initialize decoration.
        /// </summary>
        /// <param name="decorations"> List of decorations.</param>
        public void InitializeDecoration(out List<MonoSprite> decorations)
        {
            decorations = new List<MonoSprite>();

            Texture2D signTexture = _game.Content.Load<Texture2D>("Images/Decoration/Waypoint");
            Texture2D bushTexture = _game.Content.Load<Texture2D>("Images/Decoration/Bush");

            JToken[] decorationData = JsonConvert.DeserializeObject<JToken[]>(_entityData.GetValue("decoration").ToString());
            int tileSize = GetTileSize();

            int i;
            JToken deco;
            MonoSprite sprite;

            for (i = 0; i < decorationData.Length; i++)
            {
                deco = decorationData[i];
                sprite = null;

                switch ((DecorationType)((int)deco["id"]))
                {
                    case DecorationType.SIGN:
                        int translationX = 8;

                        sprite = new MonoSprite(signTexture, new Rectangle(0, 0, 32, 24), new Vector2(((int)deco["x"]) * tileSize, ((int)deco["y"]) * tileSize + translationX), 32, 24);
                        break;
                    case DecorationType.BUSH:
                        sprite = new MonoSprite(bushTexture, new Rectangle((int)deco["subtype"] * tileSize, 0, 16, 16), new Vector2(((int)deco["x"]) * tileSize, ((int)deco["y"]) * tileSize), 16, 16);
                        break;
                }

                if (sprite != null)
                {
                    decorations.Add(sprite);
                }
            }
        }

        /// <summary>
        /// Initialize enemies.
        /// </summary>
        /// <returns> List of enemies.</returns>
        public List<Entity> InitializeEnemies()
        {
            JToken[] enemiesData = JsonConvert.DeserializeObject<JToken[]>(_entityData.GetValue("enemies").ToString());

            List<Entity> enemies = new List<Entity>();

            int tileSize = GetTileSize();

            int i;
            JToken enemyData;

            int entId;
            int positionX;
            int positionY;

            Entity enemy;

            for (i = 0; i < enemiesData.Length; i++)
            {
                enemyData = enemiesData[i];

                entId = (int)enemyData["id"];
                positionX = ((int)enemyData["x"]) * tileSize;
                positionY = ((int)enemyData["y"]) * tileSize;

                enemy = null;

                switch ((EnemyType)entId)
                {
                    case EnemyType.BUBBLEGUM_ENEMY:
                        enemy = new BubbleGumEnemy(new Vector2(positionX, positionY), 16, 16, _game.Content, _level);
                        break;

                    case EnemyType.CHICKEN_ENEMY:
                        enemy = new ChickenEnemy(new Vector2(positionX, positionY), 16, 27, _game.Content, _level);
                        break;

                    case EnemyType.FLOWER_ENEMY:
                        enemy = new FlowerEnemy(new Vector2(positionX, positionY), 16, 16, _game.Content, _level);
                        break;
                }

                if (enemy != null)
                {
                    enemies.Add(enemy);
                }
            }

            return enemies;
        }
    }
}
