using System;
using System.Collections.Generic;
using System.Text;
using Zoo.Classes;

namespace Zoo.Interfaces
{
    /// <summary>
    /// interface for a pen where the poop information is important
    /// </summary>
    interface ICanidaePen
    {
        public List<Poop> GetPoops();
        public void RemovePoop(Poop poop);
    }
}
