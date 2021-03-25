using Players;
using UnityEngine;
using UnityEngine.UI;

namespace Display
{
    public class LeadingPlayerColorDislay : MonoBehaviour
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

        
        private void Awake()
        {
            _image = GetComponent<Image>();
            _playerDeselected = true;
        }

        private void Start()
        {
            _currentPlayer = _game.Players[_playerIndex];
            _game.LeadingPlayerSelected += OnLeadingPlayerSelected;
            _currentPlayer.GotOpportunityToMove += plyr => SetDisplayStateOn();
            _currentPlayer.LostOpportunityToMove += plyr => SetDisplayStateOff();
        }
    
        private void Update()
        {
            if(_playerSelected)
                LerpColorTo(_currentPlayer.PlayerColor);
        
            if(_playerDeselected)
                LerpColorTo(Color.gray);
        }
    
        private void LerpColorTo(Color to)
        {
            _image.color = Color.Lerp(_image.color, to, _lerpTime * Time.deltaTime);
        }

        private void SetDisplayStateOn()
        {
            _playerSelected = true;
            _playerDeselected = false;
        }

        private void SetDisplayStateOff()
        {
            _playerDeselected = true;
            _playerSelected = false;
        }
    
        private void OnLeadingPlayerSelected(Player obj)
        {
            /*
         * По большей мере как костыль, т.к. из-за разницы в инициализации
         * этот метод отработает в самом начале чтобы задать цвет ведущему игроку
         */
            if (obj.Equals(_currentPlayer))
                SetDisplayStateOn();

            _game.LeadingPlayerSelected -= OnLeadingPlayerSelected;
        }
    }
}
