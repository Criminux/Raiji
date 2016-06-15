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

namespace Projekt___Programmierung1___Raiji
{
    class Player : Character
    {
        public Player(ContentManager content)
        {
            characterSprite = content.Load<Texture2D>("Player");
            bounds = characterSprite.Bounds;
        }
        public void Update()
        {
            if(!isAlive(life))
            {
                //TODO is character dead
            }
        }

        public void DrawPlayer(SpriteBatch spriteBatch, Vector2 position) // TODO: Mach in der Base eine abstrakte (?) Methode, die der LevelManager dann von jedem Character aufruft und du dann in den Subclasses für jeden unterschiedlich implementieren kannst (in C++-Terminologie wäre es pure virtual, bin mir aber nichts sicher, was dafür das richtige Keyword in C# ist). Oder lege die Draw-Methode gleich in der Oberklasse fest und stelle dan für jeden Character eine Textur ein.
        {
            spriteBatch.Draw(characterSprite, position, Color.White);
        }

    }
}
