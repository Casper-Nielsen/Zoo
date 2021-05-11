using System;
using System.Threading;
using Zoo.Enums;
using Zoo.Interfaces;

namespace Zoo.Classes
{
    /// <summary>
    /// worker that will clean the pens
    /// </summary>
    class Worker : IThreadable
    {
        private Thread thread;
        private bool keepRunning;
        private Zoo zoo;

        public Worker(ref Zoo zoo)
        {
            this.zoo = zoo;
            thread = new Thread(new ParameterizedThreadStart(Run))
            {
                IsBackground = true,
                Name = "worker"
            };
            keepRunning = true;
        }

        /// <summary>
        /// starts the thread that will clean the pens
        /// </summary>
        public void Start()
        {
            thread.Start();
        }

        /// <summary>
        /// says stop to the thread
        /// </summary>
        public void Stop()
        {
            keepRunning = false;
        }

        /// <summary>
        /// The method that runs the pen itself
        /// Warning risk of while true do not use main thread
        /// </summary>
        /// <param name="run"></param>
        private void Run(object obj)
        {
            Program.Write("I started");
            Thread.Sleep((int)Math.Floor(30000 / Clock.GetSpeed()));
            do
            {
                int index = GetMostImporten();
                if (index > -1)
                {
                    RemovePoop(index);
                }
            } while (keepRunning);
        }

        /// <summary>
        /// gets the most importent pen
        /// </summary>
        /// <param name="importants">output the importants of the pen</param>
        /// <returns>the index of the pen</returns>
        private int GetMostImporten()
        {
            ImportantsEnum importants = ImportantsEnum.none;
            int index = -1;
            bool running = true;
            lock (zoo.AnimalPens)
            {

                for (int i = 0; i < zoo.AnimalPens.Count && running; i++)
                {
                    try
                    {

                        if (!zoo.AnimalPens[i].GetStatus())
                        {
                            // Checks if it is a giraffe
                            if (zoo.AnimalPens[i].GetType() == AnimalEnum.Giraffe)
                            {
                                // Checks if it is time to remove the poop
                                if (importants < ImportantsEnum.very && zoo.AnimalPens[i].GetPoopCount() > 0)
                                {
                                    index = i;
                                    importants = ImportantsEnum.very;
                                }
                            }
                            // Checks if it is a Elefant
                            else if (importants < ImportantsEnum.high && zoo.AnimalPens[i].GetType() == AnimalEnum.Elefant)
                            {
                                // Checks if is time to remove the poop
                                if (zoo.AnimalPens[i].GetPoopCount() > 0)
                                {
                                    index = i;
                                    importants = ImportantsEnum.high;
                                }
                            }
                            // Checks if it is a fox or wolf
                            else if (importants < ImportantsEnum.medium && (zoo.AnimalPens[i].GetType() == AnimalEnum.Fox || zoo.AnimalPens[i].GetType() == AnimalEnum.Wolf))
                            {
                                // Checks if it is time to remove the poop
                                if (((ICanidaePen)zoo.AnimalPens[i]).GetPoops().Count > 0)
                                {
                                    Poop poop = (((ICanidaePen)zoo.AnimalPens[i])).GetPoops()[0];
                                    if (poop != null && poop.Laid < (Clock.GetTime() - 12 * Clock.min))
                                    {
                                        index = i;
                                        importants = ImportantsEnum.medium;
                                    }
                                }
                            }
                            // Checks if it is a rabbit
                            else if (importants < ImportantsEnum.low && zoo.AnimalPens[i].GetType() == AnimalEnum.Rabbit)
                            {
                                // Checks if it is time to remove poop
                                if (importants < ImportantsEnum.very && zoo.AnimalPens[i].GetPoopCount() >= 90)
                                {
                                    index = i;
                                    importants = ImportantsEnum.low;
                                }
                            }
                        }
                    }
                    catch { }
                }
                if (index != -1)
                    zoo.AnimalPens[index].SetStatus(true);
            }
            return index;
        }

        /// <summary>
        /// removes poop from that pen
        /// </summary>
        /// <param name="index">the index of the pen</param>
        private void RemovePoop(int index)
        {

            Program.Write("Removing poop from " + zoo.AnimalPens[index].GetType());
            if (zoo.AnimalPens[index].GetType() == AnimalEnum.Giraffe || zoo.AnimalPens[index].GetType() == AnimalEnum.Elefant)
            {
                lock (zoo.AnimalPens[index])
                {
                    // If it is a giraffe or elefant just remove one poop
                    if (zoo.AnimalPens[index].GetPoopCount() > 0)
                    {
                        zoo.AnimalPens[index].RemovePoop();
                    }
                }
            }
            else if (zoo.AnimalPens[index].GetType() == AnimalEnum.Fox || zoo.AnimalPens[index].GetType() == AnimalEnum.Wolf)
            {
                lock (zoo.AnimalPens[index])
                {
                    // If it is a Fox or Wolf remove the oldest
                    if (zoo.AnimalPens[index].GetPoopCount() > 0)
                    {
                        ((ICanidaePen)zoo.AnimalPens[index]).RemovePoop(((ICanidaePen)zoo.AnimalPens[index]).GetPoops()[0]);
                    }
                }
            }
            else if (zoo.AnimalPens[index].GetType() == AnimalEnum.Rabbit)
            {
                lock (zoo.AnimalPens[index])
                {
                    // if it is a rabbit remove all the poop
                    if (zoo.AnimalPens[index].GetPoopCount() > 0)
                    {
                        ((IRabbitPen)zoo.AnimalPens[index]).ClearPoop();
                    }
                }
            }
            zoo.AnimalPens[index].SetStatus(false);
            Program.Write("Removed poop from " + zoo.AnimalPens[index].GetType());
        }
    }
}
