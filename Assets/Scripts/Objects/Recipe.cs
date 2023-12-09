using Assets.Scripts.Objects.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class Recipe : MonoBehaviour, IRecipe
    {
        #region Properties

        [SerializeField]
        private List<GameObject> ingredients;
        public List<GameObject> Ingredients { get => this.ingredients; set => ingredients = value; }        

        #endregion
    }
}
