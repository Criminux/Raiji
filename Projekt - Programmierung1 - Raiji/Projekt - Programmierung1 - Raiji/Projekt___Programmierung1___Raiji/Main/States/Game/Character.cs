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
    abstract class Character
    {
        protected float life;
        protected Texture2D characterSprite;
        public Rectangle bounds;
        
        private const float maxdir = 1f;
        private const float acceleration = 0.5f;
        private const float maxMoveSpeed = 100f;

        //Jump Constants
        protected bool isJumping;
        private float jumpTime;
        private const float maxJumpTime = 200f;
        private const float jumpVelocity = 30f;

        private Vector2 velocity;

        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        virtual public void Update(GameTime gameTime)
        {
            velocity = Vector2.Zero;
            HandleCollisions();            
        }

        //After Update Method needed, for correct Callstack order
        public void AfterUpdate(GameTime gameTime)
        {
            ApplyPhysics(gameTime);
            Position += velocity;
        }

        abstract public void Draw(SpriteBatch spriteBatch);

        //Is Character Alive 
        virtual protected bool isAlive(float life)
        {
            if (life <= 0f) return false;
            else return true;
        }

        public void Move(float dir, GameTime gameTime)
        {
            dir = MathHelper.Clamp(dir, -maxdir, maxdir);

            velocity.X = MathHelper.Clamp(dir * gameTime.ElapsedGameTime.Milliseconds * acceleration, -maxMoveSpeed, maxMoveSpeed);
        }

        public void BeginJump(GameTime gameTime)
        {
            //Start Jump
            if (!isJumping)
            {
                isJumping = true;
                jumpTime = 0f;
                //Play Jumpsound
            }
            
        }

        private void HandleCollisions()
        {

        }

        private void ApplyPhysics(GameTime gameTime)
        {
            //Jumping Velocity
            if(isJumping)
            {
                jumpTime += gameTime.ElapsedGameTime.Milliseconds;
                if (jumpTime <= maxJumpTime)
                {
                    velocity.Y -= jumpVelocity * (1f - (jumpTime / maxJumpTime));
                }
                else { isJumping = false; }
            }

            



        }


        //TODO Attack Method etc etc

    }
}
