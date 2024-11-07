using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class FinishWindow : BaseWindow<string>
    {
        [SerializeField] private TMP_Text finishText;
        [SerializeField] private Button closeButton;

        private void Start()
        {
            closeButton.onClick.AddListener(CloseButtonClicked);
        }
        
        private void CloseButtonClicked()
        {
            SoundManager.Instance.PlaySound("ButtonClick");
            Hide();
        }

        protected override void SetUp(string argument)
        {
            finishText.text = _argument;
        }

        protected override void OnShow()
        {
            finishText.text = _argument;
        }

        protected override void OnHide()
        {
        }
        
        private void OnDestroy()
        {
            closeButton.onClick.RemoveListener(CloseButtonClicked);
        }
    }
}