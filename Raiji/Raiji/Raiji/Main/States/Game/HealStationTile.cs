using Raiji;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Raiji.Main.States.Game
{
    class HealStationTile : Tile
    {
        private ContentManager content;
        private bool isUsed;
        private SoundEffect useSound;
        private SoundEffect useDeniedSound;

        public HealStationTile(ETile type, Vector2 position, ContentManager content, bool isUsed) : base(type, position, content)
        {
            this.content = content;
            this.isUsed = isUsed;

            if(isUsed)
            {
                texture = content.Load<Texture2D>("Tile/HealStationUsed");
            }
            else
            {
                texture = content.Load<Texture2D>("Tile/HealStation");
            }

            useSound = content.Load<SoundEffect>("Sound/HealthStationFinal");
            useDeniedSound = content.Load<SoundEffect>("Sound/HealthStationUsedFinal");
        }

        public void Use()
        {
            if(!isUsed)
            {
                texture = content.Load<Texture2D>("Tile/HealStationUsed");
                useSound.Play(0.6f, 0, 0);
                isUsed = true;
            }
            else
            {
                useDeniedSound.Play(0.6f,0,0);
            }
        }

        public bool IsUsed
        {
            get { return isUsed; }
        }

    }
}
