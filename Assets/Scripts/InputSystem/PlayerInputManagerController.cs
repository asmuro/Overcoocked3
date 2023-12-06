using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerInputManagerController : MonoBehaviour
{
    #region Fields

    private PlayerInputManager playerInputManager;
    private int playersJoined = 0;

    #endregion

    #region MonoBehaviour

    // Start is called before the first frame update
    void Start()
    {
        this.playerInputManager = GetComponent<PlayerInputManager>();
        this.playerInputManager.onPlayerJoined += OnPlayerJoined;
    }   

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region On Player Joined

    private void OnPlayerJoined(PlayerInput player)
    {
        this.playersJoined++;
        this.SetColor(player);
    }

    private void SetColor(PlayerInput player)
    {
        var playerMaterial = player.transform.Find("Capsule").gameObject.GetComponent<Renderer>().material;
        switch (this.playersJoined)
        {
            case 1:
                playerMaterial.color = Color.red;
                break;
            case 2:
                playerMaterial.color = Color.blue;
                break;
        }
    }

    #endregion
}
