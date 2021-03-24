using System;
using UnityEngine;

[Serializable]
public class PlayerBot : Player
{
    public PlayerBot(string playerName, Color playerColor) : base(playerName, playerColor)
    {
        _name  = playerName;
        _color = playerColor;
        _isMakeMove = false;
    }
}
