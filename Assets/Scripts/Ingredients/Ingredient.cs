using UnityEngine;
using Assets.Scripts.Objects.Interfaces;
using Assets.Scripts.Enumerables;

namespace Assets.Scripts.Objects
{
    internal class Ingredient : MonoBehaviour, IIngredient
    {
        #region Fields

        [SerializeField]
        private string ingredientName = "ingredient";          

        private IngredientState state = IngredientState.Raw;        

        #endregion

        #region IIngredient

        string IIngredient.Name => this.ingredientName;

        IngredientState IIngredient.State => this.state;

        #endregion       

    }
}
