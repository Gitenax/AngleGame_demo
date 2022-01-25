using System.Collections.Generic;
using Gitenax.AngleCheckers.PlayArea;

namespace Gitenax.AngleCheckers.MovingRules
{
    /// <summary>
    /// <para>Перемещение фишек по 4-м направлениям сторон света (или просто по горизонтали и вертикали)</para>
    /// <para>Входит возможность перепрыгивать рядом стояшую фишку</para>
    /// </summary>
    public sealed class MovingOnCardinalPointsRule : MovingRule
    {
        public MovingOnCardinalPointsRule(GameBoard targetBoard)
        {
            GameBoard = targetBoard;
        }
        
        public override Point[] GetAllAvailablePositions(Point position)
        {
            var availablePositions = new List<Point>();
            foreach (Point direction in Point.Directions)
            {
                Point nextToPosition = position + direction;
                
                if(VerifyPointForEmpty(nextToPosition))
                {
                    availablePositions.Add(nextToPosition);
                    continue;
                }

                nextToPosition += direction;
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
            foreach (Point direction in Point.Directions)
            {
                Point nextToPosition = position + direction;

                if (VerifyPointForEmpty(nextToPosition))
                    continue;
                
                nextToPosition += direction;
                if (VerifyPointForEmpty(nextToPosition))
                    availablePositions.Add(nextToPosition);
            }
            
            return availablePositions.ToArray();
        }
    }
}