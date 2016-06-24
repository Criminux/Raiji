using Projekt___Programmierung1___Raiji;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Raiji.Main.States.Game
{
    class DoorTile : Tile
    {

        private String ID;
        private String targetID;
        private int targetRoom;
        private Vector2 spawnPosition;

        public int TargetRoom
        {
            get { return targetRoom; }
        }
        public String GetID
        {
            get { return ID; }
        }
        public String GetTargetID
        {
            get { return targetID; }
        }
        public Vector2 SpawnPosition
        {
            get { return spawnPosition; }
        }


        public DoorTile(ETile type, Vector2 position, ContentManager content) : base(type, position, content)
        {


        }

        public void SetProperties(int targetRoom, String ID, String targetID, Vector2 targetPosition)
        {
            this.targetRoom = targetRoom;
            this.ID = ID;
            this.targetID = targetID;
            this.spawnPosition = targetPosition;
        }

    }
}
