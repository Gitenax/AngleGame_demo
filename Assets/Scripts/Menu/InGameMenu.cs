using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    public class InGameMenu : MonoBehaviour
    {
        #pragma warning disable CS0649
        [SerializeField] private AngleGame _game;
        [SerializeField] private Button    _restartButton;
        [SerializeField] private Button    _menuButton;
        #pragma warning restore CS0649

        
        private void Awake()
        {
            _restartButton.onClick.AddListener(_game.Restart);
            _menuButton.onClick.AddListener(LoadMainMenu);
        }

        private void LoadMainMenu()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
}