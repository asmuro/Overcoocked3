using Assets.Scripts.Objects.Interfaces;
using Assets.Scripts.Services.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RecipeReceiver : MonoBehaviour
{
    #region Fields

    private IOrderService orderService;

    #endregion

    #region Monobehaviour

    // Start is called before the first frame update
    void Start()
    {
        this.orderService = GameObject.FindGameObjectsWithTag("Services").First().transform.Find("Order Service").GetComponent<IOrderService>()
            ?? throw new ArgumentNullException("OrderService not found");         
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        var recipe = other.transform.parent?.transform.GetComponent<IRecipe>();
        if (recipe != null)
        {
            this.ProcessRecipe(recipe);
        }        
    }

    private void ProcessRecipe(IRecipe recipe)
    {
        this.orderService.ProcessReceivedRecipe(recipe);
    }

    #endregion
}
