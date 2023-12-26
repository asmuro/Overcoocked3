using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Objects.Interfaces
{
    internal interface IChoppable
    {
        bool IsChoppable { get; }
        float ChopTotalActionTime { get; }
        float ChopActionTimeConsumed { get; }

        void SetChopActionTimeConsumed(float newChopActionTimeConsumed);        
    }
}
