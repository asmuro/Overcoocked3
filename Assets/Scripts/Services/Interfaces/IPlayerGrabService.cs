using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Services.Interfaces
{
    internal interface IPlayerGrabService
    {
        void Grab();

        void GrabSpawnedObject(GameObject spawnedObject);

        bool IsGrabbing();
    }
}
