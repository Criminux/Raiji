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


        public LevelManager(ContentManager content)
        {
            inputManager = StateMachine.inputManager;
            this.content = content;

            room = new Room(content);

            player = new Player(content);
            player.Position = new Vector2(960, 530);

        }
        

        public void Update()
        {
            //Input
            ExecuteInput(inputManager.GetInput());

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

        public void ExecuteInput(EInputKey[] inputs)
        {
            int size = inputs.Length;


            for (int i = 0; i < size; i++)
            {


                switch (inputs[i])
                {
                    case EInputKey.Right:
                        player.Position += new Vector2(5, 0);                        
                        break;
                    case EInputKey.Left:
                        player.Position -= new Vector2(5, 0);
                        break;
                    case EInputKey.Up:
                        player.Position -= new Vector2(0, 5);
                        break;
                    case EInputKey.Down:
                        player.Position += new Vector2(0, 5);
                        break;
                    default:
                        break;
                }


            }
            
        }

    }
}
