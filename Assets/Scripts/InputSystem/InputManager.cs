using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region Properties

    public static CookerInputActions inputActions = new CookerInputActions();
    public static event Action<InputActionMap> actionMapChange;

    #endregion

    #region Monobehaviour

    // Start is called before the first frame update
    void Start()
    {
        ToggleActionMap(inputActions.Cooking);
    }

    #endregion

    #region Toggle

    public static void ToggleActionMap(InputActionMap actionMap)
    {
        if (actionMap.enabled)
            return;

        inputActions.Disable();
        actionMapChange?.Invoke(actionMap);
        actionMap.Enable();
    }

    #endregion
}
