using Assets.Scripts.Objects.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderReceiver : MonoBehaviour
{
    #region Monobehaviour

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.parent is IGrabable)
        {

        }
        //Get Expected Order
        //Destroy delivered object only if not grabbed
        //Process the order
    }

    private void ProcessOrder()
    {

    }

    #endregion
}
