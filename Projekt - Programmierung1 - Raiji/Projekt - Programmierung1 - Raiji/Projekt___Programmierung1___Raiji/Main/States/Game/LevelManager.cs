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
    public class LevelManager
    {
        
        ContentManager content;
        private bool levelDone;
        Room[] room;
        private int levelID;
        private int activeRoom;
        public int ActiveRoom
        {
            set { activeRoom = value; }
        }

        private bool isInitialized;

        private Player player;

        public LevelManager(ContentManager content)
        {
            this.content = content;
            levelDone = false;
            levelID = 1;
            activeRoom = 0;
            isInitialized = false; 
            
            player = new Player(content);
            player.Position = new Vector2(200, 500);
        }
        

        public void Update(GameTime gameTime)
        {
            if(!isInitialized)
            {
                InitializeLevel(levelID);
            }

            //Update Room
            room[activeRoom].Update(gameTime, this);

            //Update Player
            player.Update(gameTime, room[activeRoom]);

            //Input
            ExecuteInput(StateMachine.inputManager.GetInput(), gameTime);

            //AfterUpdatePlayer
            player.AfterUpdate(gameTime, room[activeRoom], this);

        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            //Draw Room, including Tiles, Enemies and other Items
            room[activeRoom].Draw(spriteBatch);
            
            //Draw Character
            player.Draw(spriteBatch);

        }

        public void InitializeLevel(int levelID)
        {
            if(levelID == 1)
            {
                room = new Room[5];
                for(int i = 0; i < room.Length; i++)
                {
                    room[i] = new Room(content, levelID, i + 1);
                }
            }

            isInitialized = true;
        }
        

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
                        player.Attack();
                        break;
                }
            }

        }

    }
}
