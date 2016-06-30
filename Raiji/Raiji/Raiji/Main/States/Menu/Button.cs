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

namespace Raiji
{

    class Button
    {
        //Textures for the Button
        private Texture2D texture;
        private Texture2D hoverTexture;

        //Position from Button
        private Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                bounds.X = (int)position.X;
                bounds.Y = (int)position.Y;
            }
        }

        //Is the Mouse hovering over the Button
        private bool isHoveredOver;
        //Text from the Button
        private string text;
        //Button Bounds
        Rectangle bounds;

        //Click Event
        public event EventHandler<EventArgs> Click;

        public Button(string text, ContentManager content)
        {
            //Button loads its texture and hovertexture
            texture = content.Load<Texture2D>("Menu/button");
            hoverTexture = content.Load <Texture2D>("Menu/buttonPressed");
            //Save information and set bounds
            this.text = text;
            bounds = texture.Bounds;
           
            
        }

        public void Update()
        {
            //Set isHoveredOver bool
            isHoveredOver = bounds.Contains(StateMachine.inputManager.GetMousePoint());

            //If isHovered and Mouse is clicked
            if (isHoveredOver && StateMachine.inputManager.MouseClicked()) 
            {
                //Call Onclick Method
                OnClick();
            }

           
        }
        
        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            //Draw correct texture
            if (isHoveredOver)
                spriteBatch.Draw(hoverTexture, position, Color.White);
            else
                spriteBatch.Draw(texture, position, Color.White);

            //Write the text
            spriteBatch.DrawString(spriteFont, text, position + new Vector2(texture.Width / 2f, texture.Height / 2f) - spriteFont.MeasureString(text) / 2f, Color.Black);
        }

        protected void OnClick()
        {   
            //Click Event
            Click(this, new EventArgs());
        }

    }
}
