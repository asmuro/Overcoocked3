using Assets.Scripts.Objects.Interfaces;
using Assets.Scripts.Services.Interfaces;
using System.Collections.Generic;
using UnityEngine;

public class UIOrderService : MonoBehaviour, IUIOrderService
{
    #region Fields

    private UIManager uiManager;

    #endregion

    #region Monobehaviour

    // Start is called before the first frame update
    void Start()
    {
        this.uiManager = FindAnyObjectByType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #endregion

    #region IUIOrderService

    public void RefreshOrders(List<IOrder> orders)
    {
        foreach (var recipe in orders)
        {
            this.uiManager.AddOrder(recipe.Sprite);
        }
    }    

    #endregion
}
