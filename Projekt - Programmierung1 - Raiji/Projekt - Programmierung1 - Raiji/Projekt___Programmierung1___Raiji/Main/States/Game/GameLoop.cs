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
            inputManager = StateMachine.inputManager;

            levelManager = new LevelManager(content);

            targetState = EGameState.GameLoop;

        }

        public override EGameState Update(TimeSpan totalTime)
        {
            targetState = EGameState.GameLoop;
            //ExecuteInput(Input(inputManager));

            targetState = levelManager.Update();

            return targetState; 
        }
        public override void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            levelManager.Draw(spriteBatch);
        }

        protected override void ExecuteInput(EInputKey[] inputs)
        {
            throw new NotImplementedException();
        }
    }
}
