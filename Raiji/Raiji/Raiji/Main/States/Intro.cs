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
using Raiji.Main.States;
using Raiji;

namespace Raiji.Main.States
{
    class Intro : State
    {
        //Intro plays a Video
        private float videoCountdown;
        Video video;
        VideoPlayer videoPlayer;
        Rectangle videoRectangle;
        Texture2D videoTexture;

        public Intro(ContentManager content, Viewport view)
        {
            //Save content instance
            this.content = content;

            //Load content, create instances and reset variables
            video = content.Load<Video>("IntroRaiji");
            videoPlayer = new VideoPlayer();
            videoRectangle = view.Bounds;
            videoCountdown = 21000f;
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            //Draw current frame from video
            spriteBatch.Draw(videoTexture, videoRectangle, Color.White);
        }

        public override EGameState Update(TimeSpan totalTime, GameTime gameTime)
        {
            //Set default value
            targetState = EGameState.Intro;
            //Play Video and get texture
            videoPlayer.Play(video);
            videoTexture = videoPlayer.GetTexture();

            //Countdown
            videoCountdown -= gameTime.ElapsedGameTime.Milliseconds;

            //Execute Input (Skip Intro)
            ExecuteInput(Input(StateMachine.inputManager));

            //If video is over
            if (videoCountdown <= 0)
            {
                //Go to MainMenu
                targetState = EGameState.MainMenu;
            }
            
            return targetState;
            
        }

        protected override void ExecuteInput(EInputKey[] inputs)
        {
            //Standard Input handling
            int size = inputs.Length;
            for (int i = 0; i < size; i++)
            {
                switch (inputs[i])
                {
                    //Skip Intro by pressing escape
                    case EInputKey.Escape:
                        targetState = EGameState.MainMenu;
                        videoPlayer.Stop();
                        break;
                }
            }

        }
    }
}
