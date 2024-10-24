using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class FinishWindow : BaseWindow
    {
        [SerializeField] private TMP_Text finishText;
        [SerializeField] private Button closeButton;

        private void Start()
        {
            closeButton.onClick.AddListener(Hide);
        }

        protected override void OnShow(params object[] args)
        {
            finishText.text = (string) args[0];
        }

        protected override void OnHide()
        {
        }
        
        private void OnDestroy()
        {
            closeButton.onClick.RemoveListener(Hide);
        }
    }
}