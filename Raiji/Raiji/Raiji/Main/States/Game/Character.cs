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
using Raiji.Main.States.Game;
using Raiji.Main;

namespace Raiji
{
    public abstract class Character
    {
        //General Character Fields
        public int life;
        protected float lifeCooldown;
        protected float hitCooldown;
        protected float clickCooldown = 300f;
        protected Texture2D characterSprite;
        public Rectangle bounds;
        bool click;
        protected bool canAttack;
        private bool gameOver;
        public bool GameOver
        {
            get { return gameOver; }
        }
        protected float deadCooldown;

        public bool Click
        {
            get
            {
                if(clickCooldown <= 0)
                { 
                    clickCooldown = 300f;
                    return click;   
                }
                return false;
            }
            set { click = value; }
        }

        //Animations
        protected EAnimation currentAnimationState;

        protected Animation idleAnimation;
        protected Animation runAnimation;
        protected Animation jumpAnimation;
        protected Animation attackAnimation;
        protected Animation deadAnimation;
        protected SpriteEffects animationDirection;

        protected Texture2D idleSpriteSheet;
        protected Texture2D runSpriteSheet;
        protected Texture2D jumpSpriteSheet;
        protected Texture2D attackSpriteSheet;
        protected Texture2D deadSpriteSheet;

        //Sounds
        protected SoundEffect attackSound;
        protected SoundEffect jumpSound;
        protected SoundEffect damageSound;
        protected SoundEffect stepSound;
        protected float stepCooldown;


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

        //Attack Fields
        protected float attackCooldown;

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

        public int Life
        {
            get { return life; }
            set { life = value; }
        }


        virtual public void Update(GameTime gameTime, Room room)
        {
            

            //Update all Animations
            idleAnimation.Update(gameTime);
            runAnimation.Update(gameTime);
            jumpAnimation.Update(gameTime);
            attackAnimation.Update(gameTime);
            
            attackCooldown -= gameTime.ElapsedGameTime.Milliseconds;
            stepCooldown -= gameTime.ElapsedGameTime.Milliseconds;
            clickCooldown -= gameTime.ElapsedGameTime.Milliseconds;

            if (attackCooldown >= 0)
            {
                currentAnimationState = EAnimation.Attack;

            }
            else { currentAnimationState = EAnimation.Idle; }

            // Reset variables for this cycle
            canAttack = false;
            click = false;
            velocity = Vector2.Zero;
            bounds.Location = new Point((int)Position.X, (int)Position.Y);
        }

        // Is called after retrieving the input
        public void AfterUpdate(GameTime gameTime, Room room, LevelManager level, ContentManager content, List<Enemy> enemies)
        {
            ApplyPhysics(gameTime, room);
            position += velocity;
            HandleCollisions(room, level, gameTime, content);

            HandleLife(gameTime, room, level, enemies);

            if (life <= 0)
            {
                currentAnimationState = EAnimation.Dead;
                deadAnimation.Update(gameTime);

                deadCooldown -= gameTime.ElapsedGameTime.Milliseconds;
                if(deadCooldown <= 0)
                {
                    gameOver = true;
                }
            }
        }

        virtual public void Draw(SpriteBatch spriteBatch)
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
                case EAnimation.Dead:
                    deadAnimation.Draw(spriteBatch, position, animationDirection);
                    break;
            }
        }

       
        //Moves the Player on X-Axis
        public void Move(float dir, GameTime gameTime)
        {
            if(life > 0)
            {

                dir = MathHelper.Clamp(dir, -maxdir, maxdir);

                //Set the Animation Direction
                if (dir >= 0) animationDirection = SpriteEffects.None;
                else animationDirection = SpriteEffects.FlipHorizontally;

                if (stepCooldown <= 0 && this is Player)
                {
                    stepSound.Play(0.5f, 0, 0);
                    stepCooldown = 475f;
                }

                //If Move is called: Set State to Run
                currentAnimationState = EAnimation.Run;
                
                if(this is Enemy) velocity.X = MathHelper.Clamp(dir * gameTime.ElapsedGameTime.Milliseconds * acceleration / 2, -maxMoveSpeed , maxMoveSpeed );
                else velocity.X = MathHelper.Clamp(dir * gameTime.ElapsedGameTime.Milliseconds * acceleration, -maxMoveSpeed, maxMoveSpeed);

            }

        }

        //Begins Jump - triggered by Space
        public void BeginJump(GameTime gameTime)
        {
            if(life > 0)
            {
                //Start Jump
                if (!JumpHasCooledDown)
                {
                    jumpTime = 0f;
                    jumpSound.Play(0.7f, 0, 0);
                }
            }
            
        }

        
        private void HandleCollisions(Room room, LevelManager level, GameTime gameTime, ContentManager content)
        {
            // Get the current TileRoom
            Tile[,] TileRoom = room.tileRoom;
            // Set the Location of the players Rectangle to current position
            bounds.Location = new Point((int)position.X, (int)position.Y);// TODO: Update Bounds Funktion

            // Get the Index of Tiles, the player is standing before
            int playerIndexX = (int)Math.Floor(bounds.Center.X / (float)Tile.Width);
            int playerIndexY = (int)Math.Floor(bounds.Center.Y / (float)Tile.Height);

            

            //Loop through the potential collidable Tiles on X-Axis
            for(int x = playerIndexX -1;  x <= playerIndexX +1; x++)
            {
                //Avoid OutOfRange Exception
                if(x >= 0 && x < TileRoom.GetLength(0) && playerIndexY >= 0 && playerIndexY < TileRoom.GetLength(1))
                {
                    //Get the Tile which will now be checked
                    Tile currentTile = TileRoom[x, playerIndexY];

                    Vector2 collisionDepth = CollisionUtil.CalculateCollisionDepth(bounds, currentTile.Bounds);
                    if(collisionDepth != Vector2.Zero)
                    {
                        OnTileCollision(currentTile, collisionDepth, level);

                        if (currentTile.Collision == ETileCollision.Solid)
                        {
                            //In case the Tile is solid, get the CollisionDepth from the CollisionUtil and apply it to position.X
                            position.X += CollisionUtil.CalculateCollisionDepth(bounds, currentTile.Bounds).X;
                            //Update the Rectangles position
                            bounds.Location = new Point((int)position.X, (int)position.Y); // TODO: Update Bounds Funktion
                        }
                    }

                    
                    
                }               
            }
            
            //Loop though the other potential collidable Tiles on Y-Axis
            for(int y = playerIndexY -1; y <= playerIndexY +1; y++)
            {
                //Avoid OutOfRange Exception
                if (y >= 0 && y < TileRoom.GetLength(1) && playerIndexX >= 0 && playerIndexX < TileRoom.GetLength(0))
                {
                    //Get the Tile which will now be checked              
                    Tile currentTile = TileRoom[playerIndexX, y];

                    Vector2 collisionDepth = CollisionUtil.CalculateCollisionDepth(bounds, currentTile.Bounds);
                    if (collisionDepth != Vector2.Zero)
                    {
                        OnTileCollision(currentTile, collisionDepth, level);

                        if (currentTile.Collision == ETileCollision.Solid)
                        {
                            //In case the Tile is solid, get the CollisionDepth from the CollisionUtil and apply it to position.Y
                            position.Y += CollisionUtil.CalculateCollisionDepth(bounds, currentTile.Bounds).Y;
                            //Update the Rectangles position
                            bounds.Location = new Point((int)position.X, (int)position.Y); // TODO: Update Bounds Funktion
                        }
                    }
                    
                }                
            }

            
        }

        protected abstract void HandleLife(GameTime gameTime, Room room, LevelManager level, List<Enemy> enemies);

        protected abstract void OnTileCollision(Tile collidingTile, Vector2 collisionDepth, LevelManager level);
       


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
                //Start Jump Animation
                currentAnimationState = EAnimation.Jump;
            }

        }

        


        public void Attack(GameTime gameTime, Room room)
        {

            if(attackCooldown <= 0)
            {
                //Start Attack Animation
                currentAnimationState = EAnimation.Attack;
                
                //Reset Cooldown
                attackCooldown = 500f;

                if (this is Player) attackSound.Play();

                float tempDistance = room.GetCloseEnemyDistance();
                if(tempDistance <= 100f)
                {
                    canAttack = true;
                }

            }
            

        }

    }
}