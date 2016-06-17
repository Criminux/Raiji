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
        
        ContentManager content;

        Room room;

        private Player player;

        public LevelManager(ContentManager content)
        {
            this.content = content;

            room = new Room(content);

            player = new Player(content);
            player.Position = new Vector2(960, 530);
        }
        

        public void Update(GameTime gameTime)
        {
            //Update Room
            room.Update();
            
            //Update Player
            player.Update(gameTime, room);

            //Input
            ExecuteInput(StateMachine.inputManager.GetInput(), gameTime);

            //AfterUpdatePlayer
            player.AfterUpdate(gameTime, room);

        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            //Draw Room
            room.Draw(spriteBatch);
            
            //Draw Character
            player.Draw(spriteBatch);


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
                }
            }

        }

    }
}
