using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Raiji.Main.States
{
    class Credits : State
    {
        //Credits Texture
        private Texture2D texture;

        public Credits(ContentManager content)
        {
            //Load texture
            texture = content.Load<Texture2D>("CreditsScreen");
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            //Draw Background and Credits Text
            spriteBatch.Draw(texture, Vector2.Zero, Color.White);
        }

        public override EGameState Update(TimeSpan totalTime, GameTime gameTime)
        {
            //Stay in Credits until player pressed escape
            targetState = EGameState.Credits;
            ExecuteInput(Input(StateMachine.inputManager));

            //Return targetState
            return targetState;
        }

        protected override void ExecuteInput(EInputKey[] inputs)
        {
            //Standard Inputhandling
            int size = inputs.Length;
            for (int i = 0; i < size; i++)
            {
                switch (inputs[i])
                {
                    //If Player pressed escape redirect to main menu
                    case EInputKey.Escape:
                        targetState = EGameState.MainMenu;
                        break;
                }
            }
        }
    }
}
