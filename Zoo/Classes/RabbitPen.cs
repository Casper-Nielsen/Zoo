using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Zoo.Enums;
using Zoo.Interfaces;

namespace Zoo.Classes
{
    /// <summary>
    /// Rabbit pen is close to the same as SimplePen but you can remove all poops at once
    /// </summary>
    class RabbitPen : SimplePen, IRabbitPen
    {
        public RabbitPen(int avgPoopPerDay, int timeToRemovePoop, AnimalEnum type) : base(avgPoopPerDay, timeToRemovePoop, type)
        {

        }
        public RabbitPen(int avgPoopPerDay, int timeToRemovePoop, AnimalEnum type, int randomSeed) : base(avgPoopPerDay, timeToRemovePoop, type, randomSeed)
        {

        }

        /// <summary>
        /// Setups the thread
        /// </summary>
        protected override void SetupThread()
        {
            thread = new Thread(new ParameterizedThreadStart(Run))
            {
                IsBackground = true,
                Name = "RabbitPen: " + type.ToString()
            };
        }

        /// <summary>
        /// Removes all poops
        /// Warning uses thread.sleep 
        /// </summary>
        public void ClearPoop()
        {
            if (poopCount > 0)
            {
                Thread.Sleep((int)Math.Floor(timeToRemovePoop / Clock.GetSpeed() * 1000));
                poopCount = 0;
            }
        }
    }
}
