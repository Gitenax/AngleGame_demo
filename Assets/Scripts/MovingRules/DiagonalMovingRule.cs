using System.Collections.Generic;
using PlayArea;

namespace MovingRules
{
    /// <summary>
    /// <para>Перемещение фишек по диагонали</para>
    /// <para>Входит возможность перепрыгивать рядом стояшую фишку</para>
    /// </summary>
    public sealed class DiagonalMovingRule : MovingRule
    {
        public DiagonalMovingRule(GameBoard targetBoard)
        {
            GameBoard = targetBoard;
        }
        
        public override Point[] GetAllAvailablePositions(Point position)
        {
            var availablePositions = new List<Point>();
            
            foreach (Point diagonal in Point.Diagonals)
            {
                var nextToPosition = position + diagonal;
                if(VerifyPointForEmpty(nextToPosition))
                {
                    availablePositions.Add(nextToPosition);
                    continue;
                }

                nextToPosition += diagonal;
                
                if (!VerifyPointForEmpty(nextToPosition))
                    continue;
                
                nextToPosition.Node = true;
                availablePositions.Add(nextToPosition);
            }
            
            return availablePositions.ToArray();
        }

        public override Point[] GetJumpPoints(Point position)
        {
            var availablePositions = new List<Point>();
            
            foreach (Point diagonal in Point.Diagonals)
            {
                Point nextToPosition = position + diagonal;

                if (VerifyPointForEmpty(nextToPosition) != false)
                    continue;
                
                nextToPosition += diagonal;
                if(VerifyPointForEmpty(nextToPosition))
                    availablePositions.Add(nextToPosition);
            }
            
            return availablePositions.ToArray();
        }
    }
}