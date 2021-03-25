using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "GameOptions", menuName = "Game Options", order = 0)]
    public class GameOptions : ScriptableObject
    {
        public int BoardWidth;
        public int BoardHeight;
        public GameFormat Format;
    }
}