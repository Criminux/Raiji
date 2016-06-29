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
        Song backgroundSong;
        bool isSongPlaying;
        
        
        public GameLoop(ContentManager content) 
        {
            this.content = content;

            levelManager = new LevelManager(content);
            backgroundSong = content.Load<Song>("Music/StartOver");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.15f;
            isSongPlaying = false;

            targetState = EGameState.GameLoop;

        }

        public override EGameState Update(TimeSpan totalTime, GameTime gameTime)
        {
            if (levelManager.GameOver)
            {
                levelManager = new LevelManager(content);
            }
            targetState = EGameState.GameLoop;
            ExecuteInput(Input(StateMachine.inputManager));
            if(!isSongPlaying)
            {
                MediaPlayer.Play(backgroundSong);
                isSongPlaying = true;
            }
            levelManager.Update(gameTime);
            if(levelManager.GameOver)
            {
                MediaPlayer.Stop();
                isSongPlaying = false;
                targetState = EGameState.MainMenu; 
            }


            return targetState; 
        }
        public override void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            levelManager.Draw(spriteBatch, spriteFont);
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
