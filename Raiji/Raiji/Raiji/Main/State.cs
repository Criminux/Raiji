using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Raiji
{
    //State Parentclass for all States
    abstract class State
    {

        //Managers
        protected ContentManager content;

        //TargetState
        protected EGameState targetState;

        //Update Method returns next state
        public abstract EGameState Update(TimeSpan totalTime, GameTime gameTime);
        
        //Draw Method draws state
        public abstract void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont);

        protected virtual EInputKey[] Input(InputManager inputManager)
        {
            //Update the KeyboardState
            inputManager.UpdateInput();

            //Get Array from all Inputs
            EInputKey[] inputs = inputManager.GetInput();

            //Save old KeyboardState
            inputManager.EndInput();

            //Return the Array
            return inputs;
        }

        protected abstract void ExecuteInput(EInputKey[] inputs);

    }
}
