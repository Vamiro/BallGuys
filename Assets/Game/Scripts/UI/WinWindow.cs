using TMPro;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class WinWindow : BaseWindow
    {
        [SerializeField] private TMP_Text winText;
        
        protected override void OnShow(params object[] args)
        {
            winText.text = "Winner is " + (string) args[0];
        }

        protected override void OnHide()
        {
        }
    }
}