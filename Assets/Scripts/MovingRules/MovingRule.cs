using PlayArea;

namespace MovingRules
{
    public abstract class MovingRule
    {
        protected GameBoard _gameBoard;
        
        
        public abstract Point[] GetAllAvailablePositions(Point position);
        
        public abstract Point[] GetJumpPoints(Point position);

        
        protected bool VerifyPointForEmpty(Point tilePosition)
        {
            if (CheckingPointWithin(tilePosition))
                return _gameBoard.Figures[tilePosition] == null;
            
            return false;
        }
        
        
        private bool CheckingPointWithin(Point point)
        {
            return ((point.X >= 0 && point.X < _gameBoard.Width) 
                    && (point.Y >= 0 && point.Y < _gameBoard.Height));
        }
    }
}