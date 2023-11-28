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

    private Rigidbody rigidBody;
    [SerializeField]
    private float movementForce = 1f;
    [SerializeField]
    private float runForce = 4f;
    [SerializeField]
    private float maxSpeed = 5f;
    private Vector3 forceDirection = Vector3.zero;

    [SerializeField]
    private Camera mainCamera;

    private bool isRunning;

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        this.rigidBody = this.GetComponent<Rigidbody>();
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

        this.cookerInputActions.Cooking.Enable();
    }   

    private void OnDisable()
    {
        this.movement.Disable();
        this.cookerInputActions.Cooking.Run.Disable();
        this.cookerInputActions.Cooking.Action.Disable();
        this.cookerInputActions.Cooking.Grab.Disable();
        this.cookerInputActions.Cooking.Disable();
        this.cookerInputActions.Cooking.Run.performed -= OnRunPerformed;
        this.cookerInputActions.Cooking.Action.performed -= OnActionPerformed;
        this.cookerInputActions.Cooking.Grab.performed -= OnGrabPerformed;
    }

    private void FixedUpdate()
    {
        Debug.Log("Movemement Values " + movement.ReadValue<Vector2>());
        //forceDirection += movement.ReadValue<Vector2>().x * GetCameraRight(mainCamera) * GetMovementForce();
        //forceDirection += movement.ReadValue<Vector2>().y * GetCameraForward(mainCamera) * GetMovementForce();
        forceDirection += movement.ReadValue<Vector2>().x * Vector3.back * GetMovementForce();
        forceDirection += movement.ReadValue<Vector2>().y * Vector3.right * GetMovementForce();

        this.rigidBody.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;

        Vector3 horizontalVelocity = this.rigidBody.velocity;
        horizontalVelocity.y = 0;
        if(horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            this.rigidBody.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * this.rigidBody.velocity.y;
        }
    }

    #endregion

    #region Camera

    private Vector3 GetCameraRight(Camera camera)
    {
        Vector3 forward = camera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private Vector3 GetCameraForward(Camera camera)
    {
        Vector3 right = camera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    #endregion

    #region Run

    private void OnRunPerformed(InputAction.CallbackContext obj)
    {
        if(IsGrounded())
        {
            this.isRunning = true;
        }
    }

    private float GetMovementForce()
    {
        if(this.isRunning)
        {
            this.isRunning = false;
            return runForce;            
        }

        return this.movementForce;
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

    #region IsGrounded

    private bool IsGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
        return Physics.Raycast(ray, out RaycastHit hit, 0.3f);            
    }

    #endregion
}
