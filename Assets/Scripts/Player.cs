using System;
using UnityEngine;

[Serializable]
public class Player
{
    [SerializeField] protected string _name;
                     protected Color  _color;
                     protected bool   _isMakeMove;


    public event Action<Player> GotOpportunityToMove;
                     
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
        }
    }


    public override bool Equals(object obj)
    {
        if (obj is Player player)
            return _name == player.Name;

        return false;
    }
}