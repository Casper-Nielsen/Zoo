using System;
using System.Collections.Generic;
using System.Text;

namespace Zoo.Classes
{
    /// <summary>
    /// a data class for poop info
    /// </summary>
    class Poop
    {
        private int laid;

        public int Laid { get => laid; }

        public Poop(int laid)
        {
            this.laid = laid;
        }
    }
}
