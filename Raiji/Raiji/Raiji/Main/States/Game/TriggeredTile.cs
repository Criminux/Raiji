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
            this.content = content;
        }

        public void SetProperties(String ID, float timerValue)
        {
            this.ID = ID;
            this.timerValue = timerValue;
        }

        public void Trigger()
        {
            texture = content.Load<Texture2D>("Tile/Back");
            collision = ETileCollision.Passable;
            IsTriggered = true;
            timer = timerValue;
        }
        public void UnTrigger()
        {
            texture = content.Load<Texture2D>("Tile/Stone");
            collision = ETileCollision.Solid;
            IsTriggered = false;
        }

        public void Update(GameTime gameTime)
        {
            timer -= gameTime.ElapsedGameTime.Milliseconds;

            if(timer <= 0 && IsTriggered)
            {
                UnTrigger();
            }
        }
          
    }
}
