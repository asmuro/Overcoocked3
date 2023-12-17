using Assets.Scripts.Objects.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    internal class RecipePositioner: MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Transform positionPoint;

        #endregion

        #region Collider

        private void OnTriggerEnter(Collider other)
        {
            var recipe = other.transform.parent?.transform.GetComponent<IRecipe>();
            if (recipe != null)
            {
                this.Position(recipe);
            }
        }

        #endregion

        #region Positioner

        private void Position(IRecipe recipe)
        {
            ((MonoBehaviour)recipe).transform.position = this.positionPoint.position;
        }

        #endregion
    }
}
