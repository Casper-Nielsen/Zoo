using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Zoo.Enums;
using Zoo.Interfaces;

namespace Zoo.Classes
{
    class Zoo
    {
        private readonly List<IAnimalPen> animalPens;
        private readonly List<IThreadable> workers;
        private int openTime;
        private int closeTime;
        private bool keepRunning;
        private double satisfaction;
        private Thread thread;

        public List<IAnimalPen> AnimalPens { get => animalPens; }
        public List<IThreadable> Workers { get => workers; }

        public Zoo(int openTime, int closeTime)
        {
            animalPens = new List<IAnimalPen>();
            workers = new List<IThreadable>();
            this.openTime = openTime;
            this.closeTime = closeTime;
            keepRunning = true;
            satisfaction = 100;
            thread = new Thread(new ParameterizedThreadStart(Visitor))
            {
                Name = "Visitors",
                IsBackground = true
            };
            thread.Start();
        }

        /// <summary>
        /// addes a animalPen to the zoo
        /// </summary>
        /// <param name="animalPen">the pen to add</param>
        public void AddAnimalPen(IAnimalPen animalPen)
        {
            animalPen.Start();
            animalPens.Add(animalPen);
        }

        /// <summary>
        /// addes a worker to the zoo
        /// </summary>
        /// <param name="worker">the worker to add</param>
        public void AddWorker(IThreadable worker)
        {
            worker.Start();
            workers.Add(worker);
        }

        /// <summary>
        /// removes the animal pen at that index
        /// </summary>
        /// <param name="index">the index of the pen that will be removed</param>
        public void RemoveAnimalPen(int index)
        {
            if (index < animalPens.Count)
            {
                animalPens.RemoveAt(index);
            }
        }

        /// <summary>
        /// removes the animalpen
        /// </summary>
        /// <param name="animalPen">the animalpen that will be removed</param>
        public void RemoveAnimalPen(IAnimalPen animalPen)
        {
            if (animalPens.Contains(animalPen))
            {
                animalPens.Remove(animalPen);
            }
        }

        /// <summary>
        /// removes the worker at that index
        /// </summary>
        /// <param name="index">the index for the worker that will be removed</param>
        public void RemoveWorker(int index)
        {
            if (index < workers.Count)
            {
                workers.RemoveAt(index);
            }
        }

        /// <summary>
        /// removes the worker
        /// </summary>
        /// <param name="worker">the worker that will be removed</param>
        public void RemoveWorker(IThreadable worker)
        {
            if (workers.Contains(worker))
            {
                workers.Remove(worker);
            }
        }

        /// <summary>
        /// stops the zoo
        /// </summary>
        public void Stop()
        {
            foreach (IThreadable item in AnimalPens)
            {
                item.Stop();
            }
            foreach (IThreadable item in workers)
            {
                item.Stop();
            }
            keepRunning = false;
        }

        /// <summary>
        /// The method that runs the pen itself
        /// Warning risk of while true do not use main thread
        /// </summary>
        private void Visitor(object obj)
        {
            int time = 0;
            bool everythingIsFine = true;
            do
            {
                time = Clock.GetTime();
                if (openTime <= time && time <= closeTime)
                {
                    everythingIsFine = true;
                    for (int i = 0; i < animalPens.Count; i++)
                    {
                        if (CheckForPoop(animalPens[i]))
                        {
                            if (satisfaction > 0)
                            {
                                satisfaction -= 0.1;
                                everythingIsFine = false;
                            }
                        }
                    }
                    if (everythingIsFine)
                    {
                        if (satisfaction < 100)
                        {
                            satisfaction += 0.1;
                        }
                    }
                    Program.Write("satisfaction is " + satisfaction.ToString("0.##"));
                    Thread.Sleep((int)Math.Floor(10 * 1000 / Clock.GetSpeed()));
                }
                else
                {
                    satisfaction = 100;
                    Program.Write("satisfaction is " + satisfaction.ToString("0.##"));
                    Thread.Sleep((int)Math.Floor(Clock.min* 10 * 1000 / Clock.GetSpeed()));
                }
            } while (keepRunning);
        }

        private bool CheckForPoop(IAnimalPen animalPen)
        {
            // Checks if it is a giraffe
            if (animalPen.GetType() == AnimalEnum.Giraffe || animalPen.GetType() == AnimalEnum.Elefant)
            {
                // Checks if is time to remove the poop
                if (animalPen.GetPoopCount() > 0)
                {
                    return true;
                }
            }
            // Checks if it is a fox or wolf
            else if (animalPen.GetType() == AnimalEnum.Fox || animalPen.GetType() == AnimalEnum.Wolf)
            {
                // Checks if it is time to remove the poop
                if (((ICanidaePen)animalPen).GetPoops().Count > 0)
                {
                    if (((ICanidaePen)animalPen).GetPoops()[0].Laid < (Clock.GetTime() - 15 * Clock.min))
                    {
                        return true;
                    }
                }
            }
            // Checks if it is a rabbit
            else if (animalPen.GetType() == AnimalEnum.Rabbit)
            {
                // Checks if it is time to remove poop
                if (animalPen.GetPoopCount() > 99)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
