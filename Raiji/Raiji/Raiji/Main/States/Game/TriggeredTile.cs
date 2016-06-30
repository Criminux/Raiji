using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Raiji.Main.States.Game
{
    

    public class TriggeredTile : Tile
    {
        //TriggeredTile Fields
        private String ID;
        private float timer;
        private float timerValue;
        ContentManager content;
        private bool isTriggered;
        public bool IsTriggered
        {
            set { isTriggered = value; }
            get { return isTriggered; }
        }
        public String GetID
        {
            get
            { return ID; }
        }



        public TriggeredTile(ETile type, Vector2 position, ContentManager content) : base(type, position, content)
        {
            //Save content instance
            this.content = content;
        }

        public void SetProperties(String ID, float timerValue)
        {
            //Save properties from room
            this.ID = ID;
            this.timerValue = timerValue;
        }

        public void Trigger()
        {
            //If tile is triggered, make it passable give it the correct texture
            texture = content.Load<Texture2D>("Tile/Back");
            collision = ETileCollision.Passable;
            //Update flag
            IsTriggered = true;
            //Reset timer
            timer = timerValue;
        }
        public void UnTrigger()
        {
            //Called when timer is done
            //Make it solid with matching texture
            texture = content.Load<Texture2D>("Tile/Stone");
            collision = ETileCollision.Solid;
            //Update flag
            IsTriggered = false;
        }

        public void Update(GameTime gameTime)
        {
            //Update timer
            timer -= gameTime.ElapsedGameTime.Milliseconds;

            //Timer is done and tile is triggered
            if(timer <= 0 && IsTriggered)
            {
                //Untrigger it
                UnTrigger();
            }
        }
          
    }
}
