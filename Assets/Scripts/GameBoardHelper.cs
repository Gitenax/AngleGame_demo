public partial class GameBoard
{
    /// <summary>
    /// Получить фигуру в указанной точке
    /// </summary>
    /// <param name="position">Положение фигуры на доске</param>
    public Figure GetFigureAtPoint(Point position)
    {
        if(CheckingPointWithin(position))
            return _figureCollection[position];

        return default;
    }
    
    /// <summary>
    /// Проверка, что данная точка вписывается в размеры "игровой доски"
    /// </summary>
    private bool CheckingPointWithin(Point point)
    {
        return ((point.X >= 0 && point.X < Width) 
                && (point.Y >= 0 && point.Y < Height));
    }
}