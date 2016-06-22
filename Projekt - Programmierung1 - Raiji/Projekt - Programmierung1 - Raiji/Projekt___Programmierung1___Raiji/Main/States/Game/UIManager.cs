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
using Projekt___Programmierung1___Raiji.Main.States.Game;

namespace Raiji.Main.States.Game
{
    class UIManager
    {
        //Get the Player instance for printing health
        Player player;
        int playerLife;

        //Get the Enenmy instance for printing health
        Enemy enemy;
        int enemyLife;

        //Texture for UI
        Texture2D heart;


        //Vector2s for correct Draw-Position
        Vector2 playerHeart1;
        Vector2 playerHeart2;
        Vector2 playerHeart3;

        Vector2 enemyHeart1;
        Vector2 enemyHeart2;
        Vector2 enemyHeart3;

        public UIManager(Player player, ContentManager content)
        {
            //Set the instance
            this.player = player;
            //this.enemy = enemy;

            //Load all Textures for the UI
            heart = content.Load<Texture2D>("heart");

            //Set the Vector Coordinates
            playerHeart1 = new Vector2(10, 970);
            playerHeart2 = new Vector2(84, 970);
            playerHeart3 = new Vector2(158, 970);

            enemyHeart1 = new Vector2(1698, 970);
            enemyHeart2 = new Vector2(1772, 970);
            enemyHeart3 = new Vector2(1846, 970);
        }

        public void Update(Room room)
        {
            //Get the current Life
            playerLife = player.Life;
            enemyLife = room.EnemyLife;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //Draw the Players life
            switch(playerLife)
            {
                case 3:
                    spriteBatch.Draw(heart, playerHeart1, Color.White); //TODO: 3 Vector2 anlegen mit festgestzten Koordinaten
                    spriteBatch.Draw(heart, playerHeart2, Color.White);
                    spriteBatch.Draw(heart, playerHeart3, Color.White);
                    break;
                case 2:
                    spriteBatch.Draw(heart, playerHeart1, Color.White); //TODO: 3 Vector2 anlegen mit festgestzten Koordinaten
                    spriteBatch.Draw(heart, playerHeart2, Color.White);
                    break;
                case 1:
                    spriteBatch.Draw(heart, playerHeart1, Color.White); //TODO: 3 Vector2 anlegen mit festgestzten Koordinaten
                    break;     
            }
            //Draw the Enemy life
            switch (enemyLife)
            {
                case 3:
                    spriteBatch.Draw(heart, enemyHeart1, Color.White); //TODO: 3 Vector2 anlegen mit festgestzten Koordinaten
                    spriteBatch.Draw(heart, enemyHeart2, Color.White);
                    spriteBatch.Draw(heart, enemyHeart3, Color.White);
                    break;
                case 2:
                    spriteBatch.Draw(heart, enemyHeart1, Color.White); //TODO: 3 Vector2 anlegen mit festgestzten Koordinaten
                    spriteBatch.Draw(heart, enemyHeart2, Color.White);
                    break;
                case 1:
                    spriteBatch.Draw(heart, enemyHeart1, Color.White); //TODO: 3 Vector2 anlegen mit festgestzten Koordinaten
                    break;
                case 0:
                    break;
            }
        }

    }
}
