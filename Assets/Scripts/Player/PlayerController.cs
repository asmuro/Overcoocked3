using Assets.Scripts.Services.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static Assets.Scripts.Constants;

public class PlayerController : MonoBehaviour
{

    #region Fields

    private InputActionAsset inputAsset;
    private InputActionMap player;
    private InputAction movement;
    private IPlayerService playerService;
    private IPlayerGrabService playerGrabService;
    private Rigidbody rigidBody;
    private Vector3 forceDirection = Vector3.zero;
    private bool isRunning;
    private bool shouldGrab = false;

    [SerializeField]
    private float movementForce = 1f;
    
    [SerializeField]
    private float runForce = 4f;
    
    [SerializeField]
    private float maxSpeed = 5f;

    [SerializeField]
    private Camera mainCamera;    

    #endregion

    #region MonoBehaviour

    private void Awake()
    {
        this.rigidBody = this.GetComponent<Rigidbody>();        
        this.inputAsset = this.GetComponent<PlayerInput>().actions;
        this.player = this.inputAsset.FindActionMap(COOKING_ACTION_MAP_NAME);
        if (this.player == null)
            throw new Exception("Action map Cooking not found in Input Action Asset");
        
    }

    private void OnEnable()
    {
        this.movement = this.player.FindAction(MOVE_ACTION_NAME);
        this.movement.Enable();

        this.player.FindAction(RUN_ACTION_NAME).performed += OnRunPerformed;
        this.player.FindAction(RUN_ACTION_NAME).Enable();

        this.player.FindAction(ACTION_ACTION_NAME).performed += OnActionPerformed;
        this.player.FindAction(ACTION_ACTION_NAME).Enable();

        this.player.FindAction(GRAB_ACTION_NAME).performed += OnGrabPerformed;
        this.player.FindAction(GRAB_ACTION_NAME).Enable();

        this.player.FindAction(JOIN_ACTION_NAME).performed += OnJoinPerformed;
        this.player.FindAction(JOIN_ACTION_NAME).Enable();

        this.player.Enable();

        this.playerService = GameObject.FindGameObjectsWithTag("Services").First().GetComponent<IPlayerService>();
        this.playerService.RegisterPlayer(this);
        
        this.playerGrabService = this.GetComponent<IPlayerGrabService>();        
        
    }

    

    private void OnDisable()
    {
        this.movement.Disable();
        this.player.FindAction(RUN_ACTION_NAME).Disable();
        this.player.FindAction(ACTION_ACTION_NAME).Disable();
        this.player.FindAction(GRAB_ACTION_NAME).Disable();
        this.player.FindAction(JOIN_ACTION_NAME).Disable();        
        this.player.FindAction(RUN_ACTION_NAME).performed -= OnRunPerformed;
        this.player.FindAction(ACTION_ACTION_NAME).performed -= OnActionPerformed;
        this.player.FindAction(GRAB_ACTION_NAME).performed -= OnGrabPerformed;
        this.player.FindAction(JOIN_ACTION_NAME).performed -= OnJoinPerformed;
        this.player.Disable();
    }

    private void FixedUpdate()
    {
        Vector2 currentMovement = movement.ReadValue<Vector2>();
        if (currentMovement.x > 0 || currentMovement.y > 0)
        {
            int a = 0;
            //Debug.Log("Movemement Values " + movement.ReadValue<Vector2>());
        }
        
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

        if (this.shouldGrab)
        {
            this.shouldGrab = false;
            this.playerGrabService.Grab();
        }

        LookAt();
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
        Debug.Log("Grab on grabable object performed");
        this.shouldGrab = true;
        
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

    #region Look At

    #region Look


    private void LookAt()
    {
        Vector3 direction = rigidBody.velocity;
        direction.y = 0f;
        if (movement.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
        {
            this.rigidBody.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
        else
        {
            rigidBody.angularVelocity = Vector3.zero;
        }
    }

    #endregion

    #endregion

}
