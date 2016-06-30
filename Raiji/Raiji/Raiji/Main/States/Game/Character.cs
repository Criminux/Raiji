using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
        //Character Outline
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

        //Use
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

        //Is influenced by the velocity
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
            
            //Update Cooldowns
            attackCooldown -= gameTime.ElapsedGameTime.Milliseconds;
            stepCooldown -= gameTime.ElapsedGameTime.Milliseconds;
            clickCooldown -= gameTime.ElapsedGameTime.Milliseconds;

            //Still attacking
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
            //Calculate physics from character
            ApplyPhysics(gameTime, room);
            //Apply the calculated physics to the position of character
            position += velocity;
            //Handle all collisions from character
            HandleCollisions(room, level, gameTime, content);

            //Handle the life of the character
            HandleLife(gameTime, room, level, enemies);

            if (life <= 0)
            {
                //If the character is dead show his dead animation
                currentAnimationState = EAnimation.Dead;
                deadAnimation.Update(gameTime);

                deadCooldown -= gameTime.ElapsedGameTime.Milliseconds;
                if(deadCooldown <= 0)
                {
                    //As soon as the dead animation is over, set gameOver bool true
                    gameOver = true;
                }
            }
        }

        virtual public void Draw(SpriteBatch spriteBatch)
        {
            //Draw character on correct position with matching animation and direction
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

       
        //Moves the character on X-Axis
        public void Move(float dir, GameTime gameTime)
        {
            //Only move when character is alive
            if(life > 0)
            {
                
                dir = MathHelper.Clamp(dir, -maxdir, maxdir);

                //Set the Animation Direction
                if (dir >= 0) animationDirection = SpriteEffects.None;
                else animationDirection = SpriteEffects.FlipHorizontally;

                //Calculate step cooldown for sound of player
                if (stepCooldown <= 0 && this is Player)
                {
                    stepSound.Play(0.5f, 0, 0);
                    stepCooldown = 475f;
                }

                //If move is called: Set animation state to run
                currentAnimationState = EAnimation.Run;
                
                //Enemy movement is slower then player
                if(this is Enemy) velocity.X = MathHelper.Clamp(dir * gameTime.ElapsedGameTime.Milliseconds * acceleration / 2, -maxMoveSpeed , maxMoveSpeed );
                else velocity.X = MathHelper.Clamp(dir * gameTime.ElapsedGameTime.Milliseconds * acceleration, -maxMoveSpeed, maxMoveSpeed);

            }

        }

        //Begins jump - triggered by space
        public void BeginJump(GameTime gameTime)
        {
            //only begin jump when character is alive
            if(life > 0)
            {
                //Start jump and play sound
                if (!JumpHasCooledDown)
                {
                    jumpTime = 0f;
                    jumpSound.Play(0.7f, 0, 0);
                }
            }
            
        }

        
        private void HandleCollisions(Room room, LevelManager level, GameTime gameTime, ContentManager content)
        {
            // Get the current tileroom
            Tile[,] TileRoom = room.tileRoom;
            // Set the location of the players rectangle to current position
            bounds.Location = new Point((int)position.X, (int)position.Y);// TODO: Update Bounds Funktion

            // Get the index of tiles, the player is standing on
            int playerIndexX = (int)Math.Floor(bounds.Center.X / (float)Tile.Width);
            int playerIndexY = (int)Math.Floor(bounds.Center.Y / (float)Tile.Height);
            
            //Loop through the potential collidable tiles on X-Axis
            for(int x = playerIndexX -1;  x <= playerIndexX +1; x++)
            {
                //Avoid OutOfRange Exception
                if(x >= 0 && x < TileRoom.GetLength(0) && playerIndexY >= 0 && playerIndexY < TileRoom.GetLength(1))
                {
                    //Get the tile which will now be checked
                    Tile currentTile = TileRoom[x, playerIndexY];

                    //Get the collisionDepth of the player with the current tile
                    Vector2 collisionDepth = CollisionUtil.CalculateCollisionDepth(bounds, currentTile.Bounds);
                    //When return value is zero, there would be no collision at all
                    if(collisionDepth != Vector2.Zero)
                    {
                        //Call OnTileCollision method implemented in subclass because of different actions
                        OnTileCollision(currentTile, collisionDepth, level, room);

                        //If the tile (regardless to its type) is solid then correct player position
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

                    //Get the collisionDepth of the player with the current tile
                    Vector2 collisionDepth = CollisionUtil.CalculateCollisionDepth(bounds, currentTile.Bounds);
                    //When return value is zero, there would be no collision at all
                    if (collisionDepth != Vector2.Zero)
                    {
                        //Call OnTileCollision method implemented in subclass because of different actions
                        OnTileCollision(currentTile, collisionDepth, level, room);

                        //If the tile (regardless to its type) is solid then correct player position
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

        //Handle Life and specific tile collisions are different for subclasses player and enemy: abstract for later implementation
        protected abstract void HandleLife(GameTime gameTime, Room room, LevelManager level, List<Enemy> enemies);
        protected abstract void OnTileCollision(Tile collidingTile, Vector2 collisionDepth, LevelManager level, Room room);
       


        //Moves the Player on Y-Axis
        private void ApplyPhysics(GameTime gameTime, Room room)
        {
            //Add gravity
            velocity.Y += gravity;
            //Add time from last Update to jumpTime
            jumpTime += gameTime.ElapsedGameTime.Milliseconds;
            
            //If player still jumping
            if (IsJumping)
            {
                //Add velocity (which decreases with time)
                velocity.Y -= jumpVelocity * (1f - (jumpTime / maxJumpTime));
                //Set jump to current animation
                currentAnimationState = EAnimation.Jump;
            }

        }

        


        public void Attack(GameTime gameTime, Room room)
        {
            //If attack is cooled down
            if(attackCooldown <= 0)
            {
                //Start attack animation
                currentAnimationState = EAnimation.Attack;
                
                //Reset cooldown
                attackCooldown = 500f;

                if (this is Player) attackSound.Play(); //Only play sound to player actions

                //Set the canAttack bool for fighting system
                float tempDistance = room.GetCloseEnemyDistance();
                if(tempDistance <= 100f)
                {
                    canAttack = true;
                }

            }
            

        }

    }
}
