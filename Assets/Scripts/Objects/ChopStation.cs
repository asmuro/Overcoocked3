using Assets.Scripts.Objects.Dummy;
using Assets.Scripts.Objects.Interfaces;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Objects
{
    internal class ChopStation : MonoBehaviour, IActionable
    {
        #region Fields        

        private bool startAction = false;
        private bool stopAction = false;
        private bool actionInProgress = false;
        private Coroutine actionCoroutine;
        private Canvas healthBar;
        private Image bar;        
        private float activeTime = 0f;        
        private GameObject objectOverMe;

        #endregion

        #region Properties

        IChoppable Choppable
        {
            get
            {
                if (IsChoppableObject())
                {
                    return this.objectOverMe.GetComponent<IChoppable>();                    
                }
                return new ChoppableDummy();
            }
        }

        private bool IsChoppableObject()
        {
            if (this.objectOverMe == null || this.objectOverMe.GetComponent<IChoppable>() == null)
            {
                return false;
            }
            return true;        
        }

        #endregion

        #region Monobehaviour

        private void Awake()
        {
            this.healthBar = this.transform.Find("Canvas").GetComponent<Canvas>();
            this.bar = this.healthBar.transform.Find("Healthbar").transform.Find("Bar").GetComponent<Image>();
            this.transform.GetComponent<AutoPositioner>().ObjectOnPosition += ObjectOnPosition;
            this.transform.GetComponent<AutoPositioner>().ObjectMovedAway += ObjectMovedAway;
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
            if (this.Choppable.ChopActionTimeConsumed > 0f)
            {
                this.activeTime = this.Choppable.ChopActionTimeConsumed;
            }            
            this.actionCoroutine = StartCoroutine(ActionTimeCounter(this.Choppable.ChopTotalActionTime - this.Choppable.ChopActionTimeConsumed));
        }

        private void StopCurrentAction()
        {
            this.stopAction = false;
            this.actionInProgress = false;
                        
            this.activeTime = 0f;
            if (this.actionCoroutine != null)
            {
                StopCoroutine(this.actionCoroutine);
            }
        }

        private void UpdateCurrentAction()
        {
            this.activeTime += Time.deltaTime;
            this.Choppable.SetChopActionTimeConsumed(this.activeTime);
            var percentage = (this.activeTime / this.Choppable.ChopTotalActionTime);
            this.bar.fillAmount = Mathf.Lerp(0, 1, percentage);
        }

        IEnumerator ActionTimeCounter(float maxTime)
        {
            this.actionInProgress = true;
            yield return new WaitForSeconds(maxTime);
            this.AfterActionFinish();        
        }

        private void AfterActionFinish()
        {
            this.actionInProgress = false;
            this.healthBar.enabled = false;
            this.activeTime = 0f;
            this.Choppable.SetChopActionTimeConsumed(this.Choppable.ChopTotalActionTime);
            this.OnActionFinished?.Invoke(this.transform.gameObject, EventArgs.Empty);
        }

        #endregion

        #region IActionable        

        public event EventHandler OnActionFinished;

        public void ExecuteAction()
        {
            if (this.CanExecuteAction())
            {
                this.startAction = true;
            }
        }

        private bool CanExecuteAction()
        {
            return this.IsChoppableObject()
                && this.Choppable.IsChoppable;
        }

        public void StopAction()
        {
            this.stopAction = true;
        }

        #endregion

        #region Auto Positioner

        private void ObjectMovedAway(object sender, EventArgs e)
        {
            if(this.objectOverMe != sender)
            {
                return;
            }
            
            this.stopAction = true;
            this.objectOverMe = null;
        }

        private void ObjectOnPosition(object sender, EventArgs e)
        {
            this.objectOverMe = (GameObject)sender;
        }

        #endregion
    }
}
