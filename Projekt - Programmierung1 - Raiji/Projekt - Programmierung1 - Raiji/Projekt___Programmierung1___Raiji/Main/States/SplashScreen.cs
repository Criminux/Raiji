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
    class SplashScreen : State
    {
        //Fields
        private Texture2D texture;
        private Vector2 vector;
        private Color color;
        
        //Constructor - Content to Load the SplashScreen
        public SplashScreen(ContentManager content)
        {
            //TODO
            this.content = content;
            inputManager = StateMachine.inputManager;

            //Giving the fields values
            texture = content.Load<Texture2D>("SplashScreen");
            vector = new Vector2(0, 0); //TopLeftCorner
            color = new Color(255,255,255,1); //White

            //Set Standard targetState
            targetState = EGameState.SplashScreen;
        }


        //Override Update Method showing the SplashScreen 5 Seconds
        public override EGameState Update(TimeSpan totalTime)
        {
            //Process the Input
            ExecuteInput(Input(inputManager));
            
            //After 5 Seconds SplashScreen
            if(totalTime.Seconds >= 5) 
            {
                targetState = EGameState.MainMenu;
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
            int size = inputs.Length;
            for (int i = 0; i < size; i++)
            {
                switch (inputs[i])
                {
                    case EInputKey.Escape:
                        targetState = EGameState.Quit;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
