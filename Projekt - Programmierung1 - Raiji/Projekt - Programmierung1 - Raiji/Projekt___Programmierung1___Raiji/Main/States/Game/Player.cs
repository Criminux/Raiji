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
    public class Player : Character
    {
        //Only Player Fields
        private int points;
        public int Points
        {
            get { return points; }
        }

        private bool hasKey;
        public bool HasKey
        {
            get { return hasKey; }
        }



        public Player(ContentManager content)
        {
            //Load Sprites for Animation
            idleSpriteSheet = content.Load<Texture2D>("128x128_IdleSheet");
            runSpriteSheet = content.Load<Texture2D>("128x128_RunSheet");
            jumpSpriteSheet = content.Load<Texture2D>("128x128_JumpSheet");
            attackSpriteSheet = content.Load<Texture2D>("128x128_AttackSheet");
            deadSpriteSheet = content.Load<Texture2D>("128x128_DeadSheet");
            characterSprite = content.Load<Texture2D>("Stone");

            //Create Animation
            idleAnimation = new Animation(idleSpriteSheet, 5, 2, 128, 128, new TimeSpan(0, 0, 0, 0, 100));
            runAnimation = new Animation(runSpriteSheet, 5, 2, 128, 128, new TimeSpan(0, 0, 0, 0, 100));
            jumpAnimation = new Animation(jumpSpriteSheet, 5, 2, 128, 128, new TimeSpan(0, 0, 0, 0, 100));
            attackAnimation = new Animation(attackSpriteSheet, 5, 2, 128, 128, new TimeSpan(0, 0, 0, 0, 50));
            deadAnimation = new Animation(deadSpriteSheet, 5, 2, 128, 128, new TimeSpan(0, 0, 0, 0, 100));

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
            deadCooldown = 900f;

            //Reset Variables
            points = 0;
            hasKey = false;
        }

        public override void Update(GameTime gameTime, Room room)
        {
           
            base.Update(gameTime, room);

            PickUpItem(room);
        }

        private void PickUpItem(Room room)
        {
            List<Item> tempItemList = new List<Item>();

            for(int i = 0; i < room.Items.Count; i++)
            {
                if (bounds.Intersects(room.Items[i].Bounds))
                {

                    switch (room.Items[i].Type)
                    {
                        case EItem.Diamond:
                            points += 100;
                            break;
                        case EItem.Key:
                            hasKey = true;
                            break;

                    }
                }
                else
                {
                    tempItemList.Add (room.Items[i]);
                }
            }

            room.Items = tempItemList;
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);  
        }

        protected override void HandleLife(GameTime gameTime, Room room, LevelManager level, List<Enemy> enemies)
        {
            //Decrease Countdown so Player is hitable
            lifeCooldown -= gameTime.ElapsedGameTime.Milliseconds;
            hitCooldown -= gameTime.ElapsedGameTime.Milliseconds;
            
            //Intersect with Enemy and is Attacking
            foreach(Enemy tempEnemy in enemies)
            {
                if (canAttack == true)
                {
                    if (hitCooldown <= 0)
                    {
                        tempEnemy.Life = tempEnemy.Life - 1;
                        hitCooldown = 500f;
                        if(tempEnemy.Life == 0)
                        {
                            points += 250;
                        }
                    }
                }
                else if(bounds.Intersects(tempEnemy.bounds) && currentAnimationState == EAnimation.Attack)
                {

                }
                else if (bounds.Intersects(tempEnemy.bounds))
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

        protected override void OnTileCollision(Tile collidingTile, Vector2 collisionDepth, LevelManager level)
        {
            //Is the Tile a Door?
            if (collidingTile is DoorTile)
            {
                level.ActiveRoom = ((DoorTile)collidingTile).TargetRoom;
                Position = level.GetPositionByID(((DoorTile)collidingTile).GetTargetID);
            }
            else if (collidingTile.Type == ETile.AcidFull) life = 0;
            else if (collidingTile.Type == ETile.Spike) life = 0;
            else if (collidingTile.Type == ETile.HealStation)
            {

                HealStationTile healStation = (HealStationTile)collidingTile;
                if (Click)
                {
                    if (!healStation.IsUsed)
                    {
                        IncreaseLife();
                    }
                    ((HealStationTile)collidingTile).Use();
                }

            }
            else if (collidingTile.Type == ETile.HealStationUsed)
            {
                if (Click)
                {
                    ((HealStationTile)collidingTile).Use();
                }
            }
            else if (collidingTile.Type == ETile.DoorLocked)
            {
                if(Click && points >= 1000 && hasKey)
                {
                    level.LevelDone = true;
                }
            }
            
        }

        public void IncreaseLife()
        {
            life += 1;
            if (life > 3) life = 3;
        }


    }
}
