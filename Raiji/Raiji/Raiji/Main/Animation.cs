using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Raiji.Main
{
    //Enumeration of all Animation States
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
            //Set current frame to 0
            currentFrame = 0;

            //Save the information
            this.texture = texture;

            this.spriteSheetX = spriteSheetX;
            this.spriteSheetY = spriteSheetY;
            this.spriteWidth = spriteWidth;
            this.spriteHeight = spriteHeight;

            this.frameTime = frameTime;
            currentFrameTime = frameTime;

            //List of all drawable rectangles
            rect = new Rectangle[spriteSheetX * spriteSheetY];

            //Load Animation
            Load();
        }



        private void Load()
        {
            //Reset totalFrames
            totalFrames = 0;

            //Loop though the spriteSheet
            for (int i = 0; i < spriteSheetY; i++)
            {
                for (int j = 0; j < spriteSheetX; j++)
                {
                    //new drawable rectangle is calculated by current iteration and the widht and height
                    rect[totalFrames] = new Rectangle(j * spriteWidth, i * spriteHeight, spriteWidth, spriteHeight);
                    //Increase frames
                    totalFrames++;
                }
            }
        }



        public void Update(GameTime gameTime)
        {
            //Countdown the currentFrameTime
            currentFrameTime -= gameTime.ElapsedGameTime;
            //If countdown done
            if (currentFrameTime <= new TimeSpan(0))
            {
                //reset countdown, add frame
                currentFrameTime = frameTime;
                currentFrame++;
                
                //If all list is done go back to index 0
                if (currentFrame >= rect.Length)
                {
                    currentFrame = 0;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, SpriteEffects effect)
        {
            //Draw the animations current frame at correct position
            spriteBatch.Draw(texture, position, rect[currentFrame], Color.White, 0f, new Vector2(0, 0), 0.5f, effect, 0f);
        }


    }
}
