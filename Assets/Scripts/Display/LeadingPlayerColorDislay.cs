using Gitenax.AngleCheckers.Players;
using UnityEngine;
using UnityEngine.UI;

namespace Gitenax.AngleCheckers.Display
{
    [RequireComponent(typeof(Image))]
    public sealed class LeadingPlayerColorDislay : MonoBehaviour
    {
        private const float LerpTime = 2f;
        
#pragma warning disable CS0649
        [SerializeField] private AngleGame _game;
        [SerializeField] private Image _image;
        [SerializeField] private int _playerIndex;
        private Player _currentPlayer;
        private bool _playerSelected;
        private bool _playerDeselected;
#pragma warning restore CS0649
        
        private void Awake()
        {
            _image = GetComponent<Image>();
            _playerDeselected = true;
        }

        private void Start()
        {
            _currentPlayer = _game.Players[_playerIndex];
            _game.LeadingPlayerSelected += OnLeadingPlayerSelectedHandler;
            _currentPlayer.GotOpportunityToMove += SetDisplayStateOnHandler;
            _currentPlayer.LostOpportunityToMove += SetDisplayStateOffHandler;
        }

        private void OnDestroy()
        {
            _game.LeadingPlayerSelected -= OnLeadingPlayerSelectedHandler;
            _currentPlayer.GotOpportunityToMove -= SetDisplayStateOnHandler;
            _currentPlayer.LostOpportunityToMove -= SetDisplayStateOffHandler;
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
            _image.color = Color.Lerp(_image.color, to, LerpTime * Time.deltaTime);
        }

        private void OnLeadingPlayerSelectedHandler(Player player)
        { 
            /*
             * По большей мере как костыль, т.к. из-за разницы в инициализации
             * этот метод отработает в самом начале чтобы задать цвет ведущему игроку
             */
            if (player.Equals(_currentPlayer))
                SetDisplayStateOnHandler(player);

            _game.LeadingPlayerSelected -= OnLeadingPlayerSelectedHandler;
        }

        private void SetDisplayStateOnHandler(Player player)
        {
            _playerSelected = true;
            _playerDeselected = false;
        }

        private void SetDisplayStateOffHandler(Player player)
        {
            _playerDeselected = true;
            _playerSelected = false;
        }
    }
}