using System;
using System.Collections.Generic;
using System.Threading;
using Zoo.Enums;
using Zoo.Interfaces;

namespace Zoo.Classes
{
    /// <summary>
    /// a pen that contains more information about the poop
    /// </summary>
    class CanidaePen : SimplePen, ICanidaePen
    {
        private List<Poop> poops;

        public CanidaePen(int avgPoopPerDay, int timeToRemovePoop, AnimalEnum type) : base(avgPoopPerDay, timeToRemovePoop, type)
        {
            poops = new List<Poop>();
        }
        public CanidaePen(int avgPoopPerDay, int timeToRemovePoop, AnimalEnum type, int randomSeed) : base(avgPoopPerDay, timeToRemovePoop, type, randomSeed)
        {
            poops = new List<Poop>();
        }

        /// <summary>
        /// Setups the thread
        /// </summary>
        protected override void SetupThread()
        {
            thread = new Thread(new ParameterizedThreadStart(Run))
            {
                IsBackground = true,
                Name = "CanidaePen: " + type.ToString()
            };
        }

        /// <summary>
        /// gets the poop list
        /// </summary>
        /// <returns>a list of poops</returns>
        public List<Poop> GetPoops()
        {
            return poops;
        }

        /// <summary>
        /// removes the poop
        /// Warning uses Thread.sleep
        /// </summary>
        /// <param name="poop">the poop to remove</param>
        public void RemovePoop(Poop poop)
        {
            if (poops.Contains(poop))
            {
                Thread.Sleep((int)Math.Floor(timeToRemovePoop / Clock.GetSpeed() * 1000));
                poops.Remove(poop);
                poopCount--;
            }
        }

        /// <summary>
        /// The method that runs the pen itself
        /// Warning risk of while true do not use main thread
        /// </summary>
        protected override void Run(object obj)
        {
            int avgBetweenPoop = Clock.day / avgPoopPerDay;

            Program.Write("I started");
            do
            {
                double delay = (avgBetweenPoop - random.Next(avgBetweenPoop * -1, avgBetweenPoop)) * 1000 / Clock.GetSpeed();
                Thread.Sleep((int)Math.Floor(delay));
                poopCount++;
                poops.Add(new Poop(Clock.GetTime()));
                Program.Write("I pooped");

            } while (KeepRunning);
        }
    }
}
