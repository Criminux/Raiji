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
    class GameLoop : State
    {
        //Creates a LevelManager
        LevelManager levelManager;

        
        
        public GameLoop(ContentManager content) 
        {
            this.content = content;

            levelManager = new LevelManager(content);

            targetState = EGameState.GameLoop;

        }

        public override EGameState Update(TimeSpan totalTime, GameTime gameTime)
        {
            targetState = EGameState.GameLoop;
            ExecuteInput(Input(StateMachine.inputManager));

            levelManager.Update(gameTime);

            return targetState; 
        }
        public override void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            levelManager.Draw(spriteBatch);
        }

        protected override void ExecuteInput(EInputKey[] inputs)
        {
            int size = inputs.Length;


            for (int i = 0; i < size; i++)
            {


                switch (inputs[i])
                {
                    case EInputKey.Escape:
                        targetState = EGameState.MainMenu;
                        break;
                    default:
                        break;
                }


            }

        }
    }
}
