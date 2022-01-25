using UnityEngine;

namespace Gitenax.AngleCheckers.Data
{
    [CreateAssetMenu(fileName = "FigureOptions", menuName = "Figure Options", order = 0)]
    public sealed class FigureOptions : ScriptableObject
    {
        [SerializeField] private int _figureWidth = 64;
        [SerializeField] private int _figureHeight = 64;
        
        public int FigureWidth => _figureWidth;
        public int FigureHeight => _figureHeight;
    }
}