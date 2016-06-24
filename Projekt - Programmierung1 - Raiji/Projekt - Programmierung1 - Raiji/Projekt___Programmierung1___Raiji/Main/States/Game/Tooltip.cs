using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Projekt___Programmierung1___Raiji.Main.States.Game;
using Raiji.Main;
using Raiji.Main.States.Game;

namespace Raiji.Main.States.Game
{
    class Tooltip
    {
        private Vector2 position;
        private String message;

        public Tooltip(Vector2 position, String message)
        {
            this.position = position;
            this.message = message;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            spriteBatch.DrawString(spriteFont, message, position, Color.White);
        }

    }
}
