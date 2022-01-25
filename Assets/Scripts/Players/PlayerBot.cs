using System;
using UnityEngine;

namespace Gitenax.AngleCheckers.Players
{
    [Serializable]
    public sealed class PlayerBot : Player
    {
        public PlayerBot(string playerName, Color playerColor) : base(playerName, playerColor)
        {
            _name  = playerName;
            PlayerColor = playerColor;
            IsMakeMove = false;
        }
    }
}
