using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    #region Fields

    //private CookerInputActions cookerInputActions;
    private InputActionAsset inputAsset;
    private InputActionMap player;
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
        //this.cookerActionMap = new CookerInputActions();        
        this.inputAsset = this.GetComponent<PlayerInput>().actions;
        this.player = this.inputAsset.FindActionMap("Cooking");
        if (this.player == null)
            throw new Exception("Action map Cooking not foun in Input Action Asset");
        //this.SelectColor();
    }

    private void OnEnable()
    {
        //this.movement = this.cookerActionMap.Cooking.Move;
        //this.movement.Enable();

        //this.cookerActionMap.Cooking.Run.performed += OnRunPerformed;
        //this.cookerActionMap.Cooking.Run.Enable();

        //this.cookerActionMap.Cooking.Action.performed += OnActionPerformed;
        //this.cookerActionMap.Cooking.Action.Enable();

        //this.cookerActionMap.Cooking.Grab.performed += OnGrabPerformed;
        //this.cookerActionMap.Cooking.Grab.Enable();

        //this.cookerActionMap.Cooking.Join.performed += OnJoinPerformed;
        //this.cookerActionMap.Cooking.Join.Enable();

        //this.cookerActionMap.Cooking.Enable();

        this.movement = this.player.FindAction("Move");
        this.movement.Enable();

        this.player.FindAction("Run").performed += OnRunPerformed;
        this.player.FindAction("Run").Enable();

        this.player.FindAction("Action").performed += OnActionPerformed;
        this.player.FindAction("Action").Enable();

        this.player.FindAction("Grab").performed += OnGrabPerformed;
        this.player.FindAction("Grab").Enable();

        this.player.FindAction("Join").performed += OnJoinPerformed;
        this.player.FindAction("Join").Enable();

        this.player.Enable();
    }

    

    private void OnDisable()
    {
        this.movement.Disable();
        this.player.FindAction("Run").Disable();
        this.player.FindAction("Action").Disable();
        this.player.FindAction("Grab").Disable();
        this.player.FindAction("Join").Disable();        
        this.player.FindAction("Run").performed -= OnRunPerformed;
        this.player.FindAction("Action").performed -= OnActionPerformed;
        this.player.FindAction("Grab").performed -= OnGrabPerformed;
        this.player.FindAction("Join").performed -= OnJoinPerformed;
        this.player.Disable();
    }

    private void FixedUpdate()
    {
        Vector2 currentMovement = movement.ReadValue<Vector2>();
        if(currentMovement.x > 0 || currentMovement.y > 0)
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
        Debug.Log("Grab performed");
    }

    #endregion

    #region Action

    private void OnActionPerformed(InputAction.CallbackContext obj)
    {
        Debug.Log("Action performed");
    }

    #endregion

    #region On Join 

    private void OnJoinPerformed(InputAction.CallbackContext obj)
    {
        Debug.Log("Join performed");
    }

    #endregion

    #region IsGrounded

    private bool IsGrounded()
    {
        Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
        return Physics.Raycast(ray, out RaycastHit hit, 0.3f);            
    }

    #endregion

    #region Color

    private void SelectColor()
    {
        var playerMaterial = this.transform.Find("Capsule").gameObject.GetComponent<Material>();
        playerMaterial.color = Color.red;

        var player = GameObject.FindObjectsByType<PlayerController>(FindObjectsSortMode.InstanceID);
        if(player.Length == 0)
            playerMaterial.color = Color.red;
        else
            playerMaterial.color = Color.blue;
    }

    #endregion
}
