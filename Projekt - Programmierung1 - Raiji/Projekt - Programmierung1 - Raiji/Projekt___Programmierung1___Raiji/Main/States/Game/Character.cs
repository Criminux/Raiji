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
using Projekt___Programmierung1___Raiji.Main.States.Game;
using Raiji.Main;

namespace Projekt___Programmierung1___Raiji
{
    abstract class Character
    {
        //General Character Fields
        protected float life;
        protected Texture2D characterSprite;
        public Rectangle bounds;

        //Animations
        protected EAnimation currentAnimationState;

        protected Animation idleAnimation;
        protected Animation runAnimation;
        protected Animation jumpAnimation;
        protected Animation attackAnimation;
        protected SpriteEffects animationDirection;

        protected Texture2D idleSpriteSheet;
        protected Texture2D runSpriteSheet;
        protected Texture2D jumpSpriteSheet;
        protected Texture2D attackSpriteSheet;

        //Movement Fields
        private const float maxdir = 1f;
        private const float acceleration = 0.5f;
        private const float maxMoveSpeed = 100f;
        private const float gravity = 10f;

        //Jump and Collision Fields
        protected bool isOnGround;
        private float jumpTime = jumpCooldown;
        private const float maxJumpTime = 500f;
        private const float jumpCooldown = 1200f;
        private const float jumpVelocity = 40f;

        //Collects all Movement
        private Vector2 velocity;

        //Is finally influenced by the velocity
        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        private bool IsJumping
        {
            get { return jumpTime <= maxJumpTime; }
        }
        private bool JumpHasCooledDown
        {
            get { return jumpTime <= jumpCooldown; }
        }

        virtual public void Update(GameTime gameTime, Room room)
        {
            //Update all Animations
            idleAnimation.Update(gameTime);
            runAnimation.Update(gameTime);
            jumpAnimation.Update(gameTime);
            attackAnimation.Update(gameTime);

            // Reset variables for this cycle
            currentAnimationState = EAnimation.Idle;
            velocity = Vector2.Zero;
            bounds.Location = new Point((int)Position.X, (int)Position.Y);
        }

        // Is called after retrieving the input
        public void AfterUpdate(GameTime gameTime, Room room)
        {
            ApplyPhysics(gameTime, room);
            position += velocity;
            HandleCollisions(room);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            switch(currentAnimationState)
            {
                case EAnimation.Idle:
                    idleAnimation.Draw(spriteBatch, position, animationDirection);
                    break;
                case EAnimation.Run:
                    runAnimation.Draw(spriteBatch, position, animationDirection);
                    break;
                case EAnimation.Jump:
                    jumpAnimation.Draw(spriteBatch, position, animationDirection);
                    break;
                case EAnimation.Attack:
                    attackAnimation.Draw(spriteBatch, position, animationDirection);
                    break;
            }
        }

        //Is Character Alive 
        virtual protected bool isAlive(float life)
        {
            if (life <= 0f) return false;
            else return true;
        }

        //Moves the Player on X-Axis
        public void Move(float dir, GameTime gameTime)
        {
            dir = MathHelper.Clamp(dir, -maxdir, maxdir);

            //Set the Animation Direction
            if (dir >= 0) animationDirection = SpriteEffects.None;
            else animationDirection = SpriteEffects.FlipHorizontally;

            //If Move is called: Set State to Run
            currentAnimationState = EAnimation.Run;

            velocity.X = MathHelper.Clamp(dir * gameTime.ElapsedGameTime.Milliseconds * acceleration, -maxMoveSpeed, maxMoveSpeed);
        }

        //Begins Jump - toggled by Space
        public void BeginJump(GameTime gameTime)
        {
            //Start Jump
            if (!JumpHasCooledDown)
            {
                jumpTime = 0f;
                //TODO: Play Jumpsound
            }

        }


        //Handles Collisions
        private void HandleCollisions(Room room)
        {
            // Set up some stuff
            Tile[,] TileRoom = room.tileRoom;
            bounds.Location = new Point((int)position.X, (int)position.Y);

            int playerIndexX = (int)Math.Floor(bounds.Center.X / (float)Tile.Width);
            int playerIndexY = (int)Math.Floor(bounds.Center.Y / (float)Tile.Height);

            for(int x = playerIndexX -1;  x <= playerIndexX +1; x++)
            {
                if(x >= 0 && x < TileRoom.GetLength(0) && playerIndexY >= 0 && playerIndexY < TileRoom.GetLength(1))
                {
                    Tile currentTile = TileRoom[x, playerIndexY];
                    if (currentTile.Collision == ETileCollision.Solid)
                    {
                        position.X += CollisionUtil.CalculateCollisionDepth(bounds, currentTile.Bounds).X;
                        bounds.Location = new Point((int)position.X, (int)position.Y); // TODO: Update Bounds Funktion
                    }
                }               
            }
            
            for(int y = playerIndexY -1; y <= playerIndexY +1; y++)
            {
                if(y >= 0 && y < TileRoom.GetLength(1) && playerIndexX >= 0 && playerIndexX < TileRoom.GetLength(0))
                {                 
                    Tile currentTile = TileRoom[playerIndexX, y];
                    if(currentTile.Collision == ETileCollision.Solid)
                    {
                        position.Y += CollisionUtil.CalculateCollisionDepth(bounds, currentTile.Bounds).Y;
                        bounds.Location = new Point((int)position.X, (int)position.Y);
                    }
                }                
            }
            
        }


        //Moves the Player on Y-Axis
        private void ApplyPhysics(GameTime gameTime, Room room)
        {
            //Add gravity
            velocity.Y += gravity;
            //Add time from last Update to jumpTime
            jumpTime += gameTime.ElapsedGameTime.Milliseconds;
            
            if (IsJumping)
            {
                //Add velocity (which decreases with time)
                velocity.Y -= jumpVelocity * (1f - (jumpTime / maxJumpTime));
                currentAnimationState = EAnimation.Jump;
            }

        }

        


        public void Attack()
        {
            currentAnimationState = EAnimation.Attack;
        }

    }
}
