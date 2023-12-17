using Assets.Scripts.Helpers;
using Assets.Scripts.Objects.Interfaces;
using Assets.Scripts.Services.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerSpawnerService : MonoBehaviour, IPlayerSpawnerService
{

    #region Fields

    private GameObject spawner;
    private List<GameObject> spawners = new List<GameObject>();    

    #endregion

    #region Monobehaviour    

    #endregion

    #region Trigger

    private void OnTriggerEnter(Collider other)
    {
        if (this.IsActionable(other.transform.parent)
            && !this.spawners.Contains(other.transform.parent.gameObject))
        {
            this.spawners.Add(other.transform.parent.gameObject);
        }        
    }

    private bool IsActionable(Transform gameObject)
    {
        return gameObject?.GetComponent<ISpawner>() != null;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.transform.parent?.tag == "Floor")
        {
            return;
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if (this.IsActionable(other.transform.parent))
        {
            if (this.spawners.Contains(other.transform.parent.gameObject))
            {
                this.spawners.Remove(other.transform.parent.gameObject);
            }
        }        
    }



    #endregion

    #region IPlayerSpawnerService

    private bool CanSpawn()
    {
        return this.spawners.Any() && this.spawner == null;
    }

    public GameObject Spawn()
    {
        if (!CanSpawn())
        {
            return new GameObject();
        }

        this.spawner = Operations3D.GetClosestObjectInNearby(this.transform, this.spawners);
        var spawnedObject = this.spawner.GetComponent<ISpawner>().Spawn();
        Debug.Log("Player Action Service: Action executed");
        this.spawner = null;
        return spawnedObject;
    }        
    
    #endregion
}
