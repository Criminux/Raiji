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
using Raiji.Main;

namespace Projekt___Programmierung1___Raiji
{
    class Player : Character
    {
        public Player(ContentManager content)
        {
            //Load Sprites for Animation
            runSpriteSheet = content.Load<Texture2D>("RunSheet");
            idleSpriteSheet = content.Load<Texture2D>("IdleSheet");
            characterSprite = content.Load<Texture2D>("PlayerOutline");

            //Create Animation
            idleAnimation = new Animation(idleSpriteSheet, 5, 2, 232, 439, new TimeSpan(0, 0, 0, 0, 100));
            runAnimation = new Animation(runSpriteSheet, 5, 2, 363, 458, new TimeSpan(0, 0, 0, 0, 100));

            currentAnimationState = EAnimation.Idle;

            bounds = characterSprite.Bounds;
        }

        public override void Update(GameTime gameTime, Room room)
        {
            

            if(!isAlive(life))
            {
                //TODO is character dead
            }
            base.Update(gameTime, room);
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);  
        }
    }
}
