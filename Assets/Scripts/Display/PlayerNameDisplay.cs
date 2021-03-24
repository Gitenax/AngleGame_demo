using UnityEngine;
using UnityEngine.UI;

public class PlayerNameDisplay : MonoBehaviour
{
    [SerializeField] private AngleGame _game;
    [SerializeField] private Text      _text;
    [SerializeField] private int       _playerIndex;

    private void Awake()
    {
        _game = FindObjectOfType<AngleGame>();
        _text = GetComponent<Text>();
    }

    private void Start()
    {
        if (_game != null)
            _text.text = _game.Players[_playerIndex].Name;
        else
            _text.text = $"Игрок {_playerIndex}";
    }
}
