using Assets.Scripts.Services.Interfaces;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static Assets.Scripts.Constants;

public class PlayerController : MonoBehaviour, IPlayer
{

    #region Fields

    private InputActionAsset inputAsset;
    private InputActionMap player;
    private InputAction movement;
    private IPlayerService playerService;
    private IPlayerGrabService playerGrabService;
    private IPlayerActionService playerActionService;
    private IPlayerSpawnerService playerSpawnerService;
    private Rigidbody rigidBody;
    private Vector3 forceDirection = Vector3.zero;
    private bool isRunning;
    private bool shouldGrab = false;
    private float slowDownMaxSpeed;
    private GameObject spawnedObject;

    [SerializeField]
    private float movementForce = 1f;
    
    [SerializeField]
    private float runForce = 4f;

    [SerializeField]
    private float runTime = 1f;

    [SerializeField]
    private float maxSpeed = 5f;

    [SerializeField]
    private float maxSpeedRunning = 12f;

    [SerializeField]
    private Camera mainCamera;

    public event EventHandler OnGrabPressed;

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

        this.player.Enable();

        this.playerService = GameObject.FindGameObjectsWithTag("Services").First().transform.Find("Player Service").GetComponent<IPlayerService>();
        this.playerService.RegisterPlayer(this);
        
        this.playerGrabService = this.GetComponent<IPlayerGrabService>();
        this.playerActionService = this.GetComponent<IPlayerActionService>();
        this.playerSpawnerService = this.GetComponent<IPlayerSpawnerService>();
    }    

    private void OnDisable()
    {
        this.movement.Disable();
        this.player.FindAction(RUN_ACTION_NAME).Disable();
        this.player.FindAction(ACTION_ACTION_NAME).Disable();
        this.player.FindAction(GRAB_ACTION_NAME).Disable();        
        this.player.FindAction(RUN_ACTION_NAME).performed -= OnRunPerformed;
        this.player.FindAction(ACTION_ACTION_NAME).performed -= OnActionPerformed;
        this.player.FindAction(GRAB_ACTION_NAME).performed -= OnGrabPerformed;        
        this.player.Disable();
    }

    private void FixedUpdate()
    {
        Vector2 currentMovement = movement.ReadValue<Vector2>();
        //if (currentMovement.x > 0 || currentMovement.y > 0)
        //{
        //    int a = 0;
        //    Debug.Log("Movemement Values " + movement.ReadValue<Vector2>());
        //}
        
        forceDirection += movement.ReadValue<Vector2>().x * Vector3.back * GetMovementForce();
        forceDirection += movement.ReadValue<Vector2>().y * Vector3.right * GetMovementForce();

        this.rigidBody.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;

        Vector3 horizontalVelocity = this.rigidBody.velocity;
        horizontalVelocity.y = 0;

        this.rigidBody.velocity = this.GetVelocity(horizontalVelocity);

        if (this.shouldGrab && spawnedObject != null)
        {
            this.shouldGrab = false;
            this.playerGrabService.GrabSpawnedObject(spawnedObject);
            this.spawnedObject = null;
        }

        if (this.shouldGrab)
        {
            this.shouldGrab = false;
            this.playerGrabService.Grab();
            if (!this.playerGrabService.IsGrabbing())
            {
                this.OnGrabPressed?.Invoke(this, EventArgs.Empty);
                //var spawnedObject = this.playerSpawnerService.Spawn();
                //if (!spawnedObject.name.Contains(NEW_NAME))
                //{
                //    this.playerGrabService.GrabSpawnedObject(spawnedObject);
                //}
                //else
                //{
                //    Destroy(spawnedObject);
                //}
            }
        }
        if (this.isRunning)
        {
            this.isRunning = false;
            this.slowDownMaxSpeed = this.maxSpeedRunning;
            StartCoroutine(this.SlowDownCoroutine());
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
            return runForce;            
        }

        return this.movementForce;
    }

    private Vector3 GetVelocity(Vector3 horizontalVelocity)
    {
        if (this.isRunning)
        {
            return horizontalVelocity.normalized * this.maxSpeedRunning + Vector3.up * this.rigidBody.velocity.y;                       
        }
        else
        {
            var currentMaxSpeed = this.GetMaxSpeed();
            if (horizontalVelocity.sqrMagnitude > currentMaxSpeed * currentMaxSpeed)
            {
                return horizontalVelocity.normalized * currentMaxSpeed + Vector3.up * this.rigidBody.velocity.y;
            }
        }

        return horizontalVelocity;
    }

    private float GetMaxSpeed()
    {
        if (this.slowDownMaxSpeed > this.maxSpeed)
        {
            return this.slowDownMaxSpeed;
        }

        return this.maxSpeed;
    }

    public IEnumerator SlowDownCoroutine()
    {
        var speedDifference = Mathf.Abs(this.slowDownMaxSpeed - this.maxSpeed);        

        while (speedDifference > 0.01f)
        {           
            this.slowDownMaxSpeed -= (speedDifference / 150f);
            speedDifference = Mathf.Abs(this.slowDownMaxSpeed - this.maxSpeed);
            yield return null;            
        }

        this.slowDownMaxSpeed = this.maxSpeed;
        yield return 0;        
    }
   
    #endregion

    #region Grab

    private void OnGrabPerformed(InputAction.CallbackContext obj)
    {        
        this.shouldGrab = true;        
    }

    public void SpawnerOnObjectSpawned(GameObject spawnedObject)
    {
        this.shouldGrab = true;
        this.spawnedObject = spawnedObject;
    }

    #endregion

    #region Action

    private void OnActionPerformed(InputAction.CallbackContext obj)
    {
        this.playerActionService.ExecuteAction();
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
        RaycastHit hit;
        return Physics.Raycast(ray, out hit, 1.3f);            
    }

    #endregion

    #region Look At

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

}
