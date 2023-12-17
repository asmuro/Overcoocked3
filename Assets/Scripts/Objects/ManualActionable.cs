using Assets.Scripts.Objects.Interfaces;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Objects
{
    internal class ManualActionable : MonoBehaviour, IActionable
    {
        #region Fields

        [SerializeField] 
        private float actionTime;

        private bool startAction = false;
        private bool stopAction = false;
        private bool actionInProgress = false;
        private Coroutine actionCoroutine;
        private Canvas healthBar;
        private Image bar;        
        private float activeTime = 0f;
        private float timeConsumed = 0f;

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            this.healthBar = this.transform.Find("Canvas").GetComponent<Canvas>();
            this.bar = this.healthBar.transform.Find("Healthbar").transform.Find("Bar").GetComponent<Image>();
        }


        private void FixedUpdate()
        {
            if (this.startAction)
            {
                this.StartNewAction();
            }

            if (this.stopAction)
            {
                this.StopCurrentAction();
            }

            if(this.actionInProgress)
            {
                this.UpdateCurrentAction();
            }
        }      

        #endregion

        #region In Action

        private void StartNewAction()
        {
            this.startAction = false;

            this.healthBar.enabled = true;
            if (this.timeConsumed > 0f)
            {
                this.activeTime = this.timeConsumed;
            }
            this.actionCoroutine = StartCoroutine(ActionTimeCounter(this.actionTime - this.timeConsumed));
        }

        private void StopCurrentAction()
        {
            this.stopAction = false;
            this.actionInProgress = false;
            this.timeConsumed = this.activeTime;
            this.activeTime = 0f;
            if (this.actionCoroutine != null)
            {
                StopCoroutine(this.actionCoroutine);
            }
        }

        private void UpdateCurrentAction()
        {
            this.activeTime += Time.deltaTime;
            var percent = this.activeTime / this.actionTime;
            this.bar.fillAmount = Mathf.Lerp(0, 1, percent);
        }

        IEnumerator ActionTimeCounter(float maxTime)
        {
            this.actionInProgress = true;
            yield return new WaitForSeconds(maxTime);
            this.actionInProgress = false;
            this.healthBar.enabled = false;
            this.activeTime = 0f;
            this.timeConsumed = 0f;
            this.ActionFinished();
        }

        private void ActionFinished()
        {
            this.OnActionFinished?.Invoke(this.transform.gameObject, EventArgs.Empty);
        }

        #endregion

        #region IActionable        

        public event EventHandler OnActionFinished;

        public void ExecuteAction()
        {
            this.startAction = true;
        }

        public void StopAction()
        {
            this.stopAction = true;
        }

        #endregion
    }
}
