using Assets.Scripts.Helpers;
using Assets.Scripts.Objects.Interfaces;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Objects
{
    internal class AutomaticActionable : MonoBehaviour, IActionable
    {
        #region Fields

        [SerializeField]
        private float actionTime;

        private bool executingAction = false;
        private Canvas healthBar;
        private Image bar;

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            this.healthBar = this.transform.Find("Canvas").GetComponent<Canvas>();
            this.bar = this.healthBar.transform.Find("Healthbar").transform.Find("Bar").GetComponent<Image>();
        }

        private void FixedUpdate()
        {
            if(executingAction && !this.IsRecipeOverMe())
            {
                this.StopAction();
            }
            
            if(!executingAction && this.IsRecipeOverMe())
            {
                this.ExecuteAction();
            }            
        }

        #endregion


        #region IActionable

        public event EventHandler OnActionFinished;

        public void ExecuteAction()
        {
            this.executingAction = true;
        }

        public void StopAction()
        {
            this.executingAction = false;
        }

        #endregion

        #region IsObjectOverMe

        private bool IsRecipeOverMe()
        {
            var hitIsHit = IsObjectOverMe();
            return (hitIsHit.RaycastHit.collider.transform.GetComponent<IRecipe>() != null
                && hitIsHit.IsHit);

        }

        

        private RaycastHitIsHit IsObjectOverMe()
        {
            Ray ray = new Ray(this.transform.position + Vector3.up * 0.25f, Vector3.down);
            RaycastHit hit;   
            var isHit = Physics.Raycast(ray, out hit, 0.3f);
            return new RaycastHitIsHit() { RaycastHit = hit, IsHit = isHit };
        }

        #endregion
    }
}
