using System;
using System.Collections.Generic;
using System.Text;

namespace Zoo.Interfaces
{
    /// <summary>
    /// a interface for a pen where you can remove all poop at one time
    /// </summary>
    interface IRabbitPen : IAnimalPen
    {
        public void ClearPoop();
    }
}
