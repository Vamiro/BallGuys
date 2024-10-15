using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Scripts.UI
{
    public class WinWindow : BaseWindow
    {
        [SerializeField] private TMP_Text winText;
        [SerializeField] private Button exitButton;

        public UnityAction OnExit;

        public WinWindow(UnityAction onExit)
        {
            OnExit = onExit;
        }

        private void Start()
        {
            exitButton.onClick.AddListener(OnExitHandler);
        }

        protected override void OnShow(params object[] args)
        {
            winText.text = "Winner is " + (string) args[0];
        }

        protected override void OnHide()
        {
        }
        
        private void OnExitHandler()
        {
            try
            {
                OnExit?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogError("Error during OnExit invocation: " + ex.Message);
            }
        }

        private void OnDestroy()
        {
            exitButton.onClick.RemoveListener(OnExitHandler);
        }
    }
}