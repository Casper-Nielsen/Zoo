using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace Zoo.Classes
{
    /// <summary>
    /// the clock for the simulation
    /// </summary>
    class Clock
    {
        public static int day = 24 * 60 * 60;
        public static int hour = 60 * 60;
        public static int min = 60;
        private static int time;
        private static Timer timer;
        private static double speed;

        /// <summary>
        /// gets the time
        /// </summary>
        /// <returns>the time in sec</returns>
        public static int GetTime()
        {
            return time;
        }

        public static string GetTimeString()
        {
            return string.Format("{0}:{1}:{2}", (time - time % (hour)) / (hour), (time % (hour) - time % min) / min, time % min);
        }

        public static double GetSpeed()
        {
            return speed;
        }

        /// <summary>
        /// sets the speed of the simulation
        /// </summary>
        /// <param name="msPerSec"></param>
        public static void SetSpeed(int msPerSec)
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }
            timer = new Timer(msPerSec);
            timer.Elapsed += Timer_Elapsed;
            speed = 1000 / msPerSec;
        }

        /// <summary>
        /// addeds one to the timer and sets it to 0 if it is over 24hour
        /// </summary>
        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            time++;
            if (time > day)
            {
                time = 0;
            }
            Program.WriteToPos(GetTimeString(),2);
        }

        /// <summary>
        /// starts the timer
        /// </summary>
        public static void Start()
        {
            if (timer != null)
            {
                timer.Start();
            }
        }

        /// <summary>
        /// stops the timer
        /// </summary>
        public static void Stop()
        {
            if (timer != null)
            {
                timer.Stop();
            }
        }
    }
}
