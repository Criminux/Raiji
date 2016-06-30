using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Raiji.Main.States.Game;
using System.Collections.Generic;

namespace Raiji.Main.States.Game
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public class Room
    {
        //Content instance
        private ContentManager content;

        //Constant roster amount
        const int RowX = 30;
        const int RowY = 15;
        
        //Room is a TileRoom
        private Tile[,] TileRoom = new Tile[RowX, RowY];
        public Tile[,] tileRoom
        {
            get { return TileRoom; }
        }

        //ossible tooltips
        private List<Tooltip> tooltips;

        //possible enemies
        private List<Enemy> enemies;
        public List<Enemy> Enemies
        {
            get { return enemies; }
        }

        //possible items
        private List<Item> items;
        public List<Item> Items
        {
            get { return items; }
            set { items = value; }
        }

        //Save all Triggered Tiles to update the Timer
        private List<TriggeredTile> triggeredTiles;
        public List<TriggeredTile> TriggeredTiles
        {
            set { triggeredTiles = value; }
            get { return triggeredTiles; }
        }

        //Knows playerPosition
        private Vector2 playerPosition;
        public Vector2 PlayerPosition
        {
            get { return playerPosition; }
        }

        //Saves his IDs
        private int levelID;
        private int roomID;

        public int RoomID
        {
            get { return roomID; }
            set { roomID = value; }
        }

        //StreamReader instance for reading information of current room
        StreamReader reader;

        public Room(ContentManager content, int levelID, int roomID)
        {
            //Save instance and IDs
            this.content = content;

            this.levelID = levelID;
            this.roomID = roomID;

            //Initialize the lists
            enemies = new List<Enemy>();
            tooltips = new List<Tooltip>();
            items = new List<Item>();
            triggeredTiles = new List<TriggeredTile>();

            //Initialize itself
            Initialize(levelID, roomID);
        }
        
        public void Update(GameTime gameTime, LevelManager level, Vector2 playerPosition)
        {
            //update saved playerPosition
            this.playerPosition = playerPosition;

            //Loop though triggeredTiles (backwards: in earlier development state triggered tiles where removed from the list, which threw a enmuration exception)
            for (int i = triggeredTiles.Count - 1; i >= 0; --i)
            {
                //Call Update method
                triggeredTiles[i].Update(gameTime);
            }

            //Update all enemies
            foreach (Enemy tempEnemy in enemies)
            {
                tempEnemy.Update(gameTime, this);
                tempEnemy.AfterUpdate(gameTime, this, level, content, enemies);
            }

        }

        
        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            //Draw the Tiles
            for(int i = 0; i < RowX; i++)
            {
                for(int j = 0; j < RowY; j++)
                {
                    TileRoom[i, j].DrawTile(spriteBatch);
                }
            }

            //Draw the Tooltips
            foreach (Tooltip tempTooltip in tooltips)
            {
                tempTooltip.Draw(spriteBatch, spriteFont);
            }

            //Draw the Enemies
            foreach (Enemy tempEnemy in enemies)
            {
                tempEnemy.Draw(spriteBatch);
            }

            //Draw the Items
            foreach (Item tempItem in items)
            {
                tempItem.Draw(spriteBatch);
            }

        }

        //GetCloseEnemyLife for UI Manager
        public int GetCloseEnemyLife()
        {
            //Close distance defined to 500
            float closeDistance = 500f;
            int enemyLife = 0;

            //Loop through all enemies
            foreach(Enemy tempEnemy in enemies)
            {
                //Get distance to current temp enemy
                float tempDistance = Vector2.Distance(playerPosition, tempEnemy.Position);
                //If distance is close
                if(tempDistance < closeDistance)
                {
                    //closeDistance is update in case there are multiple enemies near
                    closeDistance = tempDistance;
                    //Set life value
                    enemyLife = tempEnemy.Life;
                }
            }
            //return life value - if 0, no lifes will be drawed from UI Manager
            return enemyLife;
        }

        //Needed from Attack method
        public float GetCloseEnemyDistance()
        {
            float result = 500f;

            //Loop through all enemies
            foreach (Enemy tempEnemy in enemies)
            {
                //If distance is smaller then 500 set new distance
                float tempDistance = Vector2.Distance(playerPosition, tempEnemy.Position);
                if (tempDistance < result)
                {
                    result = tempDistance;
                }
            }
            //return the distance of close enemy or 500
            return result;
        }


        //Initialize new Room
        private void Initialize(int levelID, int roomID) 
        {
           //yCount for ReadLine
            int yCount = 0;

            //create new reader instance with path to correct roomDataFile
            reader = new StreamReader("Content/LevelData/Level" + levelID + "Room" + roomID + ".txt");

            //Fill the standard roster
            while (yCount < 15)
            {

                //Save new Line
                String tempLine = reader.ReadLine();
                //Split it at every space
                String[] IDs = tempLine.Split(' ');
                
                //Loop through the lenght of it
                for(int i = 0; i < IDs.Length; i++)
                {
                    //Convert it to int array
                    int[] ID = new int[IDs.Length];
                    ID[i] = Int32.Parse(IDs[i]);

                    //Instatiate new Tile at correct position with correct type
                    switch(ID[i])
                    {
                        case 0:
                            TileRoom[i, yCount] = new Tile(ETile.Background, new Vector2(i*Tile.Width, yCount*Tile.Height), content);
                            break;
                        case 1:
                            TileRoom[i, yCount] = new Tile(ETile.Stone, new Vector2(i * Tile.Width, yCount * Tile.Height), content);
                            break;
                        case 2:
                            TileRoom[i, yCount] = new Tile(ETile.Spike, new Vector2(i * Tile.Width, yCount * Tile.Height), content);
                            break;
                        case 3:
                            TileRoom[i, yCount] = new Tile(ETile.AcidTop, new Vector2(i * Tile.Width, yCount * Tile.Height), content);
                            break;
                        case 4:
                            TileRoom[i, yCount] = new Tile(ETile.AcidFull, new Vector2(i * Tile.Width, yCount * Tile.Height), content);
                            break;
                        case 5:
                            TileRoom[i, yCount] = new HealStationTile(ETile.HealStation, new Vector2(i * Tile.Width, yCount * Tile.Height), content, false);
                            break;
                        case 6:
                            TileRoom[i, yCount] = new TriggerTile(ETile.Trigger, new Vector2(i * Tile.Width, yCount * Tile.Height), content);
                            break;
                        case 7:
                            TileRoom[i, yCount] = new TriggeredTile(ETile.Triggered, new Vector2(i * Tile.Width, yCount * Tile.Height), content);
                            break;
                        case 8:
                            TileRoom[i, yCount] = new Tile(ETile.DoorLocked, new Vector2(i * Tile.Width, yCount * Tile.Height), content);
                            break;
                        case 9:
                            TileRoom[i, yCount] = new DoorTile(ETile.Door, new Vector2(i * Tile.Width, yCount * Tile.Height), content);
                            break;
                        case 99:
                            TileRoom[i, yCount] = new Tile(ETile.Unspecified, new Vector2(i * Tile.Width, yCount * Tile.Height), content);
                            break;
                    }
                }
                //Increase yCount
                yCount++;

                
            }

            //Read more lines for DoorTile Linking, enemy initialization, tooltips and other obstacles or items
            String Line;
            while((Line = reader.ReadLine()) != null) //While end of file not reached
            {
                //Split a space again
                String[] setting = Line.Split(' ');

                //If first position is Property -> DoorTile Property
                if (setting[0] == "Property")
                {
                    //Read following positions and save the specific information in temporary variables

                    int tempX = Int32.Parse(setting[1]);
                    int tempY = Int32.Parse(setting[2]);

                    int tempTargetRoom = Int32.Parse(setting[3]);
                    tempTargetRoom--;

                    String tempID = setting[4];
                    String tempTargetID = setting[5];

                    float spawnPositionX = float.Parse(setting[6]);
                    float spawnPositionY = float.Parse(setting[7]);
                    Vector2 spawnPosition = new Vector2(spawnPositionX, spawnPositionY);

                    //Is the Tile a DoorTile apply the settings to it
                    if (TileRoom[tempX, tempY] is DoorTile)
                    {
                        DoorTile tile = (DoorTile)TileRoom[tempX, tempY];
                        //Call the SetProperties method with all read settings
                        tile.SetProperties(tempTargetRoom, tempID, tempTargetID, spawnPosition);
                    }
                    

                    
                }

                //If first position is Enemy -> Enemy Settings
                if (setting[0] == "Enemy")
                {
                    //Read following positions and save the specific information in temporary variables

                    float tempX = float.Parse(setting[1]);
                    float tempY = float.Parse(setting[2]);

                    int tempType = int.Parse(setting[3]);

                    //Instantiate new Enemy with all properties and add it to enemy list
                    Enemy tempEnemy;

                    switch (tempType)
                    {
                        case 0:
                            tempEnemy = new Enemy(content, EEnemy.Yellow);
                            tempEnemy.Position = new Vector2(tempX, tempY);
                            enemies.Add(tempEnemy);
                            break;
                        case 1:
                            tempEnemy = new Enemy(content, EEnemy.Red);
                            tempEnemy.Position = new Vector2(tempX, tempY);
                            enemies.Add(tempEnemy);
                            break;
                    }

                    
                }

                //If first position is Tooltip -> Create new Tooltip
                if (setting[0] == "Tooltip")
                {
                    //Read following positions and save the specific information in temporary variables

                    float tempX = float.Parse(setting[1]);
                    float tempY = float.Parse(setting[2]);
                    String message = "";

                    //Following array spaces where split but belong to message, no more settings
                    for (int i = 3; i < setting.Length; i++)
                    {
                        //Add to message
                        message = message + " " + setting[i];
                    }
                    
                    //Add tooltips to list
                    tooltips.Add(new Tooltip(new Vector2(tempX, tempY), message));
                }

                //If first position is Item -> Create new Item
                if (setting[0] == "Item")
                {                    
                    //Read following positions and save the specific information in temporary variables

                    float tempX = float.Parse(setting[1]);
                    float tempY = float.Parse(setting[2]);

                    int tempType = int.Parse(setting[3]);

                    //Instantiate new Item with type
                    EItem type;

                    switch(tempType)
                    {
                        case 0:
                            type = EItem.Diamond;
                            break;
                        case 1:
                            type = EItem.Key;
                            break;
                        default:
                            type = EItem.Diamond;
                            break;
                    }

                    //Add the item to the list
                    items.Add(new Item(type, new Vector2(tempX, tempY), content));

                }

                //If first position is Trigger -> Settings for Trigger Tile
                if (setting[0] == "Trigger")
                {
                    //Read following positions and save the specific information in temporary variables

                    int tempX = Int32.Parse(setting[1]);
                    int tempY = Int32.Parse(setting[2]);

                    String tempTargetID = setting[3];

                    //If Tile is TriggerTile
                    if (TileRoom[tempX, tempY] is TriggerTile)
                    {
                        TriggerTile tile = (TriggerTile)TileRoom[tempX, tempY];
                        //Give the tile its properties
                        tile.SetProperties(tempTargetID);
                    }
                }
                
                //If first position is Trigger -> Settings for Trigger Tile
                if (setting[0] == "Triggered")
                {
                    //Read following positions and save the specific information in temporary variables

                    int tempX = Int32.Parse(setting[1]);
                    int tempY = Int32.Parse(setting[2]);

                    String tempID = setting[3];
                    float tempTimerValue = float.Parse(setting[4]);

                    //If tile is a TriggeredTile
                    if (TileRoom[tempX, tempY] is TriggeredTile)
                    {
                        TriggeredTile tile = (TriggeredTile)TileRoom[tempX, tempY];
                        //Apply the properties
                        tile.SetProperties(tempID, tempTimerValue);
                        //Add Tiles to list, so timer can be updated
                        triggeredTiles.Add(tile);
                    }
                }

            }
            
        }

        
    }
}
