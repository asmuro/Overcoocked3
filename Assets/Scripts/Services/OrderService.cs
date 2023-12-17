using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Interfaces;
using Assets.Scripts.Services.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class OrderService : MonoBehaviour, IOrderService
{
    #region Fields

    private List<IOrder> pendingOrders = new List<IOrder>();
    private IUIOrderService uiOrderService;
    private bool waitingToGenerateNextOrder = false;

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
        this.AddOrder();
    }

    // Update is called once per frame
    void Update()
    {
        if(this.pendingOrders.Count == 0 && !this.waitingToGenerateNextOrder)
        {
            StartCoroutine(WaitToGenerateTheNextOrder(Random.Range(0.5f, 2f)));
        }        
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
        for (int i = 0; i < ordersToStart; i++)
        {
            this.pendingOrders.Add(GenerateRandomPendingOrder(ordersAndWeights));
        }

        this.uiOrderService.RefreshOrders(this.pendingOrders);
    }

    private void AddOrder()
    {        
        this.pendingOrders.Add(GenerateRandomPendingOrder(ordersAndWeights));
        this.uiOrderService.RefreshOrders(this.pendingOrders);
    }

    private void RemoveOrder(IOrder orderToRemove)
    {
        this.pendingOrders.Remove(orderToRemove);
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

    IEnumerator WaitToGenerateTheNextOrder(float seconds)
    {
        this.waitingToGenerateNextOrder = true;
        yield return new WaitForSeconds(seconds);
        this.AddOrder();
        this.waitingToGenerateNextOrder = false;
    }

    #endregion

    #region ProcessOrders

    private bool IsRecipeReceivedExpected(IRecipe recipeReceived)
    {
        bool expectedRecipe = false;        
        this.pendingOrders.ForEach(o =>
        {
            if(expectedRecipe)
            {
                return;
            }
            if (o.Recipe.Ingredients.Intersect(recipeReceived.Ingredients).Count() == o.Recipe.Ingredients.Count)
            {
                expectedRecipe = true;                
            }            
        });

        return expectedRecipe;
    }

    private IOrder GetEquivalentExpectedRecipe(IRecipe receivedRecipe)
    {
        IOrder orderContainingReceivedRecipe = null;
        this.pendingOrders.ForEach(o =>
        {
            if (orderContainingReceivedRecipe != null)
            {
                return;
            }
            if (o.Recipe.Ingredients.Intersect(receivedRecipe.Ingredients).Count() == o.Recipe.Ingredients.Count)
            {
                orderContainingReceivedRecipe = o;
            }
        });

        return orderContainingReceivedRecipe;
    }

    #endregion

    #region IOrderService

    public void ProcessReceivedRecipe(IRecipe receivedRecipe)
    {
        var orderContainingReceivedRecipe = GetEquivalentExpectedRecipe(receivedRecipe);
        if (orderContainingReceivedRecipe != null)
        {
            this.RemoveOrder(orderContainingReceivedRecipe);
        }
        Destroy(((MonoBehaviour)receivedRecipe).gameObject);        
    }

    #endregion  
}
