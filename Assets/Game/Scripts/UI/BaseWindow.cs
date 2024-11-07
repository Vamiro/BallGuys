using UnityEngine;

public abstract class WindowCore : MonoBehaviour
{
    protected bool IsActive => gameObject.activeSelf;
    
    protected void ShowInternal()
    {
        gameObject.SetActive(true);
        OnShow();
    }

    protected abstract void OnShow();

    public void Hide()
    {
        OnHide();
        gameObject.SetActive(false);
    }

    protected abstract void OnHide();
}

public abstract class BaseWindow : WindowCore
{
    public void Show()
    {
        ShowInternal();
    }
}

public abstract class BaseWindow<Targ> : WindowCore
{
    protected Targ _argument;
    public void Show(Targ argument)
    {
        _argument = argument;
        SetUp(_argument);
        ShowInternal();
    }
    protected virtual void SetUp(Targ argument)
    {

    }
}
