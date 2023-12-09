using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private Image firstOrder;
    [SerializeField]
    private Image secondOrder;
    [SerializeField]
    private Image thirdOrder;
    [SerializeField]
    private Image fourthOrder;
    [SerializeField]
    private Image fifthOrder;

    #endregion

    #region Monobehaviour

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region Add order

    public void AddOrder(Sprite sprite)
    {
        this.firstOrder.enabled = true;
        this.firstOrder.sprite = sprite;
    }

    #endregion
}
