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
using Raiji;
using Raiji.Main.States.Game;

namespace Raiji.Main.States.Game
{
    class UIManager
    {
        //Get the Player instance for printing health
        Player player;
        int playerLife;
        int playerPoints;

        //Get the Enenmy instance for printing health
        int enemyLife;

        //For printing Time
        int totalSeconds;
        const int maximumTime = 100;

        //Texture for UI
        Texture2D heart;
        Texture2D key;

        //Vector2s for correct Draw-Position
        Vector2 playerHeart1;
        Vector2 playerHeart2;
        Vector2 playerHeart3;

        Vector2 enemyHeart1;
        Vector2 enemyHeart2;
        Vector2 enemyHeart3;

        Vector2 pointsLocation;
        Vector2 keyTextLocation;
        Vector2 keyTextureLocation;

        Vector2 timeTextLocation;
        Vector2 timeLocation;

        public UIManager(Player player, ContentManager content)
        {
            //Set the instance
            this.player = player;

            //Load all Textures for the UI
            heart = content.Load<Texture2D>("heart");
            key = content.Load<Texture2D>("Item/Key");

            //Set the Vector Coordinates
            playerHeart1 = new Vector2(10, 970);
            playerHeart2 = new Vector2(84, 970);
            playerHeart3 = new Vector2(158, 970);

            enemyHeart1 = new Vector2(1698, 970);
            enemyHeart2 = new Vector2(1772, 970);
            enemyHeart3 = new Vector2(1846, 970);

            pointsLocation = new Vector2(500, 990);
            keyTextLocation = new Vector2(800, 990);
            keyTextureLocation = new Vector2(870, 970);

            timeTextLocation = new Vector2(1100, 990);
            timeLocation = new Vector2(1350, 990);
        }

        public void Update(Room room, int totalSeconds)
        {
            //Update all  values for the Draw
            playerLife = player.Life;
            enemyLife = room.GetCloseEnemyLife();
            playerPoints = player.Points;
            this.totalSeconds = totalSeconds;

        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            //Draw the player life
            if(playerLife >= 1) spriteBatch.Draw(heart, playerHeart1, Color.White);
            if(playerLife >= 2) spriteBatch.Draw(heart, playerHeart2, Color.White);
            if(playerLife >= 3) spriteBatch.Draw(heart, playerHeart3, Color.White);

            //Draw the Enemy life
            if(enemyLife >= 1) spriteBatch.Draw(heart, enemyHeart1, Color.White);
            if(enemyLife >= 2) spriteBatch.Draw(heart, enemyHeart2, Color.White);
            if(enemyLife >= 3) spriteBatch.Draw(heart, enemyHeart3, Color.White);
            
            //Draw Points
            spriteBatch.DrawString(spriteFont, "Points: " + playerPoints.ToString(), pointsLocation, Color.White);

            //Draw Key
            spriteBatch.DrawString(spriteFont, "Key: ", keyTextLocation, Color.White);
            if (player.HasKey) spriteBatch.Draw(key, keyTextureLocation, Color.White);

            //Draw Time
            spriteBatch.DrawString(spriteFont, "Remaining Time: ", timeTextLocation, Color.White);
            //Calculate remaining Time
            int tempPrintTime = maximumTime - totalSeconds;
            spriteBatch.DrawString(spriteFont, tempPrintTime.ToString(), timeLocation, Color.White);
        }

    }
}
