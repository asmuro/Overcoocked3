using Assets.Scripts.Objects.Interfaces;
using System;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    internal class AutoPositioner: MonoBehaviour, IAutoPositioner
    {
        #region Fields

        [SerializeField]
        private Transform positionPoint;

        private IGrabable positionedObject;
        public event EventHandler ObjectOnPosition;
        public event EventHandler ObjectMovedAway;

        #endregion

        #region Positioner

        public void Position(IGrabable grabableObject)
        {
            ((MonoBehaviour)grabableObject).transform.position = this.positionPoint.position;
            this.ObjectOnPosition?.Invoke(((MonoBehaviour)grabableObject).transform.gameObject, EventArgs.Empty);
            this.positionedObject = grabableObject;
        }

        public void UnPosition(IGrabable grabableObject)
        {
            this.ObjectMovedAway?.Invoke(((MonoBehaviour)grabableObject).transform.gameObject, EventArgs.Empty);
            this.positionedObject = null;
        }

        private bool IsPositionOccupied()
        {
            return this.positionedObject != null;
        }

        public bool IsPositionOccupiedByThisObject(IGrabable possiblePositionedObject)
        {
            return this.positionedObject != null && this.positionedObject == possiblePositionedObject;
        }

        public bool CanPosition(IGrabable grabableObject)
        {
            return grabableObject != null && !IsPositionOccupied();
        }

        #endregion
    }
}
