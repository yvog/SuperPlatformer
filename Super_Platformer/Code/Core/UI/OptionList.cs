using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Super_Platformer.Code.Core.Input;
using Super_Platformer.Code.Core.Rendering;

namespace Super_Platformer.Code.Core.UI
{
    /// <summary>
    /// OptionList.
    /// </summary>
    public class OptionList : MonoObject
    {
        /// <summary> Option list's items.</summary>
        private List<OptionListItem> _items;

        /// <summary> Keyboard device.</summary>
        private KeyboardInput _keyboard;

        /// <summary> Shows what item is selected.</summary>
        private MonoSprite _indicator;

        /// <summary> Remember current selected item.</summary>
        private int _currentIndex;

        /// <summary> Scale.</summary>
        private int _scale;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="items"> The option list items.</param>
        /// <param name="keyboard"> Keyboard device.</param>
        /// <param name="indicator"> Sprite which indicates the selected item.</param>
        /// <param name="position"> Position of this list.</param>
        /// <param name="lineHeight"> Height per line.</param>
        /// <param name="scale"> Scale.</param>
        public OptionList(List<OptionListItem> items, KeyboardInput keyboard, MonoSprite indicator, Vector2 position, int lineHeight, int scale)
        {
            _items = items;
            _keyboard = keyboard;
            _currentIndex = 0;
            _indicator = indicator;
            _scale = scale;

            // Position the lists relatively to the list's position
            int i = 0;
            items.ForEach((item) =>
            {
                item.Position = new Vector2(position.X * _scale, (position.Y + (lineHeight * i)) * _scale);
                item.Scale = _scale;

                ++i;
            });

            // Move the indicator to the position
            Vector2 indicatorPosition = _indicator.Position;

            int paddingRight = 5;

            indicatorPosition.X = (_items[_currentIndex].Position.X / _scale) - _indicator.Width - paddingRight;
            indicatorPosition.Y = _items[_currentIndex].Position.Y / _scale;

            _indicator.Position = indicatorPosition;
        }

        /// <summary>
        /// Update the selected item.
        /// </summary>
        /// <param name="gameTime"> Game Time.</param>
        public override void Update(GameTime gameTime)
        {
            // Check if down was pressed.
            if (_keyboard.KeyDown(Keys.S))
            {
                // Increase current index and keep it lower than the number of items in the list.
                _currentIndex = Math.Min(_currentIndex + 1, _items.Count - 1);

                MoveIndicator();
            }

            // Check if up was pressed
            if (_keyboard.KeyDown(Keys.W))
            {
                // Decrease current index and keep it higher than zero.
                _currentIndex = Math.Max(_currentIndex - 1, 0);

                MoveIndicator();
            }

            // Activate the item's action
            if (_keyboard.KeyDown(Keys.Enter))
            {
                _items[_currentIndex].OnAction();
            }
        }

        /// <summary>
        /// Move the indicator to the new position.
        /// </summary>
        private void MoveIndicator()
        {
            Vector2 position = _indicator.Position;

            position.Y = _items[_currentIndex].Position.Y / _scale;

            _indicator.Position = position;
        }

        /// <summary>
        /// Render the list.
        /// </summary>
        /// <param name="spriteBatch"> Sprite batch.</param>
        /// <param name="graphics"> Graphics device.</param>
        public override void Render(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            _items.ForEach((item) =>
            {
                item.Render(spriteBatch, graphics);
            });

            _indicator.Render(spriteBatch, graphics);
        }
    }
}
