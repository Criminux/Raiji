using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
