using System;
using System.Collections.Generic;
using System.Text;

namespace FlatsLib
{
    public interface IFlatsSource
    {
        IEnumerable<Flat> GetAll();
    }
}
