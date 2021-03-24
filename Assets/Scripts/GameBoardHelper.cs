using System.Collections.Generic;

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
    /// Проверка, на возможность фишки совершить ход
    /// </summary>
    /// <param name="position">Позиция фишки</param>
    public bool VerifyMovableFigure(Point position)
    {
        var figure = GetFigureAtPoint(position);
        if (figure == null) return false;

        var availablePoints = CheckVerticalAndHorizontalSpace(position);
        return availablePoints.Length > 0;
    }
    
    
    /// <summary>
    /// Проверяет пространство вокруг указанной точки на возможность хода как на следующуюю клетку,
    /// так и через фигуру, если она стоит на пути
    /// </summary>
    /// <param name="currentFigurePosition">Проверяемая точка</param>
    /// <returns>Возвращает массив точек куда возможны ходы с проверяемой точки</returns>
    public Point[] CheckVerticalAndHorizontalSpace(Point currentFigurePosition)
    {
        var availablePositions = new List<Point>();
        
        foreach (Point direction in Point.Directions)
        {
            var nextToPosition = currentFigurePosition + direction;
            
            if(CheckFreeSpace(nextToPosition))
            {
                availablePositions.Add(nextToPosition);
            }
            else
            {
                nextToPosition += direction;
                if(CheckFreeSpace(nextToPosition))
                    availablePositions.Add(nextToPosition);
            }
        }

        return availablePositions.ToArray();
    }
    

    private bool CheckFreeSpace(Point position)
    {
        return CheckingPointWithin(position) && VerifyNextTileForEmpty(position);
    }

    
    /// <summary>
    /// Проверка, что данная точка вписывается в размеры "игровой доски"
    /// </summary>
    private bool CheckingPointWithin(Point point)
    {
        return ((point.X >= 0 && point.X < Width) 
                && (point.Y >= 0 && point.Y < Height));
    }

    
    private bool VerifyNextTileForEmpty(Point tilePosition)
    {
        return GetFigureAtPoint(tilePosition) == null;
    }
}