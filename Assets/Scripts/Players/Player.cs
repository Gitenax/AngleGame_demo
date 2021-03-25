using System;
using UnityEngine;

namespace Players
{
    [Serializable]
    public class Player
    {
        #pragma warning disable CS0649
        [SerializeField] protected string _name;
                         protected Color  _color;
                         protected bool   _isMakeMove;
        #pragma warning restore CS0649


        public event Action<Player> GotOpportunityToMove;
        public event Action<Player> LostOpportunityToMove;
    
    
        public Player(string playerName, Color playerColor)
        {
            _name  = playerName;
            _color = playerColor;
            _isMakeMove = false;
        }

        
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
            if (obj is Player player)
                return _name == player.Name;

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}