using System;
using System.Collections.Generic;
using System.Text;
using Zoo.Enums;

namespace Zoo.Interfaces
{
    /// <summary>
    /// a simple animal pen where you can get basic information about poop levels
    /// </summary>
    interface IAnimalPen : IThreadable
    {
        public void SetStatus(bool workerIn);
        public bool GetStatus();
        public AnimalEnum GetType();
        public int GetPoopCount();
        public void RemovePoop();
    }
}
