using System;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class FirstScreenMenu : MonoBehaviour
    {
        #pragma warning disable CS0649
        [SerializeField] private SetGameTypeMenu _nextMenu;
        [SerializeField] private Button          _playButton;
        #pragma warning restore CS0649

        
        private void Awake()
        {
            _playButton.onClick.AddListener(ShowNextMenu);
        }

        private void ShowNextMenu()
        {
            _nextMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}