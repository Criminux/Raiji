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
namespace Raiji.Main
{

    public class CollisionManager
    {

        public bool Colliding(Rectangle obj1, Rectangle obj2)
        {
            return (obj1.Intersects(obj2));
        }

    }
}
