using System.Collections.Generic;

namespace MovingRules
{
    /// <summary>
    /// <para>Перемещение фишек по диагонали</para>
    /// <para>Входит возможность перепрыгивать рядом стояшую фишку</para>
    /// </summary>
    public class DiagonalMovingRule : MovingRule
    {
        public DiagonalMovingRule(GameBoard targetBoard)
        {
            _gameBoard = targetBoard;
        }
        
        
        public override Point[] GetAllAvailablePositions(Point position)
        {
            var availablePositions = new List<Point>();
            foreach (var diagonal in Point.Diagonals)
            {
                var nextToPosition = position + diagonal;
                if(VerifyPointForEmpty(nextToPosition))
                {
                    availablePositions.Add(nextToPosition);
                }
                else
                {
                    nextToPosition += diagonal;
                    if(VerifyPointForEmpty(nextToPosition))
                    {
                        nextToPosition.Node = true;
                        availablePositions.Add(nextToPosition);
                    }
                }
            }
            
            return availablePositions.ToArray();
        }

        public override Point[] GetJumpPoints(Point position)
        {
            var availablePositions = new List<Point>();
            foreach (var diagonal in Point.Diagonals)
            {
                var nextToPosition = position + diagonal;

                if (VerifyPointForEmpty(nextToPosition) == false)
                {
                    nextToPosition += diagonal;
                    if(VerifyPointForEmpty(nextToPosition))
                    {
                        availablePositions.Add(nextToPosition);
                    }
                }
            }
            
            return availablePositions.ToArray();
        }
    }
}