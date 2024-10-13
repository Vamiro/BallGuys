using UnityEngine;

public abstract class BaseWindow : MonoBehaviour
{
    protected bool IsActive { get; private set; }

    public void Show(params object[] args)
    {
        gameObject.SetActive(true);
        IsActive = true;
        OnShow(args);
    }

    public void Hide()
    {
        OnHide();
        IsActive = false;
        gameObject.SetActive(false);
    }
    
    protected abstract void OnShow(params object[] args);
    protected abstract void OnHide();
}
