using UnityEngine;
using UnityEngine.UI;

public class LeadingPlayerDislay : MonoBehaviour
{
    [SerializeField] private AngleGame _game;
    [SerializeField] private Image     _image;
    [SerializeField] private int       _playerIndex;
                     private Player    _currentPlayer;
                     private Color     _disabledColor;
                     private float     _fadeTimer = 1f;
    private void Start()
    {
        _game = FindObjectOfType<AngleGame>();
        _image = GetComponent<Image>();
        _disabledColor = _image.color;
        
        if (_game != null)
        {
            _game.LeadingPlayerSelected += OnLeadingPlayerSelected;
            _currentPlayer = _game.Players[_playerIndex];
        }
    }

    private void OnLeadingPlayerSelected(Player obj)
    {
        if (obj.Equals(_currentPlayer))
        {
            SetBrightColor();
        }
        else
        {
            if(_image.color == _disabledColor)
                return;
            
            SetDullColor();
        }
    }

    private void SetBrightColor()
    {
        float elapsed = 0f;
        float total = _fadeTimer;
        
        while (elapsed < total)
        {
            elapsed += Time.deltaTime;
            _image.color = Color.Lerp(Color.gray, _currentPlayer.PlayerColor, elapsed);
        }
    }

    private void SetDullColor()
    {
        float elapsed = 0f;
        float total = _fadeTimer;
        
        while (elapsed < total)
        {
            elapsed += Time.deltaTime;
            _image.color = Color.Lerp(_currentPlayer.PlayerColor, Color.gray, elapsed);
            _disabledColor = _image.color;
        }
    }
}
