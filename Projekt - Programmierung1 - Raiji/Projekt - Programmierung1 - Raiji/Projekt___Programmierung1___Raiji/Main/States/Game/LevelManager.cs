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

namespace Projekt___Programmierung1___Raiji
{
    class LevelManager
    {

        InputManager inputManager;
        ContentManager content;

        Room room;

        private Player player;
        

        public LevelManager(InputManager inputManager, ContentManager content)
        {
            this.inputManager = inputManager;
            this.content = content;

            room = new Room(content);

            player = new Player(content);
            player.Position = new Vector2(960, 530);

        }
        

        public void Update()
        {
            //Update Room
            room.Update();

            //Update Player
            player.Update();
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            //Draw Room
            room.Draw(spriteBatch);
            
            //Draw Character
            player.Draw(spriteBatch);


        }

        
       
    }
}
