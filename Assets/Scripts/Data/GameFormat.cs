using System;
using UnityEngine;

namespace Gitenax.AngleCheckers.Data
{
    [Serializable]
    public sealed class GameFormat
    {
        [SerializeField] private int _width;
        [SerializeField] private int _height;

        public int Width => _width;
        public int Height => _height;
    }
}