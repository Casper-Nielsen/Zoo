using System;
using Zoo.Enums;
using Zoo.Interfaces;
using Zoo.Classes;
using System.Threading;

namespace Zoo
{
    class Program
    {
        static object consoleLock = new object();
        static void Main(string[] args)
        {
            Console.WriteLine("first test");

            // starts the clock
            Clock.SetSpeed(10);
            Clock.Start();

            // creates the zoo
            Classes.Zoo zoo = new Classes.Zoo(8*Clock.hour, 20*Clock.hour);
            // addes workers
            zoo.AddWorker(new Worker(ref zoo));
            zoo.AddWorker(new Worker(ref zoo));
            // addes pens
            zoo.AddAnimalPen(new SimplePen(6*12, 4*Clock.min, AnimalEnum.Giraffe, 1));
            zoo.AddAnimalPen(new SimplePen(15*6, 6*Clock.min, AnimalEnum.Elefant, 2));
            zoo.AddAnimalPen(new CanidaePen(4*20, 2*Clock.min, AnimalEnum.Wolf, 3));
            zoo.AddAnimalPen(new CanidaePen(4*20, 2*Clock.min, AnimalEnum.Fox, 4));
            zoo.AddAnimalPen(new RabbitPen(48*50, 10*Clock.min, AnimalEnum.Rabbit, 5));

            Console.ReadLine();
            // stops the zoo
            zoo.Stop();
            Clock.Stop();
        }

        /// <summary>
        /// writes to the line to the console
        /// </summary>
        /// <param name="msg">the message</param>
        /// <param name="pos">the line</param>
        public static void WriteToPos(string msg, int pos)
        {
            lock (consoleLock)
            {
                Console.SetCursorPosition(0, pos);
                Console.WriteLine("[{0}] {1} \t \t \t", pos, msg);
            }
        }

        /// <summary>
        /// writes to the console uses the thread id to select the line
        /// </summary>
        /// <param name="msg">the message</param>
        public static void Write(string msg)
        {
            lock (consoleLock)
            {
                Console.SetCursorPosition(0, Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine("[{0}] {1} {2} msg: {3} \t \t \t", Thread.CurrentThread.ManagedThreadId, Clock.GetTimeString(), Thread.CurrentThread.Name, msg);
            }
        }
    }
}
