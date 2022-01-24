using Players;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    public sealed class EndGameMenu : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField] private Text _winnerText;
        [SerializeField] private AngleGame _game;

        [Header("Кнопки")] 
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _menuButton;
#pragma warning restore CS0649
        
        private void Awake()
        {
            _restartButton.onClick.AddListener(_game.Restart);
            _menuButton.onClick.AddListener(LoadMainMenu);
            _game.GameEnded += OnEndGame;
        }

        private void OnDestroy()
        {
            _game.GameEnded -= OnEndGame;
        }

        private void OnEndGame(Player winner, Player loser)
        {
            _winnerText.text = $"ИГРОК \"{winner.Name}\" ПОБЕДИЛ!";
        }
        
        private void LoadMainMenu()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }
}
