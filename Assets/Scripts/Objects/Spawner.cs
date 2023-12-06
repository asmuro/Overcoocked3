using Assets.Scripts.Objects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    internal class Spawner : MonoBehaviour, ISpawner
    {
        #region Fields

        [SerializeField] 
        private GameObject objectToSpawn;

        [SerializeField]
        private Transform prefabPoint;

        #endregion

        #region IActionable

        void ISpawner.Spawn()
        {
            var newObject = Instantiate(objectToSpawn, prefabPoint.position, Quaternion.identity);
        }

        #endregion
    }
}
