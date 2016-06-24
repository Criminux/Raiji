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
    class Player : Character
    {
       

        public Player(ContentManager content)
        {
            //Load Sprites for Animation
            idleSpriteSheet = content.Load<Texture2D>("128x128_IdleSheet");
            runSpriteSheet = content.Load<Texture2D>("128x128_RunSheet");
            jumpSpriteSheet = content.Load<Texture2D>("128x128_JumpSheet");
            attackSpriteSheet = content.Load<Texture2D>("128x128_AttackSheet");
            characterSprite = content.Load<Texture2D>("Stone");

            //Create Animation
            idleAnimation = new Animation(idleSpriteSheet, 5, 2, 128, 128, new TimeSpan(0, 0, 0, 0, 100));
            runAnimation = new Animation(runSpriteSheet, 5, 2, 128, 128, new TimeSpan(0, 0, 0, 0, 100));
            jumpAnimation = new Animation(jumpSpriteSheet, 5, 2, 128, 128, new TimeSpan(0, 0, 0, 0, 100));
            attackAnimation = new Animation(attackSpriteSheet, 5, 2, 128, 128, new TimeSpan(0, 0, 0, 0, 50));

            //Sound Stuff
            attackSound = content.Load<SoundEffect>("SwordHitFinal");
            jumpSound = content.Load<SoundEffect>("JumpFinal");
            damageSound = content.Load<SoundEffect>("DamageFinal");
            stepSound = content.Load<SoundEffect>("SingleStepFinal");

            currentAnimationState = EAnimation.Idle;

            bounds = characterSprite.Bounds;

            //Life Stufff
            life = 3;
            lifeCooldown = 500f;
            hitCooldown = 500f;
        }

        public override void Update(GameTime gameTime, Room room)
        {
           

            base.Update(gameTime, room);
            
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);  
        }

        protected override void HandleLife(GameTime gameTime, Room room, LevelManager level)
        {
            //Decrease Countdown so Player is hitable
            lifeCooldown -= gameTime.ElapsedGameTime.Milliseconds;
            hitCooldown -= gameTime.ElapsedGameTime.Milliseconds;
            
            //Intersect with Enemy and is Attacking
            if (bounds.Intersects(room.EnemyBounds) && currentAnimationState == EAnimation.Attack)
            {
                if(hitCooldown <= 0)
                {
                    room.EnemyLife = room.EnemyLife -= 1;
                    hitCooldown = 500f;
                }
            }
            else if (bounds.Intersects(room.EnemyBounds))
            {
                if (lifeCooldown <= 0)
                {
                    life -= 1;
                    damageSound.Play();
                    lifeCooldown = 500f;
                }

            }
        }

    }
}
