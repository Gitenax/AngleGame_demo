using UnityEngine;
using UnityEngine.UI;

namespace Display
{
    public class PlayerNameDisplay : MonoBehaviour
    {
        #pragma warning disable CS0649
        [SerializeField] private AngleGame _game;
        [SerializeField] private Text      _text;
        [SerializeField] private int       _playerIndex;
        #pragma warning restore CS0649

        
        private void Awake()
        {
            _game = FindObjectOfType<AngleGame>();
            _text = GetComponent<Text>();
        }

        private void Start()
        {
            _text.text = _game!= null 
                ? _game.Players[_playerIndex].Name 
                : $"Игрок {_playerIndex}";
        }
    }
}
