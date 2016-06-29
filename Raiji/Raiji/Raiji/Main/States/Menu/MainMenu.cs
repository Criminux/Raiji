﻿using System;
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
        //TODO add all Buttons
        private Button startButton;
        private Button exitButton;
        private Texture2D menuBackground;

        public MainMenu(ContentManager content)
        {
            //Set the instances
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

            menuBackground = content.Load<Texture2D>("Menu/Menu");

            //TODO MediaPlayer Play
        }

        //ButtonEvents
        private void startButton_Click(object sender, System.EventArgs e)
        {
            targetState = EGameState.GameLoop;
        }
        private void exitButton_Click(object sender, System.EventArgs e)
        {
            targetState = EGameState.Quit; 
        }


        public override EGameState Update(TimeSpan totalTime, GameTime gameTime)
        {
            targetState = EGameState.MainMenu;
            ExecuteInput(Input(StateMachine.inputManager));

            startButton.Update();
            exitButton.Update();
            return targetState;
        }

        //Draws the MenuButtons
        public override void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            spriteBatch.Draw(menuBackground, new Vector2(0, 0), Color.White);

            startButton.Draw(spriteBatch, spriteFont);
            exitButton.Draw(spriteBatch, spriteFont);
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