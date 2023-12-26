using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Dummy;
using Assets.Scripts.Objects.Interfaces;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CookStation : MonoBehaviour, IAutoActionable
{
    #region Fields        
    
    [SerializeField]
    private float blinkingTimeBeforBurn = 0.2f;
    private bool startAction = false;
    private bool stopAction = false;
    private bool actionInProgress = false;
    private Coroutine actionCoroutine;
    private Coroutine blinkCoroutine;
    private Canvas healthBar;
    private Image bar;
    private float activeTime = 0f;
    private GameObject objectOverMe;

    #endregion

    #region Properties

    ICookable Cookable
    {
        get
        {
            if (IsCookableObject())
            {
                return this.objectOverMe.GetComponent<ICookable>();
            }
            return new CookableDummy();
        }
    }

    private bool IsCookableObject()
    {
        if (this.objectOverMe == null || this.objectOverMe.GetComponent<ICookable>() == null)
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

        if (this.actionInProgress)
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
        if (this.Cookable.CookActionTimeConsumed > 0f)
        {
            this.activeTime = this.Cookable.CookActionTimeConsumed;
        }
        this.actionCoroutine = StartCoroutine(ActionTimeCounterCoroutine(this.Cookable.CookTime 
            + this.Cookable.FromCookTimeToBurn 
            - this.Cookable.CookActionTimeConsumed));
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
        if (this.blinkCoroutine != null)
        {
            StopCoroutine(this.blinkCoroutine);
        }        
    }

    private void UpdateCurrentAction()
    {
        this.activeTime += Time.deltaTime;
        this.Cookable.SetCookActionTimeConsumed(this.activeTime);
        
        if (this.activeTime >= (this.Cookable.CookTime + this.Cookable.StartAlertTimeBeforeBurn))
        {
            this.stopAction = true;
        }        

        var percentage = (this.activeTime / this.Cookable.CookTime);
        this.bar.fillAmount = Mathf.Lerp(0, 1, percentage);
        if (this.activeTime > (this.Cookable.CookTime + this.Cookable.FromCookTimeToBurn - this.Cookable.StartAlertTimeBeforeBurn)
            && this.bar.color != Color.red)
        {
            this.bar.color = Color.red;
            this.blinkCoroutine = StartCoroutine(this.BlinkCoroutine());
        }        
    }

    IEnumerator ActionTimeCounterCoroutine(float maxTime)
    {
        this.actionInProgress = true;
        yield return new WaitForSeconds(maxTime);        
        this.AfterActionFinish();
    }

    IEnumerator BlinkCoroutine()
    {
        while (true)
        {
            this.healthBar.enabled = false;
            yield return new WaitForSeconds(blinkingTimeBeforBurn);
            this.healthBar.enabled = true;
            yield return new WaitForSeconds(blinkingTimeBeforBurn);
        }
    }

    private void AfterActionFinish()
    {
        this.OnCookableObjectBurned?.Invoke(this.Cookable, EventArgs.Empty);
        this.actionInProgress = false;
        this.healthBar.enabled = false;
        this.activeTime = 0f;
        this.Cookable.SetCookActionTimeConsumed(this.Cookable.CookTime + this.Cookable.FromCookTimeToBurn);        
    }

    #endregion

    #region IAutoActionable        

    public event EventHandler OnCookableObjectBurned;

    #endregion

    #region Auto Positioner

    private void ObjectMovedAway(object sender, EventArgs e)
    {
        if (this.objectOverMe != sender)
        {
            return;
        }

        this.stopAction = true;
        this.objectOverMe = null;
    }

    private void ObjectOnPosition(object sender, EventArgs e)
    {
        this.objectOverMe = (GameObject)sender;
        if (this.Cookable.IsCookable)
        {
            this.startAction = true;
        }
    }

    #endregion
}
