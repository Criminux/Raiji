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

namespace Raiji
{
    public class Player : Character
    {
        //Only Player Fields
        private int points;
        public int Points
        {
            get { return points; }
        }
        //HasKey for levelDone logic
        private bool hasKey;
        public bool HasKey
        {
            get { return hasKey; }
        }



        public Player(ContentManager content)
        {
            //Load Sprites for Animation
            idleSpriteSheet = content.Load<Texture2D>("Animation/Player/128x128_IdleSheet");
            runSpriteSheet = content.Load<Texture2D>("Animation/Player/128x128_RunSheet");
            jumpSpriteSheet = content.Load<Texture2D>("Animation/Player/128x128_JumpSheet");
            attackSpriteSheet = content.Load<Texture2D>("Animation/Player/128x128_AttackSheet");
            deadSpriteSheet = content.Load<Texture2D>("Animation/Player/128x128_DeadSheet");
            characterSprite = content.Load<Texture2D>("Tile/Stone");

            //Create Animation
            idleAnimation = new Animation(idleSpriteSheet, 5, 2, 128, 128, new TimeSpan(0, 0, 0, 0, 100));
            runAnimation = new Animation(runSpriteSheet, 5, 2, 128, 128, new TimeSpan(0, 0, 0, 0, 100));
            jumpAnimation = new Animation(jumpSpriteSheet, 5, 2, 128, 128, new TimeSpan(0, 0, 0, 0, 100));
            attackAnimation = new Animation(attackSpriteSheet, 5, 2, 128, 128, new TimeSpan(0, 0, 0, 0, 50));
            deadAnimation = new Animation(deadSpriteSheet, 5, 2, 128, 128, new TimeSpan(0, 0, 0, 0, 100));

            //Load Sounds
            attackSound = content.Load<SoundEffect>("Sound/SwordHitFinal");
            jumpSound = content.Load<SoundEffect>("Sound/JumpFinal");
            damageSound = content.Load<SoundEffect>("Sound/DamageFinal");
            stepSound = content.Load<SoundEffect>("Sound/SingleStepFinal");

            currentAnimationState = EAnimation.Idle;
            bounds = characterSprite.Bounds;

            //Life reset
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
            //Call character update
            base.Update(gameTime, room);

            //Call possible PickUp
            PickUpItem(room);
        }

        private void PickUpItem(Room room)
        {
            //Create new temporary List
            List<Item> tempItemList = new List<Item>();

            //Loop though all Items of current room
            for(int i = 0; i < room.Items.Count; i++)
            {
                //If player touches Item
                if (bounds.Intersects(room.Items[i].Bounds))
                {
                    //Pick up item and toggle its logic but DONT add it to temp list
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
                    //If not picked up add it to temp list
                    tempItemList.Add (room.Items[i]);
                }
            }
            //return temp list of all items (not picked up)
            room.Items = tempItemList;
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //call character dra
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
                //Close enough
                if (canAttack == true)
                {
                    //And hitcooldown is done
                    if (hitCooldown <= 0)
                    {
                        //enemy loses life, reset cooldown
                        tempEnemy.Life = tempEnemy.Life - 1;
                        hitCooldown = 500f;
                        //If enemy killed add points
                        if(tempEnemy.Life == 0)
                        {
                            points += 250;
                        }
                    }
                }
                else if(bounds.Intersects(tempEnemy.bounds) && currentAnimationState == EAnimation.Attack)
                {
                    //Dont lose life (attack blocks incoming attack)
                }
                else if (bounds.Intersects(tempEnemy.bounds))
                {
                    //If collision without attacking
                    if (lifeCooldown <= 0)
                    {
                        //Lose life if countdown is done
                        life -= 1;
                        //Play damage sound
                        damageSound.Play();
                        //Reset timer
                        lifeCooldown = 500f;
                    }

                }
            }
            
        }

        protected override void OnTileCollision(Tile collidingTile, Vector2 collisionDepth, LevelManager level, Room room)
        {
            //Is the Tile a Door?
            if (collidingTile is DoorTile)
            {
                //Set active room to targetroom from doortile
                level.ActiveRoom = ((DoorTile)collidingTile).TargetRoom;
                //Set position to spawnPosition from targetDoorTile
                Position = level.GetPositionByID(((DoorTile)collidingTile).GetTargetID);
            }
            //Dead if colliding with acidFull (quick rescue if only touch TOP)
            else if (collidingTile.Type == ETile.AcidFull) life = 0;
            //Dead if colliding with spikes
            else if (collidingTile.Type == ETile.Spike) life = 0;
            //If colliding with Station
            else if (collidingTile.Type == ETile.HealStation)
            {
                //Get Station
                HealStationTile healStation = (HealStationTile)collidingTile;
                //If Player pressed USE
                if (Click)
                {
                    //If healstation still usable
                    if (!healStation.IsUsed)
                    {
                        //call increase Life method
                        IncreaseLife();
                    }
                    //Call Use everytime, so sounds can be played
                    ((HealStationTile)collidingTile).Use();
                }

            }
            else if (collidingTile.Type == ETile.HealStationUsed)
            {
                if (Click)
                {
                    //Call use, so sound can be played
                    ((HealStationTile)collidingTile).Use();
                }
                //No life increasement
            }
            //If colliding with locked door 
            else if (collidingTile.Type == ETile.DoorLocked)
            {
                //If player has enough points and the key he can click 
                if(Click && points >= 1000 && hasKey)
                {
                    //Level is done
                    level.LevelDone = true;
                }
            }
            //Colliding with trigger
            else if(collidingTile.Type  == ETile.Trigger)
            {
                //Get tempID
                String tempID = ((TriggerTile)collidingTile).TargetID;
                //Trigger correct tile by tempID
                level.TriggerTileByID(tempID);
            }
            
        }

        //Called from HealStationCollision
        public void IncreaseLife()
        {
            //Adds life and checks if its maximum 3
            life += 1;
            if (life > 3) life = 3;
        }


    }
}
