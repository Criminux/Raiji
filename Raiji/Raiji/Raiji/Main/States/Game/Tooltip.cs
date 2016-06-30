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
using Raiji.Main.States.Game;
using Raiji.Main;

namespace Raiji.Main.States.Game
{
    class Tooltip
    {
        //Tooltip contains position and message
        private Vector2 position;
        private String message;

        public Tooltip(Vector2 position, String message)
        {
            //Save infomation from tooltip
            this.position = position;
            this.message = message;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            //Draw tooltip
            spriteBatch.DrawString(spriteFont, message, position, Color.White);
        }

    }
}
