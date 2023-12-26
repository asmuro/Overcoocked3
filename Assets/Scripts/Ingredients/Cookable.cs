using UnityEngine;
using Assets.Scripts.Objects.Interfaces;

namespace Assets.Scripts.Objects
{
    internal class Cookable : MonoBehaviour, ICookable
    {
        #region Fields        

        [SerializeField]
        private bool isCookable = false;

        [SerializeField]
        private float cookTotalActionTime = 1.5f;

        [SerializeField]
        private float fromCookTimeToBurn = 2f;
        
        [SerializeField]
        private float startAlertTimeBeforeBurn = 1f;

        [SerializeField]
        private GameObject burnedIcon;


        private float cookActionTimeConsumed;

        #endregion

        #region Monobehaviour

        private void Start()
        {
            burnedIcon.SetActive(false);
        }

        private void Update()
        {
            if (this.isBurned)
            {
                burnedIcon.SetActive(true);
            }
        }

        #endregion

        #region ICookable        

        public bool IsCookable
        {
            get
            {
                return this.isCookable
                    && !this.isBurned;
            }
        }

        private bool isBurned => this.cookActionTimeConsumed > (this.cookTotalActionTime + this.fromCookTimeToBurn);        

        public float CookTime => this.cookTotalActionTime;

        public float CookActionTimeConsumed => this.cookActionTimeConsumed;

        public float FromCookTimeToBurn => this.fromCookTimeToBurn;

        public float StartAlertTimeBeforeBurn => this.startAlertTimeBeforeBurn;        

        public void SetCookActionTimeConsumed(float newChopActionTimeConsumed)
        {
            this.cookActionTimeConsumed = newChopActionTimeConsumed;
        }

        #endregion

    }
}
