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

        private void OnDestroy()
        {
            this.OnDestroyed?.Invoke(this.transform.gameObject, EventArgs.Empty);
        }

        #endregion
    }
}