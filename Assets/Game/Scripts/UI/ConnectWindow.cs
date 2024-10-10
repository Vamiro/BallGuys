using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConnectWindow : BaseWindow
{
    [SerializeField] private TMP_Text connectingText;

    private void Update()
    {
        if (IsActive)
        {
            connectingText.text = "Connecting" + new string('.', (int)Time.time % 4);
        }
    }

    protected override void OnShow()
    {
        
    }
    
    protected override void OnHide()
    {
        
    }
}