using System.Collections.Generic;

namespace MovingRules
{
    /// <summary>
    /// <para>Перемещение фишек в любом направлении</para>
    /// <para>Отсутствует возможность перепрыгивать рядом стояшую фишку</para>
    /// </summary>
    public class FreeMoveRule : MovingRule
    {
        public FreeMoveRule(GameBoard targetBoard)
        {
            _gameBoard = targetBoard;
        }
        
        public override Point[] GetAllAvailablePositions(Point position)
        {
            var availablePositions = new List<Point>();
            var allDirections = new List<Point>();
            allDirections.AddRange(Point.Diagonals);
            allDirections.AddRange(Point.Directions);
            
            foreach (var direction in allDirections)
            {
                var nextToPosition = position + direction;
                if(VerifyPointForEmpty(nextToPosition))
                {
                    availablePositions.Add(nextToPosition);
                }
            }
            
            return availablePositions.ToArray();
        }

        public override Point[] GetJumpPoints(Point position)
        {
            return new Point[0];
        }
    }
}