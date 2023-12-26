using Assets.Scripts.Objects.Interfaces;
using System;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    internal class AutoPositioner: MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private Transform positionPoint;

        public event EventHandler ObjectOnPosition;
        public event EventHandler ObjectMovedAway;

        #endregion

        #region Collider

        private void OnTriggerEnter(Collider other)
        {
            var recipe = other.transform.parent?.transform.GetComponent<IGrabable>();
            if (recipe != null)
            {
                this.Position(recipe);
                this.ObjectOnPosition?.Invoke(other.transform.parent?.gameObject, EventArgs.Empty);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            this.ObjectMovedAway?.Invoke(other.transform.parent?.gameObject, EventArgs.Empty);
        }

        #endregion

        #region Positioner

        private void Position(IGrabable recipe)
        {
            ((MonoBehaviour)recipe).transform.position = this.positionPoint.position;
        }

        #endregion
    }
}
