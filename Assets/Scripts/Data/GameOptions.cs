using UnityEngine;

namespace Gitenax.AngleCheckers.Data
{
    [CreateAssetMenu(fileName = "GameOptions", menuName = "Game Options", order = 0)]
    public class GameOptions : ScriptableObject
    {
        [Header("Board")]
        [SerializeField] private int _boardWidth = 8;
        [SerializeField] private int _boardHeight = 8;
        
        [Header("Figure")]
        [SerializeField] private int _figureWidth = 64;
        [SerializeField] private int _figureHeight = 64;

        [Header("Other")]
        [SerializeField] private GameFormat _format;
        
        public int BoardWidth => _boardWidth;
        public int BoardHeight => _boardHeight;
        public int FigureWidth => _figureWidth;
        public int FigureHeight => _figureHeight;
        public GameFormat Format => _format;
    }
}