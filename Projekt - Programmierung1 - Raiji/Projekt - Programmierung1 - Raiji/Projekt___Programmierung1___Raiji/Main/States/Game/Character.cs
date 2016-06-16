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
    abstract class Character
    {
        protected float life;
        protected Texture2D characterSprite;
        public Rectangle bounds; 
        

        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        virtual public void Update()
        {
            
        }
        abstract public void Draw(SpriteBatch spriteBatch);

        //Is Character Alive 
        virtual protected bool isAlive(float life)
        {
            if (life <= 0f) return false;
            else return true;
        }

        //TODO Attack Method etc etc

    }
}
