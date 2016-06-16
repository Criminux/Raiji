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
using System.IO;

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
        

        private bool levelDone;
        private bool isInitialized;

        private int LevelID;
        private int RoomID;

        StreamReader reader;

        public Room(ContentManager content)
        {
            this.content = content;

            levelDone = false;
            isInitialized = false;

            LevelID = 1;
            RoomID = 1;

            LoadContent();
        }
        void LoadContent()
        {
            reader = new StreamReader("Content/LevelData/Level1Room1.txt");
        }

        public void Update()
        {
            if (!isInitialized) { Initialize(LevelID, RoomID); isInitialized = true; }
            if (levelDone) { isInitialized = false; LevelID++; RoomID = 1; }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for(int i = 0; i < RowX; i++)
            {
                for(int j = 0; j < RowY; j++)
                {
                    TileRoom[i, j].DrawTile(spriteBatch, new Vector2((64*i),(64*j)));
                }
            }
        }

        private void Initialize(int LevelID, int RoomID) 
        {
            this.LevelID = LevelID;
            this.RoomID = RoomID;

            

            switch (LevelID) // TODO: Definiere dir Levels in einer LevelCollection oder einer externen Datei. Weis ihm eine Id zu und gib dieser Funtktion dann die Id, damit sie sich das zugehörige Level aus der Collection raussucht und InitializeLevel() übergibt, was dann das Level in eine Instanz der Level-Klasse kopiert.
            {
                case 1:
                    switch(RoomID)
                    {
                        case 1: InitializeLevel(LevelID, RoomID);

                            break;
                    }
                    break;
                case 2:
                    break;

            }
        }

        private void InitializeLevel(int LevelID, int RoomID) 
        {
           
            this.LevelID = LevelID;
            this.RoomID = RoomID;
            
            int yCount = 0;
            
            while (yCount < 15)
            {

                
                String Line = reader.ReadLine();
                String[] IDs = Line.Split(' ');
                
                for(int i = 0; i < IDs.Length; i++)
                {
                    

                    int[] ID = new int[IDs.Length];
                    ID[i] = Int32.Parse(IDs[i]);

                    switch(ID[i])
                    {
                        case 0:
                            TileRoom[i, yCount] = new Tile(ETile.Background, content);
                            break;
                        case 1:
                            TileRoom[i, yCount] = new Tile(ETile.Stone, content);
                            break;
                        case 99:
                            TileRoom[i, yCount] = new Tile(ETile.Unspecified, content);
                            break;
                    }
                }

                yCount++;

                
            }
            
        }

    }
}
