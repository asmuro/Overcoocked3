using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    #region Fields

    private CookerInputActions cookerInputActions;
    private InputAction movement;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        this.cookerInputActions = new CookerInputActions();
    }

    private void OnEnable()
    {
        this.movement = this.cookerInputActions.Cooking.Move;
        this.movement.Enable();

        this.cookerInputActions.Cooking.Run.performed += OnRunPerformed;
        this.cookerInputActions.Cooking.Run.Enable();

        this.cookerInputActions.Cooking.Action.performed += OnActionPerformed;
        this.cookerInputActions.Cooking.Action.Enable();

        this.cookerInputActions.Cooking.Grab.performed += OnGrabPerformed;
        this.cookerInputActions.Cooking.Grab.Enable();
    }   

    private void OnDisable()
    {
        this.movement.Disable();
        this.cookerInputActions.Cooking.Run.Disable();
        this.cookerInputActions.Cooking.Action.Disable();
        this.cookerInputActions.Cooking.Grab.Disable();
    }

    private void FixedUpdate()
    {
        Debug.Log("Movemement Values " + movement.ReadValue<Vector2>());
    }

    #endregion


    #region Run

    private void OnRunPerformed(InputAction.CallbackContext obj)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Grab

    private void OnGrabPerformed(InputAction.CallbackContext obj)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Action

    private void OnActionPerformed(InputAction.CallbackContext obj)
    {
        throw new NotImplementedException();
    }

    #endregion
}
