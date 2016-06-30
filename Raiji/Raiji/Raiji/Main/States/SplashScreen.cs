using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Raiji
{
    class SplashScreen : State
    {
        //Fields
        private Texture2D texture;
        private Vector2 vector;
        private Color color;
        
        public SplashScreen(ContentManager content)
        {
            //Save content instance
            this.content = content;

            //Load content and create instances
            texture = content.Load<Texture2D>("SplashScreen");
            vector = new Vector2(0, 0); //TopLeftCorner
            color = new Color(255,255,255,1); //White

            //Set Standard targetState
            targetState = EGameState.SplashScreen;
        }


        public override EGameState Update(TimeSpan totalTime, GameTime gameTime)
        {
            //Reset targetState
            targetState = EGameState.SplashScreen;

            //After 5 Seconds SplashScreen
            if(totalTime.Seconds >= 5) 
            {
                targetState = EGameState.Intro;
            }

            return targetState;
        }

        //Overide Draw Method draws the SplashScreen
        public override void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            spriteBatch.Draw(texture, vector, color);
        }

        protected override void ExecuteInput(EInputKey[] inputs)
        {
            //Cant be skipped
        }
    }
}
