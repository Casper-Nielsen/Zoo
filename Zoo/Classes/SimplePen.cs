using System;
using System.Threading;
using Zoo.Enums;
using Zoo.Interfaces;

namespace Zoo.Classes
{
    /// <summary>
    /// a simple pen that will count the poop up and enables so you can remove one at a time
    /// </summary>
    class SimplePen : IAnimalPen
    {
        protected AnimalEnum type;
        protected int poopCount;
        protected int avgPoopPerDay;
        protected int timeToRemovePoop;
        protected Random random;
        protected Thread thread;
        protected bool KeepRunning;
        protected bool workerIn;

        public SimplePen(int avgPoopPerDay, int timeToRemovePoop, AnimalEnum type)
        {
            this.avgPoopPerDay = avgPoopPerDay;
            this.timeToRemovePoop = timeToRemovePoop;
            this.type = type;
            random = new Random();
            this.KeepRunning = true;
            SetupThread();
        }

        public SimplePen(int avgPoopPerDay, int timeToRemovePoop, AnimalEnum type, int randomSeed)
        {
            this.avgPoopPerDay = avgPoopPerDay;
            this.timeToRemovePoop = timeToRemovePoop;
            this.type = type;
            this.KeepRunning = true;
            random = new Random(randomSeed);
            SetupThread();
        }

        /// <summary>
        /// Setups the thread
        /// </summary>
        protected virtual void SetupThread()
        {
            thread = new Thread(new ParameterizedThreadStart(Run))
            {
                IsBackground = true,
                Name = "SimplePen: " + type.ToString()
            };

        }

        /// <summary>
        /// Gets the poop count
        /// </summary>
        /// <returns>the poop count</returns>
        public int GetPoopCount()
        {
            return poopCount;
        }

        /// <summary>
        /// Removes 1 poop
        /// Warning uses thread.sleep 
        /// </summary>
        public void RemovePoop()
        {
            if (poopCount > 0)
            {
                int sleeptime = (int)Math.Floor(timeToRemovePoop * 1000 / Clock.GetSpeed());
                Thread.Sleep(sleeptime);
                poopCount--;
            }
        }

        /// <summary>
        /// starts the Thread that runs the pen
        /// </summary>
        public void Start()
        {
            thread.Start();
        }

        /// <summary>
        /// Stops the thread that runs the pen
        /// </summary>
        public void Stop()
        {
            KeepRunning = false;
        }

        /// <summary>
        /// Gets the type of the pen
        /// </summary>
        /// <returns>the type</returns>
        public new AnimalEnum GetType()
        {
            return type;
        }


        /// <summary>
        /// The method that runs the pen itself
        /// Warning risk of while true do not use main thread
        /// </summary>
        protected virtual void Run(object obj)
        {
            int avgBetweenPoop = Clock.day / avgPoopPerDay;

            Program.Write("I started");
            do
            {
                double delay = (avgBetweenPoop - random.Next(avgBetweenPoop * -1, avgBetweenPoop)) * 1000 / Clock.GetSpeed();
                Thread.Sleep((int)Math.Floor(delay));
                poopCount++;
                Program.Write("I pooped");

            } while (KeepRunning);
        }

        /// <summary>
        /// sets the status of the pen
        /// </summary>
        /// <param name="workerIn">if there is a worker in it</param>
        public void SetStatus(bool workerIn)
        {
            this.workerIn = workerIn;
        }


        /// <summary>
        /// gets the status
        /// </summary>
        /// <returns>if there is a worker in it</returns>
        public bool GetStatus()
        {
            return workerIn;
        }
    }
}
