using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Objects.Interfaces
{
    internal interface IGrabable
    {
        bool IsBeingGrabbed { get; set; }
        event EventHandler OnDestroyed;
    }
}
