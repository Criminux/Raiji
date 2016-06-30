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
        //HealStation needs Soundeffect
        private SoundEffect useSound;
        private SoundEffect useDeniedSound;

        public HealStationTile(ETile type, Vector2 position, ContentManager content, bool isUsed) : base(type, position, content)
        {
            //Save content instance for later changes
            this.content = content;
            this.isUsed = isUsed;

            //Load correct texture
            if(isUsed)
            {
                texture = content.Load<Texture2D>("Tile/HealStationUsed");
            }
            else
            {
                texture = content.Load<Texture2D>("Tile/HealStation");
            }

            //Load sound
            useSound = content.Load<SoundEffect>("Sound/HealthStationFinal");
            useDeniedSound = content.Load<SoundEffect>("Sound/HealthStationUsedFinal");
        }

        public void Use()
        {
            //If healstation is not used
            if(!isUsed)
            {
                //Update its texture to used and play the success sound
                texture = content.Load<Texture2D>("Tile/HealStationUsed");
                useSound.Play(0.6f, 0, 0);
                isUsed = true;
            }
            else
            {
                //Is was used already play denied sound
                useDeniedSound.Play(0.6f,0,0);
            }
        }

        //Property for State of station
        public bool IsUsed
        {
            get { return isUsed; }
        }

    }
}
