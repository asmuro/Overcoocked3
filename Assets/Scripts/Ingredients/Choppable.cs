using UnityEngine;
using Assets.Scripts.Objects.Interfaces;

namespace Assets.Scripts.Objects
{
    internal class Choppable : MonoBehaviour, IChoppable
    {
        #region Fields        

        [SerializeField]
        private bool isChoppable = false;

        [SerializeField]
        private float chopTotalActionTime = 1.5f;

        private float chopActionTimeConsumed;

        #endregion        

        #region IChoppable

        public bool IsChoppable
        {
            get
            {
                return this.isChoppable
                    && this.chopActionTimeConsumed < this.chopTotalActionTime;
            }
        }

        public float ChopTotalActionTime => this.chopTotalActionTime;

        public float ChopActionTimeConsumed => this.chopActionTimeConsumed;

        public void SetChopActionTimeConsumed(float newChopActionTimeConsumed)
        {
            this.chopActionTimeConsumed = newChopActionTimeConsumed;
        }        

        #endregion

    }
}
