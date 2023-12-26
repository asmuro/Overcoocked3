using Assets.Scripts.Objects.Interfaces;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Objects
{
    public class GrabableObject : MonoBehaviour, IGrabable
    {
        #region Properties

        private bool isBeingGrabbed;
        public bool IsBeingGrabbed { get => this.isBeingGrabbed; set => this.isBeingGrabbed = value; }

        public event EventHandler OnDestroyed;

        private IChoppable choppable;

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            this.choppable = this.GetComponent<IChoppable>();            
        }

        #endregion

        #region Methods

        public bool CanBeGrabbed()
        {
            if(this.choppable == null)
            {
                return true;
            }

            return this.choppable.ChopActionTimeConsumed == 0 
                || (this.choppable.ChopActionTimeConsumed > 0 && this.choppable.ChopActionTimeConsumed >= this.choppable.ChopTotalActionTime);
        }

        #endregion

        #region OnDestroy

        private void OnDestroy()
        {
            this.OnDestroyed?.Invoke(this.transform.gameObject, EventArgs.Empty);
        }

        

        #endregion
    }
}