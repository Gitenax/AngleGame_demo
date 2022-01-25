using Gitenax.AngleCheckers.PlayArea;

namespace Gitenax.AngleCheckers.MovingRules
{
    public abstract class MovingRule
    {
        protected GameBoard GameBoard;
        
        public abstract Point[] GetAllAvailablePositions(Point position);
        
        public abstract Point[] GetJumpPoints(Point position);

        protected bool VerifyPointForEmpty(Point tilePosition)
        {
            return CheckingPointWithin(tilePosition) && GameBoard.Figures[tilePosition] == null;
        }
        
        private bool CheckingPointWithin(Point point)
        {
            return (point.X >= 0 && point.X < GameBoard.Width) 
                   && (point.Y >= 0 && point.Y < GameBoard.Height);
        }
    }
}