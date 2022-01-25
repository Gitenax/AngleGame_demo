using UnityEngine;

namespace Gitenax.AngleCheckers.Data
{
    [CreateAssetMenu(fileName = "GameOptions", menuName = "Game Options", order = 0)]
    public class GameOptions : ScriptableObject
    {
        [SerializeField] private int _boardWidth = 8;
        [SerializeField] private int _boardHeight = 8;
        [SerializeField] private GameFormat _format = GameFormat.Format3X3;

        public int BoardWidth => _boardWidth;
        public int BoardHeight => _boardHeight;
        public GameFormat Format => _format;
    }
}