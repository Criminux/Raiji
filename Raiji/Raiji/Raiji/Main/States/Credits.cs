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
        private Texture2D texture;

        public Credits(ContentManager content)
        {
            texture = content.Load<Texture2D>("CreditsScreen");
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            spriteBatch.Draw(texture, Vector2.Zero, Color.White);
            spriteBatch.DrawString(spriteFont, "You finished the Demo of Raiji!", new Vector2(100, 100), Color.White);
        }

        public override EGameState Update(TimeSpan totalTime, GameTime gameTime)
        {
            targetState = EGameState.Credits;
            ExecuteInput(Input(StateMachine.inputManager));

            return targetState;
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
