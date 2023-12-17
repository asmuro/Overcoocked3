using Assets.Scripts.Objects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
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

        #region Trigger

        private void OnTriggerEnter(Collider other)
        {
            var player = other.transform.GetComponent<IPlayer>();
            if (player != null)
            {
                player.OnGrabPressed += PlayerOnGrabPressed;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var player = other.transform.GetComponent<IPlayer>();
            if (player != null)
            {
                player.OnGrabPressed -= PlayerOnGrabPressed;
            }
        }

        private void PlayerOnGrabPressed(object sender, EventArgs e)
        {
            var spawnedObject = Instantiate(objectToSpawn, prefabPoint.position, Quaternion.identity);
            ((IPlayer)sender).SpawnerOnObjectSpawned(spawnedObject);
        }

        #endregion

        #region IActionable

        GameObject ISpawner.Spawn()
        {
            return Instantiate(objectToSpawn, prefabPoint.position, Quaternion.identity);
        }

        #endregion
    }
}
