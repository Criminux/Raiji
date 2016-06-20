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
using Projekt___Programmierung1___Raiji;

namespace Raiji.Main.States.Game
{
    class UIManager
    {
        Player player;
        int playerLife;

        Texture2D heart;

        public UIManager(Player player, ContentManager content)
        {
            this.player = player;
            heart = content.Load<Texture2D>("heart");
        }

        public void Update()
        {
            playerLife = player.Life;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw the Players life
            switch(playerLife)
            {
                case 3:
                    spriteBatch.Draw(heart, new Vector2(10, 970), Color.White); //TODO: 3 Vector2 anlegen mit festgestzten Koordinaten
                    spriteBatch.Draw(heart, new Vector2(84, 970), Color.White);
                    spriteBatch.Draw(heart, new Vector2(158, 970), Color.White);
                    break;
                case 2:
                    spriteBatch.Draw(heart, new Vector2(10, 970), Color.White); //TODO: 3 Vector2 anlegen mit festgestzten Koordinaten
                    spriteBatch.Draw(heart, new Vector2(84, 970), Color.White);
                    break;
                case 1:
                    spriteBatch.Draw(heart, new Vector2(10, 970), Color.White); //TODO: 3 Vector2 anlegen mit festgestzten Koordinaten
                    break;     
            }

        }

    }
}
