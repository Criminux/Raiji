using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Raiji.Main.States.Game;

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

        //Für eventuelle Gegner
        Enemy[] enemies;


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

            Initialize(levelID, roomID);
        }
        
        
        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw the Tiles
            for(int i = 0; i < RowX; i++)
            {
                for(int j = 0; j < RowY; j++)
                {
                    TileRoom[i, j].DrawTile(spriteBatch);
                }
            }

            //Draw the Enemies
            for(int i = 0; i < enemies.Length; i++)
            {
                try //TODO: Make the Arraysize dynamic
                {
                    enemies[i].Draw(spriteBatch);
                }
                catch (Exception e) { }
            }

        }

        

        //Initialize next Room
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
                        case 5:
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
            int enemyCount = 0;
            //Create the Array
            enemies = new Enemy[5]; //TODO: Dynamic
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

                    if (TileRoom[tempX, tempY] is DoorTile)
                    {
                        DoorTile tile = (DoorTile)TileRoom[tempX, tempY];
                        tile.SetProperties(tempTargetRoom, tempID, tempTargetID);
                    }
                    

                    
                }

                if (setting[0] == "Enemy")
                {
                    

                    int tempX = Int32.Parse(setting[1]);
                    int tempY = Int32.Parse(setting[2]);

                    //Neuer Enemy initialisieren
                    enemies[enemyCount] = new Enemy(content);
                    enemies[enemyCount].Position = new Vector2(tempX,tempY);

                    enemyCount++;
                }


            }
            
        }

    }
}
