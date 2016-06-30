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
    class TimeManager
    {
        //TimeManager holds total Time
        private TimeSpan TotalTime;

        public TimeManager()
        {
            //Instantiate TimeSpan
            TotalTime = new TimeSpan(0);
        }

        public void UpdateTime(GameTime gameTime)
        {
            //Update the Time
            TotalTime += gameTime.ElapsedGameTime;
        }
        public TimeSpan GetTotalTime()
        {
            //Return total time of this instance
            return TotalTime;
        }

    }
}
