using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Raiji.Main.States.Game
{
    //Enmeration of all ItemTypes
    public enum EItem
    {
        Diamond = 0,
        Key = 11
    }

    public class Item
    {
        //Item need position, texture, bounds and type
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
            //Load correct texture for item
            switch(type)
            {
                case EItem.Diamond:
                    texture = content.Load<Texture2D>("Item/Diamond");
                    break;
                case EItem.Key:
                    texture = content.Load<Texture2D>("Item/Key");
                    break;
            }

            //save properties and set rectangle
            this.type = type;
            this.position = position;
            bounds = new Rectangle((int)position.X, (int)position.Y, 64, 64);
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw item to its position
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
