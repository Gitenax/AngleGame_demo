using System;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
    public class FirstScreenMenu : MonoBehaviour
    {
        [SerializeField] private SetGameTypeMenu _nextMenu;
        [SerializeField] private Button          _playButton;


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