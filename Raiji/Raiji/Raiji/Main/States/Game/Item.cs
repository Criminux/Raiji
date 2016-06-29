using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Raiji.Main.States.Game
{

    public enum EItem
    {
        Diamond = 0,
        Key = 11
    }

    public class Item
    {
        private Vector2 position;
        private Texture2D texture;
        private Rectangle bounds;
        private EItem type;

        public EItem Type
        {
            get { return type; }
        }
        public Rectangle Bounds
        {
            get { return bounds; }
        }


        public Item(EItem type, Vector2 position, ContentManager content)
        {
            switch(type)
            {
                case EItem.Diamond:
                    texture = content.Load<Texture2D>("Item/Diamond");
                    break;
                case EItem.Key:
                    texture = content.Load<Texture2D>("Item/Key");
                    break;
            }

            this.type = type;
            this.position = position;
            bounds = new Rectangle((int)position.X, (int)position.Y, 64, 64);
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
