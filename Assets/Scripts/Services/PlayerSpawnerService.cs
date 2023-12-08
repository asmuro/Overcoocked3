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

    private GameObject actionObject;
    private List<GameObject> actionableObjects = new List<GameObject>();    

    #endregion

    #region Monobehaviour    

    #endregion

    #region Trigger

    private void OnTriggerEnter(Collider other)
    {
        if (this.IsActionable(other.transform.parent)
            && !this.actionableObjects.Contains(other.transform.parent.gameObject))
        {
            this.actionableObjects.Add(other.transform.parent.gameObject);
        }        
    }

    private bool IsActionable(Transform gameObject)
    {
        return gameObject.GetComponent<ISpawner>() != null;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.transform.parent.tag == "Floor")
        {
            return;
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if (this.IsActionable(other.transform.parent))
        {
            if (this.actionableObjects.Contains(other.transform.parent.gameObject))
            {
                this.actionableObjects.Remove(other.transform.parent.gameObject);
            }
        }        
    }



    #endregion

    #region IPlayerGrabService

    private bool CanExecuteAction()
    {
        return this.actionableObjects.Any() && this.actionObject == null;
    }

    public bool Spawn()
    {
        if (!CanExecuteAction())
        {
            return false;
        }

        this.actionObject = Operations3D.GetClosestObjectInNearby(this.transform, this.actionableObjects);
        this.actionObject.GetComponent<ISpawner>().Spawn();
        Debug.Log("Player Action Service: Action executed");
        this.actionObject = null;
        return true;
    }        
    
    #endregion
}
