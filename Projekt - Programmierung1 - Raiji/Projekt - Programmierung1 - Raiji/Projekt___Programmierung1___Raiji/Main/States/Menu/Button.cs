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
        private string text;

        Rectangle bounds;

        //MouseInput

        public event EventHandler<EventArgs> Click;

        public Button(string text, ContentManager content)
        {
            texture = content.Load<Texture2D>("Menu/button");
            hoverTexture = content.Load <Texture2D>("Menu/buttonPressed");
            this.text = text;
            bounds = texture.Bounds;
           
            
        }

        public void Update()
        {
            

            isHoveredOver = bounds.Contains(StateMachine.inputManager.GetMousePoint());

            if (isHoveredOver && StateMachine.inputManager.MouseClicked()) 
            {
                OnClick();
            }

           
        }
        
        public void Draw(SpriteBatch spriteBatch, SpriteFont spriteFont)
        {
            // TODO: Versuche, den HoverBegin und HoverEnd als Event abzufangen und nur dann den Button neuzuzeichnen
            if (isHoveredOver)
                spriteBatch.Draw(hoverTexture, position, Color.White);
            else
                spriteBatch.Draw(texture, position, Color.White);

            //writes the text
            spriteBatch.DrawString(spriteFont, text, position + new Vector2(texture.Width / 2f, texture.Height / 2f) - spriteFont.MeasureString(text) / 2f, Color.Black);
        }

        protected void OnClick() // TODO: Setze diese Mechanik lieber als Event um

        {
            if (Click != null)
            {
                Click(this, new EventArgs());
            }
                
        }

    }
}
