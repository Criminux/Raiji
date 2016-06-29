using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Raiji.Main.States.Game
{
    class TriggerTile : Tile
    {
        private String targetID;
        public String TargetID
        {
            get { return targetID; }
        }

        public TriggerTile(ETile type, Vector2 position, ContentManager content) : base(type, position, content)
        {
        }

        public void SetProperties(String targetID)
        {
            this.targetID = targetID;
        }
    }
}
