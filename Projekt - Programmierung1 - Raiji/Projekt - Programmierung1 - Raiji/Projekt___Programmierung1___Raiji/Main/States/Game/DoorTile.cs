using Projekt___Programmierung1___Raiji;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Raiji.Main.States.Game
{
    class DoorTile : Tile
    {
        public DoorTile(ETile type, Vector2 position, ContentManager content) : base(type, position, content)
        {
            this.type = ETile.Door;
            this.position = position;
            texture = content.Load<Texture2D>("Back");
            collision = ETileCollision.Passable;
            bounds = texture.Bounds;
            bounds.Location = new Point((int)position.X, (int)position.Y);
        }
    }
}
