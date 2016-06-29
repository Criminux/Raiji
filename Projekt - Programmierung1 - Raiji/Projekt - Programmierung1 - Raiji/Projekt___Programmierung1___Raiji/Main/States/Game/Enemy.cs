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
    public enum EEnemy
    {
        Yellow = 0,
        Red = 1
    }


    public class Enemy : Character
    {
        bool isAlive;
        private float movementCountdown;
        private float speed;
        int direction;
        Random random;
        EEnemy type;


        public Enemy(ContentManager content, EEnemy type)
        {
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
                    speed = 1f;
                    break;
                case EEnemy.Red:
                    //Load Sprites for Animation
                    idleSpriteSheet = content.Load<Texture2D>("Animation/Enemy/128x128_IdleSheetEnemy2");
                    runSpriteSheet = content.Load<Texture2D>("Animation/Enemy/128x128_RunSheetEnemy2");
                    jumpSpriteSheet = content.Load<Texture2D>("Animation/Enemy/128x128_JumpSheetEnemy2");
                    attackSpriteSheet = content.Load<Texture2D>("Animation/Enemy/128x128_AttackSheetEnemy2");
                    deadSpriteSheet = content.Load<Texture2D>("Animation/Enemy/128x128_DeadSheetEnemy2");
                    characterSprite = content.Load<Texture2D>("Tile/Stone");
                    speed = 2f;
                    break;
            }

            
            //Create Animation
            idleAnimation = new Animation(idleSpriteSheet, 5, 2, 128, 128, new TimeSpan(0, 0, 0, 0, 100));
            runAnimation = new Animation(runSpriteSheet, 4, 2, 128, 128, new TimeSpan(0, 0, 0, 0, 100));
            jumpAnimation = new Animation(jumpSpriteSheet, 5, 2, 128, 128, new TimeSpan(0, 0, 0, 0, 100));
            attackAnimation = new Animation(attackSpriteSheet, 4, 2, 128, 128, new TimeSpan(0, 0, 0, 0, 100));
            deadAnimation = new Animation(deadSpriteSheet, 5, 2, 128, 128, new TimeSpan(0, 0, 0, 0, 100));

            currentAnimationState = EAnimation.Idle;
            isAlive = true;
            bounds = characterSprite.Bounds;

            random = new Random();

            //Enemy Variable Stuff
            life = 3;
            lifeCooldown = 500f;
            movementCountdown = 500f;
            hitCooldown = 500f;
            deadCooldown = 900f;

        }

        public override void Update(GameTime gameTime, Room room)
        {
            //If Enemy is smaller than 0
            if (GameOver)
            {
                isAlive = false;
            }

            //Only Update Logic if he is alive
            if (isAlive)
            {
                
                base.Update(gameTime, room);

                //Enemy Random Movement
                if (life > 0) RandomBehaviour(gameTime, room);
                
            }

        }

        protected override void HandleLife(GameTime gameTime, Room room, LevelManager level, List<Enemy> enemies)
        {
            //Decrease Countdown so Player is hitable
            lifeCooldown -= gameTime.ElapsedGameTime.Milliseconds;

            //Intersect with Enemy and is Attacking
            if (bounds.Intersects(level.PlayerRectangle) && currentAnimationState == EAnimation.Attack)
            {
                foreach(Enemy tempEnemy in enemies)
                {
                    tempEnemy.Life = tempEnemy.Life - 1;
                }
            }
            else if (bounds.Intersects(level.PlayerRectangle))
            {
                currentAnimationState = EAnimation.Attack;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(isAlive)
            {
                base.Draw(spriteBatch);

            }
        }

        

        private void RandomBehaviour(GameTime gameTime, Room room)
        {
            if(type == EEnemy.Yellow)
            {
                movementCountdown -= gameTime.ElapsedGameTime.Milliseconds;
                if (movementCountdown <= 0)
                {
                    direction = random.Next(0, 3);
                    movementCountdown = 500f;
                }
                if (direction == 1)
                {
                    currentAnimationState = EAnimation.Run;
                    animationDirection = SpriteEffects.None;
                    Position = new Vector2((Position.X + 2), Position.Y);
                }
                else if (direction == 2)
                {
                    currentAnimationState = EAnimation.Run;
                    animationDirection = SpriteEffects.FlipHorizontally;
                    Position = new Vector2((Position.X - 2), Position.Y);
                }
                else
                {
                    currentAnimationState = EAnimation.Idle;

                }
            }
            else if(type == EEnemy.Red)
            {
                float tempDistance = Vector2.Distance(Position, room.PlayerPosition);
                if(tempDistance <= 300f)
                {
                    if(tempDistance <= 25f)
                    {
                        Attack(gameTime, room);
                    }
                    else
                    {
                        float tempDir;
                        if (Position.X > room.PlayerPosition.X) tempDir = -1f;
                        else tempDir = 1f;

                        Move(tempDir, gameTime);
                        currentAnimationState = EAnimation.Run;
                    } 
                }
                else
                {
                    currentAnimationState = EAnimation.Idle;
                }
            }

        }

        protected override void OnTileCollision(Tile collidingTile, Vector2 collisionDepth, LevelManager level)
        {
            //Gegner bekommt keinen Schaden etc.
        }
    }
}
