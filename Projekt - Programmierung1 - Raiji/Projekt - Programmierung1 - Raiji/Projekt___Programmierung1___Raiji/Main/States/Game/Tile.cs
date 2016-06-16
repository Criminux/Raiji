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

namespace Projekt___Programmierung1___Raiji
{
    public enum ETile
    {
        Background = 0,
        Stone = 1,
        Unspecified = 99

    }

    public class Tile
    {
        private Texture2D texture;
        public Rectangle bounds;
        public ETile ID;

        public Tile(ETile ID, ContentManager content)
        {
            switch(ID)
            {
                case ETile.Stone:
                    texture = content.Load<Texture2D>("Stone");
                    bounds = texture.Bounds;
                    this.ID = ID;
                    break;
                case ETile.Background:
                    texture = content.Load<Texture2D>("Back");
                    bounds = texture.Bounds; break;
                    this.ID = ID;
            }
        }

        public void DrawTile(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

    }
}
