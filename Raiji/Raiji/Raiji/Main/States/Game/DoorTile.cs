using Raiji;
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
        //ID for identification
        private String ID;
        //Target ID for identification of the target Til
        private String targetID;
        //Target room for switching
        private int targetRoom;
        //Position where player will spawn when coming out of this door
        private Vector2 spawnPosition;

        //Properties for the fields
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

        //Set the properties for a door
        public void SetProperties(int targetRoom, String ID, String targetID, Vector2 targetPosition)
        {
            this.targetRoom = targetRoom;
            this.ID = ID;
            this.targetID = targetID;
            this.spawnPosition = targetPosition;
        }

    }
}
