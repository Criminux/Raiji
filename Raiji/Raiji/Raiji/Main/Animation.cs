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
using Raiji.Main.States.Game;

namespace Raiji.Main
{
    public enum EAnimation
    {
        Idle = 0,
        Run = 1,
        Jump = 2,
        Attack = 3,
        Dead = 4
    }
    
    public class Animation
    {

        //Texture
        Texture2D texture;

        //SpriteSheet Stuff
        private int spriteSheetX;
        private int spriteSheetY;

        //SpriteProperties
        private int spriteWidth;
        private int spriteHeight;

        //Rectangle Liste
        Rectangle[] rect;

        //Time Stuff
        private int totalFrames;
        private int currentFrame;
        private TimeSpan frameTime;
        private TimeSpan currentFrameTime;

        public Animation(Texture2D texture, int spriteSheetX, int spriteSheetY, int spriteWidth, int spriteHeight, TimeSpan frameTime)
        {
            currentFrame = 0;

            this.texture = texture;

            this.spriteSheetX = spriteSheetX;
            this.spriteSheetY = spriteSheetY;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;

            this.frameTime = frameTime;
            currentFrameTime = frameTime;

            rect = new Rectangle[spriteSheetX * spriteSheetY];

            Load();
        }



        private void Load()
        {
            totalFrames = 0;
            for (int i = 0; i < spriteSheetY; i++)
            {
                for (int j = 0; j < spriteSheetX; j++)
                {
                    rect[totalFrames] = new Rectangle(j * spriteWidth, i * spriteHeight, spriteWidth, spriteHeight);
                    totalFrames++;
                }
            }
        }



        public void Update(GameTime gameTime)
        {
            currentFrameTime -= gameTime.ElapsedGameTime;
            if (currentFrameTime <= new TimeSpan(0))
            {
                currentFrameTime = frameTime;
                currentFrame++;
                if (currentFrame >= rect.Length)
                {
                    currentFrame = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects effect)
        {
            spriteBatch.Draw(texture, position, rect[currentFrame], Color.White, 0f, new Vector2(0, 0), 0.5f, effect, 0f);
        }


    }
}
