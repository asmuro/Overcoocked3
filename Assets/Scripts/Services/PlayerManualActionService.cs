using Assets.Scripts.Helpers;
using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Interfaces;
using Assets.Scripts.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerManualActionService : MonoBehaviour, IPlayerActionService
{

    #region Fields

    private GameObject actionInProgress;
    private List<GameObject> actionableObjects = new List<GameObject>();    

    #endregion    

    #region Trigger

    private void OnTriggerEnter(Collider other)
    {
        this.AddObjectToNearbyActionableObjects(other.transform);        
    }

    private bool IsActionable(Transform gameObject)
    {
        return gameObject?.GetComponent<IActionable>() != null;
    }    

    private void AddObjectToNearbyActionableObjects(Transform transform)
    {
        if (this.IsActionable(transform)
            && !this.actionableObjects.Contains(transform.gameObject))
        {
            transform.GetComponent<IActionable>().OnActionFinished += OnActionFinished;
            this.actionableObjects.Add(transform.gameObject);
        }
    }

    private void OnActionFinished(object sender, System.EventArgs e)
    {
        this.actionInProgress = null;
    }

    private void OnTriggerExit(Collider other)
    {
        if (this.IsActionable(other.transform))
        {
            if (this.actionableObjects.Contains(other.transform.gameObject))
            {
                other.transform.GetComponent<IActionable>().StopAction();
                this.actionInProgress = null;
                this.actionableObjects.Remove(other.transform.gameObject);
            }
        }        
    }



    #endregion

    #region IPlayerActionService

    private bool CanExecuteAction()
    {
        return this.actionableObjects.Any() && this.actionInProgress == null;
    }

    public void ExecuteAction()
    {
        if(!CanExecuteAction())
        {
            return;
        }

        this.actionInProgress = Operations3D.GetClosestObjectInNearby(this.transform, this.actionableObjects);
        this.actionInProgress.GetComponent<IActionable>().ExecuteAction();
        Debug.Log("Player Action Service: Action executed");
    }

    #endregion
}
