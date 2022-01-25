using System;
using System.Collections.Generic;
using Gitenax.AngleCheckers.PlayArea;

namespace Gitenax.AngleCheckers.MovingRules
{
    /// <summary>
    /// <para>Перемещение фишек в любом направлении</para>
    /// <para>Отсутствует возможность перепрыгивать рядом стояшую фишку</para>
    /// </summary>
    public sealed class FreeMoveRule : MovingRule
    {
        public FreeMoveRule(GameBoard targetBoard)
        {
            GameBoard = targetBoard;
        }
        
        public override Point[] GetAllAvailablePositions(Point position)
        {
            var availablePositions = new List<Point>();
            var allDirections = new List<Point>();
            allDirections.AddRange(Point.Diagonals);
            allDirections.AddRange(Point.Directions);
            
            foreach (Point direction in allDirections)
            {
                Point nextToPosition = position + direction;
                if (VerifyPointForEmpty(nextToPosition))
                    availablePositions.Add(nextToPosition);
            }
            
            return availablePositions.ToArray();
        }

        public override Point[] GetJumpPoints(Point position)
        {
            return Array.Empty<Point>();
        }
    }
}