using Assets.Scripts.Objects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    internal class AutomaticActionable : MonoBehaviour, IActionable
    {
        #region IActionable

        public event EventHandler OnActionFinished;

        public void ExecuteAction()
        {
            throw new NotImplementedException();
        }

        public void StopAction()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
