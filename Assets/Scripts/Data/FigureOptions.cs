using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "FigureOptions", menuName = "Figure Options", order = 0)]
    public class FigureOptions : ScriptableObject
    {
        public int FigureWidth;
        public int FigureHeight;
        public Color FigureColor;
    }
}