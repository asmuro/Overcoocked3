using Assets.Scripts.Helpers;
using Assets.Scripts.Objects.Interfaces;
using Assets.Scripts.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerGrabService : MonoBehaviour, IPlayerGrabService
{

    #region Fields

    private GameObject grabingObject;
    private List<GameObject> grabableObjects = new List<GameObject>();    

    #endregion    

    #region Trigger

    private void OnTriggerEnter(Collider other)
    {
        this.AddObjectToNearbyGrabableObjects(other.transform);        
    }

    private bool IsGrabable(Transform gameObject)
    {
        return gameObject?.GetComponent<IGrabable>() != null;
    }   

    private void AddObjectToNearbyGrabableObjects(Transform transform)
    {
        if (this.IsGrabable(transform)
            && !this.grabableObjects.Contains(transform.gameObject))
        {
            transform.GetComponent<IGrabable>().OnDestroyed += OnDestroyedGrabableObject;            
            this.grabableObjects.Add(transform.gameObject);
        }
    }

    private void OnDestroyedGrabableObject(object sender, System.EventArgs e)
    {
        if(!this.grabableObjects.Contains((GameObject)sender))
        {
            return;
        }
        this.grabableObjects.Remove((GameObject)sender);        
    }

    private void OnTriggerExit(Collider other)
    {
        if (this.IsGrabable(other.transform))
        {
            if (this.grabableObjects.Contains(other.transform.gameObject))
            {
                this.grabableObjects.Remove(other.transform.gameObject);
            }
        }        
    }



    #endregion

    #region IPlayerGrabService

    private bool CanGrab()
    {
        return this.grabableObjects.Any() && this.grabingObject == null
            && Operations3D.GetClosestObjectInNearby(this.transform, this.grabableObjects).GetComponent<IGrabable>().CanBeGrabbed();
    }

    public void Grab()
    {
        if (this.grabingObject != null)
        {
            this.Loose();
            return;
        }

        if (!CanGrab())
        {
            return;
        }

        this.grabingObject = Operations3D.GetClosestObjectInNearby(this.transform, this.grabableObjects);
        this.grabingObject.GetComponent<Rigidbody>().isKinematic = true;
        this.grabingObject.transform.position = this.transform.Find("HandsPosition").transform.position;

        this.grabingObject.transform.parent = this.transform;
        this.grabingObject.transform.Find("3D").GetComponent<Collider>().enabled = false;
        this.grabingObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        this.grabingObject.GetComponent<IGrabable>().IsBeingGrabbed = true;        
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
        this.grabingObject.GetComponent<IGrabable>().IsBeingGrabbed = false;
        this.grabingObject = null;
        
        Debug.Log("Player Grab Service: Loose executed");
    }

    public void GrabSpawnedObject(GameObject spawnedObject)
    {
        this.AddObjectToNearbyGrabableObjects(spawnedObject.transform);
        this.Grab();
    }

    #endregion
}
