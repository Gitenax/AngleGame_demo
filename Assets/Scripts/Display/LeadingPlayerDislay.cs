using System;
using UnityEngine;
using UnityEngine.UI;

public class LeadingPlayerDislay : MonoBehaviour
{
    #pragma warning disable CS0649
    [SerializeField] private AngleGame _game;
    [SerializeField] private Image     _image;
    [SerializeField] private int       _playerIndex;
                     private Player    _currentPlayer;
                     private bool      _playerSelected;
                     private bool      _playerDeselected;
                     private float     _lerpTime = 2f;
    #pragma warning restore CS0649       
                     
    private void Start()
    {
        _game = FindObjectOfType<AngleGame>();
        _image = GetComponent<Image>();
        
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
            _playerSelected = true;
            _playerDeselected = false;
        }
        else
        {
            if (_playerSelected)
            {
                _playerDeselected = true;
                _playerSelected = false;
            }
        }
    }
    
    private void Update()
    {
        if(_playerSelected)
            LerpColor(_currentPlayer.PlayerColor);
        
        if(_playerDeselected)
            LerpColor(Color.gray);
    }
    
    private void LerpColor(Color to)
    {
        _image.color = Color.Lerp(_image.color, to, _lerpTime * Time.deltaTime);
    }
}
