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
        private const float gravity = 10f;

        //Jump and Collision Fields
        protected bool isJumping;
        protected bool isOnGround;
        private float jumpTime;
        private const float maxJumpTime = 200f;
        private const float jumpVelocity = 30f;

        //Collects all Movement
        private Vector2 velocity;

        //Is finally influenced by the velocity
        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }


        virtual public void Update(GameTime gameTime, Room room)
        {
            // Reset variables for this cycle
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
            // Set up some stuff
            Tile[,] TileRoom = room.tileRoom;
            Rectangle virtualBounds = bounds; virtualBounds.Location = new Point((int)position.X, (int)position.Y);

            int playerIndexX = (int)Math.Floor(virtualBounds.Center.X / (float)Tile.Width);
            int playerIndexY = (int)Math.Floor(virtualBounds.Center.Y / (float)Tile.Height);

            for(int x = playerIndexX -1;  x <= playerIndexX +1; x++)
            {
                try // TODO: Exception richtig verhindern
                {
                    Tile currentTile = TileRoom[x, playerIndexY];
                    if(currentTile.Collision == ETileCollision.Solid)
                    {
                        position.X += CollisionUtil.CalculateCollisionDepth(virtualBounds, currentTile.Bounds).X;
                        virtualBounds.Location = new Point((int)position.X, (int)position.Y); // Kopiergenudelt
                    }
                }
                catch (IndexOutOfRangeException) { }
            }
            
            
            for(int y = playerIndexY -1; y <= playerIndexY +1; y++)
            {
                try
                {
                    Tile currentTile = TileRoom[playerIndexX, y];
                    if(currentTile.Collision == ETileCollision.Solid)
                    {
                        position.Y += CollisionUtil.CalculateCollisionDepth(virtualBounds, currentTile.Bounds).Y;
                        virtualBounds.Location = new Point((int)position.X, (int)position.Y);
                    }
                }
                catch (IndexOutOfRangeException) { }
            }
            
        }


        //Moves the Player on Y-Axis
        private void ApplyPhysics(GameTime gameTime, Room room)
        {
            velocity.Y += gravity;
            /*//Jumping Velocity
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
                    if (isOnGround)
                    {
                        isJumping = false;
                    }
                }
            }
            else //If not Jumping, not on ground: Apply gravity
            {
                if (!isOnGround)
                {
                    velocity.Y += 10f;
                }
            }*/

        }


        //TODO Attack Method etc etc

    }
}
