using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Raiji
{
    class GameLoop : State
    {
        //Creates a LevelManager, Song etc.
        LevelManager levelManager;
        Song backgroundSong;
        bool isSongPlaying;
        
        
        public GameLoop(ContentManager content) 
        {
            this.content = content;

            //Create instances, load content and reset variables
            levelManager = new LevelManager(content);
            backgroundSong = content.Load<Song>("Music/StartOver");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.15f;
            isSongPlaying = false;

            //Set default targetState
            targetState = EGameState.GameLoop;

        }

        public override EGameState Update(TimeSpan totalTime, GameTime gameTime)
        {
            //When player won
            if (levelManager.GameOver || levelManager.LevelDone)
            {
                levelManager = new LevelManager(content);
            }

            //Set defautl targetState
            targetState = EGameState.GameLoop;

            //Execute Input 
            ExecuteInput(Input(StateMachine.inputManager));

            //Play music if its not playing
            if(!isSongPlaying)
            {
                MediaPlayer.Play(backgroundSong);
                isSongPlaying = true;
            }

            //Update the LevelManager
            levelManager.Update(gameTime);

            //Is level done redirect to Credits
            if(levelManager.LevelDone)
            {
                MediaPlayer.Stop();
                isSongPlaying = false;
                targetState = EGameState.Credits;
            }

            //Is player dead redirect to MainMenu
            if(levelManager.GameOver)
            {
                MediaPlayer.Stop();
                isSongPlaying = false;
                targetState = EGameState.MainMenu; 
            }

            //Return the targetState
            return targetState; 
        }
        public override void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            //Draw the LevelManager
            levelManager.Draw(spriteBatch, spriteFont);
        }

        protected override void ExecuteInput(EInputKey[] inputs)
        {
            //Std Input procedure
            int size = inputs.Length;
            
            //Loop through inputs
            for (int i = 0; i < size; i++)
            {
                switch (inputs[i])
                {
                    //If player hits escape, redirect to MainMenu
                    case EInputKey.Escape:
                        targetState = EGameState.MainMenu;
                        break;
                }
            }
        }
    }
}
