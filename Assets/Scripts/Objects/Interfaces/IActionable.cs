﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Objects.Interfaces
{
    internal interface IActionable
    {
        void ExecuteAction();

        event EventHandler OnActionFinished;

        void StopAction();
    }
}
