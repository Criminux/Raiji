using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Raiji.Main.States.Game;
using System.Collections.Generic;

namespace Projekt___Programmierung1___Raiji.Main.States.Game
{
    public class Room
    {
        private ContentManager content;

        const int RowX = 30;
        const int RowY = 15;
        
        private Tile[,] TileRoom = new Tile[RowX, RowY];
        public Tile[,] tileRoom
        {
            get { return TileRoom; }
        }

        //Für eventuelle Tooltips
        private List<Tooltip> tooltips;

        //Für eventuelle Gegner
        private List<Enemy> enemies;
        public List<Enemy> Enemies
        {
            get { return enemies; }
        }

        private Vector2 playerPosition;

        private int levelID;
        private int roomID;

        public int RoomID
        {
            get { return roomID; }
            set { roomID = value; }
        }

        StreamReader reader;

        public Room(ContentManager content, int levelID, int roomID)
        {
            this.content = content;

            this.levelID = levelID;
            this.roomID = roomID;

            enemies = new List<Enemy>();
            tooltips = new List<Tooltip>();


            Initialize(levelID, roomID);
        }
        
        public void Update(GameTime gameTime, LevelManager level, Vector2 playerPosition)
        {
            this.playerPosition = playerPosition;

            foreach(Enemy tempEnemy in enemies)
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

        }

        public int GetCloseEnemyLife()
        {
            float closeDistance = 500f;
            int enemyLife = 0;

            foreach(Enemy tempEnemy in enemies)
            {
                float tempDistance = Vector2.Distance(playerPosition, tempEnemy.Position);
                if(tempDistance < closeDistance)
                {
                    closeDistance = tempDistance;
                    enemyLife = tempEnemy.Life;
                }
            }

            return enemyLife;
        }


        //Initialize new Room
        private void Initialize(int levelID, int roomID) 
        {
           
            this.levelID = levelID;
            this.roomID = roomID;
            
            int yCount = 0;

            reader = new StreamReader("Content/LevelData/Level" + levelID + "Room" + roomID + ".txt");

            //Fill the standard roster
            while (yCount < 15)
            {

                
                String tempLine = reader.ReadLine();
                String[] IDs = tempLine.Split(' ');
                
                for(int i = 0; i < IDs.Length; i++)
                {
                    

                    int[] ID = new int[IDs.Length];
                    ID[i] = Int32.Parse(IDs[i]);

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
                            TileRoom[i, yCount] = new Tile(ETile.HealStation, new Vector2(i * Tile.Width, yCount * Tile.Height), content);
                            break;
                        case 6:
                            TileRoom[i, yCount] = new Tile(ETile.HealStationUsed, new Vector2(i * Tile.Width, yCount * Tile.Height), content);
                            break;
                        case 7:
                            TileRoom[i, yCount] = new Tile(ETile.DoorOpen, new Vector2(i * Tile.Width, yCount * Tile.Height), content);
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

                yCount++;

                
            }

            //Read more lines for DoorTile Linking and enemy initialization
            String Line;
            while((Line = reader.ReadLine()) != null)
            {
                String[] setting = Line.Split(' ');

                if (setting[0] == "Property")
                {
                    int tempX = Int32.Parse(setting[1]);
                    int tempY = Int32.Parse(setting[2]);

                    int tempTargetRoom = Int32.Parse(setting[3]);
                    tempTargetRoom--;

                    String tempID = setting[4];
                    String tempTargetID = setting[5];

                    float spawnPositionX = float.Parse(setting[6]);
                    float spawnPositionY = float.Parse(setting[7]);
                    Vector2 spawnPosition = new Vector2(spawnPositionX, spawnPositionY);

                    if (TileRoom[tempX, tempY] is DoorTile)
                    {
                        DoorTile tile = (DoorTile)TileRoom[tempX, tempY];
                        tile.SetProperties(tempTargetRoom, tempID, tempTargetID, spawnPosition);
                    }
                    

                    
                }

                if (setting[0] == "Enemy")
                {
                    
                    float tempX = float.Parse(setting[1]);
                    float tempY = float.Parse(setting[2]);

                    //Neuer Enemy initialisieren
                    Enemy tempEnemy = new Enemy(content);
                    tempEnemy.Position = new Vector2(tempX,tempY);

                    enemies.Add(tempEnemy);

                }

                if(setting[0] == "Tooltip")
                {
                    float tempX = float.Parse(setting[1]);
                    float tempY = float.Parse(setting[2]);
                    String message = "";

                    for (int i = 3; i < setting.Length; i++)
                    {
                        message = message + " " + setting[i];
                    }
                    

                    tooltips.Add(new Tooltip(new Vector2(tempX, tempY), message));
                }


            }
            
        }

    }
}
