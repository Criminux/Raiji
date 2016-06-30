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
using Raiji.Main;
using Raiji;

namespace Raiji.Main.States.Game
{
    //Enemy type enumeration
    public enum EEnemy
    {
        Yellow = 0,
        Red = 1
    }


    public class Enemy : Character
    {
        //Enemy fields
        bool isAlive;
        //Countdown for RandomBehaviour for type Yellow
        private float movementCountdown;
        //save current direction for type Yellow
        int direction;
        Random random;
        EEnemy type;


        public Enemy(ContentManager content, EEnemy type)
        {
            //Load correct spriteSheets for the enemy type
            this.type = type;
            switch(type)
            {
                case EEnemy.Yellow:
                    //Load Sprites for Animation
                    idleSpriteSheet = content.Load<Texture2D>("Animation/Enemy/128x128_IdleSheetEnemy");
                    runSpriteSheet = content.Load<Texture2D>("Animation/Enemy/128x128_RunSheetEnemy");
                    jumpSpriteSheet = content.Load<Texture2D>("Animation/Enemy/128x128_JumpSheetEnemy");
                    attackSpriteSheet = content.Load<Texture2D>("Animation/Enemy/128x128_AttackSheetEnemy");
                    deadSpriteSheet = content.Load<Texture2D>("Animation/Enemy/128x128_DeadSheetEnemy");
                    characterSprite = content.Load<Texture2D>("Tile/Stone");
                    break;
                case EEnemy.Red:
                    //Load Sprites for Animation
                    idleSpriteSheet = content.Load<Texture2D>("Animation/Enemy/128x128_IdleSheetEnemy2");
                    runSpriteSheet = content.Load<Texture2D>("Animation/Enemy/128x128_RunSheetEnemy2");
                    jumpSpriteSheet = content.Load<Texture2D>("Animation/Enemy/128x128_JumpSheetEnemy2");
                    attackSpriteSheet = content.Load<Texture2D>("Animation/Enemy/128x128_AttackSheetEnemy2");
                    deadSpriteSheet = content.Load<Texture2D>("Animation/Enemy/128x128_DeadSheetEnemy2");
                    characterSprite = content.Load<Texture2D>("Tile/Stone");
                    break;
            }

            
            //Create Animation
            idleAnimation = new Animation(idleSpriteSheet, 5, 2, 128, 128, new TimeSpan(0, 0, 0, 0, 100));
            runAnimation = new Animation(runSpriteSheet, 4, 2, 128, 128, new TimeSpan(0, 0, 0, 0, 100));
            jumpAnimation = new Animation(jumpSpriteSheet, 5, 2, 128, 128, new TimeSpan(0, 0, 0, 0, 100));
            attackAnimation = new Animation(attackSpriteSheet, 4, 2, 128, 128, new TimeSpan(0, 0, 0, 0, 100));
            deadAnimation = new Animation(deadSpriteSheet, 5, 2, 128, 128, new TimeSpan(0, 0, 0, 0, 100));

            //Reset curtain variables
            currentAnimationState = EAnimation.Idle;
            isAlive = true;
            bounds = characterSprite.Bounds;

            random = new Random();

            //Enemy Cooldowns
            life = 3;
            lifeCooldown = 500f;
            movementCountdown = 500f;
            hitCooldown = 500f;
            deadCooldown = 900f;

        }

        public override void Update(GameTime gameTime, Room room)
        {
            //If Enemy is dead
            if (GameOver)
            {
                isAlive = false;
            }

            //Only Update Logic if he is alive
            if (isAlive)
            {
                //Call the character Update
                base.Update(gameTime, room);

                //Enemy Random Behaviour
                if (life > 0) RandomBehaviour(gameTime, room);
                
            }

        }

        protected override void HandleLife(GameTime gameTime, Room room, LevelManager level, List<Enemy> enemies)
        {
            //Decrease Countdown so enemy is hitable
            lifeCooldown -= gameTime.ElapsedGameTime.Milliseconds;

            //Intersect with player
            if (bounds.Intersects(level.PlayerRectangle))
            {
                currentAnimationState = EAnimation.Attack;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(isAlive)
            {
                //Draw the enemy
                base.Draw(spriteBatch);

            }
        }

        

        private void RandomBehaviour(GameTime gameTime, Room room)
        {
            //If enemy is yellow
            if(type == EEnemy.Yellow)
            {
                //Countdown to next direction change
                movementCountdown -= gameTime.ElapsedGameTime.Milliseconds;
                if (movementCountdown <= 0)
                {
                    //Get new direction and reset countdown
                    direction = random.Next(0, 3);
                    movementCountdown = 500f;
                }

                //check the current direction
                if (direction == 1)
                {
                    //Walk right
                    currentAnimationState = EAnimation.Run;
                    animationDirection = SpriteEffects.None;
                    Position = new Vector2((Position.X + 2), Position.Y);
                }
                else if (direction == 2)
                {
                    //Walk left
                    currentAnimationState = EAnimation.Run;
                    animationDirection = SpriteEffects.FlipHorizontally;
                    Position = new Vector2((Position.X - 2), Position.Y);
                }
                else
                {
                    //Idle
                    currentAnimationState = EAnimation.Idle;

                }
            }
            //If enemy is type Red
            else if(type == EEnemy.Red)
            {
                //Get distance to player
                float tempDistance = Vector2.Distance(Position, room.PlayerPosition);
                //If distance is smaller than 300
                if(tempDistance <= 300f)
                {
                    //If smaller than 25 attack him
                    if(tempDistance <= 25f)
                    {
                        Attack(gameTime, room);
                    }
                    else
                    {
                        //Follow the player
                        float tempDir;
                        if (Position.X > room.PlayerPosition.X) tempDir = -1f;
                        else tempDir = 1f;

                        Move(tempDir, gameTime);
                        currentAnimationState = EAnimation.Run;
                    } 
                }
                else
                {
                    //If not close idle
                    currentAnimationState = EAnimation.Idle;
                }
            }

        }

        protected override void OnTileCollision(Tile collidingTile, Vector2 collisionDepth, LevelManager level, Room room)
        {
            //Enemy takes no damage from curtain tiles and cant toggle any events
        }
    }
}
