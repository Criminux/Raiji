using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Raiji.Main.States.Game;

namespace Raiji
{
    public class LevelManager
    {
        //UI Manager and content
        UIManager uiManager;
        ContentManager content;
        TimeManager timeManager;
        //checks if level is done
        private bool levelDone;
        public bool LevelDone
        {
            set { levelDone = value; }
            get { return levelDone; }
        }
        //holds all rooms from current level
        Room[] room;
        //holds the current level ID and the active room (for draw)
        private int levelID;
        private int activeRoom;
        public int ActiveRoom
        {
            set { activeRoom = value; }
        }

        //initialize flag
        private bool isInitialized;

        //holds the player
        private Player player;
        public Rectangle PlayerRectangle
        {
            get { return player.bounds; }
        }
        //GameOver property for check from gameLoop
        public bool GameOver
        {
            get
            {   //GameOver whem Timer ran out or player dead
                if (player.GameOver || timeManager.Seconds > 100)
                {
                    return true;
                }
                else return false; 
            }
        }



        public LevelManager(ContentManager content)
        {
            //save instance of content, create new instances and reset variables
            this.content = content;
            levelDone = false;
            levelID = 1;
            activeRoom = 0;
            isInitialized = false; 
            
            player = new Player(content);

            uiManager = new UIManager(player, content);
            timeManager = new TimeManager();
        }
        

        public void Update(GameTime gameTime)
        {
            //If not initialized
            if (!isInitialized)
            {
                //Initialize the level by ID and set player position
                InitializeLevel(levelID);
                player.Position = new Vector2(200, 500);

            }

            //Update Time
            timeManager.UpdateTime(gameTime);

            //Update UI
            uiManager.Update(room[activeRoom], timeManager.Seconds);

            //Update Room
            room[activeRoom].Update(gameTime, this, player.Position);

            //Update Player
            player.Update(gameTime, room[activeRoom]);

            //Input
            ExecuteInput(StateMachine.inputManager.GetInput(), gameTime);

            //AfterUpdatePlayer
            player.AfterUpdate(gameTime, room[activeRoom], this, content, room[activeRoom].Enemies);

        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont) 
        {
            //Draw UI
            uiManager.Draw(spriteBatch, spriteFont);

            //Draw Room, including Tiles, Enemies and other Items
            room[activeRoom].Draw(spriteBatch, spriteFont);
            
            //Draw Character
            player.Draw(spriteBatch);

        }

        public void InitializeLevel(int levelID)
        {
            //Check current levelID
            if(levelID == 1)
            {
                //Level 1 contains 5 rooms
                room = new Room[5];
                for(int i = 0; i < room.Length; i++)
                {
                    //Loop through them, room will initialize itself by level and room ID
                    room[i] = new Room(content, levelID, i + 1);
                }
            }
            //Testing purpose, cant be the case atm becasue of Demo Build
            /*else if(levelID == 2)
            {
                room = new Room[1];
                room[0] = new Room(content, levelID, 1);
            }*/

            //Update flag
            isInitialized = true;
        }
        
        //Execute input and call matchig player methods
        public void ExecuteInput(EInputKey[] inputs, GameTime gameTime)
        {
            int size = inputs.Length;

            foreach(EInputKey inputKey in inputs)
            {
                switch (inputKey)
                {
                    case EInputKey.Right:
                        player.Move(1f, gameTime);
                        break;
                    case EInputKey.Left:
                        player.Move(-1f, gameTime);
                        break;
                    case EInputKey.Jump:
                        player.BeginJump(gameTime);
                        break;
                    case EInputKey.Attack:
                        player.Attack(gameTime, room[activeRoom]);
                        break;
                    case EInputKey.Use:
                        player.Click = true;
                        break;
                }
            }

        }

        //Method for DoorTile business
        public Vector2 GetPositionByID(String ID)
        {
            Vector2 result = new Vector2(0, 0);

            //Loop though all rooms
            for(int i = 0; i < room.Length; i++)
            {
                //Save tileroom of room i
                Tile[,] tempTileRoom = room[i].tileRoom;

                //Loop through Tiles of current Room i
                for(int j = 0; j < tempTileRoom.GetLength(0); j++)
                {
                    for(int k = 0; k < tempTileRoom.GetLength(1); k++)
                    {
                        //If found a DoorTile
                        Tile tempTile = tempTileRoom[j, k];
                        if(tempTile is DoorTile)
                        {
                            //Check its ID with ID from call
                            String tempID = ((DoorTile)tempTile).GetID;
                            if(tempID == ID)
                            {
                                //If it is the searched Tile get its spawnposition
                                result = ((DoorTile)tempTile).SpawnPosition;
                            }
                        }
                    }
                }
            }
            //Return the position
            return result;
        }

        //Method for TriggerTile Business
        public void TriggerTileByID(String ID)
        {
            //Loop thtough all rooms
            for (int i = 0; i < room.Length; i++)
            {
                //Save current room i
                Tile[,] tempTileRoom = room[i].tileRoom;

                //Loop through Tiles of current Room i
                for (int j = 0; j < tempTileRoom.GetLength(0); j++)
                {
                    for (int k = 0; k < tempTileRoom.GetLength(1); k++)
                    {
                        //If found TriggerTile
                        Tile tempTile = tempTileRoom[j, k];
                        if(tempTile is TriggeredTile)
                        {
                            //Compare its ID
                            TriggeredTile tempTriggeredTile = ((TriggeredTile)tempTile);
                            if (tempTriggeredTile.GetID == ID)
                            {
                                //If its the searched Tile trigger it
                                (tempTriggeredTile).Trigger();
                            }
                        }
                        
                    }
                }
            }
        }

    }
}
