using System;
using UnityEngine;

namespace Players
{
    [Serializable]
    public class Player
    {
#pragma warning disable CS0649
        [SerializeField] protected string _name;
        private Color _color;
        private bool _isMakeMove;
#pragma warning restore CS0649

        public Player(string playerName, Color playerColor)
        {
            _name  = playerName;
            _color = playerColor;
            _isMakeMove = false;
        }

        public event Action<Player> GotOpportunityToMove;
        public event Action<Player> LostOpportunityToMove;

        public string Name => _name;

        public Color PlayerColor
        {
            get => _color;
            set => _color = value;
        }

        public bool IsMakeMove
        {
            get => _isMakeMove;
            set
            {
                _isMakeMove = value;
                if (value)
                    GotOpportunityToMove?.Invoke(this);
                else
                    LostOpportunityToMove?.Invoke(this);
            }
        }

        public override bool Equals(object obj)
        {
            return obj is Player player && _name == player.Name;
        }
    }
}