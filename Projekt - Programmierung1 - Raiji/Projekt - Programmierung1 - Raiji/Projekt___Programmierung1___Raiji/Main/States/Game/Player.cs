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
    class Player : Character
    {
        public Player(ContentManager content)
        {
            characterSprite = content.Load<Texture2D>("Player");
            bounds = characterSprite.Bounds;
        }

        public override void Update()
        {


            if(!isAlive(life))
            {
                //TODO is character dead
            }
            base.Update();
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(characterSprite, Position, Color.White);
        }
    }
}
