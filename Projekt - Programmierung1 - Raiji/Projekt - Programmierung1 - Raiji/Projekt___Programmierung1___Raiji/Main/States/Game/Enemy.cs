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
using Projekt___Programmierung1___Raiji;

namespace Raiji.Main.States.Game
{
    class Enemy : Character
    {
        bool isAlive;
        private float movementCountdown;
        int direction;
        Random random;


        public Enemy(ContentManager content)
        {
            //Load Sprites for Animation
            idleSpriteSheet = content.Load<Texture2D>("128x128_IdleSheetEnemy");
            runSpriteSheet = content.Load<Texture2D>("128x128_RunSheetEnemy");
            jumpSpriteSheet = content.Load<Texture2D>("128x128_JumpSheetEnemy");
            attackSpriteSheet = content.Load<Texture2D>("128x128_AttackSheetEnemy");
            characterSprite = content.Load<Texture2D>("Stone");

            //Create Animation
            idleAnimation = new Animation(idleSpriteSheet, 5, 2, 128, 128, new TimeSpan(0, 0, 0, 0, 100));
            runAnimation = new Animation(runSpriteSheet, 4, 2, 128, 128, new TimeSpan(0, 0, 0, 0, 100));
            jumpAnimation = new Animation(jumpSpriteSheet, 5, 2, 128, 128, new TimeSpan(0, 0, 0, 0, 100));
            attackAnimation = new Animation(attackSpriteSheet, 4, 2, 128, 128, new TimeSpan(0, 0, 0, 0, 100));

            currentAnimationState = EAnimation.Idle;
            isAlive = true;
            bounds = characterSprite.Bounds;

            random = new Random();

            //Enemy Variable Stuff
            life = 3;
            lifeCooldown = 500f;
            movementCountdown = 500f;
            hitCooldown = 500f;

        }

        public override void Update(GameTime gameTime, Room room)
        {
            //If Enemy is smaller than 0
            if (life <= 0)
            {
                isAlive = false;
            }

            //Only Update Logic if he is alive
            if (isAlive)
            {
                
                base.Update(gameTime, room);
                //Enemy Random Movement
                RandomBehaviour(gameTime);
            }
            
        }

        protected override void HandleLife(GameTime gameTime, Room room, LevelManager level)
        {
            //Decrease Countdown so Player is hitable
            lifeCooldown -= gameTime.ElapsedGameTime.Milliseconds;

            //Intersect with Enemy and is Attacking
            if (bounds.Intersects(level.PlayerRectangle) && currentAnimationState == EAnimation.Attack)
            {
                room.EnemyLife = room.EnemyLife -= 1;
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

        

        private void RandomBehaviour(GameTime gameTime)
        {
            movementCountdown -= gameTime.ElapsedGameTime.Milliseconds;
            if(movementCountdown <= 0)
            {
                direction = random.Next(0, 2);   
                movementCountdown = 500f;
            }
            if (direction == 1)
            {
                currentAnimationState = EAnimation.Run;
                animationDirection = SpriteEffects.None;
                Position = new Vector2(Position.X + 5, Position.Y);
            }
            else
            {
                currentAnimationState = EAnimation.Run;
                animationDirection = SpriteEffects.FlipHorizontally;
                Position = new Vector2(Position.X - 5, Position.Y);
            }

        }
    }
}
