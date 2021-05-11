using System;
using System.Collections.Generic;
using System.Text;

namespace Zoo.Interfaces
{
    /// <summary>
    /// a interface to enable threading
    /// </summary>
    interface IThreadable
    {
        public void Start();
        public void Stop();
    }
}
