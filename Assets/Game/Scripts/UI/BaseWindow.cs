using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWindow : MonoBehaviour
{
    public bool IsActive { get; private set; }

    public void Show()
    {
        gameObject.SetActive(true);
        IsActive = true;
        OnShow();
    }

    public void Hide()
    {
        OnHide();
        IsActive = false;
        gameObject.SetActive(false);
    }
    
    protected abstract void OnShow();
    protected abstract void OnHide();
}
