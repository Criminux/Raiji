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

namespace Raiji
{
    abstract class State
    {

        //Managers
        protected ContentManager content;

        //TargetState
        protected EGameState targetState;

        public abstract EGameState Update(TimeSpan totalTime, GameTime gameTime);
        
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
