using System;
using Microsoft.Xna.Framework;

namespace Raiji
{
    class TimeManager
    {
        //TimeManager holds total Time
        private TimeSpan TotalTime;
        private TimeSpan SecondCount;
        private int seconds;
        public int Seconds
        {
            get { return seconds; }
        }

        public TimeManager()
        {
            //Instantiate TimeSpan
            TotalTime = new TimeSpan(0);
            SecondCount = new TimeSpan(0);
            seconds = 0;
        }

        public void UpdateTime(GameTime gameTime)
        {
            //Update the Time
            TotalTime += gameTime.ElapsedGameTime;

            SecondCount += gameTime.ElapsedGameTime;
            if(SecondCount.Seconds >= 1)
            {
                seconds++;
                SecondCount = new TimeSpan(0);
            }
            
        }
        public TimeSpan GetTotalTime()
        {
            //Return total time of this instance
            return TotalTime;
        }

    }
}
