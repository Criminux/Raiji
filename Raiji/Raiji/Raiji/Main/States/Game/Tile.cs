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
    public enum ETile
    {
        Background = 0,
        Stone = 1,
        Door = 11, //Teleportation
        Spike = 21,
        AcidTop = 31,
        AcidFull = 32,
        HealStation = 41,
        HealStationUsed = 42,
        DoorOpen = 51, //LevelFinish
        DoorLocked = 52, //LevelFinish
        Trigger = 61,
        Triggered = 62,
        Unspecified = 99

    }

    public enum ETileCollision
    {
        Passable = 0,
        Solid = 1,
        Unspecified = 99
    }

    public class Tile
    {
        //Tile Fields
        protected Texture2D texture;
        protected Rectangle bounds;
        protected ETile type;
        protected ETileCollision collision;
        protected Vector2 position;

        //Needed for some calculations
        public const int Height = 64;
        public const int Width = 64;

        //Tile Properties
        public Rectangle Bounds
        {
            get{ return bounds;}
        }
        public ETileCollision Collision
        {
            get { return collision; }
        }
        public ETile Type
        {
            get { return type; }
        }
        public Vector2 Position
        {
            get { return position; }
        }
        
        public Tile(ETile type, Vector2 position, ContentManager content)
        {
            switch(type)
            {
                case ETile.Stone:
                    texture = content.Load<Texture2D>("Tile/Stone");                
                    collision = ETileCollision.Solid;
                    break;
                case ETile.Background:
                    texture = content.Load<Texture2D>("Tile/Back");
                    collision = ETileCollision.Passable;
                    break;
                case ETile.Door:
                    texture = content.Load<Texture2D>("Tile/Back");
                    collision = ETileCollision.Passable;
                    break;
                case ETile.Spike:
                    texture = content.Load<Texture2D>("Tile/Spikes");
                    collision = ETileCollision.Passable;
                    break;
                case ETile.HealStation:
                    texture = content.Load<Texture2D>("Tile/HealStation");
                    collision = ETileCollision.Passable;
                    break;
                case ETile.HealStationUsed:
                    texture = content.Load<Texture2D>("Tile/HealStationUsed");
                    collision = ETileCollision.Passable;
                    break;
                case ETile.DoorOpen:
                    texture = content.Load<Texture2D>("Tile/DoorOpen");
                    collision = ETileCollision.Passable;
                    break;
                case ETile.DoorLocked:
                    texture = content.Load<Texture2D>("Tile/DoorLocked");
                    collision = ETileCollision.Passable;
                    break;
                case ETile.AcidTop:
                    texture = content.Load<Texture2D>("Tile/AcidTop");
                    collision = ETileCollision.Passable;
                    break;
                case ETile.AcidFull:
                    texture = content.Load<Texture2D>("Tile/AcidFull");
                    collision = ETileCollision.Passable;
                    break;
                case ETile.Trigger:
                    texture = content.Load<Texture2D>("Tile/Back");
                    collision = ETileCollision.Passable;
                    break;
                case ETile.Triggered:
                    texture = content.Load<Texture2D>("Tile/Stone");
                    collision = ETileCollision.Solid;
                    break;


            }

            this.type = type;
            this.position = position;
            bounds = texture.Bounds;
            bounds.Location = new Point((int)position.X, (int)position.Y);

        }

        public void DrawTile(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

    }
}
