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

namespace Projekt___Programmierung1___Raiji
{
    class Player : Character
    {
        public Player(ContentManager content)
        {
            characterSprite = content.Load<Texture2D>("Player");
            bounds = characterSprite.Bounds;
            isJumping = false;
        }

        public override void Update(GameTime gameTime, Room room)
        {


            if(!isAlive(life))
            {
                //TODO is character dead
            }
            base.Update(gameTime, room);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(characterSprite, Position, Color.White);
        }
    }
}
