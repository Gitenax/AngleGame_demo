using System.Collections.Generic;
using PlayArea;

namespace MovingRules
{
    /// <summary>
    /// <para>Перемещение фишек по 4-м направлениям сторон света (или просто по горизонтали и вертикали)</para>
    /// <para>Входит возможность перепрыгивать рядом стояшую фишку</para>
    /// </summary>
    public class MovingOnCardinalPointsRule : MovingRule
    {
        public MovingOnCardinalPointsRule(GameBoard targetBoard)
        {
            _gameBoard = targetBoard;
        }
        
        public override Point[] GetAllAvailablePositions(Point position)
        {
            var availablePositions = new List<Point>();
            foreach (var direction in Point.Directions)
            {
                var nextToPosition = position + direction;
                if(VerifyPointForEmpty(nextToPosition))
                {
                    availablePositions.Add(nextToPosition);
                }
                else
                {
                    nextToPosition += direction;
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
            foreach (var direction in Point.Directions)
            {
                var nextToPosition = position + direction;

                if (VerifyPointForEmpty(nextToPosition) == false)
                {
                    nextToPosition += direction;
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