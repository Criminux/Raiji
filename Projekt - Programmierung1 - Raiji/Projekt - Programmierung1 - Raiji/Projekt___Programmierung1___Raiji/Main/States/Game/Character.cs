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
        
        //Movement Fields
        private const float maxdir = 1f;
        private const float acceleration = 0.5f;
        private const float maxMoveSpeed = 100f;

        //Jump and Collision Fields
        protected bool isJumping;
        protected bool isOnGround;
        private float jumpTime;
        private const float maxJumpTime = 200f;
        private const float jumpVelocity = 30f;

        //Collects all Movement
        private Vector2 velocity;

        private Vector2 previousPosition;

        //Is finally influenced by the velocity
        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        virtual public void Update(GameTime gameTime, Room room)
        {
            
            bounds.Location = new Point((int)Position.X, (int)Position.Y);
            velocity = Vector2.Zero;
            
        }

        //After Update Method needed, for correct Callstack order
        public void AfterUpdate(GameTime gameTime, Room room)
        {
            ApplyPhysics(gameTime, room);
            Position += velocity;
        }

        abstract public void Draw(SpriteBatch spriteBatch);

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

            velocity.X = MathHelper.Clamp(dir * gameTime.ElapsedGameTime.Milliseconds * acceleration, -maxMoveSpeed, maxMoveSpeed);
        }

        //Begins Jump - toggled by Space
        public void BeginJump(GameTime gameTime)
        {
            //Start Jump
            if (!isJumping)
            {
                isJumping = true;
                jumpTime = 0f;
                //TODO: Play Jumpsound
            }
            
        }


        //Handles Collisions
        private void HandleCollisions(Room room)
        {
            //Save current Room
            Tile[,] TileRoom = room.tileRoom;

            //Get potential colliding tiles
            int leftTile = (int)Math.Floor((float)bounds.Left / Tile.Width) - 1;
            int rightTile = (int)Math.Ceiling(((float)bounds.Right / Tile.Width)) ;
            int topTile = (int)Math.Floor((float)bounds.Top / Tile.Height) - 1;
            int bottomTile = (int)Math.Ceiling(((float)bounds.Bottom / Tile.Height));

            //Loop though those Tiles and check for Collision
            for (int i = topTile; i <= bottomTile; i++)
            {
                for (int j = leftTile; j <= rightTile; j++)
                {
                    //Index out of Bound Exception
                    if (i < 15 && i > 0 && j < 29 && j > 0)
                    {
                        //Get the Collision Type of Tile
                        ETileCollision currentTileCollision = TileRoom[j, i].Collision;

                        //Is the Tile collidable?
                        if (currentTileCollision == ETileCollision.Solid)
                        {
                            
                            //Fix the position - if no collision at all, depth will be Zero
                            Vector2 depth = CollisionUtil.CollisionDepth(bounds, TileRoom[j, i].Bounds);
                            
                            
                            // Collision along y
                            if (Math.Abs(depth.Y) < Math.Abs(depth.X))
                            {

                                position.Y += depth.Y;
                                
                            }
                            // Collision along x
                            else
                            {
                                
                                position.X += depth.X;
                                

                            }
                        }
                    }                   
                }
            }
        }


        //Moves the Player on Y-Axis
        private void ApplyPhysics(GameTime gameTime, Room room)
        {
            

            //Jumping Velocity
            if (isJumping)
            {
                //Add time from last Update to jumpTime
                jumpTime += gameTime.ElapsedGameTime.Milliseconds;
                //If still jumping
                if (jumpTime <= maxJumpTime)
                {
                    //Add velocity (which decreases with time)
                    velocity.Y -= jumpVelocity * (1f - (jumpTime / maxJumpTime));
                }
                else
                {
                    //If time ran out and Player on ground: give back jump ability
                    if(isOnGround)
                    {
                        isJumping = false;
                    }
                }
            }
            else //If not Jumping, not on ground: Apply gravity
            {
                if(!isOnGround)
                {
                    velocity.Y += 10f;
                }      
            }
            
            
            HandleCollisions(room);
        }

       
        //TODO Attack Method etc etc

    }
}
