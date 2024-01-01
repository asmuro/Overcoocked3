using Assets.Scripts.Helpers;
using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Interfaces;
using Assets.Scripts.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerGrabAndLooseService : MonoBehaviour, IPlayerGrabService
{

    #region Fields

    private GameObject grabingObject;
    private List<GameObject> grabableObjects = new List<GameObject>();
    private List<GameObject> autoPositionerObjects = new List<GameObject>();

    #endregion

    #region Trigger

    private void OnTriggerEnter(Collider other)
    {
        this.AddObjectToNearbyGrabableObjects(other.transform);        
    }

    private void OnTriggerExit(Collider other)
    {
        var otherGameObject = other.transform.gameObject;
        if (this.IsGrabable(other.transform)
            && this.grabableObjects.Contains(otherGameObject))
        {
            this.grabableObjects.Remove(otherGameObject);
        }

        if (this.IsAutoPositioner(other.transform)
            && this.autoPositionerObjects.Contains(otherGameObject))
        {
            this.autoPositionerObjects.Remove(otherGameObject);
        }
    }

    private void AddObjectToNearbyGrabableObjects(Transform transform)
    {
        if (this.IsGrabable(transform)
            && !this.grabableObjects.Contains(transform.gameObject))
        {
            transform.GetComponent<IGrabable>().OnDestroyed += OnDestroyedGrabableObject;
            this.grabableObjects.Add(transform.gameObject);
        }

        if (this.IsAutoPositioner(transform)
            && !this.autoPositionerObjects.Contains(transform.gameObject))
        {
            this.autoPositionerObjects.Add(transform.gameObject);
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

    #endregion

    #region IPlayerGrabService

    private bool CanGrab()
    {
        return this.grabableObjects.Any() && this.grabingObject == null
            && Operations3D.GetClosestObjectInNearby(this.transform, this.grabableObjects).GetComponent<IGrabable>().CanBeGrabbed();
    }

    public void Grab()
    {
        if (this.CanLoose())
        {
            this.Loose();
            return;
        }

        if (!CanGrab())
        {
            return;
        }

        this.GrabObjectOperations();
        this.UnPositionObjectFromAutoPositioner();                
    }

    private void GrabObjectOperations()
    {
        this.grabingObject = Operations3D.GetClosestObjectInNearby(this.transform, this.grabableObjects);
        this.grabingObject.GetComponent<Rigidbody>().isKinematic = true;
        this.grabingObject.transform.position = this.transform.Find("HandsPosition").transform.position;

        this.grabingObject.transform.parent = this.transform;
        this.grabingObject.transform.Find("3D").GetComponent<Collider>().enabled = false;
        this.grabingObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        this.grabingObject.GetComponent<IGrabable>().IsBeingGrabbed = true;        
    }

    private void UnPositionObjectFromAutoPositioner()
    {
        if (!this.autoPositionerObjects.Any())
        {
            return;
        }
        
        var autoPositionerWithOurObject =  this.autoPositionerObjects.FirstOrDefault(o => 
            o.transform.GetComponent<IAutoPositioner>().IsPositionOccupiedByThisObject(this.grabingObject.GetComponent<IGrabable>()));
        if (autoPositionerWithOurObject != null)
        {
            autoPositionerWithOurObject.transform.GetComponent<IAutoPositioner>()
                .UnPosition(grabingObject.GetComponent<IGrabable>());
        }
    }

    private bool IsGrabable(Transform transform)
    {
        return transform?.GetComponent<IGrabable>() != null;
    }

    private bool IsAutoPositioner(Transform transform)
    {
        return transform?.GetComponent<IAutoPositioner>() != null;
    }

    public bool IsGrabbing()
    {
        return this.grabingObject != null;
    }   

    public void GrabSpawnedObject(GameObject spawnedObject)
    {
        this.AddObjectToNearbyGrabableObjects(spawnedObject.transform);
        this.Grab();
    }

    #endregion

    #region Loose
    private bool CanLoose()
    {
        if (this.grabingObject != null 
            && this.autoPositionerObjects.Any())
        {
            var closestAutoPositioner = Operations3D.GetClosestObjectInNearby(this.transform, this.autoPositionerObjects)
                                            .transform?.GetComponent<IAutoPositioner>();
            if(closestAutoPositioner.CanPosition(this.grabingObject.transform?.GetComponent<IGrabable>()))
            {                
                return true;
            }
            return false;
        }

        if (this.grabingObject != null)
        {
            return true;
        }

        return false;
    }

    private void Loose()
    {
        this.LooseObjectOperations();
        this.PositionObjectToAutoPositioner();
        this.grabingObject = null;
        Debug.Log("Player Grab Service: Loose executed");
    }

    private void LooseObjectOperations()
    {
        this.grabingObject.transform.parent = null;
        this.grabingObject.GetComponent<Rigidbody>().isKinematic = false;
        this.grabingObject.transform.Find("3D").GetComponent<Collider>().enabled = true;
        this.grabingObject.GetComponent<IGrabable>().IsBeingGrabbed = false;       
    }

    private void PositionObjectToAutoPositioner()
    {
        if (!this.autoPositionerObjects.Any())
        {
            return;
        }

        var closestAutoPositioner = Operations3D.GetClosestObjectInNearby(this.transform, this.autoPositionerObjects)
                                            .transform?.GetComponent<IAutoPositioner>();
        closestAutoPositioner.Position(this.grabingObject.transform?.GetComponent<IGrabable>());
    }

    #endregion
}
