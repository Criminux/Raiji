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
using Raiji.Main.States.Game;

namespace Projekt___Programmierung1___Raiji
{
    abstract class Character
    {
        //General Character Fields
        public int life;
        protected float lifeCooldown;
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

        public int Life
        {
            get { return life; }
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
        public void AfterUpdate(GameTime gameTime, Room room, LevelManager level)
        {
            ApplyPhysics(gameTime, room);
            position += velocity;
            HandleCollisions(room, level, gameTime);
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
            }
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

        //Begins Jump - triggered by Space
        public void BeginJump(GameTime gameTime)
        {
            //Start Jump
            if (!JumpHasCooledDown)
            {
                jumpTime = 0f;
                //TODO: Play Jumpsound
            }

        }

        
        private void HandleCollisions(Room room, LevelManager level, GameTime gameTime)
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

                    //Is the Tile solid?
                    if (currentTile.Collision == ETileCollision.Solid)
                    {
                        //In case the Tile is solid, get the CollisionDepth from the CollisionUtil and apply it to position.X
                        position.X += CollisionUtil.CalculateCollisionDepth(bounds, currentTile.Bounds).X;
                        //Update the Rectangles position
                        bounds.Location = new Point((int)position.X, (int)position.Y); // TODO: Update Bounds Funktion
                    }
                    //Is the Tile a Door?
                    else if(currentTile is DoorTile)
                    {
                        //Is there a CollisionDepth?
                        float tempDepth = CollisionUtil.CalculateCollisionDepth(bounds, currentTile.Bounds).X;
                        //If Depth is not 0
                        if(tempDepth != 0f)
                        {
                            level.ActiveRoom = ((DoorTile)currentTile).TargetRoom;
                            //TODO: Position not correct
                            position = new Vector2(300, 500);
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
                    if(currentTile.Collision == ETileCollision.Solid)
                    {
                        //In case the Tile is solid, get the CollisionDepth from the CollisionUtil and apply it to position.Y
                        position.Y += CollisionUtil.CalculateCollisionDepth(bounds, currentTile.Bounds).Y;
                        //Update the Rectangles position
                        bounds.Location = new Point((int)position.X, (int)position.Y); // TODO: Update Bounds Funktion
                    }
                    //Is the Tile a Door?
                    else if (currentTile is DoorTile)
                    {
                        //Is there a CollisionDepth?
                        float tempDepth = CollisionUtil.CalculateCollisionDepth(bounds, currentTile.Bounds).Y;
                        //If Depth is not 0
                        if (tempDepth != 0f)
                        {
                            level.ActiveRoom = ((DoorTile)currentTile).TargetRoom;
                            //TODO: Position not correct
                            position = new Vector2(300, 500);
                        }
                    }
                }                
            }

            lifeCooldown -= gameTime.ElapsedGameTime.Milliseconds;

            //Intersect with Enemy and is Attacking
            if(bounds.Intersects(room.EnemyBounds) && currentAnimationState == EAnimation.Attack)
            {
                room.EnemyLife = room.EnemyLife -= 1;
            }      
            else if(this is Player && bounds.Intersects(room.EnemyBounds))
            {
                if(lifeCooldown <= 0)
                {
                    life -= 1;
                    lifeCooldown = 500f;
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
                //Start Jump Animation
                currentAnimationState = EAnimation.Jump;
            }

        }

        


        public void Attack()
        {
            //Start Attack Animation
            currentAnimationState = EAnimation.Attack;


        }

    }
}
