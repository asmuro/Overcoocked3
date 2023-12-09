using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Interfaces;
using Assets.Scripts.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrderService : MonoBehaviour, IOrderService
{
    #region Fields

    private List<IOrder> pendingOrders;
    private IUIOrderService uiOrderService;

    [SerializeField]
    public List<OrderAndWeight> ordersAndWeights;

    [SerializeField]
    private int ordersToStart;

    [SerializeField]
    private int maxPendingOrder;

    [SerializeField]
    private int minPendingOrders;

    #endregion

    #region Monobehaviour

    // Start is called before the first frame update
    void Start()
    {
        this.uiOrderService = FindAnyObjectByType<UIOrderService>();
        this.AddOrdersToStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnValidate()
    {
    #if UNITY_EDITOR
        if (ordersAndWeights.Select(r => r.Weight).Sum() > 100)
        {
            var total = ordersAndWeights.Select(r => r.Weight).Sum();
            ordersAndWeights.ForEach(r => { r.Weight = r.Weight/total*100; });
        }
    #endif
    }

    #endregion

    #region Add Orders

    private void AddOrdersToStart()
    {
        this.pendingOrders = new List<IOrder>();
        for (int i = 0; i < ordersToStart; i++)
        {
            this.pendingOrders.Add(GenerateRandomPendingOrder(ordersAndWeights));
        }

        this.uiOrderService.RefreshOrders(this.pendingOrders);
    }

    public static IOrder GenerateRandomPendingOrder(List<OrderAndWeight> ordersAndWeights)
    {
        if (ordersAndWeights.Count == 0) throw new System.ArgumentException("At least one range must be included.");
        if (ordersAndWeights.Count == 1) return ordersAndWeights[0].Order;

        float total = 0f;
        for (int i = 0; i < ordersAndWeights.Count; i++)
        {
            total += ordersAndWeights[i].Weight;
        }

        float randomValue = Random.value;
        float acumulatedProbability = 0f;

        int lastIndex = ordersAndWeights.Count - 1;
        for (int i = 0; i < lastIndex; i++)
        {
            acumulatedProbability += ordersAndWeights[i].Weight / total;
            if (acumulatedProbability >= randomValue)
            {
                return ordersAndWeights[i].Order;
            }
        }

        return ordersAndWeights[lastIndex].Order;
    }

    #endregion

    #region ProcessOrders

    //private bool IsRecipeReceivedExpected(IRecipe recipeReceived)
    //{
    //    this.pendingOrders.FirstOrDefault(o => o.)
    //}

    #endregion

    #region IOrderService

    public void ProcessReceivedRecipe(IRecipe recipeReceived)
    {
        //if(this.IsOrderReceivedExpected())
        //{

        //}
    }

    #endregion  
}
