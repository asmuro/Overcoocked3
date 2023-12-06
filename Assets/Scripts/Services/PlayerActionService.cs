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

public class PlayerActionService : MonoBehaviour, IPlayerSpawnerService
{

    #region Fields

    private CapsuleCollider collider;
    private GameObject actionObject;
    private List<GameObject> actionableObjects = new List<GameObject>();    

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        this.collider = this.GetComponent<CapsuleCollider>();
    }

    #endregion

    #region Trigger

    private void OnTriggerEnter(Collider other)
    {
        if (this.IsActionable(other.transform.parent)
            && !this.actionableObjects.Contains(other.transform.parent.gameObject))
        {
            this.actionableObjects.Add(other.transform.parent.gameObject);
        }
        Debug.Log("OnTriggerEnter");
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
        
        Debug.Log("OnTriggerStay");
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
        Debug.Log("OnTriggerExit");
    }



    #endregion

    #region IPlayerGrabService

    private bool CanExecuteAction()
    {
        return this.actionableObjects.Any() && this.actionObject == null;
    }

    public void Spawn()
    {
        if (CanExecuteAction())
        {
            this.actionObject = GetClosestActionableObject();
            this.actionObject.GetComponent<ISpawner>().Spawn();
            Debug.Log("Player Action Service: Action executed");
            this.actionObject = null;
        }
    }        

    private GameObject GetClosestActionableObject()
    {
        if (this.actionableObjects.Count == 1)
        {
            return this.actionableObjects.First();
        }

        if (this.actionableObjects.Count > 1)
        {
            GameObject closestGrabableObject = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = transform.position;
            foreach (GameObject currentGrabableObject in this.actionableObjects)
            {
                float dist = Vector3.Distance(currentGrabableObject.transform.position, currentPos);
                if (dist < minDist)
                {
                    closestGrabableObject = currentGrabableObject;
                    minDist = dist;
                }
            }
            return closestGrabableObject;            
        }

        return new GameObject();
    }

        #endregion
}
