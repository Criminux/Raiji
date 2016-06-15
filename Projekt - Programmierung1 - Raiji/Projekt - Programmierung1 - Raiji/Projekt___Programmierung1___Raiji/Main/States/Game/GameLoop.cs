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

        //TODO in den LevelManager
        private int LevelCount;
        
        public GameLoop(ContentManager content) 
        {
            this.content = content;
            inputManager = StateMachine.inputManager;

            levelManager = new LevelManager(inputManager, content);
            LevelCount = 1;

            targetState = EGameState.GameLoop;

        }

        public override EGameState Update(TimeSpan totalTime)
        {
            ExecuteInput(Input(inputManager));

            levelManager.Update();

            return targetState; 
        }
        public override void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            levelManager.Draw(spriteBatch);
        }

        protected override void ExecuteInput(EInputKey[] inputs)
        {
            int size = inputs.Length;
            Vector2 pos = levelManager.playerPosition;

            for (int i = 0; i < size; i++)
            {
                

                switch (inputs[i])
                {
                    case EInputKey.Escape:
                        targetState = EGameState.MainMenu;
                        break;
                    case EInputKey.Right:
                        pos.X += 5;                        
                        break;
                    case EInputKey.Left:
                        pos.X -= 5;
                        break;
                    case EInputKey.Up:
                        pos.Y -= 5;
                        break;
                    case EInputKey.Down:
                        pos.Y += 5;
                        break;
                    default:
                        break;
                }

                
            }
            levelManager.playerPosition = pos;
        }
    }
}
