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
using Projekt___Programmierung1___Raiji;

namespace Raiji.Main.States
{
    class Intro : State
    {
        private float videoCountdown;
        Video video;
        VideoPlayer videoPlayer;
        Rectangle videoRectangle;
        Texture2D videoTexture;

        EGameState targetState;

        public Intro(ContentManager content, Viewport view)
        {
            this.content = content;

            video = content.Load<Video>("IntroRaiji");
            videoPlayer = new VideoPlayer();
            videoRectangle = view.Bounds;

            videoCountdown = 21000f;
        }

        public override void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            spriteBatch.Draw(videoTexture, videoRectangle, Color.White);
        }

        public override EGameState Update(TimeSpan totalTime, GameTime gameTime)
        {
            targetState = EGameState.Intro;
            videoPlayer.Play(video);
            videoTexture = videoPlayer.GetTexture();

            videoCountdown -= gameTime.ElapsedGameTime.Milliseconds;

            ExecuteInput(Input(StateMachine.inputManager));


            if (videoCountdown <= 0)
            {

                targetState = EGameState.MainMenu;
            }
            
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
                        videoPlayer.Stop();
                        break;
                    default:
                        break;
                }
            }

        }
    }
}
