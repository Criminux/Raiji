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
    class LevelManager
    {

        InputManager inputManager;
        ContentManager content;

        Room room;

        private Player player;
        private bool needGravity;

        private CollisionManager collisionManager = new CollisionManager();

        public LevelManager(ContentManager content)
        {
            inputManager = StateMachine.inputManager;
            this.content = content;

            room = new Room(content);

            player = new Player(content);
            player.Position = new Vector2(960, 530);

            needGravity = true;
        }
        

        public void Update()
        {
            //Update Room
            room.Update();

            //Input
            ExecuteInput(inputManager.GetInput());

            //Check Collision
            CheckCollision();
            
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

        private void CheckCollision()
        {
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    Tile temptile = room.tileRoom[i, j];
                    if (temptile.ID == ETile.Stone)
                    {
                        if (collisionManager.Colliding(player.bounds, temptile.bounds))
                        {
                            needGravity = false;
                        }
                        else { needGravity = true; }
                    }

                }
            }

        }


        public void ExecuteInput(EInputKey[] inputs)
        {
            int size = inputs.Length;


            for (int i = 0; i < size; i++)
            {
                needGravity = true;

                switch (inputs[i])
                {
                    case EInputKey.Right:
                        player.Position += new Vector2(5, 0);
                        
                        break;
                    case EInputKey.Left:
                        player.Position -= new Vector2(5, 0);
                        
                        break;
                    case EInputKey.Jump:
                        player.Position -= new Vector2(0, 20);
                        needGravity = false;
                        break;
                    
                    default:
                        break;
                }


            }
            if(needGravity)
                player.Position += new Vector2(0, 5);

        }

    }
}
