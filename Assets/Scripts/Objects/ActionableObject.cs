using Assets.Scripts.Objects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    internal class ActionableObject : MonoBehaviour, IActionable
    {

        #region IActionable

        void IActionable.Action()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
