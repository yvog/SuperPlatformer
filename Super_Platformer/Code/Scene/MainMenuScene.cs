using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Super_Platformer.Code.Core.Rendering;
using Super_Platformer.Code.Core.Scene;
using Super_Platformer.Code.Core.UI;

namespace Super_Platformer.Code.Scene
{
    /// <summary>
    /// MainMenu scene.
    /// </summary>
    public class MainMenuScene : MonoScene
    {
        /// <summary> Main menu option list. </summary>
        private OptionList _optionList;

        /// <summary> Background music. </summary>
        private Song _song;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="game">The game this scene belongs to</param>
        public MainMenuScene(SuperPlatformerGame game) : base(game)
        {
            //
        }

        /// <summary>
        /// Initialize scene.
        /// </summary>
        public override void Init()
        {
            // Call base init.
            base.Init();

            // Create background.
            MonoSprite background = new MonoSprite(Game.Content.Load<Texture2D>("Images/Screens/TitleScreen"), new Rectangle(0, 0, SuperPlatformerGame.RESOLUTION_X, SuperPlatformerGame.RESOLUTION_Y), Vector2.Zero, SuperPlatformerGame.RESOLUTION_X, SuperPlatformerGame.RESOLUTION_Y);

            // Add background to children.
            Children.Add(background);

            // Create indicator for the option list
            MonoSprite indicator = new MonoSprite(Game.Content.Load<Texture2D>("Images/Arrow"), new Rectangle(0, 0, 8, 8), Vector2.Zero, 8, 8);

            // The font for our option list
            SpriteFont font = Game.Content.Load<SpriteFont>("Font/PixelFont");

            _song = Game.Content.Load<Song>("Audio/smw_titlescreen");

            MediaPlayer.Play(_song);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.6f;

            // We want a white text
            Color fillStyle = Color.White;

            // Create the option list
            _optionList = new OptionList(new List<OptionListItem>
            {
                new OptionListItem("Play \"Starter's Home\"", font, fillStyle, () =>
                {
                    Game.SceneActivator.ActivateScene(new LevelScene(Game, "StartersHome"));
                }),
                new OptionListItem("Play \"Hill Climb Forest\"", font, fillStyle, () =>
                {
                    Game.SceneActivator.ActivateScene(new LevelScene(Game, "HillClimbForest"));
                }),
                new OptionListItem("Play \"Pipe Maze\"", font, fillStyle, () =>
                {
                    Game.SceneActivator.ActivateScene(new LevelScene(Game, "PipeMaze"));
                }),
                new OptionListItem("Play \"The Cloud\"", font, fillStyle, () =>
                {
                    Game.SceneActivator.ActivateScene(new LevelScene(Game, "TheCloudLevel"));
                }),
                new OptionListItem("Play \"Kaizo\"", font, fillStyle, () =>
                {
                    Game.SceneActivator.ActivateScene(new LevelScene(Game, "Kaizo"));
                }),
                new OptionListItem("Quit", font, fillStyle, () =>
                {
                    Game.Exit();
                })
            }, Game.KeyboardDevice, indicator, new Vector2(25, 90), 15, SuperPlatformerGame.SCALE);

            // Add option list to children.
            Children.Add(_optionList);
        }

    }
}
