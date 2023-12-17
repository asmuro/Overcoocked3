using System;
using UnityEngine;

internal interface IPlayer
{

    event EventHandler OnGrabPressed;

    void SpawnerOnObjectSpawned(GameObject spawnedObject);
}