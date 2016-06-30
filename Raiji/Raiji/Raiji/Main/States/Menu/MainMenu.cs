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
    class MainMenu : State
    {
        //Buttons for the MainMenu
        private Button startButton;
        private Button exitButton;
        //background of MainMenu
        private Texture2D menuBackground;

        public MainMenu(ContentManager content)
        {
            //Save content instance
            this.content = content;

            //Create Button Instances
            startButton = new Button("Start", content);
            exitButton = new Button("Exit", content);

            //Set the Button Position
            startButton.Position = new Vector2(860, 515);
            exitButton.Position = new Vector2(860, 585);

            //Subscribe Buttons to the Click Events and refer to the specific function
            startButton.Click += startButton_Click;
            exitButton.Click += exitButton_Click;

            //Giving stargetState a default value
            targetState = EGameState.MainMenu;

            //Load Background Texture
            menuBackground = content.Load<Texture2D>("Menu/Menu");
        }

        //ButtonEvents
        private void startButton_Click(object sender, System.EventArgs e)
        {
            //Load GameLoop
            targetState = EGameState.GameLoop;
        }
        private void exitButton_Click(object sender, System.EventArgs e)
        {
            //Quit Game
            targetState = EGameState.Quit; 
        }


        public override EGameState Update(TimeSpan totalTime, GameTime gameTime)
        {
            //Reset targetState
            targetState = EGameState.MainMenu;
            //Execute Input
            ExecuteInput(Input(StateMachine.inputManager));

            //Update Buttons
            startButton.Update();
            exitButton.Update();

            //Return targetState
            return targetState;
        }

        //Draws the MenuButtons
        public override void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            //Draw Background
            spriteBatch.Draw(menuBackground, new Vector2(0, 0), Color.White);

            //Draw Buttons
            startButton.Draw(spriteBatch, spriteFont);
            exitButton.Draw(spriteBatch, spriteFont);
        }

        protected override void ExecuteInput(EInputKey[] inputs)
        {
            //Standard Input handling
            int size = inputs.Length;
            for (int i = 0; i < size; i++)
            {
                switch (inputs[i])
                {
                    //If User pressed Escape Quit game
                    case EInputKey.Escape:
                        targetState = EGameState.Quit;
                        break;
                }
            }
        }
    }
}
