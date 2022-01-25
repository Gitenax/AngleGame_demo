namespace Gitenax.AngleCheckers.PlayArea
{
    public sealed partial class GameBoard
    {
        /// <summary>
        /// Получить фигуру в указанной точке
        /// </summary>
        /// <param name="position">Положение фигуры на доске</param>
        public Figure GetFigureAtPoint(Point position)
        {
            return CheckingPointWithin(position)
                ? _figureCollection[position]
                : default;
        }
    
        /// <summary>
        /// Проверка, что данная точка вписывается в размеры "игровой доски"
        /// </summary>
        private bool CheckingPointWithin(Point point)
        {
            return (point.X >= 0 && point.X < Width) && (point.Y >= 0 && point.Y < Height);
        }
    }
}