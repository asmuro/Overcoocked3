using Assets.Scripts.Helpers;
using Assets.Scripts.Objects.Interfaces;
using Assets.Scripts.Services.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerGrabService : MonoBehaviour, IPlayerGrabService
{

    #region Fields

    private CapsuleCollider collider;
    private GameObject grabingObject;
    private List<GameObject> grabableObjects = new List<GameObject>();    

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        this.collider = this.GetComponent<CapsuleCollider>();
        //this.collider.

    }

    #endregion

    #region Trigger

    private void OnTriggerEnter(Collider other)
    {
        if (this.IsGrabable(other.transform.parent)
            && !this.grabableObjects.Contains(other.transform.parent.gameObject))
        {
            this.grabableObjects.Add(other.transform.parent.gameObject);
        }        
    }

    private bool IsGrabable(Transform gameObject)
    {
        return gameObject.GetComponent<IGrabable>() != null;
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
        if (this.IsGrabable(other.transform.parent))
        {
            if (this.grabableObjects.Contains(other.transform.parent.gameObject))
            {
                this.grabableObjects.Remove(other.transform.parent.gameObject);
            }
        }        
    }



    #endregion

    #region IPlayerGrabService

    private bool CanGrab()
    {
        return this.grabableObjects.Any() && this.grabingObject == null;
    }

    public void Grab()
    {
        if (this.grabingObject != null)
        {
            this.Loose();
            return;
        }

        if (CanGrab())
        {
            this.grabingObject = Operations3D.GetClosestObjectInNearby(this.transform, this.grabableObjects);
            this.grabingObject.GetComponent<Rigidbody>().isKinematic = true;
            this.grabingObject.transform.position = this.transform.Find("HandsPosition").transform.position;
            
            this.grabingObject.transform.parent = this.transform;
            this.grabingObject.transform.Find("3D").GetComponent<Collider>().enabled = false;
            this.grabingObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f); ;

            Debug.Log("Player Grab Service: Grab executed");
        }
    }

    public bool IsGrabbing()
    {
        return this.grabingObject != null;
    }

    private void Loose()
    {
        this.grabingObject.transform.parent = null;
        this.grabingObject.GetComponent<Rigidbody>().isKinematic = false;
        this.grabingObject.transform.Find("3D").GetComponent<Collider>().enabled = true;
        this.grabingObject = null;
        
        Debug.Log("Player Grab Service: Loose executed");
    }
    
    #endregion
}
