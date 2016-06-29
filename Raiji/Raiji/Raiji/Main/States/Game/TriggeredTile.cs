using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Raiji.Main.States.Game
{

    class TriggeredTile : Tile
    {
        ContentManager content;
        private bool isTriggered;
        public bool IsTriggered
        {
            set { isTriggered = value; }
            get { return isTriggered; }
        }
        private String ID;

        public TriggeredTile(ETile type, Vector2 position, ContentManager content) : base(type, position, content)
        {
        }

        public void SetProperties(String ID)
        {
            this.ID = ID;
        }

        public void Trigger()
        {
            texture = content.Load<Texture2D>("Tile/Back");
            collision = ETileCollision.Passable;
        }
        public void UnTrigger()
        {
            texture = content.Load<Texture2D>("Tile/Stone");
            collision = ETileCollision.Solid;
        }
             
    }
}
